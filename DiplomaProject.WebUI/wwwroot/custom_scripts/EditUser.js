function getProfessions() {
    const array = [...professions];
    array.push({ Id: -1, Name: '-ընտրեք մասնագիտությունը-' });
    array.sort((x, y) => x.Id - y.Id);
    return array;

}

function getRoles() {
    let array = [ ...roles ];
    array.push({ Id: "-1", DisplayName: '-ընտրեք դերը-' });
    array.sort((x, y) => x.Id - y.Id);
    array = move(array, array.length - 1, 0);
    return array;
}

function addRole() {
    const isOK = isFull(edges);
    let message;
    if (isOK) {
        edges.push({ Id: getNextId(), UserId: userId, RoleId: -1, ProfessionId: -1 });
        message = "";
    }
    else {
        message = "Ոչ բոլոր զույգերն են ամբողջությամբ լրացված: Լրացրեք բոլորը և փորձեք կրկին"
    }
    setMessage(message);
    updateDependencies(edges);
}

function onSubmit(){
    const isOK = isFull(edges);
    let message;
    if (isOK) {
        message = "";
        $.ajax({         
            type: "POST",
            data: JSON.stringify({
                Id: userId,
                UserRoles: edges
            }),
            url: '/Admin/EditConfirmed',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.redirect) {
                    location.href = data.redirect;
                }
            }
        });
    }
    else {
        message = "Ոչ բոլոր զույգերն են ամբողջությամբ լրացված: Լրացրեք բոլորը և փորձեք կրկին"
    }
    setMessage(message);
}

function setMessage(message){
    if (message && message.length) {
        var div = document.getElementById('message');
        while (div.firstChild) {
            div.removeChild(div.firstChild);
        }
        const p = document.createElement('p');
        p.setAttribute('class', 'alert alert-info');
        const icon = document.createElement('span');
        icon.setAttribute('class', 'glyphicon glyphicon-info-sign');
        const span = document.createElement('span');
        span.textContent = message;
        p.appendChild(icon);
        p.appendChild(span);
        div.appendChild(p);
    }
}
function removeDependency(edge, index) {
    edges =  edges.filter((item, index) => item.Id !== edge.Id);
    updateDependencies(edges);
}

function onChange(edgeId, property, value) {
    const edge = edges.find(e => e.Id === edgeId);
    edge[property] = value;
}

function updateDependencies(edges) {
    const fields = edges.map((edge, index) => {
        const div = document.createElement('div');
        div.setAttribute('class', 'dependencyRow row');

        const fromDiv = document.createElement('div');
        fromDiv.setAttribute('class', 'col-md-5 form-group');

        const fromSelect = document.createElement('select');
        fromSelect.setAttribute('class', 'form-control');
        fromSelect.style.width = '100%';
        fromSelect.addEventListener('change', (e) => onChange(edge.Id, 'ProfessionId', e.target.value));

        const toDiv = document.createElement('div');
        toDiv.setAttribute('class', 'col-md-5 form-group');

        const toSelect = document.createElement('select');
        toSelect.setAttribute('class', 'form-control');
        toSelect.style.width = '100%';
        toSelect.addEventListener('change', (e) => onChange(edge.Id, 'RoleId', e.target.value));

        const deleteDiv = document.createElement('div');
        deleteDiv.setAttribute('class', 'col-md-2');
        const deleteLink = document.createElement('a');
        deleteLink.addEventListener('click', (e) => removeDependency(edge, index));
        const deleteIcon = document.createElement('span');
        deleteIcon.setAttribute('class', 'glyphicon glyphicon-remove-circle text-danger');
        deleteLink.appendChild(deleteIcon);
        deleteDiv.appendChild(deleteLink);

        const professions = getProfessions();
        const roles = getRoles();
        (professions || []).forEach(profession => {
            const option = document.createElement('option');
            option.value = profession.Id;
            option.text = profession.Name;

            fromSelect.appendChild(option);
        });
        fromSelect.value = edge.ProfessionId;
        fromDiv.appendChild(fromSelect);

        (roles || []).forEach(role => {
            const option = document.createElement('option');
            option.value = role.Id;
            option.text = role.DisplayName;

            toSelect.appendChild(option);
        });
        toSelect.value = edge.RoleId;
        toDiv.appendChild(toSelect);

        div.appendChild(fromDiv);
        div.appendChild(toDiv);
        div.appendChild(deleteDiv);
        return div;
    });

    const dependencies = document.getElementById('roleProfessions');
    while (dependencies.firstChild) {
        dependencies.removeChild(dependencies.firstChild);
    }
    fields.forEach(field => document.getElementById('roleProfessions').appendChild(field));
}

function isFull(edges) {
    let isOK = true;
    for (let i = 0, length = edges.length; i < length; i++) {
        if (!edges[i].RoleId || edges[i].RoleId < 0 || !edges[i].ProfessionId || edges[i].ProfessionId < 0) {
            isOK = false;
        }
    }
    return isOK;
}

function getNextId() {
    return nextId--;
}