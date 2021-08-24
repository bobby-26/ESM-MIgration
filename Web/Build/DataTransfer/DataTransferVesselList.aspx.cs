using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Registers_DataTransferVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblGuidance.Text = "You will see your vessel name if you are in ship OR all your vessels if you are in office in the scroll list below.";
            lblGuidance.Text = lblGuidance.Text + "\nThe first two buttons in the action column allows you to import and export data";
            lblGuidance.Text = lblGuidance.Text + "\nClick on the import button to accept data changes from office/ship.";
            lblGuidance.Text = lblGuidance.Text + "\nClick on the export button to send data changes to office/ship.";
            lblGuidance.Attributes.Add("style", "overflow :hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Filter.CurrentVesselCriteria = null;

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("EPSS", "EPSS", ToolBarDirection.Right);
            toolbar.AddButton("Vessel List", "VESSELLIST", ToolBarDirection.Right);
            MenuVesselList.MenuList = toolbar.Show();
            MenuVesselList.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../DataTransfer/DataTransferVesselList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvVesselList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            toolbar1.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Filter','DataTransfer/DataTransferVesselFilter.aspx')", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../DataTransfer/DataTransferVesselList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar1.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Data Synchronizer Import','DataTransfer/DataTransferExportFileUpload.aspx')", "Upload Export File", "<i class=\"fas fa-upload\"></i>", "UPLOAD");

            MenuFilter.AccessRights = this.ViewState;
            MenuFilter.MenuList = toolbar1.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    protected void gvVesselList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            RadLabel vesselid = ((RadLabel)e.Item.FindControl("lblVesselId"));
            if (vesselid != null)
            {
                LinkButton jobs = (LinkButton)e.Item.FindControl("cmdJobs");
                if (jobs != null)
                    jobs.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Data Synchronizer - Scheduled Jobs', '" + Session["sitepath"] + "/DataTransfer/DataTransferJobs.aspx?vesselid=" + vesselid.Text + "');return true;");

                LinkButton history = (LinkButton)e.Item.FindControl("cmdHistory");
                if (history != null)
                    history.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Data Synchronizer - Import History', '" + Session["sitepath"] + "/DataTransfer/DataTransferImportHistory.aspx?vesselid=" + vesselid.Text + "');return true;");

                LinkButton attachmenthistory = (LinkButton)e.Item.FindControl("cmdAttachmentHistory");
                if (attachmenthistory != null)
                    attachmenthistory.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Attachment Synchronizer - Import History', '" + Session["sitepath"] + "/DataTransfer/DataTransferAttachmentImportHistory.aspx?vesselid=" + vesselid.Text + "');return true;");

                LinkButton importerror = (LinkButton)e.Item.FindControl("cmdImportLog");
                if (importerror != null)
                    importerror.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Data Synchornizer - Error Description', '" + Session["sitepath"] + "/DataTransfer/DataTransferErrorHandler.aspx?vesselid=" + vesselid.Text + "');return true;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName.ToUpper().Equals("IMPORT"))
            {
                PhoenixDataTransferImport.DataImportJob(short.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text), null);
                PhoenixDataTransferImport.AttachmentImportJob(short.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text), null);
                ucStatus.Text = "Data/Attachment Import job scheduled";
            }
            else if (e.CommandName.ToUpper().Equals("EXPORT"))
            {
                PhoenixDataTransferExport.DataExportJob(short.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text), null);
                PhoenixDataTransferExport.AttachmentExportJob(short.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text), null);
                ucStatus.Text = "Data/Attachment Export job scheduled";
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Session["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                Session["VESSELNAME"] = ((LinkButton)e.Item.FindControl("lnkVesselName")).Text;
                MenuVesselList.SelectedMenuIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void VesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Session["VESSELID"] == null)
        {
            if ((CommandName.ToUpper().Equals("IMPORT")) || (CommandName.ToUpper().Equals("EXPORT")))
            {
                ucError.HeaderMessage = "Navigation Error";
                ucError.ErrorMessage = "Please select a Vessel and navigate to Other page.";
                ucError.Visible = true;

                MenuVesselList.SelectedMenuIndex = 0;
                return;
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("IMPORT"))
            {
                Response.Redirect("../DataTransfer/DataTransferImportHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("EXPORT"))
            {
                Response.Redirect("../DataTransfer/DataTransferExportHistory.aspx");
            }
        }
        if (CommandName.ToUpper().Equals("EPSS"))
        {
            Response.Redirect("DataTranserEPSSImport.aspx", true);
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvVesselList_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }
    protected void gvVesselList_SortCommand(object sender, GridCommandEventArgs se)
    {

    }

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDEXPORTDATE", "FLDIMPORTDATE" };
        string[] alCaptions = { "Vessel Name", " Last Export Date", "Last Import Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        if (Filter.CurrentVesselCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentVesselCriteria;
            ds = PhoenixDataTransferExport.DataTransferVesselList(General.GetNullableString(nvc.Get("txtVesselName").ToString()), General.GetNullableString(nvc.Get("ddlVesselType").ToString()), General.GetNullableString(nvc.Get("ucAddrOwner").ToString()),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(nvc.Get("ucTechFleet").ToString()));
        }
        else
            ds = PhoenixDataTransferExport.DataTransferVesselList(null, null, null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DataSynchronizerVesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Data Synchronizer - Vessel List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDEXPORTDATE", "FLDIMPORTDATE" };
        string[] alCaptions = { "Vessel Name", " Last Export Date", "Last Import Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        if (Filter.CurrentVesselCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentVesselCriteria;
            ds = PhoenixDataTransferExport.DataTransferVesselList(General.GetNullableString(nvc.Get("txtVesselName").ToString()), General.GetNullableString(nvc.Get("ddlVesselType").ToString()), General.GetNullableString(nvc.Get("ucAddrOwner").ToString()),
                sortexpression, sortdirection,
                gvVesselList.CurrentPageIndex + 1, gvVesselList.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(nvc.Get("ucTechFleet").ToString()));
        }
        else
            ds = PhoenixDataTransferExport.DataTransferVesselList(null, null, null, sortexpression, sortdirection, gvVesselList.CurrentPageIndex + 1, gvVesselList.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVesselList", "Data Synchronizer - Vessel List", alCaptions, alColumns, ds);

        gvVesselList.DataSource = ds;
        gvVesselList.VirtualItemCount = iRowCount;

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

    protected void MenuFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvVesselList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentVesselCriteria = null;
                gvVesselList.Rebind();
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
}
