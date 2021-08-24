using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVehicleType : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVehicleType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVehicleType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuVehicleType.AccessRights = this.ViewState;
            MenuVehicleType.MenuList = toolbar.Show();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDVEHICLENAME" };
        string[] alCaptions = { "Vehicle Type" };
        DataTable dt = PhoenixRegistersVehicleType.ListVehicleType();
        General.ShowExcel("Vehicle Type", dt, alColumns, alCaptions, null, string.Empty);
    }

    protected void VehicleType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvVehicleType.Rebind();
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
        string[] alColumns = { "FLDVEHICLENAME" };
        string[] alCaptions = { "Vehicle Type" };

        DataTable dt = PhoenixRegistersVehicleType.ListVehicleType();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvVehicleType", "Vehicle Type", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvVehicleType.DataSource = dt;
            //gvVehicleType.DataBind();
        }
        else
        {
            gvVehicleType.DataSource = "";
        }
    }

    protected void gvVehicleType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string lblVehicleEdit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVehicleEdit")).Text;
            string txtVehicleNameEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtVehicleNameEdit")).Text;
            if (!IsValidVehicleType(txtVehicleNameEdit))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersVehicleType.UpdateVehicleType(int.Parse(lblVehicleEdit), txtVehicleNameEdit);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidVehicleType(string VehicleName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (VehicleName.Trim().Equals(""))
            ucError.ErrorMessage = "Vehicle Type is required.";

        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvVehicleType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string txtVehicleNameAdd = ((RadTextBox)e.Item.FindControl("txtVehicleNameAdd")).Text;
                if (!IsValidVehicleType(txtVehicleNameAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersVehicleType.InsertVehicleType(txtVehicleNameAdd);
                BindData();
                gvVehicleType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string lblVehicleEdit = ((RadLabel)e.Item.FindControl("lblVehicleEdit")).Text;
                string txtVehicleNameEdit = ((RadTextBox)e.Item.FindControl("txtVehicleNameEdit")).Text;
                if (!IsValidVehicleType(txtVehicleNameEdit))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersVehicleType.UpdateVehicleType(int.Parse(lblVehicleEdit), txtVehicleNameEdit);                
                BindData();
                gvVehicleType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
              
                int id = General.GetNullableInteger((e.Item as GridEditableItem).GetDataKeyValue("FLDVEHICLETYPE").ToString()).Value;
                PhoenixRegistersVehicleType.DeleteVehicleType(id);
                BindData();
                gvVehicleType.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVehicleType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVehicleType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
}
