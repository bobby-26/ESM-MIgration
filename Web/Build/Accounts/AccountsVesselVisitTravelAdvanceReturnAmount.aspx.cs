using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceReturnAmount : PhoenixBasePage
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

                if (ViewState["TravelAdvanceId"] != null)
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Return", "RETURN", ToolBarDirection.Right);
                    MenuTravelAdvance.AccessRights = this.ViewState;
                    MenuTravelAdvance.MenuList = toolbar.Show();
                }
                TravelReturnEdit();
            }

         
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
            if (CommandName.ToUpper().Equals("RETURN"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnAmountUpdate(new Guid(ViewState["TravelAdvanceId"].ToString())
                                                                                 , General.GetNullableDecimal(txtReturnAmount.Text)
                                                                                 , General.GetNullableDateTime(txtReturnDate.Text)
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , ViewState["CASHPAYMENTVOUCHERNUMBER"].ToString());
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

                txtTravelAdvanceNo.Text = dr["FLDTRAVELADVANCENUMBER"].ToString();
                txtEmployee.Text = dr["FLDEMPLOYEEID"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtPaymentAmount.Text = string.Format(String.Format("{0:###,###,###.00}", dr["FLDPAYMENTAMOUNT"]));
                txtReturnAmount.Text = dr["FLDRETURNAMOUNT"].ToString();
                txtReturnDate.Text = General.GetDateTimeToString(DateTime.Now.Date.ToString());
                ViewState["CASHPAYMENTVOUCHERNUMBER"] = dr["FLDCASHPAYMENTVOUCHERNUMBER"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
