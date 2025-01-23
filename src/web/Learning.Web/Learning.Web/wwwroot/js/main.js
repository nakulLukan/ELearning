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

window.scrollToBottomOnNavigation = function () {
    window.scroll({ top: document.body.scrollHeight, left: 0, behavior: 'smooth' });
};

window.scrollToTarget = function (targetId){
    const element = document.getElementById(targetId);
    console.log({ element })
    if (element) {
        element.scrollIntoView({ behavior: "smooth" });
        element.focus();
    }
}