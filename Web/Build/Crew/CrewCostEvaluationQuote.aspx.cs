using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewCostEvaluationQuote : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["REQUESTID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["EVALUATIONPORTID"] = "";
            ViewState["CURRENTCITYID"] = "";
            ViewState["COMPLETEDYN"] = "";

            if (Request.QueryString["REQUESTID"] != null)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
            }
            if (Request.QueryString["EVALUATIONPORTID"] != null)
            {
                ViewState["EVALUATIONPORTID"] = Request.QueryString["EVALUATIONPORTID"].ToString();
            }
            if (Request.QueryString["CURRENTCITYID"] != null)
            {
                ViewState["CURRENTCITYID"] = Request.QueryString["CURRENTCITYID"].ToString();
            }
        }
        BindQuoteDetails();
        BindAirfare();
        BindGridMenu();
    }
    private void BindGridMenu()
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddFontAwesomeButton("../Crew/CrewCostEvaluationQuote.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvQuote')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewCostPortAgentAdd.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&EVALUATIONPORTID=" + ViewState["EVALUATIONPORTID"].ToString() + "');return false;", "Add Agent", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbarsub.AddFontAwesomeButton("../Crew/CrewCostEvaluationQuote.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
        toolbarsub.AddFontAwesomeButton("../Crew/CrewCostEvaluationQuote.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");

        MenuAgentList.AccessRights = this.ViewState;
        MenuAgentList.MenuList = toolbarsub.Show();
    }


    protected void MenuAgentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                SendForQuotation();
            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExcel()
    {
        DataTable dt = new DataTable();

        dt = PhoenixCrewCostEvaluationQuote.QuotationListByPort(new Guid(ViewState["REQUESTID"].ToString())
        , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString()));

        string[] alColumns = { "FLDAGENTNAME", "FLDQUOTEREFNO", "FLDSENTDATE", "FLDRECEIVEDDATE", "FLDCURRENCYCODE", "FLDTOTALAMOUNT", "FLDTOTALUSDAMOUNT" };
        string[] alCaptions = { "Port Agent", "Quotation No.", "Sent Date", "Received Date", "Currency", "Total Amount", "Total Amount in USD" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        General.ShowExcel("Port Agents", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindQuoteDetails()
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewCostEvaluationQuote.QuotationListByPort(new Guid(ViewState["REQUESTID"].ToString())
        , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString()));

        string[] alColumns = { "FLDAGENTNAME", "FLDQUOTEREFNO", "FLDSENTDATE", "FLDRECEIVEDDATE", "FLDCURRENCYCODE", "FLDTOTALAMOUNT", "FLDTOTALUSDAMOUNT" };
        string[] alCaptions = { "Port Agent", "Quotation No.", "Sent Date", "Received Date", "Currency", "Total Amount", "Total Amount in USD" };

        DataTable dtc = new DataTable();
        dtc = dt.Copy();
        DataSet ds = new DataSet();
        ds.Tables.Add(dtc);
        General.SetPrintOptions("gvQuote", "Port Agents", alCaptions, alColumns, ds);

        gvQuote.DataSource = dt;

    }

    protected void gvQuote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindQuoteDetails();
    }


    protected void gvQuote_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton com = (LinkButton)e.Item.FindControl("cmdCommunication");
            
            RadLabel lblAgentId = (RadLabel)e.Item.FindControl("lblAgentId");
            RadLabel lblAgentName = (RadLabel)e.Item.FindControl("lblAgentName");
            RadLabel lblQuoteId = (RadLabel)e.Item.FindControl("lblQuoteId");
            RadLabel lblAmount = (RadLabel)e.Item.FindControl("lblAmount");
            RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");

            LinkButton imgappove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton imgdeapprove = (LinkButton)e.Item.FindControl("cmdDeApprove");
            LinkButton imgapproved = (LinkButton)e.Item.FindControl("cmdApproved");
            LinkButton imgflag = (LinkButton)e.Item.FindControl("imgflag");

            if (imgappove != null) imgappove.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to Approve this Agent and Port'); return false;");
            if (imgdeapprove != null) imgdeapprove.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to Revoke Approval'); return false;");

            if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
            {
                if (lblAmount != null)
                {
                    if (lblApprovedYN != null && lblApprovedYN.Text == "1")
                    {
                        if (imgappove != null) imgappove.Visible = false;
                        if (imgdeapprove != null) imgdeapprove.Visible = true;
                        imgflag.Visible = true;
                    }
                    else if (lblAmount.Text.Trim() != "")
                    {
                        if (Convert.ToDecimal(lblAmount.Text.Trim()) > 0)
                            if (imgappove != null) imgappove.Visible = true;
                    }
                }
            }
            else
            {
                if (imgappove != null) imgappove.Visible = false;
                if (imgdeapprove != null) imgdeapprove.Visible = false;
                imgflag.Visible = false;
            }
            if (Filter.CurrentMenuCodeSelection != "CRW-OPR-CCP")
            {
                if (imgapproved != null)
                {
                    if (lblApprovedYN != null && lblApprovedYN.Text == "1")
                    {
                        imgapproved.Visible = true;
                        imgflag.Visible = true;
                    }
                    else
                    {
                        imgapproved.Visible = false;
                        imgflag.Visible = false;
                    }
                }
                if (com != null)
                {
                    com.Visible = true;
                    com.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewCostEvaluationQuoteChat.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&AGENTID=" + lblAgentId.Text + "&QUOTEID=" + lblQuoteId.Text + "&AGENTNAMEOLY=1" + "&AGENTNAME=" + lblAgentName.Text.Replace('&', '~').ToString() + " - " + PhoenixCrewCostEvaluationRequest.RequestNumber + "&ISOFFICE=1" + "');return false;");
                }
            }
        }
    }

    protected void gvQuote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                ViewState["EVALUATIONPORTID"] = ((RadLabel)e.Item.FindControl("lblEvaluationPortId")).Text;

            }

            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;

                PhoenixCrewCostEvaluationQuote.CrewCostQuoteApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(quoteid));

                PhoenixCrewCostEvaluationRequest.CrewChangeRequestCompletionUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString()));

                ucError.ErrorMessage = "Agent and Port are approved";
                ucError.Visible = true;
                BindQuoteDetails();

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                BindQuoteDetails();
                gvQuote.Rebind();

            }
            if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
            {

                string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                PhoenixCrewCostEvaluationQuote.CrewCostQuoteDeApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(quoteid));

                ucError.ErrorMessage = "Approval is revoked";
                ucError.Visible = true;

                BindQuoteDetails();
                gvQuote.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindAirfare()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixCrewCostEvaluationRequest.ListCrewCostEvaluationAirefare(General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                , General.GetNullableGuid(ViewState["EVALUATIONPORTID"] == null ? "" : ViewState["EVALUATIONPORTID"].ToString()));

            gvCrewAirfare.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewAirfare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindAirfare();
    }

    protected void gvCrewAirfare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string fromcityidadd = ViewState["CURRENTCITYID"].ToString();
                string tocityidadd = ((UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdAdd")).SelectedValue;
                string joinerairfareadd = ((UserControlMaskNumber)e.Item.FindControl("txtJoinerAmountAdd")).Text;
                string offsignerairfareadd = ((UserControlMaskNumber)e.Item.FindControl("txtOffsignerAmountAdd")).Text;

                if (!IsValidAirfare(fromcityidadd, tocityidadd, joinerairfareadd, offsignerairfareadd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewCostEvaluationRequest.InsertCrewCostEvaluationAirfare(new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(ViewState["EVALUATIONPORTID"].ToString())
                    , int.Parse(fromcityidadd)
                    , int.Parse(tocityidadd)
                    , General.GetNullableDecimal(joinerairfareadd)
                    , General.GetNullableDecimal(offsignerairfareadd));

                BindAirfare();
                gvCrewAirfare.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void gvCrewAirfare_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string fromcityidedit = ViewState["CURRENTCITYID"].ToString();

            string tocityidedit = ((UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdEdit")).SelectedValue;
            string joinerairfareedit = ((UserControlMaskNumber)e.Item.FindControl("txtJoinerAmountEdit")).Text;
            string offsignerairfareedit = ((UserControlMaskNumber)e.Item.FindControl("txtOffsignerAmountEdit")).Text;
            string airfareId = ((RadLabel)e.Item.FindControl("lblCostAirfareIdEdit")).Text;

            if (!IsValidAirfare(fromcityidedit, tocityidedit, joinerairfareedit, offsignerairfareedit))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewCostEvaluationRequest.UpdateCrewCostEvaluationAirfare(new Guid(ViewState["REQUESTID"].ToString())
                , new Guid(airfareId)
                , int.Parse(tocityidedit)
                , General.GetNullableDecimal(joinerairfareedit)
                , General.GetNullableDecimal(offsignerairfareedit));

            BindAirfare();
            gvCrewAirfare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewAirfare_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblCostAirfareId = (RadLabel)e.Item.FindControl("lblCostAirfareId");

            if (lblCostAirfareId != null)
            {
                PhoenixCrewCostEvaluationRequest.DeleteCrewCostEvaluationAirfare(new Guid(lblCostAirfareId.Text.ToString()));
            }
            BindAirfare();
            gvCrewAirfare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewAirfare_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlMultiColumnCity ucToCity = (UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdEdit");
            if (ucToCity != null)
            {
                ucToCity.SelectedValue = drv["FLDTOCITYID"].ToString();
                ucToCity.Text = drv["FLDTOCITYNAME"].ToString();
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

    private void SendForQuotation()
    {
        try
        {
            string emailid;
            string selectedagents = ",";
            foreach (GridDataItem gvr in gvQuote.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblAgentId"))).Text + ",";
                }
            }
            if (selectedagents.Length <= 1)
                selectedagents = null;

            DataSet dsagents = PhoenixCrewCostEvaluationQuote.ListCrewCostQuoteToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["REQUESTID"].ToString())
                , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString())
                , selectedagents
                );

            if (dsagents.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsagents.Tables[0].Rows)
                {
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
                            PhoenixCrewCostEvaluationQuote.InsertWebSession(new Guid(dr["FLDQUOTEID"].ToString())
                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , "CRFQ"
                                , dr["FLDREQUESTNO"].ToString());

                            emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDQUOTEID"].ToString()), dr["FLDREQUESTNO"].ToString(), dr["FLDFROMEMAIL"].ToString());

                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "CRFQ", new Guid(dr["FLDQUOTEID"].ToString())
                                , ""
                                , dr["FLDFROMEMAIL"].ToString()
                                , dr["FLDNAME"].ToString()
                                , "RFQ for " + dr["FLDREQUESTNO"].ToString() + ""
                                , emailbodytext
                                , ""
                                , ""
                                );

                            PhoenixCrewCostEvaluationQuote.UpdateCrewCostQuoteSendDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                                , General.GetNullableGuid(dr["FLDQUOTEID"].ToString())
                                );

                        }
                        ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
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

    private void SendReminderForQuotation()
    {
        string emailid;
        try
        {
            string selectedagents = ",";
            foreach (GridDataItem gvr in gvQuote.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblQuoteId"))).Text + ",";
                }
            }
            if (selectedagents.Length <= 1)
                selectedagents = null;

            DataSet dsagents = PhoenixCrewCostEvaluationQuote.ListCrewCostQuoteToSendReminder(new Guid(ViewState["REQUESTID"].ToString())
                , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString())
                , selectedagents);

            if (dsagents.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsagents.Tables[0].Rows)
                {
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
                            PhoenixCrewCostEvaluationQuote.InsertWebSession(new Guid(dr["FLDQUOTEID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "CRFQ", dr["FLDREQUESTNO"].ToString());
                            emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dr["FLDQUOTEID"].ToString()), dr["FLDREQUESTNO"].ToString());
                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "CRFQ", new Guid(dr["FLDQUOTEID"].ToString())
                                , ""
                                , dr["FLDFROMEMAIL"].ToString()
                                , dr["FLDNAME"].ToString()
                                , "CRFQ Reminder for " + dr["FLDREQUESTNO"].ToString() + ""
                                , emailbodytext
                                , ""
                                , ""
                                );
                        }
                        ucConfirm.ErrorMessage = "Reminder email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                ucConfirm.Visible = true;
            }
            else
            {
                ucConfirm.ErrorMessage = "There are no agents to whome you need to send a reminder. All the agents have quoted";
                ucConfirm.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected string PrepareEmailBodyText(Guid quoteid, string requestno, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewCostEvaluationQuote.GetQuoteDataForEmailBody(quoteid, "CRFQ");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "     " + requestno);
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] +" hereby requests you to provide your BEST quotation for the port of." + dr["FLDEVALUATIONPORT"].ToString());
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
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
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
    protected string PrepareEmailBodyTextForRemainder(Guid quoteid, string formno)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewCostEvaluationQuote.GetQuoteDataForEmailBody(quoteid, "CRFQ");
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
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine();
            sbemailbody.Append("--------------------------------------------------------");
            sbemailbody.Append(dr["FLDAGENTNAME"].ToString() + "           " + formno);
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"]+" hereby requests you to provide your BEST quotation for the port of." + dr["FLDEVALUATIONPORT"].ToString());
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
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine();
        }
        return sbemailbody.ToString();

    }
    private bool IsvalidMaild(string frommailid, string tomailid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.IsvalidEmail(frommailid))
        {
            ucError.ErrorMessage = "From Mail id Required";
        }
        if (!General.IsvalidEmail(tomailid))
        {
            ucError.ErrorMessage = "To Mail id Required";
        }

        return (!ucError.IsError);
    }
    private bool IsValidAirfare(string fromcityid, string tocityid, string joinerairfare, string offsignerairfare)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(fromcityid) == null)
            ucError.ErrorMessage = "From Port city is required.";

        if (General.GetNullableInteger(tocityid) == null)
            ucError.ErrorMessage = "To city is required.";

        if (General.GetNullableInteger(fromcityid) != null && General.GetNullableInteger(tocityid) != null)
        {
            if (int.Parse(fromcityid) == int.Parse(tocityid))
                ucError.ErrorMessage = "From and To city cannot be same.";
        }

        if (General.GetNullableDecimal(joinerairfare) == null)
            ucError.ErrorMessage = "OnSigners Amount is required.";

        if (General.GetNullableDecimal(offsignerairfare) == null)
            ucError.ErrorMessage = "Offsigners Amount is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {

    }
    protected void cmdHiddenPick_Click(object sender, EventArgs args)
    {

    }


}
