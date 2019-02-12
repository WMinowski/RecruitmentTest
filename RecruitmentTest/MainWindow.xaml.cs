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
    public delegate bool ErrorChecker();

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ErrorChecker ec;
            //ec = hasValidationErrors;
            DataContext = new MainWindowVM(new ErrorChecker(hasValidationErrors));
            ((MainWindowVM)DataContext).ClearText();
        }

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            //dataGrid1.ItemsSource = null;
            //((MainWindowVM)DataContext).Customers.Clear();
            //((MainWindowVM)DataContext).Cities.Clear();
            ((MainWindowVM)DataContext).LoadDatabase();
            //dataGrid1.ItemsSource = ((MainWindowVM)DataContext).Customers;
        }

        

        private bool hasValidationErrors()
        {
            return (Validation.GetHasError(textBoxName) || Validation.GetHasError(textBoxFirstName) || Validation.GetHasError(textBoxStreet));
        }
    }

    

    

    


}
