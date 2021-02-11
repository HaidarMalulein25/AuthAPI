using MySql.Data.MySqlClient;

namespace UserAuthentication.Classes
{
    public abstract class MySQLDappper 
    {
        public static string ConnectionString = "SERVER=127.0.0.1;PORT=3306;user=authdb;password=WwmVfmZDxtFu9xWZ;database=authdb";
        private static MySqlConnection connection;
        private void OpenConnection()
        {
            connection = new MySqlConnection(ConnectionString);
        }
        public static MySqlConnection GetMYSQLConnection()
        {
            connection = new MySqlConnection(ConnectionString);
            return connection;
        }
        public static int ExecuteMySQLQuery(string query)
        {
            int rowsaffected = 0;
            MySqlConnection connection = GetMYSQLConnection();
            MySqlCommand myCommand = new MySqlCommand(query, GetMYSQLConnection());
            myCommand.Connection.Open();
            rowsaffected= myCommand.ExecuteNonQuery();
            connection.Close();
            return rowsaffected;
        }
    }
}
