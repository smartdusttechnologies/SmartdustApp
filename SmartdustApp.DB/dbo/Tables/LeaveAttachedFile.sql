CREATE TABLE [dbo].[LeaveAttachedFile] (
    [LeaveID]    INT NULL,
    [DocumentID] INT NULL,
    FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[DocumentTable] ([ID]),
    FOREIGN KEY ([LeaveID]) REFERENCES [dbo].[Leave] ([ID])
);

