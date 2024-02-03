SELECT * FROM ADMIN.Saves;
DELETE FROM ADMIN.Saves;

CREATE OR REPLACE PROCEDURE BackupSavesMinuteAgo
IS
BEGIN
    DELETE FROM ADMIN.Saves;
    
    INSERT INTO ADMIN.Saves SELECT * FROM ADMIN.Saves AS OF TIMESTAMP (SYSTIMESTAMP - INTERVAL '1' MINUTE);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Data of saves successfully backup.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting all users: ' || SQLERRM);
        RAISE;
END;
EXEC BackupSavesMinuteAgo();


CREATE OR REPLACE PROCEDURE BackupSavesHourAgo
IS
BEGIN
    DELETE FROM ADMIN.Saves;
    
    INSERT INTO ADMIN.Saves SELECT * FROM ADMIN.Saves AS OF TIMESTAMP (SYSTIMESTAMP - INTERVAL '1' HOUR);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Data of saves successfully backup.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting all users: ' || SQLERRM);
        RAISE;
END;
EXEC BackupSavesHourAgo();


CREATE OR REPLACE PROCEDURE BackupSavesDayAgo
IS
BEGIN
    DELETE FROM ADMIN.Saves;
    
    INSERT INTO ADMIN.Saves SELECT * FROM ADMIN.Saves AS OF TIMESTAMP (SYSTIMESTAMP - INTERVAL '1' HOUR);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Data of saves successfully backup.');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error deleting all users: ' || SQLERRM);
        RAISE;
END;
EXEC BackupSavesDayAgo();



