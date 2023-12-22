CREATE TABLE [dbo].[Leave] (
    [ID]            INT          IDENTITY (1, 1) NOT NULL,
    [UserID]        INT          NULL,
    [Reason]        VARCHAR (50) NULL,
    [AppliedDate]   DATE         NULL,
    [LeaveDays]     INT          NULL,
    [LeaveTypeID]   INT          DEFAULT ((0)) NULL,
    [LeaveStatusID] INT          NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LeaveStatusID_Lookup] FOREIGN KEY ([LeaveStatusID]) REFERENCES [dbo].[Lookup] ([ID]),
    CONSTRAINT [FK_LeaveType] FOREIGN KEY ([LeaveTypeID]) REFERENCES [dbo].[Lookup] ([ID])
);

