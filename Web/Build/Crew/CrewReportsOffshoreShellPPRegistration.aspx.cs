using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewReportsOffshoreShellPPRegistration : PhoenixBasePage
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
            MenuReport.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send Mail", "SENDMAIL", ToolBarDirection.Right);
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

                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=OFFSHORESHELLPPREGISTRATION&vslid=null&fileno=null&showmenu=0&showexcel=no&showword=no&Contractno=" + txtContractno.Text.Trim();
            }

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

            if (CommandName.ToUpper().Equals("OFFSHORESHELLPD"))
            {
                Response.Redirect("../Crew/CrewReportsOffshoreShellPD.aspx?vesselid=" + ucVessel.SelectedVessel + "&fileno=" + txtFileNo.Text);
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
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=4&reportcode=OFFSHORESHELLPPREGISTRATION&vslid=" + ucVessel.SelectedVessel.ToString() + "&fileno=" + txtFileNo.Text + "&showmenu=0&showexcel=no&showword=no&Contractno=" + txtContractno.Text.Trim();
                }
            }
            else if (CommandName.ToUpper().Equals("SENDMAIL"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("CrewEmail.aspx?fileno=" + txtFileNo.Text + "&vesselid=" + ucVessel.SelectedVessel + "&pdformoffshore=1");
                
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
