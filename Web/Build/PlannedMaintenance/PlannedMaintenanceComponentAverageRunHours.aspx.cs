using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class PlannedMaintenanceComponentAverageRunHours : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Counter", "COUNTER", ToolBarDirection.Left);
            toolbarmain.AddButton("Avg Run Hrs", "AVGRUNHRS", ToolBarDirection.Left);
            MenuMain.MenuList = toolbarmain.Show();
            MenuMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvClassMap.PageSize = General.ShowRecords(null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvClassMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            gvClassMap.MasterTableView.ColumnGroups.Clear();
            gvClassMap.MasterTableView.Columns.Clear();
            DataSet ds;

            ds = PhoenixPlannedMaintenanceCounter.CounterSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , General.GetNullableString(txtCNumber.Text)
                                                            , General.GetNullableString(txtCName.Text)
                                                            , null
                                                            , null, null
                                                            , gvClassMap.CurrentPageIndex + 1
                                                            , gvClassMap.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);


            GridBoundColumn number = new GridBoundColumn();
            gvClassMap.MasterTableView.Columns.Add(number);
            number.HeaderText = "Component Number";
            number.UniqueName = "COMPONENTNUMBER";
            number.ReadOnly = true;
            number.DataField = "FLDCOMPONENTNUMBER";
            number.DataType = typeof(System.String);

            GridBoundColumn name = new GridBoundColumn();
            gvClassMap.MasterTableView.Columns.Add(name);
            name.HeaderText = "Component Name";
            name.UniqueName = "COMPONENTNAME";
            name.ReadOnly = true;
            name.DataField = "FLDCOMPONENTNAME";
            name.DataType = typeof(System.String);

            GridBoundColumn lastreaddate = new GridBoundColumn();
            gvClassMap.MasterTableView.Columns.Add(lastreaddate);
            lastreaddate.HeaderText = "Last Read";
            lastreaddate.UniqueName = "LASTREAD";
            lastreaddate.ReadOnly = true;
            lastreaddate.DataField = "FLDREADINGDATE";
            lastreaddate.DataType = typeof(System.DateTime);
            lastreaddate.DataFormatString = "{0:dd/MM/yyyy}";

            GridBoundColumn lastreadvalue = new GridBoundColumn();
            gvClassMap.MasterTableView.Columns.Add(lastreadvalue);
            lastreadvalue.HeaderText = "Last Read Value";
            lastreadvalue.UniqueName = "LASTREADVALUE";
            lastreadvalue.ReadOnly = true;
            lastreadvalue.DataField = "FLDCURRENTVALUE";
            lastreadvalue.DataType = typeof(System.Decimal);

            GridBoundColumn avghrs = new GridBoundColumn();
            gvClassMap.MasterTableView.Columns.Add(avghrs);
            avghrs.HeaderText = "Average Run Hrs";
            avghrs.UniqueName = "AVERAGERUNHOURS";
            avghrs.DataField = "FLDAVERAGERUNHOURS";
            avghrs.DataType = typeof(System.Decimal);

            number.HeaderStyle.Width = Unit.Parse("100px");
            number.ItemStyle.Width = Unit.Parse("100px");
            name.HeaderStyle.Width = Unit.Parse("240px");
            name.ItemStyle.Width = Unit.Parse("240px");
            gvClassMap.DataSource = ds;
            gvClassMap.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvClassMap_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.Item is GridDataItem)
            {
                //DataSet ds = (DataSet)grid.DataSource;
                //GridDataItem item = (GridDataItem)e.Item;
                //DataRowView drv = (DataRowView)item.DataItem;

                //foreach(GridColumn c in grid.Columns)
                //{
                //    if (General.GetNullableInteger(c.UniqueName) != null)
                //    {
                //        DataRow[] dr = ds.Tables[2].Select("FLDGLOBALCOMPONENTID = '" + drv["FLDGLOBALCOMPONENTID"].ToString() + "' AND FLDCLASS = '" + c.UniqueName + "'");
                //        if (dr.Length > 0)
                //            item[c.UniqueName].Text = dr[0]["FLDCODE"].ToString();
                //        else
                //            item[c.UniqueName].Text = "";
                //    }
                //}

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvClassMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvClassMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
        }
    }

    protected void gvClassMap_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        foreach (GridBatchEditingCommand command in e.Commands)
        {
            Hashtable newValues = command.NewValues;
            Hashtable oldValues = command.OldValues;

            PhoenixPlannedMaintenanceCounter.UpdateAverageRunHrs(new Guid(newValues["FLDCOMPONENTID"].ToString())
                            , General.GetNullableDecimal(newValues["FLDAVERAGERUNHOURS"].ToString())
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            //if (oldValues["FLDAVERAGERUNHOURS"] != newValues["FLDAVERAGERUNHOURS"])
            //{
                
            //}
            //foreach(DictionaryEntry t  in newValues)
            //{
            //  if (!t.Key.ToString().Contains("FLD") && t.Key.ToString()!="")
            //  {
            //    PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentClassCodeMap(
            //                                    new Guid(newValues["FLDGLOBALCOMPONENTID"].ToString())
            //                                    , int.Parse(t.Key.ToString())
            //                                    , t.Value.ToString()
            //                                    );
            //  }
            //}
        }
        gvClassMap.Rebind();
    }

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "COUNTER")
            {
                Response.Redirect(Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}