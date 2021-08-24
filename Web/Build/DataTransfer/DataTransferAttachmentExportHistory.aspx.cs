using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DataTransferAttachmentExportHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Export", "EXPORT", ToolBarDirection.Right);
            toolbar.AddButton("Import", "IMPORT", ToolBarDirection.Right);
            MenuExportVesselList.Title = "Data Synchronizer - Export History [" + Request.QueryString["vesselid"].ToString() + "]";
            MenuExportVesselList.MenuList = toolbar.Show();
            MenuExportVesselList.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
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

            if (CommandName.ToUpper().Equals("IMPORT"))
            {
                Response.Redirect("../DataTransfer/DataTransferAttachmentImportHistory.aspx?vesselid=" + Request.QueryString["vesselid"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselExportList_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }
    protected void gvVesselExportList_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void gvVesselExportList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvVesselExportList_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;


            if (e.CommandName.ToUpper().Equals("EXPORT"))
            {
                RadLabel lblTransferCode = (RadLabel)e.Item.FindControl("lblTransferCode");
                PhoenixDataTransferExport.AttachmentExportJob(short.Parse(Request.QueryString["vesselid"].ToString()), General.GetNullableGuid(lblTransferCode.Text));
            }
            else if (e.CommandName.ToUpper().Equals("DELETEFOLDER"))
            {

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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDataTransferExport.AttachmentTransferExportHistory(Convert.ToInt16(Request.QueryString["vesselid"].ToString()), sortexpression, sortdirection, gvVesselExportList.CurrentPageIndex + 1,
                    gvVesselExportList.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvVesselExportList.DataSource = ds;
        gvVesselExportList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
