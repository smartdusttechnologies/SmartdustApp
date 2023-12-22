CREATE TABLE [dbo].[DocumentTable] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (255)  NULL,
    [FileType]  NVARCHAR (50)   NULL,
    [DataFiles] VARBINARY (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

