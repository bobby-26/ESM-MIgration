using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class CrewOffshoreCrewListFormats : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.bind();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    {
                        ucVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                        ucVessel.bind();
                    }
                }

                if (rblFormat != null)
                {
                    rblFormat.SelectedValue = "1";
                    if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                    {
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=DEFAULTCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    }
                }
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (rblFormat.SelectedValue == "1")
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=DEFAULTCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    else if (rblFormat.SelectedValue == "2")
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=IMOCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    else if (rblFormat.SelectedValue == "3")
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=OFFSHORESHELLWEEKLYCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    else if (rblFormat.SelectedValue == "4")
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=OFFSHORESHELLQUARTERLYCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    else if (rblFormat.SelectedValue == "5")
                        ifMoreInfo.Attributes["src"] = "../Reports/ReportsViewWithSubReport.aspx" + "?applicationcode=11&reportcode=OFFSHOREOVIDCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                    else if (rblFormat.SelectedValue == "6")
                        ifMoreInfo.Attributes["src"] = "../crewOffshore/CrewOffshoreReportCrewListFormatOSVIS.aspx" + "?vesselid=" + ucVessel.SelectedVessel.ToString() + "&Vesselname=" + ucVessel.SelectedVesselName;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidFilter(string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (rblFormat.SelectedValue == "")
        {
            ucError.ErrorMessage = "Select Format";
        }

        return (!ucError.IsError);
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ucVessel.SelectedVessel;

        if (rblFormat.SelectedValue == "1")
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=DEFAULTCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
        else if (rblFormat.SelectedValue == "2")
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=IMOCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
        else if (rblFormat.SelectedValue == "3")
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=OFFSHORESHELLWEEKLYCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
        else if (rblFormat.SelectedValue == "4")
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=11&reportcode=OFFSHORESHELLQUARTERLYCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
        else if (rblFormat.SelectedValue == "5")
            ifMoreInfo.Attributes["src"] = "../Reports/ReportsViewWithSubReport.aspx" + "?applicationcode=11&reportcode=OFFSHOREOVIDCREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
        else if (rblFormat.SelectedValue == "6")
            ifMoreInfo.Attributes["src"] = "../crewOffshore/CrewOffshoreReportCrewListFormatOSVIS.aspx" + "?vesselid=" + ucVessel.SelectedVessel.ToString() + "&Vesselname=" + ucVessel.SelectedVesselName;
    }
}
