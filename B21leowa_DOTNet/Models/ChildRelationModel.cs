using MySql.Data.MySqlClient;
using System.Data;

namespace B21leowa_DOTNet.Models
{
    public class ChildRelationModel
    {
        private IConfiguration _configuration;
        private string _connectionString;
        
        public ChildRelationModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllChildRelation()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM barnRelation;", connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable childRelationTable = ds.Tables["result"];
            connection.Close();
            return childRelationTable != null ? childRelationTable : new DataTable();
        }

        public void InsertChildRelation( string namePNR1, string namePNR2, string typeOfRelation) 
        {
            if(namePNR1 != namePNR2)
            {
                string[] namePNR1Split = namePNR1.Split(',');
                string[] namePNR2Split = namePNR2.Split(',');
                MySqlConnection connection = new MySqlConnection(_connectionString);
                connection.Open();
                string insertChildRelation = "INSERT INTO barnRelation(PNR1, namn1, PNR2, namn2, typAvRelation) VALUES (@PNR1, @name1, @PNR2, @name2, @typeOfRelation)";
                MySqlCommand sqlCmd = new MySqlCommand(insertChildRelation, connection);
                sqlCmd.Parameters.AddWithValue("@PNR1", namePNR1Split[1]);
                sqlCmd.Parameters.AddWithValue("@name1", namePNR1Split[0]);
                sqlCmd.Parameters.AddWithValue("@PNR2", namePNR2Split[1]);
                sqlCmd.Parameters.AddWithValue("@name2", namePNR2Split[0]);
                sqlCmd.Parameters.AddWithValue("@typeOfRelation", typeOfRelation);
                int rows = sqlCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void DeleteChildRelation(string PNR1, string name1, string PNR2, string name2)
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            string deleteChild = "DELETE FROM barnRelation WHERE PNR1 = @PNR1 AND namn1 = @name1 AND PNR2 = @PNR2 AND namn2 = @name2;";
            MySqlCommand sqlCmd = new MySqlCommand(deleteChild, connection);
            sqlCmd.Parameters.AddWithValue("@PNR1", PNR1);
            sqlCmd.Parameters.AddWithValue("@name1", name1);
            sqlCmd.Parameters.AddWithValue("@PNR2", PNR2);
            sqlCmd.Parameters.AddWithValue("@name2", name2);
            int rows = sqlCmd.ExecuteNonQuery();
            connection.Close();
        }

    }
}
