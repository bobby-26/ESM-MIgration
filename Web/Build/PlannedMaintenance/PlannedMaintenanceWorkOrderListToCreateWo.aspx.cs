using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inventory;
using System.Web.UI;

public partial class PlannedMaintenanceWorkOrderListToCreateWo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderListToCreateWo.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderListToCreateWo.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-stack-2x\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderListToCreateWo.aspx", "Create Work Order", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                //if (Request.QueryString["from"] == "machinery")
                //{
                //    CreateFilterCriteria();
                //    ViewState["URLREFERER"] = Request.UrlReferrer.ToString();
                //}
                ////cmdHiddenSubmit.Attributes.Add("style", "display:none");
                //ViewState["ISTREENODECLICK"] = false;
                //ViewState["PAGENUMBER"] = 1;
                //ViewState["SORTEXPRESSION"] = null;
                //ViewState["SORTDIRECTION"] = null;
                //ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["WIEVCOMPONENTID"] = "";
                //ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                //ViewState["JOBID"] = null;
                //ViewState["WORKORDERNO"] = "";
                //if (Request.QueryString["COMPONENTID"] != null)
                //    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                //if (Request.QueryString["WORKORDERID"] != null)
                //{
                //    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                //}
                //if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
                //{
                //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=";
                //    ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
                //}
                //if (Request.QueryString["FromRATemplate"] != null && Request.QueryString["FromRATemplate"] == "1")
                //{
                //    ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
                //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&WORKORDERID=";

                //}
                //BindData();
                //ResetMenu();
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        //try
        //{
        //    int iRowCount = 0;
        //    int iTotalPageCount = 0;
        //    string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
        //    string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };
        //    string sortexpression;
        //    int? sortdirection = null;

        //    sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //    if (ViewState["SORTDIRECTION"] != null)
        //        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //    if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //        iRowCount = 10;
        //    else
        //        iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        //    DataSet ds;
        //    if (Filter.CurrentWorkOrderFilter != null)
        //    {
        //        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        //        ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
        //            , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
        //            , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString()), General.GetNullableString(nvc.Get("txtComponentNumber").ToString())
        //            , General.GetNullableString(nvc.Get("txtComponentName").ToString()), General.GetNullableString(nvc.Get("planning").ToString())
        //            , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableString(nvc.Get("txtClassCode"))
        //            , General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString()), General.GetNullableDateTime(nvc.Get("txtDateTo").ToString())
        //            , General.GetNullableInteger(nvc.Get("ucMainType").ToString()), General.GetNullableInteger(nvc.Get("ucMaintClass").ToString())
        //            , General.GetNullableInteger(nvc.Get("ucMainCause").ToString()), General.GetNullableInteger(nvc.Get("chkUnexpected").ToString())
        //            , General.GetNullableString(nvc.Get("status").ToString()), General.GetNullableInteger(nvc.Get("txtPriority").ToString())
        //            , nvc.Get("ucRank").ToString(), (byte?)General.GetNullableInteger(nvc.Get("chkDefect"))
        //            , sortexpression, sortdirection, gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount
        //            , (byte?)General.GetNullableInteger(nvc.Get("chkRaPendingApproval"))
        //            , (byte?)General.GetNullableInteger(nvc.Get("chkRaRequired")));
        //    }
        //    else
        //    {
        //        ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString()), null, null, null, null, null
        //            , null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
        //                     gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount, null, null);
        //    }
        //    General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
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
                BindData();
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
            else if (CommandName.ToUpper().Equals("CREATE"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupCreate.aspx?vslid=" + 1
                    + "&clist=" + 1 + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
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
            //int iRowCount = 0;
            //int iTotalPageCount = 0;

            //string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            //string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            //int? sortdirection = null;
            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //DataSet ds;
            //if (Filter.CurrentWorkOrderFilter != null)
            //{
            //    NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
            //    ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            //        , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
            //        , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
            //        , General.GetNullableString(nvc.Get("txtComponentNumber").ToString()),
            //        General.GetNullableString(nvc.Get("txtComponentName").ToString()),
            //        General.GetNullableString(nvc.Get("planning").ToString())
            //        , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableString(nvc.Get("txtClassCode"))
            //        , General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString())
            //        , General.GetNullableDateTime(nvc.Get("txtDateTo").ToString()), General.GetNullableInteger(nvc.Get("ucMainType").ToString())
            //        , General.GetNullableInteger(nvc.Get("ucMaintClass").ToString()), General.GetNullableInteger(nvc.Get("ucMainCause").ToString())
            //        , General.GetNullableInteger(nvc.Get("chkUnexpected").ToString()), General.GetNullableString(nvc.Get("status").ToString())
            //        , General.GetNullableInteger(nvc.Get("txtPriority").ToString()), nvc.Get("ucRank").ToString(), (byte?)General.GetNullableInteger(nvc.Get("chkDefect")),
            //            sortexpression, sortdirection,
            //            gvWorkOrder.CurrentPageIndex + 1,
            //            gvWorkOrder.PageSize,
            //            ref iRowCount,
            //            ref iTotalPageCount
            //            , (byte?)General.GetNullableInteger(nvc.Get("chkRaPendingApproval"))
            //            , (byte?)General.GetNullableInteger(nvc.Get("chkRaRequired")));
            //}
            //else
            //{
            //    ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
            //        , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
            //                 gvWorkOrder.CurrentPageIndex + 1,
            //                 gvWorkOrder.PageSize,
            //                 ref iRowCount,
            //                 ref iTotalPageCount
            //                 , null
            //                 , null);
            //}
            //General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            //gvWorkOrder.DataSource = ds;
            //gvWorkOrder.VirtualItemCount = iRowCount;

            //ViewState["ROWCOUNT"] = iRowCount;


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

    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        RadToolBar dl = (RadToolBar)MenuWorkOrder.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderGeneral.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderDetail.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 1;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 2;
    //            }
    //            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobDetail.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 3;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderPartsUsed.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 4;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRA.aspx"))
    //            {
    //                MenuWorkOrder.SelectedMenuIndex = 5;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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
    //private string GetWorkOrderStatus(Guid gWorkOrderId)
    //{
    //    return PhoenixPlannedMaintenanceWorkOrder.EditWorkOrder(gWorkOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDWORKORDERSTATUS"].ToString();
    //}
    //private void ResetMenu()
    //{
    //    PhoenixToolbar toolbarmain = new PhoenixToolbar();
    //    toolbarmain.AddButton("General", "GENERAL");
    //    toolbarmain.AddButton("Details", "DETAILS");

    //    MenuWorkOrder.AccessRights = this.ViewState;
    //    MenuWorkOrder.MenuList = toolbarmain.Show();
    //    //MenuWorkOrder.SetTrigger(pnlComponent);
    //    //MenuWorkOrder.SelectedMenuIndex = 0;
    //}
    //private void CreateFilterCriteria()
    //{
    //    NameValueCollection criteria = new NameValueCollection();
    //    criteria.Clear();
    //    criteria.Add("txtWorkOrderNumber", Request.QueryString["wno"] != null ? Request.QueryString["wno"] : string.Empty);
    //    criteria.Add("txtWorkOrderName", Request.QueryString["wname"] != null ? Request.QueryString["wname"] : string.Empty);
    //    criteria.Add("txtComponentNumber", Request.QueryString["cno"] != null ? Request.QueryString["cno"] : string.Empty);
    //    criteria.Add("txtComponentName", Request.QueryString["cname"] != null ? Request.QueryString["cno"] : string.Empty);
    //    criteria.Add("ucRank", string.Empty);
    //    criteria.Add("txtDateFrom", string.Empty);
    //    criteria.Add("txtDateTo", string.Empty);
    //    criteria.Add("status", string.Empty);
    //    criteria.Add("planning", string.Empty);
    //    criteria.Add("jobclass", string.Empty);
    //    criteria.Add("ucMainType", string.Empty);
    //    criteria.Add("ucMainCause", string.Empty);
    //    criteria.Add("ucMaintClass", string.Empty);
    //    criteria.Add("chkUnexpected", string.Empty);
    //    criteria.Add("txtPriority", string.Empty);
    //    criteria.Add("chkDefect", string.Empty);
    //    criteria.Add("txtClassCode", string.Empty);
    //    Filter.CurrentWorkOrderFilter = criteria;
    //}

    //protected void gvWorkOrder_DeleteCommand(object sender, GridCommandEventArgs e)
    //{

    //    try
    //    {
    //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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

    //protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridDataItem)
    //        {
    //            GridDataItem item = (GridDataItem)e.Item;
    //            DataRowView drv = (DataRowView)e.Item.DataItem;

    //            //if (e.Item.RowType == DataControlRowType.DataRow)
    //            //{
    //                string sStatus = ((RadLabel)e.Item.FindControl("lblStatus")) != null ? ((RadLabel)e.Item.FindControl("lblStatus")).Text.Trim() : string.Empty;
    //                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
    //                if (db != null)
    //                {
    //                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                    db.Visible = SessionUtil.CanAccess(this.ViewState, "DELETE") && sStatus.Trim().Equals("Cancelled");
    //                }
    //                ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");

    //                if (cmdCancel != null)
    //                {
    //                    cmdCancel.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                    cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, "CANCEL") && !sStatus.Trim().Equals("Cancelled");
    //                }
    //                ImageButton app = (ImageButton)e.Item.FindControl("cmdApprove");
    //                if (drv["FLDAPPROVAL"].ToString() == "0")
    //                {
    //                    app.Visible = false;
    //                }
    //                else
    //                {
    //                    app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);
    //                }
    //                Image imgFlag = e.Item.FindControl("imgFlag") as Image;
    //                if (drv["FLDDUESTATUS"].ToString() != "0")
    //                {
    //                    imgFlag.Visible = true;
    //                    imgFlag.ImageUrl = Session["images"] + "/" + (drv["FLDDUESTATUS"].ToString() == "1" ? "yellow-symbol.png" : (drv["FLDDUESTATUS"].ToString() == "3" ? "red-symbol.png" : "yellow-symbol.png"));
    //                    imgFlag.ToolTip = (drv["FLDDUESTATUS"].ToString() == "1" ? "Due" : (drv["FLDDUESTATUS"].ToString() == "3" ? "Overdue" : "Due"));
    //                }
    //                else
    //                    imgFlag.Visible = false;

    //                //ImageButton resapp = (ImageButton)e.Row.FindControl("cmdApproveReschedule");
    //                //if (drv["FLDPENDINGAPPROVAL"].ToString() == "0" && resapp != null) resapp.Visible = false;

    //                //ImageButton send = (ImageButton)e.Row.FindControl("cmdSendOffice");
    //                //if (drv["FLDAPPROVALREQUIRED"].ToString() == "0" && send != null) send.Visible = false;

    //                Image imgApproval = e.Item.FindControl("imgApproval") as Image;
    //                imgApproval.Visible = false;
    //                if (drv["FLDPENDINGAPPROVAL"].ToString() == "1")
    //                {
    //                    imgApproval.Visible = true;
    //                    imgApproval.ImageUrl = Session["images"] + "/14.png";
    //                }

    //                ImageButton attachments = (ImageButton)e.Item.FindControl("Attachments");
    //                if (attachments != null)
    //                {
    //                    attachments.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDCOMPJOBDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&u=n'); return false;");
    //                    if (drv["FLDCOMPJOBDTKEY"].ToString().Equals(""))
    //                    {
    //                        attachments.Visible = false;
    //                    }
    //                    if (drv["FLDISCOMPJOBATTACHMENT"].ToString() != "1")
    //                    {
    //                        attachments.Visible = false;
    //                    }
    //                }
    //                Image imgRaPending = e.Item.FindControl("imgRaPending") as Image;
    //                imgRaPending.Visible = false;
    //                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "6")
    //                {
    //                    imgRaPending.Visible = true;
    //                    imgRaPending.ImageUrl = Session["images"] + "/red.png";
    //                    imgRaPending.ToolTip = "RA Rejected";
    //                }
    //                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "5")
    //                {
    //                    imgRaPending.Visible = true;
    //                    imgRaPending.ImageUrl = Session["images"] + "/green.png";
    //                    imgRaPending.ToolTip = "RA Approved for use";
    //                }
    //                if (drv["FLDRISKASSESSMENTSTATUS"].ToString() == "4")
    //                {
    //                    imgRaPending.Visible = true;
    //                    imgRaPending.ImageUrl = Session["images"] + "/yellow.png";
    //                    imgRaPending.ToolTip = "RA Pending Approval";
    //                }
    //            }
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}

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

                BindData();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceWorkOrder.DeleteWorkOrder(new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text),
                                       PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                       );
                ViewState["WORKORDERID"] = null;
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                PhoenixPlannedMaintenanceWorkOrder.CancelWorkOrder(new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );
                ViewState["WORKORDERID"] = null;
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.ApproveWorkOrderReschedule(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text), 1);

                BindData();
            }
            if (e.CommandName.ToUpper().Equals("APPROVERESCHEDULE"))
            {
                PhoenixPlannedMaintenanceWorkOrderReschedule.ApproveWorkOrderReschedule(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text), 0);

                BindData();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;

                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


}
