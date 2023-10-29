CREATE TABLE [dbo].[States] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Players]            NVARCHAR (MAX)   NOT NULL,
    [Deck]               NVARCHAR (MAX)   NOT NULL,
    [TurnPlayerNumber]   SMALLINT         NOT NULL,
    [WinnerPlayerNumber] SMALLINT         NULL,
    [StartDate]          DATETIME2 (7)    NOT NULL,
    [EndDate]            DATETIME2 (7)    NULL,
    CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LobbyState] FOREIGN KEY ([Id]) REFERENCES [dbo].[Lobbies] ([Id]) ON DELETE CASCADE
);


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[OnGameResult_TRG]
   ON  [dbo].[States]
   AFTER UPDATE
AS 
BEGIN
	IF UPDATE([Deck])
    BEGIN
        -- Check if the updated value matches the predefined value
        IF EXISTS (
            SELECT 1
            FROM inserted AS i
            WHERE i.[Deck] = '' OR i.Deck IS NULL
        )
        BEGIN
            DECLARE @MaxPlayerNumber tinyint;
			DECLARE @players XML
			DECLARE @stateId uniqueidentifier

            SELECT @stateId = i.[Id], @players = CAST(i.[Players] AS XML)
            FROM inserted AS i
            
			SELECT @MaxPlayerNumber = (
				SELECT MAX(CAST(playerCardValue AS tinyint))
				FROM [States]
					CROSS APPLY @players.nodes('/Players/Player') AS PlayerNode(PlayerData)
					CROSS APPLY (
				SELECT
					PlayerNode.PlayerData.value('(PlayerNickname)[1]', 'NVARCHAR(MAX)') AS playerNickname,
					PlayerNode.PlayerData.value('(PlayerNumber)[1]', 'INT') AS playerNumber,
					PlayerNode.PlayerData.value('(CurrentCard)[1]', 'INT') AS playerCardValue
				) AS PlayerData
			);
			
			UPDATE [States] SET WinnerPlayerNumber = @MaxPlayerNumber WHERE [Id] = @stateId;
        END
    END
END
