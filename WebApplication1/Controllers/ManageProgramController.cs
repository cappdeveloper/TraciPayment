using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ManageProgramController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();

        // GET: ManageProgram
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPrograms(int page, int pageSize)
        {
            var programCount = obj.Programs.Count();
            var skip = (page - 1) * pageSize;
            var programs = obj.Programs.OrderBy(x => x.ProgramKey).Skip(skip).Take(pageSize).ToList();
            var programsModel = programs.Select(x => new ProgramModel()
            {
                ProgramKey = x.ProgramKey,
                ProgramName = x.ProgramName,
                ProgramNote = x.ProgramNote
            }).ToList();

            return Json(new { Programs = programsModel, ProgramCount = programCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProgram(int programKey)
        {
            var program = obj.Programs.SingleOrDefault(x => x.ProgramKey == programKey);
            ProgramModel programModel = new ProgramModel();

            if (program != null)
            {
                programModel.ProgramKey = program.ProgramKey;
                programModel.ProgramName = program.ProgramName;
                programModel.ProgramNote = program.ProgramNote;
            }

            return Json(programModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUpdateProgram(Program program)
        {
            if (program.ProgramKey == 0)
            {
                obj.Programs.Add(program);
                obj.SaveChanges();
            }
            else
            {
                var programObj = obj.Programs.SingleOrDefault(x => x.ProgramKey == program.ProgramKey);
                if (programObj != null)
                {
                    programObj.ProgramKey = program.ProgramKey;
                    programObj.ProgramName = program.ProgramName;
                    programObj.ProgramNote = program.ProgramNote;
                    obj.SaveChanges();
                }
            }

            return Json(program, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProgram(int programKey)
        {
            var program = obj.Programs.SingleOrDefault(x => x.ProgramKey == programKey);
            if (program != null)
            {
                obj.Programs.Remove(program);
                obj.SaveChanges();
            }
            return Json(new { Success = true, Message = "Program deleted successfully." }, JsonRequestBehavior.AllowGet);

        }
    }
}