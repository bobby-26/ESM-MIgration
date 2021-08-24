using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;

public partial class AccountsCashOutMDApproval : PhoenixBasePage
{
    public static decimal dTotalUSDAmount = 0;
    public static decimal dSumUSDAmount = 0;
    public static string strdTotalUSDAmount = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarM = new PhoenixToolbar();
            toolbarM.AddButton("Batch View", "BATCHVIEW");
            toolbarM.AddButton("Request View", "REQUESTVIEW");
            MenuNavigate.AccessRights = this.ViewState;
            MenuNavigate.MenuList = toolbarM.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Refresh Awaiting Approval List", "REF");
            toolbarmain.AddButton("Approve Selected Batches", "NEW");
            toolbarmain.AddButton("Approve All", "GENERATEALL");

            MenuApprove.AccessRights = this.ViewState;
            MenuApprove.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCashOutMDApproval.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvCashOut')", "Print Grid", "icon_print.png", "PRINT");
            MenuTools.AccessRights = this.ViewState;
            MenuTools.MenuList = toolbargrid.Show();
            MenuTools.SetTrigger(pnlStockItemEntry);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuNavigate_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BATCHVIEW"))
            {
                Response.Redirect("../Accounts/AccountsCashOutMDApproval.aspx");
            }
            if (dce.CommandName.ToUpper().Equals("REQUESTVIEW"))
            {
                Response.Redirect("../Accounts/AccountsCashOutMDApprovalLineItems.aspx"); 
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

        ds = PhoenixAccountsCashOut.CashOutAccountBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null)
                                            , ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCashOut.DataSource = ds;
            gvCashOut.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCashOut);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        string[] alColumns = { "FLDDESCRIPTION", "FLDSUPPLIERNAME", "FLDCURRENCYCODE", "FLDCASHPAYMENTAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Cash Account Description", "Payee", "Currency", "Amount", "USD Equivalent" };
        General.SetPrintOptions("gvCashOut", "MD Approval", alCaptions, alColumns, ds);
    }

    protected void MenuTools_TabStripCommand(object sender, EventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuApprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ArrayList GetSelectedCashAccount = new ArrayList();
                string selectedaccounts = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    GetSelectedCashAccount = (ArrayList)Session["CHECKED_ITEMS"];
                    if (GetSelectedCashAccount != null && GetSelectedCashAccount.Count > 0)
                    {
                        foreach (Guid index in GetSelectedCashAccount)
                        { selectedaccounts = selectedaccounts + index + ","; }
                    }
                }

                if (GetSelectedCashAccount.Count > 0)
                {
                    PhoenixAccountsCashOut.CashOutApprovedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedaccounts.Length > 1 ? selectedaccounts : null, 0);
                    BindData();
                }
                else
                {
                    ucError.ErrorMessage = "Please select payment voucher";
                    ucError.Visible = true;
                    return;
                }
            }
            if (dce.CommandName.ToUpper().Equals("GENERATEALL"))
            {
                PhoenixAccountsCashOut.CashOutApprovedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, 0);
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("REF"))
            {
                PhoenixAccountsCashOut.CashOutBatchStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                BindData();
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

        string[] alColumns = { "FLDDESCRIPTION", "FLDSUPPLIERNAME", "FLDCURRENCYCODE", "FLDCASHPAYMENTAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Cash Account Description", "Payee", "Currency", "Amount", "USD Equivalent" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsCashOut.CashOutAccountBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null)
                                            , ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CashOutMDApproval.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cash Out MD Approval</h3></td>");
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

    protected void gvCashOut_PreRender(object sender, EventArgs e)
    {
        try
        {
            GridDecorator.MergeRows(gvCashOut);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCashOut_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                //Response.Redirect("../Accounts/AccountsCashOutMDApprovalLineItems.aspx?accountid=" + ((Label)_gridView.Rows[iRowno].FindControl("lblAccountId")).Text);
            }
            if (e.CommandName.ToUpper().Equals("REJECT"))
            {
                PhoenixAccountsCashOut.CashOutAwaitingApprovalCancelUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCashPaymentId")).Text), 0);
                BindData();
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCashOut_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdReject = (ImageButton)e.Row.FindControl("cmdReject");
            if (cmdReject != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdReject.CommandName)) cmdReject.Visible = false;

            if (e.Row.DataItem != null)
            {
                ((Label)e.Row.FindControl("lblSumUSDEquivalent")).Text = ((Label)e.Row.FindControl("lblUSDEquivalent")).Text;
                if (((Label)e.Row.FindControl("lblUSDEquivalent")).Text != string.Empty)
                    dSumUSDAmount = dSumUSDAmount + decimal.Parse(((Label)e.Row.FindControl("lblUSDEquivalent")).Text);

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Label)e.Row.FindControl("lblTotalSumUSDEquivalent")).Text = drv["FLDAMOUNT"].ToString();

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    LinkButton lbtn = (LinkButton)e.Row.FindControl("lblAccountDescription");
                    lbtn.Attributes.Add("onclick", "Openpopup('PaymentVoucher', '', 'AccountsCashOutRequestLineItem.aspx?cashpaymentid=" + lbtn.CommandArgument + "&batched=1&popup=1'); return false;");
                }
            }
            strdTotalUSDAmount = String.Format("{0:n}", dSumUSDAmount);
        }
    }
    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                int BankAccountNumberCount = int.Parse(((Label)gridView.Rows[rowIndex].FindControl("lblAccountIdCount")).Text);
                int PreviousBankAccountNumberCount = int.Parse(((Label)gridView.Rows[rowIndex + 1].FindControl("lblAccountIdCount")).Text);

                string currentBankAccountNumber = ((Label)gridView.Rows[rowIndex].FindControl("lblAccountId")).Text;
                string previousBankAccountNumber = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblAccountId")).Text;

                string currentPayeeId = ((Label)gridView.Rows[rowIndex].FindControl("lblPayeeId")).Text;
                string previousPayeeId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblPayeeId")).Text;

                if ((currentBankAccountNumber == previousBankAccountNumber) && (currentPayeeId == previousPayeeId))
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;

                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;

                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;

                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                           previousRow.Cells[4].RowSpan + 1;

                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                           previousRow.Cells[5].RowSpan + 1;

                    row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                           previousRow.Cells[8].RowSpan + 1;

                    previousRow.Cells[0].Visible = false;
                    previousRow.Cells[1].Visible = false;
                    previousRow.Cells[2].Visible = false;
                    previousRow.Cells[3].Visible = false;
                    previousRow.Cells[4].Visible = false;
                    previousRow.Cells[5].Visible = false;
                    previousRow.Cells[8].Visible = false;
                }
            }
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCashOut.SelectedIndex = -1;
            gvCashOut.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
            BindData();
            SetPageNavigator();
            GetSelectedCashAccount();//function to get the stored ids
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
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

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvCashOut$ctl01$chkAllCashOut")
        {
            CheckBox chkAll = (CheckBox)gvCashOut.HeaderRow.FindControl("chkAllCashOut");
            foreach (GridViewRow row in gvCashOut.Rows)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedCashAccount = new ArrayList();
        Guid index = new Guid();
        foreach (GridViewRow gvrow in gvCashOut.Rows)
        {
            bool result = false;
            index = new Guid(gvCashOut.DataKeys[gvrow.RowIndex].Value.ToString());

            if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedCashAccount = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedCashAccount.Contains(index))
                    SelectedCashAccount.Add(index);
            }
            else
                SelectedCashAccount.Remove(index);
        }
        if (SelectedCashAccount != null && SelectedCashAccount.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedCashAccount;
    }

    private void GetSelectedCashAccount()
    {
        if (Session["CHECKED_ITEMS"] != null)
        {
            ArrayList SelectedCashAccount = (ArrayList)Session["CHECKED_ITEMS"];
            Guid index = new Guid();
            if (SelectedCashAccount != null && SelectedCashAccount.Count > 0)
            {
                foreach (GridViewRow row in gvCashOut.Rows)
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvCashOut.DataKeys[row.RowIndex].Value.ToString());
                    if (SelectedCashAccount.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }
}
