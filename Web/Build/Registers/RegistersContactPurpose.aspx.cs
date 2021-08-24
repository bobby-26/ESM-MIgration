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
using SouthNests.Phoenix.Registers;

public partial class RegistersContactPurpose : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersContactPurpose.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvContactPurpose')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersContactPurpose.aspx", "Find", "search.png", "FIND");
            MenuRegistersContactPurpose.AccessRights = this.ViewState;
            MenuRegistersContactPurpose.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            BindData();
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

        string[] alColumns = { "FLDPURPOSENAME", "FLDHARDNAME"};
        string[] alCaptions = { "Name", "Code"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersContactPurpose.SearchContactPurpose(txtpurposename.Text
                , General.GetNullableInteger(ddlpurposecode.SelectedHard)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Contacts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Contact Purpose Register</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
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

    protected void RegistersContactPurpose_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvContactPurpose.EditIndex = -1;
                gvContactPurpose.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPURPOSENAME", "FLDHARDNAME" };
        string[] alCaptions = { "Name", "Code" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersContactPurpose.SearchContactPurpose(txtpurposename.Text, 
                    General.GetNullableInteger(ddlpurposecode.SelectedHard), 
                    sortexpression, 
                    sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvContactPurpose", "Purpose Contacts", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvContactPurpose.DataSource = ds;
            gvContactPurpose.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvContactPurpose);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvContactPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPurposeContact(((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text,
                    ((UserControlHard)_gridView.FooterRow.FindControl("ddlPurposeCodeAdd")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertContactPurpose(
                    ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text,
                    int.Parse(((UserControlHard)_gridView.FooterRow.FindControl("ddlPurposeCodeAdd")).SelectedHard));
                ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPurposeContact(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text,
                    ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlPurposeCodeEdit")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateContactPurpose(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblpurposeidEdit")).Text),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text,
                     int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlPurposeCodeEdit")).SelectedHard)
                 );
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteContactPurpose(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblpurposeid")).Text));
            }

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvContactPurpose_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ddlPurposeCodeEdit");
            DataRowView drvHard = (DataRowView)e.Row.DataItem;
            if (ucHard != null)
            {
                ucHard.SelectedHard = drvHard["FLDPURPOSECODE"].ToString();
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ddlPurposeCodeAdd");

            if (ucHard != null)
            {
                ucHard.HardList = PhoenixRegistersHard.ListHard(1, 202);
                ucHard.DataBind();
            }
        }
    }

    protected void gvContactPurpose_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvContactPurpose_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvContactPurpose_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvContactPurpose_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvContactPurpose_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvContactPurpose.EditIndex = -1;
        gvContactPurpose.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private void InsertContactPurpose(string purposename, int purposecode)
    {
        PhoenixRegistersContactPurpose.InsertContactPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            purposename, purposecode);
    }

    private void UpdateContactPurpose(int purposeid, string purposename, int purposecode)
    {
        PhoenixRegistersContactPurpose.UpdateContactPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            purposeid, purposename, purposecode);
    }

    private void DeleteContactPurpose(int purposeid)
    {
        PhoenixRegistersContactPurpose.DeleteContactPurpose(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            purposeid);
    }

    private bool IsValidPurposeContact(string purposename, string purposecode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (purposename.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(purposecode) == null)
            ucError.ErrorMessage = "Code is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvContactPurpose.EditIndex = -1;
        gvContactPurpose.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvContactPurpose.EditIndex = -1;
        gvContactPurpose.SelectedIndex = -1;
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
        gvContactPurpose.SelectedIndex = -1;
        gvContactPurpose.EditIndex = -1;
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
}
