using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravelQuoteHopList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            string delimiter = ",";
            char[] chdelimiter = delimiter.ToCharArray();
            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
           
            ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
            ViewState["BREAKUPID"] = Request.QueryString["BREAKUPID"].ToString();
            if (Request.QueryString["TICKETNO"] != null)
            {
                ViewState["TICKETNO"] = Request.QueryString["TICKETNO"];
                Title1.Text = "Ticket Copy for Ticket number: " + ViewState["TICKETNO"].ToString();
            }
           
            
        }
        BindRoutingDetails();

    }
    protected void MenuRouting_TabStripCommand(object sender, EventArgs e)
    {
       
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
    private void UpdateQuotationLineItem(string routid, string quouteid, string airlinecode, string routeclass, string amount, string tax,string ticketnumber,string invoiceyn)
    {
        if (ViewState["TICKETNO"] != null)
        {
            PhoenixCrewTravelQuoteLine.UpdateSelectedRoutes(0, new Guid(routid), new Guid(ViewState["QUOTEID"].ToString()), ViewState["TICKETNO"].ToString(), airlinecode, routeclass, decimal.Parse(amount), decimal.Parse(tax),int.Parse(invoiceyn) );
        }
        else
        {
            PhoenixCrewTravelQuoteLine.UpdateSelectedRoutes(0, new Guid(routid), new Guid(ViewState["QUOTEID"].ToString()), ticketnumber, airlinecode, routeclass, decimal.Parse(amount), decimal.Parse(tax), int.Parse(invoiceyn));
        }
    }

    private void InsertQuotationLineItem(string origin,string destination,string departuretime, string arivaltime,string airlinecode, string routeclass, string amount, string tax, string ticketnumber)
    {

        PhoenixCrewTravelQuoteLine.InsertSelectedRoutes(0, new Guid(ViewState["QUOTEID"].ToString()), origin, destination,General.GetNullableDateTime(departuretime),General.GetNullableDateTime(arivaltime),    ticketnumber, airlinecode, routeclass, decimal.Parse(amount), decimal.Parse(tax));
     
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

    private bool IsValidTravelBreakUpInsert(string origin,string destination,string airlinecode, string routeclass, string amount, string tax)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (origin.Trim().Equals(""))
            ucError.ErrorMessage = "Origin is required.";

        if (destination.Trim().Equals(""))
            ucError.ErrorMessage = "Destination is required.";

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
   
   
    protected void gvRoutingDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            GridView _gridView = (GridView)sender;
            if (!IsValidTravelBreakUp(((TextBox)_gridView.FooterRow.FindControl("txtAirlineCodeAdd")).Text, ((TextBox)_gridView.FooterRow.FindControl("txtClassAdd")).Text,
                ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtAmountAdd")).Text,
                 ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtTaxAdd")).Text))
            {
                ucError.Visible = true;
                return;
            }
            InsertQuotationLineItem(
                ((TextBox)_gridView.FooterRow.FindControl("txtOrginAdd")).Text,
             ((TextBox)_gridView.FooterRow.FindControl("txtDestinationAdd")).Text,
             ((UserControlDate)_gridView.FooterRow.FindControl("txtDepartureDateAdd")).Text + " " + ((TextBox)_gridView.FooterRow.FindControl("txtDepartureTimeAdd")).Text,
             ((UserControlDate)_gridView.FooterRow.FindControl("txtArrivalDateAdd")).Text + " " + ((TextBox)_gridView.FooterRow.FindControl("txtDepartureTimeAdd")).Text,
                 ((TextBox)_gridView.FooterRow.FindControl("txtAirlineCodeAdd")).Text,
                ((TextBox)_gridView.FooterRow.FindControl("txtClassAdd")).Text,
                ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtAmountAdd")).Text,
                 ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtTaxAdd")).Text,
                 ((TextBox)_gridView.FooterRow.FindControl("txtTicketAdd")).Text
             );


        }
        

    }
    protected void gvRoutingDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            Label lb = (Label)e.Row.FindControl("lblInvoiceNumber");
            if (lb != null && ed != null)
            {
                if (lb.Text != "")
                    ed.Enabled = false; 
            }
                
             
        }
    }
    protected void gvRoutingDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindRoutingDetails();
       
        
    }
    protected void gvRoutingDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        if (!IsValidTravelBreakUp(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCode")).Text,((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClass")).Text,
            ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text,
             ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtTax")).Text))
        {
            ucError.Visible = true;
            return;
        }
        UpdateQuotationLineItem(
            ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRouteID")).Text,
             ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationID")).Text,
            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCode")).Text,
            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClass")).Text,
            ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text,
             ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtTax")).Text ,
             ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTicket")).Text,
             ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkIsInvoiced")).Checked?"1":"0"
         );



        _gridView.EditIndex = -1;
        BindRoutingDetails();
    }

    protected void gvRoutingDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindRoutingDetails();

    }
    
}
