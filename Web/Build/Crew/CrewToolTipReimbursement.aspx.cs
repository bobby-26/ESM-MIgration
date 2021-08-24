using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class PlannedMaintenanceToolTipSurveyRemark : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
            ViewState["REMID"] = nvc["reimid"].ToString();          
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {
        DataTable dt = PhoenixCrewReimbursement.FetchReimbursementMoreInfo(General.GetNullableGuid(ViewState["REMID"].ToString()).Value);
        if(dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lblDescription.Text = dr["FLDDESCRIPTION"].ToString();
            lblVoucher.Text = dr["FLDVOUCHERNUMBER"].ToString();
            lblPaymentMode.Text = dr["FLDPAYMENTMODE"].ToString();
            lblBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
            lblUnPaid.Text = dr["FLDUNPAIDAMOUNT"].ToString();
            lblChargeableVessel.Text = dr["FLDCHARGEABLEVESSEL"].ToString();
        }
    }
}