using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardRankFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {           
            BindRankList();
        }
    }

    private void BindRankList()
    {
        DataSet ds = PhoenixRegistersRank.ListRank();
        chkRankList.Items.Add("select");
        chkRankList.DataSource = ds;
        chkRankList.DataTextField = "FLDRANKNAME";
        chkRankList.DataValueField = "FLDRANKID";
        chkRankList.DataBind();
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
        Script += "</script>" + "\n";


        if (dce.CommandName.ToUpper().Equals("GO"))
        {            
            Filter.CurrentDashboardLastSelection["RankList"] = General.ReadCheckBoxList(chkRankList);            
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
           
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}



