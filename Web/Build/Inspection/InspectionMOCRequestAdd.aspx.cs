using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class InspectionMOCRequestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        confirm.Attributes.Add("style", "display:none;");
        ucConfirmRevision.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbarstatus = new PhoenixToolbar();
        toolbarstatus.AddButton("List", "LIST", ToolBarDirection.Left);
        toolbarstatus.AddButton("Details", "DETAILS", ToolBarDirection.Left);
        toolbarstatus.AddButton("Action Plan", "ACTIONPLAN", ToolBarDirection.Left);
        toolbarstatus.AddButton("Evaluation & Approval", "EVALUATION", ToolBarDirection.Left);
        toolbarstatus.AddButton("Intermediate Verification", "INTERMEDIATE", ToolBarDirection.Left);
        toolbarstatus.AddButton("Implementation & Verification", "IMPLEMENTATION", ToolBarDirection.Left);

        MenuMOCStatus.MenuList = toolbarstatus.Show();
        MenuMOCStatus.SelectedMenuIndex = 1;

        PhoenixToolbar toolbarsubmit = new PhoenixToolbar();
        toolbarsubmit.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
        toolbarsubmit.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMOCSubmit.MenuList = toolbarsubmit.Show();

        if (!IsPostBack)
        {
            ViewState["MOCID"] = "";
            ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
            ViewState["MOCREQUIREDID"] = "";
            ViewState["MOCRISKASSESSMENTID"] = "";
            ViewState["MOCHardAdd"] = "";
            ViewState["MOCHardEdit"] = "";
            ViewState["RiskAssessmentid"] = "";
            ViewState["MOCRequestid"] = "";
            ViewState["Vesselid"] = "";
            ViewState["DepartmentAdd"] = "";
            ViewState["DepartmentEdit"] = "";
            ViewState["MOCSupportAdd"] = "";
            ViewState["MOCSupportEdit"] = "";
            ViewState["ExternalAdd"] = "";
            ViewState["ExternalEdit"] = "";
            ViewState["MOCSupportitemAdd"] = "";
            ViewState["MOCSupportitemEdit"] = "";
            ViewState["ShipboardAdd"] = "";
            ViewState["ShipboardEdit"] = "";
            ViewState["ShorebasedAdd"] = "";
            ViewState["ShorebasedEdit"] = "";
            ViewState["txtActionToBeTakenAdd"] = "";
            ViewState["REGULATIONID"] = "";
            ViewState["FLDMOCSUPPORTITEMREQUIREDID"] = "";
            ViewState["REV"] = "";
            ViewState["SUPPORTITEMID"] = "";

            if (Request.QueryString["RiskAssessmentid"] != null && Request.QueryString["RiskAssessmentid"].ToString() != "")
            {
                ViewState["RiskAssessmentid"] = Request.QueryString["RiskAssessmentid"].ToString();
            }
            else
            {
                ViewState["RiskAssessmentid"] = "";
            }
            if (Request.QueryString["MOCRequestid"] != null && Request.QueryString["MOCRequestid"].ToString() != "")
            {
                ViewState["MOCRequestid"] = Request.QueryString["MOCRequestid"].ToString();
            }
            if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() != "")
            {
                ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
            }
            if (Request.QueryString["RefNo"] != null)
            {
                txtRANumber.Text = Request.QueryString["RefNo"].ToString();
            }
            if (ViewState["REV"].ToString() == string.Empty)
            {
                imgrevision.Visible = false;
            }
            
            BindMOC();
            EditVessel();
            BindMOCRequest();
            BindMOCCategory();
            BindMOCNewRegulation();
            RiskAssessmentGenericEdit();
            txtCrewId.Attributes.Add("style", "display:none");
            txtPersonInChargeOfficeId.Attributes.Add("style", "display:none");
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCQuestionList.aspx?MOCSectionId=10&MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "SUADD");
        MenuMOCSupportRequired.AccessRights = this.ViewState;
        MenuMOCSupportRequired.MenuList = toolbar.Show();

        PhoenixToolbar toolbarExternal = new PhoenixToolbar();
        toolbarExternal.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCQuestionList.aspx?MOCSectionId=11&MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "EXADD");
        MenuMOCExternalApproval.AccessRights = this.ViewState;
        MenuMOCExternalApproval.MenuList = toolbarExternal.Show();

        PhoenixToolbar toolbarSupportItem = new PhoenixToolbar();
        toolbarSupportItem.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCQuestionList.aspx?MOCSectionId=12&MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "SRADD");
        MenuMOCSupportItemRequired.AccessRights = this.ViewState;
        MenuMOCSupportItemRequired.MenuList = toolbarSupportItem.Show();

        PhoenixToolbar toolbarShipBoard = new PhoenixToolbar();
        toolbarShipBoard.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCQuestionList.aspx?MOCSectionId=13&MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "SIADD");
        MenuMOCShipboard.AccessRights = this.ViewState;
        MenuMOCShipboard.MenuList = toolbarShipBoard.Show();

        PhoenixToolbar toolbarShoreBoard = new PhoenixToolbar();
        toolbarShoreBoard.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCQuestionList.aspx?MOCSectionId=14&MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "SOADD");
        MenuMOCShoreBased.AccessRights = this.ViewState;
        MenuMOCShoreBased.MenuList = toolbarShoreBoard.Show();

        PhoenixToolbar toolbarTrainingRequired = new PhoenixToolbar();
        toolbarTrainingRequired.AddFontAwesomeButton("../Inspection/InspectionMOCTrainingAdd.aspx?MOCID=" + ViewState["MOCID"].ToString() + "&VESSELID=" + ViewState["Vesselid"].ToString(), "New Task", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuTrainingRequired.AccessRights = this.ViewState;
        MenuTrainingRequired.MenuList = toolbarTrainingRequired.Show();

        if (ViewState["Vesselid"].ToString() != "0")
        {
            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
        else
        {
            spnPersonInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        MOCRiskassessmentvisble();

        BindComponents();
        BindHSEQA();

        btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnShowDocuments', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + companyid + "'); ");

        if (ddlNatureofChange.SelectedValue == "1")
        {
            ucDateofrestoration.Visible = false;
            lblDateofrestoration.Visible = false;
        }
        else
        {
            ucDateofrestoration.Visible = true;
            lblDateofrestoration.Visible = true;
        }

    }

    private void BindMOCNewRegulation()
    {
        DataSet ds;

        ds = PhoenixInspectionNewRegulation.MOCRegulationDetail(new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblnewRegulationTitle.Visible = true;
            txtNewRegulationTitle.Visible = true;
            txtNewRegStatus.Visible = true;
            lblNRissueddate.Visible = true;
            dteNRIssue.Visible = true;
            txtNewRegulationTitle.Text = dr["FLDTITLE"].ToString();
            txtNewRegStatus.Text = dr["FLDSTATUS"].ToString();
            dteNRIssue.Text = dr["FLDISSUEDATE"].ToString();
            ViewState["REGULATIONID"] = dr["FLDREGULATIONID"].ToString();
        }

        if (txtNewRegulationTitle.Text == "")
        {
            lnkNewRegulation.Text = "New Regulation";
        }
        else
        {
            lnkNewRegulation.Text = "Edit/View";
            lnkNewRegulation.Visible = true;
            lnkNRStatus.Visible = true;
            lblNRissueddate.Visible = true;
            dteNRIssue.Visible = true;
        }

        lnkNRStatus.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionRegulationComplianceStatus.aspx?ismoc=true&RegulationId=" + ViewState["REGULATIONID"].ToString() + "');return true;");

    }

    protected void gvMOCSupportRequired_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindMOCsupport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCExternalApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindMOCExternalApproval();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCSupportItemRequired_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindSupportItemRequired();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCShipboard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindShipboardPersonnelaffected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCShoreBased_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindShoreBasedPersonnelaffected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCTrainingRequired_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindTrainingRequired();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMOCSubmit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMOCRequest(ddlCategory.SelectedValue
                                        , ddlSubCategory.SelectedValue
                                        , ddlNatureofChange.SelectedValue
                                        , ucTargetDateofimplementation.Text
                                        , ucDateofrestoration.Text
                                        , txtDetailsoftheProposedChange.Text
                                        , txtOptionsconsidered.Text
                                        , txtJustificationforchange.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionMOCTemplate.MOCInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                            , int.Parse(ViewState["Vesselid"].ToString())
                                            , General.GetNullableString(txtmoctitle.Text)
                                            , (ddlvessel.SelectedVessel == "0") ? General.GetNullableInteger(txtPersonInChargeOfficeId.Text) : General.GetNullableInteger(txtCrewId.Text)
                                            , (ddlvessel.SelectedVessel == "0") ? General.GetNullableString(txtOfficePersonName.Text) : General.GetNullableString(txtCrewName.Text)
                                            , (ddlvessel.SelectedVessel == "0") ? General.GetNullableString(txtOfficePersonDesignation.Text) : General.GetNullableString(txtCrewRank.Text)
                                            , General.GetNullableInteger(lblstatusid.Text)
                                            , General.GetNullableDateTime(ucMOCDate.Text)
                                            , General.GetNullableDateTime(null)
                                            , null
                                            , null
                                            , ucCompany.SelectedCompany
                                            , rdbNewRegulation.SelectedValue
                                         );

                PhoenixInspectionMOCRequestForChange.MOCRequestInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid((ViewState["MOCRequestid"]).ToString())
                                                            , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                            , int.Parse(ddlvessel.SelectedVessel)
                                                            , General.GetNullableGuid(ddlCategory.SelectedValue)
                                                            , General.GetNullableGuid(ddlSubCategory.SelectedValue)
                                                            , General.GetNullableInteger(ddlNatureofChange.SelectedValue)
                                                            , General.GetNullableDateTime(ucTargetDateofimplementation.Text)
                                                            , General.GetNullableDateTime(ucDateofrestoration.Text)
                                                            , General.GetNullableString(txtDetailsoftheProposedChange.Text)
                                                            , General.GetNullableString(txtOptionsconsidered.Text)
                                                            , General.GetNullableString(txtJustificationforchange.Text));

                if (Request.QueryString["RiskAssessmentid"] != null && Request.QueryString["RiskAssessmentid"].ToString() != "")
                {
                    PhoenixInspectionMOCRequestForChange.MOCRiskAssessmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , General.GetNullableGuid((ViewState["MOCRISKASSESSMENTID"]).ToString())
                                                                        , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                                        , int.Parse(ddlvessel.SelectedVessel)
                                                                        , General.GetNullableGuid((ViewState["RiskAssessmentid"]).ToString())
                                                                        , General.GetNullableString(txtRANumber.Text)
                                                                        , General.GetNullableInteger(null)
                                                                        , General.GetNullableInteger(null));
                }
                BindMOCRequest();
                BindMOC();
                ucStatus.Text = "Request for Change updated successfully.";

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                RadWindowManager1.RadConfirm("Are you sure you completed the Section A and B ?", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMOCStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("ACTIONPLAN"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["Vesselid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("EVALUATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestEvalutionApproval.aspx?&VESSELID=" + ViewState["Vesselid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("INTERMEDIATE"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?&VESSELID=" + ViewState["Vesselid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("IMPLEMENTATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestImplementationVerification.aspx?&VESSELID=" + ViewState["Vesselid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.ClearSelection();
        BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));
    }
    protected void ddlNatureofChange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNatureofChange.SelectedValue == "1")
        {
            ucDateofrestoration.Visible = false;
            lblDateofrestoration.Visible = false;
        }
        else
        {
            ucDateofrestoration.Visible = true;
            lblDateofrestoration.Visible = true;
        }
    }

    private bool IsValidMOCRequest(string Category, string SubCategory, string NatureofChange, string TargetDateofimplementation, string Dateofrestoration, string DetailsoftheProposedChange, string Optionsconsidered, string Justificationforchange)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Category.Equals("Dummy"))
            ucError.ErrorMessage = "Category is required.";

        if ((SubCategory.Equals("Dummy")) || (SubCategory.Equals("")))
            ucError.ErrorMessage = "Sub - Category is required.";

        if (NatureofChange.Equals("Dummy"))
            ucError.ErrorMessage = "Nature of Change is required.";

        if (General.GetNullableDateTime(TargetDateofimplementation) == null)
            ucError.ErrorMessage = "Target Date of implementation is required.";

        if ((General.GetNullableDateTime(Dateofrestoration) == null) && (NatureofChange.Equals("118")))
            ucError.ErrorMessage = "Date of restoration to original state is required.";

        if (DetailsoftheProposedChange.Equals(""))
            ucError.ErrorMessage = "Details of the Proposed Change is required.";

        if (Optionsconsidered.Equals(""))
            ucError.ErrorMessage = "Options considered for the change (What options were considered before selecting this particular option?)is required.";

        if (Justificationforchange.Equals(""))
            ucError.ErrorMessage = "Justification for change (Provide reason for proposed change safer operations, cost benefit, etc.)";

        return (!ucError.IsError);
    }
    private void BindMOCStatus()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCRequestForChange.MOCStatusList();
        if (ds.Tables[0].Rows.Count > 0)
        {
            //ddlstatus.DataSource = ds;
            //ddlstatus.DataTextField = "FLDSTATUSNAME";
            //ddlstatus.DataValueField = "FLDMOCSTATUSID";
            //ddlstatus.DataBind();
        }
    }
    protected void rdbNewRegulation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbNewRegulation.SelectedValue == "1")
        {
            lblnewRegulationTitle.Visible = true;
            lnkNewRegulation.Visible = true;
            txtNewRegulationTitle.Visible = true;
            txtNewRegStatus.Visible = true;
            dteNRIssue.Visible = true;
            lblNRissueddate.Visible = true;

            if (txtNewRegulationTitle.Text != "")
            {
                lnkNRStatus.Visible = true;
                dteNRIssue.Visible = true;
                lblNRissueddate.Visible = true;
            }
        }
        else
        {
            lblnewRegulationTitle.Visible = false;
            lnkNewRegulation.Visible = false;
            txtNewRegulationTitle.Visible = false;
            txtNewRegStatus.Visible = false;
            lnkNRStatus.Visible = false;
            dteNRIssue.Visible = false;
            lblNRissueddate.Visible = false;
        }
    }

    private void BindMOCRequest()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCRequestForChange.MOCRequestEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ddlCategory.SelectedValue = dr["FLDCATEGORY"].ToString();
            ucTargetDateofimplementation.Text = dr["FLDIMPLEMENTATIONTARGETDATE"].ToString();
            ucDateofrestoration.Text = dr["FLDRESTORATIONDATE"].ToString();
            txtDetailsoftheProposedChange.Text = dr["FLPROPOSEDCHANGEDETAILS"].ToString();
            txtOptionsconsidered.Text = dr["FLDOPTIONCONSIDERED"].ToString();
            txtJustificationforchange.Text = dr["FLDJUSTIFICATION"].ToString();
            ViewState["MOCRequestid"] = dr["FLDMOCREQUESTID"].ToString();
            ddlSubCategory.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
            ddlNatureofChange.SelectedValue = dr["FLDNATUREOFCHANGE"].ToString();
            //rblRiskAssessmentRequired.SelectedValue = 
        }
    }
   
    private void BindMOC()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.MOCEdit(new Guid(ViewState["MOCID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ddlvessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            txtmoctitle.Text = dr["FLDMOCTITLE"].ToString();

            txtFlag.Text = dr["FLDVESSELFLAG"].ToString();
            txtAddrOwner.Text = dr["FLDVESSELOWNER"].ToString();
            txtCharterer.Text = dr["FLDVESSELCHARTERER"].ToString();

            ddlstatus.Text = dr["FLDSTATUS"].ToString();
            ddlstatus.Enabled = false;
            lblstatusid.Text = dr["FLDMOCSTATUSID"].ToString();
            if (dr["FLDVESSELID"].ToString() != "0")
            {
                txtCrewName.Text = dr["FLDPROPOSERNAME"].ToString();
                txtCrewRank.Text = dr["FLDPROPOSERRANK"].ToString();
                txtCrewId.Text = dr["FLDPROPOSERID"].ToString();
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
            }
            else
            {
                txtOfficePersonName.Text = dr["FLDPROPOSERNAME"].ToString();
                txtOfficePersonDesignation.Text = dr["FLDPROPOSERRANK"].ToString();
                txtPersonInChargeOfficeId.Text = dr["FLDPROPOSERID"].ToString();
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
            }
            ucMOCDate.Text = dr["FLDMOCDATE"].ToString();
            lblrequestchangeid.Text = "Section A: Request for Change (" + dr["FLDPROPOSERNAME"].ToString() + " - " + dr["FLDPROPOSERRANK"].ToString() + ")";
            lblrequestchangesid.Text = "Section B: Request for Change (" + dr["FLDPROPOSERNAME"].ToString() + " - " + dr["FLDPROPOSERRANK"].ToString() + ")";
            txtRA.Text = dr["FLDACTIVITYCONDITIONS"].ToString();
            txtRANumber.Text = dr["FLDRANUMBER"].ToString();
            ViewState["RAGENERICID"] = dr["FLDRISKASSESSMENTID"].ToString();
            ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();

            ViewState["MOCRISKASSESSMENTID"] = dr["FLDRISKASSESSMENTID"].ToString();
            txtRAId.Text = dr["FLDRISKASSESSMENTID"].ToString();
            ViewState["REV"] = dr["FLDREVYN"].ToString();
            if (ViewState["REV"].ToString() == "1")
            {
                imgrevision.Visible = true;
            }
            else if (ViewState["REV"].ToString() == "0" || ViewState["REV"].ToString() == string.Empty)
            {
                imgrevision.Visible = false;
            }

            if (txtRA.Text == "")
            {
                lnkCreateRA.Text = "Create Risk Assessment";
            }
            else
            {
                lnkCreateRA.Text = "Edit/View";
                ImgRA.Visible = true;
            }
            rdbNewRegulation.SelectedValue = dr["FLDNEWREQULATIONREQUIREDYN"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                lblnewregulationreq.Visible = false;
                rdbNewRegulation.Visible = false;
            }
        }
    }

    protected void BindMOCCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionMOCCategory.MOCCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlCategory.DataTextField = "FLDMOCCATEGORYNAME";
        ddlCategory.DataValueField = "FLDMOCCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));

    }
    private void BindSubCategory(int? catagory)
    {
        ddlSubCategory.DataSource = PhoenixInspectionMOCCategory.MOCSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDMOCSUBCATEGORYNAME";
        ddlSubCategory.DataValueField = "FLDMOCSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void lnkNewRegulation_OnClick(object sender, EventArgs e)
    {
        // if (txtNewRegulationTitle.Text == "")
        // {
            if (rdbNewRegulation.SelectedValue == "1")
            {
                Response.Redirect("../Inspection/InspectionRegulationAdd.aspx?&Mocid=" + ViewState["MOCID"].ToString() + "", true);
            }
        //}
        //else
        //{
          //  Response.Redirect("../Inspection/InspectionRegulationUpdate.aspx?&Mocid=" + ViewState["MOCID"].ToString() + "", true);
        //}
    }
    protected void lnkCreateRA_OnClick(object sender, EventArgs e)
    {
        if (txtRA.Text == "")
        {
            Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?status=&RAType=1&Vesselid=" + ViewState["Vesselid"].ToString() + "&Vesselname=" + ddlvessel.SelectedVesselName + "&MOCID=" + ViewState["MOCID"].ToString() + "&mocextention=", true);
        }
        else
        {
            Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?status=4&RAType=1&genericid=" + ViewState["RAGENERICID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString() + "&Vesselname=" + ddlvessel.SelectedVesselName + "&MOCID=" + ViewState["MOCID"].ToString() + "&mocextention=", true);
        }
    }

    private void RiskAssessmentGenericEdit()
    {
        if ((ViewState["RAGENERICID"] != null) && (ViewState["RAGENERICID"].ToString() != ""))
        {
            DataTable dt = PhoenixInspectionRiskAssessmentGenericExtn.EditPhoenixInspectionRiskAssessmentGeneric(
            General.GetNullableGuid(ViewState["RAGENERICID"].ToString()));

            foreach (DataRow dr in dt.Rows)
            {
                decimal minscore = 0, maxscore = 0;
               
                if (!string.IsNullOrEmpty(dr["FLDMINSCORE"].ToString()))
                    minscore = decimal.Parse(dr["FLDMINSCORE"].ToString());

                if (!string.IsNullOrEmpty(dr["FLDMAXSCORE"].ToString()))
                    maxscore = decimal.Parse(dr["FLDMAXSCORE"].ToString());

                lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskHealth.Text))
                {
                    if (decimal.Parse(lblLevelofRiskHealth.Text) <= minscore)
                        levelofriskhealth.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskHealth.Text) > minscore && decimal.Parse(lblLevelofRiskHealth.Text) <= maxscore)
                        levelofriskhealth.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskHealth.Text) > maxscore)
                        levelofriskhealth.BgColor = "Red";
                }
                else
                    levelofriskhealth.BgColor = "White";

                lblLevelofRiskEnv.Text = dr["FLDENVLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskEnv.Text))
                {
                    if (decimal.Parse(lblLevelofRiskEnv.Text) <= minscore)
                        levelofriskenv.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskEnv.Text) > minscore && decimal.Parse(lblLevelofRiskEnv.Text) <= maxscore)
                        levelofriskenv.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskEnv.Text) > maxscore)
                        levelofriskenv.BgColor = "Red";
                }
                else
                    levelofriskenv.BgColor = "White";

                lblLevelofRiskEconomic.Text = dr["FLDECOLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskEconomic.Text))
                {
                    if (decimal.Parse(lblLevelofRiskEconomic.Text) <= minscore)
                        levelofriskeco.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskEconomic.Text) > minscore && decimal.Parse(lblLevelofRiskEconomic.Text) <= maxscore)
                        levelofriskeco.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskEconomic.Text) > maxscore)
                        levelofriskeco.BgColor = "Red";
                }
                else
                    levelofriskeco.BgColor = "White";

                lblLevelofRiskWorst.Text = dr["FLDWCLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskWorst.Text))
                {
                    if (decimal.Parse(lblLevelofRiskWorst.Text) <= minscore)
                        levelofriskworst.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskWorst.Text) > minscore && decimal.Parse(lblLevelofRiskWorst.Text) <= maxscore)
                        levelofriskworst.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskWorst.Text) > maxscore)
                        levelofriskworst.BgColor = "Red";
                }
                else
                    levelofriskworst.BgColor = "White";

                lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();
                lblLevelofRiskEnv.Text = dr["FLDENVLR"].ToString();
                lblLevelofRiskEconomic.Text = dr["FLDECOLR"].ToString();
                lblLevelofRiskWorst.Text = dr["FLDWCLR"].ToString();
                lblhscontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblencontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lbleccontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblwscontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
                lblreshsrisk.Text = dr["FLDHSRR"].ToString();
                lblresenrisk.Text = dr["FLDENVRR"].ToString();
                lblresecorisk.Text = dr["FLDECORR"].ToString();
                lblreswsrisk.Text = dr["FLDWCRR"].ToString();
                lblHealthDescription.Text = dr["FLDHSRISKLEVELTEXT"].ToString();
                lblEnvDescription.Text = dr["FLDENVRISKLEVELTEXT"].ToString();
                lblEconomicDescription.Text = dr["FLDECORISKLEVELTEXT"].ToString();
                lblWorstDescription.Text = dr["FLDWSRISKLEVELTEXT"].ToString();
                lblimpacthealth.Text = dr["FLDHSSCORE"].ToString();
                lblimpacteco.Text = dr["FLDECOSCORE"].ToString();
                lblimpactenv.Text = dr["FLDENVSCORE"].ToString();
                lblimpactws.Text = dr["FLDWSSCORE"].ToString();

                ViewState["ACTIVITYID"] = dr["FLDACTIVITYID"].ToString();

                lblPOOhealth.Text = dr["FLDHSPOO"].ToString();
                lblPOOeco.Text = dr["FLDECOPOO"].ToString();
                lblPOOenv.Text = dr["FLDENVPOO"].ToString();
                lblPOOws.Text = dr["FLDWSPOO"].ToString();

                lbllohhealth.Text = dr["FLDHSLOH"].ToString();
                lblloheco.Text = dr["FLDECOLOH"].ToString();
                lbllohenv.Text = dr["FLDENVLOH"].ToString();
                lbllohws.Text = dr["FLDWSLOH"].ToString();

                lblControlshealth.Text = dr["FLDHSLOC"].ToString();
                lblControlseco.Text = dr["FLDECOLOC"].ToString();
                lblControlsenv.Text = dr["FLDENVLOC"].ToString();
                lblControlsws.Text = dr["FLDWSLOC"].ToString();
            }
        }
    }
    protected void ImgRA_Click(object sender, EventArgs e)
    {
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void rblRiskAssessmentRequired_SelectedIndexChanged(object sender, EventArgs e)
    {
        MOCRiskassessmentvisble();
    }

    private void BindMOCsupport()
    {

        string[] alColumns = { "FLDSUPPORTREQUIREDQUESTIONID", "FLDSUPPORTREQUIREDYN", "FLDSUPPORTREQUIREDDETAILS", };
        string[] alCaptions = { "From Whom?", "Required", "What type of support required?" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCSupportRequiredList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , General.GetNullableGuid(ViewState["MOCID"].ToString()));


        gvMOCSupportRequired.DataSource = ds;

    }

    protected void gvMOCSupportRequired_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCSupportRequiredDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(((RadLabel)e.Item.FindControl("lblMOCSupportRequiredid")).Text));
                BindMOCsupport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvMOCSupportRequired_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvMOCSupportRequired.Rebind();
    }

    protected void gvMOCSupportRequired_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCSupportRequired(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text, Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCRequestForChange.MOCSupportRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCSupportRequiredid")).Text)
                                                                      , new Guid((ViewState["MOCID"]).ToString())
                                                                      , int.Parse(ddlvessel.SelectedVessel)
                                                                      , Int32.Parse(((RadLabel)e.Item.FindControl("lblhardcode")).Text)
                                                                      , Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                      , (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text)
                                                                      , (((RadTextBox)e.Item.FindControl("txtotherdetailedit")).Text)
                                                                      );
            BindMOCsupport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCSupportRequired_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        string mochardcode = "";

        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.mochardcode(10, "OTH");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            mochardcode = dr["FLDMOCHARDCODE"].ToString();
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            RadLabel lblquestionnameedit = ((RadLabel)e.Item.FindControl("lblquestionnameedit"));
            RadLabel lblquestion = ((RadLabel)e.Item.FindControl("lblquestion"));
            RadLabel lblquestionname = ((RadLabel)e.Item.FindControl("lblquestionname"));
            RadLabel lblquestionid = ((RadLabel)e.Item.FindControl("lblquestionid"));
            RadTextBox txtotherdetailedit = ((RadTextBox)e.Item.FindControl("txtotherdetailedit"));
            RadLabel lblotherdetail = ((RadLabel)e.Item.FindControl("lblotherdetail"));

            RadLabel lblreqid = (RadLabel)e.Item.FindControl("lblMOCSupportRequiredid");

            if (e.Item.IsInEditMode)
            {
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCSectionRemarks.aspx?MOCSectionId=10&SUPPORTREQID=" + lblreqid.Text + "');return false;");
                }
            }
            else
            {
                if ((lblquestionid.Text == mochardcode))
                {
                    lblquestionname.Visible = false;
                    lblotherdetail.Text = lblquestionname.Text + " - " + lblotherdetail.Text;
                    lblotherdetail.Visible = true;
                }
            }
        }
    }
    private bool IsValidMOCSupportRequired(string supportid, int valid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (valid == 1 && supportid.Equals(""))
            ucError.ErrorMessage = "From Whom Support required?";

        return (!ucError.IsError);
    }

    private void BindMOCExternalApproval()
    {
        string[] alColumns = { "FLDEXTERNALAPPROVALQUESTIONID", "FLDEXTERNALAPPROVALYN", "FLDEXTERNALAPPROVALDETAILS", };
        string[] alCaptions = { "From Whom?", "Required", "Details of Approval required?" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequiredList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["MOCID"].ToString()));
        gvMOCExternalApproval.DataSource = ds;

    }
    protected void gvMOCExternalApproval_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindMOCExternalApproval();
    }
    protected void gvMOCExternalApproval_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequiredDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(((RadLabel)e.Item.FindControl("lblMOCExternalApprovalid")).Text.ToString()));
                BindMOCExternalApproval();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvMOCExternalApproval_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindMOCExternalApproval();
    }
    protected void gvMOCExternalApproval_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindMOCExternalApproval();
    }
    protected void gvMOCExternalApproval_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCExternalApprovalRequired(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCRequestForChange.MOCExternalApprovalRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCExternalApprovalid")).Text)
                                                                      , new Guid((ViewState["MOCID"]).ToString())
                                                                      , int.Parse(ddlvessel.SelectedVessel)
                                                                      , Int32.Parse(((RadLabel)e.Item.FindControl("lblhardcode")).Text)
                                                                      , Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                      , (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text)
                                                                      , (((RadTextBox)e.Item.FindControl("txtotherdetailEdit")).Text));

            BindMOCExternalApproval();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCExternalApproval_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        string mochardcode = "";

        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.mochardcode(11, "OTH");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            mochardcode = dr["FLDMOCHARDCODE"].ToString();
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            RadLabel lblquestionnameedit = ((RadLabel)e.Item.FindControl("lblquestionnameedit"));
            RadLabel lblquestion = ((RadLabel)e.Item.FindControl("lblquestion"));
            RadLabel lblquestionname = ((RadLabel)e.Item.FindControl("lblquestionname"));
            RadLabel lblquestionid = ((RadLabel)e.Item.FindControl("lblquestionid"));
            RadTextBox txtotherdetailedit = ((RadTextBox)e.Item.FindControl("txtotherdetailedit"));
            RadLabel lblotherdetail = ((RadLabel)e.Item.FindControl("lblotherdetail"));

            RadLabel lblreqid = (RadLabel)e.Item.FindControl("lblMOCExternalApprovalid");

            if (e.Item.IsInEditMode)
            {
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCSectionRemarks.aspx?MOCSectionId=11&SUPPORTREQID=" + lblreqid.Text + "');return false;");
                }
            }
            else
            {
                if ((lblquestionid.Text == mochardcode))
                {
                    lblquestionname.Visible = false;
                    lblotherdetail.Text = lblquestionname.Text + " - " + lblotherdetail.Text;
                    lblotherdetail.Visible = true;
                }
            }
        }
    }

    private bool IsValidMOCExternalApprovalRequired(string supportid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supportid.Equals(""))
            ucError.ErrorMessage = "From Whom Approval required?";

        return (!ucError.IsError);
    }

    private void BindSupportItemRequired()
    {
        string[] alColumns = { "FLDMOCSUPPORTITEMQUESTIONID", "FLDMOCSUPPORTITEMREQUIREDYN", "FLDMOCSUPPORTITEMREQUIREDDETAILS", };
        string[] alCaptions = { "Item", "Required", "Details of Change required?" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , General.GetNullableGuid(ViewState["MOCID"].ToString()));

        gvMOCSupportItemRequired.DataSource = ds;
    }
    protected void gvMOCSupportItemRequired_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadLabel idd = (RadLabel)e.Item.FindControl("lblMOCSupportItemRequiredid");
            if (idd != null)
                ViewState["FLDMOCSUPPORTITEMREQUIREDID"] = idd.Text;

            //GridDataItem item = (GridDataItem)e.Item;
            //string id = item.GetDataKeyValue("FLDMOCSUPPORTITEMREQUIREDID").ToString();
            //ViewState["FLDMOCSUPPORTITEMREQUIREDID"] = item.GetDataKeyValue("FLDMOCSUPPORTITEMREQUIREDID").ToString();

            if (e.CommandName.ToUpper().Equals("HMAPPING"))
            {
                trequipment.Visible = true;
                trDocuments.Visible = false;
                //rwTraining.Visible = false;               
                string script = "function sd(){showDialog('Equipment Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString() + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

            }
            else if (e.CommandName.ToUpper().Equals("HSEQUA"))
            {
                trDocuments.Visible = true;
                trequipment.Visible = false;
                string script = "function sd(){showDialog('HSEQA Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString() + ",HEDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

            }
            else if (e.CommandName.ToUpper().Equals("EDITD"))
            {
                trDocuments.Visible = false;
                trequipment.Visible = false;
                trDescription.Visible = true;

                string script = "function sd(){showDialog('Details of Change'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString() + ",EDITD');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(((RadLabel)e.Item.FindControl("lblMOCSupportItemRequiredid")).Text));
                gvMOCSupportItemRequired.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCSupportItemRequired_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (!IsValidMOCSupportItemRequired(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCSupportItemRequiredid")).Text)
                                                                      , new Guid((ViewState["MOCID"]).ToString())
                                                                      , int.Parse(ddlvessel.SelectedVessel)
                                                                      , Int32.Parse(((RadLabel)e.Item.FindControl("lblhardcode")).Text)
                                                                      , Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                      , (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text)
                                                                      , (((RadTextBox)e.Item.FindControl("txtotherdetailEdit")).Text));

            gvMOCSupportItemRequired.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvMOCSupportItemRequired_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        string mochardcode = "";
        string mochardcode1 = "";
        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.mochardcode(12, "OTH");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            mochardcode = dr["FLDMOCHARDCODE"].ToString();
        }

        DataSet ds1;
        ds1 = PhoenixInspectionMOCTemplate.mochardcode(12, "OMP");

        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            mochardcode1 = dr["FLDMOCHARDCODE"].ToString();
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            RadLabel lblquestionedit = ((RadLabel)e.Item.FindControl("lblquestionnameedit"));
            RadLabel lblquestion = ((RadLabel)e.Item.FindControl("lblquestion"));
            RadLabel lblquestionname = ((RadLabel)e.Item.FindControl("lblquestionname"));
            RadLabel lblquestionid = ((RadLabel)e.Item.FindControl("lblquestionid"));
            RadTextBox txtotherdetailedit = ((RadTextBox)e.Item.FindControl("txtotherdetailedit"));
            RadLabel lblotherdetail = ((RadLabel)e.Item.FindControl("lblotherdetail"));
            LinkButton cmdEquipment = ((LinkButton)e.Item.FindControl("cmdEquipment"));
            LinkButton cmdHSEQUA = ((LinkButton)e.Item.FindControl("cmdHSEQUA"));
            LinkButton cmdEdit = ((LinkButton)e.Item.FindControl("cmdEdit"));
            //ViewState["SUPPORTITEMID"] = lblquestionid.Text;

            if (lblquestionid != null)
            {
                if (lblquestionid.Text == "116")
                {
                    cmdEquipment.Visible = true;
                }
                else if (lblquestionid.Text == "87")
                {
                    cmdHSEQUA.Visible = true;
                }
                else
                {
                    cmdEdit.Visible = true;
                }
            }

            RadLabel lblreqid = (RadLabel)e.Item.FindControl("lblMOCSupportItemRequiredid");

            if (e.Item.IsInEditMode)
            {
                //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                //if (eb != null)
                //{
                //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                //    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCSectionRemarks.aspx?MOCSectionId=12&SUPPORTREQID=" + lblreqid.Text + "');return false;");
                //}
            }
            else
            {
                if ((lblquestionid.Text == mochardcode) || (lblquestionid.Text == mochardcode1))
                {
                    lblquestionname.Visible = false;
                    lblotherdetail.Text = lblquestionname.Text + " - " + lblotherdetail.Text;
                    lblotherdetail.Visible = true;
                }
            }
        }
    }
    private bool IsValidMOCSupportItemRequired(string supportid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supportid.Equals(""))
            ucError.ErrorMessage = "Item Required";

        return (!ucError.IsError);
    }

    private void BindShipboardPersonnelaffected()
    {
        string[] alColumns = { "FLDSBPERSONNELAFFECTEDQUESTIONID", "FLDSBPERSONNELAFFECTEDYN", "FLDSBPERSONNELAFFECTEDDETAILS", };
        string[] alCaptions = { "Dept", "Affected?", "Details of how they will be notified of the change?" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["MOCID"].ToString()));


        gvMOCShipboard.DataSource = ds;
    }
    protected void gvMOCShipboard_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindShipboardPersonnelaffected();
    }
    protected void gvMOCShipboard_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , new Guid(((RadLabel)e.Item.FindControl("lblMOCShipboardid")).Text));
                BindShipboardPersonnelaffected();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCShipboard_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindShipboardPersonnelaffected();
    }
    protected void gvMOCShipboard_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindShipboardPersonnelaffected();
    }
    protected void gvMOCShipboard_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCShipboard(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCRequestForChange.MOCShipboardPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCShipboardid")).Text)
                                                              , new Guid((ViewState["MOCID"]).ToString())
                                                              , int.Parse(ddlvessel.SelectedVessel)
                                                              , Int32.Parse(((RadLabel)e.Item.FindControl("lblhardcode")).Text)
                                                              , Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                              , (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text)
                                                              , (((RadTextBox)e.Item.FindControl("txtotherdetailedit")).Text));
            BindShipboardPersonnelaffected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
    }
    protected void gvMOCShipboard_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        string mochardcode = "";

        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.mochardcode(13, "OTH");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            mochardcode = dr["FLDMOCHARDCODE"].ToString();
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblquestionedit = ((RadLabel)e.Item.FindControl("lblquestionnameedit"));
            RadLabel lblquestion = ((RadLabel)e.Item.FindControl("lblquestion"));
            RadLabel lblquestionname = ((RadLabel)e.Item.FindControl("lblquestionname"));
            RadLabel lblquestionid = ((RadLabel)e.Item.FindControl("lblquestionid"));
            RadTextBox txtotherdetailedit = ((RadTextBox)e.Item.FindControl("txtotherdetailedit"));
            RadLabel lblotherdetail = ((RadLabel)e.Item.FindControl("lblotherdetail"));

            RadLabel lblreqid = (RadLabel)e.Item.FindControl("lblMOCShipboardid");

            if (e.Item.IsInEditMode)
            {
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCSectionRemarks.aspx?MOCSectionId=13&SUPPORTREQID=" + lblreqid.Text + "');return false;");
                }
            }
            else
            {
                if ((lblquestionid.Text == mochardcode))
                {
                    lblquestionname.Visible = false;
                    lblotherdetail.Text = lblquestionname.Text + " - " + lblotherdetail.Text;
                    lblotherdetail.Visible = true;
                }
            }
        }
    }
    private bool IsValidMOCShipboard(string supportid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supportid.Equals("Dummy"))
            ucError.ErrorMessage = "Please enter the Dept";

        return (!ucError.IsError);
    }

    private void BindShoreBasedPersonnelaffected()
    {
        string[] alColumns = { "FLDSBPERSONNELAFFECTEDQUESTIONID", "FLDSBPERSONNELAFFECTEDYN", "FLDSBPERSONNELAFFECTEDDETAILS", };
        string[] alCaptions = { "Dept / Entity", "Affected?", "Details of how they will be notified of the change?" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["MOCID"].ToString()));

        gvMOCShoreBased.DataSource = ds;
    }
    protected void gvMOCShoreBased_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindShoreBasedPersonnelaffected();
    }
    protected void gvMOCShoreBased_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , new Guid(((RadLabel)e.Item.FindControl("lblMOCShoreBasedid")).Text));
                BindShoreBasedPersonnelaffected();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCShoreBased_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindShoreBasedPersonnelaffected();
    }
    protected void gvMOCShoreBased_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindShoreBasedPersonnelaffected();
    }
    protected void gvMOCShoreBased_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCShoreBasedPersonnelAffected(((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCRequestForChange.MOCShoreBasedPersonnelAffectedInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCShoreBasedid")).Text)
                                                                            , new Guid((ViewState["MOCID"]).ToString())
                                                                            , int.Parse(ddlvessel.SelectedVessel)
                                                                            , Int32.Parse(((RadLabel)e.Item.FindControl("lblhardcode")).Text)
                                                                            , Int32.Parse((((RadCheckBox)e.Item.FindControl("chkRequiredYNEdit")).Checked == true ? "1" : "0").ToString())
                                                                            , (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text)
                                                                            , (((RadTextBox)e.Item.FindControl("txtotherdetailedit")).Text));

            BindShoreBasedPersonnelaffected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvMOCShoreBased_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        string mochardcode = "";

        DataSet ds;
        ds = PhoenixInspectionMOCTemplate.mochardcode(14, "OTH");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            mochardcode = dr["FLDMOCHARDCODE"].ToString();
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblquestionedit = ((RadLabel)e.Item.FindControl("lblquestionnameedit"));
            RadLabel lblquestion = ((RadLabel)e.Item.FindControl("lblquestion"));
            RadLabel lblquestionname = ((RadLabel)e.Item.FindControl("lblquestionname"));
            RadLabel lblquestionid = ((RadLabel)e.Item.FindControl("lblquestionid"));
            RadTextBox txtotherdetailedit = ((RadTextBox)e.Item.FindControl("txtotherdetailedit"));
            RadLabel lblotherdetail = ((RadLabel)e.Item.FindControl("lblotherdetail"));

            RadLabel lblreqid = (RadLabel)e.Item.FindControl("lblMOCShoreBasedid");

            if (e.Item.IsInEditMode)
            {
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCSectionRemarks.aspx?MOCSectionId=14&SUPPORTREQID=" + lblreqid.Text + "');return false;");
                }
            }
            else
            {
                if ((lblquestionid.Text == mochardcode))
                {
                    lblquestionname.Visible = false;
                    lblotherdetail.Text = lblquestionname.Text + " - " + lblotherdetail.Text;
                    lblotherdetail.Visible = true;
                }
            }
        }
    }
    private bool IsValidMOCShoreBasedPersonnelAffected(string shorebasedid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shorebasedid.Equals(""))
            ucError.ErrorMessage = "Dept/Entity Required";

        return (!ucError.IsError);
    }

    private void BindTrainingRequired()
    {
        string[] alColumns = { "FLDTRAININGTYPE", "FLDTRAININGREQUIRED", "FLDDEPARTMENTORPERSONTOBETRAINED", "PIC", "FLDTARGRTDATE" };
        string[] alCaptions = { "Code", "Training Required", "Dept./Persons to be trained", "PIC", "Target date" };

        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , new Guid((ViewState["MOCID"]).ToString()));

        gvMOCTrainingRequired.DataSource = ds;
    }
    protected void gvMOCTrainingRequired_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindTrainingRequired();
        ViewState["MOCHardEdit"] = "";
    }
    protected void gvMOCTrainingRequired_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Response.Redirect("../Inspection/InspectionMOCTrainingAdd.aspx?&TrainingID=" + ((RadLabel)e.Item.FindControl("lblMOCTrainingRequiredid")).Text + "&VESSELID=" + ViewState["Vesselid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , new Guid(((RadLabel)e.Item.FindControl("lblMOCTrainingRequiredid")).Text));
                BindTrainingRequired();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCTrainingRequired_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindTrainingRequired();
    }
    protected void gvMOCTrainingRequired_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindTrainingRequired();
    }

    protected void gvMOCTrainingRequired_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

        }

    }

    public void ddltraining_SelectedIndedChanged(object sender, EventArgs e)
    {
        gvMOCTrainingRequired.Rebind();
    }

    private bool IsValidMOCTrainingRequired(string TrainingRequired, string Department, string Targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (TrainingRequired.Equals(""))
            ucError.ErrorMessage = "Training required";

        if (Department.Equals(""))
            ucError.ErrorMessage = "Dept./Persons to be trained is Required";

        if (General.GetNullableDateTime(Targetdate) == null)
            ucError.ErrorMessage = "Target date Required";

        return (!ucError.IsError);
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvMOCSupportRequired.Rebind();
        gvMOCShipboard.Rebind();
        gvMOCShoreBased.Rebind();
        gvMOCSupportItemRequired.Rebind();
        gvMOCExternalApproval.Rebind();
        gvMOCTrainingRequired.Rebind();
        BindMOC();
        RiskAssessmentGenericEdit();
    }
    private void MOCRiskassessmentvisble()
    {
        //if (txtRAId.Text != "")
        //{
        //    lblRAType.Visible = false;
        //    lnkCreateRA.Visible = false;
        //    ImgRA.Visible = true;
        //}

        if (txtRA.Text == "")
        {
            lnkCreateRA.Text = "Create Risk Assessment";
        }
        else
        {

            lnkCreateRA.Text = "Edit/View";
            ImgRA.Visible = true;
        }
    }
    protected void EditVessel()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["Vesselid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtClassName.Text = dr["FLDCLASSNAMEVALUE"].ToString();
            txtVesselType.Text = dr["FLDVESSELTYPE"].ToString();
        }
    }
    public void ucMOCSupportAdd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCSupportEdit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCexternalAdd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCexternalEdit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCsupportitemAdd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCsupportitemEdit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ucMOCShipboardAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void ucMOCShipboardEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void ucMOCShoreBasedAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void ucMOCShoreBasedEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void chkIntermediateRequired_OnCheckedChanged(object sender, EventArgs e)
    {
    }

    protected void imgShowRA_Click(object sender, EventArgs e)
    {
        string script = string.Format("javascript:showPickList('spnRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListGenericRACopyForMOC.aspx?catid=1&vesselid=" + ddlvessel.SelectedValue + "&MOCID=" + ViewState["MOCID"].ToString() + "', true);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void cmdRA_Click(object sender, ImageClickEventArgs e)
    {
        if (txtRANumber.Text != "")
        {
            ImgRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERICNEW&genericid=" + ViewState["RAGENERICID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
            //string scriptpopup = string.Format("javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + txtRAId.Text + "&showmenu=0&showexcel=NO');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
        }
    }
    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        //string supportid = gvMOCSupportItemRequired.MasterTableView.Items[0].GetDataKeyValue("FLDMOCSUPPORTITEMREQUIREDID").ToString();
        //ViewState["FLDMOCSUPPORTITEMREQUIREDID"] = supportid;


        if (General.GetNullableGuid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()) != null)
        {
            PhoenixInspectionMOCRequestForChange.SupportRequiredComponentUpdate(new Guid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()), new Guid(txtComponentId.Text));

            ucStatus.Text = "Component added.";
            txtComponentId.Text = "";
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            BindComponents();
        }
    }

    protected void BindComponents()
    {

        DataSet dss = PhoenixInspectionMOCRequestForChange.SupportRequiredComponentList(General.GetNullableGuid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()));
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
    }

    void com_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionMOCRequestForChange.SupportRequiredComponentDelete(new Guid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()), new Guid(c.ID));
            ucStatus.Text = "Deleted.";
            BindComponents();
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
            trequipment.Visible = true;
            trDocuments.Visible = false;
            trDescription.Visible = true;
            BindComponents();
            BindSupportItem();
        }
        if (cmd.ToUpper() == "HEDIT")
        {
            trequipment.Visible = false;
            trDocuments.Visible = true;
            trDescription.Visible = true;
            BindHSEQA();
            BindSupportItem();
        }
        if (cmd.ToUpper() == "EDITD")
        {
            trDescription.Visible = true;
            BindSupportItem();
        }
    }

    protected void BindSupportItem()
    {
        DataSet ds = PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDescriptionSupport.Text = dr["FLDMOCSUPPORTITEMREQUIREDDETAILS"].ToString();
            txtSectionName.Text = dr["FLDSUPPORTREQUIREDNAME"].ToString();
        }
    }
    protected void BindHSEQA()
    {
        DataSet dss = PhoenixInspectionMOCRequestForChange.SupportRequiredDocumentList(General.GetNullableGuid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblDocuments.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(doc_CheckedChanged);
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
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
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
                tblDocuments.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblDocuments.Rows.Add(tr);
                number = number + 1;
            }
            divHSEQUA.Visible = true;
        }
        //else
        //    divHSEQUA.Visible = false;
    }

    protected void lnkDocumentAdd_Click(object sender, EventArgs e)
    {
        string supportid = gvMOCSupportItemRequired.MasterTableView.Items[0].GetDataKeyValue("FLDMOCSUPPORTITEMREQUIREDID").ToString();
        ViewState["FLDMOCSUPPORTITEMREQUIREDID"] = supportid;

        if (General.GetNullableGuid(supportid) != null)
        {
            PhoenixInspectionMOCRequestForChange.SupportRequiredDocumentUpdate(new Guid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()), new Guid(txtDocumentId.Text));

            ucStatus.Text = "Documents/Forms added.";
            txtDocumentId.Text = "";
            txtDocumentname.Text = "";
            BindHSEQA();
        }
    }

    void doc_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionMOCRequestForChange.SupportRequiredDocumentDelete(new Guid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindHSEQA();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                PhoenixInspectionMOCRequestForChange.MOCSupportItemRequiredUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCID"].ToString()), new Guid(ViewState["FLDMOCSUPPORTITEMREQUIREDID"].ToString()), txtDescriptionSupport.Text);

                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

                gvMOCSupportItemRequired.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                return;
            }
        }
    }
    protected void lnkCreateRA_Click(object sender, EventArgs e)
    {


    }
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidMOCRequest(ddlCategory.SelectedValue
                                    , ddlSubCategory.SelectedValue
                                    , ddlNatureofChange.SelectedValue
                                    , ucTargetDateofimplementation.Text
                                    , ucDateofrestoration.Text
                                    , txtDetailsoftheProposedChange.Text
                                    , txtOptionsconsidered.Text
                                    , txtJustificationforchange.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionMOCTemplate.MOCProposalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , General.GetNullableGuid((ViewState["MOCRequestid"]).ToString())
                                                        , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                        , int.Parse(ddlvessel.SelectedVessel)
                                                        , General.GetNullableGuid(ddlCategory.SelectedValue)
                                                        , General.GetNullableGuid(ddlSubCategory.SelectedValue)
                                                        , General.GetNullableInteger(ddlNatureofChange.SelectedValue)
                                                        , General.GetNullableDateTime(ucTargetDateofimplementation.Text)
                                                        , General.GetNullableDateTime(ucDateofrestoration.Text)
                                                        , General.GetNullableString(txtDetailsoftheProposedChange.Text)
                                                        , General.GetNullableString(txtOptionsconsidered.Text)
                                                        , General.GetNullableString(txtJustificationforchange.Text)
                                                        , General.GetNullableInteger(ddlstatus.Text));

            BindMOC();
            BindMOCRequest();
            ucStatus.Text = "Request for Change updated successfully.";
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void imgrevision_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowManager1.RadConfirm("Are you sure you want to revise this RA?", "ConfirmRevision", 320, 150, null, "ConfirmRevision");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionRiskAssessmentGenericExtn.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                           new Guid(ViewState["RAGENERICID"].ToString()));

            PhoenixInspectionMOCExtention.MOCRARevision(General.GetNullableGuid(ViewState["MOCID"].ToString()));

            BindMOC();
            RiskAssessmentGenericEdit();
            BindMOCRequest();
            BindMOCCategory();
            ucStatus.Text = "RA Revised Successfully";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}



