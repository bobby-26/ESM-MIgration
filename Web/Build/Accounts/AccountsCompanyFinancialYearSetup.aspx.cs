using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsCompanyFinancialYearSetup : PhoenixBasePage
{
    public string strCompanyName = string.Empty;
    public string strFinancialYear = string.Empty;
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
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Fin.Year", "FINANCIYALYEAR");

        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar1.Show();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsCompanyFinancialYearSetup.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgFinancialYearSetup')", "Print Grid", "icon_print.png", "PRINT");

        MenuCompanyFinancialYearSetup.AccessRights = this.ViewState;
        MenuCompanyFinancialYearSetup.MenuList = toolbar.Show();
        MenuCompanyFinancialYearSetup.SetTrigger(pnlCompanyFinancialYearSetup);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Request.QueryString["qCompanyid"] != null && Request.QueryString["qCompanyid"] != string.Empty)
                ViewState["COMPANYID"] = Request.QueryString["qCompanyid"];
            if (Request.QueryString["qCurFinYear"] != null && Request.QueryString["qCurFinYear"] != string.Empty)
                ViewState["FINANCIYALYEAR"] = Request.QueryString["qCurFinYear"];

            BindData();
        }
        strFinancialYear = ViewState["FINANCIYALYEAR"].ToString();
        strCompanyName = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVOUCHERTYPE", "FLDSUBVOUCHERTYPE", "FLDSTARTINGNUMBER", "FLDRUNNINGNUMBER" };
        string[] alCaptions = { "Voucher Type", "Sub Voucher Type", "Starting Number", "Running Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsVoucherNumberSetup.CompanyFinancialYearList(int.Parse(ViewState["COMPANYID"].ToString()), int.Parse(ViewState["FINANCIYALYEAR"].ToString()), null,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CompanyFinancialYearSetUp.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Company Financial Year SetUp</h3></td>");

        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");

        Response.Write("<tr>");
        Response.Write("<td><h5>Company Name: " + ds.Tables[0].Rows[0]["FLDCOMPANYNAME"] + "</h5> </td>");
        Response.Write("<td><h5>Financial Year: " + ds.Tables[0].Rows[0]["FLDFINANCIALYEAR"] + "</h5> </td>");

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

    protected void CompanyFinancialYearSetup_TabStripCommand(object sender, EventArgs e)
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

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.Equals("FINANCIYALYEAR"))
            Response.Redirect("AccountsFinancialYearSetup.aspx");
    }

    private void SetPrintOptions(DataSet ds)
    {
        string[] alColumns = { "FLDVOUCHERTYPE", "FLDSUBVOUCHERTYPE", "FLDSTARTINGNUMBER", "FLDRUNNINGNUMBER" };
        string[] alCaptions = { "Voucher Type", "Sub Voucher Type", "Starting Number", "Running Number" };

        Session["PRINTHEADER"] = "Company Financial Year Setup";
        Session["PRINTCOLUMNS"] = alColumns;
        Session["PRINTCAPTIONS"] = alCaptions;
        Session["PRINTDATA"] = ds;
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERTYPE", "FLDSUBVOUCHERTYPE", "FLDSTARTINGNUMBER", "FLDRUNNINGNUMBER" };
        string[] alCaptions = { "Voucher Type", "Sub Voucher Type", "Starting Number", "Running Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixAccountsVoucherNumberSetup.CompanyFinancialYearList(int.Parse(ViewState["COMPANYID"].ToString()), int.Parse(ViewState["FINANCIYALYEAR"].ToString()), null,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        SetPrintOptions(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strCompanyName = ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString();
            strFinancialYear = ds.Tables[0].Rows[0]["FLDFINANCIALYEAR"].ToString();
            dgFinancialYearSetup.DataSource = ds;
            dgFinancialYearSetup.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, dgFinancialYearSetup);
        }

        General.SetPrintOptions("dgFinancialYearSetup", "Financial Year Setup", alCaptions, alColumns, ds);
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

    protected void dgFinancialYearSetup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            CompanyFinancialYearMapUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("hidtxtMapCode")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStartingNumberEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRunningNumberEdit")).Text,
                    (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkResetYNEdit")).Checked) ? 1 : 0
             );
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

    protected void dgFinancialYearSetup_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void dgFinancialYearSetup_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    private void CompanyFinancialYearMapUpdate(int iUserCode, int iMapcode, string strStartingNumber, string strRunningNumber, int iReset)
    {
        if (!IsValidRecord(strStartingNumber, strRunningNumber))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixAccountsVoucherNumberSetup.CompanyFinancialYearMapUpdate(iUserCode ,iMapcode, int.Parse(strStartingNumber), int.Parse(strRunningNumber), iReset);
    }

    private bool IsValidRecord(string strStartingNumber, string strRunningNumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strStartingNumber.Trim().Equals(""))
            ucError.ErrorMessage = "Starting number is required.";
        else
            try
            {
                int iNumber = int.Parse(strStartingNumber);
            }
            catch
            {
                ucError.ErrorMessage = "Enter numeric value for starting number.";
            }

        if (strRunningNumber.Trim().Equals(""))
            ucError.ErrorMessage = "Running number is required.";
        else
            try
            {
                int iNumber = int.Parse(strRunningNumber);
            }
            catch
            {
                ucError.ErrorMessage = "Enter numeric value for running number.";
            }

        return (!ucError.IsError);
    }

    private bool IsValidExchangeRate(string strExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        GridView _gridView = dgFinancialYearSetup;

        if (strExchangeRate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange rate is required.";
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
