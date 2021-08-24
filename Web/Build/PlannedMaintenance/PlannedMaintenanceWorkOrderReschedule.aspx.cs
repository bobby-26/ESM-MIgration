using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
public partial class PlannedMaintenanceWorkOrderReschedule : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvWorkOrderReschedule.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvWorkOrderReschedule.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReschedule.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvWorkOrderReschedule')", "Print Grid", "icon_print.png", "");
        MenuWorkOrderReschedule.MenuList = toolbargrid.Show();
        MenuWorkOrderReschedule.SetTrigger(pnlWorkOrderReschedule);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Detail", "DETAIL");
            
            MenuWorkOrderRescheduleMain.MenuList = toolbarmain.Show();
           
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WIEVCOMPONENTID"] = "";
            ViewState["SETCURRENTNAVIGATIONTAB"] = null;
            ViewState["COMPONENTJOBID"] = null;
            if (Request.QueryString["COMPONENTID"] != null)
                ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
            if (Request.QueryString["WORKORDERID"] != null)
            {
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
            }

            MenuWorkOrderRescheduleMain.SelectedMenuIndex = 0;

        }
        BindData();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME","FLDFREQUENCY", "FLDPLANINGPRIORITY", "FLDRANKNAME", "FLDPLANNINGESTIMETDURATION" ,"FLDPLANNINGDUEDATE"};
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name","Frequency", "Priority", "Resp Discipline","Estimation Duration",  "Due Date"};


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();
            if (Filter.CurrentRescheduleFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentRescheduleFilter;
                ds = PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
                    , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
                    , General.GetNullableString(nvc.Get("txtComponentNumber").ToString()),
                    General.GetNullableString(nvc.Get("txtComponentName").ToString()),
                    General.GetNullableString(nvc.Get("planning").ToString())
                    , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString())
                    , General.GetNullableDateTime(nvc.Get("txtDateTo").ToString()), General.GetNullableInteger(nvc.Get("ucMainType").ToString())
                    , General.GetNullableInteger(nvc.Get("ucMaintClass").ToString()), General.GetNullableInteger(nvc.Get("ucMainCause").ToString())
                    , null, General.GetNullableString(nvc.Get("status").ToString())
                    , General.GetNullableInteger(nvc.Get("txtPriority").ToString()), General.GetNullableInteger(nvc.Get("ucRank").ToString()),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount);
            }
            else
            {
                ds = null;
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvWorkOrderReschedule.DataSource = ds;
                gvWorkOrderReschedule.DataBind();
                if (ViewState["WORKORDERID"] == null)
                {
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    ViewState["COMPONENTJOBID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    gvWorkOrderReschedule.SelectedIndex = 0;
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=";
                }
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];

                SetRowSelection();
                SetTabHighlight();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvWorkOrderReschedule);
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx";
            }
            General.SetPrintOptions("gvWorkOrderReschedule", "Work Order", alCaptions, alColumns, ds);

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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDFREQUENCY", "FLDPLANINGPRIORITY", "FLDRANKNAME", "FLDPLANNINGESTIMETDURATION", "FLDPLANNINGDUEDATE" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Frequency", "Priority", "Resp Discipline", "Estimation Duration", "Due Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentRescheduleFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentRescheduleFilter;
            ds = PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderName").ToString())
                        , General.GetNullableGuid(ViewState["WIEVCOMPONENTID"].ToString())
                        , General.GetNullableString(nvc.Get("txtComponentNumber").ToString()),
                        General.GetNullableString(nvc.Get("txtComponentName").ToString()),
                        General.GetNullableString(nvc.Get("planning").ToString())
                        , General.GetNullableString(nvc.Get("jobclass").ToString()), General.GetNullableDateTime(nvc.Get("txtDateFrom").ToString())
                        , General.GetNullableDateTime(nvc.Get("txtDateTo").ToString()), General.GetNullableInteger(nvc.Get("ucMainType").ToString())
                        , General.GetNullableInteger(nvc.Get("ucMaintClass").ToString()), General.GetNullableInteger(nvc.Get("ucMainCause").ToString())
                        , null, General.GetNullableString(nvc.Get("status").ToString())
                        , General.GetNullableInteger(nvc.Get("txtPriority").ToString()), General.GetNullableInteger(nvc.Get("ucRank").ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);
        }
        General.ShowExcel("WorkOrder", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
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
    protected void MenuWorkOrderReschedule_TabStripCommand(object sender, EventArgs e)
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
    protected void MenuWorkOrderRescheduleMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("DETAIL"))
            {

                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrderReschedule_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }
    protected void gvWorkOrderReschedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderReschedule_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvWorkOrderReschedule, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
    }
    protected void gvWorkOrderReschedule_RowEditing(object sender, GridViewEditEventArgs de)
    {

        try
        {

            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
            {
                string dueDate = ((TextBox)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("ucDueDateEdit")).Text;
                string workorderid = ((Label)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("lblWorkOrderId")).Text;
                string priority = ((TextBox)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("txtPriorityEdit")).Text;
                string duration = ((TextBox)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("txtDurationEdit")).Text;
                string responsiblediscipline = ((UserControlRank)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("ucRankEdit")).SelectedRank;

                string changereason = ((TextBox)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("txtReason")).Text; ;
                PhoenixPlannedMaintenanceWorkOrderReschedule.UpdateWorkOrderReschedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , new Guid(workorderid), General.GetNullableInteger(priority), General.GetNullableInteger(responsiblediscipline), General.GetNullableInteger(duration), General.GetNullableDateTime(dueDate),General.GetNullableString(changereason));
                ((TextBox)gvWorkOrderReschedule.Rows[_gridView.EditIndex].FindControl("txtReason")).Text = "";
            }
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["WORKORDERID"] = ((Label)gvWorkOrderReschedule.Rows[de.NewEditIndex].FindControl("lblWorkOrderId")).Text;
            BindData();
            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=";
            }
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderReschedule_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton ib1 = (ImageButton)e.Row.FindControl("cmdShowReason");
                TextBox tb1 = (TextBox)e.Row.FindControl("txtReason");
                if (tb1 != null){tb1.Attributes.Add("style", "visibility:hidden");}
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceRemarksPopup.aspx?framename=filterandsearch&WORKORDERID=" + ViewState["WORKORDERID"].ToString()+"', true);");
            }

            UserControlRank ucRank = (UserControlRank)e.Row.FindControl("ucRankEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ucRank != null) ucRank.SelectedRank = drv["FLDRANKID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvWorkOrderRescheduler_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvWorkOrderReschedule.SelectedIndex = se.NewSelectedIndex;

        ViewState["COMPONENTID"] = ((Label)gvWorkOrderReschedule.Rows[se.NewSelectedIndex].FindControl("lblComponentId")).Text;
        ViewState["WORKORDERID"] = ((Label)gvWorkOrderReschedule.Rows[se.NewSelectedIndex].FindControl("lblWorkOrderId")).Text;
        ViewState["COMPONENTJOBID"] = ((Label)gvWorkOrderReschedule.Rows[se.NewSelectedIndex].FindControl("lblJobID")).Text;
        ViewState["DTKEY"] = ((Label)gvWorkOrderReschedule.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
        ComponentTypeNameClick();
        BindData();
        SetPageNavigator();

    }
    protected void ComponentTypeNameClick()
    {
        try
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SetRowSelection()
    {
        gvWorkOrderReschedule.SelectedIndex = -1;
        for (int i = 0; i < gvWorkOrderReschedule.Rows.Count; i++)
        {
            if (gvWorkOrderReschedule.DataKeys[i].Value.ToString().Equals(ViewState["WORKORDERID"].ToString()))
            {
                gvWorkOrderReschedule.SelectedIndex = i;
                ViewState["DTKEY"] = ((Label)gvWorkOrderReschedule.Rows[gvWorkOrderReschedule.SelectedIndex].FindControl("lbldtkey")).Text;
            }
        }
    }
    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRescheduleDetail.aspx"))
            {
                MenuWorkOrderRescheduleMain.SelectedMenuIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {

            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvWorkOrderReschedule.SelectedIndex = -1;
        gvWorkOrderReschedule.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvWorkOrderReschedule.EditIndex = -1;
        gvWorkOrderReschedule.SelectedIndex = -1;
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
