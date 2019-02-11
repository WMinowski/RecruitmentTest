
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestSharp;
using DataFormat = RestSharp.DataFormat;
using System.Reflection;

using Domain;

namespace RecruitmentTest
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        //MySqlConnection connection =
        //    new MySqlConnection(@"Server=localhost;Database=mydb;Uid=root;password=Password*;Convert Zero Datetime = true; Allow User Variables = True;");
        //MySqlDataAdapter adapter = new MySqlDataAdapter();
        private CustomerVM _selectedCustomer;
        public ObservableCollection<CustomerVM> Customers;
        public List<CityVM> Cities;
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
                    TextBoxId = _selectedCustomer.Id.ToString();
                    TextBoxName = _selectedCustomer.Name;
                    TextBoxFirstName = _selectedCustomer.FirstName;
                    SelectedDate = _selectedCustomer.DateOfBirth;
                    TextBoxStreet = _selectedCustomer.Street;
                    SelectedCity = Cities.ToList().Find(x => x.Name == _selectedCustomer.City);
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
        private string _textBoxId;
        public string TextBoxId
        {
            get { return _textBoxId; }
            set
            {
                _textBoxId = value;
                OnPropertyChanged("TextBoxId");


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
        private RestClient _restClient = new RestClient("http://localhost:5000");

        // команда добавления нового объекта
        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                  (_addCommand = new RelayCommand(obj =>
                  {
                      if (TextBoxId==string.Empty|| TextBoxName == String.Empty || TextBoxFirstName == String.Empty ||
                        SelectedDate == null || TextBoxStreet == String.Empty ||
                        SelectedCity == null)
                      {
                          MessageBox.Show("Будь ласка, введіть дані у всі поля.");
                      }
                      else
                      {
                          try
                          {
                              //adapter.InsertCommand = new MySqlCommand(
                              //    "INSERT INTO mydb.customers " +
                              //    "VALUES(@Name,@FirstName,@DateOfBirth,@Street,@City);",
                              //    connection);
                              //adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = TextBoxName;
                              //adapter.InsertCommand.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = TextBoxFirstName;
                              //adapter.InsertCommand.Parameters.Add("@DateOfBirth", MySqlDbType.Date).Value = SelectedDate.ToString("yyyy-MM-dd");
                              //adapter.InsertCommand.Parameters.Add("@Street", MySqlDbType.VarChar).Value = TextBoxStreet;
                              //adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.Int32).Value = SelectedCity.Id.ToString();


                              //connection.Open();
                              //adapter.InsertCommand.ExecuteNonQuery();
                              //connection.Close();


                              CustomerVM customer = new CustomerVM(new Customer(int.Parse(TextBoxId), TextBoxName, TextBoxFirstName, SelectedDate, TextBoxStreet, SelectedCity.ToCity()));
                              Customers.Insert(0, customer);
                              SelectedCustomer = customer;

                              var request = new RestRequest("api/customer", Method.POST);
                              request.RequestFormat = DataFormat.Json;
                              request.AddJsonBody(customer.Customer);// Serialize Domain.Customer, not CustomerVM!!!
                              var asyncHandler = _restClient.ExecuteAsync(request, r =>
                              {
                                  if (r.ResponseStatus == ResponseStatus.Completed)
                                  {
                                      MessageBox.Show("New customer added");

                                  }
                              });
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }

                      }

                      
                      //DataBase

                  },(obj)=>(SelectedCustomer==null||SelectedCustomer.Id!=int.Parse(TextBoxId)||SelectedCustomer.Name!=TextBoxName||SelectedCustomer.FirstName!=TextBoxFirstName||SelectedCustomer.DateOfBirth!=SelectedDate||SelectedCustomer.Street!=TextBoxStreet||SelectedCustomer.City!=SelectedCity.Name)
                  ));
            }
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
                          
                          //DataBase
                          

                              MessageBoxResult mbr = MessageBox.Show("Вы действительно хотите удалить эту запись?",
                                  "Подтвердите действие", MessageBoxButton.YesNo,
                                  MessageBoxImage.Question);
                          if (mbr == MessageBoxResult.Yes)
                          {

                              try
                              {
                                  //adapter.DeleteCommand =
                                  //    new MySqlCommand("DELETE FROM mydb.customers WHERE (Name = '" + SelectedCustomer.Name + "') AND (FirstName = '"
                                  //    + SelectedCustomer.FirstName + "');", connection);
                                  //connection.Open();
                                  //adapter.DeleteCommand.ExecuteNonQuery();
                                  //connection.Close();

                                  var request = new RestRequest("api/customer/{id}", Method.DELETE);
                                  request.AddParameter("id", customer.Id);

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

                          //DataBase


                          //string query =
                          //  @"UPDATE customers SET Name = '" + TextBoxName +
                          //  @"',FirstName = '" + TextBoxFirstName +
                          //  @"',DateOfBirth = '" + SelectedDate.ToString("yyyy-MM-dd") + //FIX!!! Parse with proper separators
                          //  @"' ,Street = '" + TextBoxStreet +
                          //  @"',CityId = " + SelectedCity.Id.ToString() +
                          //  @" WHERE (" +
                          //  @"Name = '" + SelectedCustomer.Name +
                          //  @"') AND (FirstName = '" + SelectedCustomer.FirstName + @"')";
                          if (TextBoxId == string.Empty || TextBoxName == String.Empty || TextBoxFirstName == String.Empty ||
                        SelectedDate == null || TextBoxStreet == String.Empty ||
                        SelectedCity == null)
                          {
                              MessageBox.Show("Будь ласка, введіть дані у всі поля.");
                          }
                          //MySqlDataAdapter localAadapter = new MySqlDataAdapter();
                          try
                          {
                              //localAadapter.UpdateCommand = new MySqlCommand(query, connection);
                              //connection.Open();
                              //localAadapter.UpdateCommand.ExecuteNonQuery();
                              //connection.Close();
                              //dataGrid1.ItemsSource = null;
                              //((MainWindowVM)DataContext).Customers.Clear();
                              //((MainWindowVM)DataContext).LoadDatabase("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id");
                              //dataGrid1.ItemsSource = ((MainWindowVM)DataContext).Customers;

                              

                              Customers.Remove(customer);
                              CustomerVM newcustomer = new CustomerVM(new Customer(int.Parse(TextBoxId), TextBoxName, TextBoxFirstName, SelectedDate, TextBoxStreet, SelectedCity.ToCity()));
                              Customers.Insert(0, newcustomer);
                              SelectedCustomer = newcustomer;

                              RestRequest request = new RestRequest("api/customer/{id}", Method.PUT);
                              request.AddUrlSegment("id", customer.Id);//getting property from removed item
                              request.AddObject(newcustomer.Customer);// Serialize Domain.Customer, not CustomerVM!!!


                              var asyncHandler = _restClient.ExecuteAsync(request, r =>
                              {
                                  if (r.ResponseStatus == ResponseStatus.Completed)
                                  {
                                      MessageBox.Show("Customer is updated");
                                  }
                              });
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

                      //string query = "select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id where";
                      Dictionary<string, string> textFields = new Dictionary<string, string>();
                      textFields.Add("Id", TextBoxId);
                      textFields.Add("Name", TextBoxName);
                      textFields.Add("FirstName", TextBoxFirstName);
                      textFields.Add("DateOfBirth", SelectedDate.ToString("yyyy-MM-dd"));
                      textFields.Add("Street", TextBoxStreet);
                      textFields.Add("CityId", SelectedCity.Id.ToString());
                      //do
                      //{
                      //    if (textFields.Values.First() != string.Empty)
                      //    {
                      //        query += ("(" + textFields.Keys.First() + " = '" + textFields.Values.First() + "')");
                      //        break;
                      //    }
                      //    else
                      //    {
                      //        textFields.Remove(textFields.Keys.First());
                      //    };
                      //} while (textFields.Keys.Count != 0);
                      //if (textFields.Keys.Count == 0) { return; }
                      //foreach (KeyValuePair<string, string> kvp in textFields.Skip(1))
                      //{

                      //    if (kvp.Value != string.Empty) { query += ("and(" + kvp.Key + " = '" + kvp.Value + "')"); }

                      //}
                      //UI only Filter by LINQ
                      ObservableCollection<CustomerVM> filtered = new ObservableCollection<CustomerVM>(Customers);
                      List<CustomerVM> filteredList = filtered.ToList();
                      //foreach(KeyValuePair<string,string> kvp in textFields)
                      //{
                      //    if (kvp.Value != string.Empty) {
                      //        filtered = filtered.Where(x => Type.GetProperty(kvp.Key).GetValue(x) == kvp.Value);
                      //    }
                      //}
                      List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                      foreach (KeyValuePair<string, string> kvp in textFields)
                      {
                          fields.Add(kvp);
                      }
                      if (fields[0].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.Id == int.Parse(TextBoxId)); }
                      if (fields[1].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.Name == TextBoxName); }
                      if (fields[2].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.FirstName == TextBoxFirstName); }
                      if (fields[3].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.DateOfBirth == SelectedDate); }
                      if (fields[4].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.Street == TextBoxStreet); }
                      if (fields[5].Value != string.Empty) { filteredList = (List<CustomerVM>)filteredList.Where(x => x.City == SelectedCity.Name); }
                      Customers.Clear();
                      foreach (CustomerVM c in filteredList)
                      {
                          Customers.Add(c);
                      }



                      //DataBase
                      //DataSet ds1 = new DataSet();
                      //try
                      //{

                      //    //adapter.SelectCommand = new MySqlCommand("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id", connection);
                      //    adapter.SelectCommand = new MySqlCommand(query, connection);
                      //    ds1.Clear();
                      //    adapter.Fill(ds1);
                      //    //dataGrid1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dataGrid1 });
                      //    //dataGrid1.ItemsSource = (IEnumerable)ds.Tables[0];

                      //    connection.Open();
                      //    adapter.SelectCommand.ExecuteNonQuery();
                      //    connection.Close();

                      //    //ClearText();
                      //}
                      //catch (Exception ex)
                      //{
                      //    MessageBox.Show(ex.Message);

                      //}

                      //ClearText();
                      //Customers.Clear();

                      //DataTable dt = ds1.Tables[0];
                      ////ObservableCollection<Customer> searchResult = new ObservableCollection<Customer>();
                      //for (int i = 0; i < dt.Rows.Count; i++)
                      //{
                      //    string name = dt.Rows[i].ItemArray[0].ToString();
                      //    string firstName = dt.Rows[i].ItemArray[1].ToString();
                      //    DateTime dateTime = DateTime.Parse(dt.Rows[i].ItemArray[2].ToString());
                      //    string street = dt.Rows[i].ItemArray[3].ToString();
                      //    string city = dt.Rows[i].ItemArray[4].ToString();
                      //    Customer c = new Customer(name, firstName, dateTime, street, city);
                      //    Customers.Add(c);
                      //}
                      //
                      //


                  }
                  ));
            }
        }

        public MainWindowVM()
        {

            LoadDatabase();
            
            //LoadDatabase("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id");

            //TODO: Load&Init Database

        }

        private void ClearText()
        {
            TextBoxName = String.Empty;
            TextBoxFirstName = String.Empty;
            SelectedDate = DateTime.Now;
            TextBoxStreet = String.Empty;
            SelectedCity = null;
        }

        public void LoadDatabase()
        {
            Customers = new ObservableCollection<CustomerVM>();
            Cities = new List<CityVM>();

            ///RestSharp part
            ///
            
            var request = new RestRequest("api/customer", Method.GET);
            //var asyncHandler = _restClient.ExecuteAsync<List<Customer>>(request, r =>
            //{
            //    if (r.ResponseStatus == ResponseStatus.Completed)
            //    {

            //        foreach (Customer c in r.Data)
            //        {
            //            tempListCustomers.Add(c);
            //        };//

            //    }
            //    else MessageBox.Show("Error while loading data!");
            //});
            //var queryres = _restClient.Execute<List<Customer>>(request);

            var queryResult = _restClient.Execute<List<Customer>>(request).Data;

            if (queryResult != null)
            {
                foreach (Customer c in queryResult)
                {
                    Customers.Add(new CustomerVM(c));
                }
            }

            var request2 = new RestRequest("api/city", Method.GET);
            //var asyncHandler2 = _restClient.ExecuteAsync<List<City>>(request2, r =>
            //{
            //    if (r.ResponseStatus == ResponseStatus.Completed)
            //    {
            //        tempListCities = r.Data;//
            //    }
            //});
            var queryResult2 = _restClient.Execute<List<City>>(request2).Data;
            if (queryResult2 != null)
            {
                foreach(City c in queryResult2)
                {
                    Cities.Add(new CityVM(c));
                }
            }

            
            ClearText();

            //MySqlConnection connection =
            //new MySqlConnection(@"Server=localhost;Database=mydb;Uid=root;password=Password*;Convert Zero Datetime = true; Allow User Variables = True;");
            //MySqlDataAdapter adapter = new MySqlDataAdapter();

            //DataSet ds2 = new DataSet();
            //try
            //{
            //    adapter.SelectCommand = new MySqlCommand("SELECT * FROM mydb.cities", connection);
            //    ds2.Clear();
            //    adapter.Fill(ds2);
            //    //dataGrid1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dataGrid1 });
            //    //dataGrid1.ItemsSource = (IEnumerable)ds.Tables[0];

            //    connection.Open();
            //    adapter.SelectCommand.ExecuteNonQuery();
            //    connection.Close();

            //    //ClearText();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);

            //}
            //Cities = new ObservableCollection<CityVM>();

            //DataTable dt = ds2.Tables[0];
            //for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            //{
            //    string str = ds2.Tables[0].Rows[i].ItemArray[0].ToString();
            //    int id = Convert.ToInt32(str);
            //    string name = ds2.Tables[0].Rows[i].ItemArray[1].ToString();
            //    Cities.Add(new CityVM(id
            //        , name));
            //}

            //DataSet ds1 = new DataSet();
            //try
            //{

            //    //adapter.SelectCommand = new MySqlCommand("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id", connection);
            //    adapter.SelectCommand = new MySqlCommand("SELECT * FROM mydb.customers", connection);
            //    ds1.Clear();
            //    adapter.Fill(ds1);
            //    //dataGrid1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dataGrid1 });
            //    //dataGrid1.ItemsSource = (IEnumerable)ds.Tables[0];

            //    connection.Open();
            //    adapter.SelectCommand.ExecuteNonQuery();
            //    connection.Close();

            //    //ClearText();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);

            //}


            //Customers = new ObservableCollection<CustomerVM>();
            //dt = ds1.Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    int id = int.Parse(dt.Rows[i].ItemArray[0].ToString());
            //    string name = dt.Rows[i].ItemArray[1].ToString();
            //    string firstName = dt.Rows[i].ItemArray[2].ToString();
            //    DateTime dateTime = DateTime.Parse(dt.Rows[i].ItemArray[3].ToString());
            //    string street = dt.Rows[i].ItemArray[4].ToString();
            //    Domain.City city = Cities.ToList().Find(x=>x.Id==int.Parse(dt.Rows[i].ItemArray[5].ToString())).ToCity();
            //    CustomerVM c = new CustomerVM(new Domain.Customer(id, name, firstName, dateTime, street, city));
            //    Customers.Add(c);
            //}


        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
