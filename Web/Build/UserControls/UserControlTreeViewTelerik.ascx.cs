using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UserControlTreeViewTelerik : System.Web.UI.UserControl
{
    public event EventHandler NodeClickEvent;
    public event EventHandler NodeDataBoundEvent;
    string _DataFieldParentID;
    string _DataTextField;
    string _DataValueField;
    string _RootText;
    bool _Expanded = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    public string EmptyMessage
    {
        get { return treeViewSearch.EmptyMessage; }
        set { treeViewSearch.EmptyMessage = value; }
    }
    public string RootText
    {
        get { return _RootText; }
        set { _RootText = value; }
    }
    public bool Expanded
    {
        set
        {
            _Expanded = value;
        }
    }
    public string DataFieldParentID
    {
        get { return _DataFieldParentID; }
        set { _DataFieldParentID = value; }
    }

    public string SearchText
    {
        get {return treeViewSearch.Text; }
    }
    public string DataTextField
    {
        get { return _DataTextField; }
        set { _DataTextField = value; }
    }
    public string DataValueField
    {
        get { return _DataValueField; }
        set { _DataValueField = value; }
    }
    public RadTreeNode SelectedNode
    {
        get
        {
            return treeViewLocation.SelectedNode;
        }
    }
    public string SelectNodeByValue
    {
        set
        {
            RadTreeNode node = treeViewLocation.FindNodeByValue(value);
            if (node != null)
            {
                node.Selected = true;
                node.Expanded = true;
                node.ExpandParentNodes();
                node.Focus();
            }
        }
    }
    public bool EnableSearch
    {
        set
        {
            treeViewSearch.Visible = value;
            divTreeFilter.Attributes["style"] = "display:" + (value ? "block" : "none");
        }
    }
    public string SearchEmptyMessage
    {
        set
        {
            treeViewSearch.EmptyMessage = value;
        }
    }
    public void PopulateTree(DataTable dt)
    {
        treeViewLocation.DataTextField = DataTextField;
        treeViewLocation.DataValueField = DataValueField;
        treeViewLocation.DataFieldID = DataValueField;
        treeViewLocation.DataFieldParentID = DataFieldParentID;
        treeViewLocation.DataSource = dt;
        treeViewLocation.DataBind();

        if (!string.IsNullOrEmpty(RootText))
        {
            RadTreeNode rootNode = new RadTreeNode();
            rootNode.Text = RootText;
            rootNode.Value = "_Root";
            rootNode.Expanded = _Expanded;
            rootNode.AllowEdit = false;
            rootNode.Font.Bold = true;
            while (treeViewLocation.Nodes.Count > 0)
            {
                RadTreeNode node = treeViewLocation.Nodes[0];
                rootNode.Nodes.Add(node);
            }
            treeViewLocation.Nodes.Add(rootNode);
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void treeViewLocation_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Node.DataItem;
        e.Node.Text = Server.HtmlEncode(e.Node.Text);
        bool exists = drv.DataView.ToTable().Columns.Contains("FLDISCRITICAL");
        if (exists == true && drv["FLDISCRITICAL"] != null && drv["FLDISCRITICAL"].ToString() == "1")
        {
            e.Node.ForeColor = System.Drawing.Color.Red;
        }
        e.Node.ToolTip = Server.HtmlEncode(e.Node.Text);
        OnNodeDataBoundEvent(e);
    }

    protected void treeViewLocation_NodeClick(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
    {
        OnNodeClickEvent(e);
    }
    protected void OnNodeClickEvent(Telerik.Web.UI.RadTreeNodeEventArgs e)
    {
        if (NodeClickEvent != null)
            NodeClickEvent(this, e);
    }
    protected void OnNodeDataBoundEvent(RadTreeNodeEventArgs e)
    {
        if (NodeDataBoundEvent != null)
            NodeDataBoundEvent(this, e);
    }

}