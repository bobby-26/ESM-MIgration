using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;

public partial class PlannedMaintenanceGlobalComponentRegister : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("General", "GENERAL",ToolBarDirection.Left);
        //toolbarmain.AddButton("Jobs", "JOB",ToolBarDirection.Left);

        //MenuComponent.AccessRights = this.ViewState;
        //MenuComponent.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbargeneral = new PhoenixToolbar();
        toolbargeneral.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbargeneral.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbargeneral.AddButton("New", "NEW", ToolBarDirection.Right);
        
        MenuGeneral.AccessRights = this.ViewState;
        MenuGeneral.MenuList = toolbargeneral.Show();

        PhoenixToolbar toolbarRA = new PhoenixToolbar();
        toolbarRA.AddFontAwesomeButton("", "Map Risk Assessment", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbarRA.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','RA Exception List','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselComponentExceptionRAList.aspx'); return false;", "RA Exception List", "<i class=\"fas fa-exclamation\"></i>", "EXCEPTION");
        MenuRA.AccessRights = this.ViewState;
        MenuRA.MenuList = toolbarRA.Show();

        PhoenixToolbar toolbarJob = new PhoenixToolbar();
        toolbarJob.AddFontAwesomeButton("", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbarJob.AddFontAwesomeButton("", "Add Jobs", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuJob.AccessRights = this.ViewState;
        MenuJob.MenuList = toolbarJob.Show();

        if (!IsPostBack)
        {
            ViewState["COMPONENTID"] = "";
            gvPlannedMaintenanceJob.PageSize = General.ShowRecords(null);
            gvRA.PageSize = General.ShowRecords(null);
            BindComponent();
        }
        
    }
    protected void MenuJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (ViewState["COMPONENTID"]!=null && General.GetNullableGuid(ViewState["COMPONENTID"].ToString()) != null)
                {
                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceGlobalJobAdd.aspx?GLOBALCOMPONENTID=" + ViewState["COMPONENTID"].ToString() + "');");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "Select Component to add Jobs";
                    ucError.Visible = true;
                    return;
                }
                
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvPlannedMaintenanceJob.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;



    //        if (CommandName.ToUpper().Equals("GENERAL"))
    //        {
    //        }
    //        else if (CommandName.ToUpper().Equals("JOB"))
    //        {
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            Session["CustomTreeViewKey"] = this.Session.SessionID;
            string fileId = Session["CustomTreeViewKey"].ToString();
            RadPersistenceManager1.StorageProviderKey = fileId;
            RadPersistenceManager1.SaveState();

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["COMPONENTID"].ToString() != null && ViewState["COMPONENTID"].ToString() != "")
            {
                if (CommandName.ToUpper().Equals("NEW"))
                {
                    txtParent.Text = txtNumber.Text;
                    txtName.Text = "";
                    txtNumber.Text = "";
                    hdnComponentID.Value = "";
                    ddlUnitType.ClearSelection();
                    ddlUnitType.SelectedValue = "1";
                    //ucCategory.SelectedValue = "Dummy";
                }
                else if (CommandName.ToUpper().Equals("SAVE"))
                {
                    if(!IsValidComponent())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixPlannedMaintenanceGlobalComponent.ComponentInsert(General.GetNullableGuid(hdnComponentID.Value), txtNumber.Text, txtName.Text, General.GetNullableString(txtParent.Text), int.Parse(ucCategory.SelectedQuick)
                                , chkTypeRequired.Checked == true ? 1 : 0, General.GetNullableInteger(ddlUnitType.SelectedValue)
                                , 0, 0
                                , chkShowFMS.Checked == true ? 1 : 0, chkIsVIR.Checked == true ? 1 : 0
                                , chkIsLocation.Checked == true ? 1 : 0
                                , General.GetNullableInteger(rbCompClasification.SelectedValue)
                                ,   radcbannexvi.Checked == true ? 1 : 0);
                   
                    BindComponent();
                }
                else if (CommandName.ToUpper().Equals("DELETE"))
                {
                    if (General.GetNullableGuid(hdnComponentID.Value) == null)
                    {
                        ucError.ErrorMessage = "Select the Component to Delete.";
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixPlannedMaintenanceGlobalComponent.ComponentDelete(new Guid(hdnComponentID.Value));
                    txtParent.Text = "";
                    txtName.Text = "";
                    txtNumber.Text = "";
                    hdnComponentID.Value = "";
                    ddlUnitType.ClearSelection();
                    BindComponent();
                }

            }
            else
            {
                ucError.ErrorMessage = "Select a component.";
                ucError.Visible = true;
                return;
            }

            fileId = Session["CustomTreeViewKey"].ToString();
            RadPersistenceManager1.StorageProviderKey = fileId;
            RadPersistenceManager1.LoadState();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidComponent()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtNumber.Text)==null)
            ucError.ErrorMessage = "Number is required";

        if (General.GetNullableString(txtName.Text) == null)
            ucError.ErrorMessage = "Name is required";

        if (General.GetNullableInteger(ucCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Category is required";

        return (!ucError.IsError);
    }
    protected void BindComponent()
    {
        try
        {
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.TreeGlobalComponentList(null);
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDGLOBALCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTCOMPONENTID";
            tvwComponent.DataFieldID = "FLDGLOBALCOMPONENTID";

            tvwComponent.DataSource = ds.Tables[0];
            tvwComponent.DataBind();

            RadTreeNode rootNode = new RadTreeNode();
            rootNode.Text = "Global Components"; ;
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
        }
        catch(Exception ex)
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
            e.Node.Attributes["CATEGORY"] = drv["FLDCATEGORY"].ToString();
            e.Node.Attributes["TYPEREQUIREDYN"] = drv["FLDTYPEREQUIREDYN"].ToString();
            e.Node.Attributes["INCREMENTTYPE"] = drv["FLDUNITINCREMENTTYPE"].ToString();
            //e.Node.Attributes["OPERATIONAL"] = drv["FLDOPERATIONALCRITICALYN"].ToString();
            //e.Node.Attributes["ENVIRONMENTAL"] = drv["FLDENVIRONMENTALCRITICALYN"].ToString();
            e.Node.Attributes["SHOWFMS"] = drv["FLDSHOWINFMS"].ToString();
            e.Node.Attributes["ISVIR"] = drv["FLDISVIR"].ToString();
            e.Node.Attributes["SHOWMARPOL"] = drv["FLDINCLUDEINMARPOLANNEXSIX"].ToString();
            e.Node.Attributes["ISLOCATION"] = drv["FLDISLOCATION"].ToString();
            e.Node.Attributes["CRITICALCATEGORY"] = drv["FLDCRITICALCATEGORY"].ToString();
            if (ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() == drv["FLDGLOBALCOMPONENTID"].ToString())
                e.Node.Selected = true;
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            Session["CustomTreeViewKey"] = this.Session.SessionID;
            string fileId = Session["CustomTreeViewKey"].ToString();
            RadPersistenceManager1.StorageProviderKey = fileId;
            RadPersistenceManager1.SaveState();

            ViewState["ISTREENODECLICK"] = true;

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                string selectednode = e.Node.Value.ToString();
                string selectedvalue = e.Node.Text.ToString();

                ViewState["COMPONENTID"] = e.Node.Value.ToString();
                hdnComponentID.Value = e.Node.Value.ToString();
                txtNumber.Text = e.Node.Attributes["NUMBER"].ToString();
                txtName.Text = e.Node.Attributes["NAME"].ToString();
                ucCategory.SelectedQuick = e.Node.Attributes["CATEGORY"].ToString();
                chkTypeRequired.Checked = e.Node.Attributes["TYPEREQUIREDYN"].ToString() == "1" ? true : false;
                ddlUnitType.ClearSelection();
                ddlUnitType.SelectedValue = e.Node.Attributes["INCREMENTTYPE"].ToString();
                //chkOperationalCritical.Checked = e.Node.Attributes["OPERATIONAL"].ToString() == "1" ? true : false;
                //chkEnvironmentalCritical.Checked = e.Node.Attributes["ENVIRONMENTAL"].ToString() == "1" ? true : false;
                chkIsVIR.Checked = e.Node.Attributes["ISVIR"].ToString() == "1" ? true : false;
                chkShowFMS.Checked = e.Node.Attributes["SHOWFMS"].ToString() == "1" ? true : false;
                chkIsLocation.Checked = e.Node.Attributes["ISLOCATION"].ToString() == "1" ? true : false;
                rbCompClasification.SelectedValue = e.Node.Attributes["CRITICALCATEGORY"].ToString();
                radcbannexvi.Checked = e.Node.Attributes["SHOWMARPOL"].ToString() == "1" ? true : false;
                RadTreeNode parent = e.Node.ParentNode;
                txtParent.Text = parent.Value.ToLower() != "_root" ? parent.Attributes["NUMBER"].ToString() : "";
                gvPlannedMaintenanceJob.Rebind();
                gvRA.Rebind();

                // Disabling the root node click
                //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "treeheight", "PaneResized();", true);

                fileId = Session["CustomTreeViewKey"].ToString();
                RadPersistenceManager1.StorageProviderKey = fileId;
                RadPersistenceManager1.LoadState();
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

            ds = PhoenixPlannedMaintenanceGlobalComponent.ComponentJobSearch(General.GetNullableGuid(ViewState["COMPONENTID"].ToString()),
                                                           nvc.Get("CODE") != null ? General.GetNullableString(nvc.Get("CODE").ToString()) : null,
                                                           nvc.Get("TITLE") != null ? General.GetNullableString(nvc.Get("TITLE").ToString()) : null,
                                                           sortexpression, sortdirection,
                                                           gvPlannedMaintenanceJob.CurrentPageIndex + 1,
                                                           gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount);

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
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                LinkButton jdbtn = (LinkButton)e.Item.FindControl("cmdJobDesc");
                if (jdbtn != null)
                {
                    jdbtn.Attributes.Add("onclick", "return openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?framename=ifMoreInfo&JOBID=" + drv["FLDJOBID"].ToString() + "','false','1066','320'); return false;");
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
            if (e.CommandName.Equals(RadGrid.FilterCommandName))
            {
                foreach (GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
                {
                    nvc.Remove(c.CurrentFilterValue.ToString());
                    nvc.Add(c.UniqueName, c.CurrentFilterValue.ToString());
                }
                gvPlannedMaintenanceJob.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
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
            gvRA.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuRA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (ViewState["COMPONENTID"] != null && General.GetNullableGuid(ViewState["COMPONENTID"].ToString()) != null)
                {
                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentRAMapping.aspx?GLOBALCOMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&VESSELID=0');");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "Select Component to add Jobs";
                    ucError.Visible = true;
                    return;
                }

            }
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }

                LinkButton cmdRAGeneric = (LinkButton)e.Item.FindControl("cmdRAGeneric");
                if (cmdRAGeneric != null)
                    cmdRAGeneric.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + item.GetDataKeyValue("FLDRAID").ToString() + "&showmenu=0&showexcel=NO');return true;");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentRAUnMap(new Guid(item.GetDataKeyValue("FLDID").ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["RASORTEXPRESSION"] == null) ? null : (ViewState["RASORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["RASORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["RASORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentRASearch(General.GetNullableGuid(ViewState["COMPONENTID"].ToString()),
                                                           sortexpression, sortdirection,
                                                           gvRA.CurrentPageIndex + 1,
                                                           gvRA.PageSize, ref iRowCount, ref iTotalPageCount,0);

            gvRA.DataSource = ds;
            gvRA.VirtualItemCount = iRowCount;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRA_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["RASORTEXPRESSION"] = e.SortExpression;
        ViewState["RASORTDIRECTION"] = ViewState["RASORTDIRECTION"] != null && ViewState["RASORTDIRECTION"].ToString() == "0" ? "1" : "0";
    }

    protected void gvPlannedMaintenanceJob_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        ViewState["SORTDIRECTION"] = ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0" ? "1" : "0";
    }
}