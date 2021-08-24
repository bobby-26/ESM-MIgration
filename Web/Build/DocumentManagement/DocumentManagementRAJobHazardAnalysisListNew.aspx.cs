using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System.Web;
using SouthNests.Phoenix.DocumentManagement;
using System.Web.UI.HtmlControls;

public partial class DocumentManagementRAJobHazardAnalysisListNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAJobHazardAnalysisListNew.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAJobHazardAnalysisListNew.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        try
        {
            if (!IsPostBack)
            {
                BindStatus();

                ViewState["REFNO"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["NEWDMSYN"] = 0;

                if (Request.QueryString["NEWDMSYN"] != null)
                    ViewState["NEWDMSYN"] = Request.QueryString["NEWDMSYN"].ToString();

                lblStatus.Visible = false;
                ddlStatus.SelectedValue = "3";
                ddlStatus.Visible = false;
                gvRiskAssessmentJobHazardAnalysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindCategory();
            }

            MenuJobHazardAnalysis.AccessRights = this.ViewState;
            MenuJobHazardAnalysis.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        DataSet ds = new DataSet();


        ds = PhoenixDocumentManagementDocument.NewRiskAssessmentJobHazardSearch(
               General.GetNullableString(txtHazardNo.Text),
               General.GetNullableInteger(ddlCategory.SelectedValue),
               null, null,
               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvRiskAssessmentJobHazardAnalysis.PageSize,
               ref iRowCount,
               ref iTotalPageCount,
               General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
               companyid,
               General.GetNullableString(txtjob.Text));
        
        gvRiskAssessmentJobHazardAnalysis.DataSource = ds;
        gvRiskAssessmentJobHazardAnalysis.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;

    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDHAZARDNUMBER", "FLDVESSELNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDHSRR", "FLDENVRR", "FLDECORR", "FLDWCRR" };
            string[] alCaptions = { "Hazard Number", "Vessel Name", "Process", "Job", "Status", "Revision No", "Approved", "Health & Safety", "Environmental", "Economic/Process Loss", "Worst Case" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentJHAFilter;

            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardSearch(
                    nvc.Get("txtHazardNo") != null ? General.GetNullableString(nvc.Get("txtHazardNo").ToString()) : General.GetNullableString(ViewState["txtHazardNo"].ToString()),
                    General.GetNullableInteger(null),
                    nvc.Get("ddlCategory") != null ? General.GetNullableInteger(nvc.Get("ddlCategory").ToString()) : General.GetNullableInteger(ViewState["ddlCategory"].ToString()),
                    nvc.Get("ddlStatus") != null ? General.GetNullableInteger(nvc.Get("ddlStatus").ToString()) : General.GetNullableInteger(ViewState["ddlStatus"].ToString()),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    nvc.Get("VesselID") != null ? General.GetNullableInteger(nvc.Get("VesselID").ToString()) : General.GetNullableInteger(ViewState["VesselID"].ToString()),
                    nvc.Get("txtJob") != null ? General.GetNullableString(nvc.Get("txtJob").ToString()) : General.GetNullableString(ViewState["txtJob"].ToString()),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.ShowExcel("Job Hazard Analysis", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
        gvRiskAssessmentJobHazardAnalysis.EditIndexes.Clear();
        gvRiskAssessmentJobHazardAnalysis.DataSource = null;
        gvRiskAssessmentJobHazardAnalysis.Rebind();
    }

    protected void MenuJobHazardAnalysis_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            BindStatus();
            ViewState["REFNO"] = "";
            txtHazardNo.Text = "";
            txtjob.Text = "";
            ddlStatus.SelectedValue = "3";
            BindCategory();
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }


    protected void gvRiskAssessmentJobHazardAnalysis_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
            LinkButton lnkJob = (LinkButton)e.Item.FindControl("lnkJob");
            if (lnkJob != null)
            {
                lnkJob.Visible = SessionUtil.CanAccess(this.ViewState, lnkJob.CommandName);
                lnkJob.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisExtn.aspx?DMSYN=1&jobhazardid=" + lblJobHazardid.Text + "',false,900,600);return false;");
            }

            LinkButton lnkHealth = (LinkButton)e.Item.FindControl("lnkHealth");

            if (lnkHealth != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDHSYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkHealth.Controls.Add(html);
                    lnkHealth.ToolTip = "";
                    lnkHealth.Enabled = false;
                }
                else
                {
                    lnkHealth.Visible = SessionUtil.CanAccess(this.ViewState, lnkHealth.CommandName);
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Health\"></i></span>";
                    lnkHealth.Controls.Add(html);
                    lnkHealth.ToolTip = "Health and Safety Hazard";
                    lnkHealth.Enabled = true;
                    lnkHealth.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Health and Safety Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=1&TYPE=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
                }
            }

            LinkButton lnkenv = (LinkButton)e.Item.FindControl("lnkenv");
            if (lnkenv != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDENVYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkenv.Controls.Add(html);
                    lnkenv.ToolTip = "";
                    lnkenv.Enabled = false;
                }
                else
                {
                    lnkenv.Visible = SessionUtil.CanAccess(this.ViewState, lnkenv.CommandName);
                    lnkenv.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Environmental Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=1&TYPE=2&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Environmental\"></i></span>";
                    lnkenv.Controls.Add(html);
                    lnkenv.ToolTip = "Environmental Hazard";
                    lnkenv.Enabled = false;
                }
            }

            LinkButton lnkPPE = (LinkButton)e.Item.FindControl("lnkPPE");
            if (lnkPPE != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDPPEYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkPPE.Controls.Add(html);
                    lnkPPE.ToolTip = "";
                    lnkPPE.Enabled = false;
                }
                else
                {
                    lnkPPE.Visible = SessionUtil.CanAccess(this.ViewState, lnkPPE.CommandName);
                    lnkPPE.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Recommended PPE', '" + Session["sitepath"] + "/Inspection/InspectionJHAPPEWithIcons.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-PPE\"></i></span>";
                    lnkPPE.Controls.Add(html);
                    lnkPPE.ToolTip = "Recommended PPE";
                    lnkPPE.Enabled = false;
                }
            }

            LinkButton lnkcomponent = (LinkButton)e.Item.FindControl("lnkcomponent");
            if (lnkcomponent != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDEQUIPMENTYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkcomponent.Controls.Add(html);
                    lnkcomponent.ToolTip = "";
                    lnkcomponent.Enabled = false;
                }
                else
                {
                    lnkcomponent.Visible = SessionUtil.CanAccess(this.ViewState, lnkcomponent.CommandName);
                    lnkcomponent.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Equipment', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedComponents.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-PMS\"></i></span>";
                    lnkcomponent.Controls.Add(html);
                    lnkcomponent.ToolTip = "Equipment";
                    lnkcomponent.Enabled = false;
                }
            }

            LinkButton lnkprocedure = (LinkButton)e.Item.FindControl("lnkprocedure");
            if (lnkprocedure != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDPROCEDUREYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkprocedure.Controls.Add(html);
                    lnkprocedure.ToolTip = "";
                    lnkprocedure.Enabled = false;
                }
                else
                {
                    lnkprocedure.Visible = SessionUtil.CanAccess(this.ViewState, lnkprocedure.CommandName);
                    lnkprocedure.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Procedure', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedProcedure.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-process\"></i></span>";
                    lnkprocedure.Controls.Add(html);
                    lnkprocedure.ToolTip = "Procedure";
                    lnkprocedure.Enabled = false;
                }
            }

            LinkButton lnkforms = (LinkButton)e.Item.FindControl("lnkforms");
            if (lnkforms != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDFORMSANDCHECKLISTYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkforms.Controls.Add(html);
                    lnkforms.ToolTip = "";
                    lnkforms.Enabled = false;
                }
                else
                {
                    lnkforms.Visible = SessionUtil.CanAccess(this.ViewState, lnkforms.CommandName);
                    lnkforms.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Forms and Checklist', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedForms.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span>";
                    lnkforms.Controls.Add(html);
                    lnkforms.ToolTip = "Forms and Checklist";
                    lnkforms.Enabled = false;
                }
            }

            LinkButton lnkWorkPermits = (LinkButton)e.Item.FindControl("lnkWorkPermits");
            if (lnkWorkPermits != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDWORKPERMITYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkWorkPermits.Controls.Add(html);
                    lnkWorkPermits.ToolTip = "";
                    lnkWorkPermits.Enabled = false;
                }
                else
                {
                    lnkWorkPermits.Visible = SessionUtil.CanAccess(this.ViewState, lnkWorkPermits.CommandName);
                    lnkWorkPermits.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Work Permit', '" + Session["sitepath"] + "/Inspection/InspectionJHAWorkPermitWithIcons.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-administration\"></i></span>";
                    lnkWorkPermits.Controls.Add(html);
                    lnkWorkPermits.ToolTip = "Work Permit";
                    lnkWorkPermits.Enabled = false;
                }

            }

            LinkButton lnkEPSS = (LinkButton)e.Item.FindControl("lnkEPSS");
            if (lnkEPSS != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDEPSSYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkEPSS.Controls.Add(html);
                    lnkEPSS.ToolTip = "";
                    lnkEPSS.Enabled = false;
                }
                else
                {
                    lnkEPSS.Visible = SessionUtil.CanAccess(this.ViewState, lnkEPSS.CommandName);
                    lnkEPSS.Attributes.Add("onclick", "openNewWindow('codehelp1', 'EPSS', '" + Session["sitepath"] + "/Inspection/InspectionJHAEPSSList.aspx?JHAYN=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Elog\"></i></span>";
                    lnkEPSS.Controls.Add(html);
                    lnkEPSS.ToolTip = "EPSS";
                    lnkEPSS.Enabled = false;
                }
            }


            LinkButton ed = (LinkButton)e.Item.FindControl("lnkJob");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
           
            RadLabel lblHS = (RadLabel)e.Item.FindControl("lblHS");
            RadLabel lblENV = (RadLabel)e.Item.FindControl("lblENV");
            RadLabel lblECO = (RadLabel)e.Item.FindControl("lblECO");
            RadLabel lblWCS = (RadLabel)e.Item.FindControl("lblWCS");
            RadLabel lblmaxvalue = (RadLabel)e.Item.FindControl("lblmaxvalue");
            RadLabel lblminvalue = (RadLabel)e.Item.FindControl("lblminvalue");

            DataRowView dv = (DataRowView)e.Item.DataItem;

            decimal minscore = 0, maxscore = 0;

            if (!string.IsNullOrEmpty(dv["FLDMINVALUE"].ToString()))
                minscore = decimal.Parse(dv["FLDMINVALUE"].ToString());

            if (!string.IsNullOrEmpty(dv["FLDMAXVALUE"].ToString()))
                maxscore = decimal.Parse(dv["FLDMAXVALUE"].ToString());

            if (lblHS != null)
            {
                lblHS.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblHS.Text))
                {

                    if (decimal.Parse(lblHS.Text) <= minscore)
                        lblHS.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblHS.Text) > minscore && decimal.Parse(lblHS.Text) <= maxscore)
                        lblHS.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblHS.Text) > maxscore)
                        lblHS.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblENV != null)
            {
                lblENV.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblENV.Text))
                {
                    if (decimal.Parse(lblENV.Text) <= minscore)
                        lblENV.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblENV.Text) > minscore && decimal.Parse(lblENV.Text) <= maxscore)
                        lblENV.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblENV.Text) > maxscore)
                        lblENV.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblECO != null)
            {
                lblECO.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblECO.Text))
                {
                    if (decimal.Parse(lblECO.Text) <= minscore)
                        lblECO.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblECO.Text) > minscore && decimal.Parse(lblECO.Text) <= maxscore)
                        lblECO.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblECO.Text) > maxscore)
                        lblECO.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblWCS != null)
            {
                lblWCS.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblWCS.Text))
                {
                    if (decimal.Parse(lblWCS.Text) <= minscore)
                        lblWCS.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblWCS.Text) > minscore && decimal.Parse(lblWCS.Text) <= maxscore)
                        lblWCS.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblWCS.Text) > maxscore)
                        lblWCS.BackColor = System.Drawing.Color.Red;
                }
            }
            //SetRowSelection();
        }
    }

    protected void gvRiskAssessmentJobHazardAnalysis_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("REPORT"))
            {
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.InspectionNewJHAReportExtn(new Guid(lbl.Text));

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "9");
                    nvc.Add("reportcode", "JOBHAZARDNEW");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    string filename = "JOBHAZARDNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                    PhoenixSsrsReportsCommon.getVersion();
                    PhoenixSsrsReportsCommon.getLogo();
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                }
            }
            if (gce.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void BindCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlCategory = sender as RadComboBox;
        ddlCategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = PhoenixInspectionRiskAssessmentActivityExtn.ListJobHazardAnalysis();

        RadComboBox ddlStatus = sender as RadComboBox;
        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSUBCATEGORY").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSTATUSID").CurrentFilterValue = e.Value;
        ViewState["ddlStatus"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void SetFilter()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        NameValueCollection criteria = new NameValueCollection();
        criteria.Add("txtHazardNo", ViewState["txtHazardNo"].ToString());
        criteria.Add("ddlCategory", ViewState["ddlCategory"].ToString());
        criteria.Add("ddlStatus", ViewState["ddlStatus"].ToString());
        criteria.Add("txtjob", ViewState["txtjob"].ToString());
        criteria.Add("VesselID", ViewState["VesselID"].ToString());
        Filter.CurrentJHAFilter = criteria;
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblJobHazardid = (RadLabel)gvRiskAssessmentJobHazardAnalysis.Items[rowindex].FindControl("lblJobHazardid");
            if (lblJobHazardid != null)
            {
                Filter.CurrentSelectedJHA = lblJobHazardid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        //gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
        //for (int i = 0; i < gvRiskAssessmentJobHazardAnalysis.Items.Count; i++)
        //{
        //    if (gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].GetDataKeyValue("FLDJOBHAZARDID").ToString().Equals(Filter.CurrentSelectedJHA.ToString()))
        //    {
        //        gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].Selected = true;
        //    }
        //}
    }

    protected void btnConfirmCopy_Click(object sender, EventArgs e)
    {
        try
        {
            string jobhazardid = Filter.CurrentSelectedJHA;
            if (jobhazardid != null && jobhazardid != "" && General.GetNullableGuid(jobhazardid) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardCopy(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(jobhazardid)
                                                                                    , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                ucStatus.Text = "JHA is copied successfully.";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            string jobhazardid = Filter.CurrentSelectedJHA;
            if (jobhazardid != null && jobhazardid != "" && General.GetNullableGuid(jobhazardid) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRiskAssessmentJobHazardRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(jobhazardid));

                Rebind();
                ucStatus.Text = "JHA is revised.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void gvRiskAssessmentJobHazardAnalysis_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lbljobhazardid = ((RadLabel)e.Item.FindControl("lblJobHazardid"));
            PhoenixInspectionRiskAssessmentJobHazardExtn.DeleteRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(lbljobhazardid.Text));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessmentJobHazardAnalysis_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentJobHazardAnalysis.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_DataBinding_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDVESSELID").CurrentFilterValue = e.Value;
        ViewState["VesselID"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
        gvRiskAssessmentJobHazardAnalysis.Rebind();
    }
    protected void ucVessel_DataBinding(object sender, EventArgs e)
    {

        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
        RadComboBox ucVessel = sender as RadComboBox;
        ucVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ucVessel.DataSource = dt;

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        dt.Tables[0].PrimaryKey = keyColumns;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.Enabled = false;
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ucVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }

    }
    private void BindStatus()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDSTATUSID", typeof(string));
        dt.Columns.Add("FLDSTATUSNAME", typeof(string));
        dt.Rows.Add("1", "Draft");
        dt.Rows.Add("2", "Approved");
        dt.Rows.Add("3", "Issued");
        dt.Rows.Add("8", "Preparation");
        dt.Rows.Add("4", "Pending approval");
        dt.Rows.Add("5", "Approved for use");
        dt.Rows.Add("6", "Not Approved");
        //dt.Rows.Add("7", "Completed");                

        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindCategory()
    {
        DataTable ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds;
        ddlCategory.DataBind();

    }
    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void ddlStatus_Changed(object sender, EventArgs e)
    {

    }
}
