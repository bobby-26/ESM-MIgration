using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections;
using System.Collections.Specialized;

public partial class InspectionLongTermActionWorkOrderGeneral : PhoenixBasePage
{
    PhoenixToolbar toolbar = new PhoenixToolbar(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                Session["NewList"] = "N";
                if (Filter.CurrentSelectedWOTasks != null)
                    Filter.CurrentSelectedWOTasks = null;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
               
                ViewState["WORKORDERID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;  

                if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != "")
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();

                    BindActionDetails();
                }
                toolbar.AddButton("Save", "SAVE");
                toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','InspectionLongTermActionWorkOrderComplete.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "','xlarge'); return false;", "Complete", "", "COMPLETE");
                toolbar.AddButton("Cancel", "CANCEL");

                //toolbar.AddImageLink("javascript:parent.Openpopup('WORKORDERID','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                //                    + PhoenixModule.QUALITY + "'); return false;", "Attachments", "", "ATTACHMENTS");

                MenuInspectionIncident.AccessRights = this.ViewState;
                if (Request.QueryString["Viewonly"] == null || Request.QueryString["Viewonly"].ToString() =="0")
                {
                    MenuInspectionIncident.MenuList = toolbar.Show();
                }                    

   

                toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Inspection/InspectionLongTermActionWorkOrderGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvLongTermAction')", "Print Grid", "icon_print.png", "PRINT");

                MenuLongTermAction.AccessRights = this.ViewState;
                MenuLongTermAction.MenuList = toolbar.Show();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindActionDetails()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionLongTermAction.WorkOrderEdit(new Guid(ViewState["WORKORDERID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            //ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();            
            //ucVessel.Enabled = false;
            //ucDept.SelectedDepartment = ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString();
            
            //txtUser.Text = ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString();           
            //txtStatus.Text = ds.Tables[0].Rows[0]["FLDSTATUSNAME"].ToString();

            txtWorkOrder.Text = ds.Tables[0].Rows[0]["FLDWONUMBER"].ToString();
            txtGeneratedBy.Text = ds.Tables[0].Rows[0]["FLDWOGENERATEDBYNAME"].ToString();
           // txtDept.Text = ds.Tables[0].Rows[0]["FLDDEPARTMENTNAME"].ToString();
            txtDescription.Text = ds.Tables[0].Rows[0]["FLDWODESCRIPTION"].ToString();
            txtActionTaken.Text = ds.Tables[0].Rows[0]["FLDACTIONTAKEN"].ToString();
            txtCloseddDate.Text = ds.Tables[0].Rows[0]["FLDCOMPLETEDDATE"].ToString();
           // txtCompletedDate.Text = ds.Tables[0].Rows[0]["FLDCOMPLETEDDATE"].ToString();
            ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();                      
            //ucVessel.Enabled = false;          
            //txtStatus.ReadOnly = true;

            //String script = String.Format("javascript:parent.fnReloadList('code1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                
                PhoenixInspectionLongTermAction.WorkOrderDescriptionUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , txtDescription.Text.Trim()
                                        , txtActionTaken.Text.Trim()
                                        , new Guid(ViewState["WORKORDERID"].ToString()));

                ucStatus.Text = "Information are updated.";
                BindData();

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (dce.CommandName.ToUpper().Equals("COMPLETE"))
            {
                if (!IsValidClose())
                {
                    ucError.Visible = true;
                    return;
                }
            }

            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {                
                string selectedagents = ",";
                if (Filter.CurrentSelectedWOTasks != null)
                {
                    ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;

                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                        {
                            selectedagents = selectedagents + index + ",";
                        }

                        PhoenixInspectionLongTermAction.WorkOrderStatusUpdate(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , null
                            , txtActionTaken.Text.Trim()
                            , new Guid(ViewState["WORKORDERID"].ToString())
                            , 0
                            , selectedagents);

                        if (Filter.CurrentSelectedWOTasks != null)
                            Filter.CurrentSelectedWOTasks = null;

                        ucStatus.Text = "Work Order is generated.";
                        BindData();
                    }
                }

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
   
    private bool IsValidClose()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtActionTaken.Text.Trim()) == null)
            ucError.ErrorMessage = "Action Taken is required.";

        if (General.GetNullableDateTime(txtDescription.Text) == null)
            ucError.ErrorMessage = "WO Description is required.";

        if (Filter.CurrentSelectedWOTasks == null)
            ucError.ErrorMessage = "Select the task to complete.";

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME", "FLDCREATEDFROMNAME", "FLDDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Task", "Category", "Sub Category", "Source", "Assigned Department", "Accepted by", "Target Date", "Task Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))        
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();                   

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = Filter.CurrentVesselConfiguration.ToString();

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) == null ? Guid.NewGuid() : General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null,null,null,null,null,null,null,null,null,null,null,null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvLongTermAction", "Long Term Action", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLongTermAction.DataSource = ds;
            gvLongTermAction.DataBind();

            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONPREVENTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                //Filter.CurrentSelectedBulkOrderId = ds.Tables[0].Rows[0]["FLDCORRECTIVEACTIONID"].ToString();
                //gvLongTermAction.SelectedIndex = 0;
            }
            //SetRowSelection();
            //ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionGeneral.aspx?TASKID=" + ViewState["TASKID"] + "&DTKEY=" + ViewState["DTKEY"];
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLongTermAction);
            //ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionGeneral.aspx?TASKID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME", "FLDCREATEDFROMNAME", "FLDDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Task", "Category", "Sub Category", "Source", "Assigned Department", "Accepted by", "Target Date", "Task Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //string BudgetBillingSearch = (txtSearchBudgetBillingList.Text == null) ? "" : txtSearchBudgetBillingList.Text;

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) == null ? Guid.NewGuid() : General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null, null, null, null, null, null, null, null, null, null, null, null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=BulkPOList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Long Term Action</h3></td>");
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

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("ACCEPT"))
            {
                BindPageURL(nCurrentRow);
                //if (General.GetNullableGuid(ViewState["TASKID"].ToString()) != null)
                //{
                //    PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //      , new Guid(ViewState["TASKID"].ToString())
                //      , null);
                //    BindData();
                //}USER
            }
            else if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                BindPageURL(nCurrentRow);
                //if (General.GetNullableGuid(ViewState["TASKID"].ToString()) != null)
                //{
                //    PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //      , new Guid(ViewState["TASKID"].ToString())
                //      , 1);
                //    ucStatus.Text = "";
                //    BindData();
                //}
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
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

            if (lblStatus != null && lblStatus.Text == "2")
            {
                if (chkSelect != null)
                    chkSelect.Visible = false;
            }

            Label lblTask = (Label)e.Row.FindControl("lblTask");
            UserControlToolTip ucToolTip = (UserControlToolTip)e.Row.FindControl("ucToolTip");
            lblTask.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'visible');");
            lblTask.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'hidden');");

            LinkButton lnkTaskSource = (LinkButton)e.Row.FindControl("lnkTaskSource");
            UserControlToolTip ucToolTipSource = (UserControlToolTip)e.Row.FindControl("ucToolTipSource");
            lnkTaskSource.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipSource.ToolTip + "', 'visible');");
            lnkTaskSource.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipSource.ToolTip + "', 'hidden');");

            Label lblSourceId = (Label)e.Row.FindControl("lblSourceId");
            Label lblSourceType = (Label)e.Row.FindControl("lblSourceType");
            Label lblVesselName = (Label)e.Row.FindControl("lblVesselName");

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
                    if (lblVesselName.Text.ToUpper().Equals("OFFICE"))
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
            }
        }
    }

    protected void gvLongTermAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvLongTermAction.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            Label lblLongTermActionId = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblLongTermActionId"));
            Label lblDTKey = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblDTKey"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
            // ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionGeneral.aspx?TASKID=" + ViewState["TASKID"] + "&DTKEY=" + ViewState["DTKEY"];
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
        SaveCheckedValues(sender, e);
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        foreach (GridViewRow gvrow in gvLongTermAction.Rows)
        {
            bool result = false;
            if (General.GetNullableGuid(gvLongTermAction.DataKeys[gvrow.RowIndex].Value.ToString()) != null)
                index = new Guid(gvLongTermAction.DataKeys[gvrow.RowIndex].Value.ToString());

            CheckBox cb = ((CheckBox)(gvrow.FindControl("chkSelect")));
            if (cb != null)
            {
                if (cb.Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }
            }
            // Check in the Session
            if (Filter.CurrentSelectedWOTasks != null)
                SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Filter.CurrentSelectedWOTasks = SelectedPvs;
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedWOTasks != null)
        {
            ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;
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

    protected void ddlAcceptance_Changed(Object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void ucDepartment_Changed(Object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }   
}
