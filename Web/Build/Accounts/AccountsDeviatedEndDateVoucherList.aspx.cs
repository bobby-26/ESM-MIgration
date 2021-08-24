using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsDeviatedEndDateVoucherList : PhoenixBasePage
{
    public int iUserCode;
    public string strCompanynamedisplay = string.Empty;

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in dgFinancialYearSetup.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(dgFinancialYearSetup.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsDeviatedEndDateVoucherList.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgFinancialYearSetup')", "Print Grid", "icon_print.png", "PRINT");

        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
            if (Request.QueryString["qMapcode"] != null && Request.QueryString["qMapcode"] != string.Empty)
                ViewState["FINANCIALYEARMAPCODE"] = Request.QueryString["qMapcode"];
            if (Request.QueryString["qMapcode"] != null && Request.QueryString["qMapcode"] != string.Empty)
                ViewState["NEWENDDATE"] = Request.QueryString["qNewEnddate"];

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ISFIRSTYEAR"] = "YES";
        }

        BindData();
        strCompanynamedisplay = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsVoucher.DeviatedVoucherList(int.Parse(ViewState["FINANCIALYEARMAPCODE"].ToString()), DateTime.Parse(ViewState["NEWENDDATE"].ToString()),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Voucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voucher Details</h3></td>");
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

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsVoucher.DeviatedVoucherList(int.Parse(ViewState["FINANCIALYEARMAPCODE"].ToString()), DateTime.Parse(ViewState["NEWENDDATE"].ToString()),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["ISFIRSTYEAR"] = "NO";
            dgFinancialYearSetup.DataSource = ds;
            dgFinancialYearSetup.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, dgFinancialYearSetup);
        }
        General.SetPrintOptions("dgFinancialYearSetup", "Vouchers", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void dgFinancialYearSetup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateVoucher(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVoucherId")).Text
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtVoucherDateEdit")).Text);
            _gridView.EditIndex = -1;
            BindData();
        }

        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindData();
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }

        else
        {
            _gridView.EditIndex = -1;
            BindData();
        }
        SetPageNavigator();
    }

    protected void dgFinancialYearSetup_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void dgFinancialYearSetup_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void dgFinancialYearSetup_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void dgFinancialYearSetup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void dgFinancialYearSetup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void dgFinancialYearSetup_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }

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
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        dgFinancialYearSetup.SelectedIndex = -1;
        dgFinancialYearSetup.EditIndex = -1;
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
        {
            return true;
        }
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

    protected void UpdateVoucher(string strVoucherId, string strVoucherDate)
    {
        if (!IsValidDate(strVoucherDate))
        {
            ucError.Visible = true;
            return;
        }
        try
        {
            PhoenixAccountsVoucher.VoucherDateUpdate(int.Parse(strVoucherId), DateTime.Parse(strVoucherDate),PhoenixSecurityContext.CurrentSecurityContext.UserCode );
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidDate(string VoucherDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";       
        if (VoucherDate == null)
            ucError.ErrorMessage = "Financial Year End Date is required.";        

        return (!ucError.IsError);
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
