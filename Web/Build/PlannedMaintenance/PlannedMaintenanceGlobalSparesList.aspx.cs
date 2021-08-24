using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceGlobalSparesList : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            ViewState["BOOKID"] = "0";
            ViewState["TITLE"] = "";
            ViewState["MODEL"] = "";

            if (Request.QueryString["MODELID"] != null && Request.QueryString["MODELID"].ToString() != "")
                ViewState["MODELID"] = Request.QueryString["MODELID"].ToString();
            else
                ViewState["MODELID"] = "0";

            if (Request.QueryString["MODEL"] != null && Request.QueryString["MODEL"].ToString() != "")
                ViewState["MODEL"] = Request.QueryString["MODEL"].ToString();

            gvSpares.PageSize = General.ShowRecords(null);
        }
        BindMenu();

        
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvSpares_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 10;
        int iTotalPageCount = 10;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;
        if (ViewState["NVC"] != null)
            nvc = (NameValueCollection)ViewState["NVC"];

        ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsSearch(
                                                       nvc.Get("NUMBER")!=null?General.GetNullableString(nvc.Get("NUMBER").ToString()):null,
                                                       int.Parse(ViewState["MODELID"].ToString()),
                                                       int.Parse(ViewState["BOOKID"].ToString()),
                                                       nvc.Get("NAME") != null ? General.GetNullableString(nvc.Get("NAME").ToString()) : null,
                                                       null,
                                                       sortexpression, sortdirection,
                                                       gvSpares.CurrentPageIndex + 1,
                                                       gvSpares.PageSize, ref iRowCount, ref iTotalPageCount, 
                                                       nvc.Get("MAKERREF") != null ? General.GetNullableString(nvc.Get("MAKERREF").ToString()) : null,
                                                       nvc.Get("COMPONENT") != null ? General.GetNullableString(nvc.Get("COMPONENT").ToString()) : null);

        gvSpares.DataSource = ds;
        gvSpares.VirtualItemCount = iRowCount;
    }

    protected void gvSpares_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //DataRowView drv = (DataRowView)item.DataItem; 
            //UserControlMultiColumnComponentType ucComponent = (UserControlMultiColumnComponentType)item["COMPONENT"].FindControl("ucComponent");
            //if (ucComponent != null)
            //{
            //    ucComponent.Model = ViewState["MODELID"].ToString();
            //    ucComponent.SelectedValue = drv["FLDGLOBALCOMPONENTTYPEID"].ToString();
            //}
            RadComboBox ddlComponent = (RadComboBox)item["COMPONENT"].FindControl("ddlComponent");
            RadLabel lblComponent = (RadLabel)item["NUMBER"].FindControl("lblComponentID");
            RadLabel lblMaker = (RadLabel)item["NUMBER"].FindControl("lblMakerID");
            RadLabel lblVendor = (RadLabel)item["NUMBER"].FindControl("lblVendorID");
            RadLabel lblUnitID = (RadLabel)item["NUMBER"].FindControl("lblUnitID");
            if (ddlComponent != null && lblComponent!=null)
            {
                int iTotalCount = 10;
                int iTotalPage = 10;
                ddlComponent.DataSource = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeFilterSearch(null, null, null, null, null, null, 1, 10000, ref iTotalCount, ref iTotalPage, General.GetNullableInteger(ViewState["MODELID"].ToString()));
                ddlComponent.DataBind();
                ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

                ddlComponent.SelectedValue= lblComponent.Text;
            }

            UserControlMultiColumnAddress ucMaker = (UserControlMultiColumnAddress)item["MAKER"].FindControl("ucMaker");
            if (ucMaker != null&& lblMaker!=null)
            {
                ucMaker.SelectedValue = lblMaker.Text;
            }
            UserControlMultiColumnAddress ucVendor = (UserControlMultiColumnAddress)item["VENDOR"].FindControl("ucVendor");
            if (ucVendor != null && lblVendor!=null)
            {
                ucVendor.SelectedValue = lblVendor.Text;
            }
            UserControlUnit ucUnit = (UserControlUnit)item["UNIT"].FindControl("ucUnit");
            if (ucUnit != null && lblUnitID!=null)
            {
                ucUnit.UnitList = PhoenixRegistersUnit.ListUnit();
                ucUnit.DataBind();

                if (General.GetNullableInteger(lblUnitID.Text) != null)
                    ucUnit.SelectedValue = lblUnitID.Text;
            }
        }
    }

    protected void gvSpares_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                gvSpares.ExportSettings.FileName = "SPARES_" + ViewState["TITLE"].ToString();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                String id = ((RadLabel)item["NUMBER"].FindControl("lblID")).Text;

                if (!isValidData(((RadTextBox)item["NAME"].FindControl("txtName")).Text
                                , ((RadComboBox)item["COMPONENT"].FindControl("ddlComponent")).SelectedValue
                                , ((UserControlUnit)item["Unit"].FindControl("ucUnit")).SelectedValue
                    ))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                if (General.GetNullableGuid(id) != null)
                    PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsUpdate(new Guid(id), new Guid(((RadComboBox)item["COMPONENT"].FindControl("ddlComponent")).SelectedValue)
                                                     , ((RadTextBox)item["NAME"].FindControl("txtName")).Text
                                                     , General.GetNullableInteger(((UserControlMultiColumnAddress)item["MAKER"].FindControl("ucMaker")).SelectedValue)
                                                     , General.GetNullableInteger(((UserControlMultiColumnAddress)item["VENDOR"].FindControl("ucVendor")).SelectedValue)
                                                     , General.GetNullableString(((RadTextBox)item["MAKERREF"].FindControl("txtMakerRef")).Text)
                                                     , int.Parse(((UserControlUnit)item["UNIT"].FindControl("ucUnit")).SelectedValue)
                                                     , null
                        );
                else
                {
                    PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsInsert(new Guid(((RadComboBox)item["COMPONENT"].FindControl("ddlComponent")).SelectedValue)
                                                     , int.Parse(ViewState["MODELID"].ToString())
                                                     , int.Parse(ViewState["BOOKID"].ToString())
                                                     , ((RadTextBox)item["NAME"].FindControl("txtName")).Text
                                                     , General.GetNullableInteger(((UserControlMultiColumnAddress)item["MAKER"].FindControl("ucMaker")).SelectedValue)
                                                     , General.GetNullableInteger(((UserControlMultiColumnAddress)item["VENDOR"].FindControl("ucVendor")).SelectedValue)
                                                     , General.GetNullableString(((RadTextBox)item["MAKERREF"].FindControl("txtMakerRef")).Text)
                                                     , int.Parse(((UserControlUnit)item["UNIT"].FindControl("ucUnit")).SelectedValue)
                                                     , null
                        );
                }

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartDelete(new Guid(item.GetDataKeyValue("FLDID").ToString()));
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                GridFilteringItem filterItem = e.Item as GridFilteringItem;
                foreach (GridColumn c in gvSpares.MasterTableView.Columns)
                {
                    if (filterItem[c.UniqueName].HasControls())
                    {
                        TextBox filterBox = filterItem[c.UniqueName].Controls[0] as TextBox;
                        nvc.Remove(c.UniqueName);
                        nvc.Add(c.UniqueName, filterBox.Text);
                    }
                    ViewState["NVC"] = nvc;
                }
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        


    }

    protected void gvSpares_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {

    }

    protected void gvSpares_ItemUpdated(object sender, Telerik.Web.UI.GridUpdatedEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            String id = item.GetDataKeyValue("FLDID").ToString();

            if (!isValidData(((RadTextBox)item["NAME"].FindControl("txtName")).Text
                            , ((RadComboBox)item["COMPONENT"].FindControl("ddlComponent")).SelectedValue
                            , ((UserControlUnit)item["Unit"].FindControl("ucUnit")).SelectedValue
                ))
            {
                ucError.Visible = true;
                
                return;
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvSpares_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        foreach (GridBatchEditingCommand command in e.Commands)
        {
            Hashtable newValues = command.NewValues;
            Hashtable oldValues = command.OldValues;
        }
        gvSpares.Rebind();
    }
    protected bool isValidData(string name,string component,string unit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(name) == null)
            ucError.ErrorMessage = "Name is required";
        if (General.GetNullableGuid(component) == null)
            ucError.ErrorMessage = "Component is required";
        if (General.GetNullableInteger(unit) == null)
            ucError.ErrorMessage = "Unit is required";

        return (!ucError.IsError);
    }

    protected void gvBooks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookList(
                                                       General.GetNullableInteger(ViewState["MODELID"].ToString())
                                                       );

        if (ds.Tables[0].Rows.Count > 0 && (ViewState["BOOKID"] == null || ViewState["BOOKID"].ToString() == "0"))
        {
            ViewState["BOOKID"] = ds.Tables[0].Rows[0]["FLDBOOKID"].ToString();
            ViewState["TITLE"] = ds.Tables[0].Rows[0]["FLDTITLE"].ToString();
            BindMenu();
        }
            

        gvBooks.DataSource = ds;
    }

    protected void gvBooks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                int? id = General.GetNullableInteger(((RadLabel)item["TITLE"].FindControl("lblID")).Text);

                if (General.GetNullableString(((RadTextBox)item["TITLE"].FindControl("txtTitle")).Text)==null) 
                {
                    ucError.ErrorMessage = "Title is Required";
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                    PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookInsert(((RadTextBox)item["TITLE"].FindControl("txtTitle")).Text
                                                     , int.Parse(ViewState["MODELID"].ToString())
                                                     , id
                        );
                

            }else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookDelete(int.Parse(item.GetDataKeyValue("FLDBOOKID").ToString()));
            }else if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["BOOKID"] = item.GetDataKeyValue("FLDBOOKID").ToString();
                ViewState["TITLE"] = ((RadLabel)item["TITLE"].FindControl("lblTitle")).Text;
                BindMenu();
                gvSpares.Rebind();
            }

        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindMenu()
    {
        MenuSpares.Title = "Spares [ "+ ViewState["MODEL"].ToString() + " ] [ " + ViewState["TITLE"].ToString() + " ]";
        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuSpares.MenuList = toolbar.Show();
    }
    protected void gvBooks_ItemUpdated(object sender, GridUpdatedEventArgs e)
    {

    }
}