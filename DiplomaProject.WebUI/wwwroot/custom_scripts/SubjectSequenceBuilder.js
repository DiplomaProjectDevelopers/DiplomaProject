function renderSemesters() {
    const cards = document.getElementById('semester-list');
    while (cards.firstChild) {
        cards.removeChild(cards.firstChild);
    }
    const blocks = [];
    for (let i = 0; i < model.length; i++) {
        if (i === model.length - 1 && model[i].length === 0) continue;
        const block = document.createElement('div');
        block.setAttribute('class', 'col-md-3');
        const panel = document.createElement('div');
        panel.setAttribute('class', 'panel panel-primary');
        const heading = document.createElement('div');
        heading.innerText = i === model.length - 1 ? `Չտեղավորված առարկաներ` : `Կիսամյակ ${i + 1}`;
        heading.setAttribute('class', 'panel-heading');
        panel.appendChild(heading);
        const body = document.createElement('div');
        body.setAttribute('class', 'panel-body');

        for (let j = 0; j < model[i].length; j++) {
            const p = document.createElement('p');
            p.setAttribute('class', 'thumbnail');
            p.innerText = model[i][j].name;
            const a = document.createElement('a');
            a.setAttribute('class', 'moveBtn pull-right');
            a.setAttribute('data-toggle', 'modal');
            a.setAttribute('data-target', '#moveModal');
            a.setAttribute('data-id', model[i][j].id);
            const span = document.createElement('span');
            span.setAttribute('class', 'glyphicon glyphicon-share');
            span.onclick = e => {
                e.preventDefault();
                onModalLoad(model[i][j].id, i);
            }
            a.appendChild(span);
            p.appendChild(a);
            body.appendChild(p);
        }
        panel.appendChild(body);
        block.appendChild(panel);

        blocks.push(block);
    }

    const subArrays = chunkArray(blocks, 4);
    subArrays.forEach((array) => {
        const row = document.createElement('div');
        row.setAttribute('class', 'row');
        array.forEach((block) => {
            row.appendChild(block);
        });
        cards.appendChild(row);
    });

}

function onModalLoad(subjectId, semester) {
    sessionStorage.clear();
    sessionStorage.setItem('subjectId', subjectId.toString());
    sessionStorage.setItem('semester', semester.toString());
}

function renderModal() {
    const label = document.createElement('label');
    label.setAttribute('class', 'control-label');
    label.innerText = 'Ընտրեք կիսամյակը, որտեղ ցանկանում եք տեղափոխել';
    const select = document.createElement('select');
    select.setAttribute('class', 'form-control');
    const empOption = document.createElement('option');
    empOption.value = 'NS';
    empOption.text = 'Ընտրեք կիսամյակը';
    select.appendChild(empOption);
    model.forEach((semester, index) => {
        if (index === 8) return;
        const option = document.createElement('option');
        option.value = index;
        option.text = `${index + 1} կիսամյակ`;
        select.appendChild(option);
    });
    select.addEventListener('change', e => {
        sessionStorage.setItem('toSemester', e.target.value);
    });
    select.value = parseInt(sessionStorage.getItem('toSemester')) || 'NS';
    const modal = document.getElementById('moveModal_body');
    while (modal.firstChild) {
        modal.removeChild(modal.firstChild);
    }
    modal.appendChild(label);
    modal.appendChild(select);
}

function moveSubject() {
    const semester = parseInt(sessionStorage.getItem('semester'));
    const subjectId = parseInt(sessionStorage.getItem('subjectId'));
    const toSemester = parseInt(sessionStorage.getItem('toSemester'));
    const currentSemester = model[semester];
    const currentSubjectIndex = currentSemester.findIndex(s => s.id === subjectId);
    const currentSubject = currentSemester[currentSubjectIndex];
    currentSemester.splice(currentSubjectIndex, 1);
    model[toSemester].push(currentSubject);
    renderSemesters();

}

function onSave() {
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: '/Subject/SaveSubjectSequences/?professionId=' + professionId,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            location.href = data.redirect;
        }
    });
}

