using KhyberYouth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }



        public RoleManager<IdentityRole> RoleManager { get; }

        [HttpGet]
        public IActionResult ListRole()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded) return RedirectToAction("ListRole", "Administration");
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (id == null) return NotFound();

            var model = new RoleEditViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.User.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleEditViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (model.Id == null)
            {
                return NotFound();
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound();
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if (model[i].IsSelected && !await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                        continue;
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }

            }
            return RedirectToAction("EditRole", new { Id = roleId });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            else
            {
                try
                {
                    //throw new Exception("General exception");
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded) return RedirectToAction("ListRole");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRole");
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role connot be deleted as there are users in this roles. if you want to delete this role, please remove the users from the role and then try to delete.";
                    return View("Error");
                }
            }
        }

        ////////////////////////////////-User List-////////////////////////////////////

        [HttpGet]
        public IActionResult ListUser()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            var userRole = await userManager.GetRolesAsync(user);
            var userClaim = await userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Claims = userClaim.Select(c => c.Value).ToList(),
                Roles = userRole
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null) return NotFound();
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded) return RedirectToAction("ListUser");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            //userId = "207fbb5a-627a-4ead-ad81-253c4ad455ee";
            ViewBag.userId = id;

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new List<ManageRoleViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ManageRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name

                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<ManageRoleViewModel> model, string id)
        {
            ViewBag.userId = id;

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var role = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, role);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = id });
        }

        // ManageUserClaims
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string id)
        {

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new UserClaimsViewModel { UserId = id };

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimsType = claim.Type,

                };

                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing user calim");
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user, model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimsType, c.ClaimsType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add the selected user calim to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = id });
        }

        //delete/post
        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var del = await userManager.FindByIdAsync(id);
            if (del == null) return NotFound();

            else
            {
                var result = await userManager.DeleteAsync(del);
                if (result.Succeeded) return RedirectToAction("ListUser");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUser");
            }
        }
    }
}


