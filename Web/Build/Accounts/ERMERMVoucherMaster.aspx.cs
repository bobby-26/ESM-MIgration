using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class ERMERMVoucherMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/ERMERMVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../Accounts/ERMERMdisplayVoucherFilter.aspx'); return false;", "Find", "search.png", "FIND");
            //toolbargrid.AddImageButton("../Accounts/AccountsCreditNoteVoucherMaster.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
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
                ViewState["PAGEURL"] = null;
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucher.aspx?voucherid=" + ViewState["voucherid"];
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
                ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucher.aspx?voucherid=" + ViewState["voucherid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/ERMERMVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["voucherid"].ToString());
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
                                "TRANSFERCODE					",
                                "CREATEDDATE				 	",
                                "UPDATEDDATE				 	",
                                "ZID						",
                                "ERMVOUCHER				 	",
                                "REFERENCEDOCUMENTNO		 		",
                                "VOUCHERDATE				 	",
                                "ERMLONG 					",
                                "ERMYEAR					",
                                "ERMPER					",
                                "ERMSTATUS					",
                                "ERMDATEDUE				 	",
                                "ERMACCESS					",
                                "ERMTEAM					",
                                "ERMMEMBER					",
                                "ERMMANAGER				 	",
                                "ERMVOUCHERPAY				 	",
                                "VOUCHERNUMBER			 	",
                                "COMPANYNAME				 	",
                                "VOUCHERSTATUS			 	",
                                "CREATEDBY				 	",
                                "UPDATEBY				 	",
                                "LONGDESCRIPTION		"

                              };

        string[] alColumns = {
                               "FLDTRANSFERCODE",
                                "FLDCREATEDDATE",
                                "FLDUPDATEDDATE",
                                "FLDZID",
                                "FLDXVOUCHER",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDVOUCHERDATE",
                                "FLDXLONG",
                                "FLDXYEAR",
                                "FLDXPER",
                                "FLDXSTATUS",
                                "FLDXDATEDUE",
                                "FLDXACCESS",
                                "FLDXTEAM",
                                "FLDXMEMBER",
                                "FLDXMANAGER",
                                "FLDXVOUCHERPAY",
                                "FLDVOUCHERNUMBER",
                                "FLDCOMPANYNAME",
                                "FLDVOUCHERSTATUS",
                                "FLDCREATEDBY",
                                "FLDUPDATEBY",
                                "FLDLONGDESCRIPTION"
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
        ds = PhoenixAccountsVoucher.ERMVoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtErmvoucherno")) : string.Empty
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtEsmvoucherno")) : string.Empty
                                                  , null
                                                  , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                  , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                  , null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                   , ref iRowCount, ref iTotalPageCount

                                                   );



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
                BindData();
            }
            //else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            //{

            //    Filter.CurrentInvoiceSelection = null;
            //    BindData();
            //    SetPageNavigator();
            //}
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

        NameValueCollection nvc = Filter.CurrentSelectedERMVoucherfilter;

        ds = PhoenixAccountsVoucher.ERMVoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtErmvoucherno")) : string.Empty
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtEsmvoucherno")) : string.Empty
                                                   , null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , gvFormDetails.PageSize
                                                   , ref iRowCount, ref iTotalPageCount

                                                  );

        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Company Name",
                                "Voucher Type",
                                "Voucher Year",


                              };

        string[] alColumns = {
                                "FLDXVOUCHER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDCOMPANYNAME",
                                "FLDSUBVOUCHERTYPE",
                                "FLDXYEAR",


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
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDXVOUCHER"].ToString();
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucher.aspx?voucherid=";
            }
        }

    }


    private void SetRowSelection()
    {
        gvFormDetails.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvFormDetails.Items)
        {
            if (item.GetDataKeyValue("FLDXVOUCHER").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                gvFormDetails.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvFormDetails.Items[item.ItemIndex].FindControl("lnkVoucherId")).Text;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvFormDetails.SelectedIndexes.Clear();
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

    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView drv = (DataRowView)e.Row.DataItem;
        //    Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
        //    ImageButton cmdAttachment = (ImageButton)e.Row.FindControl("cmdAttachment");
        //    if (cmdAttachment != null)
        //    {
        //        cmdAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
        //                            + PhoenixModule.ACCOUNTS + "');return true;");
        //        cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
        //    }
        //    ImageButton cmdNoAttachment = (ImageButton)e.Row.FindControl("cmdNoAttachment");
        //    if (cmdNoAttachment != null)
        //    {
        //        cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
        //                            + PhoenixModule.ACCOUNTS + "');return true;");
        //        cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
        //    }


        //    ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
        //    ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
        //    if (iab != null) iab.Visible = true;
        //    if (inab != null) inab.Visible = false;
        //    int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
        //    if (n == 0)
        //    {
        //        if (iab != null) iab.Visible = false;
        //        if (inab != null) inab.Visible = true;
        //    }
        //}
    }

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["voucherid"] = ((LinkButton)gvFormDetails.Items[rowindex].FindControl("lnkVoucherId")).CommandArgument;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvFormDetails.Items[rowindex].FindControl("lnkVoucherId")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvFormDetails.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
}
