using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsD11Consumption : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsD11Consumption.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOvertimeHolidays')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsD11Consumption.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuOverTimeHolidayList.AccessRights = this.ViewState;
        MenuOverTimeHolidayList.MenuList = toolbar.Show();
        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1 = new PhoenixToolbar();

        toolbar1.AddButton("Back", "D11CONSUMPTION", ToolBarDirection.Right);

        MenuCBA.MenuList = toolbar1.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
            ddlYear.SelectedYear = DateTime.Today.Year;
            gvOvertimeHolidays.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        ds = PhoenixVesselAccountsVictuallingRate.VesselD11ConsuptionSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
            , General.GetNullableInteger(ddlMonth.SelectedMonth.ToString()), General.GetNullableInteger(ddlYear.SelectedYear.ToString())
            , int.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Consumption List", ds.Tables[0], alColumns, alCaptions, null, null);

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


        ds = PhoenixVesselAccountsVictuallingRate.VesselD11ConsuptionSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
            , General.GetNullableInteger(ddlMonth.SelectedMonth.ToString()), General.GetNullableInteger(ddlYear.SelectedYear.ToString())
            , int.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount, ref iRowCount, ref iTotalPageCount);


        General.SetPrintOptions("gvOvertimeHolidays", "Consumption List", alCaptions, alColumns, ds);

        gvOvertimeHolidays.DataSource = ds;
        gvOvertimeHolidays.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuCBA_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("D11CONSUMPTION"))
            {
                Response.Redirect("VesselAccountsVictuallingrate.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private bool IsValidData(string currencyid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ddlVessel.SelectedVessel == "Dummy" || ddlVessel.SelectedVessel == "")
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (General.GetNullableInteger(ddlMonth.SelectedMonth.ToString()) == null)
        {
            ucError.ErrorMessage = "Month is required.";
        }
        if (General.GetNullableInteger(ddlYear.SelectedYear.ToString()) == null)
        {
            ucError.ErrorMessage = "Year is required";
        }
        if (General.GetNullableDecimal(currencyid) == null)
        {
            ucError.ErrorMessage = "Currency is required";
        }
        if (General.GetNullableDecimal(amount) == null)
        {
            ucError.ErrorMessage = "Amount is required";
        }
        else if (Decimal.Parse(amount) < 0)
        {
            ucError.ErrorMessage = "Amount is greater than or equal to zero.";
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
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
              

                string Currencyid = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string Amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text;
                RadCheckBox chkDeliveryChargeYNAdd = ((RadCheckBox)e.Item.FindControl("chkDeliveryChargeYNAdd"));
                if (!IsValidData(Currencyid, Amount))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsVictuallingRate.VesselD11ConsuptionInsert(int.Parse(ddlVessel.SelectedVessel)
                                                                                ,General.GetNullableInteger(ddlMonth.SelectedMonth.ToString())
                                                                                , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                                , General.GetNullableInteger(Currencyid), decimal.Parse(Amount)
                                                                                , (chkDeliveryChargeYNAdd.Checked) == true ? 1 : 0);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string Currencyid = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
                string Amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
                string id = ((RadLabel)e.Item.FindControl("lblid")).Text;
                RadCheckBox chkDeliveryChargeYNEdit = ((RadCheckBox)eeditedItem.FindControl("chkDeliveryChargeYNEdit"));
                if (!IsValidData(Currencyid, Amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsVictuallingRate.VesselD11ConsuptionUpdate(int.Parse(ddlVessel.SelectedVessel)
                                                                               , General.GetNullableInteger(ddlMonth.SelectedMonth.ToString())
                                                                               , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                               , General.GetNullableInteger(Currencyid), decimal.Parse(Amount),
                                                                               (chkDeliveryChargeYNEdit.Checked) == true ? 1 : 0,new Guid(id));
             
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

        if (e.Item.IsInEditMode)
        {
          
            UserControlCurrency ddlCurrency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            string Currencyid = ((RadLabel)e.Item.FindControl("lblCurrencyid")).Text;

            if (Currencyid != null)
            {
                ddlCurrency.SelectedCurrency = Currencyid;
            }
        }
    }
}
