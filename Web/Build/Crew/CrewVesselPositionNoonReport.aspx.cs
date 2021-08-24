using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewVesselPositionNoonReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuVesselPositionNoonReport.AccessRights = this.ViewState;
            MenuVesselPositionNoonReport.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["noonreportid"] != null)
                {
                    ViewState["NOONREPORTID"] = Request.QueryString["noonreportid"];
                    ViewState["OPERATIONMODE"] = "EDIT";
                }

                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                    ViewState["OPERATIONMODE"] = "ADD";
                }

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselPositionNoonReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDate(General.GetNullableDateTime(txtNoonReportDate.Text)))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixCrewVesselPosition.UpdateVesselNoonReport(int.Parse(ViewState["NOONREPORTID"].ToString())
                        , DateTime.Parse(txtNoonReportDate.Text)
                        , General.GetNullableInteger(ddlMessageType.SelectedHard)
                        , ucLatitude.Text
                        , ucLongitude.Text
                        , txtWindDirection.Text
                        , txtSeaDirection.Text
                        , General.GetNullableDecimal(ucWindForce.Text)
                        , General.GetNullableDecimal(ucSeaForce.Text)
                        , txtCurrDirection.Text
                        , ucCurrSpeed.Text
                        , txtSwellDirection.Text
                        , General.GetNullableDecimal(txtSwellHeight.Text)
                        , General.GetNullableDecimal(txtVesselCourse.Text)
                        , chkIsBallast.Checked == true ? 1 : 0
                        , txtCurrentCargo.Text
                        , txtCpSpeed.Text
                        , txtAverageSpeed.Text
                        , General.GetNullableDecimal(txtEngineRpm.Text)
                        , General.GetNullableDecimal(ucSeaTempreature.Text)
                        , txtRemarks.Text
                        , General.GetNullableDecimal(ucCpFo.Text)
                        , General.GetNullableDecimal(ucCpDo.Text)
                        , General.GetNullableDecimal(ucActualFo.Text)
                        , General.GetNullableDecimal(ucActualDo.Text)
                        );
                }
                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixCrewVesselPosition.InsertVesselNoonReport(int.Parse(ViewState["VESSELID"].ToString())
                        , DateTime.Parse(txtNoonReportDate.Text)
                        , General.GetNullableInteger(ddlMessageType.SelectedHard)
                        , ucLatitude.Text
                        , ucLongitude.Text
                        , txtWindDirection.Text
                        , txtSeaDirection.Text
                        , General.GetNullableDecimal(ucWindForce.Text)
                        , General.GetNullableDecimal(ucSeaForce.Text)
                        , txtCurrDirection.Text
                        , ucCurrSpeed.Text
                        , txtSwellDirection.Text
                        , General.GetNullableDecimal(txtSwellHeight.Text)
                        , General.GetNullableDecimal(txtVesselCourse.Text)
                        , chkIsBallast.Checked == true ? 1 : 0
                        , txtCurrentCargo.Text
                        , txtCpSpeed.Text
                        , txtAverageSpeed.Text
                        , General.GetNullableDecimal(txtEngineRpm.Text)
                        , General.GetNullableDecimal(ucSeaTempreature.Text)
                        , txtRemarks.Text
                        , General.GetNullableDecimal(ucCpFo.Text)
                        , General.GetNullableDecimal(ucCpDo.Text)
                        , General.GetNullableDecimal(ucActualFo.Text)
                        , General.GetNullableDecimal(ucActualDo.Text)
                        );
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('code1',null);", true);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            if (ViewState["NOONREPORTID"] != null)
            {   
                DataTable dt = PhoenixCrewVesselPosition.ListVesselNoonReport(int.Parse(ViewState["NOONREPORTID"].ToString()));
                DataRow dr = dt.Rows[0];

                ddlMessageType.SelectedHard = dr["FLDMESSAGETYPE"].ToString();
                txtNoonReportDate.Text = dr["FLDNOONREPORTDATE"].ToString();
                ucLatitude.Text = dr["FLDLATITUDE"].ToString();
                ucLongitude.Text = dr["FLDLONGITUDE"].ToString();
                txtWindDirection.Text = dr["FLDWINDDIRECTION"].ToString();
                txtSeaDirection.Text = dr["FLDSEADIRECTION"].ToString();
                ucWindForce.Text = dr["FLDWINDFORCE"].ToString();
                ucSeaForce.Text = dr["FLDSEAFORCE"].ToString();
                txtCurrDirection.Text = dr["FLDCURRENTDIRECTION"].ToString();
                ucCurrSpeed.Text = dr["FLDCURRENTSPEED"].ToString();
                txtSwellDirection.Text = dr["FLDSWELLDIRECTION"].ToString();
                txtSwellHeight.Text = dr["FLDSWELLHEIGHT"].ToString();
                txtVesselCourse.Text = dr["FLDVESSELCOURSE"].ToString();
                chkIsBallast.Checked = dr["FLDISBALLAST"].ToString().Equals("1");
                txtCurrentCargo.Text = dr["FLDCURRENTCARGO"].ToString();
                txtCpSpeed.Text = dr["FLDCPSPEED"].ToString();
                txtAverageSpeed.Text = dr["FLDAVERAGESPEED"].ToString();
                txtEngineRpm.Text = dr["FLDENGINERPM"].ToString();
                ucSeaTempreature.Text = dr["FLDSEATEMPERATURE"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucCpFo.Text = dr["FLDCPFO"].ToString();
                ucCpDo.Text = dr["FLDCPDO"].ToString();
                ucActualFo.Text = dr["FLDACTUALFO"].ToString();
                ucActualDo.Text = dr["FLDACTUALDO"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidDate(DateTime? noonreportdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (noonreportdate == null)
            ucError.ErrorMessage = "Date is required.";    

        return (!ucError.IsError);
    }

}
