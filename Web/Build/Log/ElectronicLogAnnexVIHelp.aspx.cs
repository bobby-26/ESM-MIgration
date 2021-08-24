using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Text;

public partial class Log_ElectronicLogAnnexVIHelp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["HELPID"] = Request.QueryString["helpid"];
            LoadHelp();
        }
    }

    private void LoadHelp()
    {
        
        if (ViewState["HELPID"].ToString() == "ods")
        {
            odstb.Visible = true;
           
           

        }

        if (ViewState["HELPID"].ToString() == "ep")
        {
            ep.Visible = true;

        }

       
    }
}