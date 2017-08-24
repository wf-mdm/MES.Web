using MES.Web.Areas.Admin.Models;
using MES.Web.Areas.Rpt.Models;
using MES.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Areas.Rpt.Controllers
{
    [Authorize]
    public class TraceabilityController : Controller
    {
        private static String ModelName = "追溯查询";
        DBHelper db = new DBHelper();

        // GET: Rpt/Traceability
        public ActionResult Index()
        {
            ViewBag.Title = ModelName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(TraceabilityRequest tr)
        {
            ViewBag.Title = ModelName;

            if (String.IsNullOrEmpty(tr.Wo) && String.IsNullOrEmpty(tr.Sn) && String.IsNullOrEmpty(tr.Pack)
                && !tr.Dt1.HasValue && !tr.Dt2.HasValue)
            {
                ModelState.AddModelError("", "请输入要查询的序列号,箱号,批次号或工单号！");
            }
            else
            {
                ViewBag.History = await GetHistoryData(tr);
                if (ViewBag.History.Rows.Count > 0)
                {
                    List<String> prdsns = GetPrdsns(ViewBag.History);
                    ViewBag.IDData = await GetWipIDData(prdsns);
                    ViewBag.CompData = await GetCompData(prdsns);
                    ViewBag.ContainerData = await GetContainerData(prdsns);
                    ViewBag.ProcData = await GetProcData(prdsns);
                    ViewBag.TicketData = await GetTicketData(prdsns);
                }
            }

            return View(tr);
        }

        private async Task<DataTable> GetHistoryData(TraceabilityRequest tr)
        {
            string sql;
            if (!tr.Dt1.HasValue)
            {
                tr.Dt1 = new DateTime(2000, 1, 1);
            }
            if (!tr.Dt2.HasValue)
            {
                tr.Dt2 = new DateTime(3000, 1, 1);
            }
            string cntrsql = "";
            if (!string.IsNullOrEmpty(tr.Pack))
                cntrsql = " and ( prdsn in (select SUBCONTAINERNO from WMS_CONTAINERSUB where CONTAINERNO='" + tr.Pack + "') OR prdsn in (SELECT R1.IDVAL FROM WIP_IDLIST R1 WHERE R1.PRDSN IN  (select SUBCONTAINERNO from WMS_CONTAINERSUB where CONTAINERNO='" + tr.Pack + "')) )";
            string wosql = "";
            if (!string.IsNullOrEmpty(tr.Wo))
                wosql = " (wo_id = '" + tr.Wo + "' OR PRDSN IN  (SELECT R3.IDVAL FROM WIP_IDLIST R3 WHERE R3.WO_ID= '" + tr.Wo + "') ) and ";
            if (tr.IsRevert)
            {
                if (!string.IsNullOrEmpty(tr.Sn))
                {
                    sql = "select * from V_RPT_WIP_PrdHistory where " + wosql  + " ( (startdt>=@1 and enddt<=@2) OR PRDSN IN ( SELECT R4.IDVAL  FROM WIP_IDLIST R4 WHERE R4.PRDSN IN ( SELECT RA.PRDSN FROM WIP_PRDHISTORY RA WHERE RA.startdt>=@1 and RA.enddt<=@2 )  ) )" + cntrsql + " and (prdsn in (SELECT PRDSN FROM WIP_PRDCOMPS WHERE COMPSN='" + tr.Sn + "') or prdsn in ( SELECT R2.IDVAL FROM WIP_IDLIST R2 WHERE R2.PRDSN IN  (SELECT PRDSN FROM WIP_PRDCOMPS WHERE COMPSN='" + tr.Sn + "')  ) ) " + " order by startdt DESC";
                }
                else
                {
                    sql = "select * from V_RPT_WIP_PrdHistory where " + wosql  + " ( (startdt>=@1 and enddt<=@2) OR PRDSN IN ( SELECT R4.IDVAL  FROM WIP_IDLIST R4 WHERE R4.PRDSN IN ( SELECT RA.PRDSN FROM WIP_PRDHISTORY RA WHERE RA.startdt>=@1 and RA.enddt<=@2 )  ) )" + cntrsql + "  order by startdt DESC";

                }

            }
            else
            {
                if (!String.IsNullOrEmpty(tr.Sn))
                {
                    sql = "select * from V_RPT_WIP_PrdHistory where ( prdsn='" + tr.Sn + "' or lotno='" + tr.Sn + "' OR PRDSN IN ( SELECT R4.IDVAL  FROM WIP_IDLIST R4 WHERE R4.PRDSN='" + tr.Sn + "') )  " + cntrsql + "  order by startdt DESC";
                }
                else
                {
                    sql = "select * from V_RPT_WIP_PrdHistory where " + wosql + " ( (startdt>=@1 and enddt<=@2) OR PRDSN IN ( SELECT R4.IDVAL  FROM WIP_IDLIST R4 WHERE R4.PRDSN IN ( SELECT RA.PRDSN FROM WIP_PRDHISTORY RA WHERE RA.startdt>=@1 and RA.enddt<=@2 )  ) )"  + cntrsql + " order by startdt DESC";
                }
            }

            return await db.QueryAsync(sql, tr.Dt1.Value, tr.Dt2.Value);
        }

        private List<String> GetPrdsns(DataTable dtHistory)
        {
            List<string> listPrdsn = new List<string>();
            foreach (DataRow dr in dtHistory.Rows)
            {
                string sno = dr["PRDSN"].ToString();
                if (!listPrdsn.Contains(sno))
                {
                    listPrdsn.Add(sno);
                }
                sno = (dr["LOTNO"] == DBNull.Value ? "" : dr["LOTNO"].ToString());
                if (string.IsNullOrEmpty(sno))
                    continue;
                if (!listPrdsn.Contains(sno))
                {
                    listPrdsn.Add(sno);
                }

            }
            return listPrdsn;
        }


        public async Task<DataTable> GetWipIDData(List<String> listPrdsn)
        {
            int loopcounter = 100;

            string where = "";
            if (listPrdsn.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = null;

            for (int i = 0; i < listPrdsn.Count; i++)
            {
                where += "'" + listPrdsn[i] + "',";
                if (i == (listPrdsn.Count - 1))
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_WIPIDLIST where " + where + " order by prdsn ";
                    DataTable dt2 = await db.QueryAsync(sql);
                    if (dt == null)
                    {
                        dt = dt2;
                    }
                    else
                    {
                        dt.Merge(dt2);
                    }
                }
                else if (i == loopcounter)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_WIPIDLIST where " + where + " order by prdsn ";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt = dt3;
                    where = "";
                }
                else if ((i % loopcounter) == 0 && i > 0)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_WIPIDLIST where " + where + " order by prdsn ";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt.Merge(dt3);
                    where = "";
                }
            }
            if (dt == null)
                dt = new DataTable();
            return (dt);
        }

        public async Task<DataTable> GetCompData(List<String> listPrdsn)
        {
            int loopcounter = 100;

            string where = "";
            if (listPrdsn.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = null;

            for (int i = 0; i < listPrdsn.Count; i++)
            {
                where += "'" + listPrdsn[i] + "',";
                if (i == (listPrdsn.Count - 1))
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_prdcomps where " + where + " order by prdsn, usedt desc";
                    DataTable dt2 = await db.QueryAsync(sql);
                    if (dt == null)
                    {
                        dt = dt2;
                    }
                    else
                    {
                        dt.Merge(dt2);
                    }
                }
                else if (i == loopcounter)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_prdcomps where " + where + " order by prdsn, usedt desc";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt = dt3;
                    where = "";
                }
                else if ((i % loopcounter) == 0 && i > 0)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_prdcomps where " + where + " order by prdsn, usedt desc";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt.Merge(dt3);
                    where = "";
                }
            }
            if (dt == null)
                dt = new DataTable();
            return (dt);
        }

        public async Task<DataTable> GetProcData(List<string> listPrdsn)
        {
            int loopcounter = 100;

            string where = "";
            if (listPrdsn.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = null;
            for (int i = 0; i < listPrdsn.Count; i++)
            {
                where += "'" + listPrdsn[i] + "',";
                if (i == (listPrdsn.Count - 1))
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_procData where " + where + " order by prdsn, PrDateTime DESC";
                    DataTable dt2 = await db.QueryAsync(sql);
                    if (dt == null)
                    {
                        dt = dt2;
                    }
                    else
                    {
                        dt.Merge(dt2);
                    }
                }
                else if (i == loopcounter)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_procData where " + where + " order by prdsn, PrDateTime DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt = dt3;
                    where = "";
                }
                else if ((i % loopcounter) == 0 && i > 0)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from v_wip_procData where " + where + " order by prdsn, PrDateTime DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt.Merge(dt3);
                    where = "";
                }
            }

            if (dt == null)
                dt = new DataTable();
            return (dt);
        }

        public async Task<DataTable> GetContainerData(List<string> listPrdsn)
        {

            int loopcounter = 100;

            string where = "";
            if (listPrdsn.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = null;
            for (int i = 0; i < listPrdsn.Count; i++)
            {
                where += "'" + listPrdsn[i] + "',";
                if (i == (listPrdsn.Count - 1))
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "SUBCONTAINERNO in (" + where + ")";
                    string sql = "select * from V_WMS_CONTAINER where " + where + " order by updtime DESC";
                    DataTable dt2 = await db.QueryAsync(sql);
                    if (dt == null)
                    {
                        dt = dt2;
                    }
                    else
                    {
                        dt.Merge(dt2);
                    }
                }
                else if (i == loopcounter)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "SUBCONTAINERNO in (" + where + ")";
                    string sql = "select * from V_WMS_CONTAINER where " + where + " order by updtime DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt = dt3;
                    where = "";
                }
                else if ((i % loopcounter) == 0 && i > 0)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "SUBCONTAINERNO in (" + where + ")";
                    string sql = "select * from V_WMS_CONTAINER where " + where + " order by updtime DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt.Merge(dt3);
                    where = "";
                }
            }

            if (dt == null)
                dt = new DataTable();
            return (dt);

        }

        public async Task<DataTable> GetTicketData(List<string> listPrdsn)
        {
            int loopcounter = 100;

            string where = "";
            if (listPrdsn.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = null;
            for (int i = 0; i < listPrdsn.Count; i++)
            {
                where += "'" + listPrdsn[i] + "',";
                if (i == (listPrdsn.Count - 1))
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_QC_TICKET where " + where + " order by sub_upddate DESC";
                    DataTable dt2 = await db.QueryAsync(sql);
                    if (dt == null)
                    {
                        dt = dt2;
                    }
                    else
                    {
                        dt.Merge(dt2);
                    }
                }
                else if (i == loopcounter)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_QC_TICKET where " + where + " order by sub_upddate DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt = dt3;
                    where = "";
                }
                else if ((i % loopcounter) == 0 && i > 0)
                {
                    where = where.Substring(0, where.Length - 1);
                    where = "prdsn in (" + where + ")";
                    string sql = "select * from V_QC_TICKET where " + where + " order by sub_upddate DESC";
                    DataTable dt3 = await db.QueryAsync(sql);
                    dt.Merge(dt3);
                    where = "";
                }
            }

            if (dt == null)
                dt = new DataTable();
            return (dt);
        }
    }
}