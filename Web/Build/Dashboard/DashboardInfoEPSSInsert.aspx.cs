using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Dashboard_DashboardInfoEPSSInsert : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuDashboard.AccessRights = this.ViewState;
        MenuDashboard.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ddlconfigid.DataTextField = "FLDDASHBOARD";
            ddlconfigid.DataValueField = "FLDCONFIGID";
            ddlconfigid.DataSource = PhoenixDashboardInfoEPSS.ListConfig();
            ddlconfigid.DataBind();
            ViewState["ID"] = string.Empty;
            if(!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
                Edit();
            }
        }
        string scriptKey = "OnSubmitScript";
        string javaScript = "onSubmitBefore()";
        this.ClientScript.RegisterOnSubmitStatement(this.GetType(), scriptKey, javaScript);
    }
    protected void MenuDashboard_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {

            PhoenixDashboardInfoEPSS.Insert(int.Parse(ddlconfigid.SelectedValue.ToString()), txtinformation.Text, txtlink.Text, txtHelpLink.Text);
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }
    private void Edit()
    {
        DataTable dt = PhoenixDashboardInfoEPSS.Edit(Guid.Parse(ViewState["ID"].ToString()));
        foreach(DataRow dr in dt.Rows)
        {
            ddlconfigid.SelectedValue = dr["FLDCONFIGID"].ToString();
            ddlconfigid.Enabled = false;
            txtinformation.Text = dr["FLDINFORMATION"].ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#34;", "\"");
            txtlink.Text = dr["FLDEPSSLINK"].ToString();
            txtHelpLink.Text = dr["FLDHELPLINK"].ToString();           
        }
    }
}