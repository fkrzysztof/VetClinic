using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.HelpersClass;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class UserTypesController : AbstractPolicyController
    {
        public UserTypesController(VetClinicContext context) : base(context) { }

        // GET: UserTypes
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.UserTypes.Include(u => u.UserTypeAddedUser).Include(u => u.UserTypeUpdatedUser).Where(w => w.Name != "No7818Permissions");
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        // GET: UserTypes/Details/5
        public async Task<IActionResult> Edit(int? id/*,string searchString*/)
        {
            DateTime time = DateTime.Now;
            if (id == null)
            {
                _context.UserTypes.Add(new UserType()
                {
                    Name = "nazwa",
                    Description = "opis",
                    IsActive = true,
                    AddedDate = time,
                    UpdatedDate = time
                }
               );
                _context.SaveChanges();
                id = _context.UserTypes.SingleOrDefault(s => s.AddedDate == time && s.IsActive == true).UserTypeID;
                if (id == null)
                    return NotFound();
            }

            //aktualnie przegladany userType (grupa)
            var usersPermissions_add = await _context.UserTypes
                .FirstOrDefaultAsync(m => m.UserTypeID == id && m.IsActive == true);

            ICollection<Permission> permissions_add = _context.UserTypePermissions.Include(i => i.Permission)
                .Where(w => w.UserTypeID == id && w.IsActive == true && w.Access == true && w.Permission.IsActive == true)
                .Select(s => s.Permission).ToList();
            
            
            //uprawnienia w int
            ICollection<int> permissions_select_name = _context.UserTypePermissions
                .Include(i => i.Permission)
                .Where(w => w.UserTypeID == id && w.IsActive == true && w.Access == true && w.Permission.IsActive == true)
                .Select(s => s.Permission.PermissionID).ToList();

            //wszystkie istniejace uprawnienia w int
            List<int> permissionsAllName = _context.Permissions.Where(w => w.IsActive == true).Select(s => s.PermissionID).ToList();
            
            //kolekcja wszystkich uprawnien max
            List<Permission> permissionsAll = _context.Permissions.Where(w => w.IsActive == true).ToList();

            foreach (int itemSelect in permissions_select_name) 
            {
                foreach (int itemAll in permissionsAllName)
                {
                    if (itemSelect == itemAll)
                    {
                        //usuwamy z listy te ktore juz sa w uprawnieniach grupy
                        Permission permissionToRemove = permissionsAll.FirstOrDefault(w => w.PermissionID == itemSelect);
                        if (permissionToRemove != null)
                            permissionsAll.Remove(permissionToRemove);
                    }
                }
            }

            ICollection<User> user_add = _context.Users.Where(w => w.IsActive == true).ToList();

            return View(new UserTypesDetails
            {
                typeUser = usersPermissions_add,
                permissionsUser = permissions_add,
                users = user_add,
                permissionsNotSelected = permissionsAll
            });

        }

        [HttpPost]
        public IActionResult Edit(UserTypesDetails hd, List<int> select_permission, List<int> select_user, List<int> select_new_user)
        {
            //userType ktory przegladamy
            //IsActive wykluczamy juz wczesniej na liscie w index
            UserType resultTypeUsers = _context.UserTypes.SingleOrDefault(u => u.UserTypeID == hd.typeUser.UserTypeID);

            //Edycja UserType
            if (resultTypeUsers != null)
            {
                resultTypeUsers.Name = hd.typeUser.Name;
                resultTypeUsers.Description = hd.typeUser.Description;
                resultTypeUsers.UpdatedDate = DateTime.Now;
                resultTypeUsers.IsActive = true;
                _context.SaveChanges();
            }

            //lista uprawnien w int dla aktualnego typu (grupy)
            List<int> permissionsQuery = _context.UserTypePermissions
                .Where(w => w.UserTypeID == hd.typeUser.UserTypeID && w.IsActive == true && w.Access == true && w.Permission.IsActive == true)
                .Select(s => s.PermissionID).ToList();

            // - odejmowanie
            foreach (int item in permissionsQuery) //juz istniejace uprawniena
            {
                if (!select_permission.Contains(item)) //nowe rozdanie nie posiada elementu starego rozdania
                {
                    UserTypePermission resulUserTypePermissions = _context.UserTypePermissions.FirstOrDefault(w => w.PermissionID == item && w.UserTypeID == hd.typeUser.UserTypeID && w.IsActive == true && w.Permission.IsActive == true );
                    if (resulUserTypePermissions != null)
                    {
                        resulUserTypePermissions.Access = false;
                        _context.SaveChanges();
                    }
                }
            }
           
            // + dodawnia
            //stare rozdanie nie posiada elementu nowego II
            foreach (int item in select_permission)
            {
                if (!permissionsQuery.Contains(item))
                {
                    UserTypePermission resultPermissionToAdd = _context.UserTypePermissions
                        .FirstOrDefault(w => w.UserTypeID == hd.typeUser.UserTypeID &&
                        w.Access == false && 
                        w.PermissionID == item && 
                        w.IsActive == true && 
                        w.Permission.IsActive == true);

                    if (resultPermissionToAdd == null)
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
                    else
                    {
                        resultPermissionToAdd.Access = true;
                        
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
                if (userToAdd != null)
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
            userType.UpdatedDate = DateTime.Now;

            var userTypePermission = _context.UserTypePermissions.Where(w => w.UserTypeID == id).ToList();
            foreach (var item in userTypePermission)
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

            foreach (var itemUser in users)
            {
                itemUser.IsActive = true;
                itemUser.UserTypeID = noPermissionsID;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
