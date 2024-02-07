using Financial_assistant.Pages;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_assistant
{
    public class DebtDB
    {
        OracleConnection connection;
        double totalDebt;
        double paidSum;
        public List<Debt> Get()
        {
            List<Debt> debts= new List<Debt>();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetAllDebts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("debtsCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Выполняем процедуру
                    //command.ExecuteNonQuery();

                    // Получаем результат из курсора
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                debts.Add(new Debt(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetInt32(2),
                                    reader.GetInt32(4),
                                    reader.GetDouble(3),
                                    reader.GetDouble(5),
                                    reader.GetDateTime(6),
                                    reader.GetDateTime(7)
                                    ));

                            }
                        }
                    }

                }

                foreach (Debt debt in debts)
                {
                    using (OracleCommand command = new OracleCommand("ADMIN.CalculateTotalDebt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры в команду
                        command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                        command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        totalDebt = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                    }


                    using (OracleCommand command = new OracleCommand("ADMIN.CalculatePaidDebt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры в команду
                        command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                        command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        paidSum = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                    }

                    debt.SetCalulatedDebt(paidSum, totalDebt);
                }

                return debts;

            }
            catch
            {
                return null;
            }
        }

        public Debt GetById(int id)
        {
            Debt debt = new Debt();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetDebtByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_debtID", OracleDbType.Int32).Value = id;
                    command.Parameters.Add("debtsCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Выполняем процедуру
                    //command.ExecuteNonQuery();

                    // Получаем результат из курсора
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                debt = new Debt(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetInt32(2),
                                    reader.GetInt32(4),
                                    reader.GetDouble(3),
                                    reader.GetDouble(5),
                                    reader.GetDateTime(6),
                                    reader.GetDateTime(7)
                                    );

                            }
                        }
                    }

                }

                    using (OracleCommand command = new OracleCommand("ADMIN.CalculateTotalDebt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры в команду
                        command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                        command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        totalDebt = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                    }


                    using (OracleCommand command = new OracleCommand("ADMIN.CalculatePaidDebt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры в команду
                        command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                        command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        paidSum = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                    }

                    debt.SetCalulatedDebt(paidSum, totalDebt);

                return debt;

            }
            catch
            {
                return null;
            }
        }
        public double GetTotalDebt()
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateTotalDebts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("totalDebts", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    return (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebts"].Value;
                }

            }
            catch
            {
                return 0;
            }
        }

        public double GetPaid()
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculatePaidDebts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("totalDebts", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    return (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebts"].Value;
                }

            }
            catch
            {
                return 0;
            }
        }


        public void Add(Debt debt)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddDebt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_nameDebt", OracleDbType.NVarchar2).Value = debt.Name;
                    command.Parameters.Add("p_amountPayment", OracleDbType.Decimal).Value = debt.AmountPayment;
                    command.Parameters.Add("p_numberPayments", OracleDbType.Int32).Value = debt.NumberPayments;
                    command.Parameters.Add("p_percent", OracleDbType.Decimal).Value = debt.Percent;
                    command.Parameters.Add("p_receivingDate", OracleDbType.Date).Value = debt.ReceivingDate;
                    command.Parameters.Add("p_maturityDate", OracleDbType.Date).Value = debt.MaturityDate;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;

                    command.ExecuteNonQuery();

                }

                using (OracleCommand command = new OracleCommand("ADMIN.CalculateTotalDebt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                    command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    totalDebt = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                }


                using (OracleCommand command = new OracleCommand("ADMIN.CalculatePaidDebt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("debtID", OracleDbType.Int32).Value = debt.ID;
                    command.Parameters.Add("totalDebt", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    paidSum = (double)(OracleDecimal)(double)(OracleDecimal)command.Parameters["totalDebt"].Value;
                }

                debt.SetCalulatedDebt(paidSum, totalDebt);
            }
            catch
            {
                return;
            }
        }

        public void Set(Debt debt)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AlterDebt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_debtID", OracleDbType.Int32).Value = debt.ID;
                    command.Parameters.Add("p_nameDebt", OracleDbType.NVarchar2).Value = debt.Name;
                    command.Parameters.Add("p_paid", OracleDbType.Int32).Value = debt.Paid;
                    command.Parameters.Add("p_amountPayment", OracleDbType.Decimal).Value = debt.AmountPayment;
                    command.Parameters.Add("p_numberPayments", OracleDbType.Int32).Value = debt.NumberPayments;
                    command.Parameters.Add("p_percent", OracleDbType.Decimal).Value = debt.Percent;
                    command.Parameters.Add("p_receivingDate", OracleDbType.Date).Value = debt.ReceivingDate;
                    command.Parameters.Add("p_maturityDate", OracleDbType.Date).Value = debt.MaturityDate;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }
        public void Delete(int id)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.DeleteDebt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_debtID", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        public DebtDB(OracleConnection connection)
        {
            this.connection = connection;
        }
    }
}
