using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;

public partial class Dashboard_DashboardBSCSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Draft", "Draft", ToolBarDirection.Left);
        toolbar.AddButton("Score Cards", "SC", ToolBarDirection.Left);

        TabstripMenu.MenuList = toolbar.Show();
        TabstripMenu.SelectedMenuIndex = 1;
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardBSCSearch.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        TabScorecard.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {

            bingkpidropdown();
            radcbmonth.SelectedMonth = DateTime.Now.Month.ToString();
            radcbyear.SelectedYear = DateTime.Now.Year;

            DataTable dt = PheonixDashboardSKKPI.Departmentlist();

            radcbdept.DataSource = dt;
            radcbdept.DataTextField = "FLDDEPARTMENTNAME";
            radcbdept.DataValueField = "FLDDEPARTMENTID";
            radcbdept.DataBind();

            SessionUtil.PageAccessRights(this.ViewState);
            gvKPISC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }
    protected void bingkpidropdown()
    {
        DataTable dt = PheonixDashboardSKKPI.KPIList(RadRadioButtonkpilevel.SelectedValue, General.GetNullableInteger(radcbdept.SelectedValue));
        RadComboKpilist.DataSource = dt;
        RadComboKpilist.DataBind();
        RadComboKpilist.Text = string.Empty;
        RadComboKpilist.Value = null;

    }
    protected void radcbdept_TextChanged(object sender, EventArgs e)
    {
        bingkpidropdown();
    }
    protected void RadRadioButtonkpilevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bingkpidropdown();
    }
    protected void TabScorecard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                gvKPISC.Rebind();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void TabstripMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DRAFT"))
            {
                Response.Redirect("~/Dashboard/DashboardBSCKPI.aspx");
            }
            if (CommandName.ToUpper().Equals("SC"))
            {

                Response.Redirect("~/Dashboard/DashboardBSCSearch.aspx");
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvKPISC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSC.KPISCSearch(RadRadioButtonkpilevel.SelectedValue, General.GetNullableGuid(RadComboKpilist.Value), General.GetNullableInteger(radcbmonth.SelectedMonth),General.GetNullableInteger(radcbyear.SelectedYear.ToString()), gvKPISC.CurrentPageIndex + 1,
                                               gvKPISC.PageSize,
                                               ref iRowCount,
                                               ref iTotalPageCount);
        gvKPISC.DataSource = dt;
        gvKPISC.VirtualItemCount = iRowCount;
    }

    protected void gvKPISC_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvKPISC_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? scorecardid = General.GetNullableGuid(item.GetDataKeyValue("FLDKPISCID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit Score Card','Dashboard/DashboardBSCEdit.aspx?scorecardid=" + scorecardid + "','false','1000px','450px');return false");

                }

                LinkButton name = ((LinkButton)item.FindControl("nameanchor"));
                if (name != null)
                {
                    name.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','KPI Score Card ','Dashboard/DashboardBSCView.aspx?scorecardid=" + scorecardid + "','false','1000px','450px');return false");

                }

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

            gvKPISC.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}