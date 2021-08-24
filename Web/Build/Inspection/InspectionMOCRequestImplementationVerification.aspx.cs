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
public partial class InspectionMOCRequestImplementationVerification : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //imgEvaluationPersonOffice.Attributes.Add("onclick", "return showPickList('spnEvaluationPersonOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");
        //imgEvaluationPersonCrew.Attributes.Add("onclick", "return showPickList('spnEvaluationPersonCrew', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + ViewState["VESSELID"].ToString() + "', true); ");
        //imgapprovedbyCrew.Attributes.Add("onclick", "return showPickList('spnapprovedbyCrew', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + ViewState["Vesselid"].ToString() + "', true); ");

        imgPersonInCharge.Attributes.Add("onclick", "return showPickList('spnPersonInCharge', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + Request.QueryString["VESSELID"].ToString() + "', true); ");
        imgPersonInChargeOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        lnkimage.Attributes.Add("onclick", "return showPickList('spnApprovalPersonOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            Impconfirm.Attributes.Add("style", "display:none;");
            Vericonfirm.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarstatus = new PhoenixToolbar();
            toolbarstatus.AddButton("List", "LIST", ToolBarDirection.Left);
            toolbarstatus.AddButton("Details", "DETAILS", ToolBarDirection.Left);
            toolbarstatus.AddButton("Action Plan", "ACTIONPLAN", ToolBarDirection.Left);
            toolbarstatus.AddButton("Evaluation & Approval", "EVALUATION", ToolBarDirection.Left);
            toolbarstatus.AddButton("Intermediate Verification", "INTERMEDIATE", ToolBarDirection.Left);
            toolbarstatus.AddButton("Implementation & Verification", "IMPLEMENTATION", ToolBarDirection.Left);

            MenuMOCStatus.MenuList = toolbarstatus.Show();
            MenuMOCStatus.SelectedMenuIndex = 5;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            
            if (!IsPostBack)
            {
                ViewState["MOCSTATUS"] = "";

                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                if (ViewState["VESSELID"].ToString() != "0")
                {
                    spnPersonInChargeOffice.Visible = false;
                    spnPersonInCharge.Visible = true;
                }
                else
                {
                    spnPersonInChargeOffice.Visible = true;
                    spnPersonInCharge.Visible = false;
                }
                BindMocEdit();
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (ViewState["MOCSTATUS"].ToString() == "6")
                {
                    toolbar.AddButton("Implement", "IMPLEMENT", ToolBarDirection.Right);
                }

                if (ViewState["MOCSTATUS"].ToString() == "9")
                {
                    toolbar.AddButton("Verify", "VERIFY", ToolBarDirection.Right);
                }
            }
            MenuMOCVerification.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void MenuMOCVerification_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixInspectionMOCImplementation.MOCImplementationInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , new Guid((ViewState["MOCID"]).ToString())
                                                             , int.Parse(ViewState["VESSELID"].ToString())
                                                             , General.GetNullableDateTime(dteImplementationDate.Text)
                                                             , int.Parse(rbnImpQ2.SelectedValue)
                                                             , txtImpQ2.Text
                                                             , int.Parse(chkconfirmation.Checked == true ? "1" : "0".ToString())
                                                             , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableInteger(txtPersonInChargeOfficeId.Text) : General.GetNullableInteger(txtCrewId.Text)
                                                             , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableString(txtOfficePersonName.Text) : General.GetNullableString(txtCrewName.Text)
                                                             , (ViewState["VESSELID"].ToString() == "0") ? General.GetNullableString(txtOfficePersonDesignation.Text) : General.GetNullableString(txtCrewRank.Text)
                                                             );

                PhoenixInspectionMOCVerificationOnCompletion.MOCVerificationOnCompletionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid((ViewState["MOCID"]).ToString())
                                                            , int.Parse(ViewState["VESSELID"].ToString())
                                                            , Int32.Parse(rbnverifiQ1.SelectedValue)
                                                            , txtverifiQ1Remarks.Text
                                                            , int.Parse(rbnverifiQ2.SelectedValue)
                                                            , txtVerifiQ2Remarks.Text
                                                            , int.Parse(chkMOCConfirmation.Checked == true ? "1" : "0".ToString())
                                                            , General.GetNullableString(txtApprovalPersonOfficeId.Text)
                                                            , General.GetNullableString(txtApprovalPersonName.Text)
                                                            , General.GetNullableString(txtApprovalPersonRank.Text)
                                                            );
                ucStatus.Text = "Saved";
            }

            if (CommandName.ToUpper().Equals("IMPLEMENT"))
            {
                RadWindowManager1.RadConfirm("Are you sure want to Implement ?", "Impconfirm", 320, 150, null, "Implement");
            }
            if (CommandName.ToUpper().Equals("VERIFY"))
            {
                RadWindowManager1.RadConfirm("Are you completed the verification ?", "Vericonfirm", 320, 150, null, "Verification");
            }
            BindMocEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMocEdit()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCImplementation.MOCImplementationEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dteImplementationDate.Text = dr["FLDMOCMAXIMPLIMENTDATE"].ToString();
           // ucimplamentationdate.Text = dr["FLDMOCIMPLIMENTDATE"].ToString();
            rbnImpQ2.SelectedValue = dr["FLDIMPLEMENTATIONRESPONSE"].ToString();
            txtImpQ2.Text = dr["FLDIMPLEMENTATIONREMARKS"].ToString();
            ViewState["MOCSTATUS"] = dr["FLDSTATUS"].ToString();
            txtApprovalPersonName.Text = dr["FLDVERIFIEDPERSONNAME"].ToString();
            txtApprovalPersonRank.Text = dr["FLDVERIFIEDPERSONRANK"].ToString();
            txtApprovalPersonOfficeId.Text = dr["FLDVERIFIEDPERSONID"].ToString();
            if (dr["FLDIMPLEMENTATIONCONFIRMATION"].ToString() == "1")
                chkconfirmation.Checked = true;
            else
                chkconfirmation.Checked = false;

            txtverifiQ1Remarks.Text = dr["FLDMOCVERIFICATIONREMARKS"].ToString();
            txtVerifiQ2Remarks.Text = dr["FLDMOCFEEDBACKREMARKS"].ToString();
            rbnverifiQ2.SelectedValue = dr["FLDMOCFEEDBACKYN"].ToString();
            rbnverifiQ1.SelectedValue = dr["FLDMOCVERIFICATIONRESPONSE"].ToString();

            if (dr["FLDCOMPLETEDYN"].ToString() == "1")
                chkMOCConfirmation.Checked = true;
            else
                chkMOCConfirmation.Checked = false;


            if (ViewState["VESSELID"].ToString() == "0")
            {
                txtPersonInChargeOfficeId.Text = dr["FLDIMPLEMENTATIONPICID"].ToString();
                txtOfficePersonName.Text = dr["FLDIMPLEMENTATIONPICNAME"].ToString();
                txtOfficePersonDesignation.Text = dr["FLDIMPLEMENTATIONPICRANK"].ToString();
            }
            else
            {
                txtCrewId.Text = dr["FLDIMPLEMENTATIONPICID"].ToString();
                txtCrewName.Text = dr["FLDIMPLEMENTATIONPICNAME"].ToString();
                txtCrewRank.Text = dr["FLDIMPLEMENTATIONPICRANK"].ToString();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (ViewState["MOCSTATUS"].ToString() == "6")
                {
                    toolbar.AddButton("Implement", "IMPLEMENT", ToolBarDirection.Right);
                }

                if (ViewState["MOCSTATUS"].ToString() == "9")
                {
                    toolbar.AddButton("Verify", "VERIFY", ToolBarDirection.Right);
                }
            }
            MenuMOCVerification.MenuList = toolbar.Show();
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
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("ACTIONPLAN"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("EVALUATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestEvalutionApproval.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["MOCID"] = null;

        RebindEvaluationProposal();
    }

    private void BindImplementation()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCImplementation.MOCImplementationList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid((ViewState["MOCID"]).ToString()));

            //gvMOCImplementation.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCImplementation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCImplementation.MOCImplementationDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , new Guid(((RadLabel)e.Item.FindControl("lblMOCImplementationid")).Text));
                RebindEvaluationProposal();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCImplementation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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

    protected void gvMOCImplementation_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCImplementation(((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue, ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            //PhoenixInspectionMOCImplementation.MOCImplementationInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //                                             , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCImplementationid")).Text)
            //                                             , new Guid((ViewState["MOCID"]).ToString())
            //                                             , int.Parse(ViewState["VESSELID"].ToString())
            //                                             , General.GetNullableDateTime(null)
            //                                             , Int32.Parse(((RadLabel)e.Item.FindControl("lblmochardcode")).Text)
            //                                             , Int32.Parse(((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue)
            //                                             , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text);
            BindImplementation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMOCImplementation(string description, string text)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((description.Equals("2") && text == ""))
            ucError.ErrorMessage = "Description is Required";

        if (description.Equals("Dummy"))
            ucError.ErrorMessage = "Answer is Required";

        return (!ucError.IsError);
    }


    protected void gvMOCImplementation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCImplementation.CurrentPageIndex + 1;
            BindImplementation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindEvaluationProposal()
    {
        //gvMOCImplementation.SelectedIndexes.Clear();
        //gvMOCImplementation.EditIndexes.Clear();
        //gvMOCImplementation.DataSource = null;
        //gvMOCImplementation.Rebind();
    }

    private void BindVerificationoncompletion()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCVerificationOnCompletion.MOCVerificationOnCompletionList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid((ViewState["MOCID"]).ToString()));
            //gvMOCVerification.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCVerification_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindVerificationoncompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCVerification_ItemDataBound(Object sender, GridItemEventArgs e)
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

    private bool IsValidMOCVerificationOnCompletion(string description, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((description.Equals("2")) && (type == ""))
            ucError.ErrorMessage = "Remarks is Required";

        if (description.Equals("Dummy"))
            ucError.ErrorMessage = "Answer is Required";

        return (!ucError.IsError);
    }

    protected void gvMOCVerification_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidMOCVerificationOnCompletion(((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue, ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            //PhoenixInspectionMOCVerificationOnCompletion.MOCVerificationOnCompletionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //                                                                       , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCVerificationid")).Text)
            //                                                                       , new Guid((ViewState["MOCID"]).ToString())
            //                                                                       , int.Parse(ViewState["VESSELID"].ToString())
            //                                                                       , Int32.Parse(((RadLabel)e.Item.FindControl("lblDescription")).Text)
            //                                                                       , Int32.Parse(((DropDownList)e.Item.FindControl("ddlResponseEdit")).SelectedValue)
            //                                                                       , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text);
            ucStatus.Text = "Approval Updated";
            BindVerificationoncompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCVerification_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCVerificationOnCompletion.MOCVerificationOnCompletionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(((RadLabel)e.Item.FindControl("lblMOCVerificationid")).Text));
                BindVerificationoncompletion();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Impconfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionMOCTemplate.MOCImplementationStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCID"].ToString()));
            ucStatus.Text = "MOC Status Updated";     
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void Vericonfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionMOCTemplate.MOCVerificationStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["MOCID"].ToString()));
            ucStatus.Text = "MOC Status Updated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
