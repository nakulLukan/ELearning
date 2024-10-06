window.initCarousel = function (dotNetHelper) {
    var carousel = document.getElementById('examNotificationCarousel');

    // Set the initial href
    var activeItem = carousel.querySelector('.carousel-item.active');
    var initialHref = activeItem.getAttribute('data-details-href');
    dotNetHelper.invokeMethodAsync('UpdateHref', initialHref);

    // Listen for the carousel's slide event
    $('#examNotificationCarousel').on('slide.bs.carousel', function (event) {
        var newActiveItem = event.relatedTarget;
        var newHref = newActiveItem.getAttribute('data-details-href');

        // Call Blazor method to update the href
        dotNetHelper.invokeMethodAsync('UpdateHref', newHref);
    });
}
