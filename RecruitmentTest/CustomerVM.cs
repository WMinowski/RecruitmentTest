using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DomainStandard;

namespace RecruitmentTest
{
    public class CustomerVM : INotifyPropertyChanged
    {
        
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
        }
        
        public int Id
        {
            get { return _customer.Id; }
            set
            {
                _customer.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _customer.Name; }
            set
            {
                _customer.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string FirstName
        {
            get { return _customer.FirstName; }
            set
            {
                _customer.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public DateTime DateOfBirth
        {
            get { return _customer.DateOfBirth; }
            set
            {
                _customer.DateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        public Place Place
        {
            get { return _customer.Place; }
            set
            {
                _customer.Place = value;
                OnPropertyChanged("Place");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public CustomerVM() { }

        public CustomerVM(Customer c)
        {
            _customer = c;
            Id = _customer.Id;
            Name = _customer.Name;
            FirstName = _customer.FirstName;
            DateOfBirth = _customer.DateOfBirth;
            

        }

    }
}
