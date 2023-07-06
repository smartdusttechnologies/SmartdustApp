CREATE TABLE [dbo].[Leaves] (
    [Id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId]      INT    NOT NULL,
    [Date]        INT    NOT NULL,
    [LeaveStatus] INT    NOT NULL,
    [IsDeleted]   BIT    CONSTRAINT [DF_Leaves_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Leaves] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Leaves_User] FOREIGN KEY ([Id]) REFERENCES [dbo].[User] ([Id])
);

