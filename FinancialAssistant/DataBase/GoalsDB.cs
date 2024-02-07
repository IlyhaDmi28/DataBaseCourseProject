using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Windows.Media.Imaging;
using Oracle.ManagedDataAccess.Types;
using Financial_assistant.Classes;

namespace Financial_assistant
{
    public class GoalsDB
    {
        OracleConnection connection;
        public ObservableCollection<Goals> Get()
        {
            ObservableCollection<Goals> goals = new ObservableCollection<Goals>();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetAllGoals", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("goalsCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Выполняем процедуру
                    //command.ExecuteNonQuery();

                    // Получаем результат из курсора
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    OracleBlob blob = reader.GetOracleBlob(4);
                                    byte[] imageBytes = new byte[blob.Length];
                                    blob.Read(imageBytes, 0, (int)blob.Length);

                                    MemoryStream stream = new MemoryStream(imageBytes);
                                    BitmapImage bitmapImage = new BitmapImage();
                                    bitmapImage.BeginInit();
                                    bitmapImage.StreamSource = stream;
                                    bitmapImage.EndInit();

                                    goals.Add(new Goals(reader.GetInt32(0), reader.GetString(1), (double)reader.GetDecimal(2), (double)reader.GetDecimal(3), bitmapImage));
                                }
                                catch
                                {
                                    goals.Add(new Goals(reader.GetInt32(0), reader.GetString(1), (double)reader.GetDecimal(2), (double)reader.GetDecimal(3), null));
                                }
                            }
                            
                        }
                    }
                    return goals;
                }
            }
            catch
            {
                return goals;
            }
        }


        public void Add(Goals goal)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddGoal", connection))
                {
                    byte[] imageBytes;
                    MemoryStream stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(goal.Picture));
                        encoder.Save(stream);
                        imageBytes = stream.ToArray();
                    }
                    catch
                    {
                        imageBytes = null;
                    }

                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_nameGoal", OracleDbType.NVarchar2).Value = goal.Name;
                    command.Parameters.Add("p_accumulated", OracleDbType.Decimal).Value = goal.Accumulated;
                    command.Parameters.Add("p_price", OracleDbType.Decimal).Value = goal.Price;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                return;
            }
        }

        public void Set(Goals goal)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AlterGoal", connection))
                {
                    byte[] imageBytes;
                    MemoryStream stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(goal.Picture));
                        encoder.Save(stream);

                        imageBytes = stream.ToArray();
                    }
                    catch
                    {
                        imageBytes = null;
                    }

                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_goalID", OracleDbType.Int32).Value = goal.ID;
                    command.Parameters.Add("p_nameGoal", OracleDbType.NVarchar2).Value = goal.Name;
                    command.Parameters.Add("p_accumulated", OracleDbType.Decimal).Value = goal.Accumulated;
                    command.Parameters.Add("p_price", OracleDbType.Decimal).Value = goal.Price;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                return;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.DeleteGoal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_goalID", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        public GoalsDB(OracleConnection connection)
        {
            this.connection = connection;
        }
    }
}
