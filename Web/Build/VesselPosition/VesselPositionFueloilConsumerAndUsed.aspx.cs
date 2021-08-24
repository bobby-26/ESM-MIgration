using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;

public partial class VesselPositionFueloilConsumerAndUsed : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvOilType.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvOilType.UniqueID, "Edit$" + r.RowIndex.ToString());
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
            toolbar.AddImageButton("../VesselPosition/VesselPositionFueloilConsumerAndUsed.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOilType')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersOilType.AccessRights = this.ViewState;
            MenuRegistersOilType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindData();

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersOilType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvOilType.EditIndex = -1;
                gvOilType.SelectedIndex = -1;
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

    protected void gvOilType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((TextBox)_gridView.FooterRow.FindControl("txtNoAdd")).Text.ToString()),(((TextBox)_gridView.FooterRow.FindControl("txtoilconsumersAdd")).Text), (((TextBox)_gridView.FooterRow.FindControl("txtPowerAdd")).Text), ((TextBox)_gridView.FooterRow.FindControl("txtFueloiltypesAdd")).Text))
                    return;

                PhoenixVesselPositionIMODCSFuelConsumption.InsertIMODCSFuelOilconsumption(
                    int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    ((TextBox)_gridView.FooterRow.FindControl("txtoilconsumersAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtPowerAdd")).Text,
                    General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtNoAdd")).Text.ToString()),
                    ((TextBox)_gridView.FooterRow.FindControl("txtFueloiltypesAdd")).Text);

                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.DeleteIMODCSFuelOilconsumption((General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbloilconsumerid")).Text)));
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool checkvalue(string number, string consumer, string power, string oiltype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((number == null) || (number == ""))
            ucError.ErrorMessage = "No. is required.";

        if ((consumer == null) || (consumer == ""))
            ucError.ErrorMessage = "Engines or other fuel oil consumers is required.";

        if ((power == null) || (power == ""))
            ucError.ErrorMessage = "Power is required.";

        if ((oiltype == null) || (oiltype == ""))
            ucError.ErrorMessage = "Fuel oil types is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDENGINEOROTHERCONSUMER", "FLDPOWER", "FLDOILFUELTYPE" };
        string[] alCaptions = { "No.", "Engines or other fuel oil consumers", "Power", "Fuel oil types" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFuelOilconsumptionSearch(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOilType", "Water Type Register", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOilType.DataSource = ds;
            gvOilType.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOilType);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvOilType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

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

                DataRowView drv = (DataRowView)e.Row.DataItem;
 
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
                
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
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOilType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvOilType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOilType, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void gvOilType_RowEditing(object sender, GridViewEditEventArgs de)
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

    protected void gvOilType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!checkvalue((((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNoEdit")).Text),(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtoilconsumersEdit")).Text), (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPowerEdit")).Text), ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFueloiltypesEdit")).Text))
                return;

            PhoenixVesselPositionIMODCSFuelConsumption.UpdateIMODCSFuelOilconsumption(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbloilconsumeridedit")).Text),
                General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtoilconsumersEdit")).Text),
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPowerEdit")).Text,
                General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNoEdit")).Text),
                General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFueloiltypesEdit")).Text));

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

    protected void gvOilType_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvOilType_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvOilType.EditIndex = -1;
        gvOilType.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvOilType.SelectedIndex = -1;
        gvOilType.EditIndex = -1;
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
        gvOilType.SelectedIndex = -1;
        gvOilType.EditIndex = -1;
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOilType.SelectedIndex = -1;
        gvOilType.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNUMBER", "FLDENGINEOROTHERCONSUMER", "FLDPOWER", "FLDOILFUELTYPE" };
        string[] alCaptions = { "No.", "Engines or other fuel oil consumers", "Power", "Fuel oil types" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFuelOilconsumptionSearch(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"oiltypesused.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Ship engines and other fuel oil consumers and fuel oil types used</h3></td>");
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
}
