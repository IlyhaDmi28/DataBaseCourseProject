-- Процедуры для взаимодействия с таблицей USER
DROP PROCEDURE GetAllUsers;
DROP PROCEDURE GetUserByID;
DROP PROCEDURE GetUserByLogin;
DROP PROCEDURE AddUser;
DROP PROCEDURE AlterUser;
DROP PROCEDURE EditUser;
DROP PROCEDURE DeleteUser;

-- Процедуры для взаимодействия с таблицей Saves
DROP PROCEDURE GetAllSaves;
DROP PROCEDURE GetSavesByCategoryType;
DROP PROCEDURE AddSaves;
DROP PROCEDURE DeleteSave;

-- Процедуры для взаимодействия с таблицей Categories
DROP PROCEDURE GetCategoriesByType;
DROP PROCEDURE AddCategory;
DROP PROCEDURE AlterCategory;
DROP PROCEDURE DeleteCategory;

-- Процедуры для взаимодействия с таблицей Goals
DROP PROCEDURE GetAllGoals;
DROP PROCEDURE AddGoal;
DROP PROCEDURE AlterGoal;
DROP PROCEDURE DeleteGoal;

-- Процедуры для взаимодействия с таблицей Debts
DROP PROCEDURE GetAllDebts;
DROP PROCEDURE GetDebt;
DROP PROCEDURE AddDebt;
DROP PROCEDURE AlterDebt;
DROP PROCEDURE DeleteDebt;

-- Процедуры для вычислений
DROP PROCEDURE CalculateSaves;
DROP PROCEDURE CalculateBudget;
DROP PROCEDURE CalculateSavesByCategory;
DROP PROCEDURE CalculateTotalDebt;
DROP PROCEDURE CalculateTotalDebts;
DROP PROCEDURE CalculatePaidDebt;
DROP PROCEDURE CalculatePaidDebts;

-------------- Процедуры для взаимодейтсвия с таблицей USER --------------

CREATE OR REPLACE PROCEDURE GetAllUsers(
    userCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM ADMIN.Users;

    DBMS_OUTPUT.PUT_LINE('Users get successfully.');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get users: ' || SQLERRM);
        CLOSE userCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetUserByID(
    p_UserID IN INT,
    userCursor OUT SYS_REFCURSOR
) 
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM ADMIN.Users WHERE ID = p_UserID;
    
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

SELECT * FROM USERS;
EXEC DeleteUser(1);

SELECT * FROM CATEGORIES;

CREATE OR REPLACE PROCEDURE GetUserByLoginAndPassword(
    p_login IN NVARCHAR2,
    p_passwod IN NVARCHAR2,
    userCursor OUT SYS_REFCURSOR
)
IS
    raw_password raw(2000) := SYS.UTL_I18N.STRING_TO_RAW (p_passwod, 'AL32UTF8');
    hash_password raw(2000);
BEGIN
    hash_password := SYS.DBMS_CRYPTO.HASH(raw_password, SYS.DBMS_CRYPTO.HASH_SH1);
    
    OPEN userCursor FOR
        SELECT * FROM ADMIN.Users WHERE Login = p_login AND Password = hash_password;
    
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

select * from Users;
CREATE OR REPLACE PROCEDURE GetUserByLogin(
    p_login IN NVARCHAR2,
    userCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN userCursor FOR
        SELECT * FROM ADMIN.Users WHERE Login = p_login;
    
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

SELECT * FROM Users;

CREATE OR REPLACE PROCEDURE AddUser(
    p_login IN NVARCHAR2,
    p_password IN NVARCHAR2,
    p_name IN NVARCHAR2,
    p_picture IN BLOB,
    p_role IN SMALLINT
)
IS
    raw_password raw(2000) := SYS.UTL_I18N.STRING_TO_RAW (p_password, 'AL32UTF8');
    hash_password raw(2000);
BEGIN
    IF p_role < 0 OR p_role > 1 THEN 
        raise_application_error(-20001, 'invalid role.');
    ELSE 
        hash_password := SYS.DBMS_CRYPTO.HASH(raw_password, SYS.DBMS_CRYPTO.HASH_SH1);
        
        INSERT INTO ADMIN.Users (Login, Password, Name, Picture, Role)
        VALUES (p_login, hash_password, p_name, p_picture, p_role);
       
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('User inserted successfully.');
    END IF;
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting user: user with the same login already exists.');
        raise_application_error(-20004, 'User with the same login already exists.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting user: ' || SQLERRM);
        RAISE;
END;

SELECT * FROM Users;
EXEC AlterUser(21, 'ADMIN', 'ADMIN', '6666', null, 1);
CREATE OR REPLACE PROCEDURE AlterUser(
   p_id IN INT,
   p_name IN NVARCHAR2,
   p_login IN NVARCHAR2,
   p_password IN NVARCHAR2,
   p_picture IN BLOB,
   p_role IN SMALLINT
)
IS
    raw_password raw(2000) := SYS.UTL_I18N.STRING_TO_RAW (p_password, 'AL32UTF8');
    hash_password raw(2000);
BEGIN
     IF p_role < 0 OR p_role > 1 THEN 
         raise_application_error(-20001, 'invalid role.');
    ELSE
         hash_password := SYS.DBMS_CRYPTO.HASH(raw_password, SYS.DBMS_CRYPTO.HASH_SH1);
         
        UPDATE ADMIN.Users
        SET Name = p_name,
            Login = p_login,
            Password = hash_password,
            Picture = p_picture,
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
    WHEN DUP_VAL_ON_INDEX THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting user: user with the same login already exists.');
        raise_application_error(-20004, 'User with the same login already exists.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE AlterUserWithoutPassword(
   p_id IN INT,
   p_name IN NVARCHAR2,
   p_login IN NVARCHAR2,
   p_picture IN BLOB,
   p_role IN SMALLINT
)
IS
BEGIN
     IF p_role < 0 OR p_role > 1 THEN 
         raise_application_error(-20001, 'invalid role.');
    ELSE
      
         
        UPDATE ADMIN.Users
        SET Name = p_name,
            Login = p_login,
            Picture = p_picture,
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
    WHEN DUP_VAL_ON_INDEX THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting user: user with the same login already exists.');
        raise_application_error(-20004, 'User with the same login already exists.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
        RAISE;
END;



select * from users;
CREATE OR REPLACE PROCEDURE AlterUserByNameAndPicture(
   p_id IN INT,
   p_name IN NVARCHAR2,
   p_picture IN BLOB
)
IS
BEGIN         
        UPDATE ADMIN.Users
        SET Name = p_name,
            Picture = p_picture
        WHERE ID = p_id;
        
        IF SQL%ROWCOUNT = 0 THEN
            RAISE NO_DATA_FOUND;
        END IF;
        
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('User altered successfully.');
EXCEPTION
   WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering user: user with ID ' || p_id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering user: ' || SQLERRM);
        RAISE;
END;



CREATE OR REPLACE PROCEDURE DeleteUser(
    p_id IN INT
)
IS
BEGIN
    DELETE FROM Saves WHERE UserID = p_id;

    DELETE FROM Goals WHERE UserID = p_id;

    DELETE FROM Debts WHERE UserID = p_id;

    DELETE FROM Users WHERE ID = p_id;
    
    DELETE FROM ADMIN.Users WHERE ID = p_id;

    COMMIT;

    DBMS_OUTPUT.PUT_LINE('User deleted successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: user with ID ' || p_id || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting user: ' || SQLERRM);
        RAISE;
END;


-------------- Процедуры для взаимодейтсвия с таблицей Saves --------------

CREATE OR REPLACE PROCEDURE GetAllSaves(
    startDate IN DATE,
    endDate IN DATE,
    p_userID IN INT,
    savesCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    IF startDate > endDate THEN 
       DBMS_OUTPUT.PUT_LINE('Error of get saves: start date ' || startDate || ' > end date ' || endDate);
    ELSE 
        OPEN savesCursor FOR
            SELECT S.*, C.CategoryName
            FROM ADMIN.Saves S
            JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
            WHERE SaveDate >= startDate AND SaveDate <= endDate AND S.UserID = p_userID;
            
            DBMS_OUTPUT.PUT_LINE('Saves get by categories successfully.');
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
            FROM ADMIN.Saves S
            JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
            WHERE C.Type = p_type AND SaveDate >= startDate AND SaveDate <= endDate AND S.UserID = p_userID;
           
           DBMS_OUTPUT.PUT_LINE('Saves get by categories successfully.');
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


CREATE OR REPLACE PROCEDURE AddSaves(
    p_amount IN NUMBER,
    p_saveDate IN DATE,
    p_categoryID IN INT,
    p_userID IN INT
)
AS
BEGIN
    INSERT INTO ADMIN.Saves (Amount, SaveDate, CategoryID, UserID)
    VALUES (p_amount, p_saveDate, p_categoryID, p_userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Save inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the save: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE DeleteSave(
    p_saveId IN INT
)
IS
BEGIN
    DELETE FROM ADMIN.Saves
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


-------------- Процедуры для взаимодейтсвия с таблицей Categories --------------
CREATE OR REPLACE PROCEDURE GetCategoriesByType(
    p_type IN SMALLINT,
    categoriesCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    IF p_type < 0 AND p_type > 1 THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: invalid category type');
    ELSE
        OPEN categoriesCursor FOR
            SELECT *
            FROM ADMIN.Categories
            WHERE Type = p_type;
            
            DBMS_OUTPUT.PUT_LINE('Categories get successfully.');
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: categories not found.');
        CLOSE categoriesCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get categories: ' || SQLERRM);
        CLOSE categoriesCursor;
        RAISE;
END;

select * from categories;
CREATE OR REPLACE PROCEDURE AddCategory(
    p_categoryName IN NVARCHAR2,
    p_type IN SMALLINT,
    p_picture IN BLOB
)
AS
BEGIN
    IF p_type < 0 OR p_type > 1 THEN 
         raise_application_error(-20002, 'invalid type.');
    ELSE
        INSERT INTO ADMIN.Categories (CategoryName, Type, Picture)
        VALUES (p_categoryName, p_type, p_picture);
    
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('Category inserted successfully.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the category: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE AlterCategory(
    p_categoryID IN INT,
    p_categoryName IN NVARCHAR2,
    p_type IN SMALLINT,
    p_picture IN BLOB
)
IS
BEGIN
    IF p_type < 0 OR p_type > 1 THEN 
        raise_application_error(-20002, 'invalid type.');
    ELSE
        UPDATE ADMIN.Categories
        SET CategoryName = p_categoryName,
            Type = p_type,
            Picture = p_picture
        WHERE CategoryID = p_categoryID;
   
        IF SQL%ROWCOUNT = 0 THEN
            RAISE NO_DATA_FOUND;
        END IF;
            COMMIT;
            DBMS_OUTPUT.PUT_LINE('Category altered successfully.');
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting category: category with ID ' || p_categoryID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering category: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE DeleteCategory(
   p_categoryID IN INT
)
IS
BEGIN
    DELETE FROM ADMIN.Saves
    WHERE CategoryID = p_categoryID;

    DELETE FROM ADMIN.Categories
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


-------------- Процедуры для взаимодейтсвия с таблицей Goals --------------

CREATE OR REPLACE PROCEDURE GetAllGoals(
    p_userID IN INT,
    goalsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN goalsCursor FOR
        SELECT *
        FROM ADMIN.Goals
        WHERE ADMIN.Goals.UserID = p_userID;
    
    
    DBMS_OUTPUT.PUT_LINE('Goals get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get goals: goals not found.');
        CLOSE goalsCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get goals: ' || SQLERRM);
        CLOSE goalsCursor;
        RAISE;
END;


exec ADMIN.AddGoal('sd', 100, 1000, null, 1);
CREATE OR REPLACE PROCEDURE AddGoal(
    p_nameGoal NVARCHAR2,
    p_accumulated IN NUMBER,
    p_price IN NUMBER,
    p_picture IN BLOB,
    p_userID IN INT
)
AS
BEGIN
    INSERT INTO ADMIN.Goals(NameGoal, Accumulated, Price, Picture, UserID)
    VALUES (p_nameGoal, p_accumulated, p_price, p_picture, p_userID);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Goal inserted successfully.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the goal: ' || SQLERRM);
        RAISE;
END;


CREATE OR REPLACE PROCEDURE AlterGoal(
    p_goalID IN INT,
    p_nameGoal IN NVARCHAR2,
    p_accumulated IN NUMBER,
    p_price IN NUMBER,
    p_picture IN BLOB
)
IS
BEGIN
    UPDATE ADMIN.Goals
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
        RAISE;
END;

select * from users;
CREATE OR REPLACE PROCEDURE DeleteGoal(
   p_goalID IN INT
)
IS
BEGIN
    DELETE FROM ADMIN.Goals
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


-------------- Процедуры для взаимодейтсвия с таблицей Debts --------------

CREATE OR REPLACE PROCEDURE GetAllDebts(
    p_userID IN INT,
    debtsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN debtsCursor FOR
        SELECT *
        FROM ADMIN.Debts
        WHERE ADMIN.Debts.UserID = p_userID;
    
    DBMS_OUTPUT.PUT_LINE('Debts get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: debts not found.');
        CLOSE debtsCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: ' || SQLERRM);
        CLOSE debtsCursor;
        RAISE;
END;


CREATE OR REPLACE PROCEDURE GetDebtByID(
    p_debtID IN INT,
    debtsCursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN debtsCursor FOR
        SELECT *
        FROM ADMIN.Debts
        WHERE ADMIN.Debts.DebtID = p_debtID;
    
    DBMS_OUTPUT.PUT_LINE('Debts get successfully.');
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: debts not found.');
        CLOSE debtsCursor;
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of get debts: ' || SQLERRM);
        CLOSE debtsCursor;
        RAISE;
END;


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
        raise_application_error(-20003, 'invalid percent.');
    ELSE
        INSERT INTO ADMIN.Debts(NameDebt, AmountPayment, NumberPayments, Percent, ReceivingDate, MaturityDate, UserID)
        VALUES (p_nameDebt, p_amountPayment, p_numberPayments, p_percent, p_receivingDate, p_maturityDate, p_userID);
        
        COMMIT;
        DBMS_OUTPUT.PUT_LINE('Debt inserted successfully.');
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inserting the Debt: ' || SQLERRM);
        RAISE;
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
    IF p_percent < 0 OR p_percent > 100 THEN
        ROLLBACK;
        raise_application_error(-20003, 'invalid percent.');
    ELSE
        UPDATE ADMIN.Debts
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
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altered debt: debt with ID ' || p_debtID || ' not found.');
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error altering debt: ' || SQLERRM);
        RAISE;
END;


select * from debts;
CREATE OR REPLACE PROCEDURE DeleteDebt(
   p_debtID IN INT
)
IS
BEGIN
    DELETE FROM ADMIN.Debts
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


-------------- Процедуры для вычеслений --------------

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
        DBMS_OUTPUT.PUT_LINE('Error of calculate saves: start date ' || startDate || ' > end date ' || endDate);
    ELSIF p_type < 0 AND p_type > 1 THEN 
        DBMS_OUTPUT.PUT_LINE('Error of calculate saves: invalid type.');
    ELSE
        SELECT SUM(Amount)
        INTO sumSaves
        FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = p_type AND S.UserID = p_userID;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate saves: ' || SQLERRM);
        RAISE; 
END;


CREATE OR REPLACE PROCEDURE CalculateBudget(
    startDate IN DATE,
    endDate IN DATE,
    p_userID IN INT,
    budget OUT NUMBER
)
IS
    sumIncome NUMBER := 0;
    sumExpense NUMBER := 0;
    incomeCount NUMBER := 0;
    expenseCount NUMBER := 0;
BEGIN 
    IF startDate > endDate THEN 
     raise_application_error(-20001, 'Error of calculate budget: start date ' || startDate || ' > end date ' || endDate);
    ELSE
   SELECT COUNT(*)
        INTO incomeCount
        FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 0 AND S.UserID = p_userID;

        IF incomeCount > 0 THEN
            SELECT SUM(Amount)
            INTO sumIncome
            FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
            WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 0 AND S.UserID = p_userID;
        END IF;

        SELECT COUNT(*)
        INTO expenseCount
        FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 1 AND S.UserID = p_userID;

        IF expenseCount > 0 THEN
            SELECT SUM(Amount)
            INTO sumExpense
            FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
            WHERE SaveDate >= startDate AND SaveDate <= endDate AND C.Type = 1 AND S.UserID = p_userID;
        END IF;

        budget := sumIncome - sumExpense;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error of calculate budget: ' || SQLERRM);
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
        DBMS_OUTPUT.PUT_LINE('Error of calculate incomes by category: start date ' || startDate || ' > end date ' || endDate);
    ELSIF p_type < 0 AND p_type > 1 THEN 
        DBMS_OUTPUT.PUT_LINE('Error of calculate saves: invalid type.');
    ELSE
        SELECT SUM(Amount)
        INTO sumSavesOfCategory
        FROM ADMIN.Saves S JOIN ADMIN.Categories C ON S.CategoryID = C.CategoryID
        WHERE SaveDate >= startDate AND SaveDate <= endDate AND S.categoryID = p_categoryID AND Type = p_type AND UserID = p_userID;
        
        CalculateSaves(startDate, endDate, p_userID, p_type, sumSaves);
        partSaves := sumSavesOfCategory / sumSaves * 100;    
    END IF;
EXCEPTION
      WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error of calculate incomes by category: ' || SQLERRM);
            RAISE; 
END;


CREATE OR REPLACE PROCEDURE CalculateTotalDebt(
    p_debtID IN INT,
    totalDebt OUT NUMBER
)
IS
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentNumberPayments NUMBER := 0;
BEGIN
    totalDebt := 0;
    SELECT AmountPayment, Percent, NumberPayments INTO currentPayment, currentPercent, currentNumberPayments
    FROM ADMIN.Debts
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
    SELECT COUNT(*) INTO userCount FROM ADMIN.Users WHERE ID = p_userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM ADMIN.Debts WHERE UserID = p_userID) 
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
    p_debtID IN INT,
    totalDebt OUT NUMBER
)
IS
    currentPayment NUMBER := 0;
    currentPercent NUMBER := 0;
    currentPaid INT := 0;
    debtCount INT := 0;
BEGIN
    totalDebt := 0;
    SELECT COUNT(*) INTO debtCount FROM ADMIN.Debts WHERE DebtID = debtID;
    
    IF debtCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    SELECT AmountPayment, Percent, Paid INTO currentPayment, currentPercent, currentPaid
    FROM ADMIN.Debts
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
    SELECT COUNT(*) INTO userCount FROM ADMIN.Users WHERE ID = p_userID;
    
    IF userCount = 0 THEN
        RAISE NO_DATA_FOUND;
    END IF;
    
    FOR debt IN (SELECT DebtID FROM ADMIN.Debts WHERE UserID = p_userID) 
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