using DomainStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTest
{
    public class PlaceVM : INotifyPropertyChanged
    {
        private Place _place;
        public Place Place { get { return _place; } }

        public int Id
        {
            get { return _place.Id; }
            set
            {
                _place.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string City
        {
            get { return _place.City.Name; }
            set
            {
                _place.City.Name = value;
                OnPropertyChanged("City");
            }
        }
        public string Street
        {
            get { return _place.Street; }
            set
            {
                _place.Street = value;
                OnPropertyChanged("Street");
            }
        }

        public override string ToString()
        {
            return _place.ToString();
        }

        public PlaceVM(int id, int cityId, string street)
        {
            Id = id;
            City = cityId.ToString();
            Street = street;
        }
        public PlaceVM(Place place)
        {
            _place = place;
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
