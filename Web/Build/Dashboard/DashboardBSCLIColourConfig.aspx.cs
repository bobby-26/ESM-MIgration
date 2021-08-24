using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardBSCLIColourConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvLIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["LIID"] = General.GetNullableGuid(Request.QueryString["shippingliid"].ToString());
            Guid? liid = General.GetNullableGuid(ViewState["LIID"].ToString());

            DataTable dt = PhoenixDashboardBSCLI.LIEditList(liid);

            Radlbllicode.Text = dt.Rows[0]["FLDLICODE"].ToString();
            Radlblliname.Text = dt.Rows[0]["FLDLINAME"].ToString();
        }
    }

    protected void gvLIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? liid = General.GetNullableGuid(ViewState["LIID"].ToString());

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSCLI.LIColorConfigSearch(liid, gvLIlist.CurrentPageIndex + 1, gvLIlist.PageSize, ref iRowCount, ref iTotalPageCount);

        gvLIlist.DataSource = dt;
        gvLIlist.VirtualItemCount = iRowCount;
    }

    protected void gvLIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

    }

    protected void gvLIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)fi.FindControl("Radtbminimumentry");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)fi.FindControl("Radtbmaximumentry");
                RadColorPicker colourpicker = (RadColorPicker)fi.FindControl("radlicolourpicker");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? liid = General.GetNullableGuid(ViewState["LIID"].ToString());
                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidLIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDashboardBSCLI.LIColorConfigInsert(rowusercode, liid, minimumvalue, maximumvalue, colour);

                gvLIlist.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)ei.FindControl("Radtbminimumedit");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)ei.FindControl("Radtbmaximumedit");
                RadColorPicker colourpicker = (RadColorPicker)ei.FindControl("radlicolourpickeredit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? licolourid = General.GetNullableGuid(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDLICOLOURID"].ToString());

                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidLIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSCLI.LIColorConfigUpdate(rowusercode, licolourid, minimumvalue, maximumvalue, colour);

                gvLIlist.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidLIColourConfiguration(int? minimumvalue, int? maximumvalue, string colour)
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