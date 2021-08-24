using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewHRTravelRequestGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("List", "LIST",ToolBarDirection.Right);

        MenuHRTravelRequestGeneral.AccessRights = this.ViewState;
        MenuHRTravelRequestGeneral.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbarbutton = new PhoenixToolbar();

        toolbarbutton.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuHRTravelRequestDetail.AccessRights = this.ViewState;
        MenuHRTravelRequestDetail.MenuList = toolbarbutton.Show();
        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            ViewState["TRAVELREQUESTID"] = "";
        }
    }

    protected void HRTravelRequestGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewHRTravelRequestList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuHRTravelRequestDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString()) == null)
                {
                    if (!IsValidTravelRequest(txtDepatureCityId.SelectedValue, txtDestinationCityId.SelectedValue,
                                               txtDepaturedate.Text + " " + "00:00" + (ddlDepartureTime.SelectedValue == "1" ? "AM" : "PM"),
                                               txtDepaturedate.Text)
                        )
                    {
                        ucError.Visible = true;
                        return;
                    }

                    Guid travelrequestid = new Guid();
                    int personalinfosn = 0;

                    int? vessel;

                    if (rblAccountFrom.SelectedValue == "0")
                    {
                        vessel = 0;
                    }
                    else
                    {
                        vessel = General.GetNullableInteger(ddlVessel.SelectedVessel);
                    }

                    PhoenixCrewHRTravelRequest.HRTravelRequestInsert(null
                               , vessel
                               , int.Parse(txtDepatureCityId.SelectedValue)
                               , General.GetNullableString(txtDepatureCityId.Text)
                               , DateTime.Parse(txtDepaturedate.Text)
                               , int.Parse(ddlDepartureTime.SelectedValue)
                               , int.Parse(txtDestinationCityId.SelectedValue)
                               , General.GetNullableString(txtDestinationCityId.Text)
                               , null
                               , null
                               , ref travelrequestid
                               , ref personalinfosn
                               );
                    ucstatus.Text = "Family travel request created.";
                    ucstatus.Visible = true;
                    Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + travelrequestid + "&personalinfosn=" + personalinfosn, false);

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelRequest(string depature, string destination, string departuredatetime, string departuredate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Travel date is required.";
        if (string.IsNullOrEmpty(depature.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (rblAccountFrom.SelectedValue == "1")
        {
            if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                ucError.ErrorMessage = "Vessel is required.";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    
    protected void rblAccountFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAccountFrom.SelectedValue == "0")
        {
            ddlVessel.Enabled = false;
            ddlVessel.CssClass = "input";
            ddlVessel.SelectedVessel = "";
        }
        else
        {
            ddlVessel.Enabled = true;
            ddlVessel.CssClass = "dropdown_mandatory";
        }
    }
}
