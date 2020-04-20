namespace COVID19Tracker.Library.DTO_Models
{
    public class DTO_Model_City
    {
        public string CityName { get; set; } = null;
        public int Confirmed { get; set; } = 0;
        public int Recovered { get; set; } = 0;
        public int Deceased { get; set; } = 0;
    }
}
