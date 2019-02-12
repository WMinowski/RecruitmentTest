
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
using Domain;

namespace RecruitmentTest
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private RestClient _restClient = new RestClient("http://localhost:5000");
        //ErrorChecker ec;
        IErrorChecker _checker;
        private CustomerVM _selectedCustomer;
        public ObservableCollection<CustomerVM> Customers { get; } = new ObservableCollection<CustomerVM>();
        public ObservableCollection<CityVM> Cities { get; } = new ObservableCollection<CityVM>();
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
                    //TextBoxId = _selectedCustomer.Id.ToString();
                    TextBoxName = _selectedCustomer.Name;
                    TextBoxFirstName = _selectedCustomer.FirstName;
                    SelectedDate = _selectedCustomer.DateOfBirth;
                    TextBoxStreet = _selectedCustomer.Street;
                    //SelectedCity = Cities.ToList().Find(x => x.Name == _selectedCustomer.City);
                    SelectedCity = Cities.First<CityVM>(x => x.Name == _selectedCustomer.City);
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
        

        // команда добавления нового объекта
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

                          CustomerVM customer = new CustomerVM(new Customer(0, TextBoxName, TextBoxFirstName, SelectedDate, TextBoxStreet, SelectedCity.ToCity()));
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
                || SelectedCustomer.Street != TextBoxStreet
                || SelectedCustomer.City != SelectedCity.Name);
            // if all fields coincide with textbox entries - return false
        }

        // команда удаления
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
                                  //request.AddParameter("id", customer.Id.ToString());

                                  var asyncHandler = _restClient.ExecuteAsync(request, r =>
                                  {
                                      if (r.ResponseStatus == ResponseStatus.Completed)
                                      {
                                          MessageBox.Show("Customer is deleted");

                                      }
                                  });
                                  //_restClient.Execute(request);

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
                              CustomerVM newcustomer = new CustomerVM(new Customer(0, TextBoxName, TextBoxFirstName, SelectedDate, TextBoxStreet, SelectedCity.ToCity()));
                              RestRequest request = new RestRequest("api/customer/" + customer.Id.ToString(), Method.PUT);
                              request.AddJsonBody(newcustomer.Customer);
                              request.RequestFormat = DataFormat.Json;
                              Customers.Remove(customer);
                              
                              Customers.Insert(0, newcustomer);
                              SelectedCustomer = newcustomer;
                              _restClient.Execute(request);
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }

                      }
                  },
                 (obj) => SelectedCustomer != null));
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
                      textFields.Add("Street", TextBoxStreet);
                      textFields.Add("CityId", SelectedCity.Id.ToString());
                      
                      //UI only Filter by LINQ
                      ObservableCollection<CustomerVM> filtered = new ObservableCollection<CustomerVM>(Customers);
                      List<CustomerVM> filteredList = filtered.ToList();
                      
                      List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                      foreach (KeyValuePair<string, string> kvp in textFields)
                      {
                          fields.Add(kvp);
                      }
                      //if (fields[0].Value != string.Empty) { filteredList = (filteredList.Where(x => x.Id == int.Parse(TextBoxId))).ToList(); }
                      if (fields[0].Value != string.Empty) { filteredList = filteredList.Where(x => x.Name == TextBoxName).ToList(); }
                      if (fields[1].Value != string.Empty) { filteredList = filteredList.Where(x => x.FirstName == TextBoxFirstName).ToList(); }
                      if (fields[2].Value != string.Empty) { filteredList = filteredList.Where(x => x.DateOfBirth == SelectedDate).ToList(); }
                      if (fields[3].Value != string.Empty) { filteredList = filteredList.Where(x => x.Street == TextBoxStreet).ToList(); }
                      if (fields[4].Value != string.Empty) { filteredList = filteredList.Where(x => x.City == SelectedCity.Name).ToList(); }
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
        }

        public void LoadDatabase()
        {

            Customers.Clear();
            Cities.Clear();


            var request = new RestRequest("api/customer", Method.GET);
            var queryResult = _restClient.Execute<List<Customer>>(request).Data;
            if (queryResult != null)
            {
                foreach (Customer c in queryResult)
                {
                    Customers.Add(new CustomerVM(c));
                }
            }

            var request2 = new RestRequest("api/city", Method.GET);
            var queryResult2 = _restClient.Execute<List<City>>(request2).Data;
            if (queryResult2 != null)
            {
                foreach(City c in queryResult2)
                {
                    Cities.Add(new CityVM(c));
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
