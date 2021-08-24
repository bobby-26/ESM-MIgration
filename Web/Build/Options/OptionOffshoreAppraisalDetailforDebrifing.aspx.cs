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

public partial class Options_OptionOffshoreAppraisalDetailforDebrifing : PhoenixBasePage
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
                ViewState["EMPID"] = "";

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
                toolbarmain.AddButton("Save & Finalize", "SAVE");

                //AppraisalTabs.AccessRights = this.ViewState;
                //AppraisalTabs.MenuList = toolbarmain.Show();

            }

            BindData();
            BindConductData();
            BindCompetenceData();
            SetEmployeePrimaryDetails();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void EditAppraisal()
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
                // txtSeafarerComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString());
                //noofvisits = ds.Tables[0].Rows[0]["FLDNOOFDOCTORVISIT"].ToString();
                //ddlNoOfDocVisits.SelectedValue = String.IsNullOrEmpty(noofvisits) ? "DUMMY" : noofvisits;

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString()) != null)
                    ddlWantToWorkAgainCrew.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINCREW"].ToString();

                if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString()) != null)
                    ddlWantToWorkAgainHOD.SelectedValue = ds.Tables[0].Rows[0]["FLDWANTTOWORKAGAINHOD"].ToString();

                if (chkRecommendPromotion.Checked)
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                //if (txtSeafarerComment.Text.Trim() == "")
                //{
                //    ucError.ErrorMessage = "Please enter the Seafarer Comment";
                //    ucError.Visible = true;
                //    return;
                //}
                //PhoenixCrewAppraisal.UpdateAppraisalSeamanComment(1,
                //   new Guid(Filter.CurrentAppraisalSelection)
                //   , txtSeafarerComment.Text);

                Response.Redirect("..\\CrewOffshore\\CrewOffshoreCrewCommentConfirmMsg.aspx?NAME=" + ViewState["NAME"].ToString() + "&RANK=" + ViewState["RANK"].ToString());
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

           // string vessel = ddlVessel.SelectedVessel;
            string fromdate = txtFromDate.Text;
            string todate = txtToDate.Text;
            string appraisaldate = txtdate.Text;
            string occassion = ddlOccassion.SelectedOccassion;

            if (dce.CommandName.ToUpper().Equals("SAVE") || dce.CommandName.ToUpper().Equals("SAVECHANGES"))
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
            DataSet ds1 = new DataSet();

            ds1 = PhoenixCrewAppraisal.EditAppraisal(new Guid(Request.QueryString["aprid"]));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                 Filter.CurrentCrewSelection = ds1.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["EMPID"] = ds1.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["VSLID"] = ds1.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["RANKID"] = ds1.Tables[0].Rows[0]["FLDRANKID"].ToString();
            }
            dt = PhoenixVesselAccountsEmployee.EditVesselCrew(int.Parse(ViewState["VSLID"].ToString()), Convert.ToInt32(ViewState["EMPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
               // ddlVessel.SelectedVessel = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();

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
                   // ddlVessel.SelectedValue = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                ConductCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalConduct.DataSource = ds.Tables[0];
                gvCrewProfileAppraisalConduct.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ConductCount = 0;
                ShowNoRecordsFound(dt, gvCrewProfileAppraisalConduct);
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "ATT"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                AttitudeCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalAttitude.DataSource = ds.Tables[0];
                gvCrewProfileAppraisalAttitude.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                AttitudeCount = 0;
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewProfileAppraisalAttitude);
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "JCS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                JudgementCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalJudgementCommonSense.DataSource = ds.Tables[0];
                gvCrewProfileAppraisalJudgementCommonSense.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                JudgementCount = 0;
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewProfileAppraisalJudgementCommonSense);
            }

            ds = PhoenixCrewAppraisalProfile.AppraisalProfileSearch(
                                                                 General.GetNullableGuid(Filter.CurrentAppraisalSelection)
                                                                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 91, "LDS"))
                                                               , int.Parse(ViewState["Rcategory"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                LeadershipCount = ds.Tables[0].Rows.Count;
                gvCrewProfileAppraisalLeadership.DataSource = ds.Tables[0];
                gvCrewProfileAppraisalLeadership.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                LeadershipCount = 0;
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewProfileAppraisalLeadership);
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
            Label lbRate = (Label)e.Row.FindControl("lblRating");
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
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
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
                gvCrewConductAppraisal.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                ProAppraisalCount = 0;
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewConductAppraisal);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int ProCount;
    int ProRatingTotal = 0;
    protected void gvCrewConductAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSIONSUB"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSUB"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTIONSUB"] == null || ViewState["SORTDIRECTIONSUB"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            ProCount = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbRate = (Label)e.Row.FindControl("lblRating");
            if (lbRate != null && !String.IsNullOrEmpty(lbRate.Text))
            {
                ProCount += 1;
                ProRatingTotal += int.Parse(lbRate.Text == "" ? "0" : lbRate.Text);
            }
            else
            {
                UserControlNumber rating = (UserControlNumber)e.Row.FindControl("ucRatingEdit");
                if (rating != null && !String.IsNullOrEmpty(rating.Text))
                {
                    ProRatingTotal += int.Parse(rating.Text == "" ? "0" : rating.Text);
                }
            }
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit1");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblFooterTotal = (Label)e.Row.FindControl("lblFooterTotal");
            int ProTotal = 0;
            ProTotal = (gvCrewConductAppraisal.Rows.Count) * 10;
            if (lblFooterTotal != null)
            {
                if (gvCrewConductAppraisal.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
                    lblFooterTotal.Text = "0" + " / " + "0";
                else
                    lblFooterTotal.Text = ProRatingTotal.ToString() + " / " + ProTotal.ToString();
                ProRatingTotal = 0;
            }
            ProAppraisalFilledCount = ProCount;
        }

    }
    protected void gvCrewConductAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewConductAppraisal');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewConductAppraisal');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
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
            ddlCategory.Items.Insert(0, new ListItem("--Select--", ""));
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
                gvCrewCompetenceAppraisal.DataBind();
                lblGrandTotal.Text = "Grand Total : " + ds.Tables[0].Rows[0]["FLDGRANDTOTAL"].ToString();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                CompetenceCount = 0;
                ShowNoRecordsFound(dt, gvCrewCompetenceAppraisal);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    int CompCount;
    int CompetenceRatingTotal = 0;
    protected void gvCrewCompetenceAppraisal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSIONSUB"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSUB"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTIONSUB"] == null || ViewState["SORTDIRECTIONSUB"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
            //{
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            //}
            CompCount = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
            //{
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            //}

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit1");
            if (eb != null)
            {
                if (!canedit.Equals("1"))
                    eb.Visible = false;
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                //    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            if (drv["FLDTOTAL"] != null && drv["FLDTOTAL"].ToString() != "")
                CompetenceRatingTotal = int.Parse(drv["FLDTOTAL"].ToString());
            Label lblCrewAppraisalCompetenceId = (Label)e.Row.FindControl("lblCrewAppraisalCompetenceId");
            Label lblEvaluationQuestionId = (Label)e.Row.FindControl("lblEvaluationQuestionId");
            Label lblRating = (Label)e.Row.FindControl("lblRating");
            UserControlNumber ucRatingItem = (UserControlNumber)e.Row.FindControl("ucRatingItem");
            ImageButton cmdTraining = (ImageButton)e.Row.FindControl("cmdTraining");
            if (cmdTraining != null)
                cmdTraining.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','../CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx?employeeid=" + Filter.CurrentCrewSelection + "&appraisalid=" + Filter.CurrentAppraisalSelection
                    + "&vesselid=" + ViewState["APPVESSEL"].ToString()
                    + "&CompetenceId=" + lblCrewAppraisalCompetenceId.Text
                    + "&CategoryId=" + lblEvaluationQuestionId.Text
                    + "&rating=" + ucRatingItem.Text + "'); return true;");
            if (lblRating != null && !String.IsNullOrEmpty(lblRating.Text))
            {
                CompCount += 1;
                //ratingtotal += int.Parse(lblRating.Text == "" ? "0" : lblRating.Text);
            }
            CheckBox chkMandatory = (CheckBox)e.Row.FindControl("chkMandatory");

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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 && canedit == "0")
            //{
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            //}
            Label lblTotal = (Label)e.Row.FindControl("lblFooterTotal");
            int CompetenceTotal = 0;
            CompetenceTotal = (gvCrewCompetenceAppraisal.Rows.Count) * 10;
            if (lblTotal != null)
            {
                if (gvCrewCompetenceAppraisal.Rows[0].Cells[0].Text.ToUpper() == "NO RECORDS FOUND")
                    lblTotal.Text = "0" + " / " + "0";
                else
                    lblTotal.Text = CompetenceRatingTotal.ToString() + " / " + CompetenceTotal.ToString();
                CompetenceRatingTotal = 0;
            }
            CompetenceFilledCount = CompCount;
        }
    }
    protected void gvCrewCompetenceAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null,'gvCrewCompetenceAppraisal');", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event, 'gvCrewCompetenceAppraisal');";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
}
