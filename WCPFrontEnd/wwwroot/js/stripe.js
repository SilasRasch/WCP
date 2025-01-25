window.confirmPayment = async function (clientSecret) {
    var stripe = Stripe('stripekey');  // Replace with your public key
    var elements = stripe.elements();

    // Create an instance of the card Element
    var card = elements.create('card');
    card.mount('#card-element');  // Attach to your HTML container

    // Wait for user input and handle the confirmation
    var result = await stripe.confirmCardPayment(clientSecret, {
        payment_method: {
            card: card
        }
    });

    if (result.error) {
        // Handle errors (e.g., display to user)
        alert(result.error.message);
    } else {
        // Payment was successful
        alert("Payment successful!");
    }
};
