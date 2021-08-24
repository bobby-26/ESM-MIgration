using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class AccountsAirfareCreditNoteLineItemDetails : PhoenixBasePage
{

    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLineItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvLineItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareCreditNoteLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            MenuLineItemgrid.AccessRights = this.ViewState;
            MenuLineItemgrid.MenuList = toolbargrid.Show();
            MenuLineItemgrid.SetTrigger(pnlStockItemEntry);


            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Credit Notes", "VOUCHER");
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

                if (Request.QueryString["voucherlineitemcode"] != null && Request.QueryString["voucherlineitemcode"] != string.Empty)
                    ViewState["voucherlineitemcode"] = Request.QueryString["voucherlineitemcode"];

                if (Request.QueryString["AIRFARELINEITEMID"] != null && Request.QueryString["AIRFARELINEITEMID"] != string.Empty)
                    ViewState["AIRFARELINEITEMID"] = Request.QueryString["AIRFARELINEITEMID"];

                ViewState["VoucherId"] = Request.QueryString["VoucherId"];
                ViewState["AirfareCreditNoteId"] = Request.QueryString["AirfareCreditNoteId"];

            }

            BindData();
            SetPageNavigator();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsAirfareCreditNoteMaster.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItemgrid_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDOWNERACCOUNT","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
        string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Owners Budget Code","Transaction Currency",
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

        ds = PhoenixAccountsVoucher.VoucherLineItemSearch(
                                                                          int.Parse(ViewState["VoucherId"].ToString())
                                                                         , null
                                                                         , null
                                                                         , string.Empty
                                                                         , string.Empty
                                                                         , null
                                                                         , null
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
        if (General.GetNullableGuid(ViewState["AirfareCreditNoteId"].ToString()) != null)
        {
            ds = PhoenixAccountsAirfareCreditNote.AirfareCreditNoteLineItemList(new Guid(ViewState["AirfareCreditNoteId"].ToString())
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvLineItem.DataSource = ds;
                gvLineItem.DataBind();

                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["AIRFARELINEITEMID"] == null)
                {
                    ViewState["AIRFARELINEITEMID"] = ds.Tables[0].Rows[0]["FLDAIRFARECREDITNOTELINEITEMID"].ToString();
                    gvLineItem.SelectedIndex = 0;
                }
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNoteLineItem.aspx?AIRFARELINEITEMID=" + ViewState["AIRFARELINEITEMID"].ToString() + "&AIRFARECREDITNOTEID=" + ViewState["AirfareCreditNoteId"].ToString();

                DataTable dt1 = ds.Tables[0];
                foreach (DataRow row in dt1.Rows)
                {
                    row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
                    row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
                    row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
                }
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNoteLineItem.aspx?AIRFARECREDITNOTEID=" + ViewState["AirfareCreditNoteId"].ToString() + "&AIRFARELINEITEMID=";
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvLineItem);
                TransactionAmountTotal = 0;
                BaseAmountTotal = 0;
                ReportAmountTotal = 0;
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            string[] alColumns = {"FLDACCOUNTCODE", "FLDACCOUNTDESCRIPTION","FLDSUBACCOUNT","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
            string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
            General.SetPrintOptions("gvLineItem", "Voucher Line Item", alCaptions, alColumns, ds);

        }
    }

    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                Literal lblAirfareLineItemId = ((Literal)_gridView.Rows[nCurrentRow].FindControl("lblAirfareLineItemId"));

                if (lblAirfareLineItemId != null)
                {
                    if (lblAirfareLineItemId.Text != null)
                    {
                        PhoenixAccountsAirfareCreditNote.AirfareCreditNoteLineItemDelete(new Guid(lblAirfareLineItemId.Text));
                    }
                }
                _gridView.EditIndex = -1;
                BindData();
                SetPageNavigator();
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                int iRowno = Int32.Parse(e.CommandArgument.ToString());

                ViewState["AIRFARELINEITEMID"] = ((Literal)gvLineItem.Rows[iRowno].FindControl("lblAirfareLineItemId")).Text;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNoteLineItem.aspx?AIRFARELINEITEMID=" + ViewState["AIRFARELINEITEMID"].ToString() + "&AIRFARECREDITNOTEID=" + ViewState["AirfareCreditNoteId"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["AIRFARELINEITEMID"] = ((Literal)gvLineItem.Rows[de.NewEditIndex].FindControl("lblAirfareLineItemId")).Text;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void gvLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            basedebittotal = 0.00;
            basecredittotal = 0.00;
            reportdebittotal = 0.00;
            reportcredittotal = 0.00;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            Literal lblBaseAmount = (Literal)e.Row.FindControl("lblBaseAmount");
            Literal lblReportAmount = (Literal)e.Row.FindControl("lblReportAmount");

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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strAccountActive = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

                ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
                if (cmdDelete != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            }


            //if (ViewState["ISPERIODLOCKED"] != null)
            //{
            //    if (ViewState["ISPERIODLOCKED"].ToString() == "1")
            //    {
            //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            //        if (cmdEdit != null)
            //            cmdEdit.Visible = false;

            //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            //        if (cmdDelete != null)
            //            cmdDelete.Visible = false;
            //    }
            //}
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());

            ImageButton cmdAttachment = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
                cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }


            ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
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

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvLineItem.SelectedIndex = -1;
            gvLineItem.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

}
