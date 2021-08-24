using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceAdditionsToComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string componentname = string.Empty;
        string number = string.Empty;
        if (Request.QueryString["Componentname"] != null)
            componentname = Request.QueryString["Componentname"].ToString();

        if (Request.QueryString["Componentnumber"] != null)
            number = Request.QueryString["Componentnumber"].ToString();

        ucMenu.Title = number + " - " + componentname;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        ucMenu.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["ComponentID"] != null && Request.QueryString["ComponentID"].ToString() != "")
                ViewState["componentid"] = Request.QueryString["ComponentID"].ToString();
            else
                ViewState["componentid"] = "";

            if (Request.QueryString["VesselID"] != null && Request.QueryString["VesselID"].ToString() != "")
                ViewState["vesselid"] = Request.QueryString["VesselID"].ToString();
            else
                ViewState["vesselid"] = "";
        }
    }

    protected void gvAdditions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //gvAdditions.MasterTableView.Columns.Clear();
        //GridDropDownColumn Item = new GridDropDownColumn();
        //gvAdditions.MasterTableView.Columns.Add(Item);
        //Item.HeaderText = "Item";
        //Item.UniqueName = "FLDITEM";
        //Item.DataType = typeof(System.Int32);
        //Item.DataField = "FLDITEM";
        //Item.ListValueField = "FLDQUICKCODE";
        //Item.ListTextField = "FLDQUICKNAME";
        //Item.DataSourceID = "ItemList";
        //Item.HeaderStyle.Width = Unit.Parse("30%");

        //GridBoundColumn value = new GridBoundColumn();
        //gvAdditions.MasterTableView.Columns.Add(value);
        //value.HeaderText = "Value";
        //value.UniqueName = "FLDVALUE";
        //value.DataField = "FLDVALUE";
        //value.DataType = typeof(System.String);
        //value.HeaderStyle.Width = Unit.Parse("60%");

        //GridButtonColumn delete = new GridButtonColumn();
        //gvAdditions.MasterTableView.Columns.Add(delete);
        //delete.HeaderText = "Delete";
        //delete.UniqueName = "DELETE";
        //delete.ConfirmDialogType = GridConfirmDialogType.RadWindow;
        //delete.ConfirmText = "Are you sure you want to delete this record";
        //delete.CommandName = "DELETE";
        //delete.Text = "Delete";
        //delete.HeaderStyle.Width = Unit.Parse("10%");


        DataTable dt = new DataTable();
        dt = PhoenixPlannedMaintenanceAdditionsToComponent.AdditionsToComponentList(new Guid(ViewState["componentid"].ToString()), int.Parse(ViewState["vesselid"].ToString()));
        gvAdditions.DataSource = dt;
    }

    protected void gvAdditions_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
    }

    protected void gvAdditions_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;

                if (command.Type == GridBatchEditingCommandType.Delete && newValues!=null && newValues["FLDID"]!=null)
                {
                    PhoenixPlannedMaintenanceAdditionsToComponent.AdditionsToComponentDelete(new Guid(newValues["FLDID"].ToString()));
                }
            }
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;
                if (command.Type != GridBatchEditingCommandType.Delete && newValues != null)
                {
                    if (oldValues != null && newValues != null && (oldValues["FLDVALUE"] != newValues["FLDVALUE"] || oldValues["FLDITEM"] != newValues["FLDITEM"]))
                    {
                        PhoenixPlannedMaintenanceAdditionsToComponent.AdditionsToComponentInsert(
                            new Guid(ViewState["componentid"].ToString())
                            , int.Parse(ViewState["vesselid"].ToString())
                            , General.GetNullableGuid(newValues["FLDID"] != null ? newValues["FLDID"].ToString() : "")
                            , int.Parse(newValues["FLDITEM"] != null ? newValues["FLDITEM"].ToString() : "")
                            , General.GetNullableString(newValues["FLDVALUE"] != null ? newValues["FLDVALUE"].ToString() : "")
                            );
                    }
                }
            }

        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
        
    }

    protected void gvAdditions_ItemDeleted(object sender, GridDeletedEventArgs e)
    {
    }

    protected void gvBatteries_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = PhoenixPlannedMaintenanceAdditionsToComponent.BatteriesToComponentList(new Guid(ViewState["componentid"].ToString()), int.Parse(ViewState["vesselid"].ToString()));
        gvBatteries.DataSource = dt;
    }

    protected void gvBatteries_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvBatteries_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;

                if (command.Type == GridBatchEditingCommandType.Delete && newValues != null && newValues["FLDID"] != null)
                {
                    PhoenixPlannedMaintenanceAdditionsToComponent.BatteriesToComponentDelete(new Guid(newValues["FLDID"].ToString()));
                }
            }
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;
                if (command.Type != GridBatchEditingCommandType.Delete && newValues != null)
                {
                    if (oldValues != null && newValues != null && (newValues["FLDTYPE"]!=null && General.GetNullableInteger(newValues["FLDTYPE"].ToString())!=null))
                    {
                        PhoenixPlannedMaintenanceAdditionsToComponent.BatteriesToComponentInsert(
                            new Guid(ViewState["componentid"].ToString())
                            , int.Parse(ViewState["vesselid"].ToString())
                            , General.GetNullableGuid(newValues["FLDID"] != null ? newValues["FLDID"].ToString() : "")
                            ,General.GetNullableInteger(newValues["FLDTYPE"].ToString())
                            ,General.GetNullableString(newValues["FLDSPECIFICATION"] !=null? newValues["FLDSPECIFICATION"].ToString() :"")
                            , General.GetNullableString(newValues["FLDVOLTAGE"] != null ? newValues["FLDVOLTAGE"].ToString() : "")
                            , General.GetNullableDateTime(newValues["FLDLASTREPLACED"] != null ? newValues["FLDLASTREPLACED"].ToString() : "")
                            , General.GetNullableDateTime(newValues["FLDSUGGESTEDREPLACEDDATE"] != null ? newValues["FLDSUGGESTEDREPLACEDDATE"].ToString() : "")
                            );
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvBatteries_ItemDeleted(object sender, GridDeletedEventArgs e)
    {

    }
}