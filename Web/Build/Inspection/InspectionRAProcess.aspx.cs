using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using System.Web;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class InspectionRAProcess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            VesselConfiguration();
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            BindData();
            ViewState["RISKASSESSMENTPROCESSID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["companyid"] = "";
            ViewState["MOCRequestid"] = string.IsNullOrEmpty(Request.QueryString["MOCRequestid"]) ? "" : Request.QueryString["MOCRequestid"];
            ViewState["MOCID"] = string.IsNullOrEmpty(Request.QueryString["MOCID"]) ? "" : Request.QueryString["MOCID"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RAType"]) ? "" : Request.QueryString["RAType"];
            ViewState["mocextention"] = string.IsNullOrEmpty(Request.QueryString["mocextention"]) ? "" : Request.QueryString["mocextention"];
            ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["Vesselname"] = string.IsNullOrEmpty(Request.QueryString["Vesselname"]) ? "" : Request.QueryString["Vesselname"];

            ViewState["QUALITYCOMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["QUALITYCOMPANYID"] = nvc.Get("QMS");
                ucCompany.SelectedCompany = ViewState["QUALITYCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            BindCategory();


            if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString() != string.Empty)
                ViewState["status"] = Request.QueryString["status"].ToString();


            if (Request.QueryString["processid"] != null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["processid"].ToString();
                RiskAssessmentProcessEdit();
            }
            if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString() == "3")
            {
                lnkImportJHA.Enabled = false;
                chkImportedJHAList.Enabled = false;
                lnkFormAdd.Visible = false;
            }

            if (Filter.CurrentVesselConfiguration != null && !Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                lnkFormAdd.Visible = false;
                btnShowDocuments.Visible = false;
            }

            lnkImportJHA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionJHATemplateMapping.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"] + "');return false;");

            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString() != "3") //if status is Draft or Approved
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        if ((Request.QueryString["RAType"] != null && Request.QueryString["RAType"].Equals("1")))
        {
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        if (Request.QueryString["RevYN"] != "1")
        {
            toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
            MenuProcess.AccessRights = this.ViewState;
            MenuProcess.MenuList = toolbar.Show();
            //MenuProcess.SetTrigger(pnlProcess);
        }
       
        // BindGrid();
        BindFormPosters();
        BindComapany();

    }

    protected void BindComapany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcess.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }

    public void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindFormPosters();
    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentProcess.FormPosterList(ViewState["RISKASSESSMENTPROCESSID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), General.GetNullableInteger(rblType.SelectedValue));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                if (ViewState["status"] != null && ViewState["status"].ToString().Equals("3"))
                    cb.Enabled = false;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                //cb.Attributes.Add("onclick","");
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];                                          
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
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
            divForms.Visible = true;
        }
        else
            divForms.Visible = false;
    }

    void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentProcess.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(c.ID), int.Parse(rblType.SelectedValue));

            string txt = "";
            if (rblType.SelectedValue == "0")
            {
                txt = "Forms/Posters/Checklists";
            }
            else if (rblType.SelectedValue == "1")
            {
                txt = "Procedures";
            }
            else if (rblType.SelectedValue == "2")
            {
                txt = "Contingency/Emergency";
            }

            ucStatus.Text = txt + " deleted.";
            BindFormPosters();
        }
    }

    protected void lnkRA_Clicked(object sender, EventArgs e)
    {
        Response.Redirect("../Inspection/InspectionRAProcessMultipleList.aspx?PROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"] + "&status=" + (ViewState["status"] == null ? null : ViewState["status"].ToString()));
    }

    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
        {
            if (rblType.SelectedValue == "0")
            {
                ucError.ErrorMessage = "Please save the Template first and then try adding the Forms/Posters/Checklists.";
                ucError.Visible = true;
                return;
            }
            else if (rblType.SelectedValue == "1")
            {
                ucError.ErrorMessage = "Please save the Template first and then try adding the Procedures.";
                ucError.Visible = true;
                return;
            }
            else if (rblType.SelectedValue == "2")
            {
                ucError.ErrorMessage = "Please save the Template first and then try adding the Contingency/Emergency.";
                ucError.Visible = true;
                return;
            }
        }
        if (General.GetNullableGuid(txtDocumentId.Text) != null)
        {
            if (rblType.SelectedValue == "0")
            {
                PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessForms(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));
                ucStatus.Text = "Forms/Posters/Checklists added.";
                txtDocumentId.Text = "";
                txtDocumentName.Text = "";
                BindFormPosters();
            }
            else if (rblType.SelectedValue == "1")
            {
                PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessProcedures(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));
                ucStatus.Text = "Procedures added.";
                txtDocumentId.Text = "";
                txtDocumentName.Text = "";
                BindFormPosters();
            }
            else if (rblType.SelectedValue == "2")
            {
                PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessEmergency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));
                ucStatus.Text = "Emergency/Contingency added.";
                txtDocumentId.Text = "";
                txtDocumentName.Text = "";
                BindFormPosters();
            }
        }
        else
        {
            if (rblType.SelectedValue == "0")
            {
                ucError.ErrorMessage = "Please select the Forms/Posters/Checklists.";
                ucError.Visible = true;
                return;
            }
            else if (rblType.SelectedValue == "1")
            {
                ucError.ErrorMessage = "Please select the Procedures.";
                ucError.Visible = true;
                return;
            }
            else if (rblType.SelectedValue == "2")
            {
                ucError.ErrorMessage = "Please select the Emergency/Contingency.";
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();
        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(5, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    //protected void BindGrid()
    //{
    //    gvEconomicRisk.Rebind();
    //    gvEnvironmentalRisk.Rebind();
    //    gvHealthSafetyRisk.Rebind();
    //}
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        RiskAssessmentProcessEdit();
    }

    protected void ClearComponent(object sender, EventArgs e)
    {
        /*txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
         */
    }
    private void RiskAssessmentProcessEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcess.EditInspectionRiskAssessmentProcess(
            General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            txtRefNo.Text = dr["FLDNUMBER"].ToString();
            txtRevNO.Text = dr["FLDREVISIONNO"].ToString();
            txtpreparedby.Text = dr["FLDPREPAREDBYNAME"].ToString();
            ucCreatedDate.Text = dr["FLDPREPAREDDATE"].ToString();
            txtApprovedby.Text = dr["FLDAPPROVEDBYNAME"].ToString();
            ucApprovedDate.Text = dr["FLDAPPROVEDDATE"].ToString();
            txtIssuedBy.Text = dr["FLDISSUEDBYNAME"].ToString();
            ucIssuedDate.Text = dr["FLDISSUEDDATE"].ToString();

            txtDate.Text = dr["FLDDATE"].ToString();
            ddlActivity.SelectedMiscellaneous = dr["FLDACTIVITYID"].ToString();
            txtWorkDetails.Content = dr["FLDWORKACTIVITY"].ToString();

            //if (rblPeopleInvolved.Items.Equals((dr["FLDNUMBEROFPEOPLE"].ToString()) != null))
            //    // rblPeopleInvolved.Items.FindByValue(dr["FLDNUMBEROFPEOPLE"].ToString()).Selected = true;
            //    rblPeopleInvolved.SelectedValue = dr["FLDNUMBEROFPEOPLE"].ToString();

            if (rblPeopleInvolved.SelectedValue != null)
            {
                rblPeopleInvolved.SelectedValue = dr["FLDNUMBEROFPEOPLE"].ToString();
            }

            BindCheckBoxList(cblReason, dr["FLDREASON"].ToString());
            txtOtherReason.Text = dr["FLDOTHERREASON"].ToString();

            //if (rblWorkDuration.Items.Equals((dr["FLDWORKDURATION"].ToString()) != null))
            //    //  rblWorkDuration.Items.FindByValue(dr["FLDWORKDURATION"].ToString()).Selected = true;
            //    rblWorkDuration.SelectedValue = dr["FLDWORKDURATION"].ToString();

            if (rblWorkDuration.SelectedValue != null)
            {
                rblWorkDuration.SelectedValue = dr["FLDWORKDURATION"].ToString();
            }

            //if (rblWorkFrequency.Items.Equals((dr["FLDWORKFREQUENCY"].ToString()) != null))
            //    //rblWorkFrequency.Items.FindByValue(dr["FLDWORKFREQUENCY"].ToString()).Selected = true;
            //    rblWorkFrequency.SelectedValue = dr["FLDWORKFREQUENCY"].ToString();

            if (rblWorkFrequency.SelectedValue != null)
            {
                rblWorkFrequency.SelectedValue = dr["FLDWORKFREQUENCY"].ToString();
            }


            BindCheckBoxList(cblOtherRisk, dr["FLDOTHERRISK"].ToString());
            txtOtherDetails.Text = dr["FLDOTHERRISKDETAILS"].ToString();

            //if (rblOtherRiskControl.Items.((dr["FLDOTHERRISKCONTROL"].ToString()) != null))
            //    rblOtherRiskControl.Items.(dr["FLDOTHERRISKCONTROL"].ToString()).Selected = true;
            rblOtherRiskControl.SelectedValue = dr["FLDOTHERRISKCONTROL"].ToString();
            txtOtherRisk.Content = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
            txtActivity.Text = dr["FLDJOBACTIVITY"].ToString();
            txtProcess.Text = dr["FLDPROCESSNAME"].ToString();
            ddlCategory.SelectedValue = dr["FLDPROCESSNAME"].ToString();
            ucCompetencyLevel.SelectedQuick = dr["FLDCOMPETENCYLEVEL"].ToString();

            BindCheckBoxList(cblImpact, dr["FLDENVIRONMENTIMPACT"].ToString());
            OtherDetailClick(null, null);
            txtActivityCondition.Text = dr["FLDACTIVITYCONDITION"].ToString();

            txtMasterRemarks.Text = dr["FLDAPPROVALREMARKSBYVESSEL"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDAPPROVALREMARKSBYVESSEL"].ToString()))
                chkOverrideByMaster.Checked = true;

            if (!string.IsNullOrEmpty(dr["FLDCOMPANYID"].ToString()))
            {
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            ViewState["companyid"] = dr["FLDCOMPANYID"].ToString();

            DataTable dt = new DataTable();
            dt = dsProcess.Tables[1];

            chkImportedJHAList.DataSource = dt;
            chkImportedJHAList.DataBindings.DataTextField = "FLDHAZARDNUMBER";
            chkImportedJHAList.DataBindings.DataValueField = "FLDJOBHAZARDID";
            chkImportedJHAList.DataBind();
            foreach (ButtonListItem chkitem in chkImportedJHAList.Items)
                chkitem.Selected = true;
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }
    }
    protected void OtherDetailClick(object sender, EventArgs e)
    {
        string reason = ReadCheckBoxList(cblReason);
        string otherrisk = ReadCheckBoxList(cblOtherRisk);
        //string standbyunit = ReadCheckBoxList(cbllStandByUnitDetails);
        //string proposedcontrol = ReadCheckBoxList(cblProposedControl);
        //string proposedcontrolM = ReadCheckBoxList(cblProposedControlM);
        //string commissioning = ReadCheckBoxList(cblCommisionProcedure);
        //string standbyunitm = ReadCheckBoxList(cblStandByUnit);

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
        if (otherrisk.Contains("100"))
        {

            txtOtherDetails.CssClass = "input";
            txtOtherDetails.ReadOnly = false;
        }
        else
        {
            txtOtherDetails.Text = "";
            txtOtherDetails.ReadOnly = true;
            txtOtherDetails.CssClass = "readonlytextbox";
        }
        /*if (standbyunit.ToUpper().Contains("F368F4BA-FAB5-41D8-9E67-79B2D1EC9036"))
        {

            //txtStandByUnitDetails.CssClass = "input";
            //txtStandByUnitDetails.ReadOnly = false;
        }
        else
        {
            //txtStandByUnitDetails.Text = "";
            //txtStandByUnitDetails.ReadOnly = true;
            //txtStandByUnitDetails.CssClass = "readonlytextbox";
        }
        if (proposedcontrol.ToUpper().Contains("F368F4BA-FAB5-41D8-9E67-79B2D1EC9036"))
        {

            //txtProposedControlDetails.CssClass = "input";
            //txtProposedControlDetails.ReadOnly = false;
        }
        else
        {
            //txtProposedControlDetails.Text = "";
            //txtProposedControlDetails.ReadOnly = true;
            //txtProposedControlDetails.CssClass = "readonlytextbox";
        }

        if (standbyunitm.ToUpper().Contains("F368F4BA-FAB5-41D8-9E67-79B2D1EC9036"))
        {
            //txtStandByUnitDetailsM.CssClass = "input";
            //txtStandByUnitDetailsM.ReadOnly = false;
        }
        else
        {
            //txtStandByUnitDetailsM.Text = "";
            //txtStandByUnitDetailsM.ReadOnly = true;
            //txtStandByUnitDetailsM.CssClass = "readonlytextbox";
        }

        if (proposedcontrolM.ToUpper().Contains("F368F4BA-FAB5-41D8-9E67-79B2D1EC9036"))
        {

            //txtProposedControlDetailsM.CssClass = "input";
            //txtProposedControlDetailsM.ReadOnly = false;
        }
        else
        {
            //txtProposedControlDetailsM.Text = "";
            //txtProposedControlDetailsM.ReadOnly = true;
            ///txtProposedControlDetailsM.CssClass = "readonlytextbox";
        }
        if (commissioning.ToUpper().Contains("F368F4BA-FAB5-41D8-9E67-79B2D1EC9036"))
        {

            //txtCtDetail.CssClass = "input";
            //txtCtDetail.ReadOnly = false;
        }
        else
        {
            //txtCtDetail.Text = "";
            //txtCtDetail.ReadOnly = true;
            //txtCtDetail.CssClass = "readonlytextbox";
        }*/
    }
    private void BindData()
    {
        rblPeopleInvolved.DataBindings.DataTextField = "FLDNAME";
        rblPeopleInvolved.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblPeopleInvolved.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(3);
        rblPeopleInvolved.DataBind();

        cblReason.DataBindings.DataTextField = "FLDNAME";
        cblReason.DataBindings.DataValueField = "FLDMISCELLANEOUSID";
        cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(1, null);
        cblReason.DataBind();

        cblOtherRisk.DataBindings.DataTextField = "FLDNAME";
        cblOtherRisk.DataBindings.DataValueField = "FLDHAZARDID";
        cblOtherRisk.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(3, 0);
        cblOtherRisk.DataBind();

        rblWorkDuration.DataBindings.DataTextField = "FLDNAME";
        rblWorkDuration.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblWorkDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
        rblWorkDuration.DataBind();

        rblWorkFrequency.DataBindings.DataTextField = "FLDNAME";
        rblWorkFrequency.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblWorkFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(2);
        rblWorkFrequency.DataBind();

        rblOtherRiskControl.DataBindings.DataTextField = "FLDNAME";
        rblOtherRiskControl.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblOtherRiskControl.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
        rblOtherRiskControl.DataBind();

        //cbllStandByUnitDetails.DataTextField = "FLDITEM";
        //cbllStandByUnitDetails.DataValueField = "FLDID";
        //cbllStandByUnitDetails.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAD")), 1);
        //cbllStandByUnitDetails.DataBind();

        //cblProposedControl.DataTextField = "FLDITEM";
        //cblProposedControl.DataValueField = "FLDID";
        //cblProposedControl.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAP")), 1);
        //cblProposedControl.DataBind();

        //cblStandByUnit.DataTextField = "FLDITEM";
        //cblStandByUnit.DataValueField = "FLDID";
        //cblStandByUnit.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAD")), 2);
        //cblStandByUnit.DataBind();

        //cblProposedControlM.DataTextField = "FLDITEM";
        //cblProposedControlM.DataValueField = "FLDID";
        //cblProposedControlM.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAP")), 2);
        //cblProposedControlM.DataBind();

        //cblCommisionProcedure.DataTextField = "FLDITEM";
        //cblCommisionProcedure.DataValueField = "FLDID";
        //cblCommisionProcedure.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RPC")), 2);
        //cblCommisionProcedure.DataBind();

        //cblFunctionality.DataTextField = "FLDITEM";
        //cblFunctionality.DataValueField = "FLDID";
        //cblFunctionality.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
        //Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "REF")), 2);
        //cblFunctionality.DataBind();
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            SaveRiskAssessmentProcess();
            lnkImportJHA.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'InspectionJHATemplateMapping.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"] + "');return false;");
        }
        if (CommandName.ToUpper().Equals("APPROVE"))
        {
            PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , null
                                                                                        , 2);
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template Approved.";
        }
        if (CommandName.ToUpper().Equals("ISSUE"))
        {
            PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString())
                                                                                       , null
                                                                                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , 3);
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template Issued.";
        }
        if (CommandName.ToUpper().Equals("LIST"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            {
                Response.Redirect("../Inspection/InspectionMainFleetRAProcessList.aspx", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRAProcessList.aspx", false);
            }
        }
        if (CommandName.ToUpper().Equals("BACK"))
        {
            if ((ViewState["RATYPE"].ToString() == "1") && (ViewState["mocextention"].ToString() == ""))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
            if ((ViewState["RATYPE"].ToString() == "1") && (ViewState["mocextention"].ToString() == "yes"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
        }
    }


    private void SaveRiskAssessmentProcess()
    {
        try
        {
            string recorddate = txtDate.Text;
            string peopleinvolved = rblPeopleInvolved.SelectedValue;
            string reasonforassessment = ReadCheckBoxList(cblReason);
            string otherreason = txtOtherReason.Text;
            string durationofworkactivity = rblWorkDuration.SelectedValue;
            string frequencyofworkactivity = rblWorkFrequency.SelectedValue;
            string otherrisk = ReadCheckBoxList(cblOtherRisk);
            string otherriskdetails = txtOtherDetails.Text;
            string otherriskcontrol = rblOtherRiskControl.SelectedValue;
            string otherriskproposed = HttpUtility.HtmlDecode(txtOtherRisk.Content);
            //string stbyunit = ReadCheckBoxList(cbllStandByUnitDetails);
            //string stbycontrols = ReadCheckBoxList(cblProposedControl);

            if (!IsValidProcessTemplate())
            {
                ucError.Visible = true;
                return;
            }
            int? processid = General.GetNullableInteger(ddlCategory.SelectedValue);

            Guid? riskassessmentprocessidout = Guid.NewGuid();
            PhoenixInspectionRiskAssessmentProcess.InsertInspectionRiskAssessmentProcess(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                General.GetNullableDateTime(recorddate),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableString(HttpUtility.HtmlDecode(txtWorkDetails.Content)),
                General.GetNullableString(durationofworkactivity),
                General.GetNullableString(frequencyofworkactivity),
                General.GetNullableInteger(rblPeopleInvolved.SelectedValue),
                null,
                null,
                General.GetNullableString(reasonforassessment),
                General.GetNullableString(otherreason),
                General.GetNullableString(otherrisk),
                General.GetNullableString(otherriskdetails),
                General.GetNullableString(otherriskcontrol),
                General.GetNullableString(otherriskproposed),
                null,//General.GetNullableInteger(rblStandByUnit.SelectedValue),
                null,//General.GetNullableString(stbyunit),
                null,//General.GetNullableString(txtStandByUnitDetails.Text),
                null,//General.GetNullableInteger(rblStandByEffective.SelectedValue),
                null,//General.GetNullableString(stbycontrols),
                null,//General.GetNullableString(txtProposedControlDetails.Text),
                null,//General.GetNullableString(txtRemarks.Text),
                null,//General.GetNullableGuid(txtComponentId.Text),
                null,//General.GetNullableString(txtStandByUnitDetailsM.Text),
                null,//General.GetNullableString(ReadCheckBoxList(cblFunctionality)),
                null,//General.GetNullableString(txtFunctionality.Text),
                null,//General.GetNullableInteger(rblStandByUnitM.SelectedValue),
                null,//General.GetNullableString(ReadCheckBoxList(cblStandByUnit)),
                null,//General.GetNullableString(txtStandByUnitDetailsM.Text),
                null,//General.GetNullableInteger(rblStandByEffectiveM.SelectedValue),
                null,//General.GetNullableString(ReadCheckBoxList(cblProposedControlM)),
                null,//General.GetNullableString(txtProposedControlDetailsM.Text),
                null,//General.GetNullableString(ReadCheckBoxList(cblCommisionProcedure)),
                null,//General.GetNullableString(txtCtDetail.Text),
                null,//General.GetNullableString(txtRemarks.Text),              
                General.GetNullableString(ReadCheckBoxList(cblImpact)),
                General.GetNullableInteger(ddlActivity.SelectedMiscellaneous),
                General.GetNullableString(txtActivity.Text),
                (processid != null ? processid.ToString() : null),
                ref riskassessmentprocessidout,
                General.GetNullableInteger(ucCompetencyLevel.SelectedQuick),
                General.GetNullableString(txtActivityCondition.Text),
                General.GetNullableGuid(txtDocumentId.Text),
                General.GetNullableInteger(rblType.SelectedValue),
                General.GetNullableInteger(ucCompany.SelectedCompany));

            txtDocumentId.Text = "";
            txtDocumentName.Text = "";

            ViewState["companyid"] = ucCompany.SelectedCompany;

            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["companyid"].ToString() + "', true); ");

            ViewState["RISKASSESSMENTPROCESSID"] = riskassessmentprocessidout.ToString();
            Filter.CurrentSelectedProcessRA = riskassessmentprocessidout.ToString();
            BindFormPosters();
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template updated.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidProcessTemplate()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableString(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }
    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                //if (cbl.Items.FindByValue(item) != null)
                //    cbl.Items.FindByValue(item).Selected = true;
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
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

    protected void gvHealthSafetyRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(1, General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                    General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            gvHealthSafetyRisk.DataSource = ds;
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
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(2, General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                    General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            gvEnvironmentalRisk.DataSource = ds;
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
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(4, General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                    General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            gvEconomicRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEconomicRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }
    protected void gvEconomicRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("CADD"))
            {
                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                gvEconomicRisk.Rebind();
                ucStatus.Text = "Hazard  Added.";

            }
            if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvEconomicRisk.Rebind();
                ucStatus.Text = "Hazard  Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("ENVIRONMENTALADD"))
            {
                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard Added.";

            }
            else if (gce.CommandName.ToUpper().Equals("ENVIRONMENTALDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            UserControlRAMiscellaneous ucImpactType = (UserControlRAMiscellaneous)ge.Item.FindControl("ucImpactType");
            if (ucImpactType != null)
            {
                ucImpactType.MiscellaneousList = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(3, 0);
                ucImpactType.DataBind();
            }

            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            if (ge.Item is GridFooterItem)
            {
                LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                {
                    cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
                }
            }
        }
    }
    protected void gvHealthSafetyRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("HEALTHSAFETYADD"))
            {
                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                        ((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)e.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard Added.";

            }
            else if (e.CommandName.ToUpper().Equals("HEALTHSAFETYDELETE"))
            {
                string categoryid = ((RadLabel)e.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard  Deleted.";
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) != null)
        {
            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Hazard Type is required.";

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
    private bool IsValidSpareTools(string name, string option)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) != null)
        {
            if (General.GetNullableGuid(name) == null)
                ucError.ErrorMessage = "Item is required";

            if (General.GetNullableInteger(option) == null)
                ucError.ErrorMessage = "Select atleast one option";

        }

        return (!ucError.IsError);

    }

    /*private void BindGridRiskQuestionsMac()
    {        
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentProcess.ListProcessControl(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID,2);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvPersonalMac.DataSource = ds;
            gvPersonalMac.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPersonalMac);
        }

        string rattype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAS");
        DataTable dtassessment = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(Convert.ToInt32(rattype), 2);
        RadComboBox ddlAssessmentType = (RadComboBox)gvPersonalMac.FooterRow.FindControl("ddlAssessmentTypeAdd");
        ddlAssessmentType.DataSource = dtassessment;
        ddlAssessmentType.DataTextField = "FLDITEM";
        ddlAssessmentType.DataValueField = "FLDID";
        ddlAssessmentType.DataBind();
    }*/

    protected void gvPersonalMac_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("RMADD"))
            {

                if (!IsValidSpareTools(((RadComboBox)_gridView.FooterRow.FindControl("ddlAssessmentTypeAdd")).SelectedValue,
                                        ((RadioButtonList)_gridView.FooterRow.FindControl("rblOptionAdd")).SelectedValue
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(((RadComboBox)_gridView.FooterRow.FindControl("ddlAssessmentTypeAdd")).SelectedValue),
                        Convert.ToInt32(((RadioButtonList)_gridView.FooterRow.FindControl("rblOptionAdd")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID, 2);

                //BindGridRiskQuestionsMac();

            }
            else if (gce.CommandName.ToUpper().Equals("RMDELETE"))
            {
                string controlid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblId")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(controlid));
                //BindGridRiskQuestionsMac();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPersonalMac_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton db = (LinkButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }

    protected void chkImportedJHAList_Changed(object sender, EventArgs e)
    {
        StringBuilder strjhaid = new StringBuilder();
        foreach (ButtonListItem item in chkImportedJHAList.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strjhaid.Append(item.Value.ToString());
                strjhaid.Append(",");
            }
        }

        if (strjhaid.Length > 1)
        {
            strjhaid.Remove(strjhaid.Length - 1, 1);
        }
        string jhaid = strjhaid.ToString();
        PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentJHAImport(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                                                     jhaid);
        ucStatus.Text = "Imported JHA has been removed successfully.";
        cmdHiddenSubmit_Click(sender, new EventArgs());
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        gvHealthSafetyRisk.Rebind();
        GridFooterItem gvHealthSafetyRiskfooteritem = (GridFooterItem)gvHealthSafetyRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlHealthSubHazardType = (RadComboBox)gvHealthSafetyRiskfooteritem.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();
    }

    protected void ucEnvHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEnvironmentalRisk.Rebind();
        GridFooterItem gvEnvironmentalRiskfooteritem = (GridFooterItem)gvEnvironmentalRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddllEnvSubHazardType = (RadComboBox)gvEnvironmentalRiskfooteritem.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();
    }

    protected void ucEconomicHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEconomicRisk.Rebind();
        GridFooterItem gvEconomicRiskfooteritem = (GridFooterItem)gvEconomicRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEcoSubHazardType = (RadComboBox)gvEconomicRiskfooteritem.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();
    }
}
