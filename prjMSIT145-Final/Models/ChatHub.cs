using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace prjMSIT145_Final.Models

{
    public class ChatHub : Hub 
    {
       
        private readonly ispanMsit145shibaContext _context;
      
        public ChatHub(ispanMsit145shibaContext context)
        {
            _context = context;
           
        }
    
        public static List<ChatroomUser> users = new List<ChatroomUser>();

        public async Task SendMessage(int otheruserid, string message)
        {  
            //抓Session裡的member
            string json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_USER));
            BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
            //找聊天室室友這兩個人的
            var userinchat = _context.Chat2Users.Where(u => u.Userid == member.ChatroomUserid);
            var otheruserinchat = _context.Chat2Users.Where(u => u.Userid == otheruserid);
            var joinchatroom = userinchat.Join(otheruserinchat, u => u.Chatid, o => o.Chatid, (u, o) => u.Chatid).FirstOrDefault();
            //如果沒找到
            if (joinchatroom == null)
            {
                //新增一個聊天室
                Chatroom Chatroom= new Chatroom();
                //取名(為了等一下可以找到者個聊天室)
                Chatroom.ChatName = member.ChatroomUserid + "to" + otheruserid;
                _context.Chatrooms.Add(Chatroom);
                _context.SaveChanges();
                //找到剛剛那個聊天室
                joinchatroom = _context.Chatrooms.FirstOrDefault(c => c.ChatName == member.ChatroomUserid + "to" + otheruserid).Chatid;
                //把雙方的ID放進去
                Chat2User chat2User =new Chat2User();
                chat2User.Chatid = joinchatroom;
                chat2User.Userid = (int)member.ChatroomUserid;
                _context.SaveChanges();
                Chat2User chat2Userother = new Chat2User();
                chat2Userother.Chatid = joinchatroom;
                chat2Userother.Userid = otheruserid;
                _context.SaveChanges();
            } 

             //對自己發出訊息
               await Clients.Client(Context.ConnectionId).SendAsync("RemoteMessage",message);
            //找出要發給的那個人的資料
            var user = users.FirstOrDefault(u => u.ChatroomUserid == otheruserid);
            if(user != null)
            {
                //對對方發出訊息
               await Clients.Client(user.ConnectionId).SendAsync("LocalMessage", message);
            }
             //把訊息存入聊天室
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.Chatid = joinchatroom;
                chatMessage.Senderid =(int)member.ChatroomUserid;
                chatMessage.Message= message;
                chatMessage.SendTime = DateTime.Now;
                _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges();
        }
        //修改連線時的動作(加上自己想要的)
        public override async Task OnConnectedAsync()
        {
            if (users.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault() == null)
            {
                ChatroomUser user = new ChatroomUser();
                if (Context.GetHttpContext().Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    string json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_Business));
                    BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
                    user.ChatroomUserid = (int)member.ChatroomUserid;
                    user.ConnectionId = Context.ConnectionId;
                    users.Add(user);
                }


            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var user = users.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                users.Remove(user);
            }
            base.OnDisconnectedAsync(ex);
        }
    }
}
