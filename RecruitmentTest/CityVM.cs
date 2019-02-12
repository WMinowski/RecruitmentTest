﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RecruitmentTest
{
    public class CityVM : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        
        public CityVM(int id, string name)
        {
            //_city = new City(id, name);
            Id = id;
            Name = name;
        }
        public CityVM(Domain.City city)
        {
            //_city = city;
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
