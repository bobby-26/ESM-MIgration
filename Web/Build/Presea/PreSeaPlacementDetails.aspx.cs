using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;

public partial class Presea_PreSeaPlacementDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuCrewCompanyExperienceList.AccessRights = this.ViewState;
        MenuCrewCompanyExperienceList.MenuList = toolbar.Show();

        ViewState["placementid"] = "0";
        PlacementDetailsEdit();
    }

    protected void PlacementDetailsEdit()
    {
        DataSet ds = new DataSet();

        ds = PhoenixPreSeaTraineePlacementDetails.PlacementDetailsEdit(int.Parse(Filter.CurrentPreSeaTraineeSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["placementid"] = dr["FLDPLACEMENTID"].ToString();

            txtNameOfShip.Text = dr["FLDNAMEOFSHIP"].ToString();
            txtFlagOfShip.Text = dr["FLDFLAGOFSHIP"].ToString();
            txtIMO.Text = dr["FLDVESSELIMO"].ToString();
            txtOfficialNo.Text = dr["FLDOFFICIALNO"].ToString();
            txtNameOfShippingCompany.Text = dr["FLDNAMEOFCOMPANY"].ToString();
            ucSignOnDate.Text = dr["FLDSIGNONDATE"].ToString();
        }

    }

    protected void CrewCompanyExperienceList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["placementid"].ToString() == "0")
                {
                    PhoenixPreSeaTraineePlacementDetails.PlacementDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection)
                                                                            , txtNameOfShip.Text, txtFlagOfShip.Text, txtIMO.Text, txtOfficialNo.Text, General.GetNullableDateTime(ucSignOnDate.Text), txtNameOfShippingCompany.Text);

                    ucStatus.Text = "Placement Details Saved";
                }
                else
                {
                    PhoenixPreSeaTraineePlacementDetails.PlacementDetailsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["placementid"].ToString()), txtNameOfShip.Text, txtFlagOfShip.Text, txtIMO.Text, txtOfficialNo.Text, General.GetNullableDateTime(ucSignOnDate.Text), txtNameOfShippingCompany.Text);

                    ucStatus.Text = "Placement Details Saved";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
