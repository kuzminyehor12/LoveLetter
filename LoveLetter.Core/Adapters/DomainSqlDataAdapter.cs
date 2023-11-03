using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoveLetter.Core.Adapters
{
    public abstract class DomainSqlDataAdapter
    {
        public SqlConnection Connection { get; }

        public DomainSqlDataAdapter()
        {
            Connection = new SqlConnection(ConfigurationUtils.GetConnectionString());
        }

        public DomainSqlDataAdapter(SqlConnection connection)
        {
            Connection = connection;
        }


        public abstract DomainEntity Populate(string command);

        public void SaveChanges(string command, SqlTransaction? transaction = null)
        {
            using (var cmd = Connection.CreateCommand())
            {
                if (transaction is not null)
                {
                    cmd.Transaction = transaction;
                }
                
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
        }

        public void DoAsTransaction(Action<SqlTransaction> action, IsolationLevel isolationLevel)
        {
            using (var transaction = Connection.BeginTransaction(isolationLevel))
            {
                try
                {
                    action(transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
