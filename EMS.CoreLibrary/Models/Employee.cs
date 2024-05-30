using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EMS.CoreLibrary.Models
{
    public class Employee : BaseModel
    {
        public string? EmployeeCode { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "FirstName should be of minimum 5 and maximum 250 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "LastName should be of minimum 5 and maximum 250 characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "Email should be of minimum 5 and maximum 250 characters.")]
        [EmailAddress]
        public string Email { get; set; }

        public bool? Active { get; set; }

        #region Methods
        public bool Create()
        {
            //Create Connection object
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            //Create Command object with SQL command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            string query = $@"INSERT INTO Employee
                                (
                                    FirstName,
                                    LastName,
                                    Email,
                                    Active
                                ) 
                                values
                                (
                                    '{FirstName}',
                                    '{LastName}',
                                    '{Email}',
                                    '{Active}'
                                );
                                DECLARE @NewEmployeeId INT
                                SET @NewEmployeeId = @@IDENTITY
                                UPDATE Employee
                                SET EmployeeCode = CONCAT('EMS', @NewEmployeeId)
                                WHERE id = @NewEmployeeId;
                                
                                SELECT @NewEmployeeId
                                ";
            command.CommandText = query;

            //Open the database connection 
            connection.Open();

            //Execute Command
            int newEmployeeId = Convert.ToInt32(command.ExecuteScalar());
            //Close the database connection
            connection.Close();

            Id = newEmployeeId;
            EmployeeCode = $"EMS{Id}";

            return newEmployeeId > 0;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public void Get()
        {
            //open Connection
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            string query = $@"SELECT
                                EmployeeCode,
                                FirstName,
                                LastName,
                                Email,
                                Active
                            FROM Employee
                            WHERE id=@Id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", Id);

            connection.Open();

            SqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.Read())
            {
                EmployeeCode = dataReader.GetValue(0).ToString();
                FirstName = dataReader.GetValue("FirstName").ToString();
                LastName = dataReader.GetValue("LastName").ToString();
                Email = dataReader.GetValue("Email").ToString();
                Active = Convert.ToBoolean(dataReader.GetValue("Active"));
            }

            dataReader.Close();
            connection.Close();
        }

        /// Gets all the employee details
        /// </summary>
        public IEnumerable<Employee> GetAll()
        {
            List<Employee> employeeList = new();
            //open Connection
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

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
            //    Employee employee = new Employee();
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
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(dr.Field<int>("Id"));
                    employee.EmployeeCode = dr.Field<string>("EmployeeCode").ToString();
                    employee.FirstName = dr.Field<string>("FirstName").ToString();
                    employee.LastName = dr.Field<string>("LastName").ToString();
                    employee.Email = dr.Field<string>("Email").ToString();
                    employee.Active = dr.Field<bool>("Active");

                    employeeList.Add(employee);
                }
            }

            return employeeList;
        }

        /// <summary>
        /// Updates the employee details of a specified Id
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            //Create Connection object
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            //Create Command object with SQL command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Employee_Update_Details"; // Stored Procedure Name. Refer Database.sql file

            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Active", Active);

            //Open the database connection 
            connection.Open();

            //Execute Command
            int recordCount = command.ExecuteNonQuery();

            //Close the database connection
            connection.Close();

            return recordCount > 0;
        }

        public bool Delete()
        {
            //Create Connection object
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            //Create Command object with SQL command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            string query = $@"  DELETE 
                                FROM Employee
                                WHERE id = {Id};";
            command.CommandText = query;

            //Open the database connection 
            connection.Open();

            //Execute Command
            int recordCount = command.ExecuteNonQuery();

            //Close the database connection
            connection.Close();

            return recordCount > 0;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public bool Exists()
        {
            //open Connection
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            string query = $@"SELECT TOP 1 Id
                            FROM Employee
                            WHERE id=@Id";
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", Id);

            connection.Open();

            var id = command.ExecuteScalar();
            connection.Close();

            return id is not null && Convert.ToInt32(id) > 0;
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public bool ExistsWithEmail()
        {
            //open Connection
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            string query = $@"SELECT TOP 1 Id
                            FROM Employee
                            WHERE Email=@Email";
            command.CommandText = query;
            command.Parameters.AddWithValue("@Email", Email);

            connection.Open();

            var id = command.ExecuteScalar();
            connection.Close();

            return id is not null && Convert.ToInt32(id) > 0;
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

            IEnumerable<Employee> employeeList = GetAll();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Id,EmployeeCode,FirstName,LastName,Email,Active");
                foreach (Employee employee in employeeList)
                {
                    writer.WriteLine($"{employee.Id},{employee.EmployeeCode},{employee.FirstName},{employee.LastName},{employee.Email},{employee.Active}");
                }
            }
            return filePath;
        }
        #endregion
    }
}
