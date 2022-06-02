using System;
using MySql.Data.MySqlClient;

namespace Model
{
    public class MySqlDatabase : IDisposable
    {
        public readonly object Users;
        public MySqlConnection Connection;

        public MySqlDatabase(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
            this.Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}