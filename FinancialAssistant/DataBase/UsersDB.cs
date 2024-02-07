using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Types;
using Financial_assistant.Classes;
using Financial_assistant.Pages;
using System.IO;
using System.Windows.Media.Imaging;

namespace Financial_assistant.DataBase
{
    public class UsersDB
    {
        OracleConnection connection;

        public bool Loggin(string login, string password)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetUserByLoginAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = login;
                    command.Parameters.Add("p_password", OracleDbType.NVarchar2).Value = password;
                    command.Parameters.Add("userCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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
                                    
                                    Account.Photo = bitmapImage;
                                }
                                catch(Exception ex)
                                {
                                    Account.Photo = null;
                                }
                                Account.ID = reader.GetInt32(0);
                                Account.Name = reader.GetString(3);
                                Account.UserRole = (Role)reader.GetInt32(5);
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void Register(string login, string name, string password)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = login;
                    command.Parameters.Add("p_password", OracleDbType.NVarchar2).Value = password;
                    command.Parameters.Add("p_name", OracleDbType.NVarchar2).Value = name;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = null;
                    command.Parameters.Add("p_role", OracleDbType.Int16).Value = 0;


                    command.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20004)
                    throw ex; 
            }
            catch (Exception ex)
            {
                
            }
        }

        public bool Add(User user)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AddUser", connection))
                {
                    byte[] imageBytes;
                    MemoryStream stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(user.Picture));
                        encoder.Save(stream);
                        imageBytes = stream.ToArray();
                    }
                    catch
                    {
                        imageBytes = null;
                    }


                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = user.Login;
                    command.Parameters.Add("p_password", OracleDbType.NVarchar2).Value = user.Password;
                    command.Parameters.Add("p_name", OracleDbType.NVarchar2).Value = user.Name;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;
                    command.Parameters.Add("p_role", OracleDbType.Int16).Value = (short)user.UserRole;


                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public void Delete(int id)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.DeleteUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                    return;
                }
            }
            catch
            {
                return;
            }
        }


        public User GetById(int id)
        {
            User user = new User();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetUserByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_UserID", OracleDbType.Int32).Value = id;
                    command.Parameters.Add("userCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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

                                    user = new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), bitmapImage, reader.GetString(2), (Role)reader.GetInt16(5));
                                }
                                catch
                                {
                                    user = new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), null, reader.GetString(2), (Role)reader.GetInt16(5));
                                }
                            }
                            return user;
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

        public User GetByLogin(string login)
        {
            User user = new User();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetUserByLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = login;
                    command.Parameters.Add("userCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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
                                    user = new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), bitmapImage, reader.GetString(2), (Role)reader.GetInt16(5));
                                }
                                catch
                                {
                                    user = new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), null, reader.GetString(2), (Role)reader.GetInt16(5));
                                }
                            }
                            return user;
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
        public List<User> Get()
        {
            List<User> users = new List<User>();
            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры в команду
                    command.Parameters.Add("userCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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
                                    users.Add(new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), bitmapImage, reader.GetString(2), (Role)reader.GetInt16(5)));

                                }
                                catch
                                {
                                    users.Add(new User(reader.GetInt32(0), reader.GetString(3), reader.GetString(1), null, reader.GetString(2), (Role)reader.GetInt16(5)));
                                }
                            }
                            return users;
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

        public void Set(User user, bool isChangePassword = true)
        {
            if (isChangePassword)
            {
                try
                {
                    using (OracleCommand command = new OracleCommand("ADMIN.AlterUser", connection))
                    {
                        MemoryStream stream = new MemoryStream();
                        BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                        encoder.Frames.Add(BitmapFrame.Create(user.Picture));
                        encoder.Save(stream);

                        byte[] imageBytes = stream.ToArray();


                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_id", OracleDbType.Int32).Value = user.ID;
                        command.Parameters.Add("p_name", OracleDbType.NVarchar2).Value = user.Name;
                        command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = user.Login;
                        command.Parameters.Add("p_password", OracleDbType.NVarchar2).Value = user.Password;
                        command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;
                        command.Parameters.Add("p_role", OracleDbType.Int16).Value = (short)user.UserRole;

                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    using (OracleCommand command = new OracleCommand("ADMIN.AlterUserWithoutPassword", connection))
                    {
                        MemoryStream stream = new MemoryStream();
                        BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                        encoder.Frames.Add(BitmapFrame.Create(user.Picture));
                        encoder.Save(stream);

                        byte[] imageBytes = stream.ToArray();


                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_id", OracleDbType.Int32).Value = user.ID;
                        command.Parameters.Add("p_name", OracleDbType.NVarchar2).Value = user.Name;
                        command.Parameters.Add("p_login", OracleDbType.NVarchar2).Value = user.Login;
                        command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;
                        command.Parameters.Add("p_role", OracleDbType.Int16).Value = (short)user.UserRole;

                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                }
            }
        }

        public void Edit(int id)
        {

            try
            {
                using (OracleCommand command = new OracleCommand("ADMIN.AlterUserByNameAndPicture", connection))
                {
                    MemoryStream stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий энкодер в зависимости от формата изображения
                    encoder.Frames.Add(BitmapFrame.Create(Account.Photo));
                    encoder.Save(stream);

                    byte[] imageBytes = stream.ToArray();


                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_id", OracleDbType.Int32).Value = Account.ID;
                    command.Parameters.Add("p_name", OracleDbType.NVarchar2).Value = Account.Name;
                    command.Parameters.Add("p_picture", OracleDbType.Blob).Value = imageBytes;

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }


        public UsersDB(OracleConnection connection)
        {
            this.connection = connection;
        }
    }
}
