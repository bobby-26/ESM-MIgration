using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaBuddyPlannerStatus : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPreSeaPlannerStatus.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvPreSeaPlannerStatus.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Planner", "PLANNER");
        MenuPreSeaPlannerStatus.AccessRights = this.ViewState;
        MenuPreSeaPlannerStatus.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddImageButton("../PreSea/PreSeaBuddyPlannerStatus.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbarmain.AddImageLink("javascript:CallPrint('gvPreSeaPlannerStatus')", "Print Grid", "icon_print.png", "PRINT");
        toolbarmain.AddImageButton("../PreSea/PreSeaBuddyPlannerStatus.aspx", "Find", "search.png", "FIND");
        MenuPreSeaPlanner.AccessRights = this.ViewState;
        MenuPreSeaPlanner.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            BindFaculty();
        }
        
        BindData();

    }

    protected void BindFaculty()
    {
        ddlFaculty.DataSource = PhoenixPreSeaBuddyPlanner.PreSeaFacultySearch();
        ddlFaculty.DataTextField = "FLDUSERNAME";
        ddlFaculty.DataValueField = "FLDUSERCODE";
        ddlFaculty.DataBind();
        ddlFaculty.Items.Insert(0, new ListItem("--Select--", "DUMMY"));
    }

    protected void MenuPreSeaPlannerStatus_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PLANNER"))
        {
            Response.Redirect("../Presea/PreSeaBuddyPlanner.aspx", true);
        }

    }

    protected void MenuPreSeaPlanner_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
        }

    }

    private void ShowExcel()
    {
        string[] alColumns = { "FLDFACULTYNAME", "FLDPLANNEDDDATE", "FLDCOMPLETED", "FLDREMARKS" };
        string[] alCaptions = { "Faculty Name", "Planned Date", "Completed Yes/No", "Remarks" };

        DataSet ds = new DataSet();

        ds = PhoenixPreSeaBuddyPlanner.PreSeaPlannerStatusUpdateSearch(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text), General.GetNullableInteger(ddlFaculty.SelectedValue));

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=PlannerStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><h3>Planner Status</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    private void BindData()
    {
        string[] alColumns = { "FLDFACULTYNAME", "FLDPLANNEDDDATE", "FLDCOMPLETED", "FLDREMARKS" };
        string[] alCaptions = { "Faculty Name", "Planned Date", "Completed Yes/No", "Remarks" };

        DataSet ds = new DataSet();

        ds = PhoenixPreSeaBuddyPlanner.PreSeaPlannerStatusUpdateSearch(General.GetNullableDateTime(ucFromDate.Text),General.GetNullableDateTime(ucToDate.Text),General.GetNullableInteger(ddlFaculty.SelectedValue));

        General.SetPrintOptions("gvPreSeaPlannerStatus", "", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaPlannerStatus.DataSource = ds;
            gvPreSeaPlannerStatus.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaPlannerStatus);
        }
    }
    protected void gvPreSeaPlannerStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvPreSeaPlannerStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    
    protected void gvPreSeaPlannerStatus_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSeaPlannerStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        string strCompleted;
        if ((((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblSelection")).SelectedItem == null)  || (((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblSelection")).SelectedValue == "0"))
            strCompleted = "0";
        else
            strCompleted = "1";
        try
        {
           
            
            PhoenixPreSeaBuddyPlanner.PreSeaPlannerStatusUpdate(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBuddyPlannerId")).Text)
                                                                , General.GetNullableInteger(strCompleted)
                                                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtemarks")).Text);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;

        BindData();
    }
    protected void gvPreSeaPlannerStatus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPreSeaPlannerStatus, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvPreSeaPlannerStatus_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;

            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblSelection");
            if (rbl != null)
            {
                if (drv["FLDISCOMPLETED"].ToString() != "")
                {
                    rbl.SelectedValue = drv["FLDISCOMPLETED"].ToString();
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
