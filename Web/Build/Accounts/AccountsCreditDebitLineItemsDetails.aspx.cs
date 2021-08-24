using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class AccountsCreditDebitLineItemsDetails : PhoenixBasePage
{
    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;

    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitLineItemsDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();



            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Credit Note", "VOUCHER");
            toolbarmain.AddButton("Line Items", "GENERAL");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            MenuLineItem.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["creditdebitnotelineitemid"] != null && Request.QueryString["creditdebitnotelineitemid"] != string.Empty)
                    ViewState["CREDITDEBITNOTELINEITEMCODE"] = Request.QueryString["creditdebitnotelineitemid"];
                ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"];

                //  Title1.Text = "Credit Note Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";
                if (Request.QueryString["creditdebitnoteid"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "AccountsCreditDebitLineItem.aspx?creditdebitnoteid=" + Request.QueryString["creditdebitnoteid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "AccountsCreditDebitLineItem.aspx";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["PAGEURL"] = "../Accounts/AccountsCreditDebitLineItem.aspx?creditdebitnoteid=";
            }
            else if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsCreditDebitNoteMaster.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString());
            }
            if (ViewState["creditdebitnotelineitemid"] != null)
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["creditdebitnoteid"] + "&creditdebitnotelineitemid=" + ViewState["creditdebitnotelineitemid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
            else
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["creditdebitnoteid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
        string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsCreditDebitNoteLineItems.CreditDebitNoteLineItemsSearch(
                                                                          new Guid(ViewState["creditdebitnoteid"].ToString())
                                                                         , null
                                                                         , null
                                                                         , string.Empty
                                                                         , string.Empty
                                                                         , null
                                                                         , null
                                                                         , sortdirection
                                                                         , sortexpression
                                                                         , (int)ViewState["PAGENUMBER"]
                                                                         , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount
                                                                         , ref TransactionAmountTotal
                                                                         , ref BaseAmountTotal
                                                                         , ref ReportAmountTotal
                                                                    );
        strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
        strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
        strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);

        DataTable dt1 = ds.Tables[0];
        foreach (DataRow row in dt1.Rows)
        {
            row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
            row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
            row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=VoucherLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voucher Line Item Details</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["creditdebitnoteid"] != null)
        {
            ds = PhoenixAccountsCreditDebitNoteLineItems.CreditDebitNoteLineItemsSearch(
                                                                    new Guid(ViewState["creditdebitnoteid"].ToString())
                                                                   , null
                                                                   , null
                                                                   , string.Empty
                                                                   , string.Empty
                                                                   , null
                                                                   , null
                                                                   , sortdirection
                                                                   , sortexpression
                                                                   , (int)ViewState["PAGENUMBER"]
                                                                   , gvLineItem.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount
                                                                   , ref TransactionAmountTotal
                                                                   , ref BaseAmountTotal
                                                                   , ref ReportAmountTotal
                                                              );
            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {

                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["ISPERIODLOCKED"] = dr["FLDISPERIODLOCKED"].ToString();

                if (ViewState["creditdebitnotelineitemid"] == null)
                {
                    ViewState["creditdebitnotelineitemid"] = ds.Tables[0].Rows[0]["FLDCREDITDEBITNOTELINEITEMID"].ToString();
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/AccountsCreditDebitLineItem.aspx?creditdebitnoteid=";
                }
                {
                    if (ViewState["creditdebitnotelineitemid"] != null)
                    {
                        string strRowno = string.Empty;
                        if (ViewState["rowno"] != null) { strRowno = ViewState["rowno"].ToString(); } else { strRowno = "10"; }
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["creditdebitnoteid"] + "&creditdebitnotelineitemid=" + ViewState["creditdebitnotelineitemid"].ToString() + "&rowno=" + strRowno + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                    }
                    else
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["creditdebitnoteid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                }

                DataTable dt1 = ds.Tables[0];
                foreach (DataRow row in dt1.Rows)
                {
                    row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
                    row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
                    row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                TransactionAmountTotal = 0;
                BaseAmountTotal = 0;
                ReportAmountTotal = 0;
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            // if (ViewState["ISPERIODLOCKED"] != null)
            // {
            //     if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 1)
            //     {
            //         for (int i = 0; i < gvLineItem.Items.Count; i++)
            //         {
            //             GridViewRow gvRow = gvLineItem.Rows[i];
            //             ((ImageButton)gvRow.FindControl("cmdEdit")).Visible = false;
            //             ((ImageButton)gvRow.FindControl("cmdDelete")).Visible = false;
            //             ((RadLabel)gvRow.FindControl("lblIsPeriodLocked")).Visible = true;
            //         }
            //     }
            // }

            string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
            string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
            General.SetPrintOptions("gvLineItem", "Voucher Line Item", alCaptions, alColumns, ds);
            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            RadLabel lblCreditDebitLineId = ((RadLabel)e.Item.FindControl("lblCreditDebitLineId"));

            try
            {
                if (lblCreditDebitLineId != null)
                {
                    if (lblCreditDebitLineId.Text != null)
                    {
                        string strVoucherLineId = lblCreditDebitLineId.Text;
                        PhoenixAccountsCreditDebitNoteLineItems.DeleteCreditDebitNoteLineItems(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strVoucherLineId));
                    }
                }
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            Rebind();
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno = Int32.Parse(e.CommandArgument.ToString());

            try
            {
                ViewState["creditdebitnotelineitemid"] = ((RadLabel)gvLineItem.Items[iRowno].FindControl("lblCreditDebitLineId")).Text;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCreditDebitLineItem.aspx?creditdebitnoteid=" + ViewState["creditdebitnoteid"].ToString() + "&creditdebitnotelineitemid=" + ViewState["creditdebitnotelineitemid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            int iRowno = Int32.Parse(e.CommandArgument.ToString());

            try
            {
                PhoenixAccountsCreditDebitNoteLineItems.UpdateCreditDebitNoteLineItems(new Guid(((RadLabel)gvLineItem.Items[iRowno].FindControl("lblCreditDebitLineId")).Text.ToString()), ((RadLabel)gvLineItem.Items[iRowno].FindControl("lblAccount")).Text.ToString(), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }


    //  protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    //  {
    //      try
    //      {
    //          GridView _gridView = (GridView)sender;
    //          _gridView.SelectedIndex = de.NewEditIndex;
    //          ViewState["creditdebitnotelineitemid"] = ((RadLabel)gvLineItem.Rows[de.NewEditIndex].FindControl("lblCreditDebitLineId")).Text;
    //          ViewState["rowno"] = ((LinkButton)gvLineItem.Rows[de.NewEditIndex].FindControl("lnkVoucherLineItemNo")).Text;
    //          if (ViewState["ISPERIODLOCKED"] != null)
    //          {
    //              if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 0)
    //              {
    //                  _gridView.EditIndex = de.NewEditIndex;
    //              }
    //              else
    //              {
    //                  _gridView.EditIndex = -1;
    //              }
    //          }
    //          BindData();
    //
    //      }
    //      catch (Exception ex)
    //      {
    //          ucError.ErrorMessage = ex.Message;
    //          ucError.Visible = true;
    //      }
    //  }


    protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    public double basedebittotal = 0.00;
    public double basecredittotal = 0.00;
    public double reportdebittotal = 0.00;
    public double reportcredittotal = 0.00;
    public string strBaseDebitTotal = string.Empty;
    public string strBaseCrebitTotal = string.Empty;
    public string strReportDebitTotal = string.Empty;
    public string strReportCrebitTotal = string.Empty;

    protected void gvLineItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            basedebittotal = 0.00;
            basecredittotal = 0.00;
            reportdebittotal = 0.00;
            reportcredittotal = 0.00;
        }

        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete"); RadLabel lblBaseAmount = (RadLabel)e.Item.FindControl("lblBaseAmount");
            RadLabel lblReportAmount = (RadLabel)e.Item.FindControl("lblReportAmount");

            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (lblBaseAmount != null && lblBaseAmount.Text != "")
            {
                if (Convert.ToDouble(lblBaseAmount.Text) <= 0)
                {
                    basecredittotal = basecredittotal + Convert.ToDouble(lblBaseAmount.Text);
                    strBaseCrebitTotal = String.Format("{0:n}", basecredittotal);
                    ViewState["basecredittotal"] = basecredittotal;
                }
                else
                {
                    basedebittotal = Math.Abs(basedebittotal) + Math.Abs(Convert.ToDouble(lblBaseAmount.Text));
                    strBaseDebitTotal = String.Format("{0:n}", basedebittotal);
                    ViewState["basedebittotal"] = basedebittotal;
                }
            }
            if (lblReportAmount != null && lblReportAmount.Text != "")
            {
                if (Convert.ToDouble(lblReportAmount.Text) <= 0)
                {
                    reportcredittotal = reportcredittotal + Convert.ToDouble(lblReportAmount.Text);
                    strReportCrebitTotal = String.Format("{0:n}", reportcredittotal);
                    ViewState["basecredittotal"] = reportcredittotal;
                }
                else
                {
                    reportdebittotal = Math.Abs(reportdebittotal) + Math.Abs(Convert.ToDouble(lblReportAmount.Text));
                    strReportDebitTotal = String.Format("{0:n}", reportdebittotal);
                    ViewState["basedebittotal"] = reportdebittotal;
                }
            }
            if (basedebittotal == 0.00 || strBaseDebitTotal == string.Empty)
                strBaseDebitTotal = "0.00";
            if (basecredittotal == 0.00 || strBaseCrebitTotal == string.Empty)
                strBaseCrebitTotal = "0.00";
            if (reportcredittotal == 0.00 || strReportCrebitTotal == string.Empty)
                strReportCrebitTotal = "0.00";
            if (reportdebittotal == 0.00 || strReportDebitTotal == string.Empty)
                strReportDebitTotal = "0.00";
        }

        if (e.Item is GridDataItem)
        {
            string strAccountActive = string.Empty;

            TextBox tb1 = (TextBox)e.Item.FindControl("txtAccountDescription");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Item.FindControl("txtAccountId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowAccountEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListCompanyAccountEdit', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true', true); ");


            if (e.Item is GridDataItem)
            {
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            }


            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (ViewState["ISPERIODLOCKED"].ToString() == "1")
                {
                    ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                }
            }

            RadLabel lblAccountActiveYN = (RadLabel)e.Item.FindControl("lblAccountActiveYN");
            if (lblAccountActiveYN != null)
            {
                strAccountActive = lblAccountActiveYN.Text;
            }

            if (strAccountActive == "0")
            {
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                    cmdEdit.Visible = false;

                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
            }
            RadLabel lblCreditDebitLineId = (RadLabel)e.Item.FindControl("lblCreditDebitLineId");
            LinkButton lnkVoucherLineItemNo = (LinkButton)e.Item.FindControl("lnkVoucherLineItemNo");
            if (lblCreditDebitLineId != null && lnkVoucherLineItemNo != null)
            {
                if (lblCreditDebitLineId.Text != "" && lnkVoucherLineItemNo.Text != null)
                {
                    if (ViewState["LASTADDEDROWNO"] != null)
                    {
                        if (Convert.ToInt32(ViewState["LASTADDEDROWNO"].ToString()) < Convert.ToInt32(lnkVoucherLineItemNo.Text))
                        {
                            ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo.Text;
                            ViewState["LASTADDEDLINEITEMID"] = lblCreditDebitLineId.Text;
                        }
                    }
                    else
                    {
                        ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo.Text;
                        ViewState["LASTADDEDLINEITEMID"] = lblCreditDebitLineId.Text;
                    }
                }
            }

        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
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




    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        //ImageButton imgbtn = (ImageButton)sender;
        //int rownumber = int.Parse(imgbtn.CommandArgument);
        //TextBox tb1 = (TextBox)gvLineItem.Rows[rownumber].FindControl("txtAccountCode");
        //Session["sAccountCode"] = tb1.Text;
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        string a = ViewState["creditdebitnotelineitemid"].ToString();

        gvLineItem.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvLineItem.Items)
        {
            if (item.GetDataKeyValue("FLDCREDITDEBITNOTELINEITEMID").ToString().Equals(ViewState["creditdebitnotelineitemid"].ToString()))
            {
                gvLineItem.SelectedIndexes.Add(item.ItemIndex);                
                
            }
        }
    }

}
