using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewReportsEmergencySignOff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.MenuList = toolbar.Show();   
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
               
             }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=EMERGENCYSIGNOFF&principal=NULL&vesselid=null&vesseltypeid=NULL&rank=null&zone=NULL";
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=EMERGENCYSIGNOFF&principal=" + ucPrincipal.SelectedAddress.ToString() + "&vesselid=" + ucVessel.SelectedVessel.ToString() + "&vesseltypeid=" + ucVesselType.SelectedVesseltype.ToString() + "&rank=" + ucRank1.SelectedRank.ToString() + "&zone=" + ucZone.SelectedZone.ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    }
