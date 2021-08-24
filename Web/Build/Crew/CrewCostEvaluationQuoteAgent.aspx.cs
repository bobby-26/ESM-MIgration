using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCostEvaluationQuoteAgent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
           

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                PhoenixToolbar toolbar = new PhoenixToolbar();
                PhoenixToolbar toolbar1 = new PhoenixToolbar();

                toolbar.AddButton("Request", "EVALUATIONREQUEST");
                toolbar.AddButton("Details", "REQUESTDETAILS");
                toolbar.AddButton("Quotation", "QUOTATION");
                toolbar.AddButton("Crew Change Form", "CREWCHANGEFORM");
                toolbar.AddButton("Port Cost Analysis", "ANALYSYS");
                if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
                {
                    toolbar.AddButton("Vessel List", "VESSELLIST");
                }


                toolbar1.AddButton("Send Mail", "SENDMAIL");

                MenuAgent.AccessRights = this.ViewState;
                MenuAgent.MenuList = toolbar.Show();
                MenuAgent.SelectedMenuIndex = 2;

                MenuCrewCostQuoteSub.AccessRights = this.ViewState;
                MenuCrewCostQuoteSub.MenuList = toolbar1.Show();

                ViewState["REQUESTNO"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PORTAGENTID"] = null;

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (Request.QueryString["requestid"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["requestid"];
                SetInformation();

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddImageButton("../Crew/CrewCostEvaluationQuoteAgent.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarsub.AddImageLink("javascript:CallPrint('gvAgent')", "Print Grid", "icon_print.png", "PRINT");
                toolbarsub.AddImageLink("javascript:Openpopup('Filter','','../Crew/CrewCostPortAgentAdd.aspx?requestid=" + ViewState["REQUESTID"].ToString() + "');return false;", "Add Agent", "add.png", "ADD");
                toolbarsub.AddImageButton("../Crew/CrewCostEvaluationQuoteAgent.aspx", "Send Query", "Email.png", "RFQ");
                toolbarsub.AddImageButton("../Crew/CrewCostEvaluationQuoteAgent.aspx", "Send Reminder", "remainder-mail.png", "RFQREMAINDER");
                toolbarsub.AddImageButton("../Crew/CrewCostEvaluationQuoteAgent.aspx", "Compare Quotations", "query.png", "QTNCOMPARE");

                MenuAgentList.AccessRights = this.ViewState;
                MenuAgentList.MenuList = toolbarsub.Show();
                MenuAgentList.SetTrigger(pnlQuoteAgentList);

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewCostEvaluationQuoteLineItem.aspx?requestid=" + ViewState["REQUESTID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewCostEvaluationQuoteLineItem.aspx?requestid=" + ViewState["REQUESTID"].ToString();
                }
            }
            BindData();
            Guidlines();
            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetInformation()
    {
        Title1.Text = Title1.Text = "Quotation Details(" + PhoenixCrewCostEvaluationRequest.RequestNumber + ")";
    }
    private void BindData()
    {
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };

        string requestid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["requestid"] != null)
            requestid = ViewState["REQUESTID"].ToString();
        DataTable dt = PhoenixCrewCostEvaluationQuote.PortAgentSearch(new Guid(ViewState["REQUESTID"].ToString())
        , General.GetNullableString(sortexpression)
        , sortdirection);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAgent", "Port Agents", alCaptions, alColumns, ds);
        if (dt.Rows.Count > 0)
        {
            gvAgent.DataSource = dt;
            gvAgent.DataBind();
            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewCostEvaluationQuoteLineItem.aspx?requestid=" + ViewState["REQUESTID"];


            if (ViewState["PORTAGENTID"] == null)
            {
                ViewState["PORTAGENTID"] = dt.Rows[0]["FLDPORTAGENTID"].ToString();
                PhoenixCrewTravelQuote.Agent = dt.Rows[0]["FLDAGENTNAME"].ToString();
                gvAgent.SelectedIndex = 0;
            }
            SetQuote();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAgent);
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
        }
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
    private void Guidlines()
    {
        ucToolTipNW.Text = "<table> <tr><td> &nbsp;1. To add agent click on &nbsp;<img id=\"img2\" runat=\"server\" src=" + Session["images"] + "/add.png style=\"vertical-align: top\" />&nbsp; button on the port agents. </td> </tr> <tr><td>&nbsp;2. To send quotation to the agent,select the agents in the quotation details and click on &nbsp;<img id=\"img3\" runat=\"server\" src=" + Session["images"] + "/Email.png style=\"vertical-align: top\" >&nbsp; button on the port agents.</td> </tr> <tr><td>&nbsp;3. To send reminder to the agent,select the agents in the quotation details and click on &nbsp;<img id=\"img4\" runat=\"server\" src=" + Session["images"] + "/remainder-mail.png style=\"vertical-align: top\" />&nbsp; button on the port agents.</td> </tr><tr><td>&nbsp;4. To compare the quotations,select the agents in the quotation details and click on &nbsp;<img id=\"img5\" runat=\"server\" src=" + Session["images"] + "/query.png style=\"vertical-align: top\" />&nbsp; button on the port agents.</td> </tr></tr><tr><td>&nbsp;6. To remove the agent click on &nbsp;<img id=\"img5\" runat=\"server\" src=" + Session["images"] + "/te_del.png style=\"vertical-align: top\" />&nbsp; button on the port agents.</td> </tr></tr></table>";
        imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();

            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage = "E-mail sent to ,<BR/>" + Session["emailsend"].ToString();
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
    protected void MenuAgentList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("RFQ"))
            {
                SendForQuotation();
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();

                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("QTNCOMPARE"))
            {
                if (ViewState["REQUESTID"] != null)
                {
                    string selectedagents = ",";
                    foreach (GridViewRow gvr in gvAgent.Rows)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                        {
                            selectedagents = selectedagents + ((Label)(gvr.FindControl("lblPortAgentId"))).Text + ",";
                        }
                    }

                    if (selectedagents.Length > 1)
                        Response.Redirect("CrewCostEvaluationQuoteCompare.aspx?agents=" + selectedagents + "&requestid=" + ViewState["REQUESTID"].ToString());

                    else
                    {
                        ucError.ErrorMessage = "There are no quotations to compare.";
                        ucError.Visible = true;
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
    protected void MenuCrewCostQuoteSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SENDMAIL"))
            {
                SendQuoteReceivedMail();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAgent_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            string requestid = null;

            if (ViewState["REQUESTID"] != null)
                requestid = ViewState["REQUESTID"].ToString();

            if (dce.CommandName.ToUpper().Equals("EVALUATIONREQUEST"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationRequest.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("REQUESTDETAILS"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationRequestGeneral.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("CREWCHANGEFORM"))
            {
                Response.Redirect("CrewChangeCostForm.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("ANALYSYS"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationFinalPortCostAnalysis.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("VESSELLIST"))
            {
                Response.Redirect("CrewChangePlanFilter.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
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
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };

        string requestid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["requestid"] != null)
            requestid = ViewState["REQUESTID"].ToString();
        DataTable dt = PhoenixCrewCostEvaluationQuote.PortAgentSearch(new Guid(ViewState["REQUESTID"].ToString())
        , General.GetNullableString(sortexpression)
        , sortdirection);

        Response.AddHeader("Content-Disposition", "attachment; filename=Port Agents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Port Agents</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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
    private void SetQuote()
    {
        if (ViewState["SETCURRENTNAVIGATIONTAB"] != null)
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() +
                "&portagentid=" + ViewState["PORTAGENTID"].ToString();
        }
    }
    private void SendForQuotation()
    {
        //try
        //{
        //    string selectedagnet = ",";
        //    foreach (GridViewRow gvr in gvAgent.Rows)
        //    {
        //        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
        //        {
        //            selectedagnet = ((Label)(gvr.FindControl("lblPortAgentId"))).Text;

        //            SendRFQMailToAgent(selectedagnet);

        //            PhoenixCrewCostEvaluationQuote.ListCrewCostQuoteToSendUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //              , new Guid(ViewState["REQUESTID"].ToString())
        //              , General.GetNullableGuid(selectedagnet));
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    private void SendRFQMailToAgent(string selectedagnet)
    {
        //string emailbodytext = "";
        //DataSet dsagent = PhoenixCrewCostEvaluationQuote.ListCrewCostQuoteToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //    , new Guid(ViewState["REQUESTID"].ToString())
        //    , General.GetNullableGuid(selectedagnet));

        //DataRow dr = dsagent.Tables[0].Rows[0];

        //PhoenixCrewCostEvaluationQuote.InsertWebSession(new Guid(selectedagnet)
        //                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                , "CRFQ"
        //                , PhoenixCrewCostEvaluationRequest.RequestNumber);

        //emailbodytext = PrepareEmailBodyText(new Guid(selectedagnet)
        //        , dr["FLDREQUESTNO"].ToString()
        //        , dr["FLDFROMEMAIL"].ToString());

        //PhoenixCommoneProcessing.PrepareEmailMessage(
        //    dr["FLDEMAIL1"].ToString(), "CRFQ", new Guid(selectedagnet)
        //    , ""
        //    , dr["FLDFROMEMAIL"].ToString()
        //    , dr["FLDNAME"].ToString()
        //    , "CRFQ for " + dsagent.Tables[0].Rows[0]["FLDREQUESTNO"].ToString() + ""
        //    , emailbodytext
        //    , ""
        //    , ""
        //    );
        //ucConfirm.Visible = true;
        //ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
    }

    private void SendRFQReminderMailToAgent(string selectedagnet)
    {
        //string emailbodytext = "";

        //DataSet dsagent = PhoenixCrewCostEvaluationQuote.ListQuoteToSendReminder(new Guid(ViewState["REQUESTID"].ToString())
        //, General.GetNullableGuid(selectedagnet));

        //DataRow dr = dsagent.Tables[0].Rows[0];
        //emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(selectedagnet)
        //    , dr["FLDREQUESTNO"].ToString());


        //PhoenixCommoneProcessing.PrepareEmailMessage(
        //    dr["FLDEMAIL1"].ToString(), "CRFQ", new Guid(selectedagnet)
        //    , ""
        //    , dr["FLDFROMEMAIL"].ToString()
        //    , dr["FLDNAME"].ToString()
        //    , "CRFQ for " + dr["FLDREQUESTNO"].ToString() + ""
        //    , emailbodytext
        //    , ""
        //    , ""
        //    );
        //ucConfirm.Visible = true;
        //ucConfirm.ErrorMessage = "Reminder sent to " + dr["FLDNAME"].ToString() + "\n";
    }

    private void SendReminderForQuotation()
    {
        //try
        //{
        //    string selectedagnet = "";
        //    foreach (GridViewRow gvr in gvAgent.Rows)
        //    {
        //        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
        //        {
        //            selectedagnet = ((Label)(gvr.FindControl("lblPortAgentId"))).Text;
        //            SendRFQReminderMailToAgent(selectedagnet);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected string PrepareEmailBodyText(Guid portagentid, string requestno, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewCostEvaluationQuote.GetQuoteDataForEmailBody(portagentid, "CRFQ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "     " + requestno);
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the port of." + dr["FLDEVALUATIONPORTS"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.Append("Please click on the link below and key in the relevant fields indicated,");
            sbemailbody.AppendLine();
            string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + ">\"");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine();
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"] );
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine();

        }
        return sbemailbody.ToString();

    }
    protected string PrepareEmailBodyTextForQuote(string requestno, string sendername, string receivername, string vesselname)
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.AppendLine("Good Day " + sendername);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Crew Change Cost Quotation Received for the Vessel " + vesselname + " for the request no. " + requestno);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(sendername);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(); 
       
        return sbemailbody.ToString();
    }
    private void SendQuoteReceivedMail()
    {
        try
        {
            string emailbodytext = "";

            DataTable dt = PhoenixCrewCostEvaluationQuote.CrewCostQuoteReceivedMailDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["REQUESTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string frmmailid = "";
                string tomailid = "";

                frmmailid = dr["FLDFROMMAILID"].ToString();
                tomailid = dr["FLDTOMAILID"].ToString();

                if (!IsvalidMaild(frmmailid, tomailid))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    emailbodytext = PrepareEmailBodyTextForQuote(dr["FLDREQUESTNO"].ToString()
                        , dr["FLDSENDERNAME"].ToString()
                        , dr["FLDRECEIVERNAME"].ToString()
                        , dr["FLDVESSELNAME"].ToString()
                        );

                    PhoenixMail.SendMail(tomailid.Trim()
                      , frmmailid.Trim()
                      , ""
                      , "Crew Cost Quote for " + dr["FLDVESSELNAME"].ToString() + " - " + dr["FLDREQUESTNO"].ToString()
                      , emailbodytext.ToString(), false
                      , System.Net.Mail.MailPriority.Normal
                      , ""
                      , null);

                    ucError.Visible = true;
                    ucError.ErrorMessage = "Mail sent";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string PrepareEmailBodyTextForRemainder(Guid portagentid, string formno)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewCostEvaluationQuote.GetQuoteDataForEmailBody(portagentid, "CRFQ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "    " + formno);
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Reminder: Awaiting for your Quotation.");
            sbemailbody.AppendLine("Reply as soon as possible");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine();
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"] );
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine();
            sbemailbody.Append("--------------------------------------------------------");
            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "           " + formno);
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the port of." + dr["FLDEVALUATIONPORTS"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.Append("Please click on the link below and key in the relevant fields indicated");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + ">\"");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine();
            sbemailbody.Append(dr["FLDUSERNAME"].ToString());
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"] );
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine();
        }
        return sbemailbody.ToString();

    }
    protected void gvAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            string portagentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPortAgentId")).Text;
            PhoenixCrewCostEvaluationQuote.DeletePortAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["REQUESTID"].ToString())
                , new Guid(portagentid));
            ViewState["PORTAGENTID"] = null;
            BindData();
        }
        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["PORTAGENTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPortAgentId")).Text;
            ViewState["AGENTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAgentID")).Text;
            SetQuote();
        }
    }
    protected void gvAgent_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAgent.EditIndex = -1;
        gvAgent.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvAgent_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();

    }
    protected void gvAgent_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnPickAgent");
            TextBox txtagentname = (TextBox)e.Row.FindControl("txtAgentName");
            TextBox txtagentcode = (TextBox)e.Row.FindControl("txtAgentCode");
            TextBox txtagentid = (TextBox)e.Row.FindControl("txtAgentID");
            if (txtagentid != null) txtagentid.Attributes.Add("style", "visibility:hidden");

            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?producttype=63&framename=filterandsearch', true);");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            Label lbl = (Label)e.Row.FindControl("lblPortAgentId");
            Label lb2 = (Label)e.Row.FindControl("lblAgentId");
            LinkButton lbn = (LinkButton)e.Row.FindControl("lnkAgentName");

            Label lblSendDate = (Label)e.Row.FindControl("lblSendDate");
            Label lblRecievedDate = (Label)e.Row.FindControl("lblRecievedDate");

            if (lblSendDate != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblSendDate.Text);
                if (dt != null)
                {
                    lblSendDate.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblSendDate.Visible = true;
                }
            }

            if (lblRecievedDate != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblRecievedDate.Text);
                if (dt != null)
                {
                    lblRecievedDate.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblRecievedDate.Visible = true;
                }
            }
        }

    }
    
    private bool IsvalidMaild(string frommailid, string tomailid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.IsvalidEmail(frommailid))
        {
            ucError.ErrorMessage = "From Mailid Required";
        }
        if (!General.IsvalidEmail(tomailid))
        {
            ucError.ErrorMessage = "To Mailid Required";
        }

        return (!ucError.IsError);
    }
}
