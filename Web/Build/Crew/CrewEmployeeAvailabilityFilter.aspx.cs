using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;
public partial class Crew_CrewEmployeeAvailabilityFilter : PhoenixBasePage
{    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        //toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        employeeAvailabilityfilter.MenuList = toolbar.Show();
       
    }
    protected void employeeAvailabilityfilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        NameValueCollection criteria = new NameValueCollection();
        if (CommandName.ToUpper().Equals("GO"))
        {
         
            criteria.Add("empId", "");
            criteria.Add("fileNo", txtFileNo.Text.Trim());
            criteria.Add("activeyn", "");
            criteria.Add("newApp", rblCrewFrom.SelectedValue);
            criteria.Add("empName", txtName.Text);
            if(ucZone.SelectedZone.ToUpper()!="DUMMY")
                criteria.Add("zoneId", ucZone.SelectedZone);
            else
                criteria.Add("zoneId", "");
            if ( ddlRank.SelectedRank.ToUpper() != "DUMMY")
                criteria.Add("rankId", ddlRank.SelectedRank.ToString());
            else
                criteria.Add("rankId", "");
        }
        //if (CommandName.ToUpper().Equals("CANCEL"))
        //{
        //    criteria.Clear();
        //    txtName.Text = "";
        //    txtFileNo.Text = "";
        //    ucZone.SelectedZone = "Dummy";
        //    ddlRank.SelectedRank = "Dummy";                      
        //}
        Filter.CurrentEmployeeAvailabilitySelection = criteria;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);        
    }
}