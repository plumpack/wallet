import Api from "plumpack-assets/src/scripts/api";
import PayPal from "./paypal";
import Vue from "vue";
import VueCurrencyInput from 'vue-currency-input';

Vue.use(VueCurrencyInput);

declare global {
    interface Window { baseUrl: string; }
}

export let Wallet = { 
    payPal: function() {
        return new PayPal(new Api(window.baseUrl));
    },
    currencyInput(element: string, culture: string, currency: string, currentValue: number, changed: (n: number) => null) {
        new Vue({
            el: element,
            data: {
                value: currentValue,
                culture: culture,
                currency: currency
            },
            computed: {
                numberValue() {
                    return this.$parseCurrency(this.value, this.culture, this.currency)
                }
            },
            watch: {
                numberValue: function (value) {
                    changed(Number(value));
                }
            }
        })
    }
}
