using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewTravelPassengersSeafarerEntry : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            Session["ENTRYPAGE"] = null;                  //USED IN PARENT PAGE TO SET NAVIGATION EITHER TO PASSENGERS ENTRY OR LIST.
         
            if (Request.QueryString["travelrequestedit"] != null)
            {
                ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();
            }
            if (Request.QueryString["travelid"] != null)
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                EditTravel();
            }

            if (Request.QueryString["requestid"] != null)
                ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
            Binddetails();
        }
     
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        if (ViewState["EDITTRAVELREQUEST"].ToString() == "1")
        {
            toolbar1.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbar1.AddButton("New", "NEW",ToolBarDirection.Right);            
            MenuCrewTraveladd.AccessRights = this.ViewState;
            MenuCrewTraveladd.Title = PhoenixCrewTravelRequest.RequestNo;
            MenuCrewTraveladd.MenuList = toolbar1.Show();
        }
    }
    private void EditTravel()
    {
        DataSet ds = PhoenixCrewTravelRequest.EditTravel(
            General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["PURPOSE"] = ds.Tables[0].Rows[0]["FLDTRAVELTYPE"].ToString();
            txtorigindate.Text = ds.Tables[0].Rows[0]["FLDDATEOFCREWCHANGE"].ToString();
        }

    }
    private void Binddetails()
    {
        DataTable dt = PhoenixCrewTravelRequest.EditTravelRequest
                            (General.GetNullableGuid(ViewState["REQUESTID"] == null ? null : ViewState["REQUESTID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            details.Visible = true;
            lblDateofBirth.Visible = true;
            txtdob.Visible = true;

            lblPassportNo.Visible = true;
            txtPassport.Visible = true;

            imgPassenger.Visible = false;
            DataRow dr = dt.Rows[0];

            //Title1.Text = "Passenger Details - " + dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Passengers List", "PASSENGERSLIST");
            toolbar.AddButton("Passenger Details", "PASSENGERSENTRY");
            if (ViewState["REQUESTID"] != null)
                toolbar.AddButton("Journey Details", "BREAKUP");

            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 1;

            txtName.Text = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();
            txtEmployeeId.Text = dr["FLDEMPLOYEEID"].ToString();
            txtdob.Text = dr["FLDDATEOFBIRTH"].ToString();

            txtPassport.Text = dr["FLDPASSPORTNO"].ToString();
            txtcdcno.Text = dr["FLDSEAMANBOOKNO"].ToString();
            txtusvisa.Text = dr["FLDUSVISANUMBER"].ToString();
            txtothervisa.Text = dr["FLDOTHERVISADETAILS"].ToString();

            txtorigindate.Text = String.Format("{0:MM/dd/yy}", dr["FLDTRAVELDATE"].ToString());
            ddlampmdeparture.SelectedValue = dr["FLDDEPARTUREAMPM"].ToString();

            txtOrigin.SelectedValue = dr["FLDORIGINID"].ToString();
            txtOrigin.Text = dr["FLDORIGINNAME"].ToString();

            txtDestinationdate.Text = String.Format("{0:MM/dd/yy}", dr["FLDARRIVALDATE"].ToString());
            ddlampmarrival.SelectedValue = dr["FLDARRIVALAMPM"].ToString();
            txtDestination.SelectedValue = dr["FLDDESTINATIONID"].ToString();
            txtDestination.Text = dr["FLDDESTINATIONNAME"].ToString();

            ucPaymentmodeAdd.SelectedHard = dr["FLDPAYMENTMODE"].ToString();

            ddlonsigner.SelectedValue = dr["FLDONSIGNERYN"].ToString();

            txtpdateofissue.Text = String.Format("{0:MM/dd/yy}", dr["FLDPASSPORTDATEOFISSUE"].ToString());
            txtpplaceodissue.Text = dr["FLDPASSPORTPLACEOFISSUE"].ToString();
            txtpdateofexpiry.Text = String.Format("{0:MM/dd/yy}", dr["FLDPASSPORTEXPIRYDATE"].ToString());

            txtcdcdateofissue.Text = String.Format("{0:MM/dd/yy}", dr["FLDCDCDATEOFISSUE"].ToString());
            txtcdcplaceofissue.Text = dr["FLDCDCPLACEOFISSUE"].ToString();
            txtcdcdateofexpiry.Text = String.Format("{0:MM/dd/yy}", dr["FLDCDCEXPIRYDATE"].ToString());
            ucnationality.SelectedNationality = dr["FLDNATIONALITY"].ToString();

        }
        else
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Passengers List", "PASSENGERSLIST");
            toolbar.AddButton("Passenger Entry", "PASSENGERSENTRY");
            if (ViewState["REQUESTID"] != null)
                toolbar.AddButton("Journey Details", "BREAKUP");

            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 1;
        }

    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PASSENGERSLIST"))
            {
                Response.Redirect("CrewTravelPassengersList.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("PASSENGERSENTRY"))
            {
                if (PhoenixCrewTravelRequest.OfficeTravelPassengerFrom.ToString() == "1")
                    Response.Redirect("CrewTravelPassengersSeafarerEntry.aspx?"+Request.QueryString, false);
                else
                    Response.Redirect("CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("BREAKUP"))
            {
                Response.Redirect("CrewTravelPassengerBreakupdetails.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString() + "&requestid=" + ViewState["REQUESTID"] + "&name=" + txtName.Text, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewTraveladd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearAll();
                details.Visible = false;
                imgPassenger.Visible = true;
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["REQUESTID"] == null)   //New Request
                {
                    if (!IsValidTravelRequest(txtName.Text.Trim(), txtOrigin.SelectedValue, txtDestination.SelectedValue,
                                               txtorigindate.Text + " " + "00:00" + (ddlampmdeparture.SelectedValue == "1" ? "AM" : "PM"),
                                               txtDestinationdate.Text + " " + "00:00" + (ddlampmarrival.SelectedValue == "1" ? "AM" : "PM"),
                                               ucPaymentmodeAdd.SelectedHard, txtorigindate.Text, txtDestinationdate.Text)
                        )
                    {
                        ucError.Visible = true;
                        return;
                    }

                    Guid? requestid = null;
                    PhoenixCrewTravelRequest.InsertPassengersSeafarersDetails(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , int.Parse(txtEmployeeId.Text)
                                , General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString())
                                , int.Parse(txtOrigin.SelectedValue)
                                , int.Parse(txtDestination.SelectedValue)
                                , DateTime.Parse(txtorigindate.Text)
                                , DateTime.Parse(txtDestinationdate.Text)
                                , int.Parse(ucPaymentmodeAdd.SelectedHard)
                                , int.Parse(ViewState["PURPOSE"].ToString())
                                , int.Parse(ddlampmdeparture.SelectedValue)
                                , int.Parse(ddlampmarrival.SelectedValue)
                                , null
                                , ref requestid);

                    ViewState["REQUESTID"] = requestid;

                }
                else
                {
                    if (!IsValidTravelRequest(txtName.Text.Trim(), txtOrigin.SelectedValue, txtDestination.SelectedValue,
                                               txtorigindate.Text + " " + "00:00" + (ddlampmdeparture.SelectedValue == "1" ? "AM" : "PM"),
                                               txtDestinationdate.Text + " " + "00:00" + (ddlampmarrival.SelectedValue == "1" ? "AM" : "PM"),
                                               ucPaymentmodeAdd.SelectedHard, txtorigindate.Text, txtDestinationdate.Text)
                        )
                    {
                        ucError.Visible = true;
                        return;
                    }


                    PhoenixCrewTravelRequest.UpdatePassengersSeafarersDetails(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString())
                                , General.GetNullableGuid(ViewState["REQUESTID"] == null ? null : ViewState["REQUESTID"].ToString())
                                , int.Parse(txtOrigin.SelectedValue)
                                , int.Parse(txtDestination.SelectedValue)
                                , DateTime.Parse(txtorigindate.Text)
                                , DateTime.Parse(txtDestinationdate.Text)
                                , int.Parse(ucPaymentmodeAdd.SelectedHard)
                                , int.Parse(ViewState["PURPOSE"].ToString())
                                , int.Parse(ddlampmdeparture.SelectedValue)
                                , int.Parse(ddlampmarrival.SelectedValue)
                                , null);
                }
                Binddetails();
                ucstatus.Text = "Passenger Details saved successfully.";
                ucstatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearAll()
    {
        lblDateofBirth.Visible = false;
        txtdob.Visible = false;

        lblPassportNo.Visible = false;
        txtPassport.Visible = false;

        //txtName.Text = txtmiddlename.Text = txtlastname.Text = txtdob.Text = txtcdcno.Text = txtDestinationdate.Text = "";
        txtName.Text = "";
        txtEmployeeId.Text = "";
        txtOrigin.Text = txtDestination.Text = "";
        txtothervisa.Text = txtusvisa.Text = txtPassport.Text = "";
        ddlonsigner.SelectedValue = "2";
        ddlampmarrival.SelectedValue = "1";
        ddlampmdeparture.SelectedValue = "1";
        ucPaymentmodeAdd.SelectedHard = "";
        ucnationality.SelectedNationality = "";
        txtName.Enabled = true;
        txtorigindate.Text = "";
        txtDestinationdate.Text = "";

        ViewState["REQUESTID"] = null;
        //Title1.Text = "Passenger Entry";
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Passengers List", "PASSENGERSLIST");
        toolbar.AddButton("Passenger Entry", "PASSENGERSENTRY");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbar.Show();
        CrewMenu.SelectedMenuIndex = 1;

    }
    private bool IsValidTravelRequest(string fname, string origin, string destination, string departuredate,
                                                string arrivaldate, string paymentmode, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (string.IsNullOrEmpty(fname.Trim()) || string.IsNullOrEmpty(txtEmployeeId.Text.ToString().Trim()))
            ucError.ErrorMessage = "Employee is required.";

        if (string.IsNullOrEmpty(origin.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival date is required.";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later or equal to departure date";


        if (paymentmode.ToString().ToUpper() == "DUMMY" || string.IsNullOrEmpty(paymentmode.Trim()))
            ucError.ErrorMessage = "Payment mode is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;
        if (Filter.CurrentPickListSelection.Keys.Count < 5)
            return;

        DataSet ds = PhoenixCrewTravelRequest.EditPassengerUser(
                     General.GetNullableInteger(Filter.CurrentPickListSelection.Get(3)));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtName.Text = dr["FLDFIRSTNAME"].ToString();
            txtdob.Text = dr["FLDDATEOFBIRTH"].ToString();
            txtPassport.Text = dr["FLDPASSPORTNO"].ToString();
            txtpdateofexpiry.Text = dr["FLDEXPIRYDATE"].ToString();
            txtpdateofissue.Text = dr["FLDDATEOFISSUE"].ToString();
            txtpplaceodissue.Text = dr["FLDPLACEOFISSUE"].ToString();
            ucnationality.SelectedNationality = dr["FLDNATIONALITY"].ToString();
        }

    }
}
