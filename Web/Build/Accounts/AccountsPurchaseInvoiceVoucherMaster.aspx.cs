using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsPurchaseInvoiceVoucherMaster : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsPurchaseInvoiceVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Accounts/AccountsPurchaseInvoiceVoucherFilter.aspx'); return false;", "Find", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Voucher";
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            MenuOrderFormMain.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
              //  ViewState["URL"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=";
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=" + ViewState["voucherid"];
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_Sorting(object sender, GridSortCommandEventArgs e)
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

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                ViewState["URL"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=";
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=" + ViewState["voucherid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["voucherid"]);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=purchaseinvoice");
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;
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
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Sub Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"
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

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;
        ds = PhoenixAccountsVoucher.VoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 72
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : string.Empty
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                   , null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus").ToString()) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , 1
                                                   , iRowCount
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlVoucherUserSearch")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtLongDescription")) : string.Empty
                                                   );

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Purchase Invoice Voucher</h3></td>");
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
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;
        ds = PhoenixAccountsVoucher.VoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 72
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : string.Empty
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                   , null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus").ToString()) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , gvFormDetails.CurrentPageIndex + 1
                                                   , gvFormDetails.PageSize
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlVoucherUserSearch")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtLongDescription")) : string.Empty
                                                    );

        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Sub Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"
                             };
        General.SetPrintOptions("gvVoucher", "Purchase Invoice Voucher", alCaptions, alColumns, ds);


        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        //if (ds.Tables[0].Rows.Count > 0 && ViewState["voucherid"] == null)
        //{
        //    ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
        //    ifMoreInfo.Attributes["src"] = ViewState["URL"] + ViewState["voucherid"].ToString();
        //}
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=";
            }
        }
    }

    private void SetRowSelection()
    {
        gvFormDetails.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvFormDetails.Items)
        {
            if (item.GetDataKeyValue("FLDVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                PhoenixAccountsVoucher.VoucherNumber = item.GetDataKeyValue("FLDVOUCHERNUMBER").ToString();
                gvFormDetails.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void Rebind()
    {
        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }

    protected void gvFormDetails_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsFileAttachment.aspx?dtkey=" + lblDtkey + "&mod=" + PhoenixModule.ACCOUNTS + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }


            ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = false;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            if (n == 0)
            {
                if (iab != null) iab.Visible = false;
                if (inab != null) inab.Visible = true;
            }
        }
    }

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        // int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindPageURL(e.Item.ItemIndex);
            SetRowSelection();
        }

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            BindPageURL(e.Item.ItemIndex);
            SetRowSelection();
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
          //  if (gvFormDetails.Items.Count > 0)
                BindPageURL(0);
          //  gvFormDetails.Rebind();
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvFormDetails.Items[rowindex];
            ViewState["voucherid"] = ((LinkButton)item.FindControl("lnkVoucherId")).CommandArgument;
            PhoenixAccountsVoucher.VoucherNumber = item.GetDataKeyValue("FLDVOUCHERNUMBER").ToString();
            gvFormDetails.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ViewState["voucherid"] = null;

    }

    protected void gvFormDetails_PreRender1(object sender, EventArgs e)
    {
      //  SetRowSelection();
    }
}
