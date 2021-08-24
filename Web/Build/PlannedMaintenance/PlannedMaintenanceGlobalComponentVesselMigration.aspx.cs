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
using System.Text;
using Telerik.Web.UI.Upload;
using System.Text.RegularExpressions;

public partial class PlannedMaintenanceGlobalComponentVesselMigration : System.Web.UI.Page
{
    //int tcount = 0;
    //int i = 0;
    DataTable dtSave = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        
        toolbarmain.AddButton("Migrate", "MIGRATE", ToolBarDirection.Right);
        toolbarmain.AddButton("Get Vessel Components", "GVC", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalComponentVesselMigration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalComponentVesselMigration.aspx", "Import from Excel", "<i class=\"fas fa-download\"></i>", "IMPORT");
        MenuComponent.AccessRights = this.ViewState;
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            //RadProgressArea1.ProgressIndicators = ProgressIndicators.SelectedFilesCount;
            ViewState["ModelID"] = "0";
            if (Request.QueryString["ModelID"] != null && General.GetNullableInteger(Request.QueryString["ModelID"].ToString()) != null)
                ViewState["ModelID"] = Request.QueryString["ModelID"].ToString();

            if (Request.QueryString["ModelName"] != null && General.GetNullableString(Request.QueryString["ModelName"].ToString()) != null)
                ViewState["ModelName"] = Request.QueryString["ModelName"].ToString();
            BindComponent();

        }

        
        //RadProgressArea1.Localization.Uploaded = "Total Components";
        //RadProgressArea1.Localization.UploadedFiles = "Components";
        //RadProgressArea1.Localization.CurrentFileName = "Component in action: ";
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
                CreateTableStructure();
                GetAllChilds(root);

                DataSet ds = new DataSet();
                ds.Tables.Add(dtSave);
                string xml = ds.GetXml();
                PhoenixPlannedMaintenanceGlobalComponent.VesselComponentInsertBulk(int.Parse(ucVessel.SelectedVessel), xml);
                ucStatus.Show("Components and Component Jobs saved.");
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("GVC"))
            {
                if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
                RadTreeNode root = tvwComponent.Nodes[0];
                GetAllVesselComponents(root);

                PhoenixPlannedMaintenanceGlobalComponent.UpdateGlobalVesselComponents(int.Parse(ucVessel.SelectedVessel));
                //BindComponent();

            }
            else if (CommandName.ToUpper().Equals("MIGRATE"))
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
                
                PhoenixPlannedMaintenanceGlobalComponent.MigrateComponentsAsync(int.Parse(ucVessel.SelectedVessel));
                ucStatus.Show("Components and Component Jobs migrated to  vessel successfully.");
            }
            else if (CommandName.ToUpper().Equals("IMPORT"))
            {
                if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }

                string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceGlobalComponentStructureTemplate.aspx?VESSELID=" + ucVessel.SelectedVessel + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDMAKE", "FLDTYPE", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDCOUNTERFREQUENCYNAME", "FLDJOBCATEGORY", "FLDDISCIPLINENAME","FLDPRIORITY" };
            string[] alCaptions = { "Component Number", "Component Name", "Maker", "Model/Type", "Job Code", "Title", "Frequency",  "Counter Frequency", "Category", "Discipline","Priority"};
            
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.VesselDraftComponentsExport(General.GetNullableInteger(ucVessel.SelectedVessel));
            General.ShowExcel("Draft Component Jobs", ds.Tables[0], alColumns, alCaptions, null, null);
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
            if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
            {
                DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselComponents(int.Parse(ucVessel.SelectedVessel));

                tvwComponent.DataTextField = "FLDCOMPONENTNAME"; ;
                tvwComponent.DataValueField = "FLDCOMPONENTNUMBER";
                tvwComponent.DataFieldID = "FLDCOMPONENTNUMBER";
                tvwComponent.DataFieldParentID = "FLDPARENTCOMPONENTNUMBER";
                tvwComponent.DataSource = ds.Tables[0];
                tvwComponent.DataBind();

                RadTreeNode rootNode = new RadTreeNode();
                rootNode.Text = ucVessel.SelectedVesselName;
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
                    lblDisplayName.Text = ucVessel.SelectedVesselName;
                if (txtUnits != null)
                    txtUnits.Visible = false;
                if (cmdAdd != null)
                    cmdAdd.Visible = false;
            }
            


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
            e.Node.Attributes["MODELID"] = drv["FLDMODELID"].ToString();
            e.Node.Attributes["GLOBALCOMPONENTID"] = drv["FLDGLOBALCOMPONENTID"].ToString();
            e.Node.Attributes["TEMPNODEYN"] = "0";
            e.Node.Attributes["INCRIMENTTYPE"] = drv["FLDUNITINCREMENTTYPE"].ToString();
            LinkButton cmdAdd = (LinkButton)e.Node.FindControl("cmdAdd");
            //if (cmdAdd != null && drv["FLDUNITSYN"].ToString() == "1" && e.Node.GetAllNodes().Count == 0)
            //    cmdAdd.Visible = false;

            e.Node.Checked = drv["FLDACTIVEYN"].ToString() == "1" ? true : false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    //{
    //    try
    //    {
    //        ViewState["ISTREENODECLICK"] = true;

    //        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
    //        if (e.Node.Value.ToLower() != "_root")
    //        {
    //            string selectednode = e.Node.Value.ToString();
    //            string selectedvalue = e.Node.Text.ToString();

    //            ViewState["COMPONENTTYPEID"] = e.Node.Attributes["ID"].ToString();
    //            RadTreeNode parent = e.Node.ParentNode;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void tvwComponent_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
    {
        RadTreeNode nodeEdited = e.Node;
        RadTreeNode parentNode = e.Node.ParentNode;
        string newText = e.Text;
        nodeEdited.Text = newText;
        RadLabel lblDisplayName = (RadLabel)nodeEdited.FindControl("lblDisplayName");
        lblDisplayName.Text = newText;
        nodeEdited.Attributes["NAME"] = newText.Substring(newText.IndexOf('-')+1,newText.Length- (newText.IndexOf('-')+1));

        PhoenixPlannedMaintenanceGlobalComponent.VesselComponentInsert(int.Parse(ucVessel.SelectedVessel)
                                                 , nodeEdited.Attributes["NUMBER"].ToString()
                                                 , nodeEdited.Attributes["NAME"].ToString()
                                                 , parentNode.Value == "_Root" ? null : parentNode.Attributes["NUMBER"].ToString()
                                                 , int.Parse(nodeEdited.Attributes["MODELID"].ToString())
                                                 , new Guid(nodeEdited.Attributes["ID"].ToString())
                                                 , nodeEdited.Checked ? 1 : 0

                         );
    }

    protected void CreateUnits(RadTreeNode node)
    {
        
        string number = node.Attributes["NUMBER"].ToString();
        RadTreeNode parentNode = (RadTreeNode)node.ParentNode;
        int incrimenttype = int.Parse(node.Attributes["INCRIMENTTYPE"].ToString());
        int units = 1;
        if (General.GetNullableInteger(txtUnits.Text) != null)
        {
            units = int.Parse(txtUnits.Text);
        }
        else
        {
            if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                PhoenixPlannedMaintenanceGlobalComponent.GetVesselComponentUnits(int.Parse(ucVessel.SelectedVessel), number, ref units);
        }

        if (node.GetAllNodes().Count() == 0)
        {
            //int currentnodecount = 0;
            RadTreeNode previndexnode = node;
            //foreach (RadTreeNode n in parentNode.GetAllNodes())
            //{
            //    string[] splitnumber = node.Attributes["NUMBER"].ToString().Split('.');
            //    string resultnumber = "";
            //    for (int count = 0; count < splitnumber.Length; count++)
            //    {
            //        if (count == splitnumber.Length - 2)
            //            resultnumber += splitnumber[count] + ".";
            //    }

            //    if (n.Attributes["NUMBER"].ToString().Contains(resultnumber))
            //    {
            //        currentnodecount = currentnodecount + 1;
            //        previndexnode = n;
            //    }

            //}

            //if (currentnodecount < units)
            //{
                string name = node.Attributes["NAME"].ToString();

                for (int i = 1; i < units; i++)
                {
                    RadTreeNode childNode = new RadTreeNode();
                    childNode.Text = node.Text;
                    childNode.Value = node.Value;

                    string[] splitnumber = node.Attributes["NUMBER"].ToString().Split('.');
                    string resultnumber = "";
                    for (int count = 0; count < splitnumber.Length; count++)
                    {
                        if (incrimenttype == 1)
                        {
                            if (count == splitnumber.Length - 1)
                                resultnumber += (int.Parse(splitnumber[count].ToString()) + i).ToString("D2");
                            else
                                resultnumber += splitnumber[count] + ".";
                        }
                        else
                        {
                            if (count == 0)
                                resultnumber += (int.Parse(splitnumber[count].ToString()) + i).ToString();
                            else
                                resultnumber += "." + splitnumber[count];
                        }
                    }

                    RadTreeNode n = tvwComponent.FindNodeByAttribute("NUMBER", resultnumber);

                    if (n == null)
                    {
                        childNode.Attributes["NUMBER"] = resultnumber;
                        if (name.Substring(name.Length - 4, 4).ToUpper() == "NO 1")
                        {
                            childNode.Attributes["NAME"] = name.Substring(0, name.Length - 4) + " NO " + (i + 1).ToString();
                        }
                        else if (name.Substring(name.Length - 2, 2).ToUpper() == "1P")
                        {
                            childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "P";
                        }
                        else if (name.Substring(name.Length - 2, 2).ToUpper() == "1S")
                        {
                            childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "S";
                        }
                        else if (name.Substring(name.Length - 2, 2).ToUpper() == "1C")
                        {
                            childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "C";
                        }
                        else
                        {
                            childNode.Attributes["NAME"] = node.Attributes["NAME"].ToString();
                        }

                        childNode.Text = resultnumber + '-' + node.Attributes["NAME"].ToString();

                        childNode.Attributes["UNITS"] = node.Attributes["UNITS"].ToString();
                        childNode.Attributes["ID"] = node.Attributes["ID"].ToString();
                        childNode.Attributes["MODELID"] = node.Attributes["MODELID"].ToString();
                        childNode.Attributes["GLOBALCOMPONENTID"] = node.Attributes["GLOBALCOMPONENTID"].ToString();
                        childNode.Attributes["TEMPNODEYN"] = "1";
                        childNode.Attributes["INCRIMENTTYPE"] = node.Attributes["INCRIMENTTYPE"].ToString();
                        childNode.Expanded = true;
                        childNode.Selected = true;
                        //childNode.InsertAfter(previndexnode);
                        int previndex = parentNode.Nodes.IndexOf(previndexnode);
                        parentNode.Nodes.Insert(previndex + 1, childNode);
                        previndexnode = childNode;
                        RadTreeNode currentNode = (RadTreeNode)tvwComponent.SelectedNode;
                        RadLabel lblDisplayName = (RadLabel)currentNode.FindControl("lblDisplayName");
                        LinkButton cmdAdd = (LinkButton)currentNode.FindControl("cmdAdd");
                        //SaveNode(childNode);
                    if (lblDisplayName != null)
                            lblDisplayName.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString();
                        if (cmdAdd != null)
                            cmdAdd.Visible = false;

                    
                    }
                    else
                    {
                        previndexnode = n;
                    }
                }
            //}

        }
    }
    protected void cmdAdd_Click(object sender, EventArgs e)
    {
        RadTreeNode node = (RadTreeNode)((LinkButton)sender).Parent;
        RadTreeNode parentNode = (RadTreeNode)node.ParentNode;
        if (node.GetAllNodes().Count() == 0)
        {
            CreateUnits(node);
        }
        else
        {
            int incrimenttype = int.Parse(node.Attributes["INCRIMENTTYPE"].ToString());
            RadTreeNode clone = node.Clone();
            
            RadTreeNode previndexnode = node;
            int childcount = 0;
            string[] splitnumber = clone.Attributes["NUMBER"].ToString().Split('.');
            string resultnumber = "";
            for (int count = 0; count < splitnumber.Length; count++)
            {
                if (incrimenttype == 1)
                {
                    if (count == splitnumber.Length - 1)
                        resultnumber += (int.Parse(splitnumber[count].ToString()) + 1).ToString("D2");
                    else
                        resultnumber += splitnumber[count] + ".";
                }
                else
                {
                    if (count == 0)
                        resultnumber += (int.Parse(splitnumber[count].ToString()) + 1).ToString();
                    else
                        resultnumber += "." + splitnumber[count];
                }
                
            }

            RadTreeNode n = tvwComponent.FindNodeByAttribute("NUMBER", resultnumber);
            if(n == null)
            {
                int index = parentNode.Nodes.IndexOf(previndexnode);
                parentNode.Nodes.Insert(index + 1, clone);
                RadTreeNode childNode = (RadTreeNode)parentNode.Nodes[index + 1];
                childNode.Attributes["NUMBER"] = resultnumber;
                childNode.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString();
                string name = childNode.Attributes["NAME"].ToString();
                Regex x = new Regex(@"NO [0-9]");
                if (x.IsMatch(name.Substring(name.Length - 4, 4).ToUpper()))
                {
                    int i = General.GetNullableInteger(name.Substring(name.Length - 1, 1)) != null ? int.Parse(name.Substring(name.Length - 1, 1)) + 1 : 2;
                    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 4) + " NO " + (i) .ToString();
                }
                //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1P")
                //{
                //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "P";
                //}
                //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1S")
                //{
                //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "S";
                //}
                //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1C")
                //{
                //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "C";
                //}
                else
                {
                    childNode.Attributes["NAME"] = node.Attributes["NAME"].ToString();
                }



                RadLabel lblDisplayName = (RadLabel)childNode.FindControl("lblDisplayName");
                LinkButton cmdAdd = (LinkButton)childNode.FindControl("cmdAdd");
                if (lblDisplayName != null)
                    lblDisplayName.Text = resultnumber + '-' + childNode.Attributes["NAME"].ToString();
                //if (cmdAdd != null)
                //    cmdAdd.Visible = false;
                //SaveNode(childNode);
                GetAllNodes(childNode, childcount, incrimenttype);
            }
            
        }

        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", "clientNodeEdited();", true);
    }
    protected void GetAllNodes(RadTreeNode node,int childcount,int incrimenttype)
    {
        if (node.Nodes.Count == 0)
            return;
        foreach (RadTreeNode item in node.Nodes)
        {
            //int incrimenttype = int.Parse(item.Attributes["INCRIMENTTYPE"].ToString());

            string[] splitnumber = item.Attributes["NUMBER"].ToString().Split('.');
            string resultnumber = "";
            for (int count = 0; count < splitnumber.Length; count++)
            {
                if (incrimenttype == 1)
                {
                    if (count == splitnumber.Length - 1)
                        resultnumber += (int.Parse(splitnumber[count].ToString()) + 1).ToString("D2");
                    else
                        resultnumber += splitnumber[count] + ".";
                }
                else
                {
                    if (count == 0)
                        resultnumber += (int.Parse(splitnumber[count].ToString()) + 1).ToString();
                    else
                        resultnumber += "." + splitnumber[count];
                }
            }
            item.Attributes["NUMBER"] = resultnumber;
            item.Text = resultnumber + '-' + item.Attributes["NAME"].ToString();

            string name = item.Attributes["NAME"].ToString();
            Regex x = new Regex(@"NO [0-9]");
            if (x.IsMatch(name.Substring(name.Length - 4, 4).ToUpper()))
            {
                int i = General.GetNullableInteger(name.Substring(name.Length - 1, 1)) != null ? int.Parse(name.Substring(name.Length - 1, 1)) + 1 : 2;
                item.Attributes["NAME"] = name.Substring(0, name.Length - 4) + " NO " + (i).ToString();
            }
            //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1P")
            //{
            //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "P";
            //}
            //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1S")
            //{
            //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "S";
            //}
            //else if (name.Substring(name.Length - 2, 2).ToUpper() == "1C")
            //{
            //    childNode.Attributes["NAME"] = name.Substring(0, name.Length - 2) + (i + 1).ToString() + "C";
            //}
            //else
            //{
            //    item.Attributes["NAME"] = item.Attributes["NAME"].ToString();
            //}


            RadLabel lblDisplayName1 = (RadLabel)item.FindControl("lblDisplayName");
            LinkButton cmdAdd1 = (LinkButton)item.FindControl("cmdAdd");
            if (lblDisplayName1 != null)
                lblDisplayName1.Text = resultnumber + '-' + item.Attributes["NAME"].ToString(); 
            if (cmdAdd1 != null)
                cmdAdd1.Visible = false;

            //SaveNode(item);

            GetAllNodes(item,childcount, incrimenttype);
        }
    }
    protected void GetAllChilds(RadTreeNode node)
    {
        //RadProgressContext progress = RadProgressContext.Current;
        //progress.Speed = "N/A";

        if (node.Nodes.Count == 0)
            return;
        foreach (RadTreeNode item in node.Nodes)
        {
            RadTreeNode parent = item.ParentNode;

            //PhoenixPlannedMaintenanceGlobalComponent.VesselComponentInsert(int.Parse(ucVessel.SelectedVessel)
            //                                , item.Attributes["NUMBER"].ToString()
            //                                , item.Attributes["NAME"].ToString()
            //                                , parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString()
            //                                , int.Parse(item.Attributes["MODELID"].ToString())
            //                                , new Guid(item.Attributes["ID"].ToString())
            //                                , item.Checked ? 1 : 0

            //        );
            DataRow dr = dtSave.NewRow();
            dr["FLDVESSELID"] = ucVessel.SelectedVessel;
            dr["FLDNUMBER"] = item.Attributes["NUMBER"].ToString();
            dr["FLDNAME"] = item.Attributes["NAME"].ToString();
            dr["FLDPARENT"] = parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString();
            dr["FLDMODELID"] = item.Attributes["MODELID"].ToString();
            dr["FLDID"] = item.Attributes["ID"].ToString();
            dr["FLDACTIVEYN"]= item.Checked ? 1 : 0;
            dtSave.Rows.Add(dr);


            GetAllChilds(item);

        }

        
    }
    protected void SaveNode(RadTreeNode node)
    {
        //RadProgressContext progress = RadProgressContext.Current;
        //progress.Speed = "N/A";
            RadTreeNode parent = node.ParentNode;

            PhoenixPlannedMaintenanceGlobalComponent.VesselComponentInsert(int.Parse(ucVessel.SelectedVessel)
                                            , node.Attributes["NUMBER"].ToString()
                                            , node.Attributes["NAME"].ToString()
                                            , parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString()
                                            , int.Parse(node.Attributes["MODELID"].ToString())
                                            , new Guid(node.Attributes["ID"].ToString())
                                            , node.Checked ? 1 : 0

                    );
    }
    protected void GetAllVesselComponents(RadTreeNode node)
    {
        if (node.Nodes.Count == 0)
            return;

        for (int i = 0; i < node.Nodes.Count; i++)
        {
            RadTreeNode temp = node.Nodes[i];
            string number = temp.Attributes["NUMBER"].ToString();
            string tempnodeyn = temp.Attributes["TEMPNODEYN"].ToString();
            if (tempnodeyn == "0")
            {
                CreateUnits(temp);
                GetAllVesselComponents(temp);
            }
            
        }
       
    }
    //protected void MigrateComponents(RadTreeNode node)
    //{
    //    if (node.Nodes.Count == 0)
    //        return;

        
        

    //    foreach (RadTreeNode item in node.Nodes)
    //    {
    //        RadTreeNode parent = item.ParentNode;
    //        PhoenixPlannedMaintenanceGlobalComponent.VesselComponentInsert(int.Parse(ucVessel.SelectedVessel)
    //                                            , item.Attributes["NUMBER"].ToString()
    //                                            , item.Attributes["NAME"].ToString()
    //                                            , parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString()
    //                                            , int.Parse(item.Attributes["MODELID"].ToString())
    //                                            , new Guid(item.Attributes["ID"].ToString())
    //                                            , item.Checked ? 1 : 0

    //                    );
    //        if (item.Checked)
    //        {
    //            PhoenixPlannedMaintenanceGlobalComponent.MigrateVessel(int.Parse(ucVessel.SelectedVessel)
    //                                            , item.Attributes["NUMBER"].ToString()
    //                                            , item.Attributes["NAME"].ToString()
    //                                            , parent.Value == "_Root" ? null : parent.Attributes["NUMBER"].ToString()
    //                                            , int.Parse(item.Attributes["MODELID"].ToString())
    //                                            , new Guid(item.Attributes["ID"].ToString())
    //                    );

    //        }

    //        MigrateComponents(item);
            
    //    }
    //}

    //protected void loop()
    //{
    //    RadProgressContext progress = RadProgressContext.Current;
    //    progress.Speed = "N/A";

    //    for(int l = 0; l <= 100; l++)
    //    {
    //        progress.PrimaryTotal = 100;
    //        progress.PrimaryValue = 1;
    //        progress.PrimaryPercent = 100;

    //        progress.SecondaryTotal = 100;
    //        progress.SecondaryValue = l;
    //        progress.SecondaryPercent = l;

    //        progress.CurrentOperationText = "Step " + l.ToString();

    //        if (!Response.IsClientConnected)
    //        {
    //            //Cancel button was clicked or the browser was closed, so stop processing
    //            break;
    //        }

    //        progress.TimeEstimated = (100 - l) * 100;
    //        //Stall the current thread for 0.1 seconds
    //        //System.Threading.Thread.Sleep(100);
    //    }
    //}
    //protected void TreeCount(RadTreeNode node)
    //{
        
    //    if (node.Nodes.Count == 0)
    //        return;
        
    //    foreach (RadTreeNode item in node.Nodes)
    //    {
    //        tcount = tcount + 1;
    //        TreeCount(item);

    //    }
        
    //}
    protected void gvModel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds;
            

            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalTypeVesselConfiguration( General.GetNullableInteger(ucVessel.SelectedVessel)!=null? int.Parse(ucVessel.SelectedVessel):0);
            gvModel.DataSource = ds;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvModel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            //DataRowView drv = (DataRowView)item.DataItem;

            LinkButton cmdDelete = (LinkButton)item.FindControl("cmdDelete");
            UserControlNumber ucUnitNo = (UserControlNumber)item.FindControl("ucUnitNo");
            RadTextBox txtParentNumber = (RadTextBox)item.FindControl("txtParentNumber");
            RadLabel lblID = (RadLabel)item.FindControl("lblID");

            if (cmdDelete != null)
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

            if (txtParentNumber != null && lblID!=null && General.GetNullableInteger(lblID.Text) != null)
                txtParentNumber.Enabled = false;
            if (ucUnitNo != null && lblID!=null && General.GetNullableInteger(lblID.Text) != null)
                ucUnitNo.Enabled = false;
        }
    }

    protected void gvModel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                {
                    PhoenixPlannedMaintenanceGlobalComponent.GlobalVesselConfigurationDelete(int.Parse(ucVessel.SelectedVessel), int.Parse(item.GetDataKeyValue("FLDCONFIGURATIONID").ToString()));
                    BindComponent();
                }
            }
            if (e.CommandName.ToUpper().Equals("REFRESH"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;
                if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                {
                    PhoenixPlannedMaintenanceGlobalComponent.ComponentTypeVesselConfigurationInsert(
                                               int.Parse(ucVessel.SelectedVessel)
                                               , int.Parse(((RadLabel)item.FindControl("lblModelID")).Text)
                                               , General.GetNullableInteger(((RadLabel)item.FindControl("lblID")).Text)
                                               , General.GetNullableInteger(((RadLabel)item.FindControl("lblUnit")).Text)
                                               , General.GetNullableString(((RadLabel)item.FindControl("lblParent")).Text)
                                               , General.GetNullableInteger(((RadLabel)item.FindControl("lblBookID")).Text)
                                            );
                    BindComponent();
                }
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvModel_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;

            if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            {
                ucError.ErrorMessage = "Vessel is required";
            }
            if (General.GetNullableInteger(((RadComboBox)item.FindControl("rcbType")).SelectedValue) == null)
            {
                ucError.ErrorMessage = "Type/Model is required";
            }
            if (ucError.IsError)
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixPlannedMaintenanceGlobalComponent.ComponentTypeVesselConfigurationInsert(
                                                int.Parse(ucVessel.SelectedVessel)
                                                , int.Parse(((RadComboBox)item.FindControl("rcbType")).SelectedValue)
                                                , General.GetNullableInteger(((RadLabel)item.FindControl("lblID")).Text)
                                                , General.GetNullableInteger(((UserControlNumber)item.FindControl("ucUnitNo")).Text)
                                                , General.GetNullableString(((RadTextBox)item.FindControl("txtParentNumber")).Text)
                                                , General.GetNullableInteger(((RadComboBox)item.FindControl("rcbSpareParts")).SelectedValue)
                );
            BindComponent();
        }
        catch(Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        

    }

    protected void RadComboBoxes_DataBinding(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        GridDataItem container = combo.NamingContainer as GridDataItem;
        //DataRowView dataRowView = (DataRowView)container.DataItem;
        string masterKey = string.Empty;
        string masterKey2 = string.Empty;
        int iRowCount = 0;
        int iTotalPageCount = 0;


        switch (combo.DataValueField)
        {
            case "FLDMAKE":
                masterKey = General.GetNullableString((container.FindControl("ddlCmpName") as RadComboBox).SelectedValue) != null ? General.GetNullableString((container.FindControl("ddlCmpName") as RadComboBox).SelectedItem.Text) : null;
                combo.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelMakeList(General.GetNullableString(masterKey), null);
                break;
            case "FLDMODELID":
                masterKey = General.GetNullableString((container.FindControl("rcbMake") as RadComboBox).SelectedValue);
                masterKey2 = General.GetNullableString((container.FindControl("ddlCmpName") as RadComboBox).SelectedValue);
                combo.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(null, masterKey, null, null, null, 1, 10000, ref iRowCount, ref iTotalPageCount, null
                                , General.GetNullableString(masterKey2));
                break;
            case "FLDCOMPONENTNUMBER":
                    combo.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelComponentList(null, null);
                break;
            case "FLDBOOKID":
                masterKey = General.GetNullableString((container.FindControl("rcbType") as RadComboBox).SelectedValue);
                combo.DataSource = PhoenixPlannedMaintenanceGlobalComponent.GlobalSparePartsBookList(General.GetNullableInteger(masterKey));
                break;
            default:
                break;
        }
    }
    protected void RadComboBoxes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        if (combo.DataValueField == "FLDCOMPONENTNUMBER")
        {
            RadComboBox rcbMake = combo.NamingContainer.FindControl("rcbMake") as RadComboBox;
            rcbMake.DataBind();

        }
        if (combo.DataValueField == "FLDMAKE" )
        {
            RadComboBox rcbType = combo.NamingContainer.FindControl("rcbType") as RadComboBox;
            rcbType.DataBind();

        }
        if (combo.DataValueField == "FLDMODELID")
        {
            RadComboBox rcbSpareParts = combo.NamingContainer.FindControl("rcbSpareParts") as RadComboBox;
            rcbSpareParts.DataBind();

        }
    }
    protected void RadComboBoxes_DataBound(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        DataRowView dataRowView = (combo.NamingContainer as GridEditableItem).DataItem as DataRowView;
        if (dataRowView != null&& General.GetNullableString(dataRowView[combo.DataValueField].ToString()) !=null)
        {
            combo.FindItemByValue(dataRowView[combo.DataValueField].ToString()).Selected = true;
        }
        else
        {
            combo.Text = string.Empty;
            combo.ClearSelection();
        }
    }
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvModel.Rebind();
        BindComponent();
        if (GetMigrationStatus())
            MenuComponent.Visible = false;
        else
            MenuComponent.Visible = true;
    }
    protected bool GetMigrationStatus()
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.MigratStatus(int.Parse(ucVessel.SelectedVessel));
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
            
    }
    protected void PerformDragAndDrop(RadTreeViewDropPosition dropPosition, RadTreeNode sourceNode,
                                               RadTreeNode destNode)
    {
        if (sourceNode.Equals(destNode) || sourceNode.IsAncestorOf(destNode))
        {
            return;
        }
        sourceNode.Owner.Nodes.Remove(sourceNode);

        switch (dropPosition)
        {
            case RadTreeViewDropPosition.Over:
                // child
                if (!sourceNode.IsAncestorOf(destNode))
                {
                    destNode.Nodes.Add(sourceNode);
                }
                break;

            case RadTreeViewDropPosition.Above:
                // sibling - above                    
                destNode.InsertBefore(sourceNode);
                break;

            case RadTreeViewDropPosition.Below:
                // sibling - below
                destNode.InsertAfter(sourceNode);
                break;
        }
        RadLabel lblDisplayName1 = (RadLabel)sourceNode.FindControl("lblDisplayName");
        if (lblDisplayName1 != null)
            lblDisplayName1.Text = sourceNode.Attributes["NUMBER"].ToString()+" - "+ destNode.Attributes["NAME"].ToString() + " - " + sourceNode.Attributes["NAME"].ToString();

        sourceNode.Attributes["NAME"] = destNode.Attributes["NAME"].ToString() + " - " + sourceNode.Attributes["NAME"].ToString();
    }


    protected void tvwComponent_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
    {
            RadTreeNode sourceNode = e.SourceDragNode;
            RadTreeNode destNode = e.DestDragNode;
            RadTreeViewDropPosition dropPosition = e.DropPosition;

            if (destNode != null) //drag&drop is performed between trees
            {
                if (sourceNode.TreeView.SelectedNodes.Count <= 1)
                {
                    PerformDragAndDrop(dropPosition, sourceNode, destNode);
                }
                else if (sourceNode.TreeView.SelectedNodes.Count > 1)
                {
                    if (dropPosition == RadTreeViewDropPosition.Below) //Needed to preserve the order of the dragged items
                    {
                        for (int i = sourceNode.TreeView.SelectedNodes.Count - 1; i >= 0; i--)
                        {
                            PerformDragAndDrop(dropPosition, sourceNode.TreeView.SelectedNodes[i], destNode);
                        }
                    }
                    else
                    {
                        foreach (RadTreeNode node in sourceNode.TreeView.SelectedNodes)
                        {
                            PerformDragAndDrop(dropPosition, node, destNode);
                        }
                    }
                }

                destNode.Expanded = true;
                sourceNode.TreeView.UnselectAllNodes();
            }
        
    }
    protected void CreateTableStructure()
    {
        dtSave.Columns.Add("FLDVESSELID");
        dtSave.Columns.Add("FLDNUMBER");
        dtSave.Columns.Add("FLDNAME");
        dtSave.Columns.Add("FLDPARENT");
        dtSave.Columns.Add("FLDMODELID");
        dtSave.Columns.Add("FLDID");
        dtSave.Columns.Add("FLDACTIVEYN");
    }

    protected void imgbtnSend_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if(General.GetNullableInteger(ucVessel.SelectedVessel)==null || General.GetNullableInteger(ucToVessel.SelectedVessel) == null)
            {
                ucError.ErrorMessage = "From Vessel and To Vessel is required";
                ucError.Visible = true;
                return;
            }

            PhoenixPlannedMaintenanceGlobalComponent.CopyGlobalVesselComponents(int.Parse(ucVessel.SelectedVessel), int.Parse(ucToVessel.SelectedVessel));
            ucStatus.Show("Global Components Copied successfully");
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
