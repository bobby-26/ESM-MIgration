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

public partial class VesselPositionESICalculationBreakup : PhoenixBasePage
{
    DataSet dscommon = new DataSet();
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
                BindData();
            }
            bindcommon();
            Bind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionNoonReport.EditNoonReport(General.GetNullableGuid(Request.QueryString["noonreportid"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            txtVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtVoyageNo.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
            txtVoyageStartDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDCOMMENCEDDATETIME"]);
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RECALCULATE"))
        {
            PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(Request.QueryString["vesselid"]),
                        General.GetNullableGuid(Request.QueryString["voyageid"]),
                        General.GetNullableGuid(Request.QueryString["noonreportid"]));

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(Request.QueryString["vesselid"]),
                General.GetNullableGuid(Request.QueryString["voyageid"]),
                General.GetNullableGuid(Request.QueryString["noonreportid"]));


            ucStatus.Text = "Re-Calculated";

            String script = "javascript:fnReloadList('codehelp1');";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void bindcommon()
    {
        dscommon = PhoenixVesselPositionNoonReport.ESICalculationBreakup(
               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               int.Parse(Request.QueryString["vesselid"]),
               General.GetNullableGuid(Request.QueryString["voyageid"]),
               General.GetNullableGuid(Request.QueryString["noonreportid"]));
    }
    private void Bind()
    {
        DataSet ds = dscommon;

       if (ds.Tables[6].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[6].Rows[0];
            txtDistanceObserved.Text = dr["FLDDISTANCE"].ToString();
            txtCargo.Text = dr["FLDCARGO"].ToString();
            txtco2e.Text = dr["FLDCO2EMISSION"].ToString();
            txtco2i.Text = dr["FLDCO2INDEX"].ToString();
            txteeoi.Text = dr["FLDEEOI"].ToString();
            txtesisox.Text = dr["FLDSOX"].ToString();
            txtoverallesi.Text = dr["FLDESI"].ToString();
        }
            
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOilConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCo2EmissionFooter");
                if (lbl != null) lbl.Text = txtco2e.Text;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvDistance_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblDistanceFooter");
                if (lbl != null) lbl.Text = txtDistanceObserved.Text;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvCargo_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCargoFooter");
                if (lbl != null) lbl.Text = txtCargo.Text;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOilConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvOilConsumption.DataSource = ds.Tables[1];
    }
    protected void RebindgvOilConsumption()
    {
        gvOilConsumption.SelectedIndexes.Clear();
        gvOilConsumption.EditIndexes.Clear();
        gvOilConsumption.DataSource = null;
        gvOilConsumption.Rebind();
    }

    protected void gvDistance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvDistance.DataSource= ds.Tables[2];

    }

    protected void gvCargo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvCargo.DataSource = ds.Tables[0];
    }

    protected void gvBaseLineSC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvBaseLineSC.DataSource = ds.Tables[4];
    }

    protected void gvNOX_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvNOX.DataSource = ds.Tables[5];
    }

    protected void gvActualSC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = dscommon;
        gvActualSC.DataSource = ds.Tables[3];
    }
}
