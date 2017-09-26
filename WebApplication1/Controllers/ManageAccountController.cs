using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ManageAccountController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();
        // GET: ManageAccount
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAccountViewData()
        {
            var accountTypes = obj.AccountTypes.ToList();
            var accountTypesModel = accountTypes.Select(x => new AccountTypeModel()
            {
                AccountTypeKey = x.AccountTypeKey,
                AccountTypeName = x.AccountTypeName
            }).ToList();

            return Json(new { AccountTypes = accountTypesModel }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccounts(int page, int pageSize)
        {
            var accountCount = obj.Accounts.Count();
            var skip = (page - 1) * pageSize;
            var accounts = obj.Accounts.OrderBy(x=>x.AccountKey).Skip(skip).Take(pageSize).ToList();
            var accountsModel = accounts.Select(x => new AccountModel()
            {
                AccountKey = x.AccountKey,
                AccountName = x.AccountName,
                AccountNote = x.AccountNote,
                GLAccountCode = x.GLAccountCode,
                AccountTypeKey = x.AccountTypeKey,
                AccountTypeName = x.AccountType.AccountTypeName,
                AccountParentKey = x.AccountParentKey
            }).ToList();

            return Json(new { Accounts = accountsModel, AccountCount = accountCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccount(int accountKey)
        {
            var account = obj.Accounts.SingleOrDefault(x => x.AccountKey == accountKey);
            AccountModel accountModel = new AccountModel();

            if (account != null)
            {
                accountModel.AccountKey = account.AccountKey;
                accountModel.AccountName = account.AccountName;
                accountModel.GLAccountCode = account.GLAccountCode;
                accountModel.AccountTypeKey = account.AccountTypeKey;
                accountModel.AccountTypeName = account.AccountType.AccountTypeName;
                accountModel.AccountParentKey = account.AccountParentKey;
                accountModel.AccountNote = account.AccountNote;
            }

            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUpdateAccount(Account account)
        {
            if (account.AccountKey == 0)
            {
                obj.Accounts.Add(account);
                obj.SaveChanges();
            }
            else
            {
                var accountObj = obj.Accounts.SingleOrDefault(x => x.AccountKey == account.AccountKey);
                if (accountObj != null)
                {
                    accountObj.AccountKey = account.AccountKey;
                    accountObj.AccountName = account.AccountName;
                    accountObj.GLAccountCode = account.GLAccountCode;
                    accountObj.AccountTypeKey = account.AccountTypeKey;
                    accountObj.AccountParentKey = account.AccountParentKey;
                    accountObj.AccountNote = account.AccountNote;
                    obj.SaveChanges();
                }
            }
            return Json(account, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAccount(int accountKey)
        {
            var account = obj.Accounts.SingleOrDefault(x => x.AccountKey == accountKey);
            if (account != null)
            {
                obj.Accounts.Remove(account);
                obj.SaveChanges();
            }
            return Json(new { Success = true, Message = "Account deleted successfully." }, JsonRequestBehavior.AllowGet);

        }

    }
}