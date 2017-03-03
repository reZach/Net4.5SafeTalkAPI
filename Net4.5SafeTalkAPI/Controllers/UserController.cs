using Net4._5SafeTalkAPI.Hubs;
using Net4._5SafeTalkAPI.Interfaces;
using SafeTalkCore;
using System.Collections.Generic;
using System.Web.Http;

namespace Net4._5SafeTalkAPI.Controllers
{
    public class UserController : ApiHubController<SafeTalkHub>, IRedisCacheUser
    {
        // Internal functions
        public void SaveUser(User user)
        {
            RedisCache cache = GetCache();            
            int userIndex = GetUserIndex(user, cache);

            if (userIndex < 0)
            {
                cache.Users.Add(user);
            }
            else
            {
                cache.Users[userIndex].PublicName = user.PublicName;
            }

            SetCache(cache);
        }

        public User GetUser(int index, RedisCache cache = null)
        {
            if (cache == null)
            {
                cache = GetCache();
            }
            User user = cache.Users[index];

            return user;
        }

        public int GetUserIndex(User user, RedisCache cache = null)
        {
            if (cache == null)
            {
                cache = GetCache();
            }
            int userIndex = cache.Users.FindIndex(x => x.Guid == user.Guid);

            return userIndex;
        }

        public int GetUserIndex(string guid, RedisCache cache = null)
        {
            if (cache == null)
            {
                cache = GetCache();
            }
            int userIndex = cache.Users.FindIndex(x => x.Guid == guid);

            return userIndex;
        }

        public List<User> GetUsers()
        {
            RedisCache cache = GetCache();

            return cache.Users;
        }

        public string AssignNewNameToUser(string guid)
        {
            string retVal = "";
            RedisCache cache = GetCache();
            int userIndex = GetUserIndex(guid, cache);

            if (userIndex >= 0)
            {
                retVal = SafeTalkCore.User.GenerateRandomName();
                cache.Users[userIndex].PublicName = retVal;

                SetCache(cache);
            }

            return retVal;
        }



        // API endpoints
        [HttpGet]
        public IHttpActionResult New()
        {
            User newUser = new User();

            return Ok(newUser);
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody]User user)
        {
            SaveUser(user);

            // http://stackoverflow.com/questions/22762697/return-empty-json-on-null-in-webapi/22765622
            // Returning an empty string returns a 404, so we pass in a string here
            return Ok("good");
        }

        [HttpGet]
        public IHttpActionResult Get(string guid)
        {
            RedisCache cache = GetCache();

            int userIndex = GetUserIndex(guid, cache);
            User user = GetUser(userIndex, cache);

            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult NewName(string guid)
        {
            string result = AssignNewNameToUser(guid);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            return Ok(GetUsers());
        }
    }
}