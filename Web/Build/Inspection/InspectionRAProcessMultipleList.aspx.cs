using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAProcessMultipleList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("List", "LIST", ToolBarDirection.Right);

        MenuGeneral.AccessRights = this.ViewState;
        MenuGeneral.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessMultipleList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentProcess')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["PROCESSID"] = null;
            Filter.CurrentMultipleRASelection = null;

            gvRiskAssessmentProcess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["PROCESSID"] != null && Request.QueryString["PROCESSID"].ToString() != string.Empty)
                ViewState["PROCESSID"] = Request.QueryString["PROCESSID"].ToString();
            if (Request.QueryString["status"] != null && Request.QueryString["status"].ToString() != string.Empty)
                ViewState["status"] = Request.QueryString["status"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACTIVITYCONDITION", "FLDCATEGORYNAME", "FLDPROCESSNAME", "FLDCREATEDDATE" };
        string[] alCaptions = { "Activity/Condition", "Category", "Process", "Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessMultipleSearch(
                                    (ViewState["PROCESSID"] == null ? null : General.GetNullableGuid(ViewState["PROCESSID"].ToString())),
                                    gvRiskAssessmentProcess.CurrentPageIndex + 1,
                                    gvRiskAssessmentProcess.PageSize,
                                    ref iRowCount,
                                    ref iTotalPageCount);

        General.SetPrintOptions("gvRiskAssessmentProcess", "Risk Assessment-Process Multiple RA", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvRiskAssessmentProcess.DataSource = ds;
            //gvRiskAssessmentProcess.DataBind();

            if (Filter.CurrentMultipleRASelection == null)
            {
                Filter.CurrentMultipleRASelection = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSMULTIPLEID"].ToString();
                // gvRiskAssessmentProcess.SelectedIndex = 0;
            }
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionRAProcessMultiple.aspx?processid="
                + ViewState["PROCESSID"].ToString()
                + "&processmultipleid=" + (Filter.CurrentMultipleRASelection == null ? null : Filter.CurrentMultipleRASelection.ToString());

            SetRowSelection();
        }

        else
        {
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionRAProcessMultiple.aspx?processid="
                + (ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString());

            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvRiskAssessmentProcess);
        }

        gvRiskAssessmentProcess.DataSource = ds;
        gvRiskAssessmentProcess.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDACTIVITYCONDITION", "FLDCATEGORYNAME", "FLDPROCESSNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Activity/Condition", "Category", "Process", "Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessMultipleSearch(
                                         (ViewState["PROCESSID"] == null ? null : General.GetNullableGuid(ViewState["PROCESSID"].ToString())),
                                         gvRiskAssessmentProcess.CurrentPageIndex + 1,
                                         gvRiskAssessmentProcess.PageSize,
                                         ref iRowCount,
                                         ref iTotalPageCount);

            General.ShowExcel("Risk Assessment-Process Multiple RA", ds.Tables[0], alColumns, alCaptions, null, null);

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
            RadLabel lblProcessMultipleId = ((RadLabel)gvRiskAssessmentProcess.Items[rowindex].FindControl("lblProcessMultipleId"));
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
        //gvRiskAssessmentProcess.SelectedIndex = -1;

        //for (int i = 0; i < gvRiskAssessmentProcess.Rows.Count; i++)
        //{
        //    if (gvRiskAssessmentProcess.DataKeys[i].Value.ToString().Equals(Filter.CurrentMultipleRASelection.ToString()))
        //    {
        //        gvRiskAssessmentProcess.SelectedIndex = i;
        //    }
        //}
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Inspection/InspectionRAProcess.aspx?processid=" + ViewState["PROCESSID"] + "&status=" + (ViewState["status"] == null ? null : ViewState["status"].ToString()));
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            {
                Response.Redirect("../Inspection/InspectionMainFleetRAProcessList.aspx");
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRAProcessList.aspx");
            }
        }
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    protected void gvRiskAssessmentProcess_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            //}

        }
    }

    protected void gvRiskAssessmentProcess_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        //GridView _gridView = (GridView)sender;
        //int nRow = int.Parse(gce.CommandArgument.ToString());
        if (gce.Item is GridDataItem)
        {
            RadGrid _gridView = (RadGrid)sender;
            int nRow = gce.Item.ItemIndex;
            if (gce.CommandName.ToUpper().Equals("EDIT"))
            {
               // gvRiskAssessmentProcess.SelectedCellIndexes = nRow;
                BindPageURL(nRow);
                //SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblProcessMultipleId = (RadLabel)gce.Item.FindControl("lblProcessMultipleId");
                if (lblProcessMultipleId != null)
                {
                    PhoenixInspectionRiskAssessmentProcess.DeleteRAProcessMultiple(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(lblProcessMultipleId.Text));
                }
            }
            BindData();
        }
    }

    //protected void gvRiskAssessmentProcess_RowEditing(object sender, GridViewEditEventArgs e)add
    //{
    //    BindData();
    //}

    //protected void gvRiskAssessmentProcess_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    Filter.CurrentMultipleRASelection = null;
    //    BindData();
    //}

    protected void gvRiskAssessmentProcess_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        // gvRiskAssessmentProcess.SelectedIndex = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
        //BindData();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //Filter.CurrentMultipleRASelection = null;
        BindData();
    }

    protected void gvRiskAssessmentProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
