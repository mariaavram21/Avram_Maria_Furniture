"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message, date) {
    //console.log(user, message);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g,
        "&gt;");
    //var encodedMsg = user + ": " + msg + " " + date;
    //var li = document.createElement("p");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
    var encodedMsg =  msg ;
    var li = document.createElement("p");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    var encodedMsgUser = user + ": ";
    var li = document.createElement("p");
    li.textContent = encodedMsgUser;
    document.getElementById("messagesUser").appendChild(li);
    var encodedMsgDate =  date;
    var li = document.createElement("p");
    li.textContent = encodedMsgDate;
    document.getElementById("messagesDate").appendChild(li);
});
//connection.on("ReceiveUser", function (user) {
    
//    var encodedMsg = user": ";
//    var li = document.createElement("p");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesUser").appendChild(li);
//});
//connection.on("ReceiveMessage", function (message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g,
//        "&gt;");
//    var encodedMsg = msg;
//    var li = document.createElement("p");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});
//connection.on("ReceiveTime", function (time) {
//    var msg = currentTime.getHours() + ":" + currentTime.getMinutes();
//    var encodedMsg = msg;
//    var li = document.createElement("p");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesTime").appendChild(li);
//});
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    }); event.preventDefault();
});