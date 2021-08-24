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

public partial class CrewTravelQuotationAgentDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                lblTitle.Attributes.Add("style", "display:none");
                ViewState["REQUISITIONNO"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                SetInformation();

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewTravelQuotationAgentLineItem.aspx?TRAVELID=";
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelQuotationAgentLineItem.aspx?TRAVELID=" + ViewState["TRAVELID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelQuotationAgentLineItem.aspx?TRAVELID=" + ViewState["TRAVELID"] +
                    "&AGENTID=" + ViewState["AGENTID"].ToString() + "&PORT=" + ViewState["PORT"].ToString()
                    + "&DATE=" + ViewState["DATE"].ToString()
                    + "&VESSEL=" + ViewState["VESSEL"].ToString();
                }
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
                gvAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


                AddDefaultAgent();
            }
           
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Travel Request", "TRAVELREQUEST");
            toolbarmain.AddButton("Travel Plan", "TRAVELPLAN");
            toolbarmain.AddButton("Quotation", "AGENT");
            toolbarmain.AddButton("Ticket", "TICKET");
            //toolbarmain.AddButton("Invoice", "INVOICE");
            MenuAgent.AccessRights = this.ViewState;
            MenuAgent.MenuList = toolbarmain.Show();
            MenuAgent.SelectedMenuIndex = 2;

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Next", "NEXT",ToolBarDirection.Right);
            Menuapprove.Title = lblTitle.Text;
            Menuapprove.AccessRights = this.ViewState;
            Menuapprove.MenuList = toolbarmain.Show();
            

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAgent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewListAddressAgent.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "'); return false;", "Add Agent", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentDetail.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentDetail.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentDetail.aspx", "Compare Quotations", "<i class=\"fab fa-quora\"></i>", "QTNCOMPARE");
            
            MenuAgentList.AccessRights = this.ViewState;
            MenuAgentList.MenuList = toolbar.Show();           
            //BindData();           
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
        DataSet ds = PhoenixCrewTravelRequest.EditTravel(new Guid(Request.QueryString["TRAVELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            
            DataRow dr = ds.Tables[0].Rows[0];
            string vsl = "";
            if (Filter.CurrentTraveltoVesselName != null)
                vsl = Filter.CurrentTraveltoVesselName.ToString();

            lblTitle.Text = "Quotation Details ("+ dr["FLDREQUISITIONNO"].ToString() + " ) " ;
            PhoenixCrewTravelRequest.RequestNo = dr["FLDREQUISITIONNO"].ToString();
            if (ViewState["DATE"] == null)
                ViewState["DATE"] = General.GetNullableDateTime(dr["FLDDATEOFCREWCHANGE"].ToString());
            if (ViewState["VESSEL"] == null)
                ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            if (ViewState["PORT"] == null)
                ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
            ViewState["REQUISITIONNO"] =" [" + dr["FLDREQUISITIONNO"].ToString() + " ] ";

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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date","Received Date" };
        string travelid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelAgentSearch(General.GetNullableGuid(travelid), sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvAgent", "Travel Agents", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAgent.DataSource = ds;
            gvAgent.VirtualItemCount = iRowCount;
            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewTravelQuotationAgentLineItem.aspx?TRAVELID=" + ViewState["TRAVELID"];


            if (ViewState["AGENTID"] == null)
            {
                ViewState["AGENTID"] = ds.Tables[0].Rows[0]["FLDAGENTID"].ToString();
                PhoenixCrewTravelQuote.Agent = ds.Tables[0].Rows[0]["FLDAGENTNAME"].ToString();
                //gvAgent.SelectedIndex = 0;
            }
            if (ViewState["TRAVELAGENTID"] == null)
            {
                ViewState["TRAVELAGENTID"] = ds.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString();
                PhoenixCrewTravelQuote.Agent = ds.Tables[0].Rows[0]["FLDAGENTNAME"].ToString();
                //gvAgent.SelectedIndex = 0;
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"] != null)
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELID"] +
                    "&AGENTID=" + ViewState["AGENTID"].ToString() + "&PORT=" + ViewState["PORT"].ToString()
                    + "&DATE=" + ViewState["DATE"].ToString()
                    + "&VESSEL=" + ViewState["VESSEL"].ToString()
                    + "&TRAVELAGENTID=" + ViewState["TRAVELAGENTID"].ToString();
            }

        }
        else
        {
            gvAgent.DataSource = "";
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELID"].ToString();
        }        
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td> &nbsp;1. To add agent click on &nbsp;<i class=\"fa fa-plus-circle\"></i>&nbsp; button on the travel agents. </td> </tr> <tr><td>&nbsp;2. To send quotation to the agent,select the agents in the quotation details and click on &nbsp;<i class=\"fas fa-envelope\"></i>&nbsp; button on the travel agents.</td> </tr> <tr><td>&nbsp;3. To send reminder to the agent,select the agents in the quotation details and click on &nbsp;<i class=\"fas fa-bell\"></i>&nbsp; button on the travel agents.</td> </tr><tr><td>&nbsp;4. To compare the quotations,select the agents in the quotation details and click on &nbsp;<i class=\"fab fa-quora\"></i>&nbsp; button on the travel agents.</td> </tr></tr><tr><td>&nbsp;5. To chat with agent click on &nbsp;<i class=\"fas fa-comments\"></i>&nbsp; button in the travel agents action column.</td> </tr><tr><td>&nbsp;6. To remove the agent click on &nbsp;<i class=\"fas fa-trash\"></i>&nbsp; button on the travel agents.</td> </tr></tr></table>";
    }

    protected void Menuapprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("NEXT"))
            {                
                Response.Redirect("../Crew/CrewTravelQuoteTicketList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
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
            BindData();
            gvAgent.Rebind();
            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            
            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage  = "E-mail sent to ,<BR/>" + Session["emailsend"].ToString();
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
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();

                BindData();
                gvAgent.Rebind();
            }
            if (CommandName.ToUpper().Equals("QTNCOMPARE"))
            {
                if (ViewState["TRAVELID"] != null)
                {
                    string selectedagents = ",";
                    foreach (GridDataItem gvr in gvAgent.Items)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked==true)
                        {
                            selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                        }
                    }

                    if (selectedagents.Length > 1)
                        ifMoreInfo.Attributes["src"] = "CrewTravelQuotationCompare.aspx?AGENTS=" + selectedagents + "&TRAVELID=" + ViewState["TRAVELID"].ToString();
                    else
                    {
                        ucError.ErrorMessage = "There are no quotations to compare.";
                        ucError.Visible = true;
                    }                    
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "CrewTravelQuotationCompare.aspx";
                }               
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
                Response.Redirect("../Crew/CrewTravelQuoteTicketList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&vessel=" + ViewState["VESSEL"].ToString() + "&date=" + ViewState["DATE"].ToString() + "&port=" + ViewState["PORT"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
			if (CommandName.ToUpper().Equals("INVOICE"))
			{
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewTravelQuote.CrewTravelAgentSearch(new Guid(ViewState["TRAVELID"].ToString()), sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Travel_Agents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Agents</h3></td>");
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
    private void SendForQuotation()
    {        
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem gvr in gvAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked==true)
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
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked==true)
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
                //string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=RFQREMAINDER&travelid=" + ViewState["TRAVELID"].ToString() + "&selectedagent=" + selectedvendors + "');return false";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
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
    
    private bool IsValidAgent(string agentid)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (agentid.Trim().Equals(""))
            ucError.ErrorMessage = "Agent is required";
        return (!ucError.IsError);

    }
    private void InsertTravelAgent(string agentid)
    {
        PhoenixCrewTravelQuote.InsertCrewTravelAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), Convert.ToInt32(agentid));

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
    
    private void UpdateSelectAgentForPo(string routequotationid)
    {
        try
        {
            PhoenixCrewTravelQuote.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), new Guid(routequotationid));

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    

    protected void gvAgent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("SELECTAGENT"))
            {
                UpdateSelectAgentForPo(((RadLabel)e.Item.FindControl("lblQuotationID")).Text);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewTravelQuote.DeleteQuoteAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblTravelID")).Text), int.Parse(((RadLabel)e.Item.FindControl("lblAGentID")).Text));
                BindData();
                gvAgent.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
                ViewState["TRAVELAGENTID"] = ((RadLabel)e.Item.FindControl("lblTravelAgentId")).Text;
                PhoenixCrewTravelQuote.Agent = ((LinkButton)e.Item.FindControl("lnkAgentName")).Text;
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELID"] +
                        "&AGENTID=" + ViewState["AGENTID"].ToString() + "&PORT=" + ViewState["PORT"].ToString()
                        + "&DATE=" + ViewState["DATE"].ToString()
                        + "&VESSEL=" + ViewState["VESSEL"].ToString()
                        + "&TRAVELAGENTID=" + ViewState["TRAVELAGENTID"].ToString();
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                string emailbodytext = "";
                DataSet ds = new DataSet();

                ds = PhoenixCrewTravelQuote.ApproveTravel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblQuotationID")).Text)
                , General.GetNullableInteger(ViewState["AGENTID"].ToString())
                , General.GetNullableDateTime(DateTime.Now.ToString()), int.Parse(((Label)e.Item.FindControl("lblIsApproved")).Text));
                if (Int32.Parse(((RadLabel)e.Item.FindControl("lblIsApproved")).Text) == 1)
                {

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        emailbodytext = PrepareApprovalText(ds.Tables[1], 1);
                        DataRow dr = ds.Tables[1].Rows[0];
                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            null,
                            dr["FLDSUBJECT"].ToString() + "     " + PhoenixCrewTravelRequest.RequestNo,
                            emailbodytext,
                            true,
                            System.Net.Mail.MailPriority.Normal,
                            "", null,
                            null);
                    }
                    ucConfirm.ErrorMessage = "Travel approval is cancelled.";
                }
                else
                {

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        emailbodytext = PrepareApprovalText(ds.Tables[1], 0);
                        DataRow dr = ds.Tables[1].Rows[0];
                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                            null,
                            dr["FLDSUBJECT"].ToString() + "     " + PhoenixCrewTravelRequest.RequestNo,
                            emailbodytext,
                            true,
                            System.Net.Mail.MailPriority.Normal,
                            "",
                            null,
                            null);
                    }
                    ucConfirm.ErrorMessage = "Travel order is approved";
                }
                ucConfirm.Visible = true;
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (IsValidAgent(((RadTextBox)e.Item.FindControl("txtAgentID")).Text))
                {
                    InsertTravelAgent(((RadTextBox)e.Item.FindControl("txtAgentID")).Text);
                    BindData();
                    gvAgent.Rebind();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvAgent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAgent.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAgent_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnPickAgent");
            RadTextBox txtagentname = (RadTextBox)e.Item.FindControl("txtAgentName");
            RadTextBox txtagentcode = (RadTextBox)e.Item.FindControl("txtAgentCode");
            RadTextBox txtagentid = (RadTextBox)e.Item.FindControl("txtAgentID");
            if (txtagentid != null) txtagentid.Attributes.Add("style", "visibility:hidden");

            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?producttype=63&framename=filterandsearch', true);");

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
        }
    }
}

