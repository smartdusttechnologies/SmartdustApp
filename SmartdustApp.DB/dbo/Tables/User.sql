﻿CREATE TABLE [dbo].[User] (
    [Id]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]               NVARCHAR (100) NULL,
    [FirstName]              NVARCHAR (50)  NOT NULL,
    [LastName]               NVARCHAR (50)  NULL,
    [Email]                  NVARCHAR (100) NOT NULL,
    [Mobile]                 NVARCHAR (50)  NOT NULL,
    [Country]                NVARCHAR (100) NULL,
    [ISDCode]                NVARCHAR (50)  NULL,
    [TwoFactor]              BIT            NULL,
    [Locked]                 BIT            NULL,
    [IsActive]               BIT            NULL,
    [EmailValidationStatus]  SMALLINT       NULL,
    [MobileValidationStatus] SMALLINT       NULL,
    [OrgId]                  INT            NOT NULL,
    [AdminLevel]             SMALLINT       NOT NULL,
    [IsDeleted]              BIT            CONSTRAINT [D_User_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organization] ([Id])
);

