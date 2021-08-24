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
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SELECTEDINDEX"] = 0;
            ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["CertificateId"] = Request.QueryString["CertificateId"];
            PopulateCertificateDetails();
        }
    }
    private void PopulateCertificateDetails()
    {
        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.GetPrvCertificateDetails(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , General.GetNullableInteger(ViewState["CertificateId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCertificateCode.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATECODE"].ToString().Trim();
            txtCertificate.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString().Trim();
            txtNumber.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENO"].ToString().Trim();
            ucIssueDate.Text = ds.Tables[0].Rows[0]["FLDDATEOFISSUE"].ToString();
            ucExpiryDate.Text = ds.Tables[0].Rows[0]["FLDDATEOFEXPIRY"].ToString();
            txtIssueAuthority.Text = ds.Tables[0].Rows[0]["FLDISSUINGAUTHORITYNAME"].ToString();
            txtPlaceofIssue.Text = ds.Tables[0].Rows[0]["FLDSEAPORTNAME"].ToString();
            txtLastSurveyType.Text = ds.Tables[0].Rows[0]["FLDSURVEYTYPENAME"].ToString();
            ucLastSurveyDate.Text = ds.Tables[0].Rows[0]["FLDNEXTDUEDATE"].ToString();
            txtCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
            txtRemarksType.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
            txtSurveyPort.Text = ds.Tables[0].Rows[0]["FLDSURVEYPOERTNAME"].ToString();
            chkAuditLog.Checked = ds.Tables[0].Rows[0]["FLDUPDATEAUDITLOG"].ToString().Equals("1") ? true : false;
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATEREMARKS"].ToString();
            ChkNotApplicable.Checked = ds.Tables[0].Rows[0]["FLDNOTAPPLICABLE"].ToString().Equals("1") ? true : false;
            txtReason.Text = ds.Tables[0].Rows[0]["FLDNOTAPPLICABLEREASON"].ToString();
            chkAttachYN.Checked = ds.Tables[0].Rows[0]["FLDATTACHCORRECTYN"].ToString().Equals("1") ? true : false;
        }
    }
   
}
