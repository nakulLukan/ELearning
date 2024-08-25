function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
    }, function (err) {
        console.error('Could not copy text: ', err);
    });
}

window.scrollToTopOnNavigation = function () {
    window.scroll({ top: 0, left: 0, behavior: 'smooth' });
};