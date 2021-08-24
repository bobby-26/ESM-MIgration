using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceDepositAdvanceRequest : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    BindData();
    //    BindESMBudgetCodes();
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Pending Deposit Utilization", "PENDING");
            DepositUtilization.AccessRights = this.ViewState;
            DepositUtilization.MenuList = toolbar.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("../Crew/CrewLicenceDepositAdvanceRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarsub.AddImageLink("javascript:CallPrint('gvAdvancesMade')", "Print Grid", "icon_print.png", "PRINT");
            DepositUtilizationSub.AccessRights = this.ViewState;
            DepositUtilizationSub.MenuList = toolbarsub.Show();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            ViewState["CONSULATEID"] = Request.QueryString["CONSULATEID"].ToString();
            ViewState["CURRENCYID"] = Request.QueryString["CURRENCYID"].ToString();
            ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

        }
        RequestDetailsEdit();
        BindAdvances();
        SetPageNavigatorAdvance();
    }

    private void BindAdvances()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDDEPOSITAMOUNT", "FLDUTILIZEDAMOUNT", "FLDREMAININGAMOUNT" };
        string[] alCaptions = { "Advance Number", "Consulate", "Currnecy", "Deposit Amount", "Utilized Amount", "Balance" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewLicenceDepositUtilization.LicenceAdvancesMadeSearch(
            General.GetNullableInteger(ViewState["CONSULATEID"].ToString()), General.GetNullableInteger(ViewState["CURRENCYID"].ToString()),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        //PhoenixRegistersBudget.BudgetSearch(null, "", "", null, null, null
        //    , sortexpression, sortdirection,
        //    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        //    General.ShowRecords(null),
        //    ref iRowCount,
        //    ref iTotalPageCount);

        General.SetPrintOptions("gvAdvancesMade", "Advance Utilization", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdvancesMade.DataSource = ds;
            gvAdvancesMade.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAdvancesMade);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void DepositUtilizationSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
      
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    public void RequestDetailsEdit()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCrewLicenceDepositUtilization.CrewPaymentDetailsEdit(General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

        txtAmountCommitted.Text = ds.Tables[0].Rows[0]["FLDOFFSETAMOUNT"].ToString();
        txtApplicationAmount.Text = ds.Tables[0].Rows[0]["FLDAMOUNT"].ToString();
        txtBalance.Text = ds.Tables[0].Rows[0]["FLDBALANCE"].ToString();
        txtRequestNumber.Text = ds.Tables[0].Rows[0]["FLDREFNUMBER"].ToString();
        ViewState["BALANCE"] = ds.Tables[0].Rows[0]["FLDBALANCE"].ToString();
    }

    protected void gvAdvancesMade_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void gvAdvancesMade_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string lblAdvancePaymentid = ((Label)e.Row.FindControl("lblAdvanceId")).Text;

            ImageButton history = (ImageButton)e.Row.FindControl("cmdHistory");
            if (history != null)
            {
                history.Visible = SessionUtil.CanAccess(this.ViewState, history.CommandName);
                history.Attributes.Add("onclick", "Openpopup('history', '', '../Crew/CrewLicenceDepositUtilizationView.aspx?advancepaymentid=" + lblAdvancePaymentid + "');return false;");
            }

            string paymentdone = ((Label)e.Row.FindControl("lblPaymentDone")).Text;
            string balanceremaining = ((Label)e.Row.FindControl("lblRequestBalance")).Text;

            if (paymentdone == "0")
            {
                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                edit.Visible = false;
            }

            if (decimal.Parse(ViewState["BALANCE"].ToString()) <= 0)
            {
                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                edit.Visible = false;
            }
        }
    }

    protected void gvAdvancesMade_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdvancesMade.SelectedIndex = -1;
        gvAdvancesMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindAdvances();
        SetPageNavigatorAdvance();
    }

    protected void gvAdvancesMade_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;

            BindAdvances();
            SetPageNavigatorAdvance();

            //((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucAmount")).Focus();
            //((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucAmount")).Attributes.Add("onfocus", "this.select();");
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancesMade_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindAdvances();
    }

    protected void gvAdvancesMade_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidAmount(General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAmount")).Text)))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceDepositUtilization.InsertDepositUtilizationDetails(
                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdvanceId")).Text),
                General.GetNullableGuid(ViewState["REQUESTID"].ToString()),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAmount")).Text),
                General.GetNullableInteger(ViewState["CURRENCYID"].ToString()),
                General.GetNullableInteger(ViewState["CONSULATEID"].ToString())
                );

            _gridView.EditIndex = -1;
            RequestDetailsEdit();
            BindAdvances();
            SetPageNavigatorAdvance();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
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

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDDEPOSITAMOUNT", "FLDUTILIZEDAMOUNT", "FLDREMAININGAMOUNT" };
        string[] alCaptions = { "Advance Number", "Consulate", "Currnecy", "Deposit Amount", "Utilized Amount", "Balance" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        DataSet ds = PhoenixCrewLicenceDepositUtilization.LicenceAdvancesMadeSearch(
            General.GetNullableInteger(ViewState["CONSULATEID"].ToString()), General.GetNullableInteger(ViewState["CURRENCYID"].ToString()),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Advance Utilization.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Advance Utilization</h3></td>");
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

    protected void DepositUtilization_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PENDING"))
        {
            Response.Redirect("../Crew/CrewLicenceDepositUtilization.aspx");

            //String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

        }

        //if (dce.CommandName.ToUpper().Equals("FIND"))
        //{
        //    ViewState["PAGENUMBER"] = 1;
        //    BindAdvances();
        //    SetPageNavigatorAdvance();
        //}
        //if (dce.CommandName.ToUpper().Equals("EXCEL"))
        //{
        //    ShowExcelESM();
        //}
    }

    private void SetPageNavigatorAdvance()
    {
        cmdPrevious1.Enabled = IsPreviousEnabled1();
        cmdNext1.Enabled = IsNextEnabled1();
        lblPagenumber1.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages1.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords1.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled1()
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

    private Boolean IsNextEnabled1()
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

    protected void cmdGo_Click1(object sender, EventArgs e)
    {
        int result;
        gvAdvancesMade.SelectedIndex = -1;
        gvAdvancesMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        if (Int32.TryParse(txtnopage1.Text, out result))
        {
            ViewState["PAGENUMBER1"] = Int32.Parse(txtnopage1.Text);

            if ((int)ViewState["TOTALPAGECOUNT1"] < Int32.Parse(txtnopage1.Text))
                ViewState["PAGENUMBER1"] = ViewState["TOTALPAGECOUNT1"];

            if (0 >= Int32.Parse(txtnopage1.Text))
                ViewState["PAGENUMBER1"] = 1;

            if ((int)ViewState["PAGENUMBER1"] == 0)
                ViewState["PAGENUMBER1"] = 1;

            txtnopage1.Text = ViewState["PAGENUMBER1"].ToString();
        }
        ViewState["BUDGETID"] = "";
        BindAdvances();
        SetPageNavigatorAdvance();
     
    }

    protected void PagerButtonClick1(object sender, CommandEventArgs ce)
    {
        gvAdvancesMade.SelectedIndex = -1;
        gvAdvancesMade.EditIndex = -1;
        ViewState["CURRENTROW1"] = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] - 1;
        else
            ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] + 1;


        ViewState["BUDGETID"] = "";
        BindAdvances();
        SetPageNavigatorAdvance();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            RequestDetailsEdit();
            BindAdvances();
            SetPageNavigatorAdvance();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAmount(decimal? amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (amount == 0 || amount == null)
            ucError.ErrorMessage = "Amount is required.";
        return (!ucError.IsError);
    }
}
