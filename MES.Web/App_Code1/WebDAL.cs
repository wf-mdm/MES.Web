using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

/// <summary>
/// WebDAL 的摘要说明
/// </summary>
public class WebDAL
{
	public WebDAL()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static DataTable GetHistoryData(string prdsn, string wo_id, System.DateTime startDatetime, System.DateTime endDatetime,bool isbackwards=false, string cntrno="")
    {
        string sql;
        if (startDatetime == DateTime.MinValue)
        {
            startDatetime = new DateTime(2000, 1, 1);
        }
        if (endDatetime == DateTime.MaxValue)
        {
            endDatetime = new DateTime(3000, 1, 1);
        }
        string cntrsql = "";
        if (!string.IsNullOrEmpty(cntrno))
            cntrsql = " or (prdsn in (select SUBCONTAINERNO from WMS_CONTAINERSUB where CONTAINERNO='" + cntrno + "') )";
        string wosql = "";
        if (!string.IsNullOrEmpty(wo_id))
            wosql = " wo_id = '" + wo_id + "' and ";
        if (isbackwards)
        {
            if (!string.IsNullOrEmpty(prdsn))
            {
                sql = "select * from V_RPT_WIP_PrdHistory where ( "+wosql + " startdt >= @1 and enddt <=@2 and prdsn in (SELECT PRDSN FROM WIP_PRDCOMPS WHERE COMPSN='" + prdsn + "') ) " + cntrsql + " order by startdt DESC";
            }
            else
            {
                sql = "select * from V_RPT_WIP_PrdHistory where ( " + wosql + "  startdt >= @1 and enddt <=@2 )  " + cntrsql + " order by startdt DESC";

            }

        }
        else
        {
            if (prdsn != "")
            {
                sql = "select * from V_RPT_WIP_PrdHistory where ( prdsn='" + prdsn +
                    "' or lotno='" +
                    prdsn + "') " + cntrsql + " order by startdt DESC";
            }
            else
            {
                sql = "select * from V_RPT_WIP_PrdHistory where ( " + wosql + "  startdt >= @1 and enddt <=@2 ) " + cntrsql + " order by startdt DESC";
            }
        }
        return WebUtils.DB.Query(sql,startDatetime,endDatetime);
    }

    public static DataTable GetCompData(List<string> listPrdsn)
    {
        int loopcounter = 100;

        string where = "";
        if (listPrdsn.Count == 0)
        {
            return new DataTable();
        }
        DataTable dt=null;

        for (int i = 0; i < listPrdsn.Count; i++)
        {
            where += "'" + listPrdsn[i] + "',";
            if (i==(listPrdsn.Count-1))
            {
                where = where.Substring(0, where.Length - 1);
                where = "prdsn in (" + where + ")";
                string sql = "select * from v_wip_prdcomps where " + where + " order by prdsn, usedt desc" ;
                DataTable dt2 = WebUtils.DB.Query(sql);
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
                DataTable dt3  = WebUtils.DB.Query(sql);
                dt = dt3;
                where = "";
            }
            else if ((i % loopcounter) == 0 && i > 0)
            {
                where = where.Substring(0, where.Length - 1);
                where = "prdsn in (" + where + ")";
                string sql = "select * from v_wip_prdcomps where " + where + " order by prdsn, usedt desc";
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt.Merge(dt3);
                where = "";
            }
        }
        if (dt == null)
            dt = new DataTable(); 
        return (dt);
    }

    public static DataTable GetProcData(List<string> listPrdsn)
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
                DataTable dt2 = WebUtils.DB.Query(sql);
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
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt = dt3;
                where = "";
            }
            else if ((i % loopcounter) == 0 && i > 0)
            {
                where = where.Substring(0, where.Length - 1);
                where = "prdsn in (" + where + ")";
                string sql = "select * from v_wip_procData where " + where + " order by prdsn, PrDateTime DESC";
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt.Merge(dt3);
                where = "";
            }
        }

        if (dt == null)
            dt = new DataTable();
        return (dt);
    }

    public static DataTable GetContainerData(List<string> listPrdsn)
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
                DataTable dt2 = WebUtils.DB.Query(sql);
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
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt = dt3;
                where = "";
            }
            else if ((i % loopcounter) == 0 && i > 0)
            {
                where = where.Substring(0, where.Length - 1);
                where = "SUBCONTAINERNO in (" + where + ")";
                string sql = "select * from V_WMS_CONTAINER where " + where + " order by updtime DESC";
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt.Merge(dt3);
                where = "";
            }
        }

        if (dt == null)
            dt = new DataTable();
        return (dt);
         
    }

    public static DataTable GetTicketData(List<string> listPrdsn)
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
                DataTable dt2 = WebUtils.DB.Query(sql);
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
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt = dt3;
                where = "";
            }
            else if ((i % loopcounter) == 0 && i > 0)
            {
                where = where.Substring(0, where.Length - 1);
                where = "prdsn in (" + where + ")";
                string sql = "select * from V_QC_TICKET where " + where + " order by sub_upddate DESC";
                DataTable dt3 = WebUtils.DB.Query(sql);
                dt.Merge(dt3);
                where = "";
            }
        }

        if (dt == null)
            dt = new DataTable();
        return (dt); 
    }


    public static string[] GetPlanNoByPartNo(string partno)
    {
        string sql = "SELECT DISTINCT  QCPLANNAME FROM  QC_PRDCHKPLAN  WHERE PARTNO='" + partno + "'";
        DataTable dt = WebUtils.DB.Query(sql);
        string[] aryPlanNo = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            aryPlanNo[i] = dt.Rows[i][0].ToString();
        }
        return aryPlanNo;
    }

    private static Hashtable FillDataToHashtable(DataTable dt)
    {
        Hashtable ht = new Hashtable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!ht.Contains(dt.Rows[i][0].ToString()))
            {
                ht.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }
        }
        return ht;
    }

    public static Hashtable GetAllLineName()
    {
        string sql = "SELECT LINENAME, LINENAME + '[' + DISPLAYNAME + ']' FROM ENG_PRDLINE";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static Hashtable GetOPByLine(string line)
    {
        string sql = "SELECT L_OPNO, L_OPNO + '[' + DISPLAYNAME + ']' FROM ENG_LINEOP WHERE LINENAME='" + line + "'";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static Hashtable GetSTNByLine(string line, string op)
    {
        string sql = "SELECT L_STNO,DISPLAYNAME FROM ENG_LINESTATION WHERE LINENAME='" +
            line + "' AND L_OPNO='" + op + "'";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static DataTable GetParamConf(string line, string op, string stn, string conf)
    {
        string sql = "SELECT * FROM ENG_LINEOPPARAMCONF a left join QC_PRDCHKPLAN b on " +
            "a.CONFID = b.CONFID where a.CONFNAME = '" + conf + "' and (a.LINENAME='" + line +
            "' OR a.LINENAME='ALL') AND (a.L_OPNO='" + op + "' OR a.L_OPNO='ALL') AND (a.L_STNO='" + stn +
            "' OR a.L_STNO='ALL' OR a.L_STNO IS NULL OR a.L_STNO='') ";
        return WebUtils.DB.Query(sql);
    }

    public static DataTable GetAllUsers()
    {
        string sql = "select * from HR_OPERATORS where ACTIVE='Y' ";
        return WebUtils.DB.Query(sql);
    }

    public static bool DelUser(string uid)
    {
        string sql = "update HR_OPERATORS set ACTIVE='N' where operid='" + uid + "'";
        return WebUtils.DB.Update(sql) > 0;
    }

    private static string[] FillDataToArray(DataTable dt)
    {
        string[] ary = new string[dt.Rows.Count];
        for (int i = 0; i < ary.Length; i++)
        {
            ary[i] = dt.Rows[i][0].ToString();
        }
        return ary;
    }

    public static string[] GetTicketType()
    {
        string sql = "SELECT DISTINCT  EVT_TYPE  FROM QC_WKFLOWCFG";
        return FillDataToArray(WebUtils.DB.Query(sql));
    }

    public static string GetSPID()
    {
        //long n =  Intelli.MidW.Utils.DB.SPID.GetIntSID("recid.qa_workticket");
        //return n.ToString().PadLeft(12, '0'); 
        Intelli.MidW.Utils.DB.SPID spidcls = new Intelli.MidW.Utils.DB.SPID();
        if (Intelli.MidW.Utils.DB.SPID.DB == null)
            Intelli.MidW.Utils.DB.SPID.DB = WebUtils.DB;
        long n = Intelli.MidW.Utils.DB.SPID.GetIntSID("recid.qa_workticket");
        return n.ToString().PadLeft(12, '0');

    }

    public static string[] GetPlanNo(string PrdNo)
    {
        string sql = "SELECT DISTINCT  QCPLANNAME FROM  QC_PRDCHKPLAN";
        if (PrdNo.Trim() != "" && PrdNo.Trim().ToUpper() != "ALL")
        {
            sql = "SELECT DISTINCT  QCPLANNAME FROM  QC_PRDCHKPLAN  WHERE PARTNO='" + PrdNo + "'";
        }
        return FillDataToArray(WebUtils.DB.Query(sql));
    }

    public static DataTable GetAllPlanStatus()
    {
        string sql = "SELECT * from ENG_codecfg  where CODENAME='QCTICKETST'";
        return WebUtils.DB.Query(sql); ;
    }

    public static DataTable GetWorkTicket(string ticketNo)
    {
        string sql = "select * from QC_WORKTICKET where EVT_ID = '" + ticketNo + "'";
        return WebUtils.DB.Query(sql);
    }

    public static DataTable GetTicketDetail(string ticketNo)
    {
        string sql = "select * from QC_TICKETEXEC where EVT_ID = '" + ticketNo + "' order by upddate";
        return WebUtils.DB.Query(sql);
    }

    public static string[] GetStepNo(string TicketType)
    {
        string sql = "SELECT  STEPNO from QC_WKFLOWCFG where EVT_TYPE='" + TicketType + "'";
        return FillDataToArray(WebUtils.DB.Query(sql));
    }

    public static DataTable GetBomHeader()
    {
        string sql = "select * from eng_bomheader order by partno, partver";
        return WebUtils.DB.Query(sql);
    }

    public static DataTable GetBomHeader(string partno, string partver)
    {
        string sql = "select * from eng_bomheader where partno=@1 and partver=@2";
        return WebUtils.DB.Query(sql, partno, partver);
    }

    public static DataTable GetBomDetail(string partno, string partver)
    {
        string sql = "select * from eng_bomdetail where partno=@1 and partver=@2 order by itemid";
        return WebUtils.DB.Query(sql, partno, partver);
    }

    public static DataTable GetBomDetail(string itemId)
    {
        string sql = "select * from eng_bomdetail where itemid=@1";
        return WebUtils.DB.Query(sql, itemId);
    }

    public static Hashtable GetQFFileType()
    {
        string sql = "SELECT DISTINCT CODEVAL,CODEDESC FROM ENG_CODECFG  WHERE CODENAME = 'QAFILETYPE'";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static Hashtable GetQFFileStatus()
    {
        string sql = "SELECT DISTINCT CODEVAL,CODEDESC FROM ENG_CODECFG  WHERE CODENAME = 'QAFILESTATUS'";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static Hashtable GetDept()
    {
        string sql = "SELECT DISTINCT DEPTNO,DEPTNAME FROM HR_DEPTSTR";
        return FillDataToHashtable(WebUtils.DB.Query(sql));
    }

    public static bool UpdateBomHeader(string partno, string partver, Hashtable htData)
    {
        string sql = "update eng_bomheader set REPEATLIMIT=@3, " +
            "bomactive=@4, UPDATETIME=@5, SN_AUTOFMT=@6, LOTNOFMT=@7, BLOCKQTY=@8, " +
            "PARTGRP2=@9, RT_NAME=@10, ISVSN=@11, CONTNRTYPE=@12, PRDUNIT=@13, " +
            "SERIALCONTROL=@14, CUSTCOMMENT=@15, CUSTNAME=@16, CUSTDESC=@17, LOTQTY=@18, " +
            "LotControl=@19, Description=@20, WARNINGQTY=@21, SN_PATTERN=@22, " +
            "NLANG_DESC=@23, CUSTPARTNO=@24, EmptyRun=@25, DEFAULT_CONFNAME=@26, " +
            "DEFAULTLOTQTY=@27 where partno=@1 and partver=@2";
        int res = WebUtils.DB.Update(sql, partno, partver, htData["REPEATLIMIT"],
            htData["bomactive"], DateTime.Now, htData["SN_AUTOFMT"], htData["LOTNOFMT"], htData["BLOCKQTY"], 
            htData["PARTGRP2"], htData["RT_NAME"], htData["ISVSN"], htData["CONTNRTYPE"],  htData["PRDUNIT"], 
            htData["SERIALCONTROL"], htData["CUSTCOMMENT"], htData["CUSTNAME"],  htData["CUSTDESC"], htData["LOTQTY"],
            htData["LotControl"], htData["Description"],  htData["WARNINGQTY"], htData["SN_PATTERN"],        
            htData["NLANG_DESC"], htData["CUSTPARTNO"], htData["EmptyRun"], htData["DEFAULT_CONFNAME"], 
            htData["DEFAULTLOTQTY"]);
        return res > 0;
    }

    public static bool InsertBomHeader(Hashtable htData)
    {
        string sql = "insert into eng_bomheader (partno,partver,REPEATLIMIT,bomactive,CREATETIME,UPDATETIME," +
            "SN_AUTOFMT,LOTNOFMT,BLOCKQTY,PARTGRP2,RT_NAME,ISVSN,CONTNRTYPE,PRDUNIT,SERIALCONTROL,CUSTCOMMENT," +
            "CUSTNAME,CUSTDESC,LOTQTY,LotControl,Description,WARNINGQTY,SN_PATTERN,NLANG_DESC,CUSTPARTNO," +
            "EmptyRun,DEFAULT_CONFNAME,DEFAULTLOTQTY, AUTOWOFMT) values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, " +
            "@12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23, @24, @25, @26, @27, @28, @29)";
        int res = WebUtils.DB.Update(sql, htData["partno"], htData["partver"], htData["REPEATLIMIT"],
            htData["bomactive"], DateTime.Now, DateTime.Now, htData["SN_AUTOFMT"], htData["LOTNOFMT"], htData["BLOCKQTY"],
            htData["PARTGRP2"], htData["RT_NAME"], htData["ISVSN"], htData["CONTNRTYPE"], htData["PRDUNIT"],
            htData["SERIALCONTROL"], htData["CUSTCOMMENT"], htData["CUSTNAME"], htData["CUSTDESC"], htData["LOTQTY"],
            htData["LotControl"], htData["Description"], htData["WARNINGQTY"], htData["SN_PATTERN"],
            htData["NLANG_DESC"], htData["CUSTPARTNO"], htData["EmptyRun"], htData["DEFAULT_CONFNAME"],
            htData["DEFAULTLOTQTY"], htData["AUTOWOFMT"]);
        return res > 0;
    }

    public static bool UpdateBomDetail(Hashtable htData)
    {
        string sql = "UPDATE  ENG_BOMDETAIL SET PARTNO=@1, PARTVER=@2, L_OPNO=@3, ITEMTYPE=@4," +
            " ORG_PARTNO=@5, COMP_PARTNO=@6, COMP_PARTVER=@7, SN_PATTERN=@8, DESCRIPTION=@9," +
            " REPEATLIMIT=@10, NLANG_DESC=@11, UNITCONSUMEQTY=@12, COMP_UNIT=@13," +
            " LotControl=@14, SERIALCONTROL=@15, IsKeyID=@16, CNTCONTROL=@17," +
            " DEFAULTLOTQTY=@18, WARNINGQTY=@19, BLOCKQTY=@20, WARNTIMES=@21, UPDATETIME=@22," +
            " BUFFERQTY=@23, EMPTYRUN=@24, ISSEMI=@25, SEMILINEGRP=@26 WHERE ITEMID=@27 ";

        int res = WebUtils.DB.Update(sql, htData["PARTNO"], htData["PARTVER"], htData["L_OPNO"], htData["ITEMTYPE"],
            htData["ORG_PARTNO"], htData["COMP_PARTNO"], htData["COMP_PARTVER"], htData["SN_PATTERN"],
            htData["DESCRIPTION"], htData["REPEATLIMIT"], htData["NLANG_DESC"], htData["UNITCONSUMEQTY"],
            htData["COMP_UNIT"], htData["LotControl"], htData["SERIALCONTROL"], htData["IsKeyID"],
            htData["CNTCONTROL"], htData["DEFAULTLOTQTY"], htData["WARNINGQTY"], htData["BLOCKQTY"],
            htData["WARNTIMES"], htData["UPDATETIME"], htData["BUFFERQTY"], htData["EMPTYRUN"],
            htData["ISSEMI"], htData["SEMILINEGRP"], htData["ITEMID"]);
        return res > 0;
    }

    public static bool InsertBomDetail(Hashtable htData)
    {
        string sql = "select top 1 itemid from eng_bomdetail order by itemid desc";
        DataTable dt = WebUtils.DB.Query(sql);
        int itemId = 1;
        if (dt.Rows.Count > 0)
        {
            itemId = int.Parse(dt.Rows[0][0].ToString());
            itemId++;
        }
        htData["ITEMID"] = itemId;
        sql = "INSERT INTO ENG_BOMDETAIL (ITEMID, PARTNO, PARTVER, L_OPNO, ITEMTYPE, ORG_PARTNO," +
            " COMP_PARTNO, COMP_PARTVER, SN_PATTERN, DESCRIPTION, REPEATLIMIT," +
            " NLANG_DESC, UNITCONSUMEQTY, COMP_UNIT, LotControl, SERIALCONTROL," +
            " IsKeyID, CNTCONTROL, DEFAULTLOTQTY, WARNINGQTY, BLOCKQTY, WARNTIMES," +
            " UPDATETIME, BUFFERQTY, EMPTYRUN, ISSEMI, SEMILINEGRP) values (@1, @2," +
            " @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18," +
            " @19, @20, @21, @22, @23, @24, @25, @26, @27)";
        int res = WebUtils.DB.Update(sql, htData["ITEMID"], htData["PARTNO"], htData["PARTVER"], htData["L_OPNO"], htData["ITEMTYPE"],
            htData["ORG_PARTNO"], htData["COMP_PARTNO"], htData["COMP_PARTVER"], htData["SN_PATTERN"],
            htData["DESCRIPTION"], htData["REPEATLIMIT"], htData["NLANG_DESC"], htData["UNITCONSUMEQTY"],
            htData["COMP_UNIT"], htData["LotControl"], htData["SERIALCONTROL"], htData["IsKeyID"],
            htData["CNTCONTROL"], htData["DEFAULTLOTQTY"], htData["WARNINGQTY"], htData["BLOCKQTY"],
            htData["WARNTIMES"], htData["UPDATETIME"], htData["BUFFERQTY"], htData["EMPTYRUN"],
            htData["ISSEMI"], htData["SEMILINEGRP"]);
        return res > 0;
    }

    public static DataTable GetPartGroup()
    {
        string sql = "select * from wms_partgrp";
        return WebUtils.DB.Query(sql);
    }

    public static DataTable GetPartData(string partGrpNo, string partGrpNo2)
    {
        string sql = "select * from wms_partdata where partGrpNo=@1 and partGrpNo2=@2";
        return WebUtils.DB.Query(sql, partGrpNo, partGrpNo2);
    }

    public static DataTable GetPartTypeID()
    {
        string sql = "select * from eng_codecfg where codename='PARTTYPEID'";
        return WebUtils.DB.Query(sql);
    }

    public static bool Insert_WMS_PARTDATA(Hashtable htData)
    {
        string sql = "INSERT INTO WMS_PARTDATA (PARTNO, PARTVER, PARTTYPE, PARTGRPNO, PARTGRPNO2," +
            " ACTIVE, DESCRIPTION, SPEC, NLANG_DESC, ABCTYPE, SN_PATTERN, DEFAULTUNIT," +
            " LotControl, VALIDCONTROL, FIFOCONTROL, IQCCONTROL, LOTQTY, SAFTQTY," +
            " STDPRICE, STDCURR, SERIALCONTROL, DEFAULTWHNO, DEFAULTLOCNO, DEFAULTCNTRTYPE," +
            " VALIDDAYS, UPDATETIME, COMMENTS, TXTPROP6, TXTPROP5, TXTPROP4," +
            " TXTPROP3, TXTPROP2, TXTPROP1, DEFAULTCONF) values (@1, @2, @3, @4," +
            " @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19," +
            " @20, @21, @22, @23, @24, @25, @26, @27, @28, @29, @30, @31, @32, @33," +
            " @34)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PARTNO"], htData["PARTVER"], htData["PARTTYPE"],
                htData["PARTGRPNO"], htData["PARTGRPNO2"],
                Convert.ToChar(htData["ACTIVE"]), htData["DESCRIPTION"], htData["SPEC"], htData["NLANG_DESC"],
                Convert.ToChar(htData["ABCTYPE"]), htData["SN_PATTERN"], htData["DEFAULTUNIT"], Convert.ToChar(htData["LotControl"]),
                Convert.ToChar(htData["VALIDCONTROL"]), Convert.ToChar(htData["FIFOCONTROL"]), Convert.ToChar(htData["IQCCONTROL"]), htData["LOTQTY"],
                htData["SAFTQTY"], htData["STDPRICE"], htData["STDCURR"], Convert.ToChar(htData["SERIALCONTROL"]),
                htData["DEFAULTWHNO"], htData["DEFAULTLOCNO"], htData["DEFAULTCNTRTYPE"], htData["VALIDDAYS"],
                htData["UPDATETIME"], htData["COMMENTS"], htData["TXTPROP6"], htData["TXTPROP5"],
                htData["TXTPROP4"], htData["TXTPROP3"], htData["TXTPROP2"], htData["TXTPROP1"],
                htData["DEFAULTCONF"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_WMS_PARTDATA Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_WMS_PARTDATA(string partno, string partver)
    {
        string sql = "SELECT * FROM WMS_PARTDATA WHERE PARTNO = @1 and PARTVER = @2 ";
        try
        {
            return WebUtils.DB.Query(sql, partno, partver);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTDATA Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Update_WMS_PARTDATA(Hashtable htData)
    {
        string sql = "UPDATE  WMS_PARTDATA SET PARTTYPE=@1, PARTGRPNO=@2, PARTGRPNO2=@3, ACTIVE=@4," +
            " DESCRIPTION=@5, SPEC=@6, NLANG_DESC=@7, ABCTYPE=@8, SN_PATTERN=@9," +
            " DEFAULTUNIT=@10, LotControl=@11, VALIDCONTROL=@12, FIFOCONTROL=@13," +
            " IQCCONTROL=@14, LOTQTY=@15, SAFTQTY=@16, STDPRICE=@17, STDCURR=@18, SERIALCONTROL=@19," +
            " DEFAULTWHNO=@20, DEFAULTLOCNO=@21, DEFAULTCNTRTYPE=@22," +
            " VALIDDAYS=@23, UPDATETIME=@24, COMMENTS=@25, TXTPROP6=@26, TXTPROP5=@27," +
            " TXTPROP4=@28, TXTPROP3=@29, TXTPROP2=@30, TXTPROP1=@31, DEFAULTCONF=@32 WHERE PARTNO=@33 and PARTVER=@34 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PARTTYPE"], htData["PARTGRPNO"], htData["PARTGRPNO2"],
                Convert.ToChar(htData["ACTIVE"]), htData["DESCRIPTION"], htData["SPEC"], htData["NLANG_DESC"],
                Convert.ToChar(htData["ABCTYPE"]), htData["SN_PATTERN"], htData["DEFAULTUNIT"], Convert.ToChar(htData["LotControl"]),
                Convert.ToChar(htData["VALIDCONTROL"]), Convert.ToChar(htData["FIFOCONTROL"]), Convert.ToChar(htData["IQCCONTROL"]), htData["LOTQTY"],
                htData["SAFTQTY"], htData["STDPRICE"], htData["STDCURR"], Convert.ToChar(htData["SERIALCONTROL"]),
                htData["DEFAULTWHNO"], htData["DEFAULTLOCNO"], htData["DEFAULTCNTRTYPE"], htData["VALIDDAYS"],
                htData["UPDATETIME"], htData["COMMENTS"], htData["TXTPROP6"], htData["TXTPROP5"],
                htData["TXTPROP4"], htData["TXTPROP3"], htData["TXTPROP2"], htData["TXTPROP1"],
                htData["DEFAULTCONF"], htData["PARTNO"], htData["PARTVER"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_WMS_PARTDATA Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_WMS_PARTDATA(string partno, string partver)
    {
        string sql = "DELETE FROM WMS_PARTDATA WHERE PARTNO = @1 and PARTVER = @2 ";
        try
        {
            return WebUtils.DB.Update(sql, partno, partver) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_WMS_PARTDATA Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_WMS_PARTUNITCONVERT(string partno)
    {
        string sql = "SELECT * FROM WMS_PARTUNITCONVERT WHERE PARTNO = @1";
        try
        {
            return WebUtils.DB.Query(sql, partno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTUNITCONVERT()
    {
        string sql = "SELECT * FROM WMS_PARTUNITCONVERT";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_WMS_PARTUNITCONVERT(Hashtable htData)
    {
        string sql = "INSERT INTO WMS_PARTUNITCONVERT (PARTNO, PART_UNITFROM, PART_UNITTO, ISGRP," +
            " CONVERTRATE) values (@1, @2, @3, @4, @5)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PARTNO"], htData["PART_UNITFROM"], htData["PART_UNITTO"],
                Convert.ToChar(htData["ISGRP"]), htData["CONVERTRATE"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_WMS_PARTUNITCONVERT(string partno, string part_unitfrom, string part_unitto)
    {
        string sql = "SELECT * FROM WMS_PARTUNITCONVERT WHERE PARTNO = @1 and PART_UNITFROM = @2 and PART_UNITTO = @3 ";
        try
        {
            return WebUtils.DB.Query(sql, partno, part_unitfrom, part_unitto);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Delete_WMS_PARTUNITCONVERT(string partno, string part_unitfrom, string part_unitto)
    {
        string sql = "DELETE FROM WMS_PARTUNITCONVERT WHERE PARTNO = @1 and PART_UNITFROM = @2 and PART_UNITTO = @3 ";
        try
        {
            return WebUtils.DB.Update(sql, partno, part_unitfrom, part_unitto) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_WMS_PARTUNITCONVERT(Hashtable htData)
    {
        string sql = "UPDATE  WMS_PARTUNITCONVERT SET ISGRP=@1, CONVERTRATE=@2 WHERE PARTNO=@3 and PART_UNITFROM=@4 and PART_UNITTO=@5 ";
        try
        {
            int res = WebUtils.DB.Update(sql, Convert.ToChar(htData["ISGRP"]), htData["CONVERTRATE"],
                htData["PARTNO"], htData["PART_UNITFROM"], htData["PART_UNITTO"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_WMS_PARTUNITCONVERT Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_WMS_PARTVENDOR()
    {
        string sql = "SELECT * FROM WMS_PARTVENDOR";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTVENDOR Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTVENDOR(string partno)
    {
        string sql = "SELECT * FROM WMS_PARTVENDOR WHERE PARTNO = @1";
        try
        {
            return WebUtils.DB.Query(sql, partno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTVENDOR Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTVENDOR(string partno, string vendorno)
    {
        string sql = "SELECT * FROM WMS_PARTVENDOR WHERE PARTNO = @1 and VENDORNO = @2 ";
        try
        {
            return WebUtils.DB.Query(sql, partno, vendorno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTVENDOR Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Update_WMS_PARTVENDOR(Hashtable htData)
    {
        string sql = "UPDATE  WMS_PARTVENDOR SET PRIORITY=@1, ACTIVE=@2, COMMENTS=@3 WHERE PARTNO=@4 and VENDORNO=@5 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PRIORITY"], Convert.ToChar(htData["ACTIVE"]), htData["COMMENTS"],
                htData["PARTNO"], htData["VENDORNO"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_WMS_PARTVENDOR Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Insert_WMS_PARTVENDOR(Hashtable htData)
    {
        string sql = "INSERT INTO WMS_PARTVENDOR (PARTNO, VENDORNO, PRIORITY, ACTIVE, COMMENTS) values (@1," +
            " @2, @3, @4, @5)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PARTNO"], htData["VENDORNO"], htData["PRIORITY"],
                Convert.ToChar(htData["ACTIVE"]), htData["COMMENTS"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_WMS_PARTVENDOR Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_WMS_PARTVENDOR(string partno, string vendorno)
    {
        string sql = "DELETE FROM WMS_PARTVENDOR WHERE PARTNO = @1 and VENDORNO = @2 ";
        try
        {
            return WebUtils.DB.Update(sql, partno, vendorno) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_WMS_PARTVENDOR Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Insert_WMS_PARTATTRIBUTES(Hashtable htData)
    {
        string sql = "INSERT INTO WMS_PARTATTRIBUTES (PARTNO, PARTVER, PROPERTYNAME, PROPERTYVALUE," +
            " FORCED) values (@1, @2, @3, @4, @5)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PARTNO"], htData["PARTVER"], htData["PROPERTYNAME"],
                htData["PROPERTYVALUE"], Convert.ToChar(htData["FORCED"]));
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_WMS_PARTATTRIBUTES(Hashtable htData)
    {
        string sql = "UPDATE  WMS_PARTATTRIBUTES SET PROPERTYVALUE=@1, FORCED=@2 WHERE PARTNO=@3 and PARTVER=@4 and PROPERTYNAME=@5 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["PROPERTYVALUE"], Convert.ToChar(htData["FORCED"]),
                htData["PARTNO"], htData["PARTVER"], htData["PROPERTYNAME"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_WMS_PARTATTRIBUTES(string partno, string partver, string propertyname)
    {
        string sql = "DELETE FROM WMS_PARTATTRIBUTES WHERE PARTNO = @1 and PARTVER = @2 and PROPERTYNAME = @3 ";
        try
        {
            return WebUtils.DB.Update(sql, partno, partver, propertyname) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_WMS_PARTATTRIBUTES(string partno, string partver, string propertyname)
    {
        string sql = "SELECT * FROM WMS_PARTATTRIBUTES WHERE PARTNO = @1 and PARTVER = @2 and PROPERTYNAME = @3 ";
        try
        {
            return WebUtils.DB.Query(sql, partno, partver, propertyname);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTATTRIBUTES(string partno)
    {
        string sql = "SELECT * FROM WMS_PARTATTRIBUTES WHERE PARTNO = @1";
        try
        {
            return WebUtils.DB.Query(sql, partno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTATTRIBUTES(string partno, string partver)
    {
        string sql = "SELECT * FROM WMS_PARTATTRIBUTES WHERE PARTNO = @1 and PARTVER = @2";
        try
        {
            return WebUtils.DB.Query(sql, partno, partver);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_WMS_PARTATTRIBUTES()
    {
        string sql = "SELECT * FROM WMS_PARTATTRIBUTES";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_WMS_PARTATTRIBUTES Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_HR_DEPTSTR()
    {
        string sql = "SELECT *  FROM HR_DEPTSTR";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_HR_DEPTSTR Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_HR_OPERATORS(Hashtable htData)
    {
        htData["OPERID"] = DateTime.Now.Ticks.ToString();
        string sql = "INSERT INTO HR_OPERATORS (OPERID, OPERNAME, PWD, ROLEID, DEPTNO, ACTIVE," +
            " POSITION, BUNO, EMAIL, CONTACTINFO, COMMENTS) values (@1, @2, @3, @4," +
            " @5, @6, @7, @8, @9, @10, @11)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["OPERID"], htData["OPERNAME"], htData["PWD"],
                htData["ROLEID"], htData["DEPTNO"],
                Convert.ToChar(htData["ACTIVE"]), htData["POSITION"], htData["BUNO"], htData["EMAIL"],
                htData["CONTACTINFO"], htData["COMMENTS"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_HR_OPERATORS Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_HR_OPERATORS(string operid)
    {
        string sql = "SELECT * FROM HR_OPERATORS WHERE OPERID = @1 ";
        try
        {
            return WebUtils.DB.Query(sql, operid);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_HR_OPERATORS Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Update_HR_OPERATORS(Hashtable htData)
    {
        string sql = "UPDATE  HR_OPERATORS SET OPERNAME=@1, PWD=@2, ROLEID=@3, DEPTNO=@4, ACTIVE=@5," +
            " POSITION=@6, BUNO=@7, EMAIL=@8, CONTACTINFO=@9, COMMENTS=@10 WHERE OPERID=@11 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["OPERNAME"], htData["PWD"], htData["ROLEID"], htData["DEPTNO"],
                Convert.ToChar(htData["ACTIVE"]), htData["POSITION"], htData["BUNO"], htData["EMAIL"],
                htData["CONTACTINFO"], htData["COMMENTS"], htData["OPERID"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_HR_OPERATORS Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Insert_ENG_PRDLINE(Hashtable htData)
    {
        string sql = "INSERT INTO ENG_PRDLINE (LINENAME, BUNO, SCRIPTFILE, DISPLAYNAME, LIBSCRIPTFILE," +
            " TYPENAME, STDYIELDRATE, STDHEADCOUNTS, DEPTNO, LINEGRP, LINETYPE) values (@1," +
            " @2, @3, @4, @5, @6, @7, @8, @9, @10, @11)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["LINENAME"], htData["BUNO"], htData["SCRIPTFILE"],
                htData["DISPLAYNAME"], htData["LIBSCRIPTFILE"],
                htData["TYPENAME"], htData["STDYIELDRATE"], htData["STDHEADCOUNTS"], htData["DEPTNO"],
                htData["LINEGRP"], htData["LINETYPE"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_PRDLINE Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_PRDLINE(Hashtable htData)
    {
        string sql = "UPDATE  ENG_PRDLINE SET BUNO=@1, DISPLAYNAME=@2, " +
            "DEPTNO=@3, LINEGRP=@4 WHERE LINENAME=@5 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["BUNO"], htData["DISPLAYNAME"], htData["DEPTNO"],
                htData["LINEGRP"], htData["LINENAME"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_PRDLINE Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_PRDLINE(string linename)
    {
        string sql = "DELETE FROM ENG_PRDLINE WHERE LINENAME = @1 ";
        try
        {
            return WebUtils.DB.Update(sql, linename) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_PRDLINE Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_PRDLINE(string linename)
    {
        string sql = "SELECT * FROM ENG_PRDLINE WHERE LINENAME = @1 ";
        try
        {
            return WebUtils.DB.Query(sql, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_PRDLINE Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_TPM_ASSETS()
    {
        string sql = "SELECT * FROM TPM_ASSETS";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_TPM_ASSETS Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_TPM_ASSETS(string assetno)
    {
        string sql = "SELECT * FROM TPM_ASSETS WHERE ASSETNO=@1";
        try
        {
            return WebUtils.DB.Query(sql,new object[]{assetno});
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_TPM_ASSETS Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable select_ENG_LINENAMEANDGRP()
    {
        string sqltxt = "SELECT LINENAME,LINENAME+':' + DISPLAYNAME DISPLAYNAME FROM ENG_PRDLINE UNION ALL SELECT DISTINCT LINEGRP LINENAME,'Grp:' + LINEGRP DISPLAYNAME FROM ENG_PRDLINE";
        try
        {
            return WebUtils.DB.Query(sqltxt);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_PRDLINE Failed:" + ex.Message);
            return new DataTable();
        }

    }
    public static DataTable Select_ENG_PRDLINE()
    {
        string sql = "SELECT * FROM ENG_PRDLINE";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_PRDLINE Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_ENG_LINEOP(Hashtable htData)
    {
        string sql = "INSERT INTO ENG_LINEOP (LINENAME, L_OPNO, DISPLAYNAME, OPDEFAULTSEQ, REPEATLIMIT) values (@1," +
            " @2, @3, @4, @5)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["LINENAME"], htData["L_OPNO"], htData["DISPLAYNAME"],
                htData["OPDEFAULTSEQ"], htData["REPEATLIMIT"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_LINEOP Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_LINEOP(Hashtable htData)
    {
        string sql = "UPDATE  ENG_LINEOP SET DISPLAYNAME=@1, OPDEFAULTSEQ=@2, REPEATLIMIT=@3 " +
            "WHERE LINENAME=@4 and L_OPNO=@5 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["DISPLAYNAME"], htData["OPDEFAULTSEQ"], 
                htData["REPEATLIMIT"], htData["LINENAME"], htData["L_OPNO"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_LINEOP Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_LINEOP(string linename, string l_opno)
    {
        string sql = "DELETE FROM ENG_LINEOP WHERE LINENAME = @1 and L_OPNO = @2 ";
        try
        {
            return WebUtils.DB.Update(sql, linename, l_opno) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_LINEOP Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_LINEOP(string linename, string l_opno)
    {
        string sql = "SELECT * FROM ENG_LINEOP WHERE LINENAME = @1 and L_OPNO = @2 ";
        try
        {
            return WebUtils.DB.Query(sql, linename, l_opno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEOP Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_LINEOP(string linename)
    {
        string sql = "SELECT * FROM ENG_LINEOP WHERE LINENAME = @1";
        try
        {
            return WebUtils.DB.Query(sql, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEOP Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_ENG_LINESTATION(Hashtable htData)
    {
        string sql = "INSERT INTO ENG_LINESTATION (LINENAME, L_OPNO, L_STNO, DISPLAYNAME" +
            ") values (@1, @2, @3, @4)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["LINENAME"], htData["L_OPNO"], htData["L_STNO"],
                htData["DISPLAYNAME"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_LINESTATION Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_LINESTATION(Hashtable htData)
    {
        string sql = "UPDATE  ENG_LINESTATION SET L_OPNO=@1, DISPLAYNAME=@2 WHERE LINENAME=@3 and L_STNO=@4 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["L_OPNO"], htData["DISPLAYNAME"], htData["LINENAME"], htData["L_STNO"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_LINESTATION Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_LINESTATION(string linename, string l_stno)
    {
        string sql = "DELETE FROM ENG_LINESTATION WHERE LINENAME = @1 and L_STNO = @3 ";
        try
        {
            return WebUtils.DB.Update(sql, linename, l_stno) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_LINESTATION Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_LINESTATION(string linename, string l_stno)
    {
        string sql = "SELECT * FROM ENG_LINESTATION WHERE LINENAME = @1 and L_STNO = @3 ";
        try
        {
            return WebUtils.DB.Query(sql, linename, l_stno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINESTATION Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_LINESTATION(string linename)
    {
        string sql = "SELECT * FROM ENG_LINESTATION WHERE LINENAME = @1";
        try
        {
            return WebUtils.DB.Query(sql, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINESTATION Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_QC_FILELIST()
    {
        string sql = "SELECT * FROM QC_FILELIST";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_FILELIST Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_QC_FILELIST(Hashtable htData)
    {
        string content = " where 1=1 ";
        foreach (string key in htData.Keys)
        {
            if (htData[key].ToString() == "")
            {
                continue;
            }
            content += " and " + key + "='" + htData[key].ToString().Replace("'", "''") + "'"; 
        }
        string sql = "SELECT * FROM QC_FILELIST" + content;
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_FILELIST Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Update_QC_FILELIST(Hashtable htData)
    {
        string sql = "UPDATE  QC_FILELIST SET FILEINFO=@1," +
            " FILEUPDTIME=@2, UPDATEEMPNO=@3, SRCSYSNO=@4," +
            " LINKQASYSNO=@5, COMMENTS=@6 WHERE RECID=@7 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["FILEINFO"], htData["FILEUPDTIME"], htData["UPDATEEMPNO"],
                htData["SRCSYSNO"], htData["LINKQASYSNO"], htData["COMMENTS"], htData["RECID"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_QC_FILELIST Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_QC_FILELIST(string recid)
    {
        string sql = "DELETE FROM QC_FILELIST WHERE RECID = @1 ";
        try
        {
            return WebUtils.DB.Update(sql, recid) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_QC_FILELIST Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_LINEPARTCONF(Hashtable htData)
    {
        string sql = "UPDATE  ENG_LINEPARTCONF SET SETTINGType=@1, STDYIELDRATE=@2, CONTNRTYPE=@3," +
            " RT_NAME=@4, STDHEADCOUNTS=@5, MODELCHGTIME=@6, MODELTYPE=@7, DEFAULT_CONFNAME=@8 WHERE LINENAME=@9 and PARTNO=@10 and PARTVER=@11 ";
        try
        {
            int res = WebUtils.DB.Update(sql, Convert.ToChar(htData["SETTINGType"]), htData["STDYIELDRATE"],
                htData["CONTNRTYPE"], htData["RT_NAME"], htData["STDHEADCOUNTS"], htData["MODELCHGTIME"],
                htData["MODELTYPE"], htData["DEFAULT_CONFNAME"], htData["LINENAME"], htData["PARTNO"], htData["PARTVER"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_LINEPARTCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_LINEPARTCONF(string linename, string partno, string partver)
    {
        string sql = "DELETE FROM ENG_LINEPARTCONF WHERE LINENAME = @1 and PARTNO = @2 and PARTVER = @3 ";
        try
        {
            return WebUtils.DB.Update(sql, linename, partno, partver) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_LINEPARTCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_LINEPARTCONF(string linename, string partno, string partver)
    {
        string sql = "SELECT * FROM ENG_LINEPARTCONF WHERE LINENAME = @1 and PARTNO = @2 and PARTVER = @3 ";
        try
        {
            return WebUtils.DB.Query(sql, linename, partno, partver);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEPARTCONF Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_LINEPARTCONF(string linename, string partno)
    {
        string sql = "SELECT * FROM ENG_LINEPARTCONF WHERE LINENAME = @1 and PARTNO = @2";
        try
        {
            return WebUtils.DB.Query(sql, linename, partno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEPARTCONF Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_LINEPARTCONF(string linename)
    {
        string sql = "SELECT * FROM ENG_LINEPARTCONF WHERE LINENAME = @1";
        try
        {
            return WebUtils.DB.Query(sql, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEPARTCONF Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_LINEPARTCONF()
    {
        string sql = "SELECT * FROM ENG_LINEPARTCONF";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEPARTCONF Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_ENG_LINEPARTCONF(Hashtable htData)
    {
        string sql = "INSERT INTO ENG_LINEPARTCONF (LINENAME, PARTNO, PARTVER, SETTINGType, STDYIELDRATE," +
            " CONTNRTYPE, RT_NAME, DEFAULT_CONFNAME) values (@1, @2, @3, @4, @5, @6, @7, @8)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["LINENAME"], htData["PARTNO"], htData["PARTVER"],
                Convert.ToChar(htData["SETTINGTYPE"]), htData["STDYIELDRATE"],
                htData["CONTNRTYPE"], htData["RT_NAME"], htData["DEFAULT_CONFNAME"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_LINEPARTCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable GetCodeCfg(string codeName)
    {
        string sql = "SELECT CODEVAL,CODEDESC FROM ENG_CODECFG WHERE CODENAME='" + codeName + "'";
        return WebUtils.DB.Query(sql);
    }

    public static bool Insert_ENG_LINEOPPARAMCONF(Hashtable htData)
    {
        string sql = "select confid from ENG_LINEOPPARAMCONF order by confid desc";
        DataTable dt = WebUtils.DB.Query(sql);
        int confid = 0;
        if (dt.Rows.Count > 0)
        {
            confid = Convert.ToInt32(dt.Rows[0][0]);
        }
        confid++;
        sql = "INSERT INTO ENG_LINEOPPARAMCONF (CONFNAME, CONFID, LINENAME, L_OPNO, L_STNO," +
            " PARAM_ID, PARAM_VAL, PARAM_TEXT, DATA_TYPE, PARAM_TYPE, COMMENTS) values (@1," +
            " @2, @3, @4, @5, @6, @7, @8, @9, @10, @11)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["CONFNAME"], confid, htData["LINENAME"],
                htData["L_OPNO"], htData["L_STNO"],
                htData["PARAM_ID"], htData["PARAM_VAL"], htData["PARAM_TEXT"], htData["DATA_TYPE"],
                htData["PARAM_TYPE"], htData["COMMENTS"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_LINEOPPARAMCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_LINEOPPARAMCONF(Hashtable htData)
    {
        string sql = "UPDATE  ENG_LINEOPPARAMCONF SET CONFNAME=@1, LINEName=@2, L_OPNO=@3, L_STNO=@4," +
            " PARAM_ID=@5, PARAM_VAL=@6, PARAM_TEXT=@7, DATA_TYPE=@8, PARAM_TYPE=@9," +
            " COMMENTS=@10 WHERE CONFID=@11 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["CONFNAME"], htData["LINENAME"], htData["L_OPNO"], htData["L_STNO"],
                htData["PARAM_ID"], htData["PARAM_VAL"], htData["PARAM_TEXT"], htData["DATA_TYPE"],
                htData["PARAM_TYPE"], htData["COMMENTS"], htData["CONFID"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_LINEOPPARAMCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_LINEOPPARAMCONF(double confid)
    {
        string sql = "DELETE FROM ENG_LINEOPPARAMCONF WHERE CONFID = @1 ";
        try
        {
            return WebUtils.DB.Update(sql, confid) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_LINEOPPARAMCONF Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_LINEOPPARAMCONF()
    {
        string sql = "SELECT * FROM ENG_LINEOPPARAMCONF";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_LINEOPPARAMCONF Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_QC_WKFLOWCFG(Hashtable htData)
    {
        string sql = "INSERT INTO QC_WKFLOWCFG (EVT_TYPE, STEPNO, ASSIGNROLEID, ASSIGNOPER, NOTIFYLIST," +
            " LNKCFG, OPTIONALCHK, COMMENTS, PRESTEPNO, LASTCONFIRM) values (@1," +
            " @2, @3, @4, @5, @6, @7, @8, @9, @10)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["EVT_TYPE"], htData["STEPNO"], htData["ASSIGNROLEID"],
                htData["ASSIGNOPER"], htData["NOTIFYLIST"],
                htData["LNKCFG"], Convert.ToChar(htData["OPTIONALCHK"]), htData["COMMENTS"], htData["PRESTEPNO"],
                Convert.ToChar(htData["LASTCONFIRM"]));
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_QC_WKFLOWCFG Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_QC_WKFLOWCFG(Hashtable htData)
    {
        string sql = "UPDATE  QC_WKFLOWCFG SET ASSIGNROLEID=@1, ASSIGNOPER=@2, NOTIFYLIST=@3," +
            " LNKCFG=@4, OPTIONALCHK=@5, COMMENTS=@6, PRESTEPNO=@7, LASTCONFIRM=@8 WHERE EVT_TYPE=@9 and STEPNO=@10 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["ASSIGNROLEID"], htData["ASSIGNOPER"], htData["NOTIFYLIST"],
                htData["LNKCFG"], Convert.ToChar(htData["OPTIONALCHK"]), htData["COMMENTS"], htData["PRESTEPNO"],
                Convert.ToChar(htData["LASTCONFIRM"]), htData["EVT_TYPE"], htData["STEPNO"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_QC_WKFLOWCFG Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_QC_WKFLOWCFG()
    {
        string sql = "SELECT * FROM QC_WKFLOWCFG";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_WKFLOWCFG Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_QC_WKFLOWCFG(string evt_type, string stepno)
    {
        string sql = "SELECT * FROM QC_WKFLOWCFG WHERE EVT_TYPE = @1 and STEPNO = @2 ";
        try
        {
            return WebUtils.DB.Query(sql, evt_type, stepno);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_WKFLOWCFG Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Delete_QC_WKFLOWCFG(string evt_type, string stepno)
    {
        string sql = "DELETE FROM QC_WKFLOWCFG WHERE EVT_TYPE = @1 and STEPNO = @2 ";
        try
        {
            return WebUtils.DB.Update(sql, evt_type, stepno) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_QC_WKFLOWCFG Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Insert_QC_PRDCHKPLAN(Hashtable htData)
    {
        string sql = "select confid from ENG_LINEOPPARAMCONF order by confid desc";
        DataTable dt = WebUtils.DB.Query(sql);
        int QCPLANID = 0;
        if (dt.Rows.Count > 0)
        {
            QCPLANID = Convert.ToInt32(dt.Rows[0][0]);
        }
        QCPLANID++;
        sql = "INSERT INTO QC_PRDCHKPLAN (QCPLANID, QCPLANNAME, LINENAME, L_OPNO, L_STNO," +
            " PARTNO, PARTTYPE, INTERVALTYPE, INTERVAL, JUMPTROUBLE, CONFID, COMMENTS," +
            " UPPERLIMIT, LOWERLIMIT, CENTRALVAL, CONTROLWAY, LINKDOC) values (@1," +
            " @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16," +
            " @17)";
        try
        {
            int res = WebUtils.DB.Update(sql, QCPLANID, htData["QCPLANNAME"], htData["LINENAME"],
                htData["L_OPNO"], htData["L_STNO"],
                htData["PARTNO"], htData["PARTTYPE"], htData["INTERVALTYPE"], htData["INTERVAL"],
                Convert.ToChar(htData["JUMPTROUBLE"]), htData["CONFID"], htData["COMMENTS"], htData["UPPERLIMIT"],
                htData["LOWERLIMIT"], htData["CENTRALVAL"], htData["CONTROLWAY"], htData["LINKDOC"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_QC_PRDCHKPLAN Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_QC_PRDCHKPLAN(Hashtable htData)
    {
        string sql = "UPDATE  QC_PRDCHKPLAN SET QCPLANNAME=@1, LINENAME=@2, L_OPNO=@3, L_STNO=@4," +
            " PARTNO=@5, PARTTYPE=@6, INTERVALTYPE=@7, INTERVAL=@8, JUMPTROUBLE=@9," +
            " CONFID=@10, COMMENTS=@11, UPPERLIMIT=@12, LOWERLIMIT=@13, CENTRALVAL=@14," +
            " CONTROLWAY=@15, LINKDOC=@16 WHERE QCPLANID=@17 ";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["QCPLANNAME"], htData["LINENAME"], htData["L_OPNO"], htData["L_STNO"],
                htData["PARTNO"], htData["PARTTYPE"], htData["INTERVALTYPE"], htData["INTERVAL"],
                Convert.ToChar(htData["JUMPTROUBLE"]), htData["CONFID"], htData["COMMENTS"], htData["UPPERLIMIT"],
                htData["LOWERLIMIT"], htData["CENTRALVAL"], htData["CONTROLWAY"], htData["LINKDOC"],
                htData["QCPLANID"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_QC_PRDCHKPLAN Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_QC_PRDCHKPLAN(double qcplanid)
    {
        string sql = "SELECT * FROM QC_PRDCHKPLAN WHERE QCPLANID = @1 ";
        try
        {
            return WebUtils.DB.Query(sql, qcplanid);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_PRDCHKPLAN Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_QC_PRDCHKPLAN()
    {
        string sql = "SELECT * FROM QC_PRDCHKPLAN";
        try
        {
            return WebUtils.DB.Query(sql);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_QC_PRDCHKPLAN Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Delete_QC_PRDCHKPLAN(double qcplanid)
    {
        string sql = "DELETE FROM QC_PRDCHKPLAN WHERE QCPLANID = @1 ";
        try
        {
            return WebUtils.DB.Update(sql, qcplanid) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_QC_PRDCHKPLAN Failed:" + ex.Message);
            return false;
        }
    }

    public static DataTable Select_ENG_RWKSCRCODE(string qccode, string linename)
    {
        string sql = "SELECT * FROM ENG_RWKSCRCODE WHERE QCCODE = @1 and LINENAME = @4 ";
        try
        {
            return WebUtils.DB.Query(sql, qccode, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_RWKSCRCODE Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static DataTable Select_ENG_RWKSCRCODE(string linename)
    {
        string sql = "SELECT * FROM ENG_RWKSCRCODE WHERE LINENAME = @1 ";
        try
        {
            return WebUtils.DB.Query(sql, linename);
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Select_ENG_RWKSCRCODE Failed:" + ex.Message);
            return new DataTable();
        }
    }

    public static bool Insert_ENG_RWKSCRCODE(Hashtable htData)
    {
        string sql = "INSERT INTO ENG_RWKSCRCODE (QCCODE, QCTYPE, DESCRIPTION, LINENAME, DEFAULTOOP," +
            " FROMOP, COMMENTS) values (@1, @2, @3, @4, @5, @6, @7)";
        try
        {
            int res = WebUtils.DB.Update(sql, htData["QCCODE"], Convert.ToChar(htData["QCTYPE"]), htData["DESCRIPTION"],
                htData["LINENAME"], htData["DEFAULTOOP"],
                htData["FROMOP"], htData["COMMENTS"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Insert_ENG_RWKSCRCODE Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Update_ENG_RWKSCRCODE(Hashtable htData)
    {
        string sql = "UPDATE  ENG_RWKSCRCODE SET QCTYPE=@1, DESCRIPTION=@2, DEFAULTOOP=@3, FROMOP=@4," +
            " COMMENTS=@5 WHERE QCCODE=@6 and LINENAME=@7 ";
        try
        {
            int res = WebUtils.DB.Update(sql, Convert.ToChar(htData["QCTYPE"]), htData["DESCRIPTION"], htData["DEFAULTOOP"],
                htData["FROMOP"], htData["COMMENTS"], htData["QCCODE"], htData["LINENAME"]);
            return res > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Update_ENG_RWKSCRCODE Failed:" + ex.Message);
            return false;
        }
    }

    public static bool Delete_ENG_RWKSCRCODE(string linename, string qccode)
    {
        string sql = "DELETE FROM ENG_RWKSCRCODE WHERE QCCODE = @1 and LINENAME = @2 ";
        try
        {
            return WebUtils.DB.Update(sql, qccode, linename) > 0;
        }
        catch (Exception ex)
        {
            WebUtils.Logger.Debug("Delete_ENG_RWKSCRCODE Failed:" + ex.Message);
            return false;
        }
    }
}