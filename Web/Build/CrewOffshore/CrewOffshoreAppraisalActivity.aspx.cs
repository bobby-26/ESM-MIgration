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


public partial class CrewOffshoreAppraisalActivity : PhoenixBasePage
{
    int ConductCount, AttitudeCount, LeadershipCount, JudgementCount, ProAppraisalCount, CompetenceCount;
    int ConductFilledCount, AttitudeFilledCount, LeadershipFilledCount, JudgementFilledCount, ProAppraisalFilledCount, CompetenceFilledCount;
    string canedit = "1", canpostmstcomment = "1";

    //string noofvisits;
    //protected override void Render(HtmlTextWriter writer)
    //{

    //    //foreach (GridDataItem r in gvCrewCompetenceAppraisal.Items)
    //    //{
          
    //    //        Page.ClientScript.RegisterForEventValidation(gvCrewCompetenceAppraisal.UniqueID, "Edit$" + r.RowIndex.ToString());
          
    //    //}

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {

                ViewState["Rcategory"] = 0;
                ViewState["Category"] = 0;
                ViewState["AppraisalStatus"] = 1;
                ViewState["APPVESSEL"] = "";
                ViewState["VSLID"] = "";
                //txtCourseId.Attributes.Add("style", "visibility:hidden;");
                cmdHiddenPick.Attributes.Add("style", "display:none;");
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                if (Request.QueryString["appraisalid"] != null)
                    Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();
                if (Request.QueryString["empid"] != null)
                    Filter.CurrentCrewSelection = Request.QueryString["empid"].ToString();
                if (Request.QueryString["VSLID"] != null)
                    ViewState["VSLID"] = Request.QueryString["VSLID"].ToString();

                if (Filter.CurrentMenuCodeSelection == "VAC-CRW-APL")
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

                SetEmployeePrimaryDetails();
                GetRankCategory();

                ddlOccassion.OccassionList = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
                ddlOccassion.Category = ViewState["Rcategory"].ToString();
                ddlOccassion.DataBind();
                BindCategoryDropDown();
                DataSet ds = PhoenixCrewAppraisal.AppraisalOffshoreSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                    canpostmstcomment = ds.Tables[0].Rows[0]["FLDCANPOSTMSTCOMMENT"].ToString();
                }
              

                EditAppraisal();


                

                chkRecommendPromotion.Attributes.Add("onclick", "javascript:OpenChkboxPopup(this, " + ViewState["Rankid"].ToString() + ", 1," + ViewState["APPVESSEL"].ToString() + ")");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    chkAttachedCopyYN.Enabled = false;

            }
            DataSet ds2 = PhoenixCrewAppraisal.AppraisalOffshoreSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds2.Tables[0].Rows.Count > 0)
            {
                canedit = ds2.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (canedit.Equals("1"))
            {

                if (Filter.CurrentAppraisalSelection != null)
                {
                    toolbarmain.AddButton("Complete", "COMPLETE", ToolBarDirection.Right);
                    toolbarmain.AddButton("Save Changes", "SAVECHANGES",ToolBarDirection.Right);
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                 
                    divOtherSection.Visible = true;
                    divSignondate.Visible = true;
                    divPrimarySection.Visible = true;
                }
                else
                {
                    toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
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
            toolbarmain = new PhoenixToolbar();
            
            toolbarmain.AddButton("Form", "FORM");
            toolbarmain.AddButton("Back", "BACK");


            if (Filter.CurrentAppraisalSelection != null)
            {
                if (canedit.Equals("0"))
                    toolbarmain.AddButton("Comment", "SEAMANCOMMENT");
                if (ViewState["AppraisalStatus"].ToString() == "0" || PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    toolbarmain.AddButton("Report", "APPRAISALREPORT");

                }
            }
            AppraisalTabs.AccessRights = this.ViewState;
            AppraisalTabs.MenuList = toolbarmain.Show();
            AppraisalTabs.SelectedMenuIndex = 0;
            //BindData();
            //BindConductData();
            //BindCompetenceData();
            EnablePage();
            if (Filter.CurrentAppraisalSelection != null)
                HodnameDisplay();
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
            txthodname.Text = ds.Tables[0].Rows[0]["FLDHODNAME"].ToString();
            txtMastername.Text = ds.Tables[0].Rows[0]["FLDMASTERNAME"].ToString();
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;
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

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["cmd"] == "1")
                {
                    Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisal.aspx?cmd=" + Request.QueryString["cmd"], false);
                }
                else if (Request.QueryString["cmd"] == "2")
                {
                    Response.Redirect("~/Crew/CrewAppraisal.aspx?cmd=" + Request.QueryString["cmd"], false);
                }
                else
                {
                    Response.Redirect("~/Crew/CrewAppraisalPendingSeafarercomments.aspx", false);
                }
            }
            if (CommandName.ToUpper().Equals("SEAMANCOMMENT"))
            {
                Response.Redirect("~/Crew/CrewAppraisalSeamanComment.aspx?cmd=" + Request.QueryString["cmd"], false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                if (Request.QueryString["cmd"] == "1")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFSHOREAPPRAISALNEW&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false); 
                }
                else
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFSHOREAPPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
                }
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
                                                    int.Parse(Filter.CurrentCrewSelection)
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

                //if (!chkAttachedCopyYN.Checked)
                //{
                //    if (!IsValidAppraisal())
                //    {
                //        ucError.Visible = true;
                //        return;
                //    }
                //}

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
                InsertAppraisalCompetenceBulk(ref result);
                if (!result)
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                    new Guid(Filter.CurrentAppraisalSelection)
                    , General.GetNullableInteger(ddlRecommendPromotion.SelectedValue)
                    , chkExposedToDuties.Checked ? 1 : 0
                    , 0
                    , null
                    , rbReemployment.Checked.Value ? 1 : 0
                    , rbWarningToBeGiven.Checked.Value ? 1 : 0
                    , rbNTBR.Checked.Value ? 1 : 0
                    , 0
                    , rbNTBR.Checked.Value ? txtNtbrRemarks.Text : null
                    , rbWarningToBeGiven.Checked.Value ? txtWarningRemarks.Text : null
                    , txtHeadOfDeptComment.Text
                    , txtMasteComment.Text
                    , General.GetNullableInteger(ddlEnvironment.SelectedValue == "1" ? "1" : (ddlEnvironment.SelectedValue == "0" ? "0" : ""))
                    , ddlEnvironment.SelectedValue == "1" ? General.GetNullableString(txtEnvironment.Text) : null
                    , General.GetNullableString(hdnSeamen.Value)
                    , 0
                     , General.GetNullableInteger(ddlCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(ddlWantToWorkAgainCrew.SelectedValue)
                    , General.GetNullableInteger(ddlWantToWorkAgainHOD.SelectedValue)
                    );

                ucStatus.Text = "Appraisal Information updated.";
                EditAppraisal();
                BindData();
                BindConductData();
                BindCompetenceData();
                EnablePage();
            }
            if (CommandName.ToUpper().Equals("COMPLETE"))
            {
                if (!chkAttachedCopyYN.Checked)
                {
                    if (!IsValidAppraisalWithRatings())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    if (!IsValidAppraisal())
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(
                    new Guid(Filter.CurrentAppraisalSelection)
                    , int.Parse(ddlRecommendPromotion.SelectedValue)
                    , chkExposedToDuties.Checked ? 1 : 0
                    , 0
                    , null
                    , rbReemployment.Checked.Value ? 1 : 0
                    , rbWarningToBeGiven.Checked.Value ? 1 : 0
                    , rbNTBR.Checked.Value ? 1 : 0
                    , 0
                    , rbNTBR.Checked.Value ? txtNtbrRemarks.Text : null
                    , rbWarningToBeGiven.Checked.Value ? txtWarningRemarks.Text : null
                    , txtHeadOfDeptComment.Text
                    , txtMasteComment.Text
                    , General.GetNullableInteger(ddlEnvironment.SelectedValue == "1" ? "1" : (ddlEnvironment.SelectedValue == "0" ? "0" : ""))
                    , ddlEnvironment.SelectedValue == "1" ? General.GetNullableString(txtEnvironment.Text) : null
                    , General.GetNullableString(hdnSeamen.Value)
                    , 0
                     , General.GetNullableInteger(ddlCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(ddlWantToWorkAgainCrew.SelectedValue)
                    , General.GetNullableInteger(ddlWantToWorkAgainHOD.SelectedValue)
                    , 1
                    );

                PhoenixCrewAppraisal.CompleteAppraisal(new Guid(Filter.CurrentAppraisalSelection));
                EditAppraisal();
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

        if (rbReemployment.Checked.Value) i++;
        if (rbNTBR.Checked.Value) i++;
        if (rbWarningToBeGiven.Checked.Value) i++;

        if (i == 0)
            ucError.ErrorMessage = "In Recommendations section either one has to be selected.";

        if (i > 1)
            ucError.ErrorMessage = "In Recommendations section only one option can be select.";

        if (rbNTBR.Checked.Value && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked.Value && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        //if (String.IsNullOrEmpty(General.GetNullableString(ddlNoOfDocVisits.SelectedValue)))
        //{
        //    ucError.ErrorMessage = "Number of Doctor visits is required.";
        //}
        if ((rbNTBR.Checked.Value || rbWarningToBeGiven.Checked.Value) && ddlCategory.SelectedValue == string.Empty)
            ucError.ErrorMessage = "Reason for Ntbr/Warning is Required";
        if (!General.GetNullableInteger(ddlRecommendPromotion.SelectedValue).HasValue)
            ucError.ErrorMessage = "In Promotion Section recommended for promotion is Required";

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
                        ucError.ErrorMessage = "Please specify Would you like to sail with this officer/crew again?";
                }
            }
        }

        return (!ucError.IsError);
    }

    private bool IsValidAppraisalWithRatings()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (rbNTBR.Checked.Value && txtNtbrRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR remarks is required.";

        if (rbWarningToBeGiven.Checked.Value && txtWarningRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Warning remarks is required.";

        if (ConductCount != ConductFilledCount || AttitudeCount != AttitudeFilledCount || LeadershipCount != LeadershipFilledCount || JudgementCount != JudgementFilledCount)
        {
            ucError.ErrorMessage = "Section 1 has to be filled before confirming the Appraisal";
        }
        if (ProAppraisalCount != ProAppraisalFilledCount)
        {
            ucError.ErrorMessage = "Section 2 has to be filled before confirming the Appraisal";
        }
        if (CompetenceCount != CompetenceFilledCount)
        {
            ucError.ErrorMessage = "Section 3 has to be filled before confirming the Appraisal";
        }
        if (!rbReemployment.Checked.Value && !rbWarningToBeGiven.Checked.Value && !rbNTBR.Checked.Value)
        {
            ucError.ErrorMessage = "In Recommendations part either one option has to be selected before confirming the Appraisal";
        }
        if (General.GetNullableString(txtHeadOfDeptComment.Text.Trim()) == null && General.GetNullableString(txtMasteComment.Text.Trim()) == null)
        {
            ucError.ErrorMessage = "Head of Dept Comments or Master Comments is required.";
        }
        //if (General.GetNullableString(txtMasteComment.Text) == null)
        //{
        //    ucError.ErrorMessage = "Master Comments is required.";
        //}

        //if (String.IsNullOrEmpty(hdnSeamen.Value))
        //{
        //    ucError.ErrorMessage = "Seafarer Comments should be posted before confirming the Appraisal";
        //}

        return (!ucError.IsError);
    }

    private void EnableControls()
    {
        if (rbNTBR.Checked !=null && rbNTBR.Checked.Value == true)
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

        if (rbWarningToBeGiven.Checked != null && rbWarningToBeGiven.Checked.Value == true)
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
        if (ddlEnvironment.SelectedValue == "1")
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
        ddlOccassion.Enabled = editable;
        if (chkRecommendPromotion.Enabled)
            chkRecommendPromotion.Enabled = editable;
        if (ddlRecommendPromotion.Enabled)
            ddlRecommendPromotion.Enabled = editable;
        if (chkExposedToDuties.Enabled)
            chkExposedToDuties.Enabled = editable;
        rbReemployment.Enabled = editable;
        rbWarningToBeGiven.Enabled = editable;
        txtWarningRemarks.Enabled = editable;
        ddlEnvironment.Enabled = editable;
        txtEnvironment.Enabled = editable;
        rbNTBR.Enabled = editable;
        txtNtbrRemarks.Enabled = editable;
        txtHeadOfDeptComment.Enabled = editable;
        txtMasteComment.Enabled = editable;
        //ddlNoOfDocVisits.Enabled = editable;
        if (editable)
            EnableControls();
        txtMasteComment.Enabled = canpostmstcomment.Equals("0") ? false : true;
        if (rbNTBR.Checked != null || rbWarningToBeGiven.Checked!=null) ddlCategory.Enabled = (rbNTBR.Checked.Value || rbWarningToBeGiven.Checked.Value) ? true : false;
        if (rbNTBR.Checked != null || rbWarningToBeGiven.Checked != null) ddlCategory.CssClass = (rbNTBR.Checked.Value || rbWarningToBeGiven.Checked.Value) ? "input_mandatory" : "input";

        //ddlCategory.SelectedValue = (rbNTBR.Checked || rbReemployment.Checked) ? ddlCategory.SelectedValue : "";
    }

    private void EditAppraisal()
    {

        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                lnkpendingcbt.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString() + "&coursetype=451'); return false;");
                
                chkRecommendPromotion.Checked = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString().Equals("1") ? true : false;
                ddlRecommendPromotion.SelectedValue = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString();
                chkExposedToDuties.Checked = ds.Tables[0].Rows[0]["FLDEXPOSEDTODUTIES"].ToString().Equals("1") ? true : false;
                rbReemployment.Checked = ds.Tables[0].Rows[0]["FLDFITFORREEMPLOYMENT"].ToString().Equals("1") ? true : false;
                ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["APPVESSEL"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                guidelines.InnerHtml = ds.Tables[0].Rows[0]["FLDGUIDELINES"].ToString();
                txtSignOnDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();
                ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["FLDNTBRWARNINGREASON"].ToString();

                if (ddlRecommendPromotion.SelectedValue.ToString() == "1" && int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString()) != 0)
                    lnkpendingcbt.Visible = true;
                else
                    lnkpendingcbt.Visible = false;

                if (ds.Tables[0].Rows[0]["FLDATTACHEDSCANCOPYYN"].ToString().Equals("1"))
                    chkAttachedCopyYN.Checked = true;
                else
                    chkAttachedCopyYN.Checked = false;

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
                    //rbEnvironment.Checked = true;
                    ddlEnvironment.SelectedValue = "1";
                    txtEnvironment.Enabled = true;
                    txtEnvironment.CssClass = "input_mandatory";
                }
                else
                {
                    //rbEnvironment.Checked = false;
                    ddlEnvironment.SelectedValue = ds.Tables[0].Rows[0]["FLDENVNONCOMPILANCE"].ToString();
                    txtEnvironment.Enabled = false;
                    txtEnvironment.Text = "";
                    txtEnvironment.CssClass = "readonlytextbox";
                }

                txtHeadOfDeptComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDHEADDEPTCOMMENT"].ToString());
                txtMasteComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDMASTERCOMMENT"].ToString());
                hdnSeamen.Value = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();
                txtNtbrRemarks.Text = ds.Tables[0].Rows[0]["FLDNTBRREMARKS"].ToString();
                txtWarningRemarks.Text = ds.Tables[0].Rows[0]["FLDWARNINGREMARKS"].ToString();
                txtEnvironment.Text = ds.Tables[0].Rows[0]["FLDENVNONCOMPREMARKS"].ToString();
                txtSeafarerComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString());

                //noofvisits = ds.Tables[0].Rows[0]["FLDNOOFDOCTORVISIT"].ToString();
                //ddlNoOfDocVisits.SelectedValue = String.IsNullOrEmpty(noofvisits) ? "DUMMY" : noofvisits;

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString()) != null)
                    ddlWantToWorkAgainCrew.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString();

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString()) != null)
                    ddlWantToWorkAgainHOD.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString();

                ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
            }
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
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlVessel.SelectedVessel = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
                if (Filter.CurrentAppraisalSelection == null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
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
                    txtRank.Text = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                ConductCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
               // gvCrewProfileAppraisalConduct.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
              
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
                
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                AttitudeCount = 0;
              
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
               
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                JudgementCount = 0;
                
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
              
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                LeadershipCount = 0;
              
                gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
            }
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
                //gvCrewProfileAppraisalConduct.SelectedIndex = -1;
                //gvCrewProfileAppraisalConduct.EditIndex = -1;
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

    int profileCount;
    int ratingtotal = 0;
   

    //protected void gvCrewProfileAppraisal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    BindData();
    //}

    //protected void gvCrewProfileAppraisal_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    ChangeEditMode(_gridView);
    //    if (_gridView.EditIndex > -1)
    //        _gridView.UpdateRow(_gridView.EditIndex, false);
    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindData();
    //    ((UserControlNumber)_gridView.Rows[e.NewEditIndex].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
    //}

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
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvCrewProfileAppraisal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            if (Filter.CurrentAppraisalSelection == null)
            {
                ucError.HeaderMessage = "Please provide the Primary Details information";
                ucError.Visible = true;
                return;
            }
            if (!IsValidAppraisalProfile(
                    ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text,
                    General.GetNullableString("")))
            {
                ucError.Visible = true;
                ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
                return;
            }

            if (!String.IsNullOrEmpty(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_')))
            {
                if (General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalProfileId")).Text) == null)
                {
                    PhoenixCrewAppraisalProfile.InsertAppraisalProfile(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Filter.CurrentAppraisalSelection)
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProfileCategoryId")).Text)
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProfileQuestionId")).Text)
                        , int.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
                        , General.GetNullableString(""));
                }
                else
                {
                    PhoenixCrewAppraisalProfile.UpdateAppraisalProfile(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalProfileId")).Text)
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProfileCategoryId")).Text)
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProfileQuestionId")).Text)
                        , int.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
                        , General.GetNullableString(""));
                }
            }

            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewProfileAppraisalConduct_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalConduct');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalConduct');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void gvCrewProfileAppraisalAttitude_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalAttitude');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalAttitude');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void gvCrewProfileAppraisalLeadership_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalLeadership');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalLeadership');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }

    protected void gvCrewProfileAppraisalJudgementCommonSense_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalJudgementCommonSense');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalJudgementCommonSense');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }

    private bool IsValidAppraisalProfile(string Rating, string Remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Rating.Trim().Equals(""))
            ucError.ErrorMessage = "Rating is required.";

        if (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) <= 0)
            ucError.ErrorMessage = "Rating should be between 1 to 10";


        return (!ucError.IsError);
    }

    protected void gvCrewProfileAppraisal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
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
              
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                ProAppraisalCount = 0;
                gvCrewConductAppraisal.DataSource = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewConductAppraisal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            if (Filter.CurrentAppraisalSelection == null)
            {
                ucError.HeaderMessage = "Please provide the Primary Details information";
                ucError.ErrorMessage = "<br/>Primary Details has to be filled and saved before enter rating's";
                ucError.Visible = true;
                return;
            }
            if (!IsValidAppraisalConduct(
                    ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'),
                    General.GetNullableString("")))
            {
                ucError.Visible = true;
                ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
                return;
            }

            if (!String.IsNullOrEmpty(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_')))
            {
                if (General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalConductId")).Text) == null)
                {
                    PhoenixCrewAappraisalConduct.InsertAppraisalConduct(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Filter.CurrentAppraisalSelection.ToString())
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationQuestionId")).Text)
                        , int.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
                        , General.GetNullableString("")
                        , int.Parse(ViewState["Rankid"].ToString()));
                }
                else
                {
                    PhoenixCrewAappraisalConduct.UpdateAppraisalConduct(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalConductId")).Text)
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationQuestionId")).Text)
                        , int.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
                        , General.GetNullableString(""));
                }
            }

            _gridView.EditIndex = -1;
            BindConductData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvCrewConductAppraisal_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (_gridView.EditIndex > -1)
    //        _gridView.UpdateRow(_gridView.EditIndex, false);
    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindConductData();
    //    ((UserControlNumber)_gridView.Rows[e.NewEditIndex].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
    //}

    //protected void gvCrewConductAppraisal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    BindConductData();
    //}

    //int ProCount;
    //int ProRatingTotal = 0;
    //protected void gvCrewConductAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSIONSUB"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSUB"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTIONSUB"] == null || ViewState["SORTDIRECTIONSUB"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //        ProCount = 0;
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label lbRate = (Label)e.Row.FindControl("lblRating");
    //        if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
    //        {
    //            ProCount += 1;
    //            ProRatingTotal += int.Parse(lbRate.Text == "" ? "0" : lbRate.Text);
    //        }
    //        else
    //        {
    //            UserControlNumber rating = (UserControlNumber)e.Row.FindControl("ucRatingEdit");
    //            if (rating != null && !String.IsNullOrEmpty(rating.Text))
    //            {
    //                ProRatingTotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
    //            }
    //        }
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit1");
    //        if (eb != null)
    //        {
    //            if (!canedit.Equals("1"))
    //                eb.Visible = false;
    //            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
    //                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        Label lblFooterTotal = (Label)e.Row.FindControl("lblFooterTotal");
    //        int ProTotal = 0;
    //        ProTotal = (gvCrewConductAppraisal.MasterTableView.Items.Count) * 10;
    //        if (lblFooterTotal != null)
    //        {
    //            if (gvCrewConductAppraisal.MasterTableView.Items.Count<=0) //gvCrewConductAppraisal.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
    //                lblFooterTotal.Text = "0" + " / " + "0";
    //            else
    //                lblFooterTotal.Text = ProRatingTotal.ToString() + " / " + ProTotal.ToString();
    //            ProRatingTotal = 0;
    //        }
    //        ProAppraisalFilledCount = ProCount;
    //    }

    //}

    //protected void gvCrewConductAppraisal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;

    //    BindConductData();
    //}

    //protected void gvCrewConductAppraisal_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidAppraisalConduct(string Rating, string Remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvCrewConductAppraisal;

        if (Rating.Trim().Equals(""))
            ucError.ErrorMessage = "Rating is required.";

        if (!Rating.Trim().Equals("") && (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) <= 0))
            ucError.ErrorMessage = "Rating should be between 1 to 10";

        return (!ucError.IsError);
    }
    private bool IsValidAppraisalConduct(string Rating, string Remark, string itemname)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvCrewConductAppraisal;

        if (Rating.Trim().Equals(""))
            ucError.ErrorMessage = "Rating is required.";

        if ((General.GetNullableInteger(Rating) != 10 || General.GetNullableInteger(Rating) != 0) && itemname.Contains("Sobriety"))
            ucError.ErrorMessage = "Sobriety Rating should be 0 or 10";

        if (!Rating.Trim().Equals("") && (General.GetNullableInteger(Rating) > 10 || General.GetNullableInteger(Rating) <= 0) && !itemname.Contains("Sobriety"))
            ucError.ErrorMessage = "Rating should be between 1 to 10";



        return (!ucError.IsError);
    }
    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out  resultdate))
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
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out  resultdate))
            ucError.ErrorMessage = "Appraisal Date is required";
        else if (!string.IsNullOrEmpty(todate)
              && DateTime.TryParse(appraisaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal Date' should be later than or equal to 'To Date'";
        }
        if (!string.IsNullOrEmpty(appraisaldate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(appraisaldate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should not be future date";
        }

        return (!ucError.IsError);
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

    //protected void gvCrewConductAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //            (e.Row.RowState == DataControlRowState.Normal ||
    //            e.Row.RowState == DataControlRowState.Alternate))
    //    {
    //        e.Row.TabIndex = 0;
    //        e.Row.Attributes["onclick"] =
    //          string.Format("javascript:SelectRow(this, {0}, null,'gvCrewConductAppraisal');", e.Row.RowIndex);
    //        e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewConductAppraisal');";
    //        e.Row.Attributes["onselectstart"] = "javascript:return false;";
    //    }
    //}

    protected void Recommendations_Changed(object sender, EventArgs e)
    {
        RadCheckBox chkBx = (RadCheckBox)sender;
        if (chkBx.Checked.Value)
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
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
    }
    private void BindCompetenceData()
    {
        try
        {
            DataSet ds = PhoenixCrewAappraisalCompetence.AppraisalCompetenceSearch(
                      General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                      , int.Parse(ViewState["Rankid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                CompetenceCount = ds.Tables[0].Rows.Count;
                gvCrewCompetenceAppraisal.DataSource = ds.Tables[0];
              
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
               
                CompetenceCount = 0;
                gvCrewCompetenceAppraisal.DataSource = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCrewCompetenceAppraisal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        if (Filter.CurrentAppraisalSelection == null)
    //        {
    //            ucError.HeaderMessage = "Please provide the Primary Details information";
    //            ucError.ErrorMessage = "<br/>Primary Details has to be filled and saved before enter rating's";
    //            ucError.Visible = true;
    //            return;
    //        }
    //        if (!IsValidAppraisalConduct(
    //                ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'),
    //                General.GetNullableString("")))
    //        {
    //            ucError.Visible = true;
    //            ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).FindControl("txtNumber").Focus();
    //            return;
    //        }

    //        if (!String.IsNullOrEmpty(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_')))
    //        {
    //            if (General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewAppraisalCompetenceId")).Text) == null)
    //            {
    //                PhoenixCrewAappraisalCompetence.InsertAppraisalCompetence(
    //                     new Guid(Filter.CurrentAppraisalSelection.ToString())
    //                    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationQuestionId")).Text)
    //                    , General.GetNullableInteger(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
    //                    , General.GetNullableInteger(ViewState["Rankid"].ToString()));
    //            }
    //            else
    //            {
    //                PhoenixCrewAappraisalCompetence.UpdateAppraisalCompetence(
    //                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewAppraisalCompetenceId")).Text)
    //                    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationQuestionId")).Text)
    //                    , General.GetNullableInteger(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucRatingEdit")).Text.TrimStart('_'))
    //                    );
    //            }
    //        }

    //        _gridView.EditIndex = -1;
    //        BindCompetenceData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    int CompCount;
    int CompetenceRatingTotal = 0;
    //protected void gvCrewCompetenceAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSIONSUB"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSUB"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTIONSUB"] == null || ViewState["SORTDIRECTIONSUB"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
    //        {
    //            e.Row.Cells[3].Visible = false;
    //            e.Row.Cells[4].Visible = false;
    //        }
    //        CompCount = 0;
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
    //        {
    //            e.Row.Cells[3].Visible = false;
    //            e.Row.Cells[4].Visible = false;
    //        }

    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit1");
    //        if (eb != null)
    //        {
    //            if (!canedit.Equals("1"))
    //                eb.Visible = false;
    //            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
    //                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
    //        }
    //        if (drv["FLDTOTAL"] != null && drv["FLDTOTAL"].ToString() != "")
    //            CompetenceRatingTotal = int.Parse(drv["FLDTOTAL"].ToString());
    //        Label lblCrewAppraisalCompetenceId = (Label)e.Row.FindControl("lblCrewAppraisalCompetenceId");
    //        Label lblEvaluationQuestionId = (Label)e.Row.FindControl("lblEvaluationQuestionId");
    //        Label lblRating = (Label)e.Row.FindControl("lblRating");
    //        UserControlNumber ucRatingItem = (UserControlNumber)e.Row.FindControl("ucRatingItem");
    //        ImageButton cmdTraining = (ImageButton)e.Row.FindControl("cmdTraining");
    //        if (cmdTraining != null)
    //            cmdTraining.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','../CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid=" + Filter.CurrentCrewSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection
    //                + "&vesselid=" + ViewState["APPVESSEL"].ToString()
    //                + "&CompetenceId=" + lblCrewAppraisalCompetenceId.Text
    //                + "&CategoryId=" + lblEvaluationQuestionId.Text
    //                + "&rating=" + ucRatingItem.Text + "'); return true;");
    //        if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text))
    //        {
    //            CompCount += 1;
    //            //ratingtotal += int.Parse(lblRating.Text == "" ? "0" : lblRating.Text);
    //        }
    //        CheckBox chkMandatory = (CheckBox)e.Row.FindControl("chkMandatory");

    //        if (chkMandatory != null)
    //        {
    //            if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text) && int.Parse(lblRating.Text) > 0 && int.Parse(lblRating.Text) <= 4)
    //                chkMandatory.Checked = true;
    //            chkMandatory.Enabled = false;
    //        }
    //        //else
    //        //{
    //        //    UserControlNumber rating = (UserControlNumber)e.Row.FindControl("ucRatingEdit");
    //        //    if (rating != null && !String.IsNullOrEmpty(rating.Text))
    //        //    {
    //        //        ratingtotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
    //        //    }
    //        //}
    //        //if (lblRating != null && lblRating.Text != "")
    //        //{
    //        //    int rating = 0;
    //        //    rating = int.Parse(lblRating.Text);
    //        //    if (rating >= 1 && rating <= 4)
    //        //        cmdTraining.Visible = true;
    //        //}
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
    //        {
    //            e.Row.Cells[3].Visible = false;
    //            e.Row.Cells[4].Visible = false;
    //        }
    //        Label lblTotal = (Label)e.Row.FindControl("lblFooterTotal");
    //        int CompetenceTotal = 0;
    //        CompetenceTotal = (gvCrewCompetenceAppraisal.MasterTableView.Items.Count) * 10;
    //        if (lblTotal != null)
    //        {
    //            if (gvCrewCompetenceAppraisal.MasterTableView.Items.Count <=0) //gvCrewCompetenceAppraisal.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
    //                lblTotal.Text = "0" + " / " + "0";
    //            else
    //                lblTotal.Text = CompetenceRatingTotal.ToString() + " / " + CompetenceTotal.ToString();
    //            CompetenceRatingTotal = 0;
    //        }
    //        CompetenceFilledCount = CompCount;
    //    }
    //}
    //protected void gvCrewCompetenceAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //            (e.Row.RowState == DataControlRowState.Normal ||
    //            e.Row.RowState == DataControlRowState.Alternate))
    //    {
    //        e.Row.TabIndex = 0;
    //        e.Row.Attributes["onclick"] =
    //          string.Format("javascript:SelectRow(this, {0}, null,'gvCrewCompetenceAppraisal');", e.Row.RowIndex);
    //        e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewCompetenceAppraisal');";
    //        e.Row.Attributes["onselectstart"] = "javascript:return false;";
    //    }
    //}
    private void InsertAppraisalProfileBulk(ref bool result)
    {
        result = true;

        DataSet ds = new DataSet();

        DataTable table = new DataTable();
        table.Columns.Add("FLDAPPRAISALPROFILEID", typeof(Guid));
        table.Columns.Add("FLDPROFILECATEGORY", typeof(int));
        table.Columns.Add("FLDPROFILEQUESTIONID", typeof(int));
        table.Columns.Add("FLDRATING", typeof(int));

        int count = 0, i = 0;
        count = gvCrewProfileAppraisalConduct.Items.Count;

        foreach (GridDataItem gv in gvCrewProfileAppraisalConduct.MasterTableView.Items)
        {
            if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {
                string rating = "";
                rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                if ((rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10) ||
                    ((RadLabel)gv.FindControl("lblevaluationitem")).Text.Contains("Sobriety") && int.Parse(rating) >= 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                            General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                            General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                            General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                               );
                }
                else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString(""), General.GetNullableString(((RadLabel)gv.FindControl("lblevaluationitem")).Text)))
                    {
                        result = false;
                        return;
                    }
                }
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalAttitude.Items.Count;
        foreach (GridDataItem gv in gvCrewProfileAppraisalAttitude.MasterTableView.Items)
        {
            
            if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {
                string rating = "";
                rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                if (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                              General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                              );
                }
                else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString("")))
                    {
                        result = false;
                        return;
                    }
                }
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalLeadership.Items.Count;
        foreach (GridDataItem gv in gvCrewProfileAppraisalLeadership.MasterTableView.Items)
        {
            if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {
                string rating = "";
                rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                if (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                              General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                              General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                                 );
                }
                else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString("")))
                    {
                        result = false;
                        return;
                    }
                }
                i++;
            }
        }
        i = 0;
        count = gvCrewProfileAppraisalJudgementCommonSense.Items.Count;

        foreach (GridDataItem gv in gvCrewProfileAppraisalJudgementCommonSense.MasterTableView.Items)
        {
            if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {
                string rating = "";
                rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                if (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                {
                    table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalProfileId")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileCategoryId")).Text),
                               General.GetNullableInteger(((RadLabel)gv.FindControl("lblProfileQuestionId")).Text),
                               General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                                  );
                }
                else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                {
                    if (!IsValidAppraisalConduct(rating, General.GetNullableString("")))
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


            int count = 0, i = 0;
            count = gvCrewConductAppraisal.Items.Count;

            foreach (GridDataItem gv in gvCrewConductAppraisal.MasterTableView.Items)
            {
                if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
                {
                    string rating = "";
                    rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                    if (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                    {
                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblAppraisalConductId")).Text),
                                  General.GetNullableInteger(((RadLabel)gv.FindControl("lblEvaluationQuestionId")).Text),
                                  General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                                     );
                    }
                    else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                    {
                        if (!IsValidAppraisalConduct(rating, General.GetNullableString("")))
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
    private void InsertAppraisalCompetenceBulk(ref bool result)
    {
        try
        {

            DataSet ds = new DataSet();

            DataTable table = new DataTable();
            table.Columns.Add("FLDCREWAPPRAISALCOMPETENCEID", typeof(Guid));
            table.Columns.Add("FLDCOMPETENCECATEGORYID", typeof(int));
            table.Columns.Add("FLDRATING", typeof(int));



            int count = 0, i = 0;
            count = gvCrewCompetenceAppraisal.Items.Count;

            foreach (GridDataItem gv in gvCrewCompetenceAppraisal.MasterTableView.Items)
            {
                if (count > i && gv.Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
                {
                    string rating = "";
                    rating = ((UserControlNumber)gv.FindControl("ucRatingItem")).Text.TrimStart('_').ToString();

                    if (rating != "" && int.Parse(rating) > 0 && int.Parse(rating) <= 10)
                    {
                        table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblCrewAppraisalCompetenceId")).Text),
                                 General.GetNullableInteger(((RadLabel)gv.FindControl("lblEvaluationQuestionId")).Text),
                                 General.GetNullableInteger(((UserControlNumber)gv.FindControl("ucRatingItem")).Text)
                                    );
                    }
                    else if (rating != "" && (int.Parse(rating) <= 0 || int.Parse(rating) > 10))
                    {
                        if (!IsValidAppraisalConduct(rating, General.GetNullableString("")))
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

            PhoenixCrewAappraisalCompetence.InsertAppraisalCompetenceBulk(new Guid(Filter.CurrentAppraisalSelection.ToString()), resultstring, int.Parse(ViewState["Rankid"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewProfileAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            profileCount = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lbRate = (RadLabel)e.Row.FindControl("lblRating");
            if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
            {
                profileCount += 1;
                ratingtotal += int.Parse(lbRate.Text == "" ? "0" : lbRate.Text);
            }
            else
            {
                UserControlNumber rating = (UserControlNumber)e.Row.FindControl("ucRatingEdit");
                if (rating != null && !String.IsNullOrEmpty(rating.Text))
                {
                    ratingtotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
                }
            }
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblFooterTotal = (Label)e.Row.FindControl("lblFooterTotal");
            int RatingTotalValue = 0;
            RatingTotalValue = (_gridView.Rows.Count) * 10;
            if (lblFooterTotal != null)
            {
                if (_gridView.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
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
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
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
                if (_gridView.MasterTableView.Items.Count<=0)
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
        ProAppraisalFilledCount = profileCount;
    }

    protected void gvCrewProfileAppraisalAttitude_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewProfileAppraisalLeadership_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewProfileAppraisalJudgementCommonSense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewConductAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindConductData();
    }

    protected void gvCrewCompetenceAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCompetenceData();
    }


    protected void gvCrewCompetenceAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

     
        if (e.Item is GridDataItem)
        {

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
            {
                e.Item.Cells[3].Visible = false;
                e.Item.Cells[4].Visible = false;
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit1");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            if (drv["FLDTOTAL"] != null && drv["FLDTOTAL"].ToString() != "")
                CompetenceRatingTotal = int.Parse(drv["FLDTOTAL"].ToString());
            RadLabel lblCrewAppraisalCompetenceId = (RadLabel)e.Item.FindControl("lblCrewAppraisalCompetenceId");
            RadLabel lblEvaluationQuestionId = (RadLabel)e.Item.FindControl("lblEvaluationQuestionId");
            RadLabel lblRating = (RadLabel)e.Item.FindControl("lblRating");
            UserControlNumber ucRatingItem = (UserControlNumber)e.Item.FindControl("ucRatingItem");
            LinkButton cmdTraining = (LinkButton)e.Item.FindControl("cmdTraining");
            if (cmdTraining != null)
                cmdTraining.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid=" + Filter.CurrentCrewSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection
                    + "&vesselid=" + ViewState["APPVESSEL"].ToString()
                    + "&CompetenceId=" + lblCrewAppraisalCompetenceId.Text
                    + "&CategoryId=" + lblEvaluationQuestionId.Text
                    + "&rating=" + ucRatingItem.Text + "'); return true;");
            if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text))
            {
                CompCount += 1;
                //ratingtotal += int.Parse(lblRating.Text == "" ? "0" : lblRating.Text);
            }
            CheckBox chkMandatory = (CheckBox)e.Item.FindControl("chkMandatory");

            if (chkMandatory != null)
            {
                if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text) && int.Parse(lblRating.Text) > 0 && int.Parse(lblRating.Text) <= 4)
                    chkMandatory.Checked = true;
                chkMandatory.Enabled = false;
            }
            //else
            //{
            //    UserControlNumber rating = (UserControlNumber)e.Row.FindControl("ucRatingEdit");
            //    if (rating != null && !String.IsNullOrEmpty(rating.Text))
            //    {
            //        ratingtotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
            //    }
            //}
            //if (lblRating != null && lblRating.Text != "")
            //{
            //    int rating = 0;
            //    rating = int.Parse(lblRating.Text);
            //    if (rating >= 1 && rating <= 4)
            //        cmdTraining.Visible = true;
            //}
        }
        if (e.Item is GridFooterItem)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
            {
                e.Item.Cells[3].Visible = false;
                e.Item.Cells[4].Visible = false;
            }
            RadLabel lblTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
            int CompetenceTotal = 0;
            CompetenceTotal = (gvCrewCompetenceAppraisal.MasterTableView.Items.Count) * 10;
            if (lblTotal != null)
            {
                if (gvCrewCompetenceAppraisal.MasterTableView.Items.Count <= 0) //gvCrewCompetenceAppraisal.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
                    lblTotal.Text = "0" + " / " + "0";
                else
                    lblTotal.Text = CompetenceRatingTotal.ToString() + " / " + CompetenceTotal.ToString();
                CompetenceRatingTotal = 0;
            }
            CompetenceFilledCount = CompCount;
        }
    }
}
