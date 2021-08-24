using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
public partial class PlannedMaintenanceGlobalJobAdd : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            MenuPlannedMaintenance.AccessRights = this.ViewState;
            MenuPlannedMaintenance.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["GLOBALCOMPONENTID"] = "";



                if (Request.QueryString["GLOBALCOMPONENTID"] != null && Request.QueryString["GLOBALCOMPONENTID"].ToString() != "")
                {
                    ViewState["GLOBALCOMPONENTID"] = Request.QueryString["GLOBALCOMPONENTID"].ToString();
                }
                gvPlannedMaintenanceJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PlannedMaintenance_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                gvPlannedMaintenanceJob.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
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
            NameValueCollection nvc = Filter.CurrentJobFilter;
            DataSet ds;

            ds = PhoenixPlannedMaintenanceGlobalComponent.JobSearch(General.GetNullableGuid(ViewState["GLOBALCOMPONENTID"].ToString()),
                                                           General.GetNullableString(txtNumber.Text),
                                                           General.GetNullableString(txtName.Text),
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
            if(e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (General.GetNullableGuid(drv["FLDGLOBALCOMPONENTJOBID"].ToString()) == null)
                        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    else
                        db.Visible = false;
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
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentJobInsert(new Guid(ViewState["GLOBALCOMPONENTID"].ToString()), new Guid(((RadLabel)item.FindControl("lblJobid")).Text));
                gvPlannedMaintenanceJob.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
