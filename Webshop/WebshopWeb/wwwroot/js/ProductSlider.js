document.addEventListener("DOMContentLoaded", function () {
    const slider = document.querySelector(".slider");
    const slides = document.querySelectorAll(".slide");
    const slideCount = slides.length;
    const slideWidth = slides[0].offsetWidth;
    let index = 0;

    for (let i = 0; i < 5; i++) {
        slider.appendChild(slides[i].cloneNode(true));
    }

    for (let i = 0; i < 5; i++) {
        slider.insertBefore(slides[slideCount - 1 - i].cloneNode(true), slides[0]);
    }

    let currentIndex = 3; 
    updateSlider();

    function moveSlide(direction) {
        currentIndex += direction;
        updateSlider();
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
});