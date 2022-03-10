using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Covid19.Models;
using Xamarin.Forms;

namespace Covid19.ViewModels
{
    public class RefreshData : INotifyPropertyChanged
    {
        #region Fields 
        private string _name;
        private bool _isRefreshing;
        private Command _refreshViewCommand;
        public INavigation Navigation { get; set; }

        #endregion

        #region Constructor
        public RefreshData(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public RefreshData()
        {
        }

        #endregion

        #region Properties  

        public string Name
        {
            get => _name;
            set => _name = value;
        }


        public Command RefreshViewCommand
        {
            get
            {
                return new Command(() =>
                {
                    RefreshTotalData();
                });
            }
        }


        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods  

        private void RefreshTotalData()
        {
            //Do your stuff
            //collectionSkills.
            this.Name = "Delpin";
            this.IsRefreshing = false;
        }

        ObservableCollection<CountryModel> _collectionSkills;
        public ObservableCollection<CountryModel> collectionSkills
        {
            get
            {
                return _collectionSkills;
            }
            set
            {
                _collectionSkills = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

