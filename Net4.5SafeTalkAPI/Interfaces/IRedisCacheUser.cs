using SafeTalkCore;
using System.Collections.Generic;

namespace Net4._5SafeTalkAPI.Interfaces
{
    public interface IRedisCacheUser
    {
        User ConvertToObject(string guid, string publicName);
        void SaveUser(string guid, string publicName);
        User GetUser(int index, RedisCache cache = null);
        int GetUserIndex(string guid, RedisCache cache = null);
        List<User> GetUsers();
        string AssignNewNameToUser(string guid);
    }
}
