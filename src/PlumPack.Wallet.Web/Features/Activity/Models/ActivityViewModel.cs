using System.Collections.Generic;
using PlumPack.Wallet.Domain;

namespace PlumPack.Wallet.Web.Features.Activity.Models
{
    public class ActivityViewModel
    {
        public List<Transaction> Transactions { get; set; }
    }
}