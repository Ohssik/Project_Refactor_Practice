

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


document.getElementById("chatMessageInput").disabled = true;
/*對方說的*/
connection.on("RemoteMessage", function (message) {
    var Fragment = document.createDocumentFragment();
    Fragment.innerHTML = `<div class="chatroomUser remote">
                            <div class="chatroomMessage">
                                    ${message}
                            </div>
                          </div>`
    document.getElementById("chatmessageDiv").appendChild(Fragment);
    
    
   
});
/*自己說的*/
connection.on("LocalMessage", function (message) {
    var Fragment = document.createDocumentFragment();
    Fragment.innerHTML = `<div class="chatroomUser local">
									<div class="chatroomMessage">
										 ${message}
									</div>
								</div>`
    document.getElementById("chatmessageDiv").appendChild(Fragment);
});


////回傳回來的
//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    li.textContent = `${user} says ${message}`;
//});

connection.start().then(function () {
    document.getElementById("chatMessageInput").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//傳過去的
document.getElementById("chatMessagebtn").addEventListener("click", function (event) {
    var user = document.getElementById("ChatNowUserChatid").value;
    var message = document.getElementById("chatMessageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});