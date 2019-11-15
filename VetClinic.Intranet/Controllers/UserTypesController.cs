using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.VetClinic;
using VetClinic.Data.HelpersClass;

namespace VetClinic.Intranet.Controllers
{
    public class UserTypesController : Controller
    {
        private readonly VetClinicContext _context;

        public UserTypesController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: UserTypes
        public async Task<IActionResult> Index()
        {
            
            var vetClinicContext = _context.UserTypes.Include(u => u.UserTypeAddedUser).Include(u => u.UserTypeUpdatedUser).Where(w => w.IsActive == true && w.Name != "No7818Permissions");
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: UserTypes/Details/5
        public async Task<IActionResult> Details(int? id/*,string searchString*/)
        {
            DateTime time = DateTime.Now;
            if (id == null)
            {
                _context.UserTypes.Add(new UserType()
                {
                    Name = "nazwa",
                    Description = "opis",
                    IsActive = true,
                    AddedDate = time
                }
               );
                _context.SaveChanges();
                id = _context.UserTypes.SingleOrDefault(s => s.AddedDate == time && s.IsActive == true).UserTypeID;
                if(id == null)
                    return NotFound();
            }

            //aktualnie przegladany userType (grupa)
            var usersPermissions_add = await _context.UserTypes
                .FirstOrDefaultAsync(m => m.UserTypeID == id && m.IsActive == true);

            //uprawnienia grupy - Access!
            ICollection<UserTypePermission> permissions_add = _context.UserTypePermissions
                .Include(i => i.Permission)
                .Include(i => i.UserType.Users)
                .Where(w => w.UserTypeID == id && w.IsActive == true && w.Access == true && w.Permission.IsActive == true).ToList();

            //uprawnienia w string
            ICollection<string> permissions_select_name = _context.UserTypePermissions
                .Include(i => i.Permission)
                .Where(w => w.UserTypeID == id && w.IsActive == true && w.Access == true)
                .Select(s => s.Permission.Name).ToList();
            
            //wszystkie istniejace uprawnienia w string
            List<string> permissionsAllName = _context.Permissions.Where(w => w.IsActive == true).Select(s => s.Name).ToList();
            //kolekcja wszystkich uprawnien max
            List<Permission> permissionsAll = _context.Permissions.Where(w => w.IsActive == true).ToList();

            foreach (string item2 in permissions_select_name)
            {
                foreach (string item in permissionsAllName)
                {
                    if(item.Contains(item2))
                    {
                        //usuwamy z listy te ktore juz sa w uprawnieniach grupy
                        Permission permissionToRemove = permissionsAll.FirstOrDefault(w => w.Name == item);
                        if(permissionToRemove != null)
                            permissionsAll.Remove(permissionToRemove);
                    }
                }
            }

            ICollection<User> user_add = _context.Users.Where(w => w.IsActive == true).ToList();
           
            return View(new HelpersDetails
            {
                typeUser = usersPermissions_add,
                permissionsUser = permissions_add,
                users = user_add,
                permissionsNotSelected = permissionsAll
            }); ;

        }

        [HttpPost]
        public async Task<IActionResult> Details (HelpersDetails hd, List<int> select_permission, List<int> select_user, List<int> select_new_user)
        {   
            //userType ktory przegladamy
            //IsActive wykluczamy juz wczesniej na liscie w index
            UserType resultTypeUsers = _context.UserTypes.SingleOrDefault(u => u.UserTypeID == hd.typeUser.UserTypeID);
            
            //Edycja UserType
            if(resultTypeUsers != null)
            {
                resultTypeUsers.Name = hd.typeUser.Name;
                resultTypeUsers.Description = hd.typeUser.Description;
                resultTypeUsers.UpdatedDate = DateTime.Now;
                resultTypeUsers.IsActive = true;
                _context.SaveChanges();
            }


            //lista uprawnien w string dla aktualnego typu (grupy)
            List<int> permissionsQuery = _context.UserTypePermissions
                .Where(w => w.UserTypeID == hd.typeUser.UserTypeID && w.IsActive == true && w.Access == true && w.Permission.IsActive == true)
                .Select(s => s.UserPermissionID).ToList();
  
          
            
            foreach (int item in permissionsQuery) //juz istniejace uprawniena
            {
                    if(!select_permission.Contains(item)) //nowe rozdanie nie posiada elementu starego rozdania
                    {
                        UserTypePermission resulUserTypePermissions = _context.UserTypePermissions.FirstOrDefault(w => w.UserPermissionID == item);
                        if( resulUserTypePermissions != null)
                        { 
                            resulUserTypePermissions.Access = false;
                            _context.SaveChanges();
                        }
                    }
            }
         
            //stare rozdanie nie posiada elementu nowego 
            foreach (int item in select_permission)
            {
                if (!permissionsQuery.Contains(item))
                {
                    Permission resultPermissionToAdd = _context.Permissions.FirstOrDefault(f => f.PermissionID == item && f.IsActive == true);
                    if (resultPermissionToAdd != null)
                    {
                        _context.UserTypePermissions.Add(new UserTypePermission
                        {
                            UserTypeID = hd.typeUser.UserTypeID,
                            PermissionID = item,
                            Access = true,
                            IsActive = true,
                            AddedDate = DateTime.Now
                        });
                        _context.SaveChanges();
                    }
                }
            }

            #region uzytkownicy grupy

            //lista users ktorzy naleza do userType (Grupy)
            List<int> oldUsersList = _context.Users
                .Where(w => w.UserTypeID == hd.typeUser.UserTypeID && w.IsActive == true)
                .Select(s => s.UserID).ToList();

            //sprawdzam czy nie zmniejszono liczbe uzytkownikow / dodawanie uzytkownikow jest osobno
            foreach (int item3 in oldUsersList) //juz istniejace
            {
                if (!select_user.Contains(item3)) //nowe rozdanie nie posiada usera item3
                {
                    User resultUser = _context.Users.FirstOrDefault(f => f.UserID == item3);
                    
                    if (resultUser != null)
                    {
                        var isTableNoPermissions = _context.UserTypes.SingleOrDefault(s => s.Name == "No7818Permissions");
                        if(isTableNoPermissions == null)
                        {
                            _context.UserTypes.Add(new UserType()
                            {
                                Name = "No7818Permissions",
                                Description = "NoPermissions",
                                IsActive = true,
                                AddedDate = DateTime.Now
                            }
                            );
                            _context.SaveChanges();
                        }
                        //grupa nie posiadajaca uprawnien

                        int noPermissionsID = _context.UserTypes.SingleOrDefault(s => s.Name == "No7818Permissions").UserTypeID;
                        resultUser.UserTypeID = noPermissionsID;
                        _context.SaveChanges();
                                               
                    }
                }
            }

            #endregion


            #region Dodawanie uzytkownika do listy


            foreach (int itemNewUserID in select_new_user)
            {
                User userToAdd = _context.Users.FirstOrDefault(f => f.UserID == itemNewUserID);
                if(userToAdd != null)
                {
                    userToAdd.UpdatedDate = DateTime.Now;
                    userToAdd.UserTypeID = hd.typeUser.UserTypeID;
                }
                _context.SaveChanges();
            }
            
            #endregion

            return RedirectToAction(nameof(Index));
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            userType.IsActive = false;

            var userTypePermission = _context.UserTypePermissions.Where(w => w.UserTypeID == id).ToList();
            foreach ( var item in userTypePermission)
            {
                item.IsActive = false;
            }
            
            var isTableNoPermissions = _context.UserTypes.SingleOrDefault(s => s.Name == "No7818Permissions");
            if (isTableNoPermissions == null)
            {
                _context.UserTypes.Add(new UserType()
                {
                    Name = "No7818Permissions",
                    Description = "NoPermissions",
                    IsActive = true,
                    AddedDate = DateTime.Now
                }
                );
                _context.SaveChanges();
            }

            int noPermissionsID = _context.UserTypes.SingleOrDefault(s => s.Name == "No7818Permissions").UserTypeID;
            var users = _context.Users.Where(w => w.UserTypeID == id).ToList();
            
            foreach ( var itemUser in users)
            {
                itemUser.IsActive = true;
                itemUser.UserTypeID = noPermissionsID;
            }
                                             
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        
        }

    }
}
