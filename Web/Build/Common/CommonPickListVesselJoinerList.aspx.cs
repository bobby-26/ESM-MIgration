using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class CommonPickListVesselJoinerList : PhoenixBasePage
{
    public PhoenixModule mod;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        //PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Search", "SEARCH");
        //MenuUser.MenuList = toolbarmain.Show();
        //MenuUser.SetTrigger(pnlUserEntry);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
        SetPageNavigator();
    }
    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;
        DataSet ds = PhoenixIntegrationAccounts.SearchCrewChangePlan(int.Parse(Request.QueryString["vesselid"].ToString()), byte.Parse("0"), sortexpression, sortdirection,
                                                                  (int)ViewState["PAGENUMBER"],
                                                                  General.ShowRecords(null),
                                                                  ref iRowCount,
                                                                  ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvUser.DataSource = ds;
            gvUser.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvUser);
        }

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

    protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();
            Label lblname = (Label)_gridView.Rows[nCurrentRow].FindControl("lblname");
            Label lblRankId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRankId");
            nvc.Add(lblname.ID, lblname.Text);
            Label lblRank = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRank");
            nvc.Add(lblRank.ID, lblRank.Text);
            Label lblEmployeeId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId");
            nvc.Add(lblEmployeeId.ID, lblEmployeeId.Text);
        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            Label lblname = (Label)_gridView.Rows[nCurrentRow].FindControl("lblname");
            Label lblRankId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRankId");
            Label lblFileNo = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFileNo");
            nvc.Set(nvc.GetKey(1), lblFileNo.Text);
            Label lblRank = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRank");
            nvc.Set(nvc.GetKey(2), lblname.Text);
            Label lblEmployeeId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId");
            nvc.Set(nvc.GetKey(3), lblEmployeeId.Text);
        }
        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void gvUser_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvUser_RowEditing(object sender, GridViewEditEventArgs de)
    {
        
    }
    protected void gvUser_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            //Label lbl = (Label)e.Row.FindControl("lblUser");

            //LinkButton lb = (LinkButton)e.Row.FindControl("lnkUserName");
            //if (lb != null) lb.Attributes.Add("onclick", "Openpopup('codehelp3', '', 'OptionsUser.aspx?usercode=" + lbl.Text + "')");

            //HtmlImage img = (HtmlImage)e.Row.FindControl("imgGroupList");
            //img.Attributes.Add("onclick", "showMoreInformation(ev, 'OptionsMoreInfoGroupList.aspx?usercode=" + lbl.Text + "')");

            //ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            //if (eb != null) eb.Attributes.Add("onclick", "Openpopup('codehelp3', '', 'OptionsUser.aspx?usercode=" + lbl.Text + "')");
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
        gvUser.SelectedIndex = -1;
        gvUser.EditIndex = -1;
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
