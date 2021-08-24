using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewVesselPositionArrival : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);


        MenuVesselPosition.AccessRights = this.ViewState;
        MenuVesselPosition.MenuList = toolbarmain.Show();

    }

    protected void MenuVesselPosition_TabStripCommand(object sender, EventArgs e)
    {
        if (IsValidateVesselPosition(ucVessel.SelectedVessel
            , ddlHard.SelectedHard
            , ddlQuick.SelectedQuick.ToString()
            , ddlPortAdd.SelectedSeaport))
        {
            Save();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidateVesselPosition(string vesselid, string month, string year, string portid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (month.Trim() == "Dummy" || month.Trim() == "")
            ucError.ErrorMessage = "Month is required.";

        if (General.GetNullableInteger(year) == null)
            ucError.ErrorMessage = "Year is required.";

        if (General.GetNullableInteger(portid) == null)
            ucError.ErrorMessage = "Port is required.";
        if (txtETAAdd.Text != null && txtETATimeAdd.SelectedTime == null)
            ucError.ErrorMessage = "ETA date and time is required.";
        if (txtETDAdd.Text != null && txtETDTimeAdd.SelectedTime == null)
            ucError.ErrorMessage = "ETD date and time is required.";

        return (!ucError.IsError);
    }

    protected void Save()
    {
        string eta = txtETAAdd.Text;
        string etd = txtETDAdd.Text;
        string etatime = "";
        string etdtime = "";

        if (txtETATimeAdd.SelectedTime != null)
        {
            etatime = txtETATimeAdd.SelectedTime.Value.ToString();//txtETATimeAdd.TextWithLiterals;
        }
        if (txtETDTimeAdd.SelectedTime != null)
        {
            etdtime = txtETDTimeAdd.SelectedTime.Value.ToString();//.TextWithLiterals;
        }
        PhoenixCrewVesselPosition.InsertVesselPosition(int.Parse(ucVessel.SelectedVessel)
                                        , ((RadComboBox)ddlHard.FindControl("ddlHard")).SelectedItem.Text + ((RadComboBox)ddlQuick.FindControl("ddlQuick")).SelectedItem.Text
                                        , int.Parse(ddlPortAdd.SelectedSeaport)
                                        , General.GetNullableDateTime(eta + " " + etatime)
                                        , General.GetNullableDateTime(etd + " " + etdtime)
                                        );
        ucStatus.Text = "Record saved successfully";
    }

}
