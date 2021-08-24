using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;   
public partial class VesselAccounts_VesselAccountsReportsRHShipboardWorkingArrangements : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                //PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
                //MenuShipboardWorking.AccessRights = this.ViewState;
                //MenuShipboardWorking.MenuList = toolbar.Show();
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (ViewState["VESSELID"].ToString() != "0")
                {
                    txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName.ToString();
                }
                else
                {
                    txtVessel.Text = "-- OFFICE --";
                }

                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=RHSHIPBOARDWORKINGARRANGEMENT&showmenu=false&showexcel=no&showword=no&vesselid="
                    + ViewState["VESSELID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuShipboardWorking_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=RHSHIPBOARDWORKINGARRANGEMENT&showmenu=false&showexcel=no&showword=no&vesselid="
                    + ViewState["VESSELID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}