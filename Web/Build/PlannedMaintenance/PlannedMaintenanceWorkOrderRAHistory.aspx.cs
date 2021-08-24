using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderRAHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarRA = new PhoenixToolbar();
        toolbarRA.AddFontAwesomeButton("", "Map Risk Assessment", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuRA.AccessRights = this.ViewState;
        MenuRA.MenuList = toolbarRA.Show();

        if (!IsPostBack)
        {
            ViewState["COMPONENTID"] = null;
            if (Request.QueryString["COMPONENTID"] != null)
                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
        }
        //BindData();
    }
    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReschedule.ListWorkOrderRAHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableGuid(ViewState["COMPONENTID"].ToString()));
            gvWRes.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void SetRADetail(ImageButton cmdRA, string raid, string ratype)
    {
        cmdRA.Visible = false;
        if (raid != string.Empty)
        {
            cmdRA.Visible = true;
            if (ratype == "1")
                //cmdRA.Attributes.Add("onclick", "parent.Openpopup('RAProcess', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + raid + "&showmenu=0&showword=NO&showexcel=NO');return true;");
                cmdRA.Attributes.Add("onclick", "javascript:openNewWindow('RAProcess','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + raid + "&showmenu=0&showword=NO&showexcel=NO'); return true;");
            else if (ratype == "2")
                //cmdRA.Attributes.Add("onclick", "parent.Openpopup('RAGeneric', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + raid + "&showmenu=0&showexcel=NO');return true;");
                cmdRA.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + raid + "&showmenu=0&showexcel=NO'); return true;");
            else if (ratype == "3")
                //cmdRA.Attributes.Add("onclick", "parent.Openpopup('RAMachinery', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + raid + "&showmenu=0&showexcel=NO');return true;");
                cmdRA.Attributes.Add("onclick", "javascript:openNewWindow('RAMachinery','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + raid + "&showmenu=0&showexcel=NO'); return true;");
            else if (ratype == "4")
                //cmdRA.Attributes.Add("onclick", "parent.Openpopup('RANavigation', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + raid + "&showmenu=0&showexcel=NO');return true;");
                cmdRA.Attributes.Add("onclick", "javascript:openNewWindow('RANavigation','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&machineryid=" + raid + "&showmenu=0&showexcel=NO'); return true;");
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvWRes_ItemCommand(object sender, GridCommandEventArgs e)
    {
      
    }
    protected void gvWRes_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void gvWRes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReschedule.ListWorkOrderRAHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableGuid(ViewState["COMPONENTID"].ToString()));
            gvWRes.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvWRes_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            ImageButton cmdRA = (ImageButton)e.Item.FindControl("cmdRA");
            if (cmdRA != null)
            {
                RadLabel Raid = (RadLabel)e.Item.FindControl("lblRaid");
                RadLabel Type = (RadLabel)e.Item.FindControl("lblType");
                SetRADetail(cmdRA, Raid.Text, Type.Text);
            }
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
                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentRAMapping.aspx?GLOBALCOMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "');");
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

            ds = PhoenixPlannedMaintenanceGlobalComponent.VesselComponentRASearch(General.GetNullableGuid(ViewState["COMPONENTID"].ToString()),
                                                           sortexpression, sortdirection,
                                                           gvRA.CurrentPageIndex + 1,
                                                           gvRA.PageSize, ref iRowCount, ref iTotalPageCount, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRA.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}