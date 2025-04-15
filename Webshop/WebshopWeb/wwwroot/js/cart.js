document.addEventListener("DOMContentLoaded", function () {
    const quantitySelects = document.querySelectorAll('select[name="quantity"]');

    function updateCartPrice() {
        let totalPrice = 0;

        document.querySelectorAll('.cart-item').forEach(function (cartItem) {
            const productPrice = parseFloat(cartItem.querySelector('.product-price').textContent.replace(' Ft', ''));
            const quantity = parseInt(cartItem.querySelector('select[name="quantity"]').value);
            totalPrice += productPrice * quantity;
        });

        const totalPriceElement = document.querySelector('.total-price p');
        if (totalPriceElement) {
            totalPriceElement.innerHTML = `<strong>Végösszeg: ${totalPrice} Ft</strong>`;
        }
    }

    function updateQuantityInCart(productId, newQuantity) {
        fetch(`/api/update/${productId}/${newQuantity}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(async response => {
                const data = await response.json();
                if (!response.ok) {
                    alert(data.error || 'Ismeretlen hiba történt.');
                    return;
                }

                if (data.totalPrice) {
                    const totalPriceElement = document.querySelector('.total-price p');
                    if (totalPriceElement) {
                        totalPriceElement.innerHTML = `<strong>Végösszeg: ${data.totalPrice} Ft</strong>`;
                    }
                    console.log('Kosár sikeresen frissítve.');
                }
            })
            .catch(error => {
                console.error('Hiba a kosár frissítésekor:', error);
            });
    }



    quantitySelects.forEach(function (select) {
        select.addEventListener('change', function () {
            const productId = select.getAttribute('data-product-id');
            const newQuantity = parseInt(select.value);

            updateQuantityInCart(productId, newQuantity);
        });
    });

    updateCartPrice();
});
