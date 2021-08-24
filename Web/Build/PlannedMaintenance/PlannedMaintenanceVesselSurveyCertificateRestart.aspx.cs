using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class PlannedMaintenanceVesselSurveyCertificateRestart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            //toolbarmain.AddButton("Cancel", "CANCEL");
            MenuCertificatesRenewal.AccessRights = this.ViewState;
            MenuCertificatesRenewal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["CID"] = Request.QueryString["cid"];
                EditCertificate();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void EditCertificate()
    {
        DataTable dt = PhoenixPlannedMaintenanceSurveySchedule.EditVesselCertificateRestart(int.Parse(ViewState["CID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtCertificate.Text = dt.Rows[0]["FLDCERTIFICATENAME"].ToString();
            txtSurveyType.Text = dt.Rows[0]["FLDSEQUENCENAME"].ToString();
            ViewState["SURVEYTYPE"] = dt.Rows[0]["FLDSEQUENCEID"].ToString();
            ViewState["AUDITYN"] = dt.Rows[0]["FLDAUDITREQUIREDYN"].ToString();
        }
    }

    protected void MenuCertificatesRenewal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(txtDueDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? g = null;
                PhoenixPlannedMaintenanceSurveySchedule.RestartVesselSurveyCertificate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                             , General.GetNullableInteger(ViewState["CID"].ToString())
                                                             , General.GetNullableString(txtRemarks.Text.Trim())
                                                             , ViewState["AUDITYN"].ToString() != "1" ? General.GetNullableInteger(ViewState["SURVEYTYPE"].ToString()) : null
                                                             , ViewState["AUDITYN"].ToString() == "1" ? General.GetNullableGuid(ViewState["SURVEYTYPE"].ToString()) : null
                                                             , ref g
                                                             , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                                                             , General.GetNullableDateTime(txtPlanDate.Text)
                                                             , txtSurveyorName.Text.Trim()
                                                             , DateTime.Parse(txtDueDate.Text)
                                                             );
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                Response.Redirect("PlannedMaintenanceVesselSurveyCertificateRenewal.aspx?" + Request.QueryString.ToString(), true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDetails(string duedate)
    {
        //DateTime resultDate;
        //Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(duedate).HasValue)
            ucError.ErrorMessage = "Due Date of Audit / Survey is required";
        return (!ucError.IsError);
    }

}

