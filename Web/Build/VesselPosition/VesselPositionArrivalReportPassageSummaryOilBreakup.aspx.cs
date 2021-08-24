using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselPositionArrivalReportPassageSummaryOilBreakup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("Re-Calculate", "RECALCULATE",ToolBarDirection.Right);
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbartap.Show();

            if (!IsPostBack)
            {
                EditData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RECALCULATE"))
        {
            PhoenixVesselPositionArrivalReport.UpdateArrivalReportPassageSummaryOil(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                            , int.Parse(Request.QueryString["vesselid"])
                                                                                            , General.GetNullableGuid(Request.QueryString["oiltype"]));
            ucStatus.Text = "Re-Calculated";

            String script = "javascript:fnReloadList('codehelp1');";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void EditData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryOilBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));

        if (ds.Tables[2].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[2].Rows[0];
            ucTotalCons.Text = dr["FLDTOTALCONS"].ToString();
            ucAvgConsPerDay.Text = dr["FLDAVGCONSPERDAY"].ToString();
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvROBCOSP_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryOilBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvROBCOSP.DataSource = ds.Tables[0];
    }

    protected void gvROBFWE_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryOilBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvROBFWE.DataSource = ds.Tables[1];
    }
}
