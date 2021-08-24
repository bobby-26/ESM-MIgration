using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardSKSPIColourConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvSPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["SPIID"] = General.GetNullableGuid(Request.QueryString["shippingspiid"].ToString());
            Guid? spiid = General.GetNullableGuid(ViewState["SPIID"].ToString());

            DataTable dt = PheonixDashboardSKSPI.SPIEditlist(spiid);

            Radlblspicode.Text = dt.Rows[0]["FLDSPIID"].ToString();
            Radlblspiname.Text = dt.Rows[0]["FLDSPITITLE"].ToString();
        }
    }

    protected void gvSPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? spiid = General.GetNullableGuid(ViewState["SPIID"].ToString());

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKSPI.SPIColorConfigSearch(spiid , gvSPIlist.CurrentPageIndex + 1, gvSPIlist.PageSize,ref iRowCount, ref iTotalPageCount);

        gvSPIlist.DataSource = dt;
        gvSPIlist.VirtualItemCount = iRowCount;


    }

    protected void gvSPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)fi.FindControl("Radtbminimumentry");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)fi.FindControl("Radtbmaximumentry");
                RadColorPicker colourpicker = (RadColorPicker)fi.FindControl("radspicolourpicker");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? spiid = General.GetNullableGuid(ViewState["SPIID"].ToString());
                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidSPIColourConfiguration(minimumvalue, maximumvalue, colour))
                {
                    ucError.Visible = true;
                    return;
                }
                PheonixDashboardSKSPI.SPIColorConfigInsert(rowusercode, spiid, minimumvalue, maximumvalue, colour);

                gvSPIlist.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                UserControlDecimal minimumvaluetb = (UserControlDecimal)ei.FindControl("Radtbminimumedit");
                UserControlDecimal maximumvaluetb = (UserControlDecimal)ei.FindControl("Radtbmaximumedit");
                RadColorPicker colourpicker = (RadColorPicker)ei.FindControl("radspicolourpickeredit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? spicolourid = General.GetNullableGuid(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDSPICOLOURID"].ToString());

                int? minimumvalue = General.GetNullableInteger(minimumvaluetb.Text);
                int? maximumvalue = General.GetNullableInteger(maximumvaluetb.Text);
                string colour = General.GetNullableString(System.Drawing.ColorTranslator.ToHtml(colourpicker.SelectedColor).ToString());

                if (!IsValidSPIColourConfiguration(minimumvalue,maximumvalue,colour))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKSPI.SPIColorConfigUpdate(rowusercode, spicolourid, minimumvalue, maximumvalue, colour);

                gvSPIlist.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvSPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    private bool IsValidSPIColourConfiguration(int? minimumvalue , int? maximumvalue , string colour)
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

        if (minimumvalue != null && maximumvalue != null && colour!=null)
        {
            if (minimumvalue<0 || maximumvalue <0)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = "Minimum and Maximum Values cannot be negative.";
            }
            if(!(maximumvalue > minimumvalue))
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = "Maximum Value Should be Greater than Minimum Value";
            }
        }
        return (!ucError.IsError);
    }
  }