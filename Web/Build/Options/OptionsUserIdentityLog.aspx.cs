using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class OptionsUserIdentityLog : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();



            toolbarmain.AddButton("Login Audit", "LOGINAUDIT", ToolBarDirection.Right);
            toolbarmain.AddButton("Identity Log", "IDENTITYLOG", ToolBarDirection.Right);
            toolbarmain.AddButton("User Identity", "USERIDENTITY", ToolBarDirection.Right);

            MenuRemoteUser.AccessRights = this.ViewState;
            MenuRemoteUser.MenuList = toolbarmain.Show();
            MenuRemoteUser.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsUserIdentityLog.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddImageButton("../Options/OptionsUserIdentityLog.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvUseridentityLog')", "Print Grid", "icon_print.png", "PRINT");
            MenuuserIdentityLog.AccessRights = this.ViewState;
            MenuuserIdentityLog.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                // gvUseridentityLog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["usercode"] != null)
                {
                    ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
                }
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            //int iTotalPageCount = 0;


            string[] alColumns = { "FLDUID", "FLDMACADDRESS", "FLDIPADDRESS", "FLDMACHINE", "FLDREMOTEADDR", "FLDCREATEDDATE" };
            string[] alCaptions = { "UID", "Mac Address", "IP Address", "machine", "Remote Address", "Created Date" };
            
            int? sortdirection = null;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

            //DataSet ds = PhoenixGeneralSettings.UserIdentityLogSearch(
            //                 null
            //                 , General.GetNullableInteger(usercode)
            //                 , sortexpression, sortdirection
            //                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                 , gvUseridentityLog.PageSize
            //                 , ref iRowCount
            //                 , ref iTotalPageCount);
            if (txtUID.Text != "")
            {
                DataSet ds = PhoenixGeneralSettings.UserIdentityLoginSearch(txtUID.Text);
                General.ShowExcel("User Identity Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuuserIdentityLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    protected void MenuRemoteUser_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

        if (CommandName.ToUpper().Equals("USERIDENTITY"))
        {
            Response.Redirect("../Options/OptionsUserIdentity.aspx?usercode=" + usercode);
        }
        else if (CommandName.ToUpper().Equals("IDENTITYLOG"))
        {
            Response.Redirect("../Options/OptionsUserIdentityLog.aspx?usercode=" + usercode);
        }
        else
        {
            Response.Redirect("../Options/OptionsUserLoginAudit.aspx?usercode=" + usercode);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDUID", "FLDMACADDRESS", "FLDIPADDRESS", "FLDMACHINE", "FLDREMOTEADDR", "FLDCREATEDDATE" };
        string[] alCaptions = { "UID", "Mac Address", "IP Address", "machine", "Remote Address", "Created Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

        if (!IsValidIdentity(txtUID.Text))
        {
            ucError.Visible = true;
            return;
        }
        //DataSet ds = PhoenixGeneralSettings.UserIdentityLogSearch(
        //                 null
        //                 , General.GetNullableInteger(usercode)
        //                 , sortexpression, sortdirection
        //                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
        //                 , gvUseridentityLog.PageSize
        //                 , ref iRowCount
        //                 , ref iTotalPageCount);
        if (txtUID.Text != "")
        {
            DataSet ds = PhoenixGeneralSettings.UserIdentityLoginSearch(txtUID.Text);

            General.SetPrintOptions("gvUseridentityLog", "User Identity Log", alCaptions, alColumns, ds);

            gvUseridentityLog.DataSource = ds;
            gvUseridentityLog.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }

    private bool IsValidIdentity(string uid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (uid.Trim() != "")
        {
            if (uid.Trim().Length < 3)
                ucError.ErrorMessage = "Enter minimum 3 charaecter.";
        }

        return (!ucError.IsError);
    }

    protected void gvUseridentityLog_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvUseridentityLog_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUseridentityLog.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvUseridentityLog_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
