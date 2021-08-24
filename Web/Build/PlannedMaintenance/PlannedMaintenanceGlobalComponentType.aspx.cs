using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Data;

public partial class PlannedMaintenanceGlobalComponentType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtNumber.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Populate", "POPULATE", ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuComponent.AccessRights = this.ViewState;
        MenuComponent.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbargeneral = new PhoenixToolbar();
        MenuGeneral.Title = "Component Details";
        toolbargeneral.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbargeneral.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbargeneral.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuGeneral.AccessRights = this.ViewState;
        MenuGeneral.MenuList = toolbargeneral.Show();

        PhoenixToolbar toolbarJob = new PhoenixToolbar();
        MenuJob.Title = "Jobs";
        toolbarJob.AddFontAwesomeButton("", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
        //toolbarJob.AddFontAwesomeButton("", "Add Jobs", "<i class=\"fas fa-plus\"></i>", "ADD");
        MenuJob.AccessRights = this.ViewState;
        MenuJob.MenuList = toolbarJob.Show();

        if (!IsPostBack)
        {
            ViewState["COMPONENTTYPEID"] = "";
            BindComponentList();
            BindMake();
            BindModel();
            BindComponent();
            gvPlannedMaintenanceJob.PageSize = General.ShowRecords(null);
        }
    }

    protected void MenuJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvPlannedMaintenanceJob.Rebind();
                gvPlannedMaintenanceJob.SelectedIndexes.Clear();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;



            if (CommandName.ToUpper().Equals("POPULATE"))
            {
                ucError.HeaderMessage = "Please provide the following required information";
                if (General.GetNullableInteger(ddlModel.SelectedValue) == null)
                    ucError.ErrorMessage = "Model is required";
                if (General.GetNullableString(txtNumber.Text) == null)
                    ucError.ErrorMessage = "Component Number is required";

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceGlobalComponent.PopulateComponentType(int.Parse(ddlModel.SelectedValue), General.GetNullableString(txtNumber.Text));
                txtNumber.Text = "";
                BindComponent();
                gvPlannedMaintenanceJob.CurrentPageIndex = 0;
                gvPlannedMaintenanceJob.Rebind();
            }
            
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ucError.HeaderMessage = "Please provide the following required information";
                if (General.GetNullableInteger(ddlModel.SelectedValue) == null)
                    ucError.ErrorMessage = "Model is required";

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }

                BindComponent();
                gvPlannedMaintenanceJob.CurrentPageIndex = 0;
                gvPlannedMaintenanceJob.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["COMPONENTTYPEID"].ToString() != null && ViewState["COMPONENTTYPEID"].ToString() != "")
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    if (General.GetNullableGuid(hdnComponentTypeID.Value) != null && General.GetNullableInteger(ddlModel.SelectedValue) != null)

                        PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeUpdate(new Guid(hdnComponentTypeID.Value), General.GetNullableString(txtParent.Text), General.GetNullableInteger(txtUnits.Text), int.Parse(ddlModel.SelectedValue));

                    BindComponent();
                }
                else if (CommandName.ToUpper().Equals("DELETE"))
                {
                    if (General.GetNullableGuid(hdnComponentTypeID.Value) == null)
                    {
                        ucError.ErrorMessage = "Select the Component to Delete.";
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeDelte(new Guid(hdnComponentTypeID.Value));
                    txtParent.Text = "";
                    txtName.Text = "";
                    txtComponentNumber.Text = "";
                    hdnComponentID.Value = "";
                    hdnComponentTypeID.Value = "";
                    txtUnits.Text = string.Empty;
                    BindComponent();
                    gvPlannedMaintenanceJob.Rebind();
                }

            }
            else
            {
                ucError.ErrorMessage = "Select a component.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindModel()
    {
        int i1 = 0;
        int i2 = 0;
        ddlModel.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(null, General.GetNullableString(ddlMake.SelectedValue) != null ? General.GetNullableString(ddlMake.SelectedItem.Text) : null, General.GetNullableInteger(ddlStroke.SelectedValue), null, null, 1, 10000, ref i1, ref i2, General.GetNullableString(ddlCmpName.SelectedValue) != null ? General.GetNullableString(ddlCmpName.SelectedItem.Text) : null, General.GetNullableString(txtNumber.Text));
        
        ddlModel.DataBind();
    }
    protected void BindComponentList()
    {
        ddlCmpName.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelComponentList(null,null);
        ddlCmpName.DataBind();
        ddlCmpName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindMake()
    {
        ddlMake.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelMakeList(General.GetNullableString(ddlCmpName.SelectedValue) != null ? General.GetNullableString(ddlCmpName.SelectedItem.Text):null, General.GetNullableInteger(ddlStroke.SelectedValue));
        ddlMake.DataBind();
        ddlMake.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void BindComponent()
    {
        try
        {
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeHierarchy( General.GetNullableInteger(ddlModel.SelectedValue)!=null? int.Parse(ddlModel.SelectedValue):0,General.GetNullableString(txtNumber.Text));
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDGLOBALCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTCOMPONENTID";
            tvwComponent.RootText = ddlModel.SelectedItem.Text;
            tvwComponent.PopulateTree(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void tvwComponent_NodeDataBoundEvent(object sender, EventArgs args)
    {
        try
        {

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            DataRowView drv = (DataRowView)e.Node.DataItem;
            e.Node.Attributes["NUMBER"] = drv["FLDCOMPONENTNUMBER"].ToString();
            e.Node.Attributes["NAME"] = drv["FLDNAME"].ToString();
            e.Node.Attributes["UNITS"] = drv["FLDNOOFUNITS"].ToString();
            e.Node.Attributes["ID"] = drv["FLDGLOBALCOMPONENTTYPEID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            ViewState["ISTREENODECLICK"] = true;

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                string selectednode = e.Node.Value.ToString();
                string selectedvalue = e.Node.Text.ToString();

                ViewState["COMPONENTTYPEID"] = e.Node.Attributes["ID"].ToString();
                hdnComponentID.Value = e.Node.Value.ToString();
                hdnComponentTypeID.Value = e.Node.Attributes["ID"].ToString();
                txtComponentNumber.Text = e.Node.Attributes["NUMBER"].ToString();
                txtName.Text = e.Node.Attributes["NAME"].ToString();
                txtUnits.Text = e.Node.Attributes["UNITS"].ToString();

                RadTreeNode parent = e.Node.ParentNode;
                txtParent.Text = parent.Value.ToLower() != "_root" ? parent.Attributes["NUMBER"].ToString() : "";
                gvPlannedMaintenanceJob.Rebind();
                gvPlannedMaintenanceJob.SelectedIndexes.Clear();

                // Disabling the root node click
                string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedMaintenanceJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDCLASS", "FLDFREQUENCYNAME" };
            string[] alCaptions = { "Code", "Title", "Job Class", "Frequency" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            DataSet ds;

            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobSearch(General.GetNullableGuid(ViewState["COMPONENTTYPEID"].ToString()),
                                                           General.GetNullableInteger(ddlModel.SelectedValue) != null ? int.Parse(ddlModel.SelectedValue) : 0,
                                                           General.GetNullableString(txtCode.Text),
                                                           General.GetNullableString(txtTitle.Text),
                                                           sortexpression, sortdirection,
                                                           gvPlannedMaintenanceJob.CurrentPageIndex + 1,
                                                           gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(ddlShow.SelectedValue));

            gvPlannedMaintenanceJob.DataSource = ds;
            gvPlannedMaintenanceJob.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPlannedMaintenanceJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJobEdit.aspx?GLOBALCOMPONENTTYPEMAPID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEJOBMAPID").ToString()+ "'); return false;");
                }
                RadLabel lblActive = (RadLabel)e.Item.FindControl("lblActive");
                if(lblActive!=null)
                {
                    lblActive.Text = drv["FLDAPPLICABLEYN"].ToString() == "1" ? "Yes" : "No";
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedMaintenanceJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentJobDelete(new Guid(item.GetDataKeyValue("FLDGLOBALCOMPONENTJOBID").ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPlannedMaintenanceJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void ddlModel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindComponent();
    //}

    //protected void ddlCmpName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindMake();
    //    BindModel();
    //    txtNumber.Text = ddlCmpName.SelectedValue;
    //}

    //protected void ddlStroke_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindMake();
    //    BindModel();
    //}

    //protected void ddlMake_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindModel();
    //}


    protected void ddlCmpName_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtNumber.Text = ddlCmpName.SelectedValue;
        BindMake();
        BindModel();
        BindComponent();
        gvPlannedMaintenanceJob.CurrentPageIndex = 0;
        gvPlannedMaintenanceJob.Rebind();

    }

    protected void ddlStroke_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindMake();
        BindModel();
        BindComponent();
        gvPlannedMaintenanceJob.CurrentPageIndex = 0;
        gvPlannedMaintenanceJob.Rebind();
    }

    protected void ddlMake_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindModel();
        BindComponent();
        gvPlannedMaintenanceJob.CurrentPageIndex = 0;
        gvPlannedMaintenanceJob.Rebind();
    }

    protected void ddlModel_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindComponent();
        gvPlannedMaintenanceJob.CurrentPageIndex = 0;
        gvPlannedMaintenanceJob.Rebind();
    }
}