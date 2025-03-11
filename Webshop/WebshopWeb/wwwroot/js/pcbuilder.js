document.getElementById('cpuSelect').addEventListener("change", function () {
    let cpuId = this.value;
    fetch(`/api/PCBuilder/GetCpu?cpuId=${cpuId}`)
        .then(response => response.json())
        .then(data => {
            let motherboardSelect = document.getElementById('motherboardSelect');
            motherboardSelect.innerHTML = '<option value="">Kérlek válasz alaplapot</option>';
            data.forEach(mb => {
                motherboardSelect.innerHTML += `<option value="${mb.id}">${mb.name}</option>`;
            });
        });
});

document.getElementById("motherboardSelect").addEventListener("change", function () {
    let motherboardId = this.value;
    fetch(`/api/PCBuilder/getram?motherboardId=${motherboardId}`)
        .then(response => response.json())
        .then(data => {
            let ramSelect = document.getElementById("ramSelect");
            ramSelect.innerHTML = '<option value="">Válassz RAM-ot</option>';
            data.forEach(ram => {
                ramSelect.innerHTML += `<option value="${ram.id}">${ram.name}</option>`;
            });
        });
});

document.getElementById("caseSelect").addEventListener("change", function () {
    let caseId = this.value;
    fetch(`/api/PCBuilder/GetMotherboard?caseId=${caseId}`)
        .then(response => response.json())
        .then(data => {
            let motherboardSelect = document.getElementById("motherboardSelect");
            motherboardSelect.innerHTML = '<option value="">Válassz alaplapot</option>';
            data.forEach(mb => {
                motherboardSelect.innerHTML += `<option value="${mb.Id}">${mb.productName}</option>`;
            });
        });
});



document.addEventListener("DOMContentLoaded", function () {
    fetch("/api/PCBuilder/GetAllPCParts")
        .then(response => response.json())
        .then(data => {
            populateSelect("cpuSelect", data.cpus);
            populateSelect("ramSelect", data.ram);
            populateSelect("motherboardSelect", data.motherboards);
            populateSelect("caseSelect", data.cases);
        });
});
function populateSelect(selectId, data) {
    let select = document.getElementById(selectId);
    select.innerHTML = 'Válassz terméket'; 
    data.forEach(item => {
        let option = document.createElement("option");
        option.value = item.id;
        option.textContent = item.productName;
        select.appendChild(option);
    });
}