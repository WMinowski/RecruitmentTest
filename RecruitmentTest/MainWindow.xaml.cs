using System.Windows;
using System.Windows.Controls;

namespace RecruitmentTest
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IErrorChecker
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM(this);
            
        }
        public bool HasValidationErrors()
        {
            return (Validation.GetHasError(textBoxName) || Validation.GetHasError(textBoxFirstName));
        }
    }

    

    

    


}
