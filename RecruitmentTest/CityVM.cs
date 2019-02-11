using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTest
{
    public class CityVM : INotifyPropertyChanged
    {
        private City _city;
        public City City
        {
            get
            {
                return _city;
            }
            //set { _city = value; }
        }
        public int Id
        {
            get { return _city.Id; }
            set
            {
                _city.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _city.Name; }
            set
            {
                _city.Name = value;
                OnPropertyChanged("Name");
            }
        }

        
        public CityVM(int id, string name)
        {
            _city = new City(id, name);
            Id = id;
            Name = name;
        }
        public CityVM(Domain.City city)
        {
            _city = city;
            Id = city.Id;
            Name = city.Name;
        }

        public Domain.City ToCity()
        {
            return new Domain.City(Id, Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
