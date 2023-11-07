using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoveLetter.Core.Adapters
{
    public class GameStateSqlAdapter : DomainSqlDataAdapter
    {
        public GameStateSqlAdapter() : base()
        {

        }

        public GameStateSqlAdapter(SqlConnection connection) : base(connection)
        {

        }

        public override DomainEntity Populate(string command)
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
    }
}
