using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using HomeworkApp.Models;
using HomeworkApp.ViewModels;

namespace HomeworkApp.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index(string sort, string search)
        {
            IEnumerable<UserListViewModel> users = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56441/api/");
                //HTTP GET
                var responseTask = client.GetAsync("user");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserModel>>();
                    readTask.Wait();

                    users = readTask.Result.Select(userModel => new UserListViewModel
                    {
                        UserID = userModel.UserID,
                        Name = userModel.FirstName + " " + userModel.LastName,
                        EmailAddress = userModel.EmailAddress,
                        JoinedDate = userModel.JoinedDate
                    });
                }
                else //web api sent error response 
                {
                    users = Enumerable.Empty<UserListViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            users = AddFilterAndSorter(users, sort, search);
            return View(users);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel user, string button)
        {
            if (!ModelState.IsValid)
            {
                return PartialView();
            }

            if (string.Compare(button, "Cancel", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return RedirectToAction("Index");
            }
             
            using (var client = new HttpClient())
            {
                //add date
                user.JoinedDate = DateTime.Now;

                client.BaseAddress = new Uri("http://localhost:56441/api/user");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<UserModel>("user", ConvertToUserModel(user));
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return PartialView(user);
        }

        [HttpGet]
        public ActionResult Edit(string userID)
        {
            return PartialView(GetUserByID(userID));
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel user, string button)
        {
            if (string.Compare(button, "Cancel", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return RedirectToAction("Index");
            }

            if (!UserExists(user.UserID))
            {
                ModelState.AddModelError(string.Empty, "This UserID doesn't exist.");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56441/api/user");

                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<UserModel>("user", ConvertToUserModel(user));
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return PartialView(user);
        }

        [HttpGet]
        public bool UserExists(string userID)
        {
            return GetUserByID(userID) != null;
        }

        [HttpGet]
        public ActionResult Delete(string userID)
        {
            var user = new UserViewModel() { UserID = userID };
            return PartialView(user);
        }

        [HttpPost]
        public ActionResult Delete(string userID, string button)
        {
            if (string.Compare(button, "Cancel", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return RedirectToAction("Index");
            }

            if (!UserExists(userID))
            {
                ModelState.AddModelError(string.Empty, "This UserID doesn't exist.");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56441/api/user");

                    //HTTP DELETE
                    var postTask = client.DeleteAsync("?userid=" + userID);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View();
        }

        private UserViewModel ConvertToUserViewModel(UserModel userModel)
        {
            return new UserViewModel()
            {
                UserID = userModel.UserID,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress = userModel.EmailAddress,
                JoinedDate = userModel.JoinedDate
            };
        }

        private UserModel ConvertToUserModel(UserViewModel userViewModel)
        {
            return new UserModel()
            {
                UserID = userViewModel.UserID,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                EmailAddress = userViewModel.EmailAddress,
                JoinedDate = userViewModel.JoinedDate
            };
        }

        private IEnumerable<UserListViewModel> AddFilterAndSorter(IEnumerable<UserListViewModel> users, string sort, string search)
        {
            ViewBag.UserNameSortParameter = string.IsNullOrEmpty(sort) ? "Name_DESC" : string.Empty;
            ViewBag.EmailAddressSortParameter = sort == "EmailAddress" ? "EmailAddress_DESC" : "EmailAddress";
            ViewBag.JoinedDateSortParameter = sort == "JoinedDate" ? "JoinedDate_DESC" : "JoinedDate";

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(c => c.Name.ToUpper().StartsWith(search.ToUpper()));
            }

            switch (sort)
            {
                case "Name_DESC":
                    users = users.OrderByDescending(c => c.Name);
                    break;
                case "EmailAddress":
                    users = users.OrderBy(c => c.EmailAddress);
                    break;
                case "EmailAddress_DESC":
                    users = users.OrderByDescending(c => c.EmailAddress);
                    break;
                case "JoinedDate":
                    users = users.OrderBy(c => c.JoinedDate);
                    break;
                case "JoinedDate_DESC":
                    users = users.OrderByDescending(c => c.JoinedDate);
                    break;
                default:
                    users = users.OrderBy(c => c.Name);
                    break;
            }

            return users;
        }

        private UserViewModel GetUserByID(string userID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56441/api/");
                //HTTP GET
                var responseTask = client.GetAsync("user?userID=" + userID);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserModel>();
                    readTask.Wait();

                    return ConvertToUserViewModel(readTask.Result);
                }
                else //web api sent error response 
                {
                    //log response status here.
                    ModelState.AddModelError(string.Empty, "Not Found.");
                }
            }
            return null;
        }
    }
}