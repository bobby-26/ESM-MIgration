using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewTravelPassengerBreakupdetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Passengers List", "PASSENGERSLIST");
        toolbar.AddButton("Passenger Details", "PASSENGERSENTRY");
        toolbar.AddButton("Journey Details", "BREAKUP");

        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbar.Show();
        CrewMenu.SelectedMenuIndex = 2;



        if (!IsPostBack)
        {
            cmdHiddenPick.Attributes.Add("style", "display:none");        
            if (Request.QueryString["requestid"] != null)
                ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();

            if (Request.QueryString["travelrequestedit"] != null)
            {
                ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();
            }
            if (Request.QueryString["travelid"] != null)
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
            }
            if (Request.QueryString["name"] != null)
            { 
                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                MenuTab.AccessRights = this.ViewState;
                MenuTab.Title = PhoenixCrewTravelRequest.RequestNo + " - " + Request.QueryString["name"];
                MenuTab.MenuList = toolbar1.Show();            
            }

            SetTravel();
        }

    }

    private void SetTravel()
    {
        DataSet ds = null;
        if (ViewState["TRAVELID"] != null)
            ds = PhoenixCrewTravelRequest.EditTravel(new Guid(ViewState["TRAVELID"].ToString()));
        ViewState["Vesselid"] = null;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Vesselid"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
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
                    Response.Redirect("CrewTravelPassengersSeafarerEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&requestid=" + ViewState["REQUESTID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
                else
                    Response.Redirect("CrewTravelPassengersEntry.aspx?travelid=" + ViewState["TRAVELID"] + "&requestid=" + ViewState["REQUESTID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);

            }
            if (CommandName.ToUpper().Equals("BREAKUP"))
            {
                Response.Redirect("CrewTravelPassengerBreakupdetails.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString() + "&requestid=" + ViewState["REQUESTID"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUpGeneral(
                General.GetNullableGuid(ViewState["REQUESTID"] == null ? "" : ViewState["REQUESTID"].ToString()));

            gvCTBreakUp.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCTBreakUp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                int? employeeid = null;
                string vesselid;

                vesselid = ViewState["Vesselid"].ToString();

                string orginid = ((UserControlMultiColumnCity)e.Item.FindControl("txtOriginIdBreakupAdd")).SelectedValue;
                string destinationid = ((UserControlMultiColumnCity)e.Item.FindControl("txtDestinationIdBreakupAdd")).SelectedValue;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateAdd")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateAdd")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmAdd")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmAdd")).SelectedValue;
                string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeAdd")).SelectedReason;
                string travelclass = ((UserControlHard)e.Item.FindControl("ucClassAdd")).SelectedHard;

                if (!IsValidTravelBreakUp(departuredate, arrivaldate, orginid, destinationid, purposeid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.InsertTravelBreakUp(
                        employeeid,
                      General.GetNullableInteger(vesselid),
                       General.GetNullableInteger(orginid),
                       General.GetNullableInteger(destinationid),
                       DateTime.Parse(departuredate),
                       General.GetNullableInteger(departureampm),
                       DateTime.Parse(arrivaldate),
                       General.GetNullableInteger(arrivalampm), General.GetNullableInteger(purposeid),
                       General.GetNullableGuid(ViewState["REQUESTID"].ToString()),
                      General.GetNullableInteger(travelclass));

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpIdEdit")).Text;
            string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeOld")).SelectedReason;
            string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
            string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;            
            string origin = ((UserControlMultiColumnCity)e.Item.FindControl("txtOriginIdOldBreakup")).SelectedValue;
            string destination = ((UserControlMultiColumnCity)e.Item.FindControl("txtDestinationIdOldBreakup")).SelectedValue;            
            string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
            string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;
            string travelclass = ((UserControlHard)e.Item.FindControl("ucClassOld")).SelectedHard;

            string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
            string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

            if (!IsValidTravelBreakUp(strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, departuredate, arrivaldate))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelRequest.UpdateTravelBreakUpGeneral(
                   new Guid(breakupid),
                   General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                  General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                  General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm),
                  General.GetNullableInteger(travelclass));

            BindDataTravelBreakUp();
            gvCTBreakUp.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCTBreakUp_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpId")).Text;

            if (General.GetNullableGuid(breakupid) != null)
            {
                PhoenixCrewTravelRequest.DeleteTravelBreakUpGeneral(new Guid(breakupid));

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCTBreakUp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
            RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

            if (departureampm != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                departureampm.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
            if (arrivalampm != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                arrivalampm.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();

            UserControlHard ucClassOld = ((UserControlHard)e.Item.FindControl("ucClassOld"));
            if (ucClassOld != null) ucClassOld.SelectedHard = drv["FLDTRAVELCLASS"].ToString();

            UserControlTravelReason ucPurposeOld = (UserControlTravelReason)e.Item.FindControl("ucPurposeOld");
            if (ucPurposeOld != null) ucPurposeOld.SelectedReason = drv["FLDPURPOSEID"].ToString();

            UserControlMultiColumnCity ucorigin = (UserControlMultiColumnCity)e.Item.FindControl("txtOriginIdOldBreakup");
            if (ucorigin != null)
            {
                ucorigin.SelectedValue = drv["FLDORIGINID"].ToString();
                ucorigin.Text = drv["FLDORIGINNAME"].ToString();
            }

            UserControlMultiColumnCity ucDestination = (UserControlMultiColumnCity)e.Item.FindControl("txtDestinationIdOldBreakup");
            if (ucDestination != null)
            {
                ucDestination.SelectedValue = drv["FLDDESTINATIONID"].ToString();
                ucDestination.Text = drv["FLDDESTINATIONNAME"].ToString();
            }

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

        }

    }


    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "Arrival Date is required";

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Purpose is required.";



        return (!ucError.IsError);
    }


    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;


        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival Date is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than or equal departure date";

        return (!ucError.IsError);
    }


    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;
        if (ViewState["CURRENTROW"] != null)
        {
            int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString());
            RadTextBox txtoriginname = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginNameBreakup");
            RadTextBox txtoriginid = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginIdBreakup");
            if (txtoriginid != null && txtoriginname != null)
            {
                txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
                txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
            }
        }
    }
    
}
