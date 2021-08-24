using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsDebitNoteVoucherList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvVoucher.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("New", "NEW");

        MenuVouchernav.AccessRights = this.ViewState;
        MenuVouchernav.MenuList = toolbarmain.Show();
        MenuVouchernav.SetTrigger(pnlVoucher);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsDebitNoteVoucherList.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("../Accounts/AccountsDebitNoteVoucherList.aspx", "Find", "search.png", "FIND");

        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        BindData();
        SetPageNavigator();
    }

    protected void VoucherNavItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Response.Redirect("../Accounts/AccountsDebitNoteVoucher.aspx");
        }
    }

    protected void RegistersAccountMenu_TabStripCommand(object sender, EventArgs e)
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { 
                                "Voucher Number",
                                "Voucher Date", 
                                "Reference No",
                                "Company Name",                               
                                "Sub Voucher Type", 
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"                              
                              };

        string[] alColumns = { 
                                "FLDVOUCHERNUMBER", 
                                "FLDVOUCHERDATE", 
                                "FLDREFERENCEDOCUMENTNO", 
                                "FLDCOMPANYNAME",                           
                                "FLDSUBVOUCHERTYPE",
                                "FLDQUICKVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"                               
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

        DateTime? VoucherFromDate = null, VoucherToDate = null;

        if (txtVoucherFromdateSearch.Text != null && txtVoucherFromdateSearch.Text.ToString().Trim() != string.Empty)
            VoucherFromDate = DateTime.Parse(txtVoucherFromdateSearch.Text.ToString());

        if (txtVoucherTodateSearch.Text != null && txtVoucherTodateSearch.Text.ToString().Trim() != string.Empty)
            VoucherToDate = DateTime.Parse(txtVoucherTodateSearch.Text.ToString());

        ds = PhoenixAccountsVoucher.VoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 73
                                                   , txtVoucherNumberSearch.Text
                                                   , VoucherFromDate
                                                   , VoucherToDate
                                                   , null
                                                   , null
                                                   , txtReferenceNumberSearch.Text.Trim()
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , General.ShowRecords(null)
                                                   , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
            ShowExcel();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
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
        DateTime? VoucherFromDate = null, VoucherToDate = null;

        if (txtVoucherFromdateSearch.Text != null && txtVoucherFromdateSearch.Text != string.Empty)
            VoucherFromDate = DateTime.Parse(txtVoucherFromdateSearch.Text.ToString());

        if (txtVoucherTodateSearch.Text != null && txtVoucherFromdateSearch.Text != string.Empty)
            VoucherToDate = DateTime.Parse(txtVoucherTodateSearch.Text.ToString());

        ds = PhoenixAccountsVoucher.VoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 73
                                                   , txtVoucherNumberSearch.Text
                                                   , VoucherFromDate
                                                   , VoucherToDate
                                                   , null
                                                   , null
                                                   , txtReferenceNumberSearch.Text.Trim()
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , General.ShowRecords(null)
                                                   , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = { 
                                "Voucher Number",
                                "Voucher Date", 
                                "Reference No",
                                "Company Name",                               
                                "Voucher Type", 
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"                              
                              };

        string[] alColumns = { 
                                "FLDVOUCHERNUMBER", 
                                "FLDVOUCHERDATE", 
                                "FLDREFERENCEDOCUMENTNO", 
                                "FLDCOMPANYNAME",                           
                                "FLDSUBVOUCHERTYPE",
                                "FLDQUICKVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYN"                               
                             };
        General.SetPrintOptions("gvVoucher", "Accounts Voucher", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVoucher.DataSource = ds;
            gvVoucher.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVoucher);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void VoucherClick(object sender, CommandEventArgs e)
    {
        ViewState["VOUCHERCODE"] = e.CommandArgument;
        Response.Redirect("../Accounts/AccountsDebitNoteVoucher.aspx?VOUCHERID=" + ViewState["VOUCHERCODE"].ToString());
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

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
        gvVoucher.SelectedIndex = -1;
        gvVoucher.EditIndex = -1;
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

    protected void gvVoucher_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvVoucher_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void gvVoucher_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    }

    protected void gvVoucher_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
