CREATE TABLE [dbo].[IncomingTrades]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[MessageId] NVARCHAR(100) NOT NULL,
	[ReceivedAt] DATETIME2 NOT NULL,
    [ProcessedAt] DATETIME2 NULL,
	[Party1Lei] NVARCHAR(200) NULL,
	[Party2Lei] NVARCHAR(200) NULL,
	[Party1Id] NVARCHAR(50) NULL,
	[Party2Id] NVARCHAR(50) NULL,
	[Fpml] XML NULL,
	[IsFpmlValid] BIT NULL
)

GO

CREATE INDEX [IX_IncomingTrades_MessageId] ON [dbo].[IncomingTrades] ([MessageId])
