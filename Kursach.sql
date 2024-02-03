CREATE PLUGGABLE DATABASE FinancialAssistant
ADMIN USER ADMIN IDENTIFIED BY qwepoi
FILE_NAME_CONVERT=('/opt/oracle/oradata/FREE/', '/opt/oracle/oradata/FinancialAssistant/')
PATH_PREFIX = '/opt/oracle/oradata/';

ALTER DATABASE OPEN;

SELECT name FROM v$services;
ALTER SESSION SET CONTAINER = FinancialAssistant;
ALTER SESSION SET CONTAINER = CDB$ROOT;

DROP TABLESPACE FA_TS INCLUDING CONTENTS AND DATAFILES;
CREATE TABLESPACE FA_TS
    DATAFILE 'FA_TS.dbf'
    SIZE 100M
    AUTOEXTEND ON
    NEXT 10M
    MAXSIZE UNLIMITED;
alter tablespace FA_TS online;

drop table Saves;
drop table Categories;
drop table Goals;
drop table Debts;
drop table Users;


select * from Users;
CREATE TABLE Users (
    ID INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    Login NVARCHAR2(255) NOT NULL UNIQUE,
    Password NVARCHAR2(50) NOT NULL,
    Name NVARCHAR2(50) NOT NULL,
    Picture BLOB,
    Role SMALLINT DEFAULT 0 NOT NULL CHECK (Role IN (0, 1)),
    PRIMARY KEY (ID)
) TABLESPACE FA_TS;

SELECT * FROM Categories;
CREATE TABLE Categories (
    CategoryID INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    CategoryName NVARCHAR2(50) NOT NULL,
    Type SMALLINT DEFAULT 0 NOT NULL CHECK (Type IN (0, 1)),
    Picture BLOB,
    PRIMARY KEY (CategoryID)
) TABLESPACE FA_TS;

SELECT COUNT(*) FROM Saves;
SELECT * FROM Saves;
SELECT * FROM Saves WHERE UserID = 7;
CREATE TABLE Saves (
    SaveID INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    Amount NUMBER(18, 2) DEFAULT 0 NOT NULL CHECK (Amount >= 0),
    SaveDate DATE NOT NULL,
    CategoryID INT NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (SaveID),
	FOREIGN KEY(UserID) REFERENCES Users(ID),
	FOREIGN KEY(CategoryID) REFERENCES Categories(CategoryID)
) TABLESPACE FA_TS;

SELECT * FROM Goals;
CREATE TABLE Goals (
    GoalID INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    NameGoal NVARCHAR2(255) NOT NULL,
    Accumulated NUMBER(18, 2) DEFAULT 0 NOT NULL CHECK (Accumulated >= 0),
    Price NUMBER(18, 2) DEFAULT 0 NOT NULL CHECK (Price >= 0),
    Picture BLOB,
    UserID INT NOT NULL,
    PRIMARY KEY (GoalID),
	FOREIGN KEY(UserID) REFERENCES Users(ID)
) TABLESPACE FA_TS;

select * from Debts;
CREATE TABLE Debts (
    DebtID INT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    NameDebt NVARCHAR2(255) NOT NULL,
    Paid INT DEFAULT 0 NOT NULL CHECK (Paid >= 0),
    AmountPayment NUMBER(18, 2) DEFAULT 0 NOT NULL CHECK (AmountPayment >= 0),
    NumberPayments INT DEFAULT 1 NOT NULL CHECK (NumberPayments > 0),
    Percent NUMBER DEFAULT 0 NOT NULL CHECK (Percent >= 0 AND Percent <= 100),
    ReceivingDate DATE NOT NULL,
    MaturityDate DATE NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (DebtID),
	FOREIGN KEY(UserID) REFERENCES Users(ID)
) TABLESPACE FA_TS;