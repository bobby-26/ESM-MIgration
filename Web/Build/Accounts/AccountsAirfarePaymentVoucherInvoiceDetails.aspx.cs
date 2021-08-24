using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsAirfarePaymentVoucherInvoiceDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            if (Request.QueryString["VesselId"] != null && Request.QueryString["VesselId"] != string.Empty)
                ViewState["VesselId"] = Request.QueryString["VesselId"];

            if (Request.QueryString["CancelledYN"] != null && Request.QueryString["CancelledYN"] != string.Empty)
                ViewState["CancelledYN"] = Request.QueryString["CancelledYN"];

            if (Request.QueryString["NonVesselYN"] != null && Request.QueryString["NonVesselYN"] != string.Empty)
                ViewState["NonVesselYN"] = Request.QueryString["NonVesselYN"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsAirfarePaymentVoucherInvoiceDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvIVList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsAirfarePaymentVoucherInvoiceDetails.aspx", "Find", "search.png", "FIND");
            MenuInvoiceListing.AccessRights = this.ViewState;
            MenuInvoiceListing.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["callfrom"] = null;
            }
            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];
          //  BindDataIVList();
         }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInvoiceListing_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindDataIVList();
                gvIVList.Rebind();
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

    protected void gvIVList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsAirfarePaymentVoucherDetails.DeleteInvoice(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadLabel)e.Item.FindControl("lblInvoiceId")).Text);
                BindDataIVList();
 
                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIVList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

           // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

        }
    }

    protected void gvIVList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataIVList();
     }

    private void BindDataIVList()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iPayAmount = 0;

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO", "FLDTOTAL" };
        string[] alCaptions = { "Invoice No", "Invoice Date", "Passenger Name", "Departure Date", "Vessel", "Ticket No", "Total Charges" };

        DataSet ds = PhoenixAccountsAirfarePaymentVoucherDetails.AirfareIVList(ViewState["voucherid"].ToString(),
            General.GetNullableString(txtInvoiceNumber.Text),
            General.GetNullableDecimal(txtRangeFrom.Text),
            General.GetNullableDecimal(txtRangeTo.Text),
            General.GetNullableInteger(ViewState["VesselId"].ToString()),
            General.GetNullableDateTime(txtFromDate.Text),
            General.GetNullableDateTime(txtToDate.Text),
            ref iPayAmount,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableString(txtTicketNumber.Text),
            General.GetNullableInteger(ViewState["CancelledYN"].ToString()),
            General.GetNullableInteger(ViewState["NonVesselYN"].ToString()));

        General.SetPrintOptions("gvIVList", "Invoice Listing", alCaptions, alColumns, ds);


        gvIVList.DataSource = ds;
        gvIVList.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        ViewState["PAYAMOUNT"] = iPayAmount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iPayAmount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDTICKETNO", "FLDTOTAL" };
        string[] alCaptions = { "Invoice No", "Invoice Date", "Passenger Name", "Departure Date", "Ticket No", "Total Charges" };

        DataSet ds = PhoenixAccountsAirfarePaymentVoucherDetails.AirfareIVList(ViewState["voucherid"].ToString(),
            General.GetNullableString(txtInvoiceNumber.Text),
            General.GetNullableDecimal(txtRangeFrom.Text),
            General.GetNullableDecimal(txtRangeTo.Text),
            General.GetNullableInteger(ViewState["VesselId"].ToString()),
            General.GetNullableDateTime(txtFromDate.Text),
            General.GetNullableDateTime(txtToDate.Text),
            ref iPayAmount,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableString(txtTicketNumber.Text),
            General.GetNullableInteger(ViewState["CancelledYN"].ToString()),
            General.GetNullableInteger(ViewState["NonVesselYN"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoicePV.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Invoice Listing</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvIVList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataIVList();
    }
}
