"use strict";
const ChatNowUserName = document.getElementById("ChatNowUser");
const ChatNowUserid = document.getElementById("ChatNowUserid");
const ChatNowUserChatid = document.getElementById("ChatNowUserChatid");
const ChatMessageul = document.getElementById("ChatMessageul");
const chatMessageInput = document.getElementById("chatMessageInput");
const chatMessagebtn = document.getElementById("chatMessagebtn");
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


/*document.getElementById("chatMessageInput").disabled = true;*/
/*對方說的*/
connection.on("RemoteMessage", function (otherName, ChatroomUserid,Fid,message) {
    
    var li = document.createElement("li");
    const ListChatMessageil = ChatMessageul.lastElementChild;
    li.setAttribute("class", "d-flex mb-2 mt-2");
    console.log(ListChatMessageil);
    ChatNowUserName.innerHTML = otherName;
    ChatNowUserid.value = `${Fid}`;
    ChatNowUserChatid.value = `${ChatroomUserid}`;
    if (ListChatMessageil == null) {
        li.innerHTML = ` <img src="#"
                                     alt="#"
                                     class="rounded-circle d-flex align-self-start me-3 shadow-1-strong mt-2"
                                     width="35"
                                    />
                                <div class="card remotemessage">
                                    <div class="card-body p-2">
                                        <p class="mb-0 small">
                                            ${message}
                                        </p>
                                    </div>
                                </div>
                              `
       
    }
    else
    {
if (ListChatMessageil.getAttribute("class") == "d-flex mb-2 mt-2") {

        li.innerHTML = ` <div class="rounded-circle d-flex align-self-start me-3 shadow-1-strong mt-2 "style="padding-right:35px">
                                </div>
                                <div class="card remotemessage">
                                    <div class="card-body p-2">
                                        <p class="mb-0 small">
                                             ${message}
                                        </p>
                                    </div>
                                </div>
                              `
        
    }
    else {
        li.innerHTML = `<img src="#"
                                     alt="#"
                                     class="rounded-circle d-flex align-self-start me-3 shadow-1-strong mt-2"
                                     width="35"
                                    />
                                <div class="card remotemessage">
                                    <div class="card-body p-2">
                                        <p class="mb-0 small">
                                            ${message}
                                        </p>
                                    </div>
                                </div>
                              `
        
    }
    }
    
    
   
    
    
    console.log(message);
    document.getElementById("ChatMessageul").appendChild(li);
   
});
/*自己說的*/
connection.on("LocalMessage", function (message) {
    console.log(message);
    var li = document.createElement("li");
    li.setAttribute("class", "d-flex justify-content-between mb-2 mt-2 flex-row-reverse");
    li.innerHTML =` <div class="card  localmessage" >
                                    <div class="card-body p-2">
                                        <p class="m-0 small text-end">
                                            ${message}
                                        </p>
                                    </div>
                                </div>`

   
    document.getElementById("ChatMessageul").appendChild(li);

});


////回傳回來的
//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    li.textContent = `${user} says ${message}`;
//});

connection.start().then(function () {
    console.log("Hub 連線完成");
    document.getElementById("chatMessagebtn").disabled = false;
}).catch(function (err) {

    return console.error(err.toString());
});

//傳過去的
document.getElementById("chatMessagebtn").addEventListener("click", function (event) {
   
    var otheruserid = document.getElementById("ChatNowUserChatid").value;
    var message = document.getElementById("chatMessageInput").value;
     console.log("Hub 傳訊息");
    connection.invoke("SendMessage", otheruserid, message).catch(function (err) {
        return console.error(err.toString());
    }); 
    chatMessageInput.value = "";
    event.preventDefault();
});