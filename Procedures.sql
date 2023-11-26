CREATE PROCEDURE AddSave
    @amount DECIMAL(18, 2),
    @saveDate DATE,
    @categoryID INT,
    @userID INT
AS
BEGIN
    INSERT INTO Saves (Amount, SaveDate, CategoryID, UserID) VALUES (@amount, @saveDate, @categoryID, @userID);
END;


CREATE PROCEDURE RemoveSave
    @saveID INT
AS
BEGIN
    DELETE FROM Saves WHERE SaveID = @saveID;
END;

--Категории--

CREATE PROCEDURE AddCategory
    @categoryName VARCHAR(50),
    @type SMALLINT,
    @picture VARCHAR(255)
AS
BEGIN
    INSERT INTO Categories (CategoryName, Type, Picture) VALUES (@categoryName, @type, @picture);
END;


CREATE PROCEDURE RemoveCategory
    @categoryID INT
AS
BEGIN
    DELETE FROM Categories WHERE CategoryID = @categoryID;
END;


CREATE PROCEDURE EditCategory
    @categoryID INT,
    @newCategoryName VARCHAR(50),
    @newType SMALLINT,
    @newPicture VARCHAR(255)
AS
BEGIN
    UPDATE Categories
    SET
        CategoryName = @newCategoryName,
        Type = @newType,
        Picture = @newPicture
    WHERE
        CategoryID = @categoryID;
END;


--Цели--

CREATE PROCEDURE AddGoal
    @nameGoal VARCHAR(255),
    @accumulated DECIMAL(18, 2),
    @price DECIMAL(18, 2),
    @picture VARCHAR(255),
    @userID INT
AS
BEGIN
    INSERT INTO Goals (NameGoal, Accumulated, Price, Picture, UserID)  VALUES (@nameGoal, @accumulated, @price, @picture, @userID);
END;


CREATE PROCEDURE RemoveGoal
    @goalID INT
AS
BEGIN
    DELETE FROM Goals WHERE GoalID = @goalID;
END;


CREATE PROCEDURE EditGoal
    @goalID INT,
    @newNameGoal VARCHAR(255),
    @newAccumulated DECIMAL(18, 2),
    @newPrice DECIMAL(18, 2),
    @newPicture VARCHAR(255)
AS
BEGIN
    UPDATE Goals
    SET
        NameGoal = @newNameGoal,
        Accumulated = @newAccumulated,
        Price = @newPrice,
        Picture = @newPicture
    WHERE
        GoalID = @goalID;
END;


--Долги--

CREATE PROCEDURE AddDebt
    @nameDebt VARCHAR(255),
    @amountPayment DECIMAL(18, 2),
    @numberPayments DECIMAL(18, 2),
    @receivingDate DECIMAL(18, 2),
    @maturityDate DECIMAL(18, 2),
    @userID INT
AS
BEGIN
    INSERT INTO Debts (NameDebt, AmountPayment, NumberPayments, ReceivingyDate, MaturityDate, UserID)
    VALUES (@nameDebt, @amountPayment, @numberPayments, @receivingDate, @maturityDate, @userID);
END;


CREATE PROCEDURE RemoveDebt
    @debtID INT
AS
BEGIN
    DELETE FROM Debts WHERE DebtID = @debtID;
END;


CREATE PROCEDURE EditDebt
    @debtID INT,
    @newNameDebt VARCHAR(255),
    @newAmountPayment DECIMAL(18, 2),
    @newNumberPayments DECIMAL(18, 2),
    @newReceivingDate DECIMAL(18, 2),
    @newMaturityDate DECIMAL(18, 2)
AS
BEGIN
    UPDATE Debts
    SET
        NameDebt = @newNameDebt,
        AmountPayment = @newAmountPayment,
        NumberPayments = @newNumberPayments,
        ReceivingyDate = @newReceivingDate,
        MaturityDate = @newMaturityDate
    WHERE
        DebtID = @debtID;
END;