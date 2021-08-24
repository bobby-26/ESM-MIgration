using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupReportLogList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportLogList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportLogList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["WORKORDERGROUPID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["WORKORDERGROUPID"] != null)
                {
                    ViewState["WORKORDERGROUPID"] = Request.QueryString["WORKORDERGROUPID"].ToString();
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx?WORKORDERID=";
                    if (ViewState["WORKORDERGROUPID"] != null && ViewState["WORKORDERGROUPID"].ToString() != string.Empty)
                        ResetMenu(new Guid(ViewState["WORKORDERGROUPID"].ToString()));
                }

                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx";
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (ViewState["WORKORDERID"] != null && ViewState["WORKORDERID"].ToString() != string.Empty)
                    ResetMenu(new Guid(ViewState["WORKORDERID"].ToString()));
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
                gvWorkOrder.CurrentPageIndex = 1;
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ViewState["WORKORDERID"] = null;
                Filter.CurrentWorkOrderReportLogFilter = null;
                gvWorkOrder.Rebind();
                ResetMenu(new Guid(ViewState["WORKORDERID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("RESOURCES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogDoneBy.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
            }
            else if (CommandName.ToUpper().Equals("PARTUSES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("TEMPLATE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("WORKREPORT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportHistory.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistory.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("JOBDESC"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkRequestJobDescription.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("RALIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("POSTPONE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("PARAMETER"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("PTW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("WAIVER"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderLogWaiver.aspx?WORKORDERID=";
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["COMPONENTJOBID"].ToString() != "")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                                           + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString()
                                           + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&p=" + ViewState["PARENTPAGENO"].ToString(), false);
                }
                else if (Request.QueryString["tv"] != null && Request.QueryString["tv"] != "")
                {
                    Response.Redirect("../Inventory/InventoryComponent.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                        + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&p=" + ViewState["PARENTPAGENO"].ToString() + "&tv=" + Request.QueryString["tv"], false);
                }
                else
                    Response.Redirect("../Inventory/InventoryComponent.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                        + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&p=" + ViewState["PARENTPAGENO"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("MAINTENANCEFORM"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderMappingList.aspx?REPORTID=" + ViewState["REPORTID"].ToString() + "&FORMID=" + ViewState["FORMID"].ToString(), true);
            }
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx", true);
            }
            SetTabHighlight();
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDWORKDONEDATE", "FLDJOBCLASS", "FLDWORKDURATION" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Done Date", "Job Class", "Total Duration" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = null;
           
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.WorkOrderGroupLogSearch(new Guid(ViewState["WORKORDERGROUPID"].ToString())
                    , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount, ref iRowCount, ref iTotalPageCount, null);
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDWORKDONEDATE", "FLDJOBCLASS", "FLDWORKDURATION" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Done Date", "Job Class", "Total Duration" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = null;

            if (ViewState["WORKORDERGROUPID"] != null && ViewState["WORKORDERGROUPID"].ToString() != string.Empty)
            {
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.WorkOrderGroupLogSearch(new Guid(ViewState["WORKORDERGROUPID"].ToString()),
                      sortexpression, sortdirection, gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount, null);
            }
            
            General.SetPrintOptions("gvWorkOrder", "Maintenance Log", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {

                ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx?WORKORDERID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
                }
            }
            else
            {
                gvWorkOrder.DataSource = "";
                DataTable dt = ds.Tables[0];
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetTabHighlight()
    {
        try
        {
            RadToolBar dl = (RadToolBar)MenuWorkOrder.FindControl("dlstTabs");
            if (dl.Items.Count > 0)
            {
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 0;
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportLogDoneBy.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 1;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportLogUsesParts.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 2;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportLogHistory.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 3;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 4;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportHistory.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 5;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 7;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRAList.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 6;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkRequestJobDescription.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 5;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRescheduleDetail.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 8;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderLogParameterList.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 9;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderLogPTW.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 10;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderLogWaiver.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 11;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private int MenuIndex(RadToolBarItemCollection items, string tab)
    {
        int i = 0;
        foreach (RadToolBarButton dl in items)
        {
            if (((LinkButton)dl.FindControl("btnMenu")).Text == tab)
            {
                i = dl.TabIndex;
                break;
            }
        }
        return i;
    }

    private void ResetMenu(Guid gWorkOrder)
    {
        //DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportLogGeneralSearch(gWorkOrder, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("General", "GENERAL");
        toolbarmain.AddButton("Resources Used", "RESOURCES");
        toolbarmain.AddButton("Stock Used", "PARTUSES");
        toolbarmain.AddButton("History", "HISTORY");
        toolbarmain.AddButton("Template", "TEMPLATE");
        //toolbarmain.AddButton("Work Report", "WORKREPORT");
        toolbarmain.AddButton("Work Details", "JOBDESC");
        toolbarmain.AddButton("RA", "RALIST");
        toolbarmain.AddButton("Attachment", "ATTACHMENT");
        toolbarmain.AddButton("Postpone History", "POSTPONE");
        toolbarmain.AddButton("Parameter", "PARAMETER");
        toolbarmain.AddButton("PTW", "PTW");
        toolbarmain.AddButton("Waiver", "WAIVER");
        if (Request.QueryString["IsComponent"] == "1")
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        if (Request.QueryString["IsMaintenanceForm"] == "1")
            toolbarmain.AddButton("Back", "MAINTENANCEFORM", ToolBarDirection.Right);
        if (Request.QueryString["islog"] == "1")
            toolbarmain.AddButton("Back", "LOG", ToolBarDirection.Right);
        MenuWorkOrder.MenuList = toolbarmain.Show();
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.SelectedMenuIndex = 0;
        //Title1.Text = "Maintenance Log  [" + ds.Tables[0].Rows[0]["FLDWORKORDERNUMBER"].ToString() + "]";
        SetTabHighlight();
    }

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvWorkOrder.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvWorkOrder.ExportSettings.IgnorePaging = true;
            gvWorkOrder.ExportSettings.ExportOnlyData = true;
            gvWorkOrder.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvWorkOrder.CurrentPageIndex = 0;
        }
        if (e.CommandName == "RebindGrid")
        {
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogFilter.aspx");
        }
        if (e.CommandName == "Select")
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
            ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
            ResetMenu(new Guid(ViewState["WORKORDERID"].ToString()));
            BindUrl();
        }
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton attachments = (ImageButton)e.Item.FindControl("Attachments");
            string attchmentCode = ((RadLabel)e.Item.FindControl("lblAttachmentCode")).Text;
            if (!string.IsNullOrEmpty(attchmentCode))
            {
                attachments.Visible = true;
                attachments.Enabled = false;
                //attachments.Image.Url = Session["images"] + "/no-attachment.png";
            }
            else
            {
                attachments.Visible = false;
                //attachments.Image.Url = Session["images"] + "/attachment.png";
            }
            ImageButton templates = (ImageButton)e.Item.FindControl("Templates");
            RadLabel template = (RadLabel)e.Item.FindControl("lblTemplate");
            if (string.IsNullOrEmpty(template.Text.ToString()))
            {
                templates.Visible = false;
            }
            else
            {
                template.Enabled = false;
            }
            ImageButton RAjob = (ImageButton)e.Item.FindControl("RAjob");
            RadLabel raJob = (RadLabel)e.Item.FindControl("lblRAJob");
            RadLabel raid = (RadLabel)e.Item.FindControl("lblRaid");
            if ((raJob.Text.ToString() != "1") || string.IsNullOrEmpty(raid.Text.ToString()))
            {
                RAjob.Visible = false;
            }
            else
            {
                if(drv["FLDNEWRATEMPLATE"].Equals("1"))
                    RAjob.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + raid.Text.ToString() + "&showmenu=0&showexcel=NO'); return false;");
                else
                    RAjob.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + raid.Text.ToString() + "&showmenu=0&showexcel=NO'); return false;");
    
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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

    private void BindUrl()
    {
        if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogGeneral.aspx?WORKORDERID=";
        }

        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
        {
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
        }

    }
}