using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionTankSoundinglogDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("Copy", "COPY",ToolBarDirection.Right);
            MenuStockCheckTap.AccessRights = this.ViewState;
            MenuStockCheckTap.MenuList = toolbarvoyagetap.Show();

            if (!IsPostBack)
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStockCheckTap_abStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                PhoenixVesselPositionEUMRVStockCheck.CopyTankSoundingLogDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(Request.QueryString["SoundingLogId"].ToString()),
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
            }
            ucStatus.Text = "Copied Successfully.";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTankSoundinglog_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlOilType");
            if (ddlOilType != null)
            {
                DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                ddlOilType.DataSource = ds;
                ddlOilType.DataBind();

                ddlOilType.SelectedValue = drv["FLDPRODUCTID"].ToString();
            }

            RadComboBox ddlSU = (RadComboBox)e.Item.FindControl("ddlSU");
            if (ddlSU != null)
            {
                ddlSU.SelectedValue = drv["FLDSOUNDINGORULLAGE"].ToString();
            }
        }
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem header = (GridHeaderItem)e.Item;
            header["sounding"].ToolTip = "Sounding / Ullage";
        }
        //if(e.Item is GridHeaderItem)
        //{
        //    RadLabel lblSoundingOrUllagehdr = (RadLabel)e.Item.FindControl("lblSoundingOrUllagehdr");
        //    if (lblSoundingOrUllagehdr != null)
        //    {
        //        lblSoundingOrUllagehdr.Visible = true;
        //        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucSoundingOrUllage");
        //        if (uct != null)
        //        {
        //            uct.Position = ToolTipPosition.TopCenter;
        //            uct.TargetControlId = lblSoundingOrUllagehdr.ClientID;
        //        }
        //    }
        //}
    }

    protected void gvTankSoundinglog_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMRVStockCheck.DeleteTankSoundingLog(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogDetailid")).Text));

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTankSoundinglog_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogDetailidEdit")).Text) == null)
            {
                Guid? detailid = null;
                PhoenixVesselPositionEUMRVStockCheck.InsertTankSoundingLog(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    General.GetNullableGuid(Request.QueryString["SoundingLogId"].ToString()),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTankid")).Text),
                    General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlOilType")).SelectedValue),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSounding")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtTemp")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtVolCorrected")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtDensity")).Text),
                     ref detailid,
                     General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlSU")).SelectedValue)
                    );
            }
            else
            {
                PhoenixVesselPositionEUMRVStockCheck.UpdateTankSoundingLog(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlOilType")).SelectedValue),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSounding")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtTemp")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtVolCorrected")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtDensity")).Text),
                     General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogDetailidEdit")).Text),
                     General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlSU")).SelectedValue)
                    );
            }

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSummaryData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            lblHFOValue.Text = dt.Rows[0]["FLDHFO"].ToString();
            lblLSFOValue.Text = dt.Rows[0]["FLDLSFO"].ToString();
            lblMDOValue.Text = dt.Rows[0]["FLDMDO"].ToString();
            lblMGOValue.Text = dt.Rows[0]["FLDMGO"].ToString();
            lblLSMGOValue.Text = dt.Rows[0]["FLDLSMGO"].ToString();
        }
        
    }

    private void BindSummaryTable(DataTable dt)
    {        
        if (dt.Rows.Count > 0)
        {
            string maas = "";
            string head = "";
            for(int i=0;i< dt.Rows.Count;i++)
            {
                if (General.GetNullableString(dt.Rows[i]["FLDOILTYPENAME"].ToString()) != null)
                {
                    head = head + "<th style=\"height: 20px;text-align: center; background:#1c84c6;color:#ffffff;\"><b>" + dt.Rows[i]["FLDOILTYPENAME"].ToString() + "</b></th>";
                    maas = maas + "<td style=\"height: 25px;text-align: center;\">" + dt.Rows[i]["FLDMASS"].ToString() + "</td>";
                }
            }
            if (head != "")
            {
                sumtable.InnerHtml = "<table width=\"100%\"  cellspacing=\"1\" cellpading=\"15\" style=\"border - collapse: collapse;\"><tr>" + head + "</tr><tr>" + maas + "</tr></table>";
                lblTableHead.Visible = true;
            }
            else
                lblTableHead.Visible = false;
            
            sumtable.Visible = true;
        }
        else
        {
            sumtable.Visible = false;
            lblTableHead.Visible = false;
        }
    }
    protected void gvTankSoundinglog_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixVesselPositionEUMRVStockCheck.TankSoundingLogList(
              General.GetNullableGuid(Request.QueryString["SoundingLogId"].ToString()));
                gvTankSoundinglog.DataSource = ds.Tables[0];

           // BindSummaryData(ds.Tables[1]);
            BindSummaryTable(ds.Tables[2]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvTankSoundinglog.SelectedIndexes.Clear();
        gvTankSoundinglog.EditIndexes.Clear();
        gvTankSoundinglog.DataSource = null;
        gvTankSoundinglog.Rebind();
    }
}
