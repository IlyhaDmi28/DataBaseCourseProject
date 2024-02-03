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
DROP PROCEDURE GetUserByID;
DROP PROCEDURE GetUsers;
DROP PROCEDURE GetUserByLogin;
DROP PROCEDURE GetSavesByCategoryType;
DROP PROCEDURE GetCategoriesByType;
DROP PROCEDURE GetAllGoals;
DROP PROCEDURE GetAllDebts;
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

-------------- пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ --------------

CREATE OR REPLACE PROCEDURE GetAlllUsers(
    userCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM Ilya.Users;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error of find users: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE GetUserByID(
    p_UserID IN INT,
    userCursor OUT SYS_REFCURSOR
) 
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM Ilya.Users WHERE ID = p_UserID;
    
    DBMS_OUTPUT.PUT_LINE('User get by id successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get by id user: user with ID ' || p_UserID || ' not found.');
        CLOSE userCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by id: ' || SQLERRM);
        CLOSE userCursor;
        RAISE;
END;

DECLARE
    user_cursor SYS_REFCURSOR;
    v_user Ilya.Users%ROWTYPE;
BEGIN
    GetUserByLogin(',', user_cursor);
    
    -- Цикл FETCH для вывода данных из курсора
    LOOP
        FETCH user_cursor INTO v_user;
        EXIT WHEN user_cursor%NOTFOUND;
        
        -- Здесь вы можете использовать данные из v_user
        DBMS_OUTPUT.PUT_LINE('User ID: ' || v_user.ID || ', Login: ' || v_user.Login || ', Name: ' || v_user.Name);
    END LOOP;

    CLOSE user_cursor;
END;

CREATE OR REPLACE PROCEDURE GetUserByLogin(
    p_login IN NVARCHAR2,
    userCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM Ilya.Users WHERE Login = p_login;
    
    IF userCursor%NOTFOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by login: user with Login ' || p_login || ' not found.');
        RETURN;
    END IF;
    
    DBMS_OUTPUT.PUT_LINE('User get by login successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by login: user with Login ' || p_login || ' not found.');
        CLOSE userCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get user by login: ' || SQLERRM);
        CLOSE userCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetSavesByCategoryType(
    startDate IN DATE,
    endDate IN DATE,
    p_type IN SMALLINT,
    p_userID IN INT,
    savesCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of get saves: start date ' || startDate || ' > end date ' || endDate);
    ELSIF p_type < 0 AND p_type > 1 THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: invalid category type');
    ELSE 
        OPEN savesCursor FOR
            SELECT S.*, C.CategoryName
            FROM Ilya.Saves S
            JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
            WHERE C.Type = p_type AND SaveDate >= startDate AND SaveDate <= endDate AND S.UserID = p_userID;
           
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: saves with period from ' || startDate || ' to ' || endDate || ' not found.');
        CLOSE savesCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: ' || SQLERRM);
        CLOSE savesCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetAllSaves(
    startDate IN DATE,
    endDate IN DATE,
    p_userID IN INT,
    savesCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN savesCursor FOR
        SELECT S.*, C.CategoryName
        FROM Ilya.Saves S
        JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND S.UserID = p_userID;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: saves with period from ' || startDate || ' to ' || endDate || ' not found.');
        CLOSE savesCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get saves: ' || SQLERRM);
        CLOSE savesCursor;
        RAISE;
END;



CREATE OR REPLACE PROCEDURE GetCategoriesByType(
    p_type IN SMALLINT,
    categoriesCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    IF p_type < 0 AND p_type > 1 THEN
        raise_application_error(-20003, 'Error of get categories: invalid category type');
    ELSE
        OPEN categoriesCursor FOR
            SELECT *
            FROM Ilya.Categories
            WHERE Type = p_type;
            
            DBMS_OUTPUT.PUT_LINE('Categories get successfully.');
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        raise_application_error(-20003, 'Error of get categories: categories not found.');
        CLOSE categoriesCursor;
    WHEN OTHERS THEN
        raise_application_error(-20003, 'Error of get categories: ' || SQLERRM);
        CLOSE categoriesCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetAllGoals(
    p_userID IN INT,
    goalsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN goalsCursor FOR
        SELECT *
        FROM Ilya.Goals
        WHERE Ilya.Goals.UserID = p_userID;
    
    
    DBMS_OUTPUT.PUT_LINE('Goals get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        raise_application_error(-20003, 'Error of get goals: goals not found.');
        CLOSE goalsCursor;
    WHEN OTHERS THEN
        raise_application_error(-20003, 'Error of get goals: ' || SQLERRM);
        CLOSE goalsCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetAllDebts(
    p_userID IN INT,
    debtsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN debtsCursor FOR
        SELECT *
        FROM Ilya.Debts
        WHERE Ilya.Debts.UserID = p_userID;
    
    DBMS_OUTPUT.PUT_LINE('Debts get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: debts not found.');
        CLOSE debtsCursor;
    WHEN OTHERS THEN
        raise_application_error(-20001, 'Error of get debts: ' || SQLERRM);
        CLOSE debtsCursor;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE GetDebt(
    p_debtID IN INT,
    debtsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN debtsCursor FOR
        SELECT *
        FROM Ilya.Debts
        WHERE Ilya.Debts.DebtID = p_debtID;
    
    DBMS_OUTPUT.PUT_LINE('Debts get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: debts not found.');
        CLOSE debtsCursor;
    WHEN OTHERS THEN
        raise_application_error(-20001, 'Error of get debts: ' || SQLERRM);
        CLOSE debtsCursor;
        RAISE;
END;

    EXEC ADDUSER('lOH', 'RRR', 'LOH', 6);
    CREATE OR REPLACE PROCEDURE AddUser(
       p_login IN NVARCHAR2,
       p_password IN NVARCHAR2,
       p_name IN NVARCHAR2,
       p_role IN SMALLINT
    )
    IS
    BEGIN
        IF p_role < 0 OR p_role > 1 THEN 
            raise_application_error(-20002, 'invalid role.');
             --DBMS_OUTPUT.PUT_LINE('Error inserting user: invalid role.');
             ROLLBACK;
        ELSE 
            INSERT INTO Ilya.Users (Login, Password, Name, Role)
            VALUES (p_login, p_password, p_name, p_role);
       
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('User inserted successfully.');
        END IF;
    EXCEPTION
       WHEN DUP_VAL_ON_INDEX THEN
          ROLLBACK;
          DBMS_OUTPUT.PUT_LINE('Error inserting user: user with the same login already exists.');
       WHEN OTHERS THEN
          ROLLBACK;
           raise_application_error(-20001, 'Error inserting user: ' || SQLERRM);
          --DBMS_OUTPUT.PUT_LINE('Error inserting user: ' || SQLERRM);
          RAISE;
    END;

select * from users;
exec AlterUser(1, 'Ilya', 'Mondax', '1234', 1);
CREATE OR REPLACE PROCEDURE AlterUser(
   p_id IN INT,
   p_name IN NVARCHAR2,
   p_login IN NVARCHAR2,
   p_password IN NVARCHAR2,
   p_role IN SMALLINT
)
IS
BEGIN
     IF p_role < 0 OR p_role > 1 THEN 
         DBMS_OUTPUT.PUT_LINE('Error inserting user: invalid role.');
         ROLLBACK;
    ELSE
        UPDATE Ilya.Users
        SET Name = p_name,
            Login = p_login,
            Password = p_password,
            Role = p_role
        WHERE ID = p_id;
        
        IF SQL%ROWCOUNT = 0 THEN
            RAISE NO_DATA_FOUND;
        END IF;
        
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('User altered successfully.');
   END IF;
EXCEPTION
   WHEN NO_DATA_FOUND THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering user: user with ID ' || p_id || ' not found.');
   WHEN OTHERS THEN
      ROLLBACK;
      DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE EditUser(
    p_id IN INT,
    p_name IN NVARCHAR2
)
IS
BEGIN
    UPDATE Ilya.Users
    SET Name = p_name
    WHERE ID = p_id;
   
    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
   
    DBMS_OUTPUT.PUT_LINE('User edit successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error edit user: user with ID ' || p_id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error edit user: ' || SQLERRM);
END;

-------------- пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ --------------

CREATE OR REPLACE PROCEDURE DeleteUser(
    p_id IN INT
)
IS
BEGIN
    DELETE FROM Saves WHERE UserID = p_id;

    DELETE FROM Goals WHERE UserID = p_id;

    DELETE FROM Debts WHERE UserID = p_id;

    DELETE FROM Users WHERE ID = p_id;
    
    DELETE FROM Ilya.Users WHERE ID = p_id;

    COMMIT;

    DBMS_OUTPUT.PUT_LINE('User deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: user with ID ' || p_id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: ' || SQLERRM);
END;
---------------------------------------------------------------------------------------------

CREATE OR REPLACE PROCEDURE AddSaves(
    p_amount NUMBER,
    p_saveDate IN DATE,
    p_categoryID IN INT,
    p_userID IN INT
)
AS
BEGIN
    INSERT INTO Ilya.Saves (Amount, SaveDate, CategoryID, UserID)
    VALUES (p_amount, p_saveDate, p_categoryID, p_userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Save inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the save: ' || SQLERRM);
END;

-------------- пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ --------------

CREATE OR REPLACE PROCEDURE DeleteSave(
    p_saveId IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Saves
    WHERE SaveId = p_saveId;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Save deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
         ROLLBACK;
            DBMS_OUTPUT.PUT_LINE('Error deleting save: save with ID ' || p_saveId || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting save: ' || SQLERRM);
        RAISE;
END;


--пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ--

SELECT * FROM Categories;
EXEC ADDCATEGORY ('DDD', 6, 'SDSD');
CREATE OR REPLACE PROCEDURE AddCategory(
    p_categoryName NVARCHAR2,
    p_type IN SMALLINT,
    p_picture IN NVARCHAR2
)
AS
BEGIN
    IF p_type < 0 OR p_type > 1 THEN 
        raise_application_error(-20002, 'invalid type.');
         --DBMS_OUTPUT.PUT_LINE('Error inserting category: invalid type.');
         ROLLBACK;
    ELSE
        INSERT INTO Ilya.Categories (CategoryName, Type, Picture)
        VALUES (p_categoryName, p_type, p_picture);
    
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('Category inserted successfully.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the category: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE AlterCategory(
    p_categoryID IN INT,
    p_categoryName IN NVARCHAR2,
    p_type IN SMALLINT,
    p_picture IN NVARCHAR2
)
IS
BEGIN
    IF p_type < 0 OR p_type > 1 THEN 
        DBMS_OUTPUT.PUT_LINE('Error altering category: invalid type.');
        ROLLBACK;
    ELSE
        UPDATE Ilya.Categories
        SET CategoryName = p_categoryName,
            Type = p_type,
            Picture = p_picture
        WHERE CategoryID = p_categoryID;
   
        IF SQL%ROWCOUNT = 0 THEN
            RAISE NO_DATA_FOUND;
        ELSE
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('Category altered successfully.');
        END IF;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting category: category with ID ' || p_categoryID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering category: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE DeleteCategory(
   p_categoryID IN INT
)
IS
BEGIN
   DELETE FROM Ilya.Categories
   WHERE CategoryID = p_categoryID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
   
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Category deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
         DBMS_OUTPUT.PUT_LINE('Error deleting category: category with ID ' || p_categoryID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting category: ' || SQLERRM);
        RAISE;
END;




--пїЅпїЅпїЅпїЅ--


CREATE OR REPLACE PROCEDURE AddGoal(
    p_nameGoal NVARCHAR2,
    p_accumulated IN NUMBER,
    p_price IN NUMBER,
    p_picture IN NVARCHAR2,
    p_userID INT
)
AS
BEGIN
    INSERT INTO Ilya.Goals(NameGoal, Accumulated, Price, Picture, UserID)
    VALUES (p_nameGoal, p_accumulated, p_price, p_picture, p_userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Goal inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the goal: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE AlterGoal(
    p_goalID INT,
    p_nameGoal NVARCHAR2,
    p_accumulated NUMBER,
    p_price NUMBER,
    p_picture NVARCHAR2
)
IS
BEGIN
    UPDATE Ilya.Goals
    SET NameGoal = p_nameGoal,
        Accumulated = p_accumulated,
        Price = p_price,
        Picture = p_picture
    WHERE GoalID = p_goalID;
   
    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Goal altered successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering goal: goal with ID ' || p_goalID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering goal: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE DeleteGoal(
   p_goalID IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Goals
    WHERE GoalID = p_goalID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
   COMMIT;
   DBMS_OUTPUT.PUT_LINE('Goal deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting goal: goal with ID ' || p_goalID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting goal: ' || SQLERRM);
        RAISE;
END;


--пїЅпїЅпїЅпїЅпїЅ--


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
    IF p_percent < 0 OR p_percent > 100 THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ssss ' || SQLERRM);
    ELSE
        INSERT INTO Ilya.Debts(NameDebt, AmountPayment, NumberPayments, Percent, ReceivingDate, MaturityDate, UserID)
        VALUES (p_nameDebt, p_amountPayment, p_numberPayments, p_percent, p_receivingDate, p_maturityDate, p_userID);
        
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('Debt inserted successfully.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the Debt: ' || SQLERRM);
END;


CREATE OR REPLACE PROCEDURE AlterDebt(
    p_debtID INT,
    p_nameDebt NVARCHAR2,
    p_paid INT,
    p_amountPayment NUMBER,
    p_numberPayments INT,
    p_percent NUMBER,
    p_receivingDate DATE,
    p_maturityDate DATE
)
IS
BEGIN
    UPDATE Ilya.Debts
    SET NameDebt = p_nameDebt,
        AmountPayment = p_amountPayment,
        Paid = p_paid,
        NumberPayments = p_numberPayments,
        Percent = p_percent,
        ReceivingDate = p_receivingDate,
        MaturityDate = p_maturityDate
    WHERE DebtID = p_debtID;
   
   IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Debt altered successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altered debt: debt with ID ' || p_debtID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering debt: ' || SQLERRM);
END;



CREATE OR REPLACE PROCEDURE DeleteDebt(
   p_debtID IN INT
)
IS
BEGIN
    DELETE FROM Ilya.Debts
    WHERE DebtID = p_debtID;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Debt deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting debt: debt with ID ' || p_debtID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Debt deleting debt: ' || SQLERRM);
        RAISE;
END;




CREATE OR REPLACE PROCEDURE CalculateSaves(
    startDate IN DATE,
    endDate IN DATE,
    p_userID IN INT,
    p_type IN SMALLINT,
    sumSaves OUT NUMBER
)
IS
BEGIN 
    IF startDate > endDate THEN 
        raise_application_error(-20001, 'Error of calculate saves: start date ' || startDate || ' > end date ' || endDate);
    ELSIF p_type < 0 AND p_type > 1 THEN 
        raise_application_error(-20001, 'Error of calculate saves: invalid type.');
        ROLLBACK;
    ELSE
        SELECT SUM(Amount)
        INTO sumIncome
        FROM Ilya.Saves S JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = p_type AND S.UserID = p_userID;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        raise_application_error(-20001, 'Error of calculate saves: ' || SQLERRM);
        RAISE; 
END;


CREATE OR REPLACE PROCEDURE CalculateBudget(
    startDate IN DATE,
    endDate IN DATE,
    p_userID IN INT,
    budget OUT NUMBER
)
IS
    sumIncome NUMBER;
    sumExpense NUMBER;
BEGIN 
    IF startDate > endDate THEN 
     raise_application_error(-20001, 'Error of calculate budget: start date ' || startDate || ' > end date ' || endDate);
    ELSE
        SELECT SUM(Amount)
        INTO sumIncome
        FROM Ilya.Saves S JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 0 AND S.UserID = p_userID;

        SELECT SUM(Amount)
        INTO sumExpense
        FROM Ilya.Saves S JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 1 AND S.UserID = p_userID;

        budget := sumIncome - sumExpense;
    END IF;
EXCEPTION
      WHEN OTHERS THEN
       raise_application_error(-20001, 'Error of calculate budget: ' || SQLERRM);
        RAISE; 
END;
  


CREATE OR REPLACE PROCEDURE CalculateSavesByCategory(
    startDate IN DATE,
    endDate IN DATE,
    p_categoryID IN INT,
    p_userID IN INT,
    p_type in SMALLINT,
    sumSavesOfCategory OUT NUMBER,
    partSaves OUT NUMBER
)
IS
    sumSaves NUMBER := 0;
BEGIN 
    IF startDate > endDate THEN 
        raise_application_error(-20001, 'Error of calculate incomes by category: start date ' || startDate || ' > end date ' || endDate);
    ELSIF p_type < 0 AND p_type > 1 THEN 
        raise_application_error(-20001, 'Error of calculate saves: invalid type.');
        ROLLBACK;
    ELSE
        SELECT SUM(Amount)
        INTO sumSavesOfCategory
        FROM Ilya.Saves S JOIN Ilya.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND S.categoryID = p_categoryID AND Type = p_type AND UserID = p_userID;
        
        
        CalculateSaves(startDate, endDate, p_userID, p_type, sumSaves);
        partSaves := sumSavesOfCategory / sumSaves * 100;    
    END IF;
EXCEPTION
      WHEN OTHERS THEN
        raise_application_error(-20001, 'Error of calculate incomes by category: ' || SQLERRM);
        RAISE; 
END;


CREATE OR REPLACE PROCEDURE CalculateTotalDebt(
    p_debtID IN NUMBER,
    totalDebt OUT NUMBER
)
IS
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentNumberPayments NUMBER := 0;
BEGIN
    totalDebt := 0;
    SELECT AmountPayment, Percent, NumberPayments INTO currentPayment, currentPercent, currentNumberPayments
    FROM Ilya.Debts
    WHERE DebtID = p_debtID;
    
    IF currentNumberPayments = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    currentPercent := currentPercent / 100;
    FOR i IN 1..currentNumberPayments LOOP
        totalDebt := totalDebt + currentPayment;
        currentPayment := currentPayment + (currentPayment * currentPercent);
    END LOOP;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate total debt: debt with ID ' || p_debtID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate total debt: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE CalculateTotalDebts(
    p_userID IN INT,
    totaDebts OUT NUMBER
)
IS
    totaDebt NUMBER := 0;
    userCount INT := 0;
BEGIN
    totaDebts := 0;
    SELECT COUNT(*) INTO userCount FROM Ilya.Users WHERE ID = p_userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM Ilya.Debts WHERE UserID = p_userID) 
    LOOP
        CalculateTotalDebt(debt.DebtID, totaDebt);
        totaDebts := totaDebts + totaDebt;
    END LOOP;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate debts: user with ID' || p_userID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate debts: ' || SQLERRM);
        RAISE;
END;



CREATE OR REPLACE PROCEDURE CalculatePaidDebt(
    p_debtID IN NUMBER,
    totalDebt OUT NUMBER
)
IS
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentPaid INT := 0;
    debtCount INT := 0;
BEGIN
    totalDebt := 0;
    SELECT COUNT(*) INTO debtCount FROM Ilya.Debts WHERE DebtID = debtID;
    
    IF debtCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    SELECT AmountPayment, Percent, Paid INTO currentPayment, currentPercent, currentPaid
    FROM Ilya.Debts
    WHERE DebtID = p_debtID;
    
    currentPercent := currentPercent / 100;
    FOR i IN 1..currentPaid LOOP
        totalDebt := totalDebt + currentPayment;
        currentPayment := currentPayment + (currentPayment * currentPercent);
    END LOOP;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debt: debt with ID ' || p_debtID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debt: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE CalculatePaidDebts(
    p_userID IN INT,
    totaDebts OUT NUMBER
)
IS
    totaDebt NUMBER := 0;
    userCount INT := 0;
BEGIN
    totaDebts := 0;
    SELECT COUNT(*) INTO userCount FROM Ilya.Users WHERE ID = p_userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM Ilya.Debts WHERE UserID = p_userID) 
    LOOP
        CalculatePaidDebt(debt.DebtID, totaDebt);
        totaDebts := totaDebts + totaDebt;
    END LOOP;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debts: user ' || p_userID || ' not found.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate paid debts: ' || SQLERRM);
        RAISE;
END;


SELECT * FROM USERS;
SELECT * FROM Debts;
EXEC AddDebt('Долг Китая', 1400, 4, 3 / 10, TO_DATE('2023-01-01', 'YYYY-MM-DD'), TO_DATE('2023-12-31', 'YYYY-MM-DD'), 1);
EXEC AddDebt('Долг США', 1400, 20, 3, TO_DATE('2023-01-01', 'YYYY-MM-DD'), TO_DATE('2023-12-31', 'YYYY-MM-DD'), 1);
exec AlterDebt(2, 'Долг Китая', 2, 1400, 4, 3, TO_DATE('2023-01-01', 'YYYY-MM-DD'), TO_DATE('2023-12-31', 'YYYY-MM-DD'));


DECLARE
    v_id INT := 23;
    v_res NUMBER;
BEGIN
    CalculateTotalDebt(v_id, v_res);
    -- Выведем результаты
    DBMS_OUTPUT.PUT_LINE('rES: ' || v_res);
END;

DECLARE
    v_id INT := 23;
    v_res NUMBER;
BEGIN
    CalculatePaidDebt(v_id, v_res);
    -- Выведем результаты
    DBMS_OUTPUT.PUT_LINE('rES: ' || v_res);
END;
