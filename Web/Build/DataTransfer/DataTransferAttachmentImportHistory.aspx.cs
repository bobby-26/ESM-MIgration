using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class DataTransferAttachmentImportHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Export", "EXPORT", ToolBarDirection.Right);
            toolbar.AddButton("Import", "IMPORT", ToolBarDirection.Right);
            MenuExportVesselList.Title = "Data Synchronizer - Import History [" + Request.QueryString["vesselid"].ToString() + "]";
            MenuExportVesselList.MenuList = toolbar.Show();
            MenuExportVesselList.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvVesselImportList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    protected void gvVesselImportList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
        }
    }
    protected void gvVesselImportList_RowEditing(object sender, GridCommandEventArgs e)
    {
    }

    protected void gvVesselImportList_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("IMPORT"))
            {
                RadLabel lblTransferCode = (RadLabel)e.Item.FindControl("lblTransferCode");
                PhoenixDataTransferImport.DataImportJob(short.Parse(Request.QueryString["vesselid"].ToString()), General.GetNullableGuid(lblTransferCode.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindData()
    {
        try
        {
                       
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixDataTransferImport.AttachmentTransferImportHistory(Convert.ToInt16(Request.QueryString["vesselid"].ToString()), sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvVesselImportList.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            gvVesselImportList.DataSource = ds;
            gvVesselImportList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselImportList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ExportVesselList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXPORT"))
            {
                Response.Redirect("../DataTransfer/DataTransferAttachmentExportHistory.aspx?vesselid=" + Request.QueryString["vesselid"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
