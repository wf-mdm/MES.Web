﻿using Intelli.MidW.BizClient;
using Intelli.MidW.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MES.Web.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            DataSet ds = await Task<DataSet>.Run(() =>
            {
                BizRequest request = ClientMgr.Instance.CreateRequest("config", "MES", "", "GETUSRAPS", new Dictionary<string, string>(){
                    { "uid", HttpContext.User.Identity.Name},
                    { "modid", "MESWMS"}
                });
                request.UserId = HttpContext.User.Identity.Name;
                try
                {
                    return ClientMgr.Instance.RunDbCmd(request.CmdName, request);
                }
                catch (Exception ex)
                {
                }
                return null;
            });

            if (null != ds)
            {
                Dictionary<String, String> Features = new Dictionary<string, string>();
                foreach (DataRow r in ds.Tables["APP_MASTDATA"].Rows)
                {
                    Dictionary<string, string> f = new Dictionary<string, string>();
                    Features[(String)r["APP_ID"]] = (String)r["APP_DESCRIPTION"];
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.Features = jss.Serialize(Features);
            }
            ViewBag.User = User.Identity.Name;
            return View();
        }
    }
}