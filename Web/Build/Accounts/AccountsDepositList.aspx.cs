using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsDepositList : PhoenixBasePage
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
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsDepositList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDeposit')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountDepositFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsDepositList.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuDepositList.AccessRights = this.ViewState;
            MenuDepositList.MenuList = toolbargrid.Show();
            //MenuDepositList.SetTrigger(pnlDeposit);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Deposit", "DEPOSIT", ToolBarDirection.Right);            

            MenuDepositTab.AccessRights = this.ViewState;
            MenuDepositTab.MenuList = toolbarmain.Show();
            


            if (!IsPostBack)
            {
                Session["New"] = "N";
                //ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DEPOSITID"] = null;
                ViewState["PAGEURL"] = null;

                gvDeposit.PageSize = General.ShowRecords(gvDeposit.PageSize);

                if (Request.QueryString["DEPOSITID"] != null)
                {
                    ViewState["DEPOSITID"] = Request.QueryString["DEPOSITID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsDeposit.aspx?DEPOSITID=" + ViewState["DEPOSITID"];
                }
                MenuDepositTab.SelectedMenuIndex = 1;
            }


            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeposit_Sorting(object sender, GridSortCommandEventArgs se)
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
        gvDeposit.Rebind();
    }

    protected void MenuDepositTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DEPOSIT"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsDeposit.aspx?depositid=" + ViewState["DEPOSITID"];
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["DEPOSITID"] != null && ViewState["DEPOSITID"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsDepositHistory.aspx?depositid=" + ViewState["DEPOSITID"].ToString() + "&qfrom=deposit", false);
            }
            else
                MenuDepositTab.SelectedMenuIndex = 1;
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

        string[] alColumns = { "FLDDEPOSITNUMBER", "FLDNAME", "FLDDATE", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDDEPOSITSTATUS", "FLDPAYMENTVOUCHERNUMBER", "FLDCREDITNOTENUMBER", "FLDCREDITNOTESTATUS" };
        string[] alCaptions = { "Deposit No", "Supplier Name", "Date", "Currency", "Amount", "Deposit Status", "Payment Voucher", "CNo Register No", "Credit Note Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAdvancePaymentSelection;


        ds = PhoenixAccountsDeposit.DepositSearch(nvc != null ? General.GetNullableString(nvc.Get("txtDepositNumber")) : null
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDepositStatus")) : null
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtDepositFromdate")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtDepositTodate")) : null
                                                  , null
                                                  , sortexpression
                                                  , sortdirection
                                                  , gvDeposit.CurrentPageIndex + 1
                                                  , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                  , ref iRowCount
                                                  , ref iTotalPageCount
                                                  );


        Response.AddHeader("Content-Disposition", "attachment; filename=Deposit.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Deposit</h3></td>");
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

    protected void MenuDepositList_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                Filter.CurrentAdvancePaymentSelection = null;
                gvDeposit.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeposit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {

        string[] alColumns = { "FLDDEPOSITNUMBER", "FLDNAME", "FLDDATE", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDDEPOSITSTATUS", "FLDPAYMENTVOUCHERNUMBER", "FLDCREDITNOTENUMBER", "FLDCREDITNOTESTATUS" };
        string[] alCaptions = { "Deposit No", "Supplier Name", "Date", "Currency", "Amount", "Deposit Status", "Payment Voucher", "CNo Register No", "Credit Note Status" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAdvancePaymentSelection;

        ds = PhoenixAccountsDeposit.DepositSearch(nvc != null ? General.GetNullableString(nvc.Get("txtDepositNumber")) : null
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDepositStatus")) : null
                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtDepositFromdate")) : null
                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtDepositTodate")) : null
                                                  , null
                                                  , sortexpression
                                                  , sortdirection
                                                  , gvDeposit.CurrentPageIndex + 1
                                                  , gvDeposit.PageSize
                                                  , ref iRowCount
                                                  , ref iTotalPageCount
                                                  );

        General.SetPrintOptions("gvDeposit", "Deposit", alCaptions, alColumns, ds);

        gvDeposit.DataSource = ds;
        gvDeposit.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["DEPOSITID"] == null)
            {
                ViewState["DEPOSITID"] = ds.Tables[0].Rows[0]["FLDDEPOSITID"].ToString();
                //gvDeposit.SelectedIndex = 0;
            }
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsDeposit.aspx?DEPOSITID=" + ViewState["DEPOSITID"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsDeposit.aspx";
            }
        }

    }

    private void SetRowSelection()
    {
        gvDeposit.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDeposit.Items)
        {

            if (item.GetDataKeyValue("FLDDEPOSITID").ToString().Equals(ViewState["DEPOSITID"].ToString()))
            {
                gvDeposit.SelectedIndexes.Add(item.ItemIndex);
               //PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvDeposit.Items[item.ItemIndex].FindControl("lblDepositid")).Text;
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }

    protected void gvDeposit_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            RadLabel lblTypeID = (RadLabel)e.Item.FindControl("lblTypeID");

            ImageButton cmdNoAtt = (ImageButton)e.Item.FindControl("cmdNoAtt");
            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");

            int? attachmentFlag = General.GetNullableInteger(drv["FLDATTACHMENTFLAG"].ToString());

            if (cmdAtt != null)
            {
                if (lblIsAtt.Text == "1")
                {
                    cmdAtt.Visible = true;
                    cmdNoAtt.Visible = false;
                }
                if (attachmentFlag == 0)
                {
                    cmdAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.ACCOUNTS + "');return true;");
                }
                else
                {
                    cmdAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.ACCOUNTS + "&u=1" + "');return true;");

                }
            }
            if (cmdNoAtt != null)
            {
                if (lblIsAtt.Text == string.Empty)
                {
                    cmdAtt.Visible = false;
                    cmdNoAtt.Visible = true;
                }
                if (attachmentFlag == 0)
                {
                    cmdNoAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.ACCOUNTS + "');return true;");
                }
                else
                {
                    cmdNoAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.ACCOUNTS + "&u=1" + "');return true;");

                }
            }

            RadLabel lblDepositid = (RadLabel)e.Item.FindControl("lblDepositid");
            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName)) cb.Visible = false;


            //ImageButton cmdApprovalHistory = (ImageButton)e.Row.FindControl("cmdApprovalHistory");
            //if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "583")
            //{
            //    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDQUOTATIONID"].ToString() + "'); return false;");
            //}
            //else if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "1233")
            //{
            //    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?potype=DirectPO&docid=" + drv["FLDORDERID"].ToString() + "'); return false;");
            //}
            //else if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "968")
            //{
            //    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?potype=BondProvision&docid=" + drv["FLDORDERID"].ToString() + "'); return false;");
            //}
            //else
            //{
            //    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
            //}

            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            //    LinkButton ll = (LinkButton)e.Row.FindControl("lblLeave");
            //    Label lb = (Label)e.Row.FindControl("lblOrderid");
            //    Label l = (Label)e.Row.FindControl("lblTypeID");
            //    Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");
            //    if (!SessionUtil.CanAccess(this.ViewState, ll.CommandName)) ll.Visible = false;
            //    if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")))
            //        ll.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Crew/CrewLicenceRequestLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + "');return false;");
            //    if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")))
            //        ll.Attributes.Add("onclick", "parent.Openpopup('codehelp1','','../Purchase/PurchaseFormLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + " +&vesselid=" + lblVesselId.Text + "');return false;");

            //}

        }
    }

    protected void gvDeposit_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                string depositid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblDepositid")).Text;
                BindPageURL(nCurrentRow);
                SetRowSelection();
                PhoenixAccountsDeposit.DepositCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(depositid));
                //BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        //if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        //{
        //    Response.Redirect("../Accounts/AccountsAdvancePaymentAttachments.aspx?qinvoicecode=" + ((TextBox)_gridView.Rows[iRowno].FindControl("txtAdvancePaymentCode")).Text + "&qfrom=INVOICE");
        //}
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvDeposit.Rebind();
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
            GridDataItem item = (GridDataItem)gvDeposit.Items[rowindex];
            ViewState["DEPOSITID"] = item.GetDataKeyValue("FLDDEPOSITID"); ;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsDeposit.aspx?DEPOSITID=" + ViewState["DEPOSITID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeposit_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvDeposit.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }

}
