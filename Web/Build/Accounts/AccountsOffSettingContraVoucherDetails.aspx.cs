using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Accounts;

public partial class AccountsOffSettingContraVoucherDetails : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = 1;
                ViewState["ROWCOUNT"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["QACCOUNTCODE"] = "";

                if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"].ToString() != string.Empty)
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();

                //Title1.Text = "Contra Vocher Items    (  " + PhoenixAccountsVoucher.VoucherNumber + "     )";

            }
            if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["qvoucherlineitemcode"];
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
                ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"];
            if (Request.QueryString["qvouchercode"] != null)
            {
                ViewState["voucherid"] = Request.QueryString["qvouchercode"].ToString();
               // ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVoucherLineItem.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&qvoucherlineitemcode" + ViewState["VOUCHERLINEITEMCODE"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"];
            }
            else
            {
                //ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVoucherLineItem.aspx";
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
                ViewState["ISPERIODLOCKED"] = dr["FLDISPERIODLOCKED"].ToString();

                if (ViewState["voucherlineitemcode"] == null)
                {
                    ViewState["voucherlineitemcode"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                    gvLineItem.SelectedIndex = 0;
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/AccountsOffSettingContraVoucherLineItem.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&qvouchercode=";
                }
                {
                    if (ViewState["voucherlineitemcode"] != null)
                    {
                        string strRowno = string.Empty;
                        if (ViewState["rowno"] != null) { strRowno = ViewState["rowno"].ToString(); } else { strRowno = "10"; }
                       // ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&rowno=" + strRowno;
                    }
                   // else
                       // ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString();
                }

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
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvLineItem);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 1)
                {
                    for (int i = 0; i < gvLineItem.Rows.Count; i++)
                    {
                        GridViewRow gvRow = gvLineItem.Rows[i];
                        ((ImageButton)gvRow.FindControl("cmdEdit")).Visible = false;
                        ((ImageButton)gvRow.FindControl("cmdDelete")).Visible = false;
                        ((Label)gvRow.FindControl("lblIsPeriodLocked")).Visible = true;
                    }
                }
            }
            //string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME",
            //                     "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
            //string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
            //                     "Prime Amount","Base Amount", "Report Amount"};
            //General.SetPrintOptions("gvContraVoucherLineItem", "Voucher Line Item", alCaptions, alColumns, ds);
        }
    }
    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["voucherlineitemcode"] = ((Label)gvLineItem.Rows[de.NewEditIndex].FindControl("lblVoucherLineId")).Text;
            ViewState["rowno"] = ((LinkButton)gvLineItem.Rows[de.NewEditIndex].FindControl("lnkVoucherLineItemNo")).Text;
            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 0)
                {
                    _gridView.EditIndex = de.NewEditIndex;
                }
                else
                {
                    _gridView.EditIndex = -1;
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
    protected void gvLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;

            ImageButton hlnkSplit = (ImageButton)e.Row.FindControl("hlnkSplit");
            if (hlnkSplit != null)
                if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;

            string strAccountActive = string.Empty;
            TextBox tb1 = (TextBox)e.Row.FindControl("txtAccountDescription");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Row.FindControl("txtAccountId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowAccountEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListCompanyAccountEdit', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (ViewState["ISPERIODLOCKED"].ToString() == "1")
                {
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                }
            }

            Label lblAccountActiveYN = (Label)e.Row.FindControl("lblAccountActiveYN");
            if (lblAccountActiveYN != null)
            {
                strAccountActive = lblAccountActiveYN.Text;
            }
            if (strAccountActive == "0")
            {
                if (cmdEdit != null)
                    cmdEdit.Visible = false;

                if (cmdDelete != null)
                    cmdDelete.Visible = false;
            }

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label lblVoucherLineId = (Label)e.Row.FindControl("lblVoucherLineId");
                LinkButton lnkVoucherLineItemNo = (LinkButton)e.Row.FindControl("lnkVoucherLineItemNo");               
                if (lnkVoucherLineItemNo != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherLineItemNo.CommandName)) lnkVoucherLineItemNo.Visible = false;
                }
                if (hlnkSplit != null)
                {
                    hlnkSplit.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AccountsVoucherLineItemSplit.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;
                }
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
