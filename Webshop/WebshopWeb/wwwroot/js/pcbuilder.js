document.addEventListener("DOMContentLoaded", function () {
    filter();

    document.getElementById('cpuSelect').addEventListener('change', filter);
    document.getElementById('motherboardSelect').addEventListener('change', filter);
    document.getElementById('ramSelect').addEventListener('change', filter);
    document.getElementById('caseSelect').addEventListener('change', filter);
    document.getElementById('gpuSelect').addEventListener('change', filter);
    document.getElementById('psuSelect').addEventListener('change', filter);
    document.getElementById('cpucoolerSelect').addEventListener('change', filter);
    document.getElementById('ssdSelect').addEventListener('change', filter);

    const addToCartLink = document.querySelector("a#addToCart");

    const form = document.getElementById('pcBuilderForm');
    form.addEventListener('change', function () {
        const cpuId = document.getElementById('cpuSelect').value;
        const motherboardId = document.getElementById('motherboardSelect').value;
        const ramId = document.getElementById('ramSelect').value;
        const caseId = document.getElementById('caseSelect').value;
        const gpuId = document.getElementById('gpuSelect').value;
        const psuId = document.getElementById('psuSelect').value;
        const cpucoolerId = document.getElementById('cpucoolerSelect').value;
        const ssdId = document.getElementById('ssdSelect').value;

        const selectedIds = [cpuId, motherboardId, ramId, caseId, gpuId, psuId,ssdId,cpucoolerId]
            .filter(id => id !== "")
            .join(",");

        if (selectedIds) {
            addToCartLink.setAttribute('href', `${addToCartBaseUrl}?ids=${selectedIds}`);
        } else {
            addToCartLink.setAttribute('href', '#');
        }
    });
});

function filter() {
    showLoader();
    const cpuId = document.getElementById('cpuSelect').value;
    const motherboardId = document.getElementById('motherboardSelect').value;
    const ramId = document.getElementById('ramSelect').value;
    const caseId = document.getElementById('caseSelect').value;

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
            populateSelect("cpucoolerSelect", data.filter(p => p.category === "ProcesszorHűtő"));
            populateSelect("ssdSelect", data.filter(p => p.category === "SSD"));
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
        document.getElementById('cpucoolerSelect').value,
        document.getElementById('ssdSelect').value,
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

    document.getElementById('totalCost').textContent = totalPrice.toLocaleString('hu-HU', { minimumFractionDigits: 0, maximumFractionDigits: 0 }) +" Ft";
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



const cartPopup = document.getElementById('cart-popup');
const closePopupBtn = document.getElementById('popup-close');
const popupMessage = document.getElementById('popup-message');

function showPopup(message) {
    popupMessage.textContent = message;
    cartPopup.style.display = 'block';

    setTimeout(() => {
        cartPopup.style.display = 'none';
    }, 3000);
}

closePopupBtn.addEventListener('click', function () {
    cartPopup.style.display = 'none';
});

const addToCartBtns = document.querySelectorAll('.add-to-cart-btn');
addToCartBtns.forEach(function (addToCartBtn) {
    addToCartBtn.addEventListener('click', function () {
        let productIds = [];
        const cpuId = document.getElementById('cpuSelect').value;
        const motherboardId = document.getElementById('motherboardSelect').value;
        const gpuId = document.getElementById('gpuSelect').value;
        const ramId = document.getElementById('ramSelect').value;
        const ssdId = document.getElementById('ssdSelect').value;
        const psuId = document.getElementById('psuSelect').value;
        const caseId = document.getElementById('caseSelect').value;
        const coolerId = document.getElementById('cpucoolerSelect').value;

        if (cpuId) productIds.push(cpuId);
        if (motherboardId) productIds.push(motherboardId);
        if (gpuId) productIds.push(gpuId);
        if (ramId) productIds.push(ramId);
        if (ssdId) productIds.push(ssdId);
        if (psuId) productIds.push(psuId);
        if (caseId) productIds.push(caseId);
        if (coolerId) productIds.push(coolerId);

        const productIdString = productIds.join(',');

        $.ajax({
            url: '/Product/AddToCart',
            type: 'POST',
            data: { ids: productIdString },
            success: function (response) {
                if (response.success) {
                    showPopup(response.message);
                    updateCartItemCount();
                } else {
                    showPopup(response.message);
                }
            },
            error: function () {
                showPopup("Hiba történt a kosárhoz adás során!");
            }
        });
    });
});




