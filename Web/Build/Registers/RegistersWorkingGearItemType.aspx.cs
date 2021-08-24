using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearItemType : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitemType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemType.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearItemType.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
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
        string[] alColumns = { "FLDWORKINGGEARITEMTYPENAME", "FLDGEARTYPE", "FLDUNITNAME" };
        string[] alCaptions = { "Working Gear Item Type", "Working Gear Type", "Unit" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearItemType.WorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtItemSearch.Text,
                                        General.GetNullableInteger(ucWorkingGearType.SelectedGearType),
                                        sortexpression,
                                        sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitemType.PageSize, ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Item Type", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
                gvRegistersworkinggearitemType.Rebind();
               
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
                gvRegistersworkinggearitemType.Rebind();
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

        string[] alColumns = { "FLDWORKINGGEARITEMTYPENAME", "FLDGEARTYPE", "FLDUNITNAME" };
        string[] alCaptions = { "Working Gear Item Type", "Working Gear Type", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearItemType.WorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtItemSearch.Text,
                                        General.GetNullableInteger(ucWorkingGearType.SelectedGearType),
                                        sortexpression,
                                        sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitemType.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitemType", "Working Gear Item Type", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRegistersworkinggearitemType.DataSource = ds;
            gvRegistersworkinggearitemType.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRegistersworkinggearitemType.DataSource = "";            
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRegistersworkinggearitemType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void InsertWorkingGearItem(string itemname, string workinggaretype, string itemunit)
    {
        if (!IsValidWorkingGearItemInsert(itemname, workinggaretype, itemunit))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItemType.InsertWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , itemname.Trim()
                                                            , Int32.Parse(workinggaretype)
                                                            , Int32.Parse(itemunit));
    }

    private void UpdateWorkingGearItem(string itemid, string itemname, string workinggaretype, string itemunit)
    {
        if (!IsValidWorkingGearItemUpdate(itemname, workinggaretype, itemunit))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItemType.UpdateWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(itemid), itemname.Trim(), Int32.Parse(workinggaretype), Int32.Parse(itemunit));
    }

    private bool IsValidWorkingGearItemInsert(string itemname, string workinggaretype, string itemunit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (itemname.Trim().Equals(""))
            ucError.ErrorMessage = "Working Gear Item Type is required.";
        if (workinggaretype.Trim().Equals("") || IsInteger(workinggaretype) == false)
            ucError.ErrorMessage = "Working Gear Type is required.";
        if (itemunit.Trim().Equals("") || IsInteger(itemunit) == false)
            ucError.ErrorMessage = "Unit is required.";
        return (!ucError.IsError);
    }

    private bool IsValidWorkingGearItemUpdate(string itemname, string workinggaretype, string itemunit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (itemname.Trim().Equals(""))
            ucError.ErrorMessage = "Working Gear Item Type is required.";
        if (workinggaretype.Trim().Equals("") || IsInteger(workinggaretype) == false)
            ucError.ErrorMessage = "Working Gear Type is required.";
        if (itemunit.Trim().Equals("") || IsInteger(itemunit) == false)
            ucError.ErrorMessage = "Unit is required.";

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
        PhoenixRegistersWorkingGearItemType.DeleteWorkingGearItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(WorkingGearItemId));
    }

    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRegistersworkinggearitemType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitemType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertWorkingGearItem(
                    ((RadTextBox)e.Item.FindControl("txtItemNameAdd")).Text,
                    ((UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesAdd")).SelectedGearType,
                    ((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit);
                BindData();
                gvRegistersworkinggearitemType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateWorkingGearItem(
                    ((RadLabel)e.Item.FindControl("lblItemidEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtItemNameEdit")).Text,
                    ((UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesEdit")).SelectedGearType,
                   ((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit
                 );               
                BindData();
                gvRegistersworkinggearitemType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteWorkingGearItem(((RadLabel)e.Item.FindControl("lblWorkingGearitemId")).Text);
                BindData();
                gvRegistersworkinggearitemType.Rebind();
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

    protected void gvRegistersworkinggearitemType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersworkinggearitemType.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegistersworkinggearitemType_ItemDataBound(object sender, GridItemEventArgs e)
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

            UserControlWorkingGearType uctype = (UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesEdit");
            UserControlUnit ucunit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            if (uctype != null)
                uctype.SelectedGearType = drv["FLDWORKINGGEARTYPEID"].ToString();
            if (ucunit != null)
                ucunit.SelectedUnit = drv["FLDUNIT"].ToString();
        }
        if(e.Item is GridFooterItem)
        {
            if (ucWorkingGearType.SelectedGearType != null && ucWorkingGearType.SelectedGearType != "Dummy")
            {
                ((UserControlWorkingGearType)e.Item.FindControl("ucWorkingGearTypesAdd")).SelectedGearType = ucWorkingGearType.SelectedGearType;
            }
        }
        
    }
}
