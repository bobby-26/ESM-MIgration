using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;
public partial class PlannedMaintenanceComponent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Parts", "PART");
            toolbarmain.AddButton("History Template", "HISTORYTEMPLATE");
            toolbarmain.AddButton("Details", "DETAILS");
            toolbarmain.AddButton("Jobs", "JOB");
            toolbarmain.AddButton("Attachment", "ATTACHMENT");
            toolbarmain.AddButton("Counter", "COUNTER");
            toolbarmain.AddButton("Work Order", "WORKORDER");
            toolbarmain.AddButton("Work Done History", "WORKDONEHISTORY");
            toolbarmain.AddButton("RA History", "RAHISTORY");

            // MenuComponent.AccessRights = this.ViewState;
            // MenuComponent.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbarsearch = new PhoenixToolbar();
            //toolbarsearch.AddFontAwesomeButton("../Inventory/InventoryComponent.aspx", "List View", "<i class=\"fas fa-file\"></i>", "LISTVIEW");
            //toolbarsearch.AddFontAwesomeButton("../Inventory/InventorySpareItem.aspx", "Deleted Components", "<i class=\"fas fa-file-excel\"></i>", "DELETEDCOMPONENTS");

            //toolbarsearch.AddImageButton("../Inventory/InventoryComponent.aspx", "List View", "annexure.png", "LISTVIEW");
            toolbarsearch.AddImageButton("", "Deleted Components", "red.png", "DELETEDCOMPONENTS");
            Menusearch.AccessRights = this.ViewState;
            Menusearch.MenuList = toolbarsearch.Show();

            if (!IsPostBack)
            {
                // Filter.CurrentPlannedMaintenanceViewFilter = "1";
                ViewState["COMPONENTID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["ISTREENODECLICK"] = false;
                if (Request.QueryString["COMPONENTID"] != null && Request.QueryString["COMPONENTID"] != "")
                {
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                    string compid = ViewState["COMPONENTID"].ToString();
                    ViewState["DTKEY"] = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentDashboard.aspx?COMPONENTID=" + Request.QueryString["COMPONENTID"].ToString();
                }
                if (Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                }
                if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
                }

                BindTreeData();
                //MenuComponent.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTreeData()
    {
        try
        {
            DataSet ds = PhoenixInventoryComponent.TreeComponentList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTID";
            tvwComponent.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            tvwComponent.PopulateTree(ds.Tables[0]);
            //TreeView tvw = tvwComponent.T;
            //((TreeNode)tvw.Nodes[0]).Expand();
            if (ViewState["COMPONENTID"] == null && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["COMPONENTID"] = ds.Tables[0].Rows[0][0].ToString();
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString();
            }
            //cmdDeleted.Visible = true;
            //cmdComponent.Visible = false;
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
            BindTreeData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        //if (txtComponentName.Text != null && txtComponentName.Text != "")
        //{
        //    tvwComponent.SearchNodeByText(txtComponentName.Text, tvwComponent.Nodes);
        //}
    }
    protected void cmdListView_Click(object sender, EventArgs e)
    {
        if (ViewState["COMPONENTID"] == null)
        {
            Response.Redirect("../Inventory/InventoryComponent.aspx");
        }
        else
        {
            Response.Redirect("../Inventory/InventoryComponent.aspx?componentid=" + ViewState["COMPONENTID"].ToString());
        }
    }

    protected void Menusearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            PhoenixToolbar toolbarsearch = new PhoenixToolbar();
            toolbarsearch.AddImageButton("../Inventory/InventoryComponent.aspx", "List View", "annexure.png", "LISTVIEW");
            //toolbarsearch.AddFontAwesomeButton("../Inventory/InventoryComponent.aspx", "List View", "<i class=\"fas fa-file\"></i>", "LISTVIEW");
            //toolbarsearch.AddFontAwesomeButton("../Inventory/InventorySpareItem.aspx", "Deleted Components", "<i class=\"fas fa-file-excel\"></i>", "DELETEDCOMPONENTS");
            if (CommandName.ToUpper().Equals("DELETEDCOMPONENTS"))
            {
                toolbarsearch.AddImageButton("", "Active Components", "green.png", "ACTIVECOMPONENTS");
                Deleted_Click();
            }
            else if (CommandName.ToUpper().Equals("ACTIVECOMPONENTS"))
            {
                toolbarsearch.AddImageButton("", "Deleted Components", "red.png", "DELETEDCOMPONENTS");
                Component_Click();
            }
            Menusearch.AccessRights = this.ViewState;
            Menusearch.MenuList = toolbarsearch.Show();
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

            PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

            if (ViewState["COMPONENTID"].ToString() != null && ViewState["COMPONENTID"].ToString() != "")
            {
                if (CommandName.ToUpper().Equals("GENERAL"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentDashboard.aspx";
                }
                else if (CommandName.ToUpper().Equals("HISTORYTEMPLATE"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentHistoryTemplate.aspx";
                }
                else if (CommandName.ToUpper().Equals("DETAILS"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentDetail.aspx";
                }
                else if (CommandName.ToUpper().Equals("ATTACHMENT"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                else if (CommandName.ToUpper().Equals("PART"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentSpareItem.aspx";
                }
                else if (CommandName.ToUpper().Equals("JOB"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobList.aspx";
                }
                else if (CommandName.ToUpper().Equals("COUNTER"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentCounters.aspx";

                }
                else if (CommandName.ToUpper().Equals("WORKORDER"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentWorkOrderList.aspx";
                }
                else if (CommandName.ToUpper().Equals("WORKDONEHISTORY"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
                }
                else if (CommandName.ToUpper().Equals("RAHISTORY"))
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderRAHistory.aspx";
                }
                if (CommandName.ToUpper().Equals("ATTACHMENT"))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"] + "&p=" + ViewState["PAGENUMBER"];
                }
                // SetTabHighlight();
                //TreeView tvw = tvwComponent.ThisTreeView;
                //((TreeNode)tvw.Nodes[0]).Expand();
                //// Disabling the root node click
                //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\nresizew();";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
                //if ((bool)ViewState["ISTREENODECLICK"] == false && ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() != string.Empty)
                //    tvwComponent.FindNodeByValue(tvw.Nodes, ViewState["COMPONENTID"].ToString());
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

    protected void Deleted_Click()
    {
        ViewState["COMPONENTID"] = null;
        ViewState["SETCURRENTNAVIGATIONTAB"] = null;
        ViewState["ISTREENODECLICK"] = false;
        if (Request.QueryString["COMPONENTID"] != null && Request.QueryString["COMPONENTID"] != "")
        {
            ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
            ViewState["DTKEY"] = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString();
        }
        if (Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null)
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
        }
        if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
        }
        BindDeletedTreeData();
        // MenuComponent.SelectedMenuIndex = 0;
    }
    protected void BindDeletedTreeData()
    {
        try
        {
            DataSet ds = PhoenixInventoryComponent.DeletedTreeComponentList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTID";
            //tvwComponent.XPathField = "xpath";
            tvwComponent.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            tvwComponent.PopulateTree(ds.Tables[0]);
            //TreeView tvw = tvwComponent.ThisTreeView;
            //((TreeNode)tvw.Nodes[0]).Expand();
            if (ViewState["COMPONENTID"] == null && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["COMPONENTID"] = ds.Tables[0].Rows[0][0].ToString();
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString();
            }

            //cmdDeleted.Visible = false;
            //cmdComponent.Visible = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Component_Click()
    {
        ViewState["COMPONENTID"] = null;
        ViewState["SETCURRENTNAVIGATIONTAB"] = null;
        ViewState["ISTREENODECLICK"] = false;
        if (Request.QueryString["COMPONENTID"] != null && Request.QueryString["COMPONENTID"] != "")
        {
            ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
            ViewState["DTKEY"] = PhoenixInventoryComponent.ListComponent(new Guid(ViewState["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID).Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentDashboard.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString();
        }
        if (Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null)
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
        }
        if (Request.QueryString["FromRA"] != null && Request.QueryString["FromRA"] == "1")
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderDone.aspx";
        }
        BindTreeData();
        //MenuComponent.SelectedMenuIndex = 0;
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

                ViewState["COMPONENTID"] = selectednode;
                DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(selectednode), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentDashboard.aspx";
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTID=" + ViewState["COMPONENTID"] + "&p=" + ViewState["PAGENUMBER"];
                }

                // Disabling the root node click
                //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentDashboard.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 0;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentSpareItem.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 1;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentHistoryTemplate.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 2;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentDetail.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 3;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobList.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 4;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 5;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentCounters.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 6;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentWorkOrderList.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 7;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderDone.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 8;
    //        }
    //        else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderRAHistory.aspx"))
    //        {
    //            MenuComponent.SelectedMenuIndex = 9;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
