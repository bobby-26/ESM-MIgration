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

public partial class PlannedMaintenanceGlobalModelRegister : PhoenixBasePage
{
    protected override void OnPreRenderComplete(EventArgs e)
    {
        
        gvModel.HeaderContextMenu.CssClass += " myCustomWidth";
        base.OnPreRenderComplete(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuAddNew.AccessRights = this.ViewState;
            MenuAddNew.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                BindContextMenu();
                gvModel.PageSize = General.ShowRecords(null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void PlannedMaintenance_TabStripCommand(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("SEARCH"))
    //        {
    //            gvModel.Rebind();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvModel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDMAKE", "FLDTYPE", "FLDTWOORFOURSTROKENAME"};
            string[] alCaptions = { "Make", "Type", "Two / Four Stroke"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds;

            //ds = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(General.GetNullableString(txtType.Text),
            //                                               General.GetNullableString(txtMake.Text),
            //                                               General.GetNullableInteger(ddlStroke.SelectedValue),
            //                                               sortexpression, sortdirection,
            //                                               gvModel.CurrentPageIndex + 1,
            //                                               gvModel.PageSize, ref iRowCount, ref iTotalPageCount
            //                                               , General.GetNullableString(ddlCmpName.SelectedValue) != null ? General.GetNullableString(ddlCmpName.SelectedItem.Text) : null
            //                                               , null);

            

            ds = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(null,
                                                       null,
                                                       null,
                                                       null, null,
                                                       1,
                                                       10000, ref iRowCount, ref iTotalPageCount
                                                       , null
                                                       , null);

            gvModel.DataSource = ds;
            //gvModel.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvModel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if(e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;
            }
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                //DataRowView drv = (DataRowView)item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton cmdCopy = (LinkButton)e.Item.FindControl("cmdCopy");
                if (db != null)
                {
                        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                if (cmdCopy != null)
                    cmdCopy.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                LinkButton cmdJobs = (LinkButton)e.Item.FindControl("cmdJobs");
                RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
                RadLabel lblID = (RadLabel)e.Item.FindControl("lblID");
                if (cmdJobs != null && lblType!=null)
                {
                    cmdJobs.Visible = SessionUtil.CanAccess(this.ViewState, cmdJobs.CommandName);
                    cmdJobs.Attributes.Add("onclick", "javascript:return openNewWindow('Jobs', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJob.aspx?MODELID=" + lblID.Text + "&MODEL=" + lblType.Text+"&LAUNCHFROM=MODEL','true'); return false;");
                }

                LinkButton cmdSpareParts = (LinkButton)e.Item.FindControl("cmdSpareParts");
                if (cmdSpareParts != null && lblType != null)
                {
                    cmdSpareParts.Visible = SessionUtil.CanAccess(this.ViewState, cmdSpareParts.CommandName);
                    cmdSpareParts.Attributes.Add("onclick", "javascript:return openNewWindow('Spares', '', 'PlannedMaintenance/PlannedMaintenanceGlobalSparesList.aspx?MODELID=" + lblID.Text + "&MODEL="+ lblType.Text + "','true'); return false;");
                }

                RadDropDownList ddlStrokeEdit = (RadDropDownList)item["STROKE"].FindControl("ddlStrokeEdit");
                RadComboBox ddlCmpNameEdit = (RadComboBox)item["COMPONENTNAME"].FindControl("ddlCmpNameEdit");
                RadLabel lblNumber = (RadLabel)item["COMPONENTNUMBER"].FindControl("lblNumber");
                if (ddlStrokeEdit != null)
                {
                    RadLabel lblStrokeEdit = (RadLabel)item["STROKE"].FindControl("lblStrokeEdit");
                    if (lblStrokeEdit != null)
                    {
                        ddlStrokeEdit.SelectedValue = lblStrokeEdit.Text;
                    }
                }
                if (ddlCmpNameEdit != null)
                {
                    ddlCmpNameEdit.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelComponentList(null, null);
                    ddlCmpNameEdit.DataBind();
                    ddlCmpNameEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

                    if (lblNumber != null)
                        ddlCmpNameEdit.SelectedValue = lblNumber.Text;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvModel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.RegisterModelDelete(int.Parse(item.GetDataKeyValue("FLDMODELID").ToString()));
                gvModel.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("POPULATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lblComponentNumber = (RadLabel)item.FindControl("lblNumber");
                PhoenixPlannedMaintenanceGlobalComponent.PopulateComponentType(int.Parse(item.GetDataKeyValue("FLDMODELID").ToString()), General.GetNullableString(lblComponentNumber.Text));
                ucStatus.Text = "Components and Jobs populated to Type from Global Register.";
            }
            else if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                gvModel.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
                gvModel.ExportSettings.IgnorePaging = true;
                gvModel.ExportSettings.ExportOnlyData = true;
                gvModel.ExportSettings.OpenInNewWindow = true;
            }
            else if(e.CommandName.ToUpper() == "PRINTGRID")
            {
                
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.CopyModel(int.Parse(item.GetDataKeyValue("FLDMODELID").ToString()));
                gvModel.Rebind();
            }
            //else if (e.CommandName.ToUpper().Equals("PARTS"))
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookList(General.GetNullableInteger(item.GetDataKeyValue("FLDMODELID").ToString()));
            //    //gvcMenu.Items.Clear();
                
            //    //gvcMenu.Items.Add(new RadMenuItem("Add New", "ADD"));
            //    //foreach (DataRow dr in ds.Tables[0].Rows)
            //    //{
            //    //    gvcMenu.Items.Add(new RadMenuItem(dr["FLDTITLE"].ToString(), dr["FLDBOOKID"].ToString()));
            //    //}
            //    string Script = "";
            //    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            //    Script += "RowContextMenu(event,"+item.ItemIndex+")";
            //    Script += "</script>" + "\n";

            //    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);

            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvModel_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            
            GridEditableItem item = (GridEditableItem)e.Item;
            if (!IsValidComponent(((RadTextBox)item["TYPE"].FindControl("txtTypeEdit")).Text, ((RadTextBox)item["MAKE"].FindControl("txtMakeEdit")).Text))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
                PhoenixPlannedMaintenanceGlobalComponent.RegisterModelInsert(((RadTextBox)item["MAKE"].FindControl("txtMakeEdit")).Text
                                                    , ((RadTextBox)item["TYPE"].FindControl("txtTypeEdit")).Text
                                                    , General.GetNullableInteger(((RadDropDownList)item["STROKE"].FindControl("ddlStrokeEdit")).SelectedValue)
                                                    , General.GetNullableInteger(((RadLabel)item["MAKE"].FindControl("lblModelID")).Text)
                                                    , General.GetNullableString(((RadComboBox)item["COMPONENTNAME"].FindControl("ddlCmpNameEdit")).SelectedItem.Text)
                                                    , General.GetNullableString(((RadComboBox)item["COMPONENTNAME"].FindControl("ddlCmpNameEdit")).SelectedValue)
                                                    );
        }
    }
    private bool IsValidComponent(string type,string make)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(make) == null)
            ucError.ErrorMessage = "Make is required.";

        if (General.GetNullableString(type) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }
    protected void gvModel_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();

        int iRowCount = 10;
        int iTotalPageCount = 10;
        DataSet ds;

        ds = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(null,
                                                       null,
                                                       null,
                                                       null, null,
                                                       1,
                                                       10000, ref iRowCount, ref iTotalPageCount
                                                       , null
                                                       , null);
        DataView view = new DataView(ds.Tables[0]);
        DataTable dt = view.ToTable(true, DataField);
        e.ListBox.DataSource = dt;
        e.ListBox.DataKeyField = DataField;
        e.ListBox.DataTextField = DataField;
        e.ListBox.DataValueField = DataField;
        e.ListBox.DataBind();
    }

    protected void gvcMenu_ItemClick(object sender, RadMenuEventArgs e)
    {

    }

    protected void MenuAddNew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindContextMenu()
    {
        DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookList(null);
        foreach(DataRow dr in ds.Tables[0].Rows)
        {
            RadMenuItem item = new RadMenuItem();
            item.Text = dr["FLDTITLE"].ToString();
            item.Value = dr["FLDBOOKID"].ToString();
            gvcMenu.Items.Add(item);
        }
    }
}