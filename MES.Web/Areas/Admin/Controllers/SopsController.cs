using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using MES.Web.Areas.Admin.Models;
using MES.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MES.Web.Areas.Admin.Controllers
{
    public class SopsController : Controller
    {
        private static String ModelName = "SOP";
        private MES.Web.Areas.Admin.Models.MESDbContext db = new MES.Web.Areas.Admin.Models.MESDbContext();

        private async Task InitSelect(String LINENAME = "", String L_STNO = "", String PARTNO = "")
        {
            ViewBag.LINE = new SelectList(await db.ENG_PRDLINE.ToListAsync(), "LINENAME", "CodeName", LINENAME);
            ViewBag.PARTNO = new SelectList(await db.ENG_BOMHEADER
                .ToListAsync(),
                "PARTNO", "Name", PARTNO);
            if (String.IsNullOrEmpty(LINENAME))
            {
                ViewBag.STN = new SelectList(new List<ENG_LINEOP>());
            }
            else
            {
                ViewBag.STN = new SelectList(await db.ENG_LINESTATION
                    .Where(s => s.LINENAME.Equals(LINENAME))
                    .ToListAsync(),
                    "L_STNO", "CodeName", L_STNO);
            }
        }

        public async Task<JsonResult> OPSTN(String LINENAME)
        {
            return Json(new
            {
                STN = await db.ENG_LINESTATION
                .Where(stn => stn.LINENAME.Equals(LINENAME))
                .ToListAsync()
            }, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Sops
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = ModelName;
            await InitSelect();
            return View(new SopRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SopRequest request)
        {
            ViewBag.Title = ModelName;
            await InitSelect(request.LINE, request.STN, request.PARTNO);

            int p = request.FILE.FileName.LastIndexOf(".");
            String fileExt = request.FILE.FileName.Substring(p);
            String path = Path.GetTempFileName() + fileExt;

            ENG_LINESTATION stn = await db.ENG_LINESTATION
                .Where(s => s.LINENAME.Equals(request.LINE) && s.L_STNO.Equals(request.STN))
                .SingleOrDefaultAsync();

            BizRequest req = ClientMgr.Instance.CreateRequest("MES",
                String.Format("{0};{1};{2}", stn.LINENAME, stn.L_OPNO, stn.L_STNO),
                "ESOP", "UploadESOP", new Dictionary<string, string>()
                {
                        { "ln",stn.LINENAME},
                        { "st",stn.L_STNO},
                        { "pn",request.PARTNO ?? "ALL"},
                        { "f",path}
                });

            BizResponse resp = await Task<BizResponse>.Run(() =>
            {
                try
                {
                    request.FILE.SaveAs(path);
                    return ClientMgr.Instance.RunCmd(req.CmdName, req);
                }
                finally
                {
                    System.IO.File.Delete(path);
                }
            });

            if (!BizResponses.OK.Equals(resp.ErrorCode))
            {
                ModelState.AddModelError("FILE", resp.ErrorMessage);
            }

            return View(request);
        }


        public async Task<JsonResult> Sops(String LINE, String STN, String PARTNO)
        {
            BizRequest req = ClientMgr.Instance.CreateRequest("MES",
                LINE, "ESOP", "LoadESOP", new Dictionary<string, string>()
                {
                    { "ln",LINE},
                    { "st",STN},
                    { "pn",String.IsNullOrEmpty(PARTNO) ? "ALL" : PARTNO}
                });
            req.UserId = User.Identity.Name;

            BizResponse resp = await Task<BizResponse>.Run(() =>
            {
                return ClientMgr.Instance.RunCmd(req.CmdName, req);
            });
            CmdResponse response = new CmdResponse()
            {
                Code = resp.ErrorCode,
                Msg = resp.ErrorMessage,
                Data = resp.Data
            };

            return Json(response);
        }
    }
}