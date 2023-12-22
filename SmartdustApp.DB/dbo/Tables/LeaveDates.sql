CREATE TABLE [dbo].[LeaveDates] (
    [ID]        INT  IDENTITY (1, 1) NOT NULL,
    [LeaveID]   INT  NULL,
    [LeaveDate] DATE NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([LeaveID]) REFERENCES [dbo].[Leave] ([ID])
);

