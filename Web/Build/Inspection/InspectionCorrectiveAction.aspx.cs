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
public partial class InspectionCorrectiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["CORRECTIVEACTIONID"] = null;
                ViewState["REFERENCEID"] = null;
                ViewState["REFFROM"] = null;
                ViewState["VESSELID"] = null;
                ViewState["View"] = "";

                if (Request.QueryString["CORRECTIVEACTIONID"] != null && Request.QueryString["CORRECTIVEACTIONID"].ToString() != string.Empty)
                    ViewState["CORRECTIVEACTIONID"] = Request.QueryString["CORRECTIVEACTIONID"].ToString();

                if (Request.QueryString["REFFROM"] != null && Request.QueryString["REFFROM"].ToString() != string.Empty)
                    ViewState["REFFROM"] = Request.QueryString["REFFROM"].ToString();

                if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
                    ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();

                if (Request.QueryString["View"] != null && Request.QueryString["View"].ToString() != string.Empty)
                    ViewState["View"] = Request.QueryString["View"].ToString();

                lblCompletionDate.Visible = false;
                ucCompletionDate.Visible = false;
                
                

                if (ViewState["REFFROM"].ToString() == "incident")
                {
                    lblChecklistRefNo.Visible = false;
                    lblDeficiencyDetails.Visible = false;
                    txtChecklistRefNo.Visible = false;
                    txtDeficiencyDetails.Visible = false;
                    txtCorrectiveAction.Focus();
                }
                else
                    txtChecklistRefNo.Focus();

                BindDeficiencyDetails();
                BindVesselid();
                BindCorrectiveAction();
                if (ViewState["CORRECTIVEACTIONID"] != null && General.GetNullableDateTime(ucTargetDate.Text) != null)
                {
                    SetRights();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if (ViewState["View"].ToString() == "1")
        {
            toolbar.AddButton("Back", "INCIDENTLIST", ToolBarDirection.Right);
        }
        else
        {
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        MenuCARGeneral.AccessRights = this.ViewState;
        MenuCARGeneral.MenuList = toolbar.Show();
    }

    protected void BindDeficiencyDetails()
    {
        DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REFERENCEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDCOPYTOCAR"].ToString() == "1")
            {
                txtChecklistRefNo.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDeficiencyDetails.Text = dr["FLDCOMPREHENSIVEDESCRIPTION"].ToString();
            }
        }

        DataSet ds1 = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["REFERENCEID"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            if (dr["FLDCOPYTOCAR"].ToString() == "1")
            {
                txtChecklistRefNo.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDeficiencyDetails.Text = dr["FLDOBSERVATION"].ToString();
            }
        }
    }

    protected void BindVesselid()
    {
        if (ViewState["REFERENCEID"] != null)
        {
            DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELID"] = int.Parse(dr["FLDVESSELID"].ToString());
            }
        }
    }

    protected void BindCorrectiveAction()
    {
        if (ViewState["CORRECTIVEACTIONID"] != null && ViewState["CORRECTIVEACTIONID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.EditCorrectiveAction(new Guid(ViewState["CORRECTIVEACTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];                
                txtCorrectiveAction.Text = dr["FLDCORRECTIVEACTION"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ucDept.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
                txtChecklistRefNo.Text = dr["FLDCACHECKLISTREFNUMBER"].ToString();
                txtDeficiencyDetails.Text = dr["FLDDEFICIENCYDETAILS"].ToString();
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["REFFROM"].ToString().Equals("nc"))
                {
                    Response.Redirect("../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REFFROM=nc&REVIEWDNC=" + ViewState["REFERENCEID"].ToString(), false);
                }
                else if (ViewState["REFFROM"].ToString().Equals("incident"))
                {
                    Response.Redirect("../Inspection/InspectionIncidentCriticalFactor.aspx", false);
                }
                else if (ViewState["REFFROM"].ToString().Equals("obs"))
                {
                    Response.Redirect("../Inspection/InspectionObservationCorrectiveAction.aspx?REFFROM=obs&DIRECTOBSERVATIONID=" + ViewState["REFERENCEID"].ToString(), false);
                }
            }
            if (CommandName.ToUpper().Equals("INCIDENTLIST"))
            {
                Response.Redirect("../Inspection/InspectionIncidentActionsView.aspx?inspectionincidentid=" + ViewState["REFERENCEID"].ToString(), false);
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CORRECTIVEACTIONID"] == null)
                    InsertCorrectiveAction();
                else
                    UpdateCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertCorrectiveAction()
    {
        if (!IsValidCorrectiveAction())
        {
            ucError.Visible = true;
            return;
        }
        if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            PhoenixInspectionCorrectiveAction.InsertIncidentCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(ucTargetDate.Text), null, null, General.GetNullableInteger(ucDept.SelectedDepartment),
                null,null, null,General.GetNullableInteger(ucVerficationLevel.SelectedHard));            
        }
        else if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            PhoenixInspectionCorrectiveAction.InsertNCCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                DateTime.Parse(ucTargetDate.Text),int.Parse(ViewState["VESSELID"].ToString()),null,
                General.GetNullableInteger(ucDept.SelectedDepartment), null, null, null, General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                General.GetNullableString(txtChecklistRefNo.Text),General.GetNullableString(txtDeficiencyDetails.Text));  
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            PhoenixInspectionCorrectiveAction.InsertObsCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                DateTime.Parse(ucTargetDate.Text), int.Parse(ViewState["VESSELID"].ToString()), null, General.GetNullableInteger(ucDept.SelectedDepartment),
                General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                General.GetNullableString(txtChecklistRefNo.Text), General.GetNullableString(txtDeficiencyDetails.Text));
        }
        ucStatus.Text = "Corrective Action inserted.";

        if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            Response.Redirect("../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REFFROM=nc&REVIEWDNC=" + ViewState["REFERENCEID"].ToString(), false);
        }
        else if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            Response.Redirect("../Inspection/InspectionIncidentCriticalFactor.aspx", false);
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            Response.Redirect("../Inspection/InspectionObservationCorrectiveAction.aspx?REFFROM=obs&DIRECTOBSERVATIONID=" + ViewState["REFERENCEID"].ToString(), false);
        }
        //BindCorrectiveAction();
        //String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
        //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateCorrectiveAction()
    {
        if (!IsValidCorrectiveAction())
        {
            ucError.Visible = true;
            return;
        }
        if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            PhoenixInspectionCorrectiveAction.UpdateIncidentCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["CORRECTIVEACTIONID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null, null,
                General.GetNullableInteger(ucDept.SelectedDepartment), null,
                null, null, General.GetNullableInteger(ucVerficationLevel.SelectedHard));            
        }
        else if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            PhoenixInspectionCorrectiveAction.UpdateNCCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["CORRECTIVEACTIONID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null,null,
                General.GetNullableInteger(ucDept.SelectedDepartment), null,
                null, null, General.GetNullableInteger(ucVerficationLevel.SelectedHard),
                General.GetNullableString(txtChecklistRefNo.Text), General.GetNullableString(txtDeficiencyDetails.Text));  
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            PhoenixInspectionCorrectiveAction.UpdateObsCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["CORRECTIVEACTIONID"].ToString()), General.GetNullableString(txtCorrectiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null,
                General.GetNullableInteger(ucVerficationLevel.SelectedHard), General.GetNullableInteger(ucDept.SelectedDepartment),
                General.GetNullableString(txtChecklistRefNo.Text), General.GetNullableString(txtDeficiencyDetails.Text));
        }
        ucStatus.Text = "Corrective Action updated.";
        BindCorrectiveAction();

        String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidCorrectiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtCorrectiveAction.Text))
            ucError.ErrorMessage = "Corrective Action is required.";

        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (General.GetNullableDateTime(ucCompletionDate.Text) != null && General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date cannot be the future date.";

        return (!ucError.IsError);
    }

    public void SetRights()
    {
        ucTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "TARGETDATE");
    }
}
