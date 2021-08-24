using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewNewApplicantAppraisalActivity : PhoenixBasePage
{
    int ConductCount, AttitudeCount, LeadershipCount, JudgementCount, ProAppraisalCount;
    int ConductFilledCount, AttitudeFilledCount, LeadershipFilledCount, JudgementFilledCount, ProAppraisalFilledCount;
    string canedit = "1", canpostmstcomment = "1";



    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            //Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();

            ViewState["Rcategory"] = 0;
            ViewState["Category"] = 0;
            ViewState["APPVESSEL"] = "";

            txtCourseId.Attributes.Add("style", "display:none;");
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SetEmployeePrimaryDetails();
            GetRankCategory();

            ddlOccassion.OccassionList = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()));
            ddlOccassion.Category = ViewState["Rcategory"].ToString();
            ddlOccassion.DataBind();

            DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                canpostmstcomment = ds.Tables[0].Rows[0]["FLDCANPOSTMSTCOMMENT"].ToString();
            }
            EditAppraisal();

            chkRecommendPromotion.Attributes.Add("onclick", "javascript:OpenChkboxPopup(this, " + ViewState["Rankid"].ToString() + ", 1," + ViewState["APPVESSEL"].ToString() + ")");

            //rbNTBR.Attributes.Add("onclick", "javascript: parent.Openpopup('codehelp1', '', '../Crew/CrewNTBR.aspx?empid=" + Filter.CurrentCrewSelection + "'); return true;");
            if (Filter.CurrentCrewLaunchedFrom == null || Filter.CurrentCrewLaunchedFrom.ToString() == "")
                chkTrainingRequired.Attributes.Add("onclick", "javascript:OpenChkboxPopup(this, " + ViewState["Rankid"].ToString() + ", 0," + ViewState["APPVESSEL"].ToString() + ", " + Filter.CurrentNewApplicantSelection + ")");

            imgShowCourse.Attributes.Add("onclick", "return showPickList('spnCourse', 'codehelp1', '', '../Common/CommonPickListCourse.aspx?rankid=" + ViewState["Rankid"].ToString() + "&vessel=" + ViewState["APPVESSEL"].ToString() + "&empid=" + Filter.CurrentNewApplicantSelection + "', 'yes'); return true");

            imgBtnPromotionDtls.Attributes.Add("onclick", "return showPickList('spnPromotion', 'codehelp1', '', '../Crew/CrewAppraisalPromotion.aspx?rankid=" + ViewState["Rankid"].ToString() + "', 'yes'); return true");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                imgShowOffshoreTraining.Visible = true;
                spnCourse.Visible = false;
            }

            imgShowOffshoreTraining.Attributes.Add("onclick", "return parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid="
                + Filter.CurrentNewApplicantSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection + "&vesselid="
                + ViewState["APPVESSEL"].ToString() + "');return false;");

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                chkAttachedCopyYN.Enabled = false;
            ddlVessel.VesselList = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");

        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (canedit.Equals("1"))
        {
            if (Filter.CurrentAppraisalSelection != null)
            {
                toolbarmain.AddButton("Finalise", "CONFIRM", ToolBarDirection.Right);
                toolbarmain.AddButton("Save Changes", "SAVECHANGES", ToolBarDirection.Right);

                divOtherSection.Visible = true;
                divPrimarySection.Visible = true;
            }
            else
            {
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                divOtherSection.Visible = false;
                divPrimarySection.Visible = true;
            }
            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbarmain.Show();
        }
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Appraisal", "APPRAISAL");
        toolbarmain.AddButton("Form", "FORM");
        if (Filter.CurrentAppraisalSelection != null)
        {
            toolbarmain.AddButton("Seafarer  Comment", "SEAMANCOMMENT");
            toolbarmain.AddButton("Appraisal Report", "APPRAISALREPORT");
            toolbarmain.AddButton("Promotion Report", "PROMOTION");


        }
        AppraisalTabs.AccessRights = this.ViewState;
        AppraisalTabs.MenuList = toolbarmain.Show();
        AppraisalTabs.SelectedMenuIndex = 1;
        BindData();
        BindConductData();
        EnablePage();
    }

    private void EnableControls()
    {
        if (rbNTBR.Checked)
        {
            txtNtbrRemarks.Enabled = true;
            txtNtbrRemarks.CssClass = "input_mandatory";
        }
        else
        {
            txtNtbrRemarks.Enabled = false;
            txtNtbrRemarks.Text = "";
            txtNtbrRemarks.CssClass = "readonlytextbox";
        }

        if (rbWarningToBeGiven.Checked)
        {
            txtWarningRemarks.Enabled = true;
            txtWarningRemarks.CssClass = "input_mandatory";
        }
        else
        {
            txtWarningRemarks.Enabled = false;
            txtWarningRemarks.Text = "";
            txtWarningRemarks.CssClass = "readonlytextbox";
        }
        if (rbEnvironment.Checked)
        {
            txtEnvironment.Enabled = true;
            txtEnvironment.CssClass = "input_mandatory";
        }
        else
        {
            txtEnvironment.Enabled = false;
            txtEnvironment.Text = "";
            txtEnvironment.CssClass = "readonlytextbox";
        }

        if (chkRecommendPromotion.Checked)
            imgBtnPromotionDtls.Visible = true;
        else
            imgBtnPromotionDtls.Visible = false;
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        if (Filter.CurrentPickListSelection.Keys[1].ToString() == "txtCourseName")
            txtCourseName.Text = Filter.CurrentPickListSelection.Get(1);

        if (Filter.CurrentPickListSelection.Keys[2].ToString() == "txtCourseId")
            txtCourseId.Text = Filter.CurrentPickListSelection.Get(2);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditAppraisal();
    }

    protected void AppraisalTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("APPRAISAL"))
            {
                if (Request.QueryString["vslid"] != null)
                    Response.Redirect("CrewNewApplicantAppraisal.aspx?vslid=" + Request.QueryString["vslid"], false);
                else
                    Response.Redirect("CrewNewApplicantAppraisal.aspx", false);
            }

            if (CommandName.ToUpper().Equals("SEAMANCOMMENT"))
            {
                Response.Redirect("CrewNewApplicantAppraisalSeamanComment.aspx", false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=NEWAPPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
            if (CommandName.ToUpper().Equals("PROMOTION"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=NEWPROMOTION&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string vessel = ddlVessel.SelectedVessel;
            string fromdate = txtFromDate.Text;
            string todate = txtToDate.Text;
            string appraisaldate = txtdate.Text;
            string occassion = ddlOccassion.SelectedOccassion;

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SAVECHANGES"))
            {
                string iAppraisalId = "";

                if (!IsValidateAppraisal(vessel, fromdate, todate, occassion, appraisaldate))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentAppraisalSelection == null)
                {
                    PhoenixCrewAppraisal.InsertAppraisal(
                                                    int.Parse(Filter.CurrentNewApplicantSelection)
                                                   , DateTime.Parse(fromdate)
                                                   , DateTime.Parse(todate)
                                                   , int.Parse(vessel)
                                                   , General.GetNullableDateTime(appraisaldate)
                                                   , int.Parse(ddlOccassion.SelectedOccassion)
                                                   , ref iAppraisalId
                                                   , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0")
                                                   );

                    Filter.CurrentAppraisalSelection = iAppraisalId.ToString();
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                                , DateTime.Parse(fromdate)
                                                , DateTime.Parse(todate)
                                                , int.Parse(vessel)
                                                , General.GetNullableDateTime(appraisaldate)
                                                , int.Parse(occassion)
                                                , null
                                                , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0")
                                    );
                }

                if (!chkAttachedCopyYN.Checked)
                {
                    if (!IsValidAppraisal())
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                            new Guid(Filter.CurrentAppraisalSelection)
                            , chkRecommendPromotion.Checked ? 1 : 0
                            , chkExposedToDuties.Checked ? 1 : 0
                            , chkTrainingRequired.Checked ? 1 : 0
                            , txtCourseId.Text
                            , rbReemployment.Checked ? 1 : 0
                            , rbWarningToBeGiven.Checked ? 1 : 0
                            , rbNTBR.Checked ? 1 : 0
                            , 0
                            , rbNTBR.Checked ? (txtNtbrRemarks.Text.Equals("") ? null : txtNtbrRemarks.Text) : null
                            , rbWarningToBeGiven.Checked ? (txtWarningRemarks.Text.Equals("") ? null : txtWarningRemarks.Text) : null
                            , txtHeadOfDeptComment.Text
                            , txtMasteComment.Text
                            , null
                            , rbEnvironment.Checked ? 1 : 0
                            , rbEnvironment.Checked ? General.GetNullableString(txtEnvironment.Text) : null
                            , General.GetNullableString(hdnSeamen.Value)
                            , null
                            );


                ucStatus.Text = "Appraisal Information updated.";
                EditAppraisal();
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (!chkAttachedCopyYN.Checked)
                {
                    if (!IsValidAppraisalWithRatings())
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                              , DateTime.Parse(fromdate)
                                              , DateTime.Parse(todate)
                                              , int.Parse(vessel)
                                              , General.GetNullableDateTime(appraisaldate)
                                              , int.Parse(occassion)
                                              , 0
                                              , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0")
                                  );

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                            new Guid(Filter.CurrentAppraisalSelection)
                            , chkRecommendPromotion.Checked ? 1 : 0
                            , chkExposedToDuties.Checked ? 1 : 0
                            , chkTrainingRequired.Checked ? 1 : 0
                            , txtCourseId.Text
                            , rbReemployment.Checked ? 1 : 0
                            , rbWarningToBeGiven.Checked ? 1 : 0
                            , rbNTBR.Checked ? 1 : 0
                            , 0
                            , rbNTBR.Checked ? (txtNtbrRemarks.Text.Equals("") ? null : txtNtbrRemarks.Text) : null
                            , rbWarningToBeGiven.Checked ? (txtWarningRemarks.Text.Equals("") ? null : txtWarningRemarks.Text) : null
                            , txtHeadOfDeptComment.Text
                            , txtMasteComment.Text
                            , null
                            , rbEnvironment.Checked ? 1 : 0
                            , rbEnvironment.Checked ? General.GetNullableString(txtEnvironment.Text) : null
                            , General.GetNullableString(hdnSeamen.Value)
                            , null);


                ucStatus.Text = "This appraisal is finalised.";
                Response.Redirect(Request.RawUrl);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAppraisal()
    {
        ucError.HeaderMessage = "Please provide the following required information.";
        int i = 0;

        if (rbReemployment.Checked) i++;
        if (rbNTBR.Checked) i++;
        if (rbWarningToBeGiven.Checked) i++;

        if (i == 0)
            ucError.ErrorMessage = "In Recommendations section either one has to be selected.";

        if (i > 1)
            ucError.ErrorMessage = "In Recommendations section only one option can be select.";

        if (rbNTBR.Checked && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        return (!ucError.IsError);
    }

    private bool IsValidAppraisalWithRatings()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (rbNTBR.Checked && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        if (ConductCount != ConductFilledCount || AttitudeCount != AttitudeFilledCount || LeadershipCount != LeadershipFilledCount || JudgementCount != JudgementFilledCount)
        {
            ucError.ErrorMessage = "Section 1 has to be filled before confirming the Appraisal";
        }
        if (ProAppraisalCount != ProAppraisalFilledCount)
        {
            ucError.ErrorMessage = "Section 2 has to be filled before confirming the Appraisal";
        }
        if (!rbReemployment.Checked && !rbWarningToBeGiven.Checked && !rbNTBR.Checked)
        {
            ucError.ErrorMessage = "In Recommendations part either one option has to be selected before confirming the Appraisal";
        }
        if (String.IsNullOrEmpty(hdnSeamen.Value))
        {
            ucError.ErrorMessage = "Seafarer  Comments should be posted before confirming the Appraisal";
        }
        return (!ucError.IsError);
    }

    private void EditAppraisal()
    {
        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                chkRecommendPromotion.Checked = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString().Equals("1") ? true : false;
                chkExposedToDuties.Checked = ds.Tables[0].Rows[0]["FLDEXPOSEDTODUTIES"].ToString().Equals("1") ? true : false;
                chkTrainingRequired.Checked = ds.Tables[0].Rows[0]["FLDTRAININGREQUIRED"].ToString().Equals("1") ? true : false;
                rbReemployment.Checked = ds.Tables[0].Rows[0]["FLDFITFORREEMPLOYMENT"].ToString().Equals("1") ? true : false;
                ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["APPVESSEL"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                guidelines.InnerHtml = ds.Tables[0].Rows[0]["FLDGUIDELINES"].ToString();

                if (ds.Tables[0].Rows[0]["FLDATTACHEDSCANCOPYYN"].ToString().Equals("1"))
                    chkAttachedCopyYN.Checked = true;
                else
                    chkAttachedCopyYN.Checked = false;

                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                {
                    if (!chkTrainingRequired.Checked)
                        imgShowOffshoreTraining.Enabled = false;
                    else
                        imgShowOffshoreTraining.Enabled = true;
                }
                else
                {
                    if (!chkTrainingRequired.Checked)
                        imgShowCourse.Enabled = false;
                    else
                        imgShowCourse.Enabled = true;
                }

                if (ds.Tables[0].Rows[0]["FLDWARNINGTOBEGIVEN"].ToString().Equals("1"))
                {
                    rbWarningToBeGiven.Checked = true;
                    txtWarningRemarks.Enabled = true;
                    txtWarningRemarks.CssClass = "input_mandatory";
                }
                else
                {
                    rbWarningToBeGiven.Checked = false;
                    txtWarningRemarks.Enabled = false;
                    txtWarningRemarks.Text = "";
                    txtWarningRemarks.CssClass = "readonlytextbox";
                }

                if (ds.Tables[0].Rows[0]["FLDNTBR"].ToString().Equals("1"))
                {
                    rbNTBR.Checked = true;
                    txtNtbrRemarks.Enabled = true;
                    txtNtbrRemarks.CssClass = "input_mandatory";
                }
                else
                {
                    rbNTBR.Checked = false;
                    txtNtbrRemarks.Enabled = false;
                    txtNtbrRemarks.Text = "";
                    txtNtbrRemarks.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDENVNONCOMPILANCE"].ToString().Equals("1"))
                {
                    rbEnvironment.Checked = true;
                    txtEnvironment.Enabled = true;
                    txtEnvironment.CssClass = "input_mandatory";
                }
                else
                {
                    rbEnvironment.Checked = false;
                    txtEnvironment.Enabled = false;
                    txtEnvironment.Text = "";
                    txtEnvironment.CssClass = "readonlytextbox";
                }


                txtHeadOfDeptComment.Text = ds.Tables[0].Rows[0]["FLDHEADDEPTCOMMENT"].ToString();
                txtMasteComment.Text = ds.Tables[0].Rows[0]["FLDMASTERCOMMENT"].ToString();
                hdnSeamen.Value = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();
                txtNtbrRemarks.Text = ds.Tables[0].Rows[0]["FLDNTBRREMARKS"].ToString();
                txtWarningRemarks.Text = ds.Tables[0].Rows[0]["FLDWARNINGREMARKS"].ToString();
                txtCourseId.Text = ds.Tables[0].Rows[0]["FLDCOURSEREQUIRED"].ToString();
                txtCourseName.Text = ds.Tables[0].Rows[0]["FLDCOURSENAME"].ToString();
                txtEnvironment.Text = ds.Tables[0].Rows[0]["FLDENVNONCOMPREMARKS"].ToString();
                txtSeafarerComment.Text = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();

                if (chkRecommendPromotion.Checked)
                    imgBtnPromotionDtls.Visible = true;
                else
                    imgBtnPromotionDtls.Visible = false;
            }
        }
        else
        {
            imgBtnPromotionDtls.Visible = false;
        }

        if (ViewState["Rcategory"] != null && ViewState["Rcategory"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 90, "SOF"))
        {
            DataSet ds1 = PhoenixRegistersRank.EditRank(int.Parse(ViewState["Rankid"].ToString()));

            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows[0]["FLDGROUPRANK"].ToString() == "MASTER" || ds1.Tables[0].Rows[0]["FLDGROUPRANK"].ToString() == "CH ENGINEER")
                {
                    chkRecommendPromotion.Enabled = false;
                    chkExposedToDuties.Enabled = false;

                    chkExposedToDuties.Checked = false;
                    chkRecommendPromotion.Checked = false;
                    imgBtnPromotionDtls.Visible = false;
                }
            }
        }
    }

    private void EnablePage()
    {
        bool editable = canedit.Equals("0") ? false : true; //Enable or disable all controls
        ddlVessel.Enabled = editable;
        txtFromDate.Enabled = editable;
        txtToDate.Enabled = editable;
        txtdate.Enabled = editable;
        ddlOccassion.Enabled = editable;
        if (chkRecommendPromotion.Enabled)
            chkRecommendPromotion.Enabled = editable;
        chkExposedToDuties.Enabled = editable;
        chkTrainingRequired.Enabled = editable;
        if (!imgShowCourse.Enabled)
            imgShowCourse.Enabled = !editable;
        rbReemployment.Enabled = editable;
        rbWarningToBeGiven.Enabled = editable;
        txtWarningRemarks.Enabled = editable;
        rbEnvironment.Enabled = editable;
        txtEnvironment.Enabled = editable;
        rbNTBR.Enabled = editable;
        txtNtbrRemarks.Enabled = editable;
        txtHeadOfDeptComment.Enabled = editable;
        txtMasteComment.Enabled = editable;
        if (editable)
            EnableControls();
        txtMasteComment.Enabled = canpostmstcomment.Equals("0") ? false : true;
    }

    public void GetRankCategory()
    {
        string Rcategory = null;

        PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["Rankid"].ToString()), ref Rcategory);

        if (Rcategory == System.DBNull.Value.ToString())
            Rcategory = "0";

        ViewState["Rcategory"] = Rcategory.ToString();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();

                if (Filter.CurrentAppraisalSelection == null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANK"].ToString();
                    guidelines.InnerHtml = dt.Rows[0]["FLDGUIDELINES"].ToString();
                }
            }

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlVessel.SelectedValue = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
                    //txtxEmployeeName.Text           = ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString();
                    //txtRank.Text                    = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
                    txtFromDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString());
                    txtToDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
                    ddlOccassion.SelectedOccassion = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();
                    txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                    ViewState["Rankid"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                               General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                               , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "CDT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ConductCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
                // gvCrewProfileAppraisalConduct.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ConductCount = 0;
                gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "ATT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                AttitudeCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalAttitude.DataSource = ds.Tables[0];
                // gvCrewProfileAppraisalAttitude.DataBind();
            }
            else
            {
                AttitudeCount = 0;
                DataTable dt = ds.Tables[0];
                gvCrewProfileAppraisalAttitude.DataSource = ds.Tables[0];
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "JCS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                JudgementCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalJudgementCommonSense.DataSource = ds.Tables[0];
                //gvCrewProfileAppraisalJudgementCommonSense.DataBind();
            }
            else
            {
                JudgementCount = 0;
                DataTable dt = ds.Tables[0];
                gvCrewProfileAppraisalJudgementCommonSense.DataSource = ds.Tables[0];
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "LDS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                LeadershipCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
                //gvCrewProfileAppraisalLeadership.DataBind();
            }
            else
            {
                LeadershipCount = 0;
                DataTable dt = ds.Tables[0];
                gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int profileCount;
    int ratingtotal = 0;

    void RecommendationSelection(CheckBox chk)
    {
        switch (chk.ID.ToString())
        {
            case "rbWarningToBeGiven":
                rbReemployment.Checked = false;
                rbNTBR.Checked = false;
                txtNtbrRemarks.Text = "";
                txtNtbrRemarks.Enabled = false;
                txtNtbrRemarks.CssClass = "readonlytextbox";
                break;
            case "rbNTBR":
                rbReemployment.Checked = false;
                rbWarningToBeGiven.Checked = false;
                txtWarningRemarks.Text = "";
                txtWarningRemarks.Enabled = false;
                txtWarningRemarks.CssClass = "readonlytextbox";
                break;
            case "rbReemployment":
                rbNTBR.Checked = false;
                txtNtbrRemarks.Text = "";
                txtNtbrRemarks.Enabled = false;
                txtNtbrRemarks.CssClass = "readonlytextbox";
                rbWarningToBeGiven.Checked = false;
                txtWarningRemarks.Text = "";
                txtWarningRemarks.Enabled = false;
                txtWarningRemarks.CssClass = "readonlytextbox";
                break;
        }
    }

    private void ChangeEditMode(GridView gv)
    {
        switch (gv.ID.ToString())
        {
            case "gvCrewProfileAppraisalLeadership":
                //gvCrewProfileAppraisalJudgementCommonSense.SelectedIndex = -1;
                //gvCrewProfileAppraisalJudgementCommonSense.EditIndex = -1;
                //gvCrewProfileAppraisalAttitude.SelectedIndex = -1;
                //gvCrewProfileAppraisalAttitude.EditIndex = -1;
                //gvCrewProfileAppraisalConduct.SelectedIndex = -1;
                //gvCrewProfileAppraisalConduct.EditIndex = -1;
                break;
            case "gvCrewProfileAppraisalJudgementCommonSense":
                //gvCrewProfileAppraisalLeadership.SelectedIndex = -1;
                //gvCrewProfileAppraisalLeadership.EditIndex = -1;
                //gvCrewProfileAppraisalAttitude.SelectedIndex = -1;
                //gvCrewProfileAppraisalAttitude.EditIndex = -1;
                //gvCrewProfileAppraisalConduct.SelectedIndex = -1;
                //gvCrewProfileAppraisalConduct.EditIndex = -1;
                break;
            case "gvCrewProfileAppraisalAttitude":
                //gvCrewProfileAppraisalLeadership.SelectedIndex = -1;
                //gvCrewProfileAppraisalLeadership.EditIndex = -1;
                ////gvCrewProfileAppraisalConduct.SelectedIndex = -1;
                ////gvCrewProfileAppraisalConduct.EditIndex = -1;
                //gvCrewProfileAppraisalJudgementCommonSense.SelectedIndex = -1;
                //gvCrewProfileAppraisalJudgementCommonSense.EditIndex = -1;
                break;
            case "gvCrewProfileAppraisalConduct":
                //gvCrewProfileAppraisalLeadership.SelectedIndex = -1;
                //gvCrewProfileAppraisalLeadership.EditIndex = -1;
                //gvCrewProfileAppraisalAttitude.SelectedIndex = -1;
                //gvCrewProfileAppraisalAttitude.EditIndex = -1;
                //gvCrewProfileAppraisalJudgementCommonSense.SelectedIndex = -1;
                //gvCrewProfileAppraisalJudgementCommonSense.EditIndex = -1;
                break;
        }
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvCrewProfileAppraisal_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixCrewAppraisalProfile.DeleteAppraisalProfile(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalProfileId")).Text));

    //            BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}




    private bool IsValidAppraisalProfile(string Rating, string Remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Rating.Trim().Equals(""))
            ucError.ErrorMessage = "Rating is required.";

        if (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) < 0)
            ucError.ErrorMessage = "Rating should be between 0 to 10";

        return (!ucError.IsError);
    }


    protected void GvPersonalProfile_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
    }

    /// <summary>
    ///  Code for Professional Conduct Grid
    /// </summary>

    private void BindConductData()
    {
        try
        {
            DataSet ds = PhoenixCrewAappraisalConduct.AppraisalConductSearch(
                      General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                      , int.Parse(ViewState["Rankid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ProAppraisalCount = ds.Tables[0].Rows.Count;
                gvCrewConductAppraisal.DataSource = ds.Tables[0];
               
            }
            else
            {
                ProAppraisalCount = 0;
                DataTable dt = ds.Tables[0];
                gvCrewConductAppraisal.DataSource = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

       

    int ProCount;


   
   
    private bool IsValidAppraisalConduct(string Rating, string Remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvCrewConductAppraisal;

        if (!Rating.Trim().Equals("") && (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) < 0))
            ucError.ErrorMessage = "Rating should be between 0 to 10";

        return (!ucError.IsError);
    }

    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'To Date' should be later than 'From Date'";
        }
        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'From date' should not be future date.";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date.";
        }
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out resultdate))
            ucError.ErrorMessage = "Appraisal Date is required";
        else if (!string.IsNullOrEmpty(todate)
              && DateTime.TryParse(appraisaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal Date' should be later than or equal to 'To Date'";
        }
        if (!string.IsNullOrEmpty(appraisaldate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(appraisaldate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should not be future date.";
        }

        return (!ucError.IsError);
    }

    protected void chkTrainingRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
        {
            if (!chkTrainingRequired.Checked)
            {
                txtCourseId.Text = String.Empty;
                txtCourseName.Text = String.Empty;
                Filter.CurrentPickListSelection = null;
                imgShowOffshoreTraining.Enabled = false;
            }
            else
            {
                imgShowOffshoreTraining.Enabled = true;
            }
        }
        else
        {
            if (!chkTrainingRequired.Checked)
            {
                txtCourseId.Text = String.Empty;
                txtCourseName.Text = String.Empty;
                imgShowCourse.Enabled = false;
            }
            else
            {
                imgShowCourse.Enabled = true;
            }
        }
    }

    protected void gvCrewConductAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            if (canedit.Equals("1") && SessionUtil.CanAccess(this.ViewState, "EDIT"))
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewConductAppraisal, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    protected void Recommendations_Changed(object sender, EventArgs e)
    {
        CheckBox chkBx = (CheckBox)sender;
        if (chkBx.Checked)
            RecommendationSelection(chkBx);
    }


    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected void gvCrewProfileAppraisalConduct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewProfileAppraisalConduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;

        if (e.Item is GridDataItem)
        {
            RadLabel lbRate = (RadLabel)e.Item.FindControl("lblRating");
            if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
            {
                profileCount += 1;
                ratingtotal += int.Parse(lbRate.Text == "" ? "0" : lbRate.Text);
            }
            else
            {
                UserControlNumber rating = (UserControlNumber)e.Item.FindControl("ucRatingEdit");
                if (rating != null && !String.IsNullOrEmpty(rating.Text))
                {
                    ratingtotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
                }
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblFooterTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
            int RatingTotalValue = 0;
            RatingTotalValue = (_gridView.MasterTableView.Items.Count) * 10;
            if (lblFooterTotal != null)
            {
                if (_gridView.MasterTableView.Items.Count <= 0)
                    lblFooterTotal.Text = "0" + " / " + "0";
                else
                    lblFooterTotal.Text = ratingtotal.ToString() + " / " + RatingTotalValue.ToString();
                ratingtotal = 0;
            }
            switch (_gridView.ID.ToString())
            {
                case "gvCrewProfileAppraisalConduct":
                    ConductFilledCount = profileCount;
                    break;
                case "gvCrewProfileAppraisalAttitude":
                    AttitudeFilledCount = profileCount;
                    break;
                case "gvCrewProfileAppraisalLeadership":
                    LeadershipFilledCount = profileCount;
                    break;
                case "gvCrewProfileAppraisalJudgementCommonSense":
                    JudgementFilledCount = profileCount;
                    break;
            }
        }
    }

    protected void gvCrewProfileAppraisalConduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {

                if (Filter.CurrentAppraisalSelection == null)
                {
                    ucError.HeaderMessage = "Please provide the Primary Details information";
                    ucError.ErrorMessage = "<br/>Primary Details has to be filled and saved before enter rating's";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidAppraisalProfile(
                        ((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text,
                        General.GetNullableString("")))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!String.IsNullOrEmpty(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_')))
                {
                    if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAppraisalProfileId")).Text) == null)
                    {
                        PhoenixCrewAppraisalProfile.InsertAppraisalProfile(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(Filter.CurrentAppraisalSelection)
                            , int.Parse(((RadLabel)e.Item.FindControl("lblProfileCategoryId")).Text)
                            , int.Parse(((RadLabel)e.Item.FindControl("lblProfileQuestionId")).Text)
                            , int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_'))
                            , General.GetNullableString(""));
                    }
                    else
                    {
                        PhoenixCrewAppraisalProfile.UpdateAppraisalProfile(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(((RadLabel)e.Item.FindControl("lblAppraisalProfileId")).Text)
                            , int.Parse(((RadLabel)e.Item.FindControl("lblProfileCategoryId")).Text)
                            , int.Parse(((RadLabel)e.Item.FindControl("lblProfileQuestionId")).Text)
                            , int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_'))
                            , General.GetNullableString(""));
                    }
                }


                BindData();
                gvCrewProfileAppraisalConduct.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewConductAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindConductData();
    }

    protected void gvCrewConductAppraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            RadGrid _gridView = (RadGrid)sender;
           

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAppraisalConductId")).Text) != null)
                {
                    PhoenixCrewAappraisalConduct.DeleteAppraisalConduct(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((RadLabel)e.Item.FindControl("lblAppraisalConductId")).Text));
                }
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
              
              
                    if (Filter.CurrentAppraisalSelection == null)
                    {
                        ucError.HeaderMessage = "Please provide the Primary Details information";
                        ucError.ErrorMessage = "<br/>Primary Details has to be filled and saved before enter rating's";
                        ucError.Visible = true;
                        return;
                    }
                    if (!IsValidAppraisalConduct(
                            ((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text,
                            General.GetNullableString("")))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    if (!String.IsNullOrEmpty(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_')))
                    {
                        if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAppraisalConductId")).Text) == null)
                        {
                            PhoenixCrewAappraisalConduct.InsertAppraisalConduct(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(Filter.CurrentAppraisalSelection)
                                , int.Parse(((RadLabel)e.Item.FindControl("lblEvaluationQuestionId")).Text)
                                , int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_'))
                                , General.GetNullableString("")
                                , int.Parse(ViewState["Rankid"].ToString()));
                        }
                        else
                        {
                            PhoenixCrewAappraisalConduct.UpdateAppraisalConduct(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(((RadLabel)e.Item.FindControl("lblAppraisalConductId")).Text)
                                , int.Parse(((RadLabel)e.Item.FindControl("lblEvaluationQuestionId")).Text)
                                , int.Parse(((UserControlNumber)e.Item.FindControl("ucRatingEdit")).Text.TrimStart('_'))
                                , General.GetNullableString(""));
                        }
                    }

                   
                    BindConductData();
                gvCrewConductAppraisal.Rebind();
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewConductAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            RadLabel lbRate = (RadLabel)e.Item.FindControl("lblRating");
            if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
                ProCount += 1;
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit1");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            ProAppraisalFilledCount = ProCount;
        }
    }
}
