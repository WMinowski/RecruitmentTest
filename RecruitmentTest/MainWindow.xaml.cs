using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using Microsoft.VisualBasic;

namespace RecruitmentTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM();
            ClearText();
        }
        
        //string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        //MySqlConnection connection =
        //    new MySqlConnection(@"Server=localhost;Database=mydb;Uid=root;password=Password*;Convert Zero Datetime = true; Allow User Variables = True;");
        //MySqlDataAdapter adapter = new MySqlDataAdapter();
        //DataSet ds = new DataSet();
        //listView1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = YourData });

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if ((string)e.Column.Header == "_customer")
            {
                e.Cancel = true;
            }
            if ((string)e.Column.Header == "Customer")
            {
                e.Cancel = true;
            }
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy-MM-dd";
        }



        private void ClearText()
        {
            textBoxName.Text = String.Empty;
            textBoxFirstName.Text = String.Empty;
            textBoxStreet.Text = String.Empty;
            comboBoxCity.Text = String.Empty;
        }





        //private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (dataGrid1.SelectedCells.Count != 0)
        //    {
        //        textBoxName.Text = (dataGrid1.SelectedCells[0].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text;
        //        textBoxFirstName.Text = (dataGrid1.SelectedCells[1].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text;
                
        //        datePicker.SelectedDate = DateTime.Parse((dataGrid1.SelectedCells[2].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text);
        //        textBoxStreet.Text = (dataGrid1.SelectedCells[3].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text;
        //        for (int i = 0; i < ((MainWindowVM)DataContext).Cities.Count; i++)
        //        {
        //            if (((MainWindowVM)DataContext).Cities.ElementAtOrDefault<City>(i).Name == (dataGrid1.SelectedCells[4].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text)
        //                comboBoxCity.SelectedItem = ((MainWindowVM)DataContext).Cities.ElementAtOrDefault<City>(i);
        //        }

        //    }
        //}

        //private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    if (((MainWindowVM)DataContext).SelectedCustomer != null)
        //    {
        //        textBoxName.Text = ((MainWindowVM)DataContext).SelectedCustomer.Name;
        //        textBoxFirstName.Text = ((MainWindowVM)DataContext).SelectedCustomer.FirstName;
        //        datePicker.SelectedDate = ((MainWindowVM)DataContext).SelectedCustomer.DateOfBirth;
        //        textBoxStreet.Text = ((MainWindowVM)DataContext).SelectedCustomer.Street;
        //        comboBoxCity.Text = ((MainWindowVM)DataContext).SelectedCustomer.City;
        //    }
        //}

        //private void Remove_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGrid1.SelectedItems.Count != 0)
        //    {
                
        //        MessageBoxResult mbr = MessageBox.Show("Вы действительно хотите удалить эту запись?",
        //            "Подтвердите действие", MessageBoxButton.YesNo,
        //            MessageBoxImage.Question);
        //        if (mbr == MessageBoxResult.Yes)
        //        {
        //            if (dataGrid1.SelectedItems.Count > 0)
        //            {
        //                try
        //                {
        //                    adapter.DeleteCommand = 
        //                        new MySqlCommand("DELETE FROM mydb.customers WHERE (Name = '" + textBoxName.Text + "') AND (FirstName = '"
        //                        + textBoxFirstName.Text + "');", connection);
        //                    connection.Open();
        //                    adapter.DeleteCommand.ExecuteNonQuery();
        //                    connection.Close();
                            
                            
        //                    ((MainWindowVM)DataContext).Customers.Remove(Array.Find<Customer>(((MainWindowVM)DataContext).Customers.ToArray(),(x)=>(x.Name==textBoxName.Text
        //                    &&x.FirstName==textBoxFirstName.Text
        //                    &&x.DateOfBirth== DateTime.Parse(datePicker.Text)
        //                    && x.Street==textBoxStreet.Text
        //                    &&x.City==comboBoxCity.Text)));
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                }
        //            }
        //        }
        //    }
            
        //}

        //private void Add_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (textBoxName.Text == String.Empty || textBoxFirstName.Text == String.Empty ||
        //        datePicker.SelectedDate == null || textBoxStreet.Text == String.Empty ||
        //        comboBoxCity.Text == String.Empty)
        //    {
        //        MessageBox.Show("Будь ласка, введіть дані у всі поля.");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            adapter.InsertCommand = new MySqlCommand(
        //                "INSERT INTO mydb.customers " +
        //                "VALUES(@Name,@FirstName,@DateOfBirth,@Street,@City);",
        //                connection);
        //            adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = textBoxName.Text;
        //            adapter.InsertCommand.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = textBoxFirstName.Text;
        //            adapter.InsertCommand.Parameters.Add("@DateOfBirth", MySqlDbType.Date).Value = DateTime.Parse(datePicker.Text);
        //            adapter.InsertCommand.Parameters.Add("@Street", MySqlDbType.VarChar).Value = textBoxStreet.Text;
        //            adapter.InsertCommand.Parameters.Add("@City", MySqlDbType.Int32).Value = ((City)comboBoxCity.SelectedItem).Id;
                    

        //            connection.Open();
        //            adapter.InsertCommand.ExecuteNonQuery();
        //            connection.Close();
        //            ((MainWindowVM)DataContext).Customers.Add(new Customer(textBoxName.Text, textBoxFirstName.Text, DateTime.Parse(datePicker.Text), textBoxStreet.Text, comboBoxCity.Text));
        //            ClearText();
                    
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
                
        //    }
        //}

        //private void Update_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGrid1.SelectedItems.Count == 0)
        //    {
        //        MessageBox.Show("Пожалуйста, выберите запись!");
        //    }
        //    else
        //    {
        //        string query = 
        //            @"UPDATE customers SET Name = '" + textBoxName.Text +
        //            @"',FirstName = '" + textBoxFirstName.Text +
        //            @"',DateOfBirth = '" + datePicker.SelectedDate.Value.ToString("yyyy-MM-dd") + //FIX!!! Parse with proper separators
        //            @"' ,Street = '" + textBoxStreet.Text +
        //            @"',CityId = " + ((City)comboBoxCity.SelectedItem).Id +
        //            @" WHERE (" +
        //            @"Name = '" + (dataGrid1.SelectedCells[0].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text +
        //            @"') AND (FirstName = '" + (dataGrid1.SelectedCells[1].Column.GetCellContent(dataGrid1.SelectedItem) as TextBlock).Text + @"')";
        //        if (textBoxName.Text == String.Empty || textBoxFirstName.Text == String.Empty ||
        //        datePicker.SelectedDate == null || textBoxStreet.Text == String.Empty ||
        //        comboBoxCity.Text == String.Empty)
        //        {
        //            MessageBox.Show("Будь ласка, введіть дані у всі поля.");
        //        }
        //        MySqlDataAdapter localAadapter = new MySqlDataAdapter();
        //        try
        //        {
        //            localAadapter.UpdateCommand = new MySqlCommand(query, connection);
        //            connection.Open();
        //            localAadapter.UpdateCommand.ExecuteNonQuery();
        //            connection.Close();
        //            dataGrid1.ItemsSource = null;
        //            ((MainWindowVM)DataContext).Customers.Clear();
        //            ((MainWindowVM)DataContext).LoadDatabase("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id");
        //            dataGrid1.ItemsSource = ((MainWindowVM)DataContext).Customers;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
                
        //    }
        //}

        //private void Search_Button_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        dataGrid1.ItemsSource = null;
        //        ((MainWindowVM)DataContext).Customers.Clear();
        //        //string query = "select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id where";
        //        //Dictionary<string, string> textFields = new Dictionary<string, string>();
        //        //textFields.Add("Name", textBoxName.Text);
        //        //textFields.Add("FirstName", textBoxFirstName.Text);
        //        //textFields.Add("DateOfBirth", datePicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
        //        //textFields.Add("Street", textBoxStreet.Text);
        //        //textFields.Add("CityId", ((City)comboBoxCity.SelectedItem).Id.ToString());
        //        ////string[] textFields = { textBoxName.Text, textBoxFirstName.Text, datePicker.SelectedDate.Value.ToString("yyyy-MM-dd"), textBoxStreet.Text, comboBoxCity.Text };
        //        //foreach (KeyValuePair<string, string> kvp in textFields)
        //        //{
        //        //    if (kvp.Value != string.Empty) { query += ("(" + kvp.Key + " = '" + kvp.Value + "');"); }
        //        //    break;
        //        //}
        //        //if (textBoxName.Text != string.Empty) { query += ("(Name = '" + textBoxName.Text + "');"); }
        //        //else if (textBoxFirstName.Text != string.Empty) { query += ("(FirstName = '" + textBoxFirstName.Text + "');"); }
        //        //else if (datePicker.SelectedDate != null) { query += ("(DateOfBirth = '" + datePicker.SelectedDate.Value.ToString("yyyy-MM-dd") + "');"); }
        //        //else if (textBoxStreet.Text != string.Empty) { query += ("(FirstName = '" + textBoxStreet.Text + "');"); }
        //        //else if (comboBoxCity.SelectedItem != null) { query += ("(CityId = '" + ((City)comboBoxCity.SelectedItem).Id.ToString() + "');"); }
        //        //else { MessageBox.Show("Enter at least one criteria"); return; }
        //        ////((MainWindowVM)DataContext).LoadDatabase("select customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id where (Name = '" + textBoxName.Text + "') and (FirstName = '" + textBoxFirstName.Text + "');");
        //        //((MainWindowVM)DataContext).LoadDatabase(query);
        //        dataGrid1.ItemsSource = ((MainWindowVM)DataContext).Customers;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
                
            
        //}

        private void OnInit(object sender, EventArgs e)
        {
            
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            //LoadDatabase();
        }

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = null;
            ((MainWindowVM)DataContext).Customers.Clear();
            ((MainWindowVM)DataContext).LoadDatabase();
            dataGrid1.ItemsSource = ((MainWindowVM)DataContext).Customers;
        }
    }

    

    

    


}
