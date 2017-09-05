using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BudgetController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();
        //
        // GET: /Budget/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetBudgetViewData()
        {
            var programs = obj.Programs.ToList();
            var programsModel = programs.Select(x => new ProgramModel()
            {
                ProgramKey = x.ProgramKey,
                ProgramName = x.ProgramName
            }).ToList();

            var accounts = obj.Accounts.ToList();
            var accountsModel = accounts.Select(x => new AccountModel()
            {
                AccountKey = x.AccountKey,
                AccountName = x.AccountName
            }).ToList();

            var terms = obj.Terms.ToList();
            var termsModel = terms.Select(x => new TermModel()
            {
                TermKey = x.TermKey,
                TermName = x.TermName,
                TermStartDate = x.TermStartDate,
                TermEndDate = x.TermEndDate

            }).ToList();

            return Json(new { Programs = programsModel, Accounts = accountsModel, Terms = termsModel }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudgets(int page, int pagesize)
        {
            var budgetCount = obj.Budgets.Count();
            var skip = (page - 1) * pagesize;
            var budgets = obj.Budgets.OrderBy(x => x.BudgetKey).Skip(skip).Take(pagesize).ToList();
            var budgetsModel = budgets.Select(x => new BudgetModel()
            {
                BudgetKey = x.BudgetKey,
                BudgetProgramKey = x.BudgetProgramKey,
                BudgetTermKey = x.BudgetTermKey,
                BudgetAccountKey = x.BudgetAccountKey,
                BudgetAmount = x.BudgetAmount,
                BudgetNote = x.BudgetNote,
                BudgetAccountName = x.Account.AccountName,
                BudgetProgramName = x.Program.ProgramName,
                BudgetTermName = x.Term.TermName,
             
                //Account = x.Account,
                //Account = from a in x.Account
                //          select new AccountModel()
                //          {
                //              AccountKey = a.AccountKey,
                //              AccountName = a.AccountName
                //          },
                //Program = x.Program,
                //Term = x.Term
            }).ToList();

            return Json(new { Budgets = budgetsModel, BudgetCount = budgetCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudget(int budgetKey)
        {
            var budget = obj.Budgets.SingleOrDefault(x => x.BudgetKey == budgetKey);
            BudgetModel budgetModel = new BudgetModel();

            if (budget != null)
            {
                budgetModel.BudgetKey = budget.BudgetKey;
                budgetModel.BudgetProgramKey = budget.BudgetProgramKey;
                budgetModel.BudgetTermKey = budget.BudgetTermKey;
                budgetModel.BudgetAccountKey = budget.BudgetAccountKey;
                budgetModel.BudgetAmount = budget.BudgetAmount;
                budgetModel.BudgetNote = budget.BudgetNote;
            }

            return Json(budgetModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUpdateBudget(Budget budget)
        {
            if (budget.BudgetKey == 0)
            {
                obj.Budgets.Add(budget);
                obj.SaveChanges();
            }
            else
            {
                var budegtObj = obj.Budgets.SingleOrDefault(x => x.BudgetKey == budget.BudgetKey);
                if (budegtObj != null)
                {
                    budegtObj.BudgetProgramKey = budget.BudgetProgramKey;
                    budegtObj.BudgetTermKey = budget.BudgetTermKey;
                    budegtObj.BudgetAccountKey = budget.BudgetAccountKey;
                    budegtObj.BudgetAmount = budget.BudgetAmount;
                    budegtObj.BudgetNote = budget.BudgetNote;
                    obj.SaveChanges();
                }
            }

            return Json(budget, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteBudget(int budgetKey)
        {
            var budget = obj.Budgets.SingleOrDefault(x => x.BudgetKey == budgetKey);
            if (budget != null)
            {
                obj.Budgets.Remove(budget);
                obj.SaveChanges();
            }
            return Json(new { Success = true, Message = "Budget deleted successfully." }, JsonRequestBehavior.AllowGet);

        }
    }
}