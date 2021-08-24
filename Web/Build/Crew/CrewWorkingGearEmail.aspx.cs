using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
using System.Web;

public partial class CrewWorkingGearEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();            
            toolbarmain.AddButton("Send", "SEND", ToolBarDirection.Right);
        
            EmailMenu.AccessRights = this.ViewState;
            EmailMenu.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["purpose"] = null;
                ViewState["EMPLOYEENAME"] = null;
                ViewState["USERNAME"] = null;
                ViewState["ISTICKETYN"] = null;
                ViewState["TICKETLIST"] = null;
                ViewState["VESSELNAME"] = null;

                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor
                
                if (Request.QueryString["Neededid"] != null && Request.QueryString["selectedagent"] != null && Request.QueryString["purpose"].ToUpper().Equals("RFQ"))
                {
                    //Title1.Text = "Send Query";
                    ViewState["NEEDEDID"] = Request.QueryString["Neededid"].ToString();
                    ViewState["selectedagent"] = Request.QueryString["selectedagent"] == null ? null : Request.QueryString["selectedagent"].ToString();
                    ViewState["purpose"] = Request.QueryString["purpose"];
                }
                if (Request.QueryString["Neededid"] != null && Request.QueryString["selectedagent"] != null && Request.QueryString["purpose"].ToUpper().Equals("RFQREMAINDER"))
                    
                {
                    //Title1.Text = "Send Reminder";
                    ViewState["NEEDEDID"] = Request.QueryString["Neededid"].ToString();
                    ViewState["selectedagent"] = Request.QueryString["selectedagent"] == null ? null : Request.QueryString["selectedagent"].ToString();
                    ViewState["purpose"] = Request.QueryString["purpose"];
                }            
            }
            if(ViewState["purpose"].ToString().ToUpper().Equals("RFQ"))
            {
              
                 SendForWorkingGeareQuotation();
            }
            else if (ViewState["purpose"].ToString().ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();
            }
           
            else
            {
                SendForWorkingGeareQuotation();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

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
             
            }
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
    private void SendForWorkingGeareQuotation()
    {
        try
        {
            string emailbodytext = "";
            string selectedvendors = ",";
            selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
            
            DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["NEEDEDID"].ToString()), selectedvendors);
            
            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                RepAgents.DataSource = dsvendor.Tables[0];
                RepAgents.DataBind();

                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                txtSubject.Text = "WRFQ for " + dsvendor.Tables[0].Rows[0]["FLDREFNUMBER"].ToString() + " - " + vsl.ToString();
                emailbodytext = PrepareEmailBodyTextdefault(new Guid(dsvendor.Tables[0].Rows[0]["FLDNEEDEDID"].ToString())//, dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                        ,dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString()
                                        ,dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString());              
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
            
            DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendRemainder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["NEEDEDID"].ToString()), selectedvendors);
            
            if (dsvendor.Tables[0].Rows.Count > 0)
            {

                RepAgents.DataSource = dsvendor.Tables[0];
                RepAgents.DataBind();

                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();

                txtSubject.Text = "WRFQ for " + dsvendor.Tables[0].Rows[0]["FLDREFNUMBER"].ToString() + " - " + vsl.ToString();

                emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dsvendor.Tables[0].Rows[0]["FLDNEEDEDID"].ToString())//, dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                        , dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString()
                                        , dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString());                          
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
  
    protected string PrepareEmailBodyTextdefault(Guid travelagentid, string agentname, string username)
    {

        DataTable dt1 = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearNeeded(new Guid(ViewState["NEEDEDID"].ToString()));
        
        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.AppendLine("<br/>");        
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"]+" hereby requests you to provide your BEST quotation for the following Working gear items.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<table style=\"border-collapse: collapse;\">");
        sbemailbody.Append("<tr>");
        sbemailbody.Append("<td style=\"border: 1px solid black;\">");
        sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:16px;color:Black;\">");
        sbemailbody.Append("Working Gear");
        sbemailbody.Append("</td>");
        sbemailbody.Append("<td style=\"border: 1px solid black;\">");
        sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:16px;color:Black;\">");
        sbemailbody.Append("Required Quantity");
        sbemailbody.Append("</td>");
        sbemailbody.Append("</tr>");

        sbemailbody.Append("<tr>");
        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            
            sbemailbody.Append("<tr>");
            sbemailbody.Append("<td style=\"border: 1px solid black;\">");
            sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:15px;color:Black;\">");
            sbemailbody.Append(dt1.Rows[j]["FLDWORKINGGEARITEMNAME"].ToString());
            sbemailbody.Append("</td>");
            sbemailbody.Append("<td align='right' style=\"border: 1px solid black;\">");
            sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:15px;\">");
            sbemailbody.Append(dt1.Rows[j]["FLDQUANTITY"].ToString());
            sbemailbody.Append("</td>");
            sbemailbody.Append("</tr>");
            
            sbemailbody.AppendLine("<br/>");
        }
        sbemailbody.AppendLine("</table>");
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
   
    protected string PrepareEmailBodyTextForRemainder(Guid travelagentid, string agentname, string username)
    {
        StringBuilder sbemailbody = new StringBuilder();    
        DataTable dt1 = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearNeeded(new Guid(ViewState["NEEDEDID"].ToString()));

        if (dt1.Rows.Count > 0)
        {
            DataRow dr = dt1.Rows[0];
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Reminder: Awaiting for your Quotation.<br/>");
            sbemailbody.AppendLine("Reply as soon as possible");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thank you,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append(username);
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("----------------------------------------------------------------------------------------------------<br/>");
            sbemailbody.AppendLine("<br/>");            
            sbemailbody.Append("Dear Sir ,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] +" hereby requests you to provide your BEST quotation for the following Working gear items.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<table style=\"border-collapse: collapse;\">");
            sbemailbody.Append("<tr>");
            sbemailbody.Append("<td style=\"border: 1px solid black;\">");
            sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:16px;color:Black;\">");
            sbemailbody.Append("Working Gear");
            sbemailbody.Append("</td>");
            sbemailbody.Append("<td style=\"border: 1px solid black;\">");
            sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:16px;color:Black;\">");
            sbemailbody.Append("Required Quantity");
            sbemailbody.Append("</td>");
            sbemailbody.Append("</tr>");

            sbemailbody.Append("<tr>");
            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                sbemailbody.Append("<tr>");
                sbemailbody.Append("<td style=\"border: 1px solid black;\">");
                sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:15px;color:Black;\">");
                sbemailbody.Append(dt1.Rows[j]["FLDWORKINGGEARITEMNAME"].ToString());
                sbemailbody.Append("</td>");
                sbemailbody.Append("<td align='right' style=\"border: 1px solid black;\">");
                sbemailbody.Append("<font style=\"font-family:Calibri (Body);font-size:15px;\">");
                sbemailbody.Append(dt1.Rows[j]["FLDQUANTITY"].ToString());
                sbemailbody.Append("</td>");
                sbemailbody.Append("</tr>");

                sbemailbody.AppendLine("<br/>");
            }
            sbemailbody.AppendLine("</table>");
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
        }
        return sbemailbody.ToString();

    }

  
    private void SendRFQ()
    {
        try
        {
            string emailbodytext = "";
            string selectedvendors = ",";
            selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
            StringBuilder strname = new StringBuilder();
            int count = 1;
            foreach (GridDataItem gvr in RepAgents.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string agentid  = ((RadLabel)gvr.FindControl("lblagentid")).Text;
                string Neededid = ((RadLabel)gvr.FindControl("lblNeededid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;
                strname.Append("<br/>&nbsp;&nbsp;&nbsp;" + count++ + ".&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text);

                DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["NEEDEDID"].ToString()), selectedvendors);

                if (IsValidateEmail(mailto, mailcc))
                {                    
                    emailbodytext = PrepareEmailBodyTextdefault(new Guid(dsvendor.Tables[0].Rows[0]["FLDNEEDEDID"].ToString())//, dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                       , dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString()
                                       , dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString());                 
                    PhoenixMail.SendMail(mailto.Trim(), mailcc.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
                    PhoenixCrewWorkingGearQuotation.updatequerysentdate(new Guid(Neededid),int.Parse(agentid));
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
            
            ucConfirm.ErrorMessage = "Email sent successfully";
            ucConfirm.Visible = true;

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
            string selectedvendors = ",";
            selectedvendors = ViewState["selectedagent"] == null ? null : ViewState["selectedagent"].ToString();
            StringBuilder strname = new StringBuilder();
            int count = 1;
            foreach (GridDataItem gvr in RepAgents.Items)
            {
                string Trvelagentid = ((RadLabel)gvr.FindControl("lbltravelagentid")).Text;
                string agentid = ((RadLabel)gvr.FindControl("lblagentid")).Text;
                string Neededid = ((RadLabel)gvr.FindControl("lblNeededid")).Text;
                string mailto = ((RadTextBox)gvr.FindControl("txtEmailto")).Text;
                string mailcc = ((RadTextBox)gvr.FindControl("txtEmailcc")).Text;
                string reqno = ((RadLabel)gvr.FindControl("lblreqno")).Text;

                strname.Append("<br/>&nbsp;&nbsp;&nbsp;" + count++ + ".&nbsp;&nbsp;&nbsp;" + ((RadTextBox)gvr.FindControl("txtname")).Text);
                DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["NEEDEDID"].ToString()), selectedvendors);

                if (IsValidateEmail(mailto, mailcc))
                {
                    emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dsvendor.Tables[0].Rows[0]["FLDNEEDEDID"].ToString())//, dsvendor.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString(),
                                       , dsvendor.Tables[0].Rows[0]["FLDNAME"].ToString()
                                       , dsvendor.Tables[0].Rows[0]["FLDUSERNAME"].ToString());

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

            ucConfirm.ErrorMessage = "Email sent successfully";
            ucConfirm.Visible = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void RepAgents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

}
