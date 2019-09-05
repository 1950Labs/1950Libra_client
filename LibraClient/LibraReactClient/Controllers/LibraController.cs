using LibraReactClient.BusinessLayer.Entities;
using LibraReactClient.BusinessLayer.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Types;

namespace LibraReactClient.Controllers
{
    [Route("api/[controller]")]
    public class LibraController : Controller
    {
        private LibraTransactions _transacitonsLogic;
        public LibraTransactions TransactionsLogic
        {
            get { return _transacitonsLogic = _transacitonsLogic ?? new LibraTransactions(); }
            set { _transacitonsLogic = value; }
        }

        private LibraAccounts _accountsLogic;

        public LibraAccounts AccountsLogic
        {
            get { return _accountsLogic = _accountsLogic ?? new LibraAccounts(); }
            set { _accountsLogic = value; }
        }

        // POST: api/Libra/SubmitTransaction
        [Authorize]
        [HttpPost("[action]")]
        public RawTransaction SubmitTransaction([FromBody] SubmitTransactionIn request)
        {
            return TransactionsLogic.SubmitTransaction(request);
            
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<AddAccountOut> AddAccountAsync([FromBody] AddAccountIn request)
        {
            return await AccountsLogic.AddAccountAsync(request);
        }

        [Authorize]
        [HttpGet("[action]")]
        public IEnumerable<Account> GetAccounts(string userUID)
        {
            return AccountsLogic.GetAccounts(new GetAccountsIn
            {
                UserUID = userUID
            });
        }

        [Authorize]
        [HttpGet("[action]")]
        public GetAccountInfoOut GetAccountInfo(int accountId)
        {
            return AccountsLogic.GetAccountInfo(new GetAccountInfoIn
            {
                AccountId = accountId
            });
        }

        [Authorize]
        [HttpPost("[action]")]
        public Task<MintOut> Mint([FromBody] MintIn request)
        {
            return TransactionsLogic.MintAsync(request);

        }
    }
}
