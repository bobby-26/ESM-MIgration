using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class RegisterSIPToolTipConfiguration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
           gvSIPTanksConfuguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void Rebind()
    {
        gvSIPTanksConfuguration.SelectedIndexes.Clear();
        gvSIPTanksConfuguration.EditIndexes.Clear();
        gvSIPTanksConfuguration.DataSource = null;
        gvSIPTanksConfuguration.Rebind();
    }
    protected void gvSIPTanksConfuguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixRegistersSIPToolTipConfiguration.SIPToolTipConfigPageList(null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSIPTanksConfuguration.DataSource = ds;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    RadLabel lblSIPTankId = (RadLabel)editableItem.FindControl("lblispTankIDEdit");
                    RadTextBox txttooltip = (RadTextBox)editableItem.FindControl("txtCapaciltyEdit");

                    PhoenixRegistersSIPToolTipConfiguration.UpdateSIPToolTipConfig(
                        General.GetNullableInteger(lblSIPTankId.Text)
                        , General.GetNullableString(txttooltip.Text)
                        );

                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSIPTanksConfuguration_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


    protected void gvSIPTanksConfuguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton lb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (lb != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                        lb.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
