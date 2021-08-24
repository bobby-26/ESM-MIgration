using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Data;
using SouthNests.Phoenix.Framework;

public partial class UserControls_UserControlFYTrailBalanceMonthly : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Company != null && Financialyear != null && CurrencyType != null && Month != null)
        {
            rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(Company), int.Parse(Financialyear), CurrencyType, int.Parse(Month), 1, null);
            rptTrailBalance.DataBind();
        }
    }

    public string Company
    {
        get;
        set;
    }

    public string Financialyear
    {
        get;
        set;
    }

    public string CurrencyType
    {
        get;
        set;
    }

    public string Month
    {
        get;
        set;
    }

    protected void rptTrailBalance_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptTrailBalance = e.Item.FindControl("rptTrailBalance2nd") as Repeater;
            rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(Company), int.Parse(Financialyear), CurrencyType, int.Parse(Month), 2, int.Parse(customerId));
            rptTrailBalance.DataBind();
        }
    }

    protected void rptTrailBalance2nd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptTrailBalance = e.Item.FindControl("rptTrailBalance3rd") as Repeater;
            rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(Company), int.Parse(Financialyear), CurrencyType, int.Parse(Month), 3, int.Parse(customerId));
            rptTrailBalance.DataBind();

        }
    }

    //public void bind()
    //{
    //    rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(Company), int.Parse(Financialyear), CurrencyType, int.Parse(Month), 1, null);
    //    rptTrailBalance.DataBind();
    //}

}