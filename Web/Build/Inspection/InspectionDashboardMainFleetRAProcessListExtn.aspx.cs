using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Reports;
using System.Web.UI.HtmlControls;

public partial class InspectionDashboardMainFleetRAProcessListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentProcess')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionMainFleetRAProcessListExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0))
        //{
        //    toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //}

        if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
        {
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        }
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Job Hazard Analysis", "JOBHAZARDANALYSIS");
        toolbarmain.AddButton("Process RA", "PROCESSRA");
        toolbarmain.AddButton("Standard Templates", "STANDARDTEMPLATES");
        toolbarmain.AddButton("Non Routine RA", "NONROUTINERA");
        MenuARSubTab.AccessRights = this.ViewState;
        MenuARSubTab.MenuList = toolbarmain.Show();

        MenuARSubTab.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {

            ucConfirm.Attributes.Add("style", "display:none");
            ucConfirmIssue.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");

            ViewState["REFNO"]          = "";
            ViewState["COMPANYID"] = "";
            ViewState["PAGENUMBER"]     = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"]  = null;

            ViewState["ddlStatus"]      = string.Empty;
            ViewState["ddlRAType"]      = string.Empty;
            ViewState["ddlCategory"]    = string.Empty;
            ViewState["txtRefNo"]       = string.Empty;

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }

            if (Filter.CurrentProcessRAFilter == null)
            {
                SetFilter();
            }

            gvRiskAssessmentProcess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuARSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("JOBHAZARDANALYSIS"))
        {
            Response.Redirect("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx");
        }
        if (CommandName.ToUpper().Equals("NONROUTINERA"))
        {
            Response.Redirect("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx");
        }
        if (CommandName.ToUpper().Equals("STANDARDTEMPLATES"))
        {
            Response.Redirect("../Inspection/InspectionStandardTemplateNonRoutineRA.aspx");
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDHSSCORE", "FLDENVSCORE", "FLDECOSCORE", "FLDWSSCORE" };
        string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Approved", "Health & Safety", "Environmental", "Economic/Process Loss", "Worst Case" };

        NameValueCollection nvc = Filter.CurrentProcessRAFilter;

        DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentMainFleetDashboardProcessSearch(
                                                                                  nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                                                                                , nvc.Get("ddlCategory") != null ? General.GetNullableInteger(nvc["ddlCategory"]) : General.GetNullableInteger(ViewState["ddlCategory"].ToString())
                                                                                , 3
                                                                                , General.GetNullableGuid(ViewState["REFNO"].ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                , gvRiskAssessmentProcess.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount
                                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                                , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString()));

        General.SetPrintOptions("gvRiskAssessmentProcess", "Risk Assessment-Process", alCaptions, alColumns, ds);

        gvRiskAssessmentProcess.DataSource = ds;
        gvRiskAssessmentProcess.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedProcessRA == null)
            {
                Filter.CurrentSelectedProcessRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSID"].ToString();
                gvRiskAssessmentProcess.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }
        ViewState["ROWCOUNT"] = iRowCount;
    }

    private void SetRowSelection()
    {
        gvRiskAssessmentProcess.SelectedIndexes.Clear();
        for (int i = 0; i < gvRiskAssessmentProcess.Items.Count; i++)
        {
            if (gvRiskAssessmentProcess.MasterTableView.Items[i].GetDataKeyValue("FLDRISKASSESSMENTPROCESSID").ToString().Equals(Filter.CurrentSelectedProcessRA.ToString()))
            {
                gvRiskAssessmentProcess.MasterTableView.Items[i].Selected = true;
            }
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDHSSCORE", "FLDENVSCORE", "FLDECOSCORE", "FLDWSSCORE" };
            string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Approved", "Health & Safety", "Environmental", "Economic/Process Loss", "Worst Case" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection nvc = Filter.CurrentProcessRAFilter;

            DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentMainFleetDashboardProcessSearch(
                                                                                      nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                                                                                    , nvc.Get("ddlCategory") != null ? General.GetNullableInteger(nvc["ddlCategory"]) : General.GetNullableInteger(ViewState["ddlCategory"].ToString())
                                                                                    , 3
                                                                                    , General.GetNullableGuid(ViewState["REFNO"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvRiskAssessmentProcess.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount
                                                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                                    , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString()));

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRiskAssessmentProcess.SelectedIndexes.Clear();
        gvRiskAssessmentProcess.EditIndexes.Clear();
        gvRiskAssessmentProcess.DataSource = null;
        gvRiskAssessmentProcess.Rebind();
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAProcessExtn.aspx?status=", false);
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["ddlStatus"] = string.Empty;
            ClearGridFilter(gvRiskAssessmentProcess.MasterTableView.GetColumn("FLDSTATUS"));

            ViewState["ddlRAType"] = string.Empty;
            ClearGridFilter(gvRiskAssessmentProcess.MasterTableView.GetColumn("FLDTYPE"));

            ViewState["ddlCategory"] = string.Empty;
            ClearGridFilter(gvRiskAssessmentProcess.MasterTableView.GetColumn("FLDPROCESSNAME"));

            ViewState["txtRefNo"] = string.Empty;
            ClearGridFilter(gvRiskAssessmentProcess.MasterTableView.GetColumn("FLDNUMBER"));

            ViewState["PAGENUMBER"] = 1;

            Filter.CurrentProcessRAFilter.Clear();
            SetFilter();
            Rebind();
        }
    }

    private void ClearGridFilter(GridColumn column)
    {
        column.ListOfFilterValues = null;
        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        column.CurrentFilterValue = string.Empty;
    }

    protected void gvRiskAssessmentProcess_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblProcessID = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cmdRAProcess = (LinkButton)e.Item.FindControl("cmdRAProcess");
            if (cmdRAProcess != null)
            {
                cmdRAProcess.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAProcess.CommandName);
            }

            LinkButton imgrev = (LinkButton)e.Item.FindControl("imgrevision");
            if (imgrev != null) imgrev.Visible = SessionUtil.CanAccess(this.ViewState, imgrev.CommandName);

            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblStatusID = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblactiveyn = (RadLabel)e.Item.FindControl("lblActiveyn");
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            //LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
            LinkButton imgOfficeCopy = (LinkButton)e.Item.FindControl("imgOfficeCopy");

            LinkButton cmdRevisions = (LinkButton)e.Item.FindControl("cmdRevisions");
            RadLabel lblReferencid = (RadLabel)e.Item.FindControl("lblReferencid");


            if (cmdRevisions != null)
                cmdRevisions.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAProcessRevisionListExtn.aspx?referenceid=" + lblReferencid.Text + "'); return true;");

            if (lblInstallcode != null && General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0)
            {
                if (imgApprove != null)
                    imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblProcessID.Text + "&TYPE=4','medium'); return true;");
            }
            if (imgOfficeCopy != null)
                imgOfficeCopy.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionCopyJHARAExtn.aspx?RAPROCESSID=" + lblProcessID.Text + "&Type=RAPROCESS','medium'); return true;");

            if (lblStatusID.Text == "1")
            {
                imgApprove.Visible = true;
                //imgIssue.Visible = false;
            }

            else if (lblStatusID.Text == "2")
            {
                //imgApprove.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgApprove.Controls.Add(html);
                imgApprove.ToolTip = "";
                imgApprove.Enabled = false;
                //imgIssue.Visible = true;
            }
            else if (lblStatusID.Text == "3")
            {
                //imgApprove.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgApprove.Controls.Add(html);
                imgApprove.ToolTip = "";
                imgApprove.Enabled = false;
                //imgIssue.Visible = false;
            }

            if (lblStatus.Text.ToUpper() == "DRAFT" || lblStatus.Text.ToUpper() == "APPROVED" || lblactiveyn.Text == "0")
            {
                if (imgrev != null)
                {
                    //imgrev.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrev.Controls.Add(html);
                    imgrev.ToolTip = "";
                    imgrev.Enabled = false;
                }
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgrev != null)
                {
                    //imgrev.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrev.Controls.Add(html);
                    imgrev.ToolTip = "";
                    imgrev.Enabled = false;
                }
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgOfficeCopy != null) imgOfficeCopy.Visible = false;
            }

            if (imgrev != null && !SessionUtil.CanAccess(this.ViewState, imgrev.CommandName))
            {
                //imgrev.Visible = false;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                imgrev.Controls.Add(html);
                imgrev.ToolTip = "";
                imgrev.Enabled = false;
            }

            LinkButton imgFilter = (LinkButton)e.Item.FindControl("imgFilter");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                if (imgFilter != null) imgFilter.Visible = false;

            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            //if (imgIssue != null)
            //{
            //    if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
            //}
            if (imgrev != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgrev.CommandName))
                {
                    //imgrev.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    imgrev.Controls.Add(html);
                    imgrev.ToolTip = "";
                    imgrev.Enabled = false;
                }
            }
            if (imgFilter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgFilter.CommandName)) imgFilter.Visible = false;
            }
            if (cmdRAProcess != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRAProcess.CommandName))
                {
                    //cmdRAProcess.Visible = false;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    cmdRAProcess.Controls.Add(html);
                    cmdRAProcess.ToolTip = "";
                    cmdRAProcess.Enabled = false;
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

            if (imgOfficeCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgOfficeCopy.CommandName)) imgOfficeCopy.Visible = false;
            }

            RadLabel lblHS = (RadLabel)e.Item.FindControl("lblHealth");
            RadLabel lblENV = (RadLabel)e.Item.FindControl("lblEnvironmental");
            RadLabel lblECO = (RadLabel)e.Item.FindControl("lblEcononmic");
            RadLabel lblWCS = (RadLabel)e.Item.FindControl("lblWorstCase");

            DataRowView dv = (DataRowView)e.Item.DataItem;

            decimal minscore = 0, maxscore = 0;

            if (!string.IsNullOrEmpty(dv["FLDMINSCORE"].ToString()))
                minscore = decimal.Parse(dv["FLDMINSCORE"].ToString());

            if (!string.IsNullOrEmpty(dv["FLDMAXSCORE"].ToString()))
                maxscore = decimal.Parse(dv["FLDMAXSCORE"].ToString());

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
            RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");

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
                    lnkHealth.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Health and Safety Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=0&TYPE=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
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
                    lnkenv.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Environmental Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=0&TYPE=2&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
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
                    lnkPPE.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Recommended PPE', '" + Session["sitepath"] + "/Inspection/InspectionJHAPPEWithIcons.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
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
                    lnkcomponent.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Equipment', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedComponents.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
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
                    lnkprocedure.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Procedure', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedProcedure.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
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
                    lnkforms.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Forms and Checklist', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedForms.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
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
                    lnkWorkPermits.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Work Permit', '" + Session["sitepath"] + "/Inspection/InspectionJHAWorkPermitWithIcons.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
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
                    lnkEPSS.Attributes.Add("onclick", "openNewWindow('codehelp1', 'EPSS', '" + Session["sitepath"] + "/Inspection/InspectionJHAEPSSList.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Elog\"></i></span>";
                    lnkEPSS.Controls.Add(html);
                    lnkEPSS.ToolTip = "EPSS";
                    lnkEPSS.Enabled = false;
                }
            }
        }
    }

    protected void gvRiskAssessmentProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentProcess.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessmentProcess_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatusID");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                Response.Redirect("../Inspection/InspectionRAProcessExtn.aspx?processid=" + lbl.Text + "&status=" + lblstatus.Text + "&HSEQADashboardYN=1", false);
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    BindPageURL(gce.Item.ItemIndex);
                    SetRowSelection();
                    ViewState["PROCESSID"] = lbl.Text;

                    RadWindowManager1.RadConfirm("RA cannot be edited after it is issued. Are you sure to continue?", "ConfirmIssue", 320, 150, null, "ConfirmIssue");
                }
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                ViewState["PROCESSID"] = lbl.Text;

                RadWindowManager1.RadConfirm("RA cannot be edited after it is issued. Are you sure to continue?", "ConfirmIssue", 320, 150, null, "ConfirmIssue");
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                ViewState["PROCESSID"] = lbl.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "confirmRevision", 320, 150, null, "confirmRevision");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("CLEARFILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                ViewState["REFNO"] = "";
            }
            if (gce.CommandName.ToUpper().Equals("COPY"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                ViewState["PROCESSID"] = lbl.Text;

                RadWindowManager1.RadConfirm("Are you sure to copy the template?", "confirm", 320, 150, null, "confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("RAPROCESS"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }

            if (gce.CommandName.ToUpper().Equals("REPORT"))
            {
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionNewProcessRAReportExtn(new Guid(lbl.Text));

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "9");
                    nvc.Add("reportcode", "RAPROCESSNEW");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    string filename = "PROCESSNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                    PhoenixSsrsReportsCommon.getVersion();
                    PhoenixSsrsReportsCommon.getLogo();
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                }
            }

            if (gce.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)gce.CommandArgument;
                ViewState["PAGENUMBER"] = "1";

                ViewState["txtRefNo"] = gvRiskAssessmentProcess.MasterTableView.GetColumn("FLDNUMBER").CurrentFilterValue;
                SetFilter();
                gvRiskAssessmentProcess.Rebind();
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

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcessExtn.CopyRiskAssessmentProcess(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(ViewState["PROCESSID"].ToString()),
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "RA is copied.";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmIssue_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , new Guid(ViewState["PROCESSID"].ToString())
                                                                              , null
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , 3);
                ucStatus.Text = "RA is Approved successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentProcess.Rebind();
    }
    

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentProcessID = (RadLabel)gvRiskAssessmentProcess.Items[rowindex].FindControl("lblRiskAssessmentProcessID");
            if (lblRiskAssessmentProcessID != null)
            {
                Filter.CurrentSelectedProcessRA = lblRiskAssessmentProcessID.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["PROCESSID"].ToString()));
                ucStatus.Text = "RA is revised.";
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

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("ddlStatus", ViewState["ddlStatus"].ToString());
        criteria.Add("ddlRAType", ViewState["ddlRAType"].ToString());
        criteria.Add("ddlCategory", ViewState["ddlCategory"].ToString());

        Filter.CurrentProcessRAFilter = criteria;
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSTATUS").CurrentFilterValue = e.Value;
        ViewState["ddlStatus"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDPROCESSNAME").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            RadComboBox ddlCategory = sender as RadComboBox; 
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataTextField = "FLDNAME";
            ddlCategory.DataValueField = "FLDACTIVITYID";
        }
    }

    protected void ddlRAType_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();

        RadComboBox ddlRAType = sender as RadComboBox;
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCATEGORYID";
        ddlRAType.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void ddlRAType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDTYPE").CurrentFilterValue = e.Value;
        ViewState["ddlRAType"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
