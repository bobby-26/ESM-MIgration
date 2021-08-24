using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardAdvancePaymentGenerateVoucher : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePaymentGenerateVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);
            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;

                gvInvoice.PageSize = General.ShowRecords(gvInvoice.PageSize);
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate Payment Voucher", "GENERATEPAYMENTVOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            // MenuOrderFormMain.SelectedMenuIndex = 1;
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression.Replace(" ASC", "").Replace(" DESC", ""); ;

        switch (se.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        gvInvoice.Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERATEPAYMENTVOUCHER"))
            {
                try
                {
                    PhoenixAccountsAdvancePayment.GeneratePaymentVoucherAdvancePayment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                    BindData();
                    gvInvoice.Rebind();

                    //PhoenixAccountsAdvancePayment.GeneratePaymentVoucherAdvancePayment()

                    //PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                //Response.Redirect("../Accounts/AccountsAdvancePaymentGenerateVoucher.aspx");

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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Reference Number",
                                "Currency",
                                "Amount",
                                "Type",
                                "Date"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDNAME",
                                "FLDREFERENCEDOCUMENT",
                                "FLDCURRENCYCODE"                             ,
                                "FLDAMOUNT",
                                "FLDHARDNAME",
                                "FLDAPPROVEDDATE"
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

        ds = PhoenixAccountsAdvancePayment.GenerateAdvancePaymentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection
                                                , gvInvoice.CurrentPageIndex + 1, PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                , ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoice.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Invoice</h3></td>");
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
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

    protected void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;

        ds = PhoenixAccountsAdvancePayment.GenerateAdvancePaymentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection
                                                , gvInvoice.CurrentPageIndex + 1, gvInvoice.PageSize
                                                , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Reference Number",
                                "Currency",
                                "Amount",
                                "Type",
                                "Date"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDNAME",
                                "FLDREFERENCEDOCUMENT",
                                "FLDCURRENCYCODE"                             ,
                                "FLDAMOUNT",
                                "FLDHARDNAME",
                                "FLDAPPROVEDDATE"
                             };

        General.SetPrintOptions("gvInvoice", "Accounts Invoice", alCaptions, alColumns, ds);

        gvInvoice.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["FLDADVANCEPAYMENTID"] == null)
            {
                ViewState["FLDADVANCEPAYMENTID"] = ds.Tables[0].Rows[0]["FLDADVANCEPAYMENTID"].ToString();
            }
            SetRowSelection();
        }

        gvInvoice.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        gvInvoice.SelectedIndexes.Clear();

        foreach (GridDataItem item in gvInvoice.Items)
        {

            if (item.GetDataKeyValue("FLDADVANCEPAYMENTID").ToString().Equals(ViewState["FLDADVANCEPAYMENTID"].ToString()))
            {
                gvInvoice.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvInvoice.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadTextBox tb = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode"));
            if (tb != null)
                ViewState["invoicecode"] = tb.Text;
            RadLabel lbl = ((RadLabel)gvInvoice.Items[rowindex].FindControl("lblInvoiceid"));
            if (lbl != null)
                PhoenixAccountsVoucher.VoucherNumber = lbl.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
}
