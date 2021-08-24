using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaPickListConfirmedCandidates : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");
        MenuAddress.MenuList = toolbarmain.Show();
        MenuAddress.SetTrigger(pnlAddressEntry);

        if (!IsPostBack)
        {

            if ((Request.QueryString["batch"] != null) && (Request.QueryString["batch"] != ""))
                ViewState["batch"] =  Request.QueryString["batch"].ToString();
            else
                Response.Redirect("../PhoenixUnderConstruction.aspx");

            if (Request.QueryString["txtEntranceRollNo"] != null)
            {
                txtRollNo.Text = Request.QueryString["txtEntranceRollNo"].ToString(); 
            }
            if (Request.QueryString["txtCandidatename"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtCandidatename"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixPreSeaCommon.SearchPreSeaConfirmedCandidates(General.GetNullableInteger(ViewState["batch"].ToString())
                , General.GetNullableInteger(txtRollNo.Text)
                , General.GetNullableString(txtNameSearch.Text.Trim())
                , General.GetNullableDecimal(txtScoreFrom.Text.Trim())
                , General.GetNullableDecimal(txtScoreTo.Text.Trim())
                , sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreseaPicklist.DataSource = ds;
                gvPreseaPicklist.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreseaPicklist);
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


    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreseaPicklist_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string Script = "";
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblInterViewId");
            nvc.Set(lbl.ID, lbl.Text);

            Label lb2 = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRollNo");
            nvc.Set(lb2.ID, lb2.Text.ToString());

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCandidateName");
            nvc.Set(lb.ID, lb.Text.ToString());

        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblInterViewId");
            nvc.Set(nvc.GetKey(1), lbl.Text);

            Label lb2 = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRollNo");
            nvc.Set(nvc.GetKey(2), lb2.Text.ToString());

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCandidateName");
            nvc.Set(nvc.GetKey(3), lb.Text.ToString());

        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvPreseaPicklist_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvPreseaPicklist_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
            }
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

    protected void gvPreseaPicklist_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
        gvPreseaPicklist.SelectedIndex = -1;
        gvPreseaPicklist.EditIndex = -1;
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


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }
}
