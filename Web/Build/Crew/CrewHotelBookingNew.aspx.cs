using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewHotelBookingNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtcityid.Attributes.Add("style", "visibility:hidden");
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("New", "NEWBOOKING");
                toolbarmain.AddButton("Save", "SAVE");

                MenuHBNewGeneral.AccessRights = this.ViewState;
                MenuHBNewGeneral.MenuList = toolbarmain.Show();

                ViewState["CITYID"] = null;
                ViewState["CITYNAME"] = null;
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuHBNewGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                InsertHotelBooking();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertHotelBooking()
    {
        //if (!IsValidBooking())
        //{
        //    ucError.Visible = true;
        //    return;
        //}
        PhoenixCrewHotelBooking.InsertHotelRoomBooking(
             General.GetNullableInteger(ucVessel.SelectedVessel)
            ,General.GetNullableInteger(txtcityid.Text)
            ,General.GetNullableGuid(ucHotelRoom.SelectedRoomID)
            ,General.GetNullableInteger(txtNoOfBeds.Text)
            ,General.GetNullableInteger(txtExtraBeds.Text)
            ,General.GetNullableDateTime(txtCheckinDate.Text)
            ,General.GetNullableDateTime(txtCheckoutDate.Text)
            ,General.GetNullableInteger(ucCrewChangeReason.SelectedReason)
            ,General.GetNullableInteger(ucCrewChangeReason.SelectedReason)
            );

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void ucHotelRoom_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
           DataSet ds = PhoenixRegistersHotelRoom.HotelRoomList(General.GetNullableGuid(ucHotelRoom.SelectedRoomID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtNoOfBeds.Text = ds.Tables[0].Rows[0]["FLDNOOFBEDS"].ToString();
                if (txtNoOfBeds.Text != null && txtNoOfBeds.Text != "")
                {
                    int? noofbeds = General.GetNullableInteger(txtNoOfBeds.Text);
                    if (noofbeds != null)
                    {
                        if (noofbeds <= 1)
                        {
                            txtExtraBeds.Text = "0";
                            txtExtraBeds.ReadOnly = "true";
                            txtExtraBeds.CssClass = "readonlytextbox";
                        }
                        else
                        {
                            txtExtraBeds.Text = null;
                            txtExtraBeds.ReadOnly = "false";
                            txtExtraBeds.CssClass = "input";
                        }
                    }
                }
            }
            else
            {
                txtNoOfBeds.Text = "0";
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void imgNoOfNights_Click(object sender,EventArgs e)
    {
        try
        {
            if (txtCheckinDate != null && txtCheckoutDate != null)
            {
                if (!IsValidCheckInOutDate(txtCheckinDate.Text, txtCheckoutDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    txtNoOfNights.Text = (DateTime.Parse(txtCheckoutDate.Text) - DateTime.Parse(txtCheckinDate.Text)).TotalDays.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidCheckInOutDate(string checkindate, string checkoutdate)
    {
        if (General.GetNullableDateTime(checkindate) == null)
        {
            ucError.ErrorMessage = "CheckIn Date is required.";
        }
        if (General.GetNullableDateTime(checkindate) == null)
        {
            ucError.ErrorMessage = "checkoutdate Date is required.";
        }

        return (!ucError.IsError);
    }
    protected void txtcityid_Changed(object sender, EventArgs e)
    {
        try
        {
            ViewState["CITYID"] = txtcityid.Text;
            ViewState["CITYNAME"] = txtcityname.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
