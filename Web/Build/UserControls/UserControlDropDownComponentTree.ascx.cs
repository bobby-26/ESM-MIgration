using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class UserControlDropDownComponentTree : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string SelectedComponent
    {
        get
        {            
            return AsycTree.SelectedValue;            
        }        
    }
    protected void txtComponentSearch_TextChanged(object sender, EventArgs e)
    {
        RadTextBox searchstring = (RadTextBox)sender;
        RadTreeView tree = (RadTreeView)AsycTree.Items[0].FindControl("DropDownTreeView");
        if (tree != null && searchstring.Text.Length > 0)
        {
            DataTable dt = PhoenixInventoryComponent.TreeComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, searchstring.Text);
            PopulateTree(dt);
            searchstring.Focus();
            if (searchstring.Text.Trim().Length > 0)
                tree.ExpandAllNodes();
        }
        else
        {
            LoadRootNodes(tree, TreeNodeExpandMode.ServerSideCallBack);
        }
    }
    private void PopulateTree(DataTable dt)
    {
        RadTreeView tree = (RadTreeView)AsycTree.Items[0].FindControl("DropDownTreeView");
        tree.DataTextField = "FLDCOMPONENTNAME";
        tree.DataValueField = "FLDCOMPONENTID";
        tree.DataFieldID = "FLDCOMPONENTID";
        tree.DataFieldParentID = "FLDPARENTID";
        tree.DataSource = dt;
        tree.DataBind();
    }

    protected void DropDownTreeView_NodeExpand(object sender, RadTreeNodeEventArgs e)
    {
        PopulateNodeOnDemand(e, TreeNodeExpandMode.ServerSideCallBack);
    }
    private void PopulateNodeOnDemand(RadTreeNodeEventArgs e, TreeNodeExpandMode expandMode)
    {
        DataTable data = PhoenixInventoryComponent.TreeComponentAsyncList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(e.Node.Value));

        foreach (DataRow row in data.Rows)
        {
            RadTreeNode node = new RadTreeNode();
            node.Text = row["FLDCOMPONENTNAME"].ToString();
            node.Value = row["FLDCOMPONENTID"].ToString();
            if (Convert.ToInt32(row["FLDHASCHILDREN"]) > 0)
            {
                node.ExpandMode = expandMode;
            }
            e.Node.Nodes.Add(node);
        }
        e.Node.Expanded = true;
    }
    private void LoadRootNodes(RadTreeView treeView, TreeNodeExpandMode expandMode)
    {
        treeView.Nodes.Clear();
        DataTable data = PhoenixInventoryComponent.TreeComponentAsyncList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);

        foreach (DataRow row in data.Rows)
        {
            RadTreeNode node = new RadTreeNode();
            node.Text = row["FLDCOMPONENTNAME"].ToString();
            node.Value = row["FLDCOMPONENTID"].ToString();
            if (Convert.ToInt32(row["FLDHASCHILDREN"]) > 0)
            {
                node.ExpandMode = expandMode;
            }
            treeView.Nodes.Add(node);
        }
    }

    protected void AsycTree_Load(object sender, EventArgs e)
    {
        RadTreeView tree = (RadTreeView)AsycTree.Items[0].FindControl("DropDownTreeView");
        LoadRootNodes(tree, TreeNodeExpandMode.ServerSideCallBack);
    }

    protected void DropDownTreeView_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        //e.Node.Text = Server.HtmlEncode(e.Node.Text);
        e.Node.ToolTip = Server.HtmlEncode(e.Node.Text);
    }
}