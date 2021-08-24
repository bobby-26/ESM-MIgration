using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class DataTransfer_DataTransferScheduleVessels : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblGuidance.Text = "You will a list of vessels if you are in office in the scroll list below.";
        lblGuidance.Text = lblGuidance.Text + "\nCheck the checkbox in column Active Y/N to enable data export. Clear the checkbox to disable.";
        lblGuidance.Text = lblGuidance.Text + "\nCheck the checkbox in column Sync. Attachments Y/N to enable attachment export. Clear the checkbox to disable";
        lblGuidance.Text = lblGuidance.Text + "\nSync Attachment is by default set to \"No\" (Checkbox is cleared).";
        lblGuidance.Attributes.Add("style", "overflow :hidden");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["VesselName"] = null;
            gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../DataTransfer/DataTransferScheduleVessels.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvVesselList')", "Print Grid", "icon_print.png", "Print");
        //toolbargrid.AddImageLink("../DataTransfer/DataTransferScheduleVessels.aspx", "Filter", "search.png", "FIND");
        //toolbargrid.AddImageButton("../DataTransfer/DataTransferScheduleVessels.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

        MenuDataSynchronizer.AccessRights = this.ViewState;
        MenuDataSynchronizer.MenuList = toolbargrid.Show();
        BindData();

    }

    protected void MenuDataSynchronizer_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            //if (CommandName.ToUpper().Equals("FIND"))
            //{
            //    ViewState["PAGENUMBER"] = 1;
            //    //ViewState["VesselName"] = txtvesselName.Text;                
            //    BindData();

            //}
            //else if (CommandName.ToUpper().Equals("CLEAR"))
            //{
            //    txtvesselName.Text = "";
            //    ucTechFleet.SelectedFleet = "";
            //    //ViewState["VesselName"] = txtvesselName.Text;
            //    BindData();

            //}
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

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDACTIVE", "FLDATTACHMENTTRANSFER", "FLDDOCUMENTTANSFER" };
        string[] alCaptions = { "Vessel Name", " Active Y/N", "Sync. Attachments Y/N", "Sync. Documents Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixDataTransferCommon.DataTransfersScheduledVessels(General.GetNullableString(txtvesselName.Text),
            sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVesselList.PageSize,
        ref iRowCount,
        ref iTotalPageCount,
        General.GetNullableInteger(ucTechFleet.SelectedFleet));

        Response.AddHeader("Content-Disposition", "attachment; filename=SynchronizerSetting.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Synchronizer Setting</h3></td>");
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

        string[] alColumns = { "FLDVESSELNAME", "FLDACTIVE", "FLDATTACHMENTTRANSFER", "FLDDOCUMENTTANSFER" };
        string[] alCaptions = { "Vessel Name", " Active Y/N", "Sync. Attachments Y/N", "Sync. Documents Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDataTransferCommon.DataTransfersScheduledVessels(General.GetNullableString(txtvesselName.Text),
            sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVesselList.PageSize,
        ref iRowCount,
        ref iTotalPageCount,
        General.GetNullableInteger(ucTechFleet.SelectedFleet));


        General.SetPrintOptions("gvVesselList", "Synchronizer Setting", alCaptions, alColumns, ds);
        gvVesselList.DataSource = ds;
        gvVesselList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    //protected void CheckBoxActiveClicked(object sender, EventArgs e)
    //{
    //    CheckBox cb = (CheckBox)sender;
    //    int nCurrentRow = Int32.Parse(cb.Text);

    //    string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true ? "1" : "0";
    //    string datatransferyn = ((RadCheckBox)gvVesselList.Rows[nCurrentRow].FindControl("chkDataTransferYN")).Checked == true ? "1" : "0";
    //    string attachmentsyn = ((RadLabel)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
    //    string documentsyn = ((RadLabel)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

    //    string vesselid = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

    //    PhoenixDataTransferCommon.DataTransfersScheduledVesselInsert(int.Parse(vesselid), short.Parse(activeyn), short.Parse(datatransferyn), short.Parse(attachmentsyn), short.Parse(documentsyn));
    //    BindData();
    //}


    //protected void AttachmentCheckBoxClicked(object sender, EventArgs e)
    //{
    //    CheckBox cb = (CheckBox)sender;
    //    int nCurrentRow = Int32.Parse(cb.Text);

    //    string activeyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblActiveYN")).Text == "" ? "0" : "1";
    //    string attachmentsyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
    //    string documentsyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

    //    string vesselid = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

    //    PhoenixDataTransferCommon.DataTransfersScheduledVesselUpdate(int.Parse(vesselid), short.Parse(attachmentsyn), short.Parse(documentsyn));
    //    BindData();

    //}

    //protected void DocumentCheckBoxClicked(object sender, EventArgs e)
    //{
    //    CheckBox cb = (CheckBox)sender;
    //    int nCurrentRow = Int32.Parse(cb.Text);

    //    string activeyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblActiveYN")).Text == "" ? "0" : "1";
    //    string attachmentsyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
    //    string documentsyn = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

    //    string vesselid = ((Label)gvVesselList.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

    //    PhoenixDataTransferCommon.DataTransfersScheduledVesselDocumentUpdate(int.Parse(vesselid), short.Parse(attachmentsyn), short.Parse(documentsyn));
    //    BindData();
    //}
    protected void ucFleet_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();

    }

    protected void txtVesselName_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();

    }


    protected void Rebind()
    {
        gvVesselList.SelectedIndexes.Clear();
        gvVesselList.EditIndexes.Clear();
        gvVesselList.DataSource = null;
        gvVesselList.Rebind();
    }

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UPDATEACTIVEYN"))
            {

                string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true ? "1" : "0";
                string datatransferyn = ((RadCheckBox)e.Item.FindControl("chkDataTransferYN")).Checked == true ? "1" : "0";
                string attachmentsyn = ((RadLabel)e.Item.FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
                string documentsyn = ((RadLabel)e.Item.FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                PhoenixDataTransferCommon.DataTransfersScheduledVesselInsert(int.Parse(vesselid), short.Parse(activeyn), short.Parse(datatransferyn), short.Parse(attachmentsyn), short.Parse(documentsyn));


                Rebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATEDATATRANSFERYN"))
            {
                string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true ? "1" : "0";
                string datatransferyn = ((RadCheckBox)e.Item.FindControl("chkDataTransferYN")).Checked == true ? "1" : "0";
                string attachmentsyn = ((RadLabel)e.Item.FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
                string documentsyn = ((RadLabel)e.Item.FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                PhoenixDataTransferCommon.DataTransfersScheduledVesselInsert(int.Parse(vesselid), short.Parse(activeyn), short.Parse(datatransferyn), short.Parse(attachmentsyn), short.Parse(documentsyn));

                Rebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATEATTACHMENTSYN"))
            {
                string activeyn = ((RadLabel)e.Item.FindControl("lblActiveYN")).Text == "" ? "0" : "1";
                string attachmentsyn = ((RadLabel)e.Item.FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
                string documentsyn = ((RadLabel)e.Item.FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                PhoenixDataTransferCommon.DataTransfersScheduledVesselUpdate(int.Parse(vesselid), short.Parse(attachmentsyn), short.Parse(documentsyn));

                Rebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATEDOCUMENTSYN"))
            {
                string activeyn = ((RadLabel)e.Item.FindControl("lblActiveYN")).Text == "" ? "0" : "1";
                string attachmentsyn = ((RadLabel)e.Item.FindControl("lblSyncAttachmentsYN")).Text == "" ? "0" : "1";
                string documentsyn = ((RadLabel)e.Item.FindControl("lblSyncDocumentsYN")).Text == "" ? "0" : "1";

                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                PhoenixDataTransferCommon.DataTransfersScheduledVesselDocumentUpdate(int.Parse(vesselid), short.Parse(attachmentsyn), short.Parse(documentsyn));

                Rebind();

            }
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

    protected void gvVesselList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkActiveYN");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            cb.Checked = drv["FLDACTIVE"].ToString().Equals("1") ? true : false;

            cb = (RadCheckBox)e.Item.FindControl("chkDataTransferYN");
            cb.Checked = drv["FLDDATATRANSFER"].ToString().Equals("1") ? true : false;

            cb = (RadCheckBox)e.Item.FindControl("chkAttachmentsYN");
            cb.Checked = drv["FLDATTACHMENTTRANSFER"].ToString().Equals("1") ? true : false;

            cb = (RadCheckBox)e.Item.FindControl("chkDocumentsYN");
            cb.Checked = drv["FLDDOCUMENTTANSFER"].ToString().Equals("1") ? true : false;

        }
    }

    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
        BindData();
    }
}
