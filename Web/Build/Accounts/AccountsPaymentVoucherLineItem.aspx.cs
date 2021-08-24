using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class AccountsPaymentVoucherLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request.QueryString["voucherid"] != null && Request.QueryString["type"] != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 27, "CTM"))
        {
            ViewState["CURRENTTAB"] = "../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?voucherid=" + Request.QueryString["voucherid"].ToString();
        }
        else if (Request.QueryString["voucherid"] != null && Request.QueryString["type"] == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 27, "CTM"))
        {
            ViewState["CURRENTTAB"] = "../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?voucherid=" + Request.QueryString["voucherid"].ToString();
        }
        ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
    }
}
