using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                DataSet ds = new DataSet();
                //cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["WORKORDERID"] = null;

                ViewState["WORKORDERNO"] = string.IsNullOrEmpty(Request.QueryString["WORKORDERNO"]) ? "" : Request.QueryString["WORKORDERNO"];
                ViewState["COMPONENTID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]) ? "" : Request.QueryString["COMPONENTID"];
                ViewState["PARENTPAGENO"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
                ViewState["COMPONENTJOBID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTJOBID"]) ? "" : Request.QueryString["COMPONENTJOBID"];
                ViewState["REPORTID"] = string.IsNullOrEmpty(Request.QueryString["REPORTID"]) ? "" : Request.QueryString["REPORTID"];
                ViewState["FORMID"] = string.IsNullOrEmpty(Request.QueryString["FORMID"]) ? "" : Request.QueryString["FORMID"];
                if (Request.QueryString["FromDashboard"] != null && Request.QueryString["FromDashboard"].ToString() == "1")
                {
                    Filter.CurrentWorkOrderReportLogFilter = null;
                }
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();

                }
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["JOBCATEGORY"] = string.Empty;
                //NameValueCollection nvc = new NameValueCollection();
                //if (Filter.CurrentWorkOrderReportLogFilter != null)
                //{
                //    nvc = Filter.CurrentWorkOrderReportLogFilter;
                //}
                //if(string.IsNullOrEmpty(nvc["txtDateFrom"]))
                //{
                //    nvc["txtDateFrom"] = ViewState["FDATE"].ToString();
                //}
                //if (string.IsNullOrEmpty(nvc["txtDateTo"]))
                //{
                //    nvc["txtDateTo"] = ViewState["TDATE"].ToString();
                //}
                //Filter.CurrentWorkOrderReportLogFilter = nvc;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                //if (ViewState["WORKORDERID"] != null && ViewState["WORKORDERID"].ToString() != string.Empty)
                if(string.IsNullOrEmpty(GetFilter("txtDateFrom")) && string.IsNullOrEmpty(GetFilter("txtDateTo")))
                {
                    SetFilter("txtDateFrom", General.GetDateTimeToString(DateTime.Now.AddMonths(-2)));
                    SetFilter("txtDateTo", General.GetDateTimeToString(DateTime.Now));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                RadGrid1.CurrentPageIndex = 0;
                RadGrid1.Rebind();
            }
            else if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ViewState["WORKORDERID"] = null;
                RadGrid1.CurrentPageIndex = 0;
                Filter.CurrentWorkOrderReportLogFilter = null;
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
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
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDREPORTBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Category", "Frequency", "Priority", "Job Type", "Done Date", "Done By", "Remarks" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            if (ViewState["WORKORDERNO"] != null && ViewState["WORKORDERNO"].ToString() != string.Empty)
            {
                nvc["txtWorkOrderNumber"] = ViewState["WORKORDERNO"].ToString();
            }
            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(nvc.Get("txtWorkOrderNumber")), General.GetNullableString(nvc.Get("txtWorkOrderName"))
                    , General.GetNullableString(nvc.Get("txtComponentNumber")),
                    General.GetNullableString(nvc.Get("txtComponentName")),
                    General.GetNullableDateTime(nvc.Get("txtDateFrom"))
                    , General.GetNullableDateTime(nvc.Get("txtDateTo"))
                    , General.GetNullableInteger(nvc.Get("totalduration"))
                    , General.GetNullableInteger(nvc.Get("ucrank"))
                    , General.GetNullableString(nvc.Get("chkoverdue"))
                    , General.GetNullableString(nvc.Get("jobclass"))
                    , General.GetNullableString(nvc.Get("txtClassCode"))
                    , General.GetNullableInteger(nvc.Get("ucMainType"))
                    , General.GetNullableInteger(nvc.Get("ucMaintClass")), General.GetNullableInteger(nvc.Get("ucMainCause"))
                    , (byte?)General.GetNullableInteger(nvc.Get("chkUnexpected"))
                    , (byte?)General.GetNullableInteger(nvc.Get("ddlJobType"))
                    , General.GetNullableGuid(nvc.Get("txtJobId"))
                    , General.GetNullableInteger(nvc.Get("txtPriority"))
                    , General.GetNullableInteger(nvc.Get("ucFrequency")), General.GetNullableInteger(nvc.Get("txtFrequencyFrom")), General.GetNullableInteger(nvc.Get("txtFrequencyTo"))
                    , General.GetNullableInteger(nvc.Get("ucCounterType")), General.GetNullableInteger(nvc.Get("ucCounterFrequencyFrom")), General.GetNullableInteger(nvc.Get("ucCounterFrequencyTo"))
                        , sortexpression, sortdirection,
                        RadGrid1.CurrentPageIndex + 1,
                        RadGrid1.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc.Get("chkPostponed")),
                        General.GetNullableInteger(nvc.Get("chkRaJob")),
                        General.GetNullableInteger(nvc.Get("chkCritical")),
                        General.GetNullableInteger(nvc.Get("ddlJobCategory")));
            General.ShowExcel("Maintenance Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDREPORTBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Category", "Frequency", "Priority", "Job Type", "Done Date", "Done By", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            if (ViewState["WORKORDERNO"] != null && ViewState["WORKORDERNO"].ToString() != string.Empty)
            {
                nvc["txtWorkOrderNumber"] = ViewState["WORKORDERNO"].ToString();
            }
            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(nvc.Get("txtWorkOrderNumber")), General.GetNullableString(nvc.Get("txtWorkOrderName"))
                    , General.GetNullableString(nvc.Get("txtComponentNumber"))
                    , General.GetNullableString(nvc.Get("txtComponentName"))
                    , General.GetNullableDateTime(nvc.Get("txtDateFrom"))
                    , General.GetNullableDateTime(nvc.Get("txtDateTo"))
                    , General.GetNullableInteger(nvc.Get("totalduration"))
                    , General.GetNullableInteger(nvc.Get("ucrank"))
                    , General.GetNullableString(nvc.Get("chkoverdue"))
                    , General.GetNullableString(nvc.Get("jobclass"))
                    , General.GetNullableString(nvc.Get("txtClassCode"))
                    , General.GetNullableInteger(nvc.Get("ucMainType"))
                    , General.GetNullableInteger(nvc.Get("ucMaintClass")), General.GetNullableInteger(nvc.Get("ucMainCause"))
                    , (byte?)General.GetNullableInteger(nvc.Get("chkUnexpected"))
                    , (byte?)General.GetNullableInteger(nvc.Get("ddlJobType"))
                    , General.GetNullableGuid(nvc.Get("txtJobId"))
                    , General.GetNullableInteger(nvc.Get("txtPriority"))
                    , General.GetNullableInteger(nvc.Get("ucFrequency")), General.GetNullableInteger(nvc.Get("txtFrequencyFrom"))
                    , General.GetNullableInteger(nvc.Get("txtFrequencyTo"))
                    , General.GetNullableInteger(nvc.Get("ucCounterType")), General.GetNullableInteger(nvc.Get("ucCounterFrequencyFrom"))
                    , General.GetNullableInteger(nvc.Get("ucCounterFrequencyTo"))
                        , sortexpression, sortdirection,
                        RadGrid1.CurrentPageIndex + 1,
                        RadGrid1.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc.Get("chkPostponed")),
                        General.GetNullableInteger(nvc.Get("chkRaJob")),
                        General.GetNullableInteger(nvc.Get("chkCritical")),
                        General.GetNullableInteger(nvc.Get("ddlJobCategory")));

            General.SetPrintOptions("RadGrid1", "Maintenance Log", alCaptions, alColumns, ds);
            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;
            //ViewState["FDATE"] = ds.Tables[1].Rows[0]["FLDFROMDATE"].ToString();
            //ViewState["TDATE"] = ds.Tables[1].Rows[0]["FLDTODATE"].ToString();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                ViewState["ROWCOUNT"] = iRowCount;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            RadGrid1.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            RadGrid1.CurrentPageIndex = 0;
        }
        if (e.CommandName.ToUpper() == "REBINDGRID")
        {
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogFilter.aspx");
        }
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
                string freq =   freqfilter.Split('~')[0];
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
            string daterange = grid.MasterTableView.GetColumn("FLDWORKDONEDATE").CurrentFilterValue;
            if (!string.IsNullOrEmpty(daterange))
            {
                nvc["txtDateFrom"] = daterange.Split('~')[0];
                nvc["txtDateTo"] = daterange.Split('~')[1];

                ViewState["FDATE"] = daterange.Split('~')[0];
                ViewState["TDATE"] = daterange.Split('~')[1];
            }
            ViewState["JOBCATEGORY"]= grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
            nvc["ddlJobCategory"] = grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
            nvc["txtPriority"] = grid.MasterTableView.GetColumn("FLDPLANINGPRIORITY").CurrentFilterValue;
            nvc["ddlJobType"] = grid.MasterTableView.GetColumn("FLDJOBDONESTATUS").CurrentFilterValue;
            Filter.CurrentWorkOrderReportLogFilter = nvc;
            
        }

    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (!IsPostBack)
        {
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = GetFilter("txtWorkOrderNumber");
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtWorkOrderName");
            grid.MasterTableView.GetColumn("FLDWORKDONEDATE").CurrentFilterValue = GetFilter("txtDateFrom") + '~' + GetFilter("txtDateTo");
            grid.MasterTableView.GetColumn("FLDJOBDONESTATUS").CurrentFilterValue = GetFilter("ddlJobType");
            grid.Rebind();
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridFilteringItem)
        {            
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = GetFilter("txtWorkOrderNumber");
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtWorkOrderName");
            grid.MasterTableView.GetColumn("FLDWORKDONEDATE").CurrentFilterValue = GetFilter("txtDateFrom") + '~' + GetFilter("txtDateTo");
            grid.MasterTableView.GetColumn("FLDJOBDONESTATUS").CurrentFilterValue = GetFilter("ddlJobType");
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton RA = (LinkButton)e.Item.FindControl("cmdRA");
            string name = drv["FLDWORKORDERNUMBER"].ToString() + " - " + drv["FLDWORKORDERNAME"].ToString();
            if (RA != null)
            {
                RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','"+ name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");
                RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
            }
            LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (cmdReschedule != null)
            {
                cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('Postpone','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

            }
            ImageButton cmdAttachments = (ImageButton)e.Item.FindControl("cmdAttachments");
            if (cmdAttachments != null)
            {
                cmdAttachments.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachments.CommandName);
                cmdAttachments.Attributes.Add("onclick", "javascript:openNewWindow('Attachments','" + name + "','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + drv["FLDDTKEY"] + "&MOD=PLANNEDMAINTENANCE',500,600); return false;");

            }

            LinkButton cmdRTemplates = (LinkButton)e.Item.FindControl("cmdRTemplates");
            if (cmdRTemplates != null)
            {
                cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

            }
            LinkButton cmdParts = (LinkButton)e.Item.FindControl("cmdParts");
            if (cmdParts != null)
            {
                cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

            }
            LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
            if (cmdParameters != null)
            {
                cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");
                if(drv["FLDISJOBPARAMETERWITHINLIMIT"].ToString() == "1")
                {
                    cmdParameters.ToolTip = "All values are within set limits";
                    cmdParameters.Attributes["style"] = "color:green !important";
                }
                else if (drv["FLDISJOBPARAMETERWITHINLIMIT"].ToString() == "0")
                {
                    cmdParameters.ToolTip = "One or more values are outside the set limits";
                    cmdParameters.Attributes["style"] = "color:red !important";
                }

            }
            LinkButton cmdPTW = (LinkButton)e.Item.FindControl("cmdPTW");
            if (cmdPTW != null)
            {
                cmdPTW.Attributes.Add("onclick", "javascript:openNewWindow('PTW','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

            }

            if (General.GetNullableInteger(drv["FLDRAYN"].ToString()) == 1)
            {
                RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
            }
            else
                RA.Visible = false;

            if (General.GetNullableInteger(drv["FLDTEMPLATE"].ToString()) == 1)
                cmdRTemplates.Visible = SessionUtil.CanAccess(this.ViewState, cmdRTemplates.CommandName);
            else
                cmdRTemplates.Visible = false;

            if (drv["FLDATTACHMENTCODE"].ToString() == "0")
                cmdAttachments.ImageUrl = Session["images"] + "/no-attachment.png";

            if (General.GetNullableInteger(drv["FLDPARAMETERSYN"].ToString()) == 1)
                cmdParameters.Visible = SessionUtil.CanAccess(this.ViewState, cmdParameters.CommandName);
            else
                cmdParameters.Visible = false;
            if (General.GetNullableInteger(drv["FLDPTWYN"].ToString()) == 1)
                cmdPTW.Visible = SessionUtil.CanAccess(this.ViewState, cmdPTW.CommandName);
            else
                cmdPTW.Visible = false;

            if (General.GetNullableInteger(drv["FLDRESCHEDULEYN"].ToString()) == 1)
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
            else
                cmdReschedule.Visible = false;

            if (General.GetNullableInteger(drv["FLDPARTSYN"].ToString()) == 1)
                cmdParts.Visible = SessionUtil.CanAccess(this.ViewState, cmdParts.CommandName);
            else
                cmdParts.Visible = false;

            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");

            if (lnkTitle != null)
            {
                if (General.GetNullableGuid(drv["FLDCOMPONENTJOBID"].ToString()) != null)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"].ToString() + "&COMPONENTID=" + drv["FLDCOMPONENTID"].ToString() + "&Cancelledjob=0','','1200','600');return false");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
            }
            ImageButton waive = (ImageButton)e.Item.FindControl("lnkPtwWaive");
            if (waive != null)
            {
                if (drv["FLDISWAIVED"].ToString() == "1")
                    waive.Attributes.Add("onclick", "javascript:openNewWindow('WAIVE','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogWaiver.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
                else
                    waive.Visible = false;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected string GetFilter(string filter)
    {
        string value = string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
        if (nvc != null)
        {
            value = nvc[filter];
        }
        return value;
    }
    protected void SetFilter(string key, string value)
    {
        NameValueCollection nvc = Filter.CurrentWorkOrderReportLogFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { key, value }
            };
        }
        else
        {
            nvc[key] = value;
        }
        Filter.CurrentWorkOrderFilter = nvc;
    }
}
