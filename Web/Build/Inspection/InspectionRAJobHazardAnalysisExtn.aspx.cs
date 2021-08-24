using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAJobHazardAnalysisExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucCompany.SelectedCompany = ViewState["COMPANYID"].ToString();
                    ucCompany.Enabled = false;
                }
                else
                    ucCompany.Enabled = true;

                BindJob();

                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                ViewState["JOBHAZARDID"] = "";
                ViewState["IMPACTID"] = "";
                ViewState["RequestFrom"] = "";
                ViewState["EVENTID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["STATUS"] = "";

                BindRecomendedPPE();

                Chkpersonsinvolved.DataTextField = "FLDGROUPRANK";
                Chkpersonsinvolved.DataValueField = "FLDGROUPRANKID";
                Chkpersonsinvolved.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                Chkpersonsinvolved.DataBind();

                ddlsupervisionlist.DataTextField = "FLDGROUPRANK";
                ddlsupervisionlist.DataValueField = "FLDGROUPRANKID";
                ddlsupervisionlist.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ddlsupervisionlist.Items.Insert(0, new RadComboBoxItem("Not Required", "Dummy"));
                ddlsupervisionlist.DataBind();

                cblReason.DataTextField = "FLDNAME";
                cblReason.DataValueField = "FLDMISCELLANEOUSID";
                cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneousExtn.ListRiskAssessmentMiscellaneous(1, null);
                cblReason.DataBind();

                ddlPeopleInvolved.DataTextField = "FLDNAME";
                ddlPeopleInvolved.DataValueField = "FLDFREQUENCYID";
                ddlPeopleInvolved.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(3);
                ddlPeopleInvolved.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlPeopleInvolved.DataBind();

                ddlWorkDuration.DataTextField = "FLDNAME";
                ddlWorkDuration.DataValueField = "FLDFREQUENCYID";
                ddlWorkDuration.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(1);
                ddlWorkDuration.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlWorkDuration.DataBind();

                ddlWorkFrequency.DataTextField = "FLDNAME";
                ddlWorkFrequency.DataValueField = "FLDFREQUENCYID";
                ddlWorkFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(2);
                ddlWorkFrequency.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlWorkFrequency.DataBind();

                if (Request.QueryString["jobhazardid"] != null)
                    ViewState["JOBHAZARDID"] = Request.QueryString["jobhazardid"].ToString();
                if (Request.QueryString["status"] != null)
                    ViewState["STATUS"] = Request.QueryString["status"].ToString();

                if (Request.QueryString["ProcessRAYN"] != null)
                    ViewState["ProcessRAYN"] = Request.QueryString["ProcessRAYN"].ToString();
                else
                    ViewState["ProcessRAYN"] = "0";

                if (Request.QueryString["processid"] != null)
                    ViewState["processid"] = Request.QueryString["processid"].ToString();
                else
                    ViewState["processid"] = "";

                if (Request.QueryString["DMSYN"] != null)
                    ViewState["DMSYN"] = Request.QueryString["DMSYN"].ToString();
                else
                    ViewState["DMSYN"] = "0";

                if (Request.QueryString["jobhazardid"] != null)
                {
                    EditJobHazard(new Guid(Request.QueryString["jobhazardid"]));
                    ViewState["JOBHAZARDID"] = Request.QueryString["jobhazardid"];
                }

                if (Request.QueryString["HSEQADashboardYN"] != null)
                    ViewState["HSEQADashboardYN"] = Request.QueryString["HSEQADashboardYN"].ToString();
                else
                    ViewState["HSEQADashboardYN"] = "0";

                if (Request.QueryString["PROCESSDASHBOARDYN"] != null)
                    ViewState["PROCESSDASHBOARDYN"] = Request.QueryString["PROCESSDASHBOARDYN"].ToString();
                else
                    ViewState["PROCESSDASHBOARDYN"] = "0";
            }
            BindComponents();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionOperationalHazardPickList.aspx?JhaId=" + ViewState["JOBHAZARDID"].ToString() + "')", "Map from Aspects Register", "<i class=\"fas fa-tasks\"></i>", "ADD");
            toolbarsub.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionGobalRAOperationalHazard.aspx')", "Add New Aspect", "<i class=\"fa fa-plus-circle\"></i>", "MADD");
            MenuOperationalHazard.AccessRights = this.ViewState;
            MenuOperationalHazard.MenuList = toolbarsub.Show();

            BindCompany();
            BindMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CommentsLink()
    {
        lnkcomment.Visible = true;
        lnkaspectcomment.Visible = true;
        lnkhealthcomments.Visible = true;
        lnkEnvironmental.Visible = true;
        lnkEconomic.Visible = true;
        lnkUndesireable.Visible = true;

        gvRAOperationalHazard.Columns[4].Visible = false;
        gvHealthSafetyRisk.Columns[10].Visible = false;
        gvEnvironmentalRisk.Columns[10].Visible = false;
        gvEconomicRisk.Columns[9].Visible = false;
        gvevent.Columns[7].Visible = false;
        gvForms.ShowFooter = false;
        gvworkpermit.ShowFooter = false;
        gvEPSS.ShowFooter = false;
        gvprocedure.ShowFooter = false;
        MenuOperationalHazard.Visible = false;
        btnShowComponents.Visible = false;
        lnkComponentAdd.Visible = false;
        gvForms.Columns[1].Visible = false;
        gvworkpermit.Columns[2].Visible = false;
        gvEPSS.Columns[1].Visible = false;
        gvprocedure.Columns[1].Visible = false;

        lnkcomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=1&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");
        lnkaspectcomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=2&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");
        lnkhealthcomments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=3&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");
        lnkEnvironmental.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=4&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");
        lnkEconomic.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=5&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");
        lnkUndesireable.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=1&SECTIONID=6&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); ");

        ddlJob.CssClass = "input";
        ddlJob.ShowDropDownOnTextboxClick = false;
        ddlJob.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlJob.Items)
        {
            item.Enabled = false;
        }
        txtJob.CssClass = "input";
        txtJob.ReadOnly = true;
        txtpreparedby.ReadOnly = true;
        ucCreatedDate.ReadOnly = true;
        txtApprovedby.ReadOnly = true;
        ddlPeopleInvolved.ShowDropDownOnTextboxClick = false;
        ddlPeopleInvolved.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlPeopleInvolved.Items)
        {
            item.Enabled = false;
        }
        txtIssuedBy.ReadOnly = true;
        ucIssuedDate.ReadOnly = true;
        ddlWorkFrequency.ShowDropDownOnTextboxClick = false;
        ddlWorkFrequency.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlWorkFrequency.Items)
        {
            item.Enabled = false;
        }
        txtVessel.ReadOnly = true;
        ddlWorkDuration.ShowDropDownOnTextboxClick = false;
        ddlWorkDuration.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlWorkDuration.Items)
        {
            item.Enabled = false;
        }
        txtStatus.ReadOnly = false;
        ddlsupervisionlist.ShowDropDownOnTextboxClick = false;
        ddlsupervisionlist.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlsupervisionlist.Items)
        {
            item.Enabled = false;
        }
        divpersonsinvolved.Visible = false;
        divcbl.Visible = false;
        Chkpersonsinvolved.Visible = false;
        cblReason.Visible = false;
        txtComponentCode.Visible = false;
        txtComponentName.Visible = false;
        txtpreparedby.CssClass = "input"; 
        ucCreatedDate.CssClass = "input"; 
        txtApprovedby.CssClass = "input"; 
        ddlPeopleInvolved.CssClass = "input"; 
        txtIssuedBy.CssClass = "input"; 
        ucIssuedDate.CssClass = "input"; 
        ddlWorkFrequency.CssClass = "input"; 
        txtVessel.CssClass = "input"; 
        ucCompany.CssClass = "input";
        ddlWorkDuration.CssClass = "input"; 
        txtStatus.CssClass = "input"; 
        ddlsupervisionlist.CssClass = "input"; 
        txtHazid.CssClass = "input";
        txtHazid.ReadOnly = true;
        txtRevNO.ReadOnly = true;
        txtpersonsinvolved.Visible = true;
        txtcbl.Visible = true;
        txtVessel.Enabled = true;
        txtVessel.ReadOnly = true;
        txtStatus.Enabled = true;
        txtStatus.ReadOnly = true;
        ucIssuedDate.Enabled = true;
        ucIssuedDate.DatePicker = false;
        ucCreatedDate.Enabled = true;
        ucCreatedDate.DatePicker = true;
        ucCompany.ShowDropDownOnTextboxClick = false;
        ucCompany.ShowToggleImage = false;
        ucCompany.Enabled = true;
        ucIssuedDate.DateIconVisible = false;
        ucCreatedDate.DateIconVisible = false;
    }

    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();


        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) != null)
        {

            if (!(General.GetNullableString(ViewState["STATUS"].ToString()) == "3" || General.GetNullableString(ViewState["STATUS"].ToString()) == "5" || General.GetNullableString(ViewState["STATUS"].ToString()) == "7")) //if status is Draft or Approved                
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            if (General.GetNullableString(ViewState["STATUS"].ToString()) == "8" || General.GetNullableString(ViewState["STATUS"].ToString()) == "6")
            {
                toolbar.AddButton("Request Approval", "REQUESTAPPROVAL", ToolBarDirection.Right);
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (General.GetNullableString(ViewState["STATUS"].ToString()) == "4" || General.GetNullableString(ViewState["STATUS"].ToString()) == "1")
                {
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                }
            }
        }

        if (ViewState["ProcessRAYN"].ToString().Equals("0"))
        {
            if (ViewState["DMSYN"].ToString() != "1") //if status is Draft or Approved
            {
                toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
            }
        }
        if (ViewState["ProcessRAYN"].ToString().Equals("1"))
        {
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        }

        MenuJobHazardGeneral.AccessRights = this.ViewState;
        MenuJobHazardGeneral.MenuList = toolbar.Show();

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionOperationalHazardPickList.aspx?JhaId=" + ViewState["JOBHAZARDID"].ToString() + "')", "Map from Aspects Register", "<i class=\"fas fa-tasks\"></i>", "ADD");
        toolbarsub.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionGobalRAOperationalHazard.aspx?RATYPE=1&RAId=" + ViewState["JOBHAZARDID"].ToString() + "')", "Add New Aspect", "<i class=\"fa fa-plus-circle\"></i>", "MADD");
        MenuOperationalHazard.AccessRights = this.ViewState;
        MenuOperationalHazard.MenuList = toolbarsub.Show();

        btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
    }

    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(txtComponentId.Text) != null)
            {
                if (ViewState["JOBHAZARDID"].Equals(""))
                {
                    ucError.ErrorMessage = "Add the Job Hazard first and then add Hazard.";
                    return;
                }
                PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAComponents(new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtComponentId.Text), 1);
                ucStatus.Text = "Component added.";
                txtComponentId.Text = "";
                txtComponentCode.Text = "";
                txtComponentName.Text = "";
                BindComponents();
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindImpactComponents()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAImpactEquipmentList(General.GetNullableGuid(ViewState["IMPACTID"].ToString()), 1);
        chkEquipment.DataSource = ds.Tables[0];
        chkEquipment.DataBind();

        chkEvent.DataSource = ds.Tables[2];
        chkEvent.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            General.RadBindCheckBoxList(chkEquipment, ds.Tables[0].Rows[0]["FLDMAPPEDCOMPONENTID"].ToString());
        }

        if (ds.Tables[1].Rows.Count > 0)
        {
            if (ds.Tables[1].Rows[0]["FLDHAZARDTYPE"].ToString().Equals("1"))
                trPPE.Visible = true;
            else
                trPPE.Visible = false;

            cblRecomendedPPE.ClearSelection();
            General.BindCheckBoxList(cblRecomendedPPE, ds.Tables[1].Rows[0]["FLDPPELIST"].ToString());
        }

        if (ds.Tables[2].Rows.Count > 0)
        {
                General.RadBindCheckBoxList(chkEvent, ds.Tables[2].Rows[0]["FLDUNDESIRABLEEVENT"].ToString().ToLower());
        }
    }
    
    protected void BindEventComponents()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAEventEquipmentList(General.GetNullableGuid(ViewState["EVENTID"].ToString()), 1);
        chkEquipment.DataSource = ds.Tables[0];
        chkEquipment.DataBind();


        if (ds.Tables[0].Rows.Count > 0)
        {
            General.RadBindCheckBoxList(chkEquipment, ds.Tables[0].Rows[0]["FLDMAPPEDCOMPONENTID"].ToString());
        }

        trPPE.Visible = true;
        if (ds.Tables[1].Rows.Count > 0)
        {
            cblRecomendedPPE.ClearSelection();
            General.BindCheckBoxList(cblRecomendedPPE, ds.Tables[1].Rows[0]["FLDPPELIST"].ToString());
        }

    }
    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentList(ViewState["JOBHAZARDID"].ToString() == "" ? null : General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 1);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                if ((ViewState["STATUS"].ToString()) == "3" || General.GetNullableString(ViewState["STATUS"].ToString()) == "5")
                {
                    cb.Enabled = false;
                }
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(com_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDCOMPONENTNAME"].ToString();
                hl.ID = "hlink2" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                number = number + 1;
            }
            divComponents.Visible = true;
        }
        else
            divComponents.Visible = false;
    }

    void com_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentDelete(new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(c.ID), 1);

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindComponents();
            EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
        }
    }

    protected void EditJobHazard(Guid jobhazardid)
    {
        try
        {
            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRiskAssessmentJobHazard(jobhazardid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtHazid.Text = ds.Tables[0].Rows[0]["FLDHAZARDNUMBER"].ToString();
                txtRevNO.Text = ds.Tables[0].Rows[0]["FLDREVISIONNO"].ToString();
                txtcbl.Text = ds.Tables[0].Rows[0]["FLDCONDTNFORADDITIONALRANAMETEXT"].ToString();
                txtcbl.Text = txtcbl.Text.Replace("</br>", Environment.NewLine);
                txtpersonsinvolved.Text = ds.Tables[0].Rows[0]["FLDPERSONINVOLVEDTEXT"].ToString();
                txtpersonsinvolved.Text = txtpersonsinvolved.Text.Replace("</br>", Environment.NewLine);
                txtpreparedby.Text = ds.Tables[0].Rows[0]["FLDPREPAREDBY"].ToString();
                ucCreatedDate.Text = ds.Tables[0].Rows[0]["FLDPREPAREDDATE"].ToString();
                txtApprovedby.Text = ds.Tables[0].Rows[0]["FLDAPPROVEDBY"].ToString();
                ucApprovedDate.Text = ds.Tables[0].Rows[0]["FLDAPPROVEDDATE"].ToString();
                txtIssuedBy.Text = ds.Tables[0].Rows[0]["FLDISSUEDBY"].ToString();
                ucIssuedDate.Text = ds.Tables[0].Rows[0]["FLDISSUEDDATE"].ToString();
                txtJob.Text = ds.Tables[0].Rows[0]["FLDJOB"].ToString();
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                txtStatus.Text = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();

                lblLevelofRiskHealth.Text = ds.Tables[0].Rows[0]["FLDHSLR"].ToString();
                lblLevelofRiskEnv.Text = ds.Tables[0].Rows[0]["FLDENVLR"].ToString();
                lblLevelofRiskEconomic.Text = ds.Tables[0].Rows[0]["FLDECOLR"].ToString();
                lblLevelofRiskWorst.Text = ds.Tables[0].Rows[0]["FLDWCLR"].ToString();
                lblhscontrols.Text = ds.Tables[0].Rows[0]["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblencontrols.Text = ds.Tables[0].Rows[0]["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lbleccontrols.Text = ds.Tables[0].Rows[0]["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblwscontrols.Text = ds.Tables[0].Rows[0]["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblreshsrisk.Text = ds.Tables[0].Rows[0]["FLDHSRR"].ToString();
                lblresenrisk.Text = ds.Tables[0].Rows[0]["FLDENVRR"].ToString();
                lblresecorisk.Text = ds.Tables[0].Rows[0]["FLDECORR"].ToString();
                lblreswsrisk.Text = ds.Tables[0].Rows[0]["FLDWCRR"].ToString();
                lblHealthDescription.Text = ds.Tables[0].Rows[0]["FLDHSRISKLEVELTEXT"].ToString();
                lblEnvDescription.Text = ds.Tables[0].Rows[0]["FLDENVRISKLEVELTEXT"].ToString();
                lblEconomicDescription.Text = ds.Tables[0].Rows[0]["FLDECORISKLEVELTEXT"].ToString();
                lblWorstDescription.Text = ds.Tables[0].Rows[0]["FLDWSRISKLEVELTEXT"].ToString();
                lblimpacthealth.Text = ds.Tables[0].Rows[0]["FLDHSSCORE"].ToString();
                lblimpacteco.Text = ds.Tables[0].Rows[0]["FLDECOSCORE"].ToString();
                lblimpactenv.Text = ds.Tables[0].Rows[0]["FLDENVSCORE"].ToString();
                lblimpactws.Text = ds.Tables[0].Rows[0]["FLDWSSCORE"].ToString();

                lblPOOhealth.Text = ds.Tables[0].Rows[0]["FLDHSPOO"].ToString();
                lblPOOeco.Text = ds.Tables[0].Rows[0]["FLDECOPOO"].ToString();
                lblPOOenv.Text = ds.Tables[0].Rows[0]["FLDENVPOO"].ToString();
                lblPOOws.Text = ds.Tables[0].Rows[0]["FLDWSPOO"].ToString();

                lbllohhealth.Text = ds.Tables[0].Rows[0]["FLDHSLOH"].ToString();
                lblloheco.Text = ds.Tables[0].Rows[0]["FLDECOLOH"].ToString();
                lbllohenv.Text = ds.Tables[0].Rows[0]["FLDENVLOH"].ToString();
                lbllohws.Text = ds.Tables[0].Rows[0]["FLDWSLOH"].ToString();

                lblControlshealth.Text = ds.Tables[0].Rows[0]["FLDHSLOC"].ToString();
                lblControlseco.Text = ds.Tables[0].Rows[0]["FLDECOLOC"].ToString();
                lblControlsenv.Text = ds.Tables[0].Rows[0]["FLDENVLOC"].ToString();
                lblControlsws.Text = ds.Tables[0].Rows[0]["FLDWSLOC"].ToString();

                decimal minscore = 0, maxscore = 0;

                DataRow dr = ds.Tables[0].Rows[0];

                if (!string.IsNullOrEmpty(dr["FLDMINSCORE"].ToString()))
                    minscore = decimal.Parse(dr["FLDMINSCORE"].ToString());

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDMAXSCORE"].ToString()))
                    maxscore = decimal.Parse(dr["FLDMAXSCORE"].ToString());

                lblLevelofRiskHealth.Text = ds.Tables[0].Rows[0]["FLDHSLR"].ToString();
                              

                lblreshsrisk.Text = ds.Tables[0].Rows[0]["FLDHSRR"].ToString();

                if (!string.IsNullOrEmpty(lblreshsrisk.Text))
                {
                    if (decimal.Parse(lblreshsrisk.Text) <= minscore)
                        tdreshsrisk.BgColor = "Lime";
                    else if (decimal.Parse(lblreshsrisk.Text) > minscore && decimal.Parse(lblreshsrisk.Text) <= maxscore)
                        tdreshsrisk.BgColor = "Yellow";
                    else if (decimal.Parse(lblreshsrisk.Text) > maxscore)
                        tdreshsrisk.BgColor = "Red";
                }
                else
                    tdreshsrisk.BgColor = "White";

                lblresenrisk.Text = dr["FLDENVRR"].ToString();

                if (!string.IsNullOrEmpty(lblresenrisk.Text))
                {
                    if (decimal.Parse(lblresenrisk.Text) <= minscore)
                        tdresenrisk.BgColor = "Lime";
                    else if (decimal.Parse(lblresenrisk.Text) > minscore && decimal.Parse(lblresenrisk.Text) <= maxscore)
                        tdresenrisk.BgColor = "Yellow";
                    else if (decimal.Parse(lblresenrisk.Text) > maxscore)
                        tdresenrisk.BgColor = "Red";
                }
                else
                    tdresenrisk.BgColor = "White";

                lblresecorisk.Text = dr["FLDECORR"].ToString();

                if (!string.IsNullOrEmpty(lblresecorisk.Text))
                {
                    if (decimal.Parse(lblresecorisk.Text) <= minscore)
                        tdresecorisk.BgColor = "Lime";
                    else if (decimal.Parse(lblresecorisk.Text) > minscore && decimal.Parse(lblresecorisk.Text) <= maxscore)
                        tdresecorisk.BgColor = "Yellow";
                    else if (decimal.Parse(lblresecorisk.Text) > maxscore)
                        tdresecorisk.BgColor = "Red";
                }
                else
                    tdresecorisk.BgColor = "White";

                lblreswsrisk.Text = dr["FLDWCRR"].ToString();

                if (!string.IsNullOrEmpty(lblreswsrisk.Text))
                {
                    if (decimal.Parse(lblreswsrisk.Text) <= minscore)
                        tdreswsrisk.BgColor = "Lime";
                    else if (decimal.Parse(lblreswsrisk.Text) > minscore && decimal.Parse(lblreswsrisk.Text) <= maxscore)
                        tdreswsrisk.BgColor = "Yellow";
                    else if (decimal.Parse(lblreswsrisk.Text) > maxscore)
                        tdreswsrisk.BgColor = "Red";
                }
                else
                    tdreswsrisk.BgColor = "White";


                if (ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString() != "")
                {
                    BindJob();
                    ddlJob.SelectedValue = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString()))
                {
                    ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                    ucCompany.Enabled = false;
                    ViewState["COMPANYID"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                }
                else
                    ucCompany.Enabled = true;

                ViewState["STATUS"] = ds.Tables[0].Rows[0]["FLDSTATUSID"].ToString();

                if ((ds.Tables[0].Rows[0]["FLDSTATUSID"].ToString().Equals("3")) || (ds.Tables[0].Rows[0]["FLDSTATUSID"].ToString().Equals("5")))
                {
                    CommentsLink();
                }

                if (ds.Tables[0].Rows[0]["FLDNUMBEROFPEOPLE"].ToString() != "")
                {
                    ddlPeopleInvolved.SelectedValue = ds.Tables[0].Rows[0]["FLDNUMBEROFPEOPLE"].ToString();
                }

                if (ds.Tables[0].Rows[0]["FLDDURATION"].ToString() != "")
                {
                    ddlWorkDuration.SelectedValue = ds.Tables[0].Rows[0]["FLDDURATION"].ToString();
                }

                if (ds.Tables[0].Rows[0]["FLDSUPERVISIONID"].ToString() != "")
                {
                    ddlsupervisionlist.SelectedValue = ds.Tables[0].Rows[0]["FLDSUPERVISIONID"].ToString();
                }

                General.BindCheckBoxList(Chkpersonsinvolved, ds.Tables[0].Rows[0]["FLDPERSONINVOLVEDLIST"].ToString());


                if (ds.Tables[0].Rows[0]["FLDFREQUENCY"].ToString() != "")
                {
                    ddlWorkFrequency.SelectedValue = ds.Tables[0].Rows[0]["FLDFREQUENCY"].ToString();
                }
                General.BindCheckBoxList(cblReason, ds.Tables[0].Rows[0]["FLDREASON"].ToString());

                string reason = General.ReadCheckBoxList(cblReason);

                if (reason.Contains("100"))
                {

                    txtOtherReason.CssClass = "input";
                    txtOtherReason.ReadOnly = false;
                    txtOtherReason.Text = ds.Tables[0].Rows[0]["FLDOTHERREASON"].ToString();
                }
                else
                {
                    txtOtherReason.Text = "";
                    txtOtherReason.ReadOnly = true;
                    txtOtherReason.CssClass = "readonlytextbox";
                }

                BindMenu();
                gvPPE.Rebind();
                gvPPE2.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuJobHazardGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidJobHazard(ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                string potentialconsequence = "";
                string reasonforassessment = General.ReadCheckBoxList(cblReason);

                if (ViewState["JOBHAZARDID"].ToString() == "")
                {
                    Guid? jobhazardid = General.GetNullableGuid(null);
                    PhoenixInspectionRiskAssessmentJobHazardExtn.InsertRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        int.Parse(ddlJob.SelectedValue),
                                                        General.GetNullableInteger(ddlJob.SelectedValue),
                                                        potentialconsequence,
                                                        null, txtJob.Text, null,
                                                        ref jobhazardid,
                                                        General.GetNullableString(null),
                                                        General.GetNullableString(null),
                                                        General.GetNullableInteger(null),
                                                        General.GetNullableInteger(ucCompany.SelectedCompany),
                                                        General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                        General.GetNullableGuid(null),
                                                        General.GetNullableInteger(ddlPeopleInvolved.SelectedValue),
                                                        General.GetNullableInteger(ddlWorkDuration.SelectedValue),
                                                        General.GetNullableInteger(ddlWorkFrequency.SelectedValue),
                                                        General.GetNullableString(reasonforassessment),
                                                        General.GetNullableString(txtOtherReason.Text.Trim()),
                                                        General.GetNullableGuid(null),
                                                        General.GetNullableString(General.ReadCheckBoxList(Chkpersonsinvolved)),
                                                        General.GetNullableInteger(ddlsupervisionlist.SelectedValue)
                                                        );

                    ViewState["JOBHAZARDID"] = jobhazardid;
                    ucStatus.Text = "Job Hazard updated. ";
                    Filter.CurrentSelectedJHA = jobhazardid.ToString();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }
                else
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(ViewState["JOBHAZARDID"].ToString()),
                                                    int.Parse(ddlJob.SelectedValue),
                                                    General.GetNullableInteger(ddlJob.SelectedValue),
                                                    potentialconsequence,
                                                    null, txtJob.Text, null,
                                                    General.GetNullableString(null),
                                                    General.GetNullableString(null),
                                                    General.GetNullableInteger(null),
                                                    General.GetNullableInteger(ucCompany.SelectedCompany),
                                                    General.GetNullableGuid(null),
                                                    General.GetNullableInteger(ddlPeopleInvolved.SelectedValue),
                                                    General.GetNullableInteger(ddlWorkDuration.SelectedValue),
                                                    General.GetNullableInteger(ddlWorkFrequency.SelectedValue),
                                                    General.GetNullableString(reasonforassessment),
                                                    General.GetNullableString(txtOtherReason.Text.Trim()),
                                                    General.GetNullableGuid(null),
                                                    General.GetNullableString(General.ReadCheckBoxList(Chkpersonsinvolved)),
                                                    General.GetNullableInteger(ddlsupervisionlist.SelectedValue)
                                                    );
                    ucStatus.Text = "Job Hazard updated. ";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }

            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (!IsValidJobHazard(ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["STATUS"].ToString() == "4")
                {
                    string script = "parent.Openpopup('Bank','','../Inspection/InspectionVesselRAJHAApprovalExtn.aspx?RATEMPLATEID=" + ViewState["JOBHAZARDID"].ToString() + "&TYPE=4','medium');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }

                else if (ViewState["STATUS"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRiskAssessmentJobHazardApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["JOBHAZARDID"].ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 3);
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Job Hazard Approved. ";
                }
            }
            if (CommandName.ToUpper().Equals("REQUESTAPPROVAL"))
            {
                if (!IsValidJobHazard(ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRiskAssessmentJobHazardApprovalRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["JOBHAZARDID"].ToString()));

                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                ucStatus.Text = "Job Hazard Approval Requested. .";
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["HSEQADashboardYN"].ToString().Equals("1"))
                {
                    Response.Redirect("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx");
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisListExtn.aspx", false);
                }
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["PROCESSDASHBOARDYN"].ToString().Equals("1"))
                {
                    Response.Redirect("../Inspection/InspectionRAProcessExtn.aspx?processid=" + ViewState["processid"].ToString() + "&HSEQADashboardYN=" + ViewState["PROCESSDASHBOARDYN"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionRAProcessExtn.aspx?processid=" + ViewState["processid"].ToString(), false);
                }
            }
            gvevent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindJob()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();

        if (dt.Rows.Count > 0)
        {
            ddlJob.Items.Clear();
            ddlJob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlJob.DataSource = dt;
            ddlJob.DataBind();
        }
    }
    protected void BindJob_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindJob();
    }

    protected void gvHealthSafetyRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }

                LinkButton mapping = (LinkButton)e.Item.FindControl("cmdEquipment");
                if (mapping != null) mapping.Visible = SessionUtil.CanAccess(this.ViewState, mapping.CommandName);


            }

            HSGridDecorator.MergeRows(gvHealthSafetyRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHealthSafetyRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentJobHazardExtn.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 1);
            gvHealthSafetyRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHealthSafetyRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvHealthSafetyRisk.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                string Healthid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;

                if (e.CommandName.ToUpper().Equals("HEALTHSAFETYDELETE"))
                {

                    PhoenixInspectionRiskAssessmentJobHazardExtn.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Healthid));

                    gvHealthSafetyRisk.Rebind();
                    gvevent.Rebind();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "HMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        var args = e.Argument;
        var array = args.Split(',');
        var id = array[0];
        var cmd = array[1];

        if (cmd.ToUpper() == "EDIT")
        {
            tbl2.Visible = false;
            tbl1.Visible = true;
            BindImpactComponents();
        }
        if (cmd.ToUpper() == "EVENTEDIT")
        {
            tbl2.Visible = true;
            tbl1.Visible = false;

            txtProcedure.Text = "";
            txtProcedureid.Text = "";
            txtEquipment.Text = "";
            txtEquipmentCode.Text = "";
            txtEquipmentid.Text = "";
            lnkEquipmentList.Attributes.Add("onclick", "return showPickList('spnPickListEquipment', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
            lnkProcedureList.Attributes.Add("onclick", "return showPickList('spnPickListProcedure', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            BindEquipment();
            BindProcedure();
            BindWorstCase();
            BindWorstcaseEvents();
        }
    }

    protected void gvEconomicRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }

            }
            ECOGridDecorator.MergeRows(gvEconomicRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEconomicRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentJobHazardExtn.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 4);
            gvEconomicRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEconomicRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvEconomicRisk.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("ECONOMICDELETE"))
                {

                    string Healthid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;
                    PhoenixInspectionRiskAssessmentJobHazardExtn.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(Healthid));

                    gvEconomicRisk.Rebind();
                    gvevent.Rebind();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "ECOMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                if (e.Item is GridFooterItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }

                }
            }
            ENVGridDecorator.MergeRows(gvEnvironmentalRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentJobHazardExtn.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 2);
            gvEnvironmentalRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvEnvironmentalRisk.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }
                if (e.CommandName.ToUpper().Equals("ENVIRONMENTALDELETE"))
                {

                    string Healthid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;
                    PhoenixInspectionRiskAssessmentJobHazardExtn.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(Healthid));

                    gvEnvironmentalRisk.Rebind();
                    gvevent.Rebind();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "ENVMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDHEALTHHAZARDID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHazard(string aspectid, string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Job Hazard first and then add.";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) != null)
        {

            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Please select the Hazard Type and then add Impact.";

            if (General.GetNullableGuid(aspectid) == null)
                ucError.ErrorMessage = "Aspects is required.";

            if (type != null && type.ToString() == "2")
            {
                if (General.GetNullableInteger(impacttype) == null)
                    ucError.ErrorMessage = "Impact Type is required.";
            }

            if (General.GetNullableGuid(subhazardid) == null)
                ucError.ErrorMessage = "Impact is required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidJobHazard(string jobid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableString(jobid) == null)
            ucError.ErrorMessage = "Process is required.";

        if (General.GetNullableString(txtJob.Text) == null)
            ucError.ErrorMessage = "Job is required.";

        if (General.GetNullableInteger(ddlPeopleInvolved.SelectedValue) == null)
            ucError.ErrorMessage = "No of People who may be affected is required.";

        if (General.GetNullableInteger(ddlWorkDuration.SelectedValue) == null)
            ucError.ErrorMessage = "Duration is required.";

        if (General.GetNullableInteger(ddlWorkFrequency.SelectedValue) == null)
            ucError.ErrorMessage = "Frequency is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(Chkpersonsinvolved)) == null)
            ucError.ErrorMessage = "Persons carrying out job is required.";

        return (!ucError.IsError);

    }
    private bool IsValidRAHazard(string Hazard, int type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //GridView _gridView = gvRAHazard;

        if (ViewState["JOBHAZARDID"].Equals(""))
            ucError.ErrorMessage = "Add the Job Hazard first and then add Hazard.";
        if (type == 1)
        {
            if (Hazard.Trim().Equals(""))
                ucError.ErrorMessage = " Operational Hazard is required.";
        }
        if (type == 2)
        {
            if (Hazard.Trim().Equals(""))
                ucError.ErrorMessage = " Controls/Precautions  is required.";
        }

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
        gvRAOperationalHazard.Rebind();
        gvHealthSafetyRisk.Rebind();
        gvEnvironmentalRisk.Rebind();
        gvEconomicRisk.Rebind();
        gvevent.Rebind();
    }
    protected void BindCompany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcess.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }



    protected void OtherDetailClick(object sender, EventArgs e)
    {
        string reason = General.ReadCheckBoxList(cblReason);

        if (reason.Contains("100"))
        {

            txtOtherReason.CssClass = "input";
            txtOtherReason.ReadOnly = false;
        }
        else
        {
            txtOtherReason.Text = "";
            txtOtherReason.ReadOnly = true;
            txtOtherReason.CssClass = "readonlytextbox";
        }
    }

    protected void gvevent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentworstcase(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 1);
            gvevent.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvevent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                RadLabel lblWorstcaseid = (RadLabel)e.Item.FindControl("lblWorstcaseid");
                HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblProcedures");
                if (lblWorstcaseid != null)
                {
                    DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.UndesirableeventFormposterList(General.GetNullableGuid(lblWorstcaseid.Text), General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()));
                    int number = 1;
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        tblForms.Rows.Clear();
                        foreach (DataRow dr in dss.Tables[0].Rows)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = dr["FLDNAME"].ToString();
                            hl.ID = "hlink" + number.ToString();
                            hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                            string type = dr["FLDTYPE"].ToString();

                            if (type == "2")
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == "3")
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == "5")
                            {
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "');return false;");
                            }
                            else if (type == "6")
                            {
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?formid=" + dr["FLDFORMPOSTERID"].ToString();
                            }

                            HtmlTableRow tr = new HtmlTableRow();
                            HtmlTableCell tc = new HtmlTableCell();
                            tr.Cells.Add(tc);
                            tc = new HtmlTableCell();
                            tc.Controls.Add(hl);
                            tr.Cells.Add(tc);
                            tblForms.Rows.Add(tr);
                            tc = new HtmlTableCell();
                            tc.InnerHtml = "<br/>";
                            tr.Cells.Add(tc);
                            tblForms.Rows.Add(tr);
                            number = number + 1;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvevent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvevent.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("UDELETE"))
                {

                    string WorstCaseid = ((RadLabel)editableItem.FindControl("lblWorstCaseid")).Text;
                    PhoenixInspectionOperationalRiskControls.DeleteRiskAssessmentWorstCase(new Guid(WorstCaseid));

                    gvevent.Rebind();
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "EMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    ViewState["EVENTID"] = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EVENTEDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "EVENT";
                }

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidWorstCase(string Worstcase)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) != null)
        {
            if (General.GetNullableInteger(Worstcase) == null)
                ucError.ErrorMessage = "Worst Case is required.";
        }

        return (!ucError.IsError);

    }
    protected void gvRAOperationalHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentJobHazardExtn.ListOperationalHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()));
            gvRAOperationalHazard.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRAOperationalHazard.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                string OperationalId = ((RadLabel)editableItem.FindControl("lblOperationalId")).Text;
                if (e.CommandName.ToUpper().Equals("ASPECTDELETE"))
                {

                    PhoenixInspectionRiskAssessmentJobHazardExtn.DeleteOperationalHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(OperationalId));

                    gvRAOperationalHazard.Rebind();
                    gvHealthSafetyRisk.Rebind();
                    gvEconomicRisk.Rebind();
                    gvEnvironmentalRisk.Rebind();
                    gvevent.Rebind();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Hazard  Deleted.";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                RadLabel lblOperationalHazard = (RadLabel)e.Item.FindControl("lblOperationalHazard");
                RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                if (cmdEdit != null)
                {
                    cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionOperationalRiskHazardMapping.aspx?ratypeid=1&Operationalhazardid=" + lblOperationalId.Text + "&RAID=" + ViewState["JOBHAZARDID"].ToString() + "'); return true;");
                }

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton cmdAspectEdit = (LinkButton)e.Item.FindControl("cmdAspectEdit");
                if (cmdAspectEdit != null)
                {
                    cmdAspectEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAspectEdit.CommandName);
                    cmdAspectEdit.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAOperationalAspectsEdit.aspx?RATYPE=1&Operationalhazardid=" + lblOperationalId.Text + "');return true;");
                }

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                if (ViewState["JOBHAZARDID"].Equals(""))
                {
                    ucError.ErrorMessage = "Add the Job Hazard first and then add Hazard.";
                    return;
                }

                if (ViewState["RequestFrom"].ToString() == "EVENT")
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEventComponents(new Guid(ViewState["EVENTID"].ToString())
                    , General.ReadCheckBoxList(cbEventEquipment)
                    , General.ReadCheckBoxList(cbProcedure)
                    , General.ReadCheckBoxList(cbEventPPE)
                    , General.GetNullableInteger(ddlWorstCase.SelectedValue), 1);

                    ucStatus.Text = "Updated Successfully";
                    BindWorstcaseEvents();
                }
                else
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAImpactComponents(new Guid(ViewState["IMPACTID"].ToString()), General.RadCheckBoxList(chkEquipment), General.ReadCheckBoxList(cblRecomendedPPE), General.RadCheckBoxList(chkEvent), 1);
                    ucStatus.Text = "Component added.";
                    BindImpactComponents();
                }

                gvHealthSafetyRisk.Rebind();
                gvEnvironmentalRisk.Rebind();
                gvEconomicRisk.Rebind();
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                return;
            }
        }
    }

    public class HSGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                     previousRow.Cells[10].RowSpan + 1;
                    previousRow.Cells[10].Visible = false;
                }
            }
        }
    }
    public class ENVGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
                                     previousRow.Cells[11].RowSpan + 1;
                    previousRow.Cells[11].Visible = false;
                }
            }
        }
    }
    public class ECOGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                     previousRow.Cells[10].RowSpan + 1;
                    previousRow.Cells[10].Visible = false;
                }
            }
        }
    }

    protected void BindRecomendedPPE()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(null,
                5,
                null, null,
                1,
                500,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblRecomendedPPE.DataSource = ds.Tables[0];
                cblRecomendedPPE.DataBind();

                cbEventPPE.DataSource = ds.Tables[0];
                cbEventPPE.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindWorstcaseEvents()
    {
        try
        {
            if (ViewState["EVENTID"] != null && ViewState["EVENTID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionOperationalRiskControls.EditRiskAssessmentworstcase(General.GetNullableGuid(ViewState["EVENTID"].ToString()));
                cbEventEquipment.ClearSelection();
                cbEventPPE.ClearSelection();
                cbProcedure.ClearSelection();
                ddlWorstCase.ClearSelection();
                General.BindCheckBoxList(cbProcedure, ds.Tables[0].Rows[0]["FLDPROCEDUREID"].ToString().ToLower());
                General.BindCheckBoxList(cbEventPPE, ds.Tables[0].Rows[0]["FLDPPE"].ToString());
                General.BindCheckBoxList(cbEventEquipment, ds.Tables[0].Rows[0]["FLDCOMPONENTID"].ToString());
                ddlWorstCase.SelectedValue = ds.Tables[0].Rows[0]["FLDHAZARDID"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void BindProcedure()
    {
        try
        {
            DataSet dss = PhoenixInspectionOperationalRiskControls.UndesirableProcedureList(General.GetNullableGuid(ViewState["EVENTID"].ToString()));
            cbProcedure.DataSource = dss.Tables[0];
            cbProcedure.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindEquipment()
    {
        try
        {
            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAEventEquipmentList(General.GetNullableGuid(ViewState["EVENTID"].ToString()), 1);
            cbEventEquipment.DataSource = ds.Tables[0];
            cbEventEquipment.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindWorstCase()
    {
        try
        {
            ddlWorstCase.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(3, null);
            ddlWorstCase.DataTextField = "FLDNAME";
            ddlWorstCase.DataValueField = "FLDHAZARDID";
            ddlWorstCase.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cbProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionOperationalRiskControls.UndesirableeventProcedureDelete(new Guid(ViewState["EVENTID"].ToString()), General.ReadCheckBoxList(cbProcedure));
            ucStatus.Text = "deleted.";
            BindProcedure();
            BindWorstcaseEvents();
            gvevent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkEquipmentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(txtEquipmentid.Text) != null)
            {
                //PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAComponents(new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtEquipmentid.Text), 1);
                //PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateWorstCaseComponents(new Guid(ViewState["EVENTID"].ToString()), General.GetNullableString(txtEquipmentid.Text));
                //ucStatus.Text = "Component added.";
                //txtEquipmentid.Text = "";
                //txtEquipmentCode.Text = "";
                //txtEquipment.Text = "";
                //BindEquipment();
                //BindWorstcaseEvents();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkProcedureAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(txtProcedureid.Text) != null)
            {
                PhoenixInspectionOperationalRiskControls.UpdateUndesirableProcedure(new Guid(ViewState["EVENTID"].ToString()), General.GetNullableGuid(txtProcedureid.Text));
                ucStatus.Text = "Procedure added.";
                txtProcedureid.Text = "";
                txtProcedure.Text = "";
                BindProcedure();
                BindWorstcaseEvents();
                gvevent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void btnEventsave_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEventComponents(new Guid(ViewState["EVENTID"].ToString())
            , General.ReadCheckBoxList(cbEventEquipment)
            , General.ReadCheckBoxList(cbProcedure)
            , General.ReadCheckBoxList(cbEventPPE)
            , General.GetNullableInteger(ddlWorstCase.SelectedValue), 1);

            ucStatus.Text = "Updated Successfully";
            BindWorstcaseEvents();

            EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvevent.Rebind();
            string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            return;
        }
    }


    protected void gvForms_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterList(ViewState["JOBHAZARDID"] == null ? null : General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), General.GetNullableInteger("0"));
        gvForms.DataSource = dss;
    }

    protected void gvForms_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text == "5") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "6") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments");
            if (btnShowDocuments != null)
            { 
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvprocedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterList(ViewState["JOBHAZARDID"] == null ? null : General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), General.GetNullableInteger("1"));
        gvprocedure.DataSource = dss;
        
    }

    protected void gvprocedure_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblFormId != null)
            {
                if (lbltype.Text == "2")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + lblFormId.Text + "');return false;");
                else if (lbltype.Text == "3")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblFormId.Text + "');return false;");

            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments1");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument1', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvPPE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JOBHAZARDID"] != null) && (ViewState["JOBHAZARDID"].ToString() != string.Empty))
        {
            DataSet DS = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAPPEWithMoreIcons(new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvPPE.DataSource = DS.Tables[0];
            gvPPE2.DataSource = DS.Tables[1];
        }
    }

    protected void gvPPE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvworkpermit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JOBHAZARDID"] != null) && (ViewState["JOBHAZARDID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAWorkPermitWithIcons(new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvworkpermit.DataSource = DT;
        }
    }

    protected void gvworkpermit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text != "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments5");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument5', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvEPSS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JOBHAZARDID"] != null) && (ViewState["JOBHAZARDID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAEPPSWithIcons(new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvEPSS.DataSource = DT;
            
        }
    }

    protected void gvEPSS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text != "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments3");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument3', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvhazards_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JOBHAZARDID"] != null) && (ViewState["JOBHAZARDID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAHazardWithIcons(1,
               new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvhazards.DataSource = DT;
           
        }
    }

    protected void gvhazards_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvenvironmental_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JOBHAZARDID"] != null) && (ViewState["JOBHAZARDID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAHazardWithIcons(2,
               new Guid(ViewState["JOBHAZARDID"].ToString()));
            gvenvironmental.DataSource = DT;
            
        }
    }

    protected void gvenvironmental_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvForms_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("FORMADD"))
            {
                if (ViewState["JOBHAZARDID"] == null || ViewState["JOBHAZARDID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Forms and Checklists.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAFormsandchecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtDocumentId.Text));
                    ucStatus.Text = "Forms & Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    gvworkpermit.Rebind();
                    gvForms.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Forms & Checklists.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("FORMDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(lblFormId.Text), int.Parse("0"));

                string txt = "Forms & Checklists";

                ucStatus.Text = txt + " deleted.";
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                gvworkpermit.Rebind();
                gvForms.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvworkpermit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("WORKPERMITADD"))
            {
                if (ViewState["JOBHAZARDID"] == null || ViewState["JOBHAZARDID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Work Permit.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId5"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName5"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAFormsandchecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtDocumentId.Text));
                    ucStatus.Text = "Work Permit added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    gvworkpermit.Rebind();
                    gvForms.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Work Permit.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("WORKPERMITDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(lblFormId.Text), int.Parse("0"));

                string txt = "Work Permit";

                ucStatus.Text = txt + " deleted.";
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                gvworkpermit.Rebind();
                gvForms.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEPSS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EPSSADD"))
            {
                if (ViewState["JOBHAZARDID"] == null || ViewState["JOBHAZARDID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the EPSS.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId3"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName3"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEPSS(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "EPSS added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    gvEPSS.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the EPSS.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("EPSSDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(lblFormId.Text), int.Parse("2"));

                string txt = "EPSS";

                ucStatus.Text = txt + " deleted.";
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                gvEPSS.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprocedure_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PROCEDUREADD"))
        {
            try
            {
                if (ViewState["JOBHAZARDID"] == null || ViewState["JOBHAZARDID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Procedure.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId1"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName1"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEmergencyProcedures(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "Procedure added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    gvprocedure.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Procedure.";
                    ucError.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

        }
        if (e.CommandName.ToUpper().Equals("PROCEDUREDELETE"))
        {
            try
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(lblFormId.Text), int.Parse("1"));

                string txt = "Procedure";

                ucStatus.Text = txt + " deleted.";
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                gvprocedure.Rebind();
            }

            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void gvPPE2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }
}
