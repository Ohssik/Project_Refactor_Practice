using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using System;
using System.Reflection.Metadata;
using System.Diagnostics.Metrics;
using prjMSIT145_Final.ViewModel;

namespace prjMSIT145_Final.Models

{
    public class ChatHub : Hub 
    {
        private IWebHostEnvironment _eviroment;
        private readonly ispanMsit145shibaContext _context;
      
        public ChatHub(ispanMsit145shibaContext context, IWebHostEnvironment p)
        {
            _context = context;
            _eviroment = p;
        } 
        public static List<ChatroomUser> users = new List<ChatroomUser>();


        //更新右邊聊天室
        public async Task ReNewChatRoom()
        {
            
            ChatroomUser MyData = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);  
            if (MyData == null)
                return;
            //有自已的聊天室
            var ChatRoom = _context.Chat2Users.Join(_context.Chat2Users, c1 => c1.Chatid, c2 => c2.Chatid, (c1, c2) => new
            {
                fid = c1.Fid,
                chatroomid = c1.Chatid,
                user = c1.Userid,
                Otheruser = c2.Userid,
            }
            ).Where(c => c.user == MyData.ChatroomUserid && c.Otheruser != MyData.ChatroomUserid);

            if (ChatRoom == null)
                return;

            var ChatRoomuser = ChatRoom.Join(_context.ChatroomUsers, other => other.Otheruser, u => u.ChatroomUserid, (other, u) => new
            {
                ChatroomUserid = u.ChatroomUserid,
                UserType = u.UserType,
                ChatroomId = other.chatroomid,
                MemberId = u.Memberfid,
                LastOnlineTime = u.LastOnlineTime
            });
            //合併訊息



            var LastMassage = _context.ChatMessages.Join(ChatRoomuser, m => m.Chatid, c => c.ChatroomId, (m, c) => new
            {
                ChatroomUserid = c.ChatroomUserid,
                UserType = c.UserType,
                ChatroomId = c.ChatroomId,
                MemberId = c.MemberId,
                LastOnlineTime = c.LastOnlineTime,
                LastMassage = m.Message,
                LastMassagetime = m .SendTime
            });
            //商家與照片合併
            var Businessimg = _context.BusinessMembers.Join(_context.BusinessImgs, b => b.Fid, i => i.BFid, (b, i) => new
            {
                Bfid=b.Fid,
                BusinessName=b.MemberName,
                LogoImg=i.LogoImgFileName,
            });
            //合併User商家
            var BChatRoomUser = ChatRoomuser.Where(u => u.UserType == 1)
               .Join(Businessimg, c => c.MemberId, b => b.Bfid, (c, b) => new
               {
                   chatroomid=c.ChatroomId,
                   chatroomuserid = c.ChatroomUserid,
                   Memberfid = b.Bfid,
                   MemberName = b.BusinessName,
                   Membertype = c.UserType,
                   MemberImg = b.LogoImg,
                   LastOnlineTime = c.LastOnlineTime,
                  
               }); 
            //合併User與一般會員
            var NChatRoomUser = ChatRoomuser.Where(u => u.UserType == 0)
               .Join(_context.NormalMembers, c => c.MemberId, n => n.Fid, (c, n) => new
               {
                   chatroomid = c.ChatroomId,
                   chatroomuserid = c.ChatroomUserid,
                   Memberfid = n.Fid,
                   MemberName = n.MemberName,
                   Membertype = c.UserType,
                   MemberImg = n.MemberPhotoFile,
                   LastOnlineTime = c.LastOnlineTime,
                  
               });

               //統一user格式
               List<CChatroomitemViewModel> ChatRoomList = new List<CChatroomitemViewModel>();
            //把BChatRoomUser丟進CChatroomitemViewModel
            foreach (var User in BChatRoomUser)
              {
                CChatroomitemViewModel vm = new CChatroomitemViewModel();
                vm.chatroomid = User.chatroomid;
                vm.chatroomUserid = User.chatroomuserid;
                vm.Memberfid = User.Memberfid;
                vm.MemberName = User.MemberName;
                vm.UserType = User.Membertype;
                vm.MemberImg = "../images/"+User.MemberImg;
                vm.LastOnlineTime = User.LastOnlineTime;
                
                ChatRoomList.Add(vm);
              }
          
            //把NChatRoomUser丟進CChatroomitemViewModel
            foreach (var User in NChatRoomUser)
            {
                CChatroomitemViewModel vm = new CChatroomitemViewModel();
                vm.chatroomid = User.chatroomid;
                vm.chatroomUserid = User.chatroomuserid;
                vm.Memberfid = User.Memberfid;
                vm.MemberName = User.MemberName;
                vm.UserType = User.Membertype;
                vm.MemberImg ="../images/Customer/Member/"+ User.MemberImg;
                vm.LastOnlineTime = User.LastOnlineTime;
               
                ChatRoomList.Add(vm);
            }
            string Json = JsonSerializer.Serialize(ChatRoomList);

            await Clients.Client(Context.ConnectionId).SendAsync("ReNewChatRoom", Json);

        }
        //更新左邊聊天室
        public async Task ChangeChatroom(int otheruserid)
        {
            
            //List<ChatMessage> chatMessages = new List<ChatMessage>();
            //找到自己
            ChatroomUser MyData = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (MyData == null)
                return;
            //有自已的聊天室
            var ChatRoom = _context.Chat2Users.Join(_context.Chat2Users, c1 => c1.Chatid, c2 => c2.Chatid, (c1, c2) => new
            {
                fid = c1.Fid,
                chatroomid = c1.Chatid,
                user = c1.Userid,
                Otheruser = c2.Userid
            }
            ).FirstOrDefault(c => c.user == MyData.ChatroomUserid && c.Otheruser == otheruserid);
            if (ChatRoom == null)
                return;
           var message = _context.ChatMessages.Where(c => c.Chatid == ChatRoom.chatroomid);
            if (message == null)
                return;
            string Json = JsonSerializer.Serialize(message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReNewChatRoomMain", Json);
        }
        //傳訊息
        public async Task SendMessage(string otheruserid, string message)
        {
            int _otheruserid = Convert.ToInt32(otheruserid);
            string json = null;
            
            if (Context.GetHttpContext().Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
            {
           json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_Business));
            BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
              
                 //抓Session裡的member

                 //找聊天室室友這兩個人的
                 var userinchat = _context.Chat2Users.Where(u => u.Userid == member.ChatroomUserid);
            var otheruserinchat = _context.Chat2Users.Where(u => u.Userid == _otheruserid);
            var joinchatroom = userinchat.Join(otheruserinchat, u => u.Chatid, o => o.Chatid, (u, o) => u.Chatid).FirstOrDefault();
            //如果沒找到
            if (joinchatroom == 0)
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
                _context.Chat2Users.Add(chat2User);
                _context.SaveChanges();
                Chat2User chat2Userother = new Chat2User();
                chat2Userother.Chatid = joinchatroom;
                chat2Userother.Userid = _otheruserid;
                _context.Chat2Users.Add(chat2Userother);
                _context.SaveChanges();
            } 

             //對自己發出訊息
               await Clients.Client(Context.ConnectionId).SendAsync("LocalMessage", message); 
            //找出要發給的那個人的資料
            var user = users.FirstOrDefault(u => u.ChatroomUserid == _otheruserid);
            if(user != null)
            {
                //對對方發出訊息
               await Clients.Client(user.ConnectionId).SendAsync("RemoteMessage", member.MemberName,member.ChatroomUserid ,member.Fid, message);
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
            else if (Context.GetHttpContext().Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_USER));
                NormalMember member = JsonSerializer.Deserialize<NormalMember>(json);
                
                //抓Session裡的member

                //找聊天室室友這兩個人的
                var userinchat = _context.Chat2Users.Where(u => u.Userid == member.ChatroomUserid);
                var otheruserinchat = _context.Chat2Users.Where(u => u.Userid == _otheruserid);
                var joinchatroom = userinchat.Join(otheruserinchat, u => u.Chatid, o => o.Chatid, (u, o) => u.Chatid).FirstOrDefault();
                //如果沒找到
                if (joinchatroom == 0)
                {
                    //新增一個聊天室
                    Chatroom Chatroom = new Chatroom();
                    //取名(為了等一下可以找到者個聊天室)
                    Chatroom.ChatName = member.ChatroomUserid + "to" + otheruserid;
                    _context.Chatrooms.Add(Chatroom);
                    _context.SaveChanges();
                    //找到剛剛那個聊天室
                    joinchatroom = _context.Chatrooms.FirstOrDefault(c => c.ChatName == member.ChatroomUserid + "to" + otheruserid).Chatid;
                    //把雙方的ID放進去
                    Chat2User chat2User = new Chat2User();
                    chat2User.Chatid = joinchatroom;
                    chat2User.Userid = (int)member.ChatroomUserid;
                    _context.Chat2Users.Add(chat2User);
                    _context.SaveChanges();
                    Chat2User chat2Userother = new Chat2User();
                    chat2Userother.Chatid = joinchatroom;
                    chat2Userother.Userid = _otheruserid;
                    _context.Chat2Users.Add(chat2Userother);
                    _context.SaveChanges();
                }

                //對自己發出訊息
                await Clients.Client(Context.ConnectionId).SendAsync("LocalMessage", message);
                //找出要發給的那個人的資料
                var user = users.FirstOrDefault(u => u.ChatroomUserid == _otheruserid);
                if (user != null)
                {
                    //對對方發出訊息
                    await Clients.Client(user.ConnectionId).SendAsync("RemoteMessage", member.MemberName, member.ChatroomUserid, member.Fid, message);
                }
                //把訊息存入聊天室
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.Chatid = joinchatroom;
                chatMessage.Senderid = (int)member.ChatroomUserid;
                chatMessage.Message = message;
                chatMessage.SendTime = DateTime.Now;
                _context.ChatMessages.Add(chatMessage);
                _context.SaveChanges();
            }
        }
        //修改連線時的動作(加上自己想要的)
        public override async Task OnConnectedAsync()
        {
            if (users.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault() == null)
            {
                ChatroomUser user = new ChatroomUser();

                if (Context.GetHttpContext().Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
                {
                    string json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_Business));
                    BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
                    user.ChatroomUserid = (int)member.ChatroomUserid;
                    user.ConnectionId = Context.ConnectionId;
                    users.Add(user);
                }
                else if(Context.GetHttpContext().Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    string json = (Context.GetHttpContext().Session.GetString(CDictionary.SK_LOGINED_USER));
                    NormalMember member = JsonSerializer.Deserialize<NormalMember>(json);
                    user.ChatroomUserid = (int)member.ChatroomUserid;
                    user.ConnectionId = Context.ConnectionId;
                    users.Add(user);

                }


            }
             await  base.OnConnectedAsync();
        }
        //離線
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var user = users.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                users.Remove(user);
            }
            await base.OnDisconnectedAsync(ex);
        }
    }
}
