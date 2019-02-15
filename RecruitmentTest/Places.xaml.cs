using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecruitmentTest
{
    /// <summary>
    /// Логика взаимодействия для Places.xaml
    /// </summary>
    public partial class Places : Window, IErrorChecker
    {
        public Places()
        {
            InitializeComponent();
        }
        public bool HasValidationErrors()
        {
            return Validation.GetHasError(textBoxStreet);
        }
    }
}
