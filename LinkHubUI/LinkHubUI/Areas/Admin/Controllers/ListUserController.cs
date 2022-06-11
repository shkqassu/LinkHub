using BLL;
using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkHubUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "A")]
    public class ListUserController : BaseAdminController
    {

        //
        // GET: /Admin/ListUser/
        public ActionResult Index(string SortOrder, string SortBy, string Page)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBy = SortBy;
            var users = objBs.userBs.GetALL();

            #region SwitchCase
            switch (SortBy)
            {
                case "UserEmail":
                    switch (SortOrder)
                    {

                        case "Asc":
                            users = users.OrderBy(x => x.UserEmail).ToList();
                            break;
                        case "Desc":
                            users = users.OrderByDescending(x => x.UserEmail).ToList();
                            break;
                        default:
                            break;
                    }
                    break;

                case "Role":
                    switch (SortOrder)
                    {

                        case "Asc":
                            users = users.OrderBy(x => x.Role).ToList();
                            break;
                        case "Desc":
                            users = users.OrderByDescending(x => x.Role).ToList();
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    users = users.OrderBy(x => x.UserEmail).ToList();
                    break;
            } 
            #endregion

            ViewBag.TotalPages = Math.Ceiling(objBs.userBs.GetALL().Count() / 10.0);
            int page = int.Parse(Page == null ? "1" : Page);
            ViewBag.Page = page;
            users = users.Skip((page - 1) * 10).Take(10).ToList();

            return View(users);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var user = objBs.userBs.GetByID(Id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(tbl_User U)
        {
            try
            {
                ModelState.Remove("UserEmail");
                if (ModelState.IsValid)
                {
                    var user = objBs.userBs.GetByID(U.UserId);
                    user.UserEmail = U.UserEmail;
                    user.Role = U.Role;
                    user.Password = U.Password;
                    user.ConfirmPassword = user.ConfirmPassword;
                    objBs.userBs.Update(user);
                    TempData["Msg"] = "Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Edit");
                }
            }
            catch (Exception e1)
            {
                TempData["Msg"] = "Create Failed : " + e1.Message;
                return RedirectToAction("Edit");
            }
        }
    }
}