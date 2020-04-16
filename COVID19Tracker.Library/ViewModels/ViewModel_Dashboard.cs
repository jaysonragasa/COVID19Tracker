﻿using covid19phlib.APIClient;
using covid19phlib.BO_Models;
using covid19phlib.DTO_Models;
using covid19phlib.Enums;
using covid19phlib.Interfaces;

using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace covid19phlib.ViewModels
{
    public class ViewModel_Dashboard : VMBase
    {
        #region events

        #endregion

        #region vars
        int i = 0;
        Enums_ListFilter _currentFilter = Enums_ListFilter.NONE;
        IIoC _ioc;
        List<DTO_Model_CountryData> _localStore = new List<DTO_Model_CountryData>();
        string[] _asean = new string[] {
            "ID", // indonesia
            "MY", // malaysia
            "PH", // philippines
            "SG", // singapore
            "TH", // thailand
            "BN", // brunai
            "LA", // laos
            "MM", // myanmar
            "KH", // cambodia
            "VN"  // vietname
        };
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

                if (value != null)
                {
                    var t = Task.Run(new System.Action(async () =>
                    {
                        await RefreshData(value.ListFilter);
                    }));
                    t.ConfigureAwait(false);

                    //var uiContent = SynchronizationContext.Current;
                    //uiContent.Send(x => RefreshData(value.ListFilter), null);
                }
            }
        }

        private bool _IsRefreshing = false;
        // this will be bound to IsRefreshing property in ListView
        // I could've use the IsLoading but something is not right
        // when using it.
        public bool IsRefreshing
        {
            get { return _IsRefreshing; }
            set { Set(nameof(IsRefreshing), ref _IsRefreshing, value); }
        }

        private int _TotalCases = 0;
        public int TotalCases
        {
            get { return _TotalCases; }
            set { Set(nameof(TotalCases), ref _TotalCases, value); }
        }

        private int _TotalActive = 0;
        public int TotalConfirmed
        {
            get { return _TotalActive; }
            set { Set(nameof(TotalConfirmed), ref _TotalActive, value); }
        }

        private int _TotalRecovery = 0;
        public int TotalRecoveries
        {
            get { return _TotalRecovery; }
            set { Set(nameof(TotalRecoveries), ref _TotalRecovery, value); }
        }

        private int _TotalDetahs = 0;
        public int TotalDeaths
        {
            get { return _TotalDetahs; }
            set { Set(nameof(TotalDeaths), ref _TotalDetahs, value); }
        }

        private DateTime _LastUpdate = new DateTime();
        public DateTime LastUpdate
        {
            get { return _LastUpdate; }
            set { Set(nameof(LastUpdate), ref _LastUpdate, value); }
        }
        #endregion

        #region commands
        public ICommand Command_SortByCountryName { get; set; }
        public ICommand Command_SortByConfirmedCases { get; set; }
        public ICommand Command_SortByRecovered { get; set; }
        public ICommand Command_SortByDeaths { get; set; }
        public ICommand Command_PullRefresh { get; set; }
        #endregion

        #region ctors
        public ViewModel_Dashboard(IIoC ioc)
        {
            this._ioc = ioc;

            InitCommands();
            RuntimeData();
        }
        #endregion

        #region command methods
        void Command_SortByCountryName_Click()
        {
            SortByCountryName();
        }

        void Command_SortByConfirmedCases_Click()
        {
            SortByConfirmedCase();
        }

        void Command_SortByRecovered_Click()
        {
            SortByRecovery();
        }

        void Command_SortByDeaths_Click()
        {
            SortByDeaths();
        }

        async void Command_PullRefresh_Click()
        {
            await RefreshData(this._currentFilter);
        }
        #endregion

        #region methods
        void InitCommands()
        {
            if (Command_SortByCountryName == null) Command_SortByCountryName = new RelayCommand(Command_SortByCountryName_Click);
            if (Command_SortByConfirmedCases == null) Command_SortByConfirmedCases = new RelayCommand(Command_SortByConfirmedCases_Click);
            if (Command_SortByRecovered == null) Command_SortByRecovered = new RelayCommand(Command_SortByRecovered_Click);
            if (Command_SortByDeaths == null) Command_SortByDeaths = new RelayCommand(Command_SortByDeaths_Click);
            if (Command_PullRefresh == null) Command_PullRefresh = new RelayCommand(Command_PullRefresh_Click);
        }

        void DesignData()
        {

        }

        void RuntimeData()
        {
            ListFilter.Clear();
            ListFilter.Add(new Model_ListFilter()
            {
                Id = 0,
                ListTypeName = "Global",
                ListFilter = Enums.Enums_ListFilter.GLOBAL
            });
            ListFilter.Add(new Model_ListFilter()
            {
                Id = 1,
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

            var apiloc = this._ioc.GI<APILocator>();
            var res = await apiloc.Country.GetCountryData();

            if (res.Status)
            {
                var countryData = (List<DTO_Model_CountryData>)res.Result;
                List<DTO_Model_CountryData> countryDataList = countryData.ToList();

                if (listFilter == Enums_ListFilter.GLOBAL || listFilter == Enums_ListFilter.NONE)
                {
                    countryDataList = countryDataList.OrderByDescending(x => x.totalConfirmed).ToList();
                }
                else if (listFilter == Enums_ListFilter.ASEAN)
                {
                    countryDataList = countryDataList.Where(x => this._asean.Contains(x.countryCode)).OrderByDescending(x => x.totalConfirmed).ToList();
                }

                countryData = null;

                // update local store for sorting
                {
                    this._localStore.Clear();
                    for (int i = 0; i < countryDataList.Count; i++)
                    {
                        this._localStore.Add(countryDataList[i]);
                    }
                }

                UpdateListFromSource(countryDataList);
            }
            else
            {

            }

            this.IsLoading = false;
            this.IsRefreshing = false;
        }

        void UpdateListFromSource(List<DTO_Model_CountryData> source)
        {
            this.Countries.Clear();

            int cas = 0;
            int conf = 0;
            int rec = 0;
            int det = 0;

            this.TotalCases = 0;
            this.TotalConfirmed = 0;
            this.TotalDeaths = 0;
            this.TotalRecoveries = 0;

            for (int i = 0; i < source.Count; i++)
            {
                this.Countries.Add(new Model_CountryData()
                {
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
                    this.LastUpdate = source[i].lastUpdated;
                }
            }

            cas = conf + rec + det;
            this.TotalCases = cas;
            this.TotalConfirmed = conf;
            this.TotalDeaths = det;
            this.TotalRecoveries = rec;
        }

        void SortByCountryName()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderBy(x => x.country).ToList();

            UpdateListFromSource(this._localStore);

            this.IsLoading = false;
        }

        void SortByConfirmedCase()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalConfirmed).ToList();

            UpdateListFromSource(this._localStore);

            this.IsLoading = false;
        }

        void SortByRecovery()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalRecovered).ToList();

            UpdateListFromSource(this._localStore);

            this.IsLoading = false;
        }

        void SortByDeaths()
        {
            this.IsLoading = true;

            this._localStore = this._localStore.OrderByDescending(x => x.totalDeaths).ToList();

            UpdateListFromSource(this._localStore);

            this.IsLoading = false;
        }

        #endregion
    }
}