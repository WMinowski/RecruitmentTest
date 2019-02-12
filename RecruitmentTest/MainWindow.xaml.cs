using System.Windows;
using System.Windows.Controls;

//using Microsoft.VisualBasic;

namespace RecruitmentTest
{
    //public delegate bool ErrorChecker();

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IErrorChecker
    {
        public MainWindow()
        {
            InitializeComponent();
            //ErrorChecker ec;
            //ec = hasValidationErrors;
            DataContext = new MainWindowVM(this);
            
        }

        

        public bool HasValidationErrors()
        {
            return (Validation.GetHasError(textBoxName) || Validation.GetHasError(textBoxFirstName) || Validation.GetHasError(textBoxStreet));
        }
    }

    

    

    


}
