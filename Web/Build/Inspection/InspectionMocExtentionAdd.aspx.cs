using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class InspectionMocExtentionAdd : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgextentionoffice.Attributes.Add("onclick", "return showPickList('Spnextentionoffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucConfirmRevision.Attributes.Add("style", "display:none");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MOCExtentionAdd.AccessRights = this.ViewState;
            MOCExtentionAdd.MenuList = toolbar.Show();

            DataSet ds = PhoenixInspectionMOCRequestForChange.MOCRequestEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(Request.QueryString["MOCID"].ToString()));
            OriginalDate.Text = ds.Tables[0].Rows[0]["FLDIMPLEMENTATIONTARGETDATE"].ToString();
            ViewState["MOCREQUESTID"] = ds.Tables[0].Rows[0]["FLDMOCREQUESTID"].ToString();
            txtextentionofficemail.Attributes.Add("style", "display:none");
            txtextentionofficeid.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                ViewState["MOCEXTENTIONID"] = "";
                ViewState["MOCID"] = "";
                ViewState["REV"] = "";

                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                {
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
                    BindRA();
                }

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                }

                if (Request.QueryString["MOCEXTENTIONID"] != null && Request.QueryString["MOCEXTENTIONID"].ToString() != string.Empty)
                {
                    ViewState["MOCEXTENTIONID"] = Request.QueryString["MOCEXTENTIONID"].ToString();
                    BindMOCExtention();
                }
                if (ViewState["REV"].ToString() == string.Empty)
                {
                    imgrevision.Visible = false;
                }

                RiskAssessmentGenericEdit();

            }

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            toolbarmenu.AddFontAwesomeButton("../Inspection/InspectionMOCExtentionActionPlanEdit.aspx?EXTN=1&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCEXTENTIONID=" + ViewState["MOCEXTENTIONID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CORRECTIVEACTIONADD");
            MOCActionPlan.AccessRights = this.ViewState;
            MOCActionPlan.MenuList = toolbarmenu.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMOCActionPlan.SelectedIndexes.Clear();
        gvMOCActionPlan.EditIndexes.Clear();
        gvMOCActionPlan.DataSource = null;
        gvMOCActionPlan.Rebind();
    }
    private void BindActionPlan()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCExtention.MOCExtentionActionPlanList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableGuid(ViewState["MOCID"].ToString())
                                                                                        , General.GetNullableGuid(null)
                                                                                        , General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString()));

            gvMOCActionPlan.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MOCExtentionAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["MOCEXTENTIONID"] != null && ViewState["MOCEXTENTIONID"].ToString() != string.Empty)
                    UpdateMOCExtention();
                else
                    InsertMOCExtention();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCREQUESTID=" + ViewState["MOCREQUESTID"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InsertMOCExtention()
    {
        Guid? NewextentionId = null;

        if (!IsValidMocExtention())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionMOCExtention.MOCExtentionInsert(General.GetNullableGuid(ViewState["MOCID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableInteger(rblMOCExtetion.SelectedValue), General.GetNullableInteger(rblActionPlan.SelectedValue)
            , General.GetNullableInteger(rblMOCCommitteInvoledExtention.SelectedValue), General.GetNullableDateTime(txtMOCMeetingExtention.Text)
            , General.GetNullableString(txtMOCcommitteeExtention.Text), General.GetNullableString(txtMOCcommitteeExtention.Text)
            , General.GetNullableString(txtMOCcommitteeExtention.Text), General.GetNullableInteger(txtextentionofficeid.ToString())
            , General.GetNullableString(txtextentionofficename.Text), General.GetNullableString(txtextentionofficerank.Text)
            , General.GetNullableDateTime(RevisedDate.Text), General.GetNullableInteger(rblRAReview.SelectedValue), ref NewextentionId);

        ucStatus.Text = "Successfully updated";

        Response.Redirect("../Inspection/InspectionMOCExtentionAdd.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCEXTENTIONID=" + NewextentionId);
    }

    protected void UpdateMOCExtention()
    {
        if (!IsValidMocExtention())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionMOCExtention.MOCExtentionUpdate(General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString()), General.GetNullableInteger(rblMOCExtetion.SelectedValue), General.GetNullableInteger(rblActionPlan.SelectedValue), General.GetNullableInteger(rblMOCCommitteInvoledExtention.SelectedValue)
            , General.GetNullableDateTime(txtMOCMeetingExtention.Text), General.GetNullableString(txtMOCcommitteeExtention.Text), General.GetNullableString(txtMOCcommitteeExtention.Text), General.GetNullableString(txtMOCcommitteeExtention.Text), General.GetNullableInteger(txtextentionofficeid.ToString()), General.GetNullableString(txtextentionofficename.Text)
            , General.GetNullableString(txtextentionofficerank.Text), General.GetNullableDateTime(RevisedDate.Text), General.GetNullableInteger(rblRAReview.SelectedValue));

        ucStatus.Text = "Successfully updated";
    }
    protected void gvMOCActionPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
    protected void gvMOCActionPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCActionPlan.CurrentPageIndex + 1;
            BindActionPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCActionPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lblMOCActionPlanid = (RadLabel)e.Item.FindControl("lblMOCActionPlanid");
                Response.Redirect("../Inspection/InspectionMOCExtentionActionPlanEdit.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&ACTIONPLANID=" + lblMOCActionPlanid.Text + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCEXTENTIONID=" + ViewState["MOCEXTENTIONID"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCActionPlan.MOCActionPlanDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblMOCActionPlanid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    private void BindMOCExtention()
    {
        if (ViewState["MOCEXTENTIONID"] != null && !string.IsNullOrEmpty(ViewState["MOCEXTENTIONID"].ToString()))
        {

            DataTable dt = PhoenixInspectionMOCExtention.MOCExtentionEdit(General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString()));
            RevisedDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREVISEDDATE"].ToString());
            rblMOCExtetion.SelectedValue = dt.Rows[0]["FLDTARGRTCHANGEYN"].ToString();
            rblMOCCommitteInvoledExtention.SelectedValue = dt.Rows[0]["FLDMOCCOMMITTEEINVOILVEDYN"].ToString();
            txtMOCMeetingExtention.Text = General.GetDateTimeToString(dt.Rows[0]["FLDMOCCOMMITTEEMEETINGDATE"].ToString());
            txtMOCcommitteeExtention.Text = dt.Rows[0]["FLDMOCCOMMITTEEMEMBERNAME"].ToString();
            txtextentionofficeid.Text = dt.Rows[0]["FLDAPPROVALAUTHORITYID"].ToString();
            txtextentionofficename.Text = dt.Rows[0]["FLDAPPROVALAUTHORITYNAME"].ToString();
            txtextentionofficerank.Text = dt.Rows[0]["FLDAPPROVALAUTHORITYRANK"].ToString();
            rblActionPlan.SelectedValue = dt.Rows[0]["FLDMOCACTIONPLANREVIEWED"].ToString();
            rblRAReview.SelectedValue = dt.Rows[0]["FLDRAREVIEWEDYN"].ToString();
            ViewState["RAGENERICID"] = dt.Rows[0]["FLDRISKASSESSMENTID"].ToString();
            int status = int.Parse(dt.Rows[0]["FLDMOCSTATUS"].ToString());

            if (status == 4)
            {
                RevisedDate.Enabled = false;
            }

            if (rblMOCCommitteInvoledExtention.SelectedValue.Equals("1"))
            {
                txtMOCMeetingExtention.Enabled = true;
                txtMOCcommitteeExtention.Enabled = true;
                imgextentionoffice.Visible = true;
            }
        }
    }
    private void BindRA()
    {
        DataSet ds;

        ds = PhoenixInspectionMOCTemplate.MOCEdit(new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["RAGENERICID"] = dr["FLDRISKASSESSMENTID"].ToString();
            txtRANumber.Text = dr["FLDRANUMBER"].ToString();
            txtRAId.Text = dr["FLDRISKASSESSMENTID"].ToString();
            txtRA.Text = dr["FLDACTIVITYCONDITIONS"].ToString();
            ViewState["REV"] = dr["FLDREVYN"].ToString();
            if (ViewState["REV"].ToString() == "1")
            {
                imgrevision.Visible = true;
            }
            else if (ViewState["REV"].ToString() == "0" || ViewState["REV"].ToString() == string.Empty)
            {
                imgrevision.Visible = false;
            }
        }        
    }
    protected void rblMOCCommitteInvoledExtention_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblMOCCommitteInvoledExtention.SelectedValue.Equals("1"))
        {
            txtMOCMeetingExtention.Enabled = true;
            txtMOCcommitteeExtention.Enabled = true;
            imgextentionoffice.Visible = true;
        }
        else
        {
            txtMOCMeetingExtention.Enabled = false;
            txtMOCcommitteeExtention.Enabled = false;
            imgextentionoffice.Visible = false;
        }
    }
    private bool IsValidMocExtention()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(RevisedDate.Text))
            ucError.ErrorMessage = "Reviced date is required.";

        return (!ucError.IsError);
    }

    protected void lnkEditRA_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?status=4&RAType=1&genericid=" + ViewState["RAGENERICID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCEXTENTIONID=" + ViewState["MOCEXTENTIONID"].ToString() + "&Vesselname=", true);
    }

    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionRiskAssessmentGenericExtn.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                           new Guid(ViewState["RAGENERICID"].ToString()));

            PhoenixInspectionMOCExtention.MOCRARevision(General.GetNullableGuid(ViewState["MOCID"].ToString()));
            BindActionPlan();
            BindMOCExtention();
            BindRA();
            RiskAssessmentGenericEdit();
            ucStatus.Text = "RA Revised Successfully";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindActionPlan();
        BindMOCExtention();
        BindRA();
    }
}
