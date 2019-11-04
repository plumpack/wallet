import Api from "plumpack-assets/src/scripts/api";

interface CreateTransactionResponse {

}

interface CreateTransactionRequest {

}

export default class PayPal {
    private _api: Api;
    constructor(api: Api) {
        this._api = api;
    }
    public async createTransaction(amount: any): Promise<CreateTransactionResponse> {
        var r = await this._api.post<CreateTransactionRequest, CreateTransactionResponse>("/api/paypal/create-transaction", {});
        return r;
    }
}