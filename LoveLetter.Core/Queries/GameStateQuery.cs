using LoveLetter.Core.Constants;

namespace LoveLetter.Core.Queries
{
    public static class GameStateQuery
    {
        public static string SelectById(Guid lobbyId) => $"SELECT * FROM {Tables.States} WHERE Id={lobbyId}";

        public static string UpdateColumn(Guid lobbyId, (string ColumnName, string ColumnValue) column) =>
            $"UPDATE {Tables.States} SET {column.ColumnName}={column.ColumnValue} WHERE Id={lobbyId}";

        public static string UpdateColumns(Guid lobbyId, params (string ColumnName, string ColumnValue)[] columns)
        {
            var query = $"UPDATE {Tables.States} SET ";

            foreach (var column in columns)
            {
                query += $"{column.ColumnName}={column.ColumnValue},";
            }

            query = query.TrimEnd(',');

            query += $" WHERE Id={lobbyId}";
            return query;
        }
    }
}
