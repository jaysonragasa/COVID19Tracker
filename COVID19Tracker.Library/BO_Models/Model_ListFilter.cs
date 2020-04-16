using covid19phlib.Enums;
using GalaSoft.MvvmLight;

namespace covid19phlib.BO_Models
{
    public class Model_ListFilter : ViewModelBase
    {
        public int Id { get; set; } = 0;
        public string ListTypeName { get; set; } = null;
        public Enums_ListFilter ListFilter { get; set; } = Enums_ListFilter.GLOBAL;
    }
}
