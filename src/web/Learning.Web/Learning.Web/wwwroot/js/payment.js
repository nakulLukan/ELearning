
window.openRazorpay = function openRazorpay(options) {
    var rzp = new Razorpay(options);
    rzp.on('payment.success', function (response) {
        console.log("Payment popup closed by user.");
        console.log({ response });
    });

    rzp.on('payment.failed', function (response) {
        console.log("Payment popup closed by user.");
        console.log({ response });
    });

    rzp.on('external.close', function () {
        console.log("Payment popup closed by user.");
    });
    rzp.open();
}

window.initRazorpayPopup = function initRazorpayPopup(appName
    , rzrpayOrderId
    , userName
    , userEmail
    , phoneNumber
    , internalOrderId) {
    options = {
        "name": appName,
        "description": "",
        "order_id": rzrpayOrderId,
        "image": "",
        "prefill": {
            "name": userName,
            "email": userEmail,
            "contact": phoneNumber,
        },
        "notes": {
            "address": "",
            "merchant_order_id": internalOrderId,
        },
        "theme": {
            "color": "#F37254"
        }
    }
    // Boolean whether to show image inside a white frame. (default: true)
    options.theme.image_padding = false;
    options.handler = function (response) {
        console.log({ response });
    };
    options.modal = {
        ondismiss: function () {
            console.log('payment process completed. Refreshing page');
        },
        // Boolean indicating whether pressing escape key
        // should close the checkout form. (default: true)
        escape: true,
        // Boolean indicating whether clicking translucent blank
        // space outside checkout form should close the form. (default: false)
        backdropclose: false
    };

    openRazorpay(options);
}