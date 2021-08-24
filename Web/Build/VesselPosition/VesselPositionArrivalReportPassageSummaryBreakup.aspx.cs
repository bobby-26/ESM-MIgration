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

public partial class VesselPositionArrivalReportPassageSummaryBreakup : PhoenixBasePage
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
            PhoenixVesselPositionArrivalReport.UpdateArrivalReportPassageSummary(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(Session["VESSELARRIVALID"].ToString())
                , General.GetNullableInteger(Request.QueryString["vesselid"])
                , null
                , null
                , null);
            
            ucStatus.Text = "Re-Calculated";

            String script = "javascript:fnReloadList('codehelp1');";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void EditData()
    {
        DataSet ds = new DataSet();
        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"]));

        if (ds.Tables[2].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[2].Rows[0];
            ucAvgSpeed.Text = dr["FLDAVGSPEED"].ToString();
            ucAvgSlip.Text = dr["FLDAVGSLIP"].ToString();
            ucAvgRPM.Text = dr["FLDAVGRPM"].ToString();  
        }
    }

    decimal FullSpeedTotal = 0, ReducedSpeedTotal = 0, Stopped = 0, TotalDistanceObserved = 0, TotalEngineDistance = 0;
    protected void gvPassageSummary_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                decimal.TryParse(drv["FLDTOTALFULLSPEED"].ToString(), out FullSpeedTotal);
                decimal.TryParse(drv["FLDTOTALREDUCEDSPEED"].ToString(), out ReducedSpeedTotal);
                decimal.TryParse(drv["FLDTOTALSTOPPED"].ToString(), out Stopped);
                decimal.TryParse(drv["FLDTOTALDISTANCEOBSERVED"].ToString(), out TotalDistanceObserved);
                decimal.TryParse(drv["FLDTOTALENGINEDISTANCE"].ToString(), out TotalEngineDistance);
            }

            if (e.Item is GridFooterItem)
            {
                RadLabel lblFullSpeedTotalFooter = (RadLabel)e.Item.FindControl("lblFullSpeedTotalFooter");
                if (lblFullSpeedTotalFooter != null) lblFullSpeedTotalFooter.Text = FullSpeedTotal.ToString();

                RadLabel lblReducedSpeedFooter = (RadLabel)e.Item.FindControl("lblReducedSpeedFooter");
                if (lblReducedSpeedFooter != null) lblReducedSpeedFooter.Text = ReducedSpeedTotal.ToString();

                RadLabel lblStoppedFooter = (RadLabel)e.Item.FindControl("lblStoppedFooter");
                if (lblStoppedFooter != null) lblStoppedFooter.Text = Stopped.ToString();

                RadLabel lblTotalDistanceObservedFooter = (RadLabel)e.Item.FindControl("lblTotalDistanceObservedFooter");
                if (lblTotalDistanceObservedFooter != null) lblTotalDistanceObservedFooter.Text = TotalDistanceObserved.ToString();

                RadLabel lblTotalEngineDistanceFooter = (RadLabel)e.Item.FindControl("lblTotalEngineDistanceFooter");
                if (lblTotalEngineDistanceFooter != null) lblTotalEngineDistanceFooter.Text = TotalEngineDistance.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    decimal Manoeuvering = 0, ManoeuveringDist = 0;
    protected void gvManoeuvering_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                decimal.TryParse(drv["FLDTOTALMANOEVERING"].ToString(), out Manoeuvering);
                decimal.TryParse(drv["FLDTOTALMANOEVERINGDIST"].ToString(), out ManoeuveringDist);
            }

            if (e.Item is GridFooterItem)
            {
                RadLabel lblManoeuveringFooter = (RadLabel)e.Item.FindControl("lblManoeuveringFooter");
                if (lblManoeuveringFooter != null) lblManoeuveringFooter.Text = Manoeuvering.ToString();

                RadLabel lblManoeuveringDistFooter = (RadLabel)e.Item.FindControl("lblManoeuveringDistFooter");
                if (lblManoeuveringDistFooter != null) lblManoeuveringDistFooter.Text = ManoeuveringDist.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvPassageSummary_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"]));


        gvPassageSummary.DataSource = ds.Tables[0];

    }

    protected void gvManoeuvering_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"]));

        gvManoeuvering.DataSource = ds.Tables[1];
    }
}
