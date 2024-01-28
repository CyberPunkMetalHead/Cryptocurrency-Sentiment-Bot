using Npgsql;
using System.Data;

public class DatabaseService : IDisposable
{
    public NpgsqlConnection connection;
    public NpgsqlTransaction? transaction;

    public DatabaseService(string connectionString)
    {
        connection = new NpgsqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (connection.State != ConnectionState.Closed)
        {
            connection.Close();
        }
    }

    public void BeginTransaction()
    {
        OpenConnection(); // Ensure the connection is open before starting a transaction
        transaction = connection.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (transaction != null)
        {
            transaction.Commit();
            transaction = null;
        }
    }

    public void RollbackTransaction()
    {
        if (transaction != null)
        {
            transaction.Rollback();
            transaction = null;
        }
    }

    // Other methods for executing queries, procedures, etc.

    public void Dispose()
    {
        // Dispose of resources like the connection and transaction.
        connection?.Dispose();
        transaction?.Dispose();
    }
}
