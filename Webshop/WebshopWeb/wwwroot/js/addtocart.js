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
            const productId = addToCartBtn.getAttribute('data-product-id');

            $.ajax({
                url: '/Product/AddToCart',
                type: 'POST',
                data: { ids: productId },
                success: function (response) {
                    if (response.success) {
                        showPopup(response.message);
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

