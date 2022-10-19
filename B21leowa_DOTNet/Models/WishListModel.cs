using MySql.Data.MySqlClient;
using System.Data;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace B21leowa_DOTNet.Models
{
    public class WishListModel
    {
        private IConfiguration _configuration;
        private string _connectionString;
        public WishListModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllWishes()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM åretsÖnskelistor " +
                "LEFT JOIN önskelistaBeskrivning ON åretsÖnskelistor.PNR = önskelistaBeskrivning.PNR " +
                "AND åretsÖnskelistor.namn = önskelistaBeskrivning.namn " +
                "AND åretsÖnskelistor.årtal =  önskelistaBeskrivning.årtal;", connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable wishListTable = ds.Tables["result"];
            connection.Close();
            return wishListTable != null ? wishListTable : new DataTable();
        }

        public void DeleteWish(string createdDate, string PNR) 
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            string deleteWish = "DELETE FROM önskelista WHERE PNR = @PNR AND årtal = @createdDate;";
            MySqlCommand sqlCmd = new MySqlCommand(deleteWish, connection);
            sqlCmd.Parameters.AddWithValue("@PNR", PNR);
            sqlCmd.Parameters.AddWithValue("@createdDate", createdDate);
            int rows = sqlCmd.ExecuteNonQuery();
            connection.Close();
        }
        
        public void CreateWish(string namePNRBirthday, string description)
        {
            string[] namePNRBirthDaySplit = namePNRBirthday.Split(',');
            DateTime now = DateTime.Now;
            DateTime dateOnly = now.Date;
            string dateString = dateOnly.ToString("d");
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            string createWish = "CALL skapaÖnskelista(@date, @PNR, @name, @description, @birthday)";
            MySqlCommand sqlCmd = new MySqlCommand(createWish, connection);
            sqlCmd.Parameters.AddWithValue("@date", dateString);
            sqlCmd.Parameters.AddWithValue("@PNR", namePNRBirthDaySplit[1]);
            sqlCmd.Parameters.AddWithValue("@name", namePNRBirthDaySplit[0]);
            sqlCmd.Parameters.AddWithValue("@description", description);
            sqlCmd.Parameters.AddWithValue("@birthday", namePNRBirthDaySplit[2]);
            int rows = sqlCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
