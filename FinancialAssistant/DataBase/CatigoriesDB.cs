using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Types;
using Financial_assistant.Pages;
using System.IO;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices.ComTypes;
using Financial_assistant.Classes;

namespace Financial_assistant.DataBase
{
    public class CatigoriesDB
    {
        OracleConnection connection;

        public ObservableCollection<Categories> Get(TypeAdditon type)
        {
            ObservableCollection<Categories> categories = new ObservableCollection<Categories>();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetCategoriesByType", connection))
                {
                   

                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)type;
                    command.Parameters.Add("categoriesCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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
                                    OracleBlob blob = reader.GetOracleBlob(3);
                                    byte[] imageBytes = new byte[blob.Length];
                                    blob.Read(imageBytes, 0, (int)blob.Length);

                                    MemoryStream stream = new MemoryStream(imageBytes);
                                    BitmapImage bitmapImage = new BitmapImage();
                                    bitmapImage.BeginInit();
                                    bitmapImage.StreamSource = stream;
                                    bitmapImage.EndInit();

                                    categories.Add(new Categories(reader.GetInt32(0), reader.GetString(1), (TypeAdditon)reader.GetInt16(2), bitmapImage));
                                }
                                catch
                                {
                                    categories.Add(new Categories(reader.GetInt32(0), reader.GetString(1), (TypeAdditon)reader.GetInt16(2), null));
                                }
                            }   
                            return categories;
                        }
                        else
                        {
                            return categories;
                        }
                    }
                    return categories;
                }
            }
            catch
            {
                return categories;
            }
        }

        public void Add(Categories category)
        {
           
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddCategory", connection))
                {
                    byte[] imageBytes;
                    MemoryStream stream = new MemoryStream();

                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(category.Picture));
                        encoder.Save(stream);

                        imageBytes = stream.ToArray();
                    }
                    catch
                    {
                        imageBytes = null;
                    }

                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_categoryName", OracleDbType.NVarchar2).Value = category.Name;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)category.Type;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;

                    command.ExecuteNonQuery();

                }
            }
            catch
            {
                return;
            }
        }

        public void Set(Categories category)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AlterCategory", connection))
                {
                    byte[] imageBytes;
                    MemoryStream stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(category.Picture));
                        encoder.Save(stream);

                        imageBytes = stream.ToArray();
                    }
                    catch
                    {
                        imageBytes = null;
                    }

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_categoryID", OracleDbType.Int32).Value = category.ID;
                    command.Parameters.Add("p_categoryName", OracleDbType.NVarchar2).Value = category.Name;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)category.Type;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;

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
                using (OracleCommand command = new OracleCommand("ADMIN.DeleteCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_categoryID", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        public double GetExpensesByCategory(DateTime StartDate, DateTime EndDate, int categoryID, out double percent)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateSavesByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_categoryID", OracleDbType.Int32).Value = categoryID;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)TypeAdditon.EXPENSES;

                    command.Parameters.Add("sumSavesOfCategory", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    command.Parameters.Add("partSaves", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    percent = (double)(OracleDecimal)command.Parameters["partSaves"].Value;
                    return (double)(OracleDecimal)command.Parameters["sumSavesOfCategory"].Value;
                }
            }
            catch
            {
                percent = 0;
                return 0;
            }
        }

        public double GetIncomesByCategory(DateTime StartDate, DateTime EndDate, int categoryID, out double percent)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.CalculateSavesByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("startDate", OracleDbType.Date).Value = StartDate;
                    command.Parameters.Add("endDate", OracleDbType.Date).Value = EndDate;
                    command.Parameters.Add("p_categoryID", OracleDbType.Int32).Value = categoryID;
                    command.Parameters.Add("p_userID", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("p_type", OracleDbType.Int16).Value = (short)TypeAdditon.INCOME;

                    command.Parameters.Add("sumSavesOfCategory", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                    command.Parameters.Add("partSaves", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    percent = (double)(OracleDecimal)command.Parameters["partSaves"].Value;
                    return (double)(OracleDecimal)command.Parameters["sumSavesOfCategory"].Value;
                }
            }
            catch
            {
                percent = 0;
                return 0;
            }
        }

        public CatigoriesDB(OracleConnection connection)
        {
            this.connection = connection;
        }
    }
}
