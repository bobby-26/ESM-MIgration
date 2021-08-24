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
using Telerik.Web.UI;

public partial class CrewOffshoreCrewCommentConfirmMsg : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["NAME"] != null)
                ViewState["NAME"] = Request.QueryString["NAME"].ToString();
            if (Request.QueryString["RANK"] != null)
                ViewState["RANK"] = Request.QueryString["RANK"].ToString();

            lblmsg.Text = lblmsg.Text + " [" + ViewState["RANK"].ToString() + "] - " + ViewState["NAME"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
