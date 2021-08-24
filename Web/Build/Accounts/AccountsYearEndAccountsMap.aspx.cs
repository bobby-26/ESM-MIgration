using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsYearEndAccountsMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);

        MenuMain.AccessRights = this.ViewState;
        MenuMain.Title = "Accounts Transferred";
        MenuMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {


            ViewState["FINYEAR"] = Request.QueryString["finyear"];

            BindAccountCode();
            EditYearEndAccountMap();
        }

    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsYearEndAccountMap.YearEndAccountMapInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                        , int.Parse(ViewState["FINYEAR"].ToString())
                                                                        , General.GetNullableInteger(ddlInvoiceAccurals.SelectedValue)
                                                                        , General.GetNullableInteger(ddlForexRevaluation.SelectedValue)
                                                                        , General.GetNullableInteger(ddlProfitLoss.SelectedValue)
                                                                        );

                EditYearEndAccountMap();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditYearEndAccountMap()
    {
        if (ViewState["FINYEAR"] != null && ViewState["FINYEAR"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixAccountsYearEndAccountMap.YearEndAccountEdit(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["FINYEAR"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (General.GetNullableInteger(dr["FLDPROFITLOSSACCOUNT"].ToString()) != null)
                    ddlProfitLoss.SelectedValue = dr["FLDPROFITLOSSACCOUNT"].ToString();
                else
                    ddlProfitLoss.SelectedValue = "Dummy";

                if (General.GetNullableInteger(dr["FLDINVOICEACCURALSACCOUNT"].ToString()) != null)
                    ddlInvoiceAccurals.SelectedValue = dr["FLDINVOICEACCURALSACCOUNT"].ToString();
                else
                    ddlInvoiceAccurals.SelectedValue = "Dummy";

                if (General.GetNullableInteger(dr["FLDFOREXREVALUATIONACCOUNT"].ToString()) != null)
                    ddlForexRevaluation.SelectedValue = dr["FLDFOREXREVALUATIONACCOUNT"].ToString();
                else
                    ddlForexRevaluation.SelectedValue = "Dummy";
            }
        }

    }
    private void BindAccountCode()
    {
        ddlInvoiceAccurals.DataSource = PhoenixAccountsYearEndAccountMap.YearEndAccountList();
        ddlInvoiceAccurals.DataTextField = "FLDDESCRIPTION";
        ddlInvoiceAccurals.DataValueField = "FLDACCOUNTID";
        ddlInvoiceAccurals.DataBind();
        ddlInvoiceAccurals.Items.Insert(0, new DropDownListItem("--Select--", ""));

        ddlForexRevaluation.DataSource = PhoenixAccountsYearEndAccountMap.YearEndAccountList();
        ddlForexRevaluation.DataTextField = "FLDDESCRIPTION";
        ddlForexRevaluation.DataValueField = "FLDACCOUNTID";
        ddlForexRevaluation.DataBind();
        ddlForexRevaluation.Items.Insert(0, new DropDownListItem("--Select--", ""));

        ddlProfitLoss.DataSource = PhoenixAccountsYearEndAccountMap.YearEndAccountList();
        ddlProfitLoss.DataTextField = "FLDDESCRIPTION";
        ddlProfitLoss.DataValueField = "FLDACCOUNTID";
        ddlProfitLoss.DataBind();
        ddlProfitLoss.Items.Insert(0, new DropDownListItem("--Select--", ""));

    }

}
