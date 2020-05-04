using covid19phlib.DTO_Models;
using COVID19Tracker.Library.DTO_Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using c = System.Console;

namespace DOHDataImporter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            
            CSVTOJSON csv = new CSVTOJSON();

            c.WriteLine("Converting CSV to JSON");
            sw.Start();
            string json = csv.ConvertToJSON();
            sw.Stop();
            c.WriteLine("Total duration converting CSV to JSON: " + sw.Elapsed);

            c.WriteLine("Parsing region and city data");
            ProgramJSONParse tojson = new ProgramJSONParse();
            tojson.Start(json);
        }
    }

    class CSVTOJSON
    {
        public string ConvertToJSON()
        {
            string json = string.Empty;
            string csv = string.Empty;

            using (StreamReader reader = new StreamReader("data.csv"))
            {
                csv = reader.ReadToEnd();
            }

            string[] lines = csv.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            c.WriteLine("CSV has " + (lines.Length - 1) + " data");

            if (lines.Length > 1)
            {
                // parse headers
                string[] headers = lines[0].Split(',');

                StringBuilder sbjson = new StringBuilder();
                sbjson.Clear();
                sbjson.Append("[");

                // parse data
                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    if (string.IsNullOrEmpty(lines[i])) continue;

                    sbjson.Append("{");

                    string[] data = lines[i].Split(',');

                    for (int h = 0; h < headers.Length; h++)
                    {
                        sbjson.Append(
                            $"\"{headers[h]}\": \"{data[h]}\"" + (h < headers.Length - 1 ? "," : null)
                        );
                    }

                    sbjson.Append("}" + (i < lines.Length - 1 ? "," : null));
                }

                sbjson.Append("]");

                json = sbjson.ToString();
            }

            return json;
        }
    }

    class ProgramJSONParse
    {
        string json = string.Empty;
        List<DTO_Model_CaseInfo> JSONData = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_city = new List<DTO_Model_CaseInfo>();

        // for filtering purposes
        public List<string> Regions { get; set; } = new List<string>();
        public List<string> City { get; set; } = new List<string>();

        public void Start(string json = "")
        {
            c.WriteLine("parsing JSON");

            ProgramJSONParse p = new ProgramJSONParse();
            p.json = json;

            if (string.IsNullOrEmpty(json))
            {
                using (StreamReader reader = new StreamReader("regioncitydata.json"))
                {
                    string j = reader.ReadToEnd();
                    p.json = j;
                }
            }

            p.JSONData = JsonConvert.DeserializeObject<List<DTO_Model_CaseInfo>>(p.json);

            p.Regions = p.JSONData.Select(x => x.ProvRes).Distinct().ToList();
            p.City = p.JSONData.Select(x => x.ProvCityRes).Distinct().ToList();

            var allregions = p.GetAllRegionsAsync();
            string jsonregion = JsonConvert.SerializeObject(allregions.Result);

            using (StreamWriter writer = new StreamWriter("PH_regions.json", false) { AutoFlush = true })
            {
                writer.Write(jsonregion);
            }

            foreach (var reg in (List<DTO_Model_Region>)allregions.Result)
            {
                var respcity = p.GetCitiesByRegionNameAsync(reg.RegionName);
                if (respcity.Status)
                {
                    var cities = (List<DTO_Model_City>)respcity.Result;
                    jsonregion = JsonConvert.SerializeObject(cities);

                    string filename = reg.RegionName;
                    filename = filename.Replace(' ', '_');
                    filename = filename.Replace('/', '~');
                    filename = "PH_" + filename + ".json";

                    using (StreamWriter writer = new StreamWriter(filename, false) { AutoFlush = true })
                    {
                        writer.Write(jsonregion);
                    }
                }
            }

            //c.ReadLine();
        }

        public ResponseData GetAllRegionsAsync()
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_Region> caseinfolist = new List<DTO_Model_Region>();

            for (int i = 0; i < this.Regions.Count; i++)
            {
                var currentRegion = this.JSONData.Where(x => x.ProvRes == this.Regions[i]).ToList();

                // get confirmed case
                DTO_Model_Region caseInfo = new DTO_Model_Region()
                {
                    RegionName = this.Regions[i],
                    Confirmed = currentRegion.Count
                };

                var recoveredList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByRegionAsync";

            return responseData;
        }

        public ResponseData GetCitiesByRegionNameAsync(string regionName)
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_City> caseinfolist = new List<DTO_Model_City>();

            this._cache_city = this.JSONData.Where(x => x.ProvRes == regionName).ToList();
            var cityNames = this._cache_city.Select(x => x.CityMunRes).Distinct().ToList();

            for (int i = 0; i < cityNames.Count; i++)
            {
                var currentCity = this._cache_city.Where(x => x.CityMunRes == cityNames[i]).ToList();

                // get confirmed case
                DTO_Model_City caseInfo = new DTO_Model_City()
                {
                    CityName = cityNames[i],
                    Confirmed = currentCity.Count
                };

                var recoveredList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByCityAsync";

            return responseData;
        }
    }
}