using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Inspection;

public partial class InspectionNonConformityExtensionReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();           

            if (Request.QueryString["ncid"] != null)
            {
                ViewState["NCID"] = Request.QueryString["ncid"];
            }
            else
            {
                ViewState["NCID"] = null;
            }

            BindData();
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    protected void MenuNCRGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("NONCONFORMITY"))
        {
            Response.Redirect("../Inspection/InspectionNonConformity.aspx?RecordResponseId=" + Request.QueryString["RecordResponseId"], false);
        }
        else if (dce.CommandName.ToUpper().Equals("CORRECTIVEACTION"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityCorrectiveAction.aspx?RecordResponseId=" + Request.QueryString["RecordResponseId"].ToString() + "&ncid=" + (ViewState["NCID"] == null ? null : ViewState["NCID"].ToString()), false);
        }
        else if (dce.CommandName.ToUpper().Equals("CAUSEANALYSIS"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityCauseAnalysis.aspx?RecordResponseId=" + Request.QueryString["RecordResponseId"].ToString() + "&ncid=" + (ViewState["NCID"] == null ? null : ViewState["NCID"].ToString()), false);
        }
    }

    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionSchedule.ReScheduleHistorySearch(
                                                                General.GetNullableGuid(ViewState["NCID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvExtensionReason.DataSource = ds.Tables[0];
            gvExtensionReason.DataBind();

            if (ViewState["HISTORYID"] == null)
            {
                //gvExtensionReason.SelectedIndex = 0;
                ViewState["HISTORYID"] = ((Label)gvExtensionReason.Rows[0].FindControl("lblHistoryId")).Text;
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvExtensionReason);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    protected void gvExtensionReason_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Label lblHistoryId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblHistoryId"));
                ViewState["HISTORYID"] = lblHistoryId.Text;
                BindData();
                SetRowSelection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExtensionReason_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{           
        //}
    }

    protected void gvExtensionReason_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvExtensionReason.SelectedIndex = se.NewSelectedIndex;
        ViewState["HISTORYID"] = ((Label)_gridView.Rows[gvExtensionReason.SelectedIndex].FindControl("lblHistoryId")).Text;

        BindData();

        SetRowSelection();
    }

    private void SetRowSelection()
    {
        gvExtensionReason.SelectedIndex = -1;
        for (int i = 0; i < gvExtensionReason.Rows.Count; i++)
        {
            if (gvExtensionReason.DataKeys[i].Value.ToString().Equals(ViewState["HISTORYID"].ToString()))
            {
                gvExtensionReason.SelectedIndex = i;
                ViewState["HISTORYID"] = ((Label)gvExtensionReason.Rows[i].FindControl("lblHistoryId")).Text;
                break;
            }
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            return true;

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

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void btnGo_Click(object sender, EventArgs e)
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
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();

    }
}
