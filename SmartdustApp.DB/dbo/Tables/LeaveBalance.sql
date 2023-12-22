CREATE TABLE [dbo].[LeaveBalance] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [UserID]    INT          NULL,
    [LeaveType] VARCHAR (50) DEFAULT ('Unknown') NOT NULL,
    [Available] INT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

