using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearItem : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItem.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItem.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
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
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDWORKINGGEARITEMTYPENAME", "FLDSIZENAME" };
        string[] alCaptions = { "Working Gear Item ", "Working Gear Item Type", "Size" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearItem.WorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtItemSearch.Text,
                                         General.GetNullableInteger(ucWorkingGearType.SelectedGearType),
                                        sortexpression,
                                        sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Item", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
                ucWorkingGearType.SelectedGearType = "";
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

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDWORKINGGEARITEMTYPENAME", "FLDSIZENAME" };
        string[] alCaptions = { "Working Gear Item ", "Working Gear Item Type", "Size" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearItem.WorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtItemSearch.Text,
                                        General.GetNullableInteger(ucWorkingGearType.SelectedGearType),
                                        sortexpression,
                                        sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear Item", alCaptions, alColumns, ds);

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

    private void InsertWorkingGearItem(String itemtypeid, string size)
    {
        if (!IsValidWorkingGearItemInsert(itemtypeid, size))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItem.InsertWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , new Guid(itemtypeid)
                                                               , int.Parse(size)

                                                           );
    }

    private void UpdateWorkingGearItem(string itemid, string itemtype, string size)
    {
        if (!IsValidWorkingGearItemUpdate(itemtype, size))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItem.UpdateWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(itemid), new Guid(itemtype), int.Parse(size));
    }

    private bool IsValidWorkingGearItemInsert(string itemid, string size)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (itemid == "" || itemid == null || itemid == "Dummy")
            ucError.ErrorMessage = "Working gear Item Type is required.";
        if (size == "" || size == null || size == "Dummy")
            ucError.ErrorMessage = "Working gear size is required.";

        return (!ucError.IsError);
    }

    private bool IsValidWorkingGearItemUpdate(string itemtypeid, string size)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (itemtypeid == "" || itemtypeid == null || itemtypeid == "Dummy")
            ucError.ErrorMessage = "Working Gear Item type Name is required.";
        if (size == "" || size == null || size == "Dummy")
            ucError.ErrorMessage = "Working Gear size is required.";

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

    private void DeleteWorkingGearItem(string WorkingGearItemId)
    {
        PhoenixRegistersWorkingGearItem.DeleteWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(WorkingGearItemId));
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

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertWorkingGearItem(((UserControlWorkingGearItemType)e.Item.FindControl("ucWorkingGearItemTypesAdd")).SelectedGearType,
                    ((UserControls_UserControlSize)e.Item.FindControl("ucSizeAdd")).SelectedSize
                    );
                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateWorkingGearItem(
                    ((RadLabel)e.Item.FindControl("lblItemidEdit")).Text,
                    ((UserControlWorkingGearItemType)e.Item.FindControl("ucWorkingGearItemTypesEdit")).SelectedGearType,
                 ((UserControls_UserControlSize)e.Item.FindControl("ucSizeEdit")).SelectedSize
                 );
                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteWorkingGearItem(((RadLabel)e.Item.FindControl("lblWorkingGearitemId")).Text);
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");               
            }
            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            RadLabel lbitemid = (RadLabel)e.Item.FindControl("lblWorkingGearitemId");
            UserControlWorkingGearType uctype = (UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesEdit");
            UserControlWorkingGearItemType ucWorkingGearItemTypesEdit = (UserControlWorkingGearItemType)e.Item.FindControl("ucWorkingGearItemTypesEdit");
            UserControls_UserControlSize ucSizeEdit = (UserControls_UserControlSize)e.Item.FindControl("ucSizeEdit");
            UserControlUnit ucunit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            if (uctype != null)
                uctype.SelectedGearType = drv["FLDWORKINGGEARTYPEID"].ToString();
            if (ucunit != null)
                ucunit.SelectedUnit = drv["FLDUNIT"].ToString();
            if (ucWorkingGearItemTypesEdit != null)
            {
                ucWorkingGearItemTypesEdit.TypeList = PhoenixRegistersWorkingGearItemType.ListWorkingGearType(null);                
                ucWorkingGearItemTypesEdit.SelectedGearType = drv["FLDWORKINGGEARITEMTYPEID"].ToString();
            }
                
            if (ucSizeEdit != null)
            {
                ucSizeEdit.SizeList = PhoenixRegistersSize.ListSize();
                ucSizeEdit.SelectedSize = drv["FLDSIZEID"].ToString();
            }
                
        }
    }
}
       