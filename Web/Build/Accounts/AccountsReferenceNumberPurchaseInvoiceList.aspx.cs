using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsReferenceNumberPurchaseInvoiceList : PhoenixBasePage
{
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        if (!IsPostBack)
        {
            if (Session["VOUCHERDETAILS"] != null)
            {
                Hashtable ht = new Hashtable();
                ht = (Hashtable)Session["VOUCHERDETAILS"];
                ViewState["REFERENCENUMBER"] = ht["REFERENCENUMBER"];
                ViewState["COMPANYID"] = ht["COMPANYID"];
                ViewState["VOUCHERDATE"] = ht["VOUCHERDATE"];
                ViewState["LOCKED"] = ht["LOCKED"];
                ViewState["LONGDESCRIPTION"] = ht["LONGDESCRIPTION"];
                ViewState["DUEDATE"] = ht["DUEDATE"];
                ViewState["USERCODE"] = ht["USERCODE"];
                ViewState["VOUCHERID"] = ht["VOUCHERID"];
                ViewState["TASK"] = ht["TASK"];
                Session.Remove("VOUCHERDETAILS");
            }
            else
            {
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                Session["New"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["LIDETAILSPAGENUMBER"] = 1;
            ViewState["LIDETAILSSORTEXPRESSION"] = null;
            ViewState["LIDETAILSSORTDIRECTION"] = null;
        }
        if (ViewState["TASK"].ToString() == "ADD")
        {
            toolbar1.AddButton("CREATE", "CREATE");
        }
        else if (ViewState["TASK"].ToString() == "UPDATE")
        {
            toolbar1.AddButton("UPDATE", "UPDATE");
        }
        toolbar1.AddButton("CANCEL", "CANCEL");

        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();
        MenuVoucher.SetTrigger(pnlVoucher);


        BindData();
        BindLIDetailsData();
        SetLIDetailsPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            DataSet ds = new DataSet();
            string referenceno = null;
            int noofrecords = 5;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["REFERENCENUMBER"] != null)
                referenceno = ViewState["REFERENCENUMBER"].ToString();
            ds = PhoenixAccountsVoucher.VoucherReferenceNumberSearch(72
                                                                       , referenceno
                                                                       , sortexpression
                                                                       , sortdirection
                                                                       , (int)ViewState["PAGENUMBER"]
                                                                       , noofrecords
                                                                       , ref iRowCount
                                                                       , ref iTotalPageCount
                                                                  );

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvVoucherLineItem.DataSource = ds;
                gvVoucherLineItem.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvVoucherLineItem);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindLIDetailsData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int noofrecords = 5;
        int voucherid = 0;
        decimal TransactionAmountTotal = 0;
        decimal BaseAmountTotal = 0;
        decimal ReportAmountTotal = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["LIDETAILSSORTEXPRESSION"] == null) ? null : (ViewState["LIDETAILSSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["LIDETAILSSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["LIDETAILSSORTDIRECTION"].ToString());
        if (ViewState["VOUCHERID"] != null)
            voucherid = int.Parse(ViewState["VOUCHERID"].ToString());
        ds = PhoenixAccountsVoucher.VoucherLineItemSearch(voucherid
                                                            , null
                                                            , null
                                                            , string.Empty
                                                            , string.Empty
                                                            , null
                                                            , null
                                                            , sortdirection
                                                            , sortexpression
                                                            , (int)ViewState["LIDETAILSPAGENUMBER"]
                                                            , noofrecords
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
            gvLineItemDetails.DataSource = ds;
            gvLineItemDetails.DataBind();
        }
        else
            ShowNoRecordsFound(ds.Tables[0], gvLineItemDetails);
        ViewState["LIDETAILSROWCOUNT"] = iRowCount;
        ViewState["LIDETAILSTOTALPAGECOUNT"] = iTotalPageCount;
        SetLIDetailsPageNavigator();
    }

    protected void gvVoucherLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVoucherLineItem.SelectedIndex = e.NewSelectedIndex;
        ViewState["VOUCHERID"] = ((Label)gvVoucherLineItem.Rows[e.NewSelectedIndex].FindControl("lblVoucherID")).Text;
        BindLIDetailsData();
        SetLIDetailsPageNavigator();
    }

    protected void gvVoucherLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvVoucherLineItem.EditIndex = -1;
        gvVoucherLineItem.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void MenuVoucher_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string duedate = null;
        if (ViewState["DUEDATE"] != null)
            duedate = ViewState["DUEDATE"].ToString();
        if (dce.CommandName.ToUpper().Equals("CREATE"))
        {
            int iVoucherId = 0;
            try
            {
                if (ViewState["SubVoucherTypeId"] != null)
                {
                    DataSet ds = PhoenixAccountsVoucherNumberSetup.VoucherNumberFormatEdit(Convert.ToInt32(ViewState["SubVoucherTypeId"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYSHORTNAME"].ToString() + ",TRANSACTIONCODE=" + dr["FLDTRANSACTIONCODE"].ToString() + ",";
                    }
                }

                PhoenixAccountsVoucher.VoucherInsert(
                                                     int.Parse(ViewState["COMPANYID"].ToString()),
                                                      72,
                                                      0,
                                                      DateTime.Parse(ViewState["VOUCHERDATE"].ToString()),
                                                      ViewState["REFERENCENUMBER"].ToString(),
                                                      bool.Parse(ViewState["LOCKED"].ToString()) ? 1 : 0,
                                                      ViewState["LONGDESCRIPTION"].ToString(),
                                                      General.GetNullableDateTime(duedate),
                                                      int.Parse(ViewState["USERCODE"].ToString()),
                                                      ref iVoucherId,
                                                      string.Empty
                                                    );
                ViewState["VOUCHERID"] = iVoucherId.ToString();
                ucStatus.Text = "Voucher information added";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            Session["New"] = "Y";
        }
        else if (dce.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                PhoenixAccountsVoucher.VoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()), DateTime.Parse(ViewState["VOUCHERDATE"].ToString()),
                                                        ViewState["REFERENCENUMBER"].ToString(),
                                                          bool.Parse(ViewState["LOCKED"].ToString()) ? 1 : 0,
                                                         ViewState["LONGDESCRIPTION"].ToString(),
                                                        General.GetNullableDateTime(duedate),
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                      );
                ucStatus.Text = "Voucher information updated";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvVoucherLineItem.EditIndex = -1;
        gvVoucherLineItem.SelectedIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvVoucherLineItem.SelectedIndex = -1;
        gvVoucherLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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

    protected void LIDetailsPagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvVoucherLineItem.SelectedIndex = -1;
        gvVoucherLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["LIDETAILSPAGENUMBER"] = (int)ViewState["LIDETAILSPAGENUMBER"] - 1;
        else
            ViewState["LIDETAILSPAGENUMBER"] = (int)ViewState["LIDETAILSPAGENUMBER"] + 1;

        BindData();
        SetLIDetailsPageNavigator();
    }

    protected void cmdLIDetailsGo_Click(object sender, EventArgs e)
    {
        gvVoucherLineItem.EditIndex = -1;
        gvVoucherLineItem.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtLIDetailsnopage.Text, out result))
        {
            ViewState["LIDETAILSPAGENUMBER"] = Int32.Parse(txtLIDetailsnopage.Text);

            if ((int)ViewState["LIDETAILSTOTALPAGECOUNT"] < Int32.Parse(txtLIDetailsnopage.Text))
                ViewState["LIDETAILSPAGENUMBER"] = ViewState["LIDETAILSTOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtLIDetailsnopage.Text))
                ViewState["LIDETAILSPAGENUMBER"] = 1;

            if ((int)ViewState["LIDETAILSPAGENUMBER"] == 0)
                ViewState["LIDETAILSPAGENUMBER"] = 1;

            txtLIDetailsnopage.Text = ViewState["LIDETAILSPAGENUMBER"].ToString();
        }
        BindLIDetailsData();
        SetLIDetailsPageNavigator();
    }

    private void SetLIDetailsPageNavigator()
    {
        cmdLIDetailsPrevious.Enabled = IsLIDetailsPreviousEnabled();
        cmdLIDetailsNext.Enabled = IsLIDetailsNextEnabled();
        lblLIDetailsPagenumber.Text = "Page " + ViewState["LIDETAILSPAGENUMBER"].ToString();
        lblLIDetailsPages.Text = " of " + ViewState["LIDETAILSTOTALPAGECOUNT"].ToString() + " Pages. ";
        lblLIDetailsRecords.Text = "(" + ViewState["LIDETAILSROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsLIDetailsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["LIDETAILSPAGENUMBER"];
        iTotalPageCount = (int)ViewState["LIDETAILSTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsLIDetailsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["LIDETAILSPAGENUMBER"];
        iTotalPageCount = (int)ViewState["LIDETAILSTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
}
