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
using Telerik.Web.UI;

public partial class Registers_RegistersMovementLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersMovementLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMovementLog')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersMovementLog.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersMovementLog.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersMovementLog.AccessRights = this.ViewState;
            MenuRegistersMovementLog.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMovementLog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDNAME", "FLDFROMDATE", "FLDTODATE", "FLDCONTRACTDETAIL", "FLDCURRENCYNAME", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Name", "Rank", "Vessel",  "Movement", "From Date", "To Date", "Contract", "Currency", "Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersMovementLog.MovementLogSearch(
                txtName.Text,
                General.GetNullableInteger(ddlVessel.SelectedVessel),
                General.GetNullableInteger(ddlMovement.SelectedMovement),
                General.GetNullableDateTime(txtFromDate.Text),
                General.GetNullableDateTime(txtToDate.Text),
                1,
                gvMovementLog.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("MovementLog", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuRegistersMovementLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtName.Text = "";
                ddlVessel.SelectedVessel = "";
                ddlMovement.SelectedMovement = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvMovementLog.SelectedIndexes.Clear();
        gvMovementLog.EditIndexes.Clear();
        gvMovementLog.DataSource = null;
        gvMovementLog.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDNAME", "FLDFROMDATE", "FLDTODATE", "FLDCONTRACTDETAIL", "FLDCURRENCYNAME", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Name", "Rank", "Vessel", "Movement", "From Date", "To Date", "Contract", "Currency", "Amount" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersMovementLog.MovementLogSearch(
                   txtName.Text,
                   General.GetNullableInteger(ddlVessel.SelectedVessel),
                   General.GetNullableInteger(ddlMovement.SelectedMovement),
                   General.GetNullableDateTime(txtFromDate.Text),
                   General.GetNullableDateTime(txtToDate.Text),
                   1,
                   gvMovementLog.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);

        General.SetPrintOptions("gvMovementLog", "MovementLog", alCaptions, alColumns, ds);

        gvMovementLog.DataSource = ds;
        gvMovementLog.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMovementLog_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvMovementLog.Rebind();
    }
    protected void gvMovementLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMovementLog.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMovementLog_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
 
}