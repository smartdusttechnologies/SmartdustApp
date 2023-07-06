CREATE TABLE [dbo].[Contact] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50)  NOT NULL,
    [Mail]    VARCHAR (100) NOT NULL,
    [Subject] VARCHAR (200) NOT NULL,
    [Message] VARCHAR (400) NOT NULL,
    [Phone]   INT           NOT NULL,
    [Address] VARCHAR (200) NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC)
);

