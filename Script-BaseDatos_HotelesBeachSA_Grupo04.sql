use [master]
go
if exists(select name from dbo.sysdatabases where name = 'HotelesBeachSA')
drop database [HotelesBeachSA]
go
create database [HotelesBeachSA]
go
use [HotelesBeachSA]
go
if exists(select name from dbo.sysobjects where name = 'Clientes')
drop table [Clientes]
go
create table [Clientes](
Cedula int not null primary key,
TipoCedula varchar(20) not null,
NombreCompleto varchar(100) not null,
Telefono int not null,
Direccion varchar(200) not null,
Email varchar(50) not null)
go
if exists(select name from dbo.sysobjects where name = 'Paquetes')
drop table [Paquetes]
go
create table [Paquetes](
PaqueteId int not null identity primary key,
Nombre varchar(50) not null,
PrimaPorcentaje decimal not null,
CostoPersona decimal not null,
PlazoMeses int not null)
go
if exists(select name from dbo.sysobjects where name = 'Reservas')
drop table [Reservas]
go
create table [Reservas](
ReservaId int not null identity primary key,
CedCliente int not null,
PaqueteId int not null,
CantidadNoches int not null,
MontoTotalColones decimal not null,
MontoTotalDolares decimal not null,
MontoPrima decimal not null,
Descuento decimal not null,
FechaReserva datetime not null default getdate(),
MetodoPago varchar(50) not null,
7
NumeroCheque varchar(20) not null,
Banco varchar(50) not null,
Activa char not null default 'A')
go
if exists(select name from dbo.sysobjects where name = 'Rols')
drop table [Rols]
go
create table [Rols](
RolId int not null identity primary key,
Nombre varchar(50) not null,
Descripcion varchar(100) not null)
go
if exists(select name from dbo.sysobjects where name = 'Usuarios')
drop table [Usuarios]
go
create table [Usuarios](
Email varchar(50) not null primary key,
NombreCompleto varchar(100) not null,
Password varchar(100) not null,
FechaRegistro datetime not null default getdate(),
RolId int not null)
go
alter table [Reservas] add
constraint [FK_Reservas_Clientes] foreign key (CedCliente) references [Clientes](Cedula)
go
alter table [Reservas] add
constraint [FK_Reservas_Paquetes] foreign key (PaqueteId) references
[Paquetes](PaqueteId)
go
alter table [Usuarios] add
constraint [FK_Usuarios_Rols] foreign key (RolId) references [Rols](RolId)
go
insert into Paquetes (Nombre, PrimaPorcentaje, CostoPersona, PlazoMeses)
values
('Todo Incluido', 45, 450, 24),
('Alimentacion', 15, 275, 12),
('Hospedaje', 15, 210, 12);
go
insert into Rols (Nombre, Descripcion)
values
('Admin', 'Usuario Administrador'),
('Usuario', 'Usuario Consumidor'),
('Empleado', 'Usuario Empleado');
go
