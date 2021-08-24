using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                if (Request.QueryString["from"] == "machinery")
                {
                    CreateFilterCriteria();
                    ViewState["URLREFERER"] = Request.UrlReferrer.ToString();
                }

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["WIEVCOMPONENTID"] = "";
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["JOBID"] = null;
                ViewState["REPORTWORKYN"] = "";
                ViewState["WORKORDERNO"] = "";
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
                if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=";
                    ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
                }
                if (Request.QueryString["FromRATemplate"] != null && Request.QueryString["FromRATemplate"] == "1")
                {
                    ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&WORKORDERID=";

                }
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
            }
            else if (CommandName.ToUpper().Equals("PERMITTOWORK"))
            {
                // ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportGeneral.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("JOBDETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("PARTREQUIRED"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderPartsUsed.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("POSTPONEMENT"))
            {
                //ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=";
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("REPORTWORK"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReport.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&WORKORDERNO=" + ViewState["WORKORDERNO"]);
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                string from = Request.QueryString["from"];
                if (from != null && from.ToUpper() == "MACHINERY")
                {
                    Response.Redirect(ViewState["URLREFERER"].ToString(), true);
                }
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&JOBID=" + ViewState["JOBID"];
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderPartsUsed.aspx"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&COMPONENTID=" + ViewState["COMPONENTID"];
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRA.aspx"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
            }
            ResetMenu();
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE", "FLDLASTDONEDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed","Last Done" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds;
            if (Filter.CurrentWorkOrderFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
                ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
                    , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString()), General.GetNullableString(nvc.Get("txtComponentNumber").ToString())
                    , General.GetNullableString(nvc.Get("txtComponentName").ToString()), General.GetNullableString(nvc.Get("planning").ToString())
                    , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableString(nvc.Get("txtClassCode"))
                    , General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString()), General.GetNullableDateTime(nvc.Get("txtDateTo").ToString())
                    , General.GetNullableInteger(nvc.Get("ucMainType").ToString()), General.GetNullableInteger(nvc.Get("ucMaintClass").ToString())
                    , General.GetNullableInteger(nvc.Get("ucMainCause").ToString()), General.GetNullableInteger(nvc.Get("chkUnexpected").ToString())
                    , General.GetNullableString(nvc.Get("status").ToString()), General.GetNullableInteger(nvc.Get("txtPriority").ToString())
                    , nvc.Get("ucRank").ToString(), (byte?)General.GetNullableInteger(nvc.Get("chkDefect"))
                    , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount
                    , (byte?)General.GetNullableInteger(nvc.Get("chkRaPendingApproval"))
                    , (byte?)General.GetNullableInteger(nvc.Get("chkRaRequired"))
                    , (byte?)General.GetNullableInteger(nvc.Get("chkPlannedInWo"))
                    , General.GetNullableInteger(nvc.Get("JobCategory")));
            }
            else
            {
                ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString()), null, null, null, null, null
                    , null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                             gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount, null, null,null,null);
            }
            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["WORKORDERID"] = null;
                Filter.CurrentWorkOrderFilter = null;
                gvWorkOrder.Rebind();
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

            ResetMenu();
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            if (Filter.CurrentWorkOrderFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
                ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
                    , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
                    , General.GetNullableString(nvc.Get("txtComponentNumber").ToString()),
                    General.GetNullableString(nvc.Get("txtComponentName").ToString()),
                    General.GetNullableString(nvc.Get("planning").ToString())
                    , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableString(nvc.Get("txtClassCode"))
                    , General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString())
                    , General.GetNullableDateTime(nvc.Get("txtDateTo").ToString()), General.GetNullableInteger(nvc.Get("ucMainType").ToString())
                    , General.GetNullableInteger(nvc.Get("ucMaintClass").ToString()), General.GetNullableInteger(nvc.Get("ucMainCause").ToString())
                    , General.GetNullableInteger(nvc.Get("chkUnexpected").ToString()), General.GetNullableString(nvc.Get("status").ToString())
                    , General.GetNullableInteger(nvc.Get("txtPriority").ToString()), nvc.Get("ucRank").ToString(), (byte?)General.GetNullableInteger(nvc.Get("chkDefect")),
                        sortexpression, sortdirection,
                        gvWorkOrder.CurrentPageIndex + 1,
                        gvWorkOrder.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount
                        , (byte?)General.GetNullableInteger(nvc.Get("chkRaPendingApproval"))
                        , (byte?)General.GetNullableInteger(nvc.Get("chkRaRequired"))
                        , (byte?)General.GetNullableInteger(nvc.Get("chkPlannedInWo"))
                        , General.GetNullableInteger(nvc.Get("JobCategory")));
            }
            else
            {
                ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
                    , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                             gvWorkOrder.CurrentPageIndex + 1,
                             gvWorkOrder.PageSize,
                             ref iRowCount,
                             ref iTotalPageCount
                             , null
                             , null
                             , null
                             , null);
            }
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {

                //if (ViewState["WORKORDERID"] == null)
                //{
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    ViewState["JOBID"] = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
                    ViewState["COMPONENTID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    ViewState["WORKORDERNO"] = ds.Tables[0].Rows[0]["FLDWORKORDERNO"].ToString();
                    //gvWorkOrder.SelectedIndex = 0;
                //}

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&JOBID=" + ViewState["JOBID"];
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderPartsUsed.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&COMPONENTID=" + ViewState["COMPONENTID"];
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRA.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString();
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
                }
                SetTabHighlight();
                ResetMenu();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx";
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //      gvWorkOrder.SelectedIndex = se.NewSelectedIndex;

        //      ViewState["COMPONENTID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblComponentId")).Text;
        //      ViewState["WORKORDERID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblWorkOrderId")).Text;
        //      ViewState["JOBID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblJobID")).Text;
        //      ViewState["DTKEY"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
        //ViewState["WORKORDERNO"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblWorkorderNo")).Text;
        //ResetMenu();
        //      BindData();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
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

    protected void SetTabHighlight()
    {
        try
        {
            RadToolBar dl = (RadToolBar)MenuWorkOrder.FindControl("dlstTabs");
            if (dl.Items.Count > 0)
            {
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderGeneral.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 0;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderDetail.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 1;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 2;
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 3;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderPartsUsed.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 4;
                }
                //else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRA.aspx"))
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRescheduleDetail.aspx"))
                {
                    MenuWorkOrder.SelectedMenuIndex = 5;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetRowSelection()
    //{
    //    gvWorkOrder.SelectedIndex = -1;
    //    for (int i = 0; i < gvWorkOrder.Rows.Count; i++)
    //    {
    //        if (gvWorkOrder.DataKeys[i].Value.ToString().Equals(ViewState["WORKORDERID"].ToString()))
    //        {
    //            gvWorkOrder.SelectedIndex = i;
    //            ViewState["DTKEY"] = ((Label)gvWorkOrder.Rows[gvWorkOrder.SelectedIndex].FindControl("lbldtkey")).Text;
    //            break;
    //        }
    //    }
    //}
    private string GetWorkOrderStatus(Guid gWorkOrderId)
    {
        string status = "";

        DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditWorkOrder(gWorkOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            status = ds.Tables[0].Rows[0]["FLDWORKORDERSTATUS"].ToString();
            ViewState["REPORTWORKYN"] = ds.Tables[0].Rows[0]["FLDREPORTWORKYN"].ToString();
        }

        return status;
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("General", "GENERAL");
        toolbarmain.AddButton("Details", "DETAILS");
        toolbarmain.AddButton("Attachment", "ATTACHMENT");
        toolbarmain.AddButton("Job Description", "JOBDETAILS");
        toolbarmain.AddButton("Parts Required", "PARTREQUIRED");
        toolbarmain.AddButton("Postponement", "POSTPONEMENT");

        if (ViewState["WORKORDERID"] != null && (",501,26,").Contains("," + GetWorkOrderStatus(new Guid(ViewState["WORKORDERID"].ToString())) + ",")&& ViewState["REPORTWORKYN"]!=null && ViewState["REPORTWORKYN"].ToString().ToUpper().Equals("Y"))
        {
            toolbarmain.AddButton("Report Work", "REPORTWORK");
        }
        if (Request.QueryString["from"] != null)
        {
            toolbarmain.AddButton("Back", "BACK");
        }
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            MenuWorkOrder.SelectedMenuIndex = 0;
        else
            SetTabHighlight();
    }
    private void CreateFilterCriteria()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtWorkOrderNumber", Request.QueryString["wno"] != null ? Request.QueryString["wno"] : string.Empty);
        criteria.Add("txtWorkOrderName", Request.QueryString["wname"] != null ? Request.QueryString["wname"] : string.Empty);
        criteria.Add("txtComponentNumber", Request.QueryString["cno"] != null ? Request.QueryString["cno"] : string.Empty);
        criteria.Add("txtComponentName", Request.QueryString["cname"] != null ? Request.QueryString["cno"] : string.Empty);
        criteria.Add("ucRank", string.Empty);
        criteria.Add("txtDateFrom", string.Empty);
        criteria.Add("txtDateTo", string.Empty);
        criteria.Add("status", string.Empty);
        criteria.Add("planning", string.Empty);
        criteria.Add("jobclass", string.Empty);
        criteria.Add("ucMainType", string.Empty);
        criteria.Add("ucMainCause", string.Empty);
        criteria.Add("ucMaintClass", string.Empty);
        criteria.Add("chkUnexpected", string.Empty);
        criteria.Add("txtPriority", string.Empty);
        criteria.Add("chkDefect", string.Empty);
        criteria.Add("txtClassCode", string.Empty);
        Filter.CurrentWorkOrderFilter = criteria;
    }

    protected void gvWorkOrder_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                //if (e.Item.RowType == DataControlRowType.DataRow)
                //{
                string sStatus = ((RadLabel)e.Item.FindControl("lblStatus")) != null ? ((RadLabel)e.Item.FindControl("lblStatus")).Text.Trim() : string.Empty;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, "DELETE") && sStatus.Trim().Equals("Cancelled");
                }
                LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");

                if (cmdCancel != null)
                {
                    cmdCancel.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, "CANCEL") && !sStatus.Trim().Equals("Cancelled");
                }
                LinkButton app = (LinkButton)e.Item.FindControl("cmdApprove");
                if (drv["FLDAPPROVAL"].ToString() == "0")
                {
                    app.Visible = false;
                }
                else
                {
                    app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);
                }
                Image imgFlag = e.Item.FindControl("imgFlag") as Image;
                if (drv["FLDDUESTATUS"].ToString() != "0")
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + (drv["FLDDUESTATUS"].ToString() == "1" ? "yellow-symbol.png" : (drv["FLDDUESTATUS"].ToString() == "3" ? "red-symbol.png" : "yellow-symbol.png"));
                    imgFlag.ToolTip = (drv["FLDDUESTATUS"].ToString() == "1" ? "Due" : (drv["FLDDUESTATUS"].ToString() == "3" ? "Overdue" : "Due"));
                }
                else
                    imgFlag.Visible = false;

                //ImageButton resapp = (ImageButton)e.Row.FindControl("cmdApproveReschedule");
                //if (drv["FLDPENDINGAPPROVAL"].ToString() == "0" && resapp != null) resapp.Visible = false;

                //ImageButton send = (ImageButton)e.Row.FindControl("cmdSendOffice");
                //if (drv["FLDAPPROVALREQUIRED"].ToString() == "0" && send != null) send.Visible = false;

                Image imgApproval = e.Item.FindControl("imgApproval") as Image;
                imgApproval.Visible = false;
                if (drv["FLDPENDINGAPPROVAL"].ToString() == "1")
                {
                    imgApproval.Visible = true;
                    imgApproval.ImageUrl = Session["images"] + "/14.png";
                }

                ImageButton attachments = (ImageButton)e.Item.FindControl("Attachments");
                if (attachments != null)
                {
                    attachments.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDCOMPJOBDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&u=n'); return false;");
                    if (drv["FLDCOMPJOBDTKEY"].ToString().Equals(""))
                    {
                        attachments.Visible = false;
                    }
                    if (drv["FLDISCOMPJOBATTACHMENT"].ToString() != "1")
                    {
                        attachments.Visible = false;
                    }
                }
                Image imgRaPending = e.Item.FindControl("imgRaPending") as Image;
                imgRaPending.Visible = false;
                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "6")
                {
                    imgRaPending.Visible = true;
                    imgRaPending.ImageUrl = Session["images"] + "/red.png";
                    imgRaPending.ToolTip = "RA Rejected";
                }
                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "5")
                {
                    imgRaPending.Visible = true;
                    imgRaPending.ImageUrl = Session["images"] + "/green.png";
                    imgRaPending.ToolTip = "RA Approved for use";
                }
                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "4")
                {
                    imgRaPending.Visible = true;
                    imgRaPending.ImageUrl = Session["images"] + "/yellow.png";
                    imgRaPending.ToolTip = "RA Pending Approval";
                }
            }
            //}
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

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixPlannedMaintenanceWorkOrder.WorkOrderApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text));
                ResetMenu();
                gvWorkOrder.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceWorkOrder.DeleteWorkOrder(new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text),
                                       PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                       );
                ViewState["WORKORDERID"] = null;
                gvWorkOrder.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                PhoenixPlannedMaintenanceWorkOrder.CancelWorkOrder(new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );
                ViewState["WORKORDERID"] = null;
                gvWorkOrder.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.ApproveWorkOrderReschedule(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text), 1);
                ResetMenu();
                gvWorkOrder.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("APPROVERESCHEDULE"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.ApproveWorkOrderReschedule(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text), 0);
                ResetMenu();
                gvWorkOrder.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                ViewState["COMPONENTID"] = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                ViewState["JOBID"] = ((RadLabel)e.Item.FindControl("lblJobID")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                ViewState["WORKORDERNO"] = ((RadLabel)e.Item.FindControl("lblWorkorderNo")).Text;
                ViewState["PAGENUMBER"] = gvWorkOrder.CurrentPageIndex;
                ResetMenu();
                BindUrl();
            }
            if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {

                ViewState["COMPONENTID"] = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                ViewState["JOBID"] = ((RadLabel)e.Item.FindControl("lblJobID")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                ViewState["WORKORDERNO"] = ((RadLabel)e.Item.FindControl("lblWorkorderNo")).Text;
                ViewState["PAGENUMBER"] = gvWorkOrder.CurrentPageIndex;
                ResetMenu();
                BindUrl();
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
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=";
        }

        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
        {
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&JOBID=" + ViewState["JOBID"];
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderPartsUsed.aspx"))
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&COMPONENTID=" + ViewState["COMPONENTID"];
        }
        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRA.aspx"))
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"] + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
        }
    }


    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
