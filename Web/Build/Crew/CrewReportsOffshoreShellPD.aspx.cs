using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportsOffshoreShellPD : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Shell PP Registration", "OFFSHORESHELLPPREGISTRATION", ToolBarDirection.Right);
            toolbar.AddButton("Shell PD", "OFFSHORESHELLPD", ToolBarDirection.Right);
            
            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar.Show();
            MenuReport.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
               
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["fileno"] != null && Request.QueryString["fileno"].ToString() != "")
                    txtFileNo.Text = Request.QueryString["fileno"].ToString();
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=OFFSHORESHELLPD&vslid=null&fileno=null&showmenu=0&showexcel=no&showword=no";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("OFFSHORESHELLPPREGISTRATION"))
            {
                Response.Redirect("../Crew/CrewReportsOffshoreShellPPRegistration.aspx?vesselid=" + ucVessel.SelectedVessel + "&fileno=" + txtFileNo.Text);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=OFFSHORESHELLPD&vslid=" + ucVessel.SelectedVessel.ToString() + "&fileno=" + txtFileNo.Text + "&showmenu=0&showexcel=no&showword=no";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(txtFileNo.Text) == null)
            ucError.ErrorMessage = "File No is required.";

        return (!ucError.IsError);
    }
}
