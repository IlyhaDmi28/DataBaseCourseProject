-------------- Роль администратора --------------
ALTER SESSION SET CONTAINER = FinancialAssistant;
ALTER SESSION SET CONTAINER = CDB$ROOT;

drop role AdminRole;
drop role UserRole;


create role AdminRole;

GRANT DBA TO AdminRole;
GRANT EXECUTE ANY PROCEDURE TO AdminRole;

-------------- Роль пользователя --------------

CREATE ROLE UserRole;


GRANT CREATE SESSION TO UserRole;
-- User-related procedures
GRANT EXECUTE ON ADMIN.GetUserByID TO UserRole;
GRANT EXECUTE ON ADMIN.AlterUser TO UserRole;
GRANT EXECUTE ON ADMIN.AlterUserByNameAndPicture TO UserRole;

-- Saves-related procedures
GRANT EXECUTE ON ADMIN.GetAllSaves TO UserRole;
GRANT EXECUTE ON ADMIN.GetSavesByCategoryType TO UserRole;
GRANT EXECUTE ON ADMIN.AddSaves TO UserRole;
GRANT EXECUTE ON ADMIN.DeleteSave TO UserRole;

-- Categories-related procedures
GRANT EXECUTE ON ADMIN.GetCategoriesByType TO UserRole;

-- Goals-related procedures
GRANT EXECUTE ON ADMIN.GetAllGoals TO UserRole;
GRANT EXECUTE ON ADMIN.AddGoal TO UserRole;
GRANT EXECUTE ON ADMIN.AlterGoal TO UserRole;
GRANT EXECUTE ON ADMIN.DeleteGoal TO UserRole;

-- Debts-related procedures
GRANT EXECUTE ON ADMIN.GetAllDebts TO UserRole;
GRANT EXECUTE ON ADMIN.GetDebtByID TO UserRole;
GRANT EXECUTE ON ADMIN.AddDebt TO UserRole;
GRANT EXECUTE ON ADMIN.AlterDebt TO UserRole;
GRANT EXECUTE ON ADMIN.DeleteDebt TO UserRole;

-- Calculation-related procedures
GRANT EXECUTE ON ADMIN.CalculateSaves TO UserRole;
GRANT EXECUTE ON ADMIN.CalculateBudget TO UserRole;
GRANT EXECUTE ON ADMIN.CalculateSavesByCategory TO UserRole;
GRANT EXECUTE ON ADMIN.CalculateTotalDebt TO UserRole;
GRANT EXECUTE ON ADMIN.CalculateTotalDebts TO UserRole;
GRANT EXECUTE ON ADMIN.CalculatePaidDebt TO UserRole;
GRANT EXECUTE ON ADMIN.CalculatePaidDebts TO UserRole;


-------------- Пользователи --------------

DROP USER ADMIN CASCADE;
DROP USER FA_USER;

CREATE USER ADMIN IDENTIFIED BY 1111;
GRANT AdminRole TO ADMIN;
GRANT EXECUTE ON SYS.DBMS_CRYPTO TO ADMIN;
ALTER USER ADMIN QUOTA UNLIMITED ON FA_TS;


CREATE USER FA_USER IDENTIFIED BY 1234;
GRANT UserRole TO FA_USER;
ALTER USER FA_USER QUOTA UNLIMITED ON FA_TS;



