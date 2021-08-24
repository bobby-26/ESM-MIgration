using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;


public partial class OptionsChooseVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gvVessel.DataSource = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, General.GetNullableString(txtVesselName.Text));
        gvVessel.DataBind();
    }

    protected void OptionChooseVessel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "showVessel();";
            Script += "</script>" + "\n";

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void txtVesselName_TextChanged(object sender, EventArgs e)
    {

    }

    protected void gvVessel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("CHOOSEVESSEL"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "showVessel();";
                Script += "</script>" + "\n";

                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(((Label)_gridview.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblVesselID")).Text);
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = ((Label)_gridview.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblVesselName")).Text;
                Filter.CurrentOrderFormFilterCriteria = null;

                Filter.CurrentPurchaseStockType = null;
                Filter.CurrentPurchaseStockClass = null;
                Filter.CurrentPurchaseOrderIdSelection = null;

                Filter.CurrentNoonReportListFilter = null;
                Filter.CurrentArrivalReportFilter = null;
                Filter.CurrentDepartureReportFilter = null;
                Filter.CurrentShiftingReportFilter = null;
                Filter.CurrentVesselPositionReportFilter = null;
                Filter.CurrentVPRSVoyageFilter = null;

                ucStatus.Text = "Current Vessel: " + PhoenixSecurityContext.CurrentSecurityContext.VesselName;

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
                SessionUtil.ReBuildMenu();  

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

}
