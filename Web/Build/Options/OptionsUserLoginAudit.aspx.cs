using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class OptionsUserLoginAudit : PhoenixBasePage
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
            MenuRemoteUser.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../Options/OptionsUserLoginAudit.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvuserLoginAudit')", "Print Grid", "icon_print.png", "PRINT");
            MenuuserLoginAudit.AccessRights = this.ViewState;
            MenuuserLoginAudit.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvuserLoginAudit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

    protected void txtUser_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDUSERNAME", "FLDCLIENTIP", "FLDACCESSSEDON", "FLDSUCCESSYN", "FLDATTEMPTCOUNT" };
            string[] alCaptions = { "User Name", "Client IP", "Accessed On", "Success", "Attempt Count" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

            DataSet ds = PhoenixGeneralSettings.UserLogAuditSearch(
                             null
                            , General.GetNullableInteger(usercode)
                             , sortexpression, sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                             , gvuserLoginAudit.PageSize
                             , ref iRowCount
                             , ref iTotalPageCount);
            General.ShowExcel("User Login Audit", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuuserLoginAudit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                // ShowExcel();
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

        string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

        string[] alColumns = { "FLDUSERNAME", "FLDCLIENTIP", "FLDACCESSSEDON", "FLDSUCCESSYN", "FLDATTEMPTCOUNT" };
        string[] alCaptions = { "User Name", "Client IP", "Accessed On", "Success", "Attempt Count" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixGeneralSettings.UserLogAuditSearch(
                         General.GetNullableString(txtUser.Text)
                         , General.GetNullableInteger(usercode)
                         , sortexpression, sortdirection
                         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                         , gvuserLoginAudit.PageSize
                         , ref iRowCount
                         , ref iTotalPageCount);


        General.SetPrintOptions("gvuserLoginAudit", "User Login Audit", alCaptions, alColumns, ds);

        gvuserLoginAudit.DataSource = ds;
        gvuserLoginAudit.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvuserLoginAudit_Sorting(object sender, GridViewSortEventArgs se)
    {
       
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

  
  
    protected void gvuserLoginAudit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName == "Page")
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

    protected void gvuserLoginAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvuserLoginAudit.CurrentPageIndex + 1;
        BindData();
    }
}
