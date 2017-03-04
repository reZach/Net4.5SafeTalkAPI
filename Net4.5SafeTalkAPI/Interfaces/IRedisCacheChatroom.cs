using SafeTalkCore;
using SafeTalkCore.Models;
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
        bool AddUserToChatroom(UserChatroom userChatroom);
        bool RemoveUserFromChatroom(UserChatroom userChatroom);
    }
}
