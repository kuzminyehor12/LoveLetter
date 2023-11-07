using LoveLetter.Core.Entities;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;

namespace LoveLetter.Core.Adapters
{
    public class LobbySqlAdapter : DomainSqlDataAdapter
    {
        public LobbySqlAdapter() : base()
        {

        }

        public LobbySqlAdapter(SqlConnection connection) : base(connection)
        {

        }

        public override DomainEntity Populate(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;

                using (var reader = cmd.ExecuteReader())
                {
                    return new Lobby(reader);
                }
            }
        }
    }
}
