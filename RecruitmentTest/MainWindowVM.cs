
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using RestSharp;
using DataFormat = RestSharp.DataFormat;
using DomainStandard;
using System.Windows.Controls;

namespace RecruitmentTest
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private RestClient _restClient = new RestClient("http://localhost:5000");
        IErrorChecker _checker;
        
        public ObservableCollection<CustomerVM> Customers { get; } = new ObservableCollection<CustomerVM>();
        public ObservableCollection<CityVM> Cities { get; } = new ObservableCollection<CityVM>();
        public ObservableCollection<PlaceVM> Places { get; } = new ObservableCollection<PlaceVM>();
        public List<PlaceUpdate> PlacesUpdates { get; set; } = new List<PlaceUpdate>();
        

        private CustomerVM _selectedCustomer;
        public CustomerVM SelectedCustomer
        {
            get {
                
                return _selectedCustomer;
            }
            set
            {
                
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
                if (_selectedCustomer != null)
                {
                    
                    TextBoxName = _selectedCustomer.Name;
                    TextBoxFirstName = _selectedCustomer.FirstName;
                    SelectedDate = _selectedCustomer.DateOfBirth;
                    SelectedPlace = Places.First<PlaceVM>(x => x.ToString() == _selectedCustomer.Place);
                    //SelectedCity = Cities.First<CityVM>(x => x.Name == _selectedCustomer.Customer.Place.City.Name);
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
        private PlaceVM _selectedPlace;
        public PlaceVM SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                _selectedPlace = value;
                OnPropertyChanged("SelectedPlace");
                if(_selectedPlace != null)
                {
                    TextBoxStreet = _selectedPlace.Street;
                    SelectedCity = Cities.First<CityVM>(x => x.Name == _selectedPlace.City);
                }
            }
        }

        private string _textBoxName;
        public string TextBoxName
        {
            get { return _textBoxName; }
            set
            {
                _textBoxName = value;
                OnPropertyChanged("TextBoxName");
                
 
            }
        }
        private string _textBoxFirstName;
        public string TextBoxFirstName
        {
            get { return _textBoxFirstName; }
            set
            {
                _textBoxFirstName = value;
                OnPropertyChanged("TextBoxFirstName");
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
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
        

        // adding new object command
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

                          CustomerVM customer = new CustomerVM(new Customer(Customers.Last().Id+1, TextBoxName, TextBoxFirstName, SelectedDate, SelectedPlace.Place));
                          Customers.Insert(0, customer);
                          SelectedCustomer = customer;

                          var request = new RestRequest("api/customer", Method.POST);
                          request.RequestFormat = DataFormat.Json;
                          request.AddJsonBody(customer.Customer);// Serialize Domain.Customer, not CustomerVM!!!
                          var asyncHandler = _restClient.ExecuteAsync(request, r =>
                          {
                              if (r.ResponseStatus == ResponseStatus.Completed)
                              {
                                  MessageBox.Show("New customer added"); //TODO: remove before release

                              }
                          });
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message); 
                      }
                  },(obj)=>AddButtonCondition()
                  ));
            }
        }

        public bool AddButtonCondition()
        {
            return !_checker.HasValidationErrors() // if 1+ fields are empty - return false
                && (SelectedCustomer == null // if no customer selected - return true
                || SelectedCustomer.Name != TextBoxName // if one of selected customer fields != proper textbox entry - return true
                || SelectedCustomer.FirstName != TextBoxFirstName
                || SelectedCustomer.DateOfBirth != SelectedDate
                || SelectedCustomer.Place != SelectedPlace.ToString());
            // if all fields coincide with textbox entries - return false
        }

        private RelayCommand _addPlaceCommand;
        public RelayCommand AddPlaceCommand
        {
            get
            {
                return _addPlaceCommand ??
                  (_addPlaceCommand = new RelayCommand(obj =>
                  {

                      try
                      {

                          PlaceVM place = new PlaceVM(new Place(Places.Last().Id+1, SelectedCity.ToCity(), TextBoxStreet));
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
                  }, (obj) => !_checker.HasPlaceValidationErrors() && (SelectedPlace == null || SelectedPlace.City!=SelectedCity.Name || SelectedPlace.Street != TextBoxStreet)
                  ));
            }
        }

        // delete command
        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new RelayCommand(obj =>
                  {
                      CustomerVM customer = obj as CustomerVM;
                      if (customer != null)
                      {
                          
                          
                          

                              MessageBoxResult mbr = MessageBox.Show("Вы действительно хотите удалить эту запись?",
                                  "Подтвердите действие", MessageBoxButton.YesNo,
                                  MessageBoxImage.Question);
                          if (mbr == MessageBoxResult.Yes)
                          {

                              try
                              {
                                  var request = new RestRequest("api/customer/"+customer.Id.ToString(), Method.DELETE);
                                  

                                  var asyncHandler = _restClient.ExecuteAsync(request, r =>
                                  {
                                      if (r.ResponseStatus == ResponseStatus.Completed)
                                      {
                                          MessageBox.Show("Customer is deleted");

                                      }
                                  });
                                  

                                  Customers.Remove(customer);
                                  
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }

                          }
                          
                      }
                  },
                 (obj) => SelectedCustomer != null));
            }
        }
        private RelayCommand _updCommand;
        public RelayCommand UpdCommand
        {
            get
            {
                return _updCommand ??
                  (_updCommand = new RelayCommand(obj =>
                  {
                      CustomerVM customer = obj as CustomerVM;
                      if (customer != null)
                      {
                          try
                          {
                              //PlacesUpdates Check
                              List<PlaceUpdate> newPlacesUpdates = new List<PlaceUpdate>();
                              var requestPlacesUpdates = new RestRequest("api/placeUpdate", Method.GET);
                              var queryResultPlacesUpdates = _restClient.Execute<List<PlaceUpdate>>(requestPlacesUpdates).Data;
                              if (queryResultPlacesUpdates != null)
                              {
                                  foreach (PlaceUpdate p in queryResultPlacesUpdates)
                                  {
                                      newPlacesUpdates.Add(p);
                                  }
                              }
                              //if (newPlacesUpdates.Where(x => x.CustomerId == customer.Id).Except(PlacesUpdates.Where(x => x.CustomerId == customer.Id)).Count() > 0)
                              if((newPlacesUpdates.Where(x => x.CustomerId == customer.Id).Last()?.UpdateTime ?? DateTime.MinValue) != (PlacesUpdates.Where(x => x.CustomerId == customer.Id).Last()?.UpdateTime ?? DateTime.MinValue))
                              {
                                  MessageBox.Show("Someone has just updated place! Update cancelled.");
                                  return;
                              }
                              //else if(SelectedPlace.Street != TextBoxStreet || SelectedPlace.City != SelectedCity.Name)
                              //{
                              //    // Adding new Place
                              //    Place place = new Place(Places.Last().Id+1, SelectedCity.ToCity(), TextBoxStreet);
                              //    RestRequest requestPlace = new RestRequest("api/placeUpdate/", Method.POST);
                              //    requestPlace.RequestFormat = DataFormat.Json;
                              //    requestPlace.AddJsonBody(place);
                              //    var asyncHandler2 = _restClient.ExecuteAsync(requestPlace, r =>
                              //    {
                              //        if (r.ResponseStatus == ResponseStatus.Completed)
                              //        {
                              //            MessageBox.Show("New place added"); //TODO: remove before release

                              //        }
                              //    });
                              //    PlaceVM placeVM = new PlaceVM(place);
                              //    Places.Add(placeVM);
                              //    SelectedPlace = placeVM;
                              //}


                              CustomerVM newcustomer = new CustomerVM(new Customer(customer.Id, TextBoxName, TextBoxFirstName, SelectedDate, SelectedPlace.Place));
                              RestRequest requestUpdate = new RestRequest("api/customer/" + customer.Id.ToString(), Method.PUT);
                              requestUpdate.AddJsonBody(newcustomer.Customer);
                              requestUpdate.RequestFormat = DataFormat.Json;
                              Customers.Remove(customer);
                              
                              Customers.Insert(0, newcustomer);
                              SelectedCustomer = newcustomer;
                              _restClient.Execute(requestUpdate);

                              PlaceUpdate placeUpdate = new PlaceUpdate(0, DateTime.Now, newcustomer.Id, newcustomer.Customer.Place.Id);
                              RestRequest requestPlaceUpdate = new RestRequest("api/placeUpdate/", Method.POST);
                              requestPlaceUpdate.RequestFormat = DataFormat.Json;
                              requestPlaceUpdate.AddJsonBody(placeUpdate);
                              var asyncHandler = _restClient.ExecuteAsync(requestPlaceUpdate, r =>
                              {
                                  if (r.ResponseStatus == ResponseStatus.Completed)
                                  {
                                      MessageBox.Show("New placeUpdate added"); //TODO: remove before release

                                  }
                              });
                              PlacesUpdates = newPlacesUpdates;
                              //_restClient.Execute(requestPlaceUpdate);
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }

                      }
                  },
                 (obj) => SelectedCustomer != null && SelectedPlace != null));
            }
        }
        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                  (_searchCommand = new RelayCommand(obj =>
                  {

                      
                      Dictionary<string, string> textFields = new Dictionary<string, string>();
                      
                      textFields.Add("Name", TextBoxName);
                      textFields.Add("FirstName", TextBoxFirstName);
                      textFields.Add("DateOfBirth", SelectedDate.ToString("yyyy-MM-dd"));
                      textFields.Add("Place", SelectedPlace?.ToString()??string.Empty);
                      
                      
                      //UI only Filter by LINQ
                      ObservableCollection<CustomerVM> filtered = new ObservableCollection<CustomerVM>(Customers);
                      List<CustomerVM> filteredList = filtered.ToList();
                      
                      List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                      foreach (KeyValuePair<string, string> kvp in textFields)
                      {
                          fields.Add(kvp);
                      }
                      if (fields[0].Value != string.Empty) { filteredList = filteredList.Where(x => x.Name == TextBoxName).ToList(); }
                      if (fields[1].Value != string.Empty) { filteredList = filteredList.Where(x => x.FirstName == TextBoxFirstName).ToList(); }
                      if (fields[2].Value != string.Empty) { filteredList = filteredList.Where(x => x.DateOfBirth == SelectedDate).ToList(); }
                      if (fields[3].Value != string.Empty) { filteredList = filteredList.Where(x => x.Place == SelectedPlace.ToString()).ToList(); }
                      Customers.Clear();
                      foreach (CustomerVM c in filteredList)
                      {
                          Customers.Add(c);
                      }
                  }
                  ));
            }
        }
        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand ??
                  (_loadCommand = new RelayCommand(obj =>
                  {
                      LoadDatabase();
                  }
                  ));
            }
        }

        public MainWindowVM(IErrorChecker checker)
        {
            LoadDatabase();
            _checker = checker;
            ClearText();
        }

        public void ClearText()
        {
            TextBoxName = String.Empty;
            TextBoxFirstName = String.Empty;
            SelectedDate = DateTime.Now;
            TextBoxStreet = String.Empty;
            SelectedCity = null;
            SelectedPlace = null;
        }

        public void LoadDatabase()
        {

            Customers.Clear();
            Cities.Clear();
            Places.Clear();


            var requestCustomers = new RestRequest("api/customer", Method.GET);
            var queryResultCustomers = _restClient.Execute<List<Customer>>(requestCustomers).Data;
            if (queryResultCustomers != null)
            {
                foreach (Customer c in queryResultCustomers)
                {
                    Customers.Add(new CustomerVM(c));
                }
            }

            var requestCities = new RestRequest("api/city", Method.GET);
            var queryResultCities = _restClient.Execute<List<City>>(requestCities).Data;
            if (queryResultCities != null)
            {
                foreach(City c in queryResultCities)
                {
                    Cities.Add(new CityVM(c));
                }
            }

            var requestPlaces = new RestRequest("api/place", Method.GET);
            var queryResultPlaces = _restClient.Execute<List<Place>>(requestPlaces).Data;
            if (queryResultPlaces != null)
            {
                foreach (Place p in queryResultPlaces)
                {
                    Places.Add(new PlaceVM(p));
                }
            }
            var requestPlacesUpdates = new RestRequest("api/placeUpdate", Method.GET);
            var queryResultPlacesUpdates = _restClient.Execute<List<PlaceUpdate>>(requestPlacesUpdates).Data;
            if (queryResultPlacesUpdates != null)
            {
                foreach (PlaceUpdate p in queryResultPlacesUpdates)
                {
                    PlacesUpdates.Add(p);
                }
            }

            ClearText();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
