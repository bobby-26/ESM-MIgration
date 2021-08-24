using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Accounts;

public partial class AccountsExport2XL : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string finyear = Request.QueryString["finyear"].ToString();

        PhoenixAccounts2XL.Export2XLInvoiceAccurals(int.Parse(finyear));


        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

    }
}
