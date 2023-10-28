CREATE TABLE [dbo].[Lobbies] (
    [Id]      UNIQUEIDENTIFIER NOT NULL,
    [Status]  SMALLINT         NOT NULL,
    [Players] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Lobbies] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Lobbies_Status_Enum] CHECK ([Status]=CONVERT([smallint],(2)) OR [Status]=CONVERT([smallint],(1)) OR [Status]=CONVERT([smallint],(0)))
);

