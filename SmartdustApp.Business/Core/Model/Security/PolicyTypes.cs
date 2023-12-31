﻿namespace SmartdustApp.Business.Core.Model
{
    /// <summary>
    /// Policy Type User Permission
    /// </summary>

    public static class PolicyTypes
    {
        public static class Users
        {
            public const string Manage = "users.manage.policy";
            public const string EditRole = "users.edit.role.policy";
        }

        public static class UIPageType
        {
            public const string Add = "uiPageType.Add.policy";
            public const string EditRole = "users.edit.role.policy";
        }
    }
}
