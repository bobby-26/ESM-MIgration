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

public partial class InspectionLongTermActionList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLongTermAction.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvLongTermAction.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

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

            //DateTime now = DateTime.Now;
            //string FromDate = now.Date.AddMonths(-6).ToShortDateString();
            //string ToDate = DateTime.Now.ToShortDateString();

            //ViewState["FROMDATE"] = FromDate.ToString();
            //ViewState["TODATE"] = ToDate.ToString();

            VesselConfiguration();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            toolbar.AddImageButton("../Inspection/InspectionLongTermActionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvLongTermAction')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("javascript:Openpopup('Filter','','InspectionLongTermActionPreventiveTaskFilter.aspx'); return false;", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionLongTermActionList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddImageButton("../Inspection/InspectionOfficeManualTasksGeneral.aspx", "Add", "Add.png", "ADD");

            MenuLongTermAction.AccessRights = this.ViewState;
            MenuLongTermAction.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Search", "SEARCH");
            toolbar.AddButton("Corrective Task", "CARTASK");
            toolbar.AddButton("Preventive Task", "PATASK");
            //toolbar.AddButton("MOC Task", "MOCTASK");
            //toolbar.AddButton("Work Order", "WORKORDER");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbar.Show();

            MenuOrderFormMain.SelectedMenuIndex = 2;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Accept Task", "ACCEPT");
            toolbar.AddButton("Generate Work Order", "GENERATEWO");

            MenuBulkPO.AccessRights = this.ViewState;
            MenuBulkPO.MenuList = toolbar.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;
            //ddlAcceptance.SelectedValue = "0";   
        }
        BindData();
        SetPageNavigator();
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentOfficeTaskFilter = null;
            BindData();
            SetPageNavigator();
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionOfficeTaskFilter.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("MOCTASK"))
        {
            Response.Redirect("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", true);
        }
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("ACCEPT"))
            {
                string selectedagents = ",";
                if (Filter.CurrentSelectedOfficeTasks != null)
                {
                    ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                        {
                            selectedagents = selectedagents + index + ",";
                        }
                        PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , selectedagents
                            , 1);

                        if (Filter.CurrentSelectedOfficeTasks != null)
                            Filter.CurrentSelectedOfficeTasks = null;

                        ucStatus.Text = "Task is accepted.";
                        BindData();
                    }
                }
            }

            if (dce.CommandName.ToUpper().Equals("GENERATEWO"))
            {
                if (!IsValidClose())
                {
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:parent.Openpopup('codehelp1', '', '../Inspection/InspectionLongTermWorkOrderDescription.aspx','medium');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                //string selectedagents = ",";
                //if (Filter.CurrentSelectedOfficeTasks != null)
                //{
                //    ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedOfficeTasks;
                //    if (SelectedPvs != null && SelectedPvs.Count > 0)
                //    {
                //        foreach (Guid index in SelectedPvs)
                //        {
                //            selectedagents = selectedagents + index + ",";
                //        }
                //        PhoenixInspectionLongTermAction.WorkOrderInsert(
                //              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //            , null
                //            , selectedagents
                //            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

                //        if (Filter.CurrentSelectedOfficeTasks != null)
                //            Filter.CurrentSelectedOfficeTasks = null;

                //        ucStatus.Text = "Work Order is generated.";
                //        BindData();
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            Filter.CurrentSelectedOfficeTasks = null;
            BindData();
            SetPageNavigator();
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDTYPE", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME", 
                                 "FLDSTATUSNAME", "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDWONUMBER" };
        string[] alCaptions = { "Vessel", "Source", "Type", "Task", "Category", "Sub Category", "Task Status", "Assigned Department", "Sub Department", "Accepted by", 
                                  "Target Date", "Completed Date", "Work Order Number" };

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
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
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
                                                                );

        General.SetPrintOptions("gvLongTermAction", "Office Task List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLongTermAction.DataSource = ds;
            gvLongTermAction.DataBind();

            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONPREVENTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                gvLongTermAction.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLongTermAction);
        }

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
                                  "Target Date", "Completed Date", "Work Order Number" };
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
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
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
                                                                );

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
        gvLongTermAction.SelectedIndex = -1;

        for (int i = 0; i < gvLongTermAction.Rows.Count; i++)
        {
            if (gvLongTermAction.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
            {
                gvLongTermAction.SelectedIndex = i;
                ViewState["WORKORDERID"] = ((Label)gvLongTermAction.Rows[gvLongTermAction.SelectedIndex].FindControl("lblWorkOrderId")).Text;
            }
        }
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

        BindData();
    }

    protected void gvLongTermAction_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLongTermAction, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvLongTermAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvLongTermAction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SHOWSOURCE"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
                Label lblLongTermActionId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblLongTermActionId");
                Label lblSourceType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSourceType");
                if (lblSourceType != null)
                {
                    if (lblLongTermActionId != null)
                    {
                        if (lblSourceType.Text == "7" && ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkmanualtask")).Checked)
                        {
                            Response.Redirect("../Inspection/InspectionOfficeManualTasksGeneral.aspx?preventiveactionid=" + lblLongTermActionId.Text);
                        }
                    }
                }

            }
            if (e.CommandName.ToUpper().Equals("MANUALTASK"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
                Label lblLongTermActionId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblLongTermActionId");
                Label lblSourceType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSourceType");
                Label lblGenerateTaskYN = (Label)_gridView.Rows[nCurrentRow].FindControl("lblGenerateTaskYN");                

                if (lblSourceType != null)
                {
                    if (lblLongTermActionId != null)
                    {
                        if (lblSourceType.Text == "7")
                        {
                            if (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkmanualtask")).Checked)
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
            if (e.CommandName.ToUpper().Equals("FILEEDIT"))
            {
                Label lblsectionno = (Label)_gridView.Rows[nCurrentRow].FindControl("lblsectionno");
                Label lblDocumentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId");
                Label lblformid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblformid");
                Label lblformno = (Label)_gridView.Rows[nCurrentRow].FindControl("lblformno");
                Label lblsectionid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblsectionid");

                if ((lblsectionid.Text == "") && (lblDocumentId.Text != "") && (lblformid.Text == "") && (lblformno.Text == ""))
                {
                    Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + lblDocumentId.Text, false);
                }
                if ((lblDocumentId.Text != "") && (lblformno.Text != ""))
                {
                    Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMNO=" + lblformno.Text.Trim(), false);
                }

            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLongTermAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermAction_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermAction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView dr = (DataRowView)e.Row.DataItem;
            Label lblWorkOrderId = (Label)e.Row.FindControl("lblWorkOrderId");
            Label lblManualTaskYN = (Label)e.Row.FindControl("lblManualTaskYN");
            Label lblGenerateTaskYN = (Label)e.Row.FindControl("lblGenerateTaskYN");
            CheckBox chkmanualtask = (CheckBox)e.Row.FindControl("chkmanualtask");
            Label lblManualGenerateTaskYN = (Label)e.Row.FindControl("lblManualGenerateTaskYN");

            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            if (lblWorkOrderId != null && lblWorkOrderId.Text != "")
            {
                if (chkSelect != null)
                    chkSelect.Visible = false;
            }

            Label lnkTask = (Label)e.Row.FindControl("lblTask");
            UserControlToolTip ucToolTip = (UserControlToolTip)e.Row.FindControl("ucToolTip");

            LinkButton lnkTaskSource = (LinkButton)e.Row.FindControl("lnkTaskSource");
            LinkButton lnkManualTaskName = (LinkButton)e.Row.FindControl("lnkManualTaskName");

            Label lblSourceId = (Label)e.Row.FindControl("lblSourceId");
            Label lblSourceType = (Label)e.Row.FindControl("lblSourceType");
            Label lblVesslName = (Label)e.Row.FindControl("lblVesselId");
            Label lblLongTermActionId = (Label)e.Row.FindControl("lblLongTermActionId");

            LinkButton lnkSubCategory = (LinkButton)e.Row.FindControl("lnkSubCategory");
            Label lblSubCategory = (Label)e.Row.FindControl("lblSubCategory");
            Label lblCategoryShortCode = (Label)e.Row.FindControl("lblCategoryShortCode");

            if (lnkSubCategory != null && lblCategoryShortCode.Text == "RCH")
            {
                lnkSubCategory.Visible = true;
                lblSubCategory.Visible = false;
            }
            else
            {
                lblSubCategory.Visible = true;
            }

            if (lnkTask != null && lnkManualTaskName != null && lblManualTaskYN != null && lblGenerateTaskYN != null && lblManualGenerateTaskYN != null)
            {
                if (lblManualTaskYN.Text == "1" || lblGenerateTaskYN.Text == "1" || lblManualGenerateTaskYN.Text == "1")
                {
                    lnkTask.Visible = true;
                    lnkManualTaskName.Visible = false;

                    lnkTask.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'visible');");
                    lnkTask.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'hidden');");

                }
                if (lblManualTaskYN.Text == "1" && lblManualGenerateTaskYN.Text == "1")
                {
                    lnkTask.Visible = true;
                    lnkManualTaskName.Visible = false;

                    lnkManualTaskName.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'visible');");
                    lnkManualTaskName.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'hidden');");
                }
            }

            if (lnkTaskSource != null)
            {
                if (lblSourceType.Text == "1") //Open Reports
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "2") //Direct
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "3") // Audit/Inspection
                {
                    if (lblVesslName.Text.ToUpper().Equals("OFFICE"))
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    else
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                }
                if (lblSourceType.Text == "4") //Vetting
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "5") //Incident
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionIncidentView.aspx?incidentid=" + lblSourceId.Text + "'); return true;");
                }
                if (lblSourceType.Text == "6") //machinery damage
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionMachineryDamageGeneral.aspx?VIEWONLY=1&MACHINERYDAMAGEID=" + lblSourceId.Text + "'); return true;");
                }
                if (lblSourceType.Text == "7") //Office Manual task
                {
                    if (chkmanualtask.Checked == false)
                        lnkTaskSource.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionOfficeManualTasksGeneralEdit.aspx?VIEWONLY=1&preventiveactionid=" + lblSourceId.Text + "'); return true;");
                }
            }
            LinkButton lnkWorkOrder = (LinkButton)e.Row.FindControl("lnkWorkOrderNumber");
            Label lblWorkOrderID = (Label)e.Row.FindControl("lblWorkOrderId");

            if (lnkWorkOrder != null)
            {
                lnkWorkOrder.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text + "&TASKID=" + ViewState["TASKID"] + "&viewonly=1'); return true;");
            }
            if (lblManualTaskYN != null && lblGenerateTaskYN != null && chkSelect != null && lblManualGenerateTaskYN != null)
            {
                if (lblManualTaskYN.Text == "1" || lblGenerateTaskYN.Text == "1")
                {
                    chkSelect.Checked = false;
                    chkSelect.Enabled = false;
                    chkSelect.Visible = false;
                }
                if (lblManualTaskYN.Text == "1" && lblManualGenerateTaskYN.Text == "0" && (lblWorkOrderId == null || lblWorkOrderId.Text == ""))
                {                    
                    chkSelect.Enabled = true;
                    chkSelect.Visible = true;                    
                }

            }


            Label lblsectionno = (Label)e.Row.FindControl("lblsectionno");
            Label lblDocumentId = (Label)e.Row.FindControl("lblDocumentId");
            Label lblformid = (Label)e.Row.FindControl("lblformid");
            Label lblformno = (Label)e.Row.FindControl("lblformno");
            Label lblsectionid = (Label)e.Row.FindControl("lblsectionid");
            Label lblRevisionId = (Label)e.Row.FindControl("lblRevisionId");
            Label lblRevisionNumber = (Label)e.Row.FindControl("lblRevisionNumber");
            Label lblRevisionStatus = (Label)e.Row.FindControl("lblRevisionStatus");

            if (lnkSubCategory != null && lblCategoryShortCode.Text == "RCH")
            {

                if (lblsectionid != null && lblsectionid.Text != "" && lblRevisionNumber.Text != "" && lblRevisionStatus.Text == "4" && ((lblformid.Text == "") && (lblformno.Text == "") || (lblformno.Text != "")))
                {

                    lnkSubCategory.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../DocumentManagement/DocumentManagementDocumentSectionContentEditReason.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblsectionid.Text + "&REVISIONID=" + lblRevisionId.Text + "'); return false;");

                    //lnkSubCategory.Attributes.Add("onclick", "javascript:parent.Openpopup('source','','../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONNO=" + lblsectionno.Text.Trim() + "'); return true;");
                    //Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONNO=" + lblsectionno.Text.Trim(), false);
                }

                if (lblsectionid != null && lblsectionid.Text != "" && lblRevisionNumber.Text == "" && lblRevisionStatus.Text != "3" && ((lblformid.Text == "") && (lblformno.Text == "") || (lblformno.Text != "")))
                {
                    lnkSubCategory.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblsectionid.Text + "'); return false;");
                }

            }


            ImageButton cmdShowStatus = (ImageButton)e.Row.FindControl("cmdShowStatus");
            if (cmdShowStatus != null)
            {
                cmdShowStatus.Attributes.Add("onclick", "javascript:parent.Openpopup('status','','../Inspection/InspectionShipboardTaskStatus.aspx?preventiveactionid=" + lblLongTermActionId.Text + "'); return true;");
                cmdShowStatus.Visible = SessionUtil.CanAccess(this.ViewState, cmdShowStatus.CommandName);
            }        
        }
    }

    //protected void gvLongTermAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvLongTermAction.SelectedIndex = se.NewSelectedIndex;
    //    BindPageURL(se.NewSelectedIndex);
    //}

    private void BindPageURL(int rowindex)
    {
        try
        {

            Label lblLongTermActionId = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblLongTermActionId"));
            Label lblDTKey = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblDTKey"));
            Label lblWorkorderId = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblWorkOrderId"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
            if (lblWorkorderId != null)
                ViewState["WORKORDERID"] = lblWorkorderId.Text;
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
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvLongTermAction.SelectedIndex = -1;
        gvLongTermAction.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
        GetSelectedPvs();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

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
            CheckBox chkAll = (CheckBox)gvLongTermAction.HeaderRow.FindControl("chkAllRemittance");
            foreach (GridViewRow row in gvLongTermAction.Rows)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
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
        foreach (GridViewRow gvrow in gvLongTermAction.Rows)
        {
            bool result = false;
            index = new Guid(gvLongTermAction.DataKeys[gvrow.RowIndex].Value.ToString());

            if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
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
                foreach (GridViewRow row in gvLongTermAction.Rows)
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvLongTermAction.DataKeys[row.RowIndex].Value.ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void selection_Changed(Object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    private bool IsValidClose()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableInteger(ucDepartment.SelectedDepartment) == null)
        //    ucError.ErrorMessage = "Department is required.";

        return (!ucError.IsError);
    }

}
