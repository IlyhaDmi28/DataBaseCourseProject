--Последний запуск бд--


--Текущее кол. сессий--
SELECT COUNT(*) FROM v$session WHERE status = 'ACTIVE';


--Время и инфа о select запрсах
SELECT * FROM V$INSTANCE;
SELECT * FROM V$SQLSTATS;
SELECT * FROM V$SESSION;
SELECT * FROM V$OSSTAT;
SELECT * FROM V$SESSION_EVENT; 
SELECT * FROM V$SYSTEM_EVENT;
SELECT * FROM V$EVENT_NAME;
SELECT* FROM V$SGASTAT;
SELECT * FROM V$FILESTAT;
SELECT * FROM V$TEMPSTAT;

SELECT * FROM SAVES WHERE saveID = 4789;
SELECT * FROM SAVES WHERE SaveDate >= TO_DATE('2023-12-01', 'YYYY-MM-DD') AND SaveDate <= TO_DATE('2023-12-31', 'YYYY-MM-DD');
SELECT * FROM SAVES WHERE CategoryID = 5;

select * from table(dbms_xplan.display_cursor(sql_id=>'15j2syr595sxp', format=>'ALLSTATS LAST'));



