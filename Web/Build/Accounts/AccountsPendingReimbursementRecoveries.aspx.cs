using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsPendingReimbursementRecoveries : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                ViewState["EMPLOYEEID"] = null;
                ViewState["VESSELID"] = null;
                ViewState["MONTH"] = null;
                ViewState["YEAR"] = null;
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
                ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixAccountsAllotmentRequestSystemChecking.AccountsPendingReimbursementRecoveriesSearch(int.Parse(ViewState["EMPLOYEEID"].ToString()),
                                                                                     int.Parse(ViewState["VESSELID"].ToString()),
                                                                                     int.Parse(ViewState["MONTH"].ToString()),
                                                                                     int.Parse(ViewState["YEAR"].ToString()));

        gvRem.DataSource = dt;

    }
    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {         
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement(Monthly / OneTime)";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery(Monthly / OneTime)";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
    }
 
}
