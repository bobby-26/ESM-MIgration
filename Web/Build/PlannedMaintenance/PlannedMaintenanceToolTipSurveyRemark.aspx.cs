using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.Collections.Specialized;
using Telerik.Web;

public partial class PlannedMaintenanceToolTipSurveyRemark : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
            ViewState["VSLID"] = nvc["VSLID"].ToString();
            ViewState["CID"] = nvc["CID"].ToString();
            ViewState["DTKEY"] = nvc["DTKEY"].ToString();
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {

        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.EditVesselCertificate(int.Parse(ViewState["VSLID"].ToString())
            , General.GetNullableInteger(ViewState["CID"].ToString()).Value
            , General.GetNullableGuid(ViewState["DTKEY"].ToString()));

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {

            DataRow dr = ds.Tables[0].Rows[0];
            lblInitialDate.Text = General.GetDateTimeToString(dr["FLDINITIALDATE"].ToString());
            lblRemark.Text = dr["FLDCERTIFICATEREMARKS"].ToString().Trim();
            lblStatus.Text = dr["FLDCERTIFICATESTATUS"].ToString().Trim();
            lblNotApplicable.Text = dr["FLDNOTAPPLICABLEREASON"].ToString().Trim();
            lblVerifiedBy.Text = dr["FLDVERIFIEDBY"].ToString().Trim();
            lblVerifiedOn.Text = General.GetDateTimeToString(dr["FLDVERIFIEDON"].ToString());
        }
    }
}