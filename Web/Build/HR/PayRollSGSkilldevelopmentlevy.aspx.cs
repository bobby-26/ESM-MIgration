using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGSkilldevelopmentlevy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddFontAwesomeButton("../HR/PayRollSGSkilldevelopmentlevy.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        gvmenu.MenuList = menu.Show();
        if (!IsPostBack)
        {
            ddlyear.SelectedYear = DateTime.Now.Year;
            gvsdl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if ((DateTime.Now.Month != 1))
            {

                ddlmonth.SelectedMonth = General.GetNullableString((DateTime.Now.Month - 1).ToString());
            }
            if ((DateTime.Now.Month == 1))
            {
                ddlyear.SelectedYear = DateTime.Now.Year - 1;
                ddlmonth.SelectedMonth = "12";

            }
        }
    }
    protected void gvsdl_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvsdl.CurrentPageIndex + 1;


        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPayRollSingapore.SDLContributionSearch(General.GetNullableInteger(ddlyear.SelectedYear.ToString())
                                                                   , General.GetNullableInteger(ddlmonth.SelectedMonth)
                                                                   , General.GetNullableInteger(ddlvessellist.SelectedVessel)
                                                                   , (int)ViewState["PAGENUMBER"], gvsdl.PageSize, ref iRowCount, ref iTotalPageCount);
        gvsdl.DataSource = dt;
        gvsdl.VirtualItemCount = iRowCount;
    }

    protected void gvsdl_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvsdl_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                gvsdl.Rebind();
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