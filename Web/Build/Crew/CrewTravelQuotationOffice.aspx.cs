using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravelQuotationOffice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
         
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                cmdHiddenSubmitclose.Attributes.Add("style", "display:none");
                if (Request.QueryString["SESSIONID"] != null)
                {
                    bindTravals();
                }
               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QUOTEPAGENUMBER"] = 1;
                ViewState["QUOTESORTEXPRESSION"] = null;
                ViewState["QUOTESORTDIRECTION"] = null;

                ViewState["CURRENTINDEX"] = 1;
                ViewState["EDITROW"] = "0";
                CheckWebSessionStatus();
                gvAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.Title = lblNote.Text;
            MenuTitle.MenuList = toolbar.Show();
            //BindQuotationDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void bindTravals()
    {
        ViewState["SESSIONID"] = Request.QueryString["SESSIONID"].ToString();
        DataSet ds = PhoenixCrewTravelQuote.CrewQuotationDetails(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["TRAVELID"] = dr["FLDTRAVELID"].ToString();
            ViewState["AGENTID"] = dr["FLDAGENTID"].ToString();
            ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
            ViewState["DATE"] = dr["FLDDATEOFCREWCHANGE"].ToString();
            ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            lblNote.Text = "Quotation Details [ " + dr["FLDREQUISITIONNO"].ToString() + " ]";
            ViewState["Title"] = dr["FLDAGENTNAME"].ToString() + " [ " + dr["FLDREQUISITIONNO"].ToString() + " ]";
            ViewState["FINALIZEYN"] = dr["FLDSESSIONSTATUS"].ToString();
            ViewState["WEBSESSIONSTATUS"] = dr["FLDSESSIONSTATUS"].ToString();
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
            DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearchForAgent(new Guid(travelid), new Guid(ViewState["SESSIONID"].ToString()) , sortexpression, sortdirection, (int)ViewState["QUOTEPAGENUMBER"], gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);
            DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAgent.DataSource = ds.Tables[0];
            gvAgent.VirtualItemCount = iRowCount;
            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ifMoreInfo.Attributes["src"] = "CrewTravelQuotationOfficeGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&QUOTEID=" + ViewState["QUOTEID"] + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
           + "&date=" + ViewState["DATE"].ToString()
           + "&vessel=" + ViewState["VESSEL"].ToString()
           + "&active=" + ViewState["FINALIZEYN"].ToString() + "&confirmed=" + ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
            }
            //gvAgent.SelectedIndex = 0; 
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "CrewTravelQuotationOfficeGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
          + "&date=" + ViewState["DATE"].ToString()
          + "&vessel=" + ViewState["VESSEL"].ToString()
          + "&active=0&confirmed=request";
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
     
    private void InsertQuotation(string quotationno,int currency)
    {
        //Guid? Quoteid = new Guid();
        //PhoenixCrewTravelQuote.InsertCrewTravelQuotationByAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["TRAVELREQUESTID"].ToString()), Convert.ToInt32(ViewState["AGENTID"].ToString()), quotationno, "Request", currency,ref Quoteid);
        //ViewState["QUOTEID"] = Quoteid;
        //BindQuotationDetails();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["QUOTEID"] = null;
        BindQuotationDetails();       
        BindRoutingDetails();
        gvAgent.Rebind();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
    }
    protected void cmdHiddenSubmitclose_Click(object sender, EventArgs e)
    {        
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('Filter',null);", true);
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
                ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();        
        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
      
    }
  
    private void UpdateQuotationLineItem(string travelid, string quouteid, string routeid, string noofstops,string departuredate,string arrivaldate, string duration, string amount,string tax)
    {
        PhoenixCrewTravelQuoteLine.UpdateQuotationLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(routeid), new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()),DateTime.Parse(departuredate),DateTime.Parse(arrivaldate), int.Parse(noofstops), decimal.Parse(duration), General.GetNullableDecimal(amount), General.GetNullableDecimal(tax),null);

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

    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAgent.CurrentPageIndex + 1;
        BindQuotationDetails();
    }

    protected void gvAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!(IsValidQuotation(
                ((RadTextBox)e.Item.FindControl("txtQuotationNo")).Text.ToString().Trim(),
                 ((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency)))
            {
                ucError.Visible = true;
                return;
            }
            InsertQuotation(((RadTextBox)e.Item.FindControl("txtQuotationNo")).Text.ToString().Trim(),
                            int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency));
            BindQuotationDetails();
        }
       else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
            ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
            string finalizeyn = ((RadLabel)e.Item.FindControl("lblfinalizeyn")).Text;
            string confirmedyn = ((RadLabel)e.Item.FindControl("lblconfirmedyn")).Text;

            BindRoutingDetails();
            ifMoreInfo.Attributes["src"] = "CrewTravelQuotationOfficeGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&QUOTEID=" + ViewState["QUOTEID"] + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
               + "&date=" + ViewState["DATE"].ToString()
               + "&vessel=" + ViewState["VESSEL"].ToString()
                 + "&active=" + ViewState["FINALIZEYN"].ToString() + "&confirmed=" + confirmedyn;
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton imgbtn = (LinkButton)e.Item.FindControl("cmdChat");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblQuotationID");
            if (imgbtn != null)
                imgbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationChat.aspx?QUOTEID=" + lb.Text + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&AGENTNAME=" + ViewState["Title"].ToString() + "&ACTIVE=" + ViewState["FINALIZEYN"].ToString() + "&VIEWONLY=Y'); return false;");
            if (imgbtn != null && Request.QueryString["OFFICE"] != null)
                imgbtn.Visible = false;
        }
    }
}
