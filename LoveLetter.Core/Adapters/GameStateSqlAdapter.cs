using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;

namespace LoveLetter.Core.Adapters
{
    public class GameStateSqlAdapter : ISqlDataAdapter
    {
        public SqlConnection Connection { get; }

        public GameStateSqlAdapter()
        {
            Connection = new SqlConnection(ConfigurationUtils.GetConnectionString());
        }

        public GameStateSqlAdapter(SqlConnection connection)
        {
            Connection = connection;
        }

        public DomainEntity Populate(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;

                using (var reader = cmd.ExecuteReader())
                {
                    return new GameState(reader);
                }
            }
        }

        public void SaveChanges(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
