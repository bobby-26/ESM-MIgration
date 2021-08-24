using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewHRTravelQuotationAgentLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvQuotation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuQuotationList.AccessRights = this.ViewState;
        MenuQuotationList.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (Request.QueryString["TRAVELAGENTID"] != null)
                lblTitle.Text = "Quotation For [ " + PhoenixCrewTravelRequest.RequestNo + " &   Agent :   " + PhoenixCrewTravelQuote.Agent + "     ]";
            else
                lblTitle.Text = "Quotation For [ " + PhoenixCrewTravelRequest.RequestNo + "  ]";
            if (Request.QueryString["travelrequestid"] != null)
                ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
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

        BindQuotationDetails();
        BindQuotePassengers();
        if (ViewState["QUOTEID"] == null)
            BindPassengerDetails();
        else
            BindRoutingDetails();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuQuotationLineItem.AccessRights = this.ViewState;
        MenuQuotationLineItem.Title = lblTitle.Text;
        MenuQuotationLineItem.MenuList = toolbar.Show();
    }
    private void BindPassengerDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelPassengerSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString()), null, null, int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount);
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
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&PORT=" + ViewState["PORT"].ToString()
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

        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDCURRENCYCODE", "FLDCURRENTAMOUNT", "FLDCURRENTTAX", "FLDCURRENTTOTALAMOUNT", "FLDTOTALAMOUNT", "APPROVERNAME", "FLDTRAVELAPPROVEDDATE" };
        string[] alCaptions = { "Quotation Number", "Currency", "Fare", "Tax", "Total Amount", "Total Amount in USD", "Approved By", "Approved Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["TRAVELREQUESTID"] != null)
            travelid = ViewState["TRAVELREQUESTID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        if (ViewState["TRAVELAGENTID"] != null)
            travelagentid = ViewState["TRAVELAGENTID"].ToString();

        ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(travelid), General.GetNullableGuid(travelagentid), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                        General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Travel Quotations.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Quotations</h3></td>");
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
        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDCURRENCYCODE", "FLDCURRENTAMOUNT", "FLDCURRENTTAX", "FLDCURRENTTOTALAMOUNT", "FLDTOTALAMOUNT", "APPROVERNAME", "FLDTRAVELAPPROVEDDATE" };
        string[] alCaptions = { "Quotation Number", "Currency", "Fare", "Tax", "Total Amount", "Total Amount in USD", "Approved By", "Approved Date" };
        string travelid = null;
        string travelagentid = null;
        int agentid = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["TRAVELREQUESTID"] != null)
            travelid = ViewState["TRAVELREQUESTID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        if (ViewState["TRAVELAGENTID"] != null)
            travelagentid = ViewState["TRAVELAGENTID"].ToString();

        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearch(new Guid(travelid), General.GetNullableGuid(travelagentid), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvQuotation.PageSize, ref iRowCount, ref iTotalPageCount);

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
        }
        else
        {
            gvQuotation.DataSource = "";
        }
        General.SetPrintOptions("gvQuotation", "Quotation Details", alCaptions, alColumns, ds);      
    }
    
    
    protected void btn_approve(object sender, EventArgs e)
    {

    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindQuotationDetails();
            gvQuotation.Rebind();
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
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()), int.Parse(ViewState["AGENTID"].ToString()), int.Parse(ViewState["PAGENUMBER"].ToString()), gvLineItem.PageSize, ref iRowCount, ref iTotalPageCount, 0);
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
            General.GetNullableGuid(ViewState["TRAVELREQUESTID"] == null ? null : ViewState["TRAVELREQUESTID"].ToString())
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
    

    protected void gvQuotation_ItemCommand(object sender, GridCommandEventArgs e)
    {     
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
            ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
            BindRoutingDetails();
            BindQuotePassengers();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvQuotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuotation.CurrentPageIndex + 1;
        BindQuotationDetails();
    }

    protected void gvQuotation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lb1 = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblTravelID");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblAgentID");
            LinkButton lbn = (LinkButton)e.Item.FindControl("lblQuotationNo");

            RadLabel lblapproved = (RadLabel)e.Item.FindControl("lblIsApproved");
            RadLabel lblfinalized = (RadLabel)e.Item.FindControl("lblIsFinalized");
            RadLabel lbltravelfinalizeyn = (RadLabel)e.Item.FindControl("lbltravelfinalizeyn");

            HtmlImage imgflag = (HtmlImage)e.Item.FindControl("imgflag");

            if (lblapproved != null && lblfinalized != null && imgflag != null)
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
        }
    }

    protected void gvQuotePassengers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PASSPAGENUMBER"] = null;
        }
    }

    protected void gvQuotePassengers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PASSPAGENUMBER"] = ViewState["PASSPAGENUMBER"] != null ? ViewState["PASSPAGENUMBER"] : gvQuotePassengers.CurrentPageIndex + 1;
        BindQuotePassengers();
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
        BindRoutingDetails();
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblQuoteID = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");

            //ImageButton ib = (ImageButton)e.Row.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
                
                LinkButton lnknostop = (LinkButton)e.Item.FindControl("lnknostop");
                if (lnknostop != null)
                {
                    lnknostop.Attributes.Add("onclick", "parent.Openpopup('Filter', '','../Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&&framename=ifMoreInfo&Requestforstop=1&ROUTEID=" + lblRouteID.Text + "')");
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