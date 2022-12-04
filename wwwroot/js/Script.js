var connection = new signalR.HubConnectionBuilder().withUrl("/BridgeController").build();
connection.start();

/* Drag&Drop */
let target = null;
document.addEventListener("mousedown", item => {
    try {
        if (!item.target.classList.contains("item") && item.target.parentElement.classList.contains("item")) {
            target = item.target.parentElement;
        }
        else if (item.target.classList.contains("item")) {
            target = item.target;
        }
        if (target.classList.contains("item")) {
            RemoveSwap(true);
        }
    } catch {}
});
document.addEventListener("dragenter", item => {
    if (item.target.classList.contains("section-items")) {
        item.target.classList.add("dragover");
    }
});
document.addEventListener("dragleave", item => {
    if (item.target.classList.contains("dragover")) {
        item.target.classList.remove("dragover");
    }
});
document.addEventListener("dragover", item => {
    item.preventDefault();
});
document.addEventListener("drop", item => {
    try {
        if (item.target.classList.contains("section-items")) {
            connection.invoke("Transfer", Number(target.id), Number(item.target.id.split('_')[1]));
            item.target.appendChild(target);
            item.target.classList.remove("dragover");
        }
        target = null;
    } catch {}
});
connection.on("Transfer", function (msg) {
    console.log(msg);
});
connection.on("Remove", function (msg) {
    console.log(msg);
    window.location.reload();
});


//Create item
let dialog = document.getElementById("dialog");
document.addEventListener("click", item => {
    if (item.target.classList.contains("removebtn")) {
        Remove(item.target.parentElement.id);
    }
    if (item.target.classList.contains("addbtn")) {
        dialog.showModal();
    }
    if (item.target.classList.contains("close-btn")) {
        dialog.close();
    }
});
function CreateItem() {
    let Title = document.getElementById("title").value.replace(`'`, '').replace(`"`, '').replace(' ', '');
    if (Title.length < 2) {
        Title = document.getElementById("title");
        Title.placeholder = "Заполните это поле";
        Title.value = null;
        return;
    }
    else if (Title.length > 30) {
        Title = document.getElementById("title");
        Title.placeholder = "Максимальная длина строки - 30";
        Title.value = null;
        return;
    }
    let Descr = document.getElementById("descr");
    Descr.value.replace(`'`, '').replace(`"`, '').replace(' ', '');
    if (Descr.value.length > 30) {
        Descr.placeholder = "Максимальная длина строки - 30";
        Descr.value = null;
        return;
    }
    Descr = Descr.value;
    let Color = document.getElementById("clr").value;
    connection.invoke("Add", 1, Title, Descr, Color);
}

connection.on("Add", function (msg) {
    console.log(msg);
    window.location.reload();
});
//Remove
function Remove(id) {
    connection.invoke("Remove", id);
}
connection.on("Remove", function (msg) {
    console.log(msg);
    window.location.reload();
});
