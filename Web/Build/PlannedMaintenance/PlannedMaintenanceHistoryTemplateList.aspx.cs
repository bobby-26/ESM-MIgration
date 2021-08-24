using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceHistoryTemplateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
           

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                //ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                gvHistoryTemplateList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["VESSELID"] = null;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (Request.QueryString["vesselid"] != null)
                toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateListAdd.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]+"&vesselid="+ ViewState["VESSELID"], "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            else
                toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateListAdd.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"], "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");


            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void MenuHistoryTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvHistoryTemplateList.CurrentPageIndex = 0;
                gvHistoryTemplateList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtTemplateName.Text = "";
                gvHistoryTemplateList.Rebind();
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
            gvHistoryTemplateList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void gvHistoryTemplateList_ItemCommand(object sender, GridCommandEventArgs e)
    {
       if(e.CommandName.ToUpper().Equals("DELETE"))
        {
            RadLabel formid = ((RadLabel)e.Item.FindControl("lblFormID"));
            string Componentjobid = ViewState["COMPONENTJOBID"].ToString();
            if (Request.QueryString["COMPONENTJOBID"] != null)
            {
                Componentjobid = Request.QueryString["COMPONENTJOBID"].ToString();
            }
            
            PhoenixPlannedMaintenanceHistoryTemplate.MaintenanceFormDelte(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , new Guid(formid.Text)
                                                                              , int.Parse(ViewState["VESSELID"].ToString())
                                                                              , new Guid(Componentjobid));
           
            gvHistoryTemplateList.Rebind();
            
          
        }
       
    }

    protected void gvHistoryTemplateList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? sortdirection = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.PMSComponentJobMaintenanceFormSearch(txtTemplateName.Text
                                                                                             , int.Parse(ViewState["VESSELID"].ToString())
                                                                                             , General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                                             , sortexpression
                                                                                             , sortdirection
                                                                                             , gvHistoryTemplateList.CurrentPageIndex + 1
                                                                                             , gvHistoryTemplateList.PageSize
                                                                                             , ref iRowCount, ref iTotalPageCount);

            gvHistoryTemplateList.DataSource = dt;
            gvHistoryTemplateList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHistoryTemplateList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;

           

            LinkButton templ = (LinkButton)e.Item.FindControl("lnkTemplateName");
            if (templ != null)
            {
                templ.Visible = SessionUtil.CanAccess(this.ViewState, templ.CommandName);
                templ.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryExcelTemplate.aspx?fid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }

           
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
              
          


        }
    }
}


