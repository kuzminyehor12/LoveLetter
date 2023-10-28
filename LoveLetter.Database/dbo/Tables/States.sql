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

