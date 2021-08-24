using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PARENTPAGENO"] = string.IsNullOrEmpty(Request.QueryString["p"]) ? 1 : int.Parse(Request.QueryString["p"]);
                ViewState["COMPONENTID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]) ? "" : Request.QueryString["COMPONENTID"];
                ViewState["COMPONENTJOBID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTJOBID"]) ? "" : Request.QueryString["COMPONENTJOBID"];
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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

            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.WorkOrderDoneSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                                , General.GetNullableGuid(ViewState["COMPONENTID"].ToString())
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvWorkOrder.CurrentPageIndex+1
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME","FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY","FLDJOBDONESTATUS", "FLDWORKDONEDATE","FLDWORKDONEBY","FLDREMARKS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title","Category", "Frequency", "Priority","Job Done (Yes / Defect)" ,"Done Date","Done By","Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.WorkOrderDoneSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(Request.QueryString["COMPONENTJOBID"])
                                                                                , General.GetNullableGuid(Request.QueryString["COMPONENTID"])
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvWorkOrder.CurrentPageIndex+1
                                                                                , gvWorkOrder.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvWorkOrder", "Work Order Done History", alCaptions, alColumns, ds);

            if (dt.Rows.Count > 0)
            {
                gvWorkOrder.DataSource = dt;
                gvWorkOrder.VirtualItemCount = iRowCount;

            }
            else
            {
                gvWorkOrder.DataSource = "";
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
                LinkButton RA = (LinkButton)e.Item.FindControl("cmdRA");
                if (RA != null)
                {
                    RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");
                    RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
                }
                LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
                if (cmdReschedule != null)
                {
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
                    cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdParts = (LinkButton)e.Item.FindControl("cmdParts");
                if (cmdParts != null)
                {
                    cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
                if (cmdParameters != null)
                {
                    cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "',500,600); return false;");

                }
                LinkButton cmdPTW = (LinkButton)e.Item.FindControl("cmdPTW");
                if (cmdPTW != null)
                {
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
}
