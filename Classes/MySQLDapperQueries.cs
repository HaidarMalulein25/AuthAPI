using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Text;
using MySql.Data.MySqlClient;
namespace UserAuthentication.Classes
{
    public abstract class MySQLDapperQueries  
    {
        public static string InsertUser(User u)
        {
            try
            {
                StringBuilder sCommand = new StringBuilder("INSERT INTO users(id, created,modified,username,password,name,email,salt,token,refresh_token,token_expiration,refresh_token_expiration) VALUES ");
                sCommand.Append(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;",
                    u.id, u.created.ToString("yyyy-MM-dd HH:mm:ss"), u.modified.ToString("yyyy-MM-dd HH:mm:ss"), u.username, u.password, u.name, u.email, u.salt, u.token, u.refresh_token, u.token_expiration.ToString("yyyy-MM-dd HH:mm:ss"), u.refresh_token_expiration.ToString("yyyy-MM-dd HH:mm:ss")));
                int res = MySQLDappper.ExecuteMySQLQuery(sCommand.ToString());
                if (res > 0)
                    return "ok";
                else
                    return "Error in inserting the record";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public static User GetUserByID(string id)
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                User u = connection.QueryFirst<User>(String.Format("select  * from users where id='{0}'", id));
                return u;
            }
            catch
            {
                return null;
            }
        }
        public static User GetUserByUserName(string username)
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                User u = connection.QueryFirst<User>("select  * from users where username=@username", new { username=username});
                return u;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public static User GetUserByAccessTokenAndRefreshToken(string accesstoken,string refreshtoken)
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                User u = connection.QueryFirst<User>(String.Format("select  * from users where token='{0}' and refresh_token='{1}'", accesstoken,refreshtoken));
                return u;
            }
            catch
            {
                return null;
            }
        }
        public static string UpdateUser(string id, string username, string password,string salt, string name, string email)
        {
            try
            {
                int rowsAffected = 0;
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                if(salt!=""&&password!="")
                rowsAffected= connection.Execute("UPDATE Users SET username = @username,password= @password,name=@name,email=@email,salt=@salt,modified=@modified WHERE Id = @id", new {id=id,username = username, password = password, name= name, email=email,salt=salt,modified=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } );
                else
                    rowsAffected = connection.Execute("UPDATE Users SET username = @username,name=@name,email=@email,modified=@modified WHERE Id = @id", new { id = id, username = username, name = name, email = email, modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                if (rowsAffected > 0)
                    return "ok";
                else
                    return "user was not updated";

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public static int UpdateUserTokens(string id, string accesstoken, DateTime accesstoken_expiration, string refreshtoken, DateTime refreshtoken_expiration)
        {
            try
            {
                int rowsAffected = 0;
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                rowsAffected = connection.Execute("UPDATE Users SET refresh_token=@refresh_token,token=@token,token_expiration=@token_expiration,refresh_token_expiration=@refresh_token_expiration,modified=@modified WHERE id =@id", new { refresh_token = refreshtoken, token = accesstoken, token_expiration = accesstoken_expiration, refresh_token_expiration = refreshtoken_expiration,id=id,modified=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                return rowsAffected;
            }
            catch
            {
                return -1;
            }
        }
        public static string DeleteUser(string id)
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                connection.Execute("DELETE from Users WHERE id=@id ", new { id = id } );
                return "ok";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public static List<User> GetUsers()
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                List<User> users = connection.Query<User>("select  * from users ").ToList();
                return users;
            }
            catch
            {
                return new List<User>();
            }
        }
        public static bool IsAccessTokenExpired(string accesstoken)
        {
            try
            {
                MySqlConnection connection = MySQLDappper.GetMYSQLConnection();
                DateTime token = connection.QueryFirst<DateTime>("select  token_expiration from users where token=@accesstoken", new { accesstoken = accesstoken });
                return DateTime.Now > token ? true : false;
            }
            catch
            {
                return true;
            }
        }
    }
}
