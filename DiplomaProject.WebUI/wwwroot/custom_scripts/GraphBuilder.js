function getOptions() {
    const array = JSON.parse(sessionStorage.getItem('nodes'));
    array.push({ Id: -1, Name: '-select outcome-' });
    array.sort((x, y) => x.Id - y.Id);
    return array;

}
function getEdges() {
    const edges = sessionStorage.getItem('edges');
    return edges && JSON.parse(edges);
}
function onChange(edgeId, property, value) {
    const edges = getEdges();
    const edge = edges.find(e => e.id === edgeId);
    edge[property] = parseInt(value);
    updateDependencies(edges);
}

function updateDependencies(edges) {
    sessionStorage.setItem('edges', JSON.stringify(edges));
    const fields = edges.map(edge => {
        let div = document.createElement('div');
        div.setAttribute('class', 'dependencyRow row');

        let fromSelect = document.createElement('select');
        fromSelect.setAttribute('class', 'col-md-6 select-box');
        fromSelect.addEventListener('change', (e) => onChange(edge.id, 'fromNode', e.target.value));

        let toSelect = document.createElement('select');
        toSelect.setAttribute('class', 'col-md-6 select-box');
        toSelect.addEventListener('change', (e) => onChange(edge.id, 'toNode', e.target.value));

        fromSelect.value = edge.fromNode;
        toSelect.value = edge.toNode;

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
        fromSelect.value = edge.fromNode;
        toSelect.value = edge.toNode;
        div.appendChild(fromSelect);
        div.appendChild(toSelect);
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
    let isOK = true;
    for (let i = 0, length = edges.length; i < length; i++) {
        if (!edges[i].fromNode || edges[i].fromNode < 0 || !edges[i].toNode || edges[i].toNode < 0) {
            isOK = false;
        }
    }
    let message;
    if (isOK) {
        edges.push({ id: getNextId(), fromNode: -1, toNode: -1 });
        message = "";
    }
    else {
        message = "There are no selected lists. Please select them and try again."
    }
    document.getElementById('message').innerText = message;

    updateDependencies(edges);
}

function getNextId() {
    const id = parseInt(sessionStorage.getItem('nextId'));
    sessionStorage.setItem('nextId', (id - 1).toString());
    return id;
}