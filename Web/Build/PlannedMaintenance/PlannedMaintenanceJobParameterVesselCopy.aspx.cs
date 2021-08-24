using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.WebControls;

public partial class PlannedMaintenanceJobParameterVesselCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Submit", "SAVE", ToolBarDirection.Right);

            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                BindVessel();
                fromVessel.SelectedValue = Request.QueryString["VESSELID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void BindVessel()
    {
        DataSet ds;
        ds = PhoenixRegistersVessel.VesselListCommon(1, 1, 0, 1, PhoenixVesselEntityType.VSL, null);
        lstVessel.DataSource = ds;
        lstVessel.DataTextField = "FLDVESSELNAME";
        lstVessel.DataValueField = "FLDVESSELID";
        lstVessel.DataBind();

        fromVessel.DataSource = ds;
        fromVessel.DataTextField = "FLDVESSELNAME";
        fromVessel.DataValueField = "FLDVESSELID";
        fromVessel.DataBind();

    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Save();
                StatusNotify.Show("Copied Successfully.");
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Save()
    {
        if (string.IsNullOrEmpty(fromVessel.SelectedValue))
        {
            return;
        }
        string vessellist = ReadComboBoxList(lstVessel);

        PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselCopy(int.Parse(fromVessel.SelectedValue), vessellist,null,null);
        
    }
    public string ReadComboBoxList(RadComboBox listbox)
    {
        string list = string.Empty;
        foreach (RadComboBoxItem item in listbox.CheckedItems)
        {
            if (item.Checked == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

}
