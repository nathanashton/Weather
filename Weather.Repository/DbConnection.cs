using System.Data;
using System.Data.SQLite;

namespace Weather.Repository
{
    public class DbConnection
    {
        private readonly SQLiteConnection _connection;

        public DbConnection()
        {
            _connection = new SQLiteConnection { ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;" };
        }

        public SQLiteConnection Connect()
        {
            if (_connection.State != ConnectionState.Closed && _connection.State != ConnectionState.Broken)
                return _connection;
            try
            {
                _connection.Open();
            }
            catch
            {
                throw;
            }
            finally
            {
            }
            return _connection;
        }
    }
}