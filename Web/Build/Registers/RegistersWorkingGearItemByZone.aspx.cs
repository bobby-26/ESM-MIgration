using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearItemByZone : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemByZone.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemByZone.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemByZone.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();
          
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ZONE"] = "1";
                gvRegistersworkinggearitem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ucZone.SelectedZone = ViewState["ZONE"].ToString();
            }
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
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
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDSTOCKQUANTITY", "FLDUNITPRICE", "FLDSTOCKVALUE" };
        string[] alCaptions = { "Working Gear Item", "Stock in Hand (Qty) ", "Unit Price", "Stock Value (INR)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearItem.WorkingGearItembyZoneSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        txtItemSearch.Text,
                                        General.GetNullableInteger(ucZone.SelectedZone != "" ? ucZone.SelectedZone : ViewState["ZONE"].ToString()),
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Working Gear Item By Zone", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
                ViewState["ZONE"] = "1";
                ucZone.SelectedZone = "";
                ucZone.SelectedZone = "1";
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

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDSTOCKQUANTITY", "FLDUNITPRICE", "FLDSTOCKVALUE" };
        string[] alCaptions = { "Working Gear Item", "Stock in Hand (Qty)", "Unit Price", "Stock Value (INR)"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearItem.WorkingGearItembyZoneSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        txtItemSearch.Text,
                                        General.GetNullableInteger(ucZone.SelectedZone!=""? ucZone.SelectedZone: ViewState["ZONE"].ToString()),
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitem","Working Gear Item By Zone", alCaptions, alColumns, ds);
        
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
    

    private void InsertWorkingGearItem(String itemtypeid,string size)
    {
        if (!IsValidWorkingGearItemInsert(itemtypeid, size))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItem.InsertWorkingGearItem( PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               ,new Guid(itemtypeid)
                                                               ,int.Parse(size)
                                                              
                                                           );
    }
    private void UpdateWorkingGearItem(int zoneid, string itemid, decimal? unitprice, decimal? stockinhand)
    {
        if (!IsValidWorkingGearItemUpdate(unitprice, stockinhand))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItem.UpdateWorkingGearItembyzone(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , zoneid
                                                            , new Guid(itemid)
                                                            , unitprice
                                                            , stockinhand);
    }


    protected void gvRegistersworkinggearitem_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        PhoenixRegistersWorkingGearItem.DeleteWorkingGearItembyzone(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          ,int.Parse(ucZone.SelectedZone)
                                                          ,new Guid(((RadLabel)e.Item.FindControl("lblWorkingGearitemId")).Text)                                                          
                                                          );
        BindData();
        gvRegistersworkinggearitem.Rebind();
    }

    private bool IsValidWorkingGearItemInsert(string itemid, string size)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (itemid == "" || itemid == null)
                ucError.ErrorMessage = "Working gear Item Type is required.";
        if ( size =="" || size ==null)
            ucError.ErrorMessage = "Working gear size is required.";
 
        return (!ucError.IsError);
    }

    private bool IsValidWorkingGearItemUpdate(decimal? unitprice, decimal? stockinhand)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (stockinhand == null|| stockinhand <= 0)
            ucError.ErrorMessage = "Stock in Hand is required.";
        if ( unitprice==null || unitprice <= 0 )
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
                UpdateWorkingGearItem(
                    int.Parse(ucZone.SelectedZone)
                    , ((RadLabel)e.Item.FindControl("lblItemidEdit")).Text
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtUnitPricedit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtWorkingGearStockinhandedit")).Text)
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

    protected void gvRegistersworkinggearitem_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
            UserControlWorkingGearType uctype = (UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesEdit");
            UserControlWorkingGearItemType ucWorkingGearItemTypesEdit = (UserControlWorkingGearItemType)e.Item.FindControl("ucWorkingGearItemTypesEdit");
            UserControls_UserControlSize ucSizeEdit = (UserControls_UserControlSize)e.Item.FindControl("ucSizeEdit");

            UserControlMaskNumber txtWorkingGearStockinhandedit = (UserControlMaskNumber)e.Item.FindControl("txtWorkingGearStockinhandedit");
            RadLabel lblWorkingGearStockinHandEdit = (RadLabel)e.Item.FindControl("lblWorkingGearStockinHandEdit");

            UserControlUnit ucunit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            if (uctype != null)
                uctype.SelectedGearType = drv["FLDWORKINGGEARTYPEID"].ToString();
            if (ucunit != null)
                ucunit.SelectedUnit = drv["FLDUNIT"].ToString();
            if (ucWorkingGearItemTypesEdit != null)
                ucWorkingGearItemTypesEdit.SelectedGearType = drv["FLDWORKINGGEARITEMTYPEID"].ToString();
            if (ucSizeEdit != null)
                ucSizeEdit.SelectedSize = drv["FLDSIZEID"].ToString();

            if (drv["FLDSTOCKQUANTITY"].ToString() != "0.00")
            {
                if (txtWorkingGearStockinhandedit != null)
                {
                    txtWorkingGearStockinhandedit.Visible = false;
                    lblWorkingGearStockinHandEdit.Visible = true;
                }
            }
        }
    }
            

    protected void gvRegistersworkinggearitem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersworkinggearitem.CurrentPageIndex + 1;
        BindData();
    }

}
