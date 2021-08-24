using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class HR_PayRollSGForeignWorkerLevy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        if (!IsPostBack)
        {
            ddlyear.SelectedYear = DateTime.Now.Year;
            if ((DateTime.Now.Month != 1))
                {

                ddlmonth.SelectedMonth = General.GetNullableString((DateTime.Now.Month - 1).ToString());
                }
            if ((DateTime.Now.Month == 1))
                 {
                ddlyear.SelectedYear = DateTime.Now.Year -1 ;
                ddlmonth.SelectedMonth = "12";

            }
            gvfwl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["year"] = DateTime.Now.Year; 
            ViewState["month"] = ddlmonth.SelectedMonth;
        }
        Showtoolbar();
       
    }
    public void Showtoolbar()
    {
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddFontAwesomeButton("../HR/PayRollSGForeignWorkerLevy.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        menu.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=21&reportcode=FWL&vid=" + ddlvessellist.SelectedVessel + "&y=" + ViewState["year"] + "&m=" + ViewState["month"] + "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");
        gvmenu.MenuList = menu.Show();
    }
    protected void gvfwl_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvfwl.CurrentPageIndex + 1;


        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPayRollSingapore.FWLContributionSearch(General.GetNullableInteger(ddlyear.SelectedYear.ToString())
                                                                   , General.GetNullableInteger(ddlmonth.SelectedMonth)
                                                                   , General.GetNullableInteger(ddlvessellist.SelectedVessel)
                                                                   , (int)ViewState["PAGENUMBER"], gvfwl.PageSize, ref iRowCount, ref iTotalPageCount);
        gvfwl.DataSource = dt;
        gvfwl.VirtualItemCount = iRowCount;
    }

    protected void gvfwl_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvfwl_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBER"] = null;

        }

    }

    protected void gvmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                gvfwl.Rebind();
                ViewState["year"] =ddlyear.SelectedYear;
                ViewState["month"] = ddlmonth.SelectedMonth;
                Showtoolbar();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (General.GetNullableInteger(ddlmonth.SelectedMonth) == null)
        {
            ucError.ErrorMessage = "Month.";
        }
        if (General.GetNullableInteger(ddlyear.SelectedYear.ToString()) == null)
        {
            ucError.ErrorMessage = "Year.";

        }
        return (!ucError.IsError);


    }
}
