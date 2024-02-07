using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Types;
using System.Security.Policy;

namespace Financial_assistant
{

    public class AdditionsDB
    {
        OracleConnection connection;
        public List<Addition> Get(DateTime StartDate, DateTime EndDate, TypeAdditon type)
        {
            List<Addition> additions = new List<Addition>();    
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetSavesByCategoryType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)type;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("savesCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Выполняем процедуру
                    //command.ExecuteNonQuery();

                    // Получаем результат из курсора
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                switch (type)
                                {
                                    case TypeAdditon.INCOME:
                                        additions.Add(new Addition(reader.GetInt32(0), reader.GetDouble(1), Account.CategoriesIncomes.FirstOrDefault(category => category.ID == reader.GetInt32(3)), reader.GetDateTime(2)));
                                        break;
                                    case TypeAdditon.EXPENSES:
                                        additions.Add(new Addition(reader.GetInt32(0), reader.GetDouble(1), Account.CategoriesExpenses.FirstOrDefault(category => category.ID == reader.GetInt32(3)), reader.GetDateTime(2)));
                                        break;
                                    default:
                                        break;
                                }
                                
                            }
                            return additions;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        public List<Addition> Get(DateTime StartDate, DateTime EndDate)
        {
            List<Addition> additions = new List<Addition>();
            List<Categories> categories = new List<Categories>();
            categories = Account.CategoriesIncomes.Concat(Account.CategoriesExpenses).ToList();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetAllSaves", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("savesCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Выполняем процедуру
                    //command.ExecuteNonQuery();

                    // Получаем результат из курсора
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                additions.Add(new Addition(reader.GetInt32(0), reader.GetDouble(1), categories.FirstOrDefault(category => category.ID == reader.GetInt32(3)), reader.GetDateTime(2)));
                            }
                            return additions;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }



        public void Add(Addition add)
        {
            if(add.Category == null) return;
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddSaves", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    
                    // Добавляем параметры в команду
                    command.Parameters.Add("p_amount", OracleDbType.Decimal).Value = add.Amount;
                    command.Parameters.Add("p_saveDate", OracleDbType.Date).Value = add.Date;
                    command.Parameters.Add("p_categoryID", OracleDbType.Int32).Value = add.Category.ID;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;

                    command.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        public double GetBudget(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateBudget", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("budget", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimal = (OracleDecimal)command.Parameters["budget"].Value;
                    return (double)oracleDecimal.Value;
                }
            }
            catch
            {
                return 0;
            }
        }

        public double GetIncomesSum(DateTime StartDate, DateTime EndDate)
        {
            try
            { 
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateSaves", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)TypeAdditon.INCOME;
                    command.Parameters.Add("sumSaves", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimal = (OracleDecimal)command.Parameters["sumSaves"].Value;
                    return (double)oracleDecimal.Value;
                }
            }
            catch
            {
                return 0;
            }
        }

        public double GetExpensesSum(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateSaves", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)TypeAdditon.EXPENSES;

                    command.Parameters.Add("sumSaves", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimal = (OracleDecimal)command.Parameters["sumSaves"].Value;
                    return (double)oracleDecimal.Value;
                }
            }
            catch
            {
                return 0;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.DeleteSave", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_saveId", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }



        public AdditionsDB(OracleConnection connection)
        {
            this.connection = connection;
        }
    }
}
