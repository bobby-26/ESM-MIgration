using System;
using System.Collections;
using System.Collections.Specialized;
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
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_DataTransferVesselFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuVesselsFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            txtVesselName.Focus();
        }
    }

    protected void VesselsFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("txtVesselName", txtVesselName.Text);
            criteria.Add("ddlVesselType", ddlVesselType.SelectedVesseltype);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);

            Filter.CurrentVesselCriteria = criteria;

        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

}
