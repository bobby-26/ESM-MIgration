using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using Telerik.Web.UI;

public partial class AccountsJournalVoucherVoucherLineItemDetails : PhoenixBasePage
{
    string _reportfile = "";
    string _filename = "";

    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;
    public string allocation = string.Empty;
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
            toolbargrid.AddImageButton("../Accounts/AccountsJournalVoucherVoucherLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsJournalVoucherVoucherLineItemDetails.aspx", "Report", "task-list.png", "REPORT");
            toolbargrid.AddImageButton("../Accounts/AccountsJournalVoucherVoucherLineItemDetails.aspx", "Voucher Print", "pdf.png", "REPORTPR");
            toolbargrid.AddImageButton("../Accounts/AccountsJournalVoucherVoucherLineItemDetails.aspx", "Voucher Upload", "download_1.png", "UPLOAD");
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
            //  MenuOrderLineItem.SetTrigger(pnlStockItemEntry);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "GENERAL", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);


            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.Title = "Journal Voucher Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";
            MenuLineItem.MenuList = toolbarmain.Show();

            MenuLineItem.SelectedMenuIndex = 0;


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "Display:None");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VOUCHERLINEID"] = null;
                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                    ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["qvoucherlineitemcode"];
                ViewState["voucherid"] = Request.QueryString["qvouchercode"];

                if (Request.QueryString["lastAddedlineitemid"] != null && Request.QueryString["lastAddedlineitemid"] != string.Empty)
                {
                    ViewState["LASTADDEDLINEITEMID"] = Request.QueryString["lastAddedlineitemid"];
                }
                else
                {
                    ViewState["LASTADDEDLINEITEMID"] = "";
                }

                //   Title1.Text = "Journal Voucher Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";
                if (Request.QueryString["qvouchercode"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "AccountsJournalVoucherVoucherLineItem.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "AccountsJournalVoucherVoucherLineItem.aspx";
                }
            }
            BindData();
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
                ViewState["PAGEURL"] = "../Accounts/AccountsJournalVoucherVoucherLineItem.aspx?voucherid=";
            }
            else if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsJournalVoucherVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
            if (ViewState["voucherlineitemcode"] != null)
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
            else
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvLineItem.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvLineItem.Items)
        {

            if (item.GetDataKeyValue("FLDVOUCHERLINEITEMID").ToString() == (ViewState["voucherlineitemcode"].ToString()))
            {
                gvLineItem.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["voucherlineitemcode"] = ((RadLabel)gvLineItem.Items[rowindex].FindControl("lblVoucherLineId")).Text;
            gvLineItem.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsJournalVoucherVoucherLineItem.aspx?qvouchercode=" + ViewState["voucherid"].ToString() + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
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
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VOUCHERLINEITEM&voucherId=" + ViewState["voucherid"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("UPLOAD"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVoucherLineitemUpload.aspx?VoucherId=" + ViewState["voucherid"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (CommandName.ToUpper().Equals("REPORTPR"))
            {
                _filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/VoucherLineItem" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf";
                // _filename = HttpContext.Current.Server.MapPath("../Attachments/Accounts/VoucherLineItem" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");

                //string reporturl = GetReportURL(_reportfile);

                DataTable dt = null;

                //NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                dt = GetReportData();

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    NameValueCollection nvc = new NameValueCollection();
                    //nvc.Remove("applicationcode");
                    nvc.Add("applicationcode", "5");
                    //nvc.Remove("reportcode");
                    nvc.Add("reportcode", "VOUCHERLINEITEMGENERAL");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;
                    string[] rdlcfilename = new string[11];
                    DataSet ds = new DataSet();
                    DataTable dtCopy = dt.Copy();
                    ds.Tables.Add(dtCopy);

                    PhoenixSSRSReportClass.ExportSSRSReport(rdlcfilename, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "VOUCHERLINEITEM", ref _filename);
                    // PhoenixSsrsReportsCommon.GetInterface(rdlcfilename, ds, ExportFileFormat.PDF, "VOUCHERLINEITEM", ref _filename);
                }
                else
                {
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVoucherGeneral.rpt");
                    PhoenixReportClass.ExportReport(_reportfile, _filename, dt);
                }

                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + _filename + "&type=pdf");

                return;
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
        string[] alColumns = {"FLDVOUCHERLINEITEMNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDOWNERACCOUNT","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT", "FLDLONGDESCRIPTION"};
        string[] alCaptions = {"Row No","Account Code", "Account Description","Sub Account Code","Owners Budget Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount", "Line item - Description"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsVoucher.VoucherLineItemSearch(
                                                                          int.Parse(ViewState["voucherid"].ToString())
                                                                         , null
                                                                         , null
                                                                         , string.Empty
                                                                         , string.Empty
                                                                         , null
                                                                         , null
                                                                         , sortdirection
                                                                         , sortexpression
                                                                         , 1
                                                                         , iRowCount
                                                                         , ref iRowCount,
                                                                           ref iTotalPageCount
                                                                         , ref TransactionAmountTotal
                                                                         , ref BaseAmountTotal
                                                                         , ref ReportAmountTotal
                                                                    );
        strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
        strTransactionAmountTotal = "Prime Amount (" + (strTransactionAmountTotal) + ")";
        strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
        strBaseAmountTotal = "Base Amount (" + (strBaseAmountTotal) + ")";
        strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);
        strReportAmountTotal = "Report Amount (" + (strReportAmountTotal) + ")";

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
        if (ViewState["voucherid"] != null)
        {
            ds = PhoenixAccountsVoucher.VoucherLineItemSearch(
                                                                    int.Parse(ViewState["voucherid"].ToString())
                                                                   , null
                                                                   , null
                                                                   , string.Empty
                                                                   , string.Empty
                                                                   , null
                                                                   , null
                                                                   , sortdirection
                                                                   , sortexpression
                                                                   , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , gvLineItem.PageSize
                                                                   , ref iRowCount,
                                                                     ref iTotalPageCount
                                                                   , ref TransactionAmountTotal
                                                                   , ref BaseAmountTotal
                                                                   , ref ReportAmountTotal
                                                              );
            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strTransactionAmountTotal = "Prime Amount (" + (strTransactionAmountTotal) + ")";
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strBaseAmountTotal = "Base Amount (" + (strBaseAmountTotal) + ")";
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);
            strReportAmountTotal = "Report Amount (" + (strReportAmountTotal) + ")";

            //{
            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

            //   gvLineItem.DataBind();


            if (ds.Tables[0].Rows.Count > 0 && ViewState["voucherlineitemcode"] == null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["ISPERIODLOCKED"] = dr["FLDISPERIODLOCKED"].ToString();
                ViewState["voucherlineitemcode"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                //   gvLineItem.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ViewState["PAGEURL"] = "../Accounts/AccountsJournalVoucherVoucherLineItem.aspx?qvouchercode=";
            }
            {
                if (ViewState["voucherlineitemcode"] != null)
                {
                    string strRowno = string.Empty;
                    if (ViewState["rowno"] != null) { strRowno = ViewState["rowno"].ToString(); } else { strRowno = "10"; }
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&rowno=" + strRowno + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                }
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
            }

            //DataTable dt1 = ds.Tables[0];
            //foreach (DataRow row in dt1.Rows)
            //{
            //    row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
            //    row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
            //    row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
            //}
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvLineItem);
            //    TransactionAmountTotal = 0;
            //    BaseAmountTotal = 0;
            //    ReportAmountTotal = 0;
            //}
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 1)
                {
                    // for (int i = 0; i < gvLineItem.Rows.Count; i++)
                    {
                        //      GridViewRow gvRow = gvLineItem.Rows[i];
                        //((ImageButton)gvRow.FindControl("cmdEdit")).Visible = false;
                        //     ((ImageButton)gvRow.FindControl("cmdDelete")).Visible = false;
                        //     ((Label)gvRow.FindControl("lblIsPeriodLocked")).Visible = true;
                    }
                }
            }

            string[] alColumns = {"FLDVOUCHERLINEITEMNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDOWNERACCOUNT","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT", "FLDLONGDESCRIPTION"};
            string[] alCaptions = {"Row No", "Account Code", "Account Description","Sub Account Code","Owners Budget Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount", "Line item - Description"};
            General.SetPrintOptions("gvLineItem", "Voucher Line Item", alCaptions, alColumns, ds);
            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strTransactionAmountTotal = "Prime Amount (" + (strTransactionAmountTotal) + ")";
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strBaseAmountTotal = "Base Amount (" + (strBaseAmountTotal) + ")";
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);
            strReportAmountTotal = "Report Amount (" + (strReportAmountTotal) + ")";

        }
    }





    protected void gvLineItem_RowCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {

            RadLabel lblVoucherLineId = ((RadLabel)e.Item.FindControl("lblVoucherLineId"));
            ViewState["VOUCHERLINEID"] = lblVoucherLineId.Text;
            RadLabel lblallocation = ((RadLabel)e.Item.FindControl("lblallocation"));
            if (lblallocation != null)
            {
                allocation = lblallocation.Text;
            }
            if (allocation == "1")
                RadWindowManager1.RadConfirm("Voucher line items is allocated against another voucher. Do you want to proceed to remove line items?", "confirmdelete", 420, 150, null, "Confirm Message");
            else
            {
                RadWindowManager1.RadConfirm("Are you sure you want to delete this record?", "confirmdelete", 420, 150, null, "Confirm Message");
            }
            //try
            //{
            //    if (lblVoucherLineId != null)
            //    {
            //        if (lblVoucherLineId.Text != null)
            //        {
            //            string strVoucherLineId = lblVoucherLineId.Text;
            //            PhoenixAccountsVoucher.VoucherLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strVoucherLineId), int.Parse(ViewState["voucherid"].ToString()));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ucError.HeaderMessage = "";
            //    ucError.ErrorMessage = ex.Message;
            //    ucError.Visible = true;
            //    return;
            //}
            Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
            gvLineItem.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            try
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                RadGrid _gridView = (RadGrid)sender;

                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            // BindData();
        }
    }

    protected void gvLineItem_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



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

    protected void gvLineItem_RowDataBound(Object sender, GridItemEventArgs e)
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
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            RadLabel lblBaseAmount = (RadLabel)e.Item.FindControl("lblBaseAmount");
            RadLabel lblReportAmount = (RadLabel)e.Item.FindControl("lblReportAmount");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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


            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;


            RadLabel lblAccountActiveYN = (RadLabel)e.Item.FindControl("lblAccountActiveYN");
            if (lblAccountActiveYN != null)
            {
                strAccountActive = lblAccountActiveYN.Text;
            }

            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (ViewState["ISPERIODLOCKED"].ToString() == "1")
                {

                    //ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    // ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                }
            }

            if (strAccountActive == "0")
            {
                // ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (cmdEdit != null)
                    cmdEdit.Visible = false;

                // ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
            }
            RadLabel lblVoucherLineId = (RadLabel)e.Item.FindControl("lblVoucherLineId");
            LinkButton lnkVoucherLineItemNo = (LinkButton)e.Item.FindControl("lnkVoucherLineItemNo");
            if (lblVoucherLineId != null && lnkVoucherLineItemNo != null)
            {
                if (lblVoucherLineId.Text != "" && lnkVoucherLineItemNo.Text != null)
                {
                    if (ViewState["LASTADDEDROWNO"] != null)
                    {
                        if (Convert.ToInt32(ViewState["LASTADDEDROWNO"].ToString()) < Convert.ToInt32(lnkVoucherLineItemNo.Text))
                        {
                            ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo.Text;
                            ViewState["LASTADDEDLINEITEMID"] = lblVoucherLineId.Text;
                        }
                    }
                    else
                    {
                        ViewState["LASTADDEDROWNO"] = lnkVoucherLineItemNo.Text;
                        ViewState["LASTADDEDLINEITEMID"] = lblVoucherLineId.Text;
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
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVoucherLevelFileAttachments.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVoucherLevelFileAttachments.aspx?dtkey=" + lblDtkey + "&mod="
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
            ImageButton hlnkSplit = (ImageButton)e.Item.FindControl("hlnkSplit");
            if (hlnkSplit != null)
                if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;

            ImageButton gstdetails = (ImageButton)e.Item.FindControl("gstdetails");
            if (gstdetails != null)
                if (!SessionUtil.CanAccess(this.ViewState, gstdetails.CommandName)) gstdetails.Visible = false;

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel lblVoucherLineId = (RadLabel)e.Item.FindControl("lblVoucherLineId");
                LinkButton lnkVoucherLineItemNo = (LinkButton)e.Item.FindControl("lnkVoucherLineItemNo");
                if (hlnkSplit != null) hlnkSplit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVoucherLineItemSplit.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
                if (gstdetails != null) gstdetails.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVoucherLineItemGST.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
            }

            if (hlnkSplit != null && General.GetNullableInteger(drv["FLDALLOCATIONYN"].ToString()) == 0)
            {
                hlnkSplit.Visible = false;
            }
            if (gstdetails != null && General.GetNullableInteger(drv["FLDALLOCATIONYN"].ToString()) == 0)
            {
                gstdetails.Visible = false;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
            if (Session["New"].ToString() == "Y")
            {
                Session["New"] = "N";
                if (gvLineItem.Items.Count > 0)
                    BindPageURL(0);
            }
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

    private DataTable GetReportData()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("applicationcode", "5");
        criteria.Add("reportcode", "VOUCHERLINEITEM");
        criteria.Add("voucherId", ViewState["voucherid"].ToString());

        Session["PHOENIXREPORTPARAMETERS"] = criteria;

        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixReportsCommon.GetReportData(nvc, ref _reportfile, ref _filename);
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

    protected void gvLineItem_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ViewState["voucherlineitemcode"] = null;
    }
    protected void confirmdelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsVoucher.VoucherLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VOUCHERLINEID"].ToString()), int.Parse(ViewState["voucherid"].ToString()));

            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
