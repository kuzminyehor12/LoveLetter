using LoveLetter.Core.Queries;
using LoveLetter.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoveLetter.UI.Infrastructure
{
    public static class DataGridUtils
    {
        public static DataTable GetLobbyDataTable(SqlConnection connection)
        {
            DataTable lobbies = new DataTable();

            using (var dataAdapter = new SqlDataAdapter())
            {
                dataAdapter.SelectCommand = new SqlCommand(LobbyQuery.SelectAll());
                connection.Open();
                dataAdapter.Fill(lobbies);
            }

            return lobbies;
        }

        public static DataTable LoadAudit(Guid gameStateId, SqlConnection connection)
        {
            DataTable audit = new DataTable();

            using (var dataAdapter = new SqlDataAdapter())
            {
                dataAdapter.SelectCommand = new SqlCommand(AuditQuery.SelectAll(gameStateId));
                connection.Open();
                dataAdapter.Fill(audit);
            }

            return audit;
        }
    }
}
