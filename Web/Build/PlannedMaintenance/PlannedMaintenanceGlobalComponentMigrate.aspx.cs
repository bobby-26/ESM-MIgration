using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;

public partial class PlannedMaintenanceGlobalComponentMigrate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuComponent.AccessRights = this.ViewState;
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["ModelID"] = "0";
            if (Request.QueryString["ModelID"] != null && General.GetNullableInteger(Request.QueryString["ModelID"].ToString()) != null)
                ViewState["ModelID"] = Request.QueryString["ModelID"].ToString();

            if (Request.QueryString["ModelName"] != null && General.GetNullableString(Request.QueryString["ModelName"].ToString()) != null)
                ViewState["ModelName"] = Request.QueryString["ModelName"].ToString();
            BindComponent();
        }
    }
    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                RadTreeNode root = tvwComponent.Nodes[0];

                if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableInteger(ViewState["ModelID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Model is required.";
                    ucError.Visible = true;
                    return;
                }

                GetAllChilds(root);
                ucStatus.Text = "Components and Component Jobs migrated to  vessel successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindComponent()
    {
        try
        {
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeHierarchy(int.Parse(ViewState["ModelID"].ToString()),null);
            
            tvwComponent.DataTextField = "FLDCOMPONENTNAME"; ;
            tvwComponent.DataValueField = "FLDGLOBALCOMPONENTID";
            tvwComponent.DataFieldID = "FLDGLOBALCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTCOMPONENTID";
            tvwComponent.DataSource = ds.Tables[0];
            tvwComponent.DataBind();

            RadTreeNode rootNode = new RadTreeNode();
            rootNode.Text = ViewState["ModelName"].ToString();
            rootNode.Value = "_Root";
            rootNode.Expanded = true;
            rootNode.AllowEdit = false;
            rootNode.Font.Bold = true;
            while (tvwComponent.Nodes.Count > 0)
            {
                RadTreeNode node = tvwComponent.Nodes[0];
                rootNode.Nodes.Add(node);
            }
            tvwComponent.Nodes.Add(rootNode);

            rootNode = tvwComponent.Nodes[0];
            RadLabel lblDisplayName = (RadLabel)rootNode.FindControl("lblDisplayName");
            UserControlNumber txtUnits = (UserControlNumber)rootNode.FindControl("txtUnit");
            LinkButton cmdAdd = (LinkButton)rootNode.FindControl("cmdAdd");
            if (lblDisplayName != null)
                lblDisplayName.Text = ViewState["ModelName"].ToString();
            if (txtUnits != null)
                txtUnits.Visible = false;
            if (cmdAdd != null)
                cmdAdd.Visible = false;


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
                //hdnComponentID.Value = e.Node.Value.ToString();
                //hdnComponentTypeID.Value = e.Node.Attributes["ID"].ToString();
                //txtComponentNumber.Text = e.Node.Attributes["NUMBER"].ToString();
                //txtName.Text = e.Node.Attributes["NAME"].ToString();
                //txtUnits.Text = e.Node.Attributes["UNITS"].ToString();

                RadTreeNode parent = e.Node.ParentNode;
                //txtParent.Text = parent.Value.ToLower() != "_root" ? parent.Attributes["NUMBER"].ToString() : "";
                //gvPlannedMaintenanceJob.Rebind();
                //gvPlannedMaintenanceJob.SelectedIndexes.Clear();

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

    protected void tvwComponent_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
    {
        RadTreeNode nodeEdited = e.Node;
        string newText = e.Text;
        nodeEdited.Text = newText;
        RadLabel lblDisplayName = (RadLabel)nodeEdited.FindControl("lblDisplayName");
        lblDisplayName.Text = newText;
        nodeEdited.Attributes["NAME"] = newText.Substring(newText.IndexOf('-')+1,newText.Length- (newText.IndexOf('-')+1));
    }

    protected void cmdAdd_Click(object sender, EventArgs e)
    {
        RadTreeNode node = (RadTreeNode)((LinkButton)sender).Parent;
        RadTreeNode parentNode = (RadTreeNode)node.ParentNode;
       

        string number = node.Attributes["NUMBER"].ToString();
        int units = General.GetNullableInteger(txtUnits.Text) != null ? int.Parse(txtUnits.Text) : 1;

        if(node.GetAllNodes().Count()==0)
        {
            int currentnodecount = 0;
            foreach(RadTreeNode n in parentNode.GetAllNodes())
            {
                string[] splitnumber = node.Attributes["NUMBER"].ToString().Split('.');
                string resultnumber = "";
                for (int count = 0; count < splitnumber.Length; count++)
                {
                    if (count == splitnumber.Length - 2)
                        resultnumber += splitnumber[count] + ".";
                }

                if (n.Attributes["NUMBER"].ToString().Contains(resultnumber))
                    currentnodecount = currentnodecount + 1;
            }

            if(currentnodecount < units)
            {
                string name = node.Attributes["NAME"].ToString();
                RadTreeNode previndexnode = node;
                for (int i = currentnodecount; i < units; i++)
                {
                    RadTreeNode childNode = new RadTreeNode();
                    childNode.Text = node.Text;
                    childNode.Value = node.Value;

                    string[] splitnumber = node.Attributes["NUMBER"].ToString().Split('.');
                    string resultnumber = "";
                    for (int count = 0; count < splitnumber.Length; count++)
                    {
                        if (count == splitnumber.Length - 1)
                            resultnumber += (int.Parse(splitnumber[count].ToString()) + i).ToString("D2");
                        else
                            resultnumber += splitnumber[count] + ".";
                    }
                    childNode.Attributes["NUMBER"] = resultnumber;
                    if (name.Substring(name.Length - 4, 4).ToUpper() == "NO 1")
                    {
                        childNode.Attributes["NAME"] = name.Substring(0, name.Length - 4)+" NO "+(i+1).ToString();
                    }
                    else
                    {
                        childNode.Attributes["NAME"] = node.Attributes["NAME"].ToString();
                    }

                    childNode.Text = resultnumber + '-' + node.Attributes["NAME"].ToString();
                     
                    childNode.Attributes["UNITS"] = node.Attributes["UNITS"].ToString();
                    childNode.Attributes["ID"] = node.Attributes["ID"].ToString();
                    childNode.Expanded = true;
                    childNode.Selected = true;
                    //childNode.InsertAfter(previndexnode);
                    int previndex = parentNode.Nodes.IndexOf(previndexnode);
                    parentNode.Nodes.Insert(previndex + 1, childNode);
                    previndexnode = childNode;
                    RadTreeNode currentNode = (RadTreeNode)tvwComponent.SelectedNode;
                    RadLabel lblDisplayName = (RadLabel)currentNode.FindControl("lblDisplayName");
                    LinkButton cmdAdd = (LinkButton)currentNode.FindControl("cmdAdd");
                    if (lblDisplayName != null)
                        lblDisplayName.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString();
                    if (cmdAdd != null)
                        cmdAdd.Visible = false;

                    
                }
            }
            
        }
        else
        {
            
            RadTreeNode clone = node.Clone();
            int index = parentNode.Nodes.IndexOf(node);
            int childcount = parentNode.Nodes.Count;
            parentNode.Nodes.Insert((childcount-1) + 1,clone);
            
            RadTreeNode childNode = (RadTreeNode)parentNode.Nodes[(childcount - 1) + 1];
            string[] splitnumber = childNode.Attributes["NUMBER"].ToString().Split('.');
            string resultnumber = "";
            for (int count = 0; count < splitnumber.Length; count++)
            {
                if (count == 0)
                    resultnumber += (int.Parse(splitnumber[count].ToString()) + childcount).ToString();
                else
                    resultnumber += "." + splitnumber[count];
            }
            childNode.Attributes["NUMBER"] = resultnumber;
            childNode.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString();
            RadLabel lblDisplayName = (RadLabel)childNode.FindControl("lblDisplayName");
            LinkButton cmdAdd = (LinkButton)childNode.FindControl("cmdAdd");
            if (lblDisplayName != null)
                lblDisplayName.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString(); 
            if (cmdAdd != null)
                cmdAdd.Visible = false;

            
           
            


            GetAllNodes(childNode,childcount);
        }
        
        
    }
    protected void GetAllNodes(RadTreeNode node,int childcount)
    {
        if (node.Nodes.Count == 0)
            return;
        foreach (RadTreeNode item in node.Nodes)
        {
            string[] splitnumber = item.Attributes["NUMBER"].ToString().Split('.');
            string resultnumber = "";
            for (int count = 0; count < splitnumber.Length; count++)
            {
                if (count == 0)
                    resultnumber += (int.Parse(splitnumber[count].ToString()) + childcount).ToString();
                else
                    resultnumber += "." + splitnumber[count];
            }
            item.Attributes["NUMBER"] = resultnumber;
            item.Text = resultnumber + '-' + item.Attributes["NAME"].ToString();

            RadLabel lblDisplayName1 = (RadLabel)item.FindControl("lblDisplayName");
            LinkButton cmdAdd1 = (LinkButton)item.FindControl("cmdAdd");
            if (lblDisplayName1 != null)
                lblDisplayName1.Text = resultnumber + '-' + item.Attributes["NAME"].ToString(); 
            if (cmdAdd1 != null)
                cmdAdd1.Visible = false;

            GetAllNodes(item,childcount);
        }
    }
    protected void GetAllChilds(RadTreeNode node)
    {
        if (node.Nodes.Count == 0)
            return;
        foreach (RadTreeNode item in node.Nodes)
        {
            RadTreeNode parent = item.ParentNode;
            if (item.Checked)
            {
                PhoenixPlannedMaintenanceGlobalComponent.MigrateVessel(int.Parse(ucVessel.SelectedVessel)
                                                , item.Attributes["NUMBER"].ToString()
                                                , item.Attributes["NAME"].ToString()
                                                , parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString()
                                                , int.Parse(ViewState["ModelID"].ToString())
                                                , new Guid(item.Attributes["ID"].ToString())
                        );
                
            }
            GetAllChilds(item);

        }
    }
    protected void tvwComponent_NodeCreated(object sender, RadTreeNodeEventArgs e)
    {

    }
}
