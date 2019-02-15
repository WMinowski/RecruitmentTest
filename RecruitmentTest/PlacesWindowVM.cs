using DomainStandard;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataFormat = RestSharp.DataFormat;

namespace RecruitmentTest
{
    class PlacesWindowVM : INotifyPropertyChanged
    {
        private RestClient _restClient = new RestClient("http://localhost:5000");
        private IErrorChecker _checker;
        private MainWindowVM _mainWindowVM;
        public ObservableCollection<CityVM> Cities { get; } = new ObservableCollection<CityVM>();
        public ObservableCollection<PlaceVM> Places { get; } = new ObservableCollection<PlaceVM>();
        //public List<CustomerPlace> CustomersPlaces { get; set; }  = new List<CustomerPlace>();


        private PlaceVM _selectedPlace;
        public PlaceVM SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                _selectedPlace = value;
                OnPropertyChanged("SelectedPlace");
                if (_selectedPlace != null)
                {
                    TextBoxStreet = _selectedPlace.Street;
                    SelectedCity = Cities.First<CityVM>(x => x.Name == _selectedPlace.City);
                }
            }
        }
        private CityVM _selectedCity;
        public CityVM SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }
        private string _textBoxStreet;
        public string TextBoxStreet
        {
            get { return _textBoxStreet; }
            set
            {
                _textBoxStreet = value;
                OnPropertyChanged("TextBoxStreet");
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                  (_addCommand = new RelayCommand(obj =>
                  {

                      try
                      {

                          PlaceVM place = new PlaceVM(new Place(_mainWindowVM.Places.Last().Id + 1, SelectedCity.ToCity(), TextBoxStreet)); // taking Id from full _mainWindow collection
                          Places.Insert(0, place);
                          SelectedPlace = place;

                          var request = new RestRequest("api/place", Method.POST);
                          request.RequestFormat = DataFormat.Json;
                          request.AddJsonBody(place.Place);
                          var asyncHandler = _restClient.ExecuteAsync(request, r =>
                          {
                              if (r.ResponseStatus == ResponseStatus.Completed)
                              {
                                  MessageBox.Show("New place added"); //TODO: remove before release

                              }
                          });
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }, (obj) => !_checker.HasValidationErrors() && (SelectedPlace == null || SelectedPlace.City != SelectedCity.Name || SelectedPlace.Street != TextBoxStreet)
                  ));
            }
        }

        public PlacesWindowVM(IErrorChecker checker, MainWindowVM mainWindowVM)
        {
            //LoadDatabase();
            _mainWindowVM = mainWindowVM;
            foreach(CityVM c in _mainWindowVM.Cities)
            {
                Cities.Add(c);
            }
            //foreach(PlaceVM p in _mainWindowVM.Places.Where(x=>x.Id==))
            foreach(CustomerPlace cp in _mainWindowVM.CustomersPlaces.Where(x=>x.CustomerId == (_mainWindowVM.SelectedCustomer.Id)))
            {
                Places.Add(_mainWindowVM.Places.First(x => x.Id == cp.CustomerId));
            }
            Places.OrderByDescending(x => x.Id);
            _checker = checker;
            ClearText();
        }

        public void ClearText()
        {
            TextBoxStreet = String.Empty;
            SelectedCity = null;
            SelectedPlace = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
