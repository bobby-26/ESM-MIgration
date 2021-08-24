using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


public partial class PlannedMaintenance_PlannedMaintenancePTWForms : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
                {
                    ViewState["CATEGORYID"] = Request.QueryString["cid"];
                }
                ViewState["WOID"] = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["woid"]))
                {
                    ViewState["WOID"] = Request.QueryString["woid"];
                }
                gvForms.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            SessionUtil.PageAccessRights(this.ViewState);
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuExportForms_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
        if (CommandName.ToUpper().Equals("EXPORT"))
        {
            
        }
    }

    protected void gvForms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDFILENAME", "FLDREVISIONNUMBER", "FLDPURPOSE" };
        string[] alCaptions = { "Category", "File Name", "Revision", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroupExtn.ListDocumentByCategory(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvForms.CurrentPageIndex + 1
                                                                , gvForms.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        gvForms.DataSource = ds;
        gvForms.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvForms_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string formid = ((RadLabel)e.Item.FindControl("lblFileId")).Text; ;
                PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderPtwInsert(new Guid(ViewState["WOID"].ToString()), new Guid(formid));
                string script = "CloseUrlModelWindow();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvForms_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
           
        }
    }
}