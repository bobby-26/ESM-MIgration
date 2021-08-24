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

public partial class CrewOffshore_CrewOffshoreAppraisalDetailforCrewComment : PhoenixBasePage
{
    int ConductCount, AttitudeCount, LeadershipCount, JudgementCount, ProAppraisalCount, CompetenceCount;
    int ConductFilledCount, AttitudeFilledCount, LeadershipFilledCount, JudgementFilledCount, ProAppraisalFilledCount, CompetenceFilledCount;
    string canedit = "1";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["Rcategory"] = 0;
                ViewState["Category"] = 0;
                ViewState["AppraisalStatus"] = 1;
                ViewState["APPVESSEL"] = "";
                ViewState["VSLID"] = "";
                ViewState["RANKID"] = "";
                ViewState["NAME"] = "";
                ViewState["RANK"] = "";

                cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

                if (Request.QueryString["aprid"] != null)
                    Filter.CurrentAppraisalSelection = Request.QueryString["aprid"].ToString();

                SetEmployeePrimaryDetails();
                GetRankCategory();

                ddlOccassion.OccassionList = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
                ddlOccassion.Category = ViewState["Rcategory"].ToString();
                ddlOccassion.DataBind();
                BindCategoryDropDown();
                //DataSet ds = PhoenixCrewAppraisal.AppraisalOffshoreSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                //    canpostmstcomment = ds.Tables[0].Rows[0]["FLDCANPOSTMSTCOMMENT"].ToString();
                //}
                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                EditAppraisal();

                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save & Finalize", "SAVE", ToolBarDirection.Right);

                AppraisalTabs.AccessRights = this.ViewState;
                AppraisalTabs.MenuList = toolbarmain.Show();

                gvCrewConductAppraisal.PageSize = 10000;
                gvCrewCompetenceAppraisal.PageSize = 10000;
            }
            //  BindData();
            //  BindConductData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditAppraisal()
    {
        try
        {

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Request.QueryString["aprid"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkRecommendPromotion.Checked = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString().Equals("1") ? true : false;
                    ddlRecommendPromotion.SelectedValue = ds.Tables[0].Rows[0]["FLDRECOMANDPROMOTION"].ToString();
                    chkExposedToDuties.Checked = ds.Tables[0].Rows[0]["FLDEXPOSEDTODUTIES"].ToString().Equals("1") ? true : false;
                    rbReemployment.Checked = ds.Tables[0].Rows[0]["FLDFITFORREEMPLOYMENT"].ToString().Equals("1") ? true : false;
                    //ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    ViewState["APPVESSEL"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    guidelines.InnerHtml = ds.Tables[0].Rows[0]["FLDGUIDELINES"].ToString();
                    txtSignOnDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();
                    ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["FLDNTBRWARNINGREASON"].ToString();

                    if (ds.Tables[0].Rows[0]["FLDATTACHEDSCANCOPYYN"].ToString().Equals("1"))
                        chkAttachedCopyYN.Checked = true;
                    else
                        chkAttachedCopyYN.Checked = false;

                    if (ds.Tables[0].Rows[0]["FLDWARNINGTOBEGIVEN"].ToString().Equals("1"))
                    {
                        rbWarningToBeGiven.Checked = true;
                        txtWarningRemarks.Enabled = true;
                        txtWarningRemarks.CssClass = "input";
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
                        txtNtbrRemarks.CssClass = "input";
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
                        txtEnvironment.CssClass = "input";
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

                    if (chkRecommendPromotion.Checked == true)
                        imgBtnPromotionDtls.Visible = true;
                    else
                        imgBtnPromotionDtls.Visible = false;

                    if (ddlRecommendPromotion.SelectedValue == "1")
                        imgBtnPromotionDtls.Visible = true;
                    else
                        imgBtnPromotionDtls.Visible = false;

                    ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
                }
            }
            else
            {
                imgBtnPromotionDtls.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (txtSeafarerComment.Text.Trim() == "")
                {
                    ucError.ErrorMessage = "Please enter the Seafarer Comment";
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAppraisal.UpdateAppraisalSeamanComment(1,new Guid(Filter.CurrentAppraisalSelection), txtSeafarerComment.Text);

                Response.Redirect("../CrewOffshore/CrewOffshoreCrewCommentConfirmMsg.aspx?NAME=" + ViewState["NAME"].ToString() + "&RANK=" + ViewState["RANK"].ToString() );
                
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
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void GetRankCategory()
    {

        try
        {
            string Rcategory = null;

            PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["Rankid"].ToString()), ref Rcategory);

            if (Rcategory == System.DBNull.Value.ToString())
                Rcategory = "0";

            ViewState["Rcategory"] = Rcategory.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();

            ds1 = PhoenixCrewAppraisal.EditAppraisal(new Guid(Request.QueryString["aprid"]));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                Filter.CurrentCrewSelection = ds1.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["VSLID"] = ds1.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["RANKID"] = ds1.Tables[0].Rows[0]["FLDRANKID"].ToString();
            }
            dt = PhoenixVesselAccountsEmployee.EditVesselCrew(int.Parse(ViewState["VSLID"].ToString()), Convert.ToInt32(Filter.CurrentCrewSelection));

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
                ViewState["NAME"] = txtFirstName.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim();
                ViewState["RANK"] = txtRank.Text.Trim();
            }

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
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

    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                               , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "CDT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            ConductCount = ds.Tables[0].Rows.Count;
            gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
            gvCrewProfileAppraisalConduct.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "ATT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            AttitudeCount = ds.Tables[0].Rows.Count;
            gvCrewProfileAppraisalAttitude.DataSource = ds.Tables[0];
            gvCrewProfileAppraisalAttitude.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "JCS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            JudgementCount = ds.Tables[0].Rows.Count;
            gvCrewProfileAppraisalJudgementCommonSense.DataSource = ds.Tables[0];
            gvCrewProfileAppraisalJudgementCommonSense.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();


            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "LDS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            LeadershipCount = ds.Tables[0].Rows.Count;
            gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
            gvCrewProfileAppraisalLeadership.VirtualItemCount = ds.Tables[0].Rows.Count;
            if(ds.Tables[0].Rows.Count>0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int profileCount;
    int ratingtotal = 0;
    protected void gvCrewProfileAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadGrid gridview = (RadGrid)sender;

            if (e.Item is GridHeaderItem)
            {
                profileCount = 0;
            }
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
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                }
            }
            if (e.Item is GridFooterItem)
            {
                RadLabel lblFooterTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
                int RatingTotalValue = 0;
                RatingTotalValue = (gridview.Items.Count) * 10;
                if (lblFooterTotal != null)
                {
                    if (gridview.Items.Count == 0)
                        lblFooterTotal.Text = "0" + " / " + "0";
                    else
                        lblFooterTotal.Text = ratingtotal.ToString() + " / " + RatingTotalValue.ToString();
                    ratingtotal = 0;
                }
                switch (gridview.ID.ToString())
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindConductData()
    {
        try
        {
            DataSet ds = PhoenixCrewAappraisalConduct.AppraisalConductSearch(
                      General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                      , int.Parse(ViewState["Rankid"].ToString()));

            ProAppraisalCount = ds.Tables[0].Rows.Count;
            gvCrewConductAppraisal.DataSource = ds.Tables[0];
            gvCrewConductAppraisal.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int ProCount;
    int ProRatingTotal = 0;
    protected void gvCrewConductAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                ProCount = 0;
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lbRate = (RadLabel)e.Item.FindControl("lblRating");
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
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit1");
                if (eb != null)
                {
                    if (!canedit.Equals("1"))
                        eb.Visible = false;
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                }
            }
            if (e.Item is GridFooterItem)
            {
                RadLabel lblFooterTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
                int ProTotal = 0;
                ProTotal = (gvCrewConductAppraisal.Items.Count) * 10;
                if (lblFooterTotal != null)
                {
                    if (gvCrewConductAppraisal.Items.Count == 0)
                        lblFooterTotal.Text = "0" + " / " + "0";
                    else
                        lblFooterTotal.Text = ProRatingTotal.ToString() + " / " + ProTotal.ToString();
                    ProRatingTotal = 0;
                }
                ProAppraisalFilledCount = ProCount;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCategoryDropDown()
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCompetenceData()
    {
        try
        {
            DataSet ds = PhoenixCrewAappraisalCompetence.AppraisalCompetenceSearch(
                      General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                      , int.Parse(ViewState["Rankid"].ToString()));


            CompetenceCount = ds.Tables[0].Rows.Count;
            gvCrewCompetenceAppraisal.DataSource = ds.Tables[0];
            gvCrewCompetenceAppraisal.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int CompCount;
    int CompetenceRatingTotal = 0;
    protected void gvCrewCompetenceAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridHeaderItem)
            {
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
                //{
                e.Item.Cells[3].Visible = false;
                e.Item.Cells[4].Visible = false;

                e.Item.Cells[1].Visible = true;
                e.Item.Cells[2].Visible = true;
                //}
                CompCount = 0;
            }
            if (e.Item is GridDataItem)
            {

                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
                //{
                e.Item.Cells[3].Visible = false;
                e.Item.Cells[4].Visible = false;

                e.Item.Cells[1].Visible = true;
                e.Item.Cells[2].Visible = true;


                //}

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit1");
                if (eb != null)
                {
                    if (!canedit.Equals("1"))
                        eb.Visible = false;
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                }
                if (drv["FLDTOTAL"] != null && drv["FLDTOTAL"].ToString() != "")
                    CompetenceRatingTotal = int.Parse(drv["FLDTOTAL"].ToString());
                RadLabel lblCrewAppraisalCompetenceId = (RadLabel)e.Item.FindControl("lblCrewAppraisalCompetenceId");
                RadLabel lblEvaluationQuestionId = (RadLabel)e.Item.FindControl("lblEvaluationQuestionId");
                RadLabel lblRating = (RadLabel)e.Item.FindControl("lblRating");
                UserControlNumber ucRatingItem = (UserControlNumber)e.Item.FindControl("ucRatingItem");
                LinkButton cmdTraining = (LinkButton)e.Item.FindControl("cmdTraining");

                if (cmdTraining != null)
                    cmdTraining.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid=" + Filter.CurrentCrewSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection
                        + "&vesselid=" + ViewState["APPVESSEL"].ToString()
                        + "&CompetenceId=" + lblCrewAppraisalCompetenceId.Text
                        + "&CategoryId=" + lblEvaluationQuestionId.Text
                        + "&rating=" + ucRatingItem.Text + "'); return true;");

                if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text))
                {
                    CompCount += 1;
                    //ratingtotal += int.Parse(lblRating.Text == "" ? "0" : lblRating.Text);
                }
                RadCheckBox chkMandatory = (RadCheckBox)e.Item.FindControl("chkMandatory");

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
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
                //{
                e.Item.Cells[3].Visible = false;
                e.Item.Cells[4].Visible = false;

                e.Item.Cells[1].Visible = true;
                e.Item.Cells[2].Visible = true;
                

                RadLabel lblTotal = (RadLabel)e.Item.FindControl("lblFooterTotal");
                int CompetenceTotal = 0;
                CompetenceTotal = (gvCrewCompetenceAppraisal.Items.Count) * 10;
                if (lblTotal != null)
                {
                    if (gvCrewCompetenceAppraisal.Items.Count == 0)
                        lblTotal.Text = "0" + " / " + "0";
                    else
                        lblTotal.Text = CompetenceRatingTotal.ToString() + " / " + CompetenceTotal.ToString();
                    
                    CompetenceRatingTotal = 0;
                }
                CompetenceFilledCount = CompCount;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewConductAppraisal_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewConductAppraisal');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewConductAppraisal');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCompetenceAppraisal_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewCompetenceAppraisal');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewCompetenceAppraisal');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewProfileAppraisalConduct_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalConduct');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalConduct');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewProfileAppraisalAttitude_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalAttitude');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalAttitude');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewProfileAppraisalLeadership_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalLeadership');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalLeadership');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewProfileAppraisalJudgementCommonSense_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                e.Item.TabIndex = 0;
                e.Item.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0}, null,'gvCrewProfileAppraisalJudgementCommonSense');", e.Item.ItemIndex);
                e.Item.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewProfileAppraisalJudgementCommonSense');";
                e.Item.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewCompetenceAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindCompetenceData();
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
    protected void gvCrewProfileAppraisalAttitude_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvCrewProfileAppraisalJudgementCommonSense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvCrewProfileAppraisalLeadership_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}