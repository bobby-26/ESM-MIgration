using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewTravelQuoteList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit1.Attributes.Add("style", "visibility:hidden");
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                }
                else
                {
                    ViewState["TRAVELID"] = "";
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
                else
                {
                    ViewState["TRAVELID"] = "";
                }
                if (Request.QueryString["PORT"] != null)
                    ViewState["PORT"] = Request.QueryString["PORT"];
                if (Request.QueryString["DATE"] != null)
                    ViewState["DATE"] = General.GetDateTimeToString(Request.QueryString["DATE"]);
                if (Request.QueryString["VESSEL"] != null)
                    ViewState["VESSEL"] = Request.QueryString["VESSEL"];

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
            BindQuotationDetails();
            BindPassengerDetails();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearchForAgent(new Guid(travelid), new Guid(ViewState["SESSIONID"].ToString()), sortexpression, sortdirection, (int)ViewState["QUOTEPAGENUMBER"], 10, ref iRowCount, ref iTotalPageCount);
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
            AddOrRemoveTab();
            if (ViewState["MODE"].ToString() != "New")
                ViewState["MODE"] = "Edit";
        }
        else
        {
            AddOrRemoveTab();
            DataTable dt = ds.Tables[0];
            ShowNoQuoteFound(dt, gvAgent);

        }
        ViewState["QUOTEROWCOUNT"] = iRowCount;
        ViewState["QUOTETOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigatorForQuotation();

    }
    private void IsQuoteFinalized()
    {
        DataTable dt = PhoenixCrewTravelQuote.IsQuoteFinalized(new Guid(ViewState["QUOTEID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDFINALIZEDYN"].ToString() == string.Empty)
                ViewState["ISQUOTEFINALIZED"] = "0";
            else
                ViewState["ISQUOTEFINALIZED"] = dt.Rows[0]["FLDFINALIZEDYN"].ToString();
        }
    }
    private void AddOrRemoveTab()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["QUOTEID"] == null)
        {
            toolbar.AddButton("Create Quote", "CREATEQUOTE");
        }
        else
        {
            IsQuoteFinalized();
            toolbar.AddButton("Create Quote", "CREATEQUOTE");
            toolbar.AddButton("View Quote", "VIEWQUOTE");
            if (ViewState["ISQUOTEFINALIZED"].ToString() == "0")
            {                
                toolbar.AddButton("Confirm Quote", "CONFIRMQUOTE");
            }
        }
        MenuQuotationLineItem.MenuList = toolbar.Show();
        MenuQuotationLineItem.SetTrigger(pnlLineItemEntry);
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
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindQuotationDetails();
    }
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if(dce.CommandName.Equals("CREATEQUOTE"))
        {
            Response.Redirect("../CREW/CrewTravelQuotationItems.aspx?TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
                 + "&date=" + ViewState["DATE"].ToString()
                 + "&vessel=" + ViewState["VESSEL"].ToString() +"&MODE=NEW");
        }
        if (dce.CommandName.Equals("VIEWQUOTE"))
        {
            Response.Redirect("../CREW/CrewTravelQuotationItems.aspx?QUOTEID=" + ViewState["QUOTEID"] + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
                 + "&date=" + ViewState["DATE"].ToString()
                 + "&vessel=" + ViewState["VESSEL"].ToString() + "&MODE=EDIT");
        }
        if (dce.CommandName.Equals("CONFIRMQUOTE"))
        {
            try
            {
                PhoenixCrewTravelQuote.FinalizeQuotationForAgent(new Guid(ViewState["QUOTEID"].ToString()));
                PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["QUOTEID"].ToString()));
                QuotationConfirmationSent();
                PhoenixCrewTravelQuote.UpdateQuotation(0, new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()), "1", "CONFIRMED");
                CheckWebSessionStatus();

                ucStatus.Text = "Quotation is confirmed";
                BindQuotationDetails();
            }
            catch(Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true; ;
            }
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
    protected void gvAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["QUOTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationID")).Text;
            ViewState["TRAVELID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAgentID")).Text;
            BindQuotationDetails();
            BindPassengerDetails();
        }
    }
}
