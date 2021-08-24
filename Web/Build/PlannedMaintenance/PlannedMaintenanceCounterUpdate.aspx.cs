using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceCounterUpdate : PhoenixBasePage
{
    //private const int _firstEditCellIndex = 2;//cell edit instead of row edit
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Counter", "COUNTER", ToolBarDirection.Left);
        toolbarmain.AddButton("Avg Run Hrs", "AVGRUNHRS", ToolBarDirection.Left);
        MenuMain.MenuList = toolbarmain.Show();
        MenuMain.SelectedMenuIndex = 0;


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCounter.MenuList = toolbar.Show();

        PhoenixToolbar toolbarbtn = new PhoenixToolbar();

        toolbarbtn.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarbtn.AddFontAwesomeButton("javascript:CallPrint('gvCounterUpdate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarbtn.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        
        MenugvCounterUpdate.MenuList = toolbarbtn.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            gvCounterUpdate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuCounter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "SAVE")
            {
                string counterid = string.Empty;
                foreach (GridDataItem gv in gvCounterUpdate.Items)
                {
                    RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
                    if (sel.Checked == true)
                        counterid += ((RadLabel)gv.FindControl("lblCounterID")).Text + ",";
                }
                if (!IsValidCounter(counterid, txtRunHour.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateCounterUpdate(counterid, txtRunHour.Text, null);
                gvCounterUpdate.Rebind();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCounterUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvCounterUpdate.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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
        string Compno = "";
        if (txtComponentCode.TextWithPromptAndLiterals.Length >= 3)
        {
            Compno = txtComponentCode.TextWithPromptAndLiterals;
        }

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME", "FLDCURRENTVALUE", "FLDREADINGDATE" };
        string[] alCaptions = { "Component Number", "Component Name", "Counter Type", "Current Value", "Date Read" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceCounter.CounterSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableString(Compno)
                    , General.GetNullableString(txtNameTitle.Text)
                    , General.GetNullableDateTime(txtDateRead.Text)
                    , sortexpression, sortdirection, gvCounterUpdate.CurrentPageIndex + 1, gvCounterUpdate.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCounterUpdate", "Counter Update", alCaptions, alColumns, ds);

        gvCounterUpdate.DataSource = ds;
        gvCounterUpdate.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {

        string Compno = "";
        if (txtComponentCode.TextWithLiterals.Length > 3)
        {
            Compno = txtComponentCode.TextWithLiterals;
        }
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME", "FLDCURRENTVALUE", "FLDREADINGDATE" };
        string[] alCaptions = { "Component Number", "Component Name", "Counter Type", "Current Value", "Date Read" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceCounter.CounterSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , General.GetNullableString(Compno)
                                                            , General.GetNullableString(txtNameTitle.Text)
                                                            , General.GetNullableDateTime(txtDateRead.Text)
                                                            , sortexpression, sortdirection
                                                            , gvCounterUpdate.CurrentPageIndex + 1
                                                            , gvCounterUpdate.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        General.ShowExcel("Counter Update", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "UPDATE")
        {
            try
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.RowIndex;

                RadLabel CounterID = (RadLabel)e.Item.FindControl("lblCounterID");
                UserControlDecimal CurrentValue = (UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit");
                UserControlDate ReadingDate = (UserControlDate)e.Item.FindControl("txtReadingDateEdit");

                UpdateCounterUpdate(CounterID.Text, CurrentValue.Text, ReadingDate.Text);
                BindData();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvCounterUpdate.CurrentPageIndex = 0;
        }
    }

    private bool IsValidCounter(string csvCounterId, string runhour)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (csvCounterId.Trim().Equals(""))
            ucError.ErrorMessage = "Select atleast one or more Counter to Update.";

        if (!General.GetNullableInteger(runhour).HasValue)
            ucError.ErrorMessage = "Increase or Decrease Run Hour  is required.";

        return (!ucError.IsError);
    }
    private void UpdateCounterUpdate(string countersid, string currentvalues, string readdate)
    {
        PhoenixPlannedMaintenanceCounter.UpdateCounters(countersid, General.GetNullableDecimal(currentvalues), General.GetNullableDateTime(readdate), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    }
    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        //CheckBox chk = sender as CheckBox;
        //foreach (GridDataItem gv in gvCounterUpdate.Items)
        //{
        //    CheckBox sel = (CheckBox)gv.FindControl("chkSelect");
        //    if (chk.Checked)
        //        sel.Checked = true;
        //    else
        //        sel.Checked = false;
        //}

        Telerik.Web.UI.RadCheckBox chk = sender as Telerik.Web.UI.RadCheckBox;
        foreach (GridDataItem gv in gvCounterUpdate.Items)
        {
            RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
            if (chk.Checked == true)
                sel.Checked = true;
            else
                sel.Checked = false;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "AVGRUNHRS")
            {
                Response.Redirect(Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentAverageRunHours.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
