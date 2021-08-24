using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravelQuoteIssueTicket : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //SessionUtil.PageAccessRights(this.ViewState);
            string delimiter = ",";
            char[] chdelimiter = delimiter.ToCharArray();
            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
            if (Request.QueryString["AGENTID"] != null)
                ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
            ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
            ViewState["BREAKUPID"] = Request.QueryString["BREAKUPID"].ToString();
            ViewState["TICKETSTATUS"] = Request.QueryString["TICKETSTATUS"].ToString(); ;
            ViewState["TICKETNO"]= Request.QueryString["TICKETNO"];
            if (ViewState["TICKETNO"] != null)
                Title1.Text = "Ticket Details: " + ViewState["TICKETNO"].ToString();
            else
                Title1.Text = "Ticket Details";

        }
        BindRoutingDetails();

    }
    protected void MenuRouting_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName == "PREVIOUS")
            {
                Response.Redirect("../Crew/CrewTravelQuoteTicket.aspx?QUOTEID=" + ViewState["QUOTEID"].ToString() + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString());
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void BindRoutingDetails()
    {
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationRouteSearch(new Guid(ViewState["QUOTEID"].ToString()), new Guid(ViewState["BREAKUPID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRoutingDetails.DataSource = ds;
            gvRoutingDetails.DataBind();

            ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);

    }
    protected void gvRoutingDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {

            if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";
                gvRoutingDetails.EditIndex = nCurrentRow;
                BindRoutingDetails();

            }
            if (e.CommandName.ToUpper() == "SAVE")
            {
                string quoteid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationIDEdit")).Text;
                string travelid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelIDEdit")).Text;
                string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestIDEdit")).Text;
                string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakupIDEdit")).Text;
                string routeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRouteIDEdit")).Text;

                string olddeparturedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTimeOld")).Text;
                string oldarrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTimeOld")).Text; ;
                string oldorigin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginOld")).Text;
                string olddestination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationOld")).Text;
                string oldairlinecode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCodeOld")).Text;
                string oldrouteclass = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClassOld")).Text;
                string oldamount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmountOld")).Text;
                string oldtax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTaxOld")).Text;

                if (!IsValidTravelBreakUp(oldairlinecode, oldrouteclass, oldamount, oldtax))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotationLineItem(quoteid, oldairlinecode, oldrouteclass, oldamount, oldtax, routeid);
                _gridView.EditIndex = -1;
                BindRoutingDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRoutingDetails_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["EDITROW"] != null)
        {
            if (ViewState["EDITROW"].ToString() == "1")
            {
                TextBox txtOrigin = (TextBox)e.Row.FindControl("txtOrigin");
                TextBox txtDestination = (TextBox)e.Row.FindControl("txtDestination");
                UserControlDate txtDepartureDate = (UserControlDate)e.Row.FindControl("txtDepartureDate");
                TextBox txtDepartureTime = (TextBox)e.Row.FindControl("txtDepartureTime");
                UserControlDate txtArrivalDate = (UserControlDate)e.Row.FindControl("txtArrivalDate");
                TextBox txtArrivalTime = (TextBox)e.Row.FindControl("txtArrivalTime");
                TextBox txtAirlineCode = (TextBox)e.Row.FindControl("txtAirlineCode");
                TextBox txtClass = (TextBox)e.Row.FindControl("txtClass");
                UserControlDecimal txtAmount = (UserControlDecimal)e.Row.FindControl("txtAmount");
                UserControlDecimal txtTax = (UserControlDecimal)e.Row.FindControl("txtTax");

                TextBox txtOriginOld = (TextBox)e.Row.FindControl("txtOriginOld");
                TextBox txtDestinationOld = (TextBox)e.Row.FindControl("txtDestinationOld");
                UserControlDate txtDepartureDateOld = (UserControlDate)e.Row.FindControl("txtDepartureDateOld");
                TextBox txtDepartureTimeOld = (TextBox)e.Row.FindControl("txtDepartureTimeOld");
                UserControlDate txtArrivalDateOld = (UserControlDate)e.Row.FindControl("txtArrivalDateOld");
                TextBox txtArrivalTimeOld = (TextBox)e.Row.FindControl("txtArrivalTimeOld");
                TextBox txtAirlineCodeOld = (TextBox)e.Row.FindControl("txtAirlineCodeOld");
                TextBox txtClassOld = (TextBox)e.Row.FindControl("txtClassOld");
                UserControlDecimal txtAmountOld = (UserControlDecimal)e.Row.FindControl("txtAmountOld");
                UserControlDecimal txtTaxOld = (UserControlDecimal)e.Row.FindControl("txtTaxOld");

                ImageButton cmdRowSave = (ImageButton)e.Row.FindControl("cmdRowSave");
                ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");

                if (txtOrigin != null)
                {
                    txtOrigin.Visible = false;
                    txtArrivalDate.Visible = false;
                    txtArrivalTime.Visible = false;
                    txtDepartureDate.Visible = false;
                    txtDepartureTime.Visible = false;
                    txtDestination.Visible = false;
                    txtAirlineCode.Visible = false;
                    txtClass.Visible = false;
                    txtAmount.Visible = false;
                    txtTax.Visible = false;
                    txtOriginOld.ReadOnly = true;
                    txtDestinationOld.ReadOnly = true;
                    txtDepartureDateOld.ReadOnly = true;
                    txtDepartureTimeOld.ReadOnly = true;
                    txtArrivalDateOld.ReadOnly = true;
                    txtArrivalTimeOld.ReadOnly = true;
                    cmdRowSave.Visible = true;
                    cmdSave.Visible = false;

                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    txtArrivalDateOld.Text = drv["FLDARRIVALDATE"].ToString();
                    txtDepartureDateOld.Text = drv["FLDDEPARTUREDATE"].ToString();
                    txtDestinationOld.Text = drv["FLDDESTINATION"].ToString();
                    txtOriginOld.Text = drv["FLDORIGIN"].ToString();
                }
            }
            else
            {
                TextBox txtDestinationOld = (TextBox)e.Row.FindControl("txtDestinationOld");
                TextBox txtDestination = (TextBox)e.Row.FindControl("txtDestination");
                UserControlDate txtDepartureDateOld = (UserControlDate)e.Row.FindControl("txtDepartureDateOld");
                TextBox txtDepartureTimeOld = (TextBox)e.Row.FindControl("txtDepartureTimeOld");
                UserControlDate txtArrivalDateOld = (UserControlDate)e.Row.FindControl("txtArrivalDateOld");
                TextBox txtArrivalTimeOld = (TextBox)e.Row.FindControl("txtArrivalTimeOld");
                TextBox txtAirlineCodeOld = (TextBox)e.Row.FindControl("txtAirlineCodeOld");
                if (txtDestinationOld != null)
                {
                    txtDestination.CssClass = "readonlytextbox";
                    txtDestinationOld.CssClass = "input_mandatory";
                    txtDepartureDateOld.CssClass = "input_mandatory";
                    txtDepartureTimeOld.CssClass = "input_mandatory";
                    txtArrivalDateOld.CssClass = "input_mandatory";
                    txtArrivalTimeOld.CssClass = "input_mandatory";
                    txtAirlineCodeOld.CssClass = "input_mandatory";
                }
            }

            Label lblTravelReqNo = ((Label)e.Row.FindControl("lblTravelReqNo"));

            ImageButton dbEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdTravelBreakUp = (ImageButton)e.Row.FindControl("cmdTravelBreakUp");

            if (dbEdit != null) dbEdit.Visible = SessionUtil.CanAccess(this.ViewState, dbEdit.CommandName);
            if (cmdTravelBreakUp != null) cmdTravelBreakUp.Visible = SessionUtil.CanAccess(this.ViewState, cmdTravelBreakUp.CommandName);


        }
        if (ViewState["TICKETSTATUS"].ToString() == "1")
        {
            gvRoutingDetails.Columns[8].Visible = false;
           
        }
        else
        {
            gvRoutingDetails.Columns[8].Visible = true;
           
        }
    }
    protected void gvRoutingDetails_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            ViewState["EDITROW"] = "0";
            BindRoutingDetails();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvAgentLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string quoteid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationIDEdit")).Text;
            string travelid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelIDEdit")).Text;
            string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestIDEdit")).Text;
            string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakupIDEdit")).Text;
            string routeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRouteIDEdit")).Text;

            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDate")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTime")).Text;
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDate")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTime")).Text;
            string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrigin")).Text;
            string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestination")).Text;
            string airlinecode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCode")).Text;
            string routeclass = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClass")).Text;
            string amount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
            string tax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTax")).Text;

            string olddeparturedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTimeOld")).Text;
            string oldarrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTimeOld")).Text;
            string oldorigin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginOld")).Text;
            string olddestination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationOld")).Text;
            string oldairlinecode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCodeOld")).Text;
            string oldrouteclass = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClassOld")).Text;
            string oldamount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmountOld")).Text;
            string oldtax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTaxOld")).Text;

            if (!IsValidTravelBreakUp(olddeparturedate, oldarrivaldate, oldorigin, olddestination, olddeparturedate, oldarrivaldate, origin, destination))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewTravelQuoteLine.CrewTravelQuoteLineItemSelectedRouteInsert(0,
                new Guid(quoteid), ViewState["TICKETNO"].ToString()
                , DateTime.Parse(olddeparturedate),
                DateTime.Parse(oldarrivaldate), oldorigin, olddestination,
                 oldairlinecode, oldrouteclass, Convert.ToDecimal(oldamount), Convert.ToDecimal(oldtax), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                origin, destination, airlinecode, routeclass, Convert.ToDecimal(amount), Convert.ToDecimal(tax),General.GetNullableGuid(routeid) );
            _gridView.EditIndex = -1;
            BindRoutingDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvAgentLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindRoutingDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidTravelBreakUp(string olddeparturedate, string oldarrivaldate, string oldorigin, string olddestination
        , string departuredate, string arrivaldate, string origin, string destination)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(oldarrivaldate) == null)
            ucError.ErrorMessage = "End Date is required";

        if (oldorigin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (General.GetNullableDateTime(olddeparturedate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        if (olddestination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "End Date is required";

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";


        return (!ucError.IsError);
    }
    private void UpdateQuotationLineItem(string quouteid, string airlinecode, string routeclass, string amount, string tax, string routeid)
    {
        PhoenixCrewTravelQuoteLine.UpdateSelectedRoute(0, new Guid(ViewState["QUOTEID"].ToString()),ViewState["TICKETNO"].ToString(), airlinecode, routeclass, decimal.Parse(amount), decimal.Parse(tax),new Guid(routeid));

    }
    private bool IsValidTravelBreakUp(string airlinecode, string routeclass, string amount, string tax)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (airlinecode.Trim().Equals(""))
            ucError.ErrorMessage = "Airline Code is required.";

        if (routeclass.Trim().Equals(""))
            ucError.ErrorMessage = "Route class is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (amount.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";
        return (!ucError.IsError);
    }
}
