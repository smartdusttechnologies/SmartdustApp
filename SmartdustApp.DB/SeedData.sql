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
IF NOT EXISTS (SELECT 1 FROM [PermissionModuleType]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[PermissionModuleType]  ON

    INSERT INTO [dbo].[PermissionModuleType]
               ([Id], [Name], [IsDeleted])
         VALUES
               (1, N'UIPageTypePermission', 0),
               (2, N'UiPageMetadataPermission', 0),
               (3, N'CubeTesting', 0),
               (1004, N'UiControlTypePermission', 0),
               (1005, N'LeaveBalancePermission', 0),
               (1006, N'EmployeeTablePermission', 0),
               (1007, N'EmployeeLeavePermission', 0),
               (1008, N'LeavePermission', 0)
    SET IDENTITY_INSERT [dbo].[PermissionModuleType]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [SubPermissionModuleType] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[SubPermissionModuleType]  ON

    INSERT INTO [dbo].[SubPermissionModuleType]
             ([Id], [Name], [PermissionModuleTypeId], [IsDeleted])
         VALUES
              (1, N'SampleReceiving', 3, 0),
			  (2, N'TestPlan', 3, 0),
			  (3, N'LabAnalysis', 3, 0),
			  (4, N'TestReport', 3, 0),
			  (5, N'Billing&Payments', 3, 0),
			  (6, N'UIPageTypePermission', 1, 0),
			  (7, N'UiPageMetadataPermission', 2, 0),
			  (8, N'UiControlTypePermission', 1004, 0),
			  (9, N'LeaveBalancePermission', 1005, 0),
			  (10, N'EmployeeTablePermission', 1006, 0),
			  (11, N'EmployeeLeavePermission', 1007, 0),
			  (12, N'LeavePermission', 1008, 0)
    SET IDENTITY_INSERT [dbo].[SubPermissionModuleType]  OFF
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
IF NOT EXISTS (SELECT 1 FROM [Organization]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Organization]  ON

    INSERT INTO [dbo].[Organization]
               ([Id], [OrgCode], [OrgName], [IsDeleted])
         VALUES
               (0, N'SYSORG', N'SYSORG', 0)
    SET IDENTITY_INSERT [dbo].[Organization]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [User]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[User]  ON

    INSERT INTO [dbo].[User]
               ([Id], [UserName], [FirstName], [LastName], [Email], [Mobile], [Country], [ISDCode], [TwoFactor], [Locked], [IsActive], [EmailValidationStatus], [MobileValidationStatus], [OrgId], [AdminLevel], [IsDeleted])
         VALUES
              (0, N'sysadmin', N'sysadmin', N'sysadmin', N'sysadmin@gmail.com', N'1234567899', N'INDIA', N'91', 0, 0, 1, 0, 0, 0, 0, 0),
               (1, N'ApplicationAdminUser', N'Application', N'Admin', N'AppAdmin@gmail.com', N'8123444349', N'India', N'91', 0, 0, 1, 0, 0, 0, 2, 0),
               (4, N'Yashraj', N'string', N'string', N'yashrajsmartdust@gmail.com', N'1111111111', N'string', N'string', 1, 0, 1, 0, 0, 0, 0, 0),
               (25, N'YashrajManager', N'YashrajManager', N'YashrajManager', N'yashrajsmartdust@gmail.com', N'1111111111', N'string', N'string', 1, 0, 1, 0, 0, 0, 0, 0)

    SET IDENTITY_INSERT [dbo].[User]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Employee] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[Employee]  ON

    INSERT INTO [dbo].[Employee]
               ([EmployeeId], [ManagerId], [ID])
         VALUES
               (4, 25, 1)

    SET IDENTITY_INSERT [dbo].[Employee]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [UserClaim] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[UserClaim]  ON

    INSERT INTO [dbo].[UserClaim]
               ([Id], [UserId], [PermissionId], [IsDeleted], [ClaimTypeId])
         VALUES
               (1, 1, 4, 1, 1),
			   (2, 0, 1, 1, 1),
			   (3, 0, 2, 1, 1),
			   (4, 0, 3, 1, 1),
			   (5, 0, 4, 1, 1),
			   (6, 0, 5, 1, 1),
			   (7, 0, 6, 1, 1),
			   (8, 0, 7, 1, 1),
			   (9, 0, 8, 1, 1),
			   (10, 0, 9, 0, 1),
			   (11, 0, 10, 0, 1)

    SET IDENTITY_INSERT [dbo].[UserClaim]  OFF
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
IF NOT EXISTS (SELECT 1 FROM [RoleClaim] WHERE Id IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[RoleClaim]  ON

    INSERT INTO [dbo].[RoleClaim]
               ([Id], [RoleId], [PermissionId], [ClaimTypeId], [IsDeleted])
         VALUES
             (1, 1, 1, 1, 0),
			 (2, 1, 3, 1, 0),
			 (3, 1, 12, 1, 0),
			 (4, 1, 13, 1, 0),
			 (5, 1, 1011, 1, 0),
			 (6, 1, 1012, 1, 0),
			 (7, 4, 1017, 1, 0),
			 (8, 4, 1018, 1, 0),
			 (9, 4, 1019, 1, 0),
			 (10, 4, 1020, 1, 0),
			 (11, 4, 1021, 1, 0),
			 (12, 4, 1022, 1, 0),
			 (13, 4, 1023, 1, 0),
			 (14, 4, 1024, 1, 0),
			 (15, 4, 1025, 1, 0),
			 (16, 4, 1026, 1, 0),
			 (17, 5, 1027, 1, 0),
			 (18, 5, 1028, 1, 0),
			 (19, 5, 1029, 1, 0),
			 (20, 4, 1027, 1, 0),
			 (21, 4, 1028, 1, 0),
			 (22, 4, 1029, 1, 0)

    SET IDENTITY_INSERT [dbo].[RoleClaim]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Group] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[Group]  ON

    INSERT INTO [dbo].[Group]
               ([Id], [Name], [IsDeleted])
         VALUES
             (1, N'Back Office ', 0)

    SET IDENTITY_INSERT [dbo].[Group]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [GroupClaim] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[GroupClaim]  ON

    INSERT INTO [dbo].[GroupClaim]
               ([Id], [GroupId], [ClaimTypeId], [PermissionId], [IsDeleted])
         VALUES
              (1, 1, 1, 1, 0),
			  (2, 1, 1, 2, 0),
			  (3, 1, 1, 3, 0),
			  (4, 1, 1, 4, 0),
			  (5, 1, 1, 5, 0),
			  (6, 1, 1, 6, 0),
			  (7, 1, 1, 7, 0),
			  (8, 1, 1, 8, 0),
			  (9, 1, 1, 9, 0),
			  (10, 1, 1, 10, 0),
			  (12, 1, 1, 12, 0),
			  (13, 1, 1, 13, 0),
			  (1011, 1, 1, 1015, 0),
			  (1013, 1, 1, 1016, 0)

    SET IDENTITY_INSERT [dbo].[GroupClaim]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [UserRole]  WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[UserRole]  ON

    INSERT INTO [dbo].[UserRole]
               ([Id], [UserId], [RoleId], [IsDeleted])
         VALUES
               (2, 4, 5, 0),
               (3, 25, 4, 0)
    SET IDENTITY_INSERT [dbo].[UserRole]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [UserGroup] WHERE ID IN (1))
BEGIN
    SET IDENTITY_INSERT [dbo].[UserGroup]  ON

    INSERT INTO [dbo].[UserGroup]
               ([Id], [GroupId], [UserId], [IsDeleted])
         VALUES
              (1, 1, 0, 0)

    SET IDENTITY_INSERT [dbo].[UserGroup]  OFF
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
               (0, N'qnVDMZYlsGjs4chNs1/qPidI70eDUZ1fzUF5EdCqdl0=', N'NDlzcm0GY1GqMgn+urXX9Q==', 0, CAST(N'2022-11-12T14:25:35.763' AS DateTime)),
               (4, N'9t8EnooTjYrMD+6MRDF9dPj9DO0hIgUfSk0nhXbTbmM=', N'FP9CmBGWPaLeP5v0VaffVw==', 4, CAST(N'2022-11-12T14:25:35.763' AS DateTime)),
               (25, N'KW5A14aPNtEBbIniV+iaIDhUghAS7vcXY2Q6g0yyexI=', N'WBlcgHQ3Ttri9Z2gSyShUg==', 25, CAST(N'2022-11-12T14:25:35.763' AS DateTime))
    SET IDENTITY_INSERT [dbo].[PasswordLogin]  OFF
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
IF NOT EXISTS (SELECT 1 FROM [LookupCategory] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[LookupCategory]  ON

    INSERT INTO [dbo].[LookupCategory]
               ([Id], [Name], [IsDeleted])
			   VALUES
              (1, N'LeaveType', 0),
			  (3, N'LeaveStatus', 0)
    SET IDENTITY_INSERT [dbo].[LookupCategory]  OFF
END
GO
IF NOT EXISTS (SELECT 1 FROM [Lookup] WHERE Id = 0)
BEGIN
    SET IDENTITY_INSERT [dbo].[Lookup]  ON

    INSERT INTO [dbo].[Lookup]
                ([Id], [Name], [LookupCategoryId], [IsDeleted])
			   VALUES
             (1, N'MedicalLeave', 1, 0),
			 (2, N'PaidLeave', 1, 0),
			 (3, N'LeaveofAbsence', 1, 0),
			 (5, N'Approve', 3, 0),
			 (6, N'Pending', 3, 0),
			 (7, N'Decline', 3, 0),
			 (8, N'Deny', 3, 0)
    SET IDENTITY_INSERT [dbo].[Lookup]  OFF
END