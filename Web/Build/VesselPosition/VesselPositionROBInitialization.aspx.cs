using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselPosition_VesselPositionROBInitialization : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtVoyageId.Attributes.Add("style", "display:none;");

            SessionUtil.PageAccessRights(this.ViewState); 
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuRobMain.AccessRights = this.ViewState;
            MenuRobMain.MenuList = toolbar.Show();           

            if (!IsPostBack)
            {
                ViewState["vesselid"] = "0";
                if (Request.QueryString["vesselid"] != null)
                    ViewState["vesselid"] = Request.QueryString["vesselid"];
                ViewState["NOONREPORTID"] = "";
                BindMainData();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString() + "', true); ");
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                if (Request.QueryString["vesselname"] != null)
                    txtVessel.Text = Request.QueryString["vesselname"];
            }

            if (!string.IsNullOrEmpty(ddlVoyagePort.VesselId) && !string.IsNullOrEmpty(ddlVoyagePort.VoyageId) && ddlVoyageStartFrom.SelectedIndex>0)
            {
                string flag;
                flag = ddlVoyageStartFrom.SelectedValue == "ARRIVAL" ? "1" : "2";
                toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionReInitializeCargoOperations.aspx?vesselid="
                    + (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString() + "&voyageid=" + txtVoyageId.Text
                    + "&NoonReportID=" + ViewState["NOONREPORTID"].ToString() + "&ReportDate=" + txtDate.Text +
                    "')", "Add Cargo Operation", "<i class=\"fa fa-plus-circle\"></i>", "ADDOPR");

                MenuOperation.AccessRights = this.ViewState;
                MenuOperation.MenuList = toolbar.Show();
            }
        }

        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRobMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("SAVE"))
            {
                Guid iInitialVoyageId = new Guid();
                if (!IsValidMain(txtVoyageId.Text, ddlVoyageStartFrom.SelectedValue.ToString(), txtDate.Text, ddlVoyagePort.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                string dttime = txtTime.SelectedTime != null ? txtTime.SelectedTime.Value.ToString() : "";
                PhoenixVesselPositionROBInitialization.VoyageInitializeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(txtVoyageId.Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    Convert.ToDateTime(txtDate.Text + " " + dttime),
                    Int32.Parse(ddlVoyagePort.SelectedValue),
                    new Guid(ddlVoyagePort.SelectedPortCallValue),
                    ddlVoyageStartFrom.SelectedValue,
                    ref iInitialVoyageId);

                ucStatus.Text = "Initial Voyage information updated";

                BindMainData();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFuel_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

        }
    }

    protected void gvFuel_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((UserControlMaskNumber)e.Item.FindControl("txtFuelROBEdit")).Text, ViewState["VOYAGEID"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionROBInitialization.InsertInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                  new Guid(ViewState["VOYAGEID"].ToString()),
                  Convert.ToDateTime(txtDate.Text),
                  Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtFuelROBEdit")).Text),
                  new Guid(((RadLabel)e.Item.FindControl("lblFuelTypeCode")).Text),
                  General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAvgSulphurPercentageEdit")).Text));


                ucStatus.Text = "Fuel updated Successfully.";
                gvFuelRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLubOil_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

        }
    }

    protected void gvLubOil_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((UserControlMaskNumber)e.Item.FindControl("txtLubOilROBEdit")).Text, ViewState["VOYAGEID"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionROBInitialization.InsertInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    new Guid(ViewState["VOYAGEID"].ToString()),
                    Convert.ToDateTime(txtDate.Text),
                    Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtLubOilROBEdit")).Text),
                    new Guid(((RadLabel)e.Item.FindControl("lblLubOilId")).Text), null);

                ucStatus.Text = "Lub Oil updated Successfully.";

                gvLubOilRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWater_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvWater_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((UserControlMaskNumber)e.Item.FindControl("txtWaterROBEdit")).Text, ViewState["VOYAGEID"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionROBInitialization.InsertInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    new Guid(ViewState["VOYAGEID"].ToString()),
                    Convert.ToDateTime(txtDate.Text),
                    Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtWaterROBEdit")).Text),
                    new Guid(((RadLabel)e.Item.FindControl("lblWaterTypeCode")).Text), null);

                ucStatus.Text = "Water updated Successfully.";

                gvWaterRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    private void BindMainData()
    {
        DataSet ds = PhoenixVesselPositionROBInitialization.VoyageInitializeEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
            ddlVoyageStartFrom.SelectedValue = dr["FLDVOYAGESTARTSFROM"].ToString();
            ddlVoyagePort.VesselId = dr["FLDVESSELID"].ToString();
            ddlVoyagePort.VoyageId = dr["FLDVOYAGEID"].ToString();
            ddlVoyagePort.SelectedValue = dr["FLDCOMMENCEDPORTID"].ToString();
            ddlVoyagePort.SelectedPortCallValue = dr["FLDCOMMENCEDPORTCALLID"].ToString(); 
            ddlVoyagePort.Text = dr["FLDCOMMENCEDSEAPORTNAME"].ToString();
            txtDate.Text = General.GetDateTimeToString(dr["FLDCOMMENCEDDATETIME"]);
            if (General.GetNullableDateTime(dr["FLDCOMMENCEDDATETIME"].ToString()) != null)
                txtTime.SelectedDate = Convert.ToDateTime(dr["FLDCOMMENCEDDATETIME"]);
            ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
            ViewState["NOONREPORTID"] = dr["FLDNOONREPORTID"].ToString();
        }
        else
        {
            ViewState["VOYAGEID"] = "";
        }
    }

    
    protected void gvFuel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionROBInitialization.ListInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvFuel.DataSource = ds.Tables[1];
    }
    protected void gvFuelRebind()
    {
        gvFuel.SelectedIndexes.Clear();
        gvFuel.EditIndexes.Clear();
        gvFuel.DataSource = null;
        gvFuel.Rebind();
    }
   
    protected void gvLubOil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionROBInitialization.ListInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvLubOil.DataSource = ds.Tables[2];
    }
    protected void gvLubOilRebind()
    {
        gvLubOil.SelectedIndexes.Clear();
        gvLubOil.EditIndexes.Clear();
        gvLubOil.DataSource = null;
        gvLubOil.Rebind();
    }

    protected void gvWater_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionROBInitialization.ListInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvWater.DataSource = ds.Tables[0];
    }
    protected void gvWaterRebind()
    {
        gvWater.SelectedIndexes.Clear();
        gvWater.EditIndexes.Clear();
        gvWater.DataSource = null;
        gvWater.Rebind();
    }
 

    private bool IsValidData(string rob,string voyageNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rob.Trim().Equals(""))
            ucError.ErrorMessage = "ROB is required.";

        if (voyageNo.Trim().Equals(""))
            ucError.ErrorMessage = "Voyage No is required.";

        return (!ucError.IsError);
    }

    private bool IsValidMain(string voyageNo, string startFrom, string date,string port)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (voyageNo.Trim().Equals(""))
            ucError.ErrorMessage = "Voyage No is required.";

        if (startFrom.Trim().Equals(""))
            ucError.ErrorMessage = "Commenced Start From is required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        if (port.Trim().Equals(""))
            ucError.ErrorMessage = "Port is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ddlVoyagePort.VesselId = (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString();
        ddlVoyagePort.VoyageId = txtVoyageId.Text;
        ddlVoyagePort.SelectedValue = "";
        ddlVoyagePort.Text = "";
        ddlVoyagePort.SelectedPortCallValue = "";
        ddlVoyagePort.bind();
    }

    protected void gvCargoOperation_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                PhoenixVesselPositionDepartureReport.DeleteDepartureOperation(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
              , General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationId")).Text)));

                gvCargoOperationRebind();
                ucStatus.Text = "Deleted Successfully";

            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargoOperation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblOperationId = (RadLabel)e.Item.FindControl("lblOperationId");
                RadLabel lblVoyageid = (RadLabel)e.Item.FindControl("lblVoyageid");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");


                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");

                if (cmdEdit != null)
                    cmdEdit.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'VesselPositionReInitializeCargoOperations.aspx?operationid=" + lblOperationId.Text
                        + "&vesselid=" + lblVesselid.Text + "&voyageid=" + lblVoyageid.Text + "&NoonReportID=" + ViewState["NOONREPORTID"].ToString() + "&ReportDate=" + txtDate.Text + "');return true;");

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                        
                    att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELPOSITION + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvCargoOperation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureOperation(
             General.GetNullableInteger((PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(ViewState["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString()),
             General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()));

        gvCargoOperation.DataSource = ds.Tables[0];
    }
    protected void gvCargoOperationRebind()
    {
        gvCargoOperation.SelectedIndexes.Clear();
        gvCargoOperation.EditIndexes.Clear();
        gvCargoOperation.DataSource = null;
        gvCargoOperation.Rebind();
    }
 
    protected void Operation_TabStripCommand(object sender, EventArgs e)
    {
        
    }

    protected void ddlVoyageStartFrom_OnSelectedIndexChanged(object sender, EventArgs e)
    {
 
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

}
