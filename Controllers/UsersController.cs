using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthentication.Classes;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        // Added to be Used in the Updated Version
        private string GetIP()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public string Login([FromBody] LogInRequest req)
        {
            ResponseData Resp = new ResponseData();
            User u = MySQLDapperQueries.GetUserByUserName(req.username);
            if (u == null)
            {
                Resp = new ResponseData
                {
                    Code = "501",
                    Message = "User Not found",
                    Data = null
                };
            }
            else
            {
                string hashedpassword = Authentication.GenerateHashedPWD(req.password, u.salt.ToString());
                if (u.password == hashedpassword)
                {
                    AccessToken atoken = Authentication.GenerateAccessToken(GetIP());
                    RefreshToken rtoken = Authentication.GenerateRefreshToken(GetIP());
                    MySQLDapperQueries.UpdateUserTokens(u.id, atoken.Token, atoken.ExpiryDate, rtoken.Token, rtoken.ExpiryDate);
                    Resp = new ResponseData
                    {
                        Code = "200",
                        Message = "Verified",
                        Data = new
                        {
                            Accesstoken = atoken.Token,
                            RefreshToken = rtoken.Token,
                            ID=u.id
                        }
                    };
                }
                else
                {
                    Resp = new ResponseData
                    {
                        Code = "503",
                        Message = "Wrong Password",
                        Data = null
                    };
                }
            }
            return JsonConvert.SerializeObject(Resp, Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("CreateUser")]

        public string CreateUser([FromBody] SignUpUserRequest user)
        {
            ResponseData Resp = new ResponseData();
            try
            {
                User u = new User();
                string IP = GetIP();
                string response = "";
                string salt = Authentication.GenerateRandomSalt(15);
                u.id = System.Guid.NewGuid().ToString();
                u.username = user.username;
                u.salt = salt;
                u.password = Authentication.GenerateHashedPWD(user.password, salt);
                u.name = user.name;
                u.email = user.email;
                u.created = DateTime.Now;
                u.modified = DateTime.Now;
                RefreshToken rtoken = Authentication.GenerateRefreshToken(IP);
                u.refresh_token = rtoken.Token;
                u.refresh_token_expiration = rtoken.ExpiryDate;
                AccessToken atoken = Authentication.GenerateAccessToken(IP);
                u.token = atoken.Token;
                u.token_expiration = atoken.ExpiryDate;
                response= MySQLDapperQueries.InsertUser(u);
                if (response == "ok")
                {
                    Resp = new ResponseData
                    {
                        Code = "200",
                        Message = "User Created",
                        Data = null
                    };
                }
                else
                {
                    Resp = new ResponseData
                    {
                        Code = "502",
                        Message = response,
                        Data = null
                    };
                }
            }
            catch(Exception ex)
            {
                Resp= new ResponseData
                        {
                            Code = "502",
                            Message = ex.Message,
                            Data = null
                        };
            }
            return JsonConvert.SerializeObject(Resp,Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateUser")]

        public string UpdateUser([FromBody] CRUDRequest req)
        {
            ResponseData Resp = new ResponseData();
            if (!this.ModelState.IsValid)
            {
                Resp = new ResponseData
                {
                    Code = "404",
                    Message = "Invalid Parameters",
                    Data = null
                };
                return JsonConvert.SerializeObject(Resp, Formatting.None);
            }
            bool IsAccessTokenExpired = false;
            IsAccessTokenExpired = MySQLDapperQueries.IsAccessTokenExpired(req.accesstoken);
            if (IsAccessTokenExpired)
            {
                Resp = new ResponseData
                {
                    Code = "600",
                    Message = "Access Token Expired",
                    Data = null
                };
            }
            else
            {
                try
                {
                    string salt = "";
                    string hashedpassword = "";
                    if (!String.IsNullOrEmpty(req.password.Trim()))
                    {
                         salt = Authentication.GenerateRandomSalt(15);
                         hashedpassword = Authentication.GenerateHashedPWD(req.password, salt);
                    }
                    string response = MySQLDapperQueries.UpdateUser(req.id, req.username, hashedpassword,salt, req.name, req.email);
                    if (response == "ok")
                    {
                        Resp = new ResponseData
                        {
                            Code = "200",
                            Message = "User Created",
                            Data = null
                        };
                    }
                    else
                    {
                        Resp = new ResponseData
                        {
                            Code = "502",
                            Message = response,
                            Data = null
                        };
                    }
                }
                catch
                {
                    Resp = new ResponseData
                    {
                        Code = "502",
                        Message = "An Error Occured",
                        Data = null
                    };
                }
            }
            return JsonConvert.SerializeObject(Resp, Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ListUsers")]
        public string ListUsers([FromBody] CRUDRequest req)
        {
            bool IsAccessTokenExpired = false;
            ResponseData Resp = new ResponseData();
            IsAccessTokenExpired = MySQLDapperQueries.IsAccessTokenExpired(req.accesstoken);
            if (IsAccessTokenExpired)
            {
                Resp = new ResponseData
                {
                    Code = "600",
                    Message = "Access Token Expired",
                    Data = null
                };
            }
            else
            {
                Resp = new ResponseData
                {
                    Code = "200",
                    Message = "OK",
                    Data = MySQLDapperQueries.GetUsers()
                };
            }
            return JsonConvert.SerializeObject(Resp, Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GetUser")]
        public string GetUser([FromBody] CRUDRequest req)
        {
            bool IsAccessTokenExpired = false;
            ResponseData Resp = new ResponseData();
            IsAccessTokenExpired = MySQLDapperQueries.IsAccessTokenExpired(req.accesstoken);
            if (IsAccessTokenExpired)
            {
                Resp = new ResponseData
                {
                    Code = "600",
                    Message = "Access Token Expired",
                    Data = null
                };
            }
            else
            {
                Resp = new ResponseData
                {
                    Code = "200",
                    Message = "OK",
                    Data = MySQLDapperQueries.GetUserByID(req.id)
                };
            }
            return JsonConvert.SerializeObject(Resp, Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("DeleteUser")]
        public string DeleteUser([FromBody] CRUDRequest req)
        {
            bool IsAccessTokenExpired = false;
            ResponseData Resp = new ResponseData();
            IsAccessTokenExpired = MySQLDapperQueries.IsAccessTokenExpired(req.accesstoken);
            if (IsAccessTokenExpired)
            {
                Resp = new ResponseData
                {
                    Code = "600",
                    Message = "Access Token Expired",
                    Data = null
                };
            }
            else
            {
                string response = MySQLDapperQueries.DeleteUser(req.id);
                if (response == "ok")
                {
                    Resp = new ResponseData
                    {
                        Code = "200",
                        Message = "User Deleted Sucessfully",
                        Data = null
                    };
                }
                else
                {
                    Resp = new ResponseData
                    {
                        Code = "502",
                        Message = response,
                        Data = null
                    };
                }
            }
            return JsonConvert.SerializeObject(Resp, Formatting.None);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public string RefreshToken([FromBody] RefreshTokenRequest request)
        {
            ResponseData resp = new ResponseData();
            string accesstoken = request.accesstoken;
            string oldrefreshtoken = request.refreshtoken;
            User u = MySQLDapperQueries.GetUserByAccessTokenAndRefreshToken(accesstoken,oldrefreshtoken);
            //  if refresh token if expired... return to login page 
            //  if not use it  to generate new access token  and new refreshtoken
            if (u == null)
            {
                resp= new ResponseData
                {
                    Code = "506",
                    Message = "invalid user",
                    Data = null
                };
            }
            else if (u.refresh_token_expiration < DateTime.Now)
            {
                // return to login page
                resp = new ResponseData
                {
                    Code = "700",
                    Message = "refresh token has expired",
                    Data = null
                };
            }
            else
            {
                RefreshToken rtoken = Authentication.RefreshToken(GetIP(), accesstoken, oldrefreshtoken);
                u.refresh_token = rtoken.Token;
                u.refresh_token_expiration = rtoken.ExpiryDate;
                AccessToken atoken = Authentication.GenerateAccessToken(GetIP());
                u.token = atoken.Token;
                u.token_expiration = atoken.ExpiryDate;
                int updatedrow= MySQLDapperQueries.UpdateUserTokens(u.id,u.token,u.token_expiration,u.refresh_token,u.refresh_token_expiration);
                if (updatedrow > 0)
                {
                    resp = new ResponseData
                    {
                        Code = "200",
                        Message = "Tokens Refreshed",
                        Data = new
                        {
                            Accesstoken = u.token,
                            RefreshToken = u.refresh_token,
                            ID=u.id
                        }
                    };
                }
                else
                {
                    resp = new ResponseData
                    {
                        Code = "508",
                        Message = "Couldn't Refresh Tokens",
                        Data = null
                    };
                }
            }
            return JsonConvert.SerializeObject(resp, Formatting.None);
        }
    }
}
