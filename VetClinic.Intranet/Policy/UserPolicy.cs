using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Data;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data.Data.Clinic;
using Microsoft.AspNetCore.Routing;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace VetClinic.Intranet.Policy
{
    public class UserPolicy : Controller
    {
        private readonly VetClinicContext context;
        private readonly int userId;
        private readonly string controllerName;
        private readonly string actionName;
        private readonly string fullActionName;

        public UserPolicy(VetClinicContext _context, HttpContext _session, RouteData _routeData)
        {
            context = _context;
            userId = Convert.ToInt32(_session.Session.GetString("UserID"));
            controllerName = _routeData.Values["controller"].ToString();
            actionName = _routeData.Values["action"].ToString();
            fullActionName = controllerName + "/" + actionName;
        }

        private async Task<string[]> GetUserTypePermissions()
        {
            var currentUser = await context.Users.FindAsync(userId);
            List<string> defaultPermissions = new List<string>();

            if (currentUser is null)
            {
                return defaultPermissions.ToArray();
            }

            var userPermissionGroup = await context.UserTypes.FindAsync(currentUser.UserTypeID);

            string[] userTypePermissions = (
                from userTypePermission in context.UserTypePermissions
                where userTypePermission.UserTypeID == userPermissionGroup.UserTypeID && userTypePermission.IsActive
                select userTypePermission.Permission.Description
            ).ToArray();

            foreach (var permission in userTypePermissions)
            {
                if (permission.Contains(","))
                {
                    foreach (var singlePermission in permission.Split(","))
                    {
                        defaultPermissions.Add(singlePermission.Trim());
                    }
                } else
                {
                    defaultPermissions.Add(permission);
                }
            }
            
            return defaultPermissions.ToArray();
        }

        public async Task<bool> hasNoAccess()
        {
            var currentUser = await context.Users.FindAsync(userId);

            if (currentUser is null)
            {
                return true;
            }

            string[] userTypePermissions = GetUserTypePermissions().Result;

            return !userTypePermissions.Contains(controllerName);
        }

        public async Task<IActionResult> RedirectUser()
        {
            return Redirect("/");
        }

        public ViewDataDictionary PopulateViewData(ViewDataDictionary viewData)
        {
            string[] userTypePermissions = GetUserTypePermissions().Result;

            if (userTypePermissions is null)
            {
                return viewData;
            }

            foreach (var permission in userTypePermissions)
            {
                viewData["HasAccessTo" + permission] = true;
            }

            return viewData;
        }
    }
}
