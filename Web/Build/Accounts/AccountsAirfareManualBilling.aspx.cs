using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Accounts_AccountsAirfareManualBilling : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Billing", "BILLING", ToolBarDirection.Right);
            toolbar.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);

            MenuAirfareBilling.AccessRights = this.ViewState;
            MenuAirfareBilling.MenuList = toolbar.Show();
            MenuAirfareBilling.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarlist = new PhoenixToolbar();
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBilling.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarlist.AddImageLink("javascript:CallPrint('gvAirfareBilling')", "Print Grid", "icon_print.png", "PRINT");
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBilling.aspx", "Find", "search.png", "FIND");
            toolbarlist.AddImageButton("../Accounts/AccountsAirfareManualBilling.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAirfareBillinglist.AccessRights = this.ViewState;
            MenuAirfareBillinglist.MenuList = toolbarlist.Show();

            txtAgentId.Attributes.Add("style", "visibility:hidden");
            txtVesselId.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarMain = new PhoenixToolbar();

            toolbarMain.AddButton("Billed", "BILLED", ToolBarDirection.Right);
            toolbarMain.AddButton("Unbilled", "UNBILLED", ToolBarDirection.Right);
            MenuAirfareBillingMain.AccessRights = this.ViewState;
            MenuAirfareBillingMain.MenuList = toolbarMain.Show();

            MenuAirfareBillingMain.SelectedMenuIndex = 1;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }

            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
            imgShowVessel.Attributes.Add("onclick", "return showPickList('spnPickListVessel', 'codehelp1', '', '../Common/CommonPickListVessel.aspx', true); ");

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

            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsAirfareManualBilling.aspx");
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
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtInvoiceNumber.Text = "";
                txtSupplierCode.Text = "";
                txtSupplierName.Text = "";
                txtAgentId.Text = "";
                txtVesselId.Text = "";
                txtVessel.Text = "";
                txtTicketNo.Text = "";
                gvAirfareBilling.CurrentPageIndex = 0;
             
                Rebind();
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

            if (e.CommandName.ToUpper().Equals("BILL"))
            {
                string ticketNo = ((RadLabel)e.Item.FindControl("lblTicketNo")).Text;
                string vesselId = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string agentInvoiceNo = ((RadLabel)e.Item.FindControl("lblAgentInvoiceNo")).Text;

                Response.Redirect("../Accounts/AccountsAirfareManualBillingDetails.aspx?ticketNo=" + ticketNo + "&vesselId=" + vesselId + "&agentInvoiceNo=" + agentInvoiceNo, false);
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDNAME", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO" };
        string[] alCaptions = { "Invoice No", "Invoice Date", "Agent Name", "Passenger Name", "Departure Date", "Vessel", "Ticket No" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsAirfareManualBilling.SearchBilling(txtInvoiceNumber.Text, General.GetNullableInteger(txtAgentId.Text),
            txtEmployeeName.Text, txtVessel.Text, txtTicketNo.Text,
            sortexpression, sortdirection,
            gvAirfareBilling.CurrentPageIndex + 1,
            gvAirfareBilling.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvAirfareBilling", "Airfare Billing", alCaptions, alColumns, ds);

        gvAirfareBilling.DataSource = ds;
        gvAirfareBilling.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDNAME", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO" };
        string[] alCaptions = { "Invoice No", "Invoice Date", "Agent Name", "Passenger Name", "Departure Date", "Vessel", "Ticket No" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsAirfareManualBilling.SearchBilling(txtInvoiceNumber.Text, General.GetNullableInteger(txtAgentId.Text),
            txtEmployeeName.Text, txtVessel.Text, txtTicketNo.Text,
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
        Response.Write("<td><h3>Airfare Billing</h3></td>");
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

    protected void Rebind()
    {
        gvAirfareBilling.SelectedIndexes.Clear();
        gvAirfareBilling.EditIndexes.Clear();
        gvAirfareBilling.DataSource = null;
        gvAirfareBilling.Rebind();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvAirfareBilling_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
