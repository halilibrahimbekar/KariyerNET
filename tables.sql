create table if not exists Company (
	Id uuid primary key,
	CreatedDateTime Date,
	PhoneNumber varchar(100) unique not null,
    CompanyName varchar(100) not null,
    Address varchar(4000) not null,
    RemainingAdvertCount int	
);
create table if not exists Job (
	Id uuid primary key,
	CreatedDateTime Date,
	   Position varchar(100) not null,
         Description varchar(4000) not null,
         Salary decimal(18,2),
		 Additional varchar(1000),
         JobType int,
         EndDate Date,
         Score int  ,
         CompanyId uuid,
         FOREIGN KEY (CompanyId)
      REFERENCES Company (Id)
);
create table if not exists Setting (
	Id int primary key,
	Name varchar(50) not null,
	Value varchar(1000) not null
);
insert into setting values (1, 'DefaultAddvertCount', '2'),(2, 'RetrictedWords', 'tembel,ucuz'),(3, 'DefaultAdvertDay', '15');
insert into Company values ('3c747f97-7549-403d-8ad2-966425e09456', now(), '12345678', 'Büyük Şirket', 'Adres Çok uzak', 2),('996aabf0-7b71-4867-80d0-da404002699c', now(), '123456789', 'Küçük Şirket', 'Adres Çok Daha Uzak', 1);