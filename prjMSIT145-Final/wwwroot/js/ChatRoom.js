﻿"use strict";
const ChatNowUserName = document.getElementById("ChatNowUser");
const ChatNowUserid = document.getElementById("ChatNowUserid");
const ChatNowUserChatid = document.getElementById("ChatNowUserChatid");
const ChatMessageul = document.getElementById("ChatMessageul");
const chatMessageInput = document.getElementById("chatMessageInput");
const chatMessagebtn = document.getElementById("chatMessagebtn");
const ChatroomItemul = document.getElementById("ChatroomItem");
const ChatNowUserimg = document.getElementById("ChatNowUserimg");

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



document.getElementById("showChatRoomBtn").setAttribute("style", "display:none");
/*完成連線*/
connection.start().then(function () {
    console.log("Hub 連線完成");
   
    document.getElementById("showChatRoomBtn").setAttribute("style", "right:10px;bottom:0px;width:90px")
    //測試
    
        
    
}).catch(function (err) {
   
    return console.error(err.toString());
});
//按下聊天室後
document.getElementById("showChatRoomBtn").addEventListener("click", async function () {
    clickChatroomBtn();
 
});
//回傳聊天室的Item
connection.on("ReNewChatRoom", function (data) {
           
    console.log(JSON.parse(data))
    JSON.parse(data).forEach(function (item)
    {
        var li = document.createElement("li");
        li.setAttribute("class", "p-2 border-bottom");
        
        li.innerHTML =`
                                    <a href="#!" class="d-flex justify-content-between">
                                        <div class="d-flex flex-row" id="chatroomitem${item.chatroomUserid}">
                                            <img src="${location.protocol + "//" + location.hostname + ":" + location.port+item.MemberImg}"
                                                 alt="avatar"
                                                 class="rounded-circle d-flex align-self-center me-3 shadow-1-strong"
                                                 width="30" />
                                            <div class="pt-1" style="max-width: 150px">
                                                <p class="fw-bold mb-0 small">${item.MemberName}</p>
                                               
                                                <p class="small m-0 text-truncate" id="${"Lastmessage" + item.chatroomUserid}">
                                                 ${item.LastMessage}
                                                </p>
                                            </div>
                                        </div>
                                        <div class="pt-1">
                                            <span class="badge bg-danger float-end"id="${"span" + item.chatroomUserid}"></span>
                                            <p class="small text-muted mb-1"></p>
                                        </div>
                                    </a>
                                `
        li.addEventListener("click", function () {
            ChatNowUserName.innerHTML = item.MemberName;
            ChatNowUserid.value = item.Memberfid;
            ChatNowUserChatid.value = item.chatroomUserid;
            document.getElementById("ChatNowUserimg").value = item.MemberImg;
            ChangeChatroom(item.chatroomUserid);
        });
        document.getElementById("ChatroomItem").appendChild(li);
    })



})
//聊天室改變時
function ChangeChatroom(otheruserid) {
    document.getElementById("ChatMessageul").innerHTML = "";
    document.getElementById(`span${otheruserid}`).innerHTML = "";
    connection.invoke("ChangeChatroom", otheruserid).catch(function (err) {
        return console.error(err.toString());
    });
}
//聊天室載入聊天紀錄
connection.on("ReNewChatRoomMain", function (data) {
    var messageData = JSON.parse(data);
    document.getElementById("ChatMessageul").innerHTML = "";
    messageData.forEach(function (item) {
        if (item.Senderid == ChatNowUserChatid.value) {
            remotemessageShow(item.Message);
        }
        else
        {
            
            localmessageShow(item.Message)
        }
    })
    document.getElementById("ChatMessageul").lastElementChild.scrollIntoView({ behavior: "smooth" })
})
//按下訊息送出後
document.getElementById("chatMessagebtn").addEventListener("click", function (event) {

    var otheruserid = document.getElementById("ChatNowUserChatid").value;
    var message = document.getElementById("chatMessageInput").value;
    console.log(document.getElementById(`chatroomitem${ChatNowUserChatid.value}`));
    connection.invoke("SendMessage", otheruserid, message).catch(function (err) {
        return console.error(err.toString());
    });
    chatMessageInput.value = "";
    event.preventDefault();

});
/*回傳對方說的*/
connection.on("RemoteMessage", function (otherName, ChatroomUserid, Fid, message, img) {
    // ChatNowUserName.innerHTML = otherName;
    //ChatNowUserid.value = `${Fid}`;
    //ChatNowUserChatid.value = `${ChatroomUserid}`;
    //console.log(ListChatMessageil);
    if (document.getElementById("showChatRoomBtn").getAttribute("style") == "right:10px;bottom:0px;width:90px") {
        document.getElementById("newMessageAlarm").setAttribute("class", "position-absolute top-0 start-100 translate-middle badge border border-light rounded-circle bg-danger p-2");
        
    }
    
    if (document.getElementById(`chatroomitem${ChatroomUserid}`) == null) {
        const li = document.createElement("li");
        li.innerHTML = `
                                    <a href="#!" class="d-flex justify-content-between">
                                        <div class="d-flex flex-row" id="chatroomitem${ChatroomUserid}">
                                            <img src="${location.protocol + "//" + location.hostname + ":" + location.port + img}"
                                                 alt="avatar"
                                                 class="rounded-circle d-flex align-self-center me-3 shadow-1-strong"
                                                 width="30" />
                                            <div class="pt-1" style="max-width: 150px">
                                                <p class="fw-bold mb-0 small">${otherName}</p>
                                              
                                                <p class="small m-0 text-truncate" id="${"Lastmessage" + ChatroomUserid}">
                                                  ${message}
                                                </p>
                                            </div>
                                        </div>
                                        <div class="pt-1">
                                            <span class="badge bg-danger float-end"id="${"span" + ChatroomUserid}">1</span>
                                            <p class="small text-muted mb-1"></p>
                                        </div>
                                    </a>
                                `;
        li.addEventListener("click", function () {
            ChatNowUserName.innerHTML = otherName;
            ChatNowUserid.value = Fid;
            ChatNowUserChatid.value = ChatroomUserid;
            document.getElementById("ChatNowUserimg").value = img;
            ChangeChatroom(ChatroomUserid);
        });
        ChatroomItemul.insertBefore(li, ChatroomItemul.firstChild);
    }
    else {
      
        const cosli = document.getElementById(`chatroomitem${ChatroomUserid}`).parentElement.parentElement
       
        //插到第一行
        ChatroomItemul.insertBefore(cosli, ChatroomItemul.firstChild);
       
       
    if (ChatNowUserChatid.value == ChatroomUserid)
    {
        remotemessageShow(message);
    }
    else
    {
        if (document.getElementById(`span${ChatroomUserid}`).innerHTML == "") {
            document.getElementById(`span${ChatroomUserid}`).innerHTML = 1;
        }
        else {
        document.getElementById(`span${ChatroomUserid}`).innerHTML++;
        }
       
    }
    }

    document.getElementById(`${"Lastmessage" + ChatroomUserid}`).innerHTML = message
   
   
});
/*回傳自己說的*/
connection.on("LocalMessage", function (message) {
    
    if (document.getElementById(`chatroomitem${ChatNowUserChatid.value}`) == null) {
        const li = document.createElement("li");
        li.innerHTML = `
                                    <a href="#!" class="d-flex justify-content-between">
                                        <div class="d-flex flex-row" id="chatroomitem${ChatNowUserChatid.value}">
                                            <img src="${location.protocol + "//" + location.hostname + ":" + location.port + ChatNowUserimg.value}"
                                                 alt="avatar"
                                                 class="rounded-circle d-flex align-self-center me-3 shadow-1-strong"
                                                 width="30" />
                                            <div class="pt-1" style="max-width: 150px">
                                                <p class="fw-bold mb-0 small">${ChatNowUserName.innerHTML}</p>
                                              
                                                <p class="small m-0 text-truncate" id="${"Lastmessage" + ChatNowUserChatid.value}">
                                                  ${message}
                                                </p>
                                            </div>
                                        </div>
                                        <div class="pt-1">
                                            <span class="badge bg-danger float-end"id="${ "span" + ChatNowUserChatid}" ></span>
                                            <p class="small text-muted mb-1"></p>
                                        </div>
                                    </a>
                                `;
        
        ChatroomItemul.insertBefore(li, ChatroomItemul.firstChild);
    }
    else
    {
      
        const cosli = document.getElementById(`chatroomitem${ChatNowUserChatid.value}`).parentElement.parentElement
        //插到第一行
        ChatroomItemul.insertBefore(cosli, ChatroomItemul.firstChild);

    }

    localmessageShow(message);
    document.getElementById(`${"Lastmessage" + ChatNowUserChatid.value}`).innerHTML = message;
    
});
//對方說的話
function remotemessageShow(message) {
    var li = document.createElement("li");
    const ListChatMessageil = ChatMessageul.lastElementChild;
    li.setAttribute("class", "d-flex mb-2 mt-2");
    if (ListChatMessageil == null) {
      
        li.innerHTML = ` <img src="${location.protocol + "//" + location.hostname + ":" + location.port+ChatNowUserimg.value}"
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
    else {
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
            li.innerHTML = `<img src="${location.protocol + "//" + location.hostname + ":" + location.port+ChatNowUserimg.value}"
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
    document.getElementById("ChatMessageul").appendChild(li);
    document.getElementById("ChatMessageul").lastElementChild.scrollIntoView({ behavior: "smooth" })
}
//自己說的話
function localmessageShow(message)
{
    var li = document.createElement("li");
    li.setAttribute("class", "d-flex justify-content-between mb-2 mt-2 flex-row-reverse");
    li.innerHTML = ` <div class="card  localmessage" >
                                    <div class="card-body p-2">
                                        <p class="m-0 small text-end">
                                            ${message}
                                        </p>
                                    </div>
                                </div>`
    document.getElementById("ChatMessageul").appendChild(li);
    document.getElementById("ChatMessageul").lastElementChild.scrollIntoView({ behavior: "smooth" })
}
//按下聊天室
function clickChatroomBtn()
{
    if (document.getElementById("showChatRoomBtn").getAttribute("style") == "right:10px;bottom:0px;width:90px") {
        document.getElementById("newMessageAlarm").setAttribute("class", "position-absolute top-0 start-100 translate-middle badge border border-light rounded-circle bg-danger p-2 d-none")
        document.getElementById("showChatRoomBtn").setAttribute("style", "right:10px;bottom:530px;width:90px");
        ChatroomItem.innerHTML = "";
    connection.invoke("ReNewChatRoom").catch(function (err) {
            return console.error(err.toString());
        }); 
    }
    else {
        document.getElementById("showChatRoomBtn").setAttribute("style", "right:10px;bottom:0px;width:90px");
        ChatroomItem.innerHTML = "";
    }
    console.log(location.protocol +"//"+ location.hostname + ":" + location.port)
}
