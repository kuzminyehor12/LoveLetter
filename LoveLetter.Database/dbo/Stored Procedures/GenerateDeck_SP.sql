CREATE PROCEDURE [dbo].[GenerateDeck_SP]
	@lobbyId uniqueidentifier,
	@players nvarchar(MAX),
	@playersCount tinyint
AS
	DECLARE @deck nvarchar(MAX);
	EXEC dbo.ShuffleArray_SP '1 1 1 1 1 2 2 3 3 4 4 5 5 6 7 8', @deck OUTPUT;
	SELECT @deck;

	DECLARE @turnPlayerNumber tinyint
	EXEC [GenerateRandomTurnPlayerNumber_SP] @MaxValue = @playersCount
	SELECT @turnPlayerNumber

	INSERT INTO States(Id, Players, Deck, TurnPlayerNumber, StartDate)
    VALUES (@lobbyId, @players, @deck, @turnPlayerNumber, GETDATE());
RETURN 0