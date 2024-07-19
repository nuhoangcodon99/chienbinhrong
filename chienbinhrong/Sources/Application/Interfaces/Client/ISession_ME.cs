using NgocRongGold.Application.IO;
using NgocRongGold.Model;
using System.Collections.Generic;

namespace NgocRongGold.Application.Interfaces.Map
{
    public interface ISession_ME
    {
        int Id { get; set; }  
        string IpV4 { get; set; }
        bool IsNewVersion { get; set; }  
        long TimeConnected { get; set; }        
        Player Player { get; set; }
        sbyte ZoomLevel { get; set; }
        string Version { get; set; }
        void CloseMessage();
        bool IsConnected();
        bool IsLogin { get; set; }
        void SendMessage(Message message);
        void HansakeMessage();
        public byte[] BlockOtherClient(Message message);
        void SetupAdmin();
        void SetConnect(Message message);
        bool LoginGame(string c_username, string c_password, string c_version, sbyte c_type, Message message);
        void Disconnect();
    }
}