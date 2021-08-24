using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;

public partial class RegistersVesselFinancialYear : PhoenixBasePage
{
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
        //PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("Period Lock", "PERIODLOCK");
        //MenuPeriodLock.MenuList = toolbar1.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersVesselFinancialYear.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgFinancialYearSetup')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Registers/RegistersVesselFinancialYear.aspx", "Find", "search.png", "FIND");
        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ISFIRSTYEAR"] = "YES";

            DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));

            DataRow drVessel = dsVessel.Tables[0].Rows[0];

            txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        }

        BindData();
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iCompanycode = null;
        int? iFinancialYear = null;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR" };
        string[] alCaptions = { "Vessel Name", "Financial Start Year", "Financial End Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (PhoenixSecurityContext.CurrentSecurityContext.CompanyID != 0)
        {
            iCompanycode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        }
        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        ds = PhoenixRegistersVesselFinancialYear.VesselFinancialYearSearch(
            int.Parse(Filter.CurrentVesselMasterFilter),
            General.GetNullableInteger("") // Account Id
            , iFinancialYear
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselFinanacialYear.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Financial Year</h3></td>");
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
        int? iFinancialYear = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR" };
        string[] alCaptions = { "Vessel Name", "Financial Start Year", "Financial End Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        DataSet ds = PhoenixRegistersVesselFinancialYear.VesselFinancialYearSearch(
            int.Parse(Filter.CurrentVesselMasterFilter),
            General.GetNullableInteger("") // account id
            , iFinancialYear
            , sortexpression, sortdirection,
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

    protected void dgFinancialYearSetup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("ADD"))
        {

            InsertFinancialYearSetup(
                ((UserControlDate)_gridView.FooterRow.FindControl("txtFinancialStartYearAdd")).Text,
                General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtFinancialYear")).Text)
            );
            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateFinancialYearSetup(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMapCode")).Text
                , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFinancialYearEdit")).Text)
                , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtFinancialStartYearEdit")).Text
                , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtFinancialEndYearEdit")).Text);

            _gridView.EditIndex = -1;
            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindData();
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeletedFinancialYearSetup(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMapCode")).Text);
            BindData();
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }

        //else if (e.CommandName.ToUpper().Equals("HISTORY"))
        //{
        //    string companyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCompanyID")).Text;
        //    string financialyear = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFinancialYear")).Text;
        //    Response.Redirect("AccountsCompanyFinancialYearSetup.aspx?qCompanyid="+companyid+"&qCurFinYear="+financialyear);
        //}
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
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblIsRecentFinancialYear = (Label)e.Row.FindControl("lblIsRecentFinancialYear");

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (lblIsRecentFinancialYear.Text == "1")
            {
                if (db != null) db.Visible = true;

                //UserControlDate txtFinancialEndYearEdit = (UserControlDate)e.Row.FindControl("txtFinancialEndYearEdit");
                //if (txtFinancialEndYearEdit != null) txtFinancialEndYearEdit.Visible = true;
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

    protected void InsertFinancialYearSetup(string strFinancialStartYear, int? iYear)
    {
        if (!IsValidData(strFinancialStartYear != null ? strFinancialStartYear : string.Empty))
        {
            ucError.Visible = true;
            return;
        }

        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(Filter.CurrentVesselMasterFilter), General.GetNullableDateTime(strFinancialStartYear), iYear,
                General.GetNullableInteger(""));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateFinancialYearSetup(string strMapCode, int? iYear, string strFinancialYearStartDate, string strFinancialYearEndDate)
    {
        if (!IsValidDate(strFinancialYearStartDate, strFinancialYearEndDate))
        {
            ucError.Visible = true;
            return;
        }
        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapUpdate(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(strMapCode), int.Parse(Filter.CurrentVesselMasterFilter), iYear, DateTime.Parse(strFinancialYearEndDate),
                General.GetNullableInteger(""));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeletedFinancialYearSetup(string strMapCode)
    {
        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapDelete(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(strMapCode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidData(string strFinancialStartYear)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Filter.CurrentVesselMasterFilter == null)
            ucError.ErrorMessage = "Please select a vessel.";

        if (strFinancialStartYear.Trim().Equals("") && ViewState["ISFIRSTYEAR"].ToString() == "YES")
            ucError.ErrorMessage = "Financial start year is required";

        return (!ucError.IsError);
    }

    private bool IsValidDate(string StartDate, string EndDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtEndDate = null;

        if (EndDate == null)
            ucError.ErrorMessage = "Financial Year End Date is required.";
        else
            dtEndDate = DateTime.Parse(EndDate);

        if (dtEndDate < DateTime.Parse(StartDate))
            ucError.ErrorMessage = "Financial Year End Date should be later than Financial Year Start Date.";

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
