<%@ page language="VB" autoeventwireup="false" inherits="paypal, App_Web_3bsbwt4p" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script src="https://www.paypalobjects.com/api/checkout.js"></script>
</head>

<body>
    <div id="paypal-button-container"></div>

    <script>
        paypal.Button.render({

            env: 'sandbox', // sandbox | production

            // PayPal Client IDs - replace with your own
            // Create a PayPal app: https://developer.paypal.com/developer/applications/create
            client: {
                sandbox:    'AYm6vXDE-Nv875YenaeroADXYlOTCgoT3ZTBXm_PkW5-JXBPUnX8DoFlG0Guff5-InYvJwrqyAfa-k3l',
                production: 'ATmnbdaQWEWzd_4ExDUth6kZErPvtH0cpLr-JrEwgb6Y--KLL456S0zG4yd4OLM_6hraN7uu5BXM5rjZ'
            },

            // Show the buyer a 'Pay Now' button in the checkout flow
            commit: true,

            // payment() is called when the button is clicked
            payment: function(data, actions) {

                // Make a call to the REST api to create the payment
                return actions.payment.create({
                    payment: {
                        transactions: [
                            {
                                amount: { total: '<%= Session("TotalCarrito") %>', currency: 'MXN'}
                            }
                        ]
                    }
                });
            },

            // onAuthorize() is called when the buyer approves the payment
            onAuthorize: function(data, actions) {

                // Make a call to the REST api to execute the payment
                return actions.payment.execute().then(function() {
                    window.alert('Payment Complete!');
                });
            }

        }, '#paypal-button-container');

    </script>
    <form action="https://www.paypal.com/mx/cgi-bin/webscr" method="post">
<input type="hidden" name="cmd" value="_xclick">
<input type="hidden" name="business" value="jpena@tie.com.mx">
<input type="hidden" name="item_name" value="Item Name">
<input type="hidden" name="currency_code" value="USD">
<input type="hidden" name="amount" value="0.00">
<input type="image" src="http://www.paypal.com/es_XC/i/btn/x-click-but01.gif" name="submit" alt="Make payments with PayPal - it's fast, free and secure!">
</form>
</body>
</html>
