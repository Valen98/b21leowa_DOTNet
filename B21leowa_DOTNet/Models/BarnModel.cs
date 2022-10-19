using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace B21leowa_DOTNet.Models
{
    public class BarnModel
    {
        private IConfiguration _configuration;
        private string _connectionString;
        public BarnModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllChildren()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM barn;", connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable barnTable = ds.Tables["result"];
            connection.Close();
            return barnTable != null ? barnTable : new DataTable();
        }

        public void InsertChild(string PNR, string firstname, string surname, string birthday, int kindnessScale, string pwd)
        {
            string fullname =  firstname + " " + surname;
            var birthdayDateAndTime = Convert.ToDateTime(birthday);
            var birthdayDate = birthdayDateAndTime.Date;

            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            string insertChild = "INSERT INTO barn(PNR, namn, födelseår, snällhetsSkala, pwd) VALUES (@PNR, @name, @birthday, @kindnessScale, @pwd);";
            MySqlCommand sqlCmd = new MySqlCommand(insertChild, connection);
            sqlCmd.Parameters.AddWithValue("@PNR", PNR);
            sqlCmd.Parameters.AddWithValue("@name", fullname);
            sqlCmd.Parameters.AddWithValue("@birthday", birthdayDate);
            sqlCmd.Parameters.AddWithValue("@kindnessScale", kindnessScale);
            sqlCmd.Parameters.AddWithValue("@pwd", pwd);
            int rows = sqlCmd.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteChild(string PNR, string name)
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            string deleteChild = "DELETE FROM barn WHERE PNR = @PNR AND namn = @name;";
            MySqlCommand sqlCmd = new MySqlCommand(deleteChild, connection);
            sqlCmd.Parameters.AddWithValue("@PNR", PNR);
            sqlCmd.Parameters.AddWithValue("@name", name);
            int rows = sqlCmd.ExecuteNonQuery();
            connection.Close();
        }

        public DataTable SearchChildren(string name) 
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            MySqlDataAdapter sqlCmd = new MySqlDataAdapter("SELECT * FROM barn WHERE namn LIKE @name;", connection);
            sqlCmd.SelectCommand.Parameters.AddWithValue("@name", "%" + name + "%");
            DataSet ds = new DataSet();
            sqlCmd.Fill(ds, "result");
            DataTable searchChildrenTable = ds.Tables["result"];
            connection.Close();
            return searchChildrenTable != null ? searchChildrenTable : new DataTable();
        }
    }
}
