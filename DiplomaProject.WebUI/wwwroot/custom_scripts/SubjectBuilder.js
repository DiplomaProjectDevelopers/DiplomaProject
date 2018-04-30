function renderCards() {
    const cards = document.getElementById('card-list');
    while (cards.firstChild) {
        cards.removeChild(cards.firstChild);
    }
    const blocks = [];
    for (let i = 0; i < model.subjects.length; i++) {
        const block = document.createElement('div');
        block.setAttribute('class', 'col-md-3 col-md-offset-1');
        const panel = document.createElement('div');
        panel.setAttribute('class', 'panel panel-primary');
        const heading = document.createElement('div');
        heading.innerText = model.subjects[i].name;
        heading.setAttribute('class', 'panel-heading');
        panel.appendChild(heading);
        const body = document.createElement('div');
        body.setAttribute('class', 'panel-body');
        const module = document.createElement('div');
        module.setAttribute('class', 'form-group');
        const moduleLabel = document.createElement('label');
        moduleLabel.setAttribute('class', 'control-label');
        moduleLabel.innerText = 'Առարկայական մոդուլ';
        module.appendChild(moduleLabel);
        const moduleSelect = document.createElement('select');
        moduleSelect.addEventListener('change', e => {
            model.subjects[i].subjectModuleId = e.target.value;
        });
        moduleSelect.setAttribute('class', 'form-control');
        moduleSelect.required = true;
        const emptyOption = document.createElement('option');
        emptyOption.value = -1;
        emptyOption.text = 'Ընտրեք մոդուլը';
        moduleSelect.appendChild(emptyOption);
        const groups = groupBy(modules, m => m.group);
        groups.forEach((values, key) => {
            const optgroup = document.createElement('optgroup');
            optgroup.label = key;
            values.forEach(module => {
                const option = document.createElement('option');
                option.value = module.id;
                option.text = module.name;
                optgroup.appendChild(option);
            });
            moduleSelect.appendChild(optgroup);
        });
        moduleSelect.value = model.subjects[i].subjectModuleId || -1;
        module.appendChild(moduleSelect);
        const span = document.createElement('span');
        span.setAttribute('class', 'text-danger');
        module.appendChild(span);
        body.appendChild(module);

        const name = document.createElement('div');
        name.setAttribute('class', 'form-group');
        const nameLabel = document.createElement('label');
        nameLabel.setAttribute('class', 'control-label');
        nameLabel.innerText = 'Առարկայի անուն';
        name.appendChild(nameLabel);
        const nameInput = document.createElement('input');
        nameInput.setAttribute('class', 'form-control');
        nameInput.required = true;
        nameInput.addEventListener('change', e => {
            model.subjects[i].name = e.target.value;
        });
        nameInput.value = model.subjects[i].name;
        name.appendChild(nameInput);
        const nameSpan = document.createElement('span');
        nameSpan.setAttribute('class', 'text=danger');
        name.appendChild(nameSpan);
        body.appendChild(name);

        for (let j = 0; j < model.subjects[i].outcomes.length; j++) {
            const p = document.createElement('p');
            p.setAttribute('class', 'thumbnail');
            p.innerText = model.subjects[i].outcomes[j].name;
            const a = document.createElement('a');
            a.setAttribute('class', 'moveBtn');
            a.setAttribute('data-toggle', 'modal');
            a.setAttribute('data-target', '#moveModal');
            a.setAttribute('data-id', model.subjects[i].outcomes[j].id);
            const span = document.createElement('span');
            span.setAttribute('class', 'glyphicon glyphicon-share');
            span.onclick = e => {
                e.preventDefault();
                onModalLoad(model.subjects[i].outcomes[j].id, model.subjects[i].id);
            }
            a.appendChild(span);
            p.appendChild(a);
            body.appendChild(p);
        }
        panel.appendChild(body);
        block.appendChild(panel);

        blocks.push(block);
    }

    const subArrays = chunkArray(blocks, 3);
    subArrays.forEach((array) => {
        const row = document.createElement('div');
        row.setAttribute('class', 'row');
        array.forEach((block) => {
            row.appendChild(block);
        });
        cards.appendChild(row);
    });

}

function renderModal() {
    const label = document.createElement('label');
    label.setAttribute('class', 'control-label');
    label.innerText = 'Ընտրեք առարկան, որի մեջ ցանկանում եք տեղափոխել';
    const select = document.createElement('select');
    select.setAttribute('class', 'form-control');
    const empOption = document.createElement('option');
    empOption.value = 'NS';
    empOption.text = 'Ընտրեք առարկան';
    select.appendChild(empOption);
    model.subjects.forEach(subject => {
        const option = document.createElement('option');
        option.value = subject.id;
        option.text = subject.name;
        select.appendChild(option);
    });
    select.addEventListener('change', e => {
        sessionStorage.setItem('toSubjectId', e.target.value);
    });
    select.value = parseInt(sessionStorage.getItem('toSubjectId')) || 'NS';
    const modal = document.getElementById('moveModal_body');
    while (modal.firstChild) {
        modal.removeChild(modal.firstChild);
    }
    modal.appendChild(label);
    modal.appendChild(select);
}


function addSubject() {
    let minId = model.subjects.length ? model.subjects.sort((x, y) => x.id - y.id)[0].id - 1 : -1;
    if (minId >= 0) minId = -1;
    const subject = {
        id: minId,
        name: `Առարկա-${model.subjects.length + 1}`,
        subjectModuleId: -1,
        professionId: model.profession.id,
        outcomes: []
    }
    model.subjects.push(subject);
    renderCards();
}
function onSave() {
    const data = model;
    const errorMessages = [];
    model.subjects.forEach(subject => {
        if (subject.subjectModuleId <= 0) {
            errorMessages.push(`Մոդուլը ընտրված չէ ՛${subject.name}՛ առարկայի համար:`);
        }
        if (!subject.name) {
            errorMessages.push(`Անունը ներմուծված չէ ՛${subject.name}՛ առարկայի համար:`);
        }
    });
    if (errorMessages.length !== 0) {
        document.getElementById('message').innerText = errorMessages.join('\n');
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(data),
            url: "/Subject/SaveSubjects",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.redirect) {
                    location.href = data.redirect;
                }
                else {
                    const message = "Առարկաները հաջողությամբ պահպանվեցին!!!";
                    document.getElementById('message').innerText = message;
                    renderCards(data.model);
                }
            },
            error: function (error) {
                console.log("Error occured!!");
                console.log(error);
            }
        });
    }
}

function moveOutcome() {
    const outcomeId = parseInt(sessionStorage.getItem('outcomeId'));
    const subjectId = parseInt(sessionStorage.getItem('subjectId'));
    const toSubjectId = parseInt(sessionStorage.getItem('toSubjectId'));
    const currentSubject = model.subjects.find(s => s.id === subjectId);
    const currentOutcomeIndex = currentSubject.outcomes.findIndex(o => o.id === outcomeId);
    const currentOutcome = currentSubject.outcomes[currentOutcomeIndex];
    currentSubject.outcomes.splice(currentOutcomeIndex, 1);
    const newSubject = model.subjects.find(s => s.id === toSubjectId);
    newSubject.outcomes.push(currentOutcome);
    if (!currentSubject.outcomes.length) {
        const index = model.subjects.findIndex(s => s.id === currentSubject.id);
        model.subjects.splice(index, 1);
    }
    renderCards();

}
