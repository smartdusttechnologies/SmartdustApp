IF NOT EXISTS (SELECT 1 FROM [Organization]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Organization]  ON

    INSERT INTO [dbo].[Organization]
               ([Id], [OrgCode], [OrgName], [IsDeleted])
         VALUES
               (0, N'SYSORG', N'SYSORG', 0),
    SET IDENTITY_INSERT [dbo].[Organization]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [User]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[User]  ON

    INSERT INTO [dbo].[User]
               ([Id], [UserName], [FirstName], [LastName], [Email], [Mobile], [Country], [ISDCode], [TwoFactor], [Locked], [IsActive], [EmailValidationStatus], [MobileValidationStatus], [OrgId], [AdminLevel], [IsDeleted])
         VALUES
               (4, N'Yashraj', N'string', N'string', N'string', N'string', N'string', N'string', 1, 0, 1, 0, 0, 0, 0, 0)

    SET IDENTITY_INSERT [dbo].[User]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [PasswordPolicy]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PasswordPolicy]  ON

    INSERT INTO [dbo].[PasswordPolicy]
               ([Id], [MinCaps], [MinSmallChars], [MinSpecialChars], [MinNumber], [MinLength], [AllowUserName], [DisAllPastPassword], [DisAllowedChars], [ChangeIntervalDays], [OrgId], [IsDeleted])
         VALUES
               (0, 1, 1, 1, 1, 8, 1, 0, NULL, 30, 0, 0)
    SET IDENTITY_INSERT [dbo].[PasswordPolicy]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [PermissionType]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PermissionType]  ON

    INSERT INTO [dbo].[PermissionType]
               ([Id], [Name], [Value], [IsDeleted])
         VALUES
               (1, N'Create', N'Create', 0),
               (2, N'Update', N'Update', 0),
               (3, N'Read', N'Read', 0),
               (4, N'Delete', N'Delete', 0)
    SET IDENTITY_INSERT [dbo].[PermissionType]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Permission]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Permission]  ON

    INSERT INTO [dbo].[Permission]
               ([Id], [Name], [PermissionModuleTypeId], [PermissionTypeId], [IsDeleted])
         VALUES
               (1, N'uiPageType.add', 1, 1, 0),
               (2, N'uiPageType.edit', 1, 2, 0),
               (3, N'uiPageType.read', 1, 3, 0),
               (4, N'uiPageType.delete', 1, 4, 0),
               (5, N'uiPageMetadata.add', 2, 1, 0),
               (6, N'uiPageMetadata.edit', 2, 2, 0),
               (7, N'uiPageMetadata.read', 2, 3, 0),
               (8, N'uiPageMetadata.delete', 2, 4, 0),
               (9, N'testPlan.add', 3, 1, 0),
               (10, N'testPlan.read', 3, 3, 0),
               (12, N'sample.add', 4, 1, 0),
               (13, N'sample.read', 4, 3, 0),
               (1011, N'uiControlType.add', 8, 1, 0),
               (1012, N'uiControlType.read', 8, 3, 1),
               (1013, N'uiC.ad', 5, 3, 0),
               (1014, N'uiC.read', 5, 1, 0),
               (1015, N'j', 4, 2, 0),
               (1016, N'k', 4, 4, 0)
    SET IDENTITY_INSERT [dbo].[Permission]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Role]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Role]  ON

    INSERT INTO [dbo].[Role]
               ([Id], [Name], [Level], [IsDeleted])
         VALUES
               (1, N'Sysadmin', 0, 0),
               (2, N'Admin', 1, 0),
               (3, N'ApplicationAdmin', 2, 0),
               (4, N'Manager', 3, 0),
               (5, N'GeneralUser', 6, 0)
    SET IDENTITY_INSERT [dbo].[Role]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [UserRole]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[UserRole]  ON

    INSERT INTO [dbo].[UserRole]
               ([Id], [UserId], [RoleId], [IsDeleted])
         VALUES
               (2, 4, 2, 0)
    SET IDENTITY_INSERT [dbo].[UserRole]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [LoginLog]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[LoginLog]  ON

    INSERT INTO [dbo].[LoginLog]
               ([Id], [UserId], [LoginDate], [Status], [UserName], [PasswordHash], [IPAddress], [Browser], [DeviceCode], [DeviceName])
         VALUES
               (1, 4, CAST(N'2023-05-25T11:39:42.380' AS DateTime), 0, N'Yashraj', N'vZi70awIXXhSRjz8hK3fO/xZn4yOgspRKxcKpcQ3Fik=', NULL, NULL, NULL, NULL)
               
    SET IDENTITY_INSERT [dbo].[LoginLog]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [LoginToken]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[LoginToken]  ON

    INSERT INTO [dbo].[LoginToken]
               ([Id], [UserId], [AccessToken], [RefreshToken], [AccessTokenExpiry], [DeviceCode], [DeviceName], [RefreshTokenExpiry])
         VALUES
               (1, 4, N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJZYXNocmFqIiwianRpIjoiYWFhN2U1NDUtMzg3Yy00NWZkLThjMGYtNjdhZDFiMmM3NTkzIiwiaWF0IjoxNjg3Nzc2MzcxLCJVc2VySWQiOiI1IiwiT3JnYW5pemF0aW9uSWQiOiIwIiwibmJmIjoxNjg3Nzc2MzcwLCJleHAiOjE2ODc4NjI3NzAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyJ9.SzVu75FEz9_zmgpAhq--gbl_suFUt-X_HNegM2AcmTY', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJZYXNocmFqIiwianRpIjoiYWFhN2U1NDUtMzg3Yy00NWZkLThjMGYtNjdhZDFiMmM3NTkzIiwiaWF0IjoxNjg3Nzc2MzcxLCJVc2VySWQiOiI1IiwiT3JnYW5pemF0aW9uSWQiOiIwIiwibmJmIjoxNjg3Nzc2MzcwLCJleHAiOjE2OTAzNjgzNzAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyJ9.AxhC32ks4-IGiBDmSNUkbiD0PudW4rM7N17a3xbfx0c', CAST(N'2023-06-27T16:16:10.743' AS DateTime), NULL, NULL, CAST(N'2023-07-26T16:16:10.743' AS DateTime))
               
    SET IDENTITY_INSERT [dbo].[LoginToken]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [LoginTokenLog]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[LoginTokenLog]  ON

    INSERT INTO [dbo].[LoginTokenLog]
               ([Id], [UserId], [AccessToken], [RefreshTokenExpiry], [DeviceCode], [DeviceName], [RefreshToken], [AccessTokenExpiry])
         VALUES
               (1, 4, N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJZYXNocmFqIiwianRpIjoiNjdmYjBmMTQtYzJlMy00M2U1LWEyMTItMDY5YTgzYjVlNzI4IiwiaWF0IjoxNjg0OTk0OTgyLCJVc2VySWQiOiI1IiwiT3JnYW5pemF0aW9uSWQiOiIwIiwibmJmIjoxNjg0OTk0OTgyLCJleHAiOjE2ODUwODEzODIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyJ9.-9t9wT04beCkDBuMJ9mxF1jkT2JobghD_vd61grfMKs', CAST(N'2023-06-24T11:39:42.363' AS DateTime), NULL, NULL, N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJZYXNocmFqIiwianRpIjoiNjdmYjBmMTQtYzJlMy00M2U1LWEyMTItMDY5YTgzYjVlNzI4IiwiaWF0IjoxNjg0OTk0OTgyLCJVc2VySWQiOiI1IiwiT3JnYW5pemF0aW9uSWQiOiIwIiwibmJmIjoxNjg0OTk0OTgyLCJleHAiOjE2ODc1ODY5ODIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzM3LyJ9.9EKBnO1wIpo0sSF8pQNlutbPTb_9zrTL9lD_taDZtW8', CAST(N'2023-05-26T11:39:42.363' AS DateTime))
    SET IDENTITY_INSERT [dbo].[LoginTokenLog]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [PasswordLogin]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PasswordLogin]  ON

    INSERT INTO [dbo].[PasswordLogin]
               ([Id], [PasswordHash], [PasswordSalt], [UserId], [ChangeDate])
         VALUES
               (0, N'qnVDMZYlsGjs4chNs1/qPidI70eDUZ1fzUF5EdCqdl0=', N'NDlzcm0GY1GqMgn+urXX9Q==', 4, CAST(N'2022-11-12T14:25:35.763' AS DateTime))
    SET IDENTITY_INSERT [dbo].[PasswordLogin]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [ClaimType]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[ClaimType]  ON

    INSERT INTO [dbo].[ClaimType]
               ([Id], [Name], [IsDeleted], [Value])
         VALUES
               (1, N'ApplicationPermission', 0, N'ApplicationPermission'),
               (2, N'UserId', 0, N'UserId'),
               (3, N'OrganizationId', 0, N'OrganizationId')

    SET IDENTITY_INSERT [dbo].[ClaimType]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Contact]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Contact]  ON

    INSERT INTO [dbo].[Contact]
               ([Id], [Name], [Mail], [Subject], [Message], [Phone], [Address])
         VALUES
               (1, N'string', N'string', N'string', N'string', 0, N'string')
    SET IDENTITY_INSERT [dbo].[Contact]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [PermissionModuleType]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PermissionModuleType]  ON

    INSERT INTO [dbo].[PermissionModuleType]
               ([Id], [Name], [IsDeleted])
         VALUES
               (1, N'UIPageTypePermission', 0),
               (2, N'UiPageMetadataPermission', 0),
               (3, N'CubeTesting', 0),
               (1004, N'UiControlTypePermission', 0)
    SET IDENTITY_INSERT [dbo].[PermissionModuleType]  OFF
END
GO