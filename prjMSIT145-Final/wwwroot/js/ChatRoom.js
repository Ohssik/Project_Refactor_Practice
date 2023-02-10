"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


document.getElementById("chatMessageInput").disabled = true;
/*對方說的*/
connection.on("RemoteMessage", function (message) {
    
    var div = document.createElement("div");
    div.setAttribute("class", "chatroomUser remote");
    div.innerHTML= `<div class="chatroomMessage">
                                    ${message}
                            </div>`
   
    document.getElementById("chatmessageDiv").appendChild(div);
    
    
   
});
/*自己說的*/
connection.on("LocalMessage", function (message) {
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
    connection.invoke("SendMessageto", otheruserid, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});