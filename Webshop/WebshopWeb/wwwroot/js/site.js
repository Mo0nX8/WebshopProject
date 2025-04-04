document.addEventListener("DOMContentLoaded", function () {
    updateCartItemCount();
    function isMobileDevice() {
        return window.innerWidth <= 768; 
    }
    var elektronikaDropdown = document.querySelector(".dropdown-submenu");
    var elektronikaMenu = elektronikaDropdown.querySelector(".dropdown-menu");


    elektronikaDropdown.addEventListener("mouseenter", function () {
        elektronikaMenu.classList.add("show");
    });

    elektronikaDropdown.addEventListener("mouseleave", function () {
        elektronikaMenu.classList.remove("show");
    });

    document.querySelectorAll(".dropdown-submenu > a").forEach(function (submenu) {
        submenu.addEventListener("click", function (e) {
            if (isMobileDevice()) {
                e.preventDefault(); 

                let submenuDropdown = this.nextElementSibling; 

                if (submenuDropdown) {
                    let isOpen = submenuDropdown.classList.contains("show");

                    document.querySelectorAll(".dropdown-submenu .submenu").forEach(function (menu) {
                        menu.classList.remove("show");
                    });

                    if (!isOpen) {
                        submenuDropdown.style.display = "block";
                        submenuDropdown.classList.add("show");
                    } else {
                        submenuDropdown.style.display = "none";
                        submenuDropdown.classList.remove("show");
                    }

                    e.stopPropagation();
                }
            }
        });
    });


    document.addEventListener("click", function (e) {
        if (isMobileDevice() && !e.target.closest(".dropdown-submenu")) {
            document.querySelectorAll(".dropdown-submenu .submenu").forEach(function (menu) {
                menu.style.display = "none";
                menu.classList.remove("show");
            });
        }
    });
});
function updateCartItemCount() {
    $.ajax({
        url: '/Cart/GetCartCount', 
        type: 'GET',
        success: function (response) {
            const cartItemCount = response.itemCount; 
            document.getElementById('cart-item-count').textContent = cartItemCount;
        },
        error: function () {
            console.error("Sikertelen fetch!");
        }
    });
}
$('.add-to-cart-btn').on('click', function () {
    updateCartItemCount();
});


function searchProduct() {
    var searchValue = document.getElementById("search").value;
    window.location.href = '@Url.Action("Search", "Product")' + '?searchValue=' + encodeURIComponent(searchValue);
}

