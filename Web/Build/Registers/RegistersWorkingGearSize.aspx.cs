using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;

public partial class Registers_RegistersWorkingGearSize : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSize.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvSize.UniqueID, "Edit$" + r.RowIndex.ToString());
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
            toolbar.AddImageButton("../Registers/RegistersWorkingGearSize.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSize')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersUnit.AccessRights = this.ViewState;
            MenuRegistersUnit.MenuList = toolbar.Show();
            MenuRegistersUnit.SetTrigger(pnlUnitEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ITEMID"] = null;
                ViewState["ITEMID"] = Request.QueryString["itemid"];
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
        string[] alColumns = { "FLDSIZENAME", "FLDUNITPRICE" };
        string[] alCaptions = { "Size Name", "Unit Price" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersWorkingGearItem.WorkingGearSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                             General.GetNullableGuid(ViewState["ITEMID"].ToString()),
                                                             sortexpression,
                                                             sortdirection,
                                                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                             iRowCount,
                                                             ref iRowCount,
                                                             ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=WokingGearSizeandPrice.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Working Gear Size and Price</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<br />");
        Response.Write("<tr></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
        Response.Write("<tr><td>Item Name:" + ds.Tables[0].Rows[0]["FLDWORKINGGEARITEMNAME"].ToString().ToUpper() + "</td></tr>");
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


    protected void RegistersUnit_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSIZENAME", "FLDUNITPRICE" };
        string[] alCaptions = { "Size Name", "Unit Price" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersWorkingGearItem.WorkingGearSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        General.GetNullableGuid(ViewState["ITEMID"].ToString()),
                                                                        sortexpression,
                                                                        sortdirection,
                                                                        (int)ViewState["PAGENUMBER"],
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);
        General.SetPrintOptions("gvSize", "Working Gear Size And Price", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvSize.DataSource = ds;
            gvSize.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSize);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    protected void gvSize_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSize_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSize.EditIndex = -1;
        gvSize.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvSize_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSize, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvSize_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidSize(((UserControls_UserControlSize)_gridView.FooterRow.FindControl("ucSizeAdd")).SelectedSize,
                                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucUnitPriceAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string D = ((UserControls_UserControlSize)_gridView.FooterRow.FindControl("ucSizeAdd")).SelectedSize;
                InsertSize(
                     General.GetNullableGuid(ViewState["ITEMID"].ToString()),
                    int.Parse(((UserControls_UserControlSize)_gridView.FooterRow.FindControl("ucSizeAdd")).SelectedSize),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucUnitPriceAdd")).Text)
                );
                BindData();
                ((UserControls_UserControlSize)_gridView.FooterRow.FindControl("ucSizeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSize(((UserControls_UserControlSize)_gridView.Rows[nCurrentRow].FindControl("ucSizeEdit")).SelectedSize,
                                   ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucUnitPriceEdit")).Text)
                                  )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateSize(
                     General.GetNullableGuid(ViewState["ITEMID"].ToString()),
                   int.Parse(((UserControls_UserControlSize)_gridView.Rows[nCurrentRow].FindControl("ucSizeEdit")).SelectedSize),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucUnitPriceEdit")).Text)

                 );
                _gridView.EditIndex = -1;
                BindData();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSize(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSizeId")).Text));
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSize_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSize_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        if (_gridView.EditIndex > -1)
            _gridView.UpdateRow(_gridView.EditIndex, false);

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((UserControls_UserControlSize)_gridView.Rows[de.NewEditIndex].FindControl("ucSizeEdit")).Focus();
        SetPageNavigator();
    }
    protected void gvSize_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            UpdateSize(
                        General.GetNullableGuid(ViewState["ITEMID"].ToString()),
                        int.Parse(((UserControls_UserControlSize)_gridView.Rows[nCurrentRow].FindControl("ucSizeEdit")).SelectedSize),
                     General.GetNullableDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucUnitPriceEdit")).Text)
                        );

            ucStatus.Text = "Size information updated";
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

    protected void gvSize_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

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
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UserControls_UserControlSize ucsize = (UserControls_UserControlSize)e.Row.FindControl("ucSizeEdit");

            if (ucsize != null)
                ucsize.SelectedSize = drv["FLDSIZEID"].ToString();
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSize.EditIndex = -1;
        gvSize.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvSize.EditIndex = -1;
        gvSize.SelectedIndex = -1;
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
        gvSize.SelectedIndex = -1;
        gvSize.EditIndex = -1;
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


    private void InsertSize(Guid? ItemId, int Sizeid, decimal? price)
    {

        PhoenixRegistersWorkingGearItem.InsertSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ItemId, Sizeid, price);
    }

    private void UpdateSize(Guid? ItemId, int Sizeid, decimal? price)
    {

        PhoenixRegistersWorkingGearItem.UpdateSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ItemId, Sizeid, price);
    }

    private bool IsValidSize(string Sizename, string Price)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvSize;

        if (Sizename.Trim().Equals("") || Sizename=="Dummy")
            ucError.ErrorMessage = "Size Name is required.";
        if (Price.Trim().Equals("") )
            ucError.ErrorMessage = "Unit Price is Required.";
        return (!ucError.IsError);
    }

    private void DeleteSize(int Sizeid)
    {
        PhoenixRegistersWorkingGearItem.DeleteSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Sizeid);
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
