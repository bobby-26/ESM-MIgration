using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsBankPaymentVoucherMaster : PhoenixBasePage
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
            toolbargrid.AddImageButton("../Accounts/AccountsBankPaymentVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Accounts/AccountsBankPaymentVoucherFilter.aspx'); return false;", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            MenuOrderFormMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["type"] = "";
                ViewState["PAGEURL"] = null;
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    if (Request.QueryString["type"] != null)
                    {
                        ViewState["type"] = Request.QueryString["type"].ToString();
                    }
                    else
                    {
                        ViewState["type"] = "";
                    }
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsBankPaymentVoucher.aspx?voucherid=" + ViewState["voucherid"] + "&type=" + ViewState["type"];
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
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsBankPaymentVoucher.aspx?voucherid=" + ViewState["voucherid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {                
                Response.Redirect("../Accounts/AccountsBankPaymentVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["type"].ToString() +"&Vouchernumber=" + PhoenixAccountsVoucher.VoucherNumber);
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Long Description",
                                "Company Name",
                                "Sub Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDLONGDESCRIPTION",
                                "FLDCOMPANYNAME",
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


        ds = PhoenixAccountsVoucher.BankVoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 68
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : string.Empty
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                   , null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , 1
                                                   , iRowCount
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtLongDescription")) : string.Empty
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBankAccount")) : null);



        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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
        ds = PhoenixAccountsVoucher.BankVoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 68
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : string.Empty
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                   , null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus").ToString()) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                   , gvFormDetails.PageSize
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtLongDescription")) : string.Empty
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBankAccount")) : null);

        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Long Description",
                                "Company Name",
                                "Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDLONGDESCRIPTION",
                                "FLDCOMPANYNAME",
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"
                             };
        General.SetPrintOptions("gvVoucher", "Accounts Voucher", alCaptions, alColumns, ds);

        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();                
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsBankPaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["type"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsBankPaymentVoucher.aspx?voucherid=";
            }
        }

    }
    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','Accounts/AccountsFileAttachment.aspx?dtkey=" + lblDtkey + "&mod=" + PhoenixModule.ACCOUNTS + "&voucherid=" + ViewState["voucherid"] + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','Accounts/AccountsFileAttachment.aspx?dtkey=" + lblDtkey + "&mod=" + PhoenixModule.ACCOUNTS + "&voucherid=" + ViewState["voucherid"] + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFormDetails.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
                BindPageURL(0);
        }
    }
    protected void gvFormDetails_PreRender(object sender, EventArgs e)
    {
        //SetRowSelection();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvFormDetails.Items[rowindex];

            ViewState["voucherid"] = ((LinkButton)item.FindControl("lnkVoucherId")).CommandArgument;
            PhoenixAccountsVoucher.VoucherNumber = item.GetDataKeyValue("FLDVOUCHERNUMBER").ToString();
            gvFormDetails.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsBankPaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
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

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

       // int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixAccountsVoucher.VoucherNumber = item.GetDataKeyValue("FLDVOUCHERNUMBER").ToString();
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
    protected void Rebind()
    {
        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }

    protected void gvFormDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ViewState["voucherid"] = null;
    }
}
