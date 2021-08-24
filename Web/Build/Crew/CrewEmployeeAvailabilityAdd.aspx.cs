using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewEmployeeAvailabilityAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewEmployeeAvailabilityFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewEmployeeAvailabilityAdd.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuSearfarereList.AccessRights = this.ViewState;
            MenuSearfarereList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentEmployeeAvailabilitySelection;

        try
        {
            DataTable dt = PhoenixCrewEmployeeAvailableDates.CrewEmployeeAvailabilityDateSearch(General.GetNullableInteger(nvc != null ? nvc.Get("empId") : string.Empty)
                                                                                                , General.GetNullableString(nvc != null ? nvc.Get("fileNo") : string.Empty)
                                                                                                , General.GetNullableInteger(nvc != null ? nvc.Get("activeyn") : string.Empty)
                                                                                                , General.GetNullableInteger(nvc != null ? nvc.Get("newApp") : string.Empty)
                                                                                                , General.GetNullableString(nvc != null ? nvc.Get("empName") : string.Empty)
                                                                                                , General.GetNullableInteger(nvc != null ? nvc.Get("zoneId") : string.Empty)
                                                                                                , General.GetNullableInteger(nvc != null ? nvc.Get("rankId") : string.Empty)
                                                                                                , sortexpression
                                                                                                , sortdirection
                                                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                                , gvCrewSearch.PageSize
                                                                                                , ref iRowCount
                                                                                                , ref iTotalPageCount);
            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.VirtualItemCount = iRowCount;                
            }
            else
            {
                gvCrewSearch.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSearfarereList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {            
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentEmployeeAvailabilitySelection.Clear();              
                BindData();
                gvCrewSearch.Rebind();
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
        ViewState["PAGENUMBER"] = 1;      
        BindData();
        gvCrewSearch.Rebind();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "AVAILABILITY")
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadLabel lblEmpId = ((RadLabel)eeditedItem.FindControl("lblEmployeeid"));

            string empid = lblEmpId.Text;

            Response.Redirect("../Crew/CrewEmployeeAvailableDate.aspx?empid=" + empid + "&from=AVAILABILITYADD", false);

            BindData();
            gvCrewSearch.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}