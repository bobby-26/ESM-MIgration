using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class InspectionAuditWorkRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionAuditWorkRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkOrder')", "Print Grid", "icon_print.png", "PRINT");
            MenuInspectionWorkRequest.AccessRights = this.ViewState;
            MenuInspectionWorkRequest.MenuList = toolbar.Show();

            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RECORDANDRESPONSEID"] = null;
            ViewState["NEW"] = "false";
            if (Request.QueryString["RecordResponseId"] != null && Request.QueryString["RecordResponseId"].ToString() != "")
                ViewState["RECORDANDRESPONSEID"] = Request.QueryString["RecordResponseId"].ToString();
            else
                ViewState["RECORDANDRESPONSEID"] = null;
        }
        if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
        {
            //Save button is not required for inspection log
        }
        else
        {
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
            MenuWorkOrderRequestion.SetTrigger(pnlWorkOrderRequisition);
        }        
        txtComponentId.Attributes.Add("style", "visibility:hidden");
        txtJobId.Attributes.Add("style", "visibility:hidden");
        if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId != string.Empty)
        {
            ViewState["SCHEDULEID"] = Filter.CurrentAuditScheduleId.ToString();            
            BindAuditNC();
            BindData();
            BindAuditShortCode();            
        }        
    }
    private void BindFields()
    {
        if (ViewState["WORKORDERID"] != null)
        {
            DataSet ds = PhoenixInspectionWorkOrder.EditWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            lblWorkOrderID.Text = dr["FLDWORKORDERID"].ToString();
            txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
            txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
            txtJobDescription.Text = dr["FLDDETAILS"].ToString();
            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
            if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
                chkUnexpected.Checked = true;
            txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
            txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
            txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
            ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
            ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
            txtJobId.Text = dr["FLDJOBID"].ToString();
            txtJobName.Text = dr["FLDJOBTITLE"].ToString();
            txtJobCode.Text = dr["FLDJOBCODE"].ToString();
            ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
        }
        else
            ViewState["NEW"] = true;

    }

    private void BindAuditNC()
    {
        if (ViewState["RECORDANDRESPONSEID"] != null)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNonConformity(new Guid(ViewState["RECORDANDRESPONSEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["NONCONFORMITYID"] = ds.Tables[0].Rows[0]["FLDREVIEWNONCONFORMITYID"].ToString();
            }
            ds = PhoenixInspectionAuditNonConformity.EditNonConformityObservation(new Guid(ViewState["RECORDANDRESPONSEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["NONCONFORMITYID"] = ds.Tables[0].Rows[0]["FLDREVIEWNCOBSERVATIONID"].ToString();
            }
        }
    }
    private void BindAuditShortCode()
    {
        if (ViewState["SCHEDULEID"] != null)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["SCHEDULEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDREVIEWSHORTCODE"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"] + "', true); ");
                DataSet dsmain = PhoenixInspectionRecordAndResponse.WorkRequestMainType(new Guid(ViewState["SCHEDULEID"].ToString()), General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD")));
                if (dsmain.Tables[0].Rows.Count > 0)
                {
                    if (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null && dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() != string.Empty)
                    {
                        ucMainType.SelectedQuick = (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null ? dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() : null);
                    }
                }
            }
        }
    }

    protected void MenuInspectionWorkRequest_TabStripCommand(object sender, EventArgs e)
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
    }

    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;            
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                int? maintype = General.GetNullableInteger(ucMainType.SelectedQuick);
                if (ViewState["RECORDANDRESPONSEID"] == null || ViewState["NONCONFORMITYID"] == null || ViewState["VESSELID"] == null)
                {
                    ucError.ErrorMessage = "Please select a question and record the 'Non Conformity' details to generate a work order";
                    ucError.Visible = true;
                    return;
                }
                
                if (ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ucError.ErrorMessage = "Sorry,You can not make any more changes here.";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidRequisition(txtComponentId.Text, txtTitle.Text, txtPlannedStartDate.Text, txtJobId.Text, txtJobDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string workorderid = null;

                byte? isDefect = null;

                if (chkIsDefect.Checked)
                    isDefect = byte.Parse("1");

                PhoenixInspectionWorkOrder.InsertWorkOrder(
                    int.Parse(ViewState["VESSELID"].ToString()),
                    null,
                    General.GetNullableString(txtTitle.Text), General.GetNullableGuid(txtComponentId.Text), null,
                    General.GetNullableGuid(txtJobId.Text), null, null, null,
                    General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, null,
                    General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
                    General.GetNullableDateTime(txtPlannedStartDate.Text),
                    General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, maintype, null, null,
                    chkUnexpected.Checked == true ? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                    isDefect, ref workorderid);
                if (txtJobDescription.Text != "")
                    PhoenixInspectionWorkOrder.UpdateDetailsWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderid), General.GetNullableString(txtJobDescription.Text));
                ViewState["WORKORDERID"] = workorderid;

                PhoenixInspectionAuditRecordAndResponse.NCWorkOrderInsert
                                               (PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                              , int.Parse(ViewState["VESSELID"].ToString())
                                              , new Guid(txtComponentId.Text)
                                              , new Guid(workorderid)
                                              , new Guid(ViewState["RECORDANDRESPONSEID"].ToString())
                                              , new Guid(ViewState["NONCONFORMITYID"].ToString())                                                                                            
                                              , new Guid(ViewState["SCHEDULEID"].ToString()));

                ViewState["NEW"] = "false";
                BindData();
                BindFields();

            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ResetTextBox();
                ViewState["WORKORDERID"] = null;
                ViewState["NEW"] = "true";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void ResetTextBox()
    {
        txtWorkOrderNumber.Text = "";
        txtTitle.Text = "";
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
        txtCreatedDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
        chkUnexpected.Checked = true;
        txtDuration.Text = "";
        txtPlannedStartDate.Text = "";
        txtPriority.Text = "1";
        chkIsDefect.Checked = true;
        ucDiscipline.SelectedDiscipline = "";
        txtJobDescription.Text = "";
        txtJobId.Text = "";
        txtJobName.Text = "";
        txtJobCode.Text = "";
        ucWTOApproval.SelectedHard = "530";
    }
    private bool IsValidRequisition(string componentid, string title, string plannedstartdate, string jobid, string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componentid.Trim().Equals(""))
            ucError.ErrorMessage = "Component is required";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (!General.GetNullableDateTime(plannedstartdate).HasValue)
            ucError.ErrorMessage = "Planned Start Date is required";

        if (string.IsNullOrEmpty(jobid) && string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Either Job Description or Work Details is required";

        return (!ucError.IsError);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixInspectionAuditRecordAndResponse.NCWorkOrderSearch(ViewState["NONCONFORMITYID"] != null ? General.GetNullableGuid(ViewState["NONCONFORMITYID"].ToString()) : null,
                             null,
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             General.ShowRecords(iRowCount),
                             ref iRowCount,
                             ref iTotalPageCount);

            General.SetPrintOptions("gvWorkOrder", "Audit Work Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvWorkOrder.DataSource = ds;
                gvWorkOrder.DataBind();
                ViewState["OPERATIONMODE"] = "EDIT";
                gvWorkOrder.SelectedIndex = 0;

                if (ViewState["WORKORDERID"] == null && ViewState["NEW"] != null && ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    BindFields();
                }                                
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvWorkOrder);
                ViewState["NEW"] = "true";
            }


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionAuditRecordAndResponse.NCWorkOrderSearch(ViewState["NONCONFORMITYID"] != null ? General.GetNullableGuid(ViewState["NONCONFORMITYID"].ToString()) : null,
                             null,
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             General.ShowRecords(iRowCount),
                             ref iRowCount,
                             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Auditworkrequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Audit Work Request</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    protected void gvWorkOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvWorkOrder.SelectedIndex = se.NewSelectedIndex;
        Label lblWorkOrderId = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblWorkOrderId"));
        if (lblWorkOrderId != null)
            ViewState["WORKORDERID"] = lblWorkOrderId.Text;
        else
            ViewState["WORKORDERID"] = null;
        BindFields();
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
            ViewState["WORKORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvWorkOrder.SelectedIndex = -1;
            gvWorkOrder.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["WORKORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }    

}
