using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewInactiveTransferToOfficeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);       
        MenuActivityFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           
        }
    }

    protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("ddlVesselType",ddlVesselType.SelectedVesseltype.ToUpper() == "DUMMY" ? null : General.GetNullableString(ddlVesselType.SelectedVesseltype));
            criteria.Add("txtFromDate", txtAppliedStartDate.Text);
            criteria.Add("txtToDate", txtAppliedEndDate.Text);
            criteria.Add("lstRank", lstRank.selectedlist == "," ? null : General.GetNullableString(lstRank.selectedlist));
            criteria.Add("chkIncludepastexp", chkIncludepastexp.Checked == true ? "1" : "0");

            Filter.CurrentTransferToOfficeFilter = criteria;
        }
        
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
}
