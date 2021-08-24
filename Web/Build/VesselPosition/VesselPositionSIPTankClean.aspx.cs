using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionSIPTankClean : PhoenixBasePage
{
    string tootip = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display: none;");

        bindToolTip();

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", tootip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddFontAwesomeButton("", "IMO Guidance – Tank Cleaning", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
        toolbartab.AddButton("Back", "BACK", ToolBarDirection.Right);
        TabTankInformation.AccessRights = this.ViewState;
        TabTankInformation.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                UcVessel.SelectedVessel = Request.QueryString["VESSELID"].ToString();
            }
            UcVessel.DataBind();
            UcVessel.bind();
        }
    }
    private void bindToolTip()
    {
        try
        {
            DataSet ds = PhoenixRegistersSIPToolTipConfiguration.SIPToolTipConfigList(General.GetNullableInteger(Request.QueryString["SIPCONFIGID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableString(ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString()) != null)
                    tootip = ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void TabTankInformation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselPosition/VesselPositionSIPConfiguration.aspx");
            }
            else if (CommandName.ToUpper().Equals("PDF"))
            {
                string filePath = Server.MapPath("~/Template/VesselPosition/IMO Guidance - TANK CLEANING.pdf");
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton lblTank = (LinkButton)e.Item.FindControl("lblTank");
                if (lblTank != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        lblTank.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=0&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=0&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton lblTank = (LinkButton)e.Item.FindControl("lblTank");
                if (lblTank != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        lblTank.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=1&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=1&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        RebindFeulTank();
        RebindLubeOilTank();
        RebindServiceSettlingTank();
    }
    protected void gvLubeOilTank_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton lblTank = (LinkButton)e.Item.FindControl("lblTank");
                if (lblTank != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        lblTank.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerLubDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=2&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == null || General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) == 0)
                        cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionSIPTankCleanandBunkerLubDetail.aspx?tankid=" + drv["FLDTANKID"].ToString() + "&siptankcleaningandbunkerid=" + drv["FLDSIPTANKCLEANANDBUNKERID"].ToString() + "&type=2&Vesselid=" + UcVessel.SelectedVessel + "'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankCleaning.SIPFuelTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        gvSIPTanksConfuguration.DataSource = ds;

    }
    protected void RebindFeulTank()
    {
        gvSIPTanksConfuguration.SelectedIndexes.Clear();
        gvSIPTanksConfuguration.EditIndexes.Clear();
        gvSIPTanksConfuguration.DataSource = null;
        gvSIPTanksConfuguration.Rebind();
    }
    protected void gvSettlingServeice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankCleaning.SIPSettlingServiceTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));
        gvSettlingServeice.DataSource = ds;

    }
    protected void RebindServiceSettlingTank()
    {
        gvSettlingServeice.SelectedIndexes.Clear();
        gvSettlingServeice.EditIndexes.Clear();
        gvSettlingServeice.DataSource = null;
        gvSettlingServeice.Rebind();
    }
    protected void gvLubeOilTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankCleaning.SIPLubeOilTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));
        gvLubeOilTank.DataSource = ds;
    }
    protected void RebindLubeOilTank()
    {
        gvLubeOilTank.SelectedIndexes.Clear();
        gvLubeOilTank.EditIndexes.Clear();
        gvLubeOilTank.DataSource = null;
        gvLubeOilTank.Rebind();
    }
}
