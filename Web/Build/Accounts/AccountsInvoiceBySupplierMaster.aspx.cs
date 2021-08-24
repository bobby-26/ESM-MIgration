using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsInvoiceBySupplierMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            //MenuOrderForm.SetTrigger(pnlOrderForm);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");

            if (!IsPostBack)
            {
                Session["New"] = "N";

                //MenuOrderFormMain.SetTrigger(pnlOrderForm);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
                gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                // MenuOrderFormMain.Title = "Invoice Register";
                if (Request.QueryString["qinvoicecode"] != null)
                {
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice", "INVOICE");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            //toolbarmain.AddButton("PO", "PO");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (ViewState["invoicecode"] != null)
            {
                if (CommandName.ToUpper().Equals("INVOICE"))
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
                if (CommandName.ToUpper().Equals("PO"))
                {
                    DataSet DsActualInvoiceCode = PhoenixAccountsInvoice.InvoiceBySupplierEdit(new Guid(ViewState["invoicecode"].ToString()));
                    if (DsActualInvoiceCode.Tables[0].Rows.Count > 0)
                    {
                        if (DsActualInvoiceCode.Tables[0].Rows[0]["FLDACTUALINVOICECODE"].ToString() != "")
                            Response.Redirect("../Accounts/AccountsInvoiceBySupplierLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoice");
                        else
                        {
                            ucError.ErrorMessage = "Please confirm the invoice before adding the PO";
                            ucError.Visible = true;
                        }
                    }
                }
                if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
                {
                    Response.Redirect("../Accounts/AccountsInvoiceBySupplierAttachments.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoice");
                }
                else
                    MenuOrderFormMain.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInvoice_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoice.CurrentPageIndex + 1;
            BindData();
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
                                "Invoice Reference",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUS",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
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

        NameValueCollection nvc = Filter.CurrentInvoiceBySupplierSelection;

        ds = PhoenixAccountsInvoice.InvoiceBySupplierSearch(
            General.GetNullableString(nvc != null ? nvc.Get("ddlInvoiceStatus") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceNumberSearch") : ""),
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableString(nvc != null ? nvc.Get("txtSupplierReferenceSearch") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceFromdateSearch") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceTodateSearch") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId") : "")
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceBySupplierSelection;

        ds = PhoenixAccountsInvoice.InvoiceBySupplierSearch(
            General.GetNullableString(nvc != null ? nvc.Get("ddlInvoiceStatus") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceNumberSearch") : ""),
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableString(nvc != null ? nvc.Get("txtSupplierReferenceSearch") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceFromdateSearch") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceTodateSearch") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId") : "")
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
            , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUS",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                             };

        General.SetPrintOptions("gvInvoice", "Accounts Invoice", alCaptions, alColumns, ds);

        gvInvoice.DataSource = ds;
        gvInvoice.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["ROWCOUNT"] = iRowCount;

            if (ViewState["invoicecode"] == null)
            {
                ViewState["invoicecode"] = ds.Tables[0].Rows[0]["FLDINVOICECODE"].ToString();
                //gvInvoice.SelectedIndex = 0;
            }
             SetRowSelection();
        }


        // if (!IsPostBack)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                ViewState["SupId"] = ds.Tables[1].Rows[0]["FLDSUPPLIERID"].ToString();
                ViewState["SupCode"] = ds.Tables[1].Rows[0]["FLDSUPPLIERSHORTCODE"].ToString();
                ViewState["SupName"] = ds.Tables[1].Rows[0]["FLDSUPPLIERNAME"].ToString();
                ViewState["SupLoginYN"] = ds.Tables[1].Rows[0]["FLDISSUPPLIERLOGIN"].ToString();

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");

                if (ViewState["SupLoginYN"].ToString().Equals("1"))
                {
                    toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierFilter.aspx?qcallfrom=SUPPLIERPORTAL", "Find", "search.png", "FIND");

                    if (ViewState["PAGEURL"] == null)
                    {
                        if (ViewState["invoicecode"] != null)
                        {
                            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString()
                                + "&supid=" + ViewState["SupId"].ToString()
                                + "&supcode=" + ViewState["SupCode"].ToString() + "&supname="
                                + ViewState["SupName"].ToString() + "&issupplierlogin=" + ViewState["SupLoginYN"].ToString();
                        }
                        else
                        {
                            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?supid=" + ViewState["SupId"].ToString()
                                + "&supcode=" + ViewState["SupCode"].ToString() + "&supname="
                                + ViewState["SupName"].ToString() + "&issupplierlogin=" + ViewState["SupLoginYN"].ToString();
                        }
                    }
                }
                else
                {
                    toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBySupplierFilter.aspx?qcallfrom=OFFICE", "Find", "search.png", "FIND");

                    if (ViewState["PAGEURL"] == null)
                    {
                        if (ViewState["invoicecode"] != null)
                        {
                            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString()
                                + "&issupplierlogin=" + ViewState["SupLoginYN"].ToString();
                        }
                        else
                        {
                            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?issupplierlogin="
                                + ViewState["SupLoginYN"].ToString();
                        }
                    }
                }

                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }
        }

        
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            //  gvInvoice.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                    cmdEdit.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Item.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName))
                    imgAttachment.Visible = false;
        }

        if (e.Item is GridEditableItem)
        {
            if (e.Item.DataItem != null)
            {
                ImageButton img1 = (ImageButton)e.Item.FindControl("imgAttachment");
                RadLabel lblControl = (RadLabel)e.Item.Cells[2].FindControl("lblAttachmentexists");
                if (lblControl.Text == "1")
                {
                    img1.Visible = true;
                }
                else { img1.Visible = false; }
            }
        }
    }

    protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {

        RadGrid _gridView = (RadGrid)sender;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            SetRowSelection();
            BindPageURL(iRowno);
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();

        if (Session["New"].ToString() == "Y")
        {
            // gvInvoice.SelectedIndex = 0;
            Session["New"] = "N";
            //  BindPageURL(gvInvoice.SelectedIndex);
        }
    }

      private void BindPageURL(int rowindex)
      {
          try
          {
              RadTextBox tb = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode"));
    
              if (tb != null)
                  ViewState["invoicecode"] = tb.Text;
    
              if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
                  ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceBySupplier.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString()
                      + "&supid=" + ViewState["SupId"].ToString()
                      + "&supcode=" + ViewState["SupCode"].ToString() + "&supname="
                      + ViewState["SupName"].ToString() + "&issupplierlogin=" + ViewState["SupLoginYN"].ToString();
          }
          catch (Exception ex)
          {
              ucError.ErrorMessage = ex.Message;
              ucError.Visible = true;
          }
      }
    
      private void SetRowSelection()
      {
        gvInvoice.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvInvoice.Items)
        {
            if (item.GetDataKeyValue("FLDINVOICECODE").ToString().Equals(ViewState["invoicecode"].ToString()))
            {
                gvInvoice.SelectedIndexes.Add(item.ItemIndex);
                //PhoenixAccountsVoucher.InvoiceNumber = ((Label)gvInvoice.Rows[i].FindControl("lblInvoiceid")).Text;
            }
          }
          //if (gvInvoice.SelectedIndex == -1)
          //{
          //    gvInvoice.SelectedIndex = 0;
          //    ViewState["invoicecode"] = gvInvoice.DataKeys[0].Value.ToString();
          //    BindPageURL(0);
          //}
      }
    
      protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
      {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
      }
}
