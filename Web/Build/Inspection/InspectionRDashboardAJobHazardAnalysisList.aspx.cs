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
using System.Web.UI.HtmlControls;

public partial class InspectionRDashboardAJobHazardAnalysisList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentJobHazardAnalysis')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        try
        {
            if (!IsPostBack)
            {
                ViewState["REFNO"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["txtHazardNo"] = string.Empty;
                ViewState["ddlCategory"] = string.Empty;
                ViewState["ddlStatus"] = string.Empty;
                ViewState["txtjob"] = string.Empty;
                ViewState["VesselID"] = string.Empty;

                ucConfirmCopy.Attributes.Add("style", "display:none");
                ucConfirmRevision.Attributes.Add("style", "display:none");
                
                ViewState["COMPANYID"] = "";
                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                }
                //BindCategory();

                if (Filter.CurrentJHAFilter == null)
                {
                    SetFilter();
                }

                gvRiskAssessmentJobHazardAnalysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx", "Find", "search.png", "SEARCH");
                toolbar.AddImageButton("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            }
            else
            {
                ViewState["callfrom"] = "";

            }
            MenuJobHazardAnalysis.AccessRights = this.ViewState;
            MenuJobHazardAnalysis.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Job Hazard Analysis", "JOBHAZARDANALYSIS");
            toolbarmain.AddButton("Process RA", "PROCESSRA");
            toolbarmain.AddButton("Standard Templates", "STANDARDTEMPLATES");
            toolbarmain.AddButton("Non Routine RA", "NONROUTINERA");
            MenuARSubTab.AccessRights = this.ViewState;
            MenuARSubTab.MenuList = toolbarmain.Show();
            MenuARSubTab.SelectedMenuIndex = 0;
        }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuARSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("NONROUTINERA"))
        {
            Response.Redirect("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx");
        }
        if (CommandName.ToUpper().Equals("STANDARDTEMPLATES"))
        {
            Response.Redirect("../Inspection/InspectionStandardTemplateNonRoutineRA.aspx");
        }
        if (CommandName.ToUpper().Equals("PROCESSRA"))
        {
            Response.Redirect("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx");
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHAZARDNUMBER", "FLDVESSELNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDHSRR", "FLDENVRR", "FLDECORR", "FLDWCRR" };
        string[] alCaptions = { "Hazard Number", "Vessel Name", "Process", "Job", "Status", "Revision No", "Approved", "Health & Safety", "Environmental", "Economic/Process Loss", "Worst Case" };

        NameValueCollection nvc = Filter.CurrentJHAFilter;

        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardDashboardSearch(
                    nvc.Get("txtHazardNo") != null ? General.GetNullableString(nvc.Get("txtHazardNo").ToString()) : General.GetNullableString(ViewState["txtHazardNo"].ToString()),
                    General.GetNullableInteger(null),
                    nvc.Get("ddlCategory") != null ? General.GetNullableInteger(nvc.Get("ddlCategory").ToString()) : General.GetNullableInteger(ViewState["ddlCategory"].ToString()),
                    3,
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentJobHazardAnalysis.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    nvc.Get("VesselID") != null ? General.GetNullableInteger(nvc.Get("VesselID").ToString()) : General.GetNullableInteger(ViewState["VesselID"].ToString()),
                    nvc.Get("txtJob") != null ? General.GetNullableString(nvc.Get("txtJob").ToString()) : General.GetNullableString(ViewState["txtJob"].ToString()),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvRiskAssessmentJobHazardAnalysis", "Job Hazard Analysis", alCaptions, alColumns, ds);

        gvRiskAssessmentJobHazardAnalysis.DataSource = ds;
        gvRiskAssessmentJobHazardAnalysis.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedJHA == null)
            {
                Filter.CurrentSelectedJHA = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }
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

            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RiskAssessmentJobHazardDashboardSearch(
                    nvc.Get("txtHazardNo") != null ? General.GetNullableString(nvc.Get("txtHazardNo").ToString()) : General.GetNullableString(ViewState["txtHazardNo"].ToString()),
                    General.GetNullableInteger(null),
                    nvc.Get("ddlCategory") != null ? General.GetNullableInteger(nvc.Get("ddlCategory").ToString()) : General.GetNullableInteger(ViewState["ddlCategory"].ToString()),
                    3,
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


        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisExtn.aspx?status=", false);
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["txtHazardNo"] = string.Empty;
            ViewState["ddlCategory"] = string.Empty;
            ViewState["ddlStatus"] = string.Empty;
            ViewState["txtjob"] = string.Empty;
            ViewState["VesselID"] = string.Empty;            

            ViewState["PAGENUMBER"] = 1;

            Filter.CurrentJHAFilter.Clear();
            SetFilter();

            Rebind();
            gvRiskAssessmentJobHazardAnalysis.Rebind();
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

            LinkButton ed = (LinkButton)e.Item.FindControl("lnkJob");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton imgdelete = (LinkButton)e.Item.FindControl("cmdDelete");
            imgdelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                //imgdelete.Visible = false;
                HtmlGenericControl emptyhtml = new HtmlGenericControl();
                emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgdelete.Controls.Add(emptyhtml);
                imgdelete.ToolTip = "";
                imgdelete.Enabled = false;
            }

            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            HtmlGenericControl imgrevisionhtml = new HtmlGenericControl();

            if (imgrevision != null) imgrevision.Visible = SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName);

            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblactiveyn = (RadLabel)e.Item.FindControl("lblActiveyn");
            if (lblStatus.Text.ToUpper() == "DRAFT" || lblStatus.Text.ToUpper() == "APPROVED" || lblactiveyn.Text == "0")
            {
                //imgrevision.Visible = false;
                imgrevisionhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevision.Controls.Add(imgrevisionhtml);
                imgrevision.ToolTip = "";
                imgrevision.Enabled = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                //imgrevision.Visible = false;
                imgrevisionhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevision.Controls.Add(imgrevisionhtml);
                imgrevision.ToolTip = "";
                imgrevision.Enabled = false;
            }

            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
            LinkButton imgAnalysis = (LinkButton)e.Item.FindControl("imgAnalysis");
            if (imgAnalysis != null)
            {
                imgAnalysis.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisReviewExtn.aspx?jobhazardid=" + lbljobhazardid.Text + "'); return false;");
            }

            LinkButton jobhzd = (LinkButton)e.Item.FindControl("cmdJobHazard");
            if (jobhzd != null)
            {
                jobhzd.Visible = SessionUtil.CanAccess(this.ViewState, jobhzd.CommandName);
            }

            LinkButton cmdRevisions = (LinkButton)e.Item.FindControl("cmdRevisions");
            LinkButton imgCopy = (LinkButton)e.Item.FindControl("imgCopy");
            LinkButton imgOfficeCopy = (LinkButton)e.Item.FindControl("imgOfficeCopy");

            RadLabel lblReferencid = (RadLabel)e.Item.FindControl("lblReferencid");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                //imgCopy.Visible = false;
                //imgdelete.Visible = false;
                //imgOfficeCopy.Visible = false;
                HtmlGenericControl emptyhtml = new HtmlGenericControl();
                emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgCopy.Controls.Add(emptyhtml);
                imgCopy.ToolTip = "";
                imgCopy.Enabled = false;
                imgdelete.Controls.Add(emptyhtml);
                imgdelete.ToolTip = "";
                imgdelete.Enabled = false;
                imgOfficeCopy.Controls.Add(emptyhtml);
                imgOfficeCopy.ToolTip = "";
                imgOfficeCopy.Enabled = false;
            }

            if (cmdRevisions != null)
                cmdRevisions.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisRevisionListExtn.aspx?referenceid=" + lblReferencid.Text + "'); return true;");

            if (imgOfficeCopy != null)
                imgOfficeCopy.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionCopyJHARAExtn.aspx?JOBHAZARDID=" + lbljobhazardid.Text + "&Type=JHA','medium'); return true;");
            if (imgrevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName))
                {
                    //imgrevision.Visible = false;
                    imgrevisionhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrevision.Controls.Add(imgrevisionhtml);
                    imgrevision.ToolTip = "";
                    imgrevision.Enabled = false;
                }
            }
            if (jobhzd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, jobhzd.CommandName))
                {
                    //jobhzd.Visible = false;
                    HtmlGenericControl emptyhtml = new HtmlGenericControl();
                    emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    jobhzd.Controls.Add(emptyhtml);
                    jobhzd.ToolTip = "";
                    jobhzd.Enabled = false;
                }
            }
            if (cmdRevisions != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevisions.CommandName))
                {
                    //cmdRevisions.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    cmdRevisions.Controls.Add(html);
                    cmdRevisions.ToolTip = "";
                    cmdRevisions.Enabled = false;
                }
            }
            if (imgCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName))
                {
                    //imgCopy.Visible = false;
                    HtmlGenericControl emptyhtml = new HtmlGenericControl();
                    emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgCopy.Controls.Add(emptyhtml);
                    imgCopy.ToolTip = "";
                    imgCopy.Enabled = false;
                }
            }
            if (imgdelete != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgdelete.CommandName))
                {
                    //imgdelete.Visible = false;
                    HtmlGenericControl emptyhtml = new HtmlGenericControl();
                    emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgdelete.Controls.Add(emptyhtml);
                    imgdelete.ToolTip = "";
                    imgdelete.Enabled = false;
                }
            }
            if (imgOfficeCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgOfficeCopy.CommandName))
                {
                    //imgOfficeCopy.Visible = false;
                    HtmlGenericControl emptyhtml = new HtmlGenericControl();
                    emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgOfficeCopy.Controls.Add(emptyhtml);
                    imgOfficeCopy.ToolTip = "";
                    imgOfficeCopy.Enabled = false;
                }
            }


            //imgrevision.Visible = false;            
            imgrevisionhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
            imgrevision.Controls.Add(imgrevisionhtml);
            imgrevision.ToolTip = "";
            imgrevision.Enabled = false;
            //imgCopy.Visible = false;
            imgCopy.Controls.Add(imgrevisionhtml);
            imgCopy.ToolTip = "";
            imgCopy.Enabled = false;

            if (lblVesselid != null && lblVesselid.Text == "0")
            {
                if (cmdRevisions != null) cmdRevisions.Visible = true;
            }
            else
            {
                if (cmdRevisions != null)
                {
                    //cmdRevisions.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    cmdRevisions.Controls.Add(html);
                    cmdRevisions.ToolTip = "";
                    cmdRevisions.Enabled = false;
                }
            }

            if (lblStatusid.Text == "1")
            {
                //imgrevision.Visible = false;
                //imgCopy.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                HtmlGenericControl imgrevhtml = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevision.Controls.Add(imgrevhtml);
                imgrevision.ToolTip = "";
                imgrevision.Enabled = false;
                imgCopy.Controls.Add(html);
                imgCopy.ToolTip = "";
                imgCopy.Enabled = false;
            }
            else if (lblStatusid.Text == "2")
            {
                //imgrevision.Visible = false;
                //imgCopy.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevision.Controls.Add(html);
                imgrevision.ToolTip = "";
                imgrevision.Enabled = false;
                imgCopy.Controls.Add(html);
                imgCopy.ToolTip = "";
                imgCopy.Enabled = false;
            }
            else if (lblStatusid.Text == "3")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    imgrevision.Visible = true;
                    imgrevision.ToolTip = "Create Revision";
                    imgrevision.Enabled = true;
                }
                else
                {
                    //imgrevision.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrevision.Controls.Add(html);
                    imgrevision.ToolTip = "";
                    imgrevision.Enabled = false;
                }

                //imgCopy.Visible = true;
                HtmlGenericControl copyhtml = new HtmlGenericControl();
                copyhtml.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-copy\"></i></span>";
                imgCopy.Controls.Add(copyhtml);
                imgCopy.ToolTip = "Copy";
                imgCopy.Enabled = true;
            }
            else if (lblStatusid.Text == "4")
            {
                //imgrevision.Visible = false;
                //imgCopy.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrevision.Controls.Add(html);
                imgrevision.ToolTip = "";
                imgrevision.Enabled = false;
                imgCopy.Controls.Add(html);
                imgCopy.ToolTip = "";
                imgCopy.Enabled = false;
            }
            else if (lblStatusid.Text == "5")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                {
                    //imgrevision.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrevision.Controls.Add(html);
                    imgrevision.ToolTip = "";
                    imgrevision.Enabled = false;
                }
                //imgCopy.Visible = true;
                HtmlGenericControl copyhtml = new HtmlGenericControl();
                copyhtml.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-copy\"></i></span>";
                imgCopy.Controls.Add(copyhtml);
                imgCopy.ToolTip = "Copy";
                imgCopy.Enabled = true;
            }
            else if (lblStatusid.Text == "6")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                {
                    //imgrevision.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrevision.Controls.Add(html);
                    imgrevision.ToolTip = "";
                    imgrevision.Enabled = false;
                }
                //imgCopy.Visible = false;
                HtmlGenericControl emptyhtml = new HtmlGenericControl();
                emptyhtml.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgCopy.Controls.Add(emptyhtml);
                imgCopy.ToolTip = "";
                imgCopy.Enabled = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgCopy != null) imgCopy.ToolTip = "Use JHA";
            }
            else
            {
                if (imgCopy != null) imgCopy.ToolTip = "Copy JHA";
            }
            if (imgCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName))
                {
                    //imgCopy.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgCopy.Controls.Add(html);
                    imgCopy.ToolTip = "";
                    imgCopy.Enabled = false;
                }
            }
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
            SetRowSelection();

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
        }
    }

    protected void gvRiskAssessmentJobHazardAnalysis_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
     {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatusID");
                Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisExtn.aspx?jobhazardid=" + lbl.Text + "&status=" + lblstatus.Text +"&HSEQADashboardYN=1", false);
                Rebind();
            }

            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();

                RadWindowManager1.RadConfirm("Are you sure you want to revise this JHA.?", "confirmRevision", 320, 150, null, "Revision");
            }
            if (gce.CommandName.ToUpper().Equals("CLEARFILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                ViewState["REFNO"] = "";
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("JOBHAZARDREPORT"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("COPY"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();

                RadWindowManager1.RadConfirm("Are you sure you want to copy this JHA.?", "ConfirmCopy", 320, 150, null, "Copy");
            }
            if (gce.CommandName.ToUpper().Equals("OFFICECOPY"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblVesselid = (RadLabel)gce.Item.FindControl("lblVesselid");
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    if (lblVesselid != null && int.Parse(lblVesselid.Text) == 0)
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRiskAssessmentJobHazardApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                        new Guid(lbl.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, 2);
                        ucStatus.Text = "Approved Successfully";
                    }
                }
                Rebind();
            }

            if (gce.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)gce.CommandArgument;
                RadLabel lbljobhazardid = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblrefno = (RadLabel)gce.Item.FindControl("lblReferencid");
                //ViewState["REFNO"] = lblrefno.Text;
                //string value = filterPair.First.ToString();//accessing function name                
                ViewState["PAGENUMBER"] = "1";
                ViewState["txtHazardNo"] = gvRiskAssessmentJobHazardAnalysis.MasterTableView.GetColumn("FLDHAZARDNUMBER").CurrentFilterValue;
                ViewState["txtjob"] = gvRiskAssessmentJobHazardAnalysis.MasterTableView.GetColumn("FLDJOB").CurrentFilterValue;
                SetFilter();
                gvRiskAssessmentJobHazardAnalysis.Rebind();
            }

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
                    string  filename = "JOBHAZARDNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
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
        gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
        for (int i = 0; i < gvRiskAssessmentJobHazardAnalysis.Items.Count; i++)
        {
            if (gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].GetDataKeyValue("FLDJOBHAZARDID").ToString().Equals(Filter.CurrentSelectedJHA.ToString()))
            {
                gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].Selected = true;
            }
        }
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
            SetFilter();
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
}
