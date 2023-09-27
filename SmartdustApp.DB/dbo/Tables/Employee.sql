CREATE TABLE [dbo].[Employee] (
    [EmployeeId] BIGINT NOT NULL,
    [ManagerId]  BIGINT NULL,
    [ID]         INT    IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[User] ([Id]),
    FOREIGN KEY ([ManagerId]) REFERENCES [dbo].[User] ([Id])
);



