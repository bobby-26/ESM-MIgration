using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearItemCost : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemCost.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemCost.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemCost.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ZONE"] = "1";
                gvRegistersworkinggearitem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE" };
        string[] alCaptions = { "Working Gear Item", "Unit Price" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixRegistersWorkingGearItem.WorkingGearItemCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        txtItemSearch.Text,
                                        General.GetNullableInteger(ucZone.SelectedZone != "" ? ucZone.SelectedZone : ViewState["ZONE"].ToString()),
                                        General.GetNullableInteger(ucMultiAddr.SelectedValue),
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Working Gear Item Cost", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtItemSearch.Text = "";
                ucZone.SelectedZone = "";
                ucMultiAddr.SelectedValue = null;
                ucMultiAddr.Text = "";
                BindData();
                gvRegistersworkinggearitem.Rebind();
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

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE" };
        string[] alCaptions = { "Working Gear Item", "Unit Price" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearItem.WorkingGearItemCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        txtItemSearch.Text,
                                        General.GetNullableInteger(ucZone.SelectedZone != "" ? ucZone.SelectedZone : ViewState["ZONE"].ToString()),
                                        General.GetNullableInteger(ucMultiAddr.SelectedValue),
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear Item Cost", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRegistersworkinggearitem.DataSource = ds;
            gvRegistersworkinggearitem.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRegistersworkinggearitem.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRegistersworkinggearitem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void UpdateWorkingGearItemunitPrice(int zoneid, int? supplierid, string itemid, decimal? unitprice)
    {
        if (!IsValidWorkingGearItemUpdate(unitprice, supplierid))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersWorkingGearItem.UpdateWorkingGearItemCost(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , zoneid
                                                            , supplierid
                                                            , new Guid(itemid)
                                                            , unitprice);
    }


    private bool IsValidWorkingGearItemUpdate(decimal? unitprice, int? supplierid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supplierid == null)
            ucError.ErrorMessage = "Supplier is required.";
        if (unitprice == null || unitprice <= 0)
            ucError.ErrorMessage = "Unit Price is required.";

        return (!ucError.IsError);
    }

    private bool IsInteger(string strVal)
    {
        try
        {
            int intout = Int32.Parse(strVal);
            return true;
        }
        catch
        {
            return false;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRegistersworkinggearitem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateWorkingGearItemunitPrice(
                    int.Parse(ucZone.SelectedZone)
                    , General.GetNullableInteger(ucMultiAddr.SelectedValue)
                    , ((RadLabel)e.Item.FindControl("lblItemidEdit")).Text/*((Label)_gridView.Rows[nCurrentRow].FindControl("lblItemidEdit")).Text*/
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtUnitPricedit")).Text)
                    );
                BindData();
                gvRegistersworkinggearitem.Rebind();
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

    protected void gvRegistersworkinggearitem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersworkinggearitem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegistersworkinggearitem_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {          
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);            
        }
    }
}

            
        
