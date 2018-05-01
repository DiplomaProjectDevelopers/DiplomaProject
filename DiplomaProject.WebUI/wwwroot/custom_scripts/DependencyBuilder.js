function getOptions() {
    let array = JSON.parse(sessionStorage.getItem('nodes'));
    array.push({ Id: -1, Name: '-ընտրեք վերջնարդյունքը-' });
    array.sort((x, y) => x.Id - y.Id);
    return array;

}
function getEdges() {
    const edges = sessionStorage.getItem('edges');
    return edges && JSON.parse(edges);
}

function onChange(edgeId, property, value) {
    const edges = getEdges();
    const edge = edges.find(e => e.Id === edgeId);
    edge[property] = parseInt(value);
    updateDependencies(edges);
}

function onSubmit() {
    const data = getEdges();
    const isOK = isFull(data);
    if (!isOK) {
        const message = "Ոչ բոլոր զույգերն են ամբողջությամբ լրացված: Լրացրեք բոլորը և փորձեք կրկին"
        document.getElementById('message').innerText = message;
        return;
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(data),
        url: "/Outcomes/SaveDependencies",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            const message = "Փոփոխությունները հաջողությամբ պահպանվեցին!!!";
            document.getElementById('message').innerText = message;
            updateDependencies(data);
        },
        error: function (error) {
            console.log("Error occured!!");
            console.log(error);
        }
    });
}

function removeDependency(edge, index) {
    const previousEdges = JSON.parse(sessionStorage.getItem('edges'));
    const currentEdges = previousEdges.filter((item, index) => item.Id !== edge.Id);
    updateDependencies(currentEdges);
}

function updateDependencies(edges) {
    sessionStorage.setItem('edges', JSON.stringify(edges));
    const nodes = JSON.parse(sessionStorage.getItem('nodes'));
    const fields = edges.map((edge, index) => {
        const div = document.createElement('div');
        div.setAttribute('class', 'dependencyRow row');

        const fromDiv = document.createElement('div');
        fromDiv.setAttribute('class', 'col-md-5');

        const fromSelect = document.createElement('select');
        fromSelect.setAttribute('class', 'select-box');
        fromSelect.style.width = '100%';
        fromSelect.addEventListener('change', (e) => onChange(edge.Id, 'FromNode', e.target.value));

        const toDiv = document.createElement('div');
        toDiv.setAttribute('class', 'col-md-5');

        const toSelect = document.createElement('select');
        toSelect.setAttribute('class', 'col-md-5 select-box');
        toSelect.style.width = '100%';
        toSelect.addEventListener('change', (e) => onChange(edge.Id, 'ToNode', e.target.value));

        const deleteDiv = document.createElement('div');
        deleteDiv.setAttribute('class', 'col-md-2');
        const deleteLink = document.createElement('a');
        deleteLink.addEventListener('click', (e) => removeDependency(edge, index));
        const deleteIcon = document.createElement('span');
        deleteIcon.setAttribute('class', 'glyphicon glyphicon-remove-circle');
        deleteLink.appendChild(deleteIcon);
        deleteDiv.appendChild(deleteLink);

        const options = getOptions();
        (options || []).forEach(option => {

            const option1 = document.createElement('option');
            option1.value = option.Id;
            option1.text = option.Name;

            const option2 = document.createElement('option');
            option2.value = option.Id;
            option2.text = option.Name;

            if (option.IsNew) {
                option1.setAttribute('class', 'isNewOutComeOption');
                option2.setAttribute('class', 'isNewOutComeOption');
            }
            fromSelect.appendChild(option1);
            toSelect.appendChild(option2);
        });
        fromSelect.value = edge.FromNode;
        fromDiv.appendChild(fromSelect);

        toSelect.value = edge.ToNode;
        toDiv.appendChild(toSelect);

        div.appendChild(fromDiv);
        div.appendChild(toDiv);
        div.appendChild(deleteDiv);
        return div;
    });
    const dependencies = document.getElementById('dependencies');
    while (dependencies.firstChild) {
        dependencies.removeChild(dependencies.firstChild);
    }
    fields.forEach(field => document.getElementById('dependencies').appendChild(field));
}


function addDependency() {
    const edges = getEdges();
    const isOK = isFull(edges);
    let message;
    if (isOK) {
        edges.push({ Id: getNextId(), FromNode: -1, ToNode: -1, professionId });
        message = "";
    }
    else {
        message = "Ոչ բոլոր զույգերն են ամբողջությամբ լրացված: Լրացրեք բոլորը և փորձեք կրկին"
    }
    document.getElementById('message').innerText = message;

    updateDependencies(edges);
}

function isFull(edges) {
    let isOK = true;
    for (let i = 0, length = edges.length; i < length; i++) {
        if (!edges[i].FromNode || edges[i].FromNode < 0 || !edges[i].ToNode || edges[i].ToNode < 0) {
            isOK = false;
        }
    }
    return isOK;
}

function getNextId() {
    const id = parseInt(sessionStorage.getItem('nextId'));
    sessionStorage.setItem('nextId', (id - 1).toString());
    return id;
}