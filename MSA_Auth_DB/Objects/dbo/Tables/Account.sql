CREATE TABLE [dbo].[Account]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	--Password information
	[Salt] CHAR (16) NOT NULL,
	[Hash] CHAR (64) NOT NULL,
	--Login information. Display name and/or username will be handled by this services clients
	[Email] VARCHAR(64) UNIQUE NOT NULL,
	[AccountType] CHAR(5) DEFAULT USER,
)
