CREATE TABLE [dbo].[Employee] (
    [EmployeeId] BIGINT NOT NULL,
    [ManagerId]  BIGINT NULL,
    PRIMARY KEY CLUSTERED ([EmployeeId] ASC),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[User] ([Id]),
    FOREIGN KEY ([ManagerId]) REFERENCES [dbo].[User] ([Id])
);

