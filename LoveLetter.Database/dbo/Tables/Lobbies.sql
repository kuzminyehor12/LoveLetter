CREATE TABLE [dbo].[Lobbies] (
    [Id]      UNIQUEIDENTIFIER NOT NULL,
    [Status]  SMALLINT         NOT NULL,
    [Players] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Lobbies] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Lobbies_Status_Enum] CHECK ([Status]=CONVERT([smallint],(2)) OR [Status]=CONVERT([smallint],(1)) OR [Status]=CONVERT([smallint],(0)))
);


GO
CREATE TRIGGER [dbo].[OnGameStarted_TRG]
ON [dbo].[Lobbies]
AFTER UPDATE
AS
BEGIN
    IF UPDATE([Status])
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM inserted AS i
            WHERE i.[Status] = 1
        )
        BEGIN
            DECLARE @id uniqueidentifier
			DECLARE @nicknames nvarchar(MAX)
			DECLARE @playersEnumeration nvarchar(MAX)
			DECLARE @count smallint

            SELECT @id = i.[Id], @nicknames = i.[Players]
            FROM inserted AS i
            
			EXECUTE EnumeratePlayers_SP @InputNVarChar = @nicknames, @Delimiter = N',', @OutputNVarChar = @playersEnumeration OUTPUT, @ElementCount = @count OUTPUT
            EXECUTE GenerateDeck_SP @lobbyId = @Id, @players = @playersEnumeration, @playersCount = @count
        END
    END
END;