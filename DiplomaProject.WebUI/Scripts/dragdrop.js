function drag(event) {
    event.dataTransfer.setData("text", event.target.innerText)

}

function allowDrop(ev) {
    ev.preventDefault();
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    ev.target.innerText = data;//.appendChild(document.getElementById(data));
}