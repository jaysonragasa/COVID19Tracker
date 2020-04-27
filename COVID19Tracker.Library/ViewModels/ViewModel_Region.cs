using covid19phlib.DTO_Models;
using covid19phlib.Interfaces;
using covid19phlib.ViewModels;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.BO_Models;
using COVID19Tracker.Library.DTO_Models;
using COVID19Tracker.Library.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COVID19Tracker.Library.ViewModels
{
    public class ViewModel_Region : VMBase
    {
        #region events
        public event EventHandler<Model_RegionData> OnRegionLookupFound;
        public event EventHandler<string> OnShowMessage;
        #endregion

        #region vars
        List<DTO_Model_Region> _localStore = new List<DTO_Model_Region>();
        Stopwatch stopwatch = new Stopwatch();
        string _countryCode = string.Empty;
        #endregion

        #region properties
        public ObservableCollection<Model_RegionData> Regions { get; set; } = new ObservableCollection<Model_RegionData>();

        private string _RegionLookup = string.Empty;
        public string RegionLookup
        {
            get { return _RegionLookup; }
            set
            {
                Set(nameof(RegionLookup), ref _RegionLookup, value);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    SearchRegion();
                }
            }
        }
        #endregion

        #region commands
        public ICommand Command_SelectedRegion { get; set; }
        #endregion

        #region ctors
        public ViewModel_Region(IIoC ioc, IAPILocator api)
        {
            this.API = api;

            this.IoC = ioc;

            this.Nav = this.IoC.GI<INavService>();

            this.Logger = this.IoC.GI<ILogger>();

            InitCommands();
            RuntimeData();
        }
        #endregion

        #region command methods
        void Command_SelectedRegion_Click(Model_RegionData countryData)
        {
            this.Logger.Log("Navigating to region: " + countryData.RegionName);

            this.Nav.GoToPage(COVID19Tracker.Library.Enums.Enum_NavService_Pages.CityPage, countryData.RegionName);
        }
        #endregion

        #region methods
        public override void InitCommands()
        {
            base.InitCommands();

            if (Command_SelectedRegion == null) Command_SelectedRegion = new RelayCommand<Model_RegionData>(Command_SelectedRegion_Click);
        }

        void DesignData()
        {

        }

        void RuntimeData()
        {

        }

        public async Task RefreshData(string countryCode)
        {
            if (this.IsLoading
                //|| this._currentFilter == listFilter
                )
            {
                return;
            };

            this.IsLoading = true;

            _countryCode = countryCode;

            //this.OnShowMessage?.Invoke(this, "This may take several seconds to load due to the volume of data.");

            ResponseData responseData = null;
            responseData = await this.API.CountryDetailedData.GetDataByCountryCode(countryCode);

            Reset();

            if (responseData.Status)
            {
                responseData = await this.API.CountryDetailedData.GetAllRegionsAsync();

                if (responseData.Status)
                {
                    if (responseData.Result != null)
                    {
                        List<DTO_Model_Region> dataList = (List<DTO_Model_Region>)responseData.Result;

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
            }
            else
            {
                this.OnShowMessage?.Invoke(this, "There are no data to show currently. Try to refresh the page by swiping down on the list.");
            }

            this.IsLoading = false;
            this.IsRefreshing = false;
        }

        void UpdateListFromSource(List<DTO_Model_Region> source)
        {
            bool isEmpty = false;

            if (this.Regions.Count == 0)
            {
                this.Regions.Clear();
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
                var countryw = this.Regions.Where(x => x.RegionName == source[i].RegionName).SingleOrDefault();

                if (countryw != null)
                {
                    int oldIndex = this.Regions.IndexOf(countryw);
                    this.Regions.Move(oldIndex, i);
                }
                else
                {
                    this.Regions.Insert(i, new Model_RegionData()
                    {
                        RegionCode = null,
                        RegionName = source[i].RegionName,
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
            if (source.Count < this.Regions.Count)
            {
                for (int i = this.Regions.Count - 1; i >= source.Count; i--)
                {
                    this.Regions.RemoveAt(i);
                }
            }

            cas = conf + rec + det;
            this.TotalCases = cas;
            this.TotalConfirmed = conf;
            this.TotalDeaths = det;
            this.TotalRecoveries = rec;
            this.LastUpdate = lastupdate;
        }

        void RefreshList(List<DTO_Model_Region> source)
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

            this._localStore = this._localStore.OrderBy(x => x.RegionName).ToList();

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
            await RefreshData(this._countryCode);
        }

        void SearchRegion()
        {
            var country = this.Regions.Where(x => x.RegionName.ToLowerInvariant().Contains(this.RegionLookup.ToLowerInvariant())).FirstOrDefault();

            if (country != null)
            {
                this.OnRegionLookupFound?.Invoke(this, country);
            }
        }

        void Reset()
        {
            this.Regions.Clear();
            this.TotalCases = 0;
            this.TotalConfirmed = 0;
            this.TotalDeaths = 0;
            this.TotalRecoveries = 0;
        }
        #endregion
    }
}
