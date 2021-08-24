using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Export2XL;
using System.Web.UI.HtmlControls;

public partial class PlannedMaintenancePostponeRAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../PlannedMaintenance/PlannedMaintenancePostponeRAList.aspx", "Find", "search.png", "FIND");
            toolbar1.AddImageButton("../PlannedMaintenance/PlannedMaintenancePostponeRAList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuHistoryTemplateReports.AccessRights = this.ViewState;
            MenuHistoryTemplateReports.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = null;
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                ViewState["FLDDTKEY"] = Request.QueryString["DTKEY"].ToString();

            }
            BindDataReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHistoryTemplateReports_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindDataReport();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                BindDataReport();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRATemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;

            int nCurrentRow = Convert.ToInt32(e.CommandArgument.ToString());
            Guid Reportid = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportId")).Text);


            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                PhoenixPMS2XL.Export2XLWorkOrderRA(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Reportid
                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , new Guid(ViewState["WORKORDERID"].ToString()));
                BindDataReport();
            }
            if (e.CommandName.ToUpper().Equals("MAPPING"))
            {
                PhoenixPlannedMaintenanceWorkOrderRA.WorkOrderPostponeRARescheduleInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["WORKORDERID"].ToString())
                                                                ,Reportid
                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , new Guid(ViewState["FLDDTKEY"].ToString()));
            }
        }


        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void BindDataReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.WorkorderPostponeRATemplateSearch(General.GetNullableDateTime(txtFromDate.Text)
                                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                , General.ShowRecords(null)
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);


            if (dt.Rows.Count > 0)
            {
                gvRATemplate.DataSource = dt;
                gvRATemplate.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvRATemplate);
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvRATemplate.SelectedIndex = -1;
        gvRATemplate.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindDataReport();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvRATemplate.EditIndex = -1;
        gvRATemplate.SelectedIndex = -1;
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
        BindDataReport();
    }

    protected void gvRATemplate_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            gvRATemplate.SelectedIndex = -1;
            gvRATemplate.EditIndex = -1;
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;

            BindDataReport();
        }
        catch (Exception e)
        {
            ucError.ErrorMessage = e.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRATemplate_RowDataBound(object sender, GridViewRowEventArgs e)
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

        if(e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgMap = (ImageButton)e.Row.FindControl("CmdMapping");
            ImageButton imgDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            Label lblRAReportID = (Label)e.Row.FindControl("lblRAReportID");
            if (!string.IsNullOrEmpty(lblRAReportID.Text))
            {
                imgDelete.Visible = true;
                imgMap.Visible = false;
            }
            else
            {
                imgDelete.Visible = false;
                imgMap.Visible = true;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindDataReport();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
