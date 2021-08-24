using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;

public partial class CrewTravelPlanHistoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuHistory.AccessRights = this.ViewState;
        MenuHistory.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["TRAVELHISTORYID"] = "";
            ViewState["EMPID"] = "";

            if (Request.QueryString["TravelHistoryId"] != null)
            {
                ViewState["TRAVELHISTORYID"] = Request.QueryString["TravelHistoryId"].ToString();

                EditCrewtravelHistory();
            }
            if (Request.QueryString["empid"] != null)
            {
                ViewState["EMPID"] = Request.QueryString["empid"].ToString();
                //EditCrewtravelHistory();
            }

            BindAccount();
            if (Request.QueryString["TravelHistoryId"] != null)
            {
                ViewState["TRAVELHISTORYID"] = Request.QueryString["TravelHistoryId"].ToString();

                EditCrewtravelHistory();
            }
        }
    }

    public void BindAccount()
    {
        ddlAccountDetails.SelectedValue = "";
        ddlAccountDetails.Text = "";


        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ucVessel.SelectedVessel) == 0 ? null : General.GetNullableInteger(ucVessel.SelectedVessel), 1);
        ddlAccountDetails.DataBind();
    }
    public void BindVesselAccount()
    {
        string vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());

     
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(vesselid) == 0 ? null : General.GetNullableInteger(vesselid), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ucVessel_OnTextChanged(object sender, EventArgs e)
    {
        BindAccount();
    }

    protected void EditCrewtravelHistory()
    {
        DataTable dt = PhoenixCrewTravelDocument.EditEmployeeTravelHistory(new Guid(ViewState["TRAVELHISTORYID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            ucOriginCity.SelectedValue = dt.Rows[0]["FLDORIGINID"].ToString();
            ucOriginCity.Text = dt.Rows[0]["FLDORIGINCITY"].ToString();
            ucDestinationCity.SelectedValue = dt.Rows[0]["FLDDESTINATIONID"].ToString();
            ucDestinationCity.Text = dt.Rows[0]["FLDDESTINATIONCITY"].ToString();
            ucDepDate.Text = dt.Rows[0]["FLDDEPARTUREDATE"].ToString();
            ucArrDate.Text = dt.Rows[0]["FLDARRIVALDATE"].ToString();
            ucCurrency.SelectedCurrency = dt.Rows[0]["FLDCURRENCYID"].ToString();
            ucClass.SelectedHard = dt.Rows[0]["FLDCLASSID"].ToString();
            txtAirlineCode.Text = dt.Rows[0]["FLDAIRLINECODE"].ToString();
            txtTicketNo.Text = dt.Rows[0]["FLDTICKETNO"].ToString();
            txtAmount.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            txtTax.Text = dt.Rows[0]["FLDTAX"].ToString();
            txtPNR.Text = dt.Rows[0]["FLDPNRNO"].ToString();

            if (General.GetNullableInteger(dt.Rows[0]["FLDVESSELID"].ToString())!= null)
            {
                ucVessel.SelectedValue = int.Parse(dt.Rows[0]["FLDVESSELID"].ToString());
            }
            
            //ddlAccountDetails.SelectedValue= dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
            ViewState["Vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
            BindVesselAccount();
            if (dt.Rows[0]["FLDVESSELACCOUNTID"].ToString() != null)
            {
                foreach (RadComboBoxItem item in ddlAccountDetails.Items)
                {
                    if (item.Value == dt.Rows[0]["FLDVESSELACCOUNTID"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

                //ddlAccountDetails.SelectedValue = dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
            }
            else
            {
                ddlAccountDetails.SelectedValue = "";
            }
            //ddlAccountDetails.SelectedValue = dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
        }
    }


    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (ViewState["TRAVELHISTORYID"] != null && ViewState["TRAVELHISTORYID"].ToString() != "" && ViewState["EMPID"].ToString() != "")
                {

                    if (!IsValidTravelBreakUp(ucDepDate.Text, ucArrDate.Text, ucOriginCity.SelectedValue, ucDestinationCity.SelectedValue, txtAirlineCode.Text
                        , ucClass.SelectedHard, txtAmount.Text, txtTax.Text, txtTicketNo.Text, txtPNR.Text,ucVessel.SelectedVessel,ddlAccountDetails.SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewTravelDocument.UpdateEmployeeTravelHistory(
                        new Guid(ViewState["TRAVELHISTORYID"].ToString()),
                         Convert.ToInt32(ViewState["EMPID"].ToString())
                       , General.GetNullableInteger(ucOriginCity.SelectedValue)
                       , General.GetNullableInteger(ucDestinationCity.SelectedValue)
                       , General.GetNullableDateTime(ucDepDate.Text)
                       , General.GetNullableDateTime(ucArrDate.Text)
                       , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                       , General.GetNullableString(txtAirlineCode.Text)
                       , General.GetNullableInteger(ucClass.SelectedHard)
                       , General.GetNullableDecimal(txtAmount.Text)
                       , General.GetNullableDecimal(txtTax.Text)
                       , General.GetNullableString(txtTicketNo.Text)
                       , General.GetNullableString(txtPNR.Text)
                       , General.GetNullableInteger(ucVessel.SelectedValue.ToString())
                       , General.GetNullableString(ucVessel.SelectedVesselName.ToString())
                       , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                       , General.GetNullableString(ddlAccountDetails.Text)
                       );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

                }
                else
                {
                    if (!IsValidTravelBreakUp(ucDepDate.Text, ucArrDate.Text, ucOriginCity.SelectedValue, ucDestinationCity.SelectedValue, txtAirlineCode.Text
                       , ucClass.SelectedHard, txtAmount.Text, txtTax.Text, txtTicketNo.Text, txtPNR.Text, ucVessel.SelectedVessel, ddlAccountDetails.SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    if (ViewState["EMPID"] != null && ViewState["EMPID"].ToString() != "")
                    {
                        PhoenixCrewTravelDocument.InsertEmployeeTravelHistory(
                        Convert.ToInt32(ViewState["EMPID"].ToString())
                      , General.GetNullableInteger(ucOriginCity.SelectedValue)
                      , General.GetNullableInteger(ucDestinationCity.SelectedValue)
                      , General.GetNullableDateTime(ucDepDate.Text)
                      , General.GetNullableDateTime(ucArrDate.Text)
                      , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                      , General.GetNullableString(txtAirlineCode.Text)
                      , General.GetNullableInteger(ucClass.SelectedHard)
                      , General.GetNullableDecimal(txtAmount.Text)
                      , General.GetNullableDecimal(txtTax.Text)
                      , General.GetNullableString(txtTicketNo.Text)
                      , General.GetNullableString(txtPNR.Text)
                      , General.GetNullableInteger(ucVessel.SelectedValue.ToString())
                      , General.GetNullableString(ucVessel.SelectedVesselName.ToString())
                      , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                      , General.GetNullableString(ddlAccountDetails.Text)
                      );
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
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

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination
                                      , string airlinecode, string aclass, string amount, string tax, string ticketno, string pnr,string vessel,string vesselaccount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if(vessel.Trim()=="")
            ucError.ErrorMessage = "Vessel is required.";

        if (vesselaccount.Trim() == "")
            ucError.ErrorMessage = "Vessel Account is required.";

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        else if (General.GetNullableDateTime(departuredate) == null || string.IsNullOrEmpty(departuredate))
            ucError.ErrorMessage = "Departure Date is required.";

        else if (General.GetNullableDateTime(arrivaldate) == null || string.IsNullOrEmpty(arrivaldate))
            ucError.ErrorMessage = "Arrival Date is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date";

        if (string.IsNullOrEmpty(ticketno.Trim()))
            ucError.ErrorMessage = "Ticket No  is required.";

        if (amount.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";

        return (!ucError.IsError);
    }



}