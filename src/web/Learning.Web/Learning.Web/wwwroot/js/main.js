function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
    }, function (err) {
        console.error('Could not copy text: ', err);
    });
}

function isMobile() {
    console.log(window.outerWidth);
    return window.outerWidth < 600;
}

window.scrollToTopOnNavigation = function () {
    window.scroll({ top: 0, left: 0, behavior: 'smooth' });
};