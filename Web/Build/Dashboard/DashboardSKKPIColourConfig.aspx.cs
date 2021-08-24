using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardSKKPIColourConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;

            gvKPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["KPIID"] = General.GetNullableGuid(Request.QueryString["shippingkpiid"].ToString());
            Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());

            DataTable dt = PheonixDashboardSKKPI.KPIEditList(kpiid);

            Radlblkpicode.Text = dt.Rows[0]["FLDKPICODE"].ToString();
            Radlblkpiname.Text = dt.Rows[0]["FLDKPINAME"].ToString();
        }
    }

    protected void gvKPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKKPI.KPIColorConfigSearch(kpiid, gvKPIlist.CurrentPageIndex + 1, gvKPIlist.PageSize, ref iRowCount, ref iTotalPageCount);

        gvKPIlist.DataSource = dt;
        gvKPIlist.VirtualItemCount = iRowCount;


    }

    protected void gvKPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)fi.FindControl("Radtbminimumentry");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)fi.FindControl("Radtbmaximumentry");
                RadColorPicker colourpicker = (RadColorPicker)fi.FindControl("radkpicolourpicker");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidKPIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }
                PheonixDashboardSKKPI.KPIColorConfigInsert(rowusercode, kpiid, minimumvalue, maximumvalue, colour);

                gvKPIlist.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)ei.FindControl("Radtbminimumedit");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)ei.FindControl("Radtbmaximumedit");
                RadColorPicker colourpicker = (RadColorPicker)ei.FindControl("radkpicolourpickeredit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? kpicolourid = General.GetNullableGuid(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDKPICOLOURID"].ToString());

                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidKPIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIColorConfigUpdate(rowusercode, kpicolourid, minimumvalue, maximumvalue, colour);

                gvKPIlist.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvKPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    private bool IsValidKPIColourConfiguration(int? minimumvalue, int? maximumvalue, string colour)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (minimumvalue == null)
        {
            ucError.ErrorMessage = "Minimum Value.";
        }
        if (maximumvalue == null)
        {
            ucError.ErrorMessage = "Maximum Value.";
        }
        if (colour == null)
        {
            ucError.ErrorMessage = "Colour.";
        }

        if (minimumvalue != null && maximumvalue != null && colour != null)
        {
            if (minimumvalue < 0 || maximumvalue < 0)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = "Minimum and Maximum Values cannot be negative.";
            }
            if (!(maximumvalue > minimumvalue))
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = "Maximum Value Should be Greater than Minimum Value";
            }
        }
        return (!ucError.IsError);
    }
}