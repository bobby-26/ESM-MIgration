using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselPositionVoyageData : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
        toolbarvoyagetap.AddButton("List", "VOYAGE");
        toolbarvoyagetap.AddButton("Voyage Detail", "VOYAGEDATA");
        MenuVoyageTap.AccessRights = this.ViewState;
        MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
        MenuVoyageTap.SelectedMenuIndex = 1;

        PhoenixToolbar toolbarvoyage = new PhoenixToolbar();
        toolbarvoyage.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarvoyage.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["VESSELID"] = "";
            //
            if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
            {
                Filter.CurrentVPRSVoyageSelection = null;                
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "0";
            }
            else
            {
                UcVessel.Enabled = false;
            }
            BindData();
        }
        
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        RebindPortDetails();
    }

    private void BindData()
    {
        if (Filter.CurrentVPRSVoyageSelection != null)
        {
            DataSet ds = PhoenixVesselPositionVoyageData.EditVoyageData(General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection));
            DataTable dt = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 0)
            {
                UcVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
                ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();

                ucCommencedDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDCOMMENCEDDATETIME"]);
                ucCompletedDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDCOMPLETEDDATE"]);
                txtVoyageNo.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
                UcCommencedPort.SelectedValue = dt.Rows[0]["FLDCOMMENCEDPORTID"].ToString();
                UcCommencedPort.Text = dt.Rows[0]["FLDCOMMENCEDPORTNAME"].ToString();
                UcCompletedPort.SelectedValue = dt.Rows[0]["FLDCOMPLETEDPORTID"].ToString();
                UcCompletedPort.Text = dt.Rows[0]["FLDCOMPLETEDPORTNAME"].ToString();

                if (General.GetNullableDateTime( dt.Rows[0]["FLDCOMMENCEDDATETIME"].ToString()) != null)
                    txtTimeOfCommenced.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDCOMMENCEDDATETIME"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDCOMPLETEDDATE"].ToString()) != null)
                    txtTimeOfCompleted.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDCOMPLETEDDATE"]);

                txtCharterer.Text = dt.Rows[0]["FLDCHARTERERDETAIL"].ToString();
                txtChartererInstruction.Text = dt.Rows[0]["FLDCHARTERERVOYAGEINSTRUCTION"].ToString();
            }
        }
    }

    protected void VoyageNewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("NEW"))
        {
            txtCharterer.Text = "";
            UcVessel.SelectedVessel = "";
            ViewState["VESSELID"] = "";
            UcCommencedPort.SelectedValue = "";
            UcCommencedPort.Text = "";
            UcCompletedPort.SelectedValue = "";
            UcCompletedPort.Text = "";
            ucCommencedDate.Text = "";
            ucCompletedDate.Text = "";
            txtVoyageNo.Text = "";
        }

        if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("SAVE"))
        {
            if (Filter.CurrentVPRSVoyageSelection != null)
            {
                UpdateVoyage();
            }
            else
            {
                AddVoyage();
            }
        }
    }

    private void AddVoyage()
    {
        if (!IsValidVoyage(txtVoyageNo.Text, ucCommencedDate.Text, UcCommencedPort.SelectedValue, txtCharterer.Text))
        {
            ucError.Visible = true;
            return;
        }

        Guid? voyageid = null;
        string timeofcommenced = txtTimeOfCommenced.SelectedTime != null ? txtTimeOfCommenced.SelectedTime.Value.ToString() : "";
        string timeofcompleted = txtTimeOfCompleted.SelectedTime != null ? txtTimeOfCompleted.SelectedTime.Value.ToString() : "";

        PhoenixVesselPositionVoyageData.InsertVoyageData(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(UcVessel.SelectedVessel.ToString()),
            txtCharterer.Text,
            txtVoyageNo.Text,
            General.GetNullableDateTime(""),
            General.GetNullableDateTime(ucCommencedDate.Text + " " + timeofcommenced),
            General.GetNullableInteger(UcCommencedPort.SelectedValue),
            General.GetNullableDateTime(ucCompletedDate.Text+" "+ timeofcompleted),
            General.GetNullableInteger(UcCompletedPort.SelectedValue),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            ref voyageid,
            General.GetNullableString(txtChartererInstruction.Text));

        Filter.CurrentVPRSVoyageSelection = voyageid.ToString();
        ucStatus.Text = "Voyage Data Added ";
    }

    private void UpdateVoyage()
    {
        if (!IsValidVoyage(txtVoyageNo.Text, ucCommencedDate.Text, UcCommencedPort.SelectedValue, txtCharterer.Text))
        {
            ucError.Visible = true;
            return;
        }

        string timeofcommenced = txtTimeOfCommenced.SelectedTime != null ? txtTimeOfCommenced.SelectedTime.Value.ToString() : "";
        string timeofcompleted = txtTimeOfCompleted.SelectedTime!=null? txtTimeOfCompleted.SelectedTime.Value.ToString(): "";

        PhoenixVesselPositionVoyageData.UpdateVoyageData(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
            General.GetNullableInteger(UcVessel.SelectedVessel.ToString()),
            txtCharterer.Text, txtVoyageNo.Text,
            General.GetNullableDateTime(""),
            General.GetNullableDateTime(ucCommencedDate.Text + " " + timeofcommenced), General.GetNullableInteger(UcCommencedPort.SelectedValue), 
            General.GetNullableDateTime(ucCompletedDate.Text+" "+timeofcompleted),
            General.GetNullableInteger(UcCompletedPort.SelectedValue), General.GetNullableDecimal(""),
            General.GetNullableDecimal(""), General.GetNullableDecimal(""), General.GetNullableString(txtChartererInstruction.Text)
                                                        );
        ucStatus.Text = "Voyage Data updated ";
    }

    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("VOYAGEDATA"))
        {

        }

        if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("VOYAGE"))
        {
            Response.Redirect("VesselPositionVoyage.aspx", false);
        }
    }
 
    protected void gvVoyagePort_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                
                if (Filter.CurrentVPRSVoyageSelection == null)
                {
                   // ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Voyage is not yet Created.Please Create the voyage first";

                    ucError.Visible = true;
                    return;
                }

                RadTimePicker radtpiceta = ((RadTimePicker)e.Item.FindControl("txtTimeOfETAAdd"));
                RadTimePicker radtpicetd = ((RadTimePicker)e.Item.FindControl("txtTimeOfETDAdd"));

                string timeofeta = radtpiceta.SelectedTime != null ? radtpiceta.SelectedTime.Value.ToString() : "";
                string timeofetd = radtpicetd.SelectedTime != null ? radtpicetd.SelectedTime.Value.ToString() : "";

                if (!IsValidPortCall(
                    ((UserControlDate)e.Item.FindControl("ucETAAdd")).Text + " " + timeofeta,
                    ((UserControlMultiColumnPort)e.Item.FindControl("ucPortAdd")).SelectedValue,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSerialNoAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionVoyageLoadDetails.InsertVoyageLoadDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucETAAdd")).Text + " " + timeofeta),
                    General.GetNullableDateTime(""),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucETDAdd")).Text + " " + timeofetd),
                    ((RadTextBox)e.Item.FindControl("txtCharterAgentNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtOwnerAgentNameAdd")).Text,
                    General.GetNullableGuid(""),
                    General.GetNullableDecimal(""), "",
                    General.GetNullableInteger(((UserControlMultiColumnPort)e.Item.FindControl("ucPortAdd")).SelectedValue),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucSerialNoAdd")).Text));

                ucStatus.Text = "Saved Successfully.";

                RebindPortDetails();


            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadTimePicker radtpiceta = ((RadTimePicker)e.Item.FindControl("txtTimeOfETAEdit"));
                RadTimePicker radtpicetd = ((RadTimePicker)e.Item.FindControl("txtTimeOfETDEdit"));

                string timeofeta = radtpiceta.SelectedTime != null ? radtpiceta.SelectedTime.Value.ToString() : "";
                string timeofetd = radtpicetd.SelectedTime != null ? radtpicetd.SelectedTime.Value.ToString() : "";

                if (!IsValidPortCall(((UserControlDate)e.Item.FindControl("ucETAEdit")).Text + " " + timeofeta,
                                ((UserControlMultiColumnPort)e.Item.FindControl("ucPortEdit")).SelectedValue,
                                ((RadLabel)e.Item.FindControl("lblSerilNoEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionVoyageLoadDetails.UpdateVoyageLoadDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblPortCallIdEdit")).Text),
                    int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucETAEdit")).Text + " " + timeofeta),
                    General.GetNullableDateTime(""),
                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucETDEdit")).Text + " " + timeofetd),
                    ((RadTextBox)e.Item.FindControl("txtCharterAgentNameEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtOwnerAgentNameEdit")).Text,
                    General.GetNullableGuid(""),
                    General.GetNullableDecimal(""),
                    General.GetNullableInteger(((UserControlMultiColumnPort)e.Item.FindControl("ucPortEdit")).SelectedValue));
                ucStatus.Text = "Updated Successfully.";

                RebindPortDetails();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionVoyageLoadDetails.DeleteVoyageLoadDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblPortCallId")).Text));
                ucStatus.Text = "Deleted Successfully.";
                RebindPortDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoyagePort_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton cmdCargoAdd = (LinkButton)e.Item.FindControl("cmdCargoAdd");
                if (cmdCargoAdd != null)
                {
                    RadLabel lblETA = (RadLabel)e.Item.FindControl("lblETA");
                    RadLabel lblPortCallId = (RadLabel)e.Item.FindControl("lblPortCallId");

                    if (lblETA != null && lblPortCallId != null)
                    {
                        cmdCargoAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdCargoAdd.CommandName);
                        cmdCargoAdd.Attributes.Add("onclick"
                        , "parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/VesselPosition/VesselPositionVoyageCargoDetails.aspx?vesselid="
                        + ViewState["VESSELID"].ToString() + "&date=" + lblETA.Text + "&voyageid="
                        + Filter.CurrentVPRSVoyageSelection + "&portcallid=" + lblPortCallId.Text + "&PortName=" + drv["FLDSEAPORTNAME"].ToString() + "');return false;");
                    }
                }

                UserControlMultiColumnPort ucPortEdit = (UserControlMultiColumnPort)e.Item.FindControl("ucPortEdit");
                if (ucPortEdit != null)
                {
                    ucPortEdit.SelectedValue = drv["FLDPORT"].ToString();
                    ucPortEdit.Text = drv["FLDSEAPORTNAME"].ToString();
                }

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPort");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblPortName");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }

                RadLabel lbtn1 = (RadLabel)e.Item.FindControl("lblCharterAgent");
                UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("uclblCharterAgent");
                if (lbtn != null)
                {
                    uct1.Position = ToolTipPosition.TopCenter;
                    uct1.TargetControlId = lbtn1.ClientID;
                }

                RadLabel lbtn2 = (RadLabel)e.Item.FindControl("lblOwnerAgent");
                UserControlToolTip uct2 = (UserControlToolTip)e.Item.FindControl("uclblOwnerAgent");
                if (lbtn2 != null)
                {
                    uct2.Position = ToolTipPosition.TopCenter;
                    uct2.TargetControlId = lbtn2.ClientID;
                }

                HtmlImage ImgShowCharterAgent = (HtmlImage)e.Item.FindControl("ImgShowCharterAgent");

                if (ImgShowCharterAgent != null)
                {
                    ImgShowCharterAgent.Attributes.Add("onclick", "return showPickList('spnPickListCharterAgentEdit', 'codehelp1', '', '../Common/CommonPickListPortAgent.aspx?addresscode=135&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }

                HtmlImage ImgShowOwnerAgent = (HtmlImage)e.Item.FindControl("ImgShowOwnerAgent");

                if (ImgShowOwnerAgent != null)
                {
                    ImgShowOwnerAgent.Attributes.Add("onclick", "return showPickList('spnPickListOwnerAgentEdit', 'codehelp1', '', '../Common/CommonPickListPortAgent.aspx?addresscode=135&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton IsCargoYn = (LinkButton)e.Item.FindControl("cmdCargoAdd");
                if (IsCargoYn != null)
                {
                    if (drv["FLDCARGOYN"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-shopping-basket\"></i></span>";
                        IsCargoYn.Controls.Add(html);
                    }
                }
            }
            if (e.Item is GridFooterItem)
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                HtmlImage ImgShowCharterAgentAdd = (HtmlImage)e.Item.FindControl("ImgShowCharterAgentAdd");

                if (ImgShowCharterAgentAdd != null)
                {
                    ImgShowCharterAgentAdd.Attributes.Add("onclick", "return showPickList('spnPickListCharterAgentAdd', 'codehelp1', '', '../Common/CommonPickListPortAgent.aspx?addresscode=135&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }

                HtmlImage ImgShowOwnerAgentAdd = (HtmlImage)e.Item.FindControl("ImgShowOwnerAgentAdd");

                if (ImgShowOwnerAgentAdd != null)
                {
                    ImgShowOwnerAgentAdd.Attributes.Add("onclick", "return showPickList('spnPickListOwnerAgentAdd', 'codehelp1', '', '../Common/CommonPickListPortAgent.aspx?addresscode=135&vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvVoyagePort_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    private bool IsValidVoyage(string voyno, string comdate, string comport, string charter)
    {
       // ucError.HeaderMessage = "Please provide the following required information";

        if ((voyno == null) || (voyno == ""))
            ucError.ErrorMessage = "Voyage No. is required.";

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null || General.GetNullableInteger(ViewState["VESSELID"].ToString()) == 0)
            ucError.ErrorMessage = "Please select a vessel.";

        return (!ucError.IsError);
    }

    private bool IsValidPortCall(string eta, string port, string serialno)
    {
       // ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(eta) == null)
            ucError.ErrorMessage = "ETA is required.";

        if (General.GetNullableInteger(serialno) == null)
            ucError.ErrorMessage = "Valid Serial is required.";

        if (General.GetNullableInteger(port) == null)
            ucError.ErrorMessage = "Port is required.";

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null || General.GetNullableInteger(ViewState["VESSELID"].ToString()) == 0)
            ucError.ErrorMessage = "Please select a vessel.";

        return (!ucError.IsError);
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvVoyageCharter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionVoyageLoadDetails.VoyageCharterDetailsList(
            General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection));
            gvVoyageCharter.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyageCharter_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void gvVoyagePort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionVoyageLoadDetails.ListVoyageLoadDetails(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
            null);
            gvVoyagePort.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void RebindPortDetails()
    {
        gvVoyagePort.SelectedIndexes.Clear();
        gvVoyagePort.EditIndexes.Clear();
        gvVoyagePort.DataSource = null;
        gvVoyagePort.Rebind();
    }
    private void RebindCharterDetails()
    {
        gvVoyageCharter.SelectedIndexes.Clear();
        gvVoyageCharter.EditIndexes.Clear();
        gvVoyageCharter.DataSource = null;
        gvVoyageCharter.Rebind();
    }


    protected void gvVoyageCharter_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (Filter.CurrentVPRSVoyageSelection == null)
                    {
                        //ucError.HeaderMessage = "Please provide the following required information";
                        ucError.ErrorMessage = "Voyage is not yet Created.Please Create the voyage first";

                        ucError.Visible = true;
                        return;
                    }

                    PhoenixVesselPositionVoyageLoadDetails.VoyageCharterDetailsUpdate(
                        General.GetNullableGuid(((RadLabel)editableItem.FindControl("lblVoyageCharteridEdit")).Text),
                        General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((RadLabel)editableItem.FindControl("lblOilTypeIdEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtBallastEcoSpeed")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtBallastFullSpeed")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtLadenEcoSpeed")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtLadenFullSpeed")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtCargoLoading")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtCargoDischarging")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtTankCleaning")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtHeatingMaintenance")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtHeatingHeatUp")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)editableItem.FindControl("txtIdle")).Text)
                        );
                    ucStatus.Text = "Updated Successfully.";

                    RebindCharterDetails();
                   
                }
                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoyageCharter_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                if (General.GetNullableInteger(drv["FLDHARDCODE"].ToString()) != General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 240, "SPD")))
                {
                    RadLabel lblOilType = (RadLabel)e.Item.FindControl("lblOilType");

                    if (lblOilType != null)
                        lblOilType.Text = lblOilType.Text + " (MT/Day)";
                }

                if (General.GetNullableInteger(drv["FLDHARDCODE"].ToString()) == General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 240, "SPD")))
                {
                    UserControlMaskNumber loading = (UserControlMaskNumber)e.Item.FindControl("txtCargoLoading");
                    UserControlMaskNumber discharging = (UserControlMaskNumber)e.Item.FindControl("txtCargoDischarging");
                    UserControlMaskNumber tankcleaning = (UserControlMaskNumber)e.Item.FindControl("txtTankCleaning");
                    UserControlMaskNumber heatingmaintenance = (UserControlMaskNumber)e.Item.FindControl("txtHeatingMaintenance");
                    UserControlMaskNumber heatingup = (UserControlMaskNumber)e.Item.FindControl("txtHeatingHeatUp");
                    UserControlMaskNumber idle = (UserControlMaskNumber)e.Item.FindControl("txtIdle");
                    RadLabel lblOilType = (RadLabel)e.Item.FindControl("lblOilType");

                    if (lblOilType != null)
                        lblOilType.Text = lblOilType.Text + " (nm/hr)";

                    if (loading != null)
                        loading.Visible = false;
                    if (discharging != null)
                        discharging.Visible = false;
                    if (tankcleaning != null)
                        tankcleaning.Visible = false;
                    if (heatingmaintenance != null)
                        heatingmaintenance.Visible = false;
                    if (heatingup != null)
                        heatingup.Visible = false;
                    if (idle != null)
                        idle.Visible = false;
                }


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
