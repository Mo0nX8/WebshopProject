document.addEventListener("DOMContentLoaded", function () {
    const slider = document.querySelector(".slider");
    const slides = document.querySelectorAll(".slide");
    const slideCount = slides.length;
    let slideWidth = slides[0].offsetWidth; 
    let index = 0;

    for (let i = 0; i < 5; i++) {
        slider.appendChild(slides[i].cloneNode(true));
    }

    for (let i = 0; i < 5; i++) {
        slider.insertBefore(slides[slideCount - 1 - i].cloneNode(true), slides[0]);
    }

    let currentIndex = 3;
    let isInAnimation = false;
    updateSlider();

    function moveSlide(direction) {
        if (isInAnimation) return;
        isInAnimation = true;
        currentIndex += direction;
        updateSlider();

        setTimeout(() => {
            isInAnimation = false;
        }, 500);
    }

    function updateSlider() {
        slider.style.transition = "transform 0.5s ease-in-out";
        slider.style.transform = `translateX(-${currentIndex * slideWidth}px)`;

        setTimeout(() => {
            if (currentIndex >= slideCount + 3) {
                slider.style.transition = "none";
                currentIndex = 3;
                slider.style.transform = `translateX(-${currentIndex * slideWidth}px)`;
            } else if (currentIndex < 3) {
                slider.style.transition = "none";
                currentIndex = slideCount + 2;
                slider.style.transform = `translateX(-${currentIndex * slideWidth}px)`;
            }
        }, 500);
    }

    document.querySelector(".prev").addEventListener("click", () => moveSlide(-1));
    document.querySelector(".next").addEventListener("click", () => moveSlide(1));

    window.addEventListener("resize", () => {
        slideWidth = slides[0].offsetWidth;
        updateSlider();
    });

    let startX = 0;

    slider.addEventListener("touchstart", (e) => {
        startX = e.touches[0].clientX;
    });

    slider.addEventListener("touchend", (e) => {
        const endX = e.changedTouches[0].clientX;
        if (endX - startX > 50) {
            moveSlide(-1);
        } else if (startX - endX > 50) {
            moveSlide(1);
        }
    });
});
