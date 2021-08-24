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


public partial class DocumentManagementDashboardUserUnreadDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDashboardUserUnreadDocuments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentSection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuUnread.AccessRights = this.ViewState;
            MenuUnread.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = "";

                gvDocumentSection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDAPPROVEDDATE", "FLDUNREADCOUNT" };
            string[] alCaptions = { "Document Section Number", "Document Section Name", "Approved Date", "Unread Count" };

            ds = PhoenixDocumentManagementDashBoard.DocumentVesselLoginUnreadDocument(gvDocumentSection.CurrentPageIndex + 1
                  , gvDocumentSection.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount);

            General.SetPrintOptions("gvDocumentSection", "UnReadDocumentSection", alCaptions, alColumns, ds);

            gvDocumentSection.DataSource = ds;
            gvDocumentSection.VirtualItemCount = iRowCount;
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

            string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDAPPROVEDDATE", "FLDUNREADCOUNT" };
            string[] alCaptions = { "Document Section Number", "Document Section Name", "Approved Date", "Unread Count" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixDocumentManagementDashBoard.DocumentVesselLoginUnreadDocument(1
                  , iRowCount
                  , ref iRowCount
                  , ref iTotalPageCount);

            General.ShowExcel("Unread Document List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvDocumentSection_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentSection.CurrentPageIndex + 1;
            BindData();
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
        gvDocumentSection.Rebind();
    }

    protected void gvDocumentSection_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

    protected void gvDocumentSection_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                LinkButton lnkSectionName = (LinkButton)item.FindControl("lnkSectionName");
                RadLabel lblDocumentid = (RadLabel)item.FindControl("lblDocumentid");
                RadLabel lblSectionid = (RadLabel)item.FindControl("lblSectionid");

                if (lnkSectionName != null)
                    lnkSectionName.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp2','','DocumentManagement/DocumentManagementDocumentSectionNewContentView.aspx?DOCUMENTID=" + lblDocumentid.Text + "&SECTIONID=" + lblSectionid.Text + "'); return false;");

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