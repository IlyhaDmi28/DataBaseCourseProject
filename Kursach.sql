USE Kursach;

CREATE TABLE Users (
    ID INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(50) NOT NULL,
    Role SMALLINT NOT NULL,
    CONSTRAINT check_values CHECK (Role IN (1, 2)),
    PRIMARY KEY (ID)
);


CREATE TABLE Categories (
    CategoryID INT NOT NULL,
    CategoryName VARCHAR(50) NOT NULL,
    Type SMALLINT NOT NULL,
    Picture varchar(255) NOT NULL,
	CONSTRAINT check_type CHECK (Type IN (1, 2)),
    PRIMARY KEY (CategoryID)
);

CREATE TABLE Saves (
    SaveID INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    SaveDate DATE NOT NULL,
    CategoryID INT NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (SaveID),
	FOREIGN KEY(UserID) REFERENCES Users(ID),
	FOREIGN KEY(CategoryID) REFERENCES Categories(CategoryID),
);

CREATE TABLE Goals (
    GoalID INT NOT NULL,
    NameGoal VARCHAR(255) NOT NULL,
    Accumulated DECIMAL(18, 2) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Picture VARCHAR(255) NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (GoalID),
	FOREIGN KEY(UserID) REFERENCES Users(ID),
);

CREATE TABLE Debts (
    DebtID INT NOT NULL,
    NameDebt VARCHAR(255) NOT NULL,
    AmountPayment DECIMAL(18, 2) NOT NULL,
    NumberPayments DECIMAL(18, 2) NOT NULL,
    ReceivingyDate DECIMAL(18, 2) NOT NULL,
    MaturityDate DECIMAL(18, 2) NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (DebtID),
	FOREIGN KEY(UserID) REFERENCES Users(ID),
);