CREATE OR REPLACE PROCEDURE LoadDataFromJsonSaves(p_JsonData CLOB) AS
BEGIN
    INSERT INTO ADMIN.Saves (Amount, SaveDate, CategoryID, UserID)
    SELECT Amount, TO_DATE(SaveDate, 'YYYY-MM-DD'), CategoryID, UserID
    FROM JSON_TABLE(
        p_JsonData,
        '$[*]'
        COLUMNS (
            Amount PATH '$.Amount',
            SaveDate PATH '$.SaveDate',
            CategoryID PATH '$.CategoryID',
            UserID PATH '$.UserID'
        )
    );

    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' ||  SQLCODE || ' - ' || SQLERRM);
        ROLLBACK;
        RAISE;
END LoadDataFromJsonSaves;

select * from users;

select * from Categories;

select * from saves;

SELECT * FROM V$VERSION;