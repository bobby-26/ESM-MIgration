using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class DryDockVesselParticulars : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../DryDock/DryDockVesselParticulars.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvVesselParticulars')", "Print Grid", "icon_print.png", "PRINT");
                MenuDryDockVesselParticulars.AccessRights = this.ViewState;
                MenuDryDockVesselParticulars.MenuList = toolbar.Show();
                
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
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVALUE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Value", "Description" };

        ds = PhoenixDryDockVesselParticulars.DryDockVesselParticularsList(General.GetNullableInteger(Filter.CurrentVesselMasterFilter));

        Response.AddHeader("Content-Disposition", "attachment; filename=DryDockVesselParticulars.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel particulars</h3></td>");
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

    private void BindData()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVALUE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Value", "Description" };

        ds = PhoenixDryDockVesselParticulars.DryDockVesselParticularsList(General.GetNullableInteger(Filter.CurrentVesselMasterFilter));

        General.SetPrintOptions("gvVesselParticulars", "Vessel Particulars", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselParticulars.DataSource = ds;
            gvVesselParticulars.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselParticulars);
        }
    }

    protected void DryDockVesselParticulars_TabStripCommand(object sender, EventArgs e)
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

    protected void gvVesselParticulars_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidVesselParticulars(
                    Filter.CurrentVesselMasterFilter,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateDryDockVesselParticulars(
                    int.Parse(Filter.CurrentVesselMasterFilter),
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblShortcodeEdit")).Text,
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDescriptionEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text
                 );
                _gridView.EditIndex = -1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselParticulars_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

        }

    }

    protected void gvVesselParticulars_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }

    protected void gvVesselParticulars_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
    }

    protected void gvVesselParticulars_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvVesselParticulars_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
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

    private void UpdateDryDockVesselParticulars(int vesselid, string shortname, string description, string value)
    {
        PhoenixDryDockVesselParticulars.DryDockVesselParticularsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            vesselid, shortname, description, value);
    }

    private bool IsValidVesselParticulars(string vesselid, string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
        {
            ucError.ErrorMessage = "Vessel Name is required.";
        }
        if (value.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Value is required.";
        }

        return (!ucError.IsError);
    }

}
