using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class VesselPositionDepartureReportOperations : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenPick.Attributes.Add("style", "display:none;");
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "DEPARTUREREPORT");
        toolbarmain.AddButton("Departure", "DEPARTURE");
        toolbarmain.AddButton("Operations", "OPERATIONS");
        toolbarmain.AddButton("Emission In Port", "MRVSUMMARY");
        MenuDRSubTab.AccessRights = this.ViewState;
        MenuDRSubTab.MenuList = toolbarmain.Show();
        MenuDRSubTab.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            ViewState["DepDate"] = "";
            ViewState["ArrDate"] = "";

            ViewState["PORTCALLID"] = "";
            ViewState["VOYAGEID"] = "";
            ViewState["DEPARTUREPORT"] = "";

            VesselDepartureEdit();

            DataSet ds = PhoenixVesselPositionDepartureReport.GetEUMRVVesselType(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            ViewState["GASSTANKERYN"] = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FLDCODE"].ToString().ToUpper().Equals("GAS"))
                    ViewState["GASSTANKERYN"] = 1;
            }
            
        }
        ShowMenu();

    }

    protected void MenuDRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DEPARTURE"))
        {
            Response.Redirect("../VesselPosition/VesselPositionDepartureReportEdit.aspx");
        }
        if (CommandName.ToUpper().Equals("MRVSUMMARY"))
        {
            Response.Redirect("../VesselPosition/VesselPositionDepartureReportMRVSummary.aspx");
        }
        if (CommandName.ToUpper().Equals("DEPARTUREREPORT"))
        {
            if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                Response.Redirect("VesselPositionReports.aspx", false);
            else
                Response.Redirect("VesselPositionDepartureReport.aspx", false);
            //Response.Redirect("../VesselPosition/VesselPositionDepartureReport.aspx");
        }
    }
    protected void VesselDepartureEdit()
    {
        try
        {
            if (Filter.CurrentVPRSDepartureReportSelection != null)
            {
                DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureReport(
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection));

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
                        ViewState["DEPARTUREPORT"] = dr["FLDPORT"].ToString();
                        ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                        txtBallastCommenced.Text = dr["FLDBALLASTCOMMENCED"].ToString();
                        if (General.GetNullableDateTime(dr["FLDBALLASTCOMMENCED"].ToString()) != null)
                            txtBallastCommencedTime.SelectedDate = Convert.ToDateTime(dr["FLDBALLASTCOMMENCED"]);
                        txtBallastCompleted.Text = dr["FLDBALLASTCOMPLETED"].ToString();
                        if (General.GetNullableDateTime(dr["FLDBALLASTCOMPLETED"].ToString()) != null)
                            txtBallastCompletedTime.SelectedDate = Convert.ToDateTime(dr["FLDBALLASTCOMPLETED"]);

                        txtVolumeBallasted.Text = dr["FLDVOLUMEBALLASTED"].ToString();
                        txtDeballastCommenced.Text = dr["FLDDEBALLASTCOMMENCED"].ToString();
                        if (General.GetNullableDateTime(dr["FLDDEBALLASTCOMMENCED"].ToString()) != null)
                            txtDeballastCommencedTime.SelectedDate = Convert.ToDateTime(dr["FLDDEBALLASTCOMMENCED"]);
                        txtDeballastCompleted.Text = dr["FLDDEBALLASTCOMPLETED"].ToString();
                        if (General.GetNullableDateTime(dr["FLDDEBALLASTCOMPLETED"].ToString()) != null)
                            txtDeballastCompletedTime.SelectedDate = Convert.ToDateTime(dr["FLDDEBALLASTCOMPLETED"]);
                        txtVolumeDeballasted.Text = dr["FLDVOLUMEDEBALLASTED"].ToString();

                        ViewState["DepDate"] = dr["FLDVESSELDEPARTUREDATE"].ToString();
                        ViewState["ArrDate"] = dr["FLDLASTARRIVALDATE"].ToString();
                        chbCargoOperationExists.Checked = General.GetNullableInteger(dr["FLDCARGOOPERATIONYN"].ToString())  == 1 ? true : false;
                       // MenuOperation.Visible = General.GetNullableInteger(dr["FLDCARGOOPERATIONYN"].ToString()) == null ? true : false;
                        if (dr["FLDCONFIRMEDYN"].ToString() == "1")
                        {
                            ProjectBilling.Visible = false;
                            lblAlertSenttoOFC.Visible = true;
                         //   MenuOperation.Visible = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowMenu()
    {
        try
        {
            DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureReport(
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();

                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    
                    if ((General.GetNullableInteger(dr["FLDCARGOOPERATIONYN"].ToString()) == null || General.GetNullableInteger(dr["FLDCARGOOPERATIONYN"].ToString()) == 0) && (General.GetNullableInteger(dr["FLDCONFIRMEDYN"].ToString()) == 0 || General.GetNullableInteger(dr["FLDCONFIRMEDYN"].ToString()) == null))
                    {
                        
                        toolbar.AddButton("Discharge", "DISCHARGE", ToolBarDirection.Right);
                        toolbar.AddButton("Load", "LOAD", ToolBarDirection.Right);
                    }
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    ProjectBilling.AccessRights = this.ViewState;
                    ProjectBilling.MenuList = toolbar.Show();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chbCargoOperationExists_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidOperation())
            {
                ucError.Visible = true;
                return;
            }

            if (Filter.CurrentVPRSDepartureReportSelection != null)
            {
                UpdateDeparture();
                ShowMenu();
            }

            VesselDepartureEdit();
            RebindgvCargoOperation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void ProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOperation())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Filter.CurrentVPRSDepartureReportSelection != null)
                {
                    UpdateDeparture();
                    ucStatus.Text = "Departure report updated";
                }
               
                VesselDepartureEdit();
                RebindgvCargoOperation();
            }
            if (CommandName.ToUpper().Equals("LOAD"))
            {
                String scriptpopup = String.Format(
                 "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/VesselPosition/VesselPositionDepartureCargoOperations.aspx?vesselid="
             + ViewState["VESSELID"].ToString() + "&voyageid=" + ViewState["VOYAGEID"].ToString() + "&lanchfrom=LOAD');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (CommandName.ToUpper().Equals("DISCHARGE"))
            {
                String scriptpopup = String.Format(
                 "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionDepartureCargoOperations.aspx?vesselid="
             + ViewState["VESSELID"].ToString() + "&voyageid=" + ViewState["VOYAGEID"].ToString() + "&lanchfrom=DISCHARGE');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void UpdateDeparture()
    {
        string blstcommence = txtBallastCommencedTime.SelectedTime != null ? txtBallastCommencedTime.SelectedTime.Value.ToString() : "";
        string blstcomplete = txtBallastCompletedTime.SelectedTime != null ? txtBallastCompletedTime.SelectedTime.Value.ToString() : "";
        string deblstcommence = txtDeballastCommencedTime.SelectedTime != null ? txtDeballastCommencedTime.SelectedTime.Value.ToString() : "";
        string deblstcomplete = txtDeballastCompletedTime.SelectedTime != null ? txtDeballastCompletedTime.SelectedTime.Value.ToString() : "";

        int? NoCogoYn = General.GetNullableInteger(chbCargoOperationExists.Checked == true ? "1" : "");
            PhoenixVesselPositionDepartureReport.UpdateDepartureReportOperations(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableDateTime(txtBallastCommenced.Text + " " + blstcommence),
            General.GetNullableDateTime(txtBallastCompleted.Text + " " + blstcomplete),
            General.GetNullableDecimal(txtVolumeBallasted.Text),
            General.GetNullableDateTime(txtDeballastCommenced.Text + " " + deblstcommence),
            General.GetNullableDateTime(txtDeballastCompleted.Text + " " + deblstcomplete),
            General.GetNullableDecimal(txtVolumeDeballasted.Text), NoCogoYn);
    }

    private bool IsValidOperation()
    {
        string blstcommence = "", blstcomplete = "", deblstcommence = "", deblstcomplete;

        string blstcommencetime = txtBallastCommencedTime.SelectedTime != null ? txtBallastCommencedTime.SelectedTime.Value.ToString() : "";
        string blstcompletetime = txtBallastCompletedTime.SelectedTime != null ? txtBallastCompletedTime.SelectedTime.Value.ToString() : "";
        string deblstcommencetime = txtDeballastCommencedTime.SelectedTime != null ? txtDeballastCommencedTime.SelectedTime.Value.ToString() : "";
        string deblstcompletetime = txtDeballastCompletedTime.SelectedTime != null ? txtDeballastCompletedTime.SelectedTime.Value.ToString() : "";



        blstcommence = txtBallastCommenced.Text + " " + blstcommencetime;
        blstcomplete = txtBallastCompleted.Text + " " + blstcompletetime;

        deblstcommence = txtDeballastCommenced.Text + " " + deblstcommencetime;
        deblstcomplete = txtDeballastCompleted.Text + " " + deblstcompletetime;

        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(blstcommence))
            ucError.ErrorMessage = "Commenced Ballasting Date/Time cannot be later than Departure Date/Time.";

        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(blstcommence))
            ucError.ErrorMessage = "Commenced Ballasting Date/Time cannot be earlier than Arrival Date/Time.";

        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(blstcomplete))
            ucError.ErrorMessage = "Completed Ballasting Date/Time cannot be later than Departure Date/Time.";

        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(blstcomplete))
            ucError.ErrorMessage = "Completed Ballasting Date/Time cannot be earlier than Arrival Date/Time.";

        if (General.GetNullableDateTime(blstcommence) > General.GetNullableDateTime(blstcomplete))
            ucError.ErrorMessage = "Completed Ballasting Date/Time cannot be earlier than Commenced Date/Time.";


        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(deblstcommence))
            ucError.ErrorMessage = "Commenced Deballasting Date/Time cannot be later than Departure Date/Time.";

        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(deblstcommence))
            ucError.ErrorMessage = "Commenced Deballasting Date/Time cannot be earlier than Arrival Date/Time.";

        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(deblstcomplete))
            ucError.ErrorMessage = "Completed Deballasting Date/Time cannot be later than Departure Date/Time.";

        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(deblstcomplete))
            ucError.ErrorMessage = "Completed Deballasting Date/Time cannot be earlier than Arrival Date/Time.";

        if (General.GetNullableDateTime(deblstcommence) > General.GetNullableDateTime(deblstcomplete))
            ucError.ErrorMessage = "Completed Deballasting Date/Time cannot be earlier than Commenced Date/Time.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        RebindgvCargoOperation();
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        //BindServices();
    }

    //////////////////////////////////////// Other Operations..

    protected void gvOtherOpr_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Filter.CurrentVPRSDepartureReportSelection == null)
                {
                    ucError.ErrorMessage = "Please fill in the Departure Report Details and click on the save button.";
                    ucError.Visible = true;
                    return;
                }

                string value = "", Commenced = "", Completed = "";

                RadTextBox txtValueDateEditTime = ((RadTextBox)e.Item.FindControl("txtValueDateAddTime"));
                RadTimePicker txtCommencedEditTime = ((RadTimePicker)e.Item.FindControl("txtCommencedAddTime"));
                RadTimePicker txtCompletedEditTime = ((RadTimePicker)e.Item.FindControl("txtCompletedAddTime"));
                UserControlDate txtValueDateEdit = ((UserControlDate)e.Item.FindControl("txtValueDateAdd"));
                UserControlDate txtCommencedAdd = ((UserControlDate)e.Item.FindControl("txtCommencedAdd"));
                UserControlDate txtCompletedAdd = ((UserControlDate)e.Item.FindControl("txtCompletedAdd"));

                string time = txtValueDateEditTime.Text.Trim().Equals("__:__") ? string.Empty : txtValueDateEditTime.Text;
                string Commencedtime = txtCommencedEditTime.SelectedTime != null ? txtCommencedEditTime.SelectedTime.Value.ToString() : "";
                string Completedtime = txtCompletedEditTime.SelectedTime != null ? txtCompletedEditTime.SelectedTime.Value.ToString() : "";


                value = txtValueDateEdit.Text + " " + time;
                Commenced = txtCommencedAdd.Text + " " + Commencedtime;
                Completed = txtCompletedAdd.Text + " " + Completedtime;

                if (!ValidateDatetime(value, Commenced, Completed))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselPositionDepartureReport.UpdateDepartureOtherOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(""),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                    General.GetNullableDateTime(value),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text,
                     General.GetNullableDateTime(Commenced),
                     General.GetNullableDateTime(Completed)
                    );

                RebindgvOtherOpr();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionDepartureReport.DeleteDepartureOtherOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationId")).Text)));

                RebindgvOtherOpr();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool ValidateDatetime(string value, string Commenced, string Completed)
    {
              
        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(Commenced))
        {
            ucError.ErrorMessage = "Commenced Date/Time cannot be later than Dep Date/Time.";
        }
        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(Commenced))
        {
            ucError.ErrorMessage = "Commenced Date/Time cannot be earlier than Arr Date/Time.";
        }

        if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(Completed))
        {
            ucError.ErrorMessage = "Completed Date/Time cannot be later than Dep Date/Time.";
        }
        if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(Completed))
        {
            ucError.ErrorMessage = "Completed Date/Time cannot be earlier than Arr Date/Time.";
        }

        if (General.GetNullableDateTime(Commenced) > General.GetNullableDateTime(Completed))
        {
            ucError.ErrorMessage = " Completed Date/Time cannot be earlier than Commenced Date/Time.";
        }
        return (!ucError.IsError);
    }

    protected void gvOtherOpr_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (Filter.CurrentVPRSDepartureReportSelection == null)
            {
                ucError.ErrorMessage = "Please fill in the Departure Report Details and click on the save button.";
                ucError.Visible = true;
                return;
            }

            string value = "",Commenced = "", Completed = "";

            RadTextBox txtValueDateEditTime = ((RadTextBox)e.Item.FindControl("txtValueDateEditTime"));
            RadTimePicker txtCommencedEditTime = ((RadTimePicker)e.Item.FindControl("txtCommencedEditTime"));
            RadTimePicker txtCompletedEditTime = ((RadTimePicker)e.Item.FindControl("txtCompletedEditTime"));
            UserControlDate txtValueDateEdit = ((UserControlDate)e.Item.FindControl("txtValueDateEdit"));
            UserControlDate txtCommencedAdd = ((UserControlDate)e.Item.FindControl("txtCommencedEdit"));
            UserControlDate txtCompletedAdd = ((UserControlDate)e.Item.FindControl("txtCompletedEdit"));

            string time = txtValueDateEditTime.Text.Trim().Equals("__:__") ? string.Empty : txtValueDateEditTime.Text;
            string Commencedtime = txtCommencedEditTime.SelectedTime != null ? txtCommencedEditTime.SelectedTime.Value.ToString() : "";
            string Completedtime = txtCompletedEditTime.SelectedTime != null ? txtCompletedEditTime.SelectedTime.Value.ToString() : "";

            value = txtValueDateEdit.Text + " " + time;
            Commenced = txtCommencedAdd.Text + " " + Commencedtime;
            Completed = txtCompletedAdd.Text + " " + Completedtime;

            if (!ValidateDatetime(value, Commenced, Completed))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixVesselPositionDepartureReport.UpdateDepartureOtherOperation(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationIdEdit")).Text)),
                General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                General.GetNullableDateTime(value),
                ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text,
                General.GetNullableDateTime(Commenced),
                     General.GetNullableDateTime(Completed)
                );

            RebindgvOtherOpr();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherOpr_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblDescription");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucDescriptionTT");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdOperation");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Cargo Operations Grid..


    protected void gvCargoOperation_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("DISCHARGE"))
            {
                Guid? newOperationId = null;
               Guid? ddd= General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationId")).Text));

                PhoenixVesselPositionDepartureReport.UpdateDepartureOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationId")).Text)),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                    General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblCargoId")).Text)),
                    General.GetNullableDecimal(""),
                    General.GetNullableString((((RadLabel)e.Item.FindControl("lblOperationName")).Text)) == "Discharging" ? "DISCH" : "LOAD", 
                    General.GetNullableDateTime((((RadLabel)e.Item.FindControl("lblCommenced")).Text)),   
                    General.GetNullableDateTime((((RadLabel)e.Item.FindControl("lblCompleted")).Text)),
                    ((RadCheckBox)e.Item.FindControl("chkOilMajor")).Checked == true ? 1 : 0,
                     General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlOilMajor")).SelectedValue.ToString()),
                    General.GetNullableDecimal((((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text)), ref newOperationId, "MT",
                    General.GetNullableDecimal((((UserControlMaskNumber)e.Item.FindControl("txtVapourQty")).Text)),null);

                RebindgvCargoOperation();
                ShowMenu();
                ucStatus.Text = "Saved Successfully";
            }
          else  if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionDepartureReport.DeleteDepartureOperation(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
              , General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOperationId")).Text)));

                RebindgvCargoOperation();
                ShowMenu();
                ucStatus.Text = "Deleted Successfully";
            }
            //RebindgvCargoOperation();
        }
        catch (Exception ex)
        {
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

                RadCheckBox chkOilMajor = (RadCheckBox)e.Item.FindControl("chkOilMajor");
                RadComboBox ddlOilMajor = (RadComboBox)e.Item.FindControl("ddlOilMajor");

                LinkButton cmdOperation = (LinkButton)e.Item.FindControl("cmdOperation");


                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


                if (chkOilMajor != null)
                {
                    chkOilMajor.Checked = drv["FLDOILMAJORCARGOYN"].ToString() == "Yes" ? true : false;
                    if (drv["FLDOPERATIONNAME"].ToString().ToUpper().Equals("DISCHARGING"))
                    {
                        chkOilMajor.Enabled = false;
                    }
                }
                if (ddlOilMajor != null)
                {
                    ddlOilMajor.SelectedValue = drv["FLDOILMAJOR"].ToString();
                    if (drv["FLDOPERATIONNAME"].ToString().ToUpper().Equals("DISCHARGING"))
                    {
                        ddlOilMajor.Enabled = false;
                    }
                }

                if (cmdOperation != null)
                    cmdOperation.Attributes.Add("onclick",
                        "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/VesselPosition/VesselPositionDepartureCargoOperationsDetailEdit.aspx?operationid=" + lblOperationId.Text
                        + "&vesselid=" + lblVesselid.Text + "&voyageid=" + lblVoyageid.Text + "');return true;");

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
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELPOSITION + "'); return false;");
                }

                UserControlMaskNumber txtVapourQty = (UserControlMaskNumber)e.Item.FindControl("txtVapourQty");

                if (txtVapourQty != null)
                {
                    if (ViewState["GASSTANKERYN"].ToString() == "0" || drv["FLDOPERATIONNAME"].ToString().ToUpper().Equals("LOADING"))
                    {
                        txtVapourQty.Visible = false;
                    }
                }

                LinkButton dbd = (LinkButton)e.Item.FindControl("cmdDelete");
                if (dbd != null) dbd.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCargoOperation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureOperation(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection));

        gvCargoOperation.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            chbCargoOperationExists.Enabled = false;
        }
    }
    protected void RebindgvCargoOperation()
    {
        gvCargoOperation.SelectedIndexes.Clear();
        gvCargoOperation.EditIndexes.Clear();
        gvCargoOperation.DataSource = null;
        gvCargoOperation.Rebind();
    }

    protected void gvOtherOpr_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureOtherOperation(
           General.GetNullableInteger(ViewState["VESSELID"].ToString())
           , General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection == null ? "" : Filter.CurrentVPRSDepartureReportSelection));

            gvOtherOpr.DataSource = ds;

    }
    protected void RebindgvOtherOpr()
    {
        gvOtherOpr.SelectedIndexes.Clear();
        gvOtherOpr.EditIndexes.Clear();
        gvOtherOpr.DataSource = null;
        gvOtherOpr.Rebind();
    }
}
