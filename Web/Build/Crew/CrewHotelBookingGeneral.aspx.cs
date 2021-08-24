using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewHotelBookingGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("New", "NEWBOOKING");
                toolbarmain.AddButton("Save", "SAVE");

                MenuHBGeneral.AccessRights = this.ViewState;
                MenuHBGeneral.MenuList = toolbarmain.Show();

                ViewState["BOOKINGID"] = null;
                ViewState["ROOMTYPEID"] = null;
                ViewState["PAYABLECHARGES"] = null;
                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                }
                cblComanyPayableCharges.DataSource  = PhoenixRegistersHotelCharges.ListHotelCharges(PhoenixSecurityContext.CurrentSecurityContext.UserCode);                
                cblComanyPayableCharges.DataBind();

                BindBooking();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuHBGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEWBOOKING"))
            {
                Response.Redirect("../Crew/CrewHotelBookingNew.aspx");
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strPayableChargeList = new StringBuilder();
                foreach (ListItem item in cblComanyPayableCharges.Items)
                {
                    if (item.Selected == true)
                    {
                        strPayableChargeList.Append(item.Value.ToString());
                        strPayableChargeList.Append(",");
                    }

                }
                if (strPayableChargeList.Length > 1)
                {
                    strPayableChargeList.Remove(strPayableChargeList.Length - 1, 1);
                }

                PhoenixCrewHotelBooking.UpdateHotelBooking(
                    General.GetNullableGuid(ViewState["BOOKINGID"] != null ? ViewState["BOOKINGID"].ToString() : null)
                    , General.GetNullableGuid(ViewState["ROOMTYPEID"] != null ? ViewState["ROOMTYPEID"].ToString() : null)
                    , General.GetNullableInteger(txtNoOfBeds.Text)
                    , General.GetNullableInteger(txtExtraBeds.Text)
                    , General.GetNullableInteger(txtNoOfRooms.Text)
                    , General.GetNullableDateTime(txtCheckinDate.Text)
                    , General.GetNullableDateTime(txtCheckoutDate.Text)
                    , null
                    , General.GetNullableInteger(ucCrewChangeReason.SelectedReason)
                    , General.GetNullableInteger(ucPurpose.SelectedReason)
                    , General.GetNullableInteger(ucPaymentmode.SelectedHard.ToString())
                    , strPayableChargeList.ToString()
                    , null
                    , null);
                ucStatus.Text = "Information Saved";
                BindBooking();
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

    }
    private void BindBooking()
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewHotelBooking.HotelBookingEdit(General.GetNullableGuid(ViewState["BOOKINGID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            //txtCityName.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
            ucHotelRoom.SelectedValue = dt.Rows[0]["FLDROOMTYPEID"].ToString();
            ViewState["ROOMTYPEID"] = dt.Rows[0]["FLDROOMTYPEID"].ToString();
            ucHotelRoom.SelectedRoomID = dt.Rows[0]["FLDROOMTYPEID"].ToString();
            txtExtraBeds.Text = dt.Rows[0]["FLDEXTRABEDS"].ToString();
            txtNoOfRooms.Text = dt.Rows[0]["FLDNOOFROOMS"].ToString();
            txtCheckinDate.Text = dt.Rows[0]["FLDCHECKINDATE"].ToString();
            txtCheckoutDate.Text = dt.Rows[0]["FLDCHECKOUTDATE"].ToString();
            ucCrewChangeReason.SelectedReason = dt.Rows[0]["FLDCREWCHANGEREASON"].ToString();
            ucPurpose.SelectedReason = dt.Rows[0]["FLDPURPOSEID"].ToString();
            ucPaymentmode.SelectedHard = dt.Rows[0]["FLDPAYMENTBY"].ToString();

            string str = dt.Rows[0]["FLDCOMPANYACCOUNTCHARGES"].ToString();
            ViewState["PAYABLECHARGES"] = str;

            if (str.Length > 1)
            {
                string[] PayableCharges = str.Split(',');
                foreach (string s in PayableCharges)
                {
                    foreach (ListItem item in cblComanyPayableCharges.Items)
                    {
                        if (int.Parse(s) == int.Parse(item.Value))
                            item.Selected = true;
                    }
                }
            }           
            CheckPayment(dt.Rows[0]["FLDPAYMENTBY"].ToString());
            CheckNoOfBeds();
            NoOfNightsCheck();
            lblReferenceNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();

            if(ViewState["ROOMTYPEID"] != null)
                NoOfBeds(General.GetNullableGuid(ViewState["ROOMTYPEID"].ToString()));
        }
    }
    protected void ucHotelRoom_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            ViewState["ROOMTYPEID"] = General.GetNullableGuid(ucHotelRoom.SelectedRoomID);

            if (ViewState["ROOMTYPEID"] != null)
                CheckNoOfBeds();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CheckNoOfBeds()
    {
        if (ViewState["ROOMTYPEID"] != null)
        {
            DataSet ds = PhoenixRegistersHotelRoom.HotelRoomList(General.GetNullableGuid(ViewState["ROOMTYPEID"].ToString()));
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
                            txtExtraBeds.ReadOnly = "false";
                            txtExtraBeds.CssClass = "input";
                        }
                    }
                }
            }
        }
        else
        {
            txtNoOfBeds.Text = "0";
        }
    }
    protected void imgNoOfNights_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidCheckInOutDate(txtCheckinDate.Text, txtCheckoutDate.Text))
            {
                ucError.Visible = true;
                return;
            }
            NoOfNightsCheck();
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
    private void NoOfNightsCheck()
    {
        if (txtCheckinDate != null && txtCheckoutDate != null)
        {
            txtNoOfNights.Text = (DateTime.Parse(txtCheckoutDate.Text) - DateTime.Parse(txtCheckinDate.Text)).TotalDays.ToString();
        }
    }
    protected void ucPaymentmode_TextChangedEvent(object sender, EventArgs args)
    {
        CheckPayment(ucPaymentmode.SelectedHard);
    }
    private void CheckPayment(string paymentmode)
    {
        if (paymentmode == "976")
        {
            cblComanyPayableCharges.Enabled = true;
        }        
        else
        {
            foreach (ListItem item in cblComanyPayableCharges.Items)
            {
                item.Selected = false;
            }
            cblComanyPayableCharges.Enabled = false;
        }
        if (ViewState["PAYABLECHARGES"] != null)
        {
        }
    }
    private void NoOfBeds(Guid? roomtypeid)
    {
        DataSet ds = PhoenixRegistersHotelRoom.HotelRoomList(roomtypeid);
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
}
