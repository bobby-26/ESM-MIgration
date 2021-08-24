using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsVictuallingrate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("D11 Consumption", "D11CONSUMPTION", ToolBarDirection.Right);
        MenuCBA.AccessRights = this.ViewState;
        MenuCBA.MenuList = toolbar1.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsVictuallingrate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOvertimeHolidays')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsVictuallingrate.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuOverTimeHolidayList.AccessRights = this.ViewState;
        MenuOverTimeHolidayList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvOvertimeHolidays.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuCBA_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("D11CONSUMPTION"))
            {
                Response.Redirect("VesselAccountsD11Consumption.aspx");
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

        string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDVICTUALRATE", "FLDD11SHOWYNDESC" };
        string[] alCaptions = { "From", "To", "Victualling Rate.", "Show in Report YN" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselAccountsVictuallingRate.VesselVictuallingRateSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                              , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                              , iRowCount
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount);

        General.ShowExcel("Victualling Rate List", ds.Tables[0], alColumns, alCaptions, null, null);

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDVICTUALRATE", "FLDD11SHOWYNDESC" };
        string[] alCaptions = { "From", "To", "Victualling Rate.", "Show in Report YN" };

        DataSet ds = new DataSet();

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselAccountsVictuallingRate.VesselVictuallingRateSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                              , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                              , gvOvertimeHolidays.PageSize
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount
                                                                              );

        General.SetPrintOptions("gvOvertimeHolidays", "Victualling Rate", alCaptions, alColumns, ds);

        gvOvertimeHolidays.DataSource = ds;
        gvOvertimeHolidays.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void SetVessel(Object sender, EventArgs e)
    {   
        Rebind();        
    }
    protected void OverTimeHolidayList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlVessel.SelectedVessel = "";
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
        gvOvertimeHolidays.SelectedIndexes.Clear();
        gvOvertimeHolidays.EditIndexes.Clear();
        gvOvertimeHolidays.DataSource = null;
        gvOvertimeHolidays.Rebind();
    }
    private bool IsValidData(string fromdate, string todate, string Rate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ddlVessel.SelectedVessel == "Dummy")
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (General.GetNullableDateTime(fromdate) == null)
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        if (General.GetNullableDateTime(todate) == null)
        {
            ucError.ErrorMessage = "To Date is required";
        }
        if (General.GetNullableDecimal(Rate) == null)
        {
            ucError.ErrorMessage = "Victualling Rate is required";
        }
        else if (Decimal.Parse(Rate) < 0)
        {
            ucError.ErrorMessage = "Victualling Rate is greater than zero.";
        }
        return (!ucError.IsError);
    }



    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvOvertimeHolidays_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string fromdate = ((UserControlDate)e.Item.FindControl("txtFromDateAdd")).Text;
                string Todate = ((UserControlDate)e.Item.FindControl("txtToDateAdd")).Text;
                string Rate = ((UserControlMaskNumber)e.Item.FindControl("txtVictuallingRateAdd")).Text;
                string showyn = ((CheckBox)e.Item.FindControl("txtShowYNAdd")).Checked == true ? "1" : "0";
                string txtSeparateDeliverychargesYNAdd = ((RadCheckBox)e.Item.FindControl("txtSeparateDeliverychargesYNAdd")).Checked == true ? "1" : "0";
                if (!IsValidData(fromdate, Todate, Rate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsVictuallingRate.VesselVictuallingRateInsert(int.Parse(ddlVessel.SelectedVessel)
                                                                                , DateTime.Parse(fromdate), DateTime.Parse(Todate)
                                                                                , decimal.Parse(Rate), int.Parse(showyn), General.GetNullableInteger(txtSeparateDeliverychargesYNAdd));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string fromdate = ((UserControlDate)e.Item.FindControl("txtFromDateEdit")).Text;
                string Todate = ((UserControlDate)e.Item.FindControl("txtToDateEdit")).Text;
                string Rate = ((UserControlMaskNumber)e.Item.FindControl("txtVictuallingRateEdit")).Text;
                string id = ((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text;
                string showyn = ((RadCheckBox)e.Item.FindControl("txtShowYNEdit")).Checked == true ? "1" : "0";
                string txtSeparateDeliverychargesYNEdit = ((RadCheckBox)e.Item.FindControl("txtSeparateDeliverychargesYNEdit")).Checked == true ? "1" : "0";
                if (!IsValidData(fromdate, Todate, Rate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsVictuallingRate.VesselVictuallingRateUpdate(new Guid(id), DateTime.Parse(fromdate), DateTime.Parse(Todate)
                                                                                 , Decimal.Parse(Rate), int.Parse(showyn),General.GetNullableInteger(txtSeparateDeliverychargesYNEdit));
                Rebind();
            }

            else if (e.CommandName == "Page")
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
    protected void gvOvertimeHolidays_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOvertimeHolidays.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOvertimeHolidays_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
