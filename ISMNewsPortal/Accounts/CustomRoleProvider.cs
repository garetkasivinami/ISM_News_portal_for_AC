﻿using ISMNewsPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ISMNewsPortal.Accounts
{
    public class CustomRoleProvider : RoleProvider
    {
		public CustomRoleProvider() { }

		public override bool IsUserInRole(string username, string roleName)
		{
			IEnumerable<string> roles = AdminHelper.GetRolesStringsByLogin(username);
			if (roles == null)
            {
				FormsAuthentication.SignOut();
				return false;
			}
			return roles.Count() != 0 && roles.Contains(roleName);
		}

		public override string[] GetRolesForUser(string username)
		{
			IEnumerable<string> roles = AdminHelper.GetRolesStringsByLogin(username);
			if (roles == null)
			{
				FormsAuthentication.SignOut();
				return new string[0];
			}
			return roles.ToArray();
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
	}
}