using Financial_assistant.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Financial_assistant.Classes;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace Financial_assistant
{
    public static class DBControler
    {
        public static AdditionsDB AdditionDB;
        public static CatigoriesDB CatigoryDB;
        public static GoalsDB GoalDB;
        public static UsersDB UserDB;
        public static DebtDB DebtDB;

        private static OracleConnection connection;

        static DBControler()
        {
            try
            {
                connection = new OracleConnection("Data Source=//oraclevm:1521/FinancialAssistant;User Id=ADMIN;Password=1111;");
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }

            AdditionDB = new AdditionsDB(connection);
            CatigoryDB = new CatigoriesDB(connection);
            GoalDB = new GoalsDB(connection);
            UserDB = new UsersDB(connection);
            DebtDB = new DebtDB(connection);
        }

        public static void ChangeToAdmin()
        {
            try
            {
                connection = new OracleConnection("Data Source=//oraclevm:1521/FinancialAssistant;User Id=ADMIN;Password=1111;");
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }

            AdditionDB = new AdditionsDB(connection);
            CatigoryDB = new CatigoriesDB(connection);
            GoalDB = new GoalsDB(connection);
            UserDB = new UsersDB(connection);
            DebtDB = new DebtDB(connection);
        }

        public static void ChangeToUser()
        {
            try
            {
                connection = new OracleConnection("Data Source=//oraclevm:1521/FinancialAssistant;User Id=FA_USER;Password=1234;");
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }

            AdditionDB = new AdditionsDB(connection);
            CatigoryDB = new CatigoriesDB(connection);
            GoalDB = new GoalsDB(connection);
            UserDB = new UsersDB(connection);
            DebtDB = new DebtDB(connection);
        }


        public static bool DBGenerate()
        {
            try
            {
                UserDB.Add(new User("ADMIN", "ADMIN", "6666", new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png")), Role.ADMIN));
                UserDB.Add(new User("Ilya", "Ilya", "1111", new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png")), Role.USER));
                UserDB.Add(new User("Alexander", "Alexander", "2222", new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png")), Role.USER));
                UserDB.Add(new User("Andrey", "Andrey", "3333", new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png")), Role.USER));
                UserDB.Add(new User("Lesha", "Lesha", "4444", new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png")), Role.USER));


                CatigoryDB.Add(new Categories("Еда", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\food.png"))));
                CatigoryDB.Add(new Categories("Вредная еда", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\junk_food.png"))));
                CatigoryDB.Add(new Categories("Одежда", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\clothes.png"))));
                CatigoryDB.Add(new Categories("Электроника", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\gadget.png"))));
                CatigoryDB.Add(new Categories("Услуги", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\service.png"))));
                CatigoryDB.Add(new Categories("Медицина", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\medicine.png"))));
                CatigoryDB.Add(new Categories("Зарплата", TypeAdditon.INCOME, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\salary.png"))));
                CatigoryDB.Add(new Categories("Подарки", TypeAdditon.INCOME, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\present.png"))));
                CatigoryDB.Add(new Categories("Другие доходы", TypeAdditon.INCOME, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\other_incomes.png"))));
                CatigoryDB.Add(new Categories("Другие расходы", TypeAdditon.EXPENSES, new BitmapImage(new Uri("D:\\Labs\\Курсач БД\\Financial assistant\\Picture\\Categories\\other_exponses.png"))));

                GenerateSavesJsonFile();
                InputSavesToTable();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void GenerateSavesJsonFile()
        {
            JArray jsonArray = new JArray();



            Random random = new Random();

            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            // Вычисляем разницу в днях между начальной и конечной датой
            int range = (endDate - startDate).Days;

            // Генерируем случайное количество дней от начальной даты
            int randomDays = 0;

            // Добавляем случайное количество дней к начальной дате
            DateTime randomDate = startDate.AddDays(randomDays);


            for (int i = 1; i <= 100000; i++)
            {
                randomDays = random.Next(range);
                JObject jsonItem = new JObject
                { 
                    { "Amount", random.Next(1, 10000).ToString() },
                    { "SaveDate", startDate.AddDays(randomDays).ToString("yyyy-MM-dd")},
                    { "CategoryID",  random.Next(1, 10).ToString()},
                    { "UserID",  random.Next(1, 5).ToString()},
                };

                jsonArray.Add(jsonItem);

            }

            // Сохранение в файл
            using (StreamWriter file = new StreamWriter("Saves.json", false, Encoding.UTF8))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jsonArray.WriteTo(writer);
                }
            }
        }

        static private void InputSavesToTable()
        {
            string jsonFilePath = "Saves.json";

            // Чтение JSON из файла
            string jsonData = File.ReadAllText(jsonFilePath);


            using (OracleCommand command = new OracleCommand("ADMIN.LoadDataFromJsonSaves", connection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_JsonData", OracleDbType.Clob).Value = jsonData;

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
