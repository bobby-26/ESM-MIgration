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

public partial class ERMERMVoucherLineItemDetails : PhoenixBasePage
{
    string _reportfile = "";
    string _filename = "";

    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;

    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in gvLineItem.Rows)
    //     {
    //         if (r.RowType == DataControlRowType.DataRow)
    //         {
    //             Page.ClientScript.RegisterForEventValidation(gvLineItem.UniqueID, "Edit$" + r.RowIndex.ToString());
    //         }
    //     }
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/ERMERMVoucherLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsCashPaymentVoucherLineItemDetails.aspx", "Report", "task-list.png", "REPORT");
            //toolbargrid.AddImageButton("../Accounts/AccountsCashPaymentVoucherLineItemDetails.aspx", "Voucher Print", "pdf.png", "REPORTPR");
            //toolbargrid.AddImageButton("../Accounts/AccountsCashPaymentVoucherLineItemDetails.aspx", "Voucher Upload", "download_1.png", "UPLOAD");
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
            MenuOrderLineItem.Title = "ERM Voucher Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "GENERAL", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();
            MenuLineItem.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
               

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                    ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["qvoucherlineitemcode"];
                ViewState["voucherid"] = Request.QueryString["qvouchercode"];

                //Title1.Text = "ERM Voucher Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";
                if (Request.QueryString["qvouchercode"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "ERMERMVoucherLineItem.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "ERMERMVoucherLineItem.aspx";
                }
            }

            // BindData();

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
                ViewState["PAGEURL"] = "../Accounts/ERMERMVoucherLineItem.aspx?voucherid=";
            }
            else if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/ERMERMVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
            if (ViewState["voucherlineitemcode"] != null)
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString();
            else
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString();

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
        string[] alColumns = {
                                "FLDTRANSFERCODE",
                                "FLDCREATEDDATE",
                                "FLDUPDATEDDATE",
                                "FLDZID",
                                "FLDXVOUCHER",
                                "FLDVOUCHERLINEITEMNO",
                                "FLDXACC",
                                "FLDXACCUSAGE",
                                "FLDXACCSOURCE",
                                "FLDXSUB",
                                "FLDXDIV",
                                "FLDXSEC",
                                "FLDXPROJ",
                                "FLDXCUR",
                                "FLDXEXCH",
                                "FLDXEXCHR",
                                "FLDXPRIME",
                                "FLDXBASE",
                                "FLDXLONG",
                                "FLDXPAYACT",
                                "FLDXCHEQUENO",
                                "FLDXYEAR",
                                "FLDXPER",
                                "FLDXDATE",
                                "FLDXDATEDUE",
                                "FLDXCHEQUE",
                                "FLDXSTATUS",
                                "FLDXINVNUM",
                                "FLDXREF",
                                "FLDXTYPETRN",
                                "FLDXMANAGER",
                                "FLDVOUCHERNUMBER",
                                "FLDERMBASE",
                                "FLDERMREPORT",
                                "FLDBASEBALANCE",
                                "FLDREPORTBALANCE",
                                "FLDCREATEDBY",
                                "FLDUPDATEBY",
                                "FLDLONGDESCRITION",
                                "FLDACCOUNTCODE",
                                "FLDDESCRIPTION",
                                "FLDBUDGETID",
                                "FLDCURRENCYCODE",
                                "FLDBASEEXCHANGERATE",
                                "FLDREPORTEXCHANGERATE",
                                "FLDAMOUNT",
                                "FLDVOUCHERLINEITEMID"
                             };
        string[] alCaptions = {
                                 "TRANSFERCODE",
                                "CREATEDDATE",
                                "UPDATEDDATE",
                                "ZID",
                                "ERMVOUCHER",
                                "VOUCHERLINEITEMNO",
                                "ERMACC",
                                "ERMACCUSAGE",
                                "ERMACCSOURCE",
                                "ERMSUB",
                                "ERMDIV",
                                "ERMSEC	",
                                "ERMPROJ",
                                "ERMCUR",
                                "ERMEERMCH",
                                "ERMEERMCHR",
                                "ERMPRIME",
                                "ERMBASE",
                                "ERMLONG",
                                "ERMPAYACT",
                                "ERMCHEQUENO",
                                "ERMYEAR",
                                "ERMPER",
                                "ERMDATE",
                                "ERMDATEDUE",
                                "ERMCHEQUE",
                                "ERMSTATUS",
                                "ERMINVNUM",
                                "ERMREF",
                                "ERMTYPETRN",
                                "ERMMANAGER",
                                "VOUCHERNUMBER",
                                "ERMBASE",
                                "ERMREPORT",
                                "BASEBALANCE",
                                "REPORTBALANCE",
                                "CREATEDBY",
                                "UPDATEBY",
                                "LONGDESCRITION	",
                                "ACCOUNTCODE",
                                "DESCRIPTION",
                                "BUDGETID",
                                "CURRENCYCODE",
                                "BASEEXCHANGERATE",
                                "REPORTEXCHANGERATE",
                                "AMOUNT",
                                "VOUCHERLINEITEMID"
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

        ds = PhoenixAccountsVoucher.ERMVoucherLineItemSearch(ViewState["voucherid"].ToString()
                                                                         , null
                                                                         , null
                                                                         , string.Empty
                                                                         , sortdirection
                                                                         , sortexpression
                                                                         , (int)ViewState["PAGENUMBER"]
                                                                         , General.ShowRecords(null)
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
            //row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
            row["FLDXBASE"] = string.Format(String.Format("{0:#####.00}", row["FLDXBASE"]));
            row["FLDERMREPORT"] = string.Format(String.Format("{0:#####.00}", row["FLDERMREPORT"]));
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
            ds = PhoenixAccountsVoucher.ERMVoucherLineItemSearch(ViewState["voucherid"].ToString()
                                                                   , null
                                                                   , null
                                                                   , string.Empty
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
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //ViewState["ISPERIODLOCKED"] = dr["FLDISPERIODLOCKED"].ToString();

                if (ViewState["voucherlineitemcode"] == null)
                {
                    ViewState["voucherlineitemcode"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                }
                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/ERMERMVoucherLineItem.aspx?qvouchercode=";
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

                DataTable dt1 = ds.Tables[0];
                foreach (DataRow row in dt1.Rows)
                {
                    //row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
                    row["FLDXBASE"] = string.Format(String.Format("{0:#####.00}", row["FLDXBASE"]));
                    row["FLDERMREPORT"] = string.Format(String.Format("{0:#####.00}", row["FLDERMREPORT"]));
                }
            }

           // if (ViewState["ISPERIODLOCKED"] != null)
           // {
           //     if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 1)
           //     {
           //         for (int i = 0; i < gvLineItem.Rows.Count; i++)
           //         {
           //             GridViewRow gvRow = gvLineItem.Rows[i];
           //             //((ImageButton)gvRow.FindControl("cmdEdit")).Visible = false;
           //             ((ImageButton)gvRow.FindControl("cmdDelete")).Visible = false;
           //             ((RadLabel)gvRow.FindControl("lblIsPeriodLocked")).Visible = true;
           //         }
           //     }
           // }


            string[] alColumns = {"FLDXACC", "FLDDESCRIPTION","FLDBUDGETID","FLDXCUR",
                                 "FLDXPRIME","FLDXBASE","FLDERMREPORT"};
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
        RadGrid _gridView = (RadGrid)sender;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            RadLabel lblVoucherLineId = ((RadLabel)e.Item.FindControl("lblVoucherLineId"));

            try
            {
                if (lblVoucherLineId != null)
                {
                    if (lblVoucherLineId.Text != null)
                    {
                        string strVoucherLineId = lblVoucherLineId.Text;
                        PhoenixAccountsVoucher.VoucherLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strVoucherLineId), int.Parse(ViewState["voucherid"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno = Int32.Parse(e.CommandArgument.ToString());

            try
            {
                ViewState["voucherlineitemcode"] = ((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text;
                ifMoreInfo.Attributes["src"] = "../Accounts/ERMERMVoucherLineItem.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&lastAddedlineitemid=" + ViewState["LASTADDEDLINEITEMID"];
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }


    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            ViewState["voucherlineitemcode"] = ((Label)gvLineItem.Items[de.NewEditIndex].FindControl("lblVoucherLineId")).Text;
            ViewState["rowno"] = ((LinkButton)gvLineItem.Items[de.NewEditIndex].FindControl("lnkVoucherLineItemNo")).Text;
            if (int.Parse(ViewState["rowno"].ToString()) != 10)
            {
                //if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 0)
                //{
                _gridView.EditIndex = de.NewEditIndex;
                //}
            }
            else
            {
                _gridView.EditIndex = -1;

            }
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        if (e.Item is GridDataItem)
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
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
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
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");


            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (ViewState["ISPERIODLOCKED"].ToString() == "1")
                {
                    //ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                    if (cmdEdit != null)
                    {
                        cmdEdit.Visible = false;
                        if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
                    }

                    //ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (cmdDelete != null)
                    {
                        cmdDelete.Visible = false;
                        if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
                    }
                }
            }


            RadLabel lblAccountActiveYN = (RadLabel)e.Item.FindControl("lblAccountActiveYN");
            if (lblAccountActiveYN != null)
            {
                strAccountActive = lblAccountActiveYN.Text;
            }

            if (strAccountActive == "0")
            {
                // ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
                }

                // ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (cmdDelete != null)
                {
                    cmdDelete.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
                }
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
            //Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            //ImageButton cmdAttachment = (ImageButton)e.Row.FindControl("cmdAttachment");
            //if (cmdAttachment != null)
            //{
            //    cmdAttachment.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
            //                        + PhoenixModule.ACCOUNTS + "');return true;");
            //    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            //}
            //ImageButton cmdNoAttachment = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            //if (cmdNoAttachment != null)
            //{
            //    cmdNoAttachment.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
            //                        + PhoenixModule.ACCOUNTS + "');return true;");
            //    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            //}


            //ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
            //ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            //if (iab != null) iab.Visible = true;
            //if (inab != null) inab.Visible = false;
            //int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            //if (n == 0)
            //{
            //    if (iab != null) iab.Visible = false;
            //    if (inab != null) inab.Visible = true;
            //}
            //ImageButton hlnkSplit = (ImageButton)e.Row.FindControl("hlnkSplit");
            //if (hlnkSplit != null)
            //    if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;

            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            //    Label lblVoucherLineId = (Label)e.Row.FindControl("lblVoucherLineId");
            //    LinkButton lnkVoucherLineItemNo = (LinkButton)e.Row.FindControl("lnkVoucherLineItemNo");
            //    if (hlnkSplit != null) hlnkSplit.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AccountsVoucherLineItemSplit.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
            //}

            //if (hlnkSplit != null && General.GetNullableInteger(drv["FLDALLOCATIONYN"].ToString()) == 0)
            //{
            //    hlnkSplit.Visible = false;
            //}
        }
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
}
