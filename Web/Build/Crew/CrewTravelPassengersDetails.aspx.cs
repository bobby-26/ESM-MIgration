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
public partial class CrewTravelPassengersDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");

            ViewState["EDITROW"]            =   "0";
            ViewState["CURRENTROW"]         =   null;
            Session["ENTRYPAGE"]            =   null;     
             //USED IN PARENT PAGE TO SET NAVIGATION EITHER TO PASSENGERS ENTRY OR LIST.

            toolbar.AddButton("Passengers List", "PASSENGERSLIST");
            toolbar.AddButton("Passenger Details", "PASSENGERSENTRY");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 1;
           
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

            if (ViewState["REQUESTID"] != null)
                Binddetails();            
        }

        BindDataTravelBreakUp();
    }
    private void EditTravel()
    {
        DataSet ds = PhoenixCrewTravelRequest.EditTravel(
            General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            Title1.Text = "Passenger Details (" + ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + ")";
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["PURPOSE"] = ds.Tables[0].Rows[0]["FLDTRAVELTYPE"].ToString();            
        }

    }
    private void Binddetails()
    {
        DataTable dt = PhoenixCrewTravelRequest.EditTravelRequest
                            (General.GetNullableGuid(ViewState["REQUESTID"] == null ? null : ViewState["REQUESTID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            Title1.Text = "Passenger Details - " + dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

            txtFirstname.Text = dr["FLDFIRSTNAME"].ToString();
            txtmiddlename.Text = dr["FLDMIDDLENAME"].ToString();
            txtlastname.Text = dr["FLDLASTNAME"].ToString();
            txtdob.Text = dr["FLDDATEOFBIRTH"].ToString();

            txtPassport.Text = dr["FLDPASSPORTNO"].ToString();
            txtcdcno.Text = dr["FLDSEAMANBOOKNO"].ToString();
            txtusvisa.Text = dr["FLDUSVISANUMBER"].ToString();
            txtothervisa.Text = dr["FLDOTHERVISADETAILS"].ToString();          

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

    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("PASSENGERSLIST"))
            {
                Response.Redirect("CrewTravelAllPassengersList.aspx?travelid=" + ViewState["TRAVELID"] + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("PASSENGERSENTRY"))
            {
                Response.Redirect("CrewTravelPassengersDetails.aspx?travelid=" + ViewState["TRAVELID"], false);
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
            DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUp(
                General.GetNullableGuid(ViewState["REQUESTID"] == null ? "" : ViewState["REQUESTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;
                gvCTBreakUp.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTBreakUp);
            }
            gvCTBreakUp.Columns[gvCTBreakUp.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (e.Row.RowIndex == 0)
            {
                if (db != null)
                    db.Visible = false;
            }

            if (ViewState["EDITROW"].ToString() == "1")
            {
                TextBox txtorigin = (TextBox)e.Row.FindControl("txtOriginIdBreakup");
                TextBox txtdestination = (TextBox)e.Row.FindControl("txtDestinationIdBreakup");
                ImageButton btnorigin = (ImageButton)e.Row.FindControl("btnShowOriginbreakup");
                ImageButton btndestination = (ImageButton)e.Row.FindControl("btnShowDestinationbreakup");
                TextBox txtoriginname = (TextBox)e.Row.FindControl("txtOriginNameBreakup");
                TextBox txtdestinationname = (TextBox)e.Row.FindControl("txtDestinationNameBreakup");

                UserControlDate txtDepartureDate = (UserControlDate)e.Row.FindControl("txtDepartureDate");
                UserControlDate txtArrivalDate = (UserControlDate)e.Row.FindControl("txtArrivalDate");
                UserControlTravelReason ucPurpose = (UserControlTravelReason)e.Row.FindControl("ucPurpose");

                TextBox txtoriginold = (TextBox)e.Row.FindControl("txtOriginIdOldBreakup");
                TextBox txtdestinationold = (TextBox)e.Row.FindControl("txtDestinationIdOldBreakup");
                TextBox txtoriginoldname = (TextBox)e.Row.FindControl("txtOriginNameOldBreakup");
                ImageButton btnoriginold = (ImageButton)e.Row.FindControl("btnShowOriginoldbreakup");
                ImageButton btndestinationold = (ImageButton)e.Row.FindControl("btnShowDestinationOldbreakup");

                UserControlDate txtDepartureDateOld = (UserControlDate)e.Row.FindControl("txtDepartureDateOld");
                UserControlDate txtArrivalDateOld = (UserControlDate)e.Row.FindControl("txtArrivalDateOld");
                UserControlTravelReason ucPurposeOld = (UserControlTravelReason)e.Row.FindControl("ucPurposeOld");

                //TextBox txtDepartureTime = (TextBox)e.Row.FindControl("txtBDepTimeAdd");
                //TextBox txtArrivalTime = (TextBox)e.Row.FindControl("txtBArrTimeAdd");

                DropDownList departureampm = ((DropDownList)e.Row.FindControl("ddldepartureampm"));
                DropDownList arrivalampm = ((DropDownList)e.Row.FindControl("ddlarrivalampm"));
                DropDownList departureampmold = ((DropDownList)e.Row.FindControl("ddldepartureampmold"));
                DropDownList arrivalampmold = ((DropDownList)e.Row.FindControl("ddlarrivalampmold"));

                ImageButton cmdRowSave = (ImageButton)e.Row.FindControl("cmdRowSave");
                ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");

                if (ucPurposeOld != null) ucPurposeOld.SelectedReason = drv["FLDPURPOSEID"].ToString();

                if (txtorigin != null)
                {
                    txtorigin.Visible = false;
                    txtArrivalDate.Visible = false;
                    txtDepartureDate.Visible = false;
                    txtdestination.Visible = false;
                    ucPurpose.Visible = false;
                    btnorigin.Visible = false;
                    btndestination.Visible = false;
                    txtoriginname.Visible = false;
                    txtdestinationname.Visible = false;
                    if (departureampm != null)
                        departureampm.Visible = false;
                    if (arrivalampm != null)
                        arrivalampm.Visible = false;

                    txtoriginoldname.CssClass = "input_mandatory";
                    txtoriginold.Visible = false;
                    btnoriginold.Visible = false;
                    cmdRowSave.Visible = true;
                    cmdSave.Visible = false;

                    if (drv != null)
                    {
                        txtArrivalDateOld.Text = drv["FLDARRIVALDATE"].ToString();
                        txtDepartureDateOld.Text = drv["FLDDEPARTUREDATE"].ToString();

                        if (departureampmold != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                            departureampmold.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
                        if (arrivalampmold != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                            arrivalampmold.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();
                    }

                }
            }
            if (ViewState["EDITROW"].ToString() == "0")
            {
                ImageButton btnoriginold = (ImageButton)e.Row.FindControl("btnShowOriginoldbreakup");
                ImageButton btnorigin = (ImageButton)e.Row.FindControl("btnShowOriginbreakup");
                ImageButton btndestination = (ImageButton)e.Row.FindControl("btnShowDestinationbreakup");
                ImageButton btndestinationold = (ImageButton)e.Row.FindControl("btnShowDestinationOldbreakup");
                UserControlDate txtDepartureDateOld = (UserControlDate)e.Row.FindControl("txtDepartureDateOld");
                UserControlDate txtArrivalDate = (UserControlDate)e.Row.FindControl("txtArrivalDate");

                if (btnoriginold != null)
                    btnoriginold.Visible = false;
                if (btndestination != null)
                    btndestination.Visible = false;
                if (btnorigin != null)
                    btnorigin.Visible = false;

                if (txtDepartureDateOld != null)
                    txtDepartureDateOld.Enabled = false;
                if (txtArrivalDate != null)
                    txtArrivalDate.Enabled = false;

                DropDownList departureampmold = ((DropDownList)e.Row.FindControl("ddldepartureampmold"));
                DropDownList arrivalampm = ((DropDownList)e.Row.FindControl("ddlarrivalampm"));
                UserControlTravelReason ucPurpose = (UserControlTravelReason)e.Row.FindControl("ucPurpose");

                if (drv != null)
                {
                    if (departureampmold != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        departureampmold.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
                    if (arrivalampm != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                        arrivalampm.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();
                    if (ucPurpose != null) ucPurpose.SelectedReason = drv["FLDPURPOSEID"].ToString();

                }
                if (departureampmold != null)
                    departureampmold.Enabled = false;

                if (arrivalampm != null)
                    arrivalampm.Enabled = false;

                TextBox txtdestinationoldname = (TextBox)e.Row.FindControl("txtDestinationNameOldBreakup");
                TextBox txtdestinationoldid = (TextBox)e.Row.FindControl("txtDestinationIdOldBreakup");

                if (txtdestinationoldname != null)
                    txtdestinationoldname.Text = "";
                if (txtdestinationoldid != null)
                    txtdestinationoldid.Text = "";
            }

            ImageButton dbbreakup = (ImageButton)e.Row.FindControl("cmdTravelBreakUp");
            ImageButton dbEdit = (ImageButton)e.Row.FindControl("cmdEdit");

            if (ViewState["EDITTRAVELREQUEST"] != null && !ViewState["EDITTRAVELREQUEST"].ToString().Equals("1"))
            {
                if (db != null) db.Visible = false;
                if (dbEdit != null) dbEdit.Visible = false;
                if (dbbreakup != null) dbbreakup.Visible = false;
            }
            else
            {
                //if (db != null) db.Visible = true;
                if (dbEdit != null) dbEdit.Visible = true;
                if (dbbreakup != null) dbbreakup.Visible = true;
            }


        }
    }

    protected void gvCTBreakUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "DELETE")
            {
                string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpId")).Text;

                //Request.QueryString["employeelist"] = Request.QueryString["employeelist"].ToString().Replace("," + employeeid, "");
                //Request.QueryString["crewplanidlist"] = Request.QueryString["crewplanidlist"].ToString().Replace("," + crewplanid, "");
                //Request.QueryString["strOnSignerYN"] = "";

                if (General.GetNullableGuid(breakupid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelBreakUp(new Guid(breakupid));

                    BindDataTravelBreakUp();
                }
            }

            if (e.CommandName.ToUpper() == "SAVE")
            {
                string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpIdEdit")).Text;
                string purposeid = ((UserControlTravelReason)_gridView.Rows[nCurrentRow].FindControl("ucPurposeOld")).SelectedReason;
                string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text;
                string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text;
                string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdOldBreakup")).Text;
                string departureampm = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlarrivalampmold")).SelectedValue;

                string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelBreakUp(strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, departuredate, arrivaldate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.UpdateTravelBreakUp(
                    new Guid(breakupid),
                    General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                   General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                   General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm));

                _gridView.EditIndex = -1;
                BindDataTravelBreakUp();
            }

            if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";
                gvCTBreakUp.EditIndex = nCurrentRow;
                //gvCTBreakUp.SelectedIndex = nCurrentRow;
                BindDataTravelBreakUp();
                //gvCTBreakUp_RowDataBound(sender, new GridViewRowEventArgs(gvCCT.Rows[nCurrentRow]));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        ViewState["CURRENTROW"] = de.NewEditIndex;
        ViewState["EDITROW"] = "0";

        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindDataTravelBreakUp();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeIdEdit")).Text;
            string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselIdEdit")).Text;
            string travelrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestIdEdit")).Text;
            string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpIdEdit")).Text;
            string onsigneryn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOnSignerYNEdit")).Text;

            string purposeid = ((UserControlTravelReason)_gridView.Rows[nCurrentRow].FindControl("ucPurpose")).SelectedReason;
            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDate")).Text; //add
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDate")).Text; //add
            string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdBreakup")).Text;
            string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdBreakup")).Text;

            string oldpurposeid = ((UserControlTravelReason)_gridView.Rows[nCurrentRow].FindControl("ucPurposeOld")).SelectedReason;
            string olddeparturedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text;
            string oldarrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text;
            string oldorigin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdOldBreakup")).Text;
            string olddestination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdOldBreakup")).Text;

            string departureampmold = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddldepartureampmold")).SelectedValue;
            string arrivalampmold = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlarrivalampmold")).SelectedValue;
            string departureampm = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddldepartureampm")).SelectedValue;
            string arrivalampm = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlarrivalampm")).SelectedValue;

            string strdeparturedatetimeeditold = olddeparturedate + " " + "00:00" + (departureampmold == "1" ? "AM" : "PM");
            string strarrivaldatetimeeditold = oldarrivaldate + " " + "00:00" + (arrivalampmold == "1" ? "AM" : "PM");
            string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
            string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

            if (!IsValidTravelBreakUp(strdeparturedatetimeeditold, strarrivaldatetimeeditold, oldorigin, olddestination, oldpurposeid
                 , strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, purposeid, olddeparturedate, oldarrivaldate, departuredate, arrivaldate))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelRequest.InsertTravelBreakUp(
                new Guid(breakupid), new Guid(travelrequestid), General.GetNullableInteger(employeeid), int.Parse(vesselid),
                int.Parse(onsigneryn), int.Parse(oldpurposeid), DateTime.Parse(olddeparturedate),
                DateTime.Parse(oldarrivaldate), General.GetNullableInteger(oldorigin), General.GetNullableInteger(olddestination),
                int.Parse(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm),
                General.GetNullableInteger(departureampmold), General.GetNullableInteger(arrivalampmold));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindDataTravelBreakUp();
    }

    private bool IsValidTravelBreakUp(string olddeparturedate, string oldarrivaldate, string oldorigin, string olddestination, string oldpurpose
         , string departuredate, string arrivaldate, string origin, string destination, string purpose, string olddepdateoly, string oldarrdateoly, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;


        if (oldorigin.Trim() == "" || origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (olddestination.Trim() == "" || destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";
        else if (olddestination.Trim().ToString().Equals(oldorigin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in Ist sector.";
        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in 2nd sector.";


        if (General.GetNullableDateTime(olddepdateoly) == null || General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure Date is required.";
        if (General.GetNullableDateTime(oldarrdateoly) == null || General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival Date is required";
        else if (DateTime.TryParse(olddeparturedate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(oldarrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date in Ist sector ";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date in 2nd sector";

        else if (DateTime.TryParse(oldarrivaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(departuredate)) > 0)
            ucError.ErrorMessage = "Departure date of 2nd sector should be later than the Arrival date of the Ist sector.";


        if (General.GetNullableInteger(purpose) == null || General.GetNullableInteger(oldpurpose) == null)
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
            ucError.ErrorMessage = "Arrival date should be later than departure date";


        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        if (ViewState["CURRENTROW"] != null)
        {
            int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString());
            TextBox txtoriginname = (TextBox)gvCTBreakUp.Rows[ncurrentrow].FindControl("txtOriginNameBreakup");
            TextBox txtoriginid = (TextBox)gvCTBreakUp.Rows[ncurrentrow].FindControl("txtOriginIdBreakup");
            if (txtoriginid != null && txtoriginname != null)
            {
                txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
                txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
            }
        }

    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
