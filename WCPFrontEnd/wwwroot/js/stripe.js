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

let stripe;
let card;

window.initializeStripe = async function (publishableKey) {
    stripe = Stripe(publishableKey);
    const elements = stripe.elements();
    card = elements.create('card', {
        hidePostalCode: true,
        style: {
            base: {
                fontSize: '18px',
                color: '#32325d',
                fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
                '::placeholder': {
                    color: '#aab7c4'
                },
                padding: '10px',
            },
            invalid: {
                color: '#fa755a'
            }
        }
    });

    card.mount('#card-element');
};

window.processStripePayment = async function () {
    const { paymentMethod, error } = await stripe.createPaymentMethod({
        type: 'card',
        card: card,
    });

    if (error) {
        console.error(error.message);
        return "";
    }

    return paymentMethod.id;
};