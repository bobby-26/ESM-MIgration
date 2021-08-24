using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionMOCRequestEvalutionApproval : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgPersonInCharge.Attributes.Add("onclick", "return showPickList('spnPersonInCharge', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + Request.QueryString["VESSELID"].ToString() + "', true); ");
        imgPersonInChargeOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        imgPersonInChargeApproval.Attributes.Add("onclick", "return showPickList('spnApprovedBy', 'codehelp1', '', '../Inspection/InspectionMOCApproverUserList.aspx?MOCID=" + Request.QueryString["MOCID"].ToString() + "', true); ");

        lnkimage.Attributes.Add("onclick", "return showPickList('spnApprovalPersonOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            confirm.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarstatus = new PhoenixToolbar();
            toolbarstatus.AddButton("List", "LIST", ToolBarDirection.Left);
            toolbarstatus.AddButton("Details", "DETAILS", ToolBarDirection.Left);
            toolbarstatus.AddButton("Action Plan", "ACTIONPLAN", ToolBarDirection.Left);
            toolbarstatus.AddButton("Evaluation & Approval", "EVALUATION", ToolBarDirection.Left);
            toolbarstatus.AddButton("Intermediate Verification", "INTERMEDIATE", ToolBarDirection.Left);
            toolbarstatus.AddButton("Implementation & Verification", "IMPLEMENTATION", ToolBarDirection.Left);

            MenuMOCStatus.MenuList = toolbarstatus.Show();
            MenuMOCStatus.SelectedMenuIndex = 3;

            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && !(string.IsNullOrEmpty(Request.QueryString["MOCID"])))
            {
                //toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCApprovalExtn.aspx?MOCID=" + Request.QueryString["MOCID"].ToString() + "')", "Request Approval", "REQUESTA", ToolBarDirection.Right);
            }
            if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

            if (!IsPostBack)
            {
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                ViewState["MOCSTATUS"] = "";
                if (ViewState["VESSELID"].ToString() != "0")
                {
                    spnPersonInCharge.Visible = true;
                    spnPersonInChargeOffice.Visible = false;
                }
                else
                {
                    spnPersonInChargeOffice.Visible = true;
                    spnPersonInCharge.Visible = false;
                }
                BindMocFrequency();
                BindMOCEvaluationEdit();
            }
            BindMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        if (ViewState["MOCSTATUS"].ToString() == "10") //Pending Evaluation
        {
            toolbar.AddButton("Request Approval", "REQUESTA", ToolBarDirection.Right);
        }

        if (ViewState["MOCSTATUS"].ToString() == "3") //Request Approval
        {
            toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionMOCApprovalExtn.aspx?MOCID=" + Request.QueryString["MOCID"].ToString() + "')", "Approve / Reject", "APPROVEREJECT", ToolBarDirection.Right);
        }

        MenuMOCApproveStatus.AccessRights = this.ViewState;
        MenuMOCApproveStatus.MenuList = toolbar.Show();
    }
    private void BindMOCEvaluationEdit()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCEvaluationProposal.MOCEvaluationApprovalEdit(new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            rbnnecessary.SelectedValue = dr["FLDEVALPROPOSALYN"].ToString();
            txtremarks.Text = dr["FLDEVALPROPOSALREMARKS"].ToString();
            rbnrequested.SelectedValue = dr["FLDSECTIONREQYN"].ToString();
            txtremarks1.Text = dr["FLDSECTIONREQDETAILS"].ToString();
            rcbIntermediateFrequency.SelectedValue = dr["FLDINTERMEDIATEYN"].ToString();
            txtremarks2.Text = dr["FLDINTERMEDIATEDETAILS"].ToString();
        }
        ds = PhoenixInspectionMOCApprovalForChange.MOCApprovalEdit(new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            rdnApprovalQ1.SelectedValue = dr["FLDAPPROVALRESPONSE"].ToString();
            txtApprovalQ1Remarks.Text = dr["FLDAPPROVALREMARKS"].ToString();
            rdnApprovalQ2.SelectedValue = dr["FLDAPPROVALESTIMATEYN"].ToString();
            txtApprovalQ2Remarks.Text = dr["FLDAPPROVALESTIMATEREMARKS"].ToString();
            rdnApprovalQ3.SelectedValue = dr["FLDAPPROVALDATEACCEPTEDYN"].ToString();
            txtApprovalQ3Remarks.Text = dr["FLDAPPROVALDATEACCEPTEDREMARKS"].ToString();
            rbnMOCCommitte.SelectedValue = dr["FLDAPPROVALMOCCOMMITTEEINVOLVED"].ToString();

            txtApprovalPerson.Text = dr["FLDMOCAPPROVALNAME"].ToString();
            txtApprovalDesignation.Text = dr["FLDMOCAPPROVALRANK"].ToString();
            txtPersonInChargeApproverId.Text = dr["FLDMOCAPPROVALUSERID"].ToString();

            if (rbnMOCCommitte.SelectedValue == "1")
                tblMOCCommitee.Visible = true;
            else
                tblMOCCommitee.Visible = false;

        }
        ds = PhoenixInspectionMOCTemplate.MOCEdit(new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (ViewState["VESSELID"].ToString() == "0")
            {
                txtPersonInChargeOfficeId.Text = dr["FLDEVALPICID"].ToString();
                txtOfficePersonName.Text = dr["FLDEVALPICNAME"].ToString();
                txtOfficePersonDesignation.Text = dr["FLDEVALPICRANK"].ToString();
            }
            else
            {
                txtCrewId.Text = dr["FLDEVALPICID"].ToString();
                txtCrewName.Text = dr["FLDEVALPICNAME"].ToString();
                txtCrewRank.Text = dr["FLDEVALPICRANK"].ToString();
            }
            ViewState["MOCSTATUS"] = dr["FLDMOCSTATUSID"].ToString();

            txtApprovalPersonName.Text = dr["FLDMOCCOMMITTEEMEMBERNAME"].ToString();
            txtApprovalPersonRank.Text = dr["FLDMOCCOMMITTEEMEMBERRANK"].ToString();
            txtApprovalPersonOfficeId.Text = dr["FLDMOCCOMMITTEEMEMBERID"].ToString();
            txtApprovalPersonOfficeEmail.Text = dr["FLDMOCCOMMITTEEMEMBERNAME"].ToString();
            ucMOCMeetingDate.Text = dr["FLDMOCMEETINGDATE"].ToString();
        }
    }

    private void BindMocFrequency()
    {
        rcbIntermediateFrequency.DataSource = PhoenixInspectionMOCEvaluationProposal.ListMOCFrequency();
        rcbIntermediateFrequency.DataTextField = "FLDMOCFEQUENCYDATES";
        rcbIntermediateFrequency.DataValueField = "FLDMOCFREQUENCYID";
        rcbIntermediateFrequency.DataBind();
        rcbIntermediateFrequency.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MenuMOCApproveStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidMOCEvaluation(rcbIntermediateFrequency.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMOCEvaluationProposal.MOCEvaluationApproalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                  , General.GetNullableGuid(ViewState["MOCID"].ToString())
                                                  , int.Parse(ViewState["VESSELID"].ToString())
                                                  , int.Parse(rbnnecessary.SelectedValue)
                                                  , General.GetNullableString(txtremarks.Text)
                                                  , int.Parse(rbnrequested.SelectedValue)
                                                  , General.GetNullableString(txtremarks1.Text)
                                                  , int.Parse(rcbIntermediateFrequency.SelectedValue)
                                                  , General.GetNullableString(txtremarks2.Text));

                PhoenixInspectionMOCApprovalForChange.MOCApprovalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                              , new Guid((ViewState["MOCID"]).ToString())
                                              , int.Parse(ViewState["VESSELID"].ToString())
                                              , int.Parse(rdnApprovalQ1.SelectedValue)
                                              , txtApprovalQ1Remarks.Text
                                              , int.Parse(rdnApprovalQ2.SelectedValue)
                                              , txtApprovalQ2Remarks.Text
                                              , int.Parse(rdnApprovalQ3.SelectedValue)
                                              , txtApprovalQ3Remarks.Text
                                              , int.Parse(rbnMOCCommitte.SelectedValue)
                                              , txtPersonInChargeApproverId.Text
                                              , txtApprovalPerson.Text
                                              , txtApprovalDesignation.Text);

                PhoenixInspectionMOCTemplate.MOCEvalutaionApprovalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                           , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                           , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableInteger(txtPersonInChargeOfficeId.Text) : General.GetNullableInteger(txtCrewId.Text)
                                           , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableString(txtOfficePersonName.Text) : General.GetNullableString(txtCrewName.Text)
                                           , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableString(txtOfficePersonDesignation.Text) : General.GetNullableString(txtCrewRank.Text)
                                           , General.GetNullableDateTime(ucMOCMeetingDate.Text)
                                           , General.GetNullableString(txtApprovalPersonOfficeId.Text)
                                           , General.GetNullableString(txtApprovalPersonName.Text)
                                           , General.GetNullableString(txtApprovalPersonRank.Text));

                ucStatus.Text = "Saved";
                BindMOCEvaluationEdit();
                BindMenu();
            }
            if (CommandName.ToUpper().Equals("REQUESTA"))
            {
                RadWindowManager1.RadConfirm("Are you completed the Evalutaion process ?", "confirm", 320, 150, null, "Request Approve");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMOCEvaluation(string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Equals(""))
            ucError.ErrorMessage = "MOC Intermediate Frequency is Required";

        return (!ucError.IsError);
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
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("INTERMEDIATE"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("IMPLEMENTATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestImplementationVerification.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rdnnecessary_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbnnecessary.SelectedValue == "2")
        {
            txtremarks.Enabled = true;
        }
        else
        {
            txtremarks.Enabled = false;
        }
    }

    protected void rdnrequested_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbnrequested.SelectedValue == "2")
        {
            txtremarks1.Enabled = true;
        }
        else
        {
            txtremarks1.Enabled = false;
        }
    }
    protected void rcbIntermediateFrequency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rcbIntermediateFrequency.SelectedValue != "0")
        {
            txtremarks2.Enabled = true;
        }
        else
        {
            txtremarks2.Enabled = false;
        }
    }

    protected void rdnApprovalQ1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdnApprovalQ1.SelectedValue == "2")
        {
            txtApprovalQ1Remarks.Enabled = true;
        }
        else
        {
            txtApprovalQ1Remarks.Enabled = false;
        }
    }

    protected void rdnApprovalQ2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdnApprovalQ2.SelectedValue == "2")
        {
            txtApprovalQ2Remarks.Enabled = true;
        }
        else
        {
            txtApprovalQ2Remarks.Enabled = false;
        }
    }
    protected void rdnApprovalQ3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdnApprovalQ3.SelectedValue == "2")
        {
            txtApprovalQ3Remarks.Enabled = true;
        }
        else
        {
            txtApprovalQ3Remarks.Enabled = false;
        }
    }

    protected void rbnMOCCommitte_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbnMOCCommitte.SelectedValue == "1")
        {
            tblMOCCommitee.Visible = true;
        }
        else
        {
            tblMOCCommitee.Visible = false;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["MOCID"] = null;

        RebindEvaluationProposal();
    }

    private void BindEvaluationProposal()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCEvaluationProposal.MOCEvaluationProposalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , General.GetNullableGuid(ViewState["MOCID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindApproval()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCApprovalForChange.MOCApprovalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , new Guid((ViewState["MOCID"]).ToString()));

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCEvaluationProposal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCEvaluationProposal.MOCEvaluationProposalDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(((RadLabel)e.Item.FindControl("lblMOCEvaluationProposalid")).Text));
                RebindEvaluationProposal();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCEvaluationProposal_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    eb.Visible = true;
                else
                    eb.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            RadLabel lblResponse = ((RadLabel)e.Item.FindControl("lblResponse"));
            RadLabel lblDescription = ((RadLabel)e.Item.FindControl("lblDescription"));
            RadLabel lblshortcodeedit = ((RadLabel)e.Item.FindControl("lblshortcodeedit"));

            DropDownList ddlResponseEdit = ((DropDownList)e.Item.FindControl("ddlResponseEdit"));
            DropDownList ddlIntermediateVerificationEdit = ((DropDownList)e.Item.FindControl("ddlIntermediateVerificationEdit"));
            string mochardcode = "";

            DataSet ds;
            ds = PhoenixInspectionMOCTemplate.mochardcode(15, "EQ3");

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                mochardcode = dr["FLDMOCHARDCODE"].ToString();
            }

            if (e.Item.IsInEditMode)
            {
                if (lblshortcodeedit.Text == "95")
                {
                    ddlResponseEdit.Visible = false;
                    ddlIntermediateVerificationEdit.SelectedValue = lblResponse.Text;
                }
                else
                {
                    ddlIntermediateVerificationEdit.Visible = false;
                    ddlResponseEdit.SelectedValue = lblResponse.Text;
                }
            }
        }
    }

    protected void gvMOCEvaluationProposal_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCEvaluationProposal((((RadLabel)e.Item.FindControl("lblmochardcodeedit")).Text == "EQ3") ? (((DropDownList)e.Item.FindControl("ddlIntermediateVerificationEdit")).SelectedValue) : (((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue), ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCEvaluationProposal.MOCEvaluationProposalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCEvaluationProposaledit")).Text)
                                                         , new Guid((ViewState["MOCID"]).ToString())
                                                         , int.Parse(ViewState["VESSELID"].ToString())
                                                         , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblshortcodeedit")).Text)
                                                         , (((RadLabel)e.Item.FindControl("lblmochardcodeedit")).Text == "EQ3") ? Int32.Parse(((DropDownList)e.Item.FindControl("ddlIntermediateVerificationEdit")).SelectedValue) : Int32.Parse(((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue)
                                                         , (((RadLabel)e.Item.FindControl("lblmochardcodeedit")).Text == "EQ3") ? Convert.ToString(((DropDownList)e.Item.FindControl("ddlIntermediateVerificationEdit")).Text) : Convert.ToString(((DropDownList)e.Item.FindControl("ddlResponseEdit")).Text)
                                                         , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text);
            BindEvaluationProposal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMOCEvaluationProposal(string description, string text)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((description.Equals("2") && text == ""))
            ucError.ErrorMessage = "Description is Required";

        if (description.Equals("Dummy"))
            ucError.ErrorMessage = "Answer is Required";

        return (!ucError.IsError);
    }


    protected void gvMOCEvaluationProposal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCEvaluationProposal.CurrentPageIndex + 1;
            BindEvaluationProposal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindEvaluationProposal()
    {
        //gvMOCEvaluationProposal.SelectedIndexes.Clear();
        //gvMOCEvaluationProposal.EditIndexes.Clear();
        //gvMOCEvaluationProposal.DataSource = null;
        //gvMOCEvaluationProposal.Rebind();
    }

    protected void gvMOCApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindApproval();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCApproval_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            RadLabel lblResponse = ((RadLabel)e.Item.FindControl("lblResponse"));
            DropDownList ddlResponseEdit = ((DropDownList)e.Item.FindControl("ddlResponseEdit"));

            if (e.Item.IsInEditMode)
            {
                ddlResponseEdit.SelectedValue = lblResponse.Text;
            }
        }
    }

    private bool IsValidMOCApproval(string description, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((description.Equals("2")) && (type == ""))
            ucError.ErrorMessage = "Remarks is Required";

        if (description.Equals("Dummy"))
            ucError.ErrorMessage = "Answer is Required";

        return (!ucError.IsError);
    }

    protected void gvMOCApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCApprovalForChange.MOCApprovalDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , new Guid(((RadLabel)e.Item.FindControl("lblMOCApprovalid")).Text));
                BindApproval();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionMOCTemplate.MOCStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCID"].ToString()));
            ucStatus.Text = "MOC Status Changed to Pending Approval";
            BindMOCEvaluationEdit();
            BindMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}

