using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Intelli.MidW.BizClient;
using Intelli.MidW.Utils.DB;
using Intelli.MidW.Utils.Log;
using System.Diagnostics;
using System.Threading;
using Intelli.MidW.Interface;
using Intelli.MES.ClientData;
using System.Data;
using System.Collections;
using System.Web.SessionState;
 
public interface Iwebqryfield
{
    /// <summary>
    /// linked parametername
    /// </summary>
    string LinkedParName { get; set; }

    /// <summary>
    /// linked db field name
    /// </summary>
    string LinkedDbFieldName { get; set; }
    string LinkedFieldType { get; set; }
    string LinkedChangeField { get; set; }
    string LinkedOperator { get; set; }
    string LinkedPrefix { get; set; }
    string LinkedQryID { get; set; }
    string LinkedDBname { get; set; }
    string DefaultFieldValue { get; set; }
    string SubQuerySQL { get; set; }

    /// <summary>
    /// allow null
    /// </summary>
    bool AllowNull { get; set; }

    Dictionary<string, string> LinkAttributes { get; set; }
    string GetAttributeValue(string attrname);
    bool FollowChange(string followedfieldname);
    object GetFieldValue();
    void SetFieldValue(object val);
    string GetSQL();
}

public class Qry_Calendar : System.Web.UI.WebControls.Calendar, Iwebqryfield
{
    public string LinkedParName { get; set; }
    public string LinkedDbFieldName { get; set; }
    public string LinkedFieldType { get; set; }
    public string LinkedChangeField { get; set; }
    public string LinkedOperator { get; set; }
    public string LinkedPrefix { get; set; }
    public string LinkedQryID { get; set; }
    public string DefaultFieldValue { get; set; }
    public string SubQuerySQL { get; set; }

    bool _allownull = true;
    public bool AllowNull { get { return (_allownull); } set { _allownull = value; } }

    private string _dbname="default";
    public string LinkedDBname 
    { get {return(_dbname);}
        set
        {
            _dbname = value;
        }
    }

    public bool FollowChange(string followedfieldname)
    {
        return (true);
    }
    public Dictionary<string, string> LinkAttributes
    {
        get
        {
            return (_linkAttributes);
        }
        set
        {
            _linkAttributes.Clear();
            _linkAttributes = value;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.SelectedDate = System.DateTime.ParseExact("1970/01/01", "yyyy/MM/dd", null);
    }
    Dictionary<string, string> _linkAttributes = new Dictionary<string, string>();
    public string GetAttributeValue(string attrname)
    {
        if (LinkAttributes.ContainsKey(attrname))
            return (LinkAttributes[attrname]);
        return ("");
    }
    public object GetFieldValue()
    {
        //return WebUtils.GetFieldValue(this.SelectedDate.ToString("yyyy/MM/dd"), "DATE", this.LinkedDbFieldName, this.LinkedDBname);
        if (this.Enabled = false && this.DefaultFieldValue != null)
        {
            DateTime l;
            try
            {
                if (DateTime.TryParseExact(this.DefaultFieldValue, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out l))
                    return (l);
                if (DateTime.TryParseExact(this.DefaultFieldValue, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out l))
                    return (l);
            }
            catch { }
        }
        return (this.SelectedDate);
    }
    public void SetFieldValue(object val)
    {
        //this.SelectedDate = (System.DateTime)(WebUtils.ConverType(val, System.DateTime));
        if (val == null)
            return;
        if (val.GetType().Name.ToUpper().IndexOf("DATE") >= 0)
            return;

        if (val.ToString().IndexOf(":") >= 0)
        {
            this.SelectedDate = DateTime.ParseExact(val.ToString(), "yyyy/MM/dd HH:mm:ss", null);
        }
        else
        {
            this.SelectedDate = DateTime.ParseExact(val.ToString(), "yyyy/MM/dd", null);
        }
    }
    public string GetSQL()
    {
        string str = this.SelectedDate.ToString("yyyy/MM/dd");
        if (this.Parent.Controls.Count > 1)
        {
            foreach (Control ctrlitem in this.Parent.Controls)
            {
                if (ctrlitem.GetType().Name.ToUpper().IndexOf("TEXT") >= 0)
                {
                    if (ctrlitem.GetType().Name.IndexOf("Qry_") < 0)
                        continue;
                    if (((Iwebqryfield)ctrlitem).LinkedFieldType != null && ((Iwebqryfield)ctrlitem).LinkedFieldType.ToUpper().IndexOf("TIME") < 0)
                        continue;
                    string timeval = ((TextBox)ctrlitem).Text;
                    if(timeval == null)
                        continue;
                    if (timeval.IndexOf(":") < 0)
                        continue;
                    str += " " + timeval;
                }
            }
        }
        return (WebUtils.GetSQLbyFieldinput(str, LinkedPrefix, LinkedFieldType, LinkedDbFieldName, LinkedOperator, LinkedDBname, SubQuerySQL));
    }
}

public class Qry_DropDownList : System.Web.UI.WebControls.DropDownList, Iwebqryfield
{
    public string LinkedParName { get; set; }
    public string LinkedDbFieldName { get; set; }
    public string LinkedFieldType { get; set; }
    public string LinkedChangeField { get; set; }
    public string LinkedOperator { get; set; }
    public string LinkedPrefix { get; set; }
    public string LinkedQryID { get; set; }
    public string LinkedChangeSQL { get; set; }
    public string DefaultFieldValue { get; set; }
    public string SubQuerySQL { get; set; }

    bool _allownull = true;
    public bool AllowNull { get { return (_allownull); } set { _allownull = value; } }

    private string _dbname = "default";

    public string LinkedDBname
    {
        get { return (_dbname); }
        set
        {
            _dbname = value;
        }
    }
    public bool FetchSQLData()
    {
        try
        {
            //WebUtils.Logger.Debug("Fetch:" + this.ID + ".enabled=" + this.Enabled.ToString());
            if (LinkedChangeSQL != null && LinkedChangeSQL != "")
            {
                //if (LinkedChangeField == "" || LinkedChangeField == null)
                {
                    string orgval = this.SelectedValue;       

                    this.Items.Clear();
                    
                    System.Data.DataTable dt = WebUtils.DB.QueryEx(this.LinkedDBname, LinkedChangeSQL);
                    dt.Constraints.Clear();
                    dt.Rows.InsertAt(dt.NewRow(),0);

                    this.DataSource = dt;
                    this.DataBind();
                    if (this.Items.FindByText(orgval) != null)
                        this.Items.FindByText(orgval).Selected = true;
                    
                } 

            }
            //this.Enabled = true;
            return (true);
        }
        catch
        {
            //this.ClearSelection();
        }
        //this.Enabled = true;
        if(this.DataSource!=null &&((System.Data.DataTable)(this.DataSource)).Rows.Count>0)
        {
            this.SelectedIndex = -1;
        }
        return (false);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.AutoPostBack = true;
        this.Enabled = true;
        this.SelectedIndexChanged -= new EventHandler(  OnfollowSelectedIndexChanged);
        this.SelectedIndexChanged += new EventHandler(  OnfollowSelectedIndexChanged);
    }

    public void OnfollowSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.LinkedChangeField))
        {
            if (this.SelectedIndex < 0 || string.IsNullOrEmpty(this.SelectedValue))
                return;
            foreach (string lnkname in this.LinkedChangeField.Split(new char[]{',',';'}))
                this.FollowChange(lnkname);
            //if(!this.Page.IsPostBack)
            //this.RaisePostDataChangedEvent();
        }
    }

    public bool FollowChange(string linkedobjname)
    {
        if (!string.IsNullOrEmpty(linkedobjname))
            {

                Control ctl = this.Parent.FindControl(linkedobjname);
                if (ctl.GetType().Name.IndexOf("Qry_") >= 0 && ctl.GetType().Name.ToUpper().IndexOf("LIST") >= 0 && ctl.GetType().Name.ToUpper().IndexOf("DROP") >= 0)
                {
                    Qry_DropDownList targetctl = (Qry_DropDownList)(ctl);

                    object s = ((Iwebqryfield)ctl).GetFieldValue();
                    object curfollowval = this.GetFieldValue();
                    System.Data.DataTable dt = WebUtils.DB.Query(targetctl.LinkedChangeSQL, curfollowval);
                    dt.Constraints.Clear();
                    dt.Rows.InsertAt(dt.NewRow(), 0);
                    int L = this.SelectedIndex; 
                    targetctl.Items.Clear();
                    //targetctl.ClearSelection();
                    
                    targetctl.DataSource = dt;
                    targetctl.DataBind(); 
                    try
                    {
                        this.SelectedIndex = L;
                    }
                    catch { }
                    targetctl.Enabled = true;
                    this.Enabled = true;
                }
            } 
        
        WebUtils.Logger.Debug("followchange:" + this.ID + ".enabled=" + this.Enabled.ToString());
        return (true);
    }
    public Dictionary<string, string> LinkAttributes
    {
        get
        {
            return (_linkAttributes);
        }
        set
        {
            _linkAttributes.Clear();
            _linkAttributes = value;
        }
    }
    Dictionary<string, string> _linkAttributes = new Dictionary<string, string>();
    public string GetAttributeValue(string attrname)
    {
        if (LinkAttributes.ContainsKey(attrname))
            return (LinkAttributes[attrname]);
        return ("");
    }
    public object GetFieldValue()
    {
        if (this.Enabled = false && this.DefaultFieldValue != null)
        {
            return (this.DefaultFieldValue);
        }

        return (this.SelectedValue);
    }
    public void SetFieldValue(object val)
    {
        if (val == null)
            this.SelectedIndex = -1;
        if (this.DataSource != null)
        {
            //this.Items.FindByValue(val.ToString()).Selected = true;
            
            for (int L=0;L<this.Items.Count;L++)
            {
                if (this.Items[L].Value == val.ToString())
                {
                    this.SelectedIndex = L;
                    return;
                }
            } 
             
        }
        else
        {
            this.Text = val.ToString();
        }
    }
    public string GetSQL()
    {
        string str = this.SelectedValue;
       
        if (this.SelectedValue == null || this.SelectedValue == "")
            str = this.Text;
        return (WebUtils.GetSQLbyFieldinput(str, LinkedPrefix, LinkedFieldType, LinkedDbFieldName, LinkedOperator, LinkedDBname, SubQuerySQL));

    }
}

public class Qry_Label : System.Web.UI.WebControls.Label, Iwebqryfield
{
    public string LinkedParName { get; set; }
    public string LinkedDbFieldName { get; set; }
    public string LinkedFieldType { get; set; }
    public string LinkedChangeField { get; set; }
    public string LinkedOperator { get; set; }
    public string LinkedPrefix { get; set; }
    public string LinkedQryID { get; set; }
    public string DefaultFieldValue { get; set; }
    public string SubQuerySQL { get; set; }

    private string _dbname = "default";


    bool _allownull = true;
    public bool AllowNull { get { return (_allownull); } set { _allownull = value; } }
    public string LinkedDBname
    {
        get { return (_dbname); }
        set
        {
            _dbname = value;
        }
    }

    public bool FollowChange(string followedfieldname)
    {
        return (true);
    }
    public Dictionary<string, string> LinkAttributes
    {
        get
        {
            return (_linkAttributes);
        }
        set
        {
            _linkAttributes.Clear();
            _linkAttributes = value;
        }
    }
    Dictionary<string, string> _linkAttributes = new Dictionary<string, string>();
    public string GetAttributeValue(string attrname)
    {
        if (LinkAttributes.ContainsKey(attrname))
            return (LinkAttributes[attrname]);
        return ("");
    }
    public object GetFieldValue()
    {
        if (this.Enabled = false && this.DefaultFieldValue != null)
        {
            return (this.DefaultFieldValue);
        }

        return (this.Text);
    }
    public void SetFieldValue(object val)
    {
        if (val == null)
            return;
        this.ToolTip = val.ToString();
    }
    public string GetSQL()
    {
        return (WebUtils.GetSQLbyFieldinput(this.Text, LinkedPrefix, LinkedFieldType, LinkedDbFieldName, LinkedOperator, LinkedDBname, SubQuerySQL));

    }
}

public class Qry_TextBox : System.Web.UI.WebControls.TextBox, Iwebqryfield
{

    public string LinkedParName { get; set; }
    public string LinkedDbFieldName { get; set; }
    public string LinkedFieldType { get; set; }
    public string LinkedChangeField { get; set; }
    public string LinkedOperator { get; set; }
    public string LinkedPrefix { get; set; }
    public string LinkedQryID { get; set; }
    public string DefaultFieldValue { get; set; }
    public string SubQuerySQL { get; set; }

    bool _allownull = true;
    public bool AllowNull { get { return (_allownull); } set { _allownull = value; } }

    private string _dbname = "default";
    public string LinkedDBname
    {
        get { return (_dbname); }
        set
        {
            _dbname = value;
        }
    }
    public bool FollowChange(string followedfieldname)
    {
        return (true);
    }
    public Dictionary<string, string> LinkAttributes
    {
        get
        {
            return (_linkAttributes);
        }
        set
        {
            _linkAttributes.Clear();
            _linkAttributes = value;
        }
    }
    Dictionary<string, string> _linkAttributes = new Dictionary<string, string>();
    public string GetAttributeValue(string attrname)
    {
        if (LinkAttributes.ContainsKey(attrname))
            return (LinkAttributes[attrname]);
        return ("");
    }
    public object GetFieldValue()
    {
        if (this.Enabled = false && this.DefaultFieldValue != null)
        {
            return (this.DefaultFieldValue);
        }

        return (this.Text);
        
    }
    public void SetFieldValue(object val)
    {
        if (val == null)
        {
            this.Text = "";
        }
        else
        {
            this.Text = val.ToString();
        }
    }

    public string GetSQL()
    {
        return (WebUtils.GetSQLbyFieldinput(this.Text, LinkedPrefix, LinkedFieldType, LinkedDbFieldName, LinkedOperator, LinkedDBname, SubQuerySQL));

    }

}


public class Qry_CheckBox : System.Web.UI.WebControls.CheckBox, Iwebqryfield
{
    public string LinkedParName { get; set; }
    public string LinkedDbFieldName { get; set; }
    public string LinkedFieldType { get; set; }
    public string LinkedChangeField { get; set; }
    public string LinkedOperator { get; set; }
    public string LinkedPrefix { get; set; }
    public string LinkedQryID { get; set; }
    public string LinkedOrderByFieldName { get; set; }
    public string DefaultFieldValue { get; set; }
    public string SubQuerySQL { get; set; }

    bool _allownull = true;
    public bool AllowNull { get { return (_allownull); } set { _allownull = value; } }
    public string GetOrderBy()
    {
        if (LinkedOrderByFieldName != "" && LinkedOrderByFieldName != null && this.Checked)
        {
            return (LinkedOrderByFieldName);
        }
        return ("");
    }

    private string _dbname = "default";
    public string LinkedDBname
    {
        get { return (_dbname); }
        set
        {
            _dbname = value;
        }
    }
    public bool FollowChange(string followedfieldname)
    {
        return (true);
    }
    public Dictionary<string, string> LinkAttributes
    {
        get
        {
            return (_linkAttributes);
        }
        set
        {
            _linkAttributes.Clear();
            _linkAttributes = value;
        }
    }
    Dictionary<string, string> _linkAttributes = new Dictionary<string, string>();
    public string GetAttributeValue(string attrname)
    {
        if (LinkAttributes.ContainsKey(attrname))
            return (LinkAttributes[attrname]);
        return ("");
    }
    public object GetFieldValue()
    {
        if (this.Enabled = false && this.DefaultFieldValue != null)
        {
            bool l;
            if(bool.TryParse(this.DefaultFieldValue, out l))
            return (l);
        }

        return (this.Checked);

    }

    public void SetFieldValue(object val)
    {
        if (val == null)
        {
            this.Checked = false;
        }
        else
        this.Checked = ( (!val.ToString().Equals("0")) && (!val.ToString().ToUpper().Equals("N"))); 
    }
    public string GetSQL()
    {
        return (WebUtils.GetSQLbyFieldinput(this.Checked.ToString(), LinkedPrefix, LinkedFieldType, LinkedDbFieldName, LinkedOperator, LinkedDBname, SubQuerySQL));
    }
}

public class permissionMatrix
{
    public string entityId { get; set; }
    public string UserName { get; set; }
    public string Permission { get; set; }
    public string moduleId { get; set; }
    public string DeptNo { get; set; }

}

/// <summary>
/// Class1 的摘要说明
/// </summary>
/// 
public class WebUtils
{

    public static string getUrlfilenameinRequest(HttpRequest req)
    {
        try
        {
            string url = req.MapPath(req.Url.AbsolutePath);
            string filename = System.IO.Path.GetFileName(url);
            return (filename);
        }
        catch { }
        return ("");
    }
    public static List<ListItem> showWos(List<WorkOrderData> wos)
    {
        List<ListItem> listitems = new List<ListItem>();
        listitems.Add(new ListItem());
        foreach (WorkOrderData wo in wos)
            {
                ListItem newitem = new ListItem();
                newitem.Value = wo.Id;
                string wodesc = wo.Description;
                if (wodesc.Length > 130)
                    wodesc = wodesc.Substring(0, 130);
                wodesc = wodesc.PadRight(135, ' ');

                newitem.Text = wo.Id + " : \t" + wo.Partno + "; \t " + wodesc + "; " + wo.Qty.ToString() + " pcs";
                newitem.Attributes.Add("warnqty", wo.WarningQty.ToString());
                newitem.Attributes.Add("blkqty", wo.BlockQty.ToString());
                listitems.Add(newitem);
            }
        return (listitems);
    }

    public static void LoadStnLog(StnData stn,string linename,string svrname)
    {
        BizRequest request = ClientMgr.Instance.CreateRequest(svrname, String.Format("{0};{1};{2}", linename, stn.Op, stn.Id),"", "StnLog", new Dictionary<string,string>());
        BizResponse response = ClientMgr.Instance.RunCmd(request.CmdName, request);
        if (BizResponses.OK.Equals(response.ErrorCode))
        {
            stn.UpdateLogFromServer(response.Data);
        }

    }

    public class LineInfo
    {
        public string lineid="";
        public DateTime updatetime = DateTime.Now;
        public ClientLineData linedata;
    }

    static Dictionary<string, LineInfo> _linelist;
    public static Dictionary<string, LineInfo> LineList
    {
        get
        {
            if (_linelist != null && _linelist.Count>0)
                return (_linelist);
            if (HttpContext.Current.Application["Global_LineList"]!=null)
            {
                _linelist = (Dictionary<string, LineInfo>)(HttpContext.Current.Application["Global_LineList"]);
                return (_linelist);
            }
            if (_linelist == null)
                _linelist = new Dictionary<string, LineInfo>();
            return (_linelist);
        }
        set
        {
            _linelist = value;
            HttpContext.Current.Application["Global_LineList"] = _linelist; 
        }
    }

    public static DateTime lastlinestatustime()
    {
        //if(HttpApplication.SessionStateSection)
        //return();
        return (DateTime.Now);
    }

    public static void GetAllLineData()
    {
        System.Data.DataTable dt = WebUtils.DB.Query("select * from eng_prdline where buno='" + WebUtils.PlantName+ "' OR buno is null");

        if (_linelist == null)
            _linelist = new Dictionary<string, LineInfo>();
        _linelist.Clear();

        foreach (System.Data.DataRow dr in dt.Rows)
        {
            string linename = dr["LINENAME"].ToString();
            string svrname = GetAppSettingValue("Global","MES","ServerName",true);
            if(string.IsNullOrEmpty(svrname))
                svrname = "MES";
            ClientLineData ln = loadLineData(linename, svrname,false);
            LineInfo linew = new LineInfo();
            linew.lineid = linename;
            linew.linedata = ln;
            linew.updatetime = DateTime.Now;
            lock(_linelist)
            _linelist.Add(linename, linew);
        }
        HttpContext.Current.Application["Global_LineList"] = _linelist; 
    }

    public static string MesServerName
    {
        get
        {
            string svrname = WebUtils.GetAppSettingValue("Global", "MES", "ServerName", false);
                if (string.IsNullOrEmpty(svrname))
                    svrname = "MES"; 
            return (svrname);
        }
    }
     public static List<ComponentData> getmaterialList(Dictionary<string,string> responsedata)
    {

        if (responsedata.ContainsKey("bom"))
        {
            List<ComponentData> comps = ComponentData.FromServer(responsedata);
            if (comps == null)
                return new List<ComponentData>();
            foreach (ComponentData d in comps.ToArray())
            {
                if (d.BomDetail == null)
                {
                    comps.Remove(d);
                    continue;
                } 
            }
            return (comps);
        }
        return new List<ComponentData>();
    }
    public static string getScanComs(string clientnm)
    {
        int i = 1; 
        //ACQIDCOMPS
        try
        {
            BizRequest r = ClientMgr.Instance.CreateRequest(WebUtils.MesServerName, clientnm, "", "ACQIDCOMPS", new Dictionary<string, string>());

            BizResponse response = ClientMgr.Instance.RunCmd(r.CmdName, r);
            if (!BizResponses.OK.Equals(response.ErrorCode))
            {
                return ("Err.GetScanCOMPListFailure: " + response.ErrorMessage);
            }
            else
            {
                return response.ReturnMessage; // 返回格式：IDtype=IDtype, COMPSeqNO=COMPPartNo.
                
            }

        }
        catch (Exception e)
        {
            WebUtils.Logger.Debug("getCompFailure：" + e.Message, false);
        }
        return ("Err.Failed");
    }

    public static List<string> displayattriblist
    {
        get
        {
            if (HttpContext.Current.Application["Global_Page_DisplayProperties"] != null)
            {
                return ((List<string>)(HttpContext.Current.Application["Global_Page_DisplayProperties"]));
            }
            string propnames = GetAppSettingValue("Global", "Page", "DisplayProperties",true);
            List<string> proplist = new List<string>();
            foreach (string propname in propnames.Split(new char[] { ';', ',' }))
            {
                proplist.Add(propname);
            }
            HttpContext.Current.Application["Global_Page_DisplayProperties"] = proplist;
            return (proplist);
        }
    }

    public static void SetLangDisplay(System.Web.UI.Page pg)
    {
        foreach (Control ctl in pg.Controls)
        {
            string[] clsnamearr = pg.GetType().Name.Split(new char[] { '.' });
            string clsname = clsnamearr[clsnamearr.Length - 1];
            SetLangDisplay(clsname.ToUpper(), ctl);
        }
        
    }
     
    public static void SetLangDisplay(string clientID, Control ctl)
    {
        string prefix = clientID;
        if (!string.IsNullOrEmpty(clientID))
        {
            prefix = clientID + "$";
        }
        else
        {
            prefix = "";
        }
        try
        {
            try
            {
                ctl.Visible = (GetPermission(ctl.Page.Session["UserName"].ToString(), "", "MES", ctl.Page.GetType().Name + "." + ctl.ID) != "---");
            }
            catch { }
            if (ctl is Repeater)
            {
                Repeater rp = (Repeater)ctl;
                
            }
            if(ctl is GridView)
            {
                GridView gv = (GridView)(ctl);
                for(int i=0;i<gv.Columns.Count;i++)
                {
                    string langmsgid = prefix + ctl.ID + "$" + gv.Columns[i].HeaderText;
                    string oldtxt = gv.Columns[i].HeaderText;
                    string newtxt = GetLangMessage(langmsgid, gv.Columns[i].HeaderText);
                    if (WebUtils.GetSysLang() != "en" && newtxt == oldtxt)
                    {
                        newtxt = GetLangMessage(ctl.ID + "$" + gv.Columns[i].HeaderText, gv.Columns[i].HeaderText);
                    }
                    gv.Columns[i].HeaderText = newtxt;

                }
                return;
            }

            foreach (string attribname in displayattriblist)
            {
                string langmsgid = prefix + ctl.ID + "$" + attribname;
                FieldInfo fi = ctl.GetType().GetField(attribname);
                if (fi != null)
                {
                    object oldfi = fi.GetValue(ctl);
                    string oldval = (oldfi == null ? "" : oldfi.ToString());
                    string newval = GetLangMessage(langmsgid, oldval.ToString());
                    if (WebUtils.GetSysLang() != "en" && newval == oldval)
                    {
                        newval = GetLangMessage(ctl.ID + "$" + attribname, oldval.ToString());
                    }
                    if (!string.IsNullOrEmpty(newval) && newval != oldval)
                    {
                        fi.SetValue(ctl, newval);
                    }
                }
                PropertyInfo pi = ctl.GetType().GetProperty(attribname);
                if (pi != null)
                {
                    object oldpi = pi.GetValue(ctl, null);
                    string oldval = (oldpi == null ? "" : oldpi.ToString());
                    string newval = GetLangMessage(langmsgid, oldval.ToString());
                    if (WebUtils.GetSysLang() != "en" && newval == oldval)
                    {
                        newval = GetLangMessage(ctl.ID + "$" + attribname, oldval.ToString());
                    }

                    if (!string.IsNullOrEmpty(newval) && newval != oldval)
                    {
                        pi.SetValue(ctl, newval, null);
                    }
                }
            }
        }
        catch { }
        foreach (Control subctl in ctl.Controls)
        {
            SetLangDisplay(clientID,subctl);
        }
    }


    public class GridViewCommandTemplate : ITemplate
    {

        private string _cmdName;
        private string _cmdType;
        public GridViewCommandTemplate(string cmdName, string cmdType)
        { 
            _cmdName = cmdName;
            _cmdType = cmdType;
        }
        /// <summary>
        /// 此方法为 GridView执行DataBind方法后触发执行 
        /// 按行顺序向下执行(行->行中各列)
        /// </summary>
        /// <param name="container"></param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            Button btn = new Button();
            btn.Text = WebUtils.GetLangMessage("GLOBAL$"+_cmdName,_cmdName);
            btn.CommandName = _cmdName;

            btn.BackColor = System.Drawing.Color.Transparent;
            btn.ForeColor = System.Drawing.Color.Blue;
            btn.Font.Underline = true;
            btn.Click += btn_Click;
                    container.Controls.Add(btn); 
                     
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)(sender));
            GridViewRow row = (GridViewRow)(btn.NamingContainer);
            switch (btn.CommandName)
            {
                case "Select":
                    row.BackColor = System.Drawing.Color.LightBlue;
                    
                    break;
                default:
                    break;
            }
        }

    }

    public class GridViewLabelTemplate : ITemplate
    {
 
        private DataControlRowType _templateType;
        private string _columnName;
        private string _dataType;
        public GridViewLabelTemplate(DataControlRowType templateType, string columnName, string dataType)
        {
            _templateType = templateType;
            _columnName = columnName;
            _dataType = dataType;
        }
        /// <summary>
        /// 此方法为 GridView执行DataBind方法后触发执行 
        /// 按行顺序向下执行(行->行中各列)
        /// </summary>
        /// <param name="container"></param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case DataControlRowType.Header:
                    //创建当前列的标题
                    Literal literal = new Literal();
                    literal.Text =_columnName;
                    literal.EnableViewState = true;
                    container.Controls.Add(literal);
                    break;
              
                case DataControlRowType.DataRow:
                    //创建当前列的一行
                    Label label = new Label();
                    label.BackColor = System.Drawing.Color.Transparent;
                    label.Style.Add("width","100%");
                    label.Style.Add("height", "100%");
                    label.EnableViewState = true;
                    switch (_dataType)
                    {
                        case "DateTime":
                            label.ForeColor = System.Drawing.Color.Blue;
                            break;
                        case "Double":
                            label.ForeColor = System.Drawing.Color.Violet;
                            break;
                        case "Int32":
                            label.ForeColor = System.Drawing.Color.Green; ;
                            break;
                        case "String":
                            label.ForeColor = System.Drawing.Color.Brown;
                            break;
                        default:
                            label.ForeColor = System.Drawing.Color.Green;
                            break;
                    }
                    // 注册用于数据绑定的事件处理程序
                    label.DataBinding -= new EventHandler(this.label_DataBinding);
                    label.DataBinding += new EventHandler(this.label_DataBinding);
                    container.Controls.Add(label);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 当InstantiateIn执行完一行后,统一触发该绑定事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_DataBinding(Object sender, EventArgs e)
        {
            // 获取触发时间的控件
            Label label = (Label)sender;
            // 获取容器行
            GridViewRow row = (GridViewRow)label.NamingContainer;
            // 获取行数据值，并将其格式化
            object dataValue = DataBinder.Eval(row.DataItem, _columnName);
            string rawValue = (dataValue==DBNull.Value?"":dataValue.ToString());
            switch (_dataType)
            {
                case "DateTime":
                    label.Text = String.Format("{0:d}", DateTime.Parse(rawValue));
                    break;
                case "Double":
                    label.Text = String.Format("{0:###,###,##0.00}",
                    Double.Parse(rawValue));
                    break;
                default:
                    label.Text = rawValue;
                    break;
            }
        }
    }

    public static ClientLineData loadLineData(string linename, bool refresh)
    {
        return(loadLineData(linename,"",refresh));
    }

    public static BizResponse RunAPCommand(string cmdname, BizRequest req)
    {
        return ClientMgr.Instance.RunCmd(cmdname, req);
    }

    public static System.Data.DataSet RunAPDbCommand(string cmdname, BizRequest req)
    {
        return ClientMgr.Instance.RunDbCmd(cmdname, req);
    }
    public static System.Data.DataSet RunAPDbCommand(string cmdname, string entityid, string svrname, string clientid, Dictionary<string, string> paramlist)
    {
        BizRequest request = ClientMgr.Instance.CreateRequest(svrname,clientid,entityid, cmdname, paramlist);
        return ClientMgr.Instance.RunDbCmd(cmdname, request);
    }
    public static BizResponse RunAPCommand(string cmdname, string entityid, string svrname, string clientid, Dictionary<string, string> paramlist)
    {
        BizRequest request = ClientMgr.Instance.CreateRequest(svrname,clientid,entityid, cmdname, paramlist);
        return ClientMgr.Instance.RunCmd(request.CmdName, request);
        
    }

    public static ClientLineData reloadlinedata(string linename)
    {
        return loadLineData(linename,true);
    }

    public static ClientLineData loadLineData(string linename,BizResponse response)
    {
        if (response == null)
            return (null);

        ClientLineData _line = new ClientLineData();
        _line.Id = linename;
        _line.UpdateFromServer(response.Data);
        if (BizResponses.OK.Equals(response.ErrorCode))
        {
            _line.IsRun = true;
        }
        else
        {
            if ("1".Equals(response.ReturnCode))
            {
                _line.IsRun = false;
                _line.WOS.Clear();

            }
        }


        return (_line);   

    }
    public static ClientLineData loadLineData(string linename, string messvrname,bool refresh)
    {

        if (LineList.ContainsKey(linename) && refresh)
        {
            LineList.Remove(linename);
        }
        if (LineList.ContainsKey(linename))
        {
            if(LineList[linename].linedata!=null && LineList[linename].updatetime.AddSeconds(WebUtils.UpdateInterval_lines)>DateTime.Now)
            return (LineList[linename].linedata);
        }
        string svrname = messvrname;
        if (string.IsNullOrEmpty(svrname))
        {
            svrname =  GetAppSettingValue("Global","MES","ServerName",true);
            if(string.IsNullOrEmpty(svrname))
                svrname = "MES";
        }
        ClientLineData _line = new ClientLineData();
            _line.Id = linename;
        BizResponse response = ClientMgr.Instance.RunCmd("LineStatus", WebUtils.globalApClient.CreateTextRequest(svrname,linename,linename, "LineStatus", "line=1;wo=1;stn=1;wip=1"));
        if (BizResponses.OK.Equals(response.ErrorCode))
        {
            _line.IsRun = true;
            _line.UpdateFromServer(response.Data);
        }
        else
        {
            if ("1".Equals(response.ReturnCode))
            {
                _line.IsRun = false;
                _line.WOS.Clear();

            }
        }
                BizRequest request = ClientMgr.Instance.CreateRequest(svrname, _line.Id, "", "LineLog", null);
                response = ClientMgr.Instance.RunCmd(request.CmdName, request);
                if (BizResponses.OK.Equals(response.ErrorCode))
                {
                    _line.UpdateLogFromServer(response.Data);
                }
                LineInfo newln = new LineInfo();
                newln.linedata = _line;
                newln.lineid = linename;
                newln.updatetime = DateTime.Now;
                lock (_linelist)
                {
                    LineList.Remove(linename);
                    LineList.Add(linename, newln);
                }
        WebUtils.Logger.Debug("LoadLineDATA:" + linename);
                return (_line);   

    }

    public static string PlantName
    {
        get
        {
            if (HttpContext.Current.Application["BUNO"] != null)
            {
                return (HttpContext.Current.Application["BUNO"].ToString());
            }
            string BUNO = WebUtils.DB.Query("SELECT * FROM ENG_BU").Rows[0]["BUNO"].ToString();
            HttpContext.Current.Application["BUNO"] = BUNO;
            return (BUNO);
        }
        set
        {
            HttpContext.Current.Application["BUNO"] = value;
        }
    }
    public static string PlantDescription
    {
        get
        {
            if (HttpContext.Current.Application["BUNAME"] != null)
            {
                return (HttpContext.Current.Application["BUNAME"].ToString());
            }
            string BUNO = WebUtils.PlantName;
            string BUNAME = WebUtils.DB.Query("SELECT * FROM ENG_BU where BUNO='" + BUNO + "'").Rows[0]["BUNAME"].ToString();
            HttpContext.Current.Application["BUNAME"] = BUNAME;
            return (BUNAME);
        }
        set
        {
            HttpContext.Current.Application["BUNAME"] = value;
        }
    }

    public static void ExportToExcel(System.Web.UI.WebControls.GridView gv, string filename, string encodingname)
    { 
        HttpResponse resp;
        resp = gv.Page.Response;
        //string encodename = (string.IsNullOrEmpty(encodingname) ? "Unicode" : encodingname);
        resp.ContentEncoding = HttpContext.Current.Request.ContentEncoding; 
        if(!string.IsNullOrEmpty(encodingname))
            resp.ContentEncoding = System.Text.Encoding.GetEncoding(encodingname);
        //resp.SuppressContent = true; 
        
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
        resp.ContentType = "application/text";
        string ls_item = "";


        if (gv.HeaderRow != null)
        {
            for (int Y = 0; Y < gv.HeaderRow.Cells.Count; Y++)
            {
                ls_item += (gv.HeaderRow.Cells[Y].Text == null ? "" : gv.HeaderRow.Cells[Y].Text) + "\t";
            }
            ls_item += "\r\n";
            resp.Write(ls_item.Replace("&nbsp;", ""));
            
            ls_item = "";
        }
            for (int X = 0; X < gv.Rows.Count; X++)
            {
                for (int L = 0; L < gv.Rows[X].Cells.Count; L++)
                    ls_item += (gv.Rows[X].Cells[L].Text == null ? "" : gv.Rows[X].Cells[L].Text) + "\t";
                ls_item += "\r\n";
                resp.Write(ls_item.Replace("&nbsp;", ""));  //FiterHtml.NoHTML().Trim().Replace("&nbsp;","")
                ls_item = "";

            }
        resp.End();
    }
    
    public static void ExportToExcel(System.Web.UI.Page pg,System.Data.DataTable dt,string fieldcaptionmap, string filename,string encodingname)
    {
        HttpResponse resp;
        resp = pg.Response;
        string encodename = (string.IsNullOrEmpty(encodingname)?"UTF8":encodingname);
        resp.ContentEncoding = System.Text.Encoding.GetEncoding(encodename);
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
        resp.ContentType = "application/text";
        string  ls_item = "";
        if (!string.IsNullOrEmpty(fieldcaptionmap))
        {
            string[] fieldmaplist = fieldcaptionmap.Split(new char[] { ';' });
            foreach (string fieldmapitem in fieldmaplist)
            {
                if (string.IsNullOrEmpty(fieldmapitem))
                    continue;
                string fieldname = fieldmapitem;
                string cap = fieldmapitem;
                if (fieldmapitem.Contains('='))
                {
                    fieldname = fieldmapitem.Substring(0, fieldmapitem.IndexOf("="));
                    cap = fieldmapitem.Substring(fieldmapitem.IndexOf("=") + 1);
                    if (dt.Columns.Contains(fieldname))
                        dt.Columns[fieldname].Caption = cap;
                }
                else if (fieldmapitem.Contains(','))
                {
                    fieldname = fieldmapitem.Substring(0, fieldmapitem.IndexOf(","));
                    cap = fieldmapitem.Substring(fieldmapitem.IndexOf(",") + 1);
                    if (dt.Columns.Contains(fieldname))
                        dt.Columns[fieldname].Caption = cap;
                }
            }
        }
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ls_item += (string.IsNullOrEmpty(dt.Columns[i].Caption) ? dt.Columns[i].ColumnName : dt.Columns[i].Caption) + "\t";
        }
        ls_item += "\n";
        resp.Write(ls_item);
        ls_item = "";
        foreach(System.Data.DataRow dr in dt.Rows )
        {
            for (int X = 0; X < dt.Columns.Count; X++)
                ls_item += (dr[X] == DBNull.Value ? "" : dr[X].ToString()) + "\t";
            ls_item += "\n";
            resp.Write(ls_item);
            ls_item = "";

        }
        resp.End();
    }
    public static void ToExcel(System.Web.UI.Control ctl)
    {
        // HttpContext.Current.Response.Charset ="GB2312"; 
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=money.xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    } 
    private static Dictionary<string, Dictionary<string, permissionMatrix>> _userpmatrix = new Dictionary<string, Dictionary<string, permissionMatrix>>();
    public static Dictionary<string, Dictionary<string, permissionMatrix>> UserPermissions 
    {
        
        get
        {
            if (HttpContext.Current.Application["PermissionMatrix"] != null)
                return ((Dictionary<string, Dictionary<string, permissionMatrix>>)(HttpContext.Current.Application["PermissionMatrix"]));

             if (_userpmatrix == null)
                _userpmatrix = new Dictionary<string, Dictionary<string, permissionMatrix>>();
             HttpContext.Current.Application["PermissionMatrix"] = _userpmatrix;
             return ((Dictionary<string, Dictionary<string, permissionMatrix>>)(HttpContext.Current.Application["PermissionMatrix"]));
        }
        set
        {
            _userpmatrix = value;
        } 
    }
    public static string GetUserDeptNo(string username)
    {

        System.Data.DataTable dt = DB.Query("select DEPTNO from HR_OPERATORS where OPERID='" + username + "' ");
        foreach (System.Data.DataRow dr in dt.Rows)
        {
            if (dr["DEPTNO"] != DBNull.Value)
                return (dr["DEPTNO"].ToString());

        }
        return ("");
    }

    //public static string ValidateUserUponLine(string )
    public static string GetPermission(string username, string password, string moduleid, string entityid)
    {
        WebUtils.Logger.Debug("verify permission:" + username + " : " + moduleid +"."+ entityid);
        string Uname = username.ToUpper();
        if (!UserPermissions.ContainsKey(username))
        {
            lock (UserPermissions)
            UserPermissions.Add(username, new Dictionary<string, permissionMatrix>());
        }
        if (UserPermissions[username].ContainsKey(moduleid + "." + entityid))
        {
            return (UserPermissions[username][moduleid + "." + entityid].Permission);
        }
        string userlist = "'" + username + "'";
        System.Data.DataTable dt;
        dt = DB.Query("select ROLEID from HR_ROLES where userid='" + username + "'");
        foreach (System.Data.DataRow dr in dt.Rows)
        {
            if (dr["ROLEID"] != DBNull.Value)
                userlist += ",'" + dr["ROLEID"].ToString() + "'";
            
        }
        string permission = "NG";

        if (userlist.ToLower().Contains("'admin'"))
        {
            permission = "RWX";
        }
        else
        {
            dt = DB.Query("SELECT * FROM HR_PERMISSIONS WHERE USERID in (" + userlist + ") and (MODULEID='" + moduleid + "' OR MODULEID='ALL') AND (ITEMID = '" + entityid + "' OR ITEMID='ALL') ");

            if (dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {

                    if (dr["PERMISSIONS"] == DBNull.Value)
                    {
                        permission = "RWX";
                    }
                    else
                    {
                        permission += dr["PERMISSIONS"].ToString();
                    }
                }

                permission = (permission.Contains("R") ? "R" : "") + (permission.Contains("W") ? "W" : "") + (permission.Contains("X") ? "X" : "");
            }
        }
        permissionMatrix perm = new permissionMatrix();
        perm.UserName = username;
        perm.Permission = permission;
        perm.moduleId = moduleid;
        perm.entityId = entityid;
        perm.DeptNo = GetUserDeptNo(username);
        lock (UserPermissions)
        UserPermissions[username].Add(moduleid + "." + entityid, perm);
        //WebUtils.Logger.Debug(username + ", got permission:" + permission);
        return (UserPermissions[username][moduleid + "." + entityid].Permission);
    }


    public static void disposeSessionDt(HttpSessionState session, string dtname)
    {
        if (session[dtname] != null)
        {
            ((DataTable)session[dtname]).Clear();
            ((DataTable)session[dtname]).Dispose();
            session[dtname] = null;
        }
    }
    public static string serverid{
        get{
            return("REPORT");
        }
    }

    public static string nopermissionurl
    {
        get
        {
            return "/Nopermission.html";
        }
    }

    public static bool LoginUser(string username, string password)
    {
        //WebUtils.Logger.Debug("verify login:" + username);
        int i= DB.Query("SELECT count(*) FROM APP_USERS").Rows.Count;
        if (i == 0)
        {
            return (true);
        }
        System.Data.DataTable dt = DB.Query("SELECT COUNT(*) verified FROM APP_USERS WHERE USERID='" +username+"' AND PASSWORD='"+password +"' AND SERVERID='"+serverid+"'");
        if (dt.Rows.Count == 1 && dt.Rows[0][0] != DBNull.Value && dt.Rows[0][0].ToString().Trim() == "1")
            return (true);
       
        return (false);
    }
    public static bool VerifyPermission(string username, string password,string moduleid, string entityid)
    {
        string perm = GetPermission(username, password, moduleid, entityid);
        if ( perm == "NG" || perm=="---")
            return (false);
        return (true);
    }
    
    public static Control GetControlByField(Control ctrlobj, string fldname,string ctrltype)
    {
        if (ctrlobj.ID == fldname)
            return (ctrlobj);
        if (ctrlobj.GetType().Name.IndexOf("Qry_") >= 0 && ctrlobj.GetType().Name.ToUpper().IndexOf(ctrltype.ToUpper())>=0)
        {
            if (((Iwebqryfield)(ctrlobj)).LinkedDbFieldName == fldname)
                return (ctrlobj);
        }
        foreach (Control ctrlitem in ctrlobj.Controls)
        {
            Control ctrl =  GetControlByField(ctrlitem, fldname,ctrltype);
            if (ctrl != null)
                return (ctrl);
        }
        return (null);
    }

    public static string GetOrderByFromCheckBox(Control inputcontainer, string sid)
    {
        string orderby="";
        if (inputcontainer.Visible == false)
            return ("");
        if (inputcontainer.GetType().Name.IndexOf("Qry_") >= 0 && inputcontainer.GetType().Name.ToUpper().IndexOf("CHECK") >= 0)
        {
            string cbsql = ((Qry_CheckBox)(inputcontainer)).GetOrderBy();
            if(cbsql!=null && cbsql!="")
                orderby += ((Qry_CheckBox)(inputcontainer)).GetOrderBy() + ",";
        }
        else
        {
            foreach (Control ctrlitem in inputcontainer.Controls)
            {
                orderby += GetOrderByFromCheckBox(ctrlitem, sid);
            }
        }
        return (orderby);
    }
    public static string AssemblySQL(string clientid, string wheresql,string orderbysql)
    {
        string selectsql = GetAppSettingValue("Client", clientid, "SQLTXT", false);
        if (selectsql == null)
            return ("");
        if (!selectsql.ToUpper().Trim().StartsWith("SELECT"))
            return ("");

        string wheretxt= wheresql;
        if (wheretxt == null)
            wheretxt = "";
        wheretxt = wheretxt.Trim();
        string WHERE = GetAppSettingValue("Client", clientid, "WHERESQL", false);
        string GROUP = GetAppSettingValue("Client", clientid, "GROUPSQL", false);
        string ORDERBY = GetAppSettingValue("Client", clientid, "ORDERBYSQL", false);

        if(WHERE==null)
            WHERE= " WHERE ";
        if(WHERE=="")
            WHERE = " WHERE ";
        if(!(WHERE.Trim().ToUpper().StartsWith("WHERE")))
            WHERE = " WHERE " + WHERE;

        if (ORDERBY == null)
            ORDERBY = "";
        if (orderbysql != "")
        {
            if (ORDERBY != "")
            {
                if (ORDERBY.Trim().EndsWith(","))
                    ORDERBY = ORDERBY.Trim().Substring(0, ORDERBY.Length - 1);
            }
            ORDERBY =( ORDERBY + orderbysql).Trim();

        }
        if (ORDERBY != "")
        {
            if (ORDERBY.EndsWith(","))
                ORDERBY = ORDERBY.Trim().Substring(0, ORDERBY.Length - 1);
        }

        if (!ORDERBY.Trim().ToUpper().StartsWith("ORDER ") && ORDERBY != "")
        {
            ORDERBY = " ORDER BY " + ORDERBY;
        }
        Logger.Debug("WHERE=" + WHERE + "; wheresql=" + wheretxt);
        if (wheretxt.IndexOf(" ") > 0 && wheretxt.Substring(wheretxt.IndexOf(" ")).Trim() != "")
        {
            if (wheretxt.ToUpper().StartsWith("AND "))
                wheretxt = wheretxt.Substring(4);
            if (wheretxt.ToUpper().StartsWith("OR "))
                wheretxt = wheretxt.Substring(3);

            WHERE = WHERE + wheretxt;
        }
        else
        {
            WHERE = WHERE + wheretxt;
        }
        if(WHERE.Trim()=="WHERE")
            WHERE ="";

        if (GROUP != null && GROUP.Trim() != "")
        {
            GROUP = (GROUP.Trim().ToUpper().StartsWith("GROUP ") ? GROUP : " GROUP BY " + GROUP);
        }
        else
        {
            GROUP = "";
        }
        if (GROUP != "")
            ORDERBY = "";
        return ((selectsql + " " + WHERE + " " + GROUP).Trim() + ORDERBY);
    }
    public static string GetSQLfromInput(Control inputcontainer, string sid)
    {
        string sql = "";

        foreach(Control ctrlitem in inputcontainer.Controls)
        {
            sql += GetSQLfromInput(ctrlitem, sid);
            
        }
        /*
        if (inputcontainer.GetType().Name.ToUpper().IndexOf("TABLEROW")>=0)
        {
            foreach (TableCell tc in ((TableRow)inputcontainer).Cells)
            {
                sql += GetSQLfromInput(tc, sid);
            }
        }
        */
        if (inputcontainer.GetType().Name.IndexOf("Qry_") >= 0)
        {
            sql += ((Iwebqryfield)inputcontainer).GetSQL();
        }

        return (sql);
    }


    public static string GetFieldValue(string fieldval,  string LinkedFieldType, string LinkedDbFieldName,string LinkedDBname)
    {
        string Text = fieldval.Trim();

        if (Text == null)
            return ("");

        if (Text == "")
            return ("");
        
        // string sql = "";
        string fieldtype = "STRING";
        if (LinkedFieldType != null && LinkedFieldType != "")
            fieldtype = LinkedFieldType;

        string dbtype = "mssql";
        if (ConfigurationManager.AppSettings["DB_" + LinkedDBname] != null && ConfigurationManager.AppSettings["DB_" + LinkedDBname] != "")
            dbtype = ConfigurationManager.AppSettings["DB_" + LinkedDBname];
        dbtype = dbtype.ToUpper();

        // DATEFORMAT: YYYY/MM/DD OR YYYY/MM/DD HH24:MI:SS
        if (fieldtype.IndexOf("DATE") >= 0)
        {
            if (dbtype == "mssql")
            {
                Text = "cast(" + fieldval + " as datetime)";
            }
            else if (dbtype == "oracle")
            {
                if (fieldval.IndexOf(":") > 0)
                {
                    Text = "TO_DATE('" + fieldval + "','YYYY/MM/DD HH24:MI:SS')";
                }
                else
                {
                    Text = "TO_DATE('" + fieldval + "','YYYY/MM/DD')";
                }
            }
            else if (dbtype == "mysql") // str_to_date("2010-11-23 14:39:51",'%Y-%m-%d %H:%i:%s');
            {
                if (fieldval.IndexOf(":") > 0)
                {
                    Text = "str_to_date('" + fieldval + "','%Y/%m/%d %H:%i:%s')";
                }
                else
                {
                    Text = "str_to_date('" + fieldval + "','%Y/%m/%d')";
                }

            }
        }
        else if (fieldtype.IndexOf("STR") >= 0)
        {
            Text = "'" + fieldval + "'";
        }
        else
        {
            Text = fieldval;
        }

        return (Text);
    }


    public static string GetSQLbyFieldinput(string fieldval, string LinkedPrefix, string LinkedFieldType, string LinkedDbFieldName, string LinkedOperator, string LinkedDBname,string subsql)
    {
       string Text = fieldval.Trim();
       string SubQuerySQL = subsql;
       if (SubQuerySQL == null)
           SubQuerySQL = "";
       if (SubQuerySQL.Trim() != "")
       {
           SubQuerySQL = SubQuerySQL.Replace("[FieldValue]", fieldval);
           if (!SubQuerySQL.StartsWith("(") || !SubQuerySQL.EndsWith(")"))
               SubQuerySQL = "(" + SubQuerySQL + ")";
           
       }

        if (Text == null)
            return ("");

        if (Text == "")
            return ("");
        if (LinkedOperator == null)
            LinkedOperator = "";

        if (LinkedOperator.ToUpper() == "X")
            return ("");
        
        string sql = "";
        string prefix = (LinkedPrefix == null ? "" : LinkedPrefix);
        prefix = (prefix == "" ? " AND " : prefix);

        if (LinkedOperator == null || LinkedOperator == "")
        {
            if (SubQuerySQL != "")
                return (prefix + " " + SubQuerySQL + " ");
            return ("");
        }
        
        if (SubQuerySQL != "")
        {
            return (" " + prefix + " " + LinkedDbFieldName + " " + LinkedOperator + " " + SubQuerySQL + " ");
        }

        string fieldtype = "STRING";
        if (LinkedFieldType != null && LinkedFieldType != "")
            fieldtype = LinkedFieldType.ToUpper();

        string dbtype = "mssql";
        if(ConfigurationManager.AppSettings["DB_"+LinkedDBname]!=null && ConfigurationManager.AppSettings["DB_"+LinkedDBname]!="")
            dbtype = ConfigurationManager.AppSettings["DB_"+LinkedDBname];
        dbtype = dbtype.ToLower();

        // DATEFORMAT: YYYY/MM/DD OR YYYY/MM/DD HH24:MI:SS
        if(fieldtype.IndexOf("DATE") >= 0)
        {
            if (dbtype == "mssql")
            {
                Text = "cast('" + fieldval + "' as datetime)";
            }
            else if (dbtype == "oracle")
            {
                if (fieldval.IndexOf(":") > 0)
                {
                    Text = "TO_DATE('" + fieldval + "','YYYY/MM/DD HH24:MI:SS')";
                }
                else
                {
                    Text = "TO_DATE('" + fieldval + "','YYYY/MM/DD')";
                }
            }
            else if (dbtype == "mysql") // str_to_date("2010-11-23 14:39:51",'%Y-%m-%d %H:%i:%s');
            {
                if (fieldval.IndexOf(":") > 0)
                {
                    Text = "str_to_date('" + fieldval + "','%Y/%m/%d %H:%i:%s')";
                }
                else
                {
                    Text = "str_to_date('" + fieldval + "','%Y/%m/%d')";
                }

            }
        }

        if (LinkedOperator.ToUpper().Trim() == "IN" && fieldtype.IndexOf("STR")>=0)
        {
            string[] vallist = Text.Split(new char[] { ',', ';' });
            string str = "";
            foreach (string valitem in vallist)
            {
                if (valitem == null || valitem == "")
                    continue;
                if (valitem.StartsWith("'") && valitem.EndsWith("'"))
                {
                    str += valitem + ",";
                }
                else
                {
                    str += "'"+valitem + "',";
                }
            }
            if (str.EndsWith(","))
            {
                str = "(" + str.Substring(0, str.Length - 1) + ")";
            }
            Text = str;
        }
        else if (LinkedOperator.ToUpper().Trim() == "IN")
        {
            Text= "(" + Text + ")";
        }
        else if (LinkedOperator.ToUpper().Trim() == "LIKE" && fieldtype.IndexOf("STR") >= 0)
        {
            if (Text.StartsWith("'") && Text.EndsWith("'"))
            {
                
            }
            else
            {
                Text = "'" + Text + "%'";
            }

        }
        else if (fieldtype.IndexOf("STR") >= 0)
        {
            if (Text.StartsWith("'") && Text.EndsWith("'"))
            {

            }
            else
            {
                Text = "'" + Text + "'";
            }

        }
 
        
        sql = prefix + " (" + LinkedDbFieldName + " " + LinkedOperator + " " +  Text + ") ";

        return (sql);
    }



    static Control initForm_CreateCustomObj(string typename, string objname)
    {
        string uppertypename = typename.ToUpper();
        if (uppertypename.IndexOf("TEXTBOX") >= 0)
        {
            Qry_TextBox newtb = new Qry_TextBox();
           
            newtb.ID = objname;
            return (newtb);
        }
        if (uppertypename.IndexOf("LABEL") >= 0)
        {
            Qry_Label newlb = new Qry_Label();
            newlb.ID = objname;
            return (newlb);
        }
        if (uppertypename.IndexOf("DROP") >= 0 && uppertypename.IndexOf("LIST") >= 0)
        {
            Qry_DropDownList newdrb = new Qry_DropDownList();
            newdrb.ID = objname;
            newdrb.FetchSQLData();
            return (newdrb);
        }
        if (uppertypename.IndexOf("CALENDAR") >= 0)
        {
            Qry_Calendar newcal = new Qry_Calendar();
            newcal.ID = objname;
            return (newcal);
        }


        if (uppertypename.IndexOf("TABLEROW") >= 0)
        {
            TableRow newTabrow = new TableRow();
            newTabrow.ID = objname;
            return (newTabrow);
        }

        if (uppertypename.IndexOf("TABLECELL") >= 0)
        {
            TableCell newTabcell = new TableCell();
            newTabcell.ID = objname;
            return (newTabcell);
        }

        if (uppertypename.IndexOf("TABLE") >= 0)
        {
            Table newTab = new Table();
            newTab.ID = objname;
            return (newTab);
        }

        if (uppertypename.IndexOf("CHECKBOX") >= 0)
        {
            Qry_CheckBox newCBK = new Qry_CheckBox();
            newCBK.ID = objname;
            return (newCBK);
        }


        return (null);
    }

    static void initLang()
    {
        string langlist = ConfigurationManager.AppSettings["Global_LangList"];
        foreach (string lngid in langlist.Split(new char[] { ',', ';' }))
        {
            _LangMessageList[lngid] = WebUtils.loadMessageSettings(lngid);
        }
        if (ConfigurationManager.AppSettings["Global_DefaultLang"] != null)
        {
            string langid = ConfigurationManager.AppSettings["Global_DefaultLang"];
            SetSysLang(langid);
        }
    }

    static Dictionary<string, Dictionary<string, string>> _LangMessageList = new Dictionary<string, Dictionary<string, string>>();
    public static Dictionary<string, Dictionary<string, string>> LangMessageList
    {
        get
        {
            if (_LangMessageList != null)
                return (_LangMessageList);
            if (HttpContext.Current.Application["MessageSettings"] != null)
            {
                _LangMessageList = (Dictionary<string, Dictionary<string, string>>)(HttpContext.Current.Application["MessageSettings"]);
                return (_LangMessageList);
            }
            if (ConfigurationManager.AppSettings["Global_LangList"]!=null && _LangMessageList.Count==0)
            {
                string langlist = ConfigurationManager.AppSettings["Global_LangList"];
                foreach (string lngid in langlist.Split(new char[] { ',', ';' }))
                {
                    _LangMessageList[lngid]= WebUtils.loadMessageSettings(lngid);
                }
            }
            HttpContext.Current.Application["MessageSettings"] = _LangMessageList;
            return (_LangMessageList);
        }
        set
        {
            _LangMessageList = value;
            HttpContext.Current.Application["MessageSettings"] = _LangMessageList;
        }
    }

    public static Dictionary<string,string> loadMessageSettings(string langid)
    {
        BizRequest req = WebUtils.globalApClient.CreateRequest("service", "WEBClient", langid, "GetLANGmsglist", new Dictionary<string, string>());

        BizResponse resp = WebUtils.globalApClient.RunCmd(req.CmdName, req);
        return (resp.Data);

    }

    public static string createtransferpagelink(Page pg, string targetpagefile,Dictionary<string,string> replacepairs)
    {
        string querystring = "";
        if (pg.Request.QueryString.Count == 0)
            return (targetpagefile);
        int i=0;
        foreach (string keyid in pg.Request.QueryString.Keys)
        {
            string keyval = (i == 0 ? keyid + "=" + pg.Request.QueryString[keyid] : "&" + keyid + "=" + pg.Request.QueryString[keyid]);
            if(replacepairs.ContainsKey(keyid))
                keyval = (i == 0 ? keyid + "=" + replacepairs[keyid] : "&" + keyid + "=" + replacepairs[keyid]);
            querystring += keyval;
            i += 1;
        }
        return (targetpagefile + (string.IsNullOrEmpty(querystring)?"":"?"+querystring));
    
    }
    public static string GetSysLang()
    {
        string langid = "en";
        if ((HttpContext.Current!=null && HttpContext.Current.Session!=null) && HttpContext.Current.Session["Session_Lang"] != null)
        {
            langid = HttpContext.Current.Session["Session_Lang"].ToString();
        }
        else
            if (ConfigurationManager.AppSettings["Global_DefaultLang"] != null)
            {
                langid = ConfigurationManager.AppSettings["Global_DefaultLang"];
            }
        return(langid);

    }
    public static bool SetSysLang(string langid)
    {
        // ConfigurationManager.AppSettings["Session_Lang"] = langid;
        if(HttpContext.Current!=null && HttpContext.Current.Session!=null)
        HttpContext.Current.Session["Session_Lang"] = langid;
        WebUtils.globalApClient.LangId = langid;
        initSQL("");
        return (true);
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="langid"></param>
    /// <param name="msgid"></param>
    /// <param name="msgtxt"></param>
    /// <returns></returns>
    public static string GetLangMessage(string msgid, string msgtxt)
    {
        string langid = WebUtils.GetSysLang();
        return (GetLangMessage(langid, msgid, msgtxt));
    }

    /// <summary>
    /// return multi-language message
    /// </summary>
    /// <param name="langid"></param>
    /// <param name="msgid"></param>
    /// <param name="msgtxt"></param>
    /// <returns></returns>
    public static string GetLangMessage(string langid,string msgid,string msgtxt)
    {
        string CMSGID = msgid.ToUpper();
        if (!LangMessageList.ContainsKey(langid))
            LangMessageList[langid] = loadMessageSettings(langid);
        if (!LangMessageList.ContainsKey(langid))
        {
            if (!string.IsNullOrEmpty(msgtxt))
                return (msgtxt);
            if (LangMessageList.ContainsKey("en") && LangMessageList["en"].ContainsKey(CMSGID))
                return (LangMessageList["en"][CMSGID]);
            return ("");

        }
        if (LangMessageList[langid] != null && LangMessageList[langid].ContainsKey(CMSGID))
            return (LangMessageList[langid][CMSGID]);
        return (string.IsNullOrEmpty(msgtxt) ? "" : msgtxt);
    }
    public static Dictionary<string, string> GetConfigList(string cfgfileid)
    {
        BizRequest req = WebUtils.globalApClient.CreateRequest("config", "WEBClient", cfgfileid, "LoadConfig", new Dictionary<string, string>());

        BizResponse resp = WebUtils.globalApClient.RunCmd(req.CmdName, req);
        if ( resp.ErrorCode == "OK" )
            return (resp.Data);
        return (new Dictionary<string, string>());
    }
    public static bool ReloadLangMessageList(string langid)
    {
        try
        {
            BizRequest req = WebUtils.globalApClient.CreateRequest("service", "WEBClient", langid, "ReloadLangMsgList", new Dictionary<string, string>());

            BizResponse resp = WebUtils.globalApClient.RunCmd(req.CmdName, req);
            if (!(resp.ErrorCode == "OK"))
                return (false);
            LangMessageList[langid] = loadMessageSettings(langid);
            return (true);
        }
        catch { }
        return (false);

    }

    static DBHelper _db = null;

    public static DBHelper DB
    {
        get
        {
            if(HttpContext.Current.Application["DB"]!=null)
                return ((DBHelper)(HttpContext.Current.Application["DB"]));
            if (_db == null)
                initDB();
            HttpContext.Current.Application["DB"] = _db;
            return (_db);
        }
        set
        {
            _db = value;
        }
    }

    public static ClientMgr globalApClient
    {
        get
        { 
            if(HttpContext.Current.Application["GlobalAPClient"]!=null)
                return((ClientMgr)(HttpContext.Current.Application["GlobalAPClient"]));
            HttpContext.Current.Application["GlobalAPClient"] = initApClient();
            return ((ClientMgr)(HttpContext.Current.Application["GlobalAPClient"]));
             
            
        }
        set
        {
            HttpContext.Current.Application["GlobalAPClient"] = value;
        }
    }

    public WebUtils()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
       

        
	}

    //public static ClientMgr intelliClient = null;

    public static ClientMgr initApClient()
    {
        if (HttpContext.Current.Application["GlobalAPClient"] != null)
            return ((ClientMgr)(HttpContext.Current.Application["GlobalAPClient"]));
        if (ClientMgr.Instance != null)
            return (ClientMgr.Instance);
        ClientMgr intelliClient = null;
        intelliClient = new ClientMgr();
        intelliClient.Init();
        Logger.Debug("currentdirectory:" + AppDomain.CurrentDomain.BaseDirectory);
        if (!string.IsNullOrEmpty(intelliClient.Config["server.listener.1"]))
            Logger.Debug("Client inited!" + intelliClient.Config["server.listener.1"]);
        return (intelliClient);
    }

    public static string CallOSexe(string exename, string workpath, string args, bool killsameprocess)
    {
        string cmdname = exename;
        string workdir = workpath;
        if (!string.IsNullOrEmpty(WebUtils.GetAppSettingValue("ap", "service", exename, true)))
            cmdname = WebUtils.GetAppSettingValue("ap", "service", exename, true);
        if (!string.IsNullOrEmpty(WebUtils.GetAppSettingValue("ap", "service", "workdir", true)))
            workdir = WebUtils.GetAppSettingValue("ap", "service", "workdir", true);
        if (!string.IsNullOrEmpty(workpath))
            workdir = workpath;
        workdir = Intelli.MidW.Utils.Common.FileUtils.getDirectoryForURL(workdir);
        /*workdir = System.IO.Path.GetFullPath(workdir);*/

        if (string.IsNullOrEmpty(workdir) && cmdname.IndexOf("\\") > 0)
        {
            workdir = System.IO.Path.GetDirectoryName(cmdname);
            cmdname = System.IO.Path.GetFileName(cmdname);
        }
        
        if (workdir.IndexOf("..\\") >= 0)
            workdir = workdir.Replace("..\\", Intelli.MidW.Utils.Common.FileUtils.getParentDirectory(System.Web.HttpContext.Current.Request.MapPath("/")).TrimEnd(new char[] { '\\' })+"\\");
        if (workdir.IndexOf(".\\") >= 0)
            workdir = workdir.Replace(".\\", System.Web.HttpContext.Current.Request.MapPath("/").TrimEnd(new char[] { '\\' })+"\\");
        

        if (killsameprocess)
        {
            Intelli.MidW.Utils.Common.CommUtils.KillProgram(cmdname);
        }
        Logger.Debug("Run exe:" + cmdname);
        // return(Intelli.MidW.Utils.Common.CommUtils.runshellcmd(programname, args));


        new Thread(new ThreadStart(() =>
        {
            ProcessStartInfo start = new ProcessStartInfo(workdir+"\\"+cmdname);// 这个文件系统会自己找到
            start.Arguments = args;

            //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe
            if (!string.IsNullOrEmpty(workdir))
                start.WorkingDirectory = workdir;
            if (cmdname.IndexOf("\\") > 0)
                start.WorkingDirectory = System.IO.Path.GetDirectoryName(cmdname);
            start.CreateNoWindow = true;//不显示dos命令行窗口
            start.RedirectStandardOutput = true;//
            start.RedirectStandardInput = true; //
            start.UseShellExecute = false; //是否指定操作系统外壳进程启动程序


            Process p = Process.Start(start);
            p.WaitForExit();
        })).Start();

        
        return ("0");
        
    }
    public static void init()
    {
        WebUtils.DB = new DBHelper();
        initLang();
        initLog();
        initDB();
        initSQL("");
        initApClient();
        //HttpContext.Current.Session.Timeout = 60;

        //initBgThread();
        Logger.Debug("Init finished");
    }

    public static bool StopBg = false;
    public static void initBgThread()
    {

        /*
        new Thread(new ThreadStart(() =>
        {
            LoopBgThread();

        })).Start();
         * */
    }


    public static Dictionary<string,string> getContainerTypeList(){
        Dictionary<string,string> tylist = new Dictionary<string, string>();

        System.Data.DataTable dt = WebUtils.DB.Query("select CONTNRTYPE,CONTAINERDESC from WMS_CONTNRTYPE");
        foreach (DataRow dr in dt.Rows)
        {
            tylist.Add(dr[0].ToString(), (dr[1] == DBNull.Value? dr[0].ToString():dr[1].ToString()));
        }
        return (tylist);
    }

    public static void SetFocus(System.Web.UI.HtmlControls.HtmlInputText control)
    {
        /* 
        * 1、编写：2005-7-25
        * 2、功能：让控件得到焦点
        * 3、参数：System.Web.UI.WebControls.TextBox control 任意已实例化的控件（文本框）
        * 4、返回值： 无
        * 5、用途：使传入该方法的控件得到焦点
        */
        if (System.Web.HttpContext.Current.Request.Browser.JavaScript)
        {
            control.Page.RegisterStartupScript("sf", "<script language='javascript'>document.forms[0]." +
            control.ClientID + ".focus();</script>");
        } 
    } 
    public static void SetFocus(System.Web.UI.WebControls.TextBox control)
    {
        /* 
        * 1、编写：2005-7-25
        * 2、功能：让控件得到焦点
        * 3、参数：System.Web.UI.WebControls.TextBox control 任意已实例化的控件（文本框）
        * 4、返回值： 无
        * 5、用途：使传入该方法的控件得到焦点
        */
        if (System.Web.HttpContext.Current.Request.Browser.JavaScript)
        {
            control.Page.RegisterStartupScript("sf", "<script language='javascript'>document.forms[0]." +
            control.ClientID + ".focus();</script>");
        }
    }


    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        string selectlangval = e.Item.Value;
        WebUtils.SetSysLang(selectlangval);
    }
    public static int UpdateInterval_lines
    {
        get
        {
            string intervalstring = GetAppSettingValue("Global", "BG", "IntervalSeconds", false);
            if (!string.IsNullOrEmpty(intervalstring))
            {
                try
                {
                    return int.Parse(intervalstring);
                }
                catch { }
            }
            return (300);
        }
    }
    public static void LoopBgThread()
    {

        new Thread(new ThreadStart(() =>
        {
            int sleepseconds = 300;
            string intervalstring = GetAppSettingValue("Global", "BG", "IntervalSeconds", false);
            if(!string.IsNullOrEmpty(intervalstring))
            {
                try
                {
                    sleepseconds = int.Parse(intervalstring);
                }
                catch { }
            }
            System.Threading.Thread.Sleep(sleepseconds * 1000);
            Logger.Debug("bgrun:");
            if (!StopBg)
            LoopBgThread();

        })).Start();
    }

    public class GridViewTemplate : ITemplate
    {
        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler eh;
        private DataControlRowType templateType;
        private string columnName;
        private string controlID;
        public GridViewTemplate(DataControlRowType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }
        public GridViewTemplate(DataControlRowType type, string controlID, string colname)
        {
            templateType = type;
            this.controlID = controlID;
            columnName = colname;
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {
            switch (templateType)
            {
                case DataControlRowType.Header:
                    Literal lc = new Literal();
                    lc.Text = columnName;
                    container.Controls.Add(lc);
                    break;
                case DataControlRowType.DataRow://可以定义自己想显示的控件以及绑定事件  
                    break;
                default:
                    break;
            }
        }
    }

        
    private static void initFieldSettings(System.Xml.XmlNode thissqlnode)
    {
        if (thissqlnode == null)
            return;

        string sqlid = thissqlnode.Name;
        if (thissqlnode.Attributes["Name"] != null)
            sqlid = thissqlnode.Attributes["Name"].Value;

    }

    public static Dictionary<string, string> GetFieldSetting(string parentid, string itemid)
    {
        string parentname = parentid;
        if (parentid.StartsWith("DBFD_"))
            parentname = parentid.Substring(5);
        Dictionary<string, string> settings= new Dictionary<string, string>();
        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            if (key.StartsWith("DBFD_" + parentname + "_" + itemid + "_"))
            {
                string attributename = key.Substring(("DBFD_" + parentname + "_" + itemid + "_").Length);
                string val = ConfigurationManager.AppSettings[key];
                settings.Add(attributename, val);
            }
        }
        return (settings);
    }

    public static object ConverType(object value,  Type targettype)
    {
        if (targettype.Name.ToUpper().IndexOf("STRING") >= 0 || targettype.Name.ToUpper().IndexOf("CHAR")>=0)
            return (value.ToString());
        if (targettype.Name.ToUpper().IndexOf("INT64") >= 0)
        { 
            long outres = -1;
            if (Int64.TryParse(value.ToString(), out outres))
                return (outres);
        }
        if (targettype.Name.ToUpper().IndexOf("INT") >= 0)
        {
            Int32 outres2 = -1;
            if (Int32.TryParse(value.ToString(), out outres2))
                return (outres2);
        }
        if (targettype.Name.ToUpper().IndexOf("NUM") >= 0 || targettype.Name.ToUpper().IndexOf("DECI") >= 0 || targettype.Name.ToUpper().IndexOf("DOUB") >= 0 || targettype.Name.ToUpper().IndexOf("FLOAT") >= 0)
        {
            double outres3 = -1;
            if (double.TryParse(value.ToString(), out outres3))
                return (outres3);
        }

        if (targettype.Name.ToUpper().IndexOf("UNIT") >= 0)
        { 
            System.Web.UI.WebControls.Unit unt = Unit.Parse(value.ToString());
            return (unt);            
        }
        //颜色一律用16进制
        if (targettype.Name.ToUpper().IndexOf("COLOR") >= 0)
        {
            int L=0;
            if (int.TryParse(value.ToString(), out L))
            {
                System.Drawing.Color cl = System.Drawing.Color.FromArgb(int.Parse(value.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier));
                return (cl);
            }
            return (System.Drawing.Color.FromName(value.ToString()));
        }

        if (targettype.Name.ToUpper().IndexOf("DATE") >= 0)
        {
            if (value.ToString().IndexOf(":") > 0)
            {
                return (DateTime.ParseExact(value.ToString(), "yyyy/MM/dd HH:mm:ss", null));
            }
            else
            {
                return (DateTime.ParseExact(value.ToString(), "yyyy/MM/dd", null));
            }
        }

        if (targettype.Name.ToUpper().IndexOf("ENUM") >= 0 || targettype.IsEnum)
        {
            return Enum.Parse(targettype, value.ToString());
        }
        if (targettype.Name.ToUpper().IndexOf("BOOL") >= 0)
        {
            return  Boolean.Parse(value.ToString());
        }

        try
        {
            value = Convert.ChangeType(value, targettype);
        }
        catch { }

        return (value);
    }

    public static string Checkinput_Control(System.Web.UI.Control ctrlobject )
    {
        string rtnmsg = "";
           
        foreach (Control subcontrol in ctrlobject.Controls)
        {  
            rtnmsg += Checkinput_Control(subcontrol); 
        }
        if (ctrlobject.GetType().Name.IndexOf("Qry_") < 0)
            return (rtnmsg);
        if(!(((Iwebqryfield)(ctrlobject)).AllowNull) && (((Iwebqryfield)(ctrlobject)).GetFieldValue()==null) )
        {
            rtnmsg += (ctrlobject.ID??"") + "=null! ";
        }
        return (rtnmsg);

    }
    public static void initForm_Control(System.Web.UI.Control ctrlobject,string parentname)
    {
        string name = ctrlobject.ID;
        Type t = ctrlobject.GetType();
        string parentid= parentname;
        if (parentid.StartsWith("DBFD_"))
            parentid = parentid.Substring(5);
        
        Dictionary<string,string> attributes = GetFieldSetting(parentid, name);
        foreach (string key in attributes.Keys)
        {
            PropertyInfo pi = t.GetProperty(key,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
            {
                object propval = null;
                Type pitype = pi.PropertyType;
                try
                {   
                    propval = Convert.ChangeType(attributes[key], pitype);
                    pi.SetValue(ctrlobject, propval, null);
                    //pi.SetValue(ctrlobject, propval);
                }
                catch {
                    try
                    {
                        propval = ConverType(attributes[key], pitype);
                        pi.SetValue(ctrlobject, propval,null);
                        
                    }
                    catch { }
                }
                
                continue;
            }
            FieldInfo fi = t.GetField(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                object fieldval = null;
                Type fitype = fi.FieldType;
                try
                {
                    fieldval = Convert.ChangeType(attributes[key], fitype);
                    fi.SetValue(ctrlobject, fieldval);
                }
                catch
                {
                    try
                    {
                        fieldval = ConverType(attributes[key], fitype);
                        fi.SetValue(ctrlobject, fieldval);
                    }
                    catch { }
                }
                
                continue;

            }
        }
        initForm_FillControl(ctrlobject, parentid);
        foreach (Control subcontrol in ctrlobject.Controls)
        {
            
            initForm_Control(subcontrol, parentid);
         
        }
        string linkdb=GetAppSettingValue("Client", name, "DB", false);
        if(string.IsNullOrEmpty(linkdb))
            linkdb = GetAppSettingValue("Client", parentname, "DB", false);
        SetForm_ControlPropertyValue(ctrlobject, "LinkedDBname", linkdb, false);
        if (ctrlobject.GetType().Name.IndexOf("Qry_") >= 0 && ctrlobject.GetType().Name.ToUpper().IndexOf("DROP") >= 0 && ctrlobject.GetType().Name.ToUpper().IndexOf("LIST") >= 0)
        {
            ((Qry_DropDownList)(ctrlobject)).FetchSQLData();
            if (string.IsNullOrEmpty(((Qry_DropDownList)(ctrlobject)).LinkedChangeField))
                ((Qry_DropDownList)(ctrlobject)).AutoPostBack = false;
            WebUtils.Logger.Debug("followchange:" + ctrlobject.ID + ".enabled=" + ((Qry_DropDownList)(ctrlobject)).Enabled.ToString());
        }
        

    }

    public static void SetForm_QryFldProp(System.Web.UI.Control ctrlobject, string fieldname, string propname, object targetval)
    {
        if (ctrlobject.GetType().Name.IndexOf("Qry_") >= 0)
        {
            if (((Iwebqryfield)(ctrlobject)).LinkedDbFieldName == null)
                return;
            if (((Iwebqryfield)(ctrlobject)).LinkedDbFieldName.ToUpper() == fieldname.ToUpper())
            {
                if (propname == "")
                {
                    ((Iwebqryfield)(ctrlobject)).SetFieldValue(targetval);
                }
                else
                {
                    SetForm_ControlPropertyValue(ctrlobject, propname, targetval, false);
                }
                return;
            }
        }
        foreach (Control ctrlitem in ctrlobject.Controls)
        {
            SetForm_QryFldProp(ctrlitem, fieldname, propname, targetval);
        }
    }

    public static void SetForm_ControlPropertyValue(System.Web.UI.Control ctrlobject, string propname, object targetval, bool setsub)
    {
        string name = ctrlobject.ID;
        //WebUtils.Logger.Debug("set " + name + "." + propname + "=" + (targetval==null?"":targetval.ToString()));
        Type t = ctrlobject.GetType();
        string key = propname;
        object propval = targetval;
            PropertyInfo pi = t.GetProperty(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
            {
                Type pitype = pi.PropertyType;
                try
                {
                    pi.SetValue(ctrlobject, propval, null);
                    //pi.SetValue(ctrlobject, propval);
                }
                catch
                {
                    try
                    {
                        propval = ConverType(propval, pitype);
                        pi.SetValue(ctrlobject, propval, null);
                    }
                    catch { }
                }
            }
            FieldInfo fi = t.GetField(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                object fieldval = propval;
                Type fitype = fi.FieldType;
                try
                {
                    fieldval = Convert.ChangeType(propname, fitype);
                    fi.SetValue(ctrlobject, fieldval);
                }
                catch
                {
                    try
                    {
                        fieldval = ConverType(propval, fitype);
                        fi.SetValue(ctrlobject, fieldval);
                    }
                    catch { }
                }
                 
            }
            if (!setsub)
                return;
        foreach (Control subcontrol in ctrlobject.Controls)
        {
            SetForm_ControlPropertyValue(subcontrol,propname,  targetval,true);
        } 

    }

    private static void initForm_FillControl(System.Web.UI.Control ctrlobject, string sid)
    {
        string parentid = sid;
        if (parentid.StartsWith("DBFD_"))
            parentid = parentid.Substring(5);

        string dbname = GetAppSettingValue("Client" , parentid, "DB", false);
        //WebUtils.Logger.Debug(parentid + ".GlobalDB=" + (dbname==null?"":dbname));
        if (dbname == null || dbname == "")
            dbname = "default";

        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            // found item
            if (key.StartsWith("DBFD_" + parentid + "_") && key.EndsWith("_ID"))
            {
                string objname =  ConfigurationManager.AppSettings[key];
                string upparentid = GetAppSettingValue("DBFD_" + parentid, objname, "ParentID", false);
                if (upparentid != "" && upparentid==ctrlobject.ID)
                {
                    System.Web.UI.Control parentctrl = ctrlobject.FindControl(upparentid);
                    if (parentctrl != null)
                    {
                        if (parentctrl.FindControl(objname) == null)
                        {
                            string typename = GetAppSettingValue("DBFD_" + parentid, objname, "Type", false);
                            /*
                             * Assembly asm = Assembly.GetExecutingAssembly();           
                            Object obj = asm.CreateInstance("Reflection4.Calculator", true);
                            // 输出：Calculator() invoked
                            ObjectHandle handler = Activator.CreateInstance(null, "Reflection4.Calculator");
                            Object obj = handler.Unwrap();
                             * */
                            Control subobj = initForm_CreateCustomObj(typename, objname);
                            if (subobj == null)
                            {
                                WebUtils.Logger.Debug("Create control:" + typename + ", name=" + objname + " failed");
                                continue;
                            }
                            /*
                            if (parentctrl.GetType().Name.ToUpper().IndexOf("CELL") >= 0)
                            {
                                PropertyInfo pi = parentctrl.GetType().GetProperty("Width");
                                pi.SetValue(subobj, ((TableCell)(parentctrl)).Width);
                            }
                            */
                            if (subobj.GetType().Name.IndexOf("Qry_") >= 0)
                            {
                                if(string.IsNullOrEmpty(((Iwebqryfield)subobj).LinkedDBname))
                                ((Iwebqryfield)subobj).LinkedDBname = dbname;
                                //WebUtils.Logger.Debug("Create control:" + typename + ", name=" + objname + ".db=" + ((Iwebqryfield)subobj).LinkedDBname);
                            }
                            //subobj.Parent = parentctrl;
                            parentctrl.Controls.Add(subobj);
                        }
                    }
                }
            }
        }
    }

    private static void initItemSetting(System.Xml.XmlNode thissqlnode, string prefix)
    {
        if (thissqlnode == null)
            return;

        if (thissqlnode.Name.StartsWith("#"))
            return;

        string fieldname = thissqlnode.Name;
        if (thissqlnode.Attributes["Name"] != null)
            fieldname = thissqlnode.Attributes["Name"].Value;
        if (thissqlnode.Attributes["ID"] != null)
            fieldname = thissqlnode.Attributes["ID"].Value;
        foreach (System.Xml.XmlAttribute attr in thissqlnode.Attributes)
        {
            ConfigurationManager.AppSettings[prefix + fieldname + "_" + attr.Name] = attr.Value;
        }
        ConfigurationManager.AppSettings[prefix + fieldname + "_ID"] = fieldname;
        // APSetting格式： DBFD_parentid_fieldname_settingname, settingname 可以是：datatype, width, displaywidth, caption, fontname, forecolor, backcolor, varname, inputtype(text,combo,datetime,file),...

    }

    private static void initClientSetting(System.Xml.XmlNode thissqlnode)
    {
        if (thissqlnode == null)
            return;

        string sqlid = thissqlnode.Name;
        if (thissqlnode.Attributes == null)
            return;

        if (thissqlnode.Attributes["Name"] != null)
            sqlid = thissqlnode.Attributes["Name"].Value;
        if (thissqlnode.Attributes["ID"] != null)
            sqlid = thissqlnode.Attributes["ID"].Value;


        initItemSetting(thissqlnode,  "Client_");
        foreach (System.Xml.XmlNode subnode in thissqlnode.ChildNodes)
        {
            initItemSetting(subnode, "DBFD_"+sqlid+"_");
        }
    }

    static object lockinitsql = new object();
    public static void initSQL(string sqlid)
    {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        //if (!System.IO.File.Exists("DBCLientSettings.XML"))
       //     return; 
        string path = (HttpRuntime.BinDirectory.Substring(0,HttpRuntime.BinDirectory.Length-4)+ "DBCLientSettings.XML");
        if(System.IO.File.Exists(HttpRuntime.BinDirectory.Substring(0,HttpRuntime.BinDirectory.Length-4)+ "DBCLientSettings_"+WebUtils.GetSysLang()+".XML"))
        {
            path = HttpRuntime.BinDirectory.Substring(0, HttpRuntime.BinDirectory.Length - 4) + "DBCLientSettings_" + WebUtils.GetSysLang() + ".XML";
        }
        doc.Load(path);
       
        if (sqlid != null && sqlid.Trim() != "")
        {
            System.Xml.XmlNode thissqlnode = doc.SelectSingleNode("descendant::Client/"+sqlid);
            lock (lockinitsql)
            initClientSetting(thissqlnode);
        }
        else
        {
            System.Xml.XmlNode sqlnode = doc.SelectSingleNode("descendant::Client");
            if (sqlnode == null)
                return;

            foreach (System.Xml.XmlNode subnode in sqlnode.ChildNodes)
            {
                initClientSetting(subnode);
            }
        }
    }

    public static void Logmsg(string message)
    {
        Logger.Debug(message);
    }
    static Log _log = null;
    public static Log Logger{
        get
        {
            if (HttpContext.Current.Application["Logger"] != null)
                return ((Log)(HttpContext.Current.Application["Logger"]));
     

            if (_log == null)
            {
                initLog(); 
            }

            HttpContext.Current.Application["Logger"] = _log;

            return (_log);
        }
        set
        {
            _log = value;
        }
    }
    
    static void initLog()
    {
        LogFactory.StartLog("WebRunLog", "WebRunLog.txt");
        Logger = LogFactory.GetLog("WebRunLog");
    }

    static string ConainerServerConfigID = "ConainerServerId";
    public static string GetContainerServerID()
    {
        if (ConfigurationManager.AppSettings["ConainerServerId"] == null)
            return ("Container1");
        return (ConfigurationManager.AppSettings[ConainerServerConfigID]);
    }

    public static string GetAppSettingValue(string sectionname,string key,string attributename,bool refreshed)
    {
        try
        {
            if (refreshed)
                ConfigurationManager.RefreshSection("AppSettings");
            string fullkeyname = "";

            if (sectionname != null && sectionname != "")
            {
                fullkeyname = sectionname;
            }
            if (key != null && key != "")
            {
                fullkeyname = (fullkeyname == "" ? key : fullkeyname + "_" + key);
            }
            if (attributename != null && attributename != "")
            {
                fullkeyname = (fullkeyname == "" ? attributename : fullkeyname + "_" + attributename);
            }

            return (ConfigurationManager.AppSettings[fullkeyname]);
        }
        catch (Exception e)
        {
            Logger.Debug("GetSettingkey:" + sectionname + "." + key + "." + attributename + "failure, Err:" + e.Message);
        }
        return ("");
    }
    static void initDB()
    {
        foreach (string appkey in ConfigurationManager.AppSettings.AllKeys)
        {
            if (appkey.StartsWith("DB_"))
            {
                string dbname = appkey.Substring(3);
                string dbtype = ConfigurationManager.AppSettings[appkey];
                string dbconnstring = ConfigurationManager.ConnectionStrings[appkey + "_connString"].ToString();
                try
                {
                    WebUtils.DB.RegisterDB(dbname, dbtype, dbconnstring);
                }
                catch { };
            }
        }
    }

    public static void SetDropDownListSelectIndex(DropDownList ddl, string value)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (ddl.Items[i].Value == value)
            {
                ddl.SelectedIndex = i;
                return;
            }
        }
    }

}
