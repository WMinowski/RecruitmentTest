using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace RecruitmentTest
{
    public class CustomerVM : INotifyPropertyChanged//, IDataErrorInfo
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
        public string Street
        {
            get { return _customer.Street; }
            set
            {
                _customer.Street = value;
                OnPropertyChanged("Street");
            }
        }
        public string City
        {
            get { return _customer.City.Name; }
            private set
            {
                //_customer.City = MainWindowVM.Cities.ToList().Find(x => x.Name == value).ToCity();
            }
            //no set for non-modified City collection
        }

        //public string Error
        //{
        //    get { return null; }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        string result = null;
        //        //TODO: validation logic
        //        if(columnName == "Name")
        //        {
        //            if(Name == string.Empty)
        //            {
        //                result = "Empty field!";
        //            }
        //        }
        //        if (columnName == "First name")
        //        {
        //            if (FirstName == string.Empty)
        //            {
        //                result = "Empty field!";
        //            }
        //        }
        //        if (columnName == "Date of birth")
        //        {
        //            if (DateOfBirth == null)
        //            {
        //                result = "Empty field!";
        //            }
        //        }
        //        if (columnName == "Street")
        //        {
        //            if (Street == string.Empty)
        //            {
        //                result = "Empty field!";
        //            }
        //        }
        //        if (columnName == "City")
        //        {
        //            if (City == string.Empty)
        //            {
        //                result = "Empty field!";
        //            }
        //        }
        //        return result;
        //    }
        //}

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
            Street = _customer.Street;

        }

    }
}
