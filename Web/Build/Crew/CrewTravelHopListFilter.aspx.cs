using System;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewTravelHopListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtArrivalDateFrom.Text = DateTime.Now.ToString();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);        
        MenuMainHopList.MenuList = toolbar.Show();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("txtArrivalCity", txtArrivalCity.SelectedValue);
        criteria.Add("txtArrivalDateFrom", txtArrivalDateFrom.Text);
        criteria.Add("txtArrivalDateTo", txtArrivalDateTo.Text);

        Filter.CurrentTravelHopListFilter = criteria;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
    }

    protected void MenuMainHopList_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("txtArrivalCity", txtArrivalCity.SelectedValue);
            criteria.Add("txtArrivalDateFrom", txtArrivalDateFrom.Text);
            criteria.Add("txtArrivalDateTo", txtArrivalDateTo.Text);

            Filter.CurrentTravelHopListFilter = criteria;            
        }
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',true);", true);        
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }
}
