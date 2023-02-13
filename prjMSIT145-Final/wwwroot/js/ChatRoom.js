"use strict";
const ChatNowUserName = document.getElementById("ChatNowUser");
const ChatNowUserid = document.getElementById("ChatNowUserid");
const ChatNowUserChatid = document.getElementById("ChatNowUserChatid");
const chatmessageDiv = document.getElementById("chatmessageDiv");
const chatMessageInput = document.getElementById("chatMessageInput");
const chatMessagebtn = document.getElementById("chatMessagebtn");
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


document.getElementById("chatMessageInput").disabled = true;
/*對方說的*/
connection.on("RemoteMessage", function (otherName, ChatroomUserid, Fid,message) {
    
    var div = document.createElement("div");
    div.setAttribute("class", "chatroomUser remote");
    div.innerHTML= `<div class="chatroomMessage">
                                    ${message}
                            </div>`
    console.log(message);
    document.getElementById("chatmessageDiv").appendChild(div);
    ChatNowUserName.innerHTML = otherName;
    ChatNowUserid.value = `${Fid}`;
    ChatNowUserChatid.value = `${ChatroomUserid}`;
});
/*自己說的*/
connection.on("LocalMessage", function (message) {
    console.log(message);
    var div = document.createElement("div");
    div.setAttribute("class", "chatroomUser local");
    div.innerHTML = `<div class="chatroomMessage">
                                    ${message}
                            </div>`
   
    document.getElementById("chatmessageDiv").appendChild(div);

});


////回傳回來的
//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    li.textContent = `${user} says ${message}`;
//});

connection.start().then(function () {
    console.log("Hub 連線完成");
    document.getElementById("chatMessageInput").disabled = false;
}).catch(function (err) {

    return console.error(err.toString());
});

//傳過去的
document.getElementById("chatMessagebtn").addEventListener("click", function (event) {
   
    var otheruserid = document.getElementById("ChatNowUserChatid").value;
    var message = document.getElementById("chatMessageInput").value;
     console.log("Hub 傳訊息2");
    connection.invoke("SendMessage", otheruserid, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});