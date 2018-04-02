function getOptions() {
    const array = JSON.parse(sessionStorage.getItem('nodes'));
    array.push({ Id: -1, Name: '-select-' });
    array.sort((x, y) => x.Id - y.Id);
    return array;

}
function getEdges(){
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
        div.setAttribute('class', 'dependencyRow')
        let fromSelect = document.createElement('select');
        fromSelect.setAttribute('class', 'col-md-6 fromSelectDropdownList');
        fromSelect.addEventListener('change', (e) => onChange(edge.id, 'fromNode', e.target.value));
        let toSelect = document.createElement('select');
        toSelect.setAttribute('class', 'col-md-6 fromSelectDropdownList');
        toSelect.addEventListener('change', (e) => onChange(edge.id, 'toNode', e.target.value));
        let options = getOptions();
        (options || []).forEach(option => {
            const opt = document.createElement('option');
            opt.value = option.Id;
            opt.text = option.Name;
            fromSelect.appendChild(opt);
            const o = document.createElement('option');
            o.value = option.Id;
            o.text = option.Name;
            toSelect.appendChild(o);
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
        if (!edges[i].fromNode || !edges[i].toNode) {
            isOK = false;
        }
    }
    isOK && edges.push({ id: getNextId(), fromNode: "", toNode: "" });
    updateDependencies(edges);
}

function getNextId() {
    const id = parseInt(sessionStorage.getItem('nextId'));
    sessionStorage.setItem('nextId', (id - 1).toString());
    return id;
}