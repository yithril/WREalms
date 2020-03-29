const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub?LivingID=@ViewBag.LivingID&PlayerName=@ViewBag.Name")
    .configureLogging(signalR.LogLevel.Information)
    .build();

function ScrollDown() {
    $('#gameRoll').animate({ scrollTop: $('#gameRoll').prop("scrollHeight") }, 500);
}

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("gameRoll").appendChild(li);
    ScrollDown();
});

connection.on("ReceiveErrorMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("gameRoll").appendChild(li);
    ScrollDown();
});

connection.on("ReceiveSkillsMessage", function (skills) {

    var li2 = document.createElement("li");
    li2.textContent = "[ Skill List ]";
    li2.style.color = "#00008B";
    li2.style.fontWeight = "bold";
    document.getElementById("gameRoll").appendChild(li2);

    skills.forEach(function (item) {
        var li = document.createElement("li");
        li.textContent = "[" + item.skill.name + "]: " + item.level;
        document.getElementById("gameRoll").appendChild(li);
        ScrollDown();
    });

});

connection.on("ReceiveInventoryMessage", function (itemList, current, max) {
    var li = document.createElement("li");

    if (itemList.length < 1) {
        li.textContent = "You are not carrying anything. " + "(" + current + " dimins/" + max + " dimins)"
    } else {
        li.textContent = "You are carrying " + GetListDescription(itemList) + " (" + current + " dimins/" + max + " dimins)";
    }

    document.getElementById("gameRoll").appendChild(li);
    ScrollDown();
});

connection.on("ReceiveRoomMessage", function (room) {
    console.log('Loading Room...')
    var li = document.createElement("li");
    li.textContent = room.title;
    li.style.fontWeight = "bold";

    var li2 = document.createElement("li");
    li2.textContent = room.description;

    var li3 = document.createElement("li");
    li3.textContent = "Exits: " + GetExitDescription(room.roomExits);
    li3.style.color = "#00008b";
    li3.style.fontWeight = "bold";

    document.getElementById("gameRoll").appendChild(li);
    document.getElementById("gameRoll").appendChild(li2);

    if (room.npcList.length > 0) {
        var li5 = document.createElement("li");
        li5.textContent = "You see " + GetListDescription(room.npcList);
        document.getElementById("gameRoll").appendChild(li5);
    }

    if (room.pCs.length > 1) {
        var li4 = document.createElement("li");
        li4.textContent = "[Players:] " + GetListDescription(room.pCs);
        li4.style.fontWeight = "bold";
        document.getElementById("gameRoll").appendChild(li4);
    }

    if (room.itemList.length > 0) {
        var li5 = document.createElement("li");
        li5.textContent = "You see " + GetListDescription(room.itemList) + " on the floor.";
        document.getElementById("gameRoll").appendChild(li5);
    }

    document.getElementById("gameRoll").appendChild(li3);

    $('#gameRoll').animate({ scrollTop: $('#gameRoll').prop("scrollHeight") }, 500);
});

connection.on("ReceiveActionMessage", function (subject, verb, obj) {
    var li = document.createElement("li");
    li.textContent = subject + " " + verb + " " + obj + ".";
    document.getElementById("gameRoll").appendChild(li);
    ScrollDown();
});

connection.on("ReceiveLimbsMessage", function (limbs) {

    var li2 = document.createElement("li");
    li2.textContent = "[ Limbs ]";
    li2.style.color = "#00008B";
    li2.style.fontWeight = "bold";
    document.getElementById("gameRoll").appendChild(li2);

    limbs.forEach(function (arrayItem) {
        var display = "[" + arrayItem.name + "]: ";

        if (arrayItem.equippedItem != null) {
            display += arrayItem.equippedItem.Name;
        } else {
            display += "Nothing";
        }

        var li = document.createElement("li");
        li.textContent = display;
        li.style.color = "#4f3f8c";
        document.getElementById("gameRoll").appendChild(li);
        ScrollDown();
    });

});

connection.start().then(function () {
    console.log("connected");
    connection.invoke("GetAllConnectedUsers").then((result) => SetUsers(result)).catch(err => console.error(err.toString()));
});

document.getElementById("btnSend").addEventListener("click", function (event) {
    var user = document.getElementById("userName").value;
    var message = document.getElementById("userInput").value;
    connection.invoke("ParseInput", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    $("#userInput").val('');
    $("#userInput").focus();
    event.preventDefault();
});

$(document).on('keypress', function (e) {
    if (e.which == 13) {
        $("#btnSend").click();
    }
});

function SetUsers(userList) {
    userList.forEach(function (item, index) {
        var li = document.createElement("li");
        li.textContent = item;
        document.getElementById("user-list").appendChild(li);
    });
}

function GetListDescription(input) {

    var list = input.filter(function (x) {
        if (x.Name == "@ViewBag.Name") {
            return false;
        }
        return true;
    });

    return list.map(function (elem) { return elem.name }).join(', ').replace(/, ([^,]*)$/, ' and $1');
}

function GetExitDescription(list) {
    return list.map(function (elem) { return elem.exitDesc }).join(', ').replace(/, ([^,]*)$/, ' and $1');
}