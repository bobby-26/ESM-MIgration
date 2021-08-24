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
using System.Text;

public partial class PlannedMaintenanceGlobalComponentJob : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarJob = new PhoenixToolbar();
        toolbarJob.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarJob.AddFontAwesomeButton("", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
        // toolbarJob.AddFontAwesomeButton("", "Add Jobs", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuJob.AccessRights = this.ViewState;
        MenuJob.MenuList = toolbarJob.Show();

        if (!IsPostBack)
        {
            ViewState["COMPONENTID"] = string.Empty;
            ViewState["JOBSUMMARYID"] = string.Empty;
            ViewState["ROUTINEWOID"] = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["JOBSUMMARYID"]))
            {
                ViewState["JOBSUMMARYID"] = Request.QueryString["JOBSUMMARYID"];
            }

            if (Request.QueryString["ROUTINEWOID"] != null)
            {
                ViewState["ROUTINEWOID"] = Request.QueryString["ROUTINEWOID"].ToString();
            }

            gvPlannedMaintenanceJob.PageSize = General.ShowRecords(null);
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
                if (ViewState["COMPONENTID"] != null && General.GetNullableGuid(ViewState["COMPONENTID"].ToString()) != null)
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

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ROUTINEWOID"].ToString() != "" && ViewState["ROUTINEWOID"].ToString() != string.Empty)
                {
                    RoutineWo();
                }

                if (ViewState["JOBSUMMARYID"].ToString() != "" && ViewState["JOBSUMMARYID"].ToString() != string.Empty)
                {
                    foreach (GridDataItem gvr in gvPlannedMaintenanceJob.Items)
                    {
                        if (((RadCheckBox)(gvr.FindControl("ChkComJob"))).Checked == true)
                        {
                            PhoenixPlannedMaintenanceCompJobSummary.GlobalCompJobInsert(new Guid(ViewState["JOBSUMMARYID"].ToString()), new Guid(gvr.GetDataKeyValue("FLDGLOBALCOMPONENTJOBID").ToString()));
                            ucStatus.Text = "Added Successfully...";
                            ucStatus.Visible = true;

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                        }
                    }
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

    protected void RoutineWo()
    {
        try
        {
            string ComponentJob = string.Empty;
            foreach (GridDataItem gvr in gvPlannedMaintenanceJob.Items)
            {
                if (((RadCheckBox)(gvr.FindControl("ChkComJob"))).Checked == true)
                {
                    ComponentJob += gvr.GetDataKeyValue("FLDGLOBALCOMPONENTJOBID").ToString() + ',';
                }
            }

            PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkorderDetailsinsert
                     (new Guid(ViewState["ROUTINEWOID"].ToString()), ComponentJob);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('List', 'Filters');", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
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
            e.Node.Attributes["CATEGORY"] = drv["FLDCATEGORY"].ToString();
            e.Node.Attributes["TYPEREQUIREDYN"] = drv["FLDTYPEREQUIREDYN"].ToString();
            e.Node.Attributes["INCREMENTTYPE"] = drv["FLDUNITINCREMENTTYPE"].ToString();
            e.Node.Attributes["OPERATIONAL"] = drv["FLDOPERATIONALCRITICALYN"].ToString();
            e.Node.Attributes["ENVIRONMENTAL"] = drv["FLDENVIRONMENTALCRITICALYN"].ToString();
            e.Node.Attributes["SHOWFMS"] = drv["FLDSHOWINFMS"].ToString();
            e.Node.Attributes["ISVIR"] = drv["FLDISVIR"].ToString();
            e.Node.Attributes["ISLOCATION"] = drv["FLDISLOCATION"].ToString();
            if (ViewState["COMPONENTID"] != null && ViewState["COMPONENTID"].ToString() == drv["FLDGLOBALCOMPONENTID"].ToString())
                e.Node.Selected = true;
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

                RadTreeNode parent = e.Node.ParentNode;
                gvPlannedMaintenanceJob.Rebind();

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
                // LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                // if (db != null)
                // {
                //     db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                // }
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
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (string.IsNullOrEmpty(ViewState["JOBSUMMARYID"].ToString()))
                {
                    return;
                }
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceCompJobSummary.GlobalCompJobInsert(new Guid(ViewState["JOBSUMMARYID"].ToString()), new Guid(item.GetDataKeyValue("FLDGLOBALCOMPONENTJOBID").ToString()));
                ucStatus.Text = "Added Successfully...";
                ucStatus.Visible = true;

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList(null,null,1);";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
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
    protected void gvPlannedMaintenanceJob_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        ViewState["SORTDIRECTION"] = ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0" ? "1" : "0";
    }
}