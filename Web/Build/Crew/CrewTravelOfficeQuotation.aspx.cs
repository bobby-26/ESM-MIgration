using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class CrewTravelOfficeQuotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Travel Request", "TRAVELREQUESTLIST");
            toolbarmain.AddButton("Quotation", "OFFICEQUOTATION");
            toolbarmain.AddButton("Ticket", "TICKET");
            MenuAgent.AccessRights = this.ViewState;
            MenuAgent.MenuList = toolbarmain.Show();
            MenuAgent.SelectedMenuIndex = 3;

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            Menuapprove.Title = "Travel Request (" + PhoenixCrewTravelRequest.RequestNo + ")";
            Menuapprove.AccessRights = this.ViewState;
            Menuapprove.MenuList = toolbarmain.Show();


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                btnconfirm.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PASSPAGENUMBER"] = 1;
                ViewState["PASSSORTEXPRESSION"] = null;
                ViewState["PASSSORTDIRECTION"] = null;

                if (Request.QueryString["TRAVELID"] != null)
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"];
                if (Request.QueryString["TRAVELREQUESTID"] != null)
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["TRAVELREQUESTID"];
                if (Request.QueryString["port"] != null)
                    ViewState["PORT"] = Request.QueryString["port"];
                if (Request.QueryString["date"] != null)
                    ViewState["DATE"] = Request.QueryString["date"];
                if (Request.QueryString["vessel"] != null)
                    ViewState["VESSEL"] = Request.QueryString["vessel"];
                if (Request.QueryString["travelrequestedit"] != null)
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();

                SetInformation();
                BindVesselAccount();
                SetTravel();
                gvAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelOfficeQuotation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAgent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewListAddressAgent.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "'); return false;", "Add Agent", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelOfficeQuotation.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelOfficeQuotation.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelOfficeQuotation.aspx", "Compare Quotations", "<i class=\"fab fa-quora\"></i>", "QTNCOMPARE");
            MenuAgentList.AccessRights = this.ViewState;
            MenuAgentList.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetTravel()
    {
        try
        {
            DataSet ds = null;
            if (ViewState["TRAVELID"] != null)
                ds = PhoenixCrewTravelRequest.EditTravel(new Guid(ViewState["TRAVELID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucport.SelectedValue = ds.Tables[0].Rows[0]["FLDSEAPORTID"].ToString();
                ucport.Text = ds.Tables[0].Rows[0]["FLDPORTNAME"].ToString();
                txtDateOfCrewChange.Text = ds.Tables[0].Rows[0]["FLDDATEOFCREWCHANGE"].ToString();
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                ViewState["Vesselid"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                BindVesselAccount();

                if (ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString() != null)
                {
                    ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();
                }
                else
                {
                    ddlAccountDetails.SelectedValue = "";
                }
                ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();

                AddDefaultAgent();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AddDefaultAgent()
    {
        try
        {
            if (ViewState["TRAVELID"] != null)
                PhoenixCrewTravelRequest.traveldefaultagentinsert(new Guid(ViewState["TRAVELID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void SetInformation()
    {
        DataSet ds = PhoenixCrewTravelRequest.EditTravel(new Guid(Request.QueryString["TRAVELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            DataRow dr = ds.Tables[0].Rows[0];
            string vsl = "";
            if (Filter.CurrentTraveltoVesselName != null)
                vsl = Filter.CurrentTraveltoVesselName.ToString();

            PhoenixCrewTravelRequest.RequestNo = dr["FLDREQUISITIONNO"].ToString();
            if (ViewState["DATE"] == null)
                ViewState["DATE"] = General.GetNullableDateTime(dr["FLDDATEOFCREWCHANGE"].ToString());
            if (ViewState["VESSEL"] == null)
                ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            if (ViewState["PORT"] == null)
                ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
            ViewState["REQUISITIONNO"] = " [" + dr["FLDREQUISITIONNO"].ToString() + " ] ";
        }
    }

    protected void MenuAgent_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            string travelid = null;

            if (ViewState["TRAVELID"] != null)
                travelid = ViewState["TRAVELID"].ToString();

            if (CommandName.ToUpper().Equals("TRAVELREQUEST"))
            {
                Response.Redirect("../Crew/CrewTravelRequest.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                Response.Redirect("../Crew/CrewTravelRequestGeneral.aspx?from=travel&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TICKET"))
            {
                Response.Redirect("../Crew/CrewTravelQuoteTicketList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString() + "&FROMLIST=fromlist", false);
            }
            if (CommandName.ToUpper().Equals("OFFICEQUOTATION"))
            {
                Response.Redirect("../Crew/CrewTravelOfficeQuotation.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELREQUESTLIST"))
            {
                Response.Redirect("../Crew/CrewTravelRequestList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Menuapprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTrvelRequest(ucport.SelectedValue, txtDateOfCrewChange.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewTravelRequest.UpdateTravel(new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ucport.SelectedValue), DateTime.Parse(txtDateOfCrewChange.Text), General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                    ucStatus.Text = "Information Updated";
                    SetTravel();
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTrvelRequest(string strPortId, string dateofcrewchange)
    {

        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(dateofcrewchange) == null)
            ucError.ErrorMessage = "Date of Crew Change is required.";

        else if (!DateTime.TryParse(dateofcrewchange.ToString(), out resultDate))
        {
            ucError.ErrorMessage = "Date of Crew Change is not Valid.";
        }
        if (General.GetNullableInteger(strPortId) == null)
            ucError.ErrorMessage = "Crew Change Port is required";

        return (!ucError.IsError);
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER","FLDFILENO", "FLDNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDPASSPORTNUMBER", "FLDOTHERVISADETAILS",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE","FLDISCANCELLEDYNSTATUS"};
            string[] alCaptions = { "S.No.","File No", "Name", "On/Off-Signer", "D.O.B.", "PP No.", "VISA Details", "Origin", "Destination", "Departure",
                                    "Arrival","Status"};

            string sortexpression = (ViewState["PASSSORTEXPRESSION"] == null) ? null : (ViewState["PASSSORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["PASSSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["PASSSORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.TravelRequestGeneralSearch(
                General.GetNullableGuid(ViewState["TRAVELID"] != null ? ViewState["TRAVELID"].ToString() : ""),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PASSPAGENUMBER"].ToString()),
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


            gvCCT.DataSource = ds.Tables[0];
            gvCCT.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PASSPAGENUMBER"] = ViewState["PASSPAGENUMBER"] != null ? ViewState["PASSPAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           if (e.CommandName.ToUpper() == "CANCELTRAVEL")
            {
                RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblRequestId.Text != "")
                {
                    PhoenixCrewTravelRequest.CancelTravelPassenger(new Guid(lblRequestId.Text));
                }
                            
                gvCCT.Rebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                int? employeeid = null;
                string requestid;
                string vesselid;

                requestid = gvCCT.MasterTableView.Items[0].GetDataKeyValue("FLDREQUESTID").ToString();

                if (requestid != null)
                {

                    vesselid = ViewState["Vesselid"].ToString();

                    string orginid = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakupAdd")).Text;
                    string destinationid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupAdd")).Text;
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
                       General.GetNullableDateTime(arrivaldate),
                       General.GetNullableInteger(arrivalampm), General.GetNullableInteger(purposeid),
                       General.GetNullableGuid(requestid),
                      General.GetNullableInteger(travelclass));

                    gvCCT.Rebind();

                }
                else
                {
                    ucError.ErrorMessage = "Please Select the Seafarer to create a breakup";
                    ucError.Visible = true;
                }
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PASSPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Purpose is required.";

        return (!ucError.IsError);
    }

    protected void gvCCT_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem edit = (GridEditableItem)e.Item;
        try
        {
            if (e.Item.OwnerTableView.Name == "gvBreakUp")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpId")).Text;

                if (General.GetNullableGuid(breakupid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelBreakUpGeneral(new Guid(breakupid));
                    e.Item.OwnerTableView.Rebind();
                }
            }
            else
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;

                if (General.GetNullableGuid(requestid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
                    BindData();
                    gvCCT.Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCT_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem edit = (GridEditableItem)e.Item;
        try
        {
            if (e.Item.OwnerTableView.Name == "gvBreakUp")
            {


                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpIdEdit")).Text;
                string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeOld")).SelectedReason;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;
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
                       General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), General.GetNullableDateTime(arrivaldate),
                      General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                      General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm),
                      General.GetNullableInteger(travelclass));


                e.Item.OwnerTableView.Rebind();
            }
            else
            {
                string rankid = string.Empty;
                string strarrivaldateedit = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string strdeparturedateedit = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddlampmdeparture")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlampmarrival")).SelectedValue;
                rankid = ((UserControlRank)e.Item.FindControl("ddlRankEdit")).SelectedRank.ToString();

                string strdeparturedatetimeedit = strdeparturedateedit + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = strarrivaldateedit + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelRequest(
                   ((LinkButton)e.Item.FindControl("lnkName")).Text,
                   ((RadLabel)e.Item.FindControl("lblDOB")).Text,
                   ((RadTextBox)e.Item.FindControl("txtoriginIdEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtDestinationIdedit")).Text,
                   strdeparturedatetimeedit,
                   strarrivaldatetimeedit,
                   ((UserControlHard)e.Item.FindControl("ucPaymentmode")).SelectedHard
                   , strdeparturedateedit, strarrivaldateedit
                   ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.UpdateTravelRequestGeneral(
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTravelRequestIdedit")).Text),
                       ((LinkButton)e.Item.FindControl("lnkName")).Text,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblonoffsignerid")).Text),
                       DateTime.Parse(((RadLabel)e.Item.FindControl("lblDOB")).Text),
                       ((RadLabel)e.Item.FindControl("lblppno")).Text,
                       ((RadLabel)e.Item.FindControl("lblothervisadet")).Text,
                        int.Parse(((RadTextBox)e.Item.FindControl("txtoriginIdEdit")).Text),
                        int.Parse(((RadTextBox)e.Item.FindControl("txtDestinationIdedit")).Text),
                        DateTime.Parse(((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text),
                        int.Parse(((UserControlHard)e.Item.FindControl("ucPaymentmode")).SelectedHard),
                        int.Parse(departureampm),
                        General.GetNullableInteger(arrivalampm),
                        General.GetNullableInteger(rankid)
                        );
                BindData();
                gvCCT.Rebind();
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

        if (General.GetNullableDateTime(arrdateoly) != null)
        {
            if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
                ucError.ErrorMessage = "Arrival date should be later than or equal departure date";
        }

        return (!ucError.IsError);
    }
    private bool IsValidTravelRequest(string name, string dob, string origin, string destination, string departuredate, string arrivaldate,
                                                   string paymentmode, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (string.IsNullOrEmpty(name.Trim()))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableDateTime(dob) == null)
            ucError.ErrorMessage = "Date of birth is required.";

        else if (DateTime.TryParse(dob, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "Date of birth should not be a future date ";

        if (string.IsNullOrEmpty(origin.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (General.GetNullableDateTime(arrdateoly) != null)
        {
            if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
                ucError.ErrorMessage = "Arrival date should be greater or equal to departure date";
        }

        if (paymentmode.ToString().ToUpper() == "DUMMY" || string.IsNullOrEmpty(paymentmode.Trim()))
            ucError.ErrorMessage = "Payment mode is required.";

        return (!ucError.IsError);
    }


    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                LinkButton dbEdit = (LinkButton)e.Item.FindControl("cmdEdit");

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancelTravel");
                if (cancel != null)
                {
                    cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                    cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this Request?')");
                }
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblTravelRequestId");

                RadLabel lblstatus = (RadLabel)e.Item.FindControl("lbltravelstatus");

                UserControlHard ucpayment = (UserControlHard)e.Item.FindControl("ucPaymentmode");
                DataRowView drvpayment = (DataRowView)e.Item.DataItem;
                if (ucpayment != null) ucpayment.SelectedHard = drvpayment["FLDPAYMENTMODE"].ToString();

                RadComboBox AMPMDEPARTURE = (RadComboBox)e.Item.FindControl("ddlampmdeparture");
                RadComboBox AMPMARRIVAL = (RadComboBox)e.Item.FindControl("ddlampmarrival");

                if (drvpayment != null)
                {
                    if (AMPMDEPARTURE != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        AMPMDEPARTURE.SelectedValue = drvpayment["FLDDEPARTUREAMPMID"].ToString();
                    if (AMPMARRIVAL != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        AMPMARRIVAL.SelectedValue = drvpayment["FLDARRIVALAMPMID"].ToString();
                }

                if (ViewState["EDITTRAVELREQUEST"] != null && !ViewState["EDITTRAVELREQUEST"].ToString().Equals("1"))
                {
                    if (db != null) db.Visible = false;
                }
                else
                {
                    if (db != null) db.Visible = true;
                    if (dbEdit != null) dbEdit.Visible = true;
                }

                if (lblstatus != null)
                {

                    if (lblstatus.Text == "CANCELLED")
                    {
                        dbEdit.Visible = false;
                        db.Visible = false;
                        cancel.Visible = false;
                    }
                }
                RadLabel lblRankIdEdit = (RadLabel)e.Item.FindControl("lblRankIdEdit");
                RadLabel lblOfficeTravel = (RadLabel)e.Item.FindControl("lblOfficeTravelYN");
                RadLabel lblFamilyTravel = (RadLabel)e.Item.FindControl("lblFamilyTravelYN");
                RadLabel lblRankName = (RadLabel)e.Item.FindControl("lblRankName");
                UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ddlRankEdit");

                if (lblOfficeTravel != null && lblOfficeTravel.Text.ToString() == "1")
                {
                    if (ucRank != null)
                    {
                        ucRank.Visible = false;
                        lblRankName.Visible = true;
                    }
                }
                else if (lblFamilyTravel != null && lblFamilyTravel.Text.ToString() == "1")
                {
                    if (ucRank != null)
                        ucRank.Visible = false;
                }
                else
                {
                    if (ucRank != null)
                        ucRank.SelectedRank = drvpayment["FLDRANKID"].ToString();
                }
            }
            if (e.Item.OwnerTableView.Name == "gvBreakUp")
            {
                if (e.Item is GridEditableItem)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;

                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                    UserControlTravelReason ucPurposeOld = (UserControlTravelReason)e.Item.FindControl("ucPurposeOld");
                    if (ucPurposeOld != null) ucPurposeOld.SelectedReason = drv["FLDPURPOSEID"].ToString();

                    UserControlHard ucClassOld = ((UserControlHard)e.Item.FindControl("ucClassOld"));
                    if (ucClassOld != null) ucClassOld.SelectedHard = drv["FLDTRAVELCLASS"].ToString();

                    RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
                    RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

                    if (departureampm != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        departureampm.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
                    if (arrivalampm != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                        arrivalampm.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAgent.CurrentPageIndex + 1;
        BindAgentData();
    }


    private void BindAgentData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };
        string travelid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();

        DataSet ds = PhoenixCrewTravelQuote.CrewTravelAgentSearch(General.GetNullableGuid(travelid)
                                                                , sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvAgent.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvAgent", "Travel Agents", alCaptions, alColumns, ds);

        gvAgent.DataSource = ds;
        gvAgent.VirtualItemCount = iRowCount;


    }

    protected void gvCCT_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "gvBreakUp":
                {
                    string requestid = "";

                    requestid = dataItem.GetDataKeyValue("FLDREQUESTID").ToString();

                    DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUpGeneral(
                    General.GetNullableGuid(requestid));

                    e.DetailTableView.DataSource = ds;
                    break;
                }

        }
    }

    protected void gvAgent_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {

        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;


        switch (e.DetailTableView.Name)
        {
            case "gvAgentQuote":
                {
                    int iRowCount = 0;
                    int iTotalPageCount = 0;

                    string travelid = null;
                    string travelagentid = null;

                    string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                    int? sortdirection = null;

                    if (ViewState["SORTDIRECTION"] != null)
                        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                    if (ViewState["TRAVELID"] != null)
                        travelid = ViewState["TRAVELID"].ToString();

                    travelagentid = dataItem.GetDataKeyValue("FLDTRAVELAGENTID").ToString();
                    ViewState["AGENTID"] = dataItem.GetDataKeyValue("FLDAGENTID").ToString();

                    DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(travelid), General.GetNullableGuid(travelagentid), sortexpression, sortdirection
                        , 1, 100, ref iRowCount, ref iTotalPageCount);

                    e.DetailTableView.DataSource = ds;
                    break;
                }

            case "gvAgentQuoteDetails":
                {
                    int iRowCount = 0;
                    int iTotalPageCount = 0;
                    string quoteid = "";

                    quoteid = dataItem.GetDataKeyValue("FLDQUOTEID").ToString();

                    DataSet ds = new DataSet();

                    ds = PhoenixCrewTravelQuote.CrewTravelQuotationPassengersList(
                        General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString())
                        , General.GetNullableGuid(quoteid)
                        , 1, 100, ref iRowCount, ref iTotalPageCount);

                    e.DetailTableView.DataSource = ds;
                    break;
                }

            case "gvLineItem":
                {
                    int iRowCount = 0;
                    int iTotalPageCount = 0;
                    string quoteid = "";

                    quoteid = dataItem.GetDataKeyValue("FLDQUOTEID").ToString();
                    
                    DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearch(new Guid(ViewState["TRAVELID"].ToString())
                        , General.GetNullableGuid(quoteid)
                        , int.Parse(ViewState["AGENTID"].ToString())
                        , 1
                        , 100, ref iRowCount, ref iTotalPageCount, 0);


                    e.DetailTableView.DataSource = ds;
                    break;
                    
                }

        }


    }


    protected void gvAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                try
                {
                    ViewState["QUOTATIONID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
                    ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;

                    PhoenixCrewTravelQuote.TicketBudgetValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblQuotationID")).Text));
                }
                catch (Exception exe)
                {
                    if (exe.Message.ToString().ToUpper().Equals("INSUFFICIENT BUDGET. CONTINUE?"))
                    {
                        RadWindowManager1.RadConfirm(exe.Message, "btnconfirm", 320, 150, null, "Confirm");
                        return;
                    }
                    else
                    {
                        ucError.ErrorMessage = exe.Message;
                        ucError.Visible = true;
                        return;
                    }
                }
                approve();
            }
            else if (e.CommandName.ToUpper().Equals("FINALIZE"))
            {

                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
                ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;

                DataTable dt = PhoenixCrewTravelQuote.OrderForSelectedAgentValidation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()), General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    PhoenixCrewTravelQuoteLine.CrewTravelBreakupInsertForTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));
                    PhoenixCrewTravelQuoteLine.CrewTravelUnallocatedVesselExpenseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));

                    string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=POORDER&travelid=" + ViewState["TRAVELID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString() + "&agentid=" + ViewState["AGENTID"].ToString() + "');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);

                }

                gvAgent.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SENDMAIL"))
            {

                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
                ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;

                DataTable dt = PhoenixCrewTravelQuote.OrderForSelectedAgentValidation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()), General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    string finalized = dt.Rows[0]["FLDFINALIZEDYN"].ToString();
                    if (finalized == "1")
                    {
                        string sScript = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=POORDER&travelid=" + ViewState["TRAVELID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString() + "&agentid=" + ViewState["AGENTID"].ToString() + "',false,700,500);";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Quote Not Finalized";
                        ucError.Visible = true;
                    }

                    gvAgent.Rebind();
                }
            }

            else if (e.CommandName.ToUpper().Equals("PASSENGERAPPROVE"))
            {
                RadLabel lblrequestid = (RadLabel)e.Item.FindControl("lblrequestid");
                RadLabel lblquoteid = (RadLabel)e.Item.FindControl("lblquoteid");

                PhoenixCrewTravelQuote.QuotePassengerapprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , new Guid(ViewState["TRAVELID"].ToString())
                                                , new Guid(lblquoteid.Text)
                                                , new Guid(lblrequestid.Text));

                //e.Item.OwnerTableView.Rebind();

                gvAgent.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("PASSENGERDEAPPROVE"))
            {
                RadLabel lblrequestid = (RadLabel)e.Item.FindControl("lblrequestid");
                RadLabel lblquoteid = (RadLabel)e.Item.FindControl("lblquoteid");

                PhoenixCrewTravelQuote.QuotePassengerDeapprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , new Guid(ViewState["TRAVELID"].ToString())
                                                , new Guid(lblquoteid.Text)
                                                , new Guid(lblrequestid.Text));

                //e.Item.OwnerTableView.Rebind();

                gvAgent.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("PASSENGERSENDMAIL"))
            {

                ViewState["QUOTATIONID"] = ((RadLabel)e.Item.FindControl("lblquoteid")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblagentid")).Text;

                ResendQuoteMail();
                e.Item.OwnerTableView.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewTravelQuote.DeleteQuoteAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblTravelID")).Text)
                    , int.Parse(((RadLabel)e.Item.FindControl("lblAGentID")).Text));

                gvAgent.Rebind();
            }

            if (e.CommandName == "Page")
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void gvAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "gvAgentQuoteDetails")
        {

            RadLabel lblapproved = (RadLabel)e.Item.FindControl("lblisapproved");
            RadLabel lblQuotationNumber = (RadLabel)e.Item.FindControl("lblQuotationNumber");
            RadLabel lblSelectedQuoteId = (RadLabel)e.Item.FindControl("lblSelectedQuoteId");
            RadLabel lblQuoteId = (RadLabel)e.Item.FindControl("lblQuoteId");

            LinkButton imgapprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton imgdeapprove = (LinkButton)e.Item.FindControl("cmdDeApprove");
            LinkButton imgmail = (LinkButton)e.Item.FindControl("cmdSemdMail");

            if (lblQuoteId.Text != null && General.GetNullableString(lblSelectedQuoteId.Text) != null)
            {
                if (lblQuoteId.Text != lblSelectedQuoteId.Text)
                {
                    imgapprove.Visible = false;
                    imgmail.Visible = false;
                    imgdeapprove.Visible = false;
                }
                else
                {
                    imgapprove.Visible = false;
                    imgmail.Visible = false;
                    imgdeapprove.Visible = true;
                }
            }
            else if (lblQuoteId.Text != null && General.GetNullableString(lblSelectedQuoteId.Text) == null)
            {
                imgapprove.Visible = true;
                imgmail.Visible = true;
                imgdeapprove.Visible = false;
            }
            RadLabel lblcancel = (RadLabel)e.Item.FindControl("lblcancellyn");
            if (lblcancel != null && lblcancel.Text.ToString().Equals("1"))
            {
                imgapprove.Visible = false;
                imgmail.Visible = false;
            }

        }
        if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "gvAgentQuote")
        {

            RadLabel lblQuotationID = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblTravelID = (RadLabel)e.Item.FindControl("lblTravelID");
            RadLabel lblAgentID = (RadLabel)e.Item.FindControl("lblAgentID");
            RadLabel lblQuotationNo = (RadLabel)e.Item.FindControl("lblQuotationNo");
            RadLabel lblapproved = (RadLabel)e.Item.FindControl("lblIsApproved");
            RadLabel lblfinalized = (RadLabel)e.Item.FindControl("lblIsFinalized");
            RadLabel lbltravelfinalizeyn = (RadLabel)e.Item.FindControl("lbltravelfinalizeyn");

            LinkButton imgappove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton imgfinalize = (LinkButton)e.Item.FindControl("cmdFinalize");
            //HtmlImage imgflag = (HtmlImage)e.Item.FindControl("imgflag");
            if (lblapproved != null && lblfinalized != null && imgappove != null && imgfinalize != null)
            {
                if (lblapproved.Text.Equals("1") && lblfinalized.Text.Equals("1"))
                {
                    //  imgflag.Visible = true;
                }
                else if (lblapproved.Text.Equals("1"))
                {
                    //imgflag.Visible = false;
                }
                else
                {
                    //imgflag.Visible = false;
                }
            }
            if (lblapproved != null && lblfinalized == null && imgappove != null)
            {
                //        imgappove.Visible = true;
            }
            if (lbltravelfinalizeyn != null && lbltravelfinalizeyn.Text.Equals("1"))
            {
                //         imgappove.Visible = false;
                //         imgfinalize.Visible = false;

            }

            LinkButton ibm = (LinkButton)e.Item.FindControl("cmdAttachmentMapping");
            if (ibm != null)
            {
                if (lblfinalized != null && lblfinalized.Text.Equals("1"))
                {
                    ibm.Visible = true;

                }

                    //RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");
                    //if (lblattachmentmappingyn != null)
                    //{
                    //    if (!lblattachmentmappingyn.Text.Equals("1"))
                    //    {
                    //        HtmlGenericControl html = new HtmlGenericControl();
                    //        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    //        ibm.Controls.Add(html);
                    //    }
                    //}
                    ibm.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelQuotationAttachmentView.aspx?TRAVELID=" + lblTravelID.Text + "&QUOTEID=" + lblQuotationID.Text + "')");
            }

        }
        if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "gvLineItem")
        {
            RadLabel lblQuoteID = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");
            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            lnkShowReason.Attributes.Add("onclick", "javascript:openNewWindow('Filter', 'Itinerary','" + Session["sitepath"] + "/Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&framename=ifMoreInfo&ROUTEID=" + lblRouteID.Text + "');return false;");

            LinkButton lnknostop = (LinkButton)e.Item.FindControl("lnknostop");
            if (lnknostop != null)
            {
                lnknostop.Attributes.Add("onclick", "javascript:openNewWindow('Filter', 'Itinerary','" + Session["sitepath"] + "/Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&&framename=ifMoreInfo&Requestforstop=1&ROUTEID=" + lblRouteID.Text + "');return false;");
            }
            RadLabel lblamount = (RadLabel)e.Item.FindControl("lblAmount");
            RadLabel lblarrivaldate = (RadLabel)e.Item.FindControl("lblArrivalDate");
            RadLabel lbldeparturedate = (RadLabel)e.Item.FindControl("lblDepartureDate");

            if (lblamount != null && string.IsNullOrEmpty(lblamount.Text))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (drv != null)
                {
                    lblarrivaldate.Text = drv["FLDARRIVALDATEONLY"].ToString();
                    lbldeparturedate.Text = drv["FLDDEPARTUREDATEONLY"].ToString();
                }
            }


        }
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }


            LinkButton dbc = (LinkButton)e.Item.FindControl("cmdCommunication");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblTravelID");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblAgentID");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTravelAgentId");
            LinkButton lbn = (LinkButton)e.Item.FindControl("lnkAgentName");
            if (dbc != null)
                dbc.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationChat.aspx?TRAVELID=" + lbl.Text + "&AGENTID=" + lb2.Text + "&TRAVELAGENTID=" + lb3.Text + "&AGENTNAMEOLY=" + lbn.Text.Replace('&', '~').ToString() + "&AGENTNAME=" + lbn.Text.Replace('&', '~').ToString() + ViewState["REQUISITIONNO"].ToString() + "&ISOFFICE=1" + "');return false;");

            LinkButton quote = (LinkButton)e.Item.FindControl("cmdNewQuote");
            if (quote != null)
                quote.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationOffice.aspx?OFFICE=Y&SESSIONID=" + lb3.Text + "');return false;");


        }


    }

    protected void MenuAgentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                SendForQuotation();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                // ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();

                gvAgent.Rebind();
            }
            if (CommandName.ToUpper().Equals("QTNCOMPARE"))
            {
                if (ViewState["TRAVELID"] != null)
                {
                    string selectedagents = ",";
                    foreach (GridDataItem gvr in gvAgent.Items)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                        {
                            selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                        }
                    }

                    if (selectedagents.Length > 1)
                    {
                        string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationCompare.aspx?AGENTS=" + selectedagents + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                    }

                    else
                    {
                        ucError.ErrorMessage = "There are no quotations to compare.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    // ifMoreInfo.Attributes["src"] = "CrewTravelQuotationCompare.aspx";

                    string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationCompare.aspx" + "');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SendForQuotation()
    {
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem gvr in gvAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                }
            }

            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixCrewTravelQuote.ListCrewTravelQuotationToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString()), selectedvendors, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=RFQ&travelid=" + ViewState["TRAVELID"].ToString() + "&selectedagent=" + selectedvendors + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
            else
            {
                ucConfirm.ErrorMessage = "Email already sent";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendReminderForQuotation()
    {
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem gvr in gvAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                }
            }

            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixCrewTravelQuote.ListQuotationToSendRemainder(General.GetNullableGuid(ViewState["TRAVELID"].ToString()), selectedvendors);
            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=RFQREMAINDER&travelid=" + ViewState["TRAVELID"].ToString() + "&selectedagent=" + selectedvendors + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
            else
            {
                ucConfirm.ErrorMessage = "No agent selected to send reminder";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvAgent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void btn_approve(object sender, EventArgs e)
    {
        approve();
    }

    private void approve()
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelQuote.ApproveTravel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString())
            , General.GetNullableGuid(ViewState["QUOTATIONID"].ToString())
            , General.GetNullableInteger(ViewState["AGENTID"].ToString())
            , General.GetNullableDateTime(DateTime.Now.ToString()), 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                DataRow dr = ds.Tables[0].Rows[0];
                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();
                PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                    dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                    null,
                    dr["FLDSUBJECT"].ToString() + "     " + PhoenixCrewTravelRequest.RequestNo.ToString() + " - " + vsl.ToString(),
                    emailbodytext,
                    true,
                    System.Net.Mail.MailPriority.Normal,
                    "",
                    null,
                    null);
            }

            ucConfirm.ErrorMessage = "Quote is approved";
            ucConfirm.Visible = true;
            //BindQuotationDetails();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string PrepareApprovalText(DataTable dt, int approved)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        if (approved == 1)
            sbemailbody.AppendLine("Purchase approval is cancelled.");
        else
            sbemailbody.AppendLine("Purchase order is approved");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDAPPROVEDBY"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }

    private void ResendQuoteMail()
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelQuote.ResendQuote(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , new Guid(ViewState["TRAVELID"].ToString())
                                        , Convert.ToInt32(ViewState["AGENTID"].ToString())
                                        );

            if (ds.Tables[0].Rows.Count > 0)
            {
                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                emailbodytext = PrepareEmailBodyText(ds.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString(), ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString()
                                                       , ds.Tables[0].Rows[0]["FLDNAME"].ToString(), ds.Tables[0].Rows[0]["FLDPAGETO"].ToString()
                                                       , ds.Tables[0].Rows[0]["FLDSENDBY"].ToString());

                PhoenixMail.SendMail(ds.Tables[0].Rows[0]["FLDAGENTEMAIL1"].ToString(), ds.Tables[0].Rows[0]["FLDEMAIL2"].ToString().TrimEnd(','), null, "TRFQ for " + ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " - " + vsl.ToString()
                                            , emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");

                ucConfirm.ErrorMessage = "Email sent to  " + ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private string PrepareEmailBodyText(string travelagentid, string formno, string agentname, string fldpageto, string sendername)
    {

        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append(agentname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to requote for the following passengers to travel.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated,");
        sbemailbody.AppendLine("<br/>");
        string url = Session["sitepath"] + "/Portal/PortalDefault.aspx";
        sbemailbody.AppendLine("<a href=" + url + " >" + url + "</a>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");

        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(sendername);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of " + HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();

    }





}