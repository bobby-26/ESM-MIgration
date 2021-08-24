using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceManualsComponentTreeView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        MenuComponent.MenuList = toolbarmain.Show();
        MenuComponent.SetTrigger(pnlComponet);

        if (!IsPostBack)
        {
            ucTitle.Text = Request.QueryString["rpath"];
            if (Request.QueryString["vslid"] != null)
                ViewState["VESSELID"] = int.Parse(Request.QueryString["vslid"].ToString());
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            
            BindTreeData();
            BindMappedComponent();
        }
    }
    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;

            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string csvComponentid = CheckedChildNodes();
                if (!string.IsNullOrEmpty(csvComponentid))
                {
                    PhoenixPlannedMaintenanceManuals.InsertPMSManualsComponentMapping(int.Parse(ViewState["VESSELID"].ToString()), Request.QueryString["rpath"], csvComponentid);
                    BindMappedComponent();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string CheckedChildNodes()
    {
        string componentid = string.Empty;
        for (int i = 0; i < tvwComponent.CheckedNodes.Count; i++)
        {
            componentid += tvwComponent.CheckedNodes[i].Value + ',';
        }
        return componentid = componentid.TrimEnd(',');
    }
    private void BindMappedComponent()
    {
        DataTable dt = PhoenixPlannedMaintenanceManuals.ListPMSManualsComponentMapping(int.Parse(ViewState["VESSELID"].ToString()), Request.QueryString["rpath"]);
        lstMappedComponent.DataSource = dt;
        lstMappedComponent.DataBind();
    }
    private string CheckedComponentId()
    {
        string csvComponentId = string.Empty;
        try
        {

            DataTable dt = PhoenixPlannedMaintenanceManuals.EditPMSManualsComponentMapping(int.Parse(ViewState["VESSELID"].ToString()), Request.QueryString["rpath"]);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                csvComponentId = dt.Rows[i]["FLDCSVCOMPONENTID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return csvComponentId;
    }
    protected void BindTreeData()
    {
        try
        {

            DataSet ds = PhoenixInventoryComponent.TreeComponent(int.Parse(ViewState["VESSELID"].ToString()));
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDCOMPONENTID";
            tvwComponent.ParentNodeField = "FLDPARENTID";
            tvwComponent.XPathField = "xpath";
            tvwComponent.RootText = Request.QueryString["vslname"];
            tvwComponent.ShowCheckBoxes = TreeNodeTypes.All;
            tvwComponent.SelectedCheckBoxValues = CheckedComponentId();
            tvwComponent.PopulateTree(ds);
            TreeView tvw = tvwComponent.ThisTreeView;
            ((TreeNode)tvw.Nodes[0]).Expand();

            // Disabling the root node click
            string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        if (txtComponentName.Text != null && txtComponentName.Text != "")
        {
            tvwComponent.SearchNodeByText(txtComponentName.Text, tvwComponent.Nodes);
        }
    }
}
