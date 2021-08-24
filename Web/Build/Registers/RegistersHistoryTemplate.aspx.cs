using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.StandardForm;


public partial class RegistersHistoryTemplate : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvHistoryTemplate.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvHistoryTemplate.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    } 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersHistoryTemplate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvHistoryTemplate')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersHistoryTemplate.aspx", "Find", "search.png", "FIND");
            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            BindData();
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

        string[] alColumns = { "FLDTEMPLATENAME", "FLDFORMNAME" };
        string[] alCaptions = { "Template Name", "Form Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersHistoryTemplate.SearchHistoryTemplate(txtName.Text, sortexpression, sortdirection,
             (int)ViewState["PAGENUMBER"],
           iRowCount,
             ref iRowCount,
             ref iTotalPageCount);

        General.ShowExcel("Work Report Template", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void HistoryTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvHistoryTemplate.EditIndex = -1;
                gvHistoryTemplate.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTEMPLATENAME", "FLDFORMNAME" };
            string[] alCaptions = { "Template Name", "Form Name" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataTable dt = PhoenixRegistersHistoryTemplate.SearchHistoryTemplate(txtName.Text,
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvHistoryTemplate", "Work Report Template", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvHistoryTemplate.DataSource = dt;
                gvHistoryTemplate.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvHistoryTemplate);
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
    protected void gvHistoryTemplate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvHistoryTemplate_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvHistoryTemplate.EditIndex = -1;
        gvHistoryTemplate.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvHistoryTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string TemplateName = ((TextBox)_gridView.FooterRow.FindControl("txtTemplateNameAdd")).Text;
                string FormId = ((DropDownList)_gridView.FooterRow.FindControl("ddlFormAdd")).SelectedValue;
                if (!IsValidHistoryTemplate(TemplateName, FormId))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersHistoryTemplate.InsertHistoryTemplate(TemplateName, int.Parse(FormId));
                ((TextBox)_gridView.FooterRow.FindControl("txtTemplateNameAdd")).Focus();
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvHistoryTemplate_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtTemplateNameEdit")).Focus();

    }
    protected void gvHistoryTemplate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string TemplateId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplateIdEdit")).Text;
            string TemplateName = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTemplateNameEdit")).Text;
            string FormId = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlFormEdit")).SelectedValue;
            if (!IsValidHistoryTemplate(TemplateName, FormId))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersHistoryTemplate.UpdateHistoryTemplate(int.Parse(TemplateId), TemplateName, int.Parse(FormId));
            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHistoryTemplate_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    
                }

                string FormUrl = ((Label)e.Row.FindControl("lblFormUrl")).Text;
                string mode = "DUMMY";
                ImageButton db1 = (ImageButton)e.Row.FindControl("cmdViewForm");
                if (db1 != null)
                    db1.Attributes.Add("onclick", "javascript:parent.Openpopup('CAPP','','../StandardForm/" + FormUrl + "?mode=" + mode + "'); return false;");
            }
            else
            {
                DropDownList ddl = e.Row.FindControl("ddlFormEdit") as DropDownList;
                ddl.DataSource = PhoenixStandardForm.ListStandardForm();
                ddl.DataBind();
                foreach (ListItem item in ddl.Items)
                {
                    if (item.Value == drv["FLDFORMID"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = e.Row.FindControl("ddlFormAdd") as DropDownList;
            ddl.DataSource = PhoenixStandardForm.ListStandardForm();
            ddl.DataBind();

            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvHistoryTemplate_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;
            string TemplateId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTemplateId")).Text;
            PhoenixRegistersHistoryTemplate.DeleteHistoryTemplate(int.Parse(TemplateId));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvHistoryTemplate.EditIndex = -1;
        gvHistoryTemplate.SelectedIndex = -1;
        BindData();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvHistoryTemplate.EditIndex = -1;
        gvHistoryTemplate.SelectedIndex = -1;
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
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvHistoryTemplate.SelectedIndex = -1;
        gvHistoryTemplate.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
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
    private bool IsValidHistoryTemplate(string TemplateName, string FormId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (TemplateName.Trim().Equals(""))
            ucError.ErrorMessage = "Template Name is required.";

        if (!General.GetNullableInteger(FormId).HasValue)
            ucError.ErrorMessage = "Form Name is required.";

        return (!ucError.IsError);
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
    protected void ddlForm_DataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
