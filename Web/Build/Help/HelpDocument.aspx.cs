using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Help;
using System.Configuration;

public partial class HelpDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //SessionUtil.PageAccessRights(this.ViewState);           
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();

                if (Request.QueryString["pid"] != null)
                    ViewState["PAGEID"] = Request.QueryString["pid"];
                else if (Request.QueryString["page"].IndexOf("?") >= 0)
                    dt = PhoenixHelp.GetHelpDocumentLink(Request.QueryString["page"].Substring(0, Request.QueryString["page"].IndexOf("?")));
                else
                    dt = PhoenixHelp.GetHelpDocumentLink(Request.QueryString["page"]);

                if(dt.Rows.Count>0)
                {
                    fraHelp.Attributes["src"] = ConfigurationManager.AppSettings.Get("helpurl").ToString()+ dt.Rows[0]["FLDDOCUMENTLINK"];
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
