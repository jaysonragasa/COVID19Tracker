using covid19phlib.Interfaces;
using covid19phlib.ViewModels;
using COVID19Tracker.Library.APIClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.ViewModels
{
    public class ViewModel_CityDetailedData : VMBase
    {
        #region events

        #endregion

        #region vars

        #endregion

        #region properties

        #endregion

        #region commands

        #endregion

        #region ctors
        public ViewModel_CityDetailedData(IIoC ioc, IAPILocator api)
        {
            InitCommands();

            // used only in UWP & WPF
            // or anything that supports design time updates
            if (base.IsInDesignMode)
            {
                DesignData();
            }
            else
            {
                RuntimeData();
            }
        }
        #endregion

        #region command methods

        #endregion

        #region methods
        void InitCommands()
        {

        }

        void DesignData()
        {

        }

        void RuntimeData()
        {

        }

        public async Task RefreshData()
        {

        }

        public override void SortByName()
        {
            throw new NotImplementedException();
        }

        public override void SortByConfirmedCase()
        {
            throw new NotImplementedException();
        }

        public override void SortByRecovery()
        {
            throw new NotImplementedException();
        }

        public override void SortByDeaths()
        {
            throw new NotImplementedException();
        }

        public override void ApplyFilter()
        {
            throw new NotImplementedException();
        }

        public override Task PullToRefresh()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
