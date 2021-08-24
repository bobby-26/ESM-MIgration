using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReApprovalforSeafarersFilters : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        CrewReApprovalSeafarerFilter.MenuList = toolbar.Show();
      
    }
    protected void CrewReApprovalSeafarer_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("txtEmployeeFileNo", txtEmployeeFileNo.Text);
            criteria.Add("txtEmployeeName", txtEmployeeName.Text);
            criteria.Add("ucDate", ucDate.Text);
            criteria.Add("ucDate1", ucDate1.Text);
            criteria.Add("ucRank", ucRank.selectedlist);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("chkApprovedYN", chkApprovedYN.Checked == true ? "1" : "0");
            Filter.CurrentCrewReEmploymentFilterSelection = criteria;
        }
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
}
