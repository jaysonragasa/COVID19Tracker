namespace COVID19Tracker.Library.DTO_Models
{
    public class DTO_Model_CaseInfo
    {
        /// <summary>
        /// Random code assigned for labelling cases; does not equate to the unique case number assigned by DOH
        /// </summary>
        public string CaseCode { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Five-year age group
        /// </summary>
        public string AgeGroup { get; set; } = string.Empty;


        public string Sex { get; set; } = string.Empty;

        /// <summary>
        /// Date publicly announced as confirmed case
        /// </summary>
        public string DateRepConf { get; set; } = string.Empty;

        /// <summary>
        /// Date recoevered
        /// </summary>
        public string DateRecover { get; set; } = string.Empty;

        /// <summary>
        /// Date died
        /// </summary>
        public string DateDied { get; set; } = string.Empty;

        /// <summary>
        /// Type of removal (recovery or death)
        /// </summary>
        public string RemovalType { get; set; } = string.Empty;

        /// <summary>
        /// Date publicly announced as removed
        /// </summary>
        public string DateRepRem { get; set; } = string.Empty;

        /// <summary>
        /// Binary variable indicating patient has been admitted to hospital
        /// </summary>
        public string Admitted { get; set; } = string.Empty;

        /// <summary>
        /// Region of residence
        /// </summary>
        public string RegionRes { get; set; } = string.Empty;

        /// <summary>
        /// Provincial
        /// </summary>
        public string ProvCityRes { get; set; } = string.Empty;

        /// <summary>
        /// Health status
        /// </summary>
        public string HealthStatus { get; set; } = string.Empty;

        /// <summary>
        /// City municipality
        /// </summary>
        public string CityMunRes { get; set; } = string.Empty;

        public string ProvRes { get; set; } = string.Empty;
    }
}
