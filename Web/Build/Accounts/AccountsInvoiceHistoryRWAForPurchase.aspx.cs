using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceHistoryRWAForPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceHistoryRWAForPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (ViewState["qfrom"] != null && ViewState["qfrom"].ToString() == "purchaseinvoice")
            {
                toolbarmain.AddButton("Voucher", "VOUCHER");
                toolbarmain.AddButton("Line Items", "VLINEITEM");
            }
            else
            {
                toolbarmain.AddButton("Invoice", "INVOICE");
                toolbarmain.AddButton("PO", "PO");
            }


            // if (ViewState["qfrom"] != null && ViewState["qfrom"].ToString() == "invoice")
          //  toolbarmain.AddButton("Direct PO", "DPO");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            toolbarmain.AddButton("History", "HISTORY");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["Invoicenumber"] = "";
                if (Request.QueryString["qinvoicecode"] != null)
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                else
                    ViewState["invoicecode"] = Guid.Empty;

                if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"].ToString() != string.Empty)
                    ViewState["qfrom"] = Request.QueryString["qfrom"];
                else
                    ViewState["qfrom"] = "";
                SetRedirectURL();



                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                if (ViewState["qfrom"] != null && ViewState["qfrom"].ToString() == "invoice")
                    MenuOrderFormMain.SelectedMenuIndex = 4;
                else
                    MenuOrderFormMain.SelectedMenuIndex = 4;
                BindInvoiceNumber(ViewState["invoicecode"].ToString());
                gvInvoiceHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoiceHistory_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect(ViewState["URL"].ToString() + ViewState["invoicecode"] + "&qcallfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["qfrom"].ToString());
            }
            if (CommandName.ToUpper().Equals("DPO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=invoice");
            }
            else if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherMaster.aspx?voucherid=" + ViewState["VOUCHERID"]);
            }
            else if (CommandName.ToUpper().Equals("VLINEITEM"))
            {
                Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["VOUCHERID"]);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachmentsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["qfrom"].ToString());
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 4;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        //int iTotalPageCount = 0;
        string type = "";

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsInvoice.InvoiceHistoryList(new Guid(ViewState["invoicecode"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoiceHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Invoice History" + "-" + ViewState["Invoicenumber"] + "</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

    private void SetRedirectURL()
    {
        if (ViewState["qfrom"] != null)
        {
            if (ViewState["qfrom"].ToString() == "invoice")
            {
                ViewState["URL"] = "../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=";
            }
            else if (ViewState["qfrom"].ToString() == "AD")
            {
                ViewState["URL"] = "../Accounts/AccountsInvoiceMasterRWAForPurchase.aspx?qinvoicecode=";
            }
            else if (ViewState["qfrom"].ToString() == "PR")
            {
                ViewState["URL"] = "../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=";
            }
            else if (ViewState["qfrom"].ToString() == "invoiceforpurchase")
            {
                ViewState["URL"] = "../Accounts/AccountsInvoiceMasterForPurchase.aspx?qinvoicecode=";
            }
        }
    }

    private void BindInvoiceNumber(string invoicenumber)
    {
        if (General.GetNullableGuid(invoicenumber) != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(invoicenumber));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                ViewState["Invoicenumber"] = dsInvoice.Tables[0].Rows[0]["FLDINVOICENUMBER"];
                // frmTitle.Text = frmTitle.Text + " - " + dsInvoice.Tables[0].Rows[0]["FLDINVOICENUMBER"].ToString();
                ViewState["VOUCHERID"] = dsInvoice.Tables[0].Rows[0]["FLDVOUCHERID"];
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string type = "";
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        type = rblHistoryType.SelectedItem.Value;
        ds = PhoenixAccountsInvoice.InvoiceHistoryList(new Guid(ViewState["invoicecode"].ToString())
                                                     , type);
        General.SetPrintOptions("gvInvoiceHistory", "Accounts Invoice History" + "-" + ViewState["Invoicenumber"], alCaptions, alColumns, ds);


        gvInvoiceHistory.DataSource = ds;
        gvInvoiceHistory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvInvoiceHistory_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvInvoiceHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        RadGrid _gridView = (RadGrid)sender;
        int iRowno;
        iRowno = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }
    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoiceHistory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

}
