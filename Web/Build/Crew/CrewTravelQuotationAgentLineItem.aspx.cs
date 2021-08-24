using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web;

public partial class CrewTravelQuotationAgentLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvQuotation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        if (Request.QueryString["TRAVELAGENTID"] != null)
        {
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationOffice.aspx?OFFICE=Y&SESSIONID=" + Request.QueryString["TRAVELAGENTID"].ToString() + "');return false;", "New Quote", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            ViewState["TRAVELAGENTID"] = Request.QueryString["TRAVELAGENTID"].ToString();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelAgentQuotationChat.aspx?OFFICE=Y&TravelId=" + Request.QueryString["TRAVELID"].ToString() + "&TravelAgentId=" + Request.QueryString["TRAVELAGENTID"].ToString() + "&title=" + PhoenixCrewTravelRequest.RequestNo + " - " + PhoenixCrewTravelQuote.Agent + "');return false;", "Quotation Chat", "<i class=\"fas fa-comments-dollar\"></i>", "");        }
        MenuQuotationList.AccessRights = this.ViewState;
        MenuQuotationList.MenuList = toolbargrid.Show();
                      
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            btnconfirm.Attributes.Add("style", "display:none");
            if (Request.QueryString["TRAVELAGENTID"] != null)
                lblTitle.Text = "Quotation For [ " + PhoenixCrewTravelRequest.RequestNo + " &   Agent :   " + PhoenixCrewTravelQuote.Agent + "     ]";
            else
                lblTitle.Text = "Quotation For [ " + PhoenixCrewTravelRequest.RequestNo + "  ]";
            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
            if (Request.QueryString["AGENTID"] != null)
                ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
            if (Request.QueryString["port"] != null)
                ViewState["PORT"] = Request.QueryString["port"];
            if (Request.QueryString["date"] != null)
                ViewState["DATE"] = Request.QueryString["date"];
            if (Request.QueryString["vessel"] != null)
                ViewState["VESSEL"] = Request.QueryString["vessel"];

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["QUOTEPAGENUMBER"] = 1;
            ViewState["QUOTESORTEXPRESSION"] = null;
            ViewState["QUOTESORTDIRECTION"] = null;

            ViewState["PASSPAGENUMBER"] = 1;
            ViewState["PASSSORTEXPRESSION"] = null;
            ViewState["PASSSORTDIRECTION"] = null;

            ViewState["CURRENTINDEX"] = 1;
            gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvQuotation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvQuotePassengers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        toolbargrid = new PhoenixToolbar();
        MenuQuotationLineItem.AccessRights = this.ViewState;
        MenuQuotationLineItem.Title = lblTitle.Text;
        MenuQuotationLineItem.MenuList = toolbargrid.Show();
        BindQuotationDetails();
        BindQuotePassengers();
        if (ViewState["QUOTEID"] == null)
            BindPassengerDetails();
        else
            BindRoutingDetails();

        Guidlines();
    }
    private void BindPassengerDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelPassengerSearch(new Guid(ViewState["TRAVELID"].ToString()), null, null, int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            for (int i = 10; i < gvLineItem.Columns.Count; i++)
            {
                gvLineItem.Columns[i].Visible = false;
            }
            ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
        }
        else
        {
            gvLineItem.DataSource = "";          
        }
        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);
    }
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("AGENT"))
            {
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&PORT=" + ViewState["PORT"].ToString()
                    + "&DATE=" + ViewState["DATE"].ToString()
                    + "&VESSEL=" + ViewState["VESSEL"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private void SendForTicketAttachment(string agentid)
    {
        string emailid;
        try
        {
            PhoenixCrewTravelQuoteLine.CrewTravelBreakupInsertForTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));
            DataSet dsvendor = PhoenixCrewTravelQuote.ListCrewTravelQuotationToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString()), agentid, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsvendor.Tables[0].Rows.Count > 0)
            {

                DataRow dr = dsvendor.Tables[0].Rows[0];

                string emailbodytext = "";
                if (dr["FLDEMAIL1"].ToString().Contains(";"))
                    emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                else
                    emailid = dr["FLDEMAIL1"].ToString();

                if (!dr["FLDEMAIL2"].ToString().Equals(""))
                {
                    emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                }
                try
                {
                    if (dr["FLDRFQPREFERENCE"].ToString().Equals("WEB"))
                    {

                        PhoenixCrewTravelQuote.InsertWebSession(new Guid(dr["FLDTRAVELID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "TRFQ", PhoenixCrewTravelRequest.RequestNo);
                        emailbodytext = PrepareEmailBodyTextForTicket(new Guid(dr["FLDTRAVELID"].ToString()), PhoenixCrewTravelRequest.RequestNo, dr["FLDAGENTID"].ToString());
                        string vsl = "";
                        if (Filter.CurrentTraveltoVesselName != null)
                            vsl = Filter.CurrentTraveltoVesselName.ToString();
                        PhoenixCommoneProcessing.PrepareEmailMessage(emailid, "TRFQ", new Guid(dr["FLDTRAVELID"].ToString()), emailid, emailid, dr["FLDNAME"].ToString(), "TRFQ for " + PhoenixCrewTravelRequest.RequestNo.ToString() + " - " + vsl.ToString(), emailbodytext, "", "");
                    }
                    ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                }
                catch (Exception ex)
                {
                    ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                }

                ucConfirm.Visible = true;
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
    protected string PrepareEmailBodyTextForTicket(Guid travelid, string formno, string agentid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewTravelQuote.GetTicketDataForEmailBody(travelid, "TPO");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "             " + formno);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide ticket number.");

        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated");
        sbemailbody.AppendLine();
        string url = Session["sitepath"] + "/" + dr["URL"].ToString();
        sbemailbody.AppendLine(url + "?SESSIONID=" + travelid + "&QUOTEID=" + ViewState["QUOTEID"].ToString() + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString());

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td> &nbsp;1. To add/view quotation, select the agent from travel agents and click on &nbsp;<i class=\"fa fa-plus-circle\"></i>&nbsp; button on the travel quotes. </td> </tr> <tr><td>&nbsp;2. To approve the quotation click on &nbsp;<i class=\"fas fa-award\"></i>&nbsp; button in the travel quotes.</td> </tr><tr><td>&nbsp;3. To finalize the quotation and send mail to order ticket click on &nbsp; <i class=\"fas fa-file-invoice-dollar\"></i>&nbsp; button in the travel quotes.</td> </tr></tr><tr><td>&nbsp;4. To view/save  itinerary click on &nbsp;<i class=\"fas fa-clipboard - list\"></i><&nbsp; button in the passengers travel Details.</td> </tr><tr><td>&nbsp;5. To approve the passenger click on &nbsp;<i class=\"fas fa-award\"></i>&nbsp; button in the Passengers List.</td> </tr><tr><td>&nbsp;6. To Resend the Quote click on &nbsp;<i class=\"fas fa-envelope\"></i>&nbsp; button in the Passengers List.</td> </tr></table>";
        //ucToolTip.Text = "<table> <tr><td> &nbsp;1. To add/view quotation, select the agent from travel agents and click on &nbsp;<img id=\"img2\" runat=\"server\" src=" + Session["images"] + "/add.png style=\"vertical-align: top\" />&nbsp; button on the travel quotes. </td> </tr> <tr><td>&nbsp;2. To approve the quotation click on &nbsp;<img id=\"img4\" runat=\"server\" src=" + Session["images"] + "/approve.png style=\"vertical-align: top\" />&nbsp; button in the travel quotes.</td> </tr><tr><td>&nbsp;3. To finalize the quotation and send mail to order ticket click on &nbsp;<img id=\"img5\" runat=\"server\" src=" + Session["images"] + "/final-quotation.png style=\"vertical-align: top\" />&nbsp; button in the travel quotes.</td> </tr></tr><tr><td>&nbsp;4. To view/save  itinerary click on &nbsp;<img id=\"img5\" runat=\"server\" src=" + Session["images"] + "/reschedule-remark.png style=\"vertical-align: top\" />&nbsp; button in the passengers travel Details.</td> </tr><tr><td>&nbsp;5. To approve the passenger click on &nbsp;<img id=\"img4\" runat=\"server\" src=" + Session["images"] + "/approve.png style=\"vertical-align: top\" />&nbsp; button in the Passengers List.</td> </tr><tr><td>&nbsp;6. To Resend the Quote click on &nbsp;<img id=\"img4\" runat=\"server\" src=" + Session["images"] + "/Email.png style=\"vertical-align: top\" />&nbsp; button in the Passengers List.</td> </tr></table>";
        //imgnote.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'visible');");
        //imgnote.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'hidden');");

    }

    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string travelid = null;
        string travelagentid = null;
        int agentid = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDRECEIVEDDATE", "FLDCURRENCYCODE", "FLDCURRENTAMOUNT", "FLDCURRENTTAX", "FLDCURRENTTOTALAMOUNT", "FLDTOTALAMOUNT", "FLDQUOTATIONSTATUS", "FLDISMARINEFAREYN", "APPROVERNAME", "FLDTRAVELAPPROVEDDATE", "FLDPOSENTDATE" };
        string[] alCaptions = { "Quotation No", "Quotation Date", "Currency", "Fare", "Tax", "Total Amount", "Total Amount in USD", "Quoted Status", "Marine/Non Marine Fare", "Approved By", "Approved Date", "PO Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        if (ViewState["TRAVELAGENTID"] != null)
            travelagentid = ViewState["TRAVELAGENTID"].ToString();

        ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(travelid), General.GetNullableGuid(travelagentid), sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
                         gvQuotation.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Travel_Quotes.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Quotes</h3></td>");
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
    private void BindQuotationDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDRECEIVEDDATE", "FLDCURRENCYCODE", "FLDCURRENTAMOUNT", "FLDCURRENTTAX", "FLDCURRENTTOTALAMOUNT", "FLDTOTALAMOUNT", "FLDQUOTATIONSTATUS", "FLDISMARINEFAREYN", "APPROVERNAME", "FLDTRAVELAPPROVEDDATE", "FLDPOSENTDATE" };
        string[] alCaptions = { "Quotation No", "Quotation Date", "Currency", "Fare", "Tax", "Total Amount", "Total Amount in USD", "Quoted Status", "Marine/Non Marine Fare", "Approved By", "Approved Date", "PO Date" };
        string travelid = null;
        string travelagentid = null;
        int agentid = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        if (ViewState["TRAVELAGENTID"] != null)
            travelagentid = ViewState["TRAVELAGENTID"].ToString();

        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(travelid), General.GetNullableGuid(travelagentid), sortexpression, sortdirection
            , int.Parse(ViewState["QUOTEPAGENUMBER"].ToString()), gvQuotation.PageSize, ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotation.DataSource = ds;
            gvQuotation.VirtualItemCount = iRowCount;
            if (ViewState["TRAVELREQUESTID"] == null)
                ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                //gvQuotation.SelectedIndex = 0;
            }
            // BindRoutingDetails();
        }
        else
        {
            gvQuotation.DataSource = "";
        }
        General.SetPrintOptions("gvQuotation", "Travel Quotes", alCaptions, alColumns, ds);
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
            BindQuotationDetails();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void btn_approve(object sender, EventArgs e)
    {
        //if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        //{
            approve();
        //}
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
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }
    private void UpdateSelectAgentForPo(string quotationid)
    {
        try
        {
            PhoenixCrewTravelQuote.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), new Guid(quotationid));
            BindQuotationDetails();
            ucConfirm.ErrorMessage = "Quote is selected";
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindQuotationDetails();
            gvQuotation.Rebind();
            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage = "E-mail send to the below Agents,<BR/>" + Session["emailsend"].ToString();
                ucConfirm.Visible = true;
            }
            Session["emailsend"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        if (ViewState["QUOTEID"] != null)
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearch(new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()), int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount, 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds;
                gvLineItem.VirtualItemCount = iRowCount;
                ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
            }
            else
            {
                gvLineItem.DataSource = "";
            }

            General.SetPrintOptions("gvAgentLineItem", "Travel Quotations", alCaptions, alColumns, ds);
        }
    }
    private void BindQuotePassengers()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        ds = PhoenixCrewTravelQuote.CrewTravelQuotationPassengersList(
            General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString())
            , General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString())
            , int.Parse(ViewState["PASSPAGENUMBER"].ToString()), gvQuotePassengers.PageSize, ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotePassengers.DataSource = ds.Tables[0];
            gvQuotePassengers.VirtualItemCount = iRowCount;
        }
        else
        {
            gvQuotePassengers.DataSource = "";
        }
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
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();

    }

    protected void gvQuotation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
                ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
                BindRoutingDetails();
                BindQuotePassengers();
                gvQuotePassengers.Rebind();
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
                BindQuotationDetails();
                gvQuotation.Rebind();
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
                        //string script = "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=POORDER&travelid=" + ViewState["TRAVELID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString() + "&agentid=" + ViewState["AGENTID"].ToString() + "');return false";                       
                        //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "", script, false);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Quote Not Finalized";
                        ucError.Visible = true;
                    }
                    BindQuotationDetails();
                    gvQuotation.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                try
                {
                    ViewState["QUOTATIONID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;                    

                    PhoenixCrewTravelQuote.TicketBudgetValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblQuotationID")).Text));
                }
                catch (Exception exe)
                {
                    if (exe.Message.ToString().ToUpper().Equals("INSUFFICIENT BUDGET. CONTINUE?"))
                    {
                        RadWindowManager1.RadConfirm(exe.Message, "btnconfirm", 320, 150, null, "Confirm");
                        //ucConfirmMessage.HeaderMessage = "Please Confirm";
                        //ucConfirmMessage.Text = exe.Message;
                        //ucConfirmMessage.OKText = "Yes";
                        //ucConfirmMessage.CancelText = "No";
                        //ucConfirmMessage.Visible = true;
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
            else if (e.CommandName == "Page")
            {
                ViewState["QUOTEPAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void gvQuotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["QUOTEPAGENUMBER"] = ViewState["QUOTEPAGENUMBER"] != null ? ViewState["QUOTEPAGENUMBER"] : gvQuotation.CurrentPageIndex + 1;
        BindQuotationDetails();
    }

    protected void gvQuotation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lb1 = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblTravelID");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblAgentID");
            LinkButton lbn = (LinkButton)e.Item.FindControl("lblQuotationNo");
            RadLabel lblapproved = (RadLabel)e.Item.FindControl("lblIsApproved");
            RadLabel lblfinalized = (RadLabel)e.Item.FindControl("lblIsFinalized");
            RadLabel lbltravelfinalizeyn = (RadLabel)e.Item.FindControl("lbltravelfinalizeyn");

            LinkButton imgappove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton imgfinalize = (LinkButton)e.Item.FindControl("cmdFinalize");
            HtmlImage imgflag = (HtmlImage)e.Item.FindControl("imgflag");
            if (lblapproved != null && lblfinalized != null && imgappove != null && imgfinalize != null)
            {
                if (lblapproved.Text.Equals("1") && lblfinalized.Text.Equals("1"))
                {
                    imgflag.Visible = true;
                }
                else if (lblapproved.Text.Equals("1"))
                {
                    imgflag.Visible = false;
                }
                else
                {
                    imgflag.Visible = false;
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
        }
    }

    protected void gvQuotePassengers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {          
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lblrequestid = (RadLabel)e.Item.FindControl("lblrequestid");

                PhoenixCrewTravelQuote.QuotePassengerapprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , new Guid(ViewState["TRAVELID"].ToString())
                                                , new Guid(ViewState["QUOTEID"].ToString())
                                                , new Guid(lblrequestid.Text));

            }
            else if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
            {
                RadLabel lblrequestid = (RadLabel)e.Item.FindControl("lblrequestid");

                PhoenixCrewTravelQuote.QuotePassengerDeapprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , new Guid(ViewState["TRAVELID"].ToString())
                                                , new Guid(ViewState["QUOTEID"].ToString())
                                                , new Guid(lblrequestid.Text));

            }
            else if (e.CommandName.ToUpper().Equals("SENDMAIL"))
            {
                ResendQuoteMail();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PASSPAGENUMBER"] = null;
            }
            BindQuotationDetails();
            gvQuotation.Rebind();
            BindQuotePassengers();
            gvQuotePassengers.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuotePassengers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PASSPAGENUMBER"] = ViewState["PASSPAGENUMBER"] != null ? ViewState["PASSPAGENUMBER"] : gvQuotePassengers.CurrentPageIndex + 1;
        BindQuotePassengers();
    }

    protected void gvQuotePassengers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblapproved = (RadLabel)e.Item.FindControl("lblisapproved");
            RadLabel lblQuotationNumber = (RadLabel)e.Item.FindControl("lblQuotationNumber");
            RadLabel lblQuoteId = (RadLabel)e.Item.FindControl("lblQuoteId");

            LinkButton imgapprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton imgdeapprove = (LinkButton)e.Item.FindControl("cmdDeApprove");
            LinkButton imgmail = (LinkButton)e.Item.FindControl("cmdSemdMail");

            if (ViewState["QUOTEID"] != null && General.GetNullableString(lblQuoteId.Text) != null)
            {
                if (ViewState["QUOTEID"].ToString() != lblQuoteId.Text)
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
            else if (ViewState["QUOTEID"] != null && General.GetNullableString(lblQuoteId.Text) == null)
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
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
        BindPassengerDetails();
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblQuoteID = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");
            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            lnkShowReason.Attributes.Add("onclick", "javascript:openNewWindow('Filter', 'Itinerary','" + Session["sitepath"] + "/Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&framename=ifMoreInfo&ROUTEID=" + lblRouteID.Text + "');return false;");


            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                ib.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&MOD=" + PhoenixModule.CREW + "');return false;");
            }
            RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");
            if (ib != null)
            {
                if (lblattachmentyn != null && lb != null)
                {
                    if (!lblattachmentyn.Text.Equals("1"))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        ib.Controls.Add(html);
                    }
                }
            }

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
    }
}
