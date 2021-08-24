using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewWorkingGearOrderFormFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           txtRefNo.Focus();          
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("ddlStatus", uctravelstatus.SelectedHard);

            Filter.CurrentWorkingGearOrderFormFilter = criteria;
        }

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

        //Response.Redirect("CrewWorkingGearOrderForm.aspx", false);
        
    }   
}
