using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollSummaryIndia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = int.Parse(Request.QueryString["id"]);
        }

        if (IsPostBack == false)
        {
            GetTaxSummaryDetails();
        }
    }

    public void GetTaxSummaryDetails()
    {
        if (Id != 0)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollTaxSummarySearch(usercode, Id, DateTime.Now);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblSalary.Text = dr["FLDSALARY"].ToString();
                lblPerquisite.Text = dr["FLDPERQUISITE"].ToString();
                lblOtherIncome.Text = dr["FLDOTHERINCOME"].ToString();
                lblExemption10.Text = dr["FLDEXEMPTINCOME10"].ToString();
                lblDeduction16.Text = dr["FLDDEDUCTION16"].ToString();
                lblChapter6a.Text = dr["FLDCHAPTER6A"].ToString();
                lblDeduction80G.Text = dr["FLDDEDUCTION80G"].ToString();
                lblDeduction80GG.Text = dr["FLDDEDUCTION80GG"].ToString();
                lblExemptIncome.Text = dr["FLDEXEMPTINCOME"].ToString();
                lbltds.Text = dr["FLDTDS"].ToString();
                lbltotalsalary.Text = dr["FLDTOTALINCOME"].ToString();
                lblstddeduction.Text = dr["FLDTOTALSTANDARDDEDUCTION"].ToString();
                lbldeduction.Text = dr["FLDTOTALDEDUCTION"].ToString();
                lbltotaltaxAfterSlab.Text = dr["FLDTAXAFTERSLABAMOUNT"].ToString();
                lblTaxableSlabAmount.Text = dr["FLDTAXSLABAMOUNT"].ToString();

                lblTotalIncome.Text = lbltotalsalary.Text;
                lblTotalStandardDeduction.Text = lblstddeduction.Text;
                lblTotalDeduction.Text = lbldeduction.Text;

                lbltds.Text = dr["FLDTDS"].ToString();
                lbltotalnettax.Text = dr["FLDNETTAXPAYABLE"].ToString();

  //              	,FLDEMPLOYEEID
		//,FLDDATE
		//,FLDSALARY
		//,FLDPERQUISITE
		//,FLDOTHERINCOME
		//,FLDEXEMPTINCOME10
		//,FLDDEDUCTION16
		//,FLDCHAPTER6A
		//,FLDDEDUCTION80G
		//,FLDDEDUCTION80GG
		//,FLDEXEMPTINCOME
		//,FLDTDS
		//,FLDTOTALINCOME
		//,FLDTOTALSTANDARDDEDUCTION
		//,FLDTOTALDEDUCTION
		//,FLDTOTALTAXPAYABLE
		//,FLDNETPAYABLETAX


            }

        }
    }


}