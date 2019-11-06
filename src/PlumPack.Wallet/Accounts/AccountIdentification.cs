using System;

namespace PlumPack.Wallet.Accounts
{
    public struct AccountIdentification
    {
        public static AccountIdentification ById(Guid id)
        {
            return new AccountIdentification
            {
                Id = id
            };
        }

        public static AccountIdentification ByGlobalUserId(Guid globalUserId)
        {
            return new AccountIdentification
            {
                GlobalUserId = globalUserId
            };
        }
        
        public Guid? Id;
        
        public Guid? GlobalUserId;

        public override string ToString()
        {
            return Id.HasValue ? $"AccountIdentification:ById:{Id}" : $"AccountIdentification:ByGlobalUserId:{GlobalUserId}";
        }
    }
}