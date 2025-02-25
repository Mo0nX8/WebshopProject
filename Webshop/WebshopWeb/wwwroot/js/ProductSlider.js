let currentSlide = 0; 
const totalSlides = $('.slide').length; 

$('.slideshow').append($('.slide').slice(0, 6).clone());

function moveSlide(direction) {
    currentSlide += direction;

    if (currentSlide >= totalSlides) {
        currentSlide = 0;
        $('.slideshow').css('transition', 'none'); 
        $('.slideshow').css('transform', `translateX(0%)`); 
    } else if (currentSlide < 0) {
        currentSlide = totalSlides - 1; 
        $('.slideshow').css('transition', 'none'); 
        const offset = -currentSlide * (100 / 6); 
        $('.slideshow').css('transform', `translateX(${offset}%)`);
    } else {
        const offset = -currentSlide * (100 / 6); 
        $('.slideshow').css('transform', `translateX(${offset}%)`);
    }
}

$('.slideshow').on('mouseover', function () {
    $(this).css('animation-play-state', 'paused');
});

$('.slideshow').on('mouseout', function () {
    $(this).css('animation-play-state', 'running');
});
