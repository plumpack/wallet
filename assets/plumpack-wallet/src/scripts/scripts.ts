import Api from "plumpack-assets/src/scripts/api";
import PayPal from "./paypal";

declare global {
    interface Window { baseUrl: string; }
}

export let Wallet = { 
    payPal: function() {
        return new PayPal(new Api(window.baseUrl));
    }
}
