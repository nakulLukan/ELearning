function openRazorpay(options) {
    console.log({ options })
    var rzp = new Razorpay(options);
    console.log({ rzp })
    rzp.open();
}