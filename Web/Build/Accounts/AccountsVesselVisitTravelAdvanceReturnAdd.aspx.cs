using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceReturnAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["visitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;

                if (Request.QueryString["TravelAdvanceId"] != "")
                    ViewState["TravelAdvanceId"] = Request.QueryString["TravelAdvanceId"];
                else
                    ViewState["TravelAdvanceId"] = null;
            }
                if (ViewState["TravelAdvanceId"] != null)
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Add", "ADD",ToolBarDirection.Right);
                    MenuTravelAdvance.AccessRights = this.ViewState;
                    MenuTravelAdvance.MenuList = toolbar.Show();
                }
                TravelReturnEdit();
         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(txtPaymentAmount.Text,ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnInsert(new Guid(ViewState["VisitId"].ToString())
                                                                                 , new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                 , int.Parse(ucCurrency.SelectedCurrency)
                                                                                 , General.GetNullableDecimal(txtPaymentAmount.Text)
                                                                                 , General.GetNullableDecimal(txtReturnAmount.Text)
                                                                                 , General.GetNullableDateTime(txtReturnDate.Text)
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void TravelReturnEdit()
    {
        try
        {
            if (ViewState["TravelAdvanceId"] != null)
            {
                DataSet ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnEdit(new Guid(ViewState["TravelAdvanceId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];
                
                txtEmployee.Text = dr["FLDEMPLOYEEID"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                txtApprovedAmount.Text = string.Format(String.Format("{0:###,###,###.00}", dr["FLDTOTALAPPROVEDAMOUNT"]));
                txtReturnDate.Text = General.GetDateTimeToString(DateTime.Now.Date.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string paymentAmount , string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (paymentAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Payment Amount cannot be blank. If not payable, please update Payment Amount as 0.00.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required";

        return (!ucError.IsError);

    }

}
