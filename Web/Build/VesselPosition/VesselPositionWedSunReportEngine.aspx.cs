using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;

public partial class VesselPositionWedSunReportEngine : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvOtherOilCons.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvOtherOilCons.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        foreach (GridViewRow r in gvConsumption.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvConsumption.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
                toolbarvoyagetap.AddButton("List", "WEDSUNREPORTLIST");
                toolbarvoyagetap.AddButton("WedSun Report", "WEDSUNREPORT");
                MenuWedSunReportTap.AccessRights = this.ViewState;
                MenuWedSunReportTap.MenuList = toolbarvoyagetap.Show();
                MenuWedSunReportTap.SelectedMenuIndex = 1;

                PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
                toolbarsubtap.AddButton("Deck Dept", "DECK");
                toolbarsubtap.AddButton("Engine Dept", "ENGINE");
                MenuNRSubTab.AccessRights = this.ViewState;
                MenuNRSubTab.MenuList = toolbarsubtap.Show();
                MenuNRSubTab.SelectedMenuIndex = 1;

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                PhoenixToolbar toolbarWedSunReporttap = new PhoenixToolbar();
                toolbarWedSunReporttap.AddButton("Save", "SAVE");
                toolbarWedSunReporttap.AddButton("Copy", "COPY");
                MenuNewSaveTabStrip.AccessRights = this.ViewState;
                MenuNewSaveTabStrip.MenuList = toolbarWedSunReporttap.Show();

                //Reset();
                BindData();
                SetFieldRange();
            }

            BindOilConsumption();
            BindOtherOilConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        if (ViewState["VESSELID"].ToString() != "")
        {
            Guid? noonreportid = null;

            if (Session["NOONREPORTID"] != null)
                noonreportid = General.GetNullableGuid(Session["NOONREPORTID"].ToString());

            DataSet ds = PhoenixVesselPositionWedSunReport.ResetWedSunReport(int.Parse(ViewState["VESSELID"].ToString()), noonreportid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                ViewState["OLDNOONREPORTID"] = dt.Rows[0]["FLDNOONREPORTID"].ToString();

                txtEngineDistance.Text = dt.Rows[0]["FLDENGINEDISTANCE"].ToString();
                txtSWPress.Text = dt.Rows[0]["FLDSWPRESS"].ToString();
                txtMERPM.Text = dt.Rows[0]["FLDMERPM"].ToString();
                txtGovernorSetting.Text = dt.Rows[0]["FLDGOVERNORSETTING"].ToString();
                txtSpeedSetting.Text = dt.Rows[0]["FLDSPEEDSETTING"].ToString();
                txtRemarksCE.Text = dt.Rows[0]["FLDREMARKSCE"].ToString();
                txtSwellTemp.Text = dt.Rows[0]["FLDSWTEMP"].ToString();
                txtERExhTemp.Text = dt.Rows[0]["FLDERTEMP"].ToString();
                txtFOInletTemp.Text = dt.Rows[0]["FLDFOINLETTEMP"].ToString();
                txtFuelOilPress.Text = dt.Rows[0]["FLDFUELOILPRESS"].ToString();
                txtBilgeROB.Text = dt.Rows[0]["FLDBILGETANKROB"].ToString();
                txtSludgeROB.Text = dt.Rows[0]["FLDSLUDGETANKROB"].ToString();
                txtBoilerWaterChlorides.Text = dt.Rows[0]["FLDBOILERCHLORIDES"].ToString();
                txtHFOTankCleaning.Text = dt.Rows[0]["FLDHFOTANKCLEANING"].ToString();
                txtGeneralLoadAE1.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE1"]));
                txtGeneralLoadAE2.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE2"]));
                txtGeneralLoadAE3.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE3"]));
                txtHFOCargoHeating.Text = dt.Rows[0]["FLDHFOCARGOHEATING"].ToString(); 
            }
        }
    }

    private void BindOilConsumption()
    {
        DataSet ds = new DataSet();

        int? iVesselId = null;

        if (ViewState["VESSELID"].ToString() != "")
        {
            iVesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        }
        else
            iVesselId = 0;

        string noonreportid = "";

        noonreportid = Session["NOONREPORTID"] == null ? (ViewState["OLDNOONREPORTID"] != null ? ViewState["OLDNOONREPORTID"].ToString() : "") : Session["NOONREPORTID"].ToString();

        if (ViewState["COPY"] != null)
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOilConsumptionReset(iVesselId, General.GetNullableGuid(noonreportid));
        else
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOilConsumption(iVesselId, General.GetNullableGuid(noonreportid), "WEDSUN");
        

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvConsumption.DataSource = ds;
            gvConsumption.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvConsumption);
        }

    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                ViewState["COPY"] = "copy"; 
                Reset();
                BindOilConsumption();
                BindOtherOilConsumption();
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["COPY"] != null)
                {
                    if (gvOtherOilCons.EditIndex >= 0)
                    {
                        ucError.ErrorMessage = "Please Cancel the Edit Mode in the Fresh Water grid to Save the Copied Details";
                        ucError.Visible = true;
                        return;
                    }
                    if (gvConsumption.EditIndex >= 0)
                    {
                        ucError.ErrorMessage = "Please Cancel the Edit Mode in the Consumption grid to Save the Copied Details";
                        ucError.Visible = true;
                        return;
                    }
                }

                UpdateWedSunReport();

                if (Session["NOONREPORTID"] != null)
                {
                    BindData();
                    SetFieldRange();
                }

                if (ViewState["COPY"] != null)
                {
                    OilConsUpdate();
                    OtherOilConsUpdate();
                }

                ViewState["COPY"] = null;
                BindOilConsumption();
                BindOtherOilConsumption();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateWedSunReport()
    {
        //if (!IsValidNoonReport())
        //{
        //    ucError.Visible = true;
        //    return;
        //}

        string lastlandsludgetime = txtLastLandSludgeTime.Text.Trim().Equals("__:__") ? string.Empty : txtLastLandSludgeTime.Text;
        string BilgeLandingTime = txtBilgeLandingTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeLandingTime.Text;

        PhoenixVesselPositionWedSunReport.UpdateWedSunReportEngineDept(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(Session["NOONREPORTID"].ToString()),
            int.Parse(ViewState["VESSELID"].ToString()),
            General.GetNullableDecimal(txtSwellTemp.Text),
            General.GetNullableDecimal(txtEngineDistance.Text),
            null, //CalculateSlip(txtEngineDistance.Text, ViewState["DistObserved"].ToString()),
            General.GetNullableDecimal(txtERExhTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            txtRemarksCE.Text,
            General.GetNullableDecimal(txtGovernorSetting.Text),
            General.GetNullableDecimal(txtSpeedSetting.Text),
            General.GetNullableDecimal(txtFOInletTemp.Text),
            General.GetNullableDecimal(txtScavAirPress.Text),
            General.GetNullableDecimal(txtFuelOilPress.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtBoilerWaterChlorides.Text),
            General.GetNullableDecimal(txtBilgeROB.Text),
            General.GetNullableDecimal(txtSludgeROB.Text),
            General.GetNullableDateTime(txtLastLandSludge.Text + " " + lastlandsludgetime),
            General.GetNullableInteger(txtLastLandingDays.Text),
            General.GetNullableDecimal(txtTCRPMInboard.Text),
            General.GetNullableDecimal(txtTCRPMOutboard.Text),
            General.GetNullableDecimal(txtExhTCInboardBefore.Text),
            General.GetNullableDecimal(txtExhTCInboardAfter.Text),
            General.GetNullableDecimal(txtExhTCOutboardBefore.Text),
            General.GetNullableDecimal(txtExhTCOutboardAfter.Text),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(txtBHP.Text),
            General.GetNullableDecimal(txtScavAirTemp.Text),
            General.GetNullableDecimal(txtMaxExhTemp.Text), 
            General.GetNullableDecimal(txtMinExhTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtSWPress.Text),
            General.GetNullableDecimal(txtMERPM.Text),
            General.GetNullableDateTime(txtBilgeLanding.Text + " " + BilgeLandingTime),
            General.GetNullableInteger(txtBilgeLandingDays.Text),
            General.GetNullableDecimal(txtHFOTankCleaning.Text),
            General.GetNullableDecimal(txtHFOCargoHeating.Text),
            General.GetNullableDecimal(txtGeneralLoadAE1.Text),
            General.GetNullableDecimal(txtGeneralLoadAE2.Text),
            General.GetNullableDecimal(txtGeneralLoadAE3.Text),
            null,null,null,null,null,
            "WEDSUN");

        ucStatus.Text = "WedSun Report updated.";
    }

    protected void WedSunReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("WEDSUNREPORTLIST"))
        {
            Response.Redirect("VesselPositionNoonReportList.aspx", false);
        }
    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("DECK"))
        {
            Response.Redirect("VesselPositionWedSunReport.aspx", false);
        }
    }

    private void BindData()
    {
        string noonreportid = Session["NOONREPORTID"].ToString();
        DataSet ds = PhoenixVesselPositionWedSunReport.EditWedSunReport(General.GetNullableGuid(noonreportid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["VOYAGEID"] = dt.Rows[0]["FLDVOYAGEID"].ToString();

            txtSwellTemp.Text = dt.Rows[0]["FLDSWTEMP"].ToString();
            txtScavAirTemp.Text = dt.Rows[0]["FLDSCAVAIRTEMP"].ToString();
            txtMinExhTemp.Text = dt.Rows[0]["FLDMINEXHTEMP"].ToString();
            txtMaxExhTemp.Text = dt.Rows[0]["FLDMAXEXHTEMP"].ToString();
            txtERExhTemp.Text = dt.Rows[0]["FLDERTEMP"].ToString();
            txtScavAirPress.Text = dt.Rows[0]["FLDSCAVAIRPRESS"].ToString();
            txtSlip.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDSLIP"]));
            txtBHP.Text = dt.Rows[0]["FLDBHP"].ToString();
            txtTCRPMInboard.Text = dt.Rows[0]["FLDTCRPMINBOARD"].ToString();
            txtTCRPMOutboard.Text = dt.Rows[0]["FLDTCRPMOUTBOARD"].ToString();
            txtExhTCInboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDBEFORE"].ToString();
            txtExhTCInboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDAFTER"].ToString();
            txtExhTCOutboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDBEFORE"].ToString();
            txtExhTCOutboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDAFTER"].ToString();
            txtEngineDistance.Text = dt.Rows[0]["FLDENGINEDISTANCE"].ToString();
            txtRemarksCE.Text = dt.Rows[0]["FLDREMARKSCE"].ToString();

            txtMERPM.Text = dt.Rows[0]["FLDMERPM"].ToString();
            txtBilgeLandingDays.Text = dt.Rows[0]["FLDDAYSLASTLANDINGBILGE"].ToString();
            txtHFOTankCleaning.Text = dt.Rows[0]["FLDHFOTANKCLEANING"].ToString();
            txtBilgeLanding.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLASTLANDINGBILGE"].ToString());
            txtLastLandSludgeTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGSLUDGE"]);
            txtBilgeLandingTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGBILGE"]);

            txtGovernorSetting.Text = dt.Rows[0]["FLDGOVERNORSETTING"].ToString();
            txtSpeedSetting.Text = dt.Rows[0]["FLDSPEEDSETTING"].ToString();
            txtFOInletTemp.Text = dt.Rows[0]["FLDFOINLETTEMP"].ToString();
            txtSWPress.Text = dt.Rows[0]["FLDSWPRESS"].ToString();
            txtFuelOilPress.Text = dt.Rows[0]["FLDFUELOILPRESS"].ToString();
            txtBoilerWaterChlorides.Text = dt.Rows[0]["FLDBOILERCHLORIDES"].ToString();
            txtBilgeROB.Text = dt.Rows[0]["FLDBILGETANKROB"].ToString();
            txtSludgeROB.Text = dt.Rows[0]["FLDSLUDGETANKROB"].ToString();
            txtLastLandSludge.Text = dt.Rows[0]["FLDLASTLANDINGSLUDGE"].ToString();
            txtLastLandSludgeTime.Text = string.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGSLUDGE"]);
            txtLastLandingDays.Text = dt.Rows[0]["FLDDAYSFROMLASTLANDING"].ToString();
            txtTCRPMInboard.Text = dt.Rows[0]["FLDTCRPMINBOARD"].ToString();
            txtTCRPMOutboard.Text = dt.Rows[0]["FLDTCRPMOUTBOARD"].ToString();
            txtExhTCInboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDBEFORE"].ToString();
            txtExhTCInboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDAFTER"].ToString();
            txtExhTCOutboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDBEFORE"].ToString();
            txtExhTCOutboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDAFTER"].ToString();

            txtGeneralLoadAE1.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE1"]));
            txtGeneralLoadAE2.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE2"]));
            txtGeneralLoadAE3.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE3"]));
            txtHFOCargoHeating.Text = dt.Rows[0]["FLDHFOCARGOHEATING"].ToString(); 

            if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("INPORT") || dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATANCHOR"))
            {
                txtEngineDistance.Text = "";
                txtEngineDistance.CssClass = "readonlytextbox";
                txtEngineDistance.Enabled = false;

                txtSlip.Text = "";
                txtSlip.CssClass = "readonlytextbox";
                txtSlip.Enabled = false;

                txtSWPress.CssClass = "readonlytextbox";
                txtSWPress.Text = "";
                txtSWPress.Enabled = false;

                txtMERPM.Text = "";
                txtMERPM.CssClass = "readonlytextbox";
                txtMERPM.Enabled = false;

                txtBHP.Text = "";
                txtBHP.CssClass = "readonlytextbox";
                txtBHP.Enabled = false;

                txtGovernorSetting.Text = "";
                txtGovernorSetting.CssClass = "readonlytextbox";
                txtGovernorSetting.Enabled = false;

                txtSpeedSetting.Text = "";
                txtSpeedSetting.CssClass = "readonlytextbox";
                txtSpeedSetting.Enabled = false;

                txtMaxExhTemp.Text = "";
                txtMaxExhTemp.CssClass = "readonlytextbox";
                txtMaxExhTemp.Enabled = false;

                txtMinExhTemp.Text = "";
                txtMinExhTemp.CssClass = "readonlytextbox";
                txtMinExhTemp.Enabled = false;

                txtScavAirTemp.Text = "";
                txtScavAirTemp.CssClass = "readonlytextbox";
                txtScavAirTemp.Enabled = false;

                txtFOInletTemp.Text = "";
                txtFOInletTemp.CssClass = "readonlytextbox";
                txtFOInletTemp.Enabled = false;

                txtScavAirPress.Text = "";
                txtScavAirPress.CssClass = "readonlytextbox";
                txtScavAirPress.Enabled = false;

                txtFuelOilPress.Text = "";
                txtFuelOilPress.CssClass = "readonlytextbox";
                txtFuelOilPress.Enabled = false;

                txtTCRPMInboard.Text = "";
                txtTCRPMInboard.CssClass = "readonlytextbox";
                txtTCRPMInboard.Enabled = false;

                txtTCRPMOutboard.Text = "";
                txtTCRPMOutboard.CssClass = "readonlytextbox";
                txtTCRPMOutboard.Enabled = false;

                txtExhTCInboardAfter.Text = "";
                txtExhTCInboardAfter.CssClass = "readonlytextbox";
                txtExhTCInboardAfter.Enabled = false;

                txtExhTCInboardBefore.Text = "";
                txtExhTCInboardBefore.CssClass = "readonlytextbox";
                txtExhTCInboardBefore.Enabled = false;

                txtExhTCOutboardAfter.Text = "";
                txtExhTCOutboardAfter.CssClass = "readonlytextbox";
                txtExhTCOutboardAfter.Enabled = false;

                txtExhTCOutboardBefore.Text = "";
                txtExhTCOutboardBefore.CssClass = "readonlytextbox";
                txtExhTCOutboardBefore.Enabled = false;
            }
        }
    }

    protected void gvConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tbOilType = new TableCell();
            TableCell tbPreviousROB = new TableCell();
            TableCell tbAtSea = new TableCell();
            TableCell tbAtHourbour = new TableCell();
            TableCell tbInPort = new TableCell();
            TableCell tbLast = new TableCell();
            TableCell tbRobAtNoon = new TableCell();
            TableCell tbAction = new TableCell();

            tbOilType.ColumnSpan = 1;
            tbPreviousROB.ColumnSpan = 1;
            tbAtSea.ColumnSpan = 6;
            tbAtHourbour.ColumnSpan = 6;
            tbInPort.ColumnSpan = 6;

            tbOilType.Text = " Oil Type";
            tbPreviousROB.Text = "Previous ROB";
            tbAtSea.Text = "AT SEA";
            tbInPort.Text = "IN PORT";
            tbLast.Text = "Total";
            tbRobAtNoon.Text = "ROB at Noon";
            tbAction.Text = "Action";

            tbLast.Attributes.Add("style", "text-align:center");
            tbRobAtNoon.Attributes.Add("style", "text-align:center");
            tbOilType.Attributes.Add("style", "text-align:center");
            tbPreviousROB.Attributes.Add("style", "text-align:center");
            tbAtSea.Attributes.Add("style", "text-align:center");
            tbInPort.Attributes.Add("style", "text-align:center");
            tbAction.Attributes.Add("style", "text-align:center");

            gv.Cells.Add(tbOilType);
            gv.Cells.Add(tbPreviousROB);
            gv.Cells.Add(tbAtSea);
            gv.Cells.Add(tbInPort);
            gv.Cells.Add(tbLast);
            gv.Cells.Add(tbRobAtNoon);
            gv.Cells.Add(tbAction);
            gvConsumption.Controls[0].Controls.AddAt(0, gv);
        }

        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvConsumption, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    protected void gvConsumption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["COPY"] != null)
                {
                    ucError.ErrorMessage = "Please save the copied details first.";
                    ucError.Visible = true;
                    return;
                }

                if (Session["NOONREPORTID"] != null)
                {
                    PhoenixVesselPositionNoonReportOilConsumption.DeleteOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeid")).Text));

                    PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

                    PhoenixVesselPositionNoonReport.UpdateESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));
                }

                BindOilConsumption();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumption_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");

            if (edit != null)
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null)
                {
                    Label ldyn = (Label)e.Row.FindControl("lbloilconsumptiononlaterdateyn");
                    if (ldyn != null)
                    {
                        if (ldyn.Text == "1")
                            sb.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Please confirm you want to proceed ?'); return false;");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvConsumption_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindOilConsumption();
    }

    protected void gvConsumption_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        int Ei = _gridView.EditIndex;
        //if (Ei != -1)
        //    gvConsumptionUpdate(sender, Ei);

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindOilConsumption();
    }

    private void OilConsUpdate()
    {

        foreach (GridViewRow gvr in gvConsumption.Rows)
        {
            PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((Label)gvr.FindControl("lblOilTypeid")).Text),
                "WEDSUN",
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaME")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaAE")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaBLR")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaIGG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaCARGOENG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtSeaOTH")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourME")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourAE")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourBLR")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourIGG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourCARGOENG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblAtHourbourOTH")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortME")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortAE")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortBLR")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortIGG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortCARGOENG")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblInPortOTH")).Text),
                null,
                null,
                1, // ROBATNOON YN
                0, // ROB @ EOSP AND ROB @ FWE YN
                0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                1, // RECALCULATE ROB
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                );

            PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                General.GetNullableGuid(Session["NOONREPORTID"].ToString()));
        }
    }

    protected void gvConsumption_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["NOONREPORTID"] == null)
            {
                ucError.ErrorMessage = "Please click the save button for the 'WedSun Report' and then update the Consumption details.";
                ucError.Visible = true;
                return;
            }

            if (ViewState["COPY"] != null)
            {
                ucError.ErrorMessage = "Please save the copied details first.";
                ucError.Visible = true;
                return;
            }

            if (((Label)_gridView.Rows[nCurrentRow].FindControl("lbloilconsumptiononlaterdateyn")).Text == "1")
            {
                ReCalculateROB(nCurrentRow);
            }
            else
            {
                PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    new Guid(Session["NOONREPORTID"].ToString()),
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeid")).Text),
                    "WEDSUN",
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtSeaOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAtHourbourOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucInPortOTHEdit")).Text),
                    null,
                    null,
                    1, // ROBATNOON YN
                    0, // ROB @ EOSP AND ROB @ FWE YN
                    0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                    0, // RECALCULATE ROB
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                    );


                PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

                PhoenixVesselPositionNoonReport.UpdateESIRegister(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

                _gridView.EditIndex = -1;
                BindOilConsumption();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumption_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        //BindData();
        //SetPageNavigator();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
        //SetPageNavigator();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private decimal? CalculateLogSpeed(string fullspeed, string fullspeeddistance, string reducedspeed, string reduceedspeeddistance)
    {
        decimal? logspeed = 0;
        decimal? fs = General.GetNullableDecimal(fullspeed);
        decimal? rs = General.GetNullableDecimal(reducedspeed);
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);
        decimal? speed = (fs == null ? 0 : fs) + (rs == null ? 0 : rs);

        if (speed > 0)
            logspeed = distance / speed;

        return logspeed;
    }

    private decimal? CalculateDistanceObserved(string fullspeeddistance, string reduceedspeeddistance)
    {
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);

        return distance;
    }

    private decimal? CalculateSlip(string enginedist, string distobserved)
    {
        decimal? engdist = General.GetNullableDecimal(enginedist);
        decimal? distobs = General.GetNullableDecimal(distobserved);

        decimal? slip = 0;

        if (engdist != null && engdist != 0)
            slip = ((engdist - (distobs == null ? 0 : distobs)) / engdist) * 100;

        return slip;
    }

    // bug id: 9826 - WedSun Report Field range configuration..

    private void SetFieldRange()
    {
        DataSet ds = PhoenixRegistersNoonReportRangeConfig.ListNoonReportRangeConfig(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            1);

        if (ds.Tables.Count > 0)
        {
            decimal? minvalue = null;
            decimal? maxvalue = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                minvalue = General.GetNullableDecimal(dr["FLDMINVALUE"].ToString());
                maxvalue = General.GetNullableDecimal(dr["FLDMAXVALUE"].ToString());

                switch (dr["FLDCOLUMNNAME"].ToString())
                {
                    case "FLDSWTEMP":
                        {
                            if (General.GetNullableDecimal(txtSwellTemp.Text) < minvalue || General.GetNullableDecimal(txtSwellTemp.Text) > maxvalue)
                                txtSwellTemp.CssClass = "maxhighlight";
                            else
                                txtSwellTemp.CssClass = "input";
                            break;
                        }
                    case "FLDENGINEDISTANCE":
                        {
                            if (General.GetNullableDecimal(txtEngineDistance.Text) < minvalue || General.GetNullableDecimal(txtEngineDistance.Text) > maxvalue)
                                txtEngineDistance.CssClass = "maxhighlight";
                            else
                                txtEngineDistance.CssClass = "input";
                            break;
                        }
                    case "FLDSLIP":
                        {
                            if (General.GetNullableDecimal(txtSlip.Text) < minvalue || General.GetNullableDecimal(txtSlip.Text) > maxvalue)
                                txtSlip.CssClass = "maxhighlight";
                            else
                                txtSlip.CssClass = "input";
                            break;
                        }
                    case "FLDSCAVAIRTEMP":
                        {
                            if (General.GetNullableDecimal(txtScavAirTemp.Text) < minvalue || General.GetNullableDecimal(txtScavAirTemp.Text) > maxvalue)
                                txtScavAirTemp.CssClass = "maxhighlight";
                            else
                                txtScavAirTemp.CssClass = "input";
                            break;
                        }
                    case "FLDMAXEXHTEMP":
                        {
                            if (General.GetNullableDecimal(txtMaxExhTemp.Text) < minvalue || General.GetNullableDecimal(txtMaxExhTemp.Text) > maxvalue)
                                txtMaxExhTemp.CssClass = "maxhighlight";
                            else
                                txtMaxExhTemp.CssClass = "input";

                            if (General.GetNullableDecimal(txtMinExhTemp.Text) < minvalue || General.GetNullableDecimal(txtMinExhTemp.Text) > maxvalue)
                                txtMinExhTemp.CssClass = "maxhighlight";
                            else
                                txtMinExhTemp.CssClass = "input";
                            break;
                        }
                    case "FLDERTEMP":
                        {
                            if (General.GetNullableDecimal(txtERExhTemp.Text) < minvalue || General.GetNullableDecimal(txtERExhTemp.Text) > maxvalue)
                                txtERExhTemp.CssClass = "maxhighlight";
                            else
                                txtERExhTemp.CssClass = "input";
                            break;
                        }
                    case "FLDSCAVAIRPRESS":
                        {
                            if (General.GetNullableDecimal(txtScavAirPress.Text) < minvalue || General.GetNullableDecimal(txtScavAirPress.Text) > maxvalue)
                                txtScavAirPress.CssClass = "maxhighlight";
                            else
                                txtScavAirPress.CssClass = "input";
                            break;
                        }
                    case "FLDBHP":
                        {
                            if (General.GetNullableDecimal(txtBHP.Text) < minvalue || General.GetNullableDecimal(txtBHP.Text) > maxvalue)
                                txtBHP.CssClass = "maxhighlight";
                            else
                                txtBHP.CssClass = "input";
                            break;
                        }
                    case "FLDTCRPMINBOARD":
                        {
                            if (General.GetNullableDecimal(txtTCRPMInboard.Text) < minvalue || General.GetNullableDecimal(txtTCRPMInboard.Text) > maxvalue)
                                txtTCRPMInboard.CssClass = "maxhighlight";
                            else
                                txtTCRPMInboard.CssClass = "input";

                            if (General.GetNullableDecimal(txtTCRPMOutboard.Text) < minvalue || General.GetNullableDecimal(txtTCRPMOutboard.Text) > maxvalue)
                                txtTCRPMOutboard.CssClass = "maxhighlight";
                            else
                                txtTCRPMOutboard.CssClass = "input";
                            break;
                        }
                    case "FLDEXHGASTEMPTCINBOARDBEFORE":
                        {
                            if (General.GetNullableDecimal(txtExhTCInboardBefore.Text) < minvalue || General.GetNullableDecimal(txtExhTCInboardBefore.Text) > maxvalue)
                                txtExhTCInboardBefore.CssClass = "maxhighlight";
                            else
                                txtExhTCInboardBefore.CssClass = "input";

                            if (General.GetNullableDecimal(txtExhTCOutboardBefore.Text) < minvalue || General.GetNullableDecimal(txtExhTCOutboardBefore.Text) > maxvalue)
                                txtExhTCOutboardBefore.CssClass = "maxhighlight";
                            else
                                txtExhTCOutboardBefore.CssClass = "input";
                            break;
                        }
                    case "FLDEXHGASTEMPTCINBOARDAFTER":
                        {
                            if (General.GetNullableDecimal(txtExhTCInboardAfter.Text) < minvalue || General.GetNullableDecimal(txtExhTCInboardAfter.Text) > maxvalue)
                                txtExhTCInboardAfter.CssClass = "maxhighlight";
                            else
                                txtExhTCInboardAfter.CssClass = "input";

                            if (General.GetNullableDecimal(txtExhTCOutboardAfter.Text) < minvalue || General.GetNullableDecimal(txtExhTCOutboardAfter.Text) > maxvalue)
                                txtExhTCOutboardAfter.CssClass = "maxhighlight";
                            else
                                txtExhTCOutboardAfter.CssClass = "input";
                            break;
                        }
                }
            }
        }
    }

    // Other oil consumption..

    private void BindOtherOilConsumption()
    {
        DataSet ds = new DataSet();

        int? iVesselId = null;

        if (ViewState["VESSELID"].ToString() != "")
        {
            iVesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        }
        else
            iVesselId = 0;

        string noonreportid = "";

        noonreportid = Session["NOONREPORTID"] == null ? (ViewState["OLDNOONREPORTID"] != null ? ViewState["OLDNOONREPORTID"].ToString() : "") : Session["NOONREPORTID"].ToString();

        if (ViewState["COPY"] != null)
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOtherOilConsumptionReset(iVesselId, General.GetNullableGuid(noonreportid));
        else
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOtherOilConsumption(iVesselId, General.GetNullableGuid(noonreportid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOtherOilCons.DataSource = ds;
            gvOtherOilCons.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOtherOilCons);
        }
    }

    protected void gvOtherOilCons_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["COPY"] != null)
                {
                    ucError.ErrorMessage = "Please save the copied details first.";
                    ucError.Visible = true;
                    return;
                }
                if (Session["NOONREPORTID"] != null)
                {
                    string OilConsumptionId = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilConsumptionId")).Text);
                    if (OilConsumptionId != "")
                        PhoenixVesselPositionNoonReportOilConsumption.DeleteOtherOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(OilConsumptionId));

                    BindOtherOilConsumption();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherOilCons_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOtherOilCons_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindOtherOilConsumption();
    }

    protected void gvOtherOilCons_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        int Ei = _gridView.EditIndex;
        //if (Ei != -1)
        //    gvOtherOilConsUpdate(sender, Ei);

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindOtherOilConsumption();
    }

    private void gvOtherOilConsUpdate(object sender, int RowIndex)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = RowIndex;
            if (Session["NOONREPORTID"] == null)
            {
                ucError.ErrorMessage = "Please click the save button for the 'WedSun Report' and then update the Consumption details.";
                ucError.Visible = true;
                return;
            }
            if (ViewState["COPY"] != null)
            {
                ucError.ErrorMessage = "Please save the copied details first.";
                ucError.Visible = true;
                return;
            }
            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeCodeEdit")).Text),
                "WEDSUN",
                General.GetNullableDecimal(((Label)_gridView.Rows[nCurrentRow].FindControl("lblConsumptionEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtROBEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtProducedEdit")).Text),
                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilConsumptionIdEdit")).Text));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void OtherOilConsUpdate()
    {
        foreach (GridViewRow gvr in gvOtherOilCons.Rows)
        {

            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((Label)gvr.FindControl("lblOilTypeCode")).Text),
                "WEDSUN",
                General.GetNullableDecimal(((Label)gvr.FindControl("lblConsumption")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblROB")).Text),
                General.GetNullableDecimal(((Label)gvr.FindControl("lblProduced")).Text),
                General.GetNullableGuid(((Label)gvr.FindControl("lblOilConsumptionId")).Text));
        }
    }

    protected void gvOtherOilCons_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["NOONREPORTID"] == null)
            {
                ucError.ErrorMessage = "Please click the save button for the 'WedSun Report' and then update the Consumption details.";
                ucError.Visible = true;
                return;
            }

            if (ViewState["COPY"] != null)
            {
                ucError.ErrorMessage = "Please save the copied details first.";
                ucError.Visible = true;
                return;
            }

            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeCodeEdit")).Text),
                "WEDSUN",
                General.GetNullableDecimal(((Label)_gridView.Rows[nCurrentRow].FindControl("lblConsumptionEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtROBEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtProducedEdit")).Text),
                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilConsumptionIdEdit")).Text));

            _gridView.EditIndex = -1;
            BindOtherOilConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherOilCons_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindOtherOilConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherOilCons_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOtherOilCons, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void ReCalculateROB(int cRow)
    {
        try
        {
            int nCurrentRow = cRow;
            GridViewRow gvr = gvConsumption.Rows[nCurrentRow];

            PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((Label)gvr.FindControl("lblOilTypeid")).Text),
                "WEDSUN",
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtSeaOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucAtHourbourOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("ucInPortOTHEdit")).Text),
                null,
                null,
                1, // ROBATNOON YN
                0, // ROB @ EOSP AND ROB @ FWE YN
                0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                1, // RECALCULATE ROB
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                );

            PhoenixVesselPositionNoonReport.InsertESIRegister(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            ucStatus.Text = "ROB Recalculated";

            gvConsumption.EditIndex = -1;
            BindOilConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
