CREATE TABLE [dbo].[Lookup] (
    [ID]               INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50) NOT NULL,
    [LookupCategoryID] INT          NOT NULL,
    [IsDeleted]        BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([LookupCategoryID]) REFERENCES [dbo].[LookupCategory] ([ID])
);

