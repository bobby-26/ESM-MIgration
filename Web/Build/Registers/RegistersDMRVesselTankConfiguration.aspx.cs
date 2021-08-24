using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersDMRVesselTankConfiguration : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvVesselTankConfig.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$lnkDoubleClick");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersDMRVesselTankConfiguration.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvVesselTankConfig')", "Print Grid", "icon_print.png", "PRINT");
            MenuDMRRangeConfig.AccessRights = this.ViewState;
            MenuDMRRangeConfig.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    UcVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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


        string[] alColumns = { "FLDVESSELNAME", "FLDHORIZONTALVALUE", "FLDVERTICALVALUE" };
        string[] alCaptions = { "Vessel", "Horizotal Value", "Vertical Value" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersDMRVesselTankConfig.DMRVesselTankConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DMRVesselTankConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Tank Configuration</h3></td>");
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

    protected void MenuDMRRangeConfig_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        //txtAgentName.Text = "";
        UcVessel.SelectedVessel = "";
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersDMRVesselTankConfig.DMRVesselTankConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        string[] alColumns = { "FLDVESSELNAME", "FLDHORIZONTALVALUE", "FLDVERTICALVALUE" };
        string[] alCaptions = { "Vessel", "Horizotal Value", "Vertical Value" };

        General.SetPrintOptions("gvVesselTankConfig", "Vessel-Tank Configuration", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselTankConfig.DataSource = ds;
            gvVesselTankConfig.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselTankConfig);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvVesselTankConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvVesselTankConfig_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string RowValue, ColumnValue,Vessel;
                RowValue = ((DropDownList)_gridView.FooterRow.FindControl("ddlHorizontalValueAdd")).SelectedValue;
                ColumnValue = ((DropDownList)_gridView.FooterRow.FindControl("ddlVerticalValueAdd")).SelectedValue;
                Vessel = ((UserControlVessel)_gridView.FooterRow.FindControl("ucVesselAdd")).SelectedVessel;

                if (!IsValidConfig(Vessel, RowValue, ColumnValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersDMRVesselTankConfig.InsertDMRVesselTankConfig(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(Vessel),
                    General.GetNullableInteger(RowValue),
                    General.GetNullableInteger(ColumnValue)
                    );

                BindData();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRVesselTankConfig.DeleteDMRVesselTankConfig(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid((((Label)_gridView.Rows[nCurrentRow].FindControl("lblConfigId")).Text)));
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidConfig(string vessel,string rowvalue,string columnvalue)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (General.GetNullableInteger(vessel)==null)
            ucError.ErrorMessage = "Vessel is required.";
        if (General.GetNullableInteger(rowvalue) == null)
            ucError.ErrorMessage = "Rowvalue is required.";
        if (General.GetNullableInteger(columnvalue) == null)
            ucError.ErrorMessage = "ColumnValue is required.";

        //if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
        //    ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }

    protected void gvVesselTankConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string  RowValue, ColumnValue, configid, Vessel;
            configid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblConfigIdEdit")).Text;
            Vessel = ((UserControlVessel)_gridView.Rows[nCurrentRow].FindControl("ucVesselEdit")).SelectedVessel;
            RowValue = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlHorizontalValueEdit")).SelectedValue;
            ColumnValue = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlVerticalValueEdit")).SelectedValue;


            if (!IsValidConfig(Vessel, RowValue, ColumnValue))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersDMRVesselTankConfig.UpdateDMRVesselTankConfig(
                new Guid(configid),
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(Vessel),
                General.GetNullableInteger(RowValue),
                General.GetNullableInteger(ColumnValue)
                );

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

    protected void gvVesselTankConfig_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvVesselTankConfig_RowEditing(object sender, GridViewEditEventArgs de)
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

    protected void gvVesselTankConfig_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvVesselTankConfig_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            _gridView.Columns[1].Visible = false;
        }

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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;       
            UserControlVessel ucVesselEdit = (UserControlVessel)e.Row.FindControl("ucVesselEdit");
            if (ucVesselEdit != null)
                ucVesselEdit.SelectedVessel = drv["FLDVESSELID"].ToString();

            DropDownList ddlHorizontalValueEdit = (DropDownList)e.Row.FindControl("ddlHorizontalValueEdit");
            if (ddlHorizontalValueEdit != null)
                ddlHorizontalValueEdit.SelectedValue = drv["FLDHORIZONTALVALUE"].ToString();

            DropDownList ddlVerticalValueEdit = (DropDownList)e.Row.FindControl("ddlVerticalValueEdit");
            if (ddlVerticalValueEdit != null)
                ddlVerticalValueEdit.SelectedValue = drv["FLDVERTICALVALUE"].ToString();

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");

                if (del != null)
                    if (!SessionUtil.CanAccess(this.ViewState, del.CommandName))
                        del.Visible = false;

                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");

                if (edit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName))
                        edit.Visible = false;

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        gvVesselTankConfig.SelectedIndex = -1;
        gvVesselTankConfig.EditIndex = -1;
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
