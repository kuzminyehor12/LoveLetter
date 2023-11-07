CREATE TABLE [dbo].[Audit] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [GameStateId]    UNIQUEIDENTIFIER NOT NULL,
    [PlayerNickname] NVARCHAR (MAX)   NOT NULL,
    [PlayerNumber]   SMALLINT         NOT NULL,
    [Message]        NVARCHAR (MAX)   NOT NULL,
    [Timestamp]      DATETIME2 (7)    DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StateAuditItems] FOREIGN KEY ([GameStateId]) REFERENCES [dbo].[States] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Audit_GameStateId]
    ON [dbo].[Audit]([GameStateId] ASC);

