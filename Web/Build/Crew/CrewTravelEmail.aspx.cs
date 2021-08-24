using System;
using System.Data;
using System.Text;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class CrewTravelEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);           

            if (!IsPostBack)
            {
                ViewState["purpose"] = null;
                ViewState["EMPLOYEENAME"] = null;
                ViewState["USERNAME"] = null;
                ViewState["ISTICKETYN"] = null;
                ViewState["TICKETLIST"] = null;
                ViewState["VESSELNAME"] = null;

                if (Request.QueryString["travelid"] != null && Request.QueryString["purpose"] != null && Request.QueryString["purpose"].ToUpper().Equals("RFQ"))
                {
                    //Title1.Text = "Send Query";
                    lblTitle.Text= "Send Query";
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                    ViewState["selectedagent"] = Request.QueryString["selectedagent"] == null ? null : Request.QueryString["selectedagent"].ToString();
                    ViewState["purpose"] = Request.QueryString["purpose"];
                }
                if (Request.QueryString["travelid"] != null && Request.QueryString["purpose"] != null && Request.QueryString["purpose"].ToUpper().Equals("RFQREMAINDER"))
                {
                    lblTitle.Text = "Send Reminder";
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                    ViewState["selectedagent"] = Request.QueryString["selectedagent"] == null ? null : Request.QueryString["selectedagent"].ToString();
                    ViewState["purpose"]   =   Request.QueryString["purpose"];
                }
                if (Request.QueryString["travelid"] != null && Request.QueryString["purpose"] != null && Request.QueryString["purpose"].ToUpper().Equals("POORDER"))
                {
                    lblTitle.Text = "Send PO";
                    lblheadertext.Text = "E-mail will be send to the below Agent.";

                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();                   
                    ViewState["purpose"] = Request.QueryString["purpose"];
                    ViewState["QUOTEID"] = Request.QueryString["quoteid"].ToString();
                    ViewState["AGENTID"]=Request.QueryString["agentid"].ToString();
                }
                if (Request.QueryString["travelrequestid"] != null && Request.QueryString["purpose"] != null && Request.QueryString["purpose"].ToUpper().Equals("CTR"))
                {
                    lblTitle.Text = "Send Mail";
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
                    ViewState["selectedagent"] = Request.QueryString["selectedagent"] == null ? null : Request.QueryString["selectedagent"].ToString();
                    ViewState["purpose"] = Request.QueryString["purpose"];
                }
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor
            }
            if(ViewState["purpose"].ToString().ToUpper().Equals("RFQ"))
            {
                 SendForTravelQuotation();
            }
            else if (ViewState["purpose"].ToString().ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();
            }
            else if (ViewState["purpose"].ToString().ToUpper().Equals("POORDER"))
            {
                SendForOrderTicket();
            }
            else if (ViewState["purpose"].ToString().ToUpper().Equals("CTR"))
            {
                SendForCancelTravelRequest();
            }
            else
            {
                SendForTravelQuotation();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Send", "SEND",ToolBarDirection.Right);            
            EmailMenu.AccessRights = this.ViewState;
            EmailMenu.Title = lblTitle.Text;
            EmailMenu.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void EmailMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (ViewState["purpose"].ToString().ToUpper().Equals("RFQ"))
                {
                    SendRFQ();
                }
                else if (ViewState["purpose"].ToString().ToUpper().Equals("RFQREMAINDER"))
                {
                    SendRFQReminder();
                }
                else if (ViewState["purpose"].ToString().ToUpper().Equals("POORDER"))
                {
                    SendOrderTicket();
                }
                if (ViewState["purpose"].ToString().ToUpper().Equals("CTR"))
                {
                    SendCTR();
                }
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (ViewState["TRAVELID"] != null)
                {
                    string Script = "";
                    Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList();";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateEmail(string mailto, string mailcc)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(mailto.Trim()))
        {
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";
        }
        if (mailcc.Trim() != string.Empty && !General.IsvalidEmail(mailcc.Trim().TrimEnd(',')))
        {
            ucError.ErrorMessage = "Please enter valid Cc E-Mail Address";
        }
        return (!ucError.IsError);
    }
    private void SendForTravelQuotation()
    {
        try
        {
            string emailbodytext = "";
            string selectedvendors = ",";
            selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
            DataSet dsvendor = PhoenixCrewTravelQuote.ListCrewTravelQuotationToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString()), selectedvendors, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                RepAgents.DataSource = dsvendor.Tables[0];
               // RepAgents.DataBind();

                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                txtSubject.Text = "TRFQ for " + dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " - " + vsl.ToString();
                emailbodytext = PrepareEmailBodyTextdefault(new Guid(dsvendor.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString()), dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                        dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString(), dsvendor.Tables[0].Rows[0]["FLDURL"].ToString(),
                                        dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString());              
                edtBody.Content = emailbodytext.ToString();
            }
            else
            {
                ucError.ErrorMessage = "Email already sent";
                ucError.Visible = true;
                return;
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
            string emailbodytext = "";
            string selectedvendors = ",";
            selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
            DataSet dsvendor = PhoenixCrewTravelQuote.ListQuotationToSendRemainder(General.GetNullableGuid(ViewState["TRAVELID"]== null? null : ViewState["TRAVELID"].ToString()), selectedvendors);
            if (dsvendor.Tables[0].Rows.Count > 0)
            {

                RepAgents.DataSource = dsvendor.Tables[0];
                //RepAgents.DataBind();

                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                txtSubject.Text = "TRFQ for " + dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " - " + vsl.ToString();

                emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dsvendor.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString()), dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString());                          
                edtBody.Content = emailbodytext.ToString();
               
            }
            else
            {
                ucError.ErrorMessage = "No agent selected to send reminder";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendForOrderTicket()
    {
        try
        {
            string emailbodytext = "";
            DataTable dsvendor = PhoenixCrewTravelQuote.OrderForSelectedAgentValidation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()), General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));
            if (dsvendor.Rows.Count > 0)
            {
                RepAgents.DataSource = dsvendor;
                //RepAgents.DataBind();

                ViewState["AGENTID"] = dsvendor.Rows[0]["FLDAGENTID"].ToString(); 
                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                txtSubject.Text = "PO for " + dsvendor.Rows[0]["FLDREQUISITIONNO"].ToString() + " - " + vsl.ToString();

                emailbodytext = PrepareEmailBodyTextForTicketdefault(new Guid(ViewState["QUOTEID"].ToString()), dsvendor.Rows[0]["FLDREQUISITIONNO"].ToString(), dsvendor.Rows[0]["FLDAGENTID"].ToString()
                                            ,dsvendor.Rows[0]["FLDNAME"].ToString(), dsvendor.Rows[0]["FLDURL"].ToString(),
                                        dsvendor.Rows[0]["FLDUSERNAME"].ToString());
                edtBody.Content = emailbodytext.ToString();
            }
            else
            {
                ucError.ErrorMessage = "Email already sent";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendForCancelTravelRequest()
    {
        ViewState["EMPLOYEENAME"] = null;
        ViewState["USERNAME"] = null;

        string emailbodytext = "";
        string selectedvendors = ",";
        selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
        DataSet dsvendor = PhoenixCrewTravelRequest.ListCrewTravelSelectedVendors(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELREQUESTID"] == null ? null : ViewState["TRAVELREQUESTID"].ToString()));
        if (dsvendor.Tables[0].Rows.Count > 0)
        {
            RepAgents.DataSource = dsvendor.Tables[0];
            //RepAgents.DataBind();                        

            ViewState["EMPLOYEENAME"] = dsvendor.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString();
            ViewState["USERNAME"] = dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString();
            ViewState["ISTICKETYN"] = dsvendor.Tables[0].Rows[0]["FLDISTICKETYN"].ToString();
            ViewState["TICKETLIST"] = dsvendor.Tables[0].Rows[0]["FLDTICKETLIST"].ToString();
            ViewState["VESSELNAME"] = dsvendor.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();

            txtSubject.Text = "TCTR for " + dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " - " + ViewState["VESSELNAME"].ToString();
            emailbodytext = PrepareEmailBodyTextForCancelRequest(new Guid(dsvendor.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString()), dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                  dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString(),
                                  dsvendor.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString(), 
                                  dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString(), 
                                  dsvendor.Tables[0].Rows[0]["FLDISTICKETYN"].ToString(),
                                  dsvendor.Tables[0].Rows[0]["FLDTICKETLIST"].ToString());
            edtBody.Content = emailbodytext.ToString();
        }
        else
        {
            ucError.ErrorMessage = "Email already sent";
            ucError.Visible = true;
            return;
        }
    }
    protected string PrepareEmailBodyTextForCancelRequest(Guid travelagentid, string formno, string agentname, string employeename, string username, string isticketYN,string ticketlist)
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append(agentname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        if (Int16.Parse(isticketYN) == 0)
        {
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " informs you that Mr." + employeename + " travel plan is cancelled .");
        }
        else if (Int16.Parse(isticketYN) == 1)
        {
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " informs you to cancel the ticket of Mr." + employeename + " ." + "and Ticket No: " + ticketlist );
        }
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();
    }
    protected string PrepareEmailBodyTextdefault(Guid travelagentid, string formno, string agentname, string pageto, string username)
    {        
        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.Append(agentname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the following passengers to travel.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated,");
        sbemailbody.AppendLine("<br/>");
        string url = Session["sitepath"] + "/Portal/PortalDefault.aspx";
        sbemailbody.AppendLine("<a href=" + url +  " >" + url + "</a>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");

        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();      

    }
    protected string PrepareEmailBodyText(Guid travelagentid, string formno)
    {
        
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewTravelQuote.GetQuotationDataForEmailBody(travelagentid, "TRFQ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the following passengers to travel.");
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
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
        }
        return sbemailbody.ToString();
        
    }
    protected string PrepareEmailBodyTextForRemainder(Guid travelagentid, string formno)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewTravelQuote.GetQuotationDataForEmailBody(travelagentid, "TRFQ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Reminder: Awaiting for your Quotation.<br/>");
            sbemailbody.AppendLine("Reply as soon as possible");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("----------------------------------------------------------------------------------------------------<br/>");
            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the following passengers to travel.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Please click on the link below and key in the relevant fields indicated");
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
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"] );
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
        }
        return sbemailbody.ToString();

    }

    protected string PrepareEmailBodyTextForTicketdefault(Guid travelid, string formno, string agentid,string agentname,string pageto,string username)
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append(agentname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide ticket number.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated");
        sbemailbody.AppendLine("<br/>");
        string url = Session["sitepath"] + "/Portal/PortalDefault.aspx";
        sbemailbody.AppendLine("<a href=" + url + " >" + url + "</a>");
        
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");

        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username );
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();

    }
    protected string PrepareEmailBodyTextForTicket(Guid travelid, string formno)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewTravelQuote.GetTicketDataForEmailBody(travelid, "TPO");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide ticket number.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Please click on the link below and key in the relevant fields indicated");
            sbemailbody.AppendLine("<br/>");
            string url = Session["sitepath"] + "/Portal/PortalDefault.aspx";
            sbemailbody.AppendLine("<a href=" + url + " >" + url + "</a>");

            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");

            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");

        }
        return sbemailbody.ToString();

    }

    private void SendRFQ()
    {
        try
        {
            string emailbodytext = "";
            StringBuilder strname = new StringBuilder();
            int count = 1;
            foreach (GridDataItem gvr in RepAgents.MasterTableView.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;
                strname.Append("<br/>&nbsp;&nbsp;&nbsp;" + count++ + ".&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text);

                if (IsValidateEmail(mailto, mailcc))
                {                    
                    PhoenixCrewTravelQuote.InsertWebSession(new Guid(Trvelagentid.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "TRFQ", PhoenixCrewTravelRequest.RequestNo);
                    emailbodytext = PrepareEmailBodyText(new Guid(Trvelagentid.ToString()), reqno);
                    PhoenixMail.SendMail(mailto.Trim(), mailcc.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }

            }
            PhoenixCrewTravelQuote.ListCrewTravelQuotationToSendUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"] == null ? null : ViewState["TRAVELID"].ToString()),
                                    ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString(), Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));

            Session["emailsend"] = strname;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SendCTR()
    {
        try
        {
            string emailbodytext = "";
            StringBuilder strname = new StringBuilder();
            int count = 1;
            foreach (GridDataItem gvr in RepAgents.MasterTableView.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;
                string agentname = ((RadTextBox)gvr.FindControl("txtname")).Text;
                strname.Append("<br/>&nbsp;&nbsp;&nbsp;" + count++ + ".&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text);

                if (IsValidateEmail(mailto, mailcc))
                {                 
                        string vsl = "";
                        if (Filter.CurrentTraveltoVesselName != null)
                            vsl = Filter.CurrentTraveltoVesselName.ToString();
                        emailbodytext = PrepareEmailBodyTextForCancelRequest(new Guid(Trvelagentid.ToString()), reqno, agentname, ViewState["EMPLOYEENAME"].ToString(), ViewState["USERNAME"].ToString(), ViewState["ISTICKETYN"].ToString(), ViewState["TICKETLIST"].ToString());
                        PhoenixMail.SendMail(mailto.Trim(), mailcc.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }

            }

            Session["emailsend"] = strname;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendRFQReminder()
    {
        try
        {
            string emailbodytext = "";
            StringBuilder strname = new StringBuilder();
            int count = 1;
            foreach (GridDataItem gvr in RepAgents.MasterTableView.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;
                strname.Append("<br/>&nbsp;&nbsp;&nbsp;" + count++ + ".&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text);

                if (IsValidateEmail(mailto, mailcc))
                {
                    emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(Trvelagentid.ToString()),reqno );
                    PhoenixMail.SendMail(mailto.Trim(), mailcc.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
                }
                else
                {
                    //ucError.Visible = true;
                    //return;
                }

            }

            Session["emailsend"] = strname;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SendOrderTicket()
    {
        try
        {
            string emailbodytext = "";
            foreach (GridDataItem gvr in RepAgents.MasterTableView.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;
                string strname ="<br/>&nbsp;&nbsp;&nbsp;1.&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text;

                if (IsValidateEmail(mailto, mailcc))
                {
                    PhoenixCrewTravelQuote.InsertWebSession(new Guid(ViewState["TRAVELID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "TPO", reqno);
                    emailbodytext = PrepareEmailBodyTextForTicket(new Guid(ViewState["TRAVELID"].ToString()), reqno);
                  //  PhoenixCrewTravelQuoteLine.CrewTravelBreakupInsertForTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));
                  //  PhoenixCrewTravelQuoteLine.CrewTravelUnallocatedVesselExpenseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));
                    PhoenixCrewTravelQuote.SendPOStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()));
                    PhoenixMail.SendMail(mailto.Trim(), mailcc.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");                    
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
                Session["emailsend"] = strname;
            }            

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RepAgents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["purpose"].ToString().ToUpper().Equals("RFQ"))
        {
            SendForTravelQuotation();
        }
        else if (ViewState["purpose"].ToString().ToUpper().Equals("RFQREMAINDER"))
        {
            SendReminderForQuotation();
        }
        else if (ViewState["purpose"].ToString().ToUpper().Equals("POORDER"))
        {
            SendForOrderTicket();
        }
        else if (ViewState["purpose"].ToString().ToUpper().Equals("CTR"))
        {
            SendForCancelTravelRequest();
        }
        else
        {
            SendForTravelQuotation();
        }

    }

    protected void RepAgents_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //if (RepAgents.MasterTableView.Items.Count > 0)
            //{
                RadLabel lbl = e.Item.FindControl("lblSNo") as RadLabel;
                lbl.Text = (e.Item.ItemIndex + 1).ToString();
            //}
        }
    }

    protected void RemoveEditorToolBarIcons()
    {
        edtBody.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        edtBody.EnsureToolsFileLoaded();
        edtBody.Modules.Remove("RadEditorHtmlInspector");
        edtBody.Modules.Remove("RadEditorNodeInspector");
        edtBody.Modules.Remove("RadEditorDomInspector");
        edtBody.Modules.Remove("RadEditorStatistics");

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in edtBody.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }
}
