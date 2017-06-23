using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Traceability : System.Web.UI.Page
{
    protected override void OnUnload(EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            WebUtils.disposeSessionDt(Session, this.Page.ClientID + "CompData");
            WebUtils.disposeSessionDt(Session, this.Page.ClientID + "ProcData");
            WebUtils.disposeSessionDt(Session, this.Page.ClientID + "TicketData");
            WebUtils.disposeSessionDt(Session, this.Page.ClientID + "ContainerData");
        }
        base.OnUnload(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtStartDateTime.Value = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            //txtEndDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        WebUtils.SetLangDisplay(this);
        
    }
    protected void btQuery_Click(object sender, EventArgs e)
    {
        string prdsn = txtPrdSN.Value.Trim().ToUpper();
        string cntrno = txtPackNo.Value.Trim();
        string wo_id = txtWO_ID.Value.Trim().ToUpper();
        bool IsBackwards = CKB_BackTrace.Checked;

        DateTime startDatetime = DateTime.MinValue;
        DateTime endDatetime = DateTime.MaxValue;
        try
        {
            if (txtStartDateTime.Value.Trim() != "")
            {
                startDatetime = DateTime.Parse(txtStartDateTime.Value);
            }
            if (txtEndDateTime.Value.Trim() != "")
            {
                endDatetime = DateTime.Parse(txtEndDateTime.Value);
            }
        }
        catch
        {
        }
        if (prdsn == "" && wo_id == "" && cntrno=="" && startDatetime == DateTime.MinValue && endDatetime == DateTime.MaxValue)
        {
            lblHistory.Text = "请输入要查询的序列号,箱号,批次号或工单号！";
            lblHistory.ForeColor = System.Drawing.Color.Red;
            return;
        }
        gvComp.DataSource = new System.Data.DataTable();
        gvComp.DataBind();
        lblComp.Text = "";
        gvProcData.DataSource = new System.Data.DataTable();
        gvProcData.DataBind();
        lblProcData.Text = "";
        gvContainer.DataSource = new System.Data.DataTable();
        gvContainer.DataBind();
        lblContainer.Text = "";
        gvTicket.DataSource = new System.Data.DataTable();
        gvTicket.DataBind();
        lblTicket.Text = "";
        DataTable dtprdhis = WebDAL.GetHistoryData(prdsn, wo_id, startDatetime, endDatetime,IsBackwards,cntrno);
        gvHistory.DataSource = dtprdhis;
        gvHistory.DataBind();
        lblHistory.Font.Bold = false;
        lblHistory.ForeColor = System.Drawing.Color.Black;
        lblHistory.Text = "";
        if (gvHistory.DataSource == null)
        {
            lblHistory.Text = "查询出错";
            lblHistory.ForeColor = System.Drawing.Color.Red;
        }
        else if (gvHistory.Rows.Count == 0)
        {
            lblHistory.Text = "没有相关的生产数据";
            lblHistory.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            lblHistory.Text = "共有" + gvHistory.Rows.Count + "条生产数据：";
            lblHistory.Font.Bold = true;
            //gvHistory.Columns[0].HeaderText = "序列号/批次号";
            List<string> listPrdsn = new List<string>();
            foreach (DataRow dr in dtprdhis.Rows )
            {
                string sno = dr["PRDSN"].ToString();
                if (!listPrdsn.Contains(sno))
                {
                    listPrdsn.Add(sno);
                }
                sno = (dr["LOTNO"]==DBNull.Value?"": dr["LOTNO"].ToString());
                if (string.IsNullOrEmpty(sno))
                    continue;
                if (!listPrdsn.Contains(sno))
                {
                    listPrdsn.Add(sno);
                }

            }
            DataTable dt = WebDAL.GetCompData(listPrdsn);
            gvComp.DataSource = dt;
            gvComp.DataBind();
            Session[this.Page.ClientID + "CompData"] = dt;
            if (gvComp.Rows.Count > 0)
            {
                lblComp.Text = "共有" + gvComp.Rows.Count + "条物料数据：";
            }
            else
            {
                lblComp.Text = "没有相关的物料数据";
            }
            DataTable dt2 = WebDAL.GetProcData(listPrdsn);
            
            gvProcData.DataSource = dt2;
            gvProcData.DataBind();
            Session[this.Page.ClientID + "ProcData"] = dt2;
            if (gvProcData.Rows.Count > 0)
            {
                lblProcData.Text = "共有" + gvProcData.Rows.Count + "条过程数据：";
            }
            else
            {
                lblProcData.Text = "没有相关的过程数据";
            }

            DataTable dt3 = WebDAL.GetContainerData(listPrdsn);
            gvContainer.DataSource = dt3;
            gvContainer.DataBind();
            Session[this.Page.ClientID + "ContainerData"] = dt3;
            if (gvContainer.Rows.Count > 0)
            {
                lblContainer.Text = "共有" + gvContainer.Rows.Count + "条包装数据：";
            }
            else
            {
                lblContainer.Text = "没有相关的包装数据";
            }
            DataTable dt4 = WebDAL.GetTicketData(listPrdsn);
            gvTicket.DataSource = dt4;
            gvTicket.DataBind();
            Session[this.Page.ClientID + "TicketData"] = dt4;
            if (gvTicket.Rows.Count > 0)
            {
                lblTicket.Text = "共有" + gvTicket.Rows.Count + "条单据数据：";
            }
            else
            {
                lblTicket.Text = "没有相关的单据数据";
            }
        }
    }

    protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Attributes["onclick"]==null)
            e.Row.Attributes.Add("onclick", "SetSelectedRow(" + e.Row.RowIndex + ")");
            System.Data.DataRow dr = ((System.Data.DataRowView)e.Row.DataItem).Row;
            if (dr["result"].ToString() == "1" || dr["result"].ToString().ToUpper() == "Y" || 
                dr["result"].ToString().ToUpper() == "P")
            {
                e.Row.Cells[7].Text = "成功";
            }
            else if (dr["result"].ToString()=="0")
            {
                e.Row.Cells[7].Text = "-";
            }
            else
            {
                e.Row.Cells[7].Text = "失败";
            }
        }
    }
    protected void gvComp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Data.DataRow dr = ((System.Data.DataRowView)e.Row.DataItem).Row;
            if (dr["active"].ToString() == "1" || dr["active"].ToString().ToUpper() == "Y")
            {
                e.Row.Cells[7].Text = "已使用";
            }
            else 
            {
                e.Row.Cells[7].Text = "被释放";
            }
        }
    }
    protected void gvProcData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Data.DataRow dr = ((System.Data.DataRowView)e.Row.DataItem).Row;
            if (dr["result"].ToString() == "1" || dr["result"].ToString().ToUpper() == "Y" ||
                dr["result"].ToString().ToUpper() == "P")
            {
                e.Row.Cells[8].Text = "成功";
            }
            else if (dr["result"].ToString() == "0")
            {
                e.Row.Cells[8].Text = "-";
            }
            else
            {
                e.Row.Cells[8].Text = "失败";
            }
        }
    }
    protected void gvTicket_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Data.DataRow dr = ((System.Data.DataRowView)e.Row.DataItem).Row;
            string status = dr["sub_STATUS"].ToString().ToUpper();
            if (status == "N")
            {
                e.Row.Cells[6].Text = "新生成";
            }
            else if (status == "I")
            {
                e.Row.Cells[6].Text = "处理中";
            }
            else if (status == "H")
            {
                e.Row.Cells[6].Text = "暂停";
            }
            else if (status == "S")
            {
                e.Row.Cells[6].Text = "终止";
            }
        }
    }

    private void filterCompData(string line,string op, string prdsn)
    {

        DataTable dt = (DataTable)Session[this.Page.ClientID + "CompData"];
        DataTable dt_new = dt.Copy();

        for (int i = dt_new.Rows.Count - 1; i >= 0; i--)
        {
            if (!(dt_new.Rows[i]["LINENAME"].ToString() == line && dt_new.Rows[i]["L_OPNO"].ToString() == op && dt_new.Rows[i]["PRDSN"].ToString() == prdsn))
            {
                dt_new.Rows.RemoveAt(i);
            }
        }
        gvComp.DataSource = dt_new;
        gvComp.DataBind();
        if (gvComp.Rows.Count > 0)
        {
            lblComp.Text = "共有" + gvComp.Rows.Count + "条物料数据：";
        }
        else
        {
            lblComp.Text = "没有相关的物料数据";
        }
    }

    private void filterProcData(string line, string op, string prdsn)
    {

        DataTable dt = (DataTable)Session[this.Page.ClientID + "ProcData"];
        DataTable dt_new = dt.Copy();
        for (int i = dt_new.Rows.Count - 1; i >= 0; i--)
        {
            if (!(dt_new.Rows[i]["LINENAME"].ToString() == line && dt_new.Rows[i]["L_OPNO"].ToString() == op && dt_new.Rows[i]["PRDSN"].ToString() == prdsn))
            {
                dt_new.Rows.RemoveAt(i);
            }
        }
        gvProcData.DataSource = dt_new;
        gvProcData.DataBind();
        if (gvProcData.Rows.Count > 0)
        {
            lblProcData.Text = "共有" + gvProcData.Rows.Count + "条过程数据：";
        }
        else
        {
            lblProcData.Text = "没有相关的过程数据";
        }

    }

    private void filterContainer(string prdsn)
    {

        DataTable dt = (DataTable)Session[this.Page.ClientID + "ContainerData"];
        DataTable dt_new = dt.Copy();
        for (int i = dt_new.Rows.Count - 1; i >= 0; i--)
        {
            if (!(dt_new.Rows[i]["SUBCONTAINERNO"].ToString() == prdsn))
            {
                dt_new.Rows.RemoveAt(i);
            }
        }
        this.gvContainer.DataSource = dt_new;
        gvContainer.DataBind();
        if (gvContainer.Rows.Count > 0)
        {
            lblContainer.Text = "共有" + gvContainer.Rows.Count + "条过程数据：";
        }
        else
        {
            lblContainer.Text = "没有相关的包装数据";
        }

    }


    private void filterTicket(string prdsn)
    {

        DataTable dt = (DataTable)Session[this.Page.ClientID + "TicketData"];
        DataTable dt_new = dt.Copy();
        for (int i = dt_new.Rows.Count - 1; i >= 0; i--)
        {
            if (!(dt_new.Rows[i]["PRDSN"].ToString() == prdsn))
            {
                dt_new.Rows.RemoveAt(i);
            }
        }
        this.gvTicket.DataSource = dt_new;
        gvTicket.DataBind();
        if (gvTicket.Rows.Count > 0)
        {
            this.lblTicket.Text = "共有" + gvTicket.Rows.Count + "条过程数据：";
        }
        else
        {
            lblTicket.Text = "没有相关的单据数据";
        }

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {

        if (txtRowIndex.Value != "" && Session[this.Page.ClientID + "CompData"] != null && Session[this.Page.ClientID + "ProcData"] != null)
        { 
            int rowIndex = int.Parse(txtRowIndex.Value);
            string line = gvHistory.Rows[rowIndex].Cells[4].Text;
            string op = gvHistory.Rows[rowIndex].Cells[5].Text;
            string prdsn = gvHistory.Rows[rowIndex].Cells[0].Text;

            filterCompData(line, op, prdsn);
            filterProcData(line, op, prdsn);
            filterContainer(prdsn);
            filterTicket(prdsn);



            for (int i = 0; i < gvHistory.Rows.Count; i ++ )
            {
                if (i % 2 == 0)
                {
                    gvHistory.Rows[i].BackColor = System.Drawing.Color.White;
                }
                else
                {
                    gvHistory.Rows[i].BackColor = (System.Drawing.Color)new WebColorConverter().ConvertFromString("#F7F6F3");
                }
            }
            gvHistory.Rows[rowIndex].BackColor = System.Drawing.Color.Yellow;
        }
    }

}