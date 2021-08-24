using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class DocumentManagementDashBoardUserUnread : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementDashBoardUserUnread.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvUsers')", "Print Grid", "icon_print.png", "PRINT");
            MenuUnread.AccessRights = this.ViewState;
            MenuUnread.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = "";
                ViewState["OFFYN"] = 0;

                ViewState["COMPANYID"] = "";

                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");

                if (Request.QueryString["OFFYN"] != null && Request.QueryString["OFFYN"].ToString() != "")
                    ViewState["OFFYN"] = Request.QueryString["OFFYN"].ToString();

                gvUsers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvUsers.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            //string a = null;
            DataSet ds = new DataSet();

            string[] alColumns = { "FLDEMPLOYEECODE", "FLDNAME", "FLDDEPARTMENTNAME", "FLDGROUPRANK", "FLDUNREADCOUNT" };
            string[] alCaptions = { "Code", "Name", "Department", "Grouprank / Designation", "Unread Section Count" };

            if (ViewState["OFFYN"] != null && ViewState["OFFYN"].ToString()=="1")
            {
                ds = PhoenixDocumentManagementDashBoard.OfficeStaffUnreadSearch(General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                   , gvUsers.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount);

                gvUsers.Columns[2].Visible = true;
                gvUsers.Columns[0].HeaderText = "Code";
                gvUsers.Columns[1].HeaderText = "Name";
                gvUsers.Columns[3].HeaderText = "Designation";
            }
            else
            {
                ds = PhoenixDocumentManagementDashBoard.CrewMemberUnreadSearch(General.GetNullableInteger(null)
                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvUsers.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

                gvUsers.Columns[2].Visible = false;
            }

            General.SetPrintOptions("gvUsers", "UnreadUser", alCaptions, alColumns, ds);

            gvUsers.DataSource = ds;
            gvUsers.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDEMPLOYEECODE", "FLDNAME", "FLDDEPARTMENTNAME", "FLDGROUPRANK","FLDUNREADCOUNT" };
            string[] alCaptions = { "Code", "Name", "Department", "Grouprank / Designation","Unread Section Count" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (ViewState["OFFYN"] != null && ViewState["OFFYN"].ToString() == "1")
            {
                ds = PhoenixDocumentManagementDashBoard.OfficeStaffUnreadSearch(General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                   , 1
                   , iRowCount
                   , ref iRowCount
                   , ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixDocumentManagementDashBoard.CrewMemberUnreadSearch(General.GetNullableInteger(null)
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

            }

            General.ShowExcel("Unread User List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvUsers_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void gvUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUsers.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuUnread_TabStripCommand(object sender, EventArgs e)
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}