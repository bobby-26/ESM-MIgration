using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsCreditDebitNoteMaster : PhoenixBasePage
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
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitNoteMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvCreditDebitNote')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitNoteSearch.aspx", "Find", "search.png", "FIND");

            MenuCreditDebitNote.AccessRights = this.ViewState;
            MenuCreditDebitNote.MenuList = toolbargrid.Show();
            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Credit Notes", "CREDITNOTE");
            //toolbarmain.AddButton("History", "HISTORY");
            //MenuCreditDebitNoteTab.AccessRights = this.ViewState;
            //MenuCreditDebitNoteTab.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["creditdebitnoteid"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["TabFlag"] = null;

                ViewState["VOUCHERNUMBER"] = "";
                if (Request.QueryString["creditdebitnoteid"] != null)
                {
                    ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"].ToString();
                }
                gvCreditDebitNote.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCreditDebitNoteTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            CreditNoteEdit();
            if (CommandName.ToUpper().Equals("CREDITNOTE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCreditDebitNote.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"];
            }
            if (CommandName.ToUpper().Equals("REBATE RECEIVABLE"))
            {
                Response.Redirect("../Accounts/AccountsCreditDebitNoteRebateReceivable.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["creditdebitnoteid"] != null && ViewState["creditdebitnoteid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsCreditDebitLineItemsDetails.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["creditdebitnoteid"] != null && ViewState["creditdebitnoteid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsCreditDebitHistory.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString() + "&qfrom=creditdebitnote", false);
            }
            else
                MenuCreditDebitNoteTab.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CreditNoteEdit()
    {
        if (ViewState["creditdebitnoteid"] != null)
        {
            DataSet ds = PhoenixAccountsCreditDebitNote.EditCreditDebitNote(new Guid(ViewState["creditdebitnoteid"].ToString()));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["AMOUNT"] = string.Format(String.Format("{0:##,###,###.000000}", dr["FLDAMOUNT"].ToString()));
                    ViewState["CURRENCYCODE"] = dr["FLDCURRENCYCODE"].ToString();
                    ViewState["CURRENCYNAME"] = dr["FLDCURRENCYNAME"].ToString();
                    ViewState["SUPPLIERCODE"] = dr["FLDSUPPLIERCODE"].ToString();
                    ViewState["VOUCHERNUMBER"] = dr["FLDVOUCHERNUMBER"].ToString();
                    ViewState["SUPPLIERNAME"] = dr["FLDCODE"].ToString() + '/' + dr["FLDSUPPLIERNAME"].ToString();

                }
            }
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
                                "Vendor Credit Note No",
                                "Currency",
                                "Amount",
                                "Credit Note Register No",
                                "Status"
                              };

        string[] alColumns = {
                                "FLDADDRESSCODE",
                                "FLDSUPPLIERNAME",
                                "FLDREFERENCENO",
                                "FLDCURRENCYCODE",
                                "FLDAMOUNT",
                                "FLDCNREGISTERNO",
                                "FLDSTATUSNAME"
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


        NameValueCollection nvc = Filter.CreditDebitNoteFilter;


        ds = PhoenixAccountsCreditDebitNote.CreditDebitNoteSearch(
              nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtCreditRegisterNo")) : string.Empty
            , nvc != null ? General.GetNullableString(nvc.Get("txtVendorCreditNoteNo")) : string.Empty
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedToDate")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : string.Empty
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
            , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CreditNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Credit Note</h3></td>");
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

    protected void MenuCreditDebitNote_TabStripCommand(object sender, EventArgs e)
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


        NameValueCollection nvc = Filter.CreditDebitNoteFilter;

        ds = PhoenixAccountsCreditDebitNote.CreditDebitNoteSearch(
                                                  nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtCreditRegisterNo")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVendorCreditNoteNo")) : string.Empty
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedFromDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedToDate")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : string.Empty
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , gvCreditDebitNote.PageSize
                                                , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Vendor Credit Note No",
                                "Currency",
                                "Amount",
                                "Credit Note Register No",
                                "Status"
                              };

        string[] alColumns = {
                                "FLDADDRESSCODE",
                                "FLDSUPPLIERNAME",
                                "FLDREFERENCENO",
                                "FLDCURRENCYCODE",
                                "FLDAMOUNT",
                                "FLDCNREGISTERNO",
                                "FLDSTATUSNAME"
                             };

        General.SetPrintOptions("gvCreditDebitNote", "Credit Note", alCaptions, alColumns, ds);

        gvCreditDebitNote.DataSource = ds;
        gvCreditDebitNote.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["creditdebitnoteid"] == null)
            {
                ViewState["creditdebitnoteid"] = ds.Tables[0].Rows[0]["FLDCREDITDEBITNOTEID"].ToString();
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCreditDebitNote.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCreditDebitNote.aspx?creditdebitnoteid=";
            }
            DataTable dt = ds.Tables[0];
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        CreditNoteEdit();
    }

    private void SetRowSelection()
    {
        if (ViewState["creditdebitnoteid"] != null)
        {
            gvCreditDebitNote.SelectedIndexes.Clear();
            foreach (GridDataItem item in gvCreditDebitNote.Items)
            {
                if (item.GetDataKeyValue("FLDCREDITDEBITNOTEID").ToString().Equals(ViewState["creditdebitnoteid"].ToString()))
                {
                    gvCreditDebitNote.SelectedIndexes.Add(item.ItemIndex);
                    ViewState["TabFlag"] = ((RadLabel)gvCreditDebitNote.Items[item.ItemIndex].FindControl("lbltabflag")).Text;
                    TabStripBind(ViewState["TabFlag"].ToString());
                }
            }
        }
    }

    protected void gvCreditDebitNote_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCreditDebitNote, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCreditDebitNote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCreditDebitNote.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditDebitNote_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "'); return false;");
            }

            ImageButton cf = (ImageButton)e.Item.FindControl("cmdPost");
            if (cf != null)
            {
                cf.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Post ?'); return false;");
                cf.Visible = SessionUtil.CanAccess(this.ViewState, cf.CommandName);
            }

            ImageButton cmdfndrq = (ImageButton)e.Item.FindControl("cmdfndrq");
            if (cmdfndrq != null)
            {
                if (drv["FLDSTATUS"].ToString() != "1379" && drv["FLDSTATUS"].ToString() != "1380")
                {
                    cmdfndrq.Visible = false;

                }
            }
            //RadCheckBox VoucherNumber = (RadCheckBox)e.Item.FindControl("chkAdjustInvoice");
            //RadCheckBox VoucherNumber1 = (RadCheckBox)e.Item.FindControl("chkAdjustInvoice1");
            //if (VoucherNumber !=null && VoucherNumber1 != null && ViewState["VOUCHERNUMBER"].ToString() != "" )
            //{
            //    VoucherNumber1.Enabled = true;
            //    VoucherNumber.Enabled = false;
            //}
            //else
            //{               
            //}
        }
    }

    protected void gvCreditDebitNote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("POST"))
            {
                string txtExchangeRate = "0";
                string txt2ndExchangeRate = "0";

                string txtReferenceNumber = ((RadLabel)e.Item.FindControl("lblRefNumber")).Text;
                string txtCreditDebitNoteId = ((RadLabel)e.Item.FindControl("lblCreditDebitNoteId")).Text;
                string txtCurrencyCode = ((RadLabel)e.Item.FindControl("lblCurrencyCode")).Text;
                string txtCompanyId = ((RadLabel)e.Item.FindControl("lblCompanyId")).Text;
                string txtAmount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;

                DataSet ds = PhoenixRegistersCompany.EditCompany(int.Parse(txtCompanyId));
                if (ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));

                    DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
                    DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];

                    decimal dTransactionExchangerate = 0;

                    DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(txtCurrencyCode), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    if (dsInvoice.Tables.Count > 0)
                    {
                        DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                        dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                    }


                    txtExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
                    txt2ndExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));

                }

                PhoenixAccountsCreditDebitNote.CreditDebitNoteVoucherInsert(new Guid(txtCreditDebitNoteId)
                                                                            , int.Parse(txtCompanyId)
                                                                            , int.Parse(txtCurrencyCode)
                                                                            , null
                                                                            , decimal.Parse(txtExchangeRate)
                                                                            , decimal.Parse(txt2ndExchangeRate)
                                                                            , decimal.Parse(txtAmount)
                                                                            , 74
                                                                            , 0
                                                                            , DateTime.Parse(DateTime.Now.ToShortDateString())
                                                                            , txtReferenceNumber
                                                                            , 0
                                                                            , string.Empty
                                                                            , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , string.Empty
                                                                            );
                //ucStatus.Text = "Voucher information added";
                Rebind();
            }
             if (e.CommandName.ToUpper().Equals("CREDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno;
                iRowno = e.Item.ItemIndex;
                BindPageURL(iRowno);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string txtReferenceNumber = ((RadLabel)e.Item.FindControl("lblRefNumber")).Text;
                string txtCreditDebitNoteId = ((RadLabel)e.Item.FindControl("lblCreditDebitNoteId")).Text;
                string txtCurrencyCode = ((RadLabel)e.Item.FindControl("lblCurrencyCode")).Text;
                string txtCompanyId = ((RadLabel)e.Item.FindControl("lblCompanyId")).Text;
                string txtAmount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;


                PhoenixAccountsCreditDebitNote.CreditDebitNoteAdjustInvoiceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(((RadLabel)e.Item.FindControl("lblCreditDebitNoteId")).Text)
                                                                    , (((RadCheckBox)e.Item.FindControl("chkAdjustInvoice")).Checked) == true ? 1 : 0);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CREATEFUNDREQUEST"))
            {



                string txtReferenceNumber = ((RadLabel)e.Item.FindControl("lblRefNumber")).Text;
                string txtCreditDebitNoteId = ((RadLabel)e.Item.FindControl("lblCreditDebitNoteId")).Text;
                string txtCurrencyCode = ((RadLabel)e.Item.FindControl("lblCurrencyCode")).Text;
                string txtCompanyId = ((RadLabel)e.Item.FindControl("lblCompanyId")).Text;
                string txtAmount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;


                //String scriptpopup = String.Format(
                //   "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsCreateFundRequest.aspx?creditnoteid=" + ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCreditDebitNoteId")).Text + "');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                Response.Redirect("../Accounts/AccountsCreateFundRequest.aspx?creditnoteid=" + ((RadLabel)e.Item.FindControl("lblCreditDebitNoteId")).Text);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
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
            ViewState["creditdebitnoteid"] = ((RadLabel)gvCreditDebitNote.Items[rowindex].FindControl("lblCreditDebitNoteId")).Text;
            ViewState["TabFlag"] = ((RadLabel)gvCreditDebitNote.Items[rowindex].FindControl("lbltabflag")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCreditDebitNote.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString();
            TabStripBind(ViewState["TabFlag"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCreditDebitNote.SelectedIndexes.Clear();
        gvCreditDebitNote.EditIndexes.Clear();
        gvCreditDebitNote.DataSource = null;
        gvCreditDebitNote.Rebind();
    }
    protected void gvCreditDebitNote_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvCreditDebitNote.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
    public void TabStripBind(string flag)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Credit Notes", "CREDITNOTE");
        if (flag == "1" || flag == "0")
            toolbarmain.AddButton("Line Items", "LINEITEM");
        if (flag == "2" || flag == "0")
            toolbarmain.AddButton("Rebate Receivable", "REBATE RECEIVABLE");
        toolbarmain.AddButton("History", "HISTORY");

        MenuCreditDebitNoteTab.AccessRights = this.ViewState;
        MenuCreditDebitNoteTab.MenuList = toolbarmain.Show();

        if ((flag == "2" || flag == "0" || flag == "1"))
            MenuCreditDebitNoteTab.SelectedMenuIndex = 0;
        else
            MenuCreditDebitNoteTab.SelectedMenuIndex = 0;

    }
}
