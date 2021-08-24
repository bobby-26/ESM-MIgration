using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class VesselPositionROBReInitialization : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtVoyageId.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none;");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuRobMain.AccessRights = this.ViewState;
            MenuRobMain.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["NOONREPORTID"] = "";
                ViewState["NEXTREPORTNOONID"] = "";
                ViewState["DATE"] = Request.QueryString["Date"];
                txtReintializeDate.Text = ViewState["DATE"].ToString();
                if (Request.QueryString["Date"]!=null && General.GetNullableDateTime(ViewState["DATE"].ToString()) != null)
                    txtReintializeTime.SelectedDate = Convert.ToDateTime( ViewState["DATE"].ToString());
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
               
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString() + "', true); ");
                imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString() + "&voyageid=" + txtVoyageId.Text + "', true); ");

                DataSet dsCurrentvoyage = PhoenixVesselPositionVoyageData.CurrentVoyageData(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dsCurrentvoyage.Tables.Count > 0)
                {
                    if (dsCurrentvoyage.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsCurrentvoyage.Tables[0].Rows[0];

                        txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                        txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();

                        ucNextPort.VoyageId = (txtNewVoyageId.Text == "" ? txtVoyageId.Text : txtNewVoyageId.Text);
                        ucNextPort.VesselId = (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString();
                    }
                }
                
                if (General.GetNullableDateTime(txtReintializeDate.Text) != null && General.GetNullableGuid(ViewState["NOONREPORTID"].ToString())!=null)
                {
                    txtReintializeDate.Enabled = false;
                    txtReintializeTime.Enabled = false;
                    txtReintializeTime.CssClass = "readonlytextbox";
                }
            }
            BindMainData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRobMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string ReinitialTime = txtReintializeTime.SelectedTime != null ? txtReintializeTime.SelectedTime.Value.ToString() : "";

                PhoenixVesselPositionROBReInitialization.InsertReInitialDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(txtVoyageId.Text),
                    General.GetNullableGuid(txtNewVoyageId.Text),
                    General.GetNullableInteger(ucNextPort.SelectedValue),
                    General.GetNullableGuid(ucNextPort.SelectedPortCallValue),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableDateTime(txtReintializeDate.Text + " " + ReinitialTime),
                    General.GetNullableString(ddlVoyageStartFrom.SelectedValue),
                    General.GetNullableString(txtReason.SelectedValue),
                    General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                    General.GetNullableString(txtRemarks.Text),
                    General.GetNullableGuid(ViewState["NEXTREPORTNOONID"].ToString()),
                    General.GetNullableString(ReinitialTime)
                    );
                ucStatus.Text = "Saved Successfully";
                BindMainData();
            }
        }
        catch (Exception ex)
        {
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
                PhoenixVesselPositionROBReInitialization.InsertReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(txtVoyageId.Text),
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   General.GetNullableDateTime(txtReintializeDate.Text),
                    General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                   General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrobAndConsumptionId")).Text),
                   General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtFuelROBEdit")).Text),
                   new Guid(((RadLabel)e.Item.FindControl("lblFuelTypeCode")).Text),
                   General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAvgSulphurPercentageEdit")).Text));

                ucStatus.Text = "Fuel updated Successfully.";

                gvFuelRebind();
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
        ucNextPort.Text = "";
        ucNextPort.SelectedValue = "";
        ucNextPort.SelectedPortCallValue = "";

        ucNextPort.VoyageId = (txtNewVoyageId.Text == "" ? txtVoyageId.Text : txtNewVoyageId.Text);
        ucNextPort.VesselId = (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString();
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

                PhoenixVesselPositionROBReInitialization.InsertReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(txtVoyageId.Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableDateTime(txtReintializeDate.Text),
                     General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrobAndConsumptionId")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtLubOilROBEdit")).Text),
                    new Guid(((RadLabel)e.Item.FindControl("lblLubOilId")).Text), null);

                ucStatus.Text = "Lub Oil updated Successfully.";

                gvLubOilRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMainData()
    {
        string ReinitialTime = txtReintializeTime.SelectedTime != null ? txtReintializeTime.SelectedTime.Value.ToString() : "";
        DataSet ds = PhoenixVesselPositionROBReInitialization.VoyageReInitializeEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                    General.GetNullableDateTime(txtReintializeDate.Text + " " + ReinitialTime));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
            txtReintializeDate.Text = General.GetDateTimeToString(dr["FLDLASTREACTIVATEDDATE"]);
            if (General.GetNullableDateTime(dr["FLDLASTREACTIVATEDDATE"].ToString()) != null)
                txtReintializeTime.SelectedDate = Convert.ToDateTime(dr["FLDLASTREACTIVATEDDATE"].ToString());
            txtLastReportDate.Text = General.GetDateTimeToString(dr["FLDMAXDATE"]);
            txtLastReportType.Text = dr["FLDLASTREPORTTYPE"].ToString();
            ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
            ViewState["NOONREPORTID"] = dr["FLDNOONREPORTID"].ToString();
            ViewState["NEXTREPORTNOONID"] = dr["FLDNEXTREPORTNOONID"].ToString();

            txtNewVoyageName.Text = dr["FLDNEWVOYAGENAME"].ToString();
            txtNewVoyageId.Text = dr["FLDNEWVOYAGEID"].ToString();
            ucNextPort.Text = dr["FLDPORTNAME"].ToString();
            ucNextPort.SelectedValue = dr["FLDPORT"].ToString();
            ucNextPort.SelectedPortCallValue = dr["FLDPORTCALLID"].ToString();
            txtReason.Text = dr["FLDREINITIALREASON"].ToString();
            ddlVoyageStartFrom.SelectedValue = dr["FLDVOYAGESTARTFROM"].ToString();
            txtRemarks.Text = dr["FLDREMARK"].ToString();

            if (txtReintializeDate.Text != string.Empty)
            {
                if (General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) != null)
                {
                    PhoenixToolbar toolbarAdd = new PhoenixToolbar();
                    toolbarAdd.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselPosition/VesselPositionReInitializeCargoOperations.aspx?vesselid="
                        + (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString()
                        + "&voyageid=" + ViewState["VOYAGEID"].ToString()
                        + "&NoonReportID=" + ViewState["NOONREPORTID"].ToString() + "&ReportDate=" + txtReintializeDate.Text +
                        "')", "Add Cargo Operation", "<i class=\"fa fa-plus-circle\"></i>", "ADDOPR");
                    MenuOperation.AccessRights = this.ViewState;
                    MenuOperation.MenuList = toolbarAdd.Show();
                    MenuOperation.Visible = true;
                }
                else
                {
                    MenuOperation.Visible = false;
                }
            }
        }
        else
        {
            ViewState["VOYAGEID"] = "";
        }
    }

    protected void gvFuel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string ReinitialTime = txtReintializeTime.SelectedTime != null ? txtReintializeTime.SelectedTime.Value.ToString() : "";
        DataSet ds = PhoenixVesselPositionROBReInitialization.ListReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                , General.GetNullableDateTime(txtReintializeDate.Text));
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
        string ReinitialTime = txtReintializeTime.SelectedTime != null ? txtReintializeTime.SelectedTime.Value.ToString() : "";
        DataSet ds = PhoenixVesselPositionROBReInitialization.ListReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                , General.GetNullableDateTime(txtReintializeDate.Text));
        gvLubOil.DataSource = ds.Tables[2];
    }
 

    protected void gvLubOilRebind()
    {
        gvLubOil.SelectedIndexes.Clear();
        gvLubOil.EditIndexes.Clear();
        gvLubOil.DataSource = null;
        gvLubOil.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCargoOperationRebind();
    }

    protected void gvCargoOperation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureOperation(
            General.GetNullableInteger((PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString()),
            General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()));
        gvCargoOperation.DataSource = ds;
    }

    protected void gvCargoOperationRebind()
    {
        gvCargoOperation.SelectedIndexes.Clear();
        gvCargoOperation.EditIndexes.Clear();
        gvCargoOperation.DataSource = null;
        gvCargoOperation.Rebind();
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
                    cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/VesselPosition/VesselPositionReInitializeCargoOperations.aspx?operationid=" + lblOperationId.Text
                        + "&vesselid=" + lblVesselid.Text + "&voyageid=" + lblVoyageid.Text + "&NoonReportID=" + ViewState["NOONREPORTID"].ToString() + "&ReportDate=" + txtReintializeDate.Text + "');return true;");

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

                    att.Attributes.Add("onclick", "javascript:openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELPOSITION + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void Operation_TabStripCommand(object sender, EventArgs e)
    {
    }

    protected void gvWater_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionROBReInitialization.ListReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID
                               , General.GetNullableDateTime(txtReintializeDate.Text));
        gvWater.DataSource = ds.Tables[0];
    }
    protected void gvWaterRebind()
    {
        gvWater.SelectedIndexes.Clear();
        gvWater.EditIndexes.Clear();
        gvWater.DataSource = null;
        gvWater.Rebind();
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

                PhoenixVesselPositionROBReInitialization.InsertReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(txtVoyageId.Text),
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableDateTime(txtReintializeDate.Text),
                     General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrobAndConsumptionId")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtWaterROBEdit")).Text),
                    new Guid(((RadLabel)e.Item.FindControl("lblWaterTypeCodeEdit")).Text), null);

                ucStatus.Text = "Water updated Successfully.";

                gvWaterRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFuel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixVesselPositionROBReInitialization.InsertReInitialRob(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(txtVoyageId.Text),
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? int.Parse(Request.QueryString["Vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   General.GetNullableDateTime(txtReintializeDate.Text),
                    General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                   General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrobAndConsumptionId")).Text),
                   General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtFuelROBEdit")).Text),
                   new Guid(((RadLabel)e.Item.FindControl("lblFuelTypeCode")).Text),
                   General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAvgSulphurPercentageEdit")).Text));

                ucStatus.Text = "Fuel updated Successfully.";

                gvFuelRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
