using covid19phlib.BO_Models;
using covid19phlib.DTO_Models;
using covid19phlib.Enums;
using covid19phlib.Interfaces;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace covid19phlib.ViewModels
{
    public class ViewModel_Dashboard : VMBase
    {
        #region events
        public event EventHandler<string> OnShowMessage;
        public event EventHandler<Model_CountryData> OnCountryLookupFound;
        #endregion

        #region vars
        
        int i = 0;
        Enums_ListFilter _currentFilter = Enums_ListFilter.NONE;
        
        List<DTO_Model_CountryData> _localStore = new List<DTO_Model_CountryData>();
        Stopwatch stopwatch = new Stopwatch();
        #endregion

        #region properties
        public ObservableCollection<Model_CountryData> Countries { get; set; } = new ObservableCollection<Model_CountryData>();

        public ObservableCollection<Model_ListFilter> ListFilter { get; set; } = new ObservableCollection<Model_ListFilter>();

        private Model_ListFilter _SelectedFilter = new Model_ListFilter();
        public Model_ListFilter SelectedFilter
        {
            get { return _SelectedFilter; }
            set
            {
                Set(nameof(SelectedFilter), ref _SelectedFilter, value);

                if (value != null && value.Id != 0)
                {
                    //    var t = Task.Run(new System.Action(async () =>
                    //    {
                    //        await RefreshData(value.ListFilter);
                    //    }));
                    //    t.ConfigureAwait(false);

                    //    //var uiContent = SynchronizationContext.Current;
                    //    //uiContent.Send(x => RefreshData(value.ListFilter), null);
                }
            }
        }

        private Model_CountryData _SelectedCountry = new Model_CountryData();
        public Model_CountryData SelectedCountry
        {
            get { return _SelectedCountry; }
            set { Set(nameof(SelectedCountry), ref _SelectedCountry, value); }
        }

        private string _CountryLookup = null;
        public string CountryLookup
        {
            get { return _CountryLookup; }
            set
            {
                Set(nameof(CountryLookup), ref _CountryLookup, value);

                if(!string.IsNullOrWhiteSpace(value))
                {
                    SearchCountry();
                }
            }
        }
        #endregion

        #region commands
        public ICommand Command_SelectedCountry { get; set; }
        #endregion

        #region ctors
        public ViewModel_Dashboard(IIoC ioc, IAPILocator api)
        {
            this.API = api;

            this.IoC = ioc;

            this.Nav = this.IoC.GI<INavService>();

            InitCommands();
            RuntimeData();
        }
        #endregion

        #region command methods
        void Command_SelectedCountry_Click(Model_CountryData countryData)
        {
            this.Nav.GoToPage(COVID19Tracker.Library.Enums.Enum_NavService_Pages.RegionPage, countryData.CountryCode);
        }
        #endregion

        #region methods
        public override void InitCommands()
        {
            base.InitCommands();

            if (Command_SelectedCountry == null) Command_SelectedCountry = new RelayCommand<Model_CountryData>(Command_SelectedCountry_Click);
        }

        void DesignData()
        {

        }

        void RuntimeData()
        {
            ListFilter.Clear();

            ListFilter.Add(new Model_ListFilter()
            {
                Id = 1,
                ListTypeName = "Global",
                ListFilter = Enums.Enums_ListFilter.GLOBAL
            });
            ListFilter.Add(new Model_ListFilter()
            {
                Id = 2,
                ListTypeName = "ASEAN",
                ListFilter = Enums.Enums_ListFilter.ASEAN
            });
        }

        public async Task RefreshData(Enums_ListFilter listFilter = Enums_ListFilter.GLOBAL)
        {
            if (this.IsLoading
                //|| this._currentFilter == listFilter
                )
            {
                return;
            };

            i++; Debug.WriteLine(i);

            this.IsLoading = true;

            this._currentFilter = listFilter;

            ResponseData responseData = null;

            if (listFilter == Enums_ListFilter.GLOBAL || listFilter == Enums_ListFilter.NONE)
            {
                responseData = await this.API.Country.GetGlobal();
            }
            else if (listFilter == Enums_ListFilter.ASEAN)
            {
                responseData = await this.API.Country.GetASEAN();
            }

            if (responseData.Status)
            {
                if (responseData.Result != null)
                {
                    List<DTO_Model_CountryData> countryDataList = (List<DTO_Model_CountryData>)responseData.Result;

                    // update local store for sorting
                    {
                        this._localStore.Clear();
                        for (int i = 0; i < countryDataList.Count; i++)
                        {
                            this._localStore.Add(countryDataList[i]);
                        }
                    }

                    RefreshList(countryDataList);
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

        /// <summary>
        /// This produces slower loading of the list
        /// even though we're clearing everything.
        /// </summary>
        /// <param name="source"></param>
        void LazyUpdateListFromSource(List<DTO_Model_CountryData> source)
        {
            this.Countries.Clear();

            int cas = 0;
            int conf = 0;
            int rec = 0;
            int det = 0;
            DateTime lastupdate = new DateTime();

            for (int i = 0; i < source.Count; i++)
            {
                this.Countries.Add(new Model_CountryData()
                {
                    CountryCode = source[i].countryCode,
                    CountryName = source[i].country,
                    TotalConfirmed = source[i].totalConfirmed,
                    TotalRecovered = source[i].totalRecovered,
                    TotalDeaths = source[i].totalDeaths
                });

                conf += source[i].totalConfirmed;
                rec += source[i].totalRecovered;
                det += source[i].totalDeaths;

                if (source[i].lastUpdated > this.LastUpdate)
                {
                    lastupdate = source[i].lastUpdated;
                }
            }

            // update values in UI
            {
                cas = conf + rec + det;
                this.TotalCases = cas;
                this.TotalConfirmed = conf;
                this.TotalDeaths = det;
                this.TotalRecoveries = rec;
                this.LastUpdate = lastupdate;
            }
        }

        void UpdateListFromSource(List<DTO_Model_CountryData> source)
        {
            bool isEmpty = false;

            if (this.Countries.Count == 0)
            {
                this.Countries.Clear();
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
                var countryw = this.Countries.Where(x => x.CountryName == source[i].country).SingleOrDefault();

                if (countryw != null)
                {
                    int oldIndex = this.Countries.IndexOf(countryw);
                    this.Countries.Move(oldIndex, i);
                }
                else
                {
                    this.Countries.Insert(i, new Model_CountryData()
                    {
                        CountryCode = source[i].countryCode,
                        CountryName = source[i].country,
                        TotalConfirmed = source[i].totalConfirmed,
                        TotalRecovered = source[i].totalRecovered,
                        TotalDeaths = source[i].totalDeaths
                    });
                }

                conf += source[i].totalConfirmed;
                rec += source[i].totalRecovered;
                det += source[i].totalDeaths;

                if (source[i].lastUpdated > this.LastUpdate)
                {
                    lastupdate = source[i].lastUpdated;
                }
            }

            // remove old items
            if (source.Count < this.Countries.Count)
            {
                for (int i = this.Countries.Count - 1; i >= source.Count; i--)
                {
                    this.Countries.RemoveAt(i);
                }
            }

            cas = conf + rec + det;
            this.TotalCases = cas;
            this.TotalConfirmed = conf;
            this.TotalDeaths = det;
            this.TotalRecoveries = rec;
            this.LastUpdate = lastupdate;
        }

        void RefreshList(List<DTO_Model_CountryData> source)
        {
            stopwatch.Restart();
            stopwatch.Start();

            UpdateListFromSource(source);
            //LazyUpdateListFromSource(source);

            stopwatch.Stop();

            Debug.WriteLine("DEBUG> refresh duration: " + stopwatch.ElapsedMilliseconds + "ms - total items: " + source.Count);
        }

        public override void SortByName()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderBy(x => x.country).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByConfirmedCase()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalConfirmed).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByRecovery()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalRecovered).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void SortByDeaths()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalDeaths).ToList();

            RefreshList(this._localStore);

            this.IsLoading = false;
        }

        public override void ApplyFilter()
        {
            if (this.SelectedFilter != null)
            {
                var t = Task.Run(new System.Action(async () =>
                {
                    await RefreshData(this.SelectedFilter.ListFilter);
                    this.CountryLookup = null;
                }));
                t.ConfigureAwait(false);
            }
            else
            {
                this.OnShowMessage?.Invoke(this, "Please select the type of filter by tapping on \"List By\"");
            }

            //    //var uiContent = SynchronizationContext.Current;
            //    //uiContent.Send(x => RefreshData(value.ListFilter), null);
        }

        public override async Task PullToRefresh()
        {
            await RefreshData(this._currentFilter);
        }

        void SearchCountry()
        {
            var country = this.Countries.Where(x => x.CountryName.ToLowerInvariant().Contains(this.CountryLookup.ToLowerInvariant())).FirstOrDefault();

            if (country != null)
            {
                this.OnCountryLookupFound?.Invoke(this, country);
            }
        }

        #endregion
    }
}