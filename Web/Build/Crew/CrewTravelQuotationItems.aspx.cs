using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewTravelQuotationItems : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {
                if (Request.QueryString["SESSIONID"] != null)
                {
                    bindTravals();
                }
                if (Request.QueryString["QUOTEID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                }
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                }
               
                if (Request.QueryString["TRAVELREQUESTID"] != null)
                {
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["TRAVELREQUESTID"].ToString();
                }
                else
                {
                    ViewState["TRAVELREQUESTID"] = "";
                }
                if (Request.QueryString["AGENTID"] != null)
                {
                    ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
                }               
                if (Request.QueryString["PORT"] != null)
                    ViewState["PORT"] = Request.QueryString["PORT"];
                if(Request.QueryString["DATE"]!=null)
                    ViewState["DATE"] = General.GetDateTimeToString(Request.QueryString["DATE"]);
                if (Request.QueryString["VESSEL"] != null)
                    ViewState["VESSEL"] = Request.QueryString["VESSEL"];

                if (Request.QueryString["MODE"] != null && Request.QueryString["MODE"].ToString() == "EDIT")
                {
                    gvAgent.ShowFooter = true;
                }
                else
                    gvAgent.ShowFooter = false;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["QUOTEPAGENUMBER"] = 1;
                ViewState["QUOTESORTEXPRESSION"] = null;
                ViewState["QUOTESORTDIRECTION"] = null;

                ViewState["CURRENTINDEX"] = 1;

                ViewState["EDITROW"] = "0";
                ViewState["MODE"] = "New";

            }
           
            if (ViewState["QUOTEID"] == null)
                BindPassengerDetails();
            else
                BindRoutingDetails();

            BindQuotationDetails();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Previous", "PREVIOUS");
            MenuQuotationLineItem.AccessRights = this.ViewState;
            MenuQuotationLineItem.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void bindTravals()
    {
      DataSet ds=  PhoenixCrewTravelQuote.CrewQuotationDetails( new Guid( Request.QueryString["SESSIONID"].ToString()));
      if (ds.Tables[0].Rows.Count > 0)
      {
          DataRow dr = ds.Tables[0].Rows[0];
          ViewState["TRAVELID"] = dr["FLDTRAVELID"].ToString();
          ViewState["AGENTID"] = dr["FLDAGENTID"].ToString();
          ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
          ViewState["DATE"] = dr["FLDDATEOFCREWCHANGE"].ToString();
          ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
      }
    }
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
         DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
         if (dce.CommandName.Equals("PREVIOUS"))
         {
             Response.Redirect("../CREW/CrewTravelQuoteList.aspx?TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
                 + "&date=" + ViewState["DATE"].ToString()
                 + "&vessel=" + ViewState["VESSEL"].ToString() );
         }
    }
    private void BindPassengerDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelPassengerSearchForAgent(new Guid(ViewState["TRAVELID"].ToString()), null, null, int.Parse(ViewState["PAGENUMBER"].ToString()), 10, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds;
            gvLineItem.DataBind();
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = false;
            gvLineItem.Columns[8].Visible = false;
            gvLineItem.Columns[9].Visible = false;
            ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLineItem);
        }

        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    private bool IsQuoteFinalized(string quoteid)
    {
        if (quoteid != null)
        {
            DataTable dt = PhoenixCrewTravelQuote.IsQuoteFinalized(new Guid(quoteid));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["FLDFINALIZEDYN"].ToString() == string.Empty)
                    return false;
                else
                    return true;
            }
        }
        return false;
    }
    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCommoneProcessing.GetSessionStatus(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["WEBSESSIONSTATUS"] = dt.Rows[0]["FLDACTIVE"].ToString();
        }
    }
  
    
   
    private void BindQuotationDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDAMOUNT" };
        string[] alCaptions = { "Agent Name", "Amount" };
        string travelid = null;
        int agentid = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["QUOTESORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["QUOTESORTDIRECTION"].ToString());
        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        if (ViewState["QUOTEID"] != null)
        {
            DataSet ds = PhoenixCrewTravelQuote.EditCrewTravelQuotation(new Guid(ViewState["QUOTEID"].ToString()));
            General.SetPrintOptions("gvAgent", "Agent Details", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAgent.DataSource = ds;
                gvAgent.DataBind();
                if (ViewState["TRAVELREQUESTID"] == null)
                    ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                if (ViewState["QUOTEID"] == null)
                {
                    ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                    gvAgent.SelectedIndex = 0;
                }
                if (ViewState["MODE"].ToString() != "New")
                    ViewState["MODE"] = "Edit";

                gvAgent.ShowFooter = false;
                gvAgent.Columns[6].Visible = false;
                BindRoutingDetails();
                ViewState["QUOTEROWCOUNT"] = iRowCount;
                ViewState["QUOTETOTALPAGECOUNT"] = iTotalPageCount;
                SetPageNavigatorForQuotation();

            }
        }
        else
        {
            DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearchForAgent(new Guid(travelid),new Guid( ViewState["SESSIONID"].ToString()), sortexpression, sortdirection, (int)ViewState["QUOTEPAGENUMBER"], 10, ref iRowCount, ref iTotalPageCount);
            DataTable dt = ds.Tables[0];
            gvAgent.ShowFooter = true;
            ShowNoQuoteFound(dt, gvAgent);

        }
       
    }
    private void ShowNoQuoteFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Cells[0].Text = "NO QUOTATION FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void gvAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!(IsValidQuotation(
                ((TextBox)_gridView.FooterRow.FindControl("txtQuotationNo")).Text.ToString().Trim(),
                 ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrency")).SelectedCurrency)))
            {
                ucError.Visible = true;
                return;
            }
            InsertQuotation(((TextBox)_gridView.FooterRow.FindControl("txtQuotationNo")).Text.ToString().Trim(),
                            int.Parse(((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrency")).SelectedCurrency));
            gvAgent.ShowFooter = false;
            BindQuotationDetails();
                            

        }
        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["QUOTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationID")).Text;
            ViewState["TRAVELID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAgentID")).Text;

            BindRoutingDetails();
            
        }
    }
    
    private bool IsValidQuotation(string quotationno,string currency)
    {
         ucError.HeaderMessage = "Please provide the following required information";
         if(quotationno.Equals(string.Empty))
            ucError.ErrorMessage = "Quotation Number is required.";
         Int16 resultint;
         ucError.HeaderMessage = "Please provide the following required information";
         if (currency.Equals("Dummy")) currency = string.Empty;

         if (currency.Equals("") || !Int16.TryParse(currency, out resultint))
             ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }
   
  
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
     
    private void InsertQuotation(string quotationno,int currency)
    {
        Guid? Quoteid = new Guid();
        PhoenixCrewTravelQuote.InsertCrewTravelQuotationByAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["TRAVELREQUESTID"].ToString()), Convert.ToInt32(ViewState["AGENTID"].ToString())
            , quotationno, "Request", currency,new Guid(Request.QueryString["SESSIONID"].ToString()),13,  ref Quoteid);
        ViewState["QUOTEID"] = Quoteid;
        BindQuotationDetails();
    }
   
   
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvLineItem.EditIndex = -1;
        gvLineItem.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        
      
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvLineItem.SelectedIndex = -1;
        gvLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    
    protected void cmdQuoteGo_Click(object sender, EventArgs e)
    {
        gvLineItem.EditIndex = -1;
        gvLineItem.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtQuotenopage.Text, out result))
        {
            ViewState["QUOTEPAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["QUOTETOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["QUOTEPAGENUMBER"] = ViewState["QUOTETOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["QUOTEPAGENUMBER"] = 1;

            if ((int)ViewState["QUOTEPAGENUMBER"] == 0)
                ViewState["QUOTEPAGENUMBER"] = 1;

            txtQuotenopage.Text = ViewState["QUOTEPAGENUMBER"].ToString();
        }


    }
    protected void QuotePagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvAgent.SelectedIndex = -1;
        gvAgent.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["QUOTEPAGENUMBER"] = (int)ViewState["QUOTEPAGENUMBER"] - 1;
        else
            ViewState["QUOTEPAGENUMBER"] = (int)ViewState["QUOTEPAGENUMBER"] + 1;


        SetPageNavigatorForQuotation();
    }

    private void SetPageNavigatorForQuotation()
    {
        cmdQuotePrevious.Enabled = IsPreviousEnabledForQuotation();
        cmdQuoteNext.Enabled = IsNextEnabledForQuotation();
        lblQuotePagenumber.Text = "Page " + ViewState["QUOTEPAGENUMBER"].ToString();
        lblQuotePages.Text = " of " + ViewState["QUOTETOTALPAGECOUNT"].ToString() + " Pages. ";
        lblQuoteRecords.Text = "(" + ViewState["QUOTEROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledForQuotation()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["QUOTEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["QUOTETOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabledForQuotation()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["QUOTEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["QUOTETOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void gvAgent_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                LinkButton lnkRoutingDetails = (LinkButton)e.Row.FindControl("lnkRouteDetails");
                Label lblQuoteID = (Label)e.Row.FindControl("lblQuotationID");
                if (lnkRoutingDetails != null)
                {
                    if (ViewState["QUOTEID"] != null && ViewState["QUOTEID"].ToString() != string.Empty)
                        lnkRoutingDetails.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Crew/CrewTravelQuotationRoutingDetails.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&QUOTEID=" + ViewState["QUOTEID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&TRAVELREQUESTID=" + ViewState["TRAVELREQUESTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
                        + "&date=" + ViewState["DATE"].ToString()
                        + "&vessel=" + ViewState["VESSEL"].ToString()
                        + "', true);");
                }
                ImageButton btn = (ImageButton)e.Row.FindControl("cmdConfirm");
                if (btn != null) btn.Visible = SessionUtil.CanAccess(this.ViewState, btn.CommandName);
                if (lblQuoteID != null && lblQuoteID.Text!="" & btn != null)
                {
                    if (IsQuoteFinalized(lblQuoteID.Text))
                    {
                        btn.Visible = false;

                    }
                    else
                        btn.Visible = true;
                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindQuotationDetails();
        BindRoutingDetails();
    }
    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        if (ViewState["QUOTEID"] != null)
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearchForAgent(new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()),int.Parse(ViewState["PAGENUMBER"].ToString()),10,ref iRowCount,ref iTotalPageCount);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds;
                gvLineItem.DataBind();

                ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvLineItem);
            }
        
        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {

            if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";
                gvLineItem.EditIndex = nCurrentRow;
                BindRoutingDetails();

            }
            if (e.CommandName.ToUpper() == "SAVE")
            {
                string quoteid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationIDEdit")).Text;
                string travelid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelIDEdit")).Text;
                string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestIDEdit")).Text;
                string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakupIDEdit")).Text;
                string routeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRouteIDEdit")).Text;

           
                string origin = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtOrigin")).Text;
                string destination = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtDestination")).Text;
                string departuredatetime = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartureDate")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTime")).Text;
                string arrivaldatetime = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblArrivalDate")).Text + " " + ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTime")).Text;
                string noofstops = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNoOfStops")).Text;
                string duration = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDuration")).Text;
                string amount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
                string tax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTax")).Text;
                string routing = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRouting")).Text;
                if (!IsValidTravelBreakUp(noofstops, duration, amount, tax,routing))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotationLineItem(travelid, quoteid, routeid, noofstops, departuredatetime,arrivaldatetime,duration, amount, tax);
                _gridView.EditIndex = -1;
                BindRoutingDetails();
            }
            else if (e.CommandName.ToUpper() == "INSTRUCTION")
            {
                String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1', '', '../Common/CommonDiscussion.aspx?travelinstruction=true');");

                string lblDTKey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;

                PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
                objdiscussion.dtkey = new Guid(lblDTKey);
                objdiscussion.userid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                objdiscussion.title = PhoenixCrewConstants.TRAVELINSTRUCTIONS;
                objdiscussion.type = PhoenixCrewConstants.TRAVELREQUESTINSTRUCTION;
                PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
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

            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDate")).Text;
            string departuretime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTime")).Text;
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDate")).Text;
             string arrivaltime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTime")).Text;
            string origin = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtOrigin")).Text;
            string destination = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtDestination")).Text;
            string airlinecode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCode")).Text;
            string routeclass = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClass")).Text;
            string amount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
            string tax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTax")).Text;

            string olddeparturedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text;
            string olddeparturetime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDepartureTimeOld")).Text;
            string oldarrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text;
            string oldarrivaltime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtArrivalTimeOld")).Text;
            string oldorigin = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtOriginOld")).Text;
            string olddestination = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtDestinationOld")).Text;
            string oldairlinecode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAirlineCodeOld")).Text;
            string oldrouteclass = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtClassOld")).Text;
            string oldamount = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtAmountOld")).Text;
            string oldtax = ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtTaxOld")).Text;
            if (!IsValidTravelBreakUp(olddeparturedate, oldarrivaldate, oldorigin, olddestination, olddeparturedate, oldarrivaldate, origin, destination))
            {
                ucError.Visible = true;
                return;
            }
           
            _gridView.EditIndex = -1;
            BindRoutingDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
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
    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
    private void UpdateQuotationLineItem(string travelid, string quouteid, string routeid, string noofstops,string departuredate,string arrivaldate, string duration, string amount,string tax)
    {
        PhoenixCrewTravelQuoteLine.UpdateQuotationLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(routeid), new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()),DateTime.Parse(departuredate),DateTime.Parse(arrivaldate), int.Parse(noofstops), decimal.Parse(duration), General.GetNullableDecimal(amount), General.GetNullableDecimal(tax));

    }
    private bool IsValidTravelBreakUp(string noofstops, string duration, string amount,string tax,string routing)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (noofstops.Trim().Equals(""))
            ucError.ErrorMessage = "Stops is required.";

        if (duration.Trim().Equals(""))
            ucError.ErrorMessage = "Druation is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (amount.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";
        
        return (!ucError.IsError);
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
    protected void gvLineItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            ImageButton lnkShowReasonEdit = (ImageButton)e.Row.FindControl("cmdShowReasonEdit");
            if (lnkShowReasonEdit != null) lnkShowReasonEdit.Visible = SessionUtil.CanAccess(this.ViewState, lnkShowReasonEdit.CommandName);

            TextBox txtRouting = (TextBox)e.Row.FindControl("txtRouting");
           
            Label lblRouteID = (Label)e.Row.FindControl("lblRouteID");
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

               
                Label lblQuoteID = (Label)e.Row.FindControl("lblQuotationID");
                ImageButton lnkShowReason = (ImageButton)e.Row.FindControl("cmdShowReason");
                if (lnkShowReason != null) lnkShowReason.Visible = SessionUtil.CanAccess(this.ViewState, lnkShowReason.CommandName);
              
                if (lblQuoteID != null && lblQuoteID.Text != "")
                {
                    if (IsQuoteFinalized(lblQuoteID.Text))
                    {
                        lnkShowReason.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=true&framename=filterandsearch&ROUTEID=" + lblRouteID.Text + "', true);");
                        if (gvAgent.FooterRow != null)
                            gvAgent.FooterRow.Visible = false;
                        gvLineItem.Columns[10].Visible = false;
                        

                    }
                    else
                    {
                        gvLineItem.Columns[9].Visible = true;
                        if(gvAgent.FooterRow!=null)
                        gvAgent.FooterRow.Visible = true;
                        lnkShowReason.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=true&framename=filterandsearch&ROUTEID=" + lblRouteID.Text + "', true);");
                        
                    }
                }

               
            }
            if (lnkShowReasonEdit != null)
            {
                lnkShowReasonEdit.Visible = SessionUtil.CanAccess(this.ViewState, lnkShowReasonEdit.CommandName);
                lnkShowReasonEdit.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&framename=filterandsearch&ROUTEID=" + lblRouteID.Text + "', true);");
                txtRouting.Attributes.Add("style", "visibility:hidden");
            }
        }
    }
    public static void MergeRows(GridView gridView)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                {
                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                           previousRow.Cells[i].RowSpan + 1;
                    previousRow.Cells[i].Visible = false;
                }
            }
        }
    }
    protected void gvLineItem_PreRender(object sender, EventArgs e)
    {
        //MergeRows(gvLineItem);
    }

}
