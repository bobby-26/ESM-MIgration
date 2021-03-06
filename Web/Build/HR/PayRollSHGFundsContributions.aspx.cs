using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSHGFundsContributions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddFontAwesomeButton("../HR/PayRollSHGFundsContributions.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        gvmenu.MenuList = menu.Show();
        if (!IsPostBack)
        {
            ddlyear.SelectedYear = DateTime.Now.Year;
            gvetf.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            Loaddata();
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
    protected void Loaddata()
    {
        DataTable dt = PhoenixPayRollSingapore.EmployeeList();
        ddlemployee.DataSource = dt;
        if (dt.Rows.Count > 0)
        {
            ddlemployee.DataTextField = "FLDEMPLOYEECODE";
            ddlemployee.DataValueField = "FLDEMPLOYEEID";
            ddlemployee.DataBind();
        }

        DataTable dt1 = PhoenixPayRollSingapore.SHGFundList();
        if (dt1.Rows.Count > 0)
        {
            ddlfund.DataSource = dt1;
            ddlfund.DataTextField = "FLDETHNICFUNDSHORTCODE";
            ddlfund.DataValueField = "FLDSGPRETHNICFUNDID";
            ddlfund.DataBind();


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
                gvetf.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvetf_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvetf.CurrentPageIndex + 1;


        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPayRollSingapore.SHGFundContributionSearch(General.GetNullableInteger(ddlyear.SelectedYear.ToString())
                                                                   , General.GetNullableInteger(ddlmonth.SelectedMonth)
                                                                   , General.GetNullableInteger(ddlvessellist.SelectedVessel)
                                                                   , General.GetNullableInteger(ddlemployee.SelectedValue)
                                                                   , (int)ViewState["PAGENUMBER"], gvetf.PageSize, ref iRowCount, ref iTotalPageCount
                                                                   ,General.GetNullableGuid(ddlfund.SelectedValue));
        gvetf.DataSource = dt;
        gvetf.VirtualItemCount = iRowCount;

    }

    protected void gvetf_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvetf_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBER"] = null;

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

    
