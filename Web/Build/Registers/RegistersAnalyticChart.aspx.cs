using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class RegistersAnalyticChart : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAnalyticChart.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAnalyticList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersAnalyticChart.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersAnalyticChart.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuList.AccessRights = this.ViewState;
            MenuList.MenuList = toolbar.Show();
           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                txtOwnerCode.Text = "";
                txtOwnerId.Text = "";
                txtOwnerName.Text = "";
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDMODULENAME", "FLDCHARTNAME", "FLDURL" };
        string[] alCaptions = { "Module", "Chart Name", "URL", };

        DataTable dt = PhoenixRegistersAnalytic.ListAnalyticChart(General.GetNullableInteger(txtOwnerId.Text), null);

        General.ShowExcel("Analytic Chart", dt, alColumns, alCaptions, null, null);

    }

    private void BindData()
    {
        string[] alColumns = { "FLDMODULENAME","FLDCHARTNAME", "FLDURL" };
        string[] alCaptions = { "Module","Chart Name", "URL",  };

        DataTable dt = PhoenixRegistersAnalytic.ListAnalyticChart(General.GetNullableInteger(txtOwnerId.Text), null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAnalyticList", "Analytic Chart", alCaptions, alColumns, ds);
        if (dt.Rows.Count  > 0 )
        {
            gvAnalyticList.DataSource = dt;
            gvAnalyticList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvAnalyticList);
        }
    }

    protected void gvAnalyticList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;        
        BindData();
    }

    protected void gvAnalyticList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ownerid = txtOwnerId.Text;
                string module = ((TextBox)_gridView.FooterRow.FindControl("txtModuleNameAdd")).Text;
                string chart = ((TextBox)_gridView.FooterRow.FindControl("txtChartNameAdd")).Text;
                string url = ((TextBox)_gridView.FooterRow.FindControl("txtURLAdd")).Text;
                if (!IsValidAnalytic(ownerid, module, chart, url))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersAnalytic.InsertAnalyticChart(int.Parse(ownerid), module, chart, url);
                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtChartNameAdd")).Focus();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAnalyticList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            string analyticid = _gridView.DataKeys[de.RowIndex].Value.ToString();
            PhoenixRegistersAnalytic.DeleteAnalyticChart(int.Parse(analyticid));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAnalyticList_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;        
        _gridView.SelectedIndex = de.NewEditIndex;
      
        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtChartNameEdit")).Focus();
        
    }
    protected void gvAnalyticList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            string analyticid = _gridView.DataKeys[e.RowIndex].Value.ToString();
            string module = ((TextBox)_gridView.Rows[e.RowIndex].FindControl("txtModuleNameEdit")).Text;
            string chart = ((TextBox)_gridView.Rows[e.RowIndex].FindControl("txtChartNameEdit")).Text;
            string url = ((TextBox)_gridView.Rows[e.RowIndex].FindControl("txtURLEdit")).Text;
            if (!IsValidAnalytic(analyticid, module, chart, url))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersAnalytic.UpdateAnalyticChart(int.Parse(analyticid), module, chart, url);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAnalyticList_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        Label lblurl = (Label)e.Row.FindControl("lblURL");     
        if (lblurl != null)
        {
            lblurl.Text = (drv["FLDURL"].ToString().Length > 50 ? drv["FLDURL"].ToString().Substring(0, 50) + "..." : drv["FLDURL"].ToString());
            UserControlToolTip tooltip = (UserControlToolTip)e.Row.FindControl("ucToolTip");
            lblurl.Attributes.Add("onmouseover", "showTooltip(ev, '" + tooltip.ToolTip + "', 'visible');");
            lblurl.Attributes.Add("onmouseout", "showTooltip(ev, '" + tooltip.ToolTip + "', 'hidden');");
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
    }
    private bool IsValidAnalytic(string ownerid,string Module, string ChartName, string URL)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(!General.GetNullableInteger(ownerid).HasValue)
        {
            ucError.ErrorMessage = "Owner is required.";
        }

        if (Module.Trim().Equals(""))
            ucError.ErrorMessage = "Module is required.";

        if (ChartName.Trim().Equals(""))
            ucError.ErrorMessage = "Chart Name is required.";

        if (URL.Trim().Equals(""))
            ucError.ErrorMessage = "URL Name is required.";
       
        return (!ucError.IsError);
    }
}
