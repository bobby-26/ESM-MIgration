using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class UserControlTreeView : System.Web.UI.UserControl
{
    public event EventHandler SelectNodeEvent;

    string _XPathField;
    string _ParentNodeField;
    string _DataTextField;
    string _DataValueField;
    string _RootText;
    bool intNodeFound = false;
    private List<string> _MatchedText = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tvwTree.FindNode(@"Root").Select();
        }
        if (tvwTree.FindNode("Root").ChildNodes.Count > 0)
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SelectTree", "if(typeof " + tvwTree.Parent.ID + "_tvwTree_Data === \"undefined\"){}else{setTimeout(\"fnScrollToTree(" + tvwTree.Parent.ID + "_tvwTree_Data)\",1000);}", true);
    }

    public string RootText
    {
        get { return _RootText; }
        set { _RootText = value; }
    }


    public string XPathField
    {
        get { return _XPathField; }
        set { _XPathField = value; }
    }

    public string ParentNodeField
    {
        get { return _ParentNodeField; }
        set { _ParentNodeField = value; }
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


    public void PopulateTree(DataTable dt)
    {
        tvwTree.FindNode("Root").ChildNodes.Clear();
        bool bfirstRow = false;
        foreach (DataRow dr in dt.Rows)    
        {
            TreeNode node = new TreeNode();
            node.Text = dr[_DataTextField].ToString();
            node.Value = dr[_DataValueField].ToString();
            if (!string.IsNullOrEmpty(SelectedCheckBoxValues))
            {
                string selectedvalues = SelectedCheckBoxValues.Trim(',');
                if (("," + selectedvalues + ",").Contains("," + node.Value + ",") == true)
                    node.Checked = true;
            }
            string CurrentNodeXPathParent = dr[_XPathField].ToString().TrimEnd('/');
            if(CurrentNodeXPathParent.Contains("/"))
                CurrentNodeXPathParent = CurrentNodeXPathParent.Substring(0, CurrentNodeXPathParent.LastIndexOf('/'));

            if (dr[_ParentNodeField].ToString().Equals("") || dr[_ParentNodeField] == null || bfirstRow == false)
                tvwTree.FindNode(@"Root").ChildNodes.Add(node);
            else
                tvwTree.FindNode(@"Root/" + CurrentNodeXPathParent.ToLower()).ChildNodes.Add(node);

            bfirstRow = true;
        }
        tvwTree.FindNode(@"Root").Text = _RootText;        
    }

    public void PopulateTree(DataSet ds)
    {        
        PopulateTree(ds.Tables[0]);
        tvwTree.CollapseAll();
        new TreeViewState().RestoreTreeView(tvwTree, this.GetType().ToString());
    }
    public void FindNodeByValue(TreeNodeCollection n, string val)
    {         
        if (intNodeFound) return;

        for (int i = 0; i < n.Count; i++)
        {
            if (n[i].Value == val)
            {
                n[i].Select();
                intNodeFound = true;
                return;
            }
            n[i].Expand();
            FindNodeByValue(n[i].ChildNodes, val);
            if (intNodeFound) return;
            n[i].Collapse();
        }
    }
    public void FindNodeByText(string searchText, TreeNodeCollection n)
    {
        if (intNodeFound) return;

        for (int i = 0; i < n.Count; i++)
        {
            if (n[i].Text.ToLower().Contains(searchText.ToLower()) && !MatchedText.Contains(n[i].Text.ToLower()))
            {
                _MatchedText.Add(n[i].Text.ToLower());
                n[i].Select();
                intNodeFound = true;
                tvwTree_SelectedNodeChanged(null, null);
                return;
            }
            n[i].Expand();
            FindNodeByText(searchText, n[i].ChildNodes);
            if (intNodeFound) return;
            n[i].Collapse();
        }
    }
    public void SearchNodeByText(string searchText, TreeNodeCollection nodes)
    {
        _MatchedText = MatchedText;
        if (LastSearchText != searchText)
        {
            //It's a new Search
            MatchedText = null;
            LastSearchText = searchText;
        }
        FindNodeByText(searchText, nodes);
        MatchedText = _MatchedText;
    }
    protected void tvwTree_Unload(object sender, EventArgs e)
    {
        new TreeViewState().SaveTreeView(tvwTree, this.GetType().ToString());
    }

    protected void tvwTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        tvwTree.SelectedNode.Expand();
        TreeViewSelectNodeEvent tvsne = new TreeViewSelectNodeEvent();
        tvsne.SelectedNode = tvwTree.SelectedNode;
        OnSelectNodeEvent(tvsne);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SelectTree", "if(typeof " + tvwTree.Parent.ID + "_tvwTree_Data === \"undefined\"){}else{setTimeout(\"fnScrollToTree(" + tvwTree.Parent.ID + "_tvwTree_Data)\",500);}", true);
    }

    protected void OnSelectNodeEvent(EventArgs e)
    {
        if (SelectNodeEvent != null)
            SelectNodeEvent(this, e);
    }

    public TreeNode SelectedNode
    {
        get
        {
            return tvwTree.SelectedNode;
        }
    }
    public TreeNodeTypes ShowCheckBoxes
    {
        set
        {
            tvwTree.ShowCheckBoxes = value;
        }
        get
        {
            return tvwTree.ShowCheckBoxes;
        }
    }
    public TreeView ThisTreeView
    {
        get
        {
            return tvwTree;
        }
    }
    public TreeNodeCollection CheckedNodes
    {
        get
        {
            return tvwTree.CheckedNodes;
        }
    }
    public TreeNodeCollection Nodes
    {
        get
        {
            return tvwTree.Nodes;
        }
    }
    public string SelectedCheckBoxValues
    {
        get;
        set;
    }
    private List<string> MatchedText
    {
        get
        {
            if (Session[tvwTree.ClientID] == null)
                return new List<string>();
            else
                return ((List<string>)Session[tvwTree.ClientID]);
        }
        set
        {
            Session[tvwTree.ClientID] = value;
        }
    }    
    //private int LastNodeIndex
    //{
    //    get
    //    {
    //        if (ViewState["LastNodeIndex"] == null)
    //            return 0;
    //        else
    //            return int.Parse(ViewState["LastNodeIndex"].ToString());
    //    }
    //    set
    //    {
    //        ViewState["LastNodeIndex"] = value;
    //    }
    //}
    private string LastSearchText
    {
        get
        {
            if (ViewState["LastSearchText"] == null)
                return null;
            else
                return ViewState["LastSearchText"].ToString();
        }
        set
        {
            ViewState["LastSearchText"] = value;
        }
    }

    //public void SearchNodeByText(string searchText, TreeNodeCollection nodes)
    //{        
    //    if (String.IsNullOrEmpty(searchText))
    //    {
    //        return;
    //    };

    //    if (LastSearchText != searchText)
    //    {
    //        //It's a new Search
    //        CurrentNodeMatches = null;
    //        LastSearchText = searchText;
    //        LastNodeIndex = 0;
    //        SearchNodes(searchText, nodes);
    //        CurrentNodeMatches = _CurrentNodeMatches;
    //    }

    //    if (LastNodeIndex >= 0 && CurrentNodeMatches.Count > 0 && LastNodeIndex < CurrentNodeMatches.Count)
    //    {            
    //        TreeNode selectedNode = CurrentNodeMatches[LastNodeIndex];
    //        LastNodeIndex++;            
    //        //tvwTree.SelectedNode = selectedNode;
    //        selectedNode.Select();
    //        selectedNode.Expand();            
    //    }
    //}
    //private void SearchNodes(string SearchText, TreeNodeCollection nodes)
    //{
    //    TreeNode node = null;
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        node = nodes[i];
    //        if (node.Text.ToLower().Contains(SearchText.ToLower()))
    //        {
    //            _CurrentNodeMatches.Add(node);
    //        };
    //        if (node.ChildNodes.Count != 0)
    //        {
    //            SearchNodes(SearchText, node.ChildNodes);//Recursive Search 
    //        };
    //    }
    //}
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

}