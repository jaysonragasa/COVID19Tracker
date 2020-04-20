using covid19phlib.DTO_Models;
using covid19phlib.Interfaces;
using covid19phlib.ViewModels;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.BO_Models;
using COVID19Tracker.Library.DTO_Models;
using COVID19Tracker.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.ViewModels
{
    public class ViewModel_City : VMBase
    {
        #region events
        public event EventHandler<Model_CityData> OnCityLookupFound;
        public event EventHandler<string> OnShowMessage;
        #endregion

        #region vars
        List<DTO_Model_City> _localStore = new List<DTO_Model_City>();
        Stopwatch stopwatch = new Stopwatch();
        string _regionName = string.Empty;
        #endregion

        #region properties
        public ObservableCollection<Model_CityData> Cities { get; set; } = new ObservableCollection<Model_CityData>();

        private string _CityLookup = string.Empty;
        public string CityLookup
        {
            get { return _CityLookup; }
            set
            {
                Set(nameof(CityLookup), ref _CityLookup, value);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    SearchRegion();
                }
            }
        }
        #endregion

        #region commands

        #endregion

        #region ctors
        public ViewModel_City(IIoC ioc, IAPILocator api)
        {
            this.API = api;

            this.IoC = ioc;

            this.Nav = this.IoC.GI<INavService>();

            InitCommands();
            RuntimeData();
        }
        #endregion

        #region command methods

        #endregion

        #region methods
        public override void InitCommands()
        {
            base.InitCommands();
        }

        void DesignData()
        {

        }

        void RuntimeData()
        {

        }

        public async Task RefreshData(string regionName)
        {
            if (this.IsLoading
                //|| this._currentFilter == listFilter
                )
            {
                return;
            };

            this.IsLoading = true;

            _regionName = regionName;

            Reset();

            ResponseData responseData = null;
            responseData = await this.API.CountryDetailedData.GetCitiesByRegionNameAsync(regionName);

            if (responseData.Status)
            {
                if (responseData.Result != null)
                {
                    List<DTO_Model_City> dataList = (List<DTO_Model_City>)responseData.Result;

                    // update local store for sorting
                    {
                        this._localStore.Clear();
                        for (int i = 0; i < dataList.Count; i++)
                        {
                            this._localStore.Add(dataList[i]);
                        }
                    }

                    RefreshList(dataList.OrderByDescending(x => x.Confirmed).ToList());
                }
                else
                {
                    this.OnShowMessage?.Invoke(this, "There are no data to show currently. Try to refresh the page by swiping down on the list.");
                }
            }
            else
            {
                this.OnShowMessage?.Invoke(this, "There are no data to show currently. Try to refresh the page by swiping down on the list.");
            }

            this.IsLoading = false;
            this.IsRefreshing = false;
        }

        void UpdateListFromSource(List<DTO_Model_City> source)
        {
            bool isEmpty = false;

            if (this.Cities.Count == 0)
            {
                this.Cities.Clear();
                isEmpty = true;
            }

            int cas = 0;
            int conf = 0;
            int rec = 0;
            int det = 0;
            DateTime lastupdate = DateTime.Now.AddDays(-1); // new DateTime();

            // update our lists without clearing our collection
            for (int i = 0; i < source.Count; i++)
            {
                var countryw = this.Cities.Where(x => x.CityName == source[i].CityName).SingleOrDefault();

                if (countryw != null)
                {
                    int oldIndex = this.Cities.IndexOf(countryw);
                    this.Cities.Move(oldIndex, i);
                }
                else
                {
                    this.Cities.Insert(i, new Model_CityData()
                    {
                        CityCode = null,
                        CityName = source[i].CityName.Replace("City of ", null),
                        TotalConfirmed = source[i].Confirmed,
                        TotalRecovered = source[i].Recovered,
                        TotalDeaths = source[i].Deceased
                    });
                }

                conf += source[i].Confirmed;
                rec += source[i].Recovered;
                det += source[i].Deceased;

                //if (source[i].lastUpdated > this.LastUpdate)
                //{
                //    lastupdate = source[i].lastUpdated;
                //}
            }

            // remove old items
            if (source.Count < this.Cities.Count)
            {
                for (int i = this.Cities.Count - 1; i >= source.Count; i--)
                {
                    this.Cities.RemoveAt(i);
                }
            }

            cas = conf + rec + det;
            this.TotalCases = cas;
            this.TotalConfirmed = conf;
            this.TotalDeaths = det;
            this.TotalRecoveries = rec;
            this.LastUpdate = lastupdate;
        }

        void RefreshList(List<DTO_Model_City> source)
        {
            stopwatch.Restart();
            stopwatch.Start();

            UpdateListFromSource(source);

            stopwatch.Stop();

            Debug.WriteLine("DEBUG> refresh duration: " + stopwatch.ElapsedMilliseconds + "ms - total items: " + source.Count);
        }

        public override void SortByName()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderBy(x => x.CityName).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByConfirmedCase()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.Confirmed).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByRecovery()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.Recovered).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByDeaths()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.Deceased).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void ApplyFilter()
        {
            // do nothing yet.
        }

        public override async Task PullToRefresh()
        {
            await RefreshData(this._regionName);
        }

        void SearchRegion()
        {
            var city = this.Cities.Where(x => x.CityName.ToLowerInvariant().Contains(this.CityLookup.ToLowerInvariant())).FirstOrDefault();

            if (city != null)
            {
                this.OnCityLookupFound?.Invoke(this, city);
            }
        }

        void Reset()
        {
            this.Cities.Clear();
            this.TotalCases = 0;
            this.TotalConfirmed = 0;
            this.TotalDeaths = 0;
            this.TotalRecoveries = 0;
        }
        #endregion
    }
}
