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

public partial class Registers_RegistersExportInitiate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["SORTEXPRESSION"] = "FLDABBREVIATION";
        ViewState["SORTDIRECTION"] = 0;
        ViewState["PAGENUMBER"] = 1;
        
        if (PreviousPage != null)
            Response.Write("Test");

        Server.Transfer("RegistersExportToExcel.aspx");

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
