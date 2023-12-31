﻿using LoveLetter.Core.Constants;
using LoveLetter.Core.Utils;

namespace LoveLetter.Core.Queries
{
    public static class LobbyQuery
    {
        public static string SelectAll() => $"SELECT * FROM {Tables.Lobbies} WHERE Status<>{(short)LobbyStatus.Closed}";

        public static string SelectById(Guid lobbyId) => $"SELECT * FROM {Tables.Lobbies} WHERE Id='{lobbyId}'";

        public static string Insert(string player, out Guid id)
        {
            id = Guid.NewGuid();
            return $"INSERT INTO {Tables.Lobbies} (Id, Status, Players) VALUES ('{id}', {(short)LobbyStatus.Open}, '{player}')";
        }

        public static string Start(Guid lobbyId) => $"UPDATE {Tables.Lobbies} SET Status={(short)LobbyStatus.InProgress} WHERE Id='{lobbyId}'";

        public static string Close(Guid lobbyId) => $"UPDATE {Tables.Lobbies} SET Status={(short)LobbyStatus.Closed}, Players='' WHERE Id='{lobbyId}'";

        public static string UpdatePlayers(Guid lobbyId, IEnumerable<string> players) => $"UPDATE {Tables.Lobbies} SET Players='{string.Join(',', players)}' WHERE Id='{lobbyId}'";
    }
}
