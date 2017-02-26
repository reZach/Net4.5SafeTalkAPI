using Net4._5SafeTalkAPI.Hubs;
using Net4._5SafeTalkAPI.Interfaces;
using SafeTalkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Net4._5SafeTalkAPI.Controllers
{
    public class ChatroomController : ApiHubController<SafeTalkHub>, IRedisCacheChatroom
    {
        // Internal functions
        public bool AddChatroom(string name)
        {
            bool retVal = false;
            RedisCache Cache = GetCache();

            if (!Cache.Chatrooms.Any(x => x.PublicName == name))
            {
                retVal = true;
                Chatroom NewChatroom = new Chatroom
                {
                    PublicName = name
                };

                Cache.Chatrooms.Add(NewChatroom);

                SetCache(Cache);
            }

            return retVal;
        }

        public bool RemoveChatroom(string name)
        {
            RedisCache Cache = GetCache();

            int count = Cache.Chatrooms.RemoveAll(x => x.PublicName == name);

            SetCache(Cache);

            return count > 0;
        }

        public List<Chatroom> GetChatrooms()
        {
            RedisCache cache = GetCache();

            return cache.Chatrooms;
        }

        public Chatroom GetChatroom(int index, RedisCache cache = null)
        {
            if (cache == null)
            {
                cache = GetCache();
            }
            Chatroom chatroom = cache.Chatrooms[index];

            return chatroom;
        }

        public int GetChatroomIndex(string name, RedisCache cache = null)
        {
            if (cache == null)
            {
                cache = GetCache();
            }
            int chatroomIndex = cache.Chatrooms.FindIndex(x => x.PublicName == name);

            return chatroomIndex;
        }

        public bool AddUserToChatroom(User user, Chatroom chatroom)
        {
            bool success = false;
            RedisCache cache = GetCache();

            int chatroomFromCache = cache.Chatrooms.FindIndex(x => x.PublicName == chatroom.PublicName);
            
            if (chatroomFromCache >= 0)
            {
                // If the user isn't in the chatroom
                if (!cache.Chatrooms[chatroomFromCache].UserGuids.Any(x => x == user.Guid))
                {
                    // Add the user's guid to the chatroom
                    cache.Chatrooms[chatroomFromCache].UserGuids.Add(user.Guid);
                    SetCache(cache);
                    success = true;
                }
            }

            return success;
        }

        public bool RemoveUserFromChatroom(User user, Chatroom chatroom)
        {
            bool success = false;
            RedisCache Cache = GetCache();

            int chatroomFromCache = Cache.Chatrooms.FindIndex(x => x.PublicName == chatroom.PublicName);

            if (chatroomFromCache >= 0)
            {
                // Remove the user from the chatroom
                Cache.Chatrooms[chatroomFromCache].UserGuids.RemoveAll(x => x == user.Guid);
                SetCache(Cache);
                success = true;
            }

            return success;
        }



        // API endpoints
        [HttpPost]
        public IHttpActionResult Add(string name)
        {
            bool result = AddChatroom(name);

            if (result)
            {
                return Ok("good");
            }
            return null;
        }

        [HttpPost]
        public IHttpActionResult Remove(string name)
        {
            bool result = RemoveChatroom(name);

            if (result)
            {
                return Ok("good");
            }
            return null;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Join(User user, Chatroom chatroom)
        {
            bool success = AddUserToChatroom(user, chatroom);

            if (success)
            {
                await Hub.Groups.Add(ConnectionId, chatroom.PublicName);
                return Ok("good");
            }

            return null;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Leave(User user, Chatroom chatroom)
        {
            bool success = RemoveUserFromChatroom(user, chatroom);

            if (success)
            {
                await Hub.Groups.Remove(ConnectionId, chatroom.PublicName);
                return Ok("good");
            }

            return null;
        } 

        [HttpGet]
        public IHttpActionResult List()
        {
            return Ok(GetChatrooms());
        }
    }
}
