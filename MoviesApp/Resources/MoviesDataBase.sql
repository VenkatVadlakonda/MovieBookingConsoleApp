CREATE DATABASE MoviesDB;

USE MoviesDB;

CREATE TABLE Users(
UserID INT PRIMARY KEY IDENTITY(100,1),
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
Username NVARCHAR(50) NOT NULL,
Password NVARCHAR(100) NOT NULL,
DateOfBirth DATE NOT NULL,
Email NVARCHAR(30) NOT NULL,
Phone NVARCHAR(12),
Role NVARCHAR(20) NOT NULL DEFAULT 'User' CHECK (Role IN('Admin','User'))
);

CREATE TABLE Movies(
MovieID INT PRIMARY KEY IDENTITY(1,1),
MovieName NVARCHAR(100) NOT NULL,
Genre NVARCHAR(50) NOT NULL,
Duration NVARCHAR(20) NOT NULL,
AgeRestriction INT NOT NULL,
ShowTime NVARCHAR(60) NOT NULL,
NumberOfTickets INT NOT NULL DEFAULT 100,
Price DECIMAL(10,2) NOT NULL
);


CREATE TABLE Bookings(
BookingID INT PRIMARY KEY IDENTITY(1000,1),
UserID INT FOREIGN KEY REFERENCES Users(UserID),
ShowTime NVARCHAR(60),
BookingTime DATETIME DEFAULT GETDATE(),
Totalprice DECIMAL(10,2)
);

INSERT INTO Users (FirstName,LastName,UserName,Password,DateOfBirth,Email,Phone,Role)
VALUES ('Venkat','Vadlakonda','Venkat','admin123','2002-07-12','venkat@gmail.com','9390443840','Admin'),
('Ravi','Teja','RaviTeja','admin1234','2002-07-13','admin@gmail.com','868378736','Admin')



