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

public partial class VesselPositionArrivalReportPassageSummaryWaterBreakup : PhoenixBasePage
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
            }
            //BindData();
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
            PhoenixVesselPositionArrivalReport.UpdateArrivalReportPassageSummaryOtherOil(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                            , int.Parse(Request.QueryString["vesselid"])
                                                                                            , General.GetNullableGuid(Request.QueryString["oiltype"]));
            ucStatus.Text = "Re-Calculated";

            String script = "javascript:fnReloadList('codehelp1');";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    

    decimal ConsinPort = 0;
    protected void gvConsinPort_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                decimal.TryParse(drv["FLDCONSUMPTIONTOTALQTY"].ToString(), out ConsinPort);
            }

            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblConsinPortFooter");
                if (lbl != null) lbl.Text = ConsinPort.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    decimal Produced = 0;
    protected void gvProduced_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                decimal.TryParse(drv["FLDTOTALPRODUCED"].ToString(), out Produced);
            }

            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblProducedFooter");
                if (lbl != null) lbl.Text = Produced.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    decimal ConsatSea = 0;
    protected void gvConsatSea_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                decimal.TryParse(drv["FLDCONSUMPTIONTOTALQTY"].ToString(), out ConsatSea);
            }

            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblConsatSeaFooter");
                if (lbl != null) lbl.Text = ConsatSea.ToString();
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
    protected void gvROBonPrevArr_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvROBonPrevArr.DataSource = ds.Tables[0];

    }

    protected void gvReceived_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));

        gvReceived.DataSource = ds.Tables[1];
    }

    protected void gvConsinPort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvConsinPort.DataSource = ds.Tables[2];
    }

    protected void gvROBonPrevDep_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvROBonPrevDep.DataSource = ds.Tables[3];
    }

    protected void gvProduced_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvProduced.DataSource = ds.Tables[4];
    }

    protected void gvROBonArr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvROBonArr.DataSource = ds.Tables[5];
    }

    protected void gvConsatSea_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvConsatSea.DataSource = ds.Tables[6];
    }

    protected void gvConsatArr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvConsatArr.DataSource = ds.Tables[7];
    }

    protected void gvConsatDep_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalReport.ArrivalPassageSummeryWaterBreakup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(Session["VESSELARRIVALID"].ToString())
                                                                                    , int.Parse(Request.QueryString["vesselid"])
                                                                                    , new Guid(Request.QueryString["oiltype"]));
        gvConsatDep.DataSource = ds.Tables[8];
    }
}
