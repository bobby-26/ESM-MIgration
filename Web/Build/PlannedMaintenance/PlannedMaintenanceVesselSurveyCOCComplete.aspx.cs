using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCOCComplete : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Complete", "SAVE",ToolBarDirection.Right);
        MenuCompleteAction.AccessRights = this.ViewState;
        MenuCompleteAction.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ViewState["CertificateCOCId"] = Request.QueryString["CertificateCOCId"] != null ? Request.QueryString["CertificateCOCId"] : "";
            txtCompleteRemarks.Focus();
            PopulateDetails();
        }
    }
    private void PopulateDetails()
    {
        try
        {
            DataTable dt = PhoenixPlannedMaintenanceVesselCertificateCOC.SurveyCOCEdit(new Guid(ViewState["CertificateCOCId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtCompleteRemarks.Text = dt.Rows[0]["FLDCOMPLETEDREMARKS"].ToString();
                ucCompletedDate.Text = dt.Rows[0]["FLDCOMPLETEDDATE"].ToString();
                txtCertificate.Text = dt.Rows[0]["FLDCERTIFICATENAME"].ToString();
                txtCertificate.ToolTip = dt.Rows[0]["FLDCERTIFICATENAME"].ToString();
                txtCOC.Text = dt.Rows[0]["FLDITEM"].ToString();
                txtDescription.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuCompleteAction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(txtCompleteRemarks.Text, ucCompletedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceVesselCertificateCOC.CompleteSureyCOC(
                          new Guid(ViewState["CertificateCOCId"].ToString())
                         , General.GetNullableString(txtCompleteRemarks.Text.Trim())
                         , General.GetNullableDateTime(ucCompletedDate.Text));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                //String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp', 'ifMoreInfo');");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDetails(
       string CompleteRemarks
       , string CompletedDate)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (CompleteRemarks.Trim().Equals(""))
            ucError.ErrorMessage = "Completed Remarks is required.";

        if (!DateTime.TryParse(CompletedDate, out resultDate))
            ucError.ErrorMessage = "Completed Date is required.";

        if (CompletedDate != null && CompletedDate != null)
        {
            if ((DateTime.TryParse(CompletedDate, out resultDate)))
                if ((DateTime.Parse(CompletedDate)) > DateTime.Today)
                    ucError.ErrorMessage = "'Completed Date' should be earlier then current date.";
        }

        return (!ucError.IsError);
    }
}
