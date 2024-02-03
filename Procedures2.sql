DROP PROCEDURE CalculateIncomeByCategory;
DROP PROCEDURE CalculateExpenseByCategory;
DROP PROCEDURE CalculateTotalDebt;
DROP PROCEDURE CalculateTotalDebts;
DROP PROCEDURE CalculatePaidDebt;
DROP PROCEDURE CalculatePaidDebts;
DROP PROCEDURE CalculateIncome;
DROP PROCEDURE CalculateExpense;
DROP PROCEDURE CalculateBudget;
DROP PROCEDURE CalculateIncomeOfCategory;
DROP FUNCTION GetUserByID;
DROP FUNCTION GetUsers;
DROP FUNCTION GetUserByLogin;
DROP FUNCTION GetSavesByCategoryType;
DROP FUNCTION GetCategoriesByType;
DROP FUNCTION GetAllGoals;
DROP FUNCTION GetAllDebts;
DROP PROCEDURE AddUser;
DROP PROCEDURE AlterUser;
DROP PROCEDURE EditUser;
DROP PROCEDURE DeleteUser;
DROP PROCEDURE AddSaves;
DROP PROCEDURE DeleteSave;
DROP PROCEDURE AddCategory;
DROP PROCEDURE AlterCategory;
DROP PROCEDURE DeleteCategory;
DROP PROCEDURE AddGoal;
DROP PROCEDURE AlterGoal;
DROP PROCEDURE DeleteGoal;
DROP PROCEDURE AddDebt;
DROP PROCEDURE AlterDebt;
DROP PROCEDURE DeleteDebt;

-------------- ���������� ������������ --------------

CREATE OR REPLACE PROCEDURE GetUsers(
 userCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM Ilya.Users;

    DBMS_OUTPUT.PUT_LINE('Users get successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error of find user: ' || SQLERRM);
END;


CREATE OR REPLACE FUNCTION GetUserByID(
    userID IN INT
)
RETURN SYS_REFCURSOR
IS
    userCursor SYS_REFCURSOR;
BEGIN
    OPEN userCursor FOR
        SELECT * FROM Ilya.Users WHERE ID = userID;
    
    DBMS_OUTPUT.PUT_LINE('User get by id successfully.');
    RETURN userCursor;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get by id user: user with ID ' || userID || ' not found.');
        CLOSE userCursor;
        RETURN NULL;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by id: ' || SQLERRM);
        CLOSE userCursor;
        RAISE;
END;




CREATE OR REPLACE PROCEDURE GetUserByLogin(
    v_login IN NVARCHAR2,
    userID OUT INT,
    name OUT NVARCHAR2,
    role OUT SMALLINT
)
IS
BEGIN
    SELECT UserID, Name, Role 
    INTO userID, name, role
    FROM Ilya.UsersTable WHERE Ilya.UsersTable.Login = v_login;
        
    DBMS_OUTPUT.PUT_LINE('User get by login successfully.');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by login: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE FUNCTION GetSavesByCategoryType(
    startDate IN DATE,
    endDate IN DATE,
    type IN SMALLINT
)
RETURN SYS_REFCURSOR
IS
    savesCursor SYS_REFCURSOR;
BEGIN
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of get saves: start date ' || startDate || ' > end date ' || endDate);
    ELSIF type < 0 AND type > 1 THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: invalid category type');
    ELSE 
        OPEN savesCursor FOR
            SELECT S.*, C.CategoryName
            FROM Ilya.Saves S
            JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
            WHERE C.Type = type AND SaveDate >= startDate AND SaveDate <= endDate;
            
            DBMS_OUTPUT.PUT_LINE('Saves get successfully.');
            RETURN savesCursor;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: saves with period from ' || startDate || ' to ' || endDate || ' not found.');
        CLOSE savesCursor;
        RETURN NULL;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: ' || SQLERRM);
        CLOSE savesCursor;
        RAISE;
END;


CREATE OR REPLACE FUNCTION GetCategoriesByType(
    type IN SMALLINT
)
RETURN SYS_REFCURSOR
IS
    categoriesCursor SYS_REFCURSOR;
BEGIN
    IF type < 0 AND type > 1 THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: invalid category type');
    ELSE
        OPEN categoriesCursor FOR
            SELECT *
            FROM Ilya.Categories
            WHERE Type = type;
            
            DBMS_OUTPUT.PUT_LINE('Categories get successfully.');
            RETURN categoriesCursor;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: categories not found.');
        CLOSE categoriesCursor;
        RETURN NULL;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: ' || SQLERRM);
        CLOSE categoriesCursor;
        RAISE;
END;


CREATE OR REPLACE FUNCTION GetAllGoals
RETURN SYS_REFCURSOR
IS
    goalsCursor SYS_REFCURSOR;
BEGIN
    OPEN goalsCursor FOR
        SELECT *
        FROM Ilya.Goals;
    
    DBMS_OUTPUT.PUT_LINE('Goals get successfully.');
    RETURN goalsCursor;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get goals: goals not found.');
        CLOSE userCursor;
        RETURN NULL;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get goals: ' || SQLERRM);
        CLOSE userCursor;
        RAISE;
END;


CREATE OR REPLACE FUNCTION GetAllDebts
RETURN SYS_REFCURSOR
IS
    debtsCursor SYS_REFCURSOR;
BEGIN
    OPEN debtsCursor FOR
        SELECT *
        FROM Ilya.Debts;
    
    DBMS_OUTPUT.PUT_LINE('Debts get successfully.');
    RETURN debtsCursor;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: debts not found.');
        CLOSE debtsCursor;
        RETURN NULL;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: ' || SQLERRM);
        CLOSE debtsCursor;
        RAISE;
END;



    CREATE OR REPLACE PROCEDURE AddUser(
       login IN NVARCHAR2,
       password IN NVARCHAR2,
       name IN NVARCHAR2,
       role IN SMALLINT
    )
    IS
    BEGIN
        IF role < 0 OR role > 1 THEN 
             DBMS_OUTPUT.PUT_LINE('Error inserting user: invalid role.');
             ROLLBACK;
        ELSE 
            INSERT INTO Ilya.UsersTable (Login, Password, Name, Role)
            VALUES (login, password, name, role);
       
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('User inserted successfully.');
        END IF;
    EXCEPTION
       WHEN DUP_VAL_ON_INDEX THEN
          ROLLBACK;
          DBMS_OUTPUT.PUT_LINE('Error inserting user: user with the same login already exists.');
       WHEN OTHERS THEN
          ROLLBACK;
          DBMS_OUTPUT.PUT_LINE('Error inserting user: ' || SQLERRM);
          RAISE;
    END;



CREATE OR REPLACE PROCEDURE AlterUser(
   id IN INT,
   name IN NVARCHAR2,
   login IN NVARCHAR2,
   password IN NVARCHAR2,
   role IN SMALLINT
)
IS
BEGIN
     IF role < 0 OR role > 1 THEN 
         DBMS_OUTPUT.PUT_LINE('Error inserting user: invalid role.');
         ROLLBACK;
    ELSE
        UPDATE Ilya.Users
        SET Name = name,
            Login = login,
            Password = password,
            Role = role
        WHERE ID = id;
        
        IF SQL%ROWCOUNT = 0 THEN
            RAISE NO_DATA_FOUND;
        END IF;
        
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('User altered successfully.');
   END IF;
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering user: user with ID ' || id || ' not found.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE EditUser(
    id IN INT,
    name IN NVARCHAR2
)
IS
BEGIN
    UPDATE Ilya.Users
    SET Name = name
    WHERE ID = id;
   
    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
   
    DBMS_OUTPUT.PUT_LINE('User edit successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error edit user: user with ID ' || id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error edit user: ' || SQLERRM);
END;

-------------- �������� ������������ --------------

CREATE OR REPLACE PROCEDURE DeleteUser(
    id IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Users
    WHERE ID = id;
   
    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;

    COMMIT;

    DBMS_OUTPUT.PUT_LINE('User deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: user with ID ' || id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: ' || SQLERRM);
END;
---------------------------------------------------------------------------------------------

CREATE OR REPLACE PROCEDURE AddSaves(
    amount NUMBER,
    saveDate IN DATE,
    categoryID IN INT,
    userID IN INT
)
AS
BEGIN
    INSERT INTO Ilya.Saves (Amount, SaveDate, CategoryID, UserID)
    VALUES (amount, saveDate, categoryID, userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Save inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the save: ' || SQLERRM);
END;

-------------- �������� ���������� --------------

CREATE OR REPLACE PROCEDURE DeleteSave(
    saveId IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Saves
    WHERE SaveId = saveId;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Save deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
         ROLLBACK;
            DBMS_OUTPUT.PUT_LINE('Error deleting save: save with ID ' || saveId || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting save: ' || SQLERRM);
        RAISE;
END;


--���������--


CREATE OR REPLACE PROCEDURE AddCategory(
    categoryName NVARCHAR2,
    type IN SMALLINT,
    picture IN NVARCHAR2
)
AS
BEGIN
    IF type < 0 OR type > 1 THEN 
         DBMS_OUTPUT.PUT_LINE('Error inserting category: invalid type.');
         ROLLBACK;
    ELSE
        INSERT INTO Ilya.Categories (CategoryName, Type, Picture)
        VALUES (categoryName, type, picture);
    
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('Category inserted successfully.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the category: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE AlterCategory(
    categoryID IN INT,
    categoryName IN NVARCHAR2,
    type IN SMALLINT,
    picture IN NVARCHAR2
)
IS
BEGIN
    IF type < 0 OR type > 1 THEN 
        DBMS_OUTPUT.PUT_LINE('Error altering category: invalid type.');
        ROLLBACK;
    ELSE
        UPDATE Ilya.Categories
        SET CategoryName = categoryName,
            Type = type,
            Picture = picture
        WHERE CategoryID = categoryID;
   
        IF SQL%ROWCOUNT = 0 THEN
            DBMS_OUTPUT.PUT_LINE('Error altering category: category with ID ' || categoryID || ' not found.');
            ROLLBACK;
        ELSE
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('Category altered successfully.');
        END IF;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting category: category with ID ' || categoryID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering category: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE DeleteCategory(
   categoryID IN INT
)
IS
BEGIN
   DELETE FROM Ilya.Categories
   WHERE CategoryID = categoryID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Category deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting category: category with ID ' || categoryID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting category: ' || SQLERRM);
        RAISE;
END;




--����--


CREATE OR REPLACE PROCEDURE AddGoal(
    nameGoal NVARCHAR2,
    accumulated IN NUMBER,
    price IN NUMBER,
    picture IN NVARCHAR2,
    userID INT
)
AS
BEGIN
    INSERT INTO Ilya.Goals(NameGoal, Accumulated, Price, Picture, UserID)
    VALUES (nameGoal, accumulated, price, picture, userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Goal inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the goal: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE AlterGoal(
    goalID INT,
    nameGoal NVARCHAR2,
    accumulated NUMBER,
    price NUMBER,
    picture NVARCHAR2
)
IS
BEGIN
    UPDATE Ilya.Goals
    SET NameGoal = nameGoal,
        Accumulated = accumulated,
        Price = price,
        Picture = picture
    WHERE GoalID = goalID;
   
    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Goal altered successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering goal: goal with ID ' || goalID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering goal: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE DeleteGoal(
   goalID IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Goals
    WHERE GoalID = goalID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
   COMMIT;
   DBMS_OUTPUT.PUT_LINE('Goal deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting goal: goal with ID ' || goalID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting goal: ' || SQLERRM);
        RAISE;
END;


--�����--


CREATE OR REPLACE PROCEDURE AddDebt(
    p_nameDebt NVARCHAR2,
    p_amountPayment NUMBER,
    p_numberPayments NUMBER,
    p_percent NUMBER,
    p_receivingDate DATE,
    p_maturityDate DATE,
    p_userID INT
)
AS
BEGIN
    INSERT INTO Ilya.Debts(NameDebt, AmountPayment, NumberPayments, Percent, ReceivingDate, MaturityDate, UserID)
    VALUES (p_nameDebt, p_amountPayment, p_numberPayments, p_percent, p_receivingDate, p_maturityDate, p_userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Debt inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the Debt: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE AlterDebt(
    debtID INT,
    nameDebt NVARCHAR2,
    amountPayment NUMBER,
    numberPayments NUMBER,
    percent NUMBER,
    receivingDate DATE,
    maturityDate DATE
)
IS
BEGIN
    UPDATE Ilya.Debts
    SET NameDebt = nameDebt,
        AmountPayment = amountPayment,
        NumberPayments = numberPayments,
        Percent = percent,
        ReceivingDate = receivingDate,
        MaturityDate = maturityDate
    WHERE DebtID = debtID;
   
   IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Debt altered successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altered debt: debt with ID ' || debtID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering debt: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE DeleteDebt(
   debtID IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Debts
    WHERE DebtID = debtID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Debt deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting debt: debt with ID ' || debtID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Debt deleting debt: ' || SQLERRM);
        RAISE;
END;




CREATE OR REPLACE FUNCTION CalculateIncome(
    startDate IN DATE,
    endDate IN DATE,
    userID IN INT
)
RETURN NUMBER
IS
    sumIncome NUMBER;
BEGIN 
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of calculate incomes: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumIncome
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Type = 0 AND UserID = userID;
        RETURN sumIncome;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate incomes: ' || SQLERRM);
        RAISE; 
END;



CREATE OR REPLACE FUNCTION CalculateExpense(
    startDate IN DATE,
    endDate IN DATE,
    userID IN INT
)
RETURN NUMBER
IS
    sumExpense NUMBER;
BEGIN 
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of calculate expenses: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumExpense
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Type = 1 AND UserID = userID;
        RETURN sumExpense;
    END IF;
EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate expenses: ' || SQLERRM);
        RAISE; 
END;




CREATE OR REPLACE FUNCTION CalculateBudget(
    startDate IN DATE,
    endDate IN DATE,
    userID IN INT
)
RETURN NUMBER
IS
    sumIncome NUMBER;
    sumExpense NUMBER;
BEGIN 
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of calculate budget: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumIncome
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Type = 0 AND UserID = userID;
        
        SELECT SUM(Amount)
        INTO sumExpense
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Type = 1 AND UserID = userID;

        RETURN sumIncome - sumExpense;        
    END IF;
EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate budget: ' || SQLERRM);
        RAISE; 
END;
  


CREATE OR REPLACE PROCEDURE CalculateIncomeByCategory(
    startDate IN DATE,
    endDate IN DATE,
    categoryID IN INT,
    userID IN INT,
    sumIncomeOfCategory OUT NUMBER,
    partIncome OUT NUMBER
)
IS
    sumIncome NUMBER;
BEGIN 
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of calculate incomes by category: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumIncomeOfCategory
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Ilya.Saves.categoryID = categoryName AND Type = 1 AND UserID = userID;
        
        partIncome := sumIncomeOfCategory / CalculateIncome(startDate, endDate, userID) * 100;    
    END IF;
EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate incomes by category: ' || SQLERRM);
        RAISE; 
END;


CREATE OR REPLACE PROCEDURE CalculateExpenseByCategory(
    startDate IN DATE,
    endDate IN DATE,
    categoryID IN INT,
    userID IN INT,
    sumExpenseOfCategory OUT NUMBER,
    partExpense OUT NUMBER
)
IS
    sumExpense NUMBER;
BEGIN 
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of calculate expenses by category: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumExpenseOfCategory
        FROM Ilya.Saves JOIN Ilya.Categories ON Ilya.Saves.CategoryID = Ilya.Categories.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND Ilya.Saves.categoryID = categoryName AND Type = 0 AND UserID = userID;
        
        partExpense := sumExpenseOfCategory / CalculateExpense(startDate, endDate, userID) * 100;    
    END IF;
EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate expenses by category: ' || SQLERRM);
        RAISE; 
END;




CREATE OR REPLACE FUNCTION CalculateTotalDebt(
    debtID IN NUMBER
)
RETURN NUMBER
IS
    totalDebt NUMBER := 0;
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentNumberPayments NUMBER := 0;
BEGIN
    SELECT AmountPayment, Percent, NumberPayments INTO currentPayment, currentPercent, currentNumberPayments
    FROM Ilya.Debts
    WHERE DebtID = debtID;
    
    IF currentNumberPayments = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR i IN 1..currentNumberPayments LOOP
        totalDebt := totalDebt + currentPayment;
        currentPayment := currentPayment + (currentPayment * currentPercent);
    END LOOP;

    RETURN totalDebt;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate total debt: debt with ID ' || debtID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate total debt: ' || SQLERRM);
        RAISE;
END;



CREATE OR REPLACE FUNCTION CalculateTotalDebts(
    userID IN INT
)
RETURN NUMBER
IS
    totaDebt NUMBER := 0;
    userCount INT := 0;
BEGIN
    SELECT COUNT(*) INTO userCount FROM Ilya.Users WHERE ID = userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM Ilya.Debts WHERE UserID = userID) 
    LOOP
        totaDebt := totaDebt + CalculateTotalDebt(debt.DebtID);
    END LOOP;

    RETURN totaDebt;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate debts: user with ID' || userID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate debts: ' || SQLERRM);
        RAISE;
END;



CREATE OR REPLACE FUNCTION CalculatePaidDebt(
    debtID IN NUMBER
)
RETURN NUMBER
IS
    totalDebt NUMBER := 0;
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentPaid INT := 0;
    debtCount INT := 0;
BEGIN
    SELECT COUNT(*) INTO debtCount FROM Ilya.Debts WHERE DebtID = debtID;
    
    IF debtCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    SELECT AmountPayment, Percent, Paid INTO currentPayment, currentPercent, currentPaid
    FROM Ilya.Debts
    WHERE DebtID = debtID;
    
    FOR i IN 1..currentPaid LOOP
        totalDebt := totalDebt + currentPayment;
        currentPayment := currentPayment + (currentPayment * currentPercent);
    END LOOP;

    RETURN totalDebt;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debt: debt with ID ' || debtID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debt: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE FUNCTION CalculatePaidDebts(
    userID IN INT
)
RETURN NUMBER
IS
    totaDebt NUMBER := 0;
    userCount INT := 0;
BEGIN
    SELECT COUNT(*) INTO userCount FROM Ilya.Users WHERE ID = userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM Ilya.Debts WHERE UserID = userID) 
    LOOP
        totaDebt := totaDebt + CalculatePaidDebt(debt.DebtID);
    END LOOP;

    RETURN totaDebt;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debts: user ' || userID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debts: ' || SQLERRM);
        RAISE;
END;
