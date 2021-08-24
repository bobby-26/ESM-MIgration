using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAirfareManualBillingBilledDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
            MenuAirfareBilling.AccessRights = this.ViewState;
            MenuAirfareBilling.MenuList = toolbar.Show();
            MenuAirfareBilling.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarlist.AddImageLink("javascript:CallPrint('gvAirfareBilling')", "Print Grid", "icon_print.png", "PRINT");
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx", "Find", "search.png", "FIND");
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAirfareBillinglist.AccessRights = this.ViewState;
            MenuAirfareBillinglist.MenuList = toolbarlist.Show();

            PhoenixToolbar toolbarMain = new PhoenixToolbar();

            toolbarMain.AddButton("Billed", "BILLED", ToolBarDirection.Right);
            toolbarMain.AddButton("Unbilled", "UNBILLED", ToolBarDirection.Right);
            MenuAirfareBillingMain.AccessRights = this.ViewState;
            MenuAirfareBillingMain.MenuList = toolbarMain.Show();
            MenuAirfareBillingMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAirfareBillingMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UNBILLED"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBilling.aspx");
            }

            else if (CommandName.ToUpper().Equals("BILLED"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuAirfareBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBillingBilledDetails.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuAirfareBillinglist_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtPassengerName.Text = "";
                ucToDate.Text = "";
                ucFromDate.Text = "";
                txtInvoiceNumber.Text = "";
                BindData();
                gvAirfareBilling.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvAirfareBilling_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                Label lblAgentInvoiceId = (Label)e.Row.FindControl("lblAgentInvoiceId");
                ImageButton cmdBill = (ImageButton)e.Row.FindControl("cmdBill");
                if (cmdBill != null)
                {
                    //cmdBill.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Accounts/AccountsAirfareManualBillingExport2PDF.aspx?AgentInvoiceId=" + lblAgentInvoiceId.Text + "'); return false;");
                    cmdBill.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Reports/ReportsView.aspx?applicationcode=4&reportcode=TRAVELINVOICE&showmenu=false&showexcel=no&AgentInvoiceId=" + lblAgentInvoiceId.Text.ToString() + "');return true;");
                    cmdBill.Visible = SessionUtil.CanAccess(this.ViewState, cmdBill.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAirfareBilling_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucInvoiceDateEdit")).Text,
                        ((UserControlDate)e.Item.FindControl("ucDepartureDateEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtPassengerNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAirfareManualBilling.UpdateInvoiceBilled(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(((Label)e.Item.FindControl("lblAgentInvoiceIdEdit")).Text),
                        Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucInvoiceDateEdit")).Text),
                        Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucDepartureDateEdit")).Text),
                        ((RadTextBox)e.Item.FindControl("txtPassengerNameEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtSector1Edit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtSector2Edit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtSector3Edit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtSector4Edit")).Text,
                        Convert.ToDecimal(((RadTextBox)e.Item.FindControl("txtTaxEdit")).Text));
                ucStatus.Text = "Billed information updated";

                BindData();
            }

            if (e.CommandName == "Page")
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

    protected void gvAirfareBilling_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            //    GridView _gridView = (GridView)sender;
            //    int nCurrentRow = e.RowIndex;

            //    if (!IsValidData(((UserControlDate)e.Item.FindControl("ucInvoiceDateEdit")).Text,
            //                ((UserControlDate)e.Item.FindControl("ucDepartureDateEdit")).Text,
            //                ((RadTextBox)e.Item.FindControl("txtPassengerNameEdit")).Text))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }

            //    PhoenixAccountsAirfareManualBilling.UpdateInvoiceBilled(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //                new Guid(((Label)e.Item.FindControl("lblAgentInvoiceIdEdit")).Text),
            //                Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucInvoiceDateEdit")).Text),
            //                Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucDepartureDateEdit")).Text),
            //                ((RadTextBox)e.Item.FindControl("txtPassengerNameEdit")).Text,
            //                ((RadTextBox)e.Item.FindControl("txtSector1Edit")).Text,
            //                ((RadTextBox)e.Item.FindControl("txtSector2Edit")).Text,
            //                ((RadTextBox)e.Item.FindControl("txtSector3Edit")).Text,
            //                ((RadTextBox)e.Item.FindControl("txtSector4Edit")).Text,
            //                Convert.ToDecimal(((RadTextBox)e.Item.FindControl("txtTaxEdit")).Text));
            //    ucStatus.Text = "Billed information updated";
            //    _gridView.EditIndex = -1;
            //    BindData();
            // SetPageNavigator();

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

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDINVOICEDATE", "FLDDEPARTUREDATE", "FLDPASSENGERNAME", "FLDSECTOR1", "FLDSECTOR2", "FLDSECTOR3", "FLDSECTOR4", "FLDCODEDESCRIPTIONS", "FLDINVOICENUMBER", "FLDORDERNO", "FLDTAX", "FLDCHARGEDFARE", "FLDREVENUEVOUCHERNO", "FLDTARGETCOMPANY", "FLDTARGETCOPURCHASEINVOICENO" };
        string[] alCaptions = { "Purchase Invoice No.", "Invoice Date", "Departure Date", "Passenger Name", "Sector 1", "Sector 2", "Sector 3", "Sector 4", "Account Code – Account Code Description", "Invoice Number", "Order Number", "Tax (USD)", "Charged Fare", "Revenue Voucher No", "Target Company", "Target Company Purchase Invoice Voucher" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsAirfareManualBilling.SearchBilled(txtInvoiceNumber.Text,
                                                                        General.GetNullableDateTime(ucFromDate.Text),
                                                                        General.GetNullableDateTime(ucToDate.Text),
                                                                        txtPassengerName.Text,
                                                                        sortexpression, sortdirection,
                                                                        (int)ViewState["PAGENUMBER"],
                                                                        gvAirfareBilling.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);

        General.SetPrintOptions("gvAirfareBilling", "Billed", alCaptions, alColumns, ds);

        gvAirfareBilling.DataSource = ds;
        gvAirfareBilling.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDINVOICEDATE", "FLDDEPARTUREDATE", "FLDPASSENGERNAME", "FLDSECTOR1", "FLDSECTOR2", "FLDSECTOR3", "FLDSECTOR4", "FLDCODEDESCRIPTIONS", "FLDINVOICENUMBER", "FLDORDERNO", "FLDTAX", "FLDCHARGEDFARE", "FLDREVENUEVOUCHERNO", "FLDTARGETCOMPANY", "FLDTARGETCOPURCHASEINVOICENO" };
        string[] alCaptions = { "Purchase Invoice No.", "Invoice Date", "Departure Date", "Passenger Name", "Sector 1", "Sector 2", "Sector 3", "Sector 4", "Account Code&Account Code Description", "Invoice Number", "Order Number", "Tax (USD)", "Charged Fare", "Revenue Voucher No", "Target Company", "Target Company Purchase Invoice Voucher" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsAirfareManualBilling.SearchBilled(txtInvoiceNumber.Text,
                                                                        General.GetNullableDateTime(ucFromDate.Text),
                                                                        General.GetNullableDateTime(ucToDate.Text),
                                                                        txtPassengerName.Text,
                                                                        sortexpression, sortdirection,
                                                                        (int)ViewState["PAGENUMBER"],
                                                                        PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Billing.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Billed</h3></td>");
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private bool IsValidData(string invoiceDate, string departureDate, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (invoiceDate == null)
            ucError.ErrorMessage = "Invoice date is required.";

        if (departureDate == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Passenger name is required.";

        return (!ucError.IsError);
    }

    protected void gvAirfareBilling_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
