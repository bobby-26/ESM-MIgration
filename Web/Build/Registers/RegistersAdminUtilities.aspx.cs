using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersAdminUtilities : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminUtilities.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAdmUtility')", "Print Grid", "icon_print.png", "Print");
            toolbar.AddImageButton("../Registers/RegistersAdminUtilities.aspx", "Find", "search.png", "FIND");
            MenuRegistersAdminUtility.AccessRights = this.ViewState;
            MenuRegistersAdminUtility.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindData();
            }
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
        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixAdministrationUtilities.SearchUtilities(txtSearch.Text, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
             General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Admin Utilities.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Utility Type </h3></td>");
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
    protected void MenuRegistersAdminUtility_TabStripCommand(object sender, EventArgs e)
    {
        try
        {  
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvAdmUtility.SelectedIndex = -1;
                gvAdmUtility.EditIndex = -1;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression;
        int? sortdirection = null;

        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Name" };

        DataSet ds = new DataSet();

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAdministrationUtilities.SearchUtilities(txtSearch.Text, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvAdmUtility", "Admin Utilities", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdmUtility.DataSource = ds;
            gvAdmUtility.DataBind();

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAdmUtility);
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
        //SetPageNavigator();
    }
    protected void gvAdmUtility_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAdmUtility, "Edit$" + e.Row.RowIndex.ToString(), false);

        }
    }
    protected void gvAdmUtility_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        _gridview.EditIndex = -1;
        BindData();
    }
    protected void gvAdmUtility_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvAdmUtility_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridview = (GridView)sender;
            _gridview.EditIndex = de.NewEditIndex;
            _gridview.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridview.Rows[de.NewEditIndex].FindControl("txtAdmUtilityNameEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvAdmUtility_SelectedIndexChanging(object sender, GridViewSelectEventArgs de)
    {
        gvAdmUtility.SelectedIndex = de.NewSelectedIndex;
        
    }

    protected void gvAdmUtility_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertAdmUtility(
                    ((TextBox)_gridView.FooterRow.FindControl("txtAdmUtilityAdd")).Text);
                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtAdmUtilityAdd")).Focus();

            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateAdmUtility(
                    Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdmUtilityTypeIDEdit")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("lnkAdmUtilityName")).Text);
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteAdmUtility(
                    Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdmUtilityTypeID")).Text));

            }
            SetPageNavigator();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAdmUtility_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvAdmUtility.EditIndex = -1;
        gvAdmUtility.SelectedIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }
    protected void gvAdmUtility_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            UpdateAdmUtility(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdmUtilityTypeIDEdit")).Text),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAdmUtilityNameEdit")).Text);

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAdmUtility_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvAdmUtility.SelectedIndex = -1;
        gvAdmUtility.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvAdmUtility.SelectedIndex = -1;
        gvAdmUtility.EditIndex = -1;

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
        gvAdmUtility.SelectedIndex = -1;
        gvAdmUtility.EditIndex = -1;
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
    private void InsertAdmUtility(string AdmUtilityName)
    {
        if (!IsValidAdmUtilityName(AdmUtilityName))
        {
            ucError.Visible = true;
            return;
        }
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixAdministrationUtilities.InsertUtilities(rowusercode, AdmUtilityName);

    }
    private void UpdateAdmUtility(int AdmUtilityTypeID, string AdmUtilityName)
    {
        if (!IsValidAdmUtilityName(AdmUtilityName))
        {
            ucError.Visible = true;
            return;
        }
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixAdministrationUtilities.UpdateUtilities(rowusercode, AdmUtilityTypeID, AdmUtilityName);
        ucStatus.Text = "Utility Type Name is updated";
    }
    private bool IsValidAdmUtilityName(string AdmUtilityName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvAdmUtility;

        if (AdmUtilityName.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }
    private void DeleteAdmUtility(int AdmUtilityTypeID)
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixAdministrationUtilities.DeleteUtilities(rowusercode, AdmUtilityTypeID);
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

