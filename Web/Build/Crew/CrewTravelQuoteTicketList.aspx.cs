using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using Telerik.Web.UI;
using System.Web;

public partial class CrewTravelQuoteTicketList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["EDITROW"] = "0";
            ViewState["CURRENTROW"] = null;
            ViewState["ROUTEID"] = null;
            ViewState["ISREQUOTE"] = "0";
            ViewState["QUOTATIONNO"] = null;
            ViewState["CURRENCY"] = null;
            ViewState["TRAVELID"] = null;

            ViewState["FROMLIST"] = Request.QueryString["FROMLIST"];

            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"];



            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();

            if (Request.QueryString["port"] != null)
                ViewState["PORT"] = Request.QueryString["port"];
            if (Request.QueryString["date"] != null)
                ViewState["DATE"] = Request.QueryString["date"];
            if (Request.QueryString["vessel"] != null)
                ViewState["VESSEL"] = Request.QueryString["vessel"];
            if (Request.QueryString["travelrequestedit"] != null)
                ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();



            if (ViewState["TRAVELID"] != null)
            {
                DataSet dsTravel = PhoenixCrewTravelRequest.EditTravel(new Guid(ViewState["TRAVELID"].ToString()));
                if (dsTravel.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsTravel.Tables[0].Rows[0];
                    if (dr["FLDOFFICEREQUESTYN"].ToString() == "1" && dr["FLDREQUESTAPPROVE"].ToString() == "0")
                    {
                        Response.Redirect("../Crew/CrewTravelRequest.aspx?offficeapprovalreq=1", false);
                    }
                }
            }
            SetInformation();
            gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        if (ViewState["FROMLIST"] != null)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Travel Request", "TRAVELLIST");           
            toolbarmain.AddButton("Quotation", "OFFICEQUOTATION");
            toolbarmain.AddButton("Ticket", "TICKET");
            MenuTicket.AccessRights = this.ViewState;
            MenuTicket.MenuList = toolbarmain.Show();
            MenuTicket.SelectedMenuIndex = 3;
        }
        else
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Travel Request", "TRAVEL");
            toolbarmain.AddButton("Travel Plan", "TRAVELPLAN");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Ticket", "TICKET");
            MenuTicket.AccessRights = this.ViewState;
            MenuTicket.MenuList = toolbarmain.Show();
            MenuTicket.SelectedMenuIndex = 3;
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelQuoteTicketList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('TicketQuotes','','" + Session["sitepath"] + "/Crew/CrewTravelTicketCopySummary.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "');return false;", "Ticket Quotes", "<i class=\"fas fa-tag\"></i>", "TICKET");
        toolbargrid.AddFontAwesomeButton("javascript:showDialog();", "Notes", "<i class=\"fas fa-info-circle\"></i>", "NOTES");
        MenuQuotationList.AccessRights = this.ViewState;
        MenuQuotationList.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbarhoplistgrid = new PhoenixToolbar();
        string querystring = "TRAVELID=" + ViewState["TRAVELID"].ToString();
        toolbarhoplistgrid.AddFontAwesomeButton("../Crew/CrewTravelQuoteTicketList.aspx?" + querystring, "Initiate New Request", "<i class=\"fas fa-plane-departure-it\"></i>", "CreateNewRequest");
        toolbarhoplistgrid.AddFontAwesomeButton("../Crew/CrewTravelQuoteTicketList.aspx?" + querystring, "Attachment", "<i class=\"fas fa-paperclip-na\"></i>", "ATTACHMENTMAPPING");
        MenuHopList.AccessRights = this.ViewState;
        MenuHopList.MenuList = toolbarhoplistgrid.Show();
        BindRoutingDetails();
        BindDataTravelBreakUp();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.Title = lblTitle.Text;
        MenuTitle.MenuList = toolbar.Show();
    }
    protected void MenuQuotationList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDNOOFSTOPS", "FLDDURATION", "FLDCURRENTAMOUNT", "FLDCURRENTTAX" };
        string[] alCaptions = { "Name", "Origin", "Destination", "Departure", "Arrival", "Stops", "Duration (Hrs)", "Fare", "Tax" };

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationTicketNumberSearch(new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Passengers_List.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Passengers List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
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
            lblTitle.Text = lblTitle.Text + " (" + dr["FLDREQUISITIONNO"].ToString() + " )";
            if (ViewState["DATE"] == null)
                ViewState["DATE"] = General.GetNullableDateTime(dr["FLDDATEOFCREWCHANGE"].ToString());
            if (ViewState["VESSEL"] == null)
                ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            if (ViewState["PORT"] == null)
                ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
        }
    }
    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDNOOFSTOPS", "FLDDURATION", "FLDCURRENTAMOUNT", "FLDCURRENTTAX" };
        string[] alCaptions = { "Name", "Origin", "Destination", "Departure", "Arrival", "Stops", "Duration (Hrs)", "Fare", "Tax" };

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationTicketNumberSearch(new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds.Tables[0];
            gvLineItem.VirtualItemCount = iRowCount;
            lblTitle.Text = "";
            lblTitle.Text = "Ticket (" + ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " & Agent : " + ds.Tables[0].Rows[0]["FLDAGENTNAME"].ToString() + " )";
            if (ViewState["ROUTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
            }
            ViewState["QUOTATIONNO"] = ds.Tables[0].Rows[0]["FLDQUOTATIONREFNO"].ToString();
            ViewState["CURRENCY"] = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
            ViewState["TRAVELDTKEY"] = ds.Tables[0].Rows[0]["FLDTRAVELDTKEY"].ToString();
            txtQuotationReference.Text = ds.Tables[0].Rows[0]["FLDQUOTATIONREFNO"].ToString();
            txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();

            SetRowSelection();
        }
        else
        {
            gvLineItem.DataSource = "";
        }
        General.SetPrintOptions("gvLineItem", "Passengers List", alCaptions, alColumns, ds);
    }
    private void SetRowSelection()
    {
        try
        {
            for (int i = 0; i < gvLineItem.MasterTableView.Items.Count; i++)
            {
                if (gvLineItem.MasterTableView.Items[i].GetDataKeyValue("FLDROUTEID").ToString().Equals(ViewState["ROUTEID"].ToString()))
                {
                    gvLineItem.MasterTableView.Items[i].Selected = true;

                }
            }
        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }

    private int IsTicketForQuoteConfirmed()
    {
        int status = 0;
        if (ViewState["QUOTEID"] != null)
        {
            DataSet ds = PhoenixCrewTravelQuote.EditCrewTravelQuotation(new Guid(ViewState["QUOTEID"].ToString()));
            status = int.Parse(ds.Tables[0].Rows[0]["FLDTICKETCONFIRMEDYN"].ToString());
        }
        return status;
    }
    protected void MenuTicket_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {

            if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("../Crew/CrewTravelRequest.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                Response.Redirect("../Crew/CrewTravelRequestGeneral.aspx?from=travel&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELLIST"))
            {
                Response.Redirect("../Crew/CrewTravelRequestList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("OFFICEQUOTATION"))
            {
                Response.Redirect("../Crew/CrewTravelOfficeQuotation.aspx?FROMLIST=travellist&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTicketNo(string amount, string tax)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindRoutingDetails();
        gvLineItem.Rebind();
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }

    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelhopLineItemSearch(
                new Guid(ViewState["TRAVELID"].ToString()), General.GetNullableGuid(ViewState["ROUTEID"] == null ? null : ViewState["ROUTEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;
                gvCTBreakUp.DataBind();
                CheckSelectedAllHopList();
            }
            else
            {
                gvCTBreakUp.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetQuotationInformation()
    {
        txtQuotationReference.Text = ViewState["QUOTATIONNO"].ToString();
        txtCurrency.Text = ViewState["CURRENCY"].ToString();
    }

    private void SendMail(string requestid, string hopid, string type)
    {
        StringBuilder sbemailbody = new StringBuilder();

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelHopItemDetailList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(requestid)
            , General.GetNullableGuid(hopid)
            );
        DataRow dr = ds.Tables[0].Rows[0];

        if (ds.Tables[0].Rows.Count > 0)
        {
            sbemailbody.Append("Dear Sir/Madam");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            string mailsubject = string.Empty;
            mailsubject = dr["FLDAGENTNAME"].ToString() + " - " + dr["FLDREQUISITIONNO"].ToString();

            if (type == "tktcancel")
                sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to cancel the below passengers ticket ");
            if (type == "tktreissue")
                sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to cancel/reissue the below passengers ticket");
            if (type == "tktRequote")
                sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to cancel/requote the below passengers ticket");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Name          :" + dr["FLDNAME"].ToString());
            sbemailbody.AppendLine("Ticket Number :" + dr["FLDTICKETNO"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine();
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("For and on behalf of " + HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine();

            PhoenixMail.SendMail(dr["FLDAGENTMAIL"].ToString().Replace(";", ",").TrimEnd(',')
                , ""
                , ""
                , mailsubject
                , sbemailbody.ToString(), false
                , System.Net.Mail.MailPriority.Normal
                , ""
                , null);
            ucStatus.Visible = true;
            ucStatus.Text = "Email Sent";
        }
    }
    private bool IsValidTravelBreakUp(string departuredateold, string departuredate, string arrivaldateold, string arrivaldate, string originold, string origin,
                                        string destinationold, string destination,
                                        string airlinecodeold, string airlinecode, string aclassold, string aclass, string amountold, string amount, string taxold, string tax,
                                        string ticketnoold, string ticketno, string pnrnoold, string pnrno)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (originold.Trim() == "" || origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destinationold.Trim() == "" || destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";
        if (destinationold.Trim().ToString().Equals(originold.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in Ist sector.";
        if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in 2nd sector.";

        if (General.GetNullableDateTime(departuredateold) == null || General.GetNullableDateTime(departuredateold) == null)
            ucError.ErrorMessage = "Departure Date is required.";


        if (General.GetNullableDateTime(departuredate) == null || General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Depature Date is required";

        if (General.GetNullableDateTime(arrivaldateold) == null || General.GetNullableDateTime(arrivaldateold) == null)
            ucError.ErrorMessage = "Arrival Date is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date in 2nd sector";

        //if (string.IsNullOrEmpty(ticketno.Trim()) || string.IsNullOrEmpty(ticketnoold.Trim()))
        //    ucError.ErrorMessage = "Ticket No not mapped.";

        if (string.IsNullOrEmpty(pnrno.Trim()) || string.IsNullOrEmpty(pnrnoold.Trim()))
            ucError.ErrorMessage = "PNR No is required.";

        if (string.IsNullOrEmpty(airlinecode.Trim()) || string.IsNullOrEmpty(airlinecodeold.Trim()))
            ucError.ErrorMessage = "Airline code  is required.";

        if (amount.Trim().Equals("") || amountold.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (tax.Trim().Equals("") || taxold.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination
                                      , string airlinecode, string aclass, string amount, string tax, string ticketno, string pnr)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

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
            ucError.ErrorMessage = "Ticket No not mapped.";

        if (string.IsNullOrEmpty(pnr.Trim()))
            ucError.ErrorMessage = "PNR No  is required.";

        if (string.IsNullOrEmpty(airlinecode.Trim()))
            ucError.ErrorMessage = "Airline code  is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentPickListSelection == null)
                return;

            if (ViewState["CURRENTROW"] != null)
            {
                int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString()) - 2;
                RadTextBox txtoriginname = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginNameBreakup");
                RadTextBox txtoriginid = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginIdBreakup");
                if (txtoriginid != null && txtoriginname != null)
                {
                    txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
                    txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void NewTravelRequisition_Confirm(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                StringBuilder strRouteIDlist = new StringBuilder();

                foreach (GridDataItem gvr in gvLineItem.Items)
                {
                    RadCheckBox chkNewRequest = (RadCheckBox)gvr.FindControl("chkNewRequest");

                    if (chkNewRequest.Checked == true && chkNewRequest.Enabled == true)
                    {

                        RadLabel lblRouteID = (RadLabel)gvr.FindControl("lblRouteID");

                        strRouteIDlist.Append(lblRouteID.Text);
                        strRouteIDlist.Append(",");
                    }
                }

                if (strRouteIDlist.Length > 1)
                {
                    strRouteIDlist.Remove(strRouteIDlist.Length - 1, 1);
                }
                if (strRouteIDlist.ToString().Trim() != "")
                {
                    if (ViewState["VESSEL"] != null && ViewState["TRAVELID"] != null)
                    {

                    }
                }
                BindRoutingDetails();
                BindDataTravelBreakUp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedHopList = new ArrayList();
        Guid index;
        foreach (GridDataItem gvrow in gvCTBreakUp.Items)
        {
            bool result = false;
            index = new Guid(gvCTBreakUp.Items[gvrow.ItemIndex].GetDataKeyValue("FLDHOPLINEITEMID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the ViewState
            if (ViewState["HOPLIST_CHECKED_ITEMS"] != null)
                SelectedHopList = (ArrayList)ViewState["HOPLIST_CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedHopList.Contains(index))
                    SelectedHopList.Add(index);
            }
            else
                SelectedHopList.Remove(index);
        }
        if (SelectedHopList != null && SelectedHopList.Count > 0)
            ViewState["HOPLIST_CHECKED_ITEMS"] = SelectedHopList;
    }

    private void CheckSelectedAllHopList()
    {
        if (ViewState["HOPLIST_CHECKED_ITEMS"] != null)
        {
            for (int i = 0; i < gvCTBreakUp.Items.Count; i++)
            {
                ArrayList SelectedHopList = new ArrayList();
                SelectedHopList = (ArrayList)ViewState["HOPLIST_CHECKED_ITEMS"];
                if (SelectedHopList != null && SelectedHopList.Count > 0)
                {
                    foreach (Guid index in SelectedHopList)
                    {
                        if (gvCTBreakUp.Items[i].GetDataKeyValue("FLDHOPLINEITEMID").ToString().Equals(index.ToString()))
                        {
                            RadCheckBox cbSelected = (RadCheckBox)gvCTBreakUp.Items[i].FindControl("chkSelect");
                            if (cbSelected != null)
                            {
                                cbSelected.Checked = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private string GetSelectedAllHopList()
    {
        string strHopList = "";
        if (ViewState["HOPLIST_CHECKED_ITEMS"] != null)
        {
            ArrayList SelectedHopList = new ArrayList();
            SelectedHopList = (ArrayList)ViewState["HOPLIST_CHECKED_ITEMS"];
            if (SelectedHopList != null && SelectedHopList.Count > 0)
            {
                foreach (Guid index in SelectedHopList)
                {
                    strHopList += index.ToString() + ",";
                }
            }
        }

        if (strHopList != "")
            strHopList = "," + strHopList;

        return strHopList;
    }

    protected void MenuHopList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CREATENEWREQUEST"))
            {
                string strhoplineitemid = GetSelectedAllHopList();
                if (strhoplineitemid == "")
                {
                    ucError.ErrorMessage = "Please select the ticket.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixCrewTravelRequest.InsertNewTravelRequest(General.GetNullableString(strhoplineitemid));
                    ucStatus.Text = "New Requisition Generated";
                    ViewState["HOPLIST_CHECKED_ITEMS"] = null;
                    BindDataTravelBreakUp();
                }
            }
            if (CommandName.ToUpper() == "ATTACHMENTMAPPING")
            {

                string Traveldtkey = (ViewState["TRAVELDTKEY"] == null) ? null : (ViewState["TRAVELDTKEY"].ToString());

                if (Traveldtkey != null && Traveldtkey != "")
                {

                    if (ViewState["TRAVELID"].ToString() != null)
                    {

                        String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewTravelAttachment.aspx?type=TRAVEL &dtkey= " + ViewState["TRAVELDTKEY"].ToString() + "&MOD=" + PhoenixModule.CREW + "&Travelid=" + ViewState["TRAVELID"].ToString() + "',false,800,500);");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper() == "VENDORDETAILS")
            {
                string lblrequestid = ((RadLabel)e.Item.FindControl("lblRequestID")).Text;
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewTravelPassengerInfo.aspx?Requestid=" + lblrequestid + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                SetQuotationInformation();
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["HOPLIST_CHECKED_ITEMS"] = null;
                string lblreqno = ((RadLabel)e.Item.FindControl("lblReqisitionNo")).Text;
                string lblagentname = ((RadLabel)e.Item.FindControl("lblAgentName")).Text;
                lblTitle.Text = "";
                lblTitle.Text = "Ticket (" + lblreqno + "& Agent : " + lblagentname + ")";
                ViewState["ROUTEID"] = ((RadLabel)e.Item.FindControl("lblRouteID")).Text;
                BindDataTravelBreakUp();
                ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                SetQuotationInformation();
            }
            else if (e.CommandName.ToUpper() == "EDIT")
            {
                //BindDataTravelBreakUp();
                string lblreqno = ((RadLabel)e.Item.FindControl("lblReqisitionNo")).Text;
                string lblagentname = ((RadLabel)e.Item.FindControl("lblAgentName")).Text;
                lblTitle.Text = "";
                lblTitle.Text = "Ticket (" + lblreqno + "& Agent : " + lblagentname + ")";
                ViewState["ROUTEID"] = ((RadLabel)e.Item.FindControl("lblRouteID")).Text;
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
                ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                SetQuotationInformation();
            }
            else if (e.CommandName.ToUpper() == "SAVE")
            {
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtamount")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txttax")).Text;

                if (!IsValidTicketNo(amount, tax))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                SetQuotationInformation();
            }
            else if (e.CommandName.ToUpper() == "INITIATENEWREQUEST")
            {
                //string lblreqno = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReqisitionNo")).Text;
                //string lblrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestID")).Text;

                //string lblagentname = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAgentName")).Text;
                //Title1.Text = "Ticket (" + lblreqno + "& Agent : " + lblagentname + ")";
                //ViewState["ROUTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRouteID")).Text;
                //string travelid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelId")).Text;

                //PhoenixCrewTravelQuoteTicket.NewTravelRequestUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                    , General.GetNullableGuid(ViewState["ROUTEID"].ToString())
                //                    , General.GetNullableGuid(lblrequestid)
                //                    , General.GetNullableGuid(travelid));

                //BindRoutingDetails();
                //BindDataTravelBreakUp();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                //string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemidedit")).Text;
                //string depdateold = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                //string arrdateold = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;
                //string originold = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                //string destinationold = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;

                //string departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                //string arrivalampmold = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

                //string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampm")).SelectedValue;
                //string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;

                //string airlinecodeold = ((RadTextBox)e.Item.FindControl("txtAirlineCodeOld")).Text;
                //string aclassold = ((RadTextBox)e.Item.FindControl("txtClassOld")).Text;
                //string amountold = ((UserControlMaskNumber)e.Item.FindControl("txtAmountOld")).Text;
                //string taxold = ((UserControlMaskNumber)e.Item.FindControl("txtTaxold")).Text;
                //string ticketnoOld = ((RadLabel)e.Item.FindControl("txtTicketNoEditOld")).Text.Trim();
                ////string ticketnoOld = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTicketNoEditOld")).Text;
                //string pnrnoOld = ((RadTextBox)e.Item.FindControl("txtpnrnoOld")).Text;


                //string depdate = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
                //string arrdate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                //string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakup")).Text;
                //string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakup")).Text;

                //string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlineCode")).Text;
                //string aclass = ((RadTextBox)e.Item.FindControl("txtClass")).Text;
                //string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                //string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTax")).Text;
                //string ticketno = ((RadLabel)e.Item.FindControl("txtTicketNoEdit")).Text;
                ////string ticketno = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtTicketNoEdit")).Text;
                //string pnrno = ((RadTextBox)e.Item.FindControl("txtpnrno")).Text;

                //string lblattachmentyn = ((RadLabel)e.Item.FindControl("lblattachmentyn")).Text;

                //if (!IsValidTravelBreakUp(depdateold, depdate, arrdateold, arrdate, originold, origin, destinationold, destination,
                //                            airlinecodeold, airlinecode,
                //                            aclassold, aclass, amountold, amount, taxold, tax, ticketnoOld, ticketno, pnrnoOld, pnrno))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                //PhoenixCrewTravelQuoteLine.InsertCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                                        , new Guid(ViewState["TRAVELID"].ToString()), new Guid(Hoplineitemid)
                //                                        , Convert.ToInt32(originold), Convert.ToInt32(origin), Convert.ToInt32(destinationold)
                //                                        , Convert.ToInt32(destination)
                //                                        , Convert.ToDateTime(depdateold)
                //                                        , Convert.ToDateTime(depdate)
                //                                        , Convert.ToDateTime(arrdateold)
                //                                        , Convert.ToDateTime(arrdate)
                //                                        , Convert.ToInt32(departureampmold)
                //                                        , Convert.ToInt32(departureampm)
                //                                        , Convert.ToInt32(arrivalampmold)
                //                                        , Convert.ToInt32(arrivalampm)
                //                                        , airlinecodeold.Trim()
                //                                        , airlinecode.Trim()
                //                                        , aclassold.Trim(), aclass.Trim(), Convert.ToDecimal(amountold), Convert.ToDecimal(amount), Convert.ToDecimal(taxold)
                //                                        , Convert.ToDecimal(tax), General.GetNullableString(ticketnoOld), General.GetNullableString(ticketno), pnrnoOld, pnrno);

                //BindDataTravelBreakUp();
                //ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                //ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                //SetQuotationInformation();
                ViewState["ROUTEID"] = ((RadLabel)e.Item.FindControl("lblRouteID")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtamount")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txttax")).Text;

                if (!IsValidTicketNo(amount, tax))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelQuoteLine.UpdateTicketNo(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblRouteID")).Text)
                    , new Guid(ViewState["QUOTEID"].ToString())
                    , new Guid(((RadLabel)e.Item.FindControl("lblBreakupID")).Text)
                    , null, Convert.ToDecimal(amount), Convert.ToDecimal(tax));

                gvLineItem.EditIndexes.Clear();
                BindRoutingDetails();
                gvLineItem.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
        BindRoutingDetails();
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            RadLabel lblagentid = (RadLabel)e.Item.FindControl("lblAgentId");
            RadLabel lblquotationid = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");

            RadLabel lbltravelstatus = (RadLabel)e.Item.FindControl("lbltravelstatus");
            RadLabel lblTicketCancelled = (RadLabel)e.Item.FindControl("lblTicketCancelled");
            RadLabel lblNewRequestYN = (RadLabel)e.Item.FindControl("lblNewRequestYN");
            RadLabel lblFinalizedYN = (RadLabel)e.Item.FindControl("lblFinalizedYN");

            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            if (lnkShowReason != null)
                lnkShowReason.Attributes.Add("onclick", "javascript:openNewWindow('Filter', '','" + Session["sitepath"] + "/Crew/CrewTravelTicketCopy.aspx?ISTICKET=1&VIEWONLY=false&framename=ifMoreInfo&ROUTEID=" + lblRouteID.Text + "')");

            RadLabel lblRequestID = (RadLabel)e.Item.FindControl("lblRequestID");
            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");
                ib.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelRequestAttachmentView.aspx?REQUESTID=" + lblRequestID.Text + "')");
                if (!lblattachmentyn.Text.Equals("1"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    ib.Controls.Add(html);
                }
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv != null && (drv["FLDISAPPROVED"].ToString().Equals("0") || drv["FLDCANCELTICKETYN"].ToString().Equals("1")))
            {
                if (ib != null && lnkShowReason != null)
                {
                    lnkShowReason.Visible = false;
                }
                else
                {
                    lnkShowReason.Visible = true;
                }
            }
            LinkButton btnupdate = (LinkButton)e.Item.FindControl("cmdSave");
            LinkButton btnsave = (LinkButton)e.Item.FindControl("cmdRowSave");
            if (ViewState["ISREQUOTE"] != null && ViewState["ISREQUOTE"].ToString().Equals("1"))
            {
                if (btnupdate != null)
                {
                    btnupdate.Visible = false;
                    btnsave.Visible = true;
                }
            }
            else
            {
                if (btnupdate != null)
                {
                    btnupdate.Visible = true;
                    btnsave.Visible = false;
                }
            }
        }
    }

    protected void gvCTBreakUp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;
            else if (e.CommandName.ToUpper() == "CANCELAPPROVAL")
            {
                string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;

                if (Hoplineitemid != null && Hoplineitemid != "")
                {
                    PhoenixCrewTravelQuoteLine.CrewTravelAttachmentCancelApprovalUpdate(new Guid(Hoplineitemid));
                    BindDataTravelBreakUp();
                    gvCTBreakUp.Rebind();
                }
            }

            else if (e.CommandName.ToUpper() == "SAVE")
            {
                string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemidedit")).Text;
                RadLabel lblTicketCancelled = (RadLabel)e.Item.FindControl("lblTicketCancelledYNEdit");


                string lblattachmentyn = ((RadLabel)e.Item.FindControl("lblattachmentyn")).Text;

                string depdate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrdate = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;
                UserControlQuick ucreason = (UserControlQuick)e.Item.FindControl("ddlTktCancelledReasonEdit");
                string cancelledreason = ucreason.SelectedQuick;

                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

                string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlineCodeOld")).Text;
                string aclass = ((RadTextBox)e.Item.FindControl("txtClassOld")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountOld")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTaxold")).Text;
                string ticketno = ((RadLabel)e.Item.FindControl("txtTicketNoEditOld")).Text;
                string pnrno = ((RadTextBox)e.Item.FindControl("txtpnrnoOld")).Text;

                if (!IsValidTravelBreakUp(depdate, arrdate, origin, destination, airlinecode, aclass, amount, tax, ticketno, pnrno))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelQuoteLine.UpdateCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , new Guid(Hoplineitemid), Convert.ToInt32(origin), Convert.ToInt32(destination), Convert.ToDateTime(depdate)
                          , Convert.ToDateTime(arrdate)
                          , General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm)
                          , airlinecode, aclass, Convert.ToDecimal(amount), Convert.ToDecimal(tax), ticketno.Trim(), pnrno.Trim(), General.GetNullableInteger(cancelledreason), 0);
                //  }               
                gvCTBreakUp.EditIndexes.Clear();
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            else if (e.CommandName.ToUpper() == "REISSUE")
            {
                string requestid = string.Empty;
                string hopid = string.Empty;
                string ticketno = string.Empty;

                string amount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;
                string tax = ((RadLabel)e.Item.FindControl("lblTax")).Text;
                requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                hopid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;
                ticketno = ((RadLabel)e.Item.FindControl("lblTicketNo")).Text;

                PhoenixCrewTravelQuoteLine.ReissueTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , new Guid(((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text)
                 , new Guid(ViewState["QUOTEID"].ToString())
                 , null
                 , new Guid(((RadLabel)e.Item.FindControl("lblRequestId")).Text)
                 , General.GetNullableString(ticketno.Trim()), Convert.ToDecimal(amount), Convert.ToDecimal(tax));

                ViewState["ISREQUOTE"] = "0";
                BindRoutingDetails();
                gvLineItem.Rebind();
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
                SendMail(requestid, hopid, "tktreissue");

                BindRoutingDetails();
                BindDataTravelBreakUp();
            }
            else if (e.CommandName.ToUpper() == "CANCELTICKET")
            {
                string requestid = string.Empty;
                string hopid = string.Empty;
                requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                hopid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;

                PhoenixCrewTravelQuoteLine.CancelTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , new Guid(((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text)
                 , new Guid(((RadLabel)e.Item.FindControl("lblRequestId")).Text));

                ViewState["ISREQUOTE"] = "0";

                BindRoutingDetails();
                gvLineItem.Rebind();
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();

                SendMail(requestid, hopid, "tktcancel");

                BindRoutingDetails();
                BindDataTravelBreakUp();
            }
            else if (e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["CURRENTROW"] = e.Item.RowIndex;
                ViewState["EDITROW"] = "0";
                BindDataTravelBreakUp();
                ViewState["EDITROW"] = "1";
                BindDataTravelBreakUp();
            }
            else if (e.CommandName.ToUpper() == "INITIATENEWREQUEST")
            {
                string hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;

                PhoenixCrewTravelRequest.InsertNewTravelRequest(General.GetNullableGuid(hoplineitemid));

                ucStatus.Text = "New Quote Initiated";
                //  BindDataTravelBreakUp();

                string requestid = string.Empty;
                string ticketno = string.Empty;

                requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ticketno = ((RadLabel)e.Item.FindControl("lblTicketNo")).Text;

                ViewState["ISREQUOTE"] = "0";

                SendMail(requestid, hoplineitemid, "tktRequote");

                BindRoutingDetails();
                BindDataTravelBreakUp();

            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemidedit")).Text;
                string depdateold = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrdateold = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;
                string originold = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destinationold = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;

                string departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampmold = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampm")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;

                string airlinecodeold = ((RadTextBox)e.Item.FindControl("txtAirlineCodeOld")).Text;
                string aclassold = ((RadTextBox)e.Item.FindControl("txtClassOld")).Text;
                string amountold = ((UserControlMaskNumber)e.Item.FindControl("txtAmountOld")).Text;
                string taxold = ((UserControlMaskNumber)e.Item.FindControl("txtTaxold")).Text;
                string ticketnoOld = ((RadLabel)e.Item.FindControl("txtTicketNoEditOld")).Text.Trim();
                string pnrnoOld = ((RadTextBox)e.Item.FindControl("txtpnrnoOld")).Text;


                string depdate = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
                string arrdate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakup")).Text;

                string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlineCode")).Text;
                string aclass = ((RadTextBox)e.Item.FindControl("txtClass")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTax")).Text;
                string ticketno = ((RadLabel)e.Item.FindControl("txtTicketNoEdit")).Text;
                string pnrno = ((RadTextBox)e.Item.FindControl("txtpnrno")).Text;

                string lblattachmentyn = ((RadLabel)e.Item.FindControl("lblattachmentyn")).Text;

                if (!IsValidTravelBreakUp(depdateold, depdate, arrdateold, arrdate, originold, origin, destinationold, destination,
                                            airlinecodeold, airlinecode,
                                            aclassold, aclass, amountold, amount, taxold, tax, ticketnoOld, ticketno, pnrnoOld, pnrno))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelQuoteLine.InsertCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , new Guid(ViewState["TRAVELID"].ToString()), new Guid(Hoplineitemid)
                                                        , Convert.ToInt32(originold), Convert.ToInt32(origin), Convert.ToInt32(destinationold)
                                                        , Convert.ToInt32(destination)
                                                        , Convert.ToDateTime(depdateold)
                                                        , Convert.ToDateTime(depdate)
                                                        , Convert.ToDateTime(arrdateold)
                                                        , Convert.ToDateTime(arrdate)
                                                        , Convert.ToInt32(departureampmold)
                                                        , Convert.ToInt32(departureampm)
                                                        , Convert.ToInt32(arrivalampmold)
                                                        , Convert.ToInt32(arrivalampm)
                                                        , airlinecodeold.Trim()
                                                        , airlinecode.Trim()
                                                        , aclassold.Trim(), aclass.Trim(), Convert.ToDecimal(amountold), Convert.ToDecimal(amount), Convert.ToDecimal(taxold)
                                                        , Convert.ToDecimal(tax), General.GetNullableString(ticketnoOld), General.GetNullableString(ticketno), pnrnoOld, pnrno);

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();

            }
            //else if (e.CommandName.ToUpper() == "MOVETICKET")
            //{
            //    string hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;
            //    PhoenixCrewTravelQuoteLine.CrewTravelTicketToEmployeeMovementInsert(new Guid(hoplineitemid));
            //    ucStatus.Text = "Ticket Moved successfully";
            //}

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

    protected void gvCTBreakUp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton imgReissue = (LinkButton)e.Item.FindControl("cmdReissue");
            LinkButton imgCancelTicket = (LinkButton)e.Item.FindControl("cmdCancelTicket");

            RadLabel lblDepDatePassedYN = (RadLabel)e.Item.FindControl("lblDepDatePassedYN");
            RadLabel lblTicketCancelledYN = (RadLabel)e.Item.FindControl("lblTicketCancelledYN");

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");
                ib.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&MOD=" + PhoenixModule.CREW + "')");
                if (!lblattachmentyn.Text.Equals("1"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    ib.Controls.Add(html);
                }
            }
            LinkButton ibm = (LinkButton)e.Item.FindControl("cmdAttachmentMapping");
            RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachment");
            RadLabel ticket = (RadLabel)e.Item.FindControl("lblTicketNo");
            if (ibm != null && lbm.Text != null && ticket.Text != null)
            {
                RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");
                if (lblattachmentmappingyn != null)
                {
                    if (!lblattachmentmappingyn.Text.Equals("1"))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        ibm.Controls.Add(html);
                    }
                }
                ibm.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAttachmentView.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "')");
            }

            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (imgCancelTicket != null) imgCancelTicket.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Cancel Ticket?'); return false;");
            if (imgReissue != null) imgReissue.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Reissue the Ticket?'); return false;");

            if (e.Item.RowIndex == 0)
            {
                if (db != null)
                    db.Visible = false;
            }

            RadLabel lblTicketCancelApproved = (RadLabel)e.Item.FindControl("lblTicketCancelApproved");
            LinkButton imgTicketCancelApproved = (LinkButton)e.Item.FindControl("imgTicketCancelApproved");
            LinkButton ImgApproved = (LinkButton)e.Item.FindControl("ImgApproved");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton imgCancelApproval = (LinkButton)e.Item.FindControl("imgCancelApproval");

            if (lblTicketCancelApproved != null && lblTicketCancelApproved.Text == "0")
            {
                if (imgTicketCancelApproved != null)
                    imgTicketCancelApproved.Visible = true;
                if (imgCancelApproval != null)
                    imgCancelApproval.Visible = true;
                if (eb != null)
                    eb.Visible = false;
            }
            if (lblTicketCancelApproved != null && lblTicketCancelApproved.Text == "1")
            {
                if (ImgApproved != null)
                    ImgApproved.Visible = true;
            }

            //if (ViewState["EDITROW"].ToString() == "1")
            //{
            RadTextBox txtorigin = (RadTextBox)e.Item.FindControl("txtOriginIdBreakup");
            RadTextBox txtdestination = (RadTextBox)e.Item.FindControl("txtDestinationIdBreakup");
            ImageButton btnorigin = (ImageButton)e.Item.FindControl("btnShowOriginbreakup");
            ImageButton btndestination = (ImageButton)e.Item.FindControl("btnShowDestinationbreakup");
            RadTextBox txtoriginname = (RadTextBox)e.Item.FindControl("txtOriginNameBreakup");
            RadTextBox txtdestinationname = (RadTextBox)e.Item.FindControl("txtDestinationNameBreakup");
            RadTextBox txtDestinationNameOldBreakup = (RadTextBox)e.Item.FindControl("txtDestinationNameOldBreakup");

            UserControlDate txtDepartureDate = (UserControlDate)e.Item.FindControl("txtDepartureDate");
            UserControlDate txtArrivalDate = (UserControlDate)e.Item.FindControl("txtArrivalDate");

            RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampm"));
            RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

            RadComboBox departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
            RadComboBox arrivalampmold = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold"));

            RadTextBox txtoriginold = (RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup");
            RadTextBox txtdestinationold = (RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup");
            RadTextBox txtoriginoldname = (RadTextBox)e.Item.FindControl("txtOriginNameOldBreakup");
            ImageButton btnoriginold = (ImageButton)e.Item.FindControl("btnShowOriginoldbreakup");
            ImageButton btndestinationold = (ImageButton)e.Item.FindControl("btnShowDestinationOldbreakup");

            UserControlDate txtDepartureDateOld = (UserControlDate)e.Item.FindControl("txtDepartureDateOld");
            UserControlDate txtArrivalDateOld = (UserControlDate)e.Item.FindControl("txtArrivalDateOld");

            UserControlMaskNumber txtamount = (UserControlMaskNumber)e.Item.FindControl("txtAmount");
            UserControlMaskNumber txttax = (UserControlMaskNumber)e.Item.FindControl("txtTax");
            RadTextBox txtairlinecode = (RadTextBox)e.Item.FindControl("txtAirlineCode");
            RadTextBox txtclass = (RadTextBox)e.Item.FindControl("txtClass");
            RadLabel txtticket = (RadLabel)e.Item.FindControl("txtTicketNoEdit");
            RadTextBox txtpnrs = (RadTextBox)e.Item.FindControl("txtpnrno");
            LinkButton cmdRowSave = (LinkButton)e.Item.FindControl("cmdRowSave");
            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");

            if (txtorigin != null)
            {
                txtorigin.Visible = false;
                txtArrivalDate.Visible = false;
                txtDepartureDate.Visible = false;
                txtdestination.Visible = false;
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

                txtamount.Visible = false;
                txttax.Visible = false;
                txtairlinecode.Visible = false;
                txtclass.Visible = false;
                txtticket.Visible = false;
                txtpnrs.Visible = false;

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
            if (lblTicketCancelledYN.Text == "1")
            {
                UserControlQuick ddlTktCancelledReason = (UserControlQuick)e.Item.FindControl("ddlTktCancelledReasonEdit");
                if (ddlTktCancelledReason != null)
                {
                    ddlTktCancelledReason.Visible = true;
                    ddlTktCancelledReason.SelectedQuick = drv["FLDCANCELLEDREASON"].ToString();
                }

            }
            //}

            RadLabel lblticketstatus = (RadLabel)e.Item.FindControl("lblTicketStatus");
            if (lblDepDatePassedYN != null && lblDepDatePassedYN.Text == "YES" && imgCancelTicket != null)
            {
                imgCancelTicket.Visible = false;
            }
            LinkButton imgTravelBreakUp = (LinkButton)e.Item.FindControl("cmdTravelBreakUp");
            //ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            RadLabel lblTravelstatus = (RadLabel)e.Item.FindControl("lblHPtravelstatus");

            if (lblTravelstatus != null && lblTravelstatus.Text.ToString() == "CANCELLED" && imgTravelBreakUp != null)
            {
                imgTravelBreakUp.Visible = false;
            }
            LinkButton imgArr = (LinkButton)e.Item.FindControl("cmdArrangements");
            RadLabel lblhoplineitemid = (RadLabel)e.Item.FindControl("lblhoplineitemid");
            if (imgArr != null && lblhoplineitemid != null && lblhoplineitemid.Text != "")
                imgArr.Attributes.Add("onclick", "javascript:openNewWindow('Filter', '','" + Session["sitepath"] + "/Crew/CrewTravelLocalArrangements.aspx?&HOPLINEITEMID=" + lblhoplineitemid.Text.Trim() + "')");
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");
            if (drv["FLDCANCELTICKETYN"].ToString() == "1" && drv["FLDNEWREQUESTYN"].ToString() != "1")
                chk.Visible = true;
            else
                chk.Visible = false;
        }
    }
}
