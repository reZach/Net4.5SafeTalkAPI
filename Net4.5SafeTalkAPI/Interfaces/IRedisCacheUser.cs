using SafeTalkCore;
using System.Collections.Generic;

namespace Net4._5SafeTalkAPI.Interfaces
{
    public interface IRedisCacheUser
    {
        void SaveUser(User user);
        User GetUser(int index, RedisCache cache = null);
        int GetUserIndex(User user, RedisCache cache = null);
        int GetUserIndex(string guid, RedisCache cache = null);
        List<User> GetUsers();
        string AssignNewNameToUser(string guid);
    }
}
