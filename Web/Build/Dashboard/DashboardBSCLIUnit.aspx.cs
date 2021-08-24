using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardBSCLIUnit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKKPIUnit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLIUnit')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripMenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);


            gvLIUnit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void LIUnit_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void ShowExcel()
    { }
    protected void gvLIUnit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSCLI.LIUnitSearch(gvLIUnit.CurrentPageIndex + 1,
                                                gvLIUnit.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvLIUnit.DataSource = dt;
        gvLIUnit.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDUNITCODE", "FLDUNIT" };
        string[] alCaptions = { "Unit Code", "Unit" };
        General.SetPrintOptions("gvLIUnit", "KPI Units", alCaptions, alColumns, ds);
    }

    protected void gvLIUnit_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    public void gvLIUnit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                RadTextBox unitcodeentry = (RadTextBox)fi.FindControl("Radliunitcodeentry");
                RadTextBox unitname = (RadTextBox)fi.FindControl("Radliunitnameentry");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string unitcode = General.GetNullableString(unitcodeentry.Text);
                string unit = General.GetNullableString(unitname.Text);

                if (!IsValidShippingLIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSCLI.LIUnitInsert(rowusercode, unitcode, unit);
                gvLIUnit.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                RadTextBox unitcodeentry = (RadTextBox)ei.FindControl("Radliunitcodeedit");
                RadTextBox unitname = (RadTextBox)ei.FindControl("Radliunitnameedit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string unitcode = General.GetNullableString(unitcodeentry.Text);
                string unit = General.GetNullableString(unitname.Text);

                if (!IsValidShippingLIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                int? liunitid = General.GetNullableInteger(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDUNITID"].ToString());

                PhoenixDashboardBSCLI.LIUnitUpdate(rowusercode, unitcode, unit, liunitid);

                gvLIUnit.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private bool IsValidShippingLIUnitDetails(string unitcode, string unit)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (unitcode == null)
        {
            ucError.ErrorMessage = "Unit Code.";
        }
        if (unit == null)
        {
            ucError.ErrorMessage = "Unit Name.";
        }

        return (!ucError.IsError);
    }

}