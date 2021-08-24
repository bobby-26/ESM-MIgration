using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;

public partial class Accounts_AccountsPVDirectRemittancetoCrew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Generate PV", "APPROVEANDGENERATEDPV", ToolBarDirection.Right);
            //toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pagenumber"] == null ? "1" : Request.QueryString["pagenumber"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["AllotmentIdList"] != null && Request.QueryString["AllotmentIdList"] != string.Empty)
                {
                    ViewState["AllotmentIdList"] = Request.QueryString["AllotmentIdList"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVoucher_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("APPROVEANDGENERATEDPV"))
            {

                    PhoenixAccountsAllotment.AllotmentPaymentVoucherGenerate(ViewState["AllotmentIdList"].ToString(), General.GetNullableString(txtPaymentvoucherremarks.Text));
                    ucStatus.Text = "Payment Voucher Generated";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}