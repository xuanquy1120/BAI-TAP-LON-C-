create database QLP3
go
use QLP3
go
create table Play
(
Name nvarchar(100) primary key,
wins int,
loses int
);
select * from Play