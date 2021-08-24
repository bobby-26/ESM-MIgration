using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework; 

public partial class CommonPickListVesselEmployeeOnboardByDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
            if (Request.QueryString["date"] != null)
                ViewState["Date"] = Request.QueryString["date"].ToString();
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
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            DateTime? ondate = (ViewState["Date"] != null) ? General.GetNullableDateTime(ViewState["Date"].ToString()) : null;

            ds = PhoenixCommonInspection.SearchVesselEmployeeByDate(vesselid, ondate, General.GetNullableInteger(ucRank.SelectedRank),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewList.DataSource = ds;
                gvCrewList.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewList);
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

    protected void gvCrewList_RowCommand(object sender, GridViewCommandEventArgs e)
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

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCrew");
            nvc.Add(lb.ID, lb.Text.ToString());
            Label lblRank = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRank");
            nvc.Add(lblRank.ID, lblRank.Text);
            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewId");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkCrew");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            Label lblRank = (Label)_gridView.Rows[nCurrentRow].FindControl("lblRank");
            nvc.Set(nvc.GetKey(2), lblRank.Text.ToString());
            Label lblCrewId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewId");
            nvc.Set(nvc.GetKey(3), lblCrewId.Text.ToString());
        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }

    protected void gvCrewList_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void gvCrewList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        gvCrewList.SelectedIndex = -1;
        gvCrewList.EditIndex = -1;
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

    protected void gvCrewList_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
