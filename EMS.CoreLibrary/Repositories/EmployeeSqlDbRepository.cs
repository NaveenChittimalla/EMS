using EMS.CoreLibrary.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EMS.CoreLibrary.Repositories
{
    public class EmployeeSqlDbRepository
    {
        private readonly string _connectionString;

        public EmployeeSqlDbRepository()
        {
            _connectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";
        }

        public bool Create(EmployeeV3 employee)
        {
            bool result;

            //Create Connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    //Create Command object with SQL command
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = @"INSERT INTO Employee (FirstName, LastName, Email, Active) 
                                values (@FirstName, @LastName, @Email, @Active);

                                DECLARE @NewEmployeeId INT
                                SET @NewEmployeeId = @@IDENTITY
                                
                                UPDATE Employee
                                SET EmployeeCode = CONCAT('EMS', @NewEmployeeId)
                                WHERE id = @NewEmployeeId;
                                
                                SELECT @NewEmployeeId";
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Active", employee.Active);

                    //Open the database connection 
                    connection.Open();

                    //Execute Command
                    int newEmployeeId = Convert.ToInt32(command.ExecuteScalar());

                    result = newEmployeeId > 0;

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();

                    //Set the id and employeecode to employee parameter of this method 
                    employee.Id = newEmployeeId;
                    employee.EmployeeCode = $"EMS{employee.Id}";
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return result;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public EmployeeV3 GetById(int id)
        {
            EmployeeV3 employee = null;

            //open Connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = $@"SELECT
                                Id,
                                EmployeeCode,
                                FirstName,
                                LastName,
                                Email,
                                Active
                            FROM Employee
                            WHERE id=@Id";
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        employee = new EmployeeV3();
                        employee.Id = Convert.ToInt32(dataReader.GetValue("Id"));
                        employee.EmployeeCode = dataReader.GetValue("EmployeeCode").ToString();
                        employee.FirstName = dataReader.GetValue("FirstName").ToString();
                        employee.LastName = dataReader.GetValue("LastName").ToString();
                        employee.Email = dataReader.GetValue("Email").ToString();
                        employee.Active = Convert.ToBoolean(dataReader.GetValue("Active"));
                    }

                    dataReader.Close();

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return employee;
        }

        /// Gets all the employee details
        /// </summary>
        public IEnumerable<EmployeeV3> GetAll()
        {
            List<EmployeeV3> employeeList = new List<EmployeeV3>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = @"SELECT 
                                Id,
                                EmployeeCode,
                                FirstName,
                                LastName,
                                Email,
                                Active
                            FROM dbo.Employee";
                    command.CommandText = query;

                    connection.Open();

                    ///Connection-Oriented approach
                    //SqlDataReader dataReader = command.ExecuteReader();

                    //while (dataReader.Read())
                    //{
                    //    EmployeeV3 employee = new EmployeeV3();
                    //    employee.Id = Convert.ToInt32(dataReader.GetValue("Id"));
                    //    employee.EmployeeCode = dataReader.GetValue("EmployeeCode").ToString();
                    //    employee.FirstName = dataReader.GetValue("FirstName").ToString();
                    //    employee.LastName = dataReader.GetValue("LastName").ToString();
                    //    employee.Email = dataReader.GetValue("Email").ToString();
                    //    employee.Active = Convert.ToBoolean(dataReader.GetValue("Active"));

                    //    employeeList.Add(employee);
                    //}

                    //dataReader.Close();
                    //connection.Close();

                    ///Disconnected-Oriented approach
                    DataSet dsEmployee = new();
                    SqlDataAdapter sd = new SqlDataAdapter(command);
                    sd.Fill(dsEmployee);
                    connection.Close();

                    if (dsEmployee != null && dsEmployee.Tables.Count == 1)
                    {
                        foreach (DataRow dr in dsEmployee.Tables[0].Rows)
                        {
                            EmployeeV3 employee = new EmployeeV3();
                            employee.Id = Convert.ToInt32(dr.Field<int>("Id"));
                            employee.EmployeeCode = dr.Field<string>("EmployeeCode").ToString();
                            employee.FirstName = dr.Field<string>("FirstName").ToString();
                            employee.LastName = dr.Field<string>("LastName").ToString();
                            employee.Email = dr.Field<string>("Email").ToString();
                            employee.Active = dr.Field<bool>("Active");

                            employeeList.Add(employee);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return employeeList;
        }

        /// <summary>
        /// Updates the employee details of a specified Id
        /// </summary>
        /// <returns></returns>
        public bool Update(int id, EmployeeV3 employee)
        {
            bool result;

            //Create Connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    //Create Command object with SQL command
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Employee_Update_Details"; // Stored Procedure Name. Refer Database.sql file

                    employee.Id = id;
                    command.Parameters.AddWithValue("@Id", (employee.Id == 0 ? id : employee.Id));
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Active", employee.Active);

                    //Open the database connection 
                    connection.Open();

                    //Execute Command
                    int recordCount = command.ExecuteNonQuery();

                    result = recordCount > 0;

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();

                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return result;
        }

        public bool Delete(int id)
        {
            bool result;

            //Create Connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    //Create Command object with SQL command
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = $@"  DELETE 
                                FROM Employee
                                WHERE id = @Id";
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Id", id);

                    //Open the database connection 
                    connection.Open();

                    //Execute Command
                    int recordCount = command.ExecuteNonQuery();
                    result = recordCount > 0;

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return result;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public bool Exists(int employeeId)
        {
            bool result;

            //open Connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = $@"SELECT TOP 1 Id
                            FROM Employee
                            WHERE id=@Id";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@Id", employeeId);

                    connection.Open();

                    var id = command.ExecuteScalar();
                    result = id is not null && Convert.ToInt32(id) > 0;

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return result;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public bool ExistsWithEmail(string email)
        {
            bool result;

            //open Connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    string query = $@"SELECT TOP 1 Id
                            FROM Employee
                            WHERE Email=@Email";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();

                    var id = command.ExecuteScalar();
                    result = id is not null && Convert.ToInt32(id) > 0;

                    //Not required when wrapped with using statement.
                    //Automatically using statement will close or dispose connection while exiting.
                    //Close the database connection
                    //connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                //finally
                //{
                //    if (connection?.State == ConnectionState.Open)
                //    {
                //        connection.Close();
                //    }
                //}
            }
            return result;
        }

        public string Export(string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                string emsDataDirectory = @"D:\EMS\Data";
                if (!Directory.Exists(emsDataDirectory))
                {
                    Directory.CreateDirectory(emsDataDirectory);
                }
                filePath = $@"{emsDataDirectory}\AllEmployees{DateTime.Now.ToString("ddMMyyyyHHMMss")}.csv";
            }

            IEnumerable<EmployeeV3> employeeList = GetAll();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Id,EmployeeCode,FirstName,LastName,Email,Active");
                foreach (EmployeeV3 employee in employeeList)
                {
                    writer.WriteLine($"{employee.Id},{employee.EmployeeCode},{employee.FirstName},{employee.LastName},{employee.Email},{employee.Active}");
                }
            }
            return filePath;
        }
    }
}
