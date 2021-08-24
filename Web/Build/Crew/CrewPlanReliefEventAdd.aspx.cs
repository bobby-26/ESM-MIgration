using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;
public partial class CrewPlanReliefEventAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                }
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewChangeEvent.CrewReliefPlanEventSearch(
                                                                   General.GetNullableInteger(ViewState["Vesselid"].ToString())                                                                
                                                                  , sortexpression, sortdirection,
                                                                   int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                   gvCCPlan.PageSize,
                                                                   ref iRowCount,
                                                                   ref iTotalPageCount);


            gvCCPlan.DataSource = ds;
            gvCCPlan.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "MAPEVENT")
            {

                string lblEventId = ((RadLabel)e.Item.FindControl("lblcreweventid")).Text;
                
                NameValueCollection nvc = Filter.CurrentCrewReliefPlanEventFilter;
                
                if (nvc != null)
                {

                    string strcrewplanlist = General.GetNullableString(nvc != null ? nvc.Get("crewplanlist") : string.Empty);
                    string signonoffiflist = General.GetNullableString(nvc != null ? nvc.Get("signonoffiflist") : string.Empty);

                    PhoenixCrewChangeEventDetail.CrewReliefPlanAdd(
                      General.GetNullableGuid(lblEventId)
                     , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                     , strcrewplanlist
                     , signonoffiflist
                    );

                    Filter.CurrentCrewReliefPlanEventFilter = null;
                    
                    gvCCPlan.Rebind();

                    String script = String.Format("javascript:fnReloadList('codehelp2','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }

            }            
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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

    }

    protected void gvCCPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            
            LinkButton MapEvent = (LinkButton)item.FindControl("cmdMapEvent");
            if (MapEvent != null)
            {
                MapEvent.Visible = SessionUtil.CanAccess(this.ViewState, MapEvent.CommandName);
                MapEvent.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to Map this event?')");
            }


        }
    }
}