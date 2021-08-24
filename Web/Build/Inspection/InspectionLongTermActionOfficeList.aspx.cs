using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionLongTermActionOfficeList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvLongTermAction.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvLongTermAction.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionLongTermActionOfficeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel"); //<span class="icon"><i class="fas fa-tasks"></i></span>
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLongTermAction')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionLongTermActionFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionLongTermActionOfficeList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeManualTasksGeneral.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionBulkPreventiveTaskCompletion.aspx?'); return false;", "Bulk Task Completion", "<i class=\"fas fas fa-tasks\"></i>", "BULKOFFICECOMMENTS");
        }

        MenuLongTermAction.AccessRights = this.ViewState;
        MenuLongTermAction.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("RA Task", "RATASK", ToolBarDirection.Right);
        toolbar.AddButton("MOC Task", "MOCTASK", ToolBarDirection.Right);
        toolbar.AddButton("Preventive Task", "PATASK", ToolBarDirection.Right);
        toolbar.AddButton("Corrective Task", "CARTASK", ToolBarDirection.Right);
        //toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);

        MenuOrderFormMain.AccessRights = this.ViewState;
        MenuOrderFormMain.MenuList = toolbar.Show();

        MenuOrderFormMain.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            Session["New"] = "N";
            if (Filter.CurrentSelectedOfficeTasks != null)
                Filter.CurrentSelectedOfficeTasks = null;
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            VesselConfiguration();

            DateTime now = DateTime.Now;
            string FromDate = now.Date.AddMonths(-3).ToShortDateString();
            string ToDate = now.Date.AddMonths(+3).ToShortDateString();            

            ViewState["FROMDATE"] = FromDate.ToString();
            ViewState["TODATE"] = ToDate.ToString();            

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Accept Task", "ACCEPT");
            toolbar.AddButton("Generate Work Order", "GENERATEWO");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["TASKCOMPANYID"] = string.Empty;

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["TASKCOMPANYID"] = nvcCompany.Get("QMS");
            }

            gvLongTermAction.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentOfficeTaskFilter = null;
            BindData();
            gvLongTermAction.Rebind();
            SetRowSelection();
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionOfficeTaskFilter.aspx", true);
        }
        if (CommandName.ToUpper().Equals("MOCTASK"))
        {
            Response.Redirect("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("RATASK"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveRATasks.aspx", true);
        }
    }

    //protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //    try
    //    {
    //        if (dce.CommandName.ToUpper().Equals("ACCEPT"))
    //        {
    //            string selectedagents = ",";
    //            if (Filter.CurrentSelectedOfficeTasks != null)
    //            {
    //                ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
    //                if (SelectedPvs != null && SelectedPvs.Count > 0)
    //                {
    //                    foreach (Guid index in SelectedPvs)
    //                    {
    //                        selectedagents = selectedagents + index + ",";
    //                    }
    //                    PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
    //                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                        , selectedagents
    //                        , 1);

    //                    if (Filter.CurrentSelectedOfficeTasks != null)
    //                        Filter.CurrentSelectedOfficeTasks = null;

    //                    ucStatus.Text = "Task is accepted.";
    //                    BindData();
    //                }
    //            }
    //        }

    //        if (dce.CommandName.ToUpper().Equals("GENERATEWO"))
    //        {
    //            if (!IsValidClose())
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            String scriptpopup = String.Format(
    //               "javascript:openNewWindow('codehelp1', '', '../Inspection/InspectionLongTermWorkOrderDescription.aspx','medium');");
    //            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

    //            //string selectedagents = ",";
    //            //if (Filter.CurrentSelectedOfficeTasks != null)
    //            //{
    //            //    ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
    //            //    if (SelectedPvs != null && SelectedPvs.Count > 0)
    //            //    {
    //            //        foreach (Guid index in SelectedPvs)
    //            //        {
    //            //            selectedagents = selectedagents + index + ",";
    //            //        }
    //            //        PhoenixInspectionLongTermAction.WorkOrderInsert(
    //            //              PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //            //            , null
    //            //            , selectedagents
    //            //            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

    //            //        if (Filter.CurrentSelectedOfficeTasks != null)
    //            //            Filter.CurrentSelectedOfficeTasks = null;

    //            //        ucStatus.Text = "Work Order is generated.";
    //            //        BindData();
    //            //    }
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Filter.CurrentSelectedOfficeTasks = null;
    //        //BindData();
    //        //SetPageNavigator();
    //        //ucError.ErrorMessage = ex.Message;
    //        //ucError.Visible = true;
    //    }
    //}

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDTYPE", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME",
                                 "FLDSTATUSNAME", "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDWONUMBER" };
        string[] alCaptions = { "Vessel", "Source", "Type", "Task", "Category", "Sub Category", "Task Status", "Assigned Department", "Sub Department", "Accepted by",
                                  "Target", "Completed", "Work Order Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentOfficeTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlSubcategory")) : null
                                                                , null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvLongTermAction.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptedBy")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoFrom")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoTo")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkManualTaskYN")) : null
                                                                , General.GetNullableInteger(ViewState["TASKCOMPANYID"].ToString()));

        General.SetPrintOptions("gvLongTermAction", "Office Task List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvLongTermAction.DataSource = ds;
            //gvLongTermAction.DataBind();

            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONPREVENTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                // gvLongTermAction.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvLongTermAction);
        }
        gvLongTermAction.DataSource = ds;
        gvLongTermAction.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDTYPE", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME",
                                 "FLDSTATUSNAME", "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDWONUMBER" };
        string[] alCaptions = { "Vessel", "Source", "Type", "Task", "Category", "Sub Category", "Task Status", "Assigned Department", "Sub Department", "Accepted by",
                                  "Target", "Completed", "Work Order Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentOfficeTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlSubcategory")) : null
                                                                , null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptedBy")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoFrom")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoTo")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkManualTaskYN")) : null
                                                                , General.GetNullableInteger(ViewState["TASKCOMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=OfficeTaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Office Task List</h3></td>");
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

    private void SetRowSelection()
    {
        //gvLongTermAction.SelectedIndex = -1;

        //for (int i = 0; i < gvLongTermAction.Rows.Count; i++)
        //{
        //    if (gvLongTermAction.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
        //    {
        //        gvLongTermAction.SelectedIndex = i;
        //        //ViewState["WORKORDERID"] = ((RadLabel)gvLongTermAction.Rows[gvLongTermAction.SelectedIndex].FindControl("lblWorkOrderId")).Text;
        //    }
        //}
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvLongTermAction_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        // BindData();

        gvLongTermAction.Rebind();
    }

    protected void gvLongTermAction_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            //e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLongTermAction, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    //protected void gvLongTermAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvLongTermAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("TASK"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    RadLabel lblLongTermActionId = (RadLabel)e.Item.FindControl("lblLongTermActionId");
                    RadLabel lblSourceType = (RadLabel)e.Item.FindControl("lblSourceType");
                    RadCheckBox chkselect = (RadCheckBox)e.Item.FindControl("chkSelect");

                    if (lblSourceType != null)
                    {
                        if (lblLongTermActionId != null)
                        {
                            if (lblSourceType.Text == "7")
                            {
                                if (((CheckBox)e.Item.FindControl("chkmanualtask")).Checked)
                                    Response.Redirect("../Inspection/InspectionOfficeManualTasksGeneral.aspx?preventiveactionid=" + lblLongTermActionId.Text);
                                else
                                    Response.Redirect("../Inspection/InspectionOfficeManualTasksGeneralEdit.aspx?preventiveactionid=" + lblLongTermActionId.Text);
                            }
                            else
                            {
                                Response.Redirect("../Inspection/InspectionOfficeTasksDetails.aspx?preventiveactionid=" + lblLongTermActionId.Text);
                            }
                        }
                    }
                }
                if (e.CommandName.ToUpper().Equals("SHOWSOURCE"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
                else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
                if (e.CommandName.ToUpper().Equals("FILEEDIT"))
                {
                    RadLabel lblsectionno = (RadLabel)e.Item.FindControl("lblsectionno");
                    RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblDocumentId");
                    RadLabel lblformid = (RadLabel)e.Item.FindControl("lblformid");
                    RadLabel lblformno = (RadLabel)e.Item.FindControl("lblformno");
                    RadLabel lblsectionid = (RadLabel)e.Item.FindControl("lblsectionid");

                    if ((lblsectionid.Text == "") && (lblDocumentId.Text != "") && (lblformid.Text == "") && (lblformno.Text == ""))
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + lblDocumentId.Text, false);
                    }
                    if ((lblDocumentId.Text != "") && (lblformno.Text != ""))
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMNO=" + lblformno.Text.Trim(), false);
                    }

                }                
            }
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
                gvLongTermAction.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvLongTermAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //}

    //protected void gvLongTermAction_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    BindData();
    //}

    protected void gvLongTermAction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadLabel lblWorkOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            CheckBox chkmanualtask = (CheckBox)e.Item.FindControl("chkmanualtask");
            RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            RadLabel lblVesselsid = (RadLabel)e.Item.FindControl("lblVessel");
            RadLabel lblPreventiveActionid = (RadLabel)e.Item.FindControl("lblLongTermActionId");

            RadLabel lblTargetDate = (RadLabel)e.Item.FindControl("lblTargetDate");

            if (dr["FLDOVERDUEYN"].ToString().Equals("1"))
            {
                lblTargetDate.Attributes["style"] = "color:Red !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Overdue";
            }
            else if (dr["FLDOVERDUEYN"].ToString().Equals("2"))
            {
                lblTargetDate.Attributes["style"] = "color:darkviolet !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Postponed";
            }

            if (Communication != null)
            {
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=TASK" + "&Referenceid=" + lblPreventiveActionid.Text + "&Vesselid=" + lblVesselsid.Text + "','large'); return true;");
            }
            if (lblStatusId != null && (lblStatusId.Text == "2" || lblStatusId.Text == "3" || lblStatusId.Text == "4"))
            {
                if (chkSelect != null)
                    chkSelect.Visible = false;
            }

            LinkButton lnkTask = (LinkButton)e.Item.FindControl("lnkTask");
            UserControlToolTip ucToolTip = (UserControlToolTip)e.Item.FindControl("ucToolTip");

            if (lnkTask != null && ucToolTip != null)
            {
                ucToolTip.Position = ToolTipPosition.TopCenter;
                ucToolTip.TargetControlId = lnkTask.ClientID;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");
            //DataRowView drv = (DataRowView)e.Item.DataItem;
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();

                if (dr["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVessel.Text + "'); return true;");
            }

            LinkButton lnkTaskSource = (LinkButton)e.Item.FindControl("lnkTaskSource");
            RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
            RadLabel lblSourceType = (RadLabel)e.Item.FindControl("lblSourceType");
            RadLabel lblVesslName = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblLongTermActionId = (RadLabel)e.Item.FindControl("lblLongTermActionId");
            LinkButton lnkSubCategory = (LinkButton)e.Item.FindControl("lnkSubCategory");
            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");
            RadLabel lblCategoryShortCode = (RadLabel)e.Item.FindControl("lblCategoryShortCode");

            if (lnkSubCategory != null && lblCategoryShortCode.Text == "RCH")
            {
                lnkSubCategory.Visible = true;
                lblSubCategory.Visible = false;
            }
            else
            {
                lblSubCategory.Visible = true;
            }


            if (lnkTaskSource != null)
            {
                if (lblSourceType.Text == "1") //Open Reports
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "2") //Direct
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "3") // Audit/Inspection
                {
                    if (lblVesslName.Text.ToUpper().Equals("OFFICE"))
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    else
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                }
                if (lblSourceType.Text == "4") //Vetting
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "5") //Incident
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionIncidentView.aspx?incidentid=" + lblSourceId.Text + "'); return true;");
                }
                if (lblSourceType.Text == "6") //machinery damage
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageGeneral.aspx?VIEWONLY=1&MACHINERYDAMAGEID=" + lblSourceId.Text + "'); return true;");
                }
                //if (lblSourceType.Text == "7") //manual office task
                //{
                //    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionOfficeManualTasksGeneralEdit.aspx?preventiveactionid=" + lblSourceId.Text + "'); return true;");
                //}

                if (lblSourceType.Text == "7") //manual office task
                {
                    if (chkmanualtask.Checked)
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionOfficeManualTasksGeneral.aspx?VIEWONLY=1&preventiveactionid=" + lblSourceId.Text + "'); return true;");
                    else
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionOfficeManualTasksGeneralEdit.aspx?VIEWONLY=1&preventiveactionid=" + lblSourceId.Text + "'); return true;");
                }
            }
            LinkButton lnkWorkOrder = (LinkButton)e.Item.FindControl("lnkWorkOrderNumber");
            RadLabel lblWorkOrderID = (RadLabel)e.Item.FindControl("lblWorkOrderId");

            if (lnkWorkOrder != null)
            {
                lnkWorkOrder.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text + "&TASKID=" + ViewState["TASKID"] + "&viewonly=1'); return true;");
            }

            RadLabel lblsectionno = (RadLabel)e.Item.FindControl("lblsectionno");
            RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblDocumentId");
            RadLabel lblformid = (RadLabel)e.Item.FindControl("lblformid");
            RadLabel lblformno = (RadLabel)e.Item.FindControl("lblformno");
            RadLabel lblsectionid = (RadLabel)e.Item.FindControl("lblsectionid");
            RadLabel lblRevisionId = (RadLabel)e.Item.FindControl("lblRevisionId");
            RadLabel lblRevisionNumber = (RadLabel)e.Item.FindControl("lblRevisionNumber");
            RadLabel lblRevisionStatus = (RadLabel)e.Item.FindControl("lblRevisionStatus");

            if (lnkSubCategory != null && lblCategoryShortCode.Text == "RCH")
            {

                if (lblsectionid != null && lblsectionid.Text != "" && lblRevisionNumber.Text != "" && lblRevisionStatus.Text == "4" && ((lblformid.Text == "") && (lblformno.Text == "") || (lblformno.Text != "")))
                {

                    lnkSubCategory.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentEditReason.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblsectionid.Text + "&REVISIONID=" + lblRevisionId.Text + "'); return false;");

                    //lnkSubCategory.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONNO=" + lblsectionno.Text.Trim() + "'); return true;");
                    //Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONNO=" + lblsectionno.Text.Trim(), false);
                }

                if (lblsectionid != null && lblsectionid.Text != "" && lblRevisionNumber.Text == "" && lblRevisionStatus.Text != "3" && ((lblformid.Text == "") && (lblformno.Text == "") || (lblformno.Text != "")))
                {
                    lnkSubCategory.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblsectionid.Text + "'); return false;");
                }

            }


            LinkButton cmdShowStatus = (LinkButton)e.Item.FindControl("cmdShowStatus");
            if (cmdShowStatus != null)
            {
                cmdShowStatus.Attributes.Add("onclick", "javascript:openNewWindow('status','','" + Session["sitepath"] + "/Inspection/InspectionShipboardTaskStatus.aspx?preventiveactionid=" + lblLongTermActionId.Text + "'); return true;");
                cmdShowStatus.Visible = SessionUtil.CanAccess(this.ViewState, cmdShowStatus.CommandName);
            }
        }
    }

    protected void gvLongTermAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblLongTermActionId = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblLongTermActionId"));
            RadLabel lblDTKey = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblDTKey"));
            RadLabel lblWorkorderId = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblWorkOrderId"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
            //if (lblWorkorderId != null)
            //    ViewState["WORKORDERID"] = lblWorkorderId.Text;
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
            if (Session["NewTask"] != null && Session["NewTask"].ToString() == "Y")
            {
                ViewState["TASKID"] = null;
                Session["NewTask"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            gvLongTermAction.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvLongTermAction.SelectedIndex = -1;
    //    gvLongTermAction.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //    GetSelectedPvs();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    //public StateBag ReturnViewState()
    //{
    //    return ViewState;
    //}

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvLongTermAction$ctl01$chkAllRemittance")
        {
            RadCheckBox chkAll = (RadCheckBox)gvLongTermAction.FindControl("chkAllRemittance");
            foreach (GridDataItem row in gvLongTermAction.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvLongTermAction.Items)
        {
            bool result = false;
            index = new Guid(gvrow.GetDataKeyValue("FLDINSPECTIONPREVENTIVEACTIONID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedOfficeTasks != null)
                SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Filter.CurrentSelectedOfficeTasks = SelectedPvs;
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedOfficeTasks != null)
        {
            ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridDataItem row in gvLongTermAction.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvLongTermAction.MasterTableView.Items[0].GetDataKeyValue("FLDINSPECTIONPREVENTIVEACTIONID").ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    //protected void selection_Changed(Object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //private bool IsValidClose()
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    if (General.GetNullableInteger(ucDepartment.SelectedDepartment) == null)
    //        ucError.ErrorMessage = "Department is required.";

    //    return (!ucError.IsError);
    //}


    protected void gvLongTermAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLongTermAction.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
