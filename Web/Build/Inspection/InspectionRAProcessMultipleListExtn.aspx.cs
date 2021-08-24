using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRAProcessMultipleListExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["PROCESSID"] = null;
            Filter.CurrentMultipleRASelection = null;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Back", "BACK");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionRAProcessMultipleListExtn.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRiskAssessmentProcess')", "Print Grid", "icon_print.png", "PRINT");
            MenuProcess.AccessRights = this.ViewState;
            MenuProcess.MenuList = toolbar.Show();
            if (Request.QueryString["PROCESSID"] != null && Request.QueryString["PROCESSID"].ToString() != string.Empty)
                ViewState["PROCESSID"] = Request.QueryString["PROCESSID"].ToString();
            if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString() != string.Empty)
                ViewState["status"] = Request.QueryString["status"].ToString();
        }
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDCATEGORYNAME", "FLDPROCESSNAME", "FLDCREATEDDATE" };
        string[] alCaptions = { "Serial Number", "Category", "Process", "Created Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentProcessMultipleSearch(
                                    (ViewState["PROCESSID"] == null ? null : General.GetNullableGuid(ViewState["PROCESSID"].ToString())),
                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                    General.ShowRecords(null),
                                    ref iRowCount,
                                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentProcess", "Risk Assessment-Process", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRiskAssessmentProcess.DataSource = ds;
            gvRiskAssessmentProcess.DataBind();

            if (Filter.CurrentMultipleRASelection == null)
            {
                Filter.CurrentMultipleRASelection = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSMULTIPLEID"].ToString();
                gvRiskAssessmentProcess.SelectedIndex = 0;
            }
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionRAProcessMultipleExtn.aspx?processid="
                + ViewState["PROCESSID"].ToString()
                + "&processmultipleid=" + (Filter.CurrentMultipleRASelection == null ? null : Filter.CurrentMultipleRASelection.ToString());

            SetRowSelection();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionRAProcessMultipleExtn.aspx?processid="
                + (ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString());

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRiskAssessmentProcess);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSERIALNUMBER", "FLDCATEGORYNAME", "FLDPROCESSNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Serial Number", "Category", "Process", "Created Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentProcessMultipleSearch(
                                                                                new Guid(ViewState["PROCESSID"].ToString()),
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                iRowCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblProcessMultipleId = ((Label)gvRiskAssessmentProcess.Rows[rowindex].FindControl("lblProcessMultipleId"));
            if (lblProcessMultipleId != null)
                Filter.CurrentMultipleRASelection = lblProcessMultipleId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvRiskAssessmentProcess.SelectedIndex = -1;

        for (int i = 0; i < gvRiskAssessmentProcess.Rows.Count; i++)
        {
            if (gvRiskAssessmentProcess.DataKeys[i].Value.ToString().Equals(Filter.CurrentMultipleRASelection.ToString()))
            {
                gvRiskAssessmentProcess.SelectedIndex = i;
            }
        }
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Inspection/InspectionRAProcessExtn.aspx?processid=" + ViewState["PROCESSID"] + "&status=" + (ViewState["status"] == null ? null : ViewState["status"].ToString()));
        }
        else if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionMainFleetRAProcessListExtn.aspx");
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

    protected void gvRiskAssessmentProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

        }
    }

    protected void gvRiskAssessmentProcess_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        GridView _gridView = (GridView)sender;
        int nRow = int.Parse(gce.CommandArgument.ToString());

        if (gce.CommandName.ToUpper().Equals("EDIT"))
        {
            //gvRiskAssessmentProcess.SelectedIndex = nRow;
            BindPageURL(nRow);
            //SetRowSelection();
        }
        else if (gce.CommandName.ToUpper().Equals("DELETE"))
        {
            Label lblProcessMultipleId = (Label)_gridView.Rows[nRow].FindControl("lblProcessMultipleId");
            if (lblProcessMultipleId != null)
            {
                PhoenixInspectionRiskAssessmentProcessExtn.DeleteRAProcessMultiple(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lblProcessMultipleId.Text));
            }
        }
    }

    protected void gvRiskAssessmentProcess_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvRiskAssessmentProcess_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        Filter.CurrentMultipleRASelection = null;
        BindData();
        SetPageNavigator();
    }

    protected void gvRiskAssessmentProcess_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvRiskAssessmentProcess.SelectedIndex = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
        //BindData();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //Filter.CurrentMultipleRASelection = null;
        BindData();
        SetPageNavigator();
    }
}
