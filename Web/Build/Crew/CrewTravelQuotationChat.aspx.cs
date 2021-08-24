using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web;

public partial class CrewTravelQuotationChat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
          
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                Button1.Attributes.Add("style", "display:none");
                ViewState["VIWEONLY"] = "0";
                ViewState["QUOTEPAGENUMBER"] = 1;
                ViewState["QUOTESORTEXPRESSION"] = null;
                ViewState["QUOTESORTDIRECTION"] = null;
                ViewState["SESSIONID"] = null;
                ViewState["ISOFFICE"] = 0;
                ViewState["QUOTEIDVNDOR"] = null;
                ViewState["defauttext"] = null;

                if (Request.QueryString["VIEWONLY"] == null)
                {
                    ViewState["VIWEONLY"] = "1";
                }

                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                }
                if (Request.QueryString["QUOTEID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                }
                if (Request.QueryString["AGENTID"] != null)
                {
                    ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
                }
                if (Request.QueryString["TRAVELAGENTID"] != null)
                {
                    ViewState["SESSIONID"] = Request.QueryString["TRAVELAGENTID"].ToString();
                }
                if (Request.QueryString["ISOFFICE"] != null)
                {
                    ViewState["ISOFFICE"] = Request.QueryString["ISOFFICE"].ToString();
                    if (!Request.QueryString["ISOFFICE"].ToString().Equals("1"))
                    {
                        chksendmail.Visible = false;
                        lblmail.Visible = false;
                    }
                }

                if (Request.QueryString["AGENTNAME"] != null)
                {
                   lblTitle.Text = Request.QueryString["AGENTNAME"].ToString().Replace('~', '&');
                }
                if (Request.QueryString["AGENTNAMEOLY"] != null)
                {
                    ViewState["defauttext"] = "Type a message to "+ Request.QueryString["AGENTNAMEOLY"].ToString() + " here";

                    Page.ClientScript.RegisterHiddenField("defauttext", ViewState["defauttext"].ToString());
                    txtChat.Text = ViewState["defauttext"].ToString();
                    txtChat.ForeColor = System.Drawing.Color.Gray;
                }

                ViewState["PAGENUMBER"] = 1;
                gvQuotation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuQuotationLineItem.AccessRights = this.ViewState;
            MenuQuotationLineItem.Title = lblTitle.Text;
            MenuQuotationLineItem.MenuList = toolbar.Show();
            BindQuotation();
            BindRoutingDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindQuotation()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDTAX", "FLDTOTALAMOUNT", "APPROVERNAME", "FLDTRAVELAPPROVEDDATE" };
        string[] alCaptions = { "Quotation Number", "Currency", "Amount", "Tax", "Total Amount", "Approved By", "Approved Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ISOFFICE"].ToString().Equals("1"))
            ViewState["QUOTEIDVNDOR"] = null;
        else
            ViewState["QUOTEIDVNDOR"] = ViewState["QUOTEID"];

        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(ViewState["TRAVELID"].ToString()), General.GetNullableGuid(ViewState["SESSIONID"] == null ? null : ViewState["SESSIONID"].ToString()),
            General.GetNullableGuid(ViewState["QUOTEIDVNDOR"] == null ? null : ViewState["QUOTEIDVNDOR"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                    General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotation.DataSource = ds;
            gvQuotation.VirtualItemCount = iRowCount;

            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                //gvQuotation.SelectedIndex = 0;
                ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelQuotationChatDetails.aspx?QUOTEID=" + ViewState["QUOTEID"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelQuotationChatDetails.aspx?QUOTEID=" + ViewState["QUOTEID"];
            }
        }
        else
        {
            gvQuotation.DataSource = "";
            txtChat.Enabled = false;
            ifMoreInfo.Attributes["src"] = "../Crew/CrewTravelQuotationChatDetails.aspx";
        }
    }
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.Equals("CHAT"))
            {
                Chat();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void QuotationConfirmationSent()
    {
        string emailbodytext = "";
        DataSet ds = new DataSet();
        ds = PhoenixCrewTravelQuote.QuotationConfirmation(new Guid(ViewState["QUOTEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            emailbodytext = PrepareApprovalText(ds.Tables[0]);
            DataRow dr = ds.Tables[0].Rows[0];
            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                null,
                null,
                dr["FLDSUBJECT"].ToString(),
                emailbodytext,
                true,
                System.Net.Mail.MailPriority.Normal,
                "", null,
                null);
        }
    }

    protected string PrepareApprovalText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Quotation is Received from  " + dr["FLDNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");

        return sbemailbody.ToString();

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
    

    private bool IsValidQuotation(string quotationno, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (quotationno.Equals(string.Empty))
            ucError.ErrorMessage = "Quotation Number is required.";
        Int16 resultint;
        ucError.HeaderMessage = "Please provide the following required information";
        if (currency.Equals("Dummy")) currency = string.Empty;

        if (currency.Equals("") || !Int16.TryParse(currency, out resultint))
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    
    private void InsertQuotationWithRout(string quotationno, int currency)
    {

    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindRoutingDetails();
    }

    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearchForAgent(new Guid(ViewState["TRAVELID"].ToString()),
            General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()), null, int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);
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
        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);        
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        Chat(); 
    }
    private void Chat()
    {
        if (ViewState["QUOTEID"] != null && txtChat.Text != "" && !ViewState["defauttext"].ToString().Equals(txtChat.Text))
        {
            if (Request.QueryString["VIEWONLY"] == null)
            {
                PhoenixCrewTravelQuote.CrewTravelQuoteChatInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QUOTEID"].ToString()), General.GetNullableInteger(ViewState["AGENTID"].ToString()), txtChat.Text);
            }
            else
            {
                PhoenixCrewTravelQuote.CrewTravelQuoteChatInsert(0, General.GetNullableGuid(ViewState["QUOTEID"].ToString()), General.GetNullableInteger(ViewState["AGENTID"].ToString()), txtChat.Text);
            }
            txtChat.Text = "";

            if (ViewState["ISOFFICE"].ToString().Equals("1") && chksendmail.Checked==true )
            {
                SendChatMail();
            }
        }
        BindQuotation();
        BindRoutingDetails();
    }
    protected string PrepareEmailBodyText(string travelagentid, string formno, string agentname, string pagename, string username, string quotationno)
    {

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.Append(agentname + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + formno);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Sir ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " has sent you chat messeage on Quotation Number - <B>" + quotationno + "</B>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Please click on the link below and do the needful,");
        sbemailbody.AppendLine("<br/>");
        string url = Session["sitepath"] + "/Portal/PortalDefault.aspx";
        sbemailbody.AppendLine("<a href=" + url + " >" + url + "</a>");
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

    private void SendChatMail()
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelQuote.ChatMailgetdata(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()), "TRFQ");

            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareEmailBodyText(ds.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString(), ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString()
                                                       , ds.Tables[0].Rows[0]["FLDNAME"].ToString(), ds.Tables[0].Rows[0]["FLDPAGETO"].ToString()
                                                       , ds.Tables[0].Rows[0]["FLDSENDBY"].ToString(), ds.Tables[0].Rows[0]["FLDQUOTATIONREFNO"].ToString());

                PhoenixMail.SendMail(ds.Tables[0].Rows[0]["FLDAGENTEMAIL"].ToString(), null, null, ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString()
                                            , emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvQuotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["QUOTEPAGENUMBER"] = ViewState["QUOTEPAGENUMBER"] != null ? ViewState["QUOTEPAGENUMBER"] : gvQuotation.CurrentPageIndex + 1;
        BindQuotation();
    }

    protected void gvQuotation_ItemDataBound(object sender, GridItemEventArgs e)
    {

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
                ViewState["SHOWTAB"] = ((RadLabel)e.Item.FindControl("lblfinalizeyn")).Text;
                BindQuotation();
                BindRoutingDetails();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["QUOTEPAGENUMBER"] = null;
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";               
                BindRoutingDetails();

            }
            else if (e.CommandName.ToUpper() == "SAVE")
            {

            }
            else if (e.CommandName.ToUpper() == "INSTRUCTION")
            {

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

    }
}
