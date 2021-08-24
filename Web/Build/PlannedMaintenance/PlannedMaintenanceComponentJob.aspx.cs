using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Job Description", "DETAILS");
            toolbarmain.AddButton("Reporting Templates", "HISTORYTEMPLATE");
            toolbarmain.AddButton("Attachments", "ATTACHMENT");

            //toolbarmain.AddButton("Work Done History", "DONEHISTORY");
            toolbarmain.AddButton("Parts Required", "PARTS");
            toolbarmain.AddButton("Parameters", "PARAMETER");
            toolbarmain.AddButton("Manual/s", "MANUAL");
            toolbarmain.AddButton("Other Documents", "DOCUMENTS");
			if (Request.QueryString["disback"] == null)
            	toolbarmain.AddButton("Back", "COMPONENT", ToolBarDirection.Right);
            MenuComponentJob.AccessRights = this.ViewState;
            MenuComponentJob.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["JOBMODE"] = string.Empty;
                ViewState["COMPONENTID"] = null;
                ViewState["COMPONENTJOBID"] = null;
                ViewState["JOBID"] = null;
                ViewState["CANCELLEDJOB"] = "";
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["VESSELID"] = null;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();

                if (Request.QueryString["COMPONENTJOBID"] != null)
                {
                    ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                }
                
                if (Request.QueryString["JOBMODE"] != null)
                    ViewState["JOBMODE"] = Request.QueryString["JOBMODE"];
                if (Request.QueryString["FromRA"] != null)
                {
                    ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["p"]) ? 1 : int.Parse(Request.QueryString["p"]);
                    if (ViewState["VESSELID"] != null)
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] + "&vesselid=" + ViewState["VESSELID"];
                    else
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];
                }
                if (Request.QueryString["Cancelledjob"] != null)
                {
                    ViewState["CANCELLEDJOB"] = Request.QueryString["Cancelledjob"];
                }

                MenuComponentJob.SelectedMenuIndex = 0;
                //BindComponentData();
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuComponentJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"] + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"];
            }
            else if (CommandName.ToUpper().Equals("HISTORYTEMPLATE"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"];
            }
            else if (CommandName.ToUpper().Equals("DONEHISTORY"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"] + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString();
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
            }
            else if (CommandName.ToUpper().Equals("COMPONENT"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    if (Request.QueryString["hierarchy"] != null && Request.QueryString["hierarchy"] == "1")
                        Response.Redirect("../Inventory/InventoryComponentTreeDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString(), false);
                    else
                        Response.Redirect("../Inventory/InventoryComponent.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx&p=" + ViewState["PAGENUMBER"], false);
                }
                else
                {
                    if (Request.QueryString["hierarchy"] != null && Request.QueryString["hierarchy"] == "1")
                        Response.Redirect("../Inventory/InventoryComponentTreeDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + ViewState["VESSELID"], false);
                    else
                        Response.Redirect("../Inventory/InventoryComponent.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx&p=" + ViewState["PAGENUMBER"] + "&vesselid=" + ViewState["VESSELID"], false);
                }
            }
            else if (CommandName.ToUpper().Equals("PARTS"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString();
            }
            else if (CommandName.ToUpper().Equals("PARAMETER"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString();
            }
            else if (CommandName.ToUpper().Equals("MANUAL"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1";
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                if (ViewState["VESSELID"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../DocumentManagement/DocumentManagementComponentJobFormList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&vesselid=" + ViewState["VESSELID"];
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../DocumentManagement/DocumentManagementComponentJobFormList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString();
            }
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkOrder.Rebind();
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
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobGeneral.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceHistoryTemplateList.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 2;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 3;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderDone.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 3;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobPartsRequired.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 4;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobParameter.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 5;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobManual.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 6;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("DocumentManagementComponentJobFormList.aspx"))
            {
                MenuComponentJob.SelectedMenuIndex = 7;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void BindComponentData()
    //{
    //    if ((ViewState["COMPONENTID"] != null) && (ViewState["COMPONENTID"].ToString() != ""))
    //    {
    //        DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        //Title1.Text += "    ( " + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + ")";
    //    }
    //}
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDWORKDONEBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Category", "Frequency", "Priority", "Job Done (Yes / Defect)", "Done Date", "Done By", "Remarks" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string CompID = ViewState["COMPONENTJOBID"].ToString();

            int vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.WorkOrderDoneSearch(vesselid
                                                                                , General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                                , General.GetNullableGuid(ViewState["COMPONENTID"].ToString())
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvWorkOrder.CurrentPageIndex + 1
                                                                                , gvWorkOrder.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

            General.ShowExcel("Work Order Done History", dt, alColumns, alCaptions, sortdirection, sortexpression);

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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDJOBDONESTATUS", "FLDWORKDONEDATE", "FLDWORKDONEBY", "FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Category", "Frequency", "Priority", "Job Done (Yes / Defect)", "Done Date", "Done By", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            int vesselid = ViewState["VESSELID"]!=null? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.WorkOrderDoneSearch(vesselid
                                                                                , General.GetNullableGuid(Request.QueryString["COMPONENTJOBID"])
                                                                                , General.GetNullableGuid(Request.QueryString["COMPONENTID"])
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvWorkOrder.CurrentPageIndex + 1
                                                                                , gvWorkOrder.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
            gvWorkOrder.DataSource = dt;
            gvWorkOrder.VirtualItemCount = iRowCount;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvWorkOrder", "Work Order Done History", alCaptions, alColumns, ds);

            
            if (dt.Rows.Count > 0)
            {
                ViewState["JOBID"] = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDCOMPJOBDTKEY"].ToString();

                if (ViewState["COMPONENTJOBID"] == null || ViewState["COMPONENTJOBID"].ToString() == "")
                {
                    ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTJOBID"].ToString();
                    
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    if (ViewState["VESSELID"] != null)
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=&vesselid=" + vesselid;
                    else
                        ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"] + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"];
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceWorkOrderDone.aspx"))
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString();
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobPartsRequired.aspx"))
                {
                    //ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString();
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString();
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceHistoryTemplateList.aspx"))
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString();
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobParameter.aspx"))
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString();
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobManual.aspx"))
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1&vesselid" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1";
                }
                else
                {
                    if (ViewState["VESSELID"] != null)
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]) + "&vesselid=" + vesselid;
                    else
                        ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]);
                }
                SetTabHighlight();
            }
            else
            {
                if (ViewState["VESSELID"] != null)
                    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString()+ (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]) + "&vesselid=" + vesselid;
                else
                    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]);
                if (ViewState["COMPONENTJOBID"]!=null && General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString()) != null)
                {
                    DataSet ds1 = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(Request.QueryString["COMPONENTJOBID"]), vesselid);
                    DataRow dr = ds1.Tables[0].Rows[0];

                    ViewState["JOBID"] = dr["FLDJOBID"].ToString();
                    ViewState["DTKEY"] = dr["FLDCOMPJOBDTKEY"].ToString();
                }
                
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

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != RadGrid.PageCommandName && e.CommandName != "ChangePageSize")
            {


                if (e.CommandName.ToUpper().Equals("MAINTENANCELOG"))
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx?IsComponent=1&WORKORDERID="
                    //    + lblWorkOrderId.Text.Trim() + "&WORKORDERNO=" + lblWorkOrderNo.Text.Trim() + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString()
                    //    + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&PAGENUMBER=" + ViewState["PARENTPAGENO"].ToString() + "&tv=" + Request.QueryString["tv"] + "'", true);



                    //Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx?IsComponent=1&WORKORDERID="
                    //    + lblWorkOrderId.Text.Trim() + "&WORKORDERNO=" + lblWorkOrderNo.Text.Trim() + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString()
                    //    + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&PAGENUMBER=" + ViewState["PARENTPAGENO"].ToString() + "&tv=" + Request.QueryString["tv"] + "'");
                }
                else if (e.CommandName.ToUpper().Equals("MACHINERYRA"))
                {

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                int vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                LinkButton RA = (LinkButton)e.Item.FindControl("cmdRA");
                if (RA != null)
                {
                    if (ViewState["VESSELID"] != null)
                        RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid="+ vesselid + "',500,600); return false;");
                    else
                        RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");
                    RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
                }
                LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
                if (cmdReschedule != null)
                {
                    if (ViewState["VESSELID"] != null)
                        cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('Postpone','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + vesselid + "',500,600); return false;");
                    else
                        cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('Postpone','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdAttachments = (LinkButton)e.Item.FindControl("cmdAttachments");
                if (cmdAttachments != null)
                {
                    cmdAttachments.Attributes.Add("onclick", "javascript:openNewWindow('Attachments','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + drv["FLDDTKEY"] + "&MOD=PLANNEDMAINTENANCE',500,600); return false;");
                }

                LinkButton cmdRTemplates = (LinkButton)e.Item.FindControl("cmdRTemplates");
                if (cmdRTemplates != null)
                {
                    if (ViewState["VESSELID"] != null)
                        cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + vesselid + "',500,600); return false;");
                    else
                        cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdParts = (LinkButton)e.Item.FindControl("cmdParts");
                if (cmdParts != null)
                {
                    if (ViewState["VESSELID"] != null)
                        cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + vesselid + "',500,600); return false;");
                    else
                        cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
                if (cmdParameters != null)
                {
                    if (ViewState["VESSELID"] != null)
                        cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + vesselid + "',500,600); return false;");
                    else
                        cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdPTW = (LinkButton)e.Item.FindControl("cmdPTW");
                if (cmdPTW != null)
                {
                    if (ViewState["VESSELID"] != null)
                        cmdPTW.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + vesselid + "',500,600); return false;");
                    else
                        cmdPTW.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

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

                if (General.GetNullableGuid(drv["FLDATTACHMENTCODE"].ToString()) != null)
                    cmdAttachments.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachments.CommandName);
                else
                    cmdAttachments.Visible = false;

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
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindUrl()
    {
        int vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
        {
            if (ViewState["VESSELID"] != null)
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=&vesselid" + vesselid;
            else
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&COMPONENTJOBID=";
        }

        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
        {
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"] + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["JOBID"];
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceWorkOrderDone.aspx"))
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString();
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobPartsRequired.aspx"))
        {
            //ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString();
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString();
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceHistoryTemplateList.aspx"))
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString() + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&Compid=" + ViewState["COMPONENTID"].ToString();
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobParameter.aspx"))
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString();
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("PlannedMaintenanceComponentJobManual.aspx"))
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1&vesselid" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString() + "&JOBYN=1";
        }
        else
        {
            if (ViewState["VESSELID"] != null)
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]) + "&vesselid=" + vesselid;
            else
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobGeneral.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + (ViewState["JOBMODE"].ToString() == "AddJob" ? string.Empty : "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]);
        }
        SetTabHighlight();
    }

}
