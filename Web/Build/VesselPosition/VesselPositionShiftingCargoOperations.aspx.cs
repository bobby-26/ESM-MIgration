using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionShiftingCargoOperations : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {

            btnconfirm.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["DepDate"] = "";
            ViewState["ArrDate"] = "";
            if (Request.QueryString["lanchfrom"].ToString().ToUpper().Equals("LOAD"))
            {
                if (Request.QueryString["operationid"] != null)
                {
                    
                    ViewState["OPERATIONID"] = Request.QueryString["operationid"].ToString();

                    OperationEdit(new Guid(Request.QueryString["operationid"].ToString()));
                }

                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuCompanyList.AccessRights = this.ViewState;
                MenuCompanyList.MenuList = toolbar.Show();
            }
            else
            {
                MenuCompanyList.Visible = false;
            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }

            if (Request.QueryString["voyageid"] != null)
            {
                ViewState["VOYAGEID"] = Request.QueryString["voyageid"].ToString();
            }

            ucCargo.VesselId = int.Parse(ViewState["VESSELID"].ToString());
            ucCargo.CargoTypeShowYesNo = "1";

            DataSet ds = PhoenixVesselPositionDepartureReport.GetEUMRVVesselType(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            if (Request.QueryString["lanchfrom"] != null)
            {
                if (Request.QueryString["lanchfrom"].ToString().ToUpper().Equals("LOAD"))
                {
                    divDischarge.Visible = false;
                    divDischarge.Attributes.Add("Style", "display:none;");
                }
                if (Request.QueryString["lanchfrom"].ToString().ToUpper().Equals("DISCHARGE"))
                {
                    divLoad.Visible = false;
                    divLoad.Attributes.Add("Style", "display:none;");
                }
            }

            ViewState["GASSTANKERYN"] = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FLDCODE"].ToString().ToUpper().Equals("GAS"))
                    ViewState["GASSTANKERYN"] = 1;
            }

            ViewState["CURRENTROW"] = null;

            ViewState["CargoId"] = "";
            ViewState["Commenced"] = "";
            ViewState["Completed"] = "";
            ViewState["OilMajoryn"] = "";
            ViewState["OilMajor"] = "";
            ViewState["DischQty"] = "";
            ViewState["VapourQty"] = "";
            ViewState["stsyn"] = "";
        }
    }

    protected void VesselDepartureEdit()
    {
        if (Filter.CurrentVPRSShiftingReportSelection != null)
        {
            DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureReport(
                General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ViewState["DepDate"] = dr["FLDVESSELDEPARTUREDATE"].ToString();
                    ViewState["ArrDate"] = dr["FLDLASTARRIVALDATE"].ToString();
                }
            }
        }
    }

    private bool IsValidOperation()
    {
        string commenced = "", completed = "";
        string commencedtime = txtCommencedTime.SelectedTime != null ? txtCommencedTime.SelectedTime.Value.ToString() : "";
        string completedtime = txtCompletedTime.SelectedTime != null ? txtCompletedTime.SelectedTime.Value.ToString() : "";

        commenced = txtCommenced.Text + " " + commencedtime;
        completed = txtCompleted.Text + " " + completedtime;

        if ((ddlCargoOperation.SelectedValue == ""))
            ucError.ErrorMessage = "Cargo Operation is required";

        if (General.GetNullableGuid(ucCargo.SelectedCargoType) == null)
            ucError.ErrorMessage = "Cargo is required.";

        //if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(commenced))
        //    ucError.ErrorMessage = "Commenced Date/Time cannot be later than Shifting Date/Time.";

        //if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(commenced))
        //    ucError.ErrorMessage = "Commenced Date/Time cannot be earlier than Arr Date/Time.";

        //if (General.GetNullableDateTime(ViewState["DepDate"].ToString()) < General.GetNullableDateTime(completed))
        //    ucError.ErrorMessage = "Completed Date/Time cannot be later than Shifting Date/Time.";

        //if (General.GetNullableDateTime(ViewState["ArrDate"].ToString()) > General.GetNullableDateTime(completed))
        //    ucError.ErrorMessage = "Completed Date/Time cannot be earlier than Arr Date/Time.";

        //if (General.GetNullableDateTime(commenced) > General.GetNullableDateTime(completed))
        //    ucError.ErrorMessage = "Completed Date/Time cannot be earlier than Commenced Date/Time.";

        return (!ucError.IsError);
    }

    protected void AgentPortList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidOperation())
                {
                    ucError.Visible = true;
                    return;
                }
                string commencedtime = txtCommencedTime.SelectedTime != null ? txtCommencedTime.SelectedTime.Value.ToString() : "";
                string completedtime = txtCompletedTime.SelectedTime != null ? txtCompletedTime.SelectedTime.Value.ToString() : "";

                Guid? newOperationId = null;
                PhoenixVesselPositionDepartureReport.UpdateDepartureOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(ViewState["OPERATIONID"] != null ? ViewState["OPERATIONID"].ToString() : ""),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                    General.GetNullableGuid(ucCargo.SelectedCargoType),
                    General.GetNullableDecimal(""),
                    ddlCargoOperation.SelectedValue,
                    General.GetNullableDateTime(txtCommenced.Text + " " + commencedtime),
                    General.GetNullableDateTime(txtCompleted.Text + " " + completedtime),
                    chkOilMajor.Checked == true ? 1 : 0, General.GetNullableString(ddlOilMajor.SelectedValue.ToString())
                    , General.GetNullableDecimal(ucQuantity.Text), ref newOperationId,ddlUnit.SelectedValue.ToString(),null, chksts.Checked == true ? 1 : 0);

                ViewState["OPERATIONID"] = newOperationId;
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            String script = "javascript:fnReloadList('codehelp1',null);";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void OperationEdit(Guid operationid)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureOperation(operationid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucCargo.SelectedCargoType = dr["FLDCARGOID"].ToString();
            ddlCargoOperation.SelectedValue = dr["FLDOPERATION"].ToString();
            //txtParcelCount.Text = dr["FLDPARCELCOUNT"].ToString();
            txtCommenced.Text = dr["FLDCOMMENCED"].ToString();
            txtCompleted.Text = dr["FLDCOMPLETED"].ToString();
            if (General.GetNullableDateTime(dr["FLDCOMMENCED"].ToString()) != null)
                txtCommencedTime.SelectedDate = Convert.ToDateTime(dr["FLDCOMMENCED"]);
            if (General.GetNullableDateTime(dr["FLDCOMPLETED"].ToString()) != null)
                txtCompletedTime.SelectedDate = Convert.ToDateTime(dr["FLDCOMPLETED"]);
            ucQuantity.Text = dr["FLDQUANTITY"].ToString();
            ddlUnit.SelectedValue = dr["FLDUNIT"].ToString();
            if (dr["FLDOILMAJORCARGOYN"].ToString() == "1")
            {
                chkOilMajor.Checked = true;
            }
            else
            {
                chkOilMajor.Checked = false;
            }
            ddlOilMajor.SelectedValue = dr["FLDOILMAJOR"].ToString();
        }
    }

    ///////////////////////// Cargo Operations..


 
    protected void gvDischarge_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DISCH"))
            {
                RadLabel lblCargoId = ((RadLabel)e.Item.FindControl("lblCargoId"));
                RadTimePicker txtCommencedTimeItem = (RadTimePicker)e.Item.FindControl("txtCommencedTimeItem");
                RadTimePicker txtCompletedTimeItem = (RadTimePicker)e.Item.FindControl("txtCompletedTimeItem");
                RadLabel lblOilMajorCargo = (RadLabel)e.Item.FindControl("lblOilMajorCargoItem");
                RadLabel lblOilMajor = (RadLabel)e.Item.FindControl("lblOilMajorItem");
                UserControlDate txtCommencedItem = (UserControlDate)e.Item.FindControl("txtCommencedItem");
                UserControlDate txtCompletedItem = (UserControlDate)e.Item.FindControl("txtCompletedItem");

                UserControlMaskNumber ucDischQty = (UserControlMaskNumber)e.Item.FindControl("ucDischQty");
                string commencedtime = txtCommencedTimeItem.SelectedTime != null ? txtCommencedTimeItem.SelectedTime.Value.ToString() : "";
                string completedtime = txtCompletedTimeItem.SelectedTime != null ? txtCompletedTimeItem.SelectedTime.Value.ToString() : "";
                UserControlMaskNumber ucVapourQty = (UserControlMaskNumber)e.Item.FindControl("ucVapourQuantity");

                RadLabel lblQtyOnBoardItem = (RadLabel)e.Item.FindControl("lblQtyOnBoardItem");

                RadCheckBox chksts1 = (RadCheckBox)e.Item.FindControl("chksts");

                ViewState["CargoId"] = lblCargoId.Text;
                ViewState["Commenced"] = txtCommencedItem.Text + " " + commencedtime;
                ViewState["Completed"] = txtCompletedItem.Text + " " + completedtime;
                ViewState["OilMajoryn"] = lblOilMajorCargo.Text == "Yes" ? 1 : 0;
                ViewState["OilMajor"] = lblOilMajor.Text;
                ViewState["DischQty"] = ucDischQty.Text;
                ViewState["VapourQty"] = ucVapourQty.Text;
                ViewState["stsyn"] = chksts1.Checked == true ? 1 : 0;

                if (General.GetNullableDecimal(lblQtyOnBoardItem.Text.ToString()) > ((General.GetNullableDecimal(ucDischQty.Text) == null ? 0 : General.GetNullableDecimal(ucDischQty.Text)) + (General.GetNullableDecimal(ucVapourQty.Text) == null ? 0 : General.GetNullableDecimal(ucVapourQty.Text))))
                {
                    ViewState["CURRENTROW"] = e.Item.RowIndex;

                    string str = "Discharging Cargo Quantity(" + ((General.GetNullableDecimal(ucDischQty.Text) == null ? 0 : General.GetNullableDecimal(ucDischQty.Text)) + (General.GetNullableDecimal(ucVapourQty.Text) == null ? 0 : General.GetNullableDecimal(ucVapourQty.Text))).ToString() + ") is Less than the available Quantity(" + lblQtyOnBoardItem.Text.ToString() + ") .";

                    RadWindowManager1.RadConfirm(str + "Do you want to continue?", "btnconfirm", 320, 150, null, "Confirm");

                }
                else
                {

                    Guid? newOperationId = null;

                    PhoenixVesselPositionDepartureReport.UpdateDepartureOperation(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["OPERATIONID"] != null ? ViewState["OPERATIONID"].ToString() : ""),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                        General.GetNullableGuid(lblCargoId.Text),
                        General.GetNullableDecimal(""),
                        "DISCH",
                        General.GetNullableDateTime(txtCommencedItem.Text + " " + commencedtime),
                        General.GetNullableDateTime(txtCompletedItem.Text + " " + completedtime),
                        lblOilMajorCargo.Text == "Yes" ? 1 : 0, General.GetNullableString(lblOilMajor.Text),
                        General.GetNullableDecimal(ucDischQty.Text), ref newOperationId, ddlUnit.SelectedValue.ToString(),
                        General.GetNullableDecimal(ucVapourQty.Text), chksts1.Checked == true ? 1 : 0);

                    ViewState["OPERATIONID"] = newOperationId;

                    RebindgvDischarge();

                    String script = "javascript:fnReloadList('codehelp1',null);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDischarge_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                UserControlMaskNumber txtVapourQty = (UserControlMaskNumber)e.Item.FindControl("ucVapourQuantity");

                if (txtVapourQty != null)
                {
                    if (ViewState["GASSTANKERYN"].ToString() == "0")
                    {
                        txtVapourQty.Visible = false;
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
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

          //  if (ucCM.confirmboxvalue == 1)
            {

                Guid? newOperationId = null;

                PhoenixVesselPositionDepartureReport.UpdateDepartureOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(ViewState["OPERATIONID"] != null ? ViewState["OPERATIONID"].ToString() : ""),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                    General.GetNullableGuid(ViewState["CargoId"].ToString()),
                    General.GetNullableDecimal(""),
                    "DISCH",
                    General.GetNullableDateTime(ViewState["Commenced"].ToString()),
                    General.GetNullableDateTime(ViewState["Completed"].ToString()),
                    int.Parse(ViewState["OilMajoryn"].ToString()), General.GetNullableString(ViewState["OilMajor"].ToString()),
                    General.GetNullableDecimal(ViewState["DischQty"].ToString()), ref newOperationId, ddlUnit.SelectedValue.ToString(),
                    General.GetNullableDecimal(ViewState["VapourQty"].ToString()), int.Parse(ViewState["stsyn"].ToString()));

                ViewState["OPERATIONID"] = newOperationId;

                RebindgvDischarge();

                String script = "javascript:fnReloadList('codehelp1',null);";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvDischarge_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.CargoOnboardList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection));
        gvDischarge.DataSource = ds;
  
    }
    protected void RebindgvDischarge()
    {
        gvDischarge.SelectedIndexes.Clear();
        gvDischarge.EditIndexes.Clear();
        gvDischarge.DataSource = null;
        gvDischarge.Rebind();
    }
}
