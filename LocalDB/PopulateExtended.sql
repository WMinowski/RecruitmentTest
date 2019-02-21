delete from mydb.cities
delete from mydb.customers
delete from mydb.places
delete from mydb.CustomersPlaces
DBCC CHECKIDENT ('mydb.places', RESEED, 0)
DBCC CHECKIDENT ('mydb.cities', RESEED, 0)
DBCC CHECKIDENT ('mydb.CustomersPlaces', RESEED, 0)
DBCC CHECKIDENT ('mydb.customers', RESEED, 0)
insert into mydb.cities values('Kharkov');
insert into mydb.cities values('Kiev');
insert into mydb.places(CityId, Street) values(1, 'street');
insert into mydb.places(CityId, Street) values(2, 'strt');
insert into mydb.customers values('Vasiliev', 'Denis', '1985-06-28') ;
insert into mydb.customers values('Kuznetsov', 'Sergey', '1983-08-31') ;
insert into mydb.customersplaces values(GETDATE(), 1, 1);
insert into mydb.customersplaces values(GETDATE(), 2, 2);