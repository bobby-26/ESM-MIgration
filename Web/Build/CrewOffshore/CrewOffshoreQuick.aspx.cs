using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshoreQuick : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreQuick.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuOffshoreQuick.AccessRights = this.ViewState;
            MenuOffshoreQuick.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QuickCodeType"] = null;
                string module = Request.QueryString["module"].ToString();
                ucQuickType.QuickTypeGroup = module;
                if (Request.QueryString["quickcodetype"] != null)
                {
                    ViewState["QuickCodeType"] = Request.QueryString["quickcodetype"].ToString();
                    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                }
                if (Request.QueryString["quickcodetype"] != null)
                {
                    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                    ucQuickType.QuickTypeShowYesNo = "0";
                    CaptionChange(module, ViewState["QuickCodeType"].ToString());
                    ucQuickType.Visible = false;
                    lblRegister.Visible = false;

                }
                else
                {
                    ucQuickType.bind();
                }
                gvQuick.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindFirstValue();
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindFirstValue()
    {
        string quicktype = null;
        if (ViewState["QuickCodeType"] == null || ViewState["QuickCodeType"].ToString() == "")
        {
            if (ucQuickType.SelectedQuickType == "")
            {
                ucQuickType.QuickTypeShowYesNo = "1";
                string yesno = ucQuickType.QuickTypeShowYesNo;
                DataSet ds1 = PhoenixCrewOffshoreQuick.ListQuickType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ucQuickType.QuickTypeGroup, Convert.ToInt32(yesno));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow drActivity = ds1.Tables[0].Rows[0];
                    quicktype = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.SelectedQuickType = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.bind();
                    ViewState["QuickCodeType"] = drActivity["FLDQUICKTYPECODE"].ToString();
                }
            }
        }
    }
    public void CaptionChange(string module, string quicktypecode)
    {
        ucQuickType.Enabled = "false";
        DataSet dsedit = new DataSet();
        dsedit = PhoenixCrewOffshoreQuick.ListQuickTypeEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, module,
                                                        Convert.ToInt32(quicktypecode));

        if (dsedit.Tables.Count > 0)
        {
            DataRow drquick = dsedit.Tables[0].Rows[0];
            ucTitle.Text = General.GetMixedCase(drquick["FLDQUICKTYPENAME"].ToString());
            ViewState["MODULENAME"] = ucTitle.Text;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        //string[] alCaptions = new string[1];
        //string[] alColumns = { "FLDQUICKNAME" };
        //alCaptions[1] = "Name";
        string[] alColumns = { "FLDSHORTNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreQuick.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    gvQuick.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ViewState["MODULENAME"] != null)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ViewState["MODULENAME"].ToString() + ".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + ViewState["MODULENAME"].ToString() + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
        else
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"Other Register.xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Other Register</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }
    protected void OffshoreQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvQuick.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreQuick.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                 (int)ViewState["PAGENUMBER"],
                gvQuick.PageSize,
                 ref iRowCount,
                 ref iTotalPageCount);

        General.SetPrintOptions("gvQuick", "Other Register", alCaptions, alColumns, ds);

        gvQuick.DataSource = ds;
        gvQuick.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;        
    }
   
    protected void gvQuick_Sorting(object sender, GridSortCommandEventArgs se)
    {
       
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    
    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertQuick(
                (ViewState["QuickCodeType"].ToString() == "" || ViewState["QuickCodeType"] == null) ? ucQuickType.SelectedQuickType : ViewState["QuickCodeType"].ToString(),
                    ((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text);
                BindData();
                gvQuick.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteQuick(Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCode")).Text));
                BindData();
                gvQuick.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
                ((LinkButton)e.Item.FindControl("lnkShortName")).Focus();
            
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtQuickNameEdit")).Text, ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateQuick( (ViewState["QuickCodeType"].ToString() == "" || ViewState["QuickCodeType"] == null) ? ucQuickType.SelectedQuickType : ViewState["QuickCodeType"].ToString(),
                             Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCodeEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtQuickNameEdit")).Text,
                             ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text);
                BindData();
                gvQuick.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void gvQuick_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


        if (e.Item is GridHeaderItem)
        {
            if (Request.QueryString["quickcodetype"] != null)
            {
                if (Request.QueryString["quickcodetype"].ToString() == "64")
                {
                    //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                    //LinkButton lnkCode = (LinkButton)e.Row.FindControl("lnkQuickCodeHeader");
                    //if (lnkCode != null) lnkCode.Text = "Cancel Code";

                    LinkButton lnkName = (LinkButton)e.Item.FindControl("lblQuickNameHeader");
                    if (lnkName != null) lnkName.Text = "Cancel Reason";
                }

                else if (Request.QueryString["quickcodetype"].ToString() == "65")
                {
                    //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                    //LinkButton lnkCode = (LinkButton)e.Row.FindControl("lnkQuickCodeHeader");
                    //if (lnkCode != null) lnkCode.Text = "Approval Code";

                    LinkButton lnkName = (LinkButton)e.Item.FindControl("lblQuickNameHeader");
                    if (lnkName != null) lnkName.Text = "Approval Authority";
                }
            }
        }
            
        if (e.Item is GridDataItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["QuickCodeType"] = ucQuickType.SelectedQuickType;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvQuick.Rebind();
    }
   
    private void InsertQuick(string Quicktypecode, string Quickname, string Shortname)
    {

        PhoenixCrewOffshoreQuick.InsertQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickname, Shortname);
    }
    private void UpdateQuick(string Quicktypecode, int Quickcode, string Quickname, string shortname)
    {
        PhoenixCrewOffshoreQuick.UpdateQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickcode, Quickname, shortname);        
    }
    
    private bool IsValidQuick(string Quickname, string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortname.Trim().Equals(""))
            ucError.ErrorMessage = "Short Code is required.";

        if (Quickname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        return (!ucError.IsError);
    }
    private void DeleteQuick(int Quickcode)
    {
        PhoenixCrewOffshoreQuick.DeleteQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Quickcode);
    }

    protected void gvQuick_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuick.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
