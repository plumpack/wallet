import Api from "plumpack-assets/src/scripts/api";
import "plumpack-assets/src/scripts/web-components";

import PayPal from "./paypal";
import * as jQuery from "jquery";

declare global {
    interface Window { baseUrl: string; }
}

export let Wallet = { 
    api: function() {
        return new Api(window.baseUrl);
    },
    payPal: function() {
        return new PayPal(Wallet.api());
    },
    currencyInput(element: string, culture: string, currency: string, currentValue: number, changed: (n: number) => null) {
        jQuery(element).change(function() {
            console.log("changed");
        })
    }
}
