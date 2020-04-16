using System;
using System.Collections.Generic;

namespace covid19phlib.DTO_Models
{
    public class DTO_Model_Countries
    {
        public DateTimeOffset updateDate { get; set; } = new DateTimeOffset();
        public List<DTO_Model_CountryData> data { get; set; } = new List<DTO_Model_CountryData>();
    }
}
