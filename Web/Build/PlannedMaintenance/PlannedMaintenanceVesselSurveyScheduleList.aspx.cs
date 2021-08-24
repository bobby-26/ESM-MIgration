using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class PlannedMaintenanceVesselSurveyScheduleList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            //cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "SHOWREPORT");
            MainMenuSurveySchedule.AccessRights = this.ViewState;
            MainMenuSurveySchedule.MenuList = toolbarmain.Show();
            MainMenuSurveySchedule.Visible = false;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurvey')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Plan", "<i class=\"fas fa-calendar-alt\"></i>", "PLAN");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Occasional", "<i class=\"fas fa-clipboard-list\"></i>", "OCCASIONAL");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx", "Flag Change", "<i class=\"fas fa-flag\"></i>", "FLAGCHANGE");
            MenuSurveySchedule.AccessRights = this.ViewState;
            MenuSurveySchedule.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvSurvey.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["certificatename"] != null)
                    txtCertificatename.Text = Request.QueryString["certificatename"].ToString();
                ViewState["SheduleId"] = "";
                ViewState["CertificateId"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 0;
                ViewState["CURRENTINDEX"] = 0;
                ViewState["SELECTEDINDEX"] = 0;
                ViewState["Category"] = "";
                ViewState["DAYS"] = string.Empty;
                if(!string.IsNullOrEmpty(Request.QueryString["d"]))
                {
                    ViewState["DAYS"] = Request.QueryString["d"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["CID"]))
                {
                    ViewState["CertificateId"] = Request.QueryString["CID"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["vesselId"]))
                {
                    Filter.CurrentCertificateSurveyVesselFilter = Request.QueryString["vesselId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cname"]))
                {
                    txtCertificatename.Text = Request.QueryString["cname"];
                }
                BindCategory();
                CurrentVessel();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.Visible = false;
                    lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                    lblVesselName.Visible = true;
                }
                else
                {
                    ddlVessel.Visible = true;
                    lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                    lblVesselName.Visible = false;
                }
                //             
            }
            // BindData();
            //MenuSurveySchedule.SetTrigger(pnlSurvey);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CurrentVessel()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            Filter.CurrentCertificateSurveyVesselFilter = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        else if (string.IsNullOrEmpty(Filter.CurrentCertificateSurveyVesselFilter))
            Filter.CurrentCertificateSurveyVesselFilter = ddlVessel.SelectedVessel;
        else
            ddlVessel.SelectedVessel = Filter.CurrentCertificateSurveyVesselFilter;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEXCELORDER", "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME", "FLDLASTSURVEYTYPENAME", "FLDLASTSURVEYDATE", "FLDSURVEYTYPENAME", "FLDNEXTDUEDATE", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER", "FLDPLANDATE", "FLDSEAPORTNAME" };
        string[] alCaptions = { "S.No", "Certificate Code", "Certificate", "Certificate No.", "Issued", "Expiry", "Issuing Authority", "Place of Issue", "Type", "Date", "Type", "Due", "Window Before", "Window After", "Date", "Port" };

        string Sortexpression = "EXCEL";
        string date = DateTime.Now.ToShortDateString();

        int Sortdirection;

        if (ViewState["SORTDIRECTION"] != null)
            Sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            Sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleSearch(General.GetNullableInteger(Filter.CurrentCertificateSurveyVesselFilter)
               , General.GetNullableInteger(ddlCategory.SelectedValue)
               , General.GetNullableString(Sortexpression)
               , Sortdirection
               , gvSurvey.CurrentPageIndex + 1
               , gvSurvey.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , General.GetNullableInteger(chkShowAll.Checked == true ? "1" : "0")
               , General.GetNullableInteger(chkShowNotVerified.Checked == true ? "1" : "")
               , txtCertificatename.Text
               , General.GetNullableInteger(ViewState["DAYS"].ToString())
               , General.GetNullableInteger(ViewState["CertificateId"].ToString())
               );
        Response.AddHeader("Content-Disposition", "attachment; filename=Certificates.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>"+ HttpContext.Current.Session["companyname"].ToString() + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>RECORD OF SHIPS' CERTIFICATES, SURVEYS AND INSPECTIONS</center></h5></td></tr>");
        Response.Write("<tr>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td align='left' style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 3).ToString() + "' align='left'><b>Vessel: </b>" + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + "</td>");
        Response.Write("<td align='left' style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 4).ToString() + "' align='left'><b>Date: </b>" + date + "</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        Response.Write("<td colspan='8'></td>");
        Response.Write("<td colspan='2' align='center'><b>Last Audit/Survey</b></td>");
        Response.Write("<td colspan='4' align='center'><b>Next Audit/Survey</b></td>");
        Response.Write("<td colspan='2' align='center'><b>Planned</b></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        ViewState["CategoryId"] = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (ViewState["CategoryId"].ToString().Trim().Equals("") || !ViewState["CategoryId"].ToString().Trim().Equals(dr["FLDCERTIFICATECATEGORY"].ToString()))
            {
                ViewState["CategoryId"] = dr["FLDCERTIFICATECATEGORY"].ToString();
                Response.Write("<tr>");
                Response.Write("<td style='font-family:Arial; font-size:12px; background-color:#FAEBD7; font-weight:bold;' colspan='" + (alColumns.Length).ToString() + "' align='center'>" + dr["FLDCATEGORYNAME"].ToString() + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (alColumns[i] == "FLDEXCELORDER")
                    Response.Write("<td align='center'>");
                else
                    Response.Write("<td>");
                string sValue = dr[alColumns[i]].GetType().ToString().Equals("System.DateTime") ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString();
                Response.Write(sValue);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindCategory()
    {
        ddlCategory.DataSource = PhoenixRegistersVesselSurvey.CertificateCategoryList();
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));

    }
    protected void ddlCategory_TextChanged(object sender, EventArgs e)
    {
        Filter.CurrentCertificateSurveyVesselFilter = ddlVessel.SelectedVessel;
        ViewState["PAGENUMBER"] = 1;
        CurrentVessel();
        BindData();
        gvSurvey.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCERTIFICATENAME", "FLDCATEGORYNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDNEXTDUEDATE", "FLDSURVEYTYPENAME", "FLDWINDOWPERIODBEFORE", "FLDSEAPORTNAME" };
            string[] alCaptions = { "Certificate", "Category", "Issued", "Expiry", "Issued By", "Due Date for Survey / Follow up", "Survey Type", "Window Before", "Port" };

            int Sortdirection;
            string Sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                Sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            else
                Sortdirection = 0;

            DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleSearch(General.GetNullableInteger(Filter.CurrentCertificateSurveyVesselFilter)
               , General.GetNullableInteger(ddlCategory.SelectedValue)
               , General.GetNullableString(Sortexpression)
               , Sortdirection
               , gvSurvey.CurrentPageIndex + 1
               , gvSurvey.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , General.GetNullableInteger(chkShowAll.Checked == true ? "1" : "0")
               , General.GetNullableInteger(chkShowNotVerified.Checked == true ? "1" : "")
               , txtCertificatename.Text
               , General.GetNullableInteger(ViewState["DAYS"].ToString())
               , General.GetNullableInteger(ViewState["CertificateId"].ToString())
               );
            General.SetPrintOptions("gvSurvey", "Certificates", alCaptions, alColumns, ds);
            gvSurvey.DataSource = ds;
            gvSurvey.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MainMenuSurveySchedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string vesselid = Filter.CurrentCertificateSurveyVesselFilter;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                string Param = "";
                NameValueCollection nvc = Filter.VesselSurveyFilter;
                if (nvc != null)
                {

                    Param += "&VesselId=" + nvc.Get("VesselId");
                    Param += "&DueFrom=" + nvc.Get("DueFrom");
                    Param += "&DueTo=" + nvc.Get("DueTo");
                    Param += "&Status=" + nvc.Get("Status");
                    Param += "&Category=" + nvc.Get("Category");
                    Param += "&PlanFrom=" + nvc.Get("PlanFrom");
                    Param += "&PlanTo=" + nvc.Get("PlanTo");
                    Param += "&showexcel=NO&showword=YES&showpdf=YES";
                }
                else
                {
                    Param += "&VesselId=" + vesselid;
                    Param += "&DueFrom=" + string.Empty;
                    Param += "&DueTo=" + string.Empty;
                    Param += "&Status=" + string.Empty;
                    Param += "&Category=" + string.Empty;
                    Param += "&PlanFrom=" + string.Empty;
                    Param += "&PlanTo=" + string.Empty;
                    Param += "&showexcel=NO&showword=YES&showpdf=YES";
                }
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=6&reportcode=CERTIFICATESURVEY" + Param);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveySchedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string vesselid = Filter.CurrentCertificateSurveyVesselFilter;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                gvSurvey.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlCategory.SelectedValue = "";
                chkShowAll.Checked = false;
                txtCertificatename.Text = "";
                chkShowNotVerified.Checked = false;
                ViewState["DAYS"] = string.Empty;
                gvSurvey.Rebind();
            }
            else if (CommandName.ToUpper().Equals("PLAN"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ucError.ErrorMessage = "Survey Planning is allowed in Office only";
                    ucError.Visible = true;
                    return;
                }
                string csvScheduleList = GetSelectedScheduleList();
                if (csvScheduleList.Trim().Equals(""))
                {
                    ucError.ErrorMessage = "Select atleast one Scheduled Certificate ";
                    ucError.Visible = true;
                    return;
                }
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyPlanner.aspx?vslid=" + vesselid
                    + "&slist=" + csvScheduleList + "&clist=" + GetSelectedScheduleCertificateList() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);

                gvSurvey.Rebind();
            }

            else if (CommandName.Trim().ToUpper().Equals("CERTIFICATEISSUE"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificatesIssue.aspx?VesselId=" + vesselid
                    + "&ScheduleId=" + ViewState["SheduleId"].ToString());
            }
            else if (CommandName.Trim().ToUpper().Equals("OCCASIONAL"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateOccasional.aspx?vslid=" + vesselid
                    + "&clist=" + GetSelectedCertificateList() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                gvSurvey.Rebind();
            }
            else if (CommandName.Trim().ToUpper().Equals("FLAGCHANGE"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateFlagChange.aspx?vslid=" + vesselid
                    + "&clist=" + GetSelectedCertificateList() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                gvSurvey.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    BindData();        
    //}
    private string GetSelectedCertificateList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvSurvey.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvSurvey.Items)
            {
                CheckBox ChkPlan = (CheckBox)gv["ClientSelectColumn"].Controls[0];

                if (ChkPlan.Checked && ChkPlan.Enabled)
                {
                    //Label lblUpdateAuditLog = (Label)gv.FindControl("lblUpdateAuditLog");
                    RadLabel lblCertificateId = (RadLabel)gv.FindControl("lblCertificateId");
                    strlist.Append(lblCertificateId.Text);
                    strlist.Append(",");
                    //strlistUpdate.Append(lblUpdateAuditLog.Text);
                    //strlistUpdate.Append(",");
                }
            }
        }
        return strlist.ToString();

    }
    private string GetSelectedScheduleList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvSurvey.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvSurvey.Items)
            {
                CheckBox ChkPlan = (CheckBox)gv["ClientSelectColumn"].Controls[0];
                //CheckBox ChkPlan = (CheckBox)gv.FindControl("ChkPlan");

                if (ChkPlan.Checked && ChkPlan.Enabled)
                {
                    RadLabel lblScheduleId = (RadLabel)gv.FindControl("lblSheduleId");
                    if (!string.IsNullOrEmpty(lblScheduleId.Text))
                    {
                        strlist.Append(lblScheduleId.Text);
                        strlist.Append(",");
                    }
                }
            }
        }
        return strlist.ToString();
    }
    private string GetSelectedScheduleCertificateList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvSurvey.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvSurvey.Items)
            {
                CheckBox ChkPlan = (CheckBox)gv["ClientSelectColumn"].Controls[0];

                if (ChkPlan.Checked && ChkPlan.Enabled)
                {
                    RadLabel lblScheduleId = (RadLabel)gv.FindControl("lblSheduleId");
                    if (!string.IsNullOrEmpty(lblScheduleId.Text))
                    {
                        RadLabel lblCertificateId = (RadLabel)gv.FindControl("lblCertificateId");
                        strlist.Append(lblCertificateId.Text);
                        strlist.Append(",");
                    }
                }
            }
        }
        return strlist.ToString();

    }
    private string GetSelectedupdatelogList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvSurvey.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvSurvey.Items)
            {
                CheckBox ChkPlan = (CheckBox)gv["ClientSelectColumn"].Controls[0];

                if (ChkPlan.Checked && ChkPlan.Enabled)
                {
                    Label lblUpdateAuditLog = (Label)gv.FindControl("lblUpdateAuditLog");
                    //Label lblCertificateId = (Label)gv.FindControl("lblCertificateId");
                    //strlist.Append(lblCertificateId.Text);
                    //strlist.Append(",");
                    strlist.Append(lblUpdateAuditLog.Text);
                    strlist.Append(",");
                }
            }
        }
        return strlist.ToString();

    }

    protected void gvSurvey_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton cmdEdit = item.FindControl("cmdEdit") as LinkButton;
                if (cmdEdit != null)
                    cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateRenewal.aspx?cid="
                    + drv["FLDCERTIFICATEID"].ToString()
                    + "&dtkey=" + drv["FLDDTKEY"].ToString()
                    + "&vslid=" + drv["FLDVESSELID"].ToString()
                    + "'); return false;");
                LinkButton cmdCOC = item.FindControl("cmdCOC") as LinkButton;
                if (cmdCOC != null)
                {
                    if (drv["FLDDTKEY"].ToString() == string.Empty) cmdCOC.Attributes.Add("style", "display:none");
                    cmdCOC.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateCOC.aspx?cid="
                    + drv["FLDCERTIFICATEID"].ToString()
                    + "&dtkey=" + drv["FLDDTKEY"].ToString()
                    + "&vslid=" + drv["FLDVESSELID"].ToString()
                    + "'); return false;");
                    if (drv["FLDNOEXPIRY"].ToString() == "1" || drv["FLDNOTAPPLICABLE"].ToString() == "1" || drv["FLDTEMPLATENAME"].ToString().Equals(""))
                        cmdCOC.Attributes.Add("style", "display:none");
                }
                LinkButton cmdRestart = item.FindControl("cmdRestart") as LinkButton;
                if (cmdRestart != null)
                {
                    if (drv["FLDDTKEY"].ToString() == string.Empty) cmdRestart.Attributes.Add("style", "display:none");
                    cmdRestart.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateRestart.aspx?cid="
                    + drv["FLDCERTIFICATEID"].ToString()
                    + "'); return false;");
                    if (drv["FLDNOEXPIRY"].ToString() == "1" || drv["FLDNOTAPPLICABLE"].ToString() == "1" || drv["FLDTEMPLATENAME"].ToString().Equals("")
                        || drv["FLDRESTARTYN"].ToString().Equals("2") || drv["FLDRESTARTYN"].ToString().Equals("3") || drv["FLDAUDITYN"].ToString().Equals("0"))
                        cmdRestart.Attributes.Add("style", "display:none");
                }
                LinkButton dePlan = item.FindControl("cmdDePlan") as LinkButton;
                if (dePlan != null)
                {
                    dePlan.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you Sure want to De-plan this Certificate?'); return false;");
                    if (drv["FLDPLANDATE"].ToString() == string.Empty)
                    {
                        dePlan.Attributes.Add("style", "display:none");
                    }
                }
                LinkButton lnk = item.FindControl("lnkCertificate") as LinkButton;
                if (lnk != null)
                {
                    lnk.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselWiseSurveyCertificateDetails.aspx?CertificateId=" + drv["FLDCERTIFICATEID"].ToString() + "&category=" + drv["FLDCATEGORYCODE"] + "');return false;");
                }
                if (drv["FLDSURVEYDUE"].ToString() == "1")
                {
                    //e.Item.Cells[9].Attributes.Add("style", "background-color:#6fb76f;font-weight:bold;");
                    item["NextDue"].Attributes.Add("style", "background-color:#6fb76f;font-weight:bold;");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "2")
                {
                    //e.Item.Cells[9].Attributes.Add("style", "background-color:#FFFFAA");
                    item["NextDue"].Attributes.Add("style", "background-color:#FFFFAA");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "3")
                {
                    //e.Item.Cells[9].Attributes.Add("style", "background-color:#FF8000");
                    item["NextDue"].Attributes.Add("style", "background-color:#FF8000");
                }
                else if (drv["FLDSURVEYDUE"].ToString() == "4")
                {
                    //e.Item.Cells[9].Attributes.Add("style", "background-color:#ff4d4d");
                    item["NextDue"].Attributes.Add("style", "background-color:#ff4d4d");
                }

                ImageButton cmdSvyAtt = item.FindControl("cmdSvyAtt") as ImageButton;
                if (cmdSvyAtt != null)
                {
                    if (drv["FLDDTKEY"].ToString() == string.Empty) cmdSvyAtt.Attributes.Add("style", "display:none");
                    if (drv["FLDATTACHMENTYN"].ToString() == "0")
                    {
                        cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    {
                        if (drv["FLDCATEGORYCODE"].ToString() == "SC")
                            cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=STATUTORYCERTIFICATE&u=n&VESSELID=" + drv["FLDVESSELID"] + "'); return false;");
                        else
                            cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE&u=n&VESSELID=" + drv["FLDVESSELID"] + "'); return false;");
                    }
                    else
                    {
                        if (drv["FLDCATEGORYCODE"].ToString() == "SC")
                            cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=STATUTORYCERTIFICATE&VESSELID=" + drv["FLDVESSELID"] + "'); return false;");
                        else
                            cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE&VESSELID=" + drv["FLDVESSELID"] + "'); return false;");
                    }
                }
                LinkButton cmdChangeSurvey = item.FindControl("cmdChangeSurvey") as LinkButton;
                if (cmdChangeSurvey != null)
                {
                    cmdChangeSurvey.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you Sure want to change the survey?'); return false;");
                    if (drv["FLDSURVEYCHANGE"].ToString() == "1") cmdChangeSurvey.Attributes.Remove("style");
                }
                LinkButton cmdCancelSurveyChange = item.FindControl("cmdCancelSurveyChange") as LinkButton;
                if (cmdCancelSurveyChange != null)
                {
                    cmdCancelSurveyChange.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you Sure want to cancel the survey change?'); return false;");
                    if (drv["FLDCANCELSURVEYCHANGE"].ToString() == "1") cmdCancelSurveyChange.Attributes.Remove("style");
                }

                Label lblRemarks = item.FindControl("lblRemarks") as Label;
                UserControlToolTip ucRemarks = item.FindControl("ucRemarks") as UserControlToolTip;
                if (lblRemarks != null && ucRemarks != null)
                {
                    lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'visible');");
                    lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'hidden');");
                }

                //UserControlCommonToolTip ttip = item.FindControl("ucCommonToolTip") as UserControlCommonToolTip;
                //if (ttip != null)
                //{
                //    ttip.Screen = "PlannedMaintenance/PlannedMaintenanceToolTipSurveyRemark.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&cid=" + drv["FLDCERTIFICATEID"].ToString() + "&vslid=" + drv["FLDVESSELID"].ToString();
                //}

                if (drv["FLDNOEXPIRY"].ToString() != "1")
                {
                    DateTime? dt = General.GetNullableDateTime(drv["FLDDATEOFEXPIRY"].ToString());
                    DateTime? extExpirydt = General.GetNullableDateTime(drv["FLDEXTNEXPIRYDATE"].ToString());
                    if(dt.HasValue && extExpirydt.HasValue)
                    {
                        int i = DateTime.Compare(Convert.ToDateTime(dt), Convert.ToDateTime(extExpirydt));
                        if(i < 0 )
                        {
                            dt = extExpirydt;
                        }
                    }
                    if (dt.HasValue)
                    {
                        double noofdays = (dt.Value - DateTime.Now).TotalDays;
                        if (noofdays <= 0)
                        {
                            //e.Item.Cells[4].Attributes.Add("style", "background-color:#ff4d4d");
                            item["ExpiryDate"].Attributes.Add("style", "background-color:#ff4d4d");
                        }
                        else if (noofdays <= 60 && noofdays >= 31)
                        {
                            //e.Item.Cells[4].Attributes.Add("style", "background-color:#FFFFAA");
                            item["ExpiryDate"].Attributes.Add("style", "background-color:#FFFFAA");
                        }
                        else if (noofdays <= 30)
                        {
                            //e.Item.Cells[4].Attributes.Add("style", "background-color:#FF8000");
                            item["ExpiryDate"].Attributes.Add("style", "background-color:#FF8000");
                        }
                    }
                }
                LinkButton verify = item.FindControl("cmdVerify") as LinkButton;
                if (verify != null)
                {
                    verify.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you Sure want to Verify this Certificate?'); return false;");
                    if (drv["FLDDTKEY"].ToString() == string.Empty || drv["FLDVERIFIEDON"].ToString() != string.Empty || drv["FLDVERIFIEDYN"].ToString() != "N/A")
                    {
                        verify.Attributes.Add("style", "display:none");
                    }
                }
                if((drv["FLDTEMPLATENAME"].ToString().Equals("")) || drv["FLDNOEXPIRY"].ToString().Equals("1"))
                {
                    CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                    checkBox.Enabled = false;
                    //item.SelectableMode = GridItemSelectableMode.None;
                    //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
                }
                LinkButton cmdExtn = item.FindControl("lnkExtension") as LinkButton;
                if (cmdExtn != null)
                {
                    cmdExtn.Visible = SessionUtil.CanAccess(this.ViewState, cmdExtn.CommandName);
                    cmdExtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateExtentionUpdate.aspx?&dtkey=" + drv["FLDDTKEY"].ToString()
                    + "&vslid=" + drv["FLDVESSELID"].ToString()
                    + "'); return false;");
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurvey_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            RadLabel lblSheduleId = ((RadLabel)e.Item.FindControl("lblSheduleId"));
            RadLabel lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId"));
            //RadLabel lblSurveyName = ((RadLabel)item.FindControl("lblSurveyName"));
            RadLabel lblCertificateId = ((RadLabel)e.Item.FindControl("lblCertificateId"));
            RadLabel lblDTKey = ((RadLabel)e.Item.FindControl("lblDTKey"));

            if (e.CommandName.ToUpper().Equals("DEPLAN"))
            {
                PhoenixPlannedMaintenanceSurveySchedule.DeplanPlanSurveyCertificate(General.GetNullableInteger(lblVesselId.Text).Value
                        , General.GetNullableGuid(lblSheduleId.Text).Value
                        );
                BindData();
                gvSurvey.Rebind();
                ucStatus.Text = "Certificate Deplaned Sucessfully";
            }
            else if (e.CommandName.ToUpper().Equals("CHANGESURVEY/AUDIT"))
            {
                PhoenixPlannedMaintenanceSurveySchedule.ChangeSurveyorAudit(General.GetNullableGuid(lblSheduleId.Text)
               , General.GetNullableInteger(lblVesselId.Text)
               , General.GetNullableInteger(lblCertificateId.Text), 1);

                BindData();
                gvSurvey.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELSURVEY/AUDIT"))
            {
                PhoenixPlannedMaintenanceSurveySchedule.ChangeSurveyorAudit(General.GetNullableGuid(lblSheduleId.Text)
                    , General.GetNullableInteger(lblVesselId.Text)
                    , General.GetNullableInteger(lblCertificateId.Text), 2);

                BindData();
                gvSurvey.Rebind();
                ucStatus.Text = "Survey Changed Sucessfully";

            }
            else if (e.CommandName.ToUpper().Equals("CMDSELECT"))
            {
                string sScript = "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceVesselWiseSurveyCertificateDetails.aspx?CertificateId=" + lblCertificateId.Text.Trim() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (e.CommandName.ToUpper().Equals("VERIFY"))
            {
                PhoenixPlannedMaintenanceSurveySchedule.VerifySurveyCertificate(General.GetNullableInteger(lblVesselId.Text).Value, General.GetNullableGuid(lblDTKey.Text).Value);

                BindData();
                gvSurvey.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = "item commands " + e.CommandName + ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurvey_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }


    protected void gvSurvey_SortCommand(object sender, GridSortCommandEventArgs e)
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
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvSurvey.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadToolTipManager1_AjaxUpdate(object sender, ToolTipUpdateEventArgs e)
    {
        HtmlGenericControl f = new HtmlGenericControl("iframe");
        f.Attributes.Add("frameborder", "0");
        f.Attributes.Add("src", Session["sitepath"] + "/" + e.Value);
        f.Attributes.Add("width", "100%");
        e.UpdatePanel.ContentTemplateContainer.Controls.Add(f);
    }
}
