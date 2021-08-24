using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsUsageSupplier : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gdBudget.Rows)
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
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsUsageSupplier.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvBudget')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsSubAccountFilter.aspx')", "Find", "search.png", "FIND");

        MenuRegistersBudget.AccessRights = this.ViewState;
        MenuRegistersBudget.MenuList = toolbargrid.Show();
        MenuRegistersBudget.SetTrigger(pnlBudget);

        EditAccount();

        if (!IsPostBack)
        {
            EnableControls();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BUDGETID"] = null;           
        }
        BindData();
        SetPageNavigator();
    }

    private void EditAccount()
    {
        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));
        DataRow dr = ds.Tables[0].Rows[0];
        txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
        txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
        txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
        ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
    }

    private void EnableControls()
    {         
        txtSubAccount.ReadOnly = true;
        txtSubAccount.CssClass = "readonlytextbox";
        txtDescription.CssClass = "readonlytextbox";
        txtDescription.ReadOnly = true;   
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void Reset()
    {
        ViewState["BUDGETID"] = null;
        txtSubAccount.Text = "";
        txtDescription.Text = "";
    }

    protected void RegistersBudgetMenu_TabStripCommand(object sender, EventArgs e)
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION" };
        string[] alCaptions = { "Budget Code", "Description"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                          General.GetNullableInteger(ViewState["USAGE"].ToString())
                          , ""
                          , ""
                           , null
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , null
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"], iRowCount
                        , ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=SubAccount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Account</h3></td>");
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
    
    public bool IsValidBudget()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSubAccount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Budget code is required.";
        if (txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        return (!ucError.IsError);
    }

    public bool IsValidSubAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (lblSubAccountMapId.Text.Trim() == "")
            ucError.ErrorMessage = "Budget code is required.";
        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        int? accountid = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION" };
        string[] alCaptions = { "Budget Code", "Description"};

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                           General.GetNullableInteger(ViewState["USAGE"].ToString())
                           , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : string.Empty
                           , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : string.Empty
                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , accountid
                           , sortexpression, sortdirection
                           , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                           , ref iRowCount
                           , ref iTotalPageCount);

        General.SetPrintOptions("gvBudget", "Budget Account", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gdBudget.DataSource = ds;
            gdBudget.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gdBudget);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void BudgetEdit()
    {
        if (ViewState["BUDGETID"] != null)
        {
            DataSet dsBudget = PhoenixRegistersBudget.EditSubAccountMap
                (int.Parse(ViewState["USAGE"].ToString()), Convert.ToInt32(ViewState["BUDGETID"].ToString()));

            if (dsBudget.Tables.Count > 0)
            {
                DataRow drBudget = dsBudget.Tables[0].Rows[0];
                txtSubAccount.Text = drBudget["FLDSUBACCOUNT"].ToString();
                txtDescription.Text = drBudget["FLDDESCRIPTION"].ToString();
                lblSubAccountMapId.Text = drBudget["FLDSUBACCOUNTMAPID"].ToString();
                BindData();
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        SetPageNavigator();
    }

    protected void gdBudget_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gdBudget_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

       if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            ViewState["BUDGETID"] = Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetid")).Text);
        }
    }

    protected void gdBudget_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gdBudget_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            BudgetEdit();

            BindData();
            txtSubAccount.Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gdBudget_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gdBudget_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
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
        gdBudget.SelectedIndex = -1;
        gdBudget.EditIndex = -1;
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
