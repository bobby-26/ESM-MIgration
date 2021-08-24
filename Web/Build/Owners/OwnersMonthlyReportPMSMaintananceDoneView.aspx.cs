using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSMaintananceDoneView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Owners/OwnersMonthlyReportPMSMaintananceDoneView.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersMonthlyReportPMSMaintananceDoneView.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersMonthlyReportPMSMaintananceDoneView.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuDivWorkOrder.AccessRights = this.ViewState;
        MenuDivWorkOrder.MenuList = toolbargrid.Show();


        if (!IsPostBack)
        {
            ViewState["FREQUENCY"] = string.Empty;
            ViewState["FREQUENCYTYPE"] = string.Empty;
            ViewState["JOBCATEGORY"] = string.Empty;
            Filter.CurrentWorkOrderReportLogFilter = null;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "FIND")
            {
                RadGrid1.Rebind();
            }
            //else if (CommandName.ToUpper() == "EXCEL")
            //{
            //    ShowExcel();
            //}
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ViewState["WORKORDERID"] = null;
                RadGrid1.CurrentPageIndex = 0;
                Filter.CurrentWorkOrderReportLogFilter = null;
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["WORKORDERNO"] = string.Empty;
                ViewState["JOBCATEGORY"] = string.Empty;
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDWORKORDERNUMBER"));
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDWORKORDERNAME"));
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDCOMPONENTNUMBER"));
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDCOMPONENTNUMBER"));
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDCOMPONENTNAME"));
                ClearGridFilter(RadGrid1.MasterTableView.GetColumn("FLDPLANINGPRIORITY"));
                RadGrid1.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void ClearGridFilter(GridColumn column)
    {
        column.ListOfFilterValues = null;
        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        column.CurrentFilterValue = string.Empty;
    }

    private void BindData()
    {
        try
        {
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDREPORTBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Category", "Frequency", "Priority", "Job Done (Yes / Defect)", "Done Date", "Done By", "Remarks" };

            NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            if (ViewState["WORKORDERNO"] != null && ViewState["WORKORDERNO"].ToString() != string.Empty)
            {
                nvc["txtWorkOrderNumber"] = ViewState["WORKORDERNO"].ToString();
            }
            DataTable dT = PhoenixOwnerReportPMS.OwnersReportWorkDone(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
            //DataSet ds = PhoenixOwnerReportPMS.OwnersReportWorkDoneReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            //            , General.GetNullableString(nvc.Get("txtWorkOrderNumber"))
            //            , General.GetNullableString(nvc.Get("txtWorkOrderName"))
            //            , General.GetNullableString(nvc.Get("txtComponentNumber"))
            //            , General.GetNullableString(nvc.Get("txtComponentName"))
            //            , General.GetNullableInteger(nvc.Get("totalduration"))
            //            , General.GetNullableInteger(nvc.Get("ucrank"))
            //            , General.GetNullableString(nvc.Get("chkoverdue"))
            //            , General.GetNullableString(nvc.Get("jobclass"))
            //            , General.GetNullableString(nvc.Get("txtClassCode"))
            //            , General.GetNullableInteger(nvc.Get("ucMainType"))
            //            , General.GetNullableInteger(nvc.Get("ucMaintClass")), General.GetNullableInteger(nvc.Get("ucMainCause"))
            //            , (byte?)General.GetNullableInteger(nvc.Get("chkUnexpected"))
            //            , (byte?)General.GetNullableInteger(nvc.Get("chkDefect"))
            //            , General.GetNullableGuid(nvc.Get("txtJobId"))
            //            , General.GetNullableInteger(nvc.Get("txtPriority"))
            //            , General.GetNullableInteger(nvc.Get("ucFrequency")), General.GetNullableInteger(nvc.Get("txtFrequencyFrom"))
            //            , General.GetNullableInteger(nvc.Get("txtFrequencyTo"))
            //            , General.GetNullableInteger(nvc.Get("ucCounterType")), General.GetNullableInteger(nvc.Get("ucCounterFrequencyFrom"))
            //            , General.GetNullableInteger(nvc.Get("ucCounterFrequencyTo"))
            //            , General.GetNullableInteger(nvc.Get("chkPostponed"))
            //            , General.GetNullableInteger(nvc.Get("chkRaJob"))
            //            , General.GetNullableInteger(nvc.Get("chkCritical"))
            //            , General.GetNullableInteger(nvc.Get("ddlJobCategory")));

            RadGrid1.DataSource = dT;
            //General.SetPrintOptions("RadGrid1", "Maintenance Log", alCaptions, alColumns, ds);
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
 
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDREPORTBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Category", "Frequency", "Priority", "Job Done (Yes / Defect)", "Done Date", "Done By", "Remarks" };

            NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            if (ViewState["WORKORDERNO"] != null && ViewState["WORKORDERNO"].ToString() != string.Empty)
            {
                nvc["txtWorkOrderNumber"] = ViewState["WORKORDERNO"].ToString();
            }

            DataSet ds = PhoenixOwnerReportPMS.OwnersReportWorkDoneReport(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                        , General.GetNullableString(nvc.Get("txtWorkOrderNumber"))
                        , General.GetNullableString(nvc.Get("txtWorkOrderName"))
                        , General.GetNullableString(nvc.Get("txtComponentNumber"))
                        , General.GetNullableString(nvc.Get("txtComponentName"))
                        , General.GetNullableInteger(nvc.Get("totalduration"))
                        , General.GetNullableInteger(nvc.Get("ucrank"))
                        , General.GetNullableString(nvc.Get("chkoverdue"))
                        , General.GetNullableString(nvc.Get("jobclass"))
                        , General.GetNullableString(nvc.Get("txtClassCode"))
                        , General.GetNullableInteger(nvc.Get("ucMainType"))
                        , General.GetNullableInteger(nvc.Get("ucMaintClass")), General.GetNullableInteger(nvc.Get("ucMainCause"))
                        , (byte?)General.GetNullableInteger(nvc.Get("chkUnexpected"))
                        , (byte?)General.GetNullableInteger(nvc.Get("chkDefect"))
                        , General.GetNullableGuid(nvc.Get("txtJobId"))
                        , General.GetNullableInteger(nvc.Get("txtPriority"))
                        , General.GetNullableInteger(nvc.Get("ucFrequency")), General.GetNullableInteger(nvc.Get("txtFrequencyFrom"))
                        , General.GetNullableInteger(nvc.Get("txtFrequencyTo"))
                        , General.GetNullableInteger(nvc.Get("ucCounterType")), General.GetNullableInteger(nvc.Get("ucCounterFrequencyFrom"))
                        , General.GetNullableInteger(nvc.Get("ucCounterFrequencyTo"))
                        , General.GetNullableInteger(nvc.Get("chkPostponed"))
                        , General.GetNullableInteger(nvc.Get("chkRaJob"))
                        , General.GetNullableInteger(nvc.Get("chkCritical"))
                        , General.GetNullableInteger(nvc.Get("ddlJobCategory")));

            General.ShowExcel("Maintenance Log", ds.Tables[0], alColumns, alCaptions, 0, "");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    DataRowView drv = (DataRowView)e.Item.DataItem;
        //    LinkButton RA = (LinkButton)e.Item.FindControl("cmdRA");
        //    if (RA != null)
        //    {
        //        RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");
        //        RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
        //    }
        //    LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
        //    if (cmdReschedule != null)
        //    {
        //        cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('Postpone','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

        //    }
        //    ImageButton cmdAttachments = (ImageButton)e.Item.FindControl("cmdAttachments");
        //    if (cmdAttachments != null)
        //    {
        //        cmdAttachments.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachments.CommandName);
        //        cmdAttachments.Attributes.Add("onclick", "javascript:openNewWindow('Attachments','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + drv["FLDDTKEY"] + "&MOD=PLANNEDMAINTENANCE',500,600); return false;");

        //    }

        //    LinkButton cmdRTemplates = (LinkButton)e.Item.FindControl("cmdRTemplates");
        //    if (cmdRTemplates != null)
        //    {
        //        cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

        //    }
        //    LinkButton cmdParts = (LinkButton)e.Item.FindControl("cmdParts");
        //    if (cmdParts != null)
        //    {
        //        cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

        //    }
        //    LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
        //    if (cmdParameters != null)
        //    {
        //        cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

        //    }
        //    LinkButton cmdPTW = (LinkButton)e.Item.FindControl("cmdPTW");
        //    if (cmdPTW != null)
        //    {
        //        cmdPTW.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

        //    }

        //    if (General.GetNullableInteger(drv["FLDRAYN"].ToString()) == 1)
        //    {
        //        RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
        //    }
        //    else
        //        RA.Visible = false;

        //    if (General.GetNullableInteger(drv["FLDTEMPLATE"].ToString()) == 1)
        //        cmdRTemplates.Visible = SessionUtil.CanAccess(this.ViewState, cmdRTemplates.CommandName);
        //    else
        //        cmdRTemplates.Visible = false;

        //    if (drv["FLDATTACHMENTCODE"].ToString() == "0")
        //        cmdAttachments.ImageUrl = Session["images"] + "/no-attachment.png";

        //    if (General.GetNullableInteger(drv["FLDPARAMETERSYN"].ToString()) == 1)
        //        cmdParameters.Visible = SessionUtil.CanAccess(this.ViewState, cmdParameters.CommandName);
        //    else
        //        cmdParameters.Visible = false;
        //    if (General.GetNullableInteger(drv["FLDPTWYN"].ToString()) == 1)
        //        cmdPTW.Visible = SessionUtil.CanAccess(this.ViewState, cmdPTW.CommandName);
        //    else
        //        cmdPTW.Visible = false;

        //    if (General.GetNullableInteger(drv["FLDRESCHEDULEYN"].ToString()) == 1)
        //        cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
        //    else
        //        cmdReschedule.Visible = false;

        //    if (General.GetNullableInteger(drv["FLDPARTSYN"].ToString()) == 1)
        //        cmdParts.Visible = SessionUtil.CanAccess(this.ViewState, cmdParts.CommandName);
        //    else
        //        cmdParts.Visible = false;

        //    LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");

        //    if (lnkTitle != null)
        //    {
        //        if (General.GetNullableGuid(drv["FLDCOMPONENTJOBID"].ToString()) != null)
        //            lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"].ToString() + "&COMPONENTID=" + drv["FLDCOMPONENTID"].ToString() + "&Cancelledjob=0','','1200','600');return false");
        //        else
        //            lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
        //    }

        //}
    }


    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            NameValueCollection nvc = new NameValueCollection();
            if (Filter.CurrentWorkOrderReportLogFilter != null)
            {
                nvc = Filter.CurrentWorkOrderReportLogFilter;
            }
            nvc["txtWorkOrderNumber"] = grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue;
            nvc["txtWorkOrderName"] = grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
            nvc["txtComponentNumber"] = grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
            nvc["txtComponentName"] = grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
            string freqfilter = grid.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue;
            if (freqfilter != "")
            {
                string freq = freqfilter.Split('~')[0];
                string freqtype = freqfilter.Split('~')[1];
                if (freqtype != "-1")
                {
                    nvc["ucFrequency"] = freqtype;
                    nvc["txtFrequencyFrom"] = freq;
                }
                else
                {
                    nvc["ucCounterFrequencyFrom"] = freq;
                }
                ViewState["FREQUENCY"] = freq;
                ViewState["FREQUENCYTYPE"] = freqtype;
            }
            ViewState["JOBCATEGORY"] = grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
            nvc["ddlJobCategory"] = grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
            nvc["txtPriority"] = grid.MasterTableView.GetColumn("FLDPLANINGPRIORITY").CurrentFilterValue;
            Filter.CurrentWorkOrderReportLogFilter = nvc;

        }
    }

    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlDiscipline.SelectedValue = ViewState["RESP"].ToString();
    }

    protected void ddlJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        frequency.Items.Add(new RadComboBoxItem("Hours", "-1"));
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }

    protected void RadGrid1_PreRender1(object sender, EventArgs e)
    {

    }
}