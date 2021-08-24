using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshoreWageScale : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreWageScale.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWageScale')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreWageScale.AccessRights = this.ViewState;
            MenuOffshoreWageScale.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            MenuWageScaleList.AccessRights = this.ViewState;
            MenuWageScaleList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REVISIONID"] = "";
                if (Request.QueryString["revisionid"] != null && Request.QueryString["revisionid"].ToString() != "")
                    ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
                EditRevision();
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

    protected void EditRevision()
    {
        DataTable dt = PhoenixCrewOffshoreWageScale.EditWageScaleRevision(new Guid(ViewState["REVISIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtDesc.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
            txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
            ucDate.Text = dt.Rows[0]["FLDEFFECTIVEDATE"].ToString();
        }
    }

    protected void MenuWageScaleList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;

        if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreWageScaleRevision.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDWAGE", "FLDCURRENCYCODE" };
        string[] alCaptions = { "Rank", "Wage/day", "Currency" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreWageScale.SearchWageScale(new Guid(ViewState["REVISIONID"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        General.ShowExcel("Wage Scale", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuOffshoreWageScale_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDWAGE", "FLDCURRENCYCODE" };
        string[] alCaptions = { "Rank", "Wage/day", "Currency" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreWageScale.SearchWageScale(new Guid(ViewState["REVISIONID"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvWageScale", "Wage Scale", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWageScale.DataSource = ds;
            gvWageScale.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvWageScale);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvWageScale_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((UserControlRank)_gridView.Rows[de.NewEditIndex].FindControl("ucRankEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWageScale_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvWageScale_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            UserControlRank ucRankEdit = (UserControlRank)e.Row.FindControl("ucRankEdit");
            if (ucRankEdit != null)
            {
                ucRankEdit.RankList = PhoenixRegistersRank.ListRank();
                ucRankEdit.DataBind();
                ucRankEdit.SelectedRank = dr["FLDRANKID"].ToString();
            }

            UserControlCurrency ucCurrencyEdit = (UserControlCurrency)e.Row.FindControl("ucCurrencyEdit");
            if (ucCurrencyEdit != null)
            {
                ucCurrencyEdit.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,true);
                ucCurrencyEdit.DataBind();
                ucCurrencyEdit.SelectedCurrency = dr["FLDCURRENCY"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            UserControlRank ucRankAdd = (UserControlRank)e.Row.FindControl("ucRankAdd");
            if (ucRankAdd != null)
            {
                ucRankAdd.RankList = PhoenixRegistersRank.ListRank();
                ucRankAdd.DataBind();
            }

            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Row.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                ucCurrencyAdd.DataBind();
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }
        }
    }

    protected void gvWageScale_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank,
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucWageAdd")).Text,
                    ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency)
                )
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreWageScale.InsertWageScale(new Guid(ViewState["REVISIONID"].ToString())
                    ,int.Parse(((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank)
                    ,decimal.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucWageAdd")).Text)
                    ,int.Parse(((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency)
                    );

                BindData();
                ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid wagescaleid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreWageScale.DeleteWageScale(wagescaleid);
                BindData();
            }
            //else if (e.CommandName.ToUpper().Equals("UPDATE"))
            //{
            //    if (!IsValidData(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank,
            //        ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucWageEdit")).Text,
            //        ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }
            //    Guid wagescaleid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

            //    PhoenixCrewOffshoreWageScale.UpdateWageScale(wagescaleid
            //        , int.Parse(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank)
            //        , decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucWageEdit")).Text)
            //        , int.Parse(((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency)
            //        );

            //    _gridView.EditIndex = -1;
            //    BindData();

            //}
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWageScale_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidData(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank,
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucWageEdit")).Text,
                    ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency))
            {
                ucError.Visible = true;
                return;
            }
            Guid wagescaleid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

            PhoenixCrewOffshoreWageScale.UpdateWageScale(wagescaleid
                , int.Parse(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank)
                , decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucWageEdit")).Text)
                , int.Parse(((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency)
                );

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWageScale_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    private bool IsValidData(string rankid, string wage, string currencyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDecimal(wage) == null)
            ucError.ErrorMessage = "Wage is required.";

        if (General.GetNullableInteger(currencyid) == null)
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvWageScale.SelectedIndex = -1;
        gvWageScale.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvWageScale.SelectedIndex = -1;
        gvWageScale.EditIndex = -1;
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
        gvWageScale.SelectedIndex = -1;
        gvWageScale.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
