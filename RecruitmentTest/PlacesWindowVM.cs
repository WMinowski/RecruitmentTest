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
                                  // do smth

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

        private RelayCommand _setPlaceCommand;
        public RelayCommand SetPlaceCommand
        {
            get
            {
                return _setPlaceCommand ??
                  (_setPlaceCommand = new RelayCommand(obj =>
                  {

                      try
                      {

                          //CustomersPlaces Check
                          List<CustomerPlace> newCustomersPlaces = new List<CustomerPlace>();
                          var requestCustomersPlaces = new RestRequest("api/customerPlace", Method.GET);
                          var queryResultCustomersPlaces = _restClient.Execute<List<CustomerPlace>>(requestCustomersPlaces).Data;
                          if (queryResultCustomersPlaces != null)
                          {
                              foreach (CustomerPlace p in queryResultCustomersPlaces)
                              {
                                  newCustomersPlaces.Add(p);
                              }
                          }
                          if (newCustomersPlaces.Count > 0)
                          {
                              MessageBox.Show("Someone has just updated place! Please, resolve this conflict later.");
                              // TODO: send event
                          }


                          // updating customer
                          CustomerVM newcustomer = new CustomerVM(new Customer(_mainWindowVM.SelectedCustomer.Id, _mainWindowVM.TextBoxName, _mainWindowVM.TextBoxFirstName, _mainWindowVM.SelectedDate, SelectedPlace.Place));
                          RestRequest requestUpdate = new RestRequest("api/customer/" + _mainWindowVM.SelectedCustomer.Id.ToString(), Method.PUT);
                          requestUpdate.AddJsonBody(newcustomer.Customer);
                          requestUpdate.RequestFormat = DataFormat.Json;
                          _mainWindowVM.Customers.Remove(_mainWindowVM.SelectedCustomer);

                          _mainWindowVM.Customers.Insert(0, newcustomer);
                          _mainWindowVM.SelectedCustomer = newcustomer;
                          _restClient.Execute(requestUpdate);

                          // adding new customerPlace
                          CustomerPlace customerPlace = new CustomerPlace(0, DateTime.Now, newcustomer.Id, newcustomer.Place.Id);
                          RestRequest requestCustomerPlace = new RestRequest("api/customerPlace/", Method.POST);
                          requestCustomerPlace.RequestFormat = DataFormat.Json;
                          requestCustomerPlace.AddJsonBody(customerPlace);
                          var asyncHandler = _restClient.ExecuteAsync(requestCustomerPlace, r =>
                          {
                              if (r.ResponseStatus == ResponseStatus.Completed)
                              {
                                  // do smth

                              }
                          });
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }, (obj) => !_checker.HasValidationErrors() && (SelectedPlace != null)
                  ));
            }
        }

        public PlacesWindowVM(IErrorChecker checker, MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
            Cities = _mainWindowVM.Cities;
            Places = _mainWindowVM.Places;
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
