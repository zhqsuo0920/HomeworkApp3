using HomeworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeworkApp.Controllers.Api
{
    /// <summary>
    /// Web API Controller for table User.
    /// CRUD operations.
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Return all users</returns>
        public IHttpActionResult Get()
        {
            using (var ctx = new HomeworkEntities())
            {
                var users = ctx.Users.Select(s => new UserListModel()
                {
                    UserID = s.UserID,
                    Name = s.FirstName + " " + s.LastName,
                    EmailAddress = s.EmailAddress,
                    JoinedDate = s.JoinedDate
                }).ToList<UserListModel>();

                if (users.Count == 0)
                {
                    return NotFound();
                }

                return Ok(users);
            }
        }

        /// <summary>
        /// Get the user by given userID
        /// </summary>
        /// <param name="userID">String for a given userID</param>
        /// <returns>Return the user if found otherwise return bad request</returns>
        public IHttpActionResult GetUserByID(string userID)
        {
            if (userID.Length == 0)
                return BadRequest("Not a valid user id");

            using (var ctx = new HomeworkEntities())
            {
                var existingUser = ctx.Users.Where(s => s.UserID == userID).FirstOrDefault();

                if (existingUser != null)
                {
                    var user = new UserModel()
                    {
                        UserID = existingUser.UserID,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        EmailAddress = existingUser.EmailAddress,
                        JoinedDate = existingUser.JoinedDate
                    };
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">UserModel for a user</param>
        /// <returns>Return ok if inserting the user otherwise return bad request</returns>
        public IHttpActionResult Post(UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new HomeworkEntities())
            {
                var existingUser = ctx.Users.Where(s => s.UserID == user.UserID).FirstOrDefault<User>();
                if (existingUser != null)
                {
                    return BadRequest("This User ID has been used.");
                }

                ctx.Users.Add(new User()
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    JoinedDate = user.JoinedDate
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        // <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">UserModel for a user</param>
        /// <returns>Return ok if updating the user otherwise return bad request</returns>
        public IHttpActionResult Put(UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new HomeworkEntities())
            {
                var existingUser = ctx.Users.Where(s => s.UserID == user.UserID).FirstOrDefault<User>();

                if (existingUser != null)
                {
                    existingUser.UserID = user.UserID;
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.EmailAddress = user.EmailAddress;
                    existingUser.JoinedDate = user.JoinedDate;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        // <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userID">String for a userID</param>
        /// <returns>Return ok if the user is deleted otherwise return bad request</returns>
        public IHttpActionResult Delete(string userID)
        {
            if (userID.Length == 0)
                return BadRequest("Not a valid user id");

            using (var ctx = new HomeworkEntities())
            {
                var existingUser = ctx.Users.Where(s => s.UserID == userID).FirstOrDefault();

                if (existingUser != null)
                {
                    ctx.Entry(existingUser).State = System.Data.EntityState.Deleted;
                    ctx.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("This user doesn't exist");
                }
            }
        }
    }
}
