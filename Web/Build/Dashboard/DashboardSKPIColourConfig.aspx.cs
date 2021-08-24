using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardSKPIColourConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;

            gvPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PIID"] = General.GetNullableGuid(Request.QueryString["shippingpiid"].ToString());
            Guid? piid = General.GetNullableGuid(ViewState["PIID"].ToString());

            DataTable dt = PheonixDashboardSKPI.PIEditList(piid);

            Radlblpicode.Text = dt.Rows[0]["FLDPICODE"].ToString();
            Radlblpiname.Text = dt.Rows[0]["FLDPINAME"].ToString();
        }
    }
    protected void gvPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? piid = General.GetNullableGuid(ViewState["PIID"].ToString());


        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKPI.PIColorConfigSearch(piid, gvPIlist.CurrentPageIndex + 1, gvPIlist.PageSize, ref iRowCount, ref iTotalPageCount);

        gvPIlist.DataSource = dt;
        gvPIlist.VirtualItemCount = iRowCount;


    }

    protected void gvPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)fi.FindControl("Radtbminimumentry");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)fi.FindControl("Radtbmaximumentry");
                RadColorPicker colourpicker = (RadColorPicker)fi.FindControl("radpicolourpicker");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? piid = General.GetNullableGuid(ViewState["PIID"].ToString());
                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidPIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }
                PheonixDashboardSKPI.PIColorConfigInsert(rowusercode, piid, minimumvalue, maximumvalue, colour);

                gvPIlist.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)ei.FindControl("Radtbminimumedit");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)ei.FindControl("Radtbmaximumedit");
                RadColorPicker colourpicker = (RadColorPicker)ei.FindControl("radpicolourpickeredit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? picolourid = General.GetNullableGuid(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDPICOLOURID"].ToString());

                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidPIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKPI.PIColorConfigUpdate(rowusercode, picolourid, minimumvalue, maximumvalue, colour);

                gvPIlist.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    private bool IsValidPIColourConfiguration(int? minimumvalue, int? maximumvalue, string colour)
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