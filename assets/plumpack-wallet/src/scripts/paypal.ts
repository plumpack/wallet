import Api from "plumpack-assets/src/scripts/api";

interface CreateOrderResponse {
    orderId: string;
}

interface CreateOrderRequest {
    amount: number;
}

export default class PayPal {
    private _api: Api;
    constructor(api: Api) {
        this._api = api;
    }
    public async createOrder(amount: any): Promise<CreateOrderResponse> {
        var r = await this._api.post<CreateOrderRequest, CreateOrderResponse>("/api/paypal/create-order", {
            amount: amount
        });
        return r;
    }
}