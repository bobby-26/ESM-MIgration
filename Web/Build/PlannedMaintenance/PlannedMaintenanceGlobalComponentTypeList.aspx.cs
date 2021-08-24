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
using System.Collections.Specialized;
public partial class PlannedMaintenanceGlobalComponentTypeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["COMPONENTTYPEID"] = "";
            gvPlannedMaintenanceJob.PageSize = General.ShowRecords(null);

            if(Request.QueryString["LAUNCHFROM"]!=null && Request.QueryString["LAUNCHFROM"].ToString().ToUpper() == "MODEL")
            {
                NameValueCollection nvc = new NameValueCollection();
                if (Request.QueryString["COMPONENTNUMBER"] != null)
                {
                    if (nvc.Get("COMPONENTNUMBER") != null)
                        nvc.Remove("COMPONENTNUMBER");

                    nvc.Add("COMPONENTNUMBER", Request.QueryString["COMPONENTNUMBER"].ToString());
                }
                if (Request.QueryString["MODEL"] != null)
                {
                    if (nvc.Get("TYPE") != null)
                        nvc.Remove("TYPE");

                    nvc.Add("TYPE", Request.QueryString["MODEL"].ToString());
                }
                Filter.CurrentGlobalComponentTypeFilter = nvc;
            }
            

        }
        
    }
    protected void gvPlannedMaintenanceJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDMAKE","FLDTYPE","FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCATEGORYNAME"};
            string[] alCaptions = { "Make", "Model/Type", "Component Number", "Component Name","Category" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            DataSet ds;
            NameValueCollection nvc = new NameValueCollection();
            if (Filter.CurrentGlobalComponentTypeFilter != null)
                nvc = Filter.CurrentGlobalComponentTypeFilter;

            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeFilterSearch(
                                                           nvc.Get("TYPE") !=null?General.GetNullableString(nvc.Get("TYPE").ToString()):null,
                                                           nvc.Get("MAKE") != null ? General.GetNullableString(nvc.Get("MAKE").ToString()) : null,
                                                           nvc.Get("COMPONENTNUMBER") != null ? General.GetNullableString(nvc.Get("COMPONENTNUMBER").ToString()) : null,
                                                           nvc.Get("COMPONENTNAME") != null ? General.GetNullableString(nvc.Get("COMPONENTNAME").ToString()) : null,
                                                           sortexpression, sortdirection,
                                                           gvPlannedMaintenanceJob.CurrentPageIndex + 1,
                                                           gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount);

            gvPlannedMaintenanceJob.DataSource = ds;
            gvPlannedMaintenanceJob.VirtualItemCount = iRowCount;

            if (Filter.CurrentGlobalComponentTypeFilter != null)
            {
                foreach (GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
                {
                    c.CurrentFilterValue = nvc.Get(c.UniqueName) != null ? nvc.Get(c.UniqueName).ToString() : "";
                }
            }

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
                LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null)
                {
                    cmdDelete.Visible= SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                    cmdDelete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                }
                LinkButton cmdJobs = (LinkButton)e.Item.FindControl("cmdJobs");
                
                if (cmdJobs != null)
                {
                    cmdJobs.Visible = SessionUtil.CanAccess(this.ViewState, cmdJobs.CommandName);
                    cmdJobs.Attributes.Add("onclick", "javascript:return openNewWindow('Jobs', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJob.aspx?COMPONENTNUMBER=" + drv["FLDCOMPONENTNUMBER"].ToString() + "&MODEL=" + drv["FLDTYPE"].ToString() + "&MAKE="+ drv["FLDMAKE"].ToString() + "&LAUNCHFROM=MODEL','true'); return false;");
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdManuals");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "javascript:return openNewWindow('Manuals', '', 'PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?COMPONENTJOBID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEID").ToString()+ "'); return false;");
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
            if(e.CommandName == RadGrid.FilterCommandName)
            {
                NameValueCollection nvc = new NameValueCollection();
                foreach(GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
                {
                    nvc.Remove(c.CurrentFilterValue.ToString());
                    nvc.Add(c.UniqueName, c.CurrentFilterValue.ToString());
                }
                Filter.CurrentGlobalComponentTypeFilter = nvc;
                gvPlannedMaintenanceJob.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeDelte(new Guid(item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEID").ToString()));
                gvPlannedMaintenanceJob.Rebind();
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
        ViewState["SORTEXPRESSION"] = e.CommandArgument.ToString();
        ViewState["SORTDIRECTION"] = ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0" ? "1" : "0";
        gvPlannedMaintenanceJob.Rebind();
    }


    protected void gvPlannedMaintenanceJob_PreRender(object sender, EventArgs e)
    {
    }

    protected void gvcMenu_ItemClick(object sender, RadMenuEventArgs e)
    {
        int radGridClickedRowIndex;

        radGridClickedRowIndex = Convert.ToInt32(Request.Form["radGridClickedRowIndex"]);
        int index = 0;
        switch (e.Item.Value)
        {
            case "Edit":
                index = 0;
                break;
            case "History":
                index = 1;
                break;
            case "Include":
                index = 2;
                break;
            case "Manuals":
                index = 3;
                break;
        }

        GridDataItem item = gvPlannedMaintenanceJob.Items[radGridClickedRowIndex];
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += " setTimeout(function(){ openNewWindow('Component', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJobEdit.aspx?GLOBALCOMPONENTTYPEMAPID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEJOBMAPID").ToString() + "&TABINDEX="+index+"')},1000)";
        Script += "</script>" + "\n";

        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    }
}