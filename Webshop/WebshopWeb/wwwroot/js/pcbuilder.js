document.addEventListener("DOMContentLoaded", function () {

    filter();

    document.getElementById('cpuSelect').addEventListener('change', filter);
    document.getElementById('motherboardSelect').addEventListener('change', filter);
    document.getElementById('ramSelect').addEventListener('change', filter);
    document.getElementById('caseSelect').addEventListener('change', filter);
});

function filter() {
    var cpuId = document.getElementById('cpuSelect').value;
    var motherboardId = document.getElementById('motherboardSelect').value;
    var ramId = document.getElementById('ramSelect').value;
    var caseId = document.getElementById('caseSelect').value;

    var url = "/api/PCBuilder/filterproducts?";
    if (cpuId) url += "cpuId=" + encodeURIComponent(cpuId) + "&";
    if (motherboardId) url += "motherboardId=" + encodeURIComponent(motherboardId) + "&";
    if (ramId) url += "ramId=" + encodeURIComponent(ramId) + "&";
    if (caseId) url += "caseId=" + encodeURIComponent(caseId) + "&";

    url = url.endsWith("&") ? url.slice(0, -1) : url;

    fetch(url)
        .then(response => response.json())
        .then(data => {
            let cpus = data.filter(p => p.category === "Processzor");
            let motherboards = data.filter(p => p.category === "Alaplap");
            let rams = data.filter(p => p.category === "Memória");
            let cases = data.filter(p => p.category === "Gépház");

            populateSelect("cpuSelect", cpus);
            populateSelect("motherboardSelect", motherboards);
            populateSelect("ramSelect", rams);
            populateSelect("caseSelect", cases);
        })
        .catch(error => console.error("Error fetching products:", error));
}

   
function populateSelect(selectId, items) {
    let select = document.getElementById(selectId);
    let selectedValue = select.value; 

    select.innerHTML = '<option value="">Válassz terméket</option>';

    let foundSelected = false; 

    items.forEach(item => {
        let option = document.createElement("option");
        option.value = item.id;
        option.textContent = item.name;

        if (item.id == selectedValue) {
            option.selected = true;
            foundSelected = true;
        }

        select.appendChild(option);
    });

   
    if (!foundSelected) {
        select.value = "";
    }
}

