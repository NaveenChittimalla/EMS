using EMS.CoreLibrary.Helpers;
using EMS.CoreLibrary.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EMS.CoreLibrary.Repositories
{
    public class DepartmentSqlDbRepository
    {
        private readonly string _connectionString;

        public DepartmentSqlDbRepository()
        {
            _connectionString = "Data Source=LocalHost\\SQLEXPRESS; Initial Catalog=EMS; Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";
        }

        public bool Create(Department department)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Name", department.Name);
            parameters.Add("Active", department.Active);

            int newId = Convert.ToInt32(SqlHelper.ExecuteScalar(_connectionString, "Department_Insert", parameters));
            //Set the id and employeecode to employee parameter of this method 
            department.Id = newId;
            return newId > 0;

            #region using statement approach
            /*
             //Create Connection object
             //using statement will automatically close or dispose connection while exiting.
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "Employee_Insert";

                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Active", employee.Active);

                //Open the database connection 
                connection.Open();

                //Execute Command
                int newEmployeeId = Convert.ToInt32(command.ExecuteScalar());

                result = newEmployeeId > 0;

                
                    // // using statement will automatically close or dispose connection while exiting.
                    // // So explicitly closing not required when wrapped with using statement.
               
                //connection.Close();

                //Set the id and employeecode to employee parameter of this method 
                employee.Id = newEmployeeId;
                employee.EmployeeCode = $"EMS{employee.Id}";
            }
                return result;
            */
            #endregion
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public Department GetById(int id)
        {
            Department department = null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);

            DataSet dsDepartment = SqlHelper.ExecuteDataSet(_connectionString, "Department_GetById", parameters);

            if (dsDepartment?.Tables.Count > 0
                && dsDepartment?.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsDepartment.Tables[0].Rows[0];
                department = new Department();
                department.Id = Convert.ToInt32(dr.Field<int>("Id"));
                department.Code = dr.Field<string>("Code").ToString();
                department.Name = dr.Field<string>("Name").ToString();
                department.Active = dr.Field<bool>("Active");
            }
            return department;

            #region using statementApproach
            /*
            //open Connection
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "Employee_GetById";

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


                ////using statement will automatically close or dispose connection while exiting.
                ////So explicitly closing not required when wrapped with using statement.
                //connection.Close();

            }
            return employee;
            */
            #endregion
        }

        /// Gets all the employee details
        /// </summary>
        public IEnumerable<Department> GetAll()
        {
            List<Department> departmentList = new List<Department>();

            DataSet dsDepartment = SqlHelper.ExecuteDataSet(_connectionString, "Department_GetAll", null); //pass null when no parameters

            if (dsDepartment != null && dsDepartment.Tables.Count == 1)
            {
                foreach (DataRow dr in dsDepartment.Tables[0].Rows)
                {
                    Department department = new Department();
                    department.Id = Convert.ToInt32(dr.Field<int>("Id"));
                    department.Code = dr.Field<string>("Code").ToString();
                    department.Name = dr.Field<string>("Name").ToString();
                    department.Active = dr.Field<bool>("Active");

                    departmentList.Add(department);
                }
            }

            return departmentList;

            #region using statement approach

            /*
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "Employee_GetAll";

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

                ///Disconnected-Oriented approach
                SqlDataAdapter sd = new SqlDataAdapter(command);
                sd.Fill(dsEmployee);

               
               //using statement will automatically close or dispose connection while exiting.
               //So explicitly closing not required when wrapped with using statement.
               
                //connection.Close();
            }
            return employeeList;
            */
            #endregion
        }

        /// <summary>
        /// Updates the employee details of a specified Id
        /// </summary>
        /// <returns></returns>
        public bool Update(int id, Department department)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", (department.Id == 0 ? id : department.Id));
            parameters.Add("Name", department.Name);
            parameters.Add("Active", department.Active);

            var result = SqlHelper.ExecuteNonQuery(_connectionString, "Department_Update_Details", parameters);
            return result > 0;

            #region using statement approach
            /*
               //bool result;

               ////Create Connection object
               //using (SqlConnection connection = new SqlConnection(_connectionString))
               //{
               //    //Create Command object with SQL command
               //    SqlCommand command = new SqlCommand();
               //    command.Connection = connection;
               //    command.CommandType = CommandType.StoredProcedure;
               //    command.CommandText = "Employee_Update_Details";

               //    employee.Id = id;
               //    command.Parameters.AddWithValue("@Id", (employee.Id == 0 ? id : employee.Id));
               //    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
               //    command.Parameters.AddWithValue("@LastName", employee.LastName);
               //    command.Parameters.AddWithValue("@Email", employee.Email);
               //    command.Parameters.AddWithValue("@Active", employee.Active);

               //    //Open the database connection 
               //    connection.Open();

               //    //Execute Command
               //    int recordCount = command.ExecuteNonQuery();

               //    result = recordCount > 0;

               //    ////using statement will automatically close or dispose connection while exiting.
               //    ////So explicitly closing not required when wrapped with using statement.
               //    //connection.Close();
               //}
               //return result;
               */
            #endregion
        }

        public bool Delete(int id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);

            var result = SqlHelper.ExecuteNonQuery(_connectionString, "Department_DeleteById", parameters);
            return result > 0;

            #region using statment approach
            /*
            bool result;
            //Create Connection object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Create Command object with SQL command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "Employee_DeleteById";

                command.Parameters.AddWithValue("@Id", id);

                //Open the database connection 
                connection.Open();

                //Execute Command
                int recordCount = command.ExecuteNonQuery();
                result = recordCount > 0;


                ////using statement will automatically close or dispose connection while exiting.
                ////So explicitly closing not required when wrapped with using statement.
                //connection.Close();
            }
            return result; 
            */
            #endregion
        }

        /// <summary>
        /// Gets the employee details by Id
        /// </summary>
        public bool Exists(int id)
        {
            string query = $@"SELECT TOP 1 Id
                               FROM Department
                               WHERE id=@Id";

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);

            var result = SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, query, parameters);
            return result is not null && Convert.ToInt32(result) > 0;

            #region using statement approach
            /*
               bool result;
               //open Connection
               using (SqlConnection connection = new SqlConnection(_connectionString))
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

                   ////using statement will automatically close or dispose connection while exiting.
                   ////So explicitly closing not required when wrapped with using statement.

                   //connection.Close();
               }
               return result;
               */
            #endregion
        }
    }
}
