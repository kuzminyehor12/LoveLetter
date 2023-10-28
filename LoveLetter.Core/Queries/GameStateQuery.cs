using LoveLetter.Core.Constants;

namespace LoveLetter.Core.Queries
{
    public static class GameStateQuery
    {
        public static string SelectById(Guid lobbyId) => $"SELECT * FROM {Tables.States} WHERE Id={lobbyId}";

        public static string UpdateColumn<T>(Guid lobbyId, T column) =>
            $"UPDATE {Tables.States} SET {nameof(column)}={column} WHERE Id={lobbyId}";

        public static string UpdateColumns(Guid lobbyId, params string[] columns)
        {
            var query = $"UPDATE {Tables.States} SET ";

            foreach (var column in columns)
            {
                query += $"{nameof(column)}={column},";
            }

            query = query.TrimEnd(',');

            query += $" WHERE Id={lobbyId}";
            return query;
        }
    }
}
