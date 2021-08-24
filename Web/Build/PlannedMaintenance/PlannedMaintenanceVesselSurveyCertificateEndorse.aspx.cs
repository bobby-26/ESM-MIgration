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
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateEndorse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuCertificatesEndorse.AccessRights = this.ViewState;
            MenuCertificatesEndorse.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SELECTEDINDEX"] = 0;
                ViewState["ScheduleId"] = Request.QueryString["ScheduleId"] != null ? Request.QueryString["ScheduleId"] : "";
                ViewState["CertificateId"] = Request.QueryString["CertificateId"] != null ? Request.QueryString["CertificateId"] : "";
                ViewState["DtKey"] = Request.QueryString["DtKey"] != null ? Request.QueryString["DtKey"] : "";
                ViewState["VesselId"] = Request.QueryString["VesselId"] != null ? Request.QueryString["VesselId"] : "";
                PopulateCertificateDetails();
                ucEndorseDate.Focus();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void PopulateCertificateDetails()
    {
        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.GetVesselCertificateEndorseDetails(General.GetNullableInteger(ViewState["VesselId"].ToString())
            , General.GetNullableInteger(ViewState["CertificateId"].ToString())
            , General.GetNullableGuid(ViewState["ScheduleId"].ToString())
            , General.GetNullableGuid(ViewState["DtKey"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCertificateCode.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATECODE"].ToString().Trim();
            txtCertificate.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString().Trim();
            ucEndorseDate.Text = ds.Tables[0].Rows[0]["FLDENDORSEDATE"].ToString();
        }
    }
    protected void MenuCertificatesEndorse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(ucEndorseDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateVesselCertificates(
                      ViewState["CertificateId"].ToString()
                      , ViewState["ScheduleId"].ToString()
                      , ViewState["DtKey"].ToString()
                      , ucEndorseDate.Text
                      );
                ucStatus.Text = "Certificate Endorsement Details Updated Sucessfully";
                ucStatus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateVesselCertificates(string CertificateID, string ScheduleId,
      string DtKey, string EndorseDate)
    {
        PhoenixPlannedMaintenanceVesselCertificateCOC.VesselCertificateEndorseDetailsUpdate(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , General.GetNullableInteger(CertificateID)
            , General.GetNullableGuid(ScheduleId)
           , General.GetNullableGuid(DtKey)
            , General.GetNullableDateTime(EndorseDate)
           );
    }
    private bool IsValidDetails(string dateofEndorse)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!DateTime.TryParse(dateofEndorse, out resultDate))
            ucError.ErrorMessage = "Endorse Date is required.";

        if (dateofEndorse != null)
        {
            if ((DateTime.TryParse(dateofEndorse, out resultDate)))
                if ((DateTime.Parse(dateofEndorse)) > DateTime.Today)
                    ucError.ErrorMessage = "'Endorse Date' should be less than 'Current Date'";
        }

        return (!ucError.IsError);
    }
}
