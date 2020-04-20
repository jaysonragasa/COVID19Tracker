using covid19phlib.Interfaces;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace covid19phlib.ViewModels
{
	public abstract class VMBase : ViewModelBase
    {
		public INavService Nav { get; set; } = null;
		public IAPILocator API { get; set; } = null;
        public IIoC IoC { get; set; } = null;

        private bool _IsLoading = false;
		public bool IsLoading
		{
			get { return _IsLoading; }
			set { Set(nameof(IsLoading), ref _IsLoading, value); }
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

        private bool _ShowFilter = false;
        public bool ShowFilter
        {
            get { return _ShowFilter; }
            set { Set(nameof(ShowFilter), ref _ShowFilter, value); }
        }

        public ICommand Command_SortByName { get; set; }
        public ICommand Command_SortByConfirmedCases { get; set; }
        public ICommand Command_SortByRecovered { get; set; }
        public ICommand Command_SortByDeaths { get; set; }
        public ICommand Command_PullRefresh { get; set; }
        public ICommand Command_ShowFilter { get; set; }
        public ICommand Command_ApplyFilter { get; set; }
        public ICommand Command_About { get; set; }
        public ICommand Command_Back { get; set; }

        public virtual void InitCommands()
        {
            if (Command_SortByName == null) Command_SortByName = new RelayCommand(Command_SortByName_Click);
            if (Command_SortByConfirmedCases == null) Command_SortByConfirmedCases = new RelayCommand(Command_SortByConfirmedCases_Click);
            if (Command_SortByRecovered == null) Command_SortByRecovered = new RelayCommand(Command_SortByRecovered_Click);
            if (Command_SortByDeaths == null) Command_SortByDeaths = new RelayCommand(Command_SortByDeaths_Click);

            if (Command_PullRefresh == null) Command_PullRefresh = new RelayCommand(Command_PullRefresh_Click);
            if (Command_About == null) Command_About = new RelayCommand(Command_About_Click);
            if (Command_ShowFilter == null) Command_ShowFilter = new RelayCommand(Command_ShowFilter_Click);
            if (Command_ApplyFilter == null) Command_ApplyFilter = new RelayCommand(Command_ApplyFilter_Click);
            if (Command_Back == null) Command_Back = new RelayCommand(Command_Back_Click);
        }

        public void Command_SortByName_Click()
        {
            SortByName();
        }

        public void Command_SortByConfirmedCases_Click()
        {
            SortByConfirmedCase();
        }

        public void Command_SortByRecovered_Click()
        {
            SortByRecovery();
        }

        public void Command_SortByDeaths_Click()
        {
            SortByDeaths();
        }

        public void Command_ShowFilter_Click()
        {
            this.ShowFilter = !this.ShowFilter;
        }

        public void Command_ApplyFilter_Click()
        {
            ApplyFilter();
        }

        public async void Command_PullRefresh_Click()
        {
            await PullToRefresh();
        }

        public void Command_About_Click()
        {
            this.Nav.GoToPage(COVID19Tracker.Library.Enums.Enum_NavService_Pages.About);
        }

        public void Command_Back_Click()
        {
            this.Nav.GoBack();
        }

        public abstract void SortByName();
        public abstract void SortByConfirmedCase();
        public abstract void SortByRecovery();
        public abstract void SortByDeaths();
        public abstract void ApplyFilter();
        public abstract Task PullToRefresh();
    }

}