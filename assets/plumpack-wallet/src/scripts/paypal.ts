import Api from "plumpack-assets/src/scripts/api";

interface CreateOrderResponse {
    orderId: string;
}

interface CreateOrderRequest {
    amount: number;
}

interface CaptureOrderRequest {
    orderId: string;
    payerId: string;
}

interface CaptureOrderResponse {

}

export default class PayPal {
    private _api: Api;
    constructor(api: Api) {
        this._api = api;
    }
    public async createOrder(amount: any): Promise<CreateOrderResponse> {
        return await this._api.post<CreateOrderRequest, CreateOrderResponse>("/api/paypal/create-order", {
            amount: amount
        });
    }
    public async captureOrder(orderId: string, payerId: string): Promise<CaptureOrderResponse> {
        return await this._api.post<CaptureOrderRequest, CaptureOrderResponse>("/api/paypal/capture-order", {
            orderId: orderId,
            payerId: payerId
        });
    }
}