using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;


using System.Data;
using Telerik.Web.UI;

public partial class Accounts_AccountsAllotmentHistory : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["ALLOTMENTID"] = "";
            ViewState["ALLOTMENTTYPE"] = "";
            ViewState["VESSELID"] = "";
            ViewState["CHECKTYPE"] = "1";
            ViewState["EMPLOYEEID"] = "";
            ViewState["MONTH"] = null;
            ViewState["YEAR"] = null;
            ViewState["VESSELNAME"] = null;
            ViewState["SIGNINOFFID"] = null;

            if (Request.QueryString["ALLOTMENTID"] != null)
                ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();

            ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
            ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
            ViewState["VESSELNAME"] = Request.QueryString["VESSELNAME"].ToString();
            ViewState["SIGNINOFFID"] = Request.QueryString["SIGNINOFFID"].ToString();

            BindEmpployeeDetails();
            //  BindGrid();
        }
    }

    private void BindEmpployeeDetails()
    {
        try
        {
            DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentRequestEdit(new Guid(ViewState["ALLOTMENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtMonthAndYear.Text = dr["FLDMONTHNAME"].ToString() + "-" + dr["FLDYEAR"].ToString();
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKNAME"].ToString();
                txtallotmentType.Text = dr["FLDALLOTMENTTYPENAME"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtReference.Text = dr["FLDREQUESTNUMBER"].ToString();
                txtAllotmentAmount.Text = dr["FLDAMOUNT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindGrid()
    {
        DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentHistoryList(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()), General.GetNullableInteger(ViewState["SIGNINOFFID"].ToString()));
        gvAccountHistory.DataSource = dt;
        gvAccountHistory.VirtualItemCount = dt.Rows.Count;
    }
    protected void gvAccountHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
