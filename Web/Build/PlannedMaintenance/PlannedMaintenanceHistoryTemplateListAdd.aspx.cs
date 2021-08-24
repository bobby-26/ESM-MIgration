using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceHistoryTemplateListAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateListAdd.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateListAdd.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddButton("Back", "COMPONENT", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                //ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                gvHistoryTemplateList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
            else if (CommandName.ToUpper().Equals("COMPONENT"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]);
                gvHistoryTemplateList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                ReportConfirm();
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

    private bool IsValidHistoryTemplate(string Componentjobid)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (Componentjobid == null || Componentjobid == "")
        {
            ucError.ErrorMessage = "Please select a Job";
        }

        return (!ucError.IsError);
    }
    private void ReportConfirm()
    {
        foreach (GridDataItem item in gvHistoryTemplateList.Items)
        {
            string formid = ((RadLabel)item.FindControl("lblFormID")).Text;
            string Componentjobid = ViewState["COMPONENTJOBID"].ToString();
            if (Request.QueryString["COMPONENTJOBID"] != null)
            {
                Componentjobid = Request.QueryString["COMPONENTJOBID"].ToString();
            }
            RadCheckBox chkitem = (RadCheckBox)item.FindControl("chkSelect");

            string verifiedyn = chkitem.Checked == true ? "1" : "0";
            if (chkitem.Checked.HasValue == true && chkitem.Checked.Value)
            {
                 PhoenixPlannedMaintenanceHistoryTemplate.PMSHistoryTemplateInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(formid)
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , new Guid(Componentjobid));
            }

           
            gvHistoryTemplateList.Rebind();
        }
        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateList.aspx?COMPONENTJOBID=" + ViewState["COMPONENTJOBID"]);
    }

    protected void gvHistoryTemplateList_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
       

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


            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.PMSHistoryTemplateSearch(txtTemplateName.Text
                                                                                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID
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
            DataRowView drv = (DataRowView)e.Item.DataItem;

           

            LinkButton templ = (LinkButton)e.Item.FindControl("lnkTemplateName");
            if (templ != null)
            {
                templ.Visible = SessionUtil.CanAccess(this.ViewState, templ.CommandName);
                templ.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryExcelTemplate.aspx?fid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }

            

        }
    }
}