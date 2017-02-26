using SafeTalkCore;
using System.Collections.Generic;

namespace Net4._5SafeTalkAPI.Interfaces
{
    public interface IRedisCacheChatroom
    {
        bool AddChatroom(string name);
        bool RemoveChatroom(string name);
        List<Chatroom> GetChatrooms();
        Chatroom GetChatroom(int index, RedisCache cache = null);
        int GetChatroomIndex(string name, RedisCache cache = null);        
        bool AddUserToChatroom(User user, Chatroom chatroom);
        bool RemoveUserFromChatroom(User user, Chatroom chatroom);
    }
}
