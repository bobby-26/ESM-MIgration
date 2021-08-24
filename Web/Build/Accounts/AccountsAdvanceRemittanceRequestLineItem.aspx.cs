using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsAdvanceRemittanceRequestLineItem : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Remittance", "REMITTANCE");
            toolbarmain.AddButton("LineItems", "LINEITEMS");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtSupplierId.Attributes.Add("style", "visibility:hidden");
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            if (!IsPostBack)
            {
                ViewState["Remittenceid"] = "";
                if ((Request.QueryString["REMITTENCEID"] != null) && (Request.QueryString["REMITTENCEID"] != ""))
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    ddlBankAccount.Enabled = false;
                    BindHeader(ViewState["Remittenceid"].ToString());
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                    ddlBankAccount.Enabled = true;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

    private void BindHeader(string remittanceid)
    {
        DataSet ds = PhoenixAccountsAdvanceRemittance.Editremittance(remittanceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
            txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iCurrnecyId = null;
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (chkShowAll.Checked == false)
            iCurrnecyId = General.GetNullableInteger(txtCurrencyId.Text);

        ds = PhoenixAccountsAdvanceRemittance.RemittancePaymentVoucherSearch(General.GetNullableString(ViewState["Remittenceid"].ToString()), "", General.GetNullableInteger(txtSupplierId.Text), iCurrnecyId
                                            , string.Empty, string.Empty
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null)
                                            , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVoucherDetails.DataSource = ds;
            gvVoucherDetails.DataBind();
            gvVoucherDetails.SelectedIndex = 0;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVoucherDetails);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REMITTANCE"))
            {
                Response.Redirect("../Accounts/AccountsAdvanceRemittance.aspx?REMITTENCEID=" + ViewState["Remittenceid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        if ((Request.QueryString["REMITTENCEID"] == null) || (Request.QueryString["REMITTENCEID"] == ""))
        {
            ddlBankAccount.Enabled = true;
        }
        else
        {
            ddlBankAccount.Enabled = false;
        }
        CheckBox cb = (CheckBox)sender;
        string strpaymentvouchernumber = cb.Text;
        {
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRemittanceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["Remittenceid"].ToString(), strpaymentvouchernumber.Trim());
            return;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
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

    protected void gvVoucherDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
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
            gvVoucherDetails.SelectedIndex = -1;
            gvVoucherDetails.EditIndex = -1;
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

    protected void gvVoucherDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cb = (CheckBox)e.Row.FindControl("chkVoucher");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            cb.Checked = drv["FLDREMITTANCEID"].ToString() != "" ? true : false;

            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(cb, drv["FLDREMITTANCEID"].ToString());
            // Add this javascript to the onclick Attribute of the row
            //cb.Attributes["onclick"] = e.Row.FindControl("lnkCheck").ClientID + ".click();";
        }
    }

    protected void gvVoucherDetails_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

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

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            if (ds.Tables[0].Rows.Count > 0)
            {

                DataRow dr = ds.Tables[0].Rows[0];
                txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
                txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
                BindData();
                SetPageNavigator();
            }
        }
    }
}
