using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselPositionSIPTankCofiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                NameValueCollection nvc = Filter.CurrentSIPVesselFilter;
                if (nvc != null)
                {
                    UcVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
                }
            }
            UcVessel.DataBind();
            UcVessel.bind();
        }
    }
    protected void gvSIPTanksConfuguration_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel TankID = (RadLabel)e.Item.FindControl("lblTankID");
                PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
                RebindFuelTanks();
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadLabel lblTankId = (RadLabel)e.Item.FindControl("lblTankIdEdit");
                UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityAdd");
                RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameAdd");

                PhoenixVesselPositionSIPTankConfuguration.InsertSIPTankInsert(
                       General.GetNullableGuid("")
                       , General.GetNullableInteger(UcVessel.SelectedVessel)
                       , General.GetNullableDecimal(Capacity.Text)
                       , General.GetNullableGuid(FuelType.SelectedValue)
                       , 0
                       ,General.GetNullableString(txttankname.Text)
                       );
                RebindFuelTanks();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel TankID = (RadLabel)e.Item.FindControl("lblTankID");
                PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
                RebindServiceSettlingTanks();
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadLabel lblTankId = (RadLabel)e.Item.FindControl("lblTankIdEdit");
                UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityAdd");
                RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameAdd");

                PhoenixVesselPositionSIPTankConfuguration.InsertSIPTankInsert(
                       General.GetNullableGuid("")
                       , General.GetNullableInteger(UcVessel.SelectedVessel)
                       , General.GetNullableDecimal(Capacity.Text)
                       , General.GetNullableGuid(FuelType.SelectedValue)
                       , 1
                       , General.GetNullableString(txttankname.Text)
                       );
                RebindServiceSettlingTanks();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel TankID = (RadLabel)e.Item.FindControl("lblTankID");
                PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
                RebindLubeOilTanks();
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadLabel lblTankId = (RadLabel)e.Item.FindControl("lblTankIdEdit");
                UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityAdd");
                RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameAdd");

                PhoenixVesselPositionSIPTankConfuguration.InsertSIPTankInsert(
                       General.GetNullableGuid("")
                       , General.GetNullableInteger(UcVessel.SelectedVessel)
                       , General.GetNullableDecimal(Capacity.Text)
                       , General.GetNullableGuid(FuelType.SelectedValue)
                       , 2
                       , General.GetNullableString(txttankname.Text)
                       );
                RebindLubeOilTanks();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del!= null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    if (General.GetNullableGuid(drv["FLDFUELTYPEID"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDFUELTYPEID"].ToString();
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadComboBox ddlOilTypefooter = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                if (ddlOilTypefooter != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilTypefooter.DataSource = ds;
                    ddlOilTypefooter.DataBind();
                    ddlOilTypefooter.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    if (General.GetNullableGuid(drv["FLDFUELTYPEID"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDFUELTYPEID"].ToString();
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadComboBox ddlOilTypefooter = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                if (ddlOilTypefooter != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilTypefooter.DataSource = ds;
                    ddlOilTypefooter.DataBind();
                    ddlOilTypefooter.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 0);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    if (General.GetNullableGuid(drv["FLDFUELTYPEID"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDFUELTYPEID"].ToString();
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadComboBox ddlOilTypefooter = (RadComboBox)e.Item.FindControl("ddlFeulTypeAdd");
                if (ddlOilTypefooter != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 0);
                    ddlOilTypefooter.DataSource = ds;
                    ddlOilTypefooter.DataBind();
                    ddlOilTypefooter.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void gvSIPTanksConfuguration_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulType");
            RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameEdit");

                PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankInsert(
                        General.GetNullableGuid(lblSIPTankId.Text)
                        , General.GetNullableDecimal(Capacity.Text)
                        , General.GetNullableGuid(FuelType.SelectedValue)
                        , General.GetNullableString(txttankname.Text)
                        );

            RebindFuelTanks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            RadLabel lblTankId = (RadLabel)e.Item.FindControl("lblTankIdEdit");
            UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulType");
            RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameEdit");

            PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankInsert(
                   General.GetNullableGuid(lblSIPTankId.Text)
                   , General.GetNullableDecimal(Capacity.Text)
                   , General.GetNullableGuid(FuelType.SelectedValue)
                   , General.GetNullableString(txttankname.Text)
                   );
            RebindServiceSettlingTanks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            RadLabel lblTankId = (RadLabel)e.Item.FindControl("lblTankIdEdit");
            UserControlMaskNumber Capacity = (UserControlMaskNumber)e.Item.FindControl("txtCapacityEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlFeulType");
            RadTextBox txttankname = (RadTextBox)e.Item.FindControl("txtTanknameEdit");

            PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankInsert(
                    General.GetNullableGuid(lblSIPTankId.Text)
                    , General.GetNullableDecimal(Capacity.Text)
                    , General.GetNullableGuid(FuelType.SelectedValue)
                    , General.GetNullableString(txttankname.Text)
                    );

            RebindLubeOilTanks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", UcVessel.SelectedVessel);
        Filter.CurrentSIPVesselFilter = criteria;

        RebindFuelTanks();
        RebindServiceSettlingTanks();
        RebindLubeOilTanks();
    }

    protected void gvSIPTanksConfuguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPFuelTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));
        gvSIPTanksConfuguration.DataSource = ds;

    }
    protected void RebindFuelTanks()
    {
        gvSIPTanksConfuguration.SelectedIndexes.Clear();
        gvSIPTanksConfuguration.EditIndexes.Clear();
        gvSIPTanksConfuguration.DataSource = null;
        gvSIPTanksConfuguration.Rebind();
    }
    protected void gvSettlingServeice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPSettlingServiceTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));
        gvSettlingServeice.DataSource = ds;

    }
    protected void RebindServiceSettlingTanks()
    {
        gvSettlingServeice.SelectedIndexes.Clear();
        gvSettlingServeice.EditIndexes.Clear();
        gvSettlingServeice.DataSource = null;
        gvSettlingServeice.Rebind();
    }
    protected void gvLubeOilTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPLubeOilTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));
        gvLubeOilTank.DataSource = ds;

    }
    protected void RebindLubeOilTanks()
    {
        gvLubeOilTank.SelectedIndexes.Clear();
        gvLubeOilTank.EditIndexes.Clear();
        gvLubeOilTank.DataSource = null;
        gvLubeOilTank.Rebind();
    }
}
