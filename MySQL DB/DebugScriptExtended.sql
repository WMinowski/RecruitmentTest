#insert into mydb.cities values(0, 'Kharkov');
#insert into mydb.cities values(0, 'Kiev');
#insert into mydb.places values(0, 1, 'street');
#insert into mydb.places values(0, 2, 'strt');
#insert into mydb.customers values(0,'Vasiliev', 'Denis', '1985-06-28', 1) ;
#insert into mydb.customers values(0,'Kuznetsov', 'Sergey', '1983-08-31', 2) ;
#select * from mydb.customers where (CityId = 2)
#select * from mydb.cities
#delete from customers where (Name = 'Ivanov')and (FirstName = 'Ivan');
#update customers set Name = '', FirstName = '', DateOfBirth = '', Street = '', CityId = 2 where (Name = '') and (FirstName = '')
#UPDATE mydb.customers SET Id = 1, Name = 'Ivan',FirstName = 'Ivanov',DateOfBirth = '1986-12-12' ,Street = 'street',CityId = 2 WHERE (Id = '4');
#select * from mydb.customers
#select mydb.customers.Name, mydb.customers.FirstName, mydb.customers.DateOfBirth, mydb.customers.Street, mydb.cities.City from mydb.customers left join mydb.cities on mydb.customers.CityId=mydb.cities.Id where (CityId = '2') and (FirstName = 'Ivan');
#select customers.Id, customers.Name, customers.FirstName, customers.DateOfBirth, customers.Street, cities.City from customers left join cities on customers.CityId=cities.Id
#select mydb.customers.Name, mydb.customers.FirstName, mydb.customers.DateOfBirth, mydb.customers.Street, mydb.cities.City from mydb.customers left join mydb.cities on mydb.customers.CityId=mydb.cities.Id where(Name = 'Kuznetsov')and(FirstName = 'Sergey')and(DateOfBirth = '1973-06-08')and(Street = 'street')and(CityId = '1')
#DELETE FROM mydb.customers WHERE (Id = '3');
#insert into mydb.customersplaces values(0, now(), 2, 1);
#select mydb.customersplaces.UpdateTime from mydb.customersplaces where CustomerId = 2 order by mydb.customersplaces.UpdateTime desc limit 1
#select mydb.customers.Id, mydb.customers.Name, mydb.customers.FirstName, mydb.customers.DateOfBirth, mydb.customersplaces.PlaceId from mydb.customers left join mydb.customersplaces on mydb.customers.Id = mydb.customersplaces.CustomerId
#select * from mydb.customersplaces where mydb.customersplaces.UpdateTime > now()-15
#delete from mydb.customersplaces 
select * from mydb.customersplaces
