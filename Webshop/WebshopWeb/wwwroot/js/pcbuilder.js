document.addEventListener("DOMContentLoaded", function () {
    filter();

    document.getElementById('cpuSelect').addEventListener('change', filter);
    document.getElementById('motherboardSelect').addEventListener('change', filter);
    document.getElementById('ramSelect').addEventListener('change', filter);
    document.getElementById('caseSelect').addEventListener('change', filter);
    document.getElementById('gpuSelect').addEventListener('change', filter);
    document.getElementById('psuSelect').addEventListener('change', filter);
});

function filter() {
    showLoader();
    const cpuId = document.getElementById('cpuSelect').value;
    const motherboardId = document.getElementById('motherboardSelect').value;
    const ramId = document.getElementById('ramSelect').value;
    const caseId = document.getElementById('caseSelect').value;
    const gpuId = document.getElementById('gpuSelect').value;
    const psuId = document.getElementById('psuSelect').value;

    let url = "/api/PCBuilder/filterproducts?";
    if (cpuId) url += `cpuId=${encodeURIComponent(cpuId)}&`;
    if (motherboardId) url += `motherboardId=${encodeURIComponent(motherboardId)}&`;
    if (ramId) url += `ramId=${encodeURIComponent(ramId)}&`;
    if (caseId) url += `caseId=${encodeURIComponent(caseId)}&`;

    url = url.endsWith("&") ? url.slice(0, -1) : url;

    

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            populateSelect("cpuSelect", data.filter(p => p.category === "Processzor"));
            populateSelect("motherboardSelect", data.filter(p => p.category === "Alaplap"));
            populateSelect("ramSelect", data.filter(p => p.category === "Memória"));
            populateSelect("caseSelect", data.filter(p => p.category === "Gépház"));
            populateSelect("gpuSelect", data.filter(p => p.category === "Videókártya"));
            populateSelect("psuSelect", data.filter(p => p.category === "Tápegység"));
            calculateTotalPrice(data);
            
        })
        .catch(error => {
            console.error("Error fetching products:", error);
            alert("Failed to fetch products. Please try again later.");
        })
        .finally(() => {
            hideLoader();
        });
}

function populateSelect(selectId, items) {
    const select = document.getElementById(selectId);
    const selectedValue = select.value;

    select.innerHTML = '<option value="">Válassz</option>';

    let foundSelected = false;

    items.forEach(item => {
        const option = document.createElement("option");
        option.value = item.id;
        option.textContent = item.name;

        if (item.id.toString() === selectedValue) {
            option.selected = true;
            foundSelected = true;
        }

        select.appendChild(option);
    });

    if (!foundSelected) {
        select.value = "";
    }
}

function calculateTotalPrice(data) {
    const selectedIds = [
        document.getElementById('cpuSelect').value,
        document.getElementById('motherboardSelect').value,
        document.getElementById('ramSelect').value,
        document.getElementById('caseSelect').value,
        document.getElementById('gpuSelect').value,
        document.getElementById('psuSelect').value,
    ];

    let totalPrice = 0;

    selectedIds.forEach(id => {
        if (id) {
            const item = data.find(p => p.id.toString() === id);
            if (item) {
                totalPrice += item.price || 0;
            }
        }
    });

    document.getElementById('totalCost').textContent = totalPrice.toFixed(0)+" Ft";
}

function showLoader() {
    document.getElementById('overlay').style.display = 'flex';
    document.body.classList.add('loading');
    console.log("Loading...");
    const selects = document.querySelectorAll('select');
    selects.forEach(x => x.disabled = true);
}

function hideLoader() {
    document.getElementById('overlay').style.display = 'none';
    document.body.classList.remove('loading');
    console.log("Loaded.");
    const selects = document.querySelectorAll('select');
    selects.forEach(x => x.disabled = false);
}





