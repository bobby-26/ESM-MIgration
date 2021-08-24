using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using System.IO;
using Telerik.Web.UI;
public partial class CrewAppraisalActivity : PhoenixBasePage
{
    int ConductCount, AttitudeCount, LeadershipCount, JudgementCount, ProAppraisalCount;
    int ConductFilledCount, AttitudeFilledCount, LeadershipFilledCount, JudgementFilledCount, ProAppraisalFilledCount;
    string canedit = "1", canpostmstcomment = "1", canposthodcomment = "1";
    string noofvisits;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            //Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();
            //if (Filter.CurrentMenuCodeSelection.Contains("VAC-"))
            //{ Appraisalactivity.ShowMenu = "true"; }
            ViewState["Rcategory"] = 0;
            ViewState["Category"] = 0;
            ViewState["AppraisalStatus"] = 1;
            ViewState["APPVESSEL"] = "";
            ViewState["VSLID"] = "";
            ViewState["SIGNONOFFID"] = "";
            ViewState["SIGNONDATE"] = "";
            ViewState["OCCASIONNAME"] = "";
            ViewState["APPRAISALNEW"] = "";

            txtCourseId.Attributes.Add("style", "display:none;");
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (Request.QueryString["appraisalid"] != null)
                Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();

            if (Request.QueryString["empid"] != null)
                Filter.CurrentCrewSelection = Request.QueryString["empid"].ToString();
            if (Request.QueryString["vslid"] != null)
                ViewState["VSLID"] = Request.QueryString["vslid"].ToString();

            if (Filter.CurrentMenuCodeSelection == "VAC-CRW-APL")
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            SetEmployeePrimaryDetails();
            GetRankCategory();

            DataSet occassionds = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
            ddlOccassion.OccassionList = occassionds;
            ddlOccassion.Category = ViewState["Rcategory"].ToString();
            ddlOccassion.DataBind();

            if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
            {
                foreach (DataRow dr in occassionds.Tables[0].Rows)
                {
                    if (dr["FLDOCCASION"].ToString().Contains("Owner"))
                    {
                        ddlOccassion.SelectedOccassion = dr["FLDOCCASIONID"].ToString();
                        break;
                    }
                }
                ddlOccassion.Enabled = false;
            }

            BindCategoryDropDown();
            DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                canpostmstcomment = ds.Tables[0].Rows[0]["FLDCANPOSTMSTCOMMENT"].ToString();
                canposthodcomment = ds.Tables[0].Rows[0]["FLDCANPOSTSUPCOMMENT"].ToString();

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDAPPRAISALACTIVEYN"].ToString()))
                    hdnappraisalcomplatedyn.Value = ds.Tables[0].Rows[0]["FLDAPPRAISALACTIVEYN"].ToString();
                else
                    hdnappraisalcomplatedyn.Value = "1";
            }


            chkRecommendPromotion.Attributes.Add("onclick", "javascript:OpenChkboxPopup(this, " + ViewState["Rankid"].ToString() + ", 1," + ViewState["APPVESSEL"].ToString() + ")");

            //rbNTBR.Attributes.Add("onclick", "javascript: parent.Openpopup('codehelp1', '', '../Crew/CrewNTBR.aspx?empid=" + Filter.CurrentCrewSelection + "'); return true;");

            if (Filter.CurrentCrewLaunchedFrom == null || Filter.CurrentCrewLaunchedFrom.ToString() == "")
                chkTrainingRequired.Attributes.Add("onclick", "javascript:OpenChkboxPopup(this, " + ViewState["Rankid"].ToString() + ", 0," + ViewState["APPVESSEL"].ToString() + ", " + Filter.CurrentCrewSelection + ")");

            imgShowCourse.Attributes.Add("onclick", "return showPickList('spnCourse', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCourse.aspx?rankid=" + ViewState["Rankid"].ToString() + "&vessel=" + ViewState["APPVESSEL"].ToString() + "&empid=" + Filter.CurrentCrewSelection + "', 'yes'); return true");

            imgBtnPromotionDtls.Attributes.Add("onclick", "return showPickList('spnPromotion', 'codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAppraisalPromotion.aspx?rankid=" + ViewState["Rankid"].ToString() + "', 'yes'); return true");

            imgBtnCourseDone.Attributes.Add("onclick", "return showPickList('spnTraining', 'codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAppraisalTrainingCourseDone.aspx?empid=" + ViewState["empid"].ToString() + "&signonoffid=" + ViewState["SIGNONOFFID"].ToString() + "&signondate=" + ViewState["SIGNONDATE"].ToString() + "', 'yes'); return true");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                imgShowOffshoreTraining.Visible = true;
                //  spnCourse.Visible = false;
            }

            imgShowOffshoreTraining.Attributes.Add("onclick", "return parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid="
                  + Filter.CurrentCrewSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection + "&vesselid="
                  + ViewState["APPVESSEL"].ToString() + "');return false;");

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                chkAttachedCopyYN.Enabled = false;

            EditAppraisal();

        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (canedit.Equals("1"))
        {
            if (Filter.CurrentAppraisalSelection != null)
            {

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    toolbarmain.AddButton("Complete", "CONFIRM", ToolBarDirection.Right);
                toolbarmain.AddButton("Save Changes", "SAVECHANGES", ToolBarDirection.Right);

                divOtherSection.Visible = true;
                divSignondate.Visible = true;
                divPrimarySection.Visible = true;
            }
            else
            {

                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["APPVESSEL"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
                divOtherSection.Visible = false;
                divSignondate.Visible = false;
                divPrimarySection.Visible = true;
            }
            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbarmain.Show();
        }
        if (hdnappraisalcomplatedyn.Value == "0" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
            divOtherSection.Visible = true;
            divSignondate.Visible = true;
            divPrimarySection.Visible = true;
            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbarmain.Show();
        }

        PhoenixToolbar toolbarmain1 = new PhoenixToolbar();
        toolbarmain1.AddButton("List", "APPRAISAL");
        toolbarmain1.AddButton("Form", "FORM");
        if (Filter.CurrentAppraisalSelection != null)
        {
            toolbarmain1.AddButton("Seafarer  Comment", "SEAMANCOMMENT");
            if (ViewState["AppraisalStatus"].ToString() == "0" || PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain1.AddButton("Appraisal Report", "APPRAISALREPORT");
                toolbarmain1.AddButton("Promotion Report", "PROMOTION");
            }
        }
        AppraisalTabs.AccessRights = this.ViewState;
        AppraisalTabs.MenuList = toolbarmain1.Show();
        AppraisalTabs.SelectedMenuIndex = 1;


        DataSet ds2 = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        if (ds2.Tables[0].Rows.Count > 0)
        {
            canedit = ds2.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
        }

        //BindData();
        //BindConductData();
        EnablePage();

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
                    Response.Redirect("CrewAppraisal.aspx?occ=1&vslid=" + Request.QueryString["vslid"], false);
                else
                    Response.Redirect("CrewAppraisal.aspx?occ=1", false);
            }

            if (CommandName.ToUpper().Equals("SEAMANCOMMENT"))
            {
                Response.Redirect("CrewAppraisalSeamanComment.aspx", false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
            if (CommandName.ToUpper().Equals("PROMOTION"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void HodnameDisplay()
    {
        DataSet ds = PhoenixCrewAppraisal.CrewAppraisalHODNamelist(new Guid(Filter.CurrentAppraisalSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txthodname.Text = "Head of the Department: "+ ds.Tables[0].Rows[0]["FLDHODNAME"].ToString();
            txtmastername.Text ="Master: "+ ds.Tables[0].Rows[0]["FLDMASTERNAME"].ToString();
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
                                                    int.Parse(Filter.CurrentCrewSelection)
                                                   , DateTime.Parse(fromdate)
                                                   , DateTime.Parse(todate)
                                                   , int.Parse(vessel)
                                                   , General.GetNullableDateTime(appraisaldate)
                                                   , int.Parse(ddlOccassion.SelectedOccassion)
                                                   , ref iAppraisalId
                                                   , General.GetNullableInteger(chkAttachedCopyYN.Checked == true ? "1" : "0")
                                                   );

                    Filter.CurrentAppraisalSelection = iAppraisalId.ToString();

                    GetOccasionName(int.Parse(ddlOccassion.SelectedOccassion), new Guid(iAppraisalId.ToString()));

                    if (ViewState["OCCASSIONNAME"].ToString().ToUpper().Trim() == "MTR")
                    {
                        PhoenixCrewAppraisal.InsertAppraisalMidTenure(new Guid(iAppraisalId.ToString())
                                                                      , int.Parse(Filter.CurrentCrewSelection)
                                                                      , int.Parse(vessel)
                                                                      , int.Parse(ddlOccassion.SelectedOccassion)
                                                                      , General.GetNullableInteger(ViewState["Rankid"].ToString()));


                        Response.Redirect("CrewAppraisalMidtenureactivity.aspx?vslid=" + ViewState["VSLID"].ToString() + "&cmd=2", true);
                    }
                    if (ViewState["OCCASSIONNAME"].ToString().ToUpper().Trim() == "OWF")
                    {

                        PhoenixCrewAppraisal.InsertAppraisalOwnerFeedBack(new Guid(iAppraisalId.ToString())
                                                                      , int.Parse(Filter.CurrentCrewSelection)
                                                                      , int.Parse(vessel)
                                                                      , int.Parse(ddlOccassion.SelectedOccassion)
                                                                      , General.GetNullableInteger(ViewState["Rankid"].ToString()));


                        Response.Redirect("../Owners/OwnersCrewAppraisal.aspx?vslid=" + ViewState["VSLID"].ToString() + "&cmd=2", false);

                    }

                    else
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                }
                else
                {
                    PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                                , DateTime.Parse(fromdate)
                                                , DateTime.Parse(todate)
                                                , int.Parse(vessel)
                                                , General.GetNullableDateTime(appraisaldate)
                                                , int.Parse(occassion)
                                                , (hdnappraisalcomplatedyn.Value == "0") ? General.GetNullableByte(hdnappraisalcomplatedyn.Value) : null
                                                , General.GetNullableInteger(chkAttachedCopyYN.Checked == true ? "1" : "0")
                                    );
                }

                if (!chkAttachedCopyYN.Checked == true)
                {
                    if (!IsValidAppraisal())
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                bool result;
                result = true;
                InsertAppraisalProfileBulk(ref result);
                if (!result)
                {
                    ucError.Visible = true;
                    return;
                }
                InsertAppraisalConductBulk(ref result);
                if (!result)
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                    new Guid(Filter.CurrentAppraisalSelection)
                    , int.Parse(ddlRecommendPromotion.SelectedValue)
                    , chkExposedToDuties.Checked == true ? 1 : 0
                    , chkTrainingRequired.Checked == true ? 1 : 0
                    , txtCourseId.Text
                    , rbReemployment.Checked == true ? 1 : 0
                    , rbWarningToBeGiven.Checked == true ? 1 : 0
                    , rbNTBR.Checked == true ? 1 : 0
                    , 0
                    , rbNTBR.Checked == true ? txtNtbrRemarks.Text : null
                    , rbWarningToBeGiven.Checked == true ? txtWarningRemarks.Text : null
                    , General.GetNullableString(txtHeadOfDeptComment.Text)
                    , General.GetNullableString(txtMasteComment.Text)
                    , rbEnvironment.Checked == true ? 1 : 0
                    , rbEnvironment.Checked == true ? General.GetNullableString(txtEnvironment.Text) : null
                    , General.GetNullableString(txtSeafarerComment.Text)
                    , General.GetNullableInteger(ddlNoOfDocVisits.SelectedValue)
                    , General.GetNullableInteger(ddlCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(ddlWantToWorkAgainCrew.SelectedValue)
                    , General.GetNullableInteger(ddlWantToWorkAgainHOD.SelectedValue)
                    , General.GetNullableInteger(ddlAddTrainingYN.SelectedValue)
                    , txtTrainingRemarks.Text.Trim()
                    , General.GetNullableString(txtSuperintendentComment.Text)
                    );

                ucStatus.Text = "Appraisal Information updated.";
                //EditAppraisal();
                //BindData();
                //BindConductData(); ResetMenu();
                //EnablePage();

            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (!chkAttachedCopyYN.Checked == true)
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
                              , General.GetNullableInteger(chkAttachedCopyYN.Checked == true ? "1" : "0")
                  );

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                    new Guid(Filter.CurrentAppraisalSelection)
                    , int.Parse(ddlRecommendPromotion.SelectedValue)
                    , chkExposedToDuties.Checked == true ? 1 : 0
                    , chkTrainingRequired.Checked == true ? 1 : 0
                    , txtCourseId.Text
                    , rbReemployment.Checked == true ? 1 : 0
                    , rbWarningToBeGiven.Checked == true ? 1 : 0
                    , rbNTBR.Checked == true ? 1 : 0
                    , 0
                    , rbNTBR.Checked == true ? txtNtbrRemarks.Text : null
                    , rbWarningToBeGiven.Checked == true ? txtWarningRemarks.Text : null
                    , General.GetNullableString(txtHeadOfDeptComment.Text)
                    , General.GetNullableString(txtMasteComment.Text)
                    , rbEnvironment.Checked == true ? 1 : 0
                    , rbEnvironment.Checked == true ? General.GetNullableString(txtEnvironment.Text) : null
                    , General.GetNullableString(txtSeafarerComment.Text)
                    , General.GetNullableInteger(ddlNoOfDocVisits.SelectedValue)
                    , General.GetNullableInteger(ddlCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(ddlWantToWorkAgainCrew.SelectedValue)
                    , General.GetNullableInteger(ddlWantToWorkAgainHOD.SelectedValue)
                    , General.GetNullableInteger(ddlAddTrainingYN.SelectedValue)
                    , txtTrainingRemarks.Text.Trim()
                    , General.GetNullableString(txtSuperintendentComment.Text)
                    );

                ucStatus.Text = "This appraisal is finalised.";
                EditAppraisal();
                Response.Redirect(Request.RawUrl);
            }
            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                ViewState["UNLOCK"] = "1";
                ResetMenu();
                txtHeadOfDeptComment.ReadOnly = false;
                txtMasteComment.ReadOnly = false;
                //txtMasteComment.Enabled = true;
                txtSeafarerComment.ReadOnly = false;
                //txtSeafarerComment.Enabled = true;
                txtSeafarerComment.CssClass = "input_mandatory";
            }
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                ViewState["UNLOCK"] = "0";
                ResetMenu();
                txtHeadOfDeptComment.ReadOnly = true;
                //txtMasteComment.Enabled = false;
                txtMasteComment.ReadOnly = true;
                txtSeafarerComment.ReadOnly = true;
                //txtSeafarerComment.Enabled = false;
                txtSeafarerComment.CssClass = "readonlytextbox";
            }
            Response.Redirect("CrewAppraisalActivity.aspx", false);

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

        if (rbReemployment.Checked == true) i++;
        if (rbNTBR.Checked == true) i++;
        if (rbWarningToBeGiven.Checked == true) i++;
        if (i == 0)
            ucError.ErrorMessage = "In Recommendations section either one has to be selected.";
        if (i > 1)
            ucError.ErrorMessage = "In Recommendations section only one option can be select.";

        if (rbNTBR.Checked == true && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbNTBR.Checked == true && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked == true && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        if (String.IsNullOrEmpty(General.GetNullableString(ddlNoOfDocVisits.SelectedValue)))
        {
            ucError.ErrorMessage = "Number of Doctor visits is required.";
        }
        if ((rbNTBR.Checked == true || rbWarningToBeGiven.Checked == true) && ddlCategory.SelectedValue == string.Empty)
            ucError.ErrorMessage = "Reason for Ntbr/Warning is Required";
        if (!General.GetNullableInteger(ddlRecommendPromotion.SelectedValue).HasValue)
            ucError.ErrorMessage = "In Promotion Section recommended for promotion is Required";

        if (!General.GetNullableInteger(ddlAddTrainingYN.SelectedValue).HasValue)
            ucError.ErrorMessage = "In Training section training's effective is required";

        if (ddlAddTrainingYN.SelectedValue == "0" && txtTrainingRemarks.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Training remarks is required";

        if (ViewState["Rcategory"] != null)
        {
            DataSet ds1 = PhoenixRegistersRank.EditRank(int.Parse(ViewState["Rankid"].ToString()));

            if (ds1.Tables[0].Rows.Count > 0)
            {
                string rankgroup = ds1.Tables[0].Rows[0]["FLDGROUPRANK"].ToString();
                string ranklevel = ds1.Tables[0].Rows[0]["FLDLEVEL"].ToString();
                if (rankgroup == "MASTER" || rankgroup == "CH ENGINEER" || ranklevel == "1" || ranklevel == "11")
                {
                    if (General.GetNullableInteger(ddlWantToWorkAgainHOD.SelectedValue) == null)
                        ucError.ErrorMessage = "Please specify Would you like to work with him in Future?";
                }
                else
                {
                    if (General.GetNullableInteger(ddlWantToWorkAgainCrew.SelectedValue) == null)
                        ucError.ErrorMessage = "Please specify Would you like to work with him in Future?";
                }
            }
        }

        return (!ucError.IsError);
    }
    private bool IsValidAppraisalWithRatings()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (rbNTBR.Checked == true && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked == true && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        if (ConductCount != ConductFilledCount || AttitudeCount != AttitudeFilledCount || LeadershipCount != LeadershipFilledCount || JudgementCount != JudgementFilledCount)
        {
            ucError.ErrorMessage = "Section 1 has to be filled before confirming the Appraisal";
        }
        if (ProAppraisalCount != ProAppraisalFilledCount)
        {
            ucError.ErrorMessage = "Section 2 has to be filled before confirming the Appraisal";
        }
        if (!rbReemployment.Checked == true && !rbWarningToBeGiven.Checked == true && !rbNTBR.Checked == true)
        {
            ucError.ErrorMessage = "In Recommendations part either one option has to be selected before confirming the Appraisal";
        }
        if (General.GetNullableString(txtMasteComment.Text) == null)
        {
            ucError.ErrorMessage = "Master Comments should be posted before completing the Appraisal.";
        }
        //if (General.GetNullableString(txtHeadOfDeptComment.Text) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode==0)
        //{
        //    ucError.ErrorMessage = "Department head Comments should be posted.";
        //}

        return (!ucError.IsError);
    }
    private void EnableControls()
    {
        if (rbNTBR.Checked == true)
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

        if (rbWarningToBeGiven.Checked == true)
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
        if (rbEnvironment.Checked == true)
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
        if (chkRecommendPromotion.Checked == true)
            imgBtnPromotionDtls.Visible = true;
        else
            imgBtnPromotionDtls.Visible = false;

        if (ddlRecommendPromotion.SelectedValue == "1")
            imgBtnPromotionDtls.Visible = true;
        else
            imgBtnPromotionDtls.Visible = false;

    }
    private void EnablePage()
    {
        bool editable = canedit.Equals("0") ? false : true; //Enable or disable all controls
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 || Filter.CurrentMenuCodeSelection == "VAC-CRW-APL")
            ddlVessel.Enabled = false;
        else
            ddlVessel.Enabled = editable;
        txtFromDate.Enabled = editable;
        txtToDate.Enabled = editable;
        txtdate.Enabled = editable;
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
        {
            ddlOccassion.Enabled = false;
        }
        else
        {
            ddlOccassion.Enabled = editable;
        }

        if (chkRecommendPromotion.Enabled)
            chkRecommendPromotion.Enabled = editable;
        if (ddlRecommendPromotion.Enabled)
            ddlRecommendPromotion.Enabled = editable;
        if (chkExposedToDuties.Enabled)
            chkExposedToDuties.Enabled = editable;
        //     chkTrainingRequired.Enabled = editable;
        //if (!imgShowCourse.Disabled)
        //    imgShowCourse.Disabled = !editable;
        rbReemployment.Enabled = editable;
        rbWarningToBeGiven.Enabled = editable;
        txtWarningRemarks.Enabled = editable;
        rbEnvironment.Enabled = editable;
        txtEnvironment.Enabled = editable;
        rbNTBR.Enabled = editable;
        txtNtbrRemarks.Enabled = editable;
        //txtMasteComment.Enabled = editable;
        txtMasteComment.ReadOnly = !editable;
        ddlNoOfDocVisits.Enabled = editable;
        ddlAddTrainingYN.Enabled = editable;
        ddlWantToWorkAgainHOD.Enabled = editable;
        ddlWantToWorkAgainCrew.Enabled = editable;
        txtHeadOfDeptComment.ReadOnly = !editable;
        if (editable)
            EnableControls();
        //txtMasteComment.Enabled = canpostmstcomment.Equals("0") ? false : true;
        //txtHeadOfDeptComment.Enabled = canposthodcomment.Equals("0") ? false : true;
        ddlCategory.Enabled = (rbNTBR.Checked == true || rbWarningToBeGiven.Checked == true) ? true : false;
        ddlCategory.CssClass = (rbNTBR.Checked == true || rbWarningToBeGiven.Checked == true) ? "input_mandatory" : "input";
        //ddlCategory.SelectedValue = (rbNTBR.Checked || rbReemployment.Checked) ? ddlCategory.SelectedValue : "";

        if (hdnappraisalcomplatedyn.Value == "0")
        {
            txtTrainingRemarks.Enabled = false;
            txtTrainingRemarks.CssClass = "readonlytextbox";
            //foreach (GridDataItem gvr in gvCrewProfileAppraisalConduct.Items)
            //{
            //    UserControlNumber ucr = ((UserControlNumber)gvr.FindControl("ucRatingItem"));
            //    if (ucr != null)
            //        ucr.ReadOnly = "true";
            //}
            //foreach (GridDataItem gvr in gvCrewProfileAppraisalAttitude.Items)
            //{
            //    UserControlNumber ucr = ((UserControlNumber)gvr.FindControl("ucRatingItem"));
            //    if (ucr != null)
            //        ucr.ReadOnly = "true";
            //}
            //foreach (GridDataItem gvr in gvCrewProfileAppraisalLeadership.Items)
            //{
            //    UserControlNumber ucr = ((UserControlNumber)gvr.FindControl("ucRatingItem"));
            //    if (ucr != null)
            //        ucr.ReadOnly = "true";
            //}
            //foreach (GridDataItem gvr in gvCrewProfileAppraisalJudgementCommonSense.Items)
            //{
            //    UserControlNumber ucr = ((UserControlNumber)gvr.FindControl("ucRatingItem"));
            //    if (ucr != null)
            //        ucr.ReadOnly = "true";
            //}
            //foreach (GridDataItem gvr in gvCrewConductAppraisal.Items)
            //{
            //    UserControlNumber ucr = ((UserControlNumber)gvr.FindControl("ucRatingItem"));
            //    if (ucr != null)
            //        ucr.ReadOnly = "true";
            //}
        }
    }
    private void EditAppraisal()
    {

        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                //lblConduct.Text = ds.Tables[0].Rows[0]["FLDCONDUCT"].ToString();
                //lblatt.Text = ds.Tables[0].Rows[0]["FLDATTITUDE"].ToString();
                //lblLeadership.Text = ds.Tables[0].Rows[0]["FLDLEADERSHIP"].ToString();
                //lblJudgementandCommonSense.Text = ds.Tables[0].Rows[0]["FLDJUDGEMENTANDCOMMONSENSE"].ToString();
                chkRecommendPromotion.Checked = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString().Equals("1") ? true : false;
                ddlRecommendPromotion.SelectedValue = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString();
                chkExposedToDuties.Checked = ds.Tables[0].Rows[0]["FLDEXPOSEDTODUTIES"].ToString().Equals("1") ? true : false;
                chkTrainingRequired.Checked = ds.Tables[0].Rows[0]["FLDTRAININGREQUIRED"].ToString().Equals("1") ? true : false;
                rbReemployment.Checked = ds.Tables[0].Rows[0]["FLDFITFORREEMPLOYMENT"].ToString().Equals("1") ? true : false;
                ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["APPVESSEL"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                //guidelines.InnerHtml = ds.Tables[0].Rows[0]["FLDGUIDELINES"].ToString();
                txtSignOnDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();
                ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["FLDNTBRWARNINGREASON"].ToString();
                ddlAddTrainingYN.SelectedValue = ds.Tables[0].Rows[0]["FLDTRAININGISEFFECTIVE"].ToString();
                txtSuperintendentComment.Text = ds.Tables[0].Rows[0]["FLDSUPERINTENDENTCOMMENT"].ToString();
                ViewState["SIGNONOFFID"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                ViewState["SIGNONDATE"] = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();

                if (ds.Tables[0].Rows[0]["FLDATTACHEDSCANCOPYYN"].ToString().Equals("1"))
                    chkAttachedCopyYN.Checked = true;
                else
                    chkAttachedCopyYN.Checked = false;

                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                {
                    //if (!chkTrainingRequired.Checked)
                    //    imgShowOffshoreTraining.Disabled = true;
                    //else
                    //    imgShowOffshoreTraining.Disabled = false;
                }
                else
                {
                    if (!chkTrainingRequired.Checked == true)
                        imgShowCourse.Enabled = true;
                    else
                        imgShowCourse.Enabled = false;
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

                if (ds.Tables[0].Rows[0]["FLDTRAININGISEFFECTIVE"].ToString().Equals("1"))
                {
                    txtTrainingRemarks.CssClass = "input";
                }
                else
                {
                    txtTrainingRemarks.CssClass = "input_mandatory";

                }

                txtHeadOfDeptComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDHEADDEPTCOMMENT"].ToString());
                txtMasteComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDMASTERCOMMENT"].ToString());
                hdnSeamen.Value = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();
                txtNtbrRemarks.Text = ds.Tables[0].Rows[0]["FLDNTBRREMARKS"].ToString();
                txtWarningRemarks.Text = ds.Tables[0].Rows[0]["FLDWARNINGREMARKS"].ToString();
                txtCourseId.Text = ds.Tables[0].Rows[0]["FLDCOURSEREQUIRED"].ToString();
                txtCourseName.Text = ds.Tables[0].Rows[0]["FLDCOURSENAME"].ToString();
                txtEnvironment.Text = ds.Tables[0].Rows[0]["FLDENVNONCOMPREMARKS"].ToString();
                txtSeafarerComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString());
                noofvisits = ds.Tables[0].Rows[0]["FLDNOOFDOCTORVISIT"].ToString();
                ddlNoOfDocVisits.SelectedValue = String.IsNullOrEmpty(noofvisits) ? "DUMMY" : noofvisits;
                txtTrainingRemarks.Text = ds.Tables[0].Rows[0]["FLDTRAININGREMARKS"].ToString();

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString()) != null)
                    ddlWantToWorkAgainCrew.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString();

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString()) != null)
                    ddlWantToWorkAgainHOD.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString();

                if (chkExposedToDuties.Checked == true)
                    ddlRecommendPromotion.Enabled = true;
                else
                {
                    ddlRecommendPromotion.Enabled = false;
                    ddlRecommendPromotion.SelectedValue = "0";
                }

                if (chkRecommendPromotion.Checked == true)
                    imgBtnPromotionDtls.Visible = true;
                else
                    imgBtnPromotionDtls.Visible = false;

                if (ddlRecommendPromotion.SelectedValue == "1")
                    imgBtnPromotionDtls.Visible = true;
                else
                    imgBtnPromotionDtls.Visible = false;

                ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());

                lblPersonalProfileTotal.Text = ds.Tables[0].Rows[0]["PERSONALPROFILETOTAL"].ToString() + "/" + ds.Tables[0].Rows[0]["PERSONALPROFILEMAXTOTAL"].ToString();
                HodnameDisplay();
            }
        }
        else
        {
            imgBtnPromotionDtls.Visible = false;
            imgBtnCourseDone.Visible = false;
        }

        if (ViewState["Rcategory"] != null)
        {
            DataSet ds1 = PhoenixRegistersRank.EditRank(int.Parse(ViewState["Rankid"].ToString()));

            if (ds1.Tables[0].Rows.Count > 0)
            {
                string rankgroup = ds1.Tables[0].Rows[0]["FLDGROUPRANK"].ToString();
                string ranklevel = ds1.Tables[0].Rows[0]["FLDLEVEL"].ToString();
                if (rankgroup == "MASTER" || rankgroup == "CH ENGINEER" || ranklevel == "1" || ranklevel == "11")
                {
                    chkExposedToDuties.Checked = false;
                    chkRecommendPromotion.Checked = false;
                    ddlRecommendPromotion.SelectedValue = "0";
                    imgBtnPromotionDtls.Visible = false;

                    chkRecommendPromotion.Enabled = false;
                    ddlRecommendPromotion.Enabled = false;
                    chkExposedToDuties.Enabled = false;

                    lblWantToWorkAgainCrew.Visible = false;
                    ddlWantToWorkAgainCrew.Visible = false;
                }
                else
                {
                    lblWantToWorkAgainHOD.Visible = false;
                    ddlWantToWorkAgainHOD.Visible = false;
                }
            }
        }
    }
    public void GetRankCategory()
    {
        string Rcategory = null;

        PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["Rankid"].ToString()), ref Rcategory);

        if (Rcategory == System.DBNull.Value.ToString())
            Rcategory = "0";
        ViewState["Rcategory"] = Rcategory.ToString();
    }
    public void GetOccasionName(int OccasionId, Guid AppraisalId)
    {
        string OccasionName = null;
        string Appraisalnew = "0";

        PhoenixCrewAppraisal.AppraisalOccasionName(OccasionId, AppraisalId, ref OccasionName, ref Appraisalnew);

        if (OccasionName == System.DBNull.Value.ToString())
            OccasionName = "";

        if (Appraisalnew == System.DBNull.Value.ToString())
            Appraisalnew = "0";

        ViewState["OCCASSIONNAME"] = OccasionName.ToString();
        ViewState["APPRAISALNEW"] = Appraisalnew.ToString();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.InstallCode, Convert.ToInt32(Filter.CurrentCrewSelection));
            else
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                //txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                //txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                //txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCH"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlVessel.SelectedVessel = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
                if (Filter.CurrentAppraisalSelection == null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                    //guidelines.InnerHtml = dt.Rows[0]["FLDGUIDELINES"].ToString();
                    ViewState["empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                }
            }

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlVessel.SelectedValue = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
                    //txtxEmployeeName.Text           = ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString();
                    txtRank.Text = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
                    txtFromDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString());
                    txtToDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
                    ddlOccassion.SelectedOccassion = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();
                    txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                    ViewState["Rankid"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
                    ViewState["empid"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
                    ViewState["SIGNONOFFID"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetSignonDate()
    {
        DataTable dt = new DataTable();
        string vslid = ViewState["APPVESSEL"].ToString();
        if (General.GetNullableInteger(vslid) != null && General.GetNullableInteger(Filter.CurrentCrewSelection) != null)
            dt = PhoenixCommonVesselAccounts.VesselEmployeeAppraisalEdit(General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                , int.Parse(vslid)
                , General.GetNullableDateTime(txtFromDate.Text)
                , General.GetNullableDateTime(txtToDate.Text)
                , General.GetNullableInteger(Filter.CurrentCrewSelection)
                );

        if (dt.Rows.Count > 0)
            txtSignOnDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString();
    }
    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                               , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "CDT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ConductCount = ds.Tables[0].Rows.Count;
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
                ConductCount = 0;
            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "ATT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));
            gvCrewProfileAppraisalAttitude.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                AttitudeCount = ds.Tables[0].Rows.Count;
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else AttitudeCount = 0;

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "JCS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));
            gvCrewProfileAppraisalJudgementCommonSense.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                JudgementCount = ds.Tables[0].Rows.Count;
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else JudgementCount = 0;
            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "LDS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));
            gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                LeadershipCount = ds.Tables[0].Rows.Count;
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else LeadershipCount = 0;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewProfileAppraisalConduct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    void RecommendationSelection(RadCheckBox chk)
    {
        switch (chk.ID.ToString())
        {
            case "rbWarningToBeGiven":
                rbReemployment.Checked = false;
                rbNTBR.Checked = false;
                txtNtbrRemarks.Text = "";
                txtNtbrRemarks.Enabled = false;
                txtNtbrRemarks.CssClass = "readonlytextbox";
                ddlCategory.Enabled = true;
                ddlCategory.CssClass = "input_mandatory";
                break;
            case "rbNTBR":
                rbReemployment.Checked = false;
                rbWarningToBeGiven.Checked = false;
                txtWarningRemarks.Text = "";
                txtWarningRemarks.Enabled = false;
                txtWarningRemarks.CssClass = "readonlytextbox";
                ddlCategory.Enabled = true;
                ddlCategory.CssClass = "input_mandatory";
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
                ddlCategory.Enabled = false;
                ddlCategory.CssClass = "input";
                ddlCategory.SelectedValue = "";
                break;
        }
    }

    int profileCount;
    int ratingtotal = 0;
    int maxscoretotal = 0;
    private bool IsValidAppraisalProfile(string Rating, string Remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Rating.Trim().Equals(""))
            ucError.ErrorMessage = "Rating is required.";

        if (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) <= 0)
            ucError.ErrorMessage = "Rating should be between 1 to 10";


        return (!ucError.IsError);
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
            gvCrewConductAppraisal.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ProAppraisalCount = ds.Tables[0].Rows.Count;
                int ProTotal = 0;
                foreach (GridDataItem gv in gvCrewConductAppraisal.Items)
                {
                    RadLabel lblshortcode = (RadLabel)gv.FindControl("lblshortcode");
                    if (lblshortcode.Text.Trim() == "KPI")
                        ProTotal = ProTotal + (1 * 70);
                    else
                        ProTotal = ProTotal + (1 * 10);
                }
                ProTotal = ProTotal + (gvCrewProfileAppraisalAttitude.Items.Count) * 10;
                ProTotal = ProTotal + (gvCrewProfileAppraisalConduct.Items.Count) * 10;
                ProTotal = ProTotal + (gvCrewProfileAppraisalLeadership.Items.Count) * 10;
                ProTotal = ProTotal + (gvCrewProfileAppraisalJudgementCommonSense.Items.Count) * 10;
                lblGrandTotal.Text = "Final Total Score achieved - " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString() + " out of  " + ds.Tables[0].Rows[0]["FLDMAXGRANDTOTAL"].ToString();// conductmaxscoretotal.ToString();//+ ProTotal.ToString();
                lblpercentage.Text = "Final Percentage achieved - " + String.Format("{0:0.00}", ((decimal.Parse(ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString()) / decimal.Parse(ds.Tables[0].Rows[0]["FLDMAXGRANDTOTAL"].ToString())) * 100));//decimal.Parse(ProTotal.ToString())) * 100));

            }
            else
                ProAppraisalCount = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewConductAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindConductData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewConductAppraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("REFRESH"))
            {
                string vessel = ddlVessel.SelectedVessel;
                string fromdate = txtFromDate.Text;
                string todate = txtToDate.Text;
                string appraisaldate = txtdate.Text;
                string occassion = ddlOccassion.SelectedOccassion;

                PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                               , DateTime.Parse(fromdate)
                                               , DateTime.Parse(todate)
                                               , int.Parse(vessel)
                                               , General.GetNullableDateTime(appraisaldate)
                                               , int.Parse(occassion)
                                               , (hdnappraisalcomplatedyn.Value == "0") ? General.GetNullableByte(hdnappraisalcomplatedyn.Value) : null
                                               , General.GetNullableInteger(chkAttachedCopyYN.Checked == true ? "1" : "0")
                                   );
                BindData();
                BindConductData();
                EnablePage();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewProfileAppraisalConduct_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridHeaderItem)
        {
            profileCount = 0;
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            // LinkButton db = (LinkButton)e.Item.FindControl("cmdattachedimg");
            RadLabel lbRate = (RadLabel)e.Item.FindControl("lblRating");
            RadLabel lblMaxscore = (RadLabel)e.Item.FindControl("lblMaxscore");
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
            maxscoretotal += int.Parse(lblMaxscore.Text == "" ? "10" : lblMaxscore.Text);
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            if (hdnappraisalcomplatedyn.Value == "0")
            {
                UserControlNumber ucr = ((UserControlNumber)e.Item.FindControl("ucRatingItem"));
                if (ucr != null)
                {
                    ucr.ReadOnly = "true";
                    ucr.Enabled = false;
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem Gridname = e.Item as GridFooterItem;


            RadLabel lblFooterTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
            int RatingTotalValue = 0;
            RatingTotalValue = maxscoretotal; //(_gridView.Rows.Count) * 10;
            if (lblFooterTotal != null)
            {

                lblFooterTotal.Text = ratingtotal.ToString() + " / " + RatingTotalValue.ToString();
                ratingtotal = 0;
                maxscoretotal = 0;
            }
            RadLabel lblgvid = (RadLabel)e.Item.FindControl("lblgvid");
            switch (lblgvid.Text)
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

    int ProCount;
    int ProRatingTotal = 0;
    //int   = 0;
    protected void gvCrewConductAppraisal_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            // LinkButton db = (LinkButton)e.Item.FindControl("cmdattachedimg");
            RadLabel lbRate = (RadLabel)e.Item.FindControl("lblRating");
            RadLabel lblMaxscore = (RadLabel)e.Item.FindControl("lblMaxscore");
            RadLabel lblshortcode = (RadLabel)e.Item.FindControl("lblshortcode");
            LinkButton imgkpiscore = (LinkButton)e.Item.FindControl("imgkpiscore");
            LinkButton imgkpiscorerefresh = (LinkButton)e.Item.FindControl("imgkpiscorerefresh");
            LinkButton imgoldscore = (LinkButton)e.Item.FindControl("imgoldscore");
            RadLabel lbloldscore = (RadLabel)e.Item.FindControl("lbloldscore");
            RadLabel lblcurrentscore = (RadLabel)e.Item.FindControl("lblcurrentscore");
            UserControlNumber ucRatingItem = (UserControlNumber)e.Item.FindControl("ucRatingItem");
            UserControlNumber ucoldscore = (UserControlNumber)e.Item.FindControl("ucoldscore");

            if (lblshortcode != null && lblshortcode.Text == "KPI")
            {
                ucRatingItem.ReadOnly = "true";
                ucoldscore.ReadOnly = "true";
                ucoldscore.Visible = true;
                imgkpiscore.Visible = true;
                imgkpiscorerefresh.Visible = true;
                imgkpiscore.Attributes.Add("onclick", "return showPickList('spnTraining', 'codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAppraisalKPIScorelist.aspx?empid=" + ViewState["empid"].ToString() + "&signonoffid=" + ViewState["SIGNONOFFID"].ToString() + "&signondate=" + ViewState["SIGNONDATE"].ToString() + "', 'yes'); return true");
                imgoldscore.Visible = true;
                imgoldscore.Attributes.Add("onclick", "return showPickList('spnTraining', 'codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAppraisalKPIScorelist.aspx?CAID=" + Filter.CurrentAppraisalSelection + "', 'yes'); return true");
                lbloldscore.Visible = true;
                lblcurrentscore.Visible = true;
            }
            else
            {
                imgkpiscore.Visible = false;
                imgkpiscorerefresh.Visible = false;
                imgoldscore.Visible = false;
                ucoldscore.Visible = false;
                lbloldscore.Visible = false;
                lblcurrentscore.Visible = false;
            }
            if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
            {
                ProCount += 1;
                ProRatingTotal += int.Parse(lbRate.Text == "" ? "0" : lbRate.Text);
            }
            else
            {
                UserControlNumber rating = (UserControlNumber)e.Item.FindControl("ucRatingEdit");
                if (rating != null && !String.IsNullOrEmpty(rating.Text))
                {
                    ProRatingTotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
                }
            }
            maxscoretotal += int.Parse(lblMaxscore.Text == "" ? "0" : lblMaxscore.Text);
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit1");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            if (hdnappraisalcomplatedyn.Value == "0")
            {
                UserControlNumber ucr = ((UserControlNumber)e.Item.FindControl("ucRatingItem"));
                if (ucr != null)
                {
                    ucr.ReadOnly = "true";
                    ucr.Enabled = false;
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblFooterTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
            int ProTotal = 0;
            foreach (GridDataItem gv in gvCrewConductAppraisal.Items)
            {
                RadLabel lblshortcode = (RadLabel)gv.FindControl("lblshortcode");
                if (lblshortcode.Text.Trim() == "KPI")
                    ProTotal = ProTotal + (1 * 70);
                else
                    ProTotal = ProTotal + (1 * 10);
            }

            if (lblFooterTotal != null)
            {
                //if (gvCrewConductAppraisal.Items.Count > 0)
                //    lblFooterTotal.Text = "0" + " / " + "0";
                //else
                lblFooterTotal.Text = ProRatingTotal.ToString() + " / " + maxscoretotal.ToString();// ProRatingTotal.ToString() + " / " + ProTotal.ToString();
                ProRatingTotal = 0;
                maxscoretotal = 0;
            }
            ProAppraisalFilledCount = ProCount;
        }
    }



    private bool IsValidAppraisalConduct(string Rating, string Remark, string itemname, string maxmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((General.GetNullableInteger(Rating) != 10 && General.GetNullableInteger(Rating) != 0) && itemname.Contains("Sobriety"))
            ucError.ErrorMessage = "Sobriety Rating should be 0 or 10";

        if (Rating=="" || General.GetNullableInteger(Rating) > General.GetNullableInteger(maxmark))
            ucError.ErrorMessage = "Rating should be less than max score";

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
            ucError.ErrorMessage = "'From date' should not be future date";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date";
        }
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out resultdate))
            ucError.ErrorMessage = "Appraisal date is required";
        else if (!string.IsNullOrEmpty(todate)
              && DateTime.TryParse(appraisaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should be later than or equal to 'To Date'";
        }
        if (!string.IsNullOrEmpty(appraisaldate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(appraisaldate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should not be future date";
        }
        if (General.GetNullableString(txtSeafarerComment.Text) == null && hdnappraisalcomplatedyn.Value == "0")
        {
            ucError.ErrorMessage = "Sefarer comment is required.";
        }
        return (!ucError.IsError);
    }

    protected void chkTrainingRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
        {
            if (!chkTrainingRequired.Checked == true)
            {
                txtCourseId.Text = String.Empty;
                txtCourseName.Text = String.Empty;
                Filter.CurrentPickListSelection = null;
                //imgShowOffshoreTraining.Disabled = true;
            }
            else imgShowOffshoreTraining.Disabled = false;
        }
        else
        {
            if (!chkTrainingRequired.Checked == true)
            {
                txtCourseId.Text = String.Empty;
                txtCourseName.Text = String.Empty;
                Filter.CurrentPickListSelection = null;
                imgShowCourse.Enabled = true;
            }
            else
            {
                imgShowCourse.Enabled = false;
            }
        }
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
        RadCheckBox chkBx = (RadCheckBox)sender;
        if (chkBx.Checked == true)
            RecommendationSelection(chkBx);
    }
    private void BindCategoryDropDown()
    {
        DataTable dt = PhoenixCrewReApprovalSeafarer.CategoryList();
        if (dt.Rows.Count > 0)
        {
            ddlCategory.DataSource = dt;
            ddlCategory.DataValueField = "FLDCATEGORYID";
            ddlCategory.DataTextField = "FLDCATEGORYNAME";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new DropDownListItem("--Select--", ""));
        }
    }
    private void InsertAppraisalProfileBulk(ref bool result)
    {
        result = true;
        DataSet ds = new DataSet();

        DataTable table = new DataTable();
        table.Columns.Add("FLDAPPRAISALPROFILEID", typeof(Guid));
        table.Columns.Add("FLDPROFILECATEGORY", typeof(int));
        table.Columns.Add("FLDPROFILEQUESTIONID", typeof(int));
        table.Columns.Add("FLDRATING", typeof(int));
        table.Columns.Add("FLDMAXMARK", typeof(int));
        int count = 0, i = 0;
        count = gvCrewProfileAppraisalConduct.Items.Count;

        foreach (GridDataItem gv in gvCrewProfileAppraisalConduct.Items)
        {

            if (count > i)
            {
                string rating = "";
                string maxmark = "";
                UserControlNumber ucr = ((UserControlNumber)gv.FindControl("ucRatingItem"));
                RadLabel lblMaxscore = ((RadLabel)gv.FindControl("lblMaxscore"));
                if (ucr != null)
                    rating = ucr.Text.TrimStart('_').ToString();
                if (lblMaxscore != null)
                    maxmark = lblMaxscore.Text;

                if ((rating != "" && int.Parse(rating) <= int.Parse(maxmark)))

                {
                    if (((RadLabel)gv.FindControl("lblevaluationitem")).Text.Contains("Sobriety") && (int.Parse(rating) == 0 || int.Parse(rating) == 10))
                    {
                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                                General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)
                                   );
                    }
                    else if (!((RadLabel)gv.FindControl("lblevaluationitem")).Text.Contains("Sobriety"))
                    {

                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                                General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                                General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)
                                   );
                    }

                }
              //  else if (rating != "")//&& (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                //{
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString(""), General.GetNullableString(((RadLabel)gv.FindControl("lblevaluationitem")).Text),
                        ((RadLabel)gv.FindControl("lblMaxscore")).Text))
                    {
                        result = false;
                        return;
                    }
                //}
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalAttitude.Items.Count;

        foreach (GridDataItem gv in gvCrewProfileAppraisalAttitude.Items)
        {
            if (count > i)
            {
                string rating = "";
                string maxmark = "";
                UserControlNumber ucr = ((UserControlNumber)gv.FindControl("ucRatingItem"));
                RadLabel lblMaxscore = ((RadLabel)gv.FindControl("lblMaxscore"));
                if (ucr != null)
                    rating = ucr.Text.TrimStart('_').ToString();

                if (lblMaxscore != null)
                    maxmark = lblMaxscore.Text;

                if (rating != "" && int.Parse(rating) <= int.Parse(maxmark))// int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                              General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)
                              );
                }
              //  else if (rating != "")//&& (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                //{
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString(""), "", ((RadLabel)gv.FindControl("lblMaxscore")).Text))
                    {
                        result = false;
                        return;
                    }
                //}
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalLeadership.Items.Count;
        foreach (GridDataItem gv in gvCrewProfileAppraisalLeadership.Items)
        {
            if (count > i)
            {
                string rating = "";
                string maxmark = "";
                UserControlNumber ucr = ((UserControlNumber)gv.FindControl("ucRatingItem"));
                RadLabel lblMaxscore = ((RadLabel)gv.FindControl("lblMaxscore"));
                if (ucr != null)
                    rating = ucr.Text.TrimStart('_').ToString();

                if (lblMaxscore != null)
                    maxmark = lblMaxscore.Text;

                if (rating != "" && int.Parse(rating) <= int.Parse(maxmark)) // && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                              General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)
                                 );
                }
             //   else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
              //  {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString(""), "", ((RadLabel)gv.FindControl("lblMaxscore")).Text))
                    {
                        result = false;
                        return;
                    }
              //  }
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalJudgementCommonSense.Items.Count;

        foreach (GridDataItem gv in gvCrewProfileAppraisalJudgementCommonSense.Items)
        {
            if (count > i)
            {
                string rating = "";
                string maxmark = "";
                UserControlNumber ucr = ((UserControlNumber)gv.FindControl("ucRatingItem"));
                RadLabel lblMaxscore = ((RadLabel)gv.FindControl("lblMaxscore"));
                if (ucr != null)
                    rating = ucr.Text.TrimStart('_').ToString();

                if (lblMaxscore != null)
                    maxmark = lblMaxscore.Text;

                if (rating != "" && int.Parse(rating) <= int.Parse(maxmark))// && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                               General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)
                                  );
                }
             //   else if (rating != "")// && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString(""), "", ((RadLabel)gv.FindControl("lblMaxscore")).Text))
                    {
                        result = false;
                        return;
                    }
                }

                i++;
            }
        }
        ds.Tables.Add(table);

        StringWriter sw = new StringWriter();
        ds.WriteXml(sw);
        string resultstring = sw.ToString();

        PhoenixCrewAppraisalProfile.InsertAppraisalProfileBulk(new Guid(Filter.CurrentAppraisalSelection.ToString()), resultstring);
    }

    private void InsertAppraisalConductBulk(ref bool result)
    {
        try
        {
            DataSet ds = new DataSet();

            DataTable table = new DataTable();
            table.Columns.Add("FLDAPPRAISALCONDUCTID", typeof(Guid));
            table.Columns.Add("FLDCONDUCTQUESTIONID", typeof(int));
            table.Columns.Add("FLDRATING", typeof(int));
            table.Columns.Add("FLDMAXMARK", typeof(int));

            int count = 0, i = 0;
            count = gvCrewConductAppraisal.Items.Count;

            foreach (GridDataItem gv in gvCrewConductAppraisal.Items)
            {
                if (count > i)
                {
                    string rating = "";
                    string maxmark = "";
                    RadLabel lblshortcode = ((RadLabel)gv.FindControl("lblshortcode"));
                    UserControlNumber ucr = ((UserControlNumber)gv.FindControl("ucRatingItem"));
                    RadLabel lblMaxscore = ((RadLabel)gv.FindControl("lblMaxscore"));
                    if (ucr != null)
                        rating = ucr.Text.TrimStart('_').ToString();

                    if (lblMaxscore != null)
                        maxmark = lblMaxscore.Text;

                    if ((rating != "" && int.Parse(rating) <= int.Parse(maxmark))) //&& lblshortcode.Text.Trim() != "KPI") || (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 70 && lblshortcode.Text.Trim() == "KPI"))
                    {
                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalConductId")).Text),
                                  General.GetNullableInteger(((RadLabel)gv.FindControl("lblEvaluationQuestionId")).Text),
                                  General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text),
                                  General.GetNullableInteger(((RadLabel)gv.FindControl("lblMaxscore")).Text)

                                     );
                    }
                  //  else if (rating != "")
                    {
                        if (!IsValidAppraisalConduct(rating, lblshortcode.Text.Trim(), "", ((RadLabel)gv.FindControl("lblMaxscore")).Text))
                        {
                            result = false;
                            return;
                        }
                    }
                    i++;
                }
            }
            ds.Tables.Add(table);

            StringWriter sw = new StringWriter();
            ds.WriteXml(sw);
            string resultstring = sw.ToString();
            PhoenixCrewAappraisalConduct.InsertAppraisalConductBulk(new Guid(Filter.CurrentAppraisalSelection.ToString()), resultstring, int.Parse(ViewState["Rankid"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlAddTrainingYN_Changed(object sender, EventArgs e)
    {
        if (ddlAddTrainingYN.SelectedValue == "0")
            txtTrainingRemarks.CssClass = "input_mandatory";
        else
            txtTrainingRemarks.CssClass = "input";
    }
    protected void chkExposedToDuties_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExposedToDuties.Checked == true)
            ddlRecommendPromotion.Enabled = true;
        else
        {
            ddlRecommendPromotion.SelectedValue = "0";
            ddlRecommendPromotion.Enabled = false;
            imgBtnPromotionDtls.Visible = false;
        }
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["UNLOCK"] != null && ViewState["UNLOCK"].ToString() == "0")
                toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
            if (ViewState["UNLOCK"] != null && ViewState["UNLOCK"].ToString() == "1")
            {
                toolbar.AddButton("Save Changes", "SAVECHANGES", ToolBarDirection.Right);
                toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
            }
            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
