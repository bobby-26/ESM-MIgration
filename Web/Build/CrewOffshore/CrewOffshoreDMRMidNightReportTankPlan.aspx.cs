using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReportTankPlan : PhoenixBasePage
{
    public string unpumpableAlert = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbarReporttap.AddButton("Tank Plan Report", "REPORT", ToolBarDirection.Right);
            toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
           
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "MIDNIGHTREPORTLIST");
            toolbarvoyagetap.AddButton("MidNight Report", "MIDNIGHTREPORT");
            toolbarvoyagetap.AddButton("Tank Plan", "TANKPLAN");
            toolbarvoyagetap.AddButton("HSE", "HSE");
            toolbarvoyagetap.AddButton("Passenger List", "PASSENGERLIST");
            toolbarvoyagetap.AddButton("Crew", "CREW");
            toolbarvoyagetap.AddButton("Technical", "TECHNICAL");

            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();
            MenuReportTap.SelectedMenuIndex = 2;

            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;

                }
                else
                {
                    ViewState["VESSELID"] = "";
                    ucVessel.Enabled = false;
                }

                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();
                }
                else
                {
                    ucError.ErrorMessage = "Midnight Reprot is not yet saved.Please Save the Midnight Report First";
                    ucError.Visible = true;
                    return;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreInitailMidNightReportYN(int.Parse(Session["VESSELID"].ToString()), General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
                ViewState["INITIALTANKPLANYN"] = ds.Tables[0].Rows[0]["FLDINITIALTANKPLANYN"].ToString();

                DisableControls();
                //BindMethanolTanks();
                BindOilLoadandConsumption();
            }
            BindDryBulkCargoSummary();
            BindLiquidBulkCargoSummary();
            BindMethanolSummary();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void EditMidNightReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(new Guid(Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["REPORTDATE"] = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            txtDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            txtCrew.Text = ds.Tables[0].Rows[0]["FLDCREW"].ToString();
            Session["MIDNIGHTREPORTID"] = ds.Tables[0].Rows[0]["FLDMIDNIGHTREPORTID"].ToString();

        }
    }

    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("MIDNIGHTREPORT"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }

        if (CommandName.ToUpper().Equals("MIDNIGHTREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("HSE"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportHSE.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("CREW"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportCrew.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportTechnical.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("PASSENGERLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportPassengerList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ucStatus.Text = "Midnight Report Saved";
        //UpdateMethanolTank();
        BindOilLoadandConsumption();
        BindDryBulkCargoSummary();
        BindLiquidBulkCargoSummary();
        BindMethanolSummary();
        
    }
    public void BindDryBulkCargoSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreDMRDryBulkCargoSummary(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()),int.Parse(ViewState["VESSELID"].ToString()),"DB");
            gvDryBulk.DataSource = ds;
            gvDryBulk.DataBind();
           
             
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDryBulk_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            TableCell tbf = new TableCell();
            tbf.ColumnSpan = 5;
            tbf.Text = "Cargo Summary";
            tbf.Attributes.Add("style", "text-align:center;");
            gv.Cells.Add(tbf);
            gvDryBulk.Controls[0].Controls.AddAt(0, gv);
        }
    }
    public void BindLiquidBulkCargoSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreDMRDryBulkCargoSummary(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()),int.Parse(ViewState["VESSELID"].ToString()), "WB");

            gvLiquidBulk.DataSource = ds;
            gvLiquidBulk.DataBind();
           

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindMethanolSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreDMRDryBulkCargoSummary(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()),int.Parse(ViewState["VESSELID"].ToString()), "MNL");
            gvMethanol.DataSource = ds;
            gvMethanol.DataBind();
         

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindOilLoadandConsumption()
    {
        try
        {
            unpumpableAlert = "";
            int generalalertdays;
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()),int.Parse(ViewState["VESSELID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FLDVERTICALVALUE"].ToString() == "1" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP1.Disabled = false;
                        lblTankNoP1I.Visible = true;
                        lblTankNoP1.Visible = true;
                        lbl100VolP1I.Visible = true;
                        lbl100VolP1.Visible = true;
                        lbl85VolP1I.Visible = true;
                        lbl85VolP1.Visible = true;
                        lblProductP1I.Visible = true;
                        lblProductP1.Visible = true;
                        lblSpcGravityP1I.Visible = true;
                        lblSpcGravityP1.Visible = true;
                        lblDateLoadedP1.Visible = true;
                        ucDateLoadedP1.Visible = true;
                        lblROBMTP1.Visible = true;
                        ucROBMTP1.Visible = true;
                        lblROBCUMP1.Visible = true;
                        ucROBCUMP1.Visible = true;
                        lblTankCleanedP1.Visible = true;
                        chkTankCleanedP1.Visible = true;
                        lblDateP1.Visible = true;
                        ucDateP1.Visible = true;
                        //ucDateP1.Enabled = false;
                        ucROBCUMP1.Enabled = false;
                        lblUnpumpableYNP1.Visible = true;
                        chkUnpumpableP1.Visible = true;
                        lblpostponealertP1.Visible = true;
                        chkpostponealertP1.Visible = true;
                        lblpostponealertremarksP1.Visible = true;
                        txtpostponeexpremarksP1.Visible = true;
                        txtpostponeexpremarksP1.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();

                        hdnConfiguratoinIDP1.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP1.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP1.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP1.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP1.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP1.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP1.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP1.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        //lblProductP1.Text = dr["FLDOILTYPENAME"].ToString();
                        lblSpcGravityP1.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP1.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP1.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP1.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP1.SelectedProduct = "Dummy";
                            lblSpcGravityP1.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP1.Enabled = false;
                            ucROBMTP1.CssClass = "readonlytextbox";
                            ucROBCUMP1.Enabled = true;
                            ucROBCUMP1.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP1.Enabled = true;
                            ucROBMTP1.CssClass = "input";
                            ucROBCUMP1.Enabled = false;
                            ucROBCUMP1.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP1.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP1.Checked == true)
                            ucDateP1.Enabled = true;
                        ucDateP1.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP1.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP1.Visible = true;
                            ucROBCharterUnitP1.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP1.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        chkpostponealertP1.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP1.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }

                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP1.Style.Add("backcolor", "RED");
                        //lblProductP1.CssClass. .BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "1" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI1.Disabled = false;
                        lblTankNoPI1I.Visible = true;
                        lblTankNoPI1.Visible = true;
                        lbl100VolPI1I.Visible = true;
                        lbl100VolPI1.Visible = true;
                        lbl85VolPI1I.Visible = true;
                        lbl85VolPI1.Visible = true;
                        lblProductPI1I.Visible = true;
                        lblProductPI1.Visible = true;
                        lblSpcGravityPI1I.Visible = true;
                        lblSpcGravityPI1.Visible = true;
                        lblDateLoadedPI1.Visible = true;
                        ucDateLoadedPI1.Visible = true;
                        lblROBMTPI1.Visible = true;
                        ucROBMTPI1.Visible = true;
                        lblROBCUMPI1.Visible = true;
                        ucROBCUMPI1.Visible = true;
                        lblTankCleanedPI1.Visible = true;
                        chkTankCleanedPI1.Visible = true;
                        lblDatePI1.Visible = true;
                        ucDatePI1.Visible = true;
                        lblUnpumpableYNPI1.Visible = true;
                        chkUnpumpablePI1.Visible = true;
                        lblpostponealertPI1.Visible = true;
                        chkpostponealertPI1.Visible = true;
                        lblpostponealertremarksPI1.Visible = true;
                        txtpostponeexpremarksPI1.Visible = true;
                        
                       
                        hdnConfiguratoinIDPI1.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI1.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI1.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI1.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI1.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI1.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI1.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI1.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI1.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI1.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI1.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI1.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI1.SelectedProduct = "Dummy";
                            lblSpcGravityPI1.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI1.Enabled = false;
                            ucROBMTPI1.CssClass = "readonlytextbox";
                            ucROBCUMPI1.Enabled = true;
                            ucROBCUMPI1.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI1.Enabled = true;
                            ucROBMTPI1.CssClass = "input";
                            ucROBCUMPI1.Enabled = false;
                            ucROBCUMPI1.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI1.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI1.Checked == true)
                            ucDatePI1.Enabled = true;
                        ucDatePI1.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI1.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI1.Visible = true;
                            ucROBCharterUnitPI1.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI1.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI1.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI1.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI1.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI1.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "1" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC1.Disabled = false;
                        lblTankNoC1I.Visible = true;
                        lblTankNoC1.Visible = true;
                        lbl100VolC1I.Visible = true;
                        lbl100VolC1.Visible = true;
                        lbl85VolC1I.Visible = true;
                        lbl85VolC1.Visible = true;
                        lblProductC1I.Visible = true;
                        lblProductC1.Visible = true;
                        lblSpcGravityC1I.Visible = true;
                        lblSpcGravityC1.Visible = true;
                        lblDateLoadedC1.Visible = true;
                        ucDateLoadedC1.Visible = true;
                        lblROBMTC1.Visible = true;
                        ucROBMTC1.Visible = true;
                        lblROBCUMC1.Visible = true;
                        ucROBCUMC1.Visible = true;
                        lblTankCleanedC1.Visible = true;
                        chkTankCleanedC1.Visible = true;
                        lblDateC1.Visible = true;
                        ucDateC1.Visible = true;
                        lblUnpumpableYNC1.Visible = true;
                        chkUnpumpableC1.Visible = true;
                        lblpostponealertC1.Visible = true;
                        chkpostponealertC1.Visible = true;
                        lblpostponealertremarksC1.Visible = true;
                        txtpostponeexpremarksC1.Visible = true;                        

                        hdnConfiguratoinIDC1.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC1.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC1.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC1.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC1.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC1.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC1.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC1.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC1.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC1.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC1.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC1.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC1.SelectedProduct = "Dummy";
                            lblSpcGravityC1.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC1.Enabled = false;
                            ucROBCUMC1.Enabled = true;
                            ucROBMTC1.CssClass = "readonlytextbox";
                            ucROBCUMC1.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC1.Enabled = true;
                            ucROBCUMC1.Enabled = false;
                            ucROBMTC1.CssClass = "input";
                            ucROBCUMC1.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC1.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC1.Checked == true)
                            ucDateC1.Enabled = true;
                        ucDateC1.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC1.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC1.Visible = true;
                            ucROBCharterUnitC1.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC1.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC1.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC1.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC1.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC1.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "1" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI1.Disabled = false;
                        lblTankNoSI1I.Visible = true;
                        lblTankNoSI1.Visible = true;
                        lbl100VolSI1I.Visible = true;
                        lbl100VolSI1.Visible = true;
                        lbl85VolSI1I.Visible = true;
                        lbl85VolSI1.Visible = true;
                        lblProductSI1I.Visible = true;
                        lblProductSI1.Visible = true;
                        lblSpcGravitySI1I.Visible = true;
                        lblSpcGravitySI1.Visible = true;
                        lblDateLoadedSI1.Visible = true;
                        ucDateLoadedSI1.Visible = true;
                        lblROBMTSI1.Visible = true;
                        ucROBMTSI1.Visible = true;
                        lblROBCUMSI1.Visible = true;
                        ucROBCUMSI1.Visible = true;
                        lblTankCleanedSI1.Visible = true;
                        chkTankCleanedSI1.Visible = true;
                        lblDateSI1.Visible = true;
                        ucDateSI1.Visible = true;
                        lblUnpumpableYNSI1.Visible = true;
                        chkUnpumpableSI1.Visible = true;
                        lblpostponealertSI1.Visible = true;
                        chkpostponealertSI1.Visible = true;
                        lblpostponealertremarksSI1.Visible = true;
                        txtpostponeexpremarksSI1.Visible = true;                        
                        
                        hdnConfiguratoinIDSI1.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI1.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI1.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI1.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI1.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI1.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI1.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI1.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI1.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI1.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI1.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI1.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI1.SelectedProduct = "Dummy";
                            lblSpcGravitySI1.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI1.Enabled = false;
                            ucROBCUMSI1.Enabled = true;
                            ucROBMTSI1.CssClass = "readonlytextbox";
                            ucROBCUMSI1.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI1.Enabled = true;
                            ucROBCUMSI1.Enabled = false;
                            ucROBMTSI1.CssClass = "input";
                            ucROBCUMSI1.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI1.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI1.Checked == true)
                            ucDateSI1.Enabled = true;
                        ucDateSI1.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI1.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI1.Visible = true;
                            ucROBCharterUnitSI1.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI1.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI1.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI1.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI1.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI1.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "1" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS1.Disabled = false;
                        lblTankNoS1I.Visible = true;
                        lblTankNoS1.Visible = true;
                        lbl100VolS1I.Visible = true;
                        lbl100VolS1.Visible = true;
                        lbl85VolS1I.Visible = true;
                        lbl85VolS1.Visible = true;
                        lblProductS1I.Visible = true;
                        lblProductS1.Visible = true;
                        lblSpcGravityS1I.Visible = true;
                        lblSpcGravityS1.Visible = true;
                        lblDateLoadedS1.Visible = true;
                        ucDateLoadedS1.Visible = true;
                        lblROBMTS1.Visible = true;
                        ucROBMTS1.Visible = true;
                        lblROBCUMS1.Visible = true;
                        ucROBCUMS1.Visible = true;
                        lblTankCleanedS1.Visible = true;
                        chkTankCleanedS1.Visible = true;
                        lblDateS1.Visible = true;
                        ucDateS1.Visible = true;
                        lblUnpumpableYNS1.Visible = true;
                        chkUnpumpableS1.Visible = true;
                        lblpostponealertS1.Visible = true;
                        chkpostponealertS1.Visible = true;

                        lblpostponealertremarksS1.Visible = true;
                        txtpostponeexpremarksS1.Visible = true;                        
                        
                        hdnConfiguratoinIDS1.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS1.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS1.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS1.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS1.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS1.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS1.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS1.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS1.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS1.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS1.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS1.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS1.SelectedProduct = "Dummy";
                            lblSpcGravityS1.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS1.Enabled = false;
                            ucROBCUMS1.Enabled = true;
                            ucROBMTS1.CssClass = "readonlytextbox";
                            ucROBCUMS1.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS1.Enabled = true;
                            ucROBCUMS1.Enabled = false;
                            ucROBMTS1.CssClass = "input";
                            ucROBCUMS1.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS1.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS1.Checked == true)
                            ucDateS1.Enabled = true;
                        ucDateS1.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS1.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS1.Visible = true;
                            ucROBCharterUnitS1.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS1.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS1.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS1.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS1.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS1.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "2" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP2.Disabled = false;
                        lblTankNoP2I.Visible = true;
                        lblTankNoP2.Visible = true;
                        lbl100VolP2I.Visible = true;
                        lbl100VolP2.Visible = true;
                        lbl85VolP2I.Visible = true;
                        lbl85VolP2.Visible = true;
                        lblProductP2I.Visible = true;
                        lblProductP2.Visible = true;
                        lblSpcGravityP2I.Visible = true;
                        lblSpcGravityP2.Visible = true;
                        lblDateLoadedP2.Visible = true;
                        ucDateLoadedP2.Visible = true;
                        lblROBMTP2.Visible = true;
                        ucROBMTP2.Visible = true;
                        lblROBCUMP2.Visible = true;
                        ucROBCUMP2.Visible = true;
                        lblTankCleanedP2.Visible = true;
                        chkTankCleanedP2.Visible = true;
                        lblDateP2.Visible = true;
                        ucDateP2.Visible = true;
                        lblUnpumpableYNP2.Visible = true;
                        chkUnpumpableP2.Visible = true;
                        lblpostponealertP2.Visible = true;
                        chkpostponealertP2.Visible = true;

                        lblpostponealertremarksP2.Visible = true;
                        txtpostponeexpremarksP2.Visible = true;                        
                        
                        hdnConfiguratoinIDP2.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP2.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP2.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP2.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP2.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP2.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP2.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP2.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP2.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP2.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP2.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP2.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP2.SelectedProduct = "Dummy";
                            lblSpcGravityP2.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP2.Enabled = false;
                            ucROBCUMP2.Enabled = true;
                            ucROBMTP2.CssClass = "readonlytextbox";
                            ucROBCUMP2.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP2.Enabled = true;
                            ucROBCUMP2.Enabled = false;
                            ucROBMTP2.CssClass = "input";
                            ucROBCUMP2.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP2.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP2.Checked == true)
                            ucDateP2.Enabled = true;
                        ucDateP2.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP2.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP2.Visible = true;
                            ucROBCharterUnitP2.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP2.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP2.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP2.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP2.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                        //lblProductP2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "2" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI2.Disabled = false;
                        lblTankNoPI2I.Visible = true;
                        lblTankNoPI2.Visible = true;
                        lbl100VolPI2I.Visible = true;
                        lbl100VolPI2.Visible = true;
                        lbl85VolPI2I.Visible = true;
                        lbl85VolPI2.Visible = true;
                        lblProductPI2I.Visible = true;
                        lblProductPI2.Visible = true;
                        lblSpcGravityPI2I.Visible = true;
                        lblSpcGravityPI2.Visible = true;
                        lblDateLoadedPI2.Visible = true;
                        ucDateLoadedPI2.Visible = true;
                        lblROBMTPI2.Visible = true;
                        ucROBMTPI2.Visible = true;
                        lblROBCUMPI2.Visible = true;
                        ucROBCUMPI2.Visible = true;
                        lblTankCleanedPI2.Visible = true;
                        chkTankCleanedPI2.Visible = true;
                        lblDatePI2.Visible = true;
                        ucDatePI2.Visible = true;
                        lblUnpumpableYNPI2.Visible = true;
                        chkUnpumpablePI2.Visible = true;
                        lblpostponealertPI2.Visible = true;
                        chkpostponealertPI2.Visible = true;
                        lblpostponealertremarksPI2.Visible = true;
                        txtpostponeexpremarksPI2.Visible = true;
                        
                        

                        hdnConfiguratoinIDPI2.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI2.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI2.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI2.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI2.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI2.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI2.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI2.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI2.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI2.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI2.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI2.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI2.SelectedProduct = "Dummy";
                            lblSpcGravityPI2.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI2.Enabled = false;
                            ucROBCUMPI2.Enabled = true;
                            ucROBMTPI2.CssClass = "readonlytextbox";
                            ucROBCUMPI2.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI2.Enabled = true;
                            ucROBCUMPI2.Enabled = false;
                            ucROBMTPI2.CssClass = "input";
                            ucROBCUMPI2.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI2.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI2.Checked == true)
                            ucDatePI2.Enabled = true;
                        ucDatePI2.Text = dr["FLDDATE"].ToString();
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI2.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI2.Visible = true;
                            ucROBCharterUnitPI2.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI2.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI2.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI2.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI2.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "2" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC2.Disabled = false;
                        lblTankNoC2I.Visible = true;
                        lblTankNoC2.Visible = true;
                        lbl100VolC2I.Visible = true;
                        lbl100VolC2.Visible = true;
                        lbl85VolC2I.Visible = true;
                        lbl85VolC2.Visible = true;
                        lblProductC2I.Visible = true;
                        lblProductC2.Visible = true;
                        lblSpcGravityC2I.Visible = true;
                        lblSpcGravityC2.Visible = true;
                        lblDateLoadedC2.Visible = true;
                        ucDateLoadedC2.Visible = true;
                        lblROBMTC2.Visible = true;
                        ucROBMTC2.Visible = true;
                        lblROBCUMC2.Visible = true;
                        ucROBCUMC2.Visible = true;
                        lblTankCleanedC2.Visible = true;
                        chkTankCleanedC2.Visible = true;
                        lblDateC2.Visible = true;
                        ucDateC2.Visible = true;
                        lblUnpumpableYNC2.Visible = true;
                        chkUnpumpableC2.Visible = true;
                        lblpostponealertC2.Visible = true;
                        chkpostponealertC2.Visible = true;
                        lblpostponealertremarksC2.Visible = true;
                        txtpostponeexpremarksC2.Visible = true;                        

                        hdnConfiguratoinIDC2.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC2.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC2.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC2.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC2.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC2.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC2.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC2.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC2.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC2.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC2.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC2.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC2.SelectedProduct = "Dummy";
                            lblSpcGravityC2.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC2.Enabled = false;
                            ucROBCUMC2.Enabled = true;
                            ucROBMTC2.CssClass = "readonlytextbox";
                            ucROBCUMC2.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC2.Enabled = true;
                            ucROBCUMC2.Enabled = false;
                            ucROBMTC2.CssClass = "input";
                            ucROBCUMC2.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC2.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC2.Checked == true)
                            ucDateC2.Enabled = true;
                        ucDateC2.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC2.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC2.Visible = true;
                            ucROBCharterUnitC2.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC2.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC2.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC2.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC2.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                        
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "2" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI2.Disabled = false;
                        lblTankNoSI2I.Visible = true;
                        lblTankNoSI2.Visible = true;
                        lbl100VolSI2I.Visible = true;
                        lbl100VolSI2.Visible = true;
                        lbl85VolSI2I.Visible = true;
                        lbl85VolSI2.Visible = true;
                        lblProductSI2I.Visible = true;
                        lblProductSI2.Visible = true;
                        lblSpcGravitySI2I.Visible = true;
                        lblSpcGravitySI2.Visible = true;
                        lblDateLoadedSI2.Visible = true;
                        ucDateLoadedSI2.Visible = true;
                        lblROBMTSI2.Visible = true;
                        ucROBMTSI2.Visible = true;
                        lblROBCUMSI2.Visible = true;
                        ucROBCUMSI2.Visible = true;
                        lblTankCleanedSI2.Visible = true;
                        chkTankCleanedSI2.Visible = true;
                        lblDateSI2.Visible = true;
                        ucDateSI2.Visible = true;
                        lblUnpumpableYNSI2.Visible = true;
                        chkUnpumpableSI2.Visible = true;
                        lblpostponealertSI2.Visible = true;
                        chkpostponealertSI2.Visible = true;

                        lblpostponealertremarksSI2.Visible = true;
                        txtpostponeexpremarksSI2.Visible = true;                        

                        hdnConfiguratoinIDSI2.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI2.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI2.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI2.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI2.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI2.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI2.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI2.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI2.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI2.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI2.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI2.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI2.SelectedProduct = "Dummy";
                            lblSpcGravitySI2.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI2.Enabled = false;
                            ucROBCUMSI2.Enabled = true;
                            ucROBMTSI2.CssClass = "readonlytextbox";
                            ucROBCUMSI2.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI2.Enabled = true;
                            ucROBCUMSI2.Enabled = false;
                            ucROBMTSI2.CssClass = "input";
                            ucROBCUMSI2.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI2.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        ucDateSI2.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI2.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI2.Visible = true;
                            ucROBCharterUnitSI2.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI2.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI2.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI2.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI2.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "2" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS2.Disabled = false;
                        lblTankNoS2I.Visible = true;
                        lblTankNoS2.Visible = true;
                        lbl100VolS2I.Visible = true;
                        lbl100VolS2.Visible = true;
                        lbl85VolS2I.Visible = true;
                        lbl85VolS2.Visible = true;
                        lblProductS2I.Visible = true;
                        lblProductS2.Visible = true;
                        lblSpcGravityS2I.Visible = true;
                        lblSpcGravityS2.Visible = true;
                        lblDateLoadedS2.Visible = true;
                        ucDateLoadedS2.Visible = true;
                        lblROBMTS2.Visible = true;
                        ucROBMTS2.Visible = true;
                        lblROBCUMS2.Visible = true;
                        ucROBCUMS2.Visible = true;
                        lblTankCleanedS2.Visible = true;
                        chkTankCleanedS2.Visible = true;
                        lblDateS2.Visible = true;
                        ucDateS2.Visible = true;
                        lblUnpumpableYNS2.Visible = true;
                        chkUnpumpableS2.Visible = true;
                        lblpostponealertS2.Visible = true;
                        chkpostponealertS2.Visible = true;

                        lblpostponealertremarksS2.Visible = true;
                        txtpostponeexpremarksS2.Visible = true;                        

                        hdnConfiguratoinIDS2.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS2.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS2.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS2.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS2.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS2.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS2.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS2.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS2.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS2.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS2.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS2.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS2.SelectedProduct = "Dummy";
                            lblSpcGravityS2.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS2.Enabled = false;
                            ucROBCUMS2.Enabled = true;
                            ucROBMTS2.CssClass = "readonlytextbox";
                            ucROBCUMS2.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS2.Enabled = true;
                            ucROBCUMS2.Enabled = false;
                            ucROBMTS2.CssClass = "input";
                            ucROBCUMS2.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS2.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS2.Checked == true)
                            ucDateS2.Enabled = true;
                        ucDateS2.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS2.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS2.Visible = true;
                            ucROBCharterUnitS2.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS2.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS2.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS2.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS2.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS2.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "3" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP3.Disabled = false;
                        lblTankNoP3I.Visible = true;
                        lblTankNoP3.Visible = true;
                        lbl100VolP3I.Visible = true;
                        lbl100VolP3.Visible = true;
                        lbl85VolP3I.Visible = true;
                        lbl85VolP3.Visible = true;
                        lblProductP3I.Visible = true;
                        lblProductP3.Visible = true;
                        lblSpcGravityP3I.Visible = true;
                        lblSpcGravityP3.Visible = true;
                        lblDateLoadedP3.Visible = true;
                        ucDateLoadedP3.Visible = true;
                        lblROBMTP3.Visible = true;
                        ucROBMTP3.Visible = true;
                        lblROBCUMP3.Visible = true;
                        ucROBCUMP3.Visible = true;
                        lblTankCleanedP3.Visible = true;
                        chkTankCleanedP3.Visible = true;
                        lblDateP3.Visible = true;
                        ucDateP3.Visible = true;
                        lblUnpumpableYNP3.Visible = true;
                        chkUnpumpableP3.Visible = true;
                        lblpostponealertP3.Visible = true;
                        chkpostponealertP3.Visible = true;
                        lblpostponealertremarksP3.Visible = true;
                        txtpostponeexpremarksP3.Visible = true;                        
                       
                        hdnConfiguratoinIDP3.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP3.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP3.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP3.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP3.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP3.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP3.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP3.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP3.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP3.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP3.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP3.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP3.SelectedProduct = "Dummy";
                            lblSpcGravityP3.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP3.Enabled = false;
                            ucROBCUMP3.Enabled = true;
                            ucROBMTP3.CssClass = "readonlytextbox";
                            ucROBCUMP3.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP3.Enabled = true;
                            ucROBCUMP3.Enabled = false;
                            ucROBMTP3.CssClass = "input";
                            ucROBCUMP3.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP3.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP3.Checked == true)
                            ucDateP3.Enabled = true;
                        ucDateP3.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP3.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP3.Visible = true;
                            ucROBCharterUnitP3.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP3.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP3.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP3.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP3.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP3.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "3" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI3.Disabled = false;
                        lblTankNoPI3I.Visible = true;
                        lblTankNoPI3.Visible = true;
                        lbl100VolPI3I.Visible = true;
                        lbl100VolPI3.Visible = true;
                        lbl85VolPI3I.Visible = true;
                        lbl85VolPI3.Visible = true;
                        lblProductPI3I.Visible = true;
                        lblProductPI3.Visible = true;
                        lblSpcGravityPI3I.Visible = true;
                        lblSpcGravityPI3.Visible = true;
                        lblDateLoadedPI3.Visible = true;
                        ucDateLoadedPI3.Visible = true;
                        lblROBMTPI3.Visible = true;
                        ucROBMTPI3.Visible = true;
                        lblROBCUMPI3.Visible = true;
                        ucROBCUMPI3.Visible = true;
                        lblTankCleanedPI3.Visible = true;
                        chkTankCleanedPI3.Visible = true;
                        lblDatePI3.Visible = true;
                        ucDatePI3.Visible = true;
                        lblUnpumpableYNPI3.Visible = true;
                        chkUnpumpablePI3.Visible = true;
                        lblpostponealertPI3.Visible = true;
                        chkpostponealertPI3.Visible = true;
                        lblpostponealertremarksPI3.Visible = true;
                        txtpostponeexpremarksPI3.Visible = true;
                        

                        hdnConfiguratoinIDPI3.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI3.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI3.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI3.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI3.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI3.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI3.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI3.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI3.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI3.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI3.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI3.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI3.SelectedProduct = "Dummy";
                            lblSpcGravityPI3.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI3.Enabled = false;
                            ucROBCUMPI3.Enabled = true;
                            ucROBMTPI3.CssClass = "readonlytextbox";
                            ucROBCUMPI3.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI3.Enabled = true;
                            ucROBCUMPI3.Enabled = false;
                            ucROBMTPI3.CssClass = "input";
                            ucROBCUMPI3.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI3.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI3.Checked == true)
                            ucDatePI3.Enabled = true;
                        ucDatePI3.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI3.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI3.Visible = true;
                            ucROBCharterUnitPI3.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI3.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI3.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI3.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI3.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI3.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "3" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC3.Disabled = false;
                        lblTankNoC3I.Visible = true;
                        lblTankNoC3.Visible = true;
                        lbl100VolC3I.Visible = true;
                        lbl100VolC3.Visible = true;
                        lbl85VolC3I.Visible = true;
                        lbl85VolC3.Visible = true;
                        lblProductC3I.Visible = true;
                        lblProductC3.Visible = true;
                        lblSpcGravityC3I.Visible = true;
                        lblSpcGravityC3.Visible = true;
                        lblDateLoadedC3.Visible = true;
                        ucDateLoadedC3.Visible = true;
                        lblROBMTC3.Visible = true;
                        ucROBMTC3.Visible = true;
                        lblROBCUMC3.Visible = true;
                        ucROBCUMC3.Visible = true;
                        lblTankCleanedC3.Visible = true;
                        chkTankCleanedC3.Visible = true;
                        lblDateC3.Visible = true;
                        ucDateC3.Visible = true;
                        lblUnpumpableYNC3.Visible = true;
                        chkUnpumpableC3.Visible = true;
                        lblpostponealertC3.Visible = true;
                        chkpostponealertC3.Visible = true;
                        lblpostponealertremarksC3.Visible = true;
                        txtpostponeexpremarksC3.Visible = true;

                        hdnConfiguratoinIDC3.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC3.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC3.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC3.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC3.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC3.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC3.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC3.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC3.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC3.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC3.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC3.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC3.SelectedProduct = "Dummy";
                            lblSpcGravityC3.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC3.Enabled = false;
                            ucROBCUMC3.Enabled = true;
                            ucROBMTC3.CssClass = "readonlytextbox";
                            ucROBCUMC3.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC3.Enabled = true;
                            ucROBCUMC3.Enabled = false;
                            ucROBMTC3.CssClass = "input";
                            ucROBCUMC3.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC3.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC3.Checked == true)
                            ucDateC3.Enabled = true;
                        ucDateC3.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC3.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC3.Visible = true;
                            ucROBCharterUnitC3.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC3.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC3.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC3.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC3.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC3.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "3" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI3.Disabled = false;
                        lblTankNoSI3I.Visible = true;
                        lblTankNoSI3.Visible = true;
                        lbl100VolSI3I.Visible = true;
                        lbl100VolSI3.Visible = true;
                        lbl85VolSI3I.Visible = true;
                        lbl85VolSI3.Visible = true;
                        lblProductSI3I.Visible = true;
                        lblProductSI3.Visible = true;
                        lblSpcGravitySI3I.Visible = true;
                        lblSpcGravitySI3.Visible = true;
                        lblDateLoadedSI3.Visible = true;
                        ucDateLoadedSI3.Visible = true;
                        lblROBMTSI3.Visible = true;
                        ucROBMTSI3.Visible = true;
                        lblROBCUMSI3.Visible = true;
                        ucROBCUMSI3.Visible = true;
                        lblTankCleanedSI3.Visible = true;
                        chkTankCleanedSI3.Visible = true;
                        lblDateSI3.Visible = true;
                        ucDateSI3.Visible = true;
                        lblUnpumpableYNSI3.Visible = true;
                        chkUnpumpableSI3.Visible = true;
                        lblpostponealertSI3.Visible = true;
                        chkpostponealertSI3.Visible = true;
                        lblpostponealertremarksSI3.Visible = true;
                        txtpostponeexpremarksSI3.Visible = true;                        
                        
                        hdnConfiguratoinIDSI3.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI3.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI3.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI3.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI3.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI3.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI3.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI3.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI3.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI3.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI3.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI3.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI3.SelectedProduct = "Dummy";
                            lblSpcGravitySI3.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI3.Enabled = false;
                            ucROBCUMSI3.Enabled = true;
                            ucROBMTSI3.CssClass = "readonlytextbox";
                            ucROBCUMSI3.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI3.Enabled = true;
                            ucROBCUMSI3.Enabled = false;
                            ucROBMTSI3.CssClass = "input";
                            ucROBCUMSI3.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI3.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI3.Checked == true)
                            ucDateSI3.Enabled = true;
                        ucDateSI3.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI3.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI3.Visible = true;
                            ucROBCharterUnitSI3.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI3.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI3.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI3.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI3.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI3.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "3" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS3.Disabled = false;
                        lblTankNoS3I.Visible = true;
                        lblTankNoS3.Visible = true;
                        lbl100VolS3I.Visible = true;
                        lbl100VolS3.Visible = true;
                        lbl85VolS3I.Visible = true;
                        lbl85VolS3.Visible = true;
                        lblProductS3I.Visible = true;
                        lblProductS3.Visible = true;
                        lblSpcGravityS3I.Visible = true;
                        lblSpcGravityS3.Visible = true;
                        lblDateLoadedS3.Visible = true;
                        ucDateLoadedS3.Visible = true;
                        lblROBMTS3.Visible = true;
                        ucROBMTS3.Visible = true;
                        lblROBCUMS3.Visible = true;
                        ucROBCUMS3.Visible = true;
                        lblTankCleanedS3.Visible = true;
                        chkTankCleanedS3.Visible = true;
                        lblDateS3.Visible = true;
                        ucDateS3.Visible = true;
                        lblUnpumpableYNS3.Visible = true;
                        chkUnpumpableS3.Visible = true;
                        lblpostponealertS3.Visible = true;
                        chkpostponealertS3.Visible = true;
                        lblpostponealertremarksS3.Visible = true;
                        txtpostponeexpremarksS3.Visible = true;                        
                        
                        hdnConfiguratoinIDS3.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS3.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS3.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS3.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS3.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS3.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS3.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS3.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS3.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS3.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS3.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS3.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS3.SelectedProduct = "Dummy";
                            lblSpcGravityS3.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS3.Enabled = false;
                            ucROBCUMS3.Enabled = true;
                            ucROBMTS3.CssClass = "readonlytextbox";
                            ucROBCUMS3.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS3.Enabled = true;
                            ucROBCUMS3.Enabled = false;
                            ucROBMTS3.CssClass = "input";
                            ucROBCUMS3.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS3.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS3.Checked == true)
                            ucDateS3.Enabled = true;
                        ucDateS3.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS3.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS3.Visible = true;
                            ucROBCharterUnitS3.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS3.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS3.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS3.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS3.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS3.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "4" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP4.Disabled = false;
                        lblTankNoP4I.Visible = true;
                        lblTankNoP4.Visible = true;
                        lbl100VolP4I.Visible = true;
                        lbl100VolP4.Visible = true;
                        lbl85VolP4I.Visible = true;
                        lbl85VolP4.Visible = true;
                        lblProductP4I.Visible = true;
                        lblProductP4.Visible = true;
                        lblSpcGravityP4I.Visible = true;
                        lblSpcGravityP4.Visible = true;
                        lblDateLoadedP4.Visible = true;
                        ucDateLoadedP4.Visible = true;
                        lblROBMTP4.Visible = true;
                        ucROBMTP4.Visible = true;
                        lblROBCUMP4.Visible = true;
                        ucROBCUMP4.Visible = true;
                        lblTankCleanedP4.Visible = true;
                        chkTankCleanedP4.Visible = true;
                        lblDateP4.Visible = true;
                        ucDateP4.Visible = true;
                        lblUnpumpableYNP4.Visible = true;
                        chkUnpumpableP4.Visible = true;
                        lblpostponealertP4.Visible = true;
                        chkpostponealertP4.Visible = true;
                        lblpostponealertremarksP4.Visible = true;
                        txtpostponeexpremarksP4.Visible = true;                        
                        
                        hdnConfiguratoinIDP4.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP4.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP4.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP4.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP4.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP4.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP4.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP4.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP4.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP4.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP4.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP4.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP4.SelectedProduct = "Dummy";
                            lblSpcGravityP4.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP4.Enabled = false;
                            ucROBCUMP4.Enabled = true;
                            ucROBMTP4.CssClass = "readonlytextbox";
                            ucROBCUMP4.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP4.Enabled = true;
                            ucROBCUMP4.Enabled = false;
                            ucROBMTP4.CssClass = "input";
                            ucROBCUMP4.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP4.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP4.Checked == true)
                            ucDateP4.Enabled = true;
                        ucDateP4.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP4.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP4.Visible = true;
                            ucROBCharterUnitP4.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP4.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP4.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP4.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP4.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP4.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "4" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI4.Disabled = false;
                        lblTankNoPI4I.Visible = true;
                        lblTankNoPI4.Visible = true;
                        lbl100VolPI4I.Visible = true;
                        lbl100VolPI4.Visible = true;
                        lbl85VolPI4I.Visible = true;
                        lbl85VolPI4.Visible = true;
                        lblProductPI4I.Visible = true;
                        lblProductPI4.Visible = true;
                        lblSpcGravityPI4I.Visible = true;
                        lblSpcGravityPI4.Visible = true;
                        lblDateLoadedPI4.Visible = true;
                        ucDateLoadedPI4.Visible = true;
                        lblROBMTPI4.Visible = true;
                        ucROBMTPI4.Visible = true;
                        lblROBCUMPI4.Visible = true;
                        ucROBCUMPI4.Visible = true;
                        lblTankCleanedPI4.Visible = true;
                        chkTankCleanedPI4.Visible = true;
                        lblDatePI4.Visible = true;
                        ucDatePI4.Visible = true;
                        lblUnpumpableYNPI4.Visible = true;
                        chkUnpumpablePI4.Visible = true;
                        lblpostponealertPI4.Visible = true;
                        chkpostponealertPI4.Visible = true;
                        lblpostponealertremarksPI4.Visible = true;
                        txtpostponeexpremarksPI4.Visible = true;                        

                        hdnConfiguratoinIDPI4.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI4.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI4.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI4.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI4.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI4.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI4.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI4.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI4.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI4.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI4.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI4.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI4.SelectedProduct = "Dummy";
                            lblSpcGravityPI4.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI4.Enabled = false;
                            ucROBCUMPI4.Enabled = true;
                            ucROBMTPI4.CssClass = "readonlytextbox";
                            ucROBCUMPI4.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI4.Enabled = true;
                            ucROBCUMPI4.Enabled = false;
                            ucROBMTPI4.CssClass = "input";
                            ucROBCUMPI4.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI4.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false; ;
                        if (chkTankCleanedPI4.Checked == true)
                            ucDatePI4.Enabled = true;
                        ucDatePI4.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI4.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI4.Visible = true;
                            ucROBCharterUnitPI4.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI4.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI4.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI4.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI4.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI4.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "4" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC4.Disabled = false;
                        lblTankNoC4I.Visible = true;
                        lblTankNoC4.Visible = true;
                        lbl100VolC4I.Visible = true;
                        lbl100VolC4.Visible = true;
                        lbl85VolC4I.Visible = true;
                        lbl85VolC4.Visible = true;
                        lblProductC4I.Visible = true;
                        lblProductC4.Visible = true;
                        lblSpcGravityC4I.Visible = true;
                        lblSpcGravityC4.Visible = true;
                        lblDateLoadedC4.Visible = true;
                        ucDateLoadedC4.Visible = true;
                        lblROBMTC4.Visible = true;
                        ucROBMTC4.Visible = true;
                        lblROBCUMC4.Visible = true;
                        ucROBCUMC4.Visible = true;
                        lblTankCleanedC4.Visible = true;
                        chkTankCleanedC4.Visible = true;
                        lblDateC4.Visible = true;
                        ucDateC4.Visible = true;
                        lblUnpumpableYNC4.Visible = true;
                        chkUnpumpableC4.Visible = true;
                        lblpostponealertC4.Visible = true;
                        chkpostponealertC4.Visible = true;
                        lblpostponealertremarksC4.Visible = true;
                        txtpostponeexpremarksC4.Visible = true;                        

                        hdnConfiguratoinIDC4.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC4.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC4.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC4.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC4.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC4.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC4.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC4.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC4.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC4.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC4.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC4.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC4.SelectedProduct = "Dummy";
                            lblSpcGravityC4.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC4.Enabled = false;
                            ucROBCUMC4.Enabled = true;
                            ucROBMTC4.CssClass = "readonlytextbox";
                            ucROBCUMC4.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC4.Enabled = true;
                            ucROBCUMC4.Enabled = false;
                            ucROBMTC4.CssClass = "input";
                            ucROBCUMC4.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC4.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false; ;
                        if (chkTankCleanedC4.Checked == true)
                            ucDateC4.Enabled = true;
                        ucDateC4.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC4.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC4.Visible = true;
                            ucROBCharterUnitC4.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC4.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC4.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC4.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC4.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC4.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "4" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI4.Disabled = false;
                        lblTankNoSI4I.Visible = true;
                        lblTankNoSI4.Visible = true;
                        lbl100VolSI4I.Visible = true;
                        lbl100VolSI4.Visible = true;
                        lbl85VolSI4I.Visible = true;
                        lbl85VolSI4.Visible = true;
                        lblProductSI4I.Visible = true;
                        lblProductSI4.Visible = true;
                        lblSpcGravitySI4I.Visible = true;
                        lblSpcGravitySI4.Visible = true;
                        lblDateLoadedSI4.Visible = true;
                        ucDateLoadedSI4.Visible = true;
                        lblROBMTSI4.Visible = true;
                        ucROBMTSI4.Visible = true;
                        lblROBCUMSI4.Visible = true;
                        ucROBCUMSI4.Visible = true;
                        lblTankCleanedSI4.Visible = true;
                        chkTankCleanedSI4.Visible = true;
                        lblDateSI4.Visible = true;
                        ucDateSI4.Visible = true;
                        lblUnpumpableYNSI4.Visible = true;
                        chkUnpumpableSI4.Visible = true;
                        lblpostponealertSI4.Visible = true;
                        chkpostponealertSI4.Visible = true;
                        lblpostponealertremarksSI4.Visible = true;
                        txtpostponeexpremarksSI4.Visible = true;                        

                        hdnConfiguratoinIDSI4.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI4.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI4.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI4.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI4.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI4.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI4.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI4.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI4.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI4.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI4.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI4.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI4.SelectedProduct = "Dummy";
                            lblSpcGravitySI4.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI4.Enabled = false;
                            ucROBCUMSI4.Enabled = true;
                            ucROBMTSI4.CssClass = "readonlytextbox";
                            ucROBCUMSI4.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI4.Enabled = true;
                            ucROBCUMSI4.Enabled = false;
                            ucROBMTSI4.CssClass = "input";
                            ucROBCUMSI4.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI4.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false; ;
                        if (chkTankCleanedSI4.Checked == true)
                            ucDateSI4.Enabled = true;
                        ucDateSI4.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI4.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI4.Visible = true;
                            ucROBCharterUnitSI4.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI4.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI4.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI4.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI4.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI4.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "4" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS4.Disabled = false;
                        lblTankNoS4I.Visible = true;
                        lblTankNoS4.Visible = true;
                        lbl100VolS4I.Visible = true;
                        lbl100VolS4.Visible = true;
                        lbl85VolS4I.Visible = true;
                        lbl85VolS4.Visible = true;
                        lblProductS4I.Visible = true;
                        lblProductS4.Visible = true;
                        lblSpcGravityS4I.Visible = true;
                        lblSpcGravityS4.Visible = true;
                        lblDateLoadedS4.Visible = true;
                        ucDateLoadedS4.Visible = true;
                        lblROBMTS4.Visible = true;
                        ucROBMTS4.Visible = true;
                        lblROBCUMS4.Visible = true;
                        ucROBCUMS4.Visible = true;
                        lblTankCleanedS4.Visible = true;
                        chkTankCleanedS4.Visible = true;
                        lblDateS4.Visible = true;
                        ucDateS4.Visible = true;
                        lblUnpumpableYNS4.Visible = true;
                        chkUnpumpableS4.Visible = true;
                        lblpostponealertS4.Visible = true;
                        chkpostponealertS4.Visible = true;
                        lblpostponealertremarksS4.Visible = true;
                        txtpostponeexpremarksS4.Visible = true;
                        
                        
                        hdnConfiguratoinIDS4.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS4.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS4.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS4.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS4.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS4.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS4.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS4.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS4.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS4.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS4.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS4.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS4.SelectedProduct = "Dummy";
                            lblSpcGravityS4.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS4.Enabled = false;
                            ucROBCUMS4.Enabled = true;
                            ucROBMTS4.CssClass = "readonlytextbox";
                            ucROBCUMS4.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS4.Enabled = true;
                            ucROBCUMS4.Enabled = false;
                            ucROBMTS4.CssClass = "input";
                            ucROBCUMS4.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS4.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false; ;
                        if (chkTankCleanedS4.Checked == true)
                            ucDateS4.Enabled = true;
                        ucDateS4.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS4.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS4.Visible = true;
                            ucROBCharterUnitS4.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS4.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS4.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS4.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS4.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS4.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "5" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP5.Disabled = false;
                        lblTankNoP5I.Visible = true;
                        lblTankNoP5.Visible = true;
                        lbl100VolP5I.Visible = true;
                        lbl100VolP5.Visible = true;
                        lbl85VolP5I.Visible = true;
                        lbl85VolP5.Visible = true;
                        lblProductP5I.Visible = true;
                        lblProductP5.Visible = true;
                        lblSpcGravityP5I.Visible = true;
                        lblSpcGravityP5.Visible = true;
                        lblDateLoadedP5.Visible = true;
                        ucDateLoadedP5.Visible = true;
                        lblROBMTP5.Visible = true;
                        ucROBMTP5.Visible = true;
                        lblROBCUMP5.Visible = true;
                        ucROBCUMP5.Visible = true;
                        lblTankCleanedP5.Visible = true;
                        chkTankCleanedP5.Visible = true;
                        lblDateP5.Visible = true;
                        ucDateP5.Visible = true;
                        lblUnpumpableYNP5.Visible = true;
                        chkUnpumpableP5.Visible = true;
                        lblpostponealertP5.Visible = true;
                        chkpostponealertP5.Visible = true;
                        lblpostponealertremarksP5.Visible = true;
                        txtpostponeexpremarksP5.Visible = true;                        
                        
                        hdnConfiguratoinIDP5.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP5.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP5.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP5.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP5.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP5.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP5.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP5.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP5.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP5.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP5.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP5.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP5.SelectedProduct = "Dummy";
                            lblSpcGravityP5.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP5.Enabled = false;
                            ucROBCUMP5.Enabled = true;
                            ucROBMTP5.CssClass = "readonlytextbox";
                            ucROBCUMP5.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP5.Enabled = true;
                            ucROBCUMP5.Enabled = false;
                            ucROBMTP5.CssClass = "input";
                            ucROBCUMP5.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP5.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false; ;
                        if (chkTankCleanedP5.Checked == true)
                            ucDateP5.Enabled = true;
                        ucDateP5.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP5.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP5.Visible = true;
                            ucROBCharterUnitP5.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP5.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP5.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP5.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP5.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP5.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "5" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI5.Disabled = false;
                        lblTankNoPI5I.Visible = true;
                        lblTankNoPI5.Visible = true;
                        lbl100VolPI5I.Visible = true;
                        lbl100VolPI5.Visible = true;
                        lbl85VolPI5I.Visible = true;
                        lbl85VolPI5.Visible = true;
                        lblProductPI5I.Visible = true;
                        lblProductPI5.Visible = true;
                        lblSpcGravityPI5I.Visible = true;
                        lblSpcGravityPI5.Visible = true;
                        lblDateLoadedPI5.Visible = true;
                        ucDateLoadedPI5.Visible = true;
                        lblROBMTPI5.Visible = true;
                        ucROBMTPI5.Visible = true;
                        lblROBCUMPI5.Visible = true;
                        ucROBCUMPI5.Visible = true;
                        lblTankCleanedPI5.Visible = true;
                        chkTankCleanedPI5.Visible = true;
                        lblDatePI5.Visible = true;
                        ucDatePI5.Visible = true;
                        lblUnpumpableYNPI5.Visible = true;
                        chkUnpumpablePI5.Visible = true;
                        lblpostponealertPI5.Visible = true;
                        chkpostponealertPI5.Visible = true;

                        lblpostponealertremarksPI5.Visible = true;
                        txtpostponeexpremarksPI5.Visible = true;                        
                        
                        hdnConfiguratoinIDPI5.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI5.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI5.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI5.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI5.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI5.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI5.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI5.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI5.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI5.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI5.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI5.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI5.SelectedProduct = "Dummy";
                            lblSpcGravityPI5.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI5.Enabled = false;
                            ucROBCUMPI5.Enabled = true;
                            ucROBMTPI5.CssClass = "readonlytextbox";
                            ucROBCUMPI5.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI5.Enabled = true;
                            ucROBCUMPI5.Enabled = false;
                            ucROBMTPI5.CssClass = "input";
                            ucROBCUMPI5.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI5.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI5.Checked == true)
                            ucDatePI5.Enabled = true;
                        ucDatePI5.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI5.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI5.Visible = true;
                            ucROBCharterUnitPI5.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI5.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI5.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI5.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI5.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI5.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "5" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC5.Disabled = false;
                        lblTankNoC5I.Visible = true;
                        lblTankNoC5.Visible = true;
                        lbl100VolC5I.Visible = true;
                        lbl100VolC5.Visible = true;
                        lbl85VolC5I.Visible = true;
                        lbl85VolC5.Visible = true;
                        lblProductC5I.Visible = true;
                        lblProductC5.Visible = true;
                        lblSpcGravityC5I.Visible = true;
                        lblSpcGravityC5.Visible = true;
                        lblDateLoadedC5.Visible = true;
                        ucDateLoadedC5.Visible = true;
                        lblROBMTC5.Visible = true;
                        ucROBMTC5.Visible = true;
                        lblROBCUMC5.Visible = true;
                        ucROBCUMC5.Visible = true;
                        lblTankCleanedC5.Visible = true;
                        chkTankCleanedC5.Visible = true;
                        lblDateC5.Visible = true;
                        ucDateC5.Visible = true;
                        lblUnpumpableYNC5.Visible = true;
                        chkUnpumpableC5.Visible = true;
                        lblpostponealertC5.Visible = true;
                        chkpostponealertC5.Visible = true;
                        lblpostponealertremarksC5.Visible = true;
                        txtpostponeexpremarksC5.Visible = true;
                                                
                        hdnConfiguratoinIDC5.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC5.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC5.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC5.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC5.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC5.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC5.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC5.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC5.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC5.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC5.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC5.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC5.SelectedProduct = "Dummy";
                            lblSpcGravityC5.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC5.Enabled = false;
                            ucROBCUMC5.Enabled = true;
                            ucROBMTC5.CssClass = "readonlytextbox";
                            ucROBCUMC5.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC5.Enabled = true;
                            ucROBCUMC5.Enabled = false;
                            ucROBMTC5.CssClass = "input";
                            ucROBCUMC5.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC5.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC5.Checked == true)
                            ucDateC5.Enabled = true;
                        ucDateC5.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC5.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC5.Visible = true;
                            ucROBCharterUnitC5.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC5.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC5.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC5.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC5.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC5.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "5" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI5.Disabled = false;
                        lblTankNoSI5I.Visible = true;
                        lblTankNoSI5.Visible = true;
                        lbl100VolSI5I.Visible = true;
                        lbl100VolSI5.Visible = true;
                        lbl85VolSI5I.Visible = true;
                        lbl85VolSI5.Visible = true;
                        lblProductSI5I.Visible = true;
                        lblProductSI5.Visible = true;
                        lblSpcGravitySI5I.Visible = true;
                        lblSpcGravitySI5.Visible = true;
                        lblDateLoadedSI5.Visible = true;
                        ucDateLoadedSI5.Visible = true;
                        lblROBMTSI5.Visible = true;
                        ucROBMTSI5.Visible = true;
                        lblROBCUMSI5.Visible = true;
                        ucROBCUMSI5.Visible = true;
                        lblTankCleanedSI5.Visible = true;
                        chkTankCleanedSI5.Visible = true;
                        lblDateSI5.Visible = true;
                        ucDateSI5.Visible = true;
                        lblUnpumpableYNSI5.Visible = true;
                        chkUnpumpableSI5.Visible = true;
                        lblpostponealertSI5.Visible = true;
                        chkpostponealertSI5.Visible = true;
                        lblpostponealertremarksSI5.Visible = true;
                        txtpostponeexpremarksSI5.Visible = true;                        

                        hdnConfiguratoinIDSI5.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI5.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI5.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI5.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI5.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI5.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI5.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI5.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI5.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI5.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI5.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI5.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI5.SelectedProduct = "Dummy";
                            lblSpcGravitySI5.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI5.Enabled = false;
                            ucROBCUMSI5.Enabled = true;
                            ucROBMTSI5.CssClass = "readonlytextbox";
                            ucROBCUMSI5.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI5.Enabled = true;
                            ucROBCUMSI5.Enabled = false;
                            ucROBMTSI5.CssClass = "input";
                            ucROBCUMSI5.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI5.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI5.Checked == true)
                            ucDateSI5.Enabled = true;
                        ucDateSI5.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI5.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI5.Visible = true;
                            ucROBCharterUnitSI5.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI5.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI5.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI5.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI5.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI5.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "5" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS5.Disabled = false;
                        lblTankNoS5I.Visible = true;
                        lblTankNoS5.Visible = true;
                        lbl100VolS5I.Visible = true;
                        lbl100VolS5.Visible = true;
                        lbl85VolS5I.Visible = true;
                        lbl85VolS5.Visible = true;
                        lblProductS5I.Visible = true;
                        lblProductS5.Visible = true;
                        lblSpcGravityS5I.Visible = true;
                        lblSpcGravityS5.Visible = true;
                        lblDateLoadedS5.Visible = true;
                        ucDateLoadedS5.Visible = true;
                        lblROBMTS5.Visible = true;
                        ucROBMTS5.Visible = true;
                        lblROBCUMS5.Visible = true;
                        ucROBCUMS5.Visible = true;
                        lblTankCleanedS5.Visible = true;
                        chkTankCleanedS5.Visible = true;
                        lblDateS5.Visible = true;
                        ucDateS5.Visible = true;
                        lblUnpumpableYNS5.Visible = true;
                        chkUnpumpableS5.Visible = true;
                        lblpostponealertS5.Visible = true;
                        chkpostponealertS5.Visible = true;
                        lblpostponealertremarksS5.Visible = true;
                        txtpostponeexpremarksS5.Visible = true;                        

                        hdnConfiguratoinIDS5.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS5.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS5.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS5.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS5.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS5.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS5.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS5.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS5.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS5.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS5.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS5.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS5.SelectedProduct = "Dummy";
                            lblSpcGravityS5.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS5.Enabled = false;
                            ucROBCUMS5.Enabled = true;
                            ucROBMTS5.CssClass = "readonlytextbox";
                            ucROBCUMS5.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS5.Enabled = true;
                            ucROBCUMS5.Enabled = false;
                            ucROBMTS5.CssClass = "input";
                            ucROBCUMS5.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS5.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS5.Checked == true)
                            ucDateS5.Enabled = true;
                        ucDateS5.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS5.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS5.Visible = true;
                            ucROBCharterUnitS5.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS5.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS5.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS5.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS5.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS5.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "6" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP6.Disabled = false;
                        lblTankNoP6I.Visible = true;
                        lblTankNoP6.Visible = true;
                        lbl100VolP6I.Visible = true;
                        lbl100VolP6.Visible = true;
                        lbl85VolP6I.Visible = true;
                        lbl85VolP6.Visible = true;
                        lblProductP6I.Visible = true;
                        lblProductP6.Visible = true;
                        lblSpcGravityP6I.Visible = true;
                        lblSpcGravityP6.Visible = true;
                        lblDateLoadedP6.Visible = true;
                        ucDateLoadedP6.Visible = true;
                        lblROBMTP6.Visible = true;
                        ucROBMTP6.Visible = true;
                        lblROBCUMP6.Visible = true;
                        ucROBCUMP6.Visible = true;
                        lblTankCleanedP6.Visible = true;
                        chkTankCleanedP6.Visible = true;
                        lblDateP6.Visible = true;
                        ucDateP6.Visible = true;
                        lblUnpumpableYNP6.Visible = true;
                        chkUnpumpableP6.Visible = true;
                        lblpostponealertP6.Visible = true;
                        chkpostponealertP6.Visible = true;
                        lblpostponealertremarksP6.Visible = true;
                        txtpostponeexpremarksP6.Visible = true;                       
                        
                        hdnConfiguratoinIDP6.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP6.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP6.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP6.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP6.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP6.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP6.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP6.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP6.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP6.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP6.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP6.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP6.SelectedProduct = "Dummy";
                            lblSpcGravityP6.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP6.Enabled = false;
                            ucROBCUMP6.Enabled = true;
                            ucROBMTP6.CssClass = "readonlytextbox";
                            ucROBCUMP6.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP6.Enabled = true;
                            ucROBCUMP6.Enabled = false;
                            ucROBMTP6.CssClass = "input";
                            ucROBCUMP6.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP6.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP6.Checked == true)
                            ucDateP6.Enabled = true;
                        ucDateP6.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP6.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP6.Visible = true;
                            ucROBCharterUnitP6.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP6.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP6.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP6.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP6.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP6.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "6" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI6.Disabled = false;
                        lblTankNoPI6I.Visible = true;
                        lblTankNoPI6.Visible = true;
                        lbl100VolPI6I.Visible = true;
                        lbl100VolPI6.Visible = true;
                        lbl85VolPI6I.Visible = true;
                        lbl85VolPI6.Visible = true;
                        lblProductPI6I.Visible = true;
                        lblProductPI6.Visible = true;
                        lblSpcGravityPI6I.Visible = true;
                        lblSpcGravityPI6.Visible = true;
                        lblDateLoadedPI6.Visible = true;
                        ucDateLoadedPI6.Visible = true;
                        lblROBMTPI6.Visible = true;
                        ucROBMTPI6.Visible = true;
                        lblROBCUMPI6.Visible = true;
                        ucROBCUMPI6.Visible = true;
                        lblTankCleanedPI6.Visible = true;
                        chkTankCleanedPI6.Visible = true;
                        lblDatePI6.Visible = true;
                        ucDatePI6.Visible = true;
                        lblUnpumpableYNPI6.Visible = true;
                        chkUnpumpablePI6.Visible = true;
                        lblpostponealertPI6.Visible = true;
                        chkpostponealertPI6.Visible = true;
                        lblpostponealertremarksPI6.Visible = true;
                        txtpostponeexpremarksPI6.Visible = true;                       


                        hdnConfiguratoinIDPI6.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI6.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI6.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI6.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI6.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI6.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI6.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI6.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI6.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI6.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI6.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI6.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI6.SelectedProduct = "Dummy";
                            lblSpcGravityPI6.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI6.Enabled = false;
                            ucROBCUMPI6.Enabled = true;
                            ucROBMTPI6.CssClass = "readonlytextbox";
                            ucROBCUMPI6.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI6.Enabled = true;
                            ucROBCUMPI6.Enabled = false;
                            ucROBMTPI6.CssClass = "input";
                            ucROBCUMPI6.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI6.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI6.Checked == true)
                            ucDatePI6.Enabled = true;
                        ucDatePI6.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI6.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI6.Visible = true;
                            ucROBCharterUnitPI6.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI6.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI6.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI6.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI6.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI6.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "6" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC6.Disabled = false;
                        lblTankNoC6I.Visible = true;
                        lblTankNoC6.Visible = true;
                        lbl100VolC6I.Visible = true;
                        lbl100VolC6.Visible = true;
                        lbl85VolC6I.Visible = true;
                        lbl85VolC6.Visible = true;
                        lblProductC6I.Visible = true;
                        lblProductC6.Visible = true;
                        lblSpcGravityC6I.Visible = true;
                        lblSpcGravityC6.Visible = true;
                        lblDateLoadedC6.Visible = true;
                        ucDateLoadedC6.Visible = true;
                        lblROBMTC6.Visible = true;
                        ucROBMTC6.Visible = true;
                        lblROBCUMC6.Visible = true;
                        ucROBCUMC6.Visible = true;
                        lblTankCleanedC6.Visible = true;
                        chkTankCleanedC6.Visible = true;
                        lblDateC6.Visible = true;
                        ucDateC6.Visible = true;
                        lblUnpumpableYNC6.Visible = true;
                        chkUnpumpableC6.Visible = true;
                        lblpostponealertC6.Visible = true;
                        chkpostponealertC6.Visible = true;
                        lblpostponealertremarksC6.Visible = true;
                        txtpostponeexpremarksC6.Visible = true;                       

                        hdnConfiguratoinIDC6.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC6.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC6.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC6.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC6.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC6.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC6.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC6.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC6.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC6.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC6.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC6.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC6.SelectedProduct = "Dummy";
                            lblSpcGravityC6.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC6.Enabled = false;
                            ucROBCUMC6.Enabled = true;
                            ucROBMTC6.CssClass = "readonlytextbox";
                            ucROBCUMC6.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC6.Enabled = true;
                            ucROBCUMC6.Enabled = false;
                            ucROBMTC6.CssClass = "input";
                            ucROBCUMC6.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC6.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC6.Checked == true)
                            ucDateC6.Enabled = true;
                        ucDateC6.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC6.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC6.Visible = true;
                            ucROBCharterUnitC6.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC6.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC6.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC6.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC6.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC6.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "6" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI6.Disabled = false;
                        lblTankNoSI6I.Visible = true;
                        lblTankNoSI6.Visible = true;
                        lbl100VolSI6I.Visible = true;
                        lbl100VolSI6.Visible = true;
                        lbl85VolSI6I.Visible = true;
                        lbl85VolSI6.Visible = true;
                        lblProductSI6I.Visible = true;
                        lblProductSI6.Visible = true;
                        lblSpcGravitySI6I.Visible = true;
                        lblSpcGravitySI6.Visible = true;
                        lblDateLoadedSI6.Visible = true;
                        ucDateLoadedSI6.Visible = true;
                        lblROBMTSI6.Visible = true;
                        ucROBMTSI6.Visible = true;
                        lblROBCUMSI6.Visible = true;
                        ucROBCUMSI6.Visible = true;
                        lblTankCleanedSI6.Visible = true;
                        chkTankCleanedSI6.Visible = true;
                        lblDateSI6.Visible = true;
                        ucDateSI6.Visible = true;
                        lblUnpumpableYNSI6.Visible = true;
                        chkUnpumpableSI6.Visible = true;
                        lblpostponealertSI6.Visible = true;
                        chkpostponealertSI6.Visible = true;
                        lblpostponealertremarksSI6.Visible = true;
                        txtpostponeexpremarksSI6.Visible = true;                       

                        hdnConfiguratoinIDSI6.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI6.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI6.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI6.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI6.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI6.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI6.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI6.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI6.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI6.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI6.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI6.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI6.SelectedProduct = "Dummy";
                            lblSpcGravitySI6.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI6.Enabled = false;
                            ucROBCUMSI6.Enabled = true;
                            ucROBMTSI6.CssClass = "readonlytextbox";
                            ucROBCUMSI6.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI6.Enabled = true;
                            ucROBCUMSI6.Enabled = false;
                            ucROBMTSI6.CssClass = "input";
                            ucROBCUMSI6.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI6.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI6.Checked == true)
                            ucDateSI6.Enabled = true;
                        ucDateSI6.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI6.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI6.Visible = true;
                            ucROBCharterUnitSI6.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI6.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI6.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI6.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI6.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI6.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "6" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS6.Disabled = false;
                        lblTankNoS6I.Visible = true;
                        lblTankNoS6.Visible = true;
                        lbl100VolS6I.Visible = true;
                        lbl100VolS6.Visible = true;
                        lbl85VolS6I.Visible = true;
                        lbl85VolS6.Visible = true;
                        lblProductS6I.Visible = true;
                        lblProductS6.Visible = true;
                        lblSpcGravityS6I.Visible = true;
                        lblSpcGravityS6.Visible = true;
                        lblDateLoadedS6.Visible = true;
                        ucDateLoadedS6.Visible = true;
                        lblROBMTS6.Visible = true;
                        ucROBMTS6.Visible = true;
                        lblROBCUMS6.Visible = true;
                        ucROBCUMS6.Visible = true;
                        lblTankCleanedS6.Visible = true;
                        chkTankCleanedS6.Visible = true;
                        lblDateS6.Visible = true;
                        ucDateS6.Visible = true;
                        lblUnpumpableYNS6.Visible = true;
                        chkUnpumpableS6.Visible = true;
                        lblpostponealertS6.Visible = true;
                        chkpostponealertS6.Visible = true;
                        lblpostponealertremarksS6.Visible = true;
                        txtpostponeexpremarksS6.Visible = true;
                        
                        
                        hdnConfiguratoinIDS6.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS6.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS6.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS6.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS6.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS6.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS6.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS6.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS6.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS6.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS6.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS6.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS6.SelectedProduct = "Dummy";
                            lblSpcGravityS6.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS6.Enabled = false;
                            ucROBCUMS6.Enabled = true;
                            ucROBMTS6.CssClass = "readonlytextbox";
                            ucROBCUMS6.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS6.Enabled = true;
                            ucROBCUMS6.Enabled = false;
                            ucROBMTS6.CssClass = "input";
                            ucROBCUMS6.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS6.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS6.Checked == true)
                            ucDateS6.Enabled = true;
                        ucDateS6.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS6.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS6.Visible = true;
                            ucROBCharterUnitS6.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS6.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS6.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS6.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS6.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS6.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "7" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP7.Disabled = false;
                        lblTankNoP7I.Visible = true;
                        lblTankNoP7.Visible = true;
                        lbl100VolP7I.Visible = true;
                        lbl100VolP7.Visible = true;
                        lbl85VolP7I.Visible = true;
                        lbl85VolP7.Visible = true;
                        lblProductP7I.Visible = true;
                        lblProductP7.Visible = true;
                        lblSpcGravityP7I.Visible = true;
                        lblSpcGravityP7.Visible = true;
                        lblDateLoadedP7.Visible = true;
                        ucDateLoadedP7.Visible = true;
                        lblROBMTP7.Visible = true;
                        ucROBMTP7.Visible = true;
                        lblROBCUMP7.Visible = true;
                        ucROBCUMP7.Visible = true;
                        lblTankCleanedP7.Visible = true;
                        chkTankCleanedP7.Visible = true;
                        lblDateP7.Visible = true;
                        ucDateP7.Visible = true;
                        lblUnpumpableYNP7.Visible = true;
                        chkUnpumpableP7.Visible = true;
                        lblpostponealertP7.Visible = true;
                        chkpostponealertP7.Visible = true;
                        lblpostponealertremarksP7.Visible = true;
                        txtpostponeexpremarksP7.Visible = true;                        
                        
                        hdnConfiguratoinIDP7.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP7.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP7.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP7.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP7.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP7.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP7.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP7.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP7.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP7.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP7.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP7.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP7.SelectedProduct = "Dummy";
                            lblSpcGravityP7.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP7.Enabled = false;
                            ucROBCUMP7.Enabled = true;
                            ucROBMTP7.CssClass = "readonlytextbox";
                            ucROBCUMP7.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP7.Enabled = true;
                            ucROBCUMP7.Enabled = false;
                            ucROBMTP7.CssClass = "input";
                            ucROBCUMP7.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP7.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP7.Checked == true)
                            ucDateP7.Enabled = true;
                        ucDateP7.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP7.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP7.Visible = true;
                            ucROBCharterUnitP7.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP7.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP7.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP7.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP7.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP7.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "7" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI7.Disabled = false;
                        lblTankNoPI7I.Visible = true;
                        lblTankNoPI7.Visible = true;
                        lbl100VolPI7I.Visible = true;
                        lbl100VolPI7.Visible = true;
                        lbl85VolPI7I.Visible = true;
                        lbl85VolPI7.Visible = true;
                        lblProductPI7I.Visible = true;
                        lblProductPI7.Visible = true;
                        lblSpcGravityPI7I.Visible = true;
                        lblSpcGravityPI7.Visible = true;
                        lblDateLoadedPI7.Visible = true;
                        ucDateLoadedPI7.Visible = true;
                        lblROBMTPI7.Visible = true;
                        ucROBMTPI7.Visible = true;
                        lblROBCUMPI7.Visible = true;
                        ucROBCUMPI7.Visible = true;
                        lblTankCleanedPI7.Visible = true;
                        chkTankCleanedPI7.Visible = true;
                        lblDatePI7.Visible = true;
                        ucDatePI7.Visible = true;
                        lblUnpumpableYNPI7.Visible = true;
                        chkUnpumpablePI7.Visible = true;
                        lblpostponealertPI7.Visible = true;
                        chkpostponealertPI7.Visible = true;
                        lblpostponealertremarksPI7.Visible = true;
                        txtpostponeexpremarksPI7.Visible = true;                       

                        hdnConfiguratoinIDPI7.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI7.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI7.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI7.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI7.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI7.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI7.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI7.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI7.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI7.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI7.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI7.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI7.SelectedProduct = "Dummy";
                            lblSpcGravityPI7.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI7.Enabled = false;
                            ucROBCUMPI7.Enabled = true;
                            ucROBMTPI7.CssClass = "readonlytextbox";
                            ucROBCUMPI7.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI7.Enabled = true;
                            ucROBCUMPI7.Enabled = false;
                            ucROBMTPI7.CssClass = "input";
                            ucROBCUMPI7.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI7.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI7.Checked == true)
                            ucDatePI7.Enabled = true;
                        ucDatePI7.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI7.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI7.Visible = true;
                            ucROBCharterUnitPI7.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI7.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI7.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI7.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI7.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI7.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "7" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC7.Disabled = false;
                        lblTankNoC7I.Visible = true;
                        lblTankNoC7.Visible = true;
                        lbl100VolC7I.Visible = true;
                        lbl100VolC7.Visible = true;
                        lbl85VolC7I.Visible = true;
                        lbl85VolC7.Visible = true;
                        lblProductC7I.Visible = true;
                        lblProductC7.Visible = true;
                        lblSpcGravityC7I.Visible = true;
                        lblSpcGravityC7.Visible = true;
                        lblDateLoadedC7.Visible = true;
                        ucDateLoadedC7.Visible = true;
                        lblROBMTC7.Visible = true;
                        ucROBMTC7.Visible = true;
                        lblROBCUMC7.Visible = true;
                        ucROBCUMC7.Visible = true;
                        lblTankCleanedC7.Visible = true;
                        chkTankCleanedC7.Visible = true;
                        lblDateC7.Visible = true;
                        ucDateC7.Visible = true;
                        lblUnpumpableYNC7.Visible = true;
                        chkUnpumpableC7.Visible = true;
                        lblpostponealertC7.Visible = true;
                        chkpostponealertC7.Visible = true;
                        lblpostponealertremarksC7.Visible = true;
                        txtpostponeexpremarksC7.Visible = true;                        
                        
                        hdnConfiguratoinIDC7.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC7.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC7.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC7.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC7.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC7.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC7.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC7.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC7.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC7.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC7.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC7.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC7.SelectedProduct = "Dummy";
                            lblSpcGravityC7.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC7.Enabled = false;
                            ucROBCUMC7.Enabled = true;
                            ucROBMTC7.CssClass = "readonlytextbox";
                            ucROBCUMC7.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC7.Enabled = true;
                            ucROBCUMC7.Enabled = false;
                            ucROBMTC7.CssClass = "input";
                            ucROBCUMC7.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC7.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC7.Checked == true)
                            ucDateC7.Enabled = true;
                        ucDateC7.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC7.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC7.Visible = true;
                            ucROBCharterUnitC7.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC7.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC7.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC7.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC7.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC7.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "7" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI7.Disabled = false;
                        lblTankNoSI7I.Visible = true;
                        lblTankNoSI7.Visible = true;
                        lbl100VolSI7I.Visible = true;
                        lbl100VolSI7.Visible = true;
                        lbl85VolSI7I.Visible = true;
                        lbl85VolSI7.Visible = true;
                        lblProductSI7I.Visible = true;
                        lblProductSI7.Visible = true;
                        lblSpcGravitySI7I.Visible = true;
                        lblSpcGravitySI7.Visible = true;
                        lblDateLoadedSI7.Visible = true;
                        ucDateLoadedSI7.Visible = true;
                        lblROBMTSI7.Visible = true;
                        ucROBMTSI7.Visible = true;
                        lblROBCUMSI7.Visible = true;
                        ucROBCUMSI7.Visible = true;
                        lblTankCleanedSI7.Visible = true;
                        chkTankCleanedSI7.Visible = true;
                        lblDateSI7.Visible = true;
                        ucDateSI7.Visible = true;
                        lblUnpumpableYNSI7.Visible = true;
                        chkUnpumpableSI7.Visible = true;
                        lblpostponealertSI7.Visible = true;
                        chkpostponealertSI7.Visible = true;
                        lblpostponealertremarksSI7.Visible = true;
                        txtpostponeexpremarksSI7.Visible = true;
                        
                        
                        hdnConfiguratoinIDSI7.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI7.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI7.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI7.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI7.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI7.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI7.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI7.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI7.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI7.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI7.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI7.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI7.SelectedProduct = "Dummy";
                            lblSpcGravitySI7.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI7.Enabled = false;
                            ucROBCUMSI7.Enabled = true;
                            ucROBMTSI7.CssClass = "readonlytextbox";
                            ucROBCUMSI7.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI7.Enabled = true;
                            ucROBCUMSI7.Enabled = false;
                            ucROBMTSI7.CssClass = "input";
                            ucROBCUMSI7.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI7.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI7.Checked == true)
                            ucDateSI7.Enabled = true;
                        ucDateSI7.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI7.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI7.Visible = true;
                            ucROBCharterUnitSI7.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI7.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI7.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI7.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI7.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI7.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString()); 
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "7" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS7.Disabled = false;
                        lblTankNoS7I.Visible = true;
                        lblTankNoS7.Visible = true;
                        lbl100VolS7I.Visible = true;
                        lbl100VolS7.Visible = true;
                        lbl85VolS7I.Visible = true;
                        lbl85VolS7.Visible = true;
                        lblProductS7I.Visible = true;
                        lblProductS7.Visible = true;
                        lblSpcGravityS7I.Visible = true;
                        lblSpcGravityS7.Visible = true;
                        lblDateLoadedS7.Visible = true;
                        ucDateLoadedS7.Visible = true;
                        lblROBMTS7.Visible = true;
                        ucROBMTS7.Visible = true;
                        lblROBCUMS7.Visible = true;
                        ucROBCUMS7.Visible = true;
                        lblTankCleanedS7.Visible = true;
                        chkTankCleanedS7.Visible = true;
                        lblDateS7.Visible = true;
                        ucDateS7.Visible = true;
                        lblUnpumpableYNS7.Visible = true;
                        chkUnpumpableS7.Visible = true;
                        lblpostponealertS7.Visible = true;
                        chkpostponealertS7.Visible = true;
                        lblpostponealertremarksS7.Visible = true;
                        txtpostponeexpremarksS7.Visible = true;                        
                        
                        hdnConfiguratoinIDS7.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS7.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS7.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS7.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS7.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS7.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS7.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS7.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS7.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS7.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS7.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS7.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS7.SelectedProduct = "Dummy";
                            lblSpcGravityS7.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS7.Enabled = false;
                            ucROBCUMS7.Enabled = true;
                            ucROBMTS7.CssClass = "readonlytextbox";
                            ucROBCUMS7.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS7.Enabled = true;
                            ucROBCUMS7.Enabled = false;
                            ucROBMTS7.CssClass = "input";
                            ucROBCUMS7.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS7.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS7.Checked == true)
                            ucDateS7.Enabled = true;
                        ucDateS7.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS7.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS7.Visible = true;
                            ucROBCharterUnitS7.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS7.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS7.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS7.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS7.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS7.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "8" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP8.Disabled = false;
                        lblTankNoP8I.Visible = true;
                        lblTankNoP8.Visible = true;
                        lbl100VolP8I.Visible = true;
                        lbl100VolP8.Visible = true;
                        lbl85VolP8I.Visible = true;
                        lbl85VolP8.Visible = true;
                        lblProductP8I.Visible = true;
                        lblProductP8.Visible = true;
                        lblSpcGravityP8I.Visible = true;
                        lblSpcGravityP8.Visible = true;
                        lblDateLoadedP8.Visible = true;
                        ucDateLoadedP8.Visible = true;
                        lblROBMTP8.Visible = true;
                        ucROBMTP8.Visible = true;
                        lblROBCUMP8.Visible = true;
                        ucROBCUMP8.Visible = true;
                        lblTankCleanedP8.Visible = true;
                        chkTankCleanedP8.Visible = true;
                        lblDateP8.Visible = true;
                        ucDateP8.Visible = true;
                        lblUnpumpableYNP8.Visible = true;
                        chkUnpumpableP8.Visible = true;
                        lblpostponealertP8.Visible = true;
                        chkpostponealertP8.Visible = true;
                        lblpostponealertremarksP8.Visible = true;
                        txtpostponeexpremarksP8.Visible = true;                        

                        hdnConfiguratoinIDP8.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP8.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP8.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP8.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP8.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP8.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP8.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP8.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP8.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP8.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP8.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP8.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP8.SelectedProduct = "Dummy";
                            lblSpcGravityP8.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP8.Enabled = false;
                            ucROBCUMP8.Enabled = true;
                            ucROBMTP8.CssClass = "readonlytextbox";
                            ucROBCUMP8.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP8.Enabled = true;
                            ucROBCUMP8.Enabled = false;
                            ucROBMTP8.CssClass = "input";
                            ucROBCUMP8.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP8.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP8.Checked == true)
                            ucDateP8.Enabled = true;
                        ucDateP8.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP8.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP8.Visible = true;
                            ucROBCharterUnitP8.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP8.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP8.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP8.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP8.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP8.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "8" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI8.Disabled = false;
                        lblTankNoPI8I.Visible = true;
                        lblTankNoPI8.Visible = true;
                        lbl100VolPI8I.Visible = true;
                        lbl100VolPI8.Visible = true;
                        lbl85VolPI8I.Visible = true;
                        lbl85VolPI8.Visible = true;
                        lblProductPI8I.Visible = true;
                        lblProductPI8.Visible = true;
                        lblSpcGravityPI8I.Visible = true;
                        lblSpcGravityPI8.Visible = true;
                        lblDateLoadedPI8.Visible = true;
                        ucDateLoadedPI8.Visible = true;
                        lblROBMTPI8.Visible = true;
                        ucROBMTPI8.Visible = true;
                        lblROBCUMPI8.Visible = true;
                        ucROBCUMPI8.Visible = true;
                        lblTankCleanedPI8.Visible = true;
                        chkTankCleanedPI8.Visible = true;
                        lblDatePI8.Visible = true;
                        ucDatePI8.Visible = true;
                        lblUnpumpableYNPI8.Visible = true;
                        chkUnpumpablePI8.Visible = true;
                        lblpostponealertPI8.Visible = true;
                        chkpostponealertPI8.Visible = true;
                        lblpostponealertremarksPI8.Visible = true;
                        txtpostponeexpremarksPI8.Visible = true;                        
                        
                        hdnConfiguratoinIDPI8.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI8.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI8.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI8.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI8.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI8.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI8.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI8.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI8.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI8.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI8.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI8.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI8.SelectedProduct = "Dummy";
                            lblSpcGravityPI8.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI8.Enabled = false;
                            ucROBCUMPI8.Enabled = true;
                            ucROBMTPI8.CssClass = "readonlytextbox";
                            ucROBCUMPI8.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI8.Enabled = true;
                            ucROBCUMPI8.Enabled = false;
                            ucROBMTPI8.CssClass = "input";
                            ucROBCUMPI8.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI8.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI8.Checked == true)
                            ucDatePI8.Enabled = true;
                        ucDatePI8.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI8.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI8.Visible = true;
                            ucROBCharterUnitPI8.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI8.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI8.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI8.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI8.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI8.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "8" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC8.Disabled = false;
                        lblTankNoC8I.Visible = true;
                        lblTankNoC8.Visible = true;
                        lbl100VolC8I.Visible = true;
                        lbl100VolC8.Visible = true;
                        lbl85VolC8I.Visible = true;
                        lbl85VolC8.Visible = true;
                        lblProductC8I.Visible = true;
                        lblProductC8.Visible = true;
                        lblSpcGravityC8I.Visible = true;
                        lblSpcGravityC8.Visible = true;
                        lblDateLoadedC8.Visible = true;
                        ucDateLoadedC8.Visible = true;
                        lblROBMTC8.Visible = true;
                        ucROBMTC8.Visible = true;
                        lblROBCUMC8.Visible = true;
                        ucROBCUMC8.Visible = true;
                        lblTankCleanedC8.Visible = true;
                        chkTankCleanedC8.Visible = true;
                        lblDateC8.Visible = true;
                        ucDateC8.Visible = true;
                        lblUnpumpableYNC8.Visible = true;
                        chkUnpumpableC8.Visible = true;
                        lblpostponealertC8.Visible = true;
                        chkpostponealertC8.Visible = true;
                        lblpostponealertremarksC8.Visible = true;
                        txtpostponeexpremarksC8.Visible = true;                        
                        
                        hdnConfiguratoinIDC8.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC8.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC8.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC8.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC8.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC8.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC8.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC8.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC8.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC8.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC8.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC8.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC8.SelectedProduct = "Dummy";
                            lblSpcGravityC8.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC8.Enabled = false;
                            ucROBCUMC8.Enabled = true;
                            ucROBMTC8.CssClass = "readonlytextbox";
                            ucROBCUMC8.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC8.Enabled = true;
                            ucROBCUMC8.Enabled = false;
                            ucROBMTC8.CssClass = "input";
                            ucROBCUMC8.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC8.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC8.Checked == true)
                            ucDateC8.Enabled = true;
                        ucDateC8.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC8.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC8.Visible = true;
                            ucROBCharterUnitC8.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC8.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC8.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC8.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC8.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC8.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "8" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI8.Disabled = false;
                        lblTankNoSI8I.Visible = true;
                        lblTankNoSI8.Visible = true;
                        lbl100VolSI8I.Visible = true;
                        lbl100VolSI8.Visible = true;
                        lbl85VolSI8I.Visible = true;
                        lbl85VolSI8.Visible = true;
                        lblProductSI8I.Visible = true;
                        lblProductSI8.Visible = true;
                        lblSpcGravitySI8I.Visible = true;
                        lblSpcGravitySI8.Visible = true;
                        lblDateLoadedSI8.Visible = true;
                        ucDateLoadedSI8.Visible = true;
                        lblROBMTSI8.Visible = true;
                        ucROBMTSI8.Visible = true;
                        lblROBCUMSI8.Visible = true;
                        ucROBCUMSI8.Visible = true;
                        lblTankCleanedSI8.Visible = true;
                        chkTankCleanedSI8.Visible = true;
                        lblDateSI8.Visible = true;
                        ucDateSI8.Visible = true;
                        lblUnpumpableYNSI8.Visible = true;
                        chkUnpumpableSI8.Visible = true;
                        lblpostponealertSI8.Visible = true;
                        chkpostponealertSI8.Visible = true;
                        lblpostponealertremarksSI8.Visible = true;
                        txtpostponeexpremarksSI8.Visible = true;                        

                        hdnConfiguratoinIDSI8.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI8.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI8.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI8.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI8.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI8.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI8.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI8.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI8.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI8.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI8.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI8.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI8.SelectedProduct = "Dummy";
                            lblSpcGravitySI8.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI8.Enabled = false;
                            ucROBCUMSI8.Enabled = true;
                            ucROBMTSI8.CssClass = "readonlytextbox";
                            ucROBCUMSI8.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI8.Enabled = true;
                            ucROBCUMSI8.Enabled = false;
                            ucROBMTSI8.CssClass = "input";
                            ucROBCUMSI8.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI8.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI8.Checked == true)
                            ucDateSI8.Enabled = true;
                        ucDateSI8.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI8.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI8.Visible = true;
                            ucROBCharterUnitSI8.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI8.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI8.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI8.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI8.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI8.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "8" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS8.Disabled = false;
                        lblTankNoS8I.Visible = true;
                        lblTankNoS8.Visible = true;
                        lbl100VolS8I.Visible = true;
                        lbl100VolS8.Visible = true;
                        lbl85VolS8I.Visible = true;
                        lbl85VolS8.Visible = true;
                        lblProductS8I.Visible = true;
                        lblProductS8.Visible = true;
                        lblSpcGravityS8I.Visible = true;
                        lblSpcGravityS8.Visible = true;
                        lblDateLoadedS8.Visible = true;
                        ucDateLoadedS8.Visible = true;
                        lblROBMTS8.Visible = true;
                        ucROBMTS8.Visible = true;
                        lblROBCUMS8.Visible = true;
                        ucROBCUMS8.Visible = true;
                        lblTankCleanedS8.Visible = true;
                        chkTankCleanedS8.Visible = true;
                        lblDateS8.Visible = true;
                        ucDateS8.Visible = true;
                        lblUnpumpableYNS8.Visible = true;
                        chkUnpumpableS8.Visible = true;
                        lblpostponealertS8.Visible = true;
                        chkpostponealertS8.Visible = true;
                        lblpostponealertremarksS8.Visible = true;
                        txtpostponeexpremarksS8.Visible = true;                        
                        
                        hdnConfiguratoinIDS8.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS8.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS8.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS8.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS8.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS8.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS8.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS8.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS8.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS8.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS8.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS8.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS8.SelectedProduct = "Dummy";
                            lblSpcGravityS8.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS8.Enabled = false;
                            ucROBCUMS8.Enabled = true;
                            ucROBMTS8.CssClass = "readonlytextbox";
                            ucROBCUMS8.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS8.Enabled = true;
                            ucROBCUMS8.Enabled = false;
                            ucROBMTS8.CssClass = "input";
                            ucROBCUMS8.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS8.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS8.Checked == true)
                            ucDateS8.Enabled = true;
                        ucDateS8.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS8.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS8.Visible = true;
                            ucROBCharterUnitS8.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS8.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS8.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS8.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS8.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS8.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "9" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP9.Disabled = false;
                        lblTankNoP9I.Visible = true;
                        lblTankNoP9.Visible = true;
                        lbl100VolP9I.Visible = true;
                        lbl100VolP9.Visible = true;
                        lbl85VolP9I.Visible = true;
                        lbl85VolP9.Visible = true;
                        lblProductP9I.Visible = true;
                        lblProductP9.Visible = true;
                        lblSpcGravityP9I.Visible = true;
                        lblSpcGravityP9.Visible = true;
                        lblDateLoadedP9.Visible = true;
                        ucDateLoadedP9.Visible = true;
                        lblROBMTP9.Visible = true;
                        ucROBMTP9.Visible = true;
                        lblROBCUMP9.Visible = true;
                        ucROBCUMP9.Visible = true;
                        lblTankCleanedP9.Visible = true;
                        chkTankCleanedP9.Visible = true;
                        lblDateP9.Visible = true;
                        ucDateP9.Visible = true;
                        lblUnpumpableYNP9.Visible = true;
                        chkUnpumpableP9.Visible = true;
                        lblpostponealertP9.Visible = true;
                        chkpostponealertP9.Visible = true;
                        lblpostponealertremarksP9.Visible = true;
                        txtpostponeexpremarksP9.Visible = true;                        

                        hdnConfiguratoinIDP9.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP9.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP9.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP9.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP9.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP9.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP9.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP9.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP9.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP9.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP9.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP9.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP9.SelectedProduct = "Dummy";
                            lblSpcGravityP9.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP9.Enabled = false;
                            ucROBCUMP9.Enabled = true;
                            ucROBMTP9.CssClass = "readonlytextbox";
                            ucROBCUMP9.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP9.Enabled = true;
                            ucROBCUMP9.Enabled = false;
                            ucROBMTP9.CssClass = "input";
                            ucROBCUMP9.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP9.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP9.Checked == true)
                            ucDateP9.Enabled = true;
                        ucDateP9.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP9.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP9.Visible = true;
                            ucROBCharterUnitP9.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP9.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP9.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP9.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP9.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP9.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "9" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI9.Disabled = false;
                        lblTankNoPI9I.Visible = true;
                        lblTankNoPI9.Visible = true;
                        lbl100VolPI9I.Visible = true;
                        lbl100VolPI9.Visible = true;
                        lbl85VolPI9I.Visible = true;
                        lbl85VolPI9.Visible = true;
                        lblProductPI9I.Visible = true;
                        lblProductPI9.Visible = true;
                        lblSpcGravityPI9I.Visible = true;
                        lblSpcGravityPI9.Visible = true;
                        lblDateLoadedPI9.Visible = true;
                        ucDateLoadedPI9.Visible = true;
                        lblROBMTPI9.Visible = true;
                        ucROBMTPI9.Visible = true;
                        lblROBCUMPI9.Visible = true;
                        ucROBCUMPI9.Visible = true;
                        lblTankCleanedPI9.Visible = true;
                        chkTankCleanedPI9.Visible = true;
                        lblDatePI9.Visible = true;
                        ucDatePI9.Visible = true;
                        lblUnpumpableYNPI9.Visible = true;
                        chkUnpumpablePI9.Visible = true;
                        lblpostponealertPI9.Visible = true;
                        chkpostponealertPI9.Visible = true;
                        lblpostponealertremarksPI9.Visible = true;
                        txtpostponeexpremarksPI9.Visible = true;                        
                        
                        hdnConfiguratoinIDPI9.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI9.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI9.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI9.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI9.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI9.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI9.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI9.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI9.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI9.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI9.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI9.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI9.SelectedProduct = "Dummy";
                            lblSpcGravityPI9.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI9.Enabled = false;
                            ucROBCUMPI9.Enabled = true;
                            ucROBMTPI9.CssClass = "readonlytextbox";
                            ucROBCUMPI9.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI9.Enabled = true;
                            ucROBCUMPI9.Enabled = false;
                            ucROBMTPI9.CssClass = "input";
                            ucROBCUMPI9.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI9.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI9.Checked == true)
                            ucDatePI9.Enabled = true;
                        ucDatePI9.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI9.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI9.Visible = true;
                            ucROBCharterUnitPI9.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI9.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI9.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI9.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI9.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI9.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "9" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC9.Disabled = false;
                        lblTankNoC9I.Visible = true;
                        lblTankNoC9.Visible = true;
                        lbl100VolC9I.Visible = true;
                        lbl100VolC9.Visible = true;
                        lbl85VolC9I.Visible = true;
                        lbl85VolC9.Visible = true;
                        lblProductC9I.Visible = true;
                        lblProductC9.Visible = true;
                        lblSpcGravityC9I.Visible = true;
                        lblSpcGravityC9.Visible = true;
                        lblDateLoadedC9.Visible = true;
                        ucDateLoadedC9.Visible = true;
                        lblROBMTC9.Visible = true;
                        ucROBMTC9.Visible = true;
                        lblROBCUMC9.Visible = true;
                        ucROBCUMC9.Visible = true;
                        lblTankCleanedC9.Visible = true;
                        chkTankCleanedC9.Visible = true;
                        lblDateC9.Visible = true;
                        ucDateC9.Visible = true;
                        lblUnpumpableYNC9.Visible = true;
                        chkUnpumpableC9.Visible = true;
                        lblpostponealertC9.Visible = true;
                        chkpostponealertC9.Visible = true;
                        lblpostponealertremarksC9.Visible = true;
                        txtpostponeexpremarksC9.Visible = true;                        

                        hdnConfiguratoinIDC9.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC9.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC9.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC9.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC9.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC9.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC9.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC9.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC9.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC9.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC9.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC9.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC9.SelectedProduct = "Dummy";
                            lblSpcGravityC9.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC9.Enabled = false;
                            ucROBCUMC9.Enabled = true;
                            ucROBMTC9.CssClass = "readonlytextbox";
                            ucROBCUMC9.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC9.Enabled = true;
                            ucROBCUMC9.Enabled = false;
                            ucROBMTC9.CssClass = "input";
                            ucROBCUMC9.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC9.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC9.Checked == true)
                            ucDateC9.Enabled = true;
                        ucDateC9.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC9.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC9.Visible = true;
                            ucROBCharterUnitC9.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC9.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC9.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC9.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC9.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC9.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "9" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI9.Disabled = false;
                        lblTankNoSI9I.Visible = true;
                        lblTankNoSI9.Visible = true;
                        lbl100VolSI9I.Visible = true;
                        lbl100VolSI9.Visible = true;
                        lbl85VolSI9I.Visible = true;
                        lbl85VolSI9.Visible = true;
                        lblProductSI9I.Visible = true;
                        lblProductSI9.Visible = true;
                        lblSpcGravitySI9I.Visible = true;
                        lblSpcGravitySI9.Visible = true;
                        lblDateLoadedSI9.Visible = true;
                        ucDateLoadedSI9.Visible = true;
                        lblROBMTSI9.Visible = true;
                        ucROBMTSI9.Visible = true;
                        lblROBCUMSI9.Visible = true;
                        ucROBCUMSI9.Visible = true;
                        lblTankCleanedSI9.Visible = true;
                        chkTankCleanedSI9.Visible = true;
                        lblDateSI9.Visible = true;
                        ucDateSI9.Visible = true;
                        lblUnpumpableYNSI9.Visible = true;
                        chkUnpumpableSI9.Visible = true;
                        lblpostponealertSI9.Visible = true;
                        chkpostponealertSI9.Visible = true;
                        lblpostponealertremarksSI9.Visible = true;
                        txtpostponeexpremarksSI9.Visible = true;
                        

                        hdnConfiguratoinIDSI9.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI9.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI9.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI9.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI9.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI9.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI9.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI9.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI9.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI9.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI9.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI9.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI9.SelectedProduct = "Dummy";
                            lblSpcGravitySI9.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI9.Enabled = false;
                            ucROBCUMSI9.Enabled = true;
                            ucROBMTSI9.CssClass = "readonlytextbox";
                            ucROBCUMSI9.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI9.Enabled = true;
                            ucROBCUMSI9.Enabled = false;
                            ucROBMTSI9.CssClass = "input";
                            ucROBCUMSI9.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI9.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI9.Checked == true)
                            ucDateSI9.Enabled = true;
                        ucDateSI9.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI9.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI9.Visible = true;
                            ucROBCharterUnitSI9.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI9.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI9.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI9.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI9.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI9.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "9" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS9.Disabled = false;
                        lblTankNoS9I.Visible = true;
                        lblTankNoS9.Visible = true;
                        lbl100VolS9I.Visible = true;
                        lbl100VolS9.Visible = true;
                        lbl85VolS9I.Visible = true;
                        lbl85VolS9.Visible = true;
                        lblProductS9I.Visible = true;
                        lblProductS9.Visible = true;
                        lblSpcGravityS9I.Visible = true;
                        lblSpcGravityS9.Visible = true;
                        lblDateLoadedS9.Visible = true;
                        ucDateLoadedS9.Visible = true;
                        lblROBMTS9.Visible = true;
                        ucROBMTS9.Visible = true;
                        lblROBCUMS9.Visible = true;
                        ucROBCUMS9.Visible = true;
                        lblTankCleanedS9.Visible = true;
                        chkTankCleanedS9.Visible = true;
                        lblDateS9.Visible = true;
                        ucDateS9.Visible = true;
                        lblUnpumpableYNS9.Visible = true;
                        chkUnpumpableS9.Visible = true;
                        lblpostponealertS9.Visible = true;
                        chkpostponealertS9.Visible = true;
                        lblpostponealertremarksS9.Visible = true;
                        txtpostponeexpremarksS9.Visible = true;
                        
                        
                        hdnConfiguratoinIDS9.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS9.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS9.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS9.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS9.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS9.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS9.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS9.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS9.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS9.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS9.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS9.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS9.SelectedProduct = "Dummy";
                            lblSpcGravityS9.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS9.Enabled = false;
                            ucROBCUMS9.Enabled = true;
                            ucROBMTS9.CssClass = "readonlytextbox";
                            ucROBCUMS9.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS9.Enabled = true;
                            ucROBCUMS9.Enabled = false;
                            ucROBMTS9.CssClass = "input";
                            ucROBCUMS9.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS9.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS9.Checked == true)
                            ucDateS9.Enabled = true;
                        ucDateS9.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS9.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS9.Visible = true;
                            ucROBCharterUnitS9.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS9.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS9.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS9.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS9.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS9.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "10" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP10.Disabled = false;
                        lblTankNoP10I.Visible = true;
                        lblTankNoP10.Visible = true;
                        lbl100VolP10I.Visible = true;
                        lbl100VolP10.Visible = true;
                        lbl85VolP10I.Visible = true;
                        lbl85VolP10.Visible = true;
                        lblProductP10I.Visible = true;
                        lblProductP10.Visible = true;
                        lblSpcGravityP10I.Visible = true;
                        lblSpcGravityP10.Visible = true;
                        lblDateLoadedP10.Visible = true;
                        ucDateLoadedP10.Visible = true;
                        lblROBMTP10.Visible = true;
                        ucROBMTP10.Visible = true;
                        lblROBCUMP10.Visible = true;
                        ucROBCUMP10.Visible = true;
                        lblTankCleanedP10.Visible = true;
                        chkTankCleanedP10.Visible = true;
                        lblDateP10.Visible = true;
                        ucDateP10.Visible = true;
                        lblUnpumpableYNP10.Visible = true;
                        chkUnpumpableP10.Visible = true;
                        lblpostponealertP10.Visible = true;
                        chkpostponealertP10.Visible = true;
                        lblpostponealertremarksP10.Visible = true;
                        txtpostponeexpremarksP10.Visible = true;
                        

                        hdnConfiguratoinIDP10.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP10.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP10.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP10.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP10.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP10.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP10.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP10.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP10.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP10.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP10.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP10.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP10.SelectedProduct = "Dummy";
                            lblSpcGravityP10.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP10.Enabled = false;
                            ucROBCUMP10.Enabled = true;
                            ucROBMTP10.CssClass = "readonlytextbox";
                            ucROBCUMP10.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP10.Enabled = true;
                            ucROBCUMP10.Enabled = false;
                            ucROBMTP10.CssClass = "input";
                            ucROBCUMP10.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP10.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP10.Checked == true)
                            ucDateP10.Enabled = true;
                        ucDateP10.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP10.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP10.Visible = true;
                            ucROBCharterUnitP10.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP10.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP10.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP10.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP10.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP10.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "10" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI10.Disabled = false;
                        lblTankNoPI10I.Visible = true;
                        lblTankNoPI10.Visible = true;
                        lbl100VolPI10I.Visible = true;
                        lbl100VolPI10.Visible = true;
                        lbl85VolPI10I.Visible = true;
                        lbl85VolPI10.Visible = true;
                        lblProductPI10I.Visible = true;
                        lblProductPI10.Visible = true;
                        lblSpcGravityPI10I.Visible = true;
                        lblSpcGravityPI10.Visible = true;
                        lblDateLoadedPI10.Visible = true;
                        ucDateLoadedPI10.Visible = true;
                        lblROBMTPI10.Visible = true;
                        ucROBMTPI10.Visible = true;
                        lblROBCUMPI10.Visible = true;
                        ucROBCUMPI10.Visible = true;
                        lblTankCleanedPI10.Visible = true;
                        chkTankCleanedPI10.Visible = true;
                        lblDatePI10.Visible = true;
                        ucDatePI10.Visible = true;
                        lblUnpumpableYNPI10.Visible = true;
                        chkUnpumpablePI10.Visible = true;
                        lblpostponealertPI10.Visible = true;
                        chkpostponealertPI10.Visible = true;
                        lblpostponealertremarksPI10.Visible = true;
                        txtpostponeexpremarksPI10.Visible = true;                        

                        hdnConfiguratoinIDPI10.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI10.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI10.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI10.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI10.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI10.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI10.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI10.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI10.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI10.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI10.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI10.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI10.SelectedProduct = "Dummy";
                            lblSpcGravityPI10.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI10.Enabled = false;
                            ucROBCUMPI10.Enabled = true;
                            ucROBMTPI10.CssClass = "readonlytextbox";
                            ucROBCUMPI10.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI10.Enabled = true;
                            ucROBCUMPI10.Enabled = false;
                            ucROBMTPI10.CssClass = "input";
                            ucROBCUMPI10.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI10.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI10.Checked == true)
                            ucDatePI10.Enabled = true;
                        ucDatePI10.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI10.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI10.Visible = true;
                            ucROBCharterUnitPI10.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI10.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI10.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI10.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI10.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI10.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "10" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC10.Disabled = false;
                        lblTankNoC10I.Visible = true;
                        lblTankNoC10.Visible = true;
                        lbl100VolC10I.Visible = true;
                        lbl100VolC10.Visible = true;
                        lbl85VolC10I.Visible = true;
                        lbl85VolC10.Visible = true;
                        lblProductC10I.Visible = true;
                        lblProductC10.Visible = true;
                        lblSpcGravityC10I.Visible = true;
                        lblSpcGravityC10.Visible = true;
                        lblDateLoadedC10.Visible = true;
                        ucDateLoadedC10.Visible = true;
                        lblROBMTC10.Visible = true;
                        ucROBMTC10.Visible = true;
                        lblROBCUMC10.Visible = true;
                        ucROBCUMC10.Visible = true;
                        lblTankCleanedC10.Visible = true;
                        chkTankCleanedC10.Visible = true;
                        lblDateC10.Visible = true;
                        ucDateC10.Visible = true;
                        lblUnpumpableYNC10.Visible = true;
                        chkUnpumpableC10.Visible = true;
                        lblpostponealertC10.Visible = true;
                        chkpostponealertC10.Visible = true;
                        lblpostponealertremarksC10.Visible = true;
                        txtpostponeexpremarksC10.Visible = true;                        
                        
                        hdnConfiguratoinIDC10.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC10.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC10.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC10.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC10.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC10.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC10.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC10.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC10.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC10.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC10.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC10.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC10.SelectedProduct = "Dummy";
                            lblSpcGravityC10.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC10.Enabled = false;
                            ucROBCUMC10.Enabled = true;
                            ucROBMTC10.CssClass = "readonlytextbox";
                            ucROBCUMC10.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC10.Enabled = true;
                            ucROBCUMC10.Enabled = false;
                            ucROBMTC10.CssClass = "input";
                            ucROBCUMC10.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC10.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC10.Checked == true)
                            ucDateC10.Enabled = true;
                        ucDateC10.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC10.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC10.Visible = true;
                            ucROBCharterUnitC10.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC10.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC10.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC10.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC10.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC10.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "10" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI10.Disabled = false;
                        lblTankNoSI10I.Visible = true;
                        lblTankNoSI10.Visible = true;
                        lbl100VolSI10I.Visible = true;
                        lbl100VolSI10.Visible = true;
                        lbl85VolSI10I.Visible = true;
                        lbl85VolSI10.Visible = true;
                        lblProductSI10I.Visible = true;
                        lblProductSI10.Visible = true;
                        lblSpcGravitySI10I.Visible = true;
                        lblSpcGravitySI10.Visible = true;
                        lblDateLoadedSI10.Visible = true;
                        ucDateLoadedSI10.Visible = true;
                        lblROBMTSI10.Visible = true;
                        ucROBMTSI10.Visible = true;
                        lblROBCUMSI10.Visible = true;
                        ucROBCUMSI10.Visible = true;
                        lblTankCleanedSI10.Visible = true;
                        chkTankCleanedSI10.Visible = true;
                        lblDateSI10.Visible = true;
                        ucDateSI10.Visible = true;
                        lblUnpumpableYNSI10.Visible = true;
                        chkUnpumpableSI10.Visible = true;
                        lblpostponealertSI10.Visible = true;
                        chkpostponealertSI10.Visible = true;
                        lblpostponealertremarksSI10.Visible = true;
                        txtpostponeexpremarksSI10.Visible = true;                        
                        
                        hdnConfiguratoinIDSI10.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI10.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI10.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI10.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI10.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI10.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI10.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI10.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI10.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI10.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI10.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI10.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI10.SelectedProduct = "Dummy";
                            lblSpcGravitySI10.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI10.Enabled = false;
                            ucROBCUMSI10.Enabled = true;
                            ucROBMTSI10.CssClass = "readonlytextbox";
                            ucROBCUMSI10.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI10.Enabled = true;
                            ucROBCUMSI10.Enabled = false;
                            ucROBMTSI10.CssClass = "input";
                            ucROBCUMSI10.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI10.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI10.Checked == true)
                            ucDateSI10.Enabled = true;
                        ucDateSI10.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI10.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI10.Visible = true;
                            ucROBCharterUnitSI10.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI10.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI10.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI10.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI10.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI10.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "10" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS10.Disabled = false;
                        lblTankNoS10I.Visible = true;
                        lblTankNoS10.Visible = true;
                        lbl100VolS10I.Visible = true;
                        lbl100VolS10.Visible = true;
                        lbl85VolS10I.Visible = true;
                        lbl85VolS10.Visible = true;
                        lblProductS10I.Visible = true;
                        lblProductS10.Visible = true;
                        lblSpcGravityS10I.Visible = true;
                        lblSpcGravityS10.Visible = true;
                        lblDateLoadedS10.Visible = true;
                        ucDateLoadedS10.Visible = true;
                        lblROBMTS10.Visible = true;
                        ucROBMTS10.Visible = true;
                        lblROBCUMS10.Visible = true;
                        ucROBCUMS10.Visible = true;
                        lblTankCleanedS10.Visible = true;
                        chkTankCleanedS10.Visible = true;
                        lblDateS10.Visible = true;
                        ucDateS10.Visible = true;
                        lblUnpumpableYNS10.Visible = true;
                        chkUnpumpableS10.Visible = true;
                        lblpostponealertS10.Visible = true;
                        chkpostponealertS10.Visible = true;
                        lblpostponealertremarksS10.Visible = true;
                        txtpostponeexpremarksS10.Visible = true;                        
                                                
                        hdnConfiguratoinIDS10.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS10.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS10.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS10.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS10.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS10.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS10.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS10.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS10.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS10.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS10.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS10.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS10.SelectedProduct = "Dummy";
                            lblSpcGravityS10.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS10.Enabled = false;
                            ucROBCUMS10.Enabled = true;
                            ucROBMTS10.CssClass = "readonlytextbox";
                            ucROBCUMS10.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS10.Enabled = true;
                            ucROBCUMS10.Enabled = false;
                            ucROBMTS10.CssClass = "input";
                            ucROBCUMS10.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS10.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS10.Checked == true)
                            ucDateS10.Enabled = true;
                        ucDateS10.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS10.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS10.Visible = true;
                            ucROBCharterUnitS10.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS10.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS10.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS10.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS10.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS10.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "11" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP11.Disabled = false;
                        lblTankNoP11I.Visible = true;
                        lblTankNoP11.Visible = true;
                        lbl100VolP11I.Visible = true;
                        lbl100VolP11.Visible = true;
                        lbl85VolP11I.Visible = true;
                        lbl85VolP11.Visible = true;
                        lblProductP11I.Visible = true;
                        lblProductP11.Visible = true;
                        lblSpcGravityP11I.Visible = true;
                        lblSpcGravityP11.Visible = true;
                        lblDateLoadedP11.Visible = true;
                        ucDateLoadedP11.Visible = true;
                        lblROBMTP11.Visible = true;
                        ucROBMTP11.Visible = true;
                        lblROBCUMP11.Visible = true;
                        ucROBCUMP11.Visible = true;
                        lblTankCleanedP11.Visible = true;
                        chkTankCleanedP11.Visible = true;
                        lblDateP11.Visible = true;
                        ucDateP11.Visible = true;
                        lblUnpumpableYNP11.Visible = true;
                        chkUnpumpableP11.Visible = true;
                        lblpostponealertP11.Visible = true;
                        chkpostponealertP11.Visible = true;
                        lblpostponealertremarksP11.Visible = true;
                        txtpostponeexpremarksP11.Visible = true;                        

                        hdnConfiguratoinIDP11.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP11.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP11.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP11.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP11.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP11.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP11.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP11.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP11.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP11.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP11.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP11.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP11.SelectedProduct = "Dummy";
                            lblSpcGravityP11.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP11.Enabled = false;
                            ucROBCUMP11.Enabled = true;
                            ucROBMTP11.CssClass = "readonlytextbox";
                            ucROBCUMP11.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP11.Enabled = true;
                            ucROBCUMP11.Enabled = false;
                            ucROBMTP11.CssClass = "input";
                            ucROBCUMP11.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP11.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP11.Checked == true)
                            ucDateP11.Enabled = true;
                        ucDateP11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP11.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP11.Visible = true;
                            ucROBCharterUnitP11.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP11.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP11.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP11.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP11.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "11" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI11.Disabled = false;
                        lblTankNoPI11I.Visible = true;
                        lblTankNoPI11.Visible = true;
                        lbl100VolPI11I.Visible = true;
                        lbl100VolPI11.Visible = true;
                        lbl85VolPI11I.Visible = true;
                        lbl85VolPI11.Visible = true;
                        lblProductPI11I.Visible = true;
                        lblProductPI11.Visible = true;
                        lblSpcGravityPI11I.Visible = true;
                        lblSpcGravityPI11.Visible = true;
                        lblDateLoadedPI11.Visible = true;
                        ucDateLoadedPI11.Visible = true;
                        lblROBMTPI11.Visible = true;
                        ucROBMTPI11.Visible = true;
                        lblROBCUMPI11.Visible = true;
                        ucROBCUMPI11.Visible = true;
                        lblTankCleanedPI11.Visible = true;
                        chkTankCleanedPI11.Visible = true;
                        lblDatePI11.Visible = true;
                        ucDatePI11.Visible = true;
                        lblUnpumpableYNPI11.Visible = true;
                        chkUnpumpablePI11.Visible = true;
                        lblpostponealertPI11.Visible = true;
                        chkpostponealertPI11.Visible = true;
                        lblpostponealertremarksPI11.Visible = true;
                        txtpostponeexpremarksPI11.Visible = true;
                        

                        hdnConfiguratoinIDPI11.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI11.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI11.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI11.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI11.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI11.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI11.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI11.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI11.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI11.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI11.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI11.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI11.SelectedProduct = "Dummy";
                            lblSpcGravityPI11.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI11.Enabled = false;
                            ucROBCUMPI11.Enabled = true;
                            ucROBMTPI11.CssClass = "readonlytextbox";
                            ucROBCUMPI11.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI11.Enabled = true;
                            ucROBCUMPI11.Enabled = false;
                            ucROBMTPI11.CssClass = "input";
                            ucROBCUMPI11.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI11.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI11.Checked == true)
                            ucDatePI11.Enabled = true;
                        ucDatePI11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI11.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI11.Visible = true;
                            ucROBCharterUnitPI11.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI11.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI11.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI11.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI11.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "11" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC11.Disabled = false;
                        lblTankNoC11I.Visible = true;
                        lblTankNoC11.Visible = true;
                        lbl100VolC11I.Visible = true;
                        lbl100VolC11.Visible = true;
                        lbl85VolC11I.Visible = true;
                        lbl85VolC11.Visible = true;
                        lblProductC11I.Visible = true;
                        lblProductC11.Visible = true;
                        lblSpcGravityC11I.Visible = true;
                        lblSpcGravityC11.Visible = true;
                        lblDateLoadedC11.Visible = true;
                        ucDateLoadedC11.Visible = true;
                        lblROBMTC11.Visible = true;
                        ucROBMTC11.Visible = true;
                        lblROBCUMC11.Visible = true;
                        ucROBCUMC11.Visible = true;
                        lblTankCleanedC11.Visible = true;
                        chkTankCleanedC11.Visible = true;
                        lblDateC11.Visible = true;
                        ucDateC11.Visible = true;
                        lblUnpumpableYNC11.Visible = true;
                        chkUnpumpableC11.Visible = true;
                        lblpostponealertC11.Visible = true;
                        chkpostponealertC11.Visible = true;
                        lblpostponealertremarksC11.Visible = true;
                        txtpostponeexpremarksC11.Visible = true;                        
                        
                        hdnConfiguratoinIDC11.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC11.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC11.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC11.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC11.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC11.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC11.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC11.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC11.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC11.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC11.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC11.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC11.SelectedProduct = "Dummy";
                            lblSpcGravityC11.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC11.Enabled = false;
                            ucROBCUMC11.Enabled = true;
                            ucROBMTC11.CssClass = "readonlytextbox";
                            ucROBCUMC11.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC11.Enabled = true;
                            ucROBCUMC11.Enabled = false;
                            ucROBMTC11.CssClass = "input";
                            ucROBCUMC11.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC11.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC11.Checked == true)
                            ucDateC11.Enabled = true;
                        ucDateC11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC11.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC11.Visible = true;
                            ucROBCharterUnitC11.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC11.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC11.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC11.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC11.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "11" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI11.Disabled = false;
                        lblTankNoSI11I.Visible = true;
                        lblTankNoSI11.Visible = true;
                        lbl100VolSI11I.Visible = true;
                        lbl100VolSI11.Visible = true;
                        lbl85VolSI11I.Visible = true;
                        lbl85VolSI11.Visible = true;
                        lblProductSI11I.Visible = true;
                        lblProductSI11.Visible = true;
                        lblSpcGravitySI11I.Visible = true;
                        lblSpcGravitySI11.Visible = true;
                        lblDateLoadedSI11.Visible = true;
                        ucDateLoadedSI11.Visible = true;
                        lblROBMTSI11.Visible = true;
                        ucROBMTSI11.Visible = true;
                        lblROBCUMSI11.Visible = true;
                        ucROBCUMSI11.Visible = true;
                        lblTankCleanedSI11.Visible = true;
                        chkTankCleanedSI11.Visible = true;
                        lblDateSI11.Visible = true;
                        ucDateSI11.Visible = true;
                        lblUnpumpableYNSI11.Visible = true;
                        chkUnpumpableSI11.Visible = true;
                        lblpostponealertSI11.Visible = true;
                        chkpostponealertSI11.Visible = true;
                        lblpostponealertremarksSI11.Visible = true;
                        txtpostponeexpremarksSI11.Visible = true;                        
                        
                        hdnConfiguratoinIDSI11.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI11.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI11.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI11.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI11.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI11.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI11.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI11.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI11.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI11.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI11.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI11.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI11.SelectedProduct = "Dummy";
                            lblSpcGravitySI11.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI11.Enabled = false;
                            ucROBCUMSI11.Enabled = true;
                            ucROBMTSI11.CssClass = "readonlytextbox";
                            ucROBCUMSI11.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI11.Enabled = true;
                            ucROBCUMSI11.Enabled = false;
                            ucROBMTSI11.CssClass = "input";
                            ucROBCUMSI11.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI11.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI11.Checked == true)
                            ucDateSI11.Enabled = true;
                        ucDateSI11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI11.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI11.Visible = true;
                            ucROBCharterUnitSI11.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI11.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI11.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI11.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI11.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "11" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS11.Disabled = false;
                        lblTankNoS11I.Visible = true;
                        lblTankNoS11.Visible = true;
                        lbl100VolS11I.Visible = true;
                        lbl100VolS11.Visible = true;
                        lbl85VolS11I.Visible = true;
                        lbl85VolS11.Visible = true;
                        lblProductS11I.Visible = true;
                        lblProductS11.Visible = true;
                        lblSpcGravityS11I.Visible = true;
                        lblSpcGravityS11.Visible = true;
                        lblDateLoadedS11.Visible = true;
                        ucDateLoadedS11.Visible = true;
                        lblROBMTS11.Visible = true;
                        ucROBMTS11.Visible = true;
                        lblROBCUMS11.Visible = true;
                        ucROBCUMS11.Visible = true;
                        lblTankCleanedS11.Visible = true;
                        chkTankCleanedS11.Visible = true;
                        lblDateS11.Visible = true;
                        ucDateS11.Visible = true;
                        lblUnpumpableYNS11.Visible = true;
                        chkUnpumpableS11.Visible = true;
                        lblpostponealertS11.Visible = true;
                        chkpostponealertS11.Visible = true;
                        lblpostponealertremarksS11.Visible = true;
                        txtpostponeexpremarksS11.Visible = true;                        
                        
                        hdnConfiguratoinIDS11.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS11.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS11.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS11.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS11.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS11.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS11.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS11.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS11.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS11.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS11.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS11.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS11.SelectedProduct = "Dummy";
                            lblSpcGravityS11.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS11.Enabled = false;
                            ucROBCUMS11.Enabled = true;
                            ucROBMTS11.CssClass = "readonlytextbox";
                            ucROBCUMS11.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS11.Enabled = true;
                            ucROBCUMS11.Enabled = false;
                            ucROBMTS11.CssClass = "input";
                            ucROBCUMS11.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS11.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS11.Checked == true)
                            ucDateS11.Enabled = true;
                        ucDateS11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS11.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS11.Visible = true;
                            ucROBCharterUnitS11.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS11.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS11.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS11.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS11.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "12" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP12.Disabled = false;
                        lblTankNoP12I.Visible = true;
                        lblTankNoP12.Visible = true;
                        lbl100VolP12I.Visible = true;
                        lbl100VolP12.Visible = true;
                        lbl85VolP12I.Visible = true;
                        lbl85VolP12.Visible = true;
                        lblProductP12I.Visible = true;
                        lblProductP12.Visible = true;
                        lblSpcGravityP12I.Visible = true;
                        lblSpcGravityP12.Visible = true;
                        lblDateLoadedP12.Visible = true;
                        ucDateLoadedP12.Visible = true;
                        lblROBMTP12.Visible = true;
                        ucROBMTP12.Visible = true;
                        lblROBCUMP12.Visible = true;
                        ucROBCUMP12.Visible = true;
                        lblTankCleanedP12.Visible = true;
                        chkTankCleanedP12.Visible = true;
                        lblDateP12.Visible = true;
                        ucDateP12.Visible = true;
                        lblUnpumpableYNP12.Visible = true;
                        chkUnpumpableP12.Visible = true;
                        lblpostponealertP12.Visible = true;
                        chkpostponealertP12.Visible = true;
                        lblpostponealertremarksP12.Visible = true;
                        txtpostponeexpremarksP12.Visible = true;
                        
                        
                        hdnConfiguratoinIDP12.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP12.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP12.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP12.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP12.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP12.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP12.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP12.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP12.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP12.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP12.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP12.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP12.SelectedProduct = "Dummy";
                            lblSpcGravityP12.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP12.Enabled = false;
                            ucROBCUMP12.Enabled = true;
                            ucROBMTP12.CssClass = "readonlytextbox";
                            ucROBCUMP12.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBCUMP12.Enabled = false;
                        }
                        chkTankCleanedP12.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP12.Checked == true)
                            ucDateP12.Enabled = true;
                        ucDateP12.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP12.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP12.Visible = true;
                            ucROBCharterUnitP12.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP12.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP12.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP12.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP12.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP12.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "12" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI12.Disabled = false;
                        lblTankNoPI12I.Visible = true;
                        lblTankNoPI12.Visible = true;
                        lbl100VolPI12I.Visible = true;
                        lbl100VolPI12.Visible = true;
                        lbl85VolPI12I.Visible = true;
                        lbl85VolPI12.Visible = true;
                        lblProductPI12I.Visible = true;
                        lblProductPI12.Visible = true;
                        lblSpcGravityPI12I.Visible = true;
                        lblSpcGravityPI12.Visible = true;
                        lblDateLoadedPI12.Visible = true;
                        ucDateLoadedPI12.Visible = true;
                        lblROBMTPI12.Visible = true;
                        ucROBMTPI12.Visible = true;
                        lblROBCUMPI12.Visible = true;
                        ucROBCUMPI12.Visible = true;
                        lblTankCleanedPI12.Visible = true;
                        chkTankCleanedPI12.Visible = true;
                        lblDatePI12.Visible = true;
                        ucDatePI12.Visible = true;
                        lblUnpumpableYNPI12.Visible = true;
                        chkUnpumpablePI12.Visible = true;
                        lblpostponealertPI12.Visible = true;
                        chkpostponealertPI12.Visible = true;
                        lblpostponealertremarksPI12.Visible = true;
                        txtpostponeexpremarksPI12.Visible = true;
                        
                        hdnConfiguratoinIDPI12.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI12.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI12.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI12.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI12.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI12.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI12.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI12.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI12.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI12.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI12.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI12.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI12.SelectedProduct = "Dummy";
                            lblSpcGravityPI12.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI12.Enabled = false;
                            ucROBCUMPI12.Enabled = true;
                            ucROBMTPI12.CssClass = "readonlytextbox";
                            ucROBCUMPI12.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI12.Enabled = true;
                            ucROBCUMPI12.Enabled = false;
                            ucROBMTPI12.CssClass = "input";
                            ucROBCUMPI12.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI12.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI12.Checked == true)
                            ucDatePI12.Enabled = true;
                        ucDatePI12.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI12.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI12.Visible = true;
                            ucROBCharterUnitPI12.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI12.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI12.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI12.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI12.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI12.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "12" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC12.Disabled = false;
                        lblTankNoC12I.Visible = true;
                        lblTankNoC12.Visible = true;
                        lbl100VolC12I.Visible = true;
                        lbl100VolC12.Visible = true;
                        lbl85VolC12I.Visible = true;
                        lbl85VolC12.Visible = true;
                        lblProductC12I.Visible = true;
                        lblProductC12.Visible = true;
                        lblSpcGravityC12I.Visible = true;
                        lblSpcGravityC12.Visible = true;
                        lblDateLoadedC12.Visible = true;
                        ucDateLoadedC12.Visible = true;
                        lblROBMTC12.Visible = true;
                        ucROBMTC12.Visible = true;
                        lblROBCUMC12.Visible = true;
                        ucROBCUMC12.Visible = true;
                        lblTankCleanedC12.Visible = true;
                        chkTankCleanedC12.Visible = true;
                        lblDateC12.Visible = true;
                        ucDateC12.Visible = true;
                        lblUnpumpableYNC12.Visible = true;
                        chkUnpumpableC12.Visible = true;
                        lblpostponealertC12.Visible = true;
                        chkpostponealertC12.Visible = true;
                        lblpostponealertremarksC12.Visible = true;
                        txtpostponeexpremarksC12.Visible = true;
                       
                        hdnConfiguratoinIDC12.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC12.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC12.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC12.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC12.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC12.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC12.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC12.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC12.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC12.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC12.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC12.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC12.SelectedProduct = "Dummy";
                            lblSpcGravityC12.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC12.Enabled = false;
                            ucROBCUMC12.Enabled = true;
                            ucROBMTC12.CssClass = "readonlytextbox";
                            ucROBCUMC12.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC12.Enabled = true;
                            ucROBCUMC12.Enabled = false;
                            ucROBMTC12.CssClass = "input";
                            ucROBCUMC12.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC12.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC12.Checked == true)
                            ucDateC12.Enabled = true;
                        ucDateC12.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC12.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC12.Visible = true;
                            ucROBCharterUnitC12.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC12.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC12.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC12.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC12.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC12.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "12" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI12.Disabled = false;
                        lblTankNoSI12I.Visible = true;
                        lblTankNoSI12.Visible = true;
                        lbl100VolSI12I.Visible = true;
                        lbl100VolSI12.Visible = true;
                        lbl85VolSI12I.Visible = true;
                        lbl85VolSI12.Visible = true;
                        lblProductSI12I.Visible = true;
                        lblProductSI12.Visible = true;
                        lblSpcGravitySI12I.Visible = true;
                        lblSpcGravitySI12.Visible = true;
                        lblDateLoadedSI12.Visible = true;
                        ucDateLoadedSI12.Visible = true;
                        lblROBMTSI12.Visible = true;
                        ucROBMTSI12.Visible = true;
                        lblROBCUMSI12.Visible = true;
                        ucROBCUMSI12.Visible = true;
                        lblTankCleanedSI12.Visible = true;
                        chkTankCleanedSI12.Visible = true;
                        lblDateSI12.Visible = true;
                        ucDateSI12.Visible = true;
                        lblUnpumpableYNSI12.Visible = true;
                        chkUnpumpableSI12.Visible = true;
                        lblpostponealertSI12.Visible = true;
                        chkpostponealertSI12.Visible = true;
                        lblpostponealertremarksSI12.Visible = true;
                        txtpostponeexpremarksSI12.Visible = true;                        
                       
                        hdnConfiguratoinIDSI12.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI12.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI12.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI12.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI12.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI12.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI12.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI12.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI12.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI12.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI12.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI12.Text = dr["FLDROBCUM"].ToString();
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI12.Enabled = false;
                            ucROBCUMSI12.Enabled = true;
                            ucROBMTSI12.CssClass = "readonlytextbox";
                            ucROBCUMSI12.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI12.Enabled = true;
                            ucROBCUMSI12.Enabled = false;
                            ucROBMTSI12.CssClass = "input";
                            ucROBCUMSI12.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI12.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI12.Checked == true)
                            ucDateSI12.Enabled = true;
                        ucDateSI12.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI12.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI12.Visible = true;
                            ucROBCharterUnitSI12.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI12.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI12.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI12.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI12.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI12.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "12" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS12.Disabled = false;
                        lblTankNoS12I.Visible = true;
                        lblTankNoS12.Visible = true;
                        lbl100VolS12I.Visible = true;
                        lbl100VolS12.Visible = true;
                        lbl85VolS12I.Visible = true;
                        lbl85VolS12.Visible = true;
                        lblProductS12I.Visible = true;
                        lblProductS12.Visible = true;
                        lblSpcGravityS12I.Visible = true;
                        lblSpcGravityS12.Visible = true;
                        lblDateLoadedS12.Visible = true;
                        ucDateLoadedS12.Visible = true;
                        lblROBMTS12.Visible = true;
                        ucROBMTS12.Visible = true;
                        lblROBCUMS12.Visible = true;
                        ucROBCUMS12.Visible = true;
                        lblTankCleanedS12.Visible = true;
                        chkTankCleanedS12.Visible = true;
                        lblDateS12.Visible = true;
                        ucDateS12.Visible = true;
                        lblUnpumpableYNS12.Visible = true;
                        chkUnpumpableS12.Visible = true;
                        lblpostponealertS12.Visible = true;
                        chkpostponealertS12.Visible = true;
                        lblpostponealertremarksS12.Visible = true;
                        txtpostponeexpremarksS12.Visible = true;                        

                        hdnConfiguratoinIDS12.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS12.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS12.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS12.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS12.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS12.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS12.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS12.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS12.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS12.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS12.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS12.Text = dr["FLDROBCUM"].ToString();
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS12.Enabled = false;
                            ucROBCUMS12.Enabled = true;
                            ucROBMTS12.CssClass = "readonlytextbox";
                            ucROBCUMS12.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS12.Enabled = true;
                            ucROBCUMS12.Enabled = false;
                            ucROBMTS12.CssClass = "input";
                            ucROBCUMS12.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS12.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS12.Checked == true)
                            ucDateS12.Enabled = true;
                        ucDateS12.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS12.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS12.Visible = true;
                            ucROBCharterUnitS12.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS12.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS12.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();                       
                        chkpostponealertS12.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS12.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "13" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP13.Disabled = false;
                        lblTankNoP13I.Visible = true;
                        lblTankNoP13.Visible = true;
                        lbl100VolP13I.Visible = true;
                        lbl100VolP13.Visible = true;
                        lbl85VolP13I.Visible = true;
                        lbl85VolP13.Visible = true;
                        lblProductP13I.Visible = true;
                        lblProductP13.Visible = true;
                        lblSpcGravityP13I.Visible = true;
                        lblSpcGravityP13.Visible = true;
                        lblDateLoadedP13.Visible = true;
                        ucDateLoadedP13.Visible = true;
                        lblROBMTP13.Visible = true;
                        ucROBMTP13.Visible = true;
                        lblROBCUMP13.Visible = true;
                        ucROBCUMP13.Visible = true;
                        lblTankCleanedP13.Visible = true;
                        chkTankCleanedP13.Visible = true;
                        lblDateP13.Visible = true;
                        ucDateP13.Visible = true;
                        lblUnpumpableYNP13.Visible = true;
                        chkUnpumpableP13.Visible = true;
                        lblpostponealertP13.Visible = true;
                        chkpostponealertP13.Visible = true;
                        lblpostponealertremarksP13.Visible = true;
                        txtpostponeexpremarksP13.Visible = true;                        

                        hdnConfiguratoinIDP13.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP13.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP13.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP13.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP13.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP13.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP13.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP13.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP13.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP13.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP13.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP13.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP13.SelectedProduct = "Dummy";
                            lblSpcGravityP13.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP13.Enabled = false;
                            ucROBCUMP13.Enabled = true;
                            ucROBMTP13.CssClass = "readonlytextbox";
                            ucROBCUMP13.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP13.Enabled = true;
                            ucROBCUMP13.Enabled = false;
                            ucROBMTP13.CssClass = "input";
                            ucROBCUMP13.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP13.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP13.Checked == true)
                            ucDateP13.Enabled = true;
                        ucDateP13.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP13.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP13.Visible = true;
                            ucROBCharterUnitP13.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP13.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP13.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP13.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP13.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP13.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "13" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI13.Disabled = false;
                        lblTankNoPI13I.Visible = true;
                        lblTankNoPI13.Visible = true;
                        lbl100VolPI13I.Visible = true;
                        lbl100VolPI13.Visible = true;
                        lbl85VolPI13I.Visible = true;
                        lbl85VolPI13.Visible = true;
                        lblProductPI13I.Visible = true;
                        lblProductPI13.Visible = true;
                        lblSpcGravityPI13I.Visible = true;
                        lblSpcGravityPI13.Visible = true;
                        lblDateLoadedPI13.Visible = true;
                        ucDateLoadedPI13.Visible = true;
                        lblROBMTPI13.Visible = true;
                        ucROBMTPI13.Visible = true;
                        lblROBCUMPI13.Visible = true;
                        ucROBCUMPI13.Visible = true;
                        lblTankCleanedPI13.Visible = true;
                        chkTankCleanedPI13.Visible = true;
                        lblDatePI13.Visible = true;
                        ucDatePI13.Visible = true;
                        lblUnpumpableYNPI13.Visible = true;
                        chkUnpumpablePI13.Visible = true;
                        lblpostponealertPI13.Visible = true;
                        chkpostponealertPI13.Visible = true;
                        lblpostponealertremarksPI13.Visible = true;
                        txtpostponeexpremarksPI13.Visible = true;

                        hdnConfiguratoinIDPI13.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI13.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI13.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI13.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI13.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI13.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI13.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI13.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI13.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI13.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI13.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI13.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI13.SelectedProduct = "Dummy";
                            lblSpcGravityPI13.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI13.Enabled = false;
                            ucROBCUMPI13.Enabled = true;
                            ucROBMTPI13.CssClass = "readonlytextbox";
                            ucROBCUMPI13.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI13.Enabled = true;
                            ucROBCUMPI13.Enabled = false;
                            ucROBMTPI13.CssClass = "input";
                            ucROBCUMPI13.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI13.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI13.Checked == true)
                            ucDatePI13.Enabled = true;
                        ucDatePI13.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI13.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI13.Visible = true;
                            ucROBCharterUnitPI13.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI13.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI13.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI13.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI13.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI13.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "13" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC13.Disabled = false;
                        lblTankNoC13I.Visible = true;
                        lblTankNoC13.Visible = true;
                        lbl100VolC13I.Visible = true;
                        lbl100VolC13.Visible = true;
                        lbl85VolC13I.Visible = true;
                        lbl85VolC13.Visible = true;
                        lblProductC13I.Visible = true;
                        lblProductC13.Visible = true;
                        lblSpcGravityC13I.Visible = true;
                        lblSpcGravityC13.Visible = true;
                        lblDateLoadedC13.Visible = true;
                        ucDateLoadedC13.Visible = true;
                        lblROBMTC13.Visible = true;
                        ucROBMTC13.Visible = true;
                        lblROBCUMC13.Visible = true;
                        ucROBCUMC13.Visible = true;
                        lblTankCleanedC13.Visible = true;
                        chkTankCleanedC13.Visible = true;
                        lblDateC13.Visible = true;
                        ucDateC13.Visible = true;
                        lblUnpumpableYNC13.Visible = true;
                        chkUnpumpableC13.Visible = true;
                        lblpostponealertC13.Visible = true;
                        chkpostponealertC13.Visible = true;
                        lblpostponealertremarksC13.Visible = true;
                        txtpostponeexpremarksC13.Visible = true;
                        
                        hdnConfiguratoinIDC13.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC13.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC13.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC13.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC13.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC13.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC13.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC13.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC13.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC13.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC13.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC13.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC13.SelectedProduct = "Dummy";
                            lblSpcGravityC13.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC13.Enabled = false;
                            ucROBCUMC13.Enabled = true;
                            ucROBMTC13.CssClass = "readonlytextbox";
                            ucROBCUMC13.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC13.Enabled = true;
                            ucROBCUMC13.Enabled = false;
                            ucROBMTC13.CssClass = "input";
                            ucROBCUMC13.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC13.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC13.Checked == true)
                            ucDateC13.Enabled = true;
                        ucDateC13.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC13.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC13.Visible = true;
                            ucROBCharterUnitC13.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC13.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC13.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC13.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC13.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC13.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "13" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI13.Disabled = false;
                        lblTankNoSI13I.Visible = true;
                        lblTankNoSI13.Visible = true;
                        lbl100VolSI13I.Visible = true;
                        lbl100VolSI13.Visible = true;
                        lbl85VolSI13I.Visible = true;
                        lbl85VolSI13.Visible = true;
                        lblProductSI13I.Visible = true;
                        lblProductSI13.Visible = true;
                        lblSpcGravitySI13I.Visible = true;
                        lblSpcGravitySI13.Visible = true;
                        lblDateLoadedSI13.Visible = true;
                        ucDateLoadedSI13.Visible = true;
                        lblROBMTSI13.Visible = true;
                        ucROBMTSI13.Visible = true;
                        lblROBCUMSI13.Visible = true;
                        ucROBCUMSI13.Visible = true;
                        lblTankCleanedSI13.Visible = true;
                        chkTankCleanedSI13.Visible = true;
                        lblDateSI13.Visible = true;
                        ucDateSI13.Visible = true;
                        lblUnpumpableYNSI13.Visible = true;
                        chkUnpumpableSI13.Visible = true;
                        lblpostponealertSI13.Visible = true;
                        chkpostponealertSI13.Visible = true;
                        lblpostponealertremarksSI13.Visible = true;
                        txtpostponeexpremarksSI13.Visible = true;
                        
                        
                        hdnConfiguratoinIDSI13.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI13.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI13.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI13.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI13.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI13.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI13.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI13.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI13.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI13.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI13.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI13.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI13.SelectedProduct = "Dummy";
                            lblSpcGravitySI13.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI13.Enabled = false;
                            ucROBCUMSI13.Enabled = true;
                            ucROBMTSI13.CssClass = "readonlytextbox";
                            ucROBCUMSI13.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI13.Enabled = true;
                            ucROBCUMSI13.Enabled = false;
                            ucROBMTSI13.CssClass = "input";
                            ucROBCUMSI13.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI13.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI13.Checked == true)
                            ucDateSI13.Enabled = true;
                        ucDateSI13.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI13.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI13.Visible = true;
                            ucROBCharterUnitSI13.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI13.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI13.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI13.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI13.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI13.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "13" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS13.Disabled = false;
                        lblTankNoS13I.Visible = true;
                        lblTankNoS13.Visible = true;
                        lbl100VolS13I.Visible = true;
                        lbl100VolS13.Visible = true;
                        lbl85VolS13I.Visible = true;
                        lbl85VolS13.Visible = true;
                        lblProductS13I.Visible = true;
                        lblProductS13.Visible = true;
                        lblSpcGravityS13I.Visible = true;
                        lblSpcGravityS13.Visible = true;
                        lblDateLoadedS13.Visible = true;
                        ucDateLoadedS13.Visible = true;
                        lblROBMTS13.Visible = true;
                        ucROBMTS13.Visible = true;
                        lblROBCUMS13.Visible = true;
                        ucROBCUMS13.Visible = true;
                        lblTankCleanedS13.Visible = true;
                        chkTankCleanedS13.Visible = true;
                        lblDateS13.Visible = true;
                        ucDateS13.Visible = true;
                        lblUnpumpableYNS13.Visible = true;
                        chkUnpumpableS13.Visible = true;
                        lblpostponealertS13.Visible = true;
                        chkpostponealertS13.Visible = true;
                        lblpostponealertremarksS13.Visible = true;
                        txtpostponeexpremarksS13.Visible = true;
                        

                        hdnConfiguratoinIDS13.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS13.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS13.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS13.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS13.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS13.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS13.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS13.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS13.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS13.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS13.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS13.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS13.SelectedProduct = "Dummy";
                            lblSpcGravityS13.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS13.Enabled = false;
                            ucROBCUMS13.Enabled = true;
                            ucROBMTS13.CssClass = "readonlytextbox";
                            ucROBCUMS13.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS13.Enabled = true;
                            ucROBCUMS13.Enabled = false;
                            ucROBMTS13.CssClass = "input";
                            ucROBCUMS13.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS13.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS13.Checked == true)
                            ucDateS13.Enabled = true;
                        ucDateS13.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS13.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS13.Visible = true;
                            ucROBCharterUnitS13.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS13.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS13.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS13.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS13.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS13.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "14" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP14.Disabled = false;
                        lblTankNoP14I.Visible = true;
                        lblTankNoP14.Visible = true;
                        lbl100VolP14I.Visible = true;
                        lbl100VolP14.Visible = true;
                        lbl85VolP14I.Visible = true;
                        lbl85VolP14.Visible = true;
                        lblProductP14I.Visible = true;
                        lblProductP14.Visible = true;
                        lblSpcGravityP14I.Visible = true;
                        lblSpcGravityP14.Visible = true;
                        lblDateLoadedP14.Visible = true;
                        ucDateLoadedP14.Visible = true;
                        lblROBMTP14.Visible = true;
                        ucROBMTP14.Visible = true;
                        lblROBCUMP14.Visible = true;
                        ucROBCUMP14.Visible = true;
                        lblTankCleanedP14.Visible = true;
                        chkTankCleanedP14.Visible = true;
                        lblDateP14.Visible = true;
                        ucDateP14.Visible = true;
                        lblUnpumpableYNP14.Visible = true;
                        chkUnpumpableP14.Visible = true;
                        lblpostponealertP14.Visible = true;
                        chkpostponealertP14.Visible = true;
                        lblpostponealertremarksP14.Visible = true;
                        txtpostponeexpremarksP14.Visible = true;                        
                        
                        hdnConfiguratoinIDP14.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP14.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP14.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP14.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP14.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP14.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP14.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP14.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP14.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP14.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP14.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP14.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP14.SelectedProduct = "Dummy";
                            lblSpcGravityP14.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP14.Enabled = false;
                            ucROBCUMP14.Enabled = true;
                            ucROBMTP14.CssClass = "readonlytextbox";
                            ucROBCUMP14.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP14.Enabled = true;
                            ucROBCUMP14.Enabled = false;
                            ucROBMTP14.CssClass = "input";
                            ucROBCUMP14.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP14.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP14.Checked == true)
                            ucDateP14.Enabled = true;
                        ucDateP14.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP14.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP14.Visible = true;
                            ucROBCharterUnitP14.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP14.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP14.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP14.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP14.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP14.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "14" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI14.Disabled = false;
                        lblTankNoPI14I.Visible = true;
                        lblTankNoPI14.Visible = true;
                        lbl100VolPI14I.Visible = true;
                        lbl100VolPI14.Visible = true;
                        lbl85VolPI14I.Visible = true;
                        lbl85VolPI14.Visible = true;
                        lblProductPI14I.Visible = true;
                        lblProductPI14.Visible = true;
                        lblSpcGravityPI14I.Visible = true;
                        lblSpcGravityPI14.Visible = true;
                        lblDateLoadedPI14.Visible = true;
                        ucDateLoadedPI14.Visible = true;
                        lblROBMTPI14.Visible = true;
                        ucROBMTPI14.Visible = true;
                        lblROBCUMPI14.Visible = true;
                        ucROBCUMPI14.Visible = true;
                        lblTankCleanedPI14.Visible = true;
                        chkTankCleanedPI14.Visible = true;
                        lblDatePI14.Visible = true;
                        ucDatePI14.Visible = true;
                        lblUnpumpableYNPI14.Visible = true;
                        chkUnpumpablePI14.Visible = true;
                        lblpostponealertPI14.Visible = true;
                        chkpostponealertPI14.Visible = true;
                        lblpostponealertremarksPI14.Visible = true;
                        txtpostponeexpremarksPI14.Visible = true;
                        

                        hdnConfiguratoinIDPI14.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI14.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI14.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI14.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI14.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI14.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI14.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI14.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI14.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI14.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI14.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI14.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI14.SelectedProduct = "Dummy";
                            lblSpcGravityPI14.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI14.Enabled = false;
                            ucROBCUMPI14.Enabled = true;
                            ucROBMTPI14.CssClass = "readonlytextbox";
                            ucROBCUMPI14.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI14.Enabled = true;
                            ucROBCUMPI14.Enabled = false;
                            ucROBMTPI14.CssClass = "input";
                            ucROBCUMPI14.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI14.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI14.Checked == true)
                            ucDatePI14.Enabled = true;
                        ucDatePI14.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI14.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI14.Visible = true;
                            ucROBCharterUnitPI14.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI14.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI14.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI14.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI14.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI14.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "14" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC14.Disabled = false;
                        lblTankNoC14I.Visible = true;
                        lblTankNoC14.Visible = true;
                        lbl100VolC14I.Visible = true;
                        lbl100VolC14.Visible = true;
                        lbl85VolC14I.Visible = true;
                        lbl85VolC14.Visible = true;
                        lblProductC14I.Visible = true;
                        lblProductC14.Visible = true;
                        lblSpcGravityC14I.Visible = true;
                        lblSpcGravityC14.Visible = true;
                        lblDateLoadedC14.Visible = true;
                        ucDateLoadedC14.Visible = true;
                        lblROBMTC14.Visible = true;
                        ucROBMTC14.Visible = true;
                        lblROBCUMC14.Visible = true;
                        ucROBCUMC14.Visible = true;
                        lblTankCleanedC14.Visible = true;
                        chkTankCleanedC14.Visible = true;
                        lblDateC14.Visible = true;
                        ucDateC14.Visible = true;
                        lblUnpumpableYNC14.Visible = true;
                        chkUnpumpableC14.Visible = true;
                        lblpostponealertC14.Visible = true;
                        chkpostponealertC14.Visible = true;
                        lblpostponealertremarksC14.Visible = true;
                        txtpostponeexpremarksC14.Visible = true;
                        

                        hdnConfiguratoinIDC14.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC14.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC14.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC14.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC14.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC14.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC14.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC14.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC14.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC14.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC14.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC14.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC14.SelectedProduct = "Dummy";
                            lblSpcGravityC14.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC14.Enabled = false;
                            ucROBCUMC14.Enabled = true;
                            ucROBMTC14.CssClass = "readonlytextbox";
                            ucROBCUMC14.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC14.Enabled = true;
                            ucROBCUMC14.Enabled = false;
                            ucROBMTC14.CssClass = "input";
                            ucROBCUMC14.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC14.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC14.Checked == true)
                            ucDateC14.Enabled = true;
                        ucDateC14.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC14.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC14.Visible = true;
                            ucROBCharterUnitC14.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC14.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC14.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC14.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC14.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC14.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "14" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI14.Disabled = false;
                        lblTankNoSI14I.Visible = true;
                        lblTankNoSI14.Visible = true;
                        lbl100VolSI14I.Visible = true;
                        lbl100VolSI14.Visible = true;
                        lbl85VolSI14I.Visible = true;
                        lbl85VolSI14.Visible = true;
                        lblProductSI14I.Visible = true;
                        lblProductSI14.Visible = true;
                        lblSpcGravitySI14I.Visible = true;
                        lblSpcGravitySI14.Visible = true;
                        lblDateLoadedSI14.Visible = true;
                        ucDateLoadedSI14.Visible = true;
                        lblROBMTSI14.Visible = true;
                        ucROBMTSI14.Visible = true;
                        lblROBCUMSI14.Visible = true;
                        ucROBCUMSI14.Visible = true;
                        lblTankCleanedSI14.Visible = true;
                        chkTankCleanedSI14.Visible = true;
                        lblDateSI14.Visible = true;
                        ucDateSI14.Visible = true;
                        lblUnpumpableYNSI14.Visible = true;
                        chkUnpumpableSI14.Visible = true;
                        lblpostponealertSI14.Visible = true;
                        chkpostponealertSI14.Visible = true;
                        lblpostponealertremarksSI14.Visible = true;
                        txtpostponeexpremarksSI14.Visible = true;
                        

                        hdnConfiguratoinIDSI14.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI14.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI14.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI14.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI14.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI14.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI14.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI14.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI14.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI14.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI14.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI14.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI14.SelectedProduct = "Dummy";
                            lblSpcGravitySI14.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI14.Enabled = false;
                            ucROBCUMSI14.Enabled = true;
                            ucROBMTSI14.CssClass = "readonlytextbox";
                            ucROBCUMSI14.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI14.Enabled = true;
                            ucROBCUMSI14.Enabled = false;
                            ucROBMTSI14.CssClass = "input";
                            ucROBCUMSI14.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI14.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI14.Checked == true)
                            ucDateSI14.Enabled = true;
                        ucDateSI14.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI14.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI14.Visible = true;
                            ucROBCharterUnitSI14.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI14.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI14.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI14.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI14.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI14.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "14" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS14.Disabled = false;
                        lblTankNoS14I.Visible = true;
                        lblTankNoS14.Visible = true;
                        lbl100VolS14I.Visible = true;
                        lbl100VolS14.Visible = true;
                        lbl85VolS14I.Visible = true;
                        lbl85VolS14.Visible = true;
                        lblProductS14I.Visible = true;
                        lblProductS14.Visible = true;
                        lblSpcGravityS14I.Visible = true;
                        lblSpcGravityS14.Visible = true;
                        lblDateLoadedS14.Visible = true;
                        ucDateLoadedS14.Visible = true;
                        lblROBMTS14.Visible = true;
                        ucROBMTS14.Visible = true;
                        lblROBCUMS14.Visible = true;
                        ucROBCUMS14.Visible = true;
                        lblTankCleanedS14.Visible = true;
                        chkTankCleanedS14.Visible = true;
                        lblDateS14.Visible = true;
                        ucDateS14.Visible = true;
                        lblUnpumpableYNS14.Visible = true;
                        chkUnpumpableS14.Visible = true;
                        lblpostponealertS14.Visible = true;
                        chkpostponealertS14.Visible = true;
                        lblpostponealertremarksS14.Visible = true;
                        txtpostponeexpremarksS14.Visible = true;
                        

                        hdnConfiguratoinIDS14.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS14.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS14.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS14.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS14.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS14.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP14.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblProductP14.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblSpcGravityS14.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS14.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS14.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS14.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS14.SelectedProduct = "Dummy";
                            lblSpcGravityS14.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS14.Enabled = false;
                            ucROBCUMS14.Enabled = true;
                            ucROBMTS14.CssClass = "readonlytextbox";
                            ucROBCUMS14.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS14.Enabled = true;
                            ucROBCUMS14.Enabled = false;
                            ucROBMTS14.CssClass = "input";
                            ucROBCUMS14.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS14.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS14.Checked == true)
                            ucDateS14.Enabled = true;
                        ucDateS11.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS14.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS14.Visible = true;
                            ucROBCharterUnitS14.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS14.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS14.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS14.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS14.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "15" && dr["FLDHORIZONTALVALUE"].ToString() == "1")
                    {
                        spnP15.Disabled = false;
                        lblTankNoP15I.Visible = true;
                        lblTankNoP15.Visible = true;
                        lbl100VolP15I.Visible = true;
                        lbl100VolP15.Visible = true;
                        lbl85VolP15I.Visible = true;
                        lbl85VolP15.Visible = true;
                        lblProductP15I.Visible = true;
                        lblProductP15.Visible = true;
                        lblSpcGravityP15I.Visible = true;
                        lblSpcGravityP15.Visible = true;
                        lblDateLoadedP15.Visible = true;
                        ucDateLoadedP15.Visible = true;
                        lblROBMTP15.Visible = true;
                        ucROBMTP15.Visible = true;
                        lblROBCUMP15.Visible = true;
                        ucROBCUMP15.Visible = true;
                        lblTankCleanedP15.Visible = true;
                        chkTankCleanedP15.Visible = true;
                        lblDateP15.Visible = true;
                        ucDateP15.Visible = true;
                        lblUnpumpableYNP15.Visible = true;
                        chkUnpumpableP15.Visible = true;
                        lblpostponealertP15.Visible = true;
                        chkpostponealertP15.Visible = true;
                        lblpostponealertremarksP15.Visible = true;
                        txtpostponeexpremarksP15.Visible = true;
                        

                        hdnConfiguratoinIDP15.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionP15.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDP15.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoP15.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolP15.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolP15.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductP15.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductP15.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityP15.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedP15.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTP15.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMP15.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductP15.SelectedProduct = "Dummy";
                            lblSpcGravityP15.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTP15.Enabled = false;
                            ucROBCUMP15.Enabled = true;
                            ucROBMTP15.CssClass = "readonlytextbox";
                            ucROBCUMP15.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTP15.Enabled = true;
                            ucROBCUMP15.Enabled = false;
                            ucROBMTP15.CssClass = "input";
                            ucROBCUMP15.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedP15.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedP15.Checked == true)
                            ucDateP15.Enabled = true;
                        ucDateP15.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitP15.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitP15.Visible = true;
                            ucROBCharterUnitP15.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableP15.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksP15.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertP15.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertP15.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductP15.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "15" && dr["FLDHORIZONTALVALUE"].ToString() == "2")
                    {
                        spnPI15.Disabled = false;
                        lblTankNoPI15I.Visible = true;
                        lblTankNoPI15.Visible = true;
                        lbl100VolPI15I.Visible = true;
                        lbl100VolPI15.Visible = true;
                        lbl85VolPI15I.Visible = true;
                        lbl85VolPI15.Visible = true;
                        lblProductPI15I.Visible = true;
                        lblProductPI15.Visible = true;
                        lblSpcGravityPI15I.Visible = true;
                        lblSpcGravityPI15.Visible = true;
                        lblDateLoadedPI15.Visible = true;
                        ucDateLoadedPI15.Visible = true;
                        lblROBMTPI15.Visible = true;
                        ucROBMTPI15.Visible = true;
                        lblROBCUMPI15.Visible = true;
                        ucROBCUMPI15.Visible = true;
                        lblTankCleanedPI15.Visible = true;
                        chkTankCleanedPI15.Visible = true;
                        lblDatePI15.Visible = true;
                        ucDatePI15.Visible = true;
                        lblUnpumpableYNPI15.Visible = true;
                        chkUnpumpablePI15.Visible = true;
                        lblpostponealertPI15.Visible = true;
                        chkpostponealertPI15.Visible = true;
                        lblpostponealertremarksPI15.Visible = true;
                        txtpostponeexpremarksPI15.Visible = true;
                        

                        hdnConfiguratoinIDPI15.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionPI15.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDPI15.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoPI15.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolPI15.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolPI15.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductPI15.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductPI15.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityPI15.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedPI15.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTPI15.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMPI15.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductPI15.SelectedProduct = "Dummy";
                            lblSpcGravityPI15.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTPI15.Enabled = false;
                            ucROBCUMPI15.Enabled = true;
                            ucROBMTPI15.CssClass = "readonlytextbox";
                            ucROBCUMPI15.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTPI15.Enabled = true;
                            ucROBCUMPI15.Enabled = false;
                            ucROBMTPI15.CssClass = "input";
                            ucROBCUMPI15.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedPI15.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedPI15.Checked == true)
                            ucDatePI15.Enabled = true;
                        ucDatePI15.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitPI15.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitPI15.Visible = true;
                            ucROBCharterUnitPI15.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpablePI15.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksPI15.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertPI15.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertPI15.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductPI15.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());

                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "15" && dr["FLDHORIZONTALVALUE"].ToString() == "3")
                    {
                        spnC15.Disabled = false;
                        lblTankNoC15I.Visible = true;
                        lblTankNoC15.Visible = true;
                        lbl100VolC15I.Visible = true;
                        lbl100VolC15.Visible = true;
                        lbl85VolC15I.Visible = true;
                        lbl85VolC15.Visible = true;
                        lblProductC15I.Visible = true;
                        lblProductC15.Visible = true;
                        lblSpcGravityC15I.Visible = true;
                        lblSpcGravityC15.Visible = true;
                        lblDateLoadedC15.Visible = true;
                        ucDateLoadedC15.Visible = true;
                        lblROBMTC15.Visible = true;
                        ucROBMTC15.Visible = true;
                        lblROBCUMC15.Visible = true;
                        ucROBCUMC15.Visible = true;
                        lblTankCleanedC15.Visible = true;
                        chkTankCleanedC15.Visible = true;
                        lblDateC15.Visible = true;
                        ucDateC15.Visible = true;
                        lblUnpumpableYNC15.Visible = true;
                        chkUnpumpableC15.Visible = true;
                        lblpostponealertC15.Visible = true;
                        chkpostponealertC15.Visible = true;
                        lblpostponealertremarksC15.Visible = true;
                        txtpostponeexpremarksC15.Visible = true;

                        hdnConfiguratoinIDC15.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionC15.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDC15.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoC15.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolC15.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolC15.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductC15.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductC15.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityC15.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedC15.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTC15.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMC15.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductC15.SelectedProduct = "Dummy";
                            lblSpcGravityC15.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTC15.Enabled = false;
                            ucROBCUMC15.Enabled = true;
                            ucROBMTC15.CssClass = "readonlytextbox";
                            ucROBCUMC15.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTC15.Enabled = true;
                            ucROBCUMC15.Enabled = false;
                            ucROBMTC15.CssClass = "input";
                            ucROBCUMC15.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedC15.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedC15.Checked == true)
                            ucDateC15.Enabled = true;
                        ucDateC15.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitC15.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitC15.Visible = true;
                            ucROBCharterUnitC15.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableC15.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksC15.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertC15.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertC15.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductC15.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "15" && dr["FLDHORIZONTALVALUE"].ToString() == "4")
                    {
                        spnSI15.Disabled = false;
                        lblTankNoSI15I.Visible = true;
                        lblTankNoSI15.Visible = true;
                        lbl100VolSI15I.Visible = true;
                        lbl100VolSI15.Visible = true;
                        lbl85VolSI15I.Visible = true;
                        lbl85VolSI15.Visible = true;
                        lblProductSI15I.Visible = true;
                        lblProductSI15.Visible = true;
                        lblSpcGravitySI15I.Visible = true;
                        lblSpcGravitySI15.Visible = true;
                        lblDateLoadedSI15.Visible = true;
                        ucDateLoadedSI15.Visible = true;
                        lblROBMTSI15.Visible = true;
                        ucROBMTSI15.Visible = true;
                        lblROBCUMSI15.Visible = true;
                        ucROBCUMSI15.Visible = true;
                        lblTankCleanedSI15.Visible = true;
                        chkTankCleanedSI15.Visible = true;
                        lblDateSI15.Visible = true;
                        ucDateSI15.Visible = true;
                        lblUnpumpableYNSI15.Visible = true;
                        chkUnpumpableSI15.Visible = true;
                        lblpostponealertSI15.Visible = true;
                        chkpostponealertSI15.Visible = true;
                        lblpostponealertremarksSI15.Visible = true;
                        txtpostponeexpremarksSI15.Visible = true;
                        
                        
                        hdnConfiguratoinIDSI15.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionSI15.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDSI15.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoSI15.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolSI15.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolSI15.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductSI15.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductSI15.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravitySI15.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedSI15.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTSI15.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMSI15.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductSI15.SelectedProduct = "Dummy";
                            lblSpcGravitySI15.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTSI15.Enabled = false;
                            ucROBCUMSI15.Enabled = true;
                            ucROBMTSI15.CssClass = "readonlytextbox";
                            ucROBCUMSI15.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTSI15.Enabled = true;
                            ucROBCUMSI15.Enabled = false;
                            ucROBMTSI15.CssClass = "input";
                            ucROBCUMSI15.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedSI15.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedSI15.Checked == true)
                            ucDateSI15.Enabled = true;
                        ucDateSI15.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitSI15.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitSI15.Visible = true;
                            ucROBCharterUnitSI15.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableSI15.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksSI15.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertSI15.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertSI15.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductSI15.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }
                    else if (dr["FLDVERTICALVALUE"].ToString() == "15" && dr["FLDHORIZONTALVALUE"].ToString() == "5")
                    {
                        spnS15.Disabled = false;
                        lblTankNoS15I.Visible = true;
                        lblTankNoS15.Visible = true;
                        lbl100VolS15I.Visible = true;
                        lbl100VolS15.Visible = true;
                        lbl85VolS15I.Visible = true;
                        lbl85VolS15.Visible = true;
                        lblProductS15I.Visible = true;
                        lblProductS15.Visible = true;
                        lblSpcGravityS15I.Visible = true;
                        lblSpcGravityS15.Visible = true;
                        lblDateLoadedS15.Visible = true;
                        ucDateLoadedS15.Visible = true;
                        lblROBMTS15.Visible = true;
                        ucROBMTS15.Visible = true;
                        lblROBCUMS15.Visible = true;
                        ucROBCUMS15.Visible = true;
                        lblTankCleanedS15.Visible = true;
                        chkTankCleanedS15.Visible = true;
                        lblDateS15.Visible = true;
                        ucDateS15.Visible = true;
                        lblUnpumpableYNS15.Visible = true;
                        chkUnpumpableS15.Visible = true;
                        lblpostponealertS15.Visible = true;
                        chkpostponealertS15.Visible = true;
                        lblpostponealertremarksS15.Visible = true;
                        txtpostponeexpremarksS15.Visible = true;                        

                        hdnConfiguratoinIDS15.Value = dr["FLDTANKPLANCONFIGURATIONID"].ToString();
                        hdnConsumptionS15.Value = dr["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
                        hdnMidnightReportIDS15.Value = dr["FLDMIDNIGHTREPORTID"].ToString();
                        lblTankNoS15.Text = dr["FLDLOCATIONNAME"].ToString();
                        lbl100VolS15.Text = dr["FLDCAPACITYAT100PERCENT"].ToString();
                        lbl85VolS15.Text = dr["FLDCAPACITYAT85PERCENT"].ToString();
                        lblProductS15.ShortNameFilter = dr["FLDPRODUCTTYPEID"].ToString();
                        lblProductS15.SelectedProduct = dr["FLDOILTYPECODE"].ToString();
                        lblSpcGravityS15.Text = dr["FLDSPECIFICGRAVITY"].ToString();
                        ucDateLoadedS15.Text = dr["FLDDATELOADED"].ToString();
                        ucROBMTS15.Text = dr["FLDROBMT"].ToString();
                        ucROBCUMS15.Text = dr["FLDROBCUM"].ToString();
                        if (double.Parse(dr["FLDROBMT"].ToString()) == 0 && double.Parse(dr["FLDROBCUM"].ToString()) == 0)
                        {
                            lblProductS15.SelectedProduct = "Dummy";
                            lblSpcGravityS15.Text = "";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB" || dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "MNL")
                        {
                            ucROBMTS15.Enabled = false;
                            ucROBCUMS15.Enabled = true;
                            ucROBMTS15.CssClass = "readonlytextbox";
                            ucROBCUMS15.CssClass = "input";
                        }
                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "DB")
                        {
                            ucROBMTS15.Enabled = true;
                            ucROBCUMS15.Enabled = false;
                            ucROBMTS15.CssClass = "input";
                            ucROBCUMS15.CssClass = "readonlytextbox";
                        }
                        chkTankCleanedS15.Checked = (dr["FLDTANKCLEANED"].ToString() == "CHECKED") ? true : false;
                        if (chkTankCleanedS15.Checked == true)
                            ucDateS15.Enabled = true;
                        ucDateS15.Text = dr["FLDDATE"].ToString();

                        if (dr["FLDPRODUCTTYPESHORTCODE"].ToString() == "WB")
                        {
                            lblROBCharterUnitS15.Text = "ROB " + dr["FLDCHARTERUNIT"].ToString();
                            ucROBCharterUnitS15.Visible = true;
                            ucROBCharterUnitS15.Text = dr["FLDCHARTERROB"].ToString();
                        }
                        chkUnpumpableS15.Checked = dr["FLDUNPUMPABLEYN"].ToString() == "0" ? false : true;
                        txtpostponeexpremarksS15.Text = dr["FLDPOSTPONEALERTREMARKS"].ToString();
                        chkpostponealertS15.Checked = dr["FLDPOSTPONEALERTBY7DAYS"].ToString() == "0" ? false : true;
                        generalalertdays = 14;
                        if (chkpostponealertS15.Checked.Equals(true))
                        {
                            generalalertdays = generalalertdays + 7;
                        }
                        if (int.Parse(dr["FLDNOOFDAYSLASTLOADED"].ToString()) > generalalertdays && dr["FLDUNPUMPABLEYN"].ToString() == "0" && double.Parse(dr["FLDROBCUM"].ToString()) > 0)
                            unpumpableAlert = unpumpableAlert + (unpumpableAlert == string.Empty ? " " : " ,</br>") + dr["FLDLOCATIONNAME"].ToString();
                        //lblProductS11.BackColor = System.Drawing.Color.FromName(dr["FLDCOLOR"].ToString());
                    }

                }
                int maxvertical = 0;
                if (General.GetNullableString(ds.Tables[1].Rows[0]["FLDVERTICALVALUE"].ToString()) != null)
                    maxvertical = int.Parse(ds.Tables[1].Rows[0]["FLDVERTICALVALUE"].ToString());

                HideControls(maxvertical);

                BindDryBulkCargoSummary();
                BindLiquidBulkCargoSummary();
                BindMethanolSummary();

                if (unpumpableAlert.Length > 0)
                    lblAlert.Text = "Cargo Loaded has been on board for 14 days in Tank </br>" + unpumpableAlert +
                        " .</br>Message to be sent to Office after checking condition of the Cargo.";
             
            }

            else
            {
                
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DisableControls()
    {
        spnP1.Disabled = true;
        spnPI1.Disabled = true;
        spnC1.Disabled = true;
        spnSI1.Disabled = true;
        spnS1.Disabled = true;

        spnP2.Disabled = true;
        spnPI2.Disabled = true;
        spnC2.Disabled = true;
        spnSI2.Disabled = true;
        spnS2.Disabled = true;

        spnP3.Disabled = true;
        spnPI3.Disabled = true;
        spnC3.Disabled = true;
        spnSI3.Disabled = true;
        spnS3.Disabled = true;

        spnP4.Disabled = true;
        spnPI4.Disabled = true;
        spnC4.Disabled = true;
        spnSI4.Disabled = true;
        spnS4.Disabled = true;

        spnP5.Disabled = true;
        spnPI5.Disabled = true;
        spnC5.Disabled = true;
        spnSI5.Disabled = true;
        spnS5.Disabled = true;

        spnP6.Disabled = true;
        spnPI6.Disabled = true;
        spnC6.Disabled = true;
        spnSI6.Disabled = true;
        spnS6.Disabled = true;

        spnP7.Disabled = true;
        spnPI7.Disabled = true;
        spnC7.Disabled = true;
        spnSI7.Disabled = true;
        spnS7.Disabled = true;

        spnP8.Disabled = true;
        spnPI8.Disabled = true;
        spnC8.Disabled = true;
        spnSI8.Disabled = true;
        spnS8.Disabled = true;

        spnP9.Disabled = true;
        spnPI9.Disabled = true;
        spnC9.Disabled = true;
        spnSI9.Disabled = true;
        spnS9.Disabled = true;

        spnP10.Disabled = true;
        spnPI10.Disabled = true;
        spnC10.Disabled = true;
        spnSI10.Disabled = true;
        spnS10.Disabled = true;

        spnP11.Disabled = true;
        spnPI11.Disabled = true;
        spnC11.Disabled = true;
        spnSI11.Disabled = true;
        spnS11.Disabled = true;

        spnP12.Disabled = true;
        spnPI12.Disabled = true;
        spnC12.Disabled = true;
        spnSI12.Disabled = true;
        spnS12.Disabled = true;

        spnP13.Disabled = true;
        spnPI13.Disabled = true;
        spnC13.Disabled = true;
        spnSI13.Disabled = true;
        spnS13.Disabled = true;

        spnP14.Disabled = true;
        spnPI14.Disabled = true;
        spnC14.Disabled = true;
        spnSI14.Disabled = true;
        spnS14.Disabled = true;

        spnP15.Disabled = true;
        spnPI15.Disabled = true;
        spnC15.Disabled = true;
        spnSI15.Disabled = true;
        spnS15.Disabled = true;


        lblTankNoP1I.Visible = false;
        lblTankNoP1.Visible = false;
        lbl100VolP1I.Visible = false;
        lbl100VolP1.Visible = false;
        lbl85VolP1I.Visible = false;
        lbl85VolP1.Visible = false;
        lblProductP1I.Visible = false;
        lblProductP1.Visible = false;
        lblSpcGravityP1I.Visible = false;
        lblSpcGravityP1.Visible = false;
        lblDateLoadedP1.Visible = false;
        ucDateLoadedP1.Visible = false;
        lblROBMTP1.Visible = false;
        ucROBMTP1.Visible = false;
        lblROBCUMP1.Visible = false;
        ucROBCUMP1.Visible = false;
        lblTankCleanedP1.Visible = false;
        chkTankCleanedP1.Visible = false;
        lblDateP1.Visible = false;
        ucDateP1.Visible = false;


        lblTankNoP2I.Visible = false;
        lblTankNoP2.Visible = false;
        lbl100VolP2I.Visible = false;
        lbl100VolP2.Visible = false;
        lbl85VolP2I.Visible = false;
        lbl85VolP2.Visible = false;
        lblProductP2I.Visible = false;
        lblProductP2.Visible = false;
        lblSpcGravityP2I.Visible = false;
        lblSpcGravityP2.Visible = false;
        lblDateLoadedP2.Visible = false;
        ucDateLoadedP2.Visible = false;
        lblROBMTP2.Visible = false;
        ucROBMTP2.Visible = false;
        lblROBCUMP2.Visible = false;
        ucROBCUMP2.Visible = false;
        lblTankCleanedP2.Visible = false;
        chkTankCleanedP2.Visible = false;
        lblDateP2.Visible = false;
        ucDateP2.Visible = false;


        lblTankNoP3I.Visible = false;
        lblTankNoP3.Visible = false;
        lbl100VolP3I.Visible = false;
        lbl100VolP3.Visible = false;
        lbl85VolP3I.Visible = false;
        lbl85VolP3.Visible = false;
        lblProductP3I.Visible = false;
        lblProductP3.Visible = false;
        lblSpcGravityP3I.Visible = false;
        lblSpcGravityP3.Visible = false;
        lblDateLoadedP3.Visible = false;
        ucDateLoadedP3.Visible = false;
        lblROBMTP3.Visible = false;
        ucROBMTP3.Visible = false;
        lblROBCUMP3.Visible = false;
        ucROBCUMP3.Visible = false;
        lblTankCleanedP3.Visible = false;
        chkTankCleanedP3.Visible = false;
        lblDateP3.Visible = false;
        ucDateP3.Visible = false;


        lblTankNoP4I.Visible = false;
        lblTankNoP4.Visible = false;
        lbl100VolP4I.Visible = false;
        lbl100VolP4.Visible = false;
        lbl85VolP4I.Visible = false;
        lbl85VolP4.Visible = false;
        lblProductP4I.Visible = false;
        lblProductP4.Visible = false;
        lblSpcGravityP4I.Visible = false;
        lblSpcGravityP4.Visible = false;
        lblDateLoadedP4.Visible = false;
        ucDateLoadedP4.Visible = false;
        lblROBMTP4.Visible = false;
        ucROBMTP4.Visible = false;
        lblROBCUMP4.Visible = false;
        ucROBCUMP4.Visible = false;
        lblTankCleanedP4.Visible = false;
        chkTankCleanedP4.Visible = false;
        lblDateP4.Visible = false;
        ucDateP4.Visible = false;


        lblTankNoP5I.Visible = false;
        lblTankNoP5.Visible = false;
        lbl100VolP5I.Visible = false;
        lbl100VolP5.Visible = false;
        lbl85VolP5I.Visible = false;
        lbl85VolP5.Visible = false;
        lblProductP5I.Visible = false;
        lblProductP5.Visible = false;
        lblSpcGravityP5I.Visible = false;
        lblSpcGravityP5.Visible = false;
        lblDateLoadedP5.Visible = false;
        ucDateLoadedP5.Visible = false;
        lblROBMTP5.Visible = false;
        ucROBMTP5.Visible = false;
        lblROBCUMP5.Visible = false;
        ucROBCUMP5.Visible = false;
        lblTankCleanedP5.Visible = false;
        chkTankCleanedP5.Visible = false;
        lblDateP5.Visible = false;
        ucDateP5.Visible = false;


        lblTankNoP6I.Visible = false;
        lblTankNoP6.Visible = false;
        lbl100VolP6I.Visible = false;
        lbl100VolP6.Visible = false;
        lbl85VolP6I.Visible = false;
        lbl85VolP6.Visible = false;
        lblProductP6I.Visible = false;
        lblProductP6.Visible = false;
        lblSpcGravityP6I.Visible = false;
        lblSpcGravityP6.Visible = false;
        lblDateLoadedP6.Visible = false;
        ucDateLoadedP6.Visible = false;
        lblROBMTP6.Visible = false;
        ucROBMTP6.Visible = false;
        lblROBCUMP6.Visible = false;
        ucROBCUMP6.Visible = false;
        lblTankCleanedP6.Visible = false;
        chkTankCleanedP6.Visible = false;
        lblDateP6.Visible = false;
        ucDateP6.Visible = false;


        lblTankNoP7I.Visible = false;
        lblTankNoP7.Visible = false;
        lbl100VolP7I.Visible = false;
        lbl100VolP7.Visible = false;
        lbl85VolP7I.Visible = false;
        lbl85VolP7.Visible = false;
        lblProductP7I.Visible = false;
        lblProductP7.Visible = false;
        lblSpcGravityP7I.Visible = false;
        lblSpcGravityP7.Visible = false;
        lblDateLoadedP7.Visible = false;
        ucDateLoadedP7.Visible = false;
        lblROBMTP7.Visible = false;
        ucROBMTP7.Visible = false;
        lblROBCUMP7.Visible = false;
        ucROBCUMP7.Visible = false;
        lblTankCleanedP7.Visible = false;
        chkTankCleanedP7.Visible = false;
        lblDateP7.Visible = false;
        ucDateP7.Visible = false;


        lblTankNoP8I.Visible = false;
        lblTankNoP8.Visible = false;
        lbl100VolP8I.Visible = false;
        lbl100VolP8.Visible = false;
        lbl85VolP8I.Visible = false;
        lbl85VolP8.Visible = false;
        lblProductP8I.Visible = false;
        lblProductP8.Visible = false;
        lblSpcGravityP8I.Visible = false;
        lblSpcGravityP8.Visible = false;
        lblDateLoadedP8.Visible = false;
        ucDateLoadedP8.Visible = false;
        lblROBMTP8.Visible = false;
        ucROBMTP8.Visible = false;
        lblROBCUMP8.Visible = false;
        ucROBCUMP8.Visible = false;
        lblTankCleanedP8.Visible = false;
        chkTankCleanedP8.Visible = false;
        lblDateP8.Visible = false;
        ucDateP8.Visible = false;


        lblTankNoP9I.Visible = false;
        lblTankNoP9.Visible = false;
        lbl100VolP9I.Visible = false;
        lbl100VolP9.Visible = false;
        lbl85VolP9I.Visible = false;
        lbl85VolP9.Visible = false;
        lblProductP9I.Visible = false;
        lblProductP9.Visible = false;
        lblSpcGravityP9I.Visible = false;
        lblSpcGravityP9.Visible = false;
        lblDateLoadedP9.Visible = false;
        ucDateLoadedP9.Visible = false;
        lblROBMTP9.Visible = false;
        ucROBMTP9.Visible = false;
        lblROBCUMP9.Visible = false;
        ucROBCUMP9.Visible = false;
        lblTankCleanedP9.Visible = false;
        chkTankCleanedP9.Visible = false;
        lblDateP9.Visible = false;
        ucDateP9.Visible = false;


        lblTankNoP10I.Visible = false;
        lblTankNoP10.Visible = false;
        lbl100VolP10I.Visible = false;
        lbl100VolP10.Visible = false;
        lbl85VolP10I.Visible = false;
        lbl85VolP10.Visible = false;
        lblProductP10I.Visible = false;
        lblProductP10.Visible = false;
        lblSpcGravityP10I.Visible = false;
        lblSpcGravityP10.Visible = false;
        lblDateLoadedP10.Visible = false;
        ucDateLoadedP10.Visible = false;
        lblROBMTP10.Visible = false;
        ucROBMTP10.Visible = false;
        lblROBCUMP10.Visible = false;
        ucROBCUMP10.Visible = false;
        lblTankCleanedP10.Visible = false;
        chkTankCleanedP10.Visible = false;
        lblDateP10.Visible = false;
        ucDateP10.Visible = false;


        lblTankNoP11I.Visible = false;
        lblTankNoP11.Visible = false;
        lbl100VolP11I.Visible = false;
        lbl100VolP11.Visible = false;
        lbl85VolP11I.Visible = false;
        lbl85VolP11.Visible = false;
        lblProductP11I.Visible = false;
        lblProductP11.Visible = false;
        lblSpcGravityP11I.Visible = false;
        lblSpcGravityP11.Visible = false;
        lblDateLoadedP11.Visible = false;
        ucDateLoadedP11.Visible = false;
        lblROBMTP11.Visible = false;
        ucROBMTP11.Visible = false;
        lblROBCUMP11.Visible = false;
        ucROBCUMP11.Visible = false;
        lblTankCleanedP11.Visible = false;
        chkTankCleanedP11.Visible = false;
        lblDateP11.Visible = false;
        ucDateP11.Visible = false;


        lblTankNoP12I.Visible = false;
        lblTankNoP12.Visible = false;
        lbl100VolP12I.Visible = false;
        lbl100VolP12.Visible = false;
        lbl85VolP12I.Visible = false;
        lbl85VolP12.Visible = false;
        lblProductP12I.Visible = false;
        lblProductP12.Visible = false;
        lblSpcGravityP12I.Visible = false;
        lblSpcGravityP12.Visible = false;
        lblDateLoadedP12.Visible = false;
        ucDateLoadedP12.Visible = false;
        lblROBMTP12.Visible = false;
        ucROBMTP12.Visible = false;
        lblROBCUMP12.Visible = false;
        ucROBCUMP12.Visible = false;
        lblTankCleanedP12.Visible = false;
        chkTankCleanedP12.Visible = false;
        lblDateP12.Visible = false;
        ucDateP12.Visible = false;


        lblTankNoP13I.Visible = false;
        lblTankNoP13.Visible = false;
        lbl100VolP13I.Visible = false;
        lbl100VolP13.Visible = false;
        lbl85VolP13I.Visible = false;
        lbl85VolP13.Visible = false;
        lblProductP13I.Visible = false;
        lblProductP13.Visible = false;
        lblSpcGravityP13I.Visible = false;
        lblSpcGravityP13.Visible = false;
        lblDateLoadedP13.Visible = false;
        ucDateLoadedP13.Visible = false;
        lblROBMTP13.Visible = false;
        ucROBMTP13.Visible = false;
        lblROBCUMP13.Visible = false;
        ucROBCUMP13.Visible = false;
        lblTankCleanedP13.Visible = false;
        chkTankCleanedP13.Visible = false;
        lblDateP13.Visible = false;
        ucDateP13.Visible = false;


        lblTankNoP14I.Visible = false;
        lblTankNoP14.Visible = false;
        lbl100VolP14I.Visible = false;
        lbl100VolP14.Visible = false;
        lbl85VolP14I.Visible = false;
        lbl85VolP14.Visible = false;
        lblProductP14I.Visible = false;
        lblProductP14.Visible = false;
        lblSpcGravityP14I.Visible = false;
        lblSpcGravityP14.Visible = false;
        lblDateLoadedP14.Visible = false;
        ucDateLoadedP14.Visible = false;
        lblROBMTP14.Visible = false;
        ucROBMTP14.Visible = false;
        lblROBCUMP14.Visible = false;
        ucROBCUMP14.Visible = false;
        lblTankCleanedP14.Visible = false;
        chkTankCleanedP14.Visible = false;
        lblDateP14.Visible = false;
        ucDateP14.Visible = false;


        lblTankNoP15I.Visible = false;
        lblTankNoP15.Visible = false;
        lbl100VolP15I.Visible = false;
        lbl100VolP15.Visible = false;
        lbl85VolP15I.Visible = false;
        lbl85VolP15.Visible = false;
        lblProductP15I.Visible = false;
        lblProductP15.Visible = false;
        lblSpcGravityP15I.Visible = false;
        lblSpcGravityP15.Visible = false;
        lblDateLoadedP15.Visible = false;
        ucDateLoadedP15.Visible = false;
        lblROBMTP15.Visible = false;
        ucROBMTP15.Visible = false;
        lblROBCUMP15.Visible = false;
        ucROBCUMP15.Visible = false;
        lblTankCleanedP15.Visible = false;
        chkTankCleanedP15.Visible = false;
        lblDateP15.Visible = false;
        ucDateP15.Visible = false;



        lblTankNoPI1I.Visible = false;
        lblTankNoPI1.Visible = false;
        lbl100VolPI1I.Visible = false;
        lbl100VolPI1.Visible = false;
        lbl85VolPI1I.Visible = false;
        lbl85VolPI1.Visible = false;
        lblProductPI1I.Visible = false;
        lblProductPI1.Visible = false;
        lblSpcGravityPI1I.Visible = false;
        lblSpcGravityPI1.Visible = false;
        lblDateLoadedPI1.Visible = false;
        ucDateLoadedPI1.Visible = false;
        lblROBMTPI1.Visible = false;
        ucROBMTPI1.Visible = false;
        lblROBCUMPI1.Visible = false;
        ucROBCUMPI1.Visible = false;
        lblTankCleanedPI1.Visible = false;
        chkTankCleanedPI1.Visible = false;
        lblDatePI1.Visible = false;
        ucDatePI1.Visible = false;


        lblTankNoPI2I.Visible = false;
        lblTankNoPI2.Visible = false;
        lbl100VolPI2I.Visible = false;
        lbl100VolPI2.Visible = false;
        lbl85VolPI2I.Visible = false;
        lbl85VolPI2.Visible = false;
        lblProductPI2I.Visible = false;
        lblProductPI2.Visible = false;
        lblSpcGravityPI2I.Visible = false;
        lblSpcGravityPI2.Visible = false;
        lblDateLoadedPI2.Visible = false;
        ucDateLoadedPI2.Visible = false;
        lblROBMTPI2.Visible = false;
        ucROBMTPI2.Visible = false;
        lblROBCUMPI2.Visible = false;
        ucROBCUMPI2.Visible = false;
        lblTankCleanedPI2.Visible = false;
        chkTankCleanedPI2.Visible = false;
        lblDatePI2.Visible = false;
        ucDatePI2.Visible = false;


        lblTankNoPI3I.Visible = false;
        lblTankNoPI3.Visible = false;
        lbl100VolPI3I.Visible = false;
        lbl100VolPI3.Visible = false;
        lbl85VolPI3I.Visible = false;
        lbl85VolPI3.Visible = false;
        lblProductPI3I.Visible = false;
        lblProductPI3.Visible = false;
        lblSpcGravityPI3I.Visible = false;
        lblSpcGravityPI3.Visible = false;
        lblDateLoadedPI3.Visible = false;
        ucDateLoadedPI3.Visible = false;
        lblROBMTPI3.Visible = false;
        ucROBMTPI3.Visible = false;
        lblROBCUMPI3.Visible = false;
        ucROBCUMPI3.Visible = false;
        lblTankCleanedPI3.Visible = false;
        chkTankCleanedPI3.Visible = false;
        lblDatePI3.Visible = false;
        ucDatePI3.Visible = false;


        lblTankNoPI4I.Visible = false;
        lblTankNoPI4.Visible = false;
        lbl100VolPI4I.Visible = false;
        lbl100VolPI4.Visible = false;
        lbl85VolPI4I.Visible = false;
        lbl85VolPI4.Visible = false;
        lblProductPI4I.Visible = false;
        lblProductPI4.Visible = false;
        lblSpcGravityPI4I.Visible = false;
        lblSpcGravityPI4.Visible = false;
        lblDateLoadedPI4.Visible = false;
        ucDateLoadedPI4.Visible = false;
        lblROBMTPI4.Visible = false;
        ucROBMTPI4.Visible = false;
        lblROBCUMPI4.Visible = false;
        ucROBCUMPI4.Visible = false;
        lblTankCleanedPI4.Visible = false;
        chkTankCleanedPI4.Visible = false;
        lblDatePI4.Visible = false;
        ucDatePI4.Visible = false;


        lblTankNoPI5I.Visible = false;
        lblTankNoPI5.Visible = false;
        lbl100VolPI5I.Visible = false;
        lbl100VolPI5.Visible = false;
        lbl85VolPI5I.Visible = false;
        lbl85VolPI5.Visible = false;
        lblProductPI5I.Visible = false;
        lblProductPI5.Visible = false;
        lblSpcGravityPI5I.Visible = false;
        lblSpcGravityPI5.Visible = false;
        lblDateLoadedPI5.Visible = false;
        ucDateLoadedPI5.Visible = false;
        lblROBMTPI5.Visible = false;
        ucROBMTPI5.Visible = false;
        lblROBCUMPI5.Visible = false;
        ucROBCUMPI5.Visible = false;
        lblTankCleanedPI5.Visible = false;
        chkTankCleanedPI5.Visible = false;
        lblDatePI5.Visible = false;
        ucDatePI5.Visible = false;


        lblTankNoPI6I.Visible = false;
        lblTankNoPI6.Visible = false;
        lbl100VolPI6I.Visible = false;
        lbl100VolPI6.Visible = false;
        lbl85VolPI6I.Visible = false;
        lbl85VolPI6.Visible = false;
        lblProductPI6I.Visible = false;
        lblProductPI6.Visible = false;
        lblSpcGravityPI6I.Visible = false;
        lblSpcGravityPI6.Visible = false;
        lblDateLoadedPI6.Visible = false;
        ucDateLoadedPI6.Visible = false;
        lblROBMTPI6.Visible = false;
        ucROBMTPI6.Visible = false;
        lblROBCUMPI6.Visible = false;
        ucROBCUMPI6.Visible = false;
        lblTankCleanedPI6.Visible = false;
        chkTankCleanedPI6.Visible = false;
        lblDatePI6.Visible = false;
        ucDatePI6.Visible = false;


        lblTankNoPI7I.Visible = false;
        lblTankNoPI7.Visible = false;
        lbl100VolPI7I.Visible = false;
        lbl100VolPI7.Visible = false;
        lbl85VolPI7I.Visible = false;
        lbl85VolPI7.Visible = false;
        lblProductPI7I.Visible = false;
        lblProductPI7.Visible = false;
        lblSpcGravityPI7I.Visible = false;
        lblSpcGravityPI7.Visible = false;
        lblDateLoadedPI7.Visible = false;
        ucDateLoadedPI7.Visible = false;
        lblROBMTPI7.Visible = false;
        ucROBMTPI7.Visible = false;
        lblROBCUMPI7.Visible = false;
        ucROBCUMPI7.Visible = false;
        lblTankCleanedPI7.Visible = false;
        chkTankCleanedPI7.Visible = false;
        lblDatePI7.Visible = false;
        ucDatePI7.Visible = false;


        lblTankNoPI8I.Visible = false;
        lblTankNoPI8.Visible = false;
        lbl100VolPI8I.Visible = false;
        lbl100VolPI8.Visible = false;
        lbl85VolPI8I.Visible = false;
        lbl85VolPI8.Visible = false;
        lblProductPI8I.Visible = false;
        lblProductPI8.Visible = false;
        lblSpcGravityPI8I.Visible = false;
        lblSpcGravityPI8.Visible = false;
        lblDateLoadedPI8.Visible = false;
        ucDateLoadedPI8.Visible = false;
        lblROBMTPI8.Visible = false;
        ucROBMTPI8.Visible = false;
        lblROBCUMPI8.Visible = false;
        ucROBCUMPI8.Visible = false;
        lblTankCleanedPI8.Visible = false;
        chkTankCleanedPI8.Visible = false;
        lblDatePI8.Visible = false;
        ucDatePI8.Visible = false;


        lblTankNoPI9I.Visible = false;
        lblTankNoPI9.Visible = false;
        lbl100VolPI9I.Visible = false;
        lbl100VolPI9.Visible = false;
        lbl85VolPI9I.Visible = false;
        lbl85VolPI9.Visible = false;
        lblProductPI9I.Visible = false;
        lblProductPI9.Visible = false;
        lblSpcGravityPI9I.Visible = false;
        lblSpcGravityPI9.Visible = false;
        lblDateLoadedPI9.Visible = false;
        ucDateLoadedPI9.Visible = false;
        lblROBMTPI9.Visible = false;
        ucROBMTPI9.Visible = false;
        lblROBCUMPI9.Visible = false;
        ucROBCUMPI9.Visible = false;
        lblTankCleanedPI9.Visible = false;
        chkTankCleanedPI9.Visible = false;
        lblDatePI9.Visible = false;
        ucDatePI9.Visible = false;


        lblTankNoPI10I.Visible = false;
        lblTankNoPI10.Visible = false;
        lbl100VolPI10I.Visible = false;
        lbl100VolPI10.Visible = false;
        lbl85VolPI10I.Visible = false;
        lbl85VolPI10.Visible = false;
        lblProductPI10I.Visible = false;
        lblProductPI10.Visible = false;
        lblSpcGravityPI10I.Visible = false;
        lblSpcGravityPI10.Visible = false;
        lblDateLoadedPI10.Visible = false;
        ucDateLoadedPI10.Visible = false;
        lblROBMTPI10.Visible = false;
        ucROBMTPI10.Visible = false;
        lblROBCUMPI10.Visible = false;
        ucROBCUMPI10.Visible = false;
        lblTankCleanedPI10.Visible = false;
        chkTankCleanedPI10.Visible = false;
        lblDatePI10.Visible = false;
        ucDatePI10.Visible = false;


        lblTankNoPI11I.Visible = false;
        lblTankNoPI11.Visible = false;
        lbl100VolPI11I.Visible = false;
        lbl100VolPI11.Visible = false;
        lbl85VolPI11I.Visible = false;
        lbl85VolPI11.Visible = false;
        lblProductPI11I.Visible = false;
        lblProductPI11.Visible = false;
        lblSpcGravityPI11I.Visible = false;
        lblSpcGravityPI11.Visible = false;
        lblDateLoadedPI11.Visible = false;
        ucDateLoadedPI11.Visible = false;
        lblROBMTPI11.Visible = false;
        ucROBMTPI11.Visible = false;
        lblROBCUMPI11.Visible = false;
        ucROBCUMPI11.Visible = false;
        lblTankCleanedPI11.Visible = false;
        chkTankCleanedPI11.Visible = false;
        lblDatePI11.Visible = false;
        ucDatePI11.Visible = false;


        lblTankNoPI12I.Visible = false;
        lblTankNoPI12.Visible = false;
        lbl100VolPI12I.Visible = false;
        lbl100VolPI12.Visible = false;
        lbl85VolPI12I.Visible = false;
        lbl85VolPI12.Visible = false;
        lblProductPI12I.Visible = false;
        lblProductPI12.Visible = false;
        lblSpcGravityPI12I.Visible = false;
        lblSpcGravityPI12.Visible = false;
        lblDateLoadedPI12.Visible = false;
        ucDateLoadedPI12.Visible = false;
        lblROBMTPI12.Visible = false;
        ucROBMTPI12.Visible = false;
        lblROBCUMPI12.Visible = false;
        ucROBCUMPI12.Visible = false;
        lblTankCleanedPI12.Visible = false;
        chkTankCleanedPI12.Visible = false;
        lblDatePI12.Visible = false;
        ucDatePI12.Visible = false;


        lblTankNoPI13I.Visible = false;
        lblTankNoPI13.Visible = false;
        lbl100VolPI13I.Visible = false;
        lbl100VolPI13.Visible = false;
        lbl85VolPI13I.Visible = false;
        lbl85VolPI13.Visible = false;
        lblProductPI13I.Visible = false;
        lblProductPI13.Visible = false;
        lblSpcGravityPI13I.Visible = false;
        lblSpcGravityPI13.Visible = false;
        lblDateLoadedPI13.Visible = false;
        ucDateLoadedPI13.Visible = false;
        lblROBMTPI13.Visible = false;
        ucROBMTPI13.Visible = false;
        lblROBCUMPI13.Visible = false;
        ucROBCUMPI13.Visible = false;
        lblTankCleanedPI13.Visible = false;
        chkTankCleanedPI13.Visible = false;
        lblDatePI13.Visible = false;
        ucDatePI13.Visible = false;


        lblTankNoPI14I.Visible = false;
        lblTankNoPI14.Visible = false;
        lbl100VolPI14I.Visible = false;
        lbl100VolPI14.Visible = false;
        lbl85VolPI14I.Visible = false;
        lbl85VolPI14.Visible = false;
        lblProductPI14I.Visible = false;
        lblProductPI14.Visible = false;
        lblSpcGravityPI14I.Visible = false;
        lblSpcGravityPI14.Visible = false;
        lblDateLoadedPI14.Visible = false;
        ucDateLoadedPI14.Visible = false;
        lblROBMTPI14.Visible = false;
        ucROBMTPI14.Visible = false;
        lblROBCUMPI14.Visible = false;
        ucROBCUMPI14.Visible = false;
        lblTankCleanedPI14.Visible = false;
        chkTankCleanedPI14.Visible = false;
        lblDatePI14.Visible = false;
        ucDatePI14.Visible = false;


        lblTankNoPI15I.Visible = false;
        lblTankNoPI15.Visible = false;
        lbl100VolPI15I.Visible = false;
        lbl100VolPI15.Visible = false;
        lbl85VolPI15I.Visible = false;
        lbl85VolPI15.Visible = false;
        lblProductPI15I.Visible = false;
        lblProductPI15.Visible = false;
        lblSpcGravityPI15I.Visible = false;
        lblSpcGravityPI15.Visible = false;
        lblDateLoadedPI15.Visible = false;
        ucDateLoadedPI15.Visible = false;
        lblROBMTPI15.Visible = false;
        ucROBMTPI15.Visible = false;
        lblROBCUMPI15.Visible = false;
        ucROBCUMPI15.Visible = false;
        lblTankCleanedPI15.Visible = false;
        chkTankCleanedPI15.Visible = false;
        lblDatePI15.Visible = false;
        ucDatePI15.Visible = false;




        lblTankNoC1I.Visible = false;
        lblTankNoC1.Visible = false;
        lbl100VolC1I.Visible = false;
        lbl100VolC1.Visible = false;
        lbl85VolC1I.Visible = false;
        lbl85VolC1.Visible = false;
        lblProductC1I.Visible = false;
        lblProductC1.Visible = false;
        lblSpcGravityC1I.Visible = false;
        lblSpcGravityC1.Visible = false;
        lblDateLoadedC1.Visible = false;
        ucDateLoadedC1.Visible = false;
        lblROBMTC1.Visible = false;
        ucROBMTC1.Visible = false;
        lblROBCUMC1.Visible = false;
        ucROBCUMC1.Visible = false;
        lblTankCleanedC1.Visible = false;
        chkTankCleanedC1.Visible = false;
        lblDateC1.Visible = false;
        ucDateC1.Visible = false;


        lblTankNoC2I.Visible = false;
        lblTankNoC2.Visible = false;
        lbl100VolC2I.Visible = false;
        lbl100VolC2.Visible = false;
        lbl85VolC2I.Visible = false;
        lbl85VolC2.Visible = false;
        lblProductC2I.Visible = false;
        lblProductC2.Visible = false;
        lblSpcGravityC2I.Visible = false;
        lblSpcGravityC2.Visible = false;
        lblDateLoadedC2.Visible = false;
        ucDateLoadedC2.Visible = false;
        lblROBMTC2.Visible = false;
        ucROBMTC2.Visible = false;
        lblROBCUMC2.Visible = false;
        ucROBCUMC2.Visible = false;
        lblTankCleanedC2.Visible = false;
        chkTankCleanedC2.Visible = false;
        lblDateC2.Visible = false;
        ucDateC2.Visible = false;


        lblTankNoC3I.Visible = false;
        lblTankNoC3.Visible = false;
        lbl100VolC3I.Visible = false;
        lbl100VolC3.Visible = false;
        lbl85VolC3I.Visible = false;
        lbl85VolC3.Visible = false;
        lblProductC3I.Visible = false;
        lblProductC3.Visible = false;
        lblSpcGravityC3I.Visible = false;
        lblSpcGravityC3.Visible = false;
        lblDateLoadedC3.Visible = false;
        ucDateLoadedC3.Visible = false;
        lblROBMTC3.Visible = false;
        ucROBMTC3.Visible = false;
        lblROBCUMC3.Visible = false;
        ucROBCUMC3.Visible = false;
        lblTankCleanedC3.Visible = false;
        chkTankCleanedC3.Visible = false;
        lblDateC3.Visible = false;
        ucDateC3.Visible = false;


        lblTankNoC4I.Visible = false;
        lblTankNoC4.Visible = false;
        lbl100VolC4I.Visible = false;
        lbl100VolC4.Visible = false;
        lbl85VolC4I.Visible = false;
        lbl85VolC4.Visible = false;
        lblProductC4I.Visible = false;
        lblProductC4.Visible = false;
        lblSpcGravityC4I.Visible = false;
        lblSpcGravityC4.Visible = false;
        lblDateLoadedC4.Visible = false;
        ucDateLoadedC4.Visible = false;
        lblROBMTC4.Visible = false;
        ucROBMTC4.Visible = false;
        lblROBCUMC4.Visible = false;
        ucROBCUMC4.Visible = false;
        lblTankCleanedC4.Visible = false;
        chkTankCleanedC4.Visible = false;
        lblDateC4.Visible = false;
        ucDateC4.Visible = false;


        lblTankNoC5I.Visible = false;
        lblTankNoC5.Visible = false;
        lbl100VolC5I.Visible = false;
        lbl100VolC5.Visible = false;
        lbl85VolC5I.Visible = false;
        lbl85VolC5.Visible = false;
        lblProductC5I.Visible = false;
        lblProductC5.Visible = false;
        lblSpcGravityC5I.Visible = false;
        lblSpcGravityC5.Visible = false;
        lblDateLoadedC5.Visible = false;
        ucDateLoadedC5.Visible = false;
        lblROBMTC5.Visible = false;
        ucROBMTC5.Visible = false;
        lblROBCUMC5.Visible = false;
        ucROBCUMC5.Visible = false;
        lblTankCleanedC5.Visible = false;
        chkTankCleanedC5.Visible = false;
        lblDateC5.Visible = false;
        ucDateC5.Visible = false;


        lblTankNoC6I.Visible = false;
        lblTankNoC6.Visible = false;
        lbl100VolC6I.Visible = false;
        lbl100VolC6.Visible = false;
        lbl85VolC6I.Visible = false;
        lbl85VolC6.Visible = false;
        lblProductC6I.Visible = false;
        lblProductC6.Visible = false;
        lblSpcGravityC6I.Visible = false;
        lblSpcGravityC6.Visible = false;
        lblDateLoadedC6.Visible = false;
        ucDateLoadedC6.Visible = false;
        lblROBMTC6.Visible = false;
        ucROBMTC6.Visible = false;
        lblROBCUMC6.Visible = false;
        ucROBCUMC6.Visible = false;
        lblTankCleanedC6.Visible = false;
        chkTankCleanedC6.Visible = false;
        lblDateC6.Visible = false;
        ucDateC6.Visible = false;


        lblTankNoC7I.Visible = false;
        lblTankNoC7.Visible = false;
        lbl100VolC7I.Visible = false;
        lbl100VolC7.Visible = false;
        lbl85VolC7I.Visible = false;
        lbl85VolC7.Visible = false;
        lblProductC7I.Visible = false;
        lblProductC7.Visible = false;
        lblSpcGravityC7I.Visible = false;
        lblSpcGravityC7.Visible = false;
        lblDateLoadedC7.Visible = false;
        ucDateLoadedC7.Visible = false;
        lblROBMTC7.Visible = false;
        ucROBMTC7.Visible = false;
        lblROBCUMC7.Visible = false;
        ucROBCUMC7.Visible = false;
        lblTankCleanedC7.Visible = false;
        chkTankCleanedC7.Visible = false;
        lblDateC7.Visible = false;
        ucDateC7.Visible = false;


        lblTankNoC8I.Visible = false;
        lblTankNoC8.Visible = false;
        lbl100VolC8I.Visible = false;
        lbl100VolC8.Visible = false;
        lbl85VolC8I.Visible = false;
        lbl85VolC8.Visible = false;
        lblProductC8I.Visible = false;
        lblProductC8.Visible = false;
        lblSpcGravityC8I.Visible = false;
        lblSpcGravityC8.Visible = false;
        lblDateLoadedC8.Visible = false;
        ucDateLoadedC8.Visible = false;
        lblROBMTC8.Visible = false;
        ucROBMTC8.Visible = false;
        lblROBCUMC8.Visible = false;
        ucROBCUMC8.Visible = false;
        lblTankCleanedC8.Visible = false;
        chkTankCleanedC8.Visible = false;
        lblDateC8.Visible = false;
        ucDateC8.Visible = false;


        lblTankNoC9I.Visible = false;
        lblTankNoC9.Visible = false;
        lbl100VolC9I.Visible = false;
        lbl100VolC9.Visible = false;
        lbl85VolC9I.Visible = false;
        lbl85VolC9.Visible = false;
        lblProductC9I.Visible = false;
        lblProductC9.Visible = false;
        lblSpcGravityC9I.Visible = false;
        lblSpcGravityC9.Visible = false;
        lblDateLoadedC9.Visible = false;
        ucDateLoadedC9.Visible = false;
        lblROBMTC9.Visible = false;
        ucROBMTC9.Visible = false;
        lblROBCUMC9.Visible = false;
        ucROBCUMC9.Visible = false;
        lblTankCleanedC9.Visible = false;
        chkTankCleanedC9.Visible = false;
        lblDateC9.Visible = false;
        ucDateC9.Visible = false;


        lblTankNoC10I.Visible = false;
        lblTankNoC10.Visible = false;
        lbl100VolC10I.Visible = false;
        lbl100VolC10.Visible = false;
        lbl85VolC10I.Visible = false;
        lbl85VolC10.Visible = false;
        lblProductC10I.Visible = false;
        lblProductC10.Visible = false;
        lblSpcGravityC10I.Visible = false;
        lblSpcGravityC10.Visible = false;
        lblDateLoadedC10.Visible = false;
        ucDateLoadedC10.Visible = false;
        lblROBMTC10.Visible = false;
        ucROBMTC10.Visible = false;
        lblROBCUMC10.Visible = false;
        ucROBCUMC10.Visible = false;
        lblTankCleanedC10.Visible = false;
        chkTankCleanedC10.Visible = false;
        lblDateC10.Visible = false;
        ucDateC10.Visible = false;


        lblTankNoC11I.Visible = false;
        lblTankNoC11.Visible = false;
        lbl100VolC11I.Visible = false;
        lbl100VolC11.Visible = false;
        lbl85VolC11I.Visible = false;
        lbl85VolC11.Visible = false;
        lblProductC11I.Visible = false;
        lblProductC11.Visible = false;
        lblSpcGravityC11I.Visible = false;
        lblSpcGravityC11.Visible = false;
        lblDateLoadedC11.Visible = false;
        ucDateLoadedC11.Visible = false;
        lblROBMTC11.Visible = false;
        ucROBMTC11.Visible = false;
        lblROBCUMC11.Visible = false;
        ucROBCUMC11.Visible = false;
        lblTankCleanedC11.Visible = false;
        chkTankCleanedC11.Visible = false;
        lblDateC11.Visible = false;
        ucDateC11.Visible = false;


        lblTankNoC12I.Visible = false;
        lblTankNoC12.Visible = false;
        lbl100VolC12I.Visible = false;
        lbl100VolC12.Visible = false;
        lbl85VolC12I.Visible = false;
        lbl85VolC12.Visible = false;
        lblProductC12I.Visible = false;
        lblProductC12.Visible = false;
        lblSpcGravityC12I.Visible = false;
        lblSpcGravityC12.Visible = false;
        lblDateLoadedC12.Visible = false;
        ucDateLoadedC12.Visible = false;
        lblROBMTC12.Visible = false;
        ucROBMTC12.Visible = false;
        lblROBCUMC12.Visible = false;
        ucROBCUMC12.Visible = false;
        lblTankCleanedC12.Visible = false;
        chkTankCleanedC12.Visible = false;
        lblDateC12.Visible = false;
        ucDateC12.Visible = false;


        lblTankNoC13I.Visible = false;
        lblTankNoC13.Visible = false;
        lbl100VolC13I.Visible = false;
        lbl100VolC13.Visible = false;
        lbl85VolC13I.Visible = false;
        lbl85VolC13.Visible = false;
        lblProductC13I.Visible = false;
        lblProductC13.Visible = false;
        lblSpcGravityC13I.Visible = false;
        lblSpcGravityC13.Visible = false;
        lblDateLoadedC13.Visible = false;
        ucDateLoadedC13.Visible = false;
        lblROBMTC13.Visible = false;
        ucROBMTC13.Visible = false;
        lblROBCUMC13.Visible = false;
        ucROBCUMC13.Visible = false;
        lblTankCleanedC13.Visible = false;
        chkTankCleanedC13.Visible = false;
        lblDateC13.Visible = false;
        ucDateC13.Visible = false;


        lblTankNoC14I.Visible = false;
        lblTankNoC14.Visible = false;
        lbl100VolC14I.Visible = false;
        lbl100VolC14.Visible = false;
        lbl85VolC14I.Visible = false;
        lbl85VolC14.Visible = false;
        lblProductC14I.Visible = false;
        lblProductC14.Visible = false;
        lblSpcGravityC14I.Visible = false;
        lblSpcGravityC14.Visible = false;
        lblDateLoadedC14.Visible = false;
        ucDateLoadedC14.Visible = false;
        lblROBMTC14.Visible = false;
        ucROBMTC14.Visible = false;
        lblROBCUMC14.Visible = false;
        ucROBCUMC14.Visible = false;
        lblTankCleanedC14.Visible = false;
        chkTankCleanedC14.Visible = false;
        lblDateC14.Visible = false;
        ucDateC14.Visible = false;


        lblTankNoC15I.Visible = false;
        lblTankNoC15.Visible = false;
        lbl100VolC15I.Visible = false;
        lbl100VolC15.Visible = false;
        lbl85VolC15I.Visible = false;
        lbl85VolC15.Visible = false;
        lblProductC15I.Visible = false;
        lblProductC15.Visible = false;
        lblSpcGravityC15I.Visible = false;
        lblSpcGravityC15.Visible = false;
        lblDateLoadedC15.Visible = false;
        ucDateLoadedC15.Visible = false;
        lblROBMTC15.Visible = false;
        ucROBMTC15.Visible = false;
        lblROBCUMC15.Visible = false;
        ucROBCUMC15.Visible = false;
        lblTankCleanedC15.Visible = false;
        chkTankCleanedC15.Visible = false;
        lblDateC15.Visible = false;
        ucDateC15.Visible = false;




        lblTankNoSI1I.Visible = false;
        lblTankNoSI1.Visible = false;
        lbl100VolSI1I.Visible = false;
        lbl100VolSI1.Visible = false;
        lbl85VolSI1I.Visible = false;
        lbl85VolSI1.Visible = false;
        lblProductSI1I.Visible = false;
        lblProductSI1.Visible = false;
        lblSpcGravitySI1I.Visible = false;
        lblSpcGravitySI1.Visible = false;
        lblDateLoadedSI1.Visible = false;
        ucDateLoadedSI1.Visible = false;
        lblROBMTSI1.Visible = false;
        ucROBMTSI1.Visible = false;
        lblROBCUMSI1.Visible = false;
        ucROBCUMSI1.Visible = false;
        lblTankCleanedSI1.Visible = false;
        chkTankCleanedSI1.Visible = false;
        lblDateSI1.Visible = false;
        ucDateSI1.Visible = false;


        lblTankNoSI2I.Visible = false;
        lblTankNoSI2.Visible = false;
        lbl100VolSI2I.Visible = false;
        lbl100VolSI2.Visible = false;
        lbl85VolSI2I.Visible = false;
        lbl85VolSI2.Visible = false;
        lblProductSI2I.Visible = false;
        lblProductSI2.Visible = false;
        lblSpcGravitySI2I.Visible = false;
        lblSpcGravitySI2.Visible = false;
        lblDateLoadedSI2.Visible = false;
        ucDateLoadedSI2.Visible = false;
        lblROBMTSI2.Visible = false;
        ucROBMTSI2.Visible = false;
        lblROBCUMSI2.Visible = false;
        ucROBCUMSI2.Visible = false;
        lblTankCleanedSI2.Visible = false;
        chkTankCleanedSI2.Visible = false;
        lblDateSI2.Visible = false;
        ucDateSI2.Visible = false;


        lblTankNoSI3I.Visible = false;
        lblTankNoSI3.Visible = false;
        lbl100VolSI3I.Visible = false;
        lbl100VolSI3.Visible = false;
        lbl85VolSI3I.Visible = false;
        lbl85VolSI3.Visible = false;
        lblProductSI3I.Visible = false;
        lblProductSI3.Visible = false;
        lblSpcGravitySI3I.Visible = false;
        lblSpcGravitySI3.Visible = false;
        lblDateLoadedSI3.Visible = false;
        ucDateLoadedSI3.Visible = false;
        lblROBMTSI3.Visible = false;
        ucROBMTSI3.Visible = false;
        lblROBCUMSI3.Visible = false;
        ucROBCUMSI3.Visible = false;
        lblTankCleanedSI3.Visible = false;
        chkTankCleanedSI3.Visible = false;
        lblDateSI3.Visible = false;
        ucDateSI3.Visible = false;


        lblTankNoSI4I.Visible = false;
        lblTankNoSI4.Visible = false;
        lbl100VolSI4I.Visible = false;
        lbl100VolSI4.Visible = false;
        lbl85VolSI4I.Visible = false;
        lbl85VolSI4.Visible = false;
        lblProductSI4I.Visible = false;
        lblProductSI4.Visible = false;
        lblSpcGravitySI4I.Visible = false;
        lblSpcGravitySI4.Visible = false;
        lblDateLoadedSI4.Visible = false;
        ucDateLoadedSI4.Visible = false;
        lblROBMTSI4.Visible = false;
        ucROBMTSI4.Visible = false;
        lblROBCUMSI4.Visible = false;
        ucROBCUMSI4.Visible = false;
        lblTankCleanedSI4.Visible = false;
        chkTankCleanedSI4.Visible = false;
        lblDateSI4.Visible = false;
        ucDateSI4.Visible = false;


        lblTankNoSI5I.Visible = false;
        lblTankNoSI5.Visible = false;
        lbl100VolSI5I.Visible = false;
        lbl100VolSI5.Visible = false;
        lbl85VolSI5I.Visible = false;
        lbl85VolSI5.Visible = false;
        lblProductSI5I.Visible = false;
        lblProductSI5.Visible = false;
        lblSpcGravitySI5I.Visible = false;
        lblSpcGravitySI5.Visible = false;
        lblDateLoadedSI5.Visible = false;
        ucDateLoadedSI5.Visible = false;
        lblROBMTSI5.Visible = false;
        ucROBMTSI5.Visible = false;
        lblROBCUMSI5.Visible = false;
        ucROBCUMSI5.Visible = false;
        lblTankCleanedSI5.Visible = false;
        chkTankCleanedSI5.Visible = false;
        lblDateSI5.Visible = false;
        ucDateSI5.Visible = false;


        lblTankNoSI6I.Visible = false;
        lblTankNoSI6.Visible = false;
        lbl100VolSI6I.Visible = false;
        lbl100VolSI6.Visible = false;
        lbl85VolSI6I.Visible = false;
        lbl85VolSI6.Visible = false;
        lblProductSI6I.Visible = false;
        lblProductSI6.Visible = false;
        lblSpcGravitySI6I.Visible = false;
        lblSpcGravitySI6.Visible = false;
        lblDateLoadedSI6.Visible = false;
        ucDateLoadedSI6.Visible = false;
        lblROBMTSI6.Visible = false;
        ucROBMTSI6.Visible = false;
        lblROBCUMSI6.Visible = false;
        ucROBCUMSI6.Visible = false;
        lblTankCleanedSI6.Visible = false;
        chkTankCleanedSI6.Visible = false;
        lblDateSI6.Visible = false;
        ucDateSI6.Visible = false;


        lblTankNoSI7I.Visible = false;
        lblTankNoSI7.Visible = false;
        lbl100VolSI7I.Visible = false;
        lbl100VolSI7.Visible = false;
        lbl85VolSI7I.Visible = false;
        lbl85VolSI7.Visible = false;
        lblProductSI7I.Visible = false;
        lblProductSI7.Visible = false;
        lblSpcGravitySI7I.Visible = false;
        lblSpcGravitySI7.Visible = false;
        lblDateLoadedSI7.Visible = false;
        ucDateLoadedSI7.Visible = false;
        lblROBMTSI7.Visible = false;
        ucROBMTSI7.Visible = false;
        lblROBCUMSI7.Visible = false;
        ucROBCUMSI7.Visible = false;
        lblTankCleanedSI7.Visible = false;
        chkTankCleanedSI7.Visible = false;
        lblDateSI7.Visible = false;
        ucDateSI7.Visible = false;


        lblTankNoSI8I.Visible = false;
        lblTankNoSI8.Visible = false;
        lbl100VolSI8I.Visible = false;
        lbl100VolSI8.Visible = false;
        lbl85VolSI8I.Visible = false;
        lbl85VolSI8.Visible = false;
        lblProductSI8I.Visible = false;
        lblProductSI8.Visible = false;
        lblSpcGravitySI8I.Visible = false;
        lblSpcGravitySI8.Visible = false;
        lblDateLoadedSI8.Visible = false;
        ucDateLoadedSI8.Visible = false;
        lblROBMTSI8.Visible = false;
        ucROBMTSI8.Visible = false;
        lblROBCUMSI8.Visible = false;
        ucROBCUMSI8.Visible = false;
        lblTankCleanedSI8.Visible = false;
        chkTankCleanedSI8.Visible = false;
        lblDateSI8.Visible = false;
        ucDateSI8.Visible = false;


        lblTankNoSI9I.Visible = false;
        lblTankNoSI9.Visible = false;
        lbl100VolSI9I.Visible = false;
        lbl100VolSI9.Visible = false;
        lbl85VolSI9I.Visible = false;
        lbl85VolSI9.Visible = false;
        lblProductSI9I.Visible = false;
        lblProductSI9.Visible = false;
        lblSpcGravitySI9I.Visible = false;
        lblSpcGravitySI9.Visible = false;
        lblDateLoadedSI9.Visible = false;
        ucDateLoadedSI9.Visible = false;
        lblROBMTSI9.Visible = false;
        ucROBMTSI9.Visible = false;
        lblROBCUMSI9.Visible = false;
        ucROBCUMSI9.Visible = false;
        lblTankCleanedSI9.Visible = false;
        chkTankCleanedSI9.Visible = false;
        lblDateSI9.Visible = false;
        ucDateSI9.Visible = false;


        lblTankNoSI10I.Visible = false;
        lblTankNoSI10.Visible = false;
        lbl100VolSI10I.Visible = false;
        lbl100VolSI10.Visible = false;
        lbl85VolSI10I.Visible = false;
        lbl85VolSI10.Visible = false;
        lblProductSI10I.Visible = false;
        lblProductSI10.Visible = false;
        lblSpcGravitySI10I.Visible = false;
        lblSpcGravitySI10.Visible = false;
        lblDateLoadedSI10.Visible = false;
        ucDateLoadedSI10.Visible = false;
        lblROBMTSI10.Visible = false;
        ucROBMTSI10.Visible = false;
        lblROBCUMSI10.Visible = false;
        ucROBCUMSI10.Visible = false;
        lblTankCleanedSI10.Visible = false;
        chkTankCleanedSI10.Visible = false;
        lblDateSI10.Visible = false;
        ucDateSI10.Visible = false;


        lblTankNoSI11I.Visible = false;
        lblTankNoSI11.Visible = false;
        lbl100VolSI11I.Visible = false;
        lbl100VolSI11.Visible = false;
        lbl85VolSI11I.Visible = false;
        lbl85VolSI11.Visible = false;
        lblProductSI11I.Visible = false;
        lblProductSI11.Visible = false;
        lblSpcGravitySI11I.Visible = false;
        lblSpcGravitySI11.Visible = false;
        lblDateLoadedSI11.Visible = false;
        ucDateLoadedSI11.Visible = false;
        lblROBMTSI11.Visible = false;
        ucROBMTSI11.Visible = false;
        lblROBCUMSI11.Visible = false;
        ucROBCUMSI11.Visible = false;
        lblTankCleanedSI11.Visible = false;
        chkTankCleanedSI11.Visible = false;
        lblDateSI11.Visible = false;
        ucDateSI11.Visible = false;


        lblTankNoSI12I.Visible = false;
        lblTankNoSI12.Visible = false;
        lbl100VolSI12I.Visible = false;
        lbl100VolSI12.Visible = false;
        lbl85VolSI12I.Visible = false;
        lbl85VolSI12.Visible = false;
        lblProductSI12I.Visible = false;
        lblProductSI12.Visible = false;
        lblSpcGravitySI12I.Visible = false;
        lblSpcGravitySI12.Visible = false;
        lblDateLoadedSI12.Visible = false;
        ucDateLoadedSI12.Visible = false;
        lblROBMTSI12.Visible = false;
        ucROBMTSI12.Visible = false;
        lblROBCUMSI12.Visible = false;
        ucROBCUMSI12.Visible = false;
        lblTankCleanedSI12.Visible = false;
        chkTankCleanedSI12.Visible = false;
        lblDateSI12.Visible = false;
        ucDateSI12.Visible = false;


        lblTankNoSI13I.Visible = false;
        lblTankNoSI13.Visible = false;
        lbl100VolSI13I.Visible = false;
        lbl100VolSI13.Visible = false;
        lbl85VolSI13I.Visible = false;
        lbl85VolSI13.Visible = false;
        lblProductSI13I.Visible = false;
        lblProductSI13.Visible = false;
        lblSpcGravitySI13I.Visible = false;
        lblSpcGravitySI13.Visible = false;
        lblDateLoadedSI13.Visible = false;
        ucDateLoadedSI13.Visible = false;
        lblROBMTSI13.Visible = false;
        ucROBMTSI13.Visible = false;
        lblROBCUMSI13.Visible = false;
        ucROBCUMSI13.Visible = false;
        lblTankCleanedSI13.Visible = false;
        chkTankCleanedSI13.Visible = false;
        lblDateSI13.Visible = false;
        ucDateSI13.Visible = false;


        lblTankNoSI14I.Visible = false;
        lblTankNoSI14.Visible = false;
        lbl100VolSI14I.Visible = false;
        lbl100VolSI14.Visible = false;
        lbl85VolSI14I.Visible = false;
        lbl85VolSI14.Visible = false;
        lblProductSI14I.Visible = false;
        lblProductSI14.Visible = false;
        lblSpcGravitySI14I.Visible = false;
        lblSpcGravitySI14.Visible = false;
        lblDateLoadedSI14.Visible = false;
        ucDateLoadedSI14.Visible = false;
        lblROBMTSI14.Visible = false;
        ucROBMTSI14.Visible = false;
        lblROBCUMSI14.Visible = false;
        ucROBCUMSI14.Visible = false;
        lblTankCleanedSI14.Visible = false;
        chkTankCleanedSI14.Visible = false;
        lblDateSI14.Visible = false;
        ucDateSI14.Visible = false;


        lblTankNoSI15I.Visible = false;
        lblTankNoSI15.Visible = false;
        lbl100VolSI15I.Visible = false;
        lbl100VolSI15.Visible = false;
        lbl85VolSI15I.Visible = false;
        lbl85VolSI15.Visible = false;
        lblProductSI15I.Visible = false;
        lblProductSI15.Visible = false;
        lblSpcGravitySI15I.Visible = false;
        lblSpcGravitySI15.Visible = false;
        lblDateLoadedSI15.Visible = false;
        ucDateLoadedSI15.Visible = false;
        lblROBMTSI15.Visible = false;
        ucROBMTSI15.Visible = false;
        lblROBCUMSI15.Visible = false;
        ucROBCUMSI15.Visible = false;
        lblTankCleanedSI15.Visible = false;
        chkTankCleanedSI15.Visible = false;
        lblDateSI15.Visible = false;
        ucDateSI15.Visible = false;


        lblTankNoS1I.Visible = false;
        lblTankNoS1.Visible = false;
        lbl100VolS1I.Visible = false;
        lbl100VolS1.Visible = false;
        lbl85VolS1I.Visible = false;
        lbl85VolS1.Visible = false;
        lblProductS1I.Visible = false;
        lblProductS1.Visible = false;
        lblSpcGravityS1I.Visible = false;
        lblSpcGravityS1.Visible = false;
        lblDateLoadedS1.Visible = false;
        ucDateLoadedS1.Visible = false;
        lblROBMTS1.Visible = false;
        ucROBMTS1.Visible = false;
        lblROBCUMS1.Visible = false;
        ucROBCUMS1.Visible = false;
        lblTankCleanedS1.Visible = false;
        chkTankCleanedS1.Visible = false;
        lblDateS1.Visible = false;
        ucDateS1.Visible = false;


        lblTankNoS2I.Visible = false;
        lblTankNoS2.Visible = false;
        lbl100VolS2I.Visible = false;
        lbl100VolS2.Visible = false;
        lbl85VolS2I.Visible = false;
        lbl85VolS2.Visible = false;
        lblProductS2I.Visible = false;
        lblProductS2.Visible = false;
        lblSpcGravityS2I.Visible = false;
        lblSpcGravityS2.Visible = false;
        lblDateLoadedS2.Visible = false;
        ucDateLoadedS2.Visible = false;
        lblROBMTS2.Visible = false;
        ucROBMTS2.Visible = false;
        lblROBCUMS2.Visible = false;
        ucROBCUMS2.Visible = false;
        lblTankCleanedS2.Visible = false;
        chkTankCleanedS2.Visible = false;
        lblDateS2.Visible = false;
        ucDateS2.Visible = false;


        lblTankNoS3I.Visible = false;
        lblTankNoS3.Visible = false;
        lbl100VolS3I.Visible = false;
        lbl100VolS3.Visible = false;
        lbl85VolS3I.Visible = false;
        lbl85VolS3.Visible = false;
        lblProductS3I.Visible = false;
        lblProductS3.Visible = false;
        lblSpcGravityS3I.Visible = false;
        lblSpcGravityS3.Visible = false;
        lblDateLoadedS3.Visible = false;
        ucDateLoadedS3.Visible = false;
        lblROBMTS3.Visible = false;
        ucROBMTS3.Visible = false;
        lblROBCUMS3.Visible = false;
        ucROBCUMS3.Visible = false;
        lblTankCleanedS3.Visible = false;
        chkTankCleanedS3.Visible = false;
        lblDateS3.Visible = false;
        ucDateS3.Visible = false;


        lblTankNoS4I.Visible = false;
        lblTankNoS4.Visible = false;
        lbl100VolS4I.Visible = false;
        lbl100VolS4.Visible = false;
        lbl85VolS4I.Visible = false;
        lbl85VolS4.Visible = false;
        lblProductS4I.Visible = false;
        lblProductS4.Visible = false;
        lblSpcGravityS4I.Visible = false;
        lblSpcGravityS4.Visible = false;
        lblDateLoadedS4.Visible = false;
        ucDateLoadedS4.Visible = false;
        lblROBMTS4.Visible = false;
        ucROBMTS4.Visible = false;
        lblROBCUMS4.Visible = false;
        ucROBCUMS4.Visible = false;
        lblTankCleanedS4.Visible = false;
        chkTankCleanedS4.Visible = false;
        lblDateS4.Visible = false;
        ucDateS4.Visible = false;


        lblTankNoS5I.Visible = false;
        lblTankNoS5.Visible = false;
        lbl100VolS5I.Visible = false;
        lbl100VolS5.Visible = false;
        lbl85VolS5I.Visible = false;
        lbl85VolS5.Visible = false;
        lblProductS5I.Visible = false;
        lblProductS5.Visible = false;
        lblSpcGravityS5I.Visible = false;
        lblSpcGravityS5.Visible = false;
        lblDateLoadedS5.Visible = false;
        ucDateLoadedS5.Visible = false;
        lblROBMTS5.Visible = false;
        ucROBMTS5.Visible = false;
        lblROBCUMS5.Visible = false;
        ucROBCUMS5.Visible = false;
        lblTankCleanedS5.Visible = false;
        chkTankCleanedS5.Visible = false;
        lblDateS5.Visible = false;
        ucDateS5.Visible = false;


        lblTankNoS6I.Visible = false;
        lblTankNoS6.Visible = false;
        lbl100VolS6I.Visible = false;
        lbl100VolS6.Visible = false;
        lbl85VolS6I.Visible = false;
        lbl85VolS6.Visible = false;
        lblProductS6I.Visible = false;
        lblProductS6.Visible = false;
        lblSpcGravityS6I.Visible = false;
        lblSpcGravityS6.Visible = false;
        lblDateLoadedS6.Visible = false;
        ucDateLoadedS6.Visible = false;
        lblROBMTS6.Visible = false;
        ucROBMTS6.Visible = false;
        lblROBCUMS6.Visible = false;
        ucROBCUMS6.Visible = false;
        lblTankCleanedS6.Visible = false;
        chkTankCleanedS6.Visible = false;
        lblDateS6.Visible = false;
        ucDateS6.Visible = false;


        lblTankNoS7I.Visible = false;
        lblTankNoS7.Visible = false;
        lbl100VolS7I.Visible = false;
        lbl100VolS7.Visible = false;
        lbl85VolS7I.Visible = false;
        lbl85VolS7.Visible = false;
        lblProductS7I.Visible = false;
        lblProductS7.Visible = false;
        lblSpcGravityS7I.Visible = false;
        lblSpcGravityS7.Visible = false;
        lblDateLoadedS7.Visible = false;
        ucDateLoadedS7.Visible = false;
        lblROBMTS7.Visible = false;
        ucROBMTS7.Visible = false;
        lblROBCUMS7.Visible = false;
        ucROBCUMS7.Visible = false;
        lblTankCleanedS7.Visible = false;
        chkTankCleanedS7.Visible = false;
        lblDateS7.Visible = false;
        ucDateS7.Visible = false;


        lblTankNoS8I.Visible = false;
        lblTankNoS8.Visible = false;
        lbl100VolS8I.Visible = false;
        lbl100VolS8.Visible = false;
        lbl85VolS8I.Visible = false;
        lbl85VolS8.Visible = false;
        lblProductS8I.Visible = false;
        lblProductS8.Visible = false;
        lblSpcGravityS8I.Visible = false;
        lblSpcGravityS8.Visible = false;
        lblDateLoadedS8.Visible = false;
        ucDateLoadedS8.Visible = false;
        lblROBMTS8.Visible = false;
        ucROBMTS8.Visible = false;
        lblROBCUMS8.Visible = false;
        ucROBCUMS8.Visible = false;
        lblTankCleanedS8.Visible = false;
        chkTankCleanedS8.Visible = false;
        lblDateS8.Visible = false;
        ucDateS8.Visible = false;


        lblTankNoS9I.Visible = false;
        lblTankNoS9.Visible = false;
        lbl100VolS9I.Visible = false;
        lbl100VolS9.Visible = false;
        lbl85VolS9I.Visible = false;
        lbl85VolS9.Visible = false;
        lblProductS9I.Visible = false;
        lblProductS9.Visible = false;
        lblSpcGravityS9I.Visible = false;
        lblSpcGravityS9.Visible = false;
        lblDateLoadedS9.Visible = false;
        ucDateLoadedS9.Visible = false;
        lblROBMTS9.Visible = false;
        ucROBMTS9.Visible = false;
        lblROBCUMS9.Visible = false;
        ucROBCUMS9.Visible = false;
        lblTankCleanedS9.Visible = false;
        chkTankCleanedS9.Visible = false;
        lblDateS9.Visible = false;
        ucDateS9.Visible = false;


        lblTankNoS10I.Visible = false;
        lblTankNoS10.Visible = false;
        lbl100VolS10I.Visible = false;
        lbl100VolS10.Visible = false;
        lbl85VolS10I.Visible = false;
        lbl85VolS10.Visible = false;
        lblProductS10I.Visible = false;
        lblProductS10.Visible = false;
        lblSpcGravityS10I.Visible = false;
        lblSpcGravityS10.Visible = false;
        lblDateLoadedS10.Visible = false;
        ucDateLoadedS10.Visible = false;
        lblROBMTS10.Visible = false;
        ucROBMTS10.Visible = false;
        lblROBCUMS10.Visible = false;
        ucROBCUMS10.Visible = false;
        lblTankCleanedS10.Visible = false;
        chkTankCleanedS10.Visible = false;
        lblDateS10.Visible = false;
        ucDateS10.Visible = false;


        lblTankNoS11I.Visible = false;
        lblTankNoS11.Visible = false;
        lbl100VolS11I.Visible = false;
        lbl100VolS11.Visible = false;
        lbl85VolS11I.Visible = false;
        lbl85VolS11.Visible = false;
        lblProductS11I.Visible = false;
        lblProductS11.Visible = false;
        lblSpcGravityS11I.Visible = false;
        lblSpcGravityS11.Visible = false;
        lblDateLoadedS11.Visible = false;
        ucDateLoadedS11.Visible = false;
        lblROBMTS11.Visible = false;
        ucROBMTS11.Visible = false;
        lblROBCUMS11.Visible = false;
        ucROBCUMS11.Visible = false;
        lblTankCleanedS11.Visible = false;
        chkTankCleanedS11.Visible = false;
        lblDateS11.Visible = false;
        ucDateS11.Visible = false;


        lblTankNoS12I.Visible = false;
        lblTankNoS12.Visible = false;
        lbl100VolS12I.Visible = false;
        lbl100VolS12.Visible = false;
        lbl85VolS12I.Visible = false;
        lbl85VolS12.Visible = false;
        lblProductS12I.Visible = false;
        lblProductS12.Visible = false;
        lblSpcGravityS12I.Visible = false;
        lblSpcGravityS12.Visible = false;
        lblDateLoadedS12.Visible = false;
        ucDateLoadedS12.Visible = false;
        lblROBMTS12.Visible = false;
        ucROBMTS12.Visible = false;
        lblROBCUMS12.Visible = false;
        ucROBCUMS12.Visible = false;
        lblTankCleanedS12.Visible = false;
        chkTankCleanedS12.Visible = false;
        lblDateS12.Visible = false;
        ucDateS12.Visible = false;


        lblTankNoS13I.Visible = false;
        lblTankNoS13.Visible = false;
        lbl100VolS13I.Visible = false;
        lbl100VolS13.Visible = false;
        lbl85VolS13I.Visible = false;
        lbl85VolS13.Visible = false;
        lblProductS13I.Visible = false;
        lblProductS13.Visible = false;
        lblSpcGravityS13I.Visible = false;
        lblSpcGravityS13.Visible = false;
        lblDateLoadedS13.Visible = false;
        ucDateLoadedS13.Visible = false;
        lblROBMTS13.Visible = false;
        ucROBMTS13.Visible = false;
        lblROBCUMS13.Visible = false;
        ucROBCUMS13.Visible = false;
        lblTankCleanedS13.Visible = false;
        chkTankCleanedS13.Visible = false;
        lblDateS13.Visible = false;
        ucDateS13.Visible = false;


        lblTankNoS14I.Visible = false;
        lblTankNoS14.Visible = false;
        lbl100VolS14I.Visible = false;
        lbl100VolS14.Visible = false;
        lbl85VolS14I.Visible = false;
        lbl85VolS14.Visible = false;
        lblProductS14I.Visible = false;
        lblProductS14.Visible = false;
        lblSpcGravityS14I.Visible = false;
        lblSpcGravityS14.Visible = false;
        lblDateLoadedS14.Visible = false;
        ucDateLoadedS14.Visible = false;
        lblROBMTS14.Visible = false;
        ucROBMTS14.Visible = false;
        lblROBCUMS14.Visible = false;
        ucROBCUMS14.Visible = false;
        lblTankCleanedS14.Visible = false;
        chkTankCleanedS14.Visible = false;
        lblDateS14.Visible = false;
        ucDateS14.Visible = false;


        lblTankNoS15I.Visible = false;
        lblTankNoS15.Visible = false;
        lbl100VolS15I.Visible = false;
        lbl100VolS15.Visible = false;
        lbl85VolS15I.Visible = false;
        lbl85VolS15.Visible = false;
        lblProductS15I.Visible = false;
        lblProductS15.Visible = false;
        lblSpcGravityS15I.Visible = false;
        lblSpcGravityS15.Visible = false;
        lblDateLoadedS15.Visible = false;
        ucDateLoadedS15.Visible = false;
        lblROBMTS15.Visible = false;
        ucROBMTS15.Visible = false;
        lblROBCUMS15.Visible = false;
        ucROBCUMS15.Visible = false;
        lblTankCleanedS15.Visible = false;
        chkTankCleanedS15.Visible = false;
        lblDateS15.Visible = false;
        ucDateS15.Visible = false;

        ucROBCUMP1.Enabled = false;
        ucDateP1.Enabled = false;
        ucROBCUMPI1.Enabled = false;
        ucDatePI1.Enabled = false;
        ucROBCUMC1.Enabled = false;
        ucDateC1.Enabled = false;
        ucROBCUMSI1.Enabled = false;
        ucDateSI1.Enabled = false;
        ucROBCUMS1.Enabled = false;
        ucDateS1.Enabled = false;

        ucROBCUMP2.Enabled = false;
        ucDateP2.Enabled = false;
        ucROBCUMPI2.Enabled = false;
        ucDatePI2.Enabled = false;
        ucROBCUMC2.Enabled = false;
        ucDateC2.Enabled = false;
        ucROBCUMSI2.Enabled = false;
        ucDateSI2.Enabled = false;
        ucROBCUMS2.Enabled = false;
        ucDateS2.Enabled = false;

        ucROBCUMP3.Enabled = false;
        ucDateP3.Enabled = false;
        ucROBCUMPI3.Enabled = false;
        ucDatePI3.Enabled = false;
        ucROBCUMC3.Enabled = false;
        ucDateC3.Enabled = false;
        ucROBCUMSI3.Enabled = false;
        ucDateSI3.Enabled = false;
        ucROBCUMS3.Enabled = false;
        ucDateS3.Enabled = false;

        ucROBCUMP4.Enabled = false;
        ucDateP4.Enabled = false;
        ucROBCUMPI4.Enabled = false;
        ucDatePI4.Enabled = false;
        ucROBCUMC4.Enabled = false;
        ucDateC4.Enabled = false;
        ucROBCUMSI4.Enabled = false;
        ucDateSI4.Enabled = false;
        ucROBCUMS4.Enabled = false;
        ucDateS4.Enabled = false;

        ucROBCUMP5.Enabled = false;
        ucDateP5.Enabled = false;
        ucROBCUMPI5.Enabled = false;
        ucDatePI5.Enabled = false;
        ucROBCUMC5.Enabled = false;
        ucDateC5.Enabled = false;
        ucROBCUMSI5.Enabled = false;
        ucDateSI5.Enabled = false;
        ucROBCUMS5.Enabled = false;
        ucDateS5.Enabled = false;

        ucROBCUMP6.Enabled = false;
        ucDateP6.Enabled = false;
        ucROBCUMPI6.Enabled = false;
        ucDatePI6.Enabled = false;
        ucROBCUMC6.Enabled = false;
        ucDateC6.Enabled = false;
        ucROBCUMSI6.Enabled = false;
        ucDateSI6.Enabled = false;
        ucROBCUMS6.Enabled = false;
        ucDateS6.Enabled = false;

        ucROBCUMP7.Enabled = false;
        ucDateP7.Enabled = false;
        ucROBCUMPI7.Enabled = false;
        ucDatePI7.Enabled = false;
        ucROBCUMC7.Enabled = false;
        ucDateC7.Enabled = false;
        ucROBCUMSI7.Enabled = false;
        ucDateSI7.Enabled = false;
        ucROBCUMS7.Enabled = false;
        ucDateS7.Enabled = false;

        ucROBCUMP8.Enabled = false;
        ucDateP8.Enabled = false;
        ucROBCUMPI8.Enabled = false;
        ucDatePI8.Enabled = false;
        ucROBCUMC8.Enabled = false;
        ucDateC8.Enabled = false;
        ucROBCUMSI8.Enabled = false;
        ucDateSI8.Enabled = false;
        ucROBCUMS8.Enabled = false;
        ucDateS8.Enabled = false;

        ucROBCUMP9.Enabled = false;
        ucDateP9.Enabled = false;
        ucROBCUMPI9.Enabled = false;
        ucDatePI9.Enabled = false;
        ucROBCUMC9.Enabled = false;
        ucDateC9.Enabled = false;
        ucROBCUMSI9.Enabled = false;
        ucDateSI9.Enabled = false;
        ucROBCUMS9.Enabled = false;
        ucDateS9.Enabled = false;

        ucROBCUMP10.Enabled = false;
        ucDateP10.Enabled = false;
        ucROBCUMPI10.Enabled = false;
        ucDatePI10.Enabled = false;
        ucROBCUMC10.Enabled = false;
        ucDateC10.Enabled = false;
        ucROBCUMSI10.Enabled = false;
        ucDateSI10.Enabled = false;
        ucROBCUMS10.Enabled = false;
        ucDateS10.Enabled = false;

        ucROBCUMP11.Enabled = false;
        ucDateP11.Enabled = false;
        ucROBCUMPI11.Enabled = false;
        ucDatePI11.Enabled = false;
        ucROBCUMC11.Enabled = false;
        ucDateC11.Enabled = false;
        ucROBCUMSI11.Enabled = false;
        ucDateSI11.Enabled = false;
        ucROBCUMS11.Enabled = false;
        ucDateS11.Enabled = false;

        ucROBCUMP12.Enabled = false;
        ucDateP12.Enabled = false;
        ucROBCUMPI12.Enabled = false;
        ucDatePI12.Enabled = false;
        ucROBCUMC12.Enabled = false;
        ucDateC12.Enabled = false;
        ucROBCUMSI12.Enabled = false;
        ucDateSI12.Enabled = false;
        ucROBCUMS12.Enabled = false;
        ucDateS12.Enabled = false;

        ucROBCUMP13.Enabled = false;
        ucDateP13.Enabled = false;
        ucROBCUMPI13.Enabled = false;
        ucDatePI13.Enabled = false;
        ucROBCUMC13.Enabled = false;
        ucDateC13.Enabled = false;
        ucROBCUMSI13.Enabled = false;
        ucDateSI13.Enabled = false;
        ucROBCUMS13.Enabled = false;
        ucDateS13.Enabled = false;

        ucROBCUMP14.Enabled = false;
        ucDateP14.Enabled = false;
        ucROBCUMPI14.Enabled = false;
        ucDatePI14.Enabled = false;
        ucROBCUMC14.Enabled = false;
        ucDateC14.Enabled = false;
        ucROBCUMSI14.Enabled = false;
        ucDateSI14.Enabled = false;
        ucROBCUMS14.Enabled = false;
        ucDateS14.Enabled = false;

        ucROBCUMP15.Enabled = false;
        ucDateP15.Enabled = false;
        ucROBCUMPI15.Enabled = false;
        ucDatePI15.Enabled = false;
        ucROBCUMC15.Enabled = false;
        ucDateC15.Enabled = false;
        ucROBCUMSI15.Enabled = false;
        ucDateSI15.Enabled = false;
        ucROBCUMS15.Enabled = false;
        ucDateS15.Enabled = false;
    }
    private void HideControls(int maxverticalvalue)
    {
        if (maxverticalvalue == 0)
        {
            spn1.Visible = false;
            spn2.Visible = false;
            spn3.Visible = false;
            spn4.Visible = false;
            spn5.Visible = false;
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
            
        }
        else if (maxverticalvalue == 1)
        {
            spn2.Visible = false;
            spn3.Visible = false;
            spn4.Visible = false;
            spn5.Visible = false;
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 2)
        {
            spn3.Visible = false;
            spn4.Visible = false;
            spn5.Visible = false;
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 3)
        {
            spn4.Visible = false;
            spn5.Visible = false;
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 4)
        {
            spn5.Visible = false;
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 5)
        {
            spn6.Visible = false;
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 6)
        {
            spn7.Visible = false;
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 7)
        {
            spn8.Visible = false;
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 8)
        {
            spn9.Visible = false;
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 9)
        {
            spn10.Visible = false;
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 10)
        {
            spn11.Visible = false;
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 11)
        {
            spn12.Visible = false;
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 12)
        {
            spn13.Visible = false;
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 13)
        {
            spn14.Visible = false;
            spn15.Visible = false;
        }
        else if (maxverticalvalue == 14)
        {
            spn15.Visible = false;
        }

    }
    //private void BindMethanolTanks()
    //{
    //    try
    //    {
    //        DataSet ds = PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreTankPlanMethanolLoadandConsumptionList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
    //        DataTable dt = ds.Tables[0];
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            div1.Visible = true;
    //            lblPortTankNo.Text=dt.Rows[0]["FLDLOCATIONNAME"].ToString();
    //            lblCap100Port.Text=dt.Rows[0]["FLDCAPACITYAT100PERCENT"].ToString();
    //            lblCap85Port.Text=dt.Rows[0]["FLDCAPACITYAT85PERCENT"].ToString();
    //            lblProductPort.Text=dt.Rows[0]["FLDOILTYPENAME"].ToString();
    //            txtDateLoadedPort.Text=dt.Rows[0]["FLDDATELOADED"].ToString();
    //            txtrobCumPort.Text = dt.Rows[0]["FLDROBCUM"].ToString();
    //            lblTankPortConfigid.Text = dt.Rows[0]["FLDTANKPLANCONFIGURATIONID"].ToString();
    //            lblPortCounsumptionid.Text = dt.Rows[0]["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();

    //        }
    //        if (ds.Tables[0].Rows.Count == 2)
    //        {
    //            lblStbdTankNo.Text = dt.Rows[1]["FLDLOCATIONNAME"].ToString();
    //            lblCap100Stbd.Text = dt.Rows[1]["FLDCAPACITYAT100PERCENT"].ToString();
    //            lblCap85Stbd.Text = dt.Rows[1]["FLDCAPACITYAT85PERCENT"].ToString();
    //            lblProductStbd.Text = dt.Rows[1]["FLDOILTYPENAME"].ToString();
    //            txtDateLoadedstbd.Text = dt.Rows[1]["FLDDATELOADEDSTBD"].ToString();
    //            txtrobCumstbd.Text = dt.Rows[1]["FLDROBSTBD"].ToString();
    //            lblTankStbdConfigid.Text = dt.Rows[1]["FLDTANKPLANCONFIGURATIONID"].ToString();
    //            lblStbdCounsumptionid.Text = dt.Rows[1]["FLDDMRTANKPLANLOADEDANDCONSUMPTION"].ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
        
    //}

    //private void UpdateMethanolTank()
    //{
    //    PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreDMRTankPlanMethanolConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //    , General.GetNullableDateTime(txtDateLoadedPort.Text), General.GetNullableDecimal(txtrobCumPort.Text),null, null, General.GetNullableGuid(lblPortCounsumptionid.Text), General.GetNullableGuid(lblTankPortConfigid.Text));
        
    //    PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreDMRTankPlanMethanolConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //    , General.GetNullableDateTime(txtDateLoadedstbd.Text), null, null, General.GetNullableDecimal(txtrobCumstbd.Text), General.GetNullableGuid(lblStbdCounsumptionid.Text), General.GetNullableGuid(lblTankStbdConfigid.Text));
       
    //    BindMethanolTanks();

    //}

    

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        try
        {
            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {

                    if (spn1.Visible)
                    {
                        if (!spnP1.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP1.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP1.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP1.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP1.Value),
                                                                                                                    chkTankCleanedP1.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP1.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP1.Text),
                                                                                                                    General.GetNullableGuid(lblProductP1.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP1.Checked == true ? "1" : "0")
                                                                                                                    ,General.GetNullableInteger(chkpostponealertP1.Checked == true ? "1" : "0")
                                                                                                                    ,txtpostponeexpremarksP1.Text
                                                                                                                    );
                        }

                        if (!spnPI1.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI1.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI1.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI1.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI1.Value),
                                                                                                                    chkTankCleanedPI1.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI1.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI1.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI1.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI1.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI1.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI1.Text
                                                                                                                    );
                        }

                        if (!spnC1.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC1.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC1.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC1.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC1.Value),
                                                                                                                    chkTankCleanedC1.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC1.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC1.Text),
                                                                                                                    General.GetNullableGuid(lblProductC1.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC1.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC1.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC1.Text
                                                                                                                    );
                        }

                        if (!spnSI1.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI1.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI1.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI1.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI1.Value),
                                                                                                                    chkTankCleanedSI1.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI1.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI1.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI1.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI1.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI1.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI1.Text
                                                                                                                    );
                        }
                        if (!spnS1.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS1.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS1.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS1.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS1.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS1.Value),
                                                                                                                    chkTankCleanedS1.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS1.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS1.Text),
                                                                                                                    General.GetNullableGuid(lblProductS1.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS1.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS1.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS1.Text
                                                                                                                    );
                        }

                    }
                    if (spn2.Visible)
                    {
                        if (!spnP2.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP2.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP2.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP2.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP2.Value),
                                                                                                                    chkTankCleanedP2.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP2.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP2.Text),
                                                                                                                    General.GetNullableGuid(lblProductP2.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP2.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP2.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP2.Text
                                                                                                                    );
                        }

                        if (!spnPI2.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI2.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI2.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI2.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI2.Value),
                                                                                                                    chkTankCleanedPI2.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI2.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI2.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI2.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI2.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI2.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI2.Text
                                                                                                                    );
                        }

                        if (!spnC2.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC2.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC2.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC2.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC2.Value),
                                                                                                                    chkTankCleanedC2.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC2.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC2.Text),
                                                                                                                    General.GetNullableGuid(lblProductC2.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC2.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC2.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC2.Text
                                                                                                                    );
                        }

                        if (!spnSI2.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI2.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI2.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI2.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI2.Value),
                                                                                                                    chkTankCleanedSI2.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI2.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI2.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI2.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI2.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI2.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI2.Text
                                                                                                                    );
                        }
                        if (!spnS2.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS2.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS2.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS2.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS2.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS2.Value),
                                                                                                                    chkTankCleanedS2.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS2.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS2.Text),
                                                                                                                    General.GetNullableGuid(lblProductS2.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS2.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS2.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS2.Text
                                                                                                                    );
                        }

                    }
                    if (spn3.Visible)
                    {
                        if (!spnP3.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP3.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP3.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP3.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP3.Value),
                                                                                                                    chkTankCleanedP3.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP3.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP3.Text),
                                                                                                                    General.GetNullableGuid(lblProductP3.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP3.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP3.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP3.Text
                                                                                                                    );
                        }

                        if (!spnPI3.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI3.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI3.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI3.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI3.Value),
                                                                                                                    chkTankCleanedPI3.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI3.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI3.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI3.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI3.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI3.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI3.Text
                                                                                                                    );
                        }

                        if (!spnC3.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC3.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC3.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC3.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC3.Value),
                                                                                                                    chkTankCleanedC3.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC3.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC3.Text),
                                                                                                                    General.GetNullableGuid(lblProductC3.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC3.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC3.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC3.Text
                                                                                                                    );
                        }

                        if (!spnSI3.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI3.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI3.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI3.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI3.Value),
                                                                                                                    chkTankCleanedSI3.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI3.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI3.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI3.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI3.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI3.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI3.Text
                                                                                                                    );
                        }
                        if (!spnS3.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS3.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS3.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS3.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS3.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS3.Value),
                                                                                                                    chkTankCleanedS3.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS3.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS3.Text),
                                                                                                                    General.GetNullableGuid(lblProductS3.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS3.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS3.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS3.Text
                                                                                                                    );
                        }

                    }
                    if (spn4.Visible)
                    {
                        if (!spnP4.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP4.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP4.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP4.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP4.Value),
                                                                                                                    chkTankCleanedP4.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP4.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP4.Text),
                                                                                                                    General.GetNullableGuid(lblProductP4.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP4.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP4.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP4.Text
                                                                                                                    );
                        }

                        if (!spnPI4.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI4.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI4.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI4.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI4.Value),
                                                                                                                    chkTankCleanedPI4.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI4.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI4.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI4.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI4.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI4.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI4.Text
                                                                                                                    );
                        }

                        if (!spnC4.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC4.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC4.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC4.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC4.Value),
                                                                                                                    chkTankCleanedC4.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC4.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC4.Text),
                                                                                                                    General.GetNullableGuid(lblProductC4.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC4.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC4.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC4.Text
                                                                                                                    );
                        }

                        if (!spnSI4.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI4.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI4.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI4.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI4.Value),
                                                                                                                    chkTankCleanedSI4.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI4.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI4.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI4.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI4.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI4.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI4.Text
                                                                                                                    );
                        }
                        if (!spnS4.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS4.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS4.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS4.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS4.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS4.Value),
                                                                                                                    chkTankCleanedS4.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS4.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS4.Text),
                                                                                                                    General.GetNullableGuid(lblProductS4.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS4.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS4.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS4.Text
                                                                                                                    );
                        }

                    }
                    if (spn5.Visible)
                    {
                        if (!spnP5.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP5.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP5.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP5.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP5.Value),
                                                                                                                    chkTankCleanedP5.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP5.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP5.Text),
                                                                                                                    General.GetNullableGuid(lblProductP5.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP5.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP5.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP5.Text
                                                                                                                    );
                        }

                        if (!spnPI5.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI5.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI5.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI5.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI5.Value),
                                                                                                                    chkTankCleanedPI5.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI5.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI5.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI5.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI5.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI5.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI5.Text
                                                                                                                    );
                        }

                        if (!spnC5.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC5.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC5.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC5.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC5.Value),
                                                                                                                    chkTankCleanedC5.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC5.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC5.Text),
                                                                                                                    General.GetNullableGuid(lblProductC5.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC5.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC5.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC5.Text
                                                                                                                    );
                        }

                        if (!spnSI5.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI5.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI5.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI5.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI5.Value),
                                                                                                                    chkTankCleanedSI5.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI5.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI5.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI5.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI5.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI5.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI5.Text
                                                                                                                    );
                        }
                        if (!spnS5.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS5.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS5.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS5.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS5.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS5.Value),
                                                                                                                    chkTankCleanedS5.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS5.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS5.Text),
                                                                                                                    General.GetNullableGuid(lblProductS5.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS5.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS5.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS5.Text
                                                                                                                    );
                        }

                    }
                    if (spn6.Visible)
                    {
                        if (!spnP6.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP6.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP6.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP6.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP6.Value),
                                                                                                                    chkTankCleanedP6.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP6.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP6.Text),
                                                                                                                    General.GetNullableGuid(lblProductP6.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP6.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP6.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP6.Text
                                                                                                                    );
                        }

                        if (!spnPI6.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI6.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI6.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI6.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI6.Value),
                                                                                                                    chkTankCleanedPI6.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI6.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI6.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI6.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI6.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI6.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI6.Text
                                                                                                                    );
                        }

                        if (!spnC6.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC6.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC6.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC6.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC6.Value),
                                                                                                                    chkTankCleanedC6.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC6.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC6.Text),
                                                                                                                    General.GetNullableGuid(lblProductC6.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC6.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC6.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC6.Text
                                                                                                                    );
                        }

                        if (!spnSI6.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI6.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI6.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI6.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI6.Value),
                                                                                                                    chkTankCleanedSI6.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI6.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI6.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI6.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI6.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI6.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI6.Text
                                                                                                                    );
                        }
                        if (!spnS6.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS6.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS6.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS6.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS6.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS6.Value),
                                                                                                                    chkTankCleanedS6.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS6.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS6.Text),
                                                                                                                    General.GetNullableGuid(lblProductS6.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS6.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS6.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS6.Text
                                                                                                                    );
                        }

                    }
                    if (spn7.Visible)
                    {
                        if (!spnP7.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP7.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP7.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP7.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP7.Value),
                                                                                                                    chkTankCleanedP7.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP7.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP7.Text),
                                                                                                                    General.GetNullableGuid(lblProductP7.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP7.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP7.Checked == true ? "1" : "0")
                                                                                                                    ,txtpostponeexpremarksP7.Text
                                                                                                                    );
                        }

                        if (!spnPI7.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI7.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI7.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI7.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI7.Value),
                                                                                                                    chkTankCleanedPI7.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI7.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI7.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI7.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI7.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI7.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI7.Text
                                                                                                                    );
                        }

                        if (!spnC7.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC7.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC7.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC7.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC7.Value),
                                                                                                                    chkTankCleanedC7.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC7.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC7.Text),
                                                                                                                    General.GetNullableGuid(lblProductC7.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC7.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC7.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC7.Text
                                                                                                                    );
                        }

                        if (!spnSI7.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI7.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI7.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI7.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI7.Value),
                                                                                                                    chkTankCleanedSI7.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI7.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI7.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI7.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI7.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI7.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI7.Text
                                                                                                                    );
                        }
                        if (!spnS7.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS7.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS7.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS7.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS7.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS7.Value),
                                                                                                                    chkTankCleanedS7.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS7.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS7.Text),
                                                                                                                    General.GetNullableGuid(lblProductS7.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS7.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS7.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS7.Text
                                                                                                                    );
                        }

                    }
                    if (spn8.Visible)
                    {
                        if (!spnP8.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP8.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP8.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP8.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP8.Value),
                                                                                                                    chkTankCleanedP8.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP8.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP8.Text),
                                                                                                                    General.GetNullableGuid(lblProductP8.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP8.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP8.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP8.Text
                                                                                                                    );
                        }

                        if (!spnPI8.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI8.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI8.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI8.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI8.Value),
                                                                                                                    chkTankCleanedPI8.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI8.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI8.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI8.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI8.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI8.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI8.Text
                                                                                                                    );
                        }

                        if (!spnC8.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC8.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC8.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC8.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC8.Value),
                                                                                                                    chkTankCleanedC8.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC8.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC8.Text),
                                                                                                                    General.GetNullableGuid(lblProductC8.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC8.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC8.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC8.Text
                                                                                                                    );
                        }

                        if (!spnSI8.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI8.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI8.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI8.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI8.Value),
                                                                                                                    chkTankCleanedSI8.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI8.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI8.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI8.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI8.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI8.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI8.Text
                                                                                                                    );
                        }
                        if (!spnS8.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS8.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS8.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS8.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS8.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS8.Value),
                                                                                                                    chkTankCleanedS8.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS8.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS8.Text),
                                                                                                                    General.GetNullableGuid(lblProductS8.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS8.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS8.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS8.Text
                                                                                                                    );
                        }

                    }
                    if (spn9.Visible)
                    {
                        if (!spnP9.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP9.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP9.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP9.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP9.Value),
                                                                                                                    chkTankCleanedP9.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP9.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP9.Text),
                                                                                                                    General.GetNullableGuid(lblProductP9.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP9.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP9.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP9.Text
                                                                                                                    );
                        }

                        if (!spnPI9.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI9.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI9.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI9.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI9.Value),
                                                                                                                    chkTankCleanedPI9.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI9.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI9.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI9.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI9.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI9.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI9.Text
                                                                                                                    );
                        }

                        if (!spnC9.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC9.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC9.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC9.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC9.Value),
                                                                                                                    chkTankCleanedC9.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC9.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC9.Text),
                                                                                                                    General.GetNullableGuid(lblProductC9.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC9.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC9.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC9.Text
                                                                                                                    );
                        }

                        if (!spnSI9.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI9.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI9.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI9.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI9.Value),
                                                                                                                    chkTankCleanedSI9.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI9.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI9.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI9.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI9.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI9.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI9.Text
                                                                                                                    );
                        }
                        if (!spnS9.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS9.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS9.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS9.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS9.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS9.Value),
                                                                                                                    chkTankCleanedS9.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS9.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS9.Text),
                                                                                                                    General.GetNullableGuid(lblProductS9.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS9.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS9.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS9.Text
                                                                                                                    );
                        }

                    }
                    if (spn10.Visible)
                    {
                        if (!spnP10.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP10.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP10.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP10.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP10.Value),
                                                                                                                    chkTankCleanedP10.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP10.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP10.Text),
                                                                                                                    General.GetNullableGuid(lblProductP10.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP10.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP10.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP10.Text
                                                                                                                    );
                        }

                        if (!spnPI10.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI10.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI10.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI10.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI10.Value),
                                                                                                                    chkTankCleanedPI10.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI10.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI10.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI10.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI10.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI10.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI10.Text
                                                                                                                    );
                        }

                        if (!spnC10.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC10.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC10.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC10.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC10.Value),
                                                                                                                    chkTankCleanedC10.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC10.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC10.Text),
                                                                                                                    General.GetNullableGuid(lblProductC10.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC10.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC10.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC10.Text
                                                                                                                    );
                        }

                        if (!spnSI10.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI10.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI10.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI10.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI10.Value),
                                                                                                                    chkTankCleanedSI10.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI10.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI10.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI10.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI10.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI10.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI10.Text
                                                                                                                    );
                        }
                        if (!spnS10.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS10.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS10.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS10.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS10.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS10.Value),
                                                                                                                    chkTankCleanedS10.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS10.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS10.Text),
                                                                                                                    General.GetNullableGuid(lblProductS10.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS10.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS10.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS10.Text
                                                                                                                    );
                        }

                    }
                    if (spn11.Visible)
                    {
                        if (!spnP11.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP11.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP11.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP11.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP11.Value),
                                                                                                                    chkTankCleanedP11.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP11.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP11.Text),
                                                                                                                    General.GetNullableGuid(lblProductP11.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP11.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP11.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP11.Text
                                                                                                                    );
                        }

                        if (!spnPI11.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI11.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI11.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI11.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI11.Value),
                                                                                                                    chkTankCleanedPI11.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI11.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI11.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI11.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI11.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI11.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI11.Text
                                                                                                                    );
                        }

                        if (!spnC11.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC11.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC11.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC11.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC11.Value),
                                                                                                                    chkTankCleanedC11.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC11.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC11.Text),
                                                                                                                    General.GetNullableGuid(lblProductC11.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC11.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC11.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC11.Text
                                                                                                                    );
                        }

                        if (!spnSI11.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI11.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI11.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI11.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI11.Value),
                                                                                                                    chkTankCleanedSI11.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI11.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI11.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI11.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI11.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI11.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI11.Text
                                                                                                                    );
                        }
                        if (!spnS11.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS11.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS11.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS11.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS11.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS11.Value),
                                                                                                                    chkTankCleanedS11.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS11.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS11.Text),
                                                                                                                    General.GetNullableGuid(lblProductS11.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS11.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS11.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS11.Text
                                                                                                                    );
                        }

                    }
                    if (spn12.Visible)
                    {
                        if (!spnP12.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP12.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP12.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP12.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP12.Value),
                                                                                                                    chkTankCleanedP12.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP12.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP12.Text),
                                                                                                                    General.GetNullableGuid(lblProductP12.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP12.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP12.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP12.Text
                                                                                                                    );
                        }

                        if (!spnPI12.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI12.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI12.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI12.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI12.Value),
                                                                                                                    chkTankCleanedPI12.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI12.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI12.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI12.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI12.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI12.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI12.Text
                                                                                                                    );
                        }

                        if (!spnC12.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC12.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC12.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC12.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC12.Value),
                                                                                                                    chkTankCleanedC12.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC12.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC12.Text),
                                                                                                                    General.GetNullableGuid(lblProductC12.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC12.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC12.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC12.Text
                                                                                                                    );
                        }

                        if (!spnSI12.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI12.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI12.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI12.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI12.Value),
                                                                                                                    chkTankCleanedSI12.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI12.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI12.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI12.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI12.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI12.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI12.Text
                                                                                                                    );
                        }
                        if (!spnS12.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS12.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS12.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS12.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS12.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS12.Value),
                                                                                                                    chkTankCleanedS12.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS12.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS12.Text),
                                                                                                                    General.GetNullableGuid(lblProductS12.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS12.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS12.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS12.Text
                                                                                                                    );
                        }

                    }
                    if (spn13.Visible)
                    {
                        if (!spnP13.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP13.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP13.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP13.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP13.Value),
                                                                                                                    chkTankCleanedP13.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP13.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP13.Text),
                                                                                                                    General.GetNullableGuid(lblProductP13.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP13.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP13.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP13.Text
                                                                                                                    );
                        }

                        if (!spnPI13.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI13.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI13.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI13.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI13.Value),
                                                                                                                    chkTankCleanedPI13.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI13.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI13.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI13.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI13.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI13.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI13.Text
                                                                                                                    );
                        }

                        if (!spnC13.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC13.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC13.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC13.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC13.Value),
                                                                                                                    chkTankCleanedC13.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC13.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC13.Text),
                                                                                                                    General.GetNullableGuid(lblProductC13.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC13.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC13.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC13.Text
                                                                                                                    );
                        }

                        if (!spnSI13.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI13.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI13.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI13.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI13.Value),
                                                                                                                    chkTankCleanedSI13.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI13.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI13.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI13.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI13.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI13.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI13.Text
                                                                                                                    );
                        }
                        if (!spnS13.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS13.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS13.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS13.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS13.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS13.Value),
                                                                                                                    chkTankCleanedS13.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS13.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS13.Text),
                                                                                                                    General.GetNullableGuid(lblProductS13.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS13.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS13.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS13.Text
                                                                                                                    );
                        }

                    }
                    if (spn14.Visible)
                    {
                        if (!spnP14.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP14.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP14.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP14.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP14.Value),
                                                                                                                    chkTankCleanedP14.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP14.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP14.Text),
                                                                                                                    General.GetNullableGuid(lblProductP14.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP14.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP14.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP14.Text
                                                                                                                    );
                        }

                        if (!spnPI14.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI14.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI14.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI14.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI14.Value),
                                                                                                                    chkTankCleanedPI14.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI14.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI14.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI14.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI14.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI14.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI14.Text
                                                                                                                    );
                        }

                        if (!spnC14.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC14.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC14.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC14.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC14.Value),
                                                                                                                    chkTankCleanedC14.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC14.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC14.Text),
                                                                                                                    General.GetNullableGuid(lblProductC14.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC14.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC14.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC14.Text
                                                                                                                    );
                        }

                        if (!spnSI14.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI14.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI14.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI14.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI14.Value),
                                                                                                                    chkTankCleanedSI14.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI14.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI14.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI14.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI14.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI14.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI14.Text
                                                                                                                    );
                        }
                        if (!spnS14.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS14.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS14.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS14.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS14.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS14.Value),
                                                                                                                    chkTankCleanedS14.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS14.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS14.Text),
                                                                                                                    General.GetNullableGuid(lblProductS14.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS14.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertS14.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS14.Text
                                                                                                                    );
                        }

                    }
                    if (spn15.Visible)
                    {
                        if (!spnP15.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDP15.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedP15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTP15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMP15.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionP15.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDP15.Value),
                                                                                                                    chkTankCleanedP15.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateP15.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityP15.Text),
                                                                                                                    General.GetNullableGuid(lblProductP15.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableP15.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertP15.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksP15.Text
                                                                                                                    );
                        }

                        if (!spnPI15.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDPI15.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedPI15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTPI15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMPI15.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionPI15.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDPI15.Value),
                                                                                                                    chkTankCleanedPI15.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDatePI15.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityPI15.Text),
                                                                                                                    General.GetNullableGuid(lblProductPI15.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpablePI15.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertPI15.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksPI15.Text
                                                                                                                    );
                        }

                        if (!spnC15.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDC15.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedC15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTC15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMC15.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionC15.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDC15.Value),
                                                                                                                    chkTankCleanedC15.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateC15.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityC15.Text),
                                                                                                                    General.GetNullableGuid(lblProductC15.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableC15.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertC15.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksC15.Text
                                                                                                                    );
                        }

                        if (!spnSI15.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDSI15.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedSI15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTSI15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMSI15.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionSI15.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDSI15.Value),
                                                                                                                    chkTankCleanedSI15.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateSI15.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravitySI15.Text),
                                                                                                                    General.GetNullableGuid(lblProductSI15.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableSI15.Checked == true ? "1" : "0")
                                                                                                                    , General.GetNullableInteger(chkpostponealertSI15.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksSI15.Text
                                                                                                                    );
                        }
                        if (!spnS15.Disabled)
                        {
                            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                                    General.GetNullableGuid(hdnMidnightReportIDS15.Value),
                                                                                                                    General.GetNullableDateTime(ucDateLoadedS15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBMTS15.Text),
                                                                                                                    General.GetNullableDecimal(ucROBCUMS15.Text),
                                                                                                                    General.GetNullableGuid(hdnConsumptionS15.Value),
                                                                                                                    General.GetNullableGuid(hdnConfiguratoinIDS15.Value),
                                                                                                                    chkTankCleanedS15.Checked.Value ? 1 : 0,
                                                                                                                    General.GetNullableDateTime(ucDateS15.Text),
                                                                                                                    General.GetNullableDecimal(lblSpcGravityS15.Text),
                                                                                                                    General.GetNullableGuid(lblProductS15.SelectedProduct),
                                                                                                                    General.GetNullableInteger(chkUnpumpableS15.Checked == true ? "1" : "0")
                                                                                                                    ,General.GetNullableInteger(chkpostponealertS15.Checked == true ? "1" : "0")
                                                                                                                    , txtpostponeexpremarksS15.Text
                                                                                                                    );
                        }

                    }

                    ucStatus.Text = "Updated Successfully.";
                    ucStatus.Visible = true;
                    Response.Redirect("CrewOffshoreDMRMidNightReportTankPlan.aspx", false);
                }
                else
                {
                    ucError.ErrorMessage = "You cannot save. Please save the Midnight Report first.";
                    ucError.Visible = true;
                    return;
                }


            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    String scriptpopup = String.Format(
                   "javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=DMRTANKPLAN&midNightReportId=" + Session["MIDNIGHTREPORTID"].ToString()
                                                    + "&VesselID=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "Midnight Report is not yet Created.";
                    ucError.Visible = true;
                }
            }

            DisableControls();
            BindOilLoadandConsumption();
            BindDryBulkCargoSummary();
            BindLiquidBulkCargoSummary();
            BindMethanolSummary();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkTankCleanedP1_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP1.Checked == true)
        {
            ucDateP1.Enabled = true;
            ucDateP1.Text = txtDate.Text;
        }
        if (chkTankCleanedP1.Checked == false)
        {
            ucDateP1.Enabled = false;
            ucDateP1.Text = "";
        }
    }
    protected void chkTankCleanedPI1_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI1.Checked == true)
        {
            ucDatePI1.Enabled = true;
            ucDatePI1.Text = txtDate.Text;
        }
        if (chkTankCleanedPI1.Checked == false)
        {
            ucDatePI1.Enabled = false;
            ucDatePI1.Text = "";
        }
    }
    protected void chkTankCleanedC1_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC1.Checked == true)
        {
            ucDateC1.Enabled = true;
            ucDateC1.Text = txtDate.Text;
        }
        if (chkTankCleanedC1.Checked == false)
        {
            ucDateC1.Enabled = false;
            ucDateC1.Text = "";
        }
    }
    protected void chkTankCleanedSI1_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI1.Checked == true)
        {
            ucDateSI1.Enabled = true;
            ucDateSI1.Text = txtDate.Text;
        }
        if (chkTankCleanedSI1.Checked == false)
        {
            ucDateSI1.Enabled = false;
            ucDateSI1.Text = "";
        }
    }
    protected void chkTankCleanedS1_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS1.Checked == true)
        {
            ucDateS1.Enabled = true;
            ucDateS1.Text = txtDate.Text;
        }
        if (chkTankCleanedS1.Checked == false)
        {
            ucDateS1.Enabled = false;
            ucDateS1.Text = "";
        }
    }

    protected void chkTankCleanedP2_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP2.Checked == true)
        {
            ucDateP2.Enabled = true;
            ucDateP2.Text = txtDate.Text;
        }
        if (chkTankCleanedP2.Checked == false)
        {
            ucDateP2.Enabled = false;
            ucDateP2.Text = "";
        }
    }
    protected void chkTankCleanedPI2_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI2.Checked == true)
        {
            ucDatePI2.Enabled = true;
            ucDatePI2.Text = txtDate.Text;
        }
        if (chkTankCleanedPI2.Checked == false)
        {
            ucDatePI2.Enabled = false;
            ucDatePI2.Text = "";
        }
    }
    protected void chkTankCleanedC2_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC2.Checked == true)
        {
            ucDateC2.Enabled = true;
            ucDateC2.Text = txtDate.Text;
        }
        if (chkTankCleanedC2.Checked == false)
        {
            ucDateC2.Enabled = false;
            ucDateC2.Text = "";
        }
    }
    protected void chkTankCleanedSI2_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI2.Checked == true)
        {
            ucDateSI2.Enabled = true;
            ucDateSI2.Text = txtDate.Text;
        }
        if (chkTankCleanedSI2.Checked == false)
        {
            ucDateSI2.Enabled = false;
            ucDateSI2.Text = "";
        }
    }
    protected void chkTankCleanedS2_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS2.Checked == true)
        {
            ucDateS2.Enabled = true;
            ucDateS2.Text = txtDate.Text;
        }
        if (chkTankCleanedS2.Checked == false)
        {
            ucDateS2.Enabled = false;
            ucDateS2.Text = "";
        }
    }

    protected void chkTankCleanedP3_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP3.Checked == true)
        {
            ucDateP3.Enabled = true;
            ucDateP3.Text = txtDate.Text;
        }
        if (chkTankCleanedP3.Checked == false)
        {
            ucDateP3.Enabled = false;
            ucDateP3.Text = "";
        }
    }
    protected void chkTankCleanedPI3_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI3.Checked == true)
        {
            ucDatePI3.Enabled = true;
            ucDatePI3.Text = txtDate.Text;
        }
        if (chkTankCleanedPI3.Checked == false)
        {
            ucDatePI3.Enabled = false;
            ucDatePI3.Text = "";
        }
    }
    protected void chkTankCleanedC3_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC3.Checked == true)
        {
            ucDateC3.Enabled = true;
            ucDateC3.Text = txtDate.Text;
        }
        if (chkTankCleanedC3.Checked == false)
        {
            ucDateC3.Enabled = false;
            ucDateC3.Text = "";
        }
    }
    protected void chkTankCleanedSI3_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI3.Checked == true)
        {
            ucDateSI3.Enabled = true;
            ucDateSI3.Text = txtDate.Text;
        }
        if (chkTankCleanedSI3.Checked == false)
        {
            ucDateSI3.Enabled = false;
            ucDateSI3.Text = "";
        }
    }
    protected void chkTankCleanedS3_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS3.Checked == true)
        {
            ucDateS3.Enabled = true;
            ucDateS3.Text = txtDate.Text;
        }
        if (chkTankCleanedS3.Checked == false)
        {
            ucDateS3.Enabled = false;
            ucDateS3.Text = "";
        }
    }

    protected void chkTankCleanedP4_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP4.Checked == true)
        {
            ucDateP4.Enabled = true;
            ucDateP4.Text = txtDate.Text;
        }
        if (chkTankCleanedP4.Checked == false)
        {
            ucDateP4.Enabled = false;
            ucDateP4.Text = "";
        }
    }
    protected void chkTankCleanedPI4_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI4.Checked == true)
        {
            ucDatePI4.Enabled = true;
            ucDatePI4.Text = txtDate.Text;
        }
        if (chkTankCleanedPI4.Checked == false)
        {
            ucDatePI4.Enabled = false;
            ucDatePI4.Text = "";
        }
    }
    protected void chkTankCleanedC4_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC4.Checked == true)
        {
            ucDateC4.Enabled = true;
            ucDateC4.Text = txtDate.Text;
        }
        if (chkTankCleanedC4.Checked == false)
        {
            ucDateC4.Enabled = false;
            ucDateC4.Text = "";
        }
    }
    protected void chkTankCleanedSI4_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI4.Checked == true)
        {
            ucDateSI4.Enabled = true;
            ucDateSI4.Text = txtDate.Text;
        }
        if (chkTankCleanedSI4.Checked == false)
        {
            ucDateSI4.Enabled = false;
            ucDateSI4.Text = "";
        }
    }
    protected void chkTankCleanedS4_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS4.Checked == true)
        {
            ucDateS4.Enabled = true;
            ucDateS4.Text = txtDate.Text;
        }
        if (chkTankCleanedS4.Checked == false)
        {
            ucDateS4.Enabled = false;
            ucDateS4.Text = "";
        }
    }

    protected void chkTankCleanedP5_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP5.Checked == true)
        {
            ucDateP5.Enabled = true;
            ucDateP5.Text = txtDate.Text;
        }
        if (chkTankCleanedP5.Checked == false)
        {
            ucDateP5.Enabled = false;
            ucDateP5.Text = "";
        }
    }
    protected void chkTankCleanedPI5_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI5.Checked == true)
        {
            ucDatePI5.Enabled = true;
            ucDatePI5.Text = txtDate.Text;
        }
        if (chkTankCleanedPI5.Checked == false)
        {
            ucDatePI5.Enabled = false;
            ucDatePI5.Text = "";
        }
    }
    protected void chkTankCleanedC5_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC5.Checked == true)
        {
            ucDateC5.Enabled = true;
            ucDateC5.Text = txtDate.Text;
        }
        if (chkTankCleanedC5.Checked == false)
        {
            ucDateC5.Enabled = false;
            ucDateC5.Text = "";
        }
    }
    protected void chkTankCleanedSI5_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI5.Checked == true)
        {
            ucDateSI5.Enabled = true;
            ucDateSI5.Text = txtDate.Text;
        }
        if (chkTankCleanedSI5.Checked == false)
        {
            ucDateSI5.Enabled = false;
            ucDateSI5.Text = "";
        }
    }
    protected void chkTankCleanedS5_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS5.Checked == true)
        {
            ucDateS5.Enabled = true;
            ucDateS5.Text = txtDate.Text;
        }
        if (chkTankCleanedS5.Checked == false)
        {
            ucDateS5.Enabled = false;
            ucDateS5.Text = "";
        }
    }

    protected void chkTankCleanedP6_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP6.Checked == true)
        {
            ucDateP6.Enabled = true;
            ucDateP6.Text = txtDate.Text;
        }
        if (chkTankCleanedP6.Checked == false)
        {
            ucDateP6.Enabled = false;
            ucDateP6.Text = "";
        }
    }
    protected void chkTankCleanedPI6_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI6.Checked == true)
        {
            ucDatePI6.Enabled = true;
            ucDatePI6.Text = txtDate.Text;
        }
        if (chkTankCleanedPI6.Checked == false)
        {
            ucDatePI6.Enabled = false;
            ucDatePI6.Text = "";
        }
    }
    protected void chkTankCleanedC6_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC6.Checked == true)
        {
            ucDateC6.Enabled = true;
            ucDateC6.Text = txtDate.Text;
        }
        if (chkTankCleanedC6.Checked == false)
        {
            ucDateC6.Enabled = false;
            ucDateC6.Text = "";
        }
    }
    protected void chkTankCleanedSI6_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI6.Checked == true)
        {
            ucDateSI6.Enabled = true;
            ucDateSI6.Text = txtDate.Text;
        }
        if (chkTankCleanedSI6.Checked == false)
        {
            ucDateSI6.Enabled = false;
            ucDateSI6.Text = "";
        }
    }
    protected void chkTankCleanedS6_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS6.Checked == true)
        {
            ucDateS6.Enabled = true;
            ucDateS6.Text = txtDate.Text;
        }
        if (chkTankCleanedS6.Checked == false)
        {
            ucDateS6.Enabled = false;
            ucDateS6.Text = "";
        }
    }

    protected void chkTankCleanedP7_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP7.Checked == true)
        {
            ucDateP7.Enabled = true;
            ucDateP7.Text = txtDate.Text;
        }
        if (chkTankCleanedP7.Checked == false)
        {
            ucDateP7.Enabled = false;
            ucDateP7.Text = "";
        }
    }
    protected void chkTankCleanedPI7_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI7.Checked == true)
        {
            ucDatePI7.Enabled = true;
            ucDatePI7.Text = txtDate.Text;
        }
        if (chkTankCleanedPI7.Checked == false)
        {
            ucDatePI7.Enabled = false;
            ucDatePI7.Text = "";
        }
    }
    protected void chkTankCleanedC7_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC7.Checked == true)
        {
            ucDateC7.Enabled = true;
            ucDateC7.Text = txtDate.Text;
        }
        if (chkTankCleanedC7.Checked == false)
        {
            ucDateC7.Enabled = false;
            ucDateC7.Text = "";
        }
    }
    protected void chkTankCleanedSI7_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI7.Checked == true)
        {
            ucDateSI7.Enabled = true;
            ucDateSI7.Text = txtDate.Text;
        }
        if (chkTankCleanedSI7.Checked == false)
        {
            ucDateSI7.Enabled = false;
            ucDateSI7.Text = "";
        }
    }
    protected void chkTankCleanedS7_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS7.Checked == true)
        {
            ucDateS7.Enabled = true;
            ucDateS7.Text = txtDate.Text;
        }
        if (chkTankCleanedS7.Checked == false)
        {
            ucDateS7.Enabled = false;
            ucDateS7.Text = "";
        }
    }

    protected void chkTankCleanedP8_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP8.Checked == true)
        {
            ucDateP8.Enabled = true;
            ucDateP8.Text = txtDate.Text;
        }
        if (chkTankCleanedP8.Checked == false)
        {
            ucDateP8.Enabled = false;
            ucDateP8.Text = "";
        }
    }
    protected void chkTankCleanedPI8_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI8.Checked == true)
        {
            ucDatePI8.Enabled = true;
            ucDatePI8.Text = txtDate.Text;
        }
        if (chkTankCleanedPI8.Checked == false)
        {
            ucDatePI8.Enabled = false;
            ucDatePI8.Text = "";
        }
    }
    protected void chkTankCleanedC8_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC8.Checked == true)
        {
            ucDateC8.Enabled = true;
            ucDateC8.Text = txtDate.Text;
        }
        if (chkTankCleanedC8.Checked == false)
        {
            ucDateC8.Enabled = false;
            ucDateC8.Text = "";
        }
    }
    protected void chkTankCleanedSI8_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI8.Checked == true)
        {
            ucDateSI8.Enabled = true;
            ucDateSI8.Text = txtDate.Text;
        }
        if (chkTankCleanedSI8.Checked == false)
        {
            ucDateSI8.Enabled = false;
            ucDateSI8.Text = "";
        }
    }
    protected void chkTankCleanedS8_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS8.Checked == true)
        {
            ucDateS8.Enabled = true;
            ucDateS8.Text = txtDate.Text;
        }
        if (chkTankCleanedS8.Checked == false)
        {
            ucDateS8.Enabled = false;
            ucDateS8.Text = "";
        }
    }

    protected void chkTankCleanedP9_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP9.Checked == true)
        {
            ucDateP9.Enabled = true;
            ucDateP9.Text = txtDate.Text;
        }
        if (chkTankCleanedP9.Checked == false)
        {
            ucDateP9.Enabled = false;
            ucDateP9.Text = "";
        }
    }
    protected void chkTankCleanedPI9_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI9.Checked == true)
        {
            ucDatePI9.Enabled = true;
            ucDatePI9.Text = txtDate.Text;
        }
        if (chkTankCleanedPI9.Checked == false)
        {
            ucDatePI9.Enabled = false;
            ucDatePI9.Text = "";
        }
    }
    protected void chkTankCleanedC9_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC9.Checked == true)
        {
            ucDateC9.Enabled = true;
            ucDateC9.Text = txtDate.Text;
        }
        if (chkTankCleanedC9.Checked == false)
        {
            ucDateC9.Enabled = false;
            ucDateC9.Text = "";
        }
    }
    protected void chkTankCleanedSI9_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI9.Checked == true)
        {
            ucDateSI9.Enabled = true;
            ucDateSI9.Text = txtDate.Text;
        }
        if (chkTankCleanedSI9.Checked == false)
        {
            ucDateSI9.Enabled = false;
            ucDateSI9.Text = "";
        }
    }
    protected void chkTankCleanedS9_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS9.Checked == true)
        {
            ucDateS9.Enabled = true;
            ucDateS9.Text = txtDate.Text;
        }
        if (chkTankCleanedS9.Checked == false)
        {
            ucDateS9.Enabled = false;
            ucDateS9.Text = "";
        }
    }

    protected void chkTankCleanedP10_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP10.Checked == true)
        {
            ucDateP10.Enabled = true;
            ucDateP10.Text = txtDate.Text;
        }
        if (chkTankCleanedP10.Checked == false)
        {
            ucDateP10.Enabled = false;
            ucDateP10.Text = "";
        }
    }
    protected void chkTankCleanedPI10_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI10.Checked == true)
        {
            ucDatePI10.Enabled = true;
            ucDatePI10.Text = txtDate.Text;
        }
        if (chkTankCleanedPI10.Checked == false)
        {
            ucDatePI10.Enabled = false;
            ucDatePI10.Text = "";
        }
    }
    protected void chkTankCleanedC10_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC10.Checked == true)
        {
            ucDateC10.Enabled = true;
            ucDateC10.Text = txtDate.Text;
        }
        if (chkTankCleanedC10.Checked == false)
        {
            ucDateC10.Enabled = false;
            ucDateC10.Text = "";
        }
    }
    protected void chkTankCleanedSI10_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI10.Checked == true)
        {
            ucDateSI10.Enabled = true;
            ucDateSI10.Text = txtDate.Text;
        }
        if (chkTankCleanedSI10.Checked == false)
        {
            ucDateSI10.Enabled = false;
            ucDateSI10.Text = "";
        }
    }
    protected void chkTankCleanedS10_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS10.Checked == true)
        {
            ucDateS10.Enabled = true;
            ucDateS10.Text = txtDate.Text;
        }
        if (chkTankCleanedS10.Checked == false)
        {
            ucDateS10.Enabled = false;
            ucDateS10.Text = "";
        }
    }

    protected void chkTankCleanedP11_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP11.Checked == true)
        {
            ucDateP11.Enabled = true;
            ucDateP11.Text = txtDate.Text;
        }
        if (chkTankCleanedP11.Checked == false)
        {
            ucDateP11.Enabled = false;
            ucDateP11.Text = "";
        }
    }
    protected void chkTankCleanedPI11_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI11.Checked == true)
        {
            ucDatePI11.Enabled = true;
            ucDatePI11.Text = txtDate.Text;
        }
        if (chkTankCleanedPI11.Checked == false)
        {
            ucDatePI11.Enabled = false;
            ucDatePI11.Text = "";
        }
    }
    protected void chkTankCleanedC11_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC11.Checked == true)
        {
            ucDateC11.Enabled = true;
            ucDateC11.Text = txtDate.Text;
        }
        if (chkTankCleanedC11.Checked == false)
        {
            ucDateC11.Enabled = false;
            ucDateC11.Text = "";
        }
    }
    protected void chkTankCleanedSI11_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI11.Checked == true)
        {
            ucDateSI11.Enabled = true;
            ucDateSI11.Text = txtDate.Text;
        }
        if (chkTankCleanedSI11.Checked == false)
        {
            ucDateSI11.Enabled = false;
            ucDateSI11.Text = "";
        }
    }
    protected void chkTankCleanedS11_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS11.Checked == true)
        {
            ucDateS11.Enabled = true;
            ucDateS11.Text = txtDate.Text;
        }
        if (chkTankCleanedS11.Checked == false)
        {
            ucDateS11.Enabled = false;
            ucDateS11.Text = "";
        }
    }

    protected void chkTankCleanedP12_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP12.Checked == true)
        {
            ucDateP12.Enabled = true;
            ucDateP12.Text = txtDate.Text;
        }
        if (chkTankCleanedP12.Checked == false)
        {
            ucDateP12.Enabled = false;
            ucDateP12.Text = "";
        }
    }
    protected void chkTankCleanedPI12_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI12.Checked == true)
        {
            ucDatePI12.Enabled = true;
            ucDatePI12.Text = txtDate.Text;
        }
        if (chkTankCleanedPI12.Checked == false)
        {
            ucDatePI12.Enabled = false;
            ucDatePI12.Text = "";
        }
    }
    protected void chkTankCleanedC12_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC12.Checked == true)
        {
            ucDateC12.Enabled = true;
            ucDateC12.Text = txtDate.Text;
        }
        if (chkTankCleanedC12.Checked == false)
        {
            ucDateC12.Enabled = false;
            ucDateC12.Text = "";
        }
    }
    protected void chkTankCleanedSI12_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI12.Checked == true)
        {
            ucDateSI12.Enabled = true;
            ucDateSI12.Text = txtDate.Text;
        }
        if (chkTankCleanedSI12.Checked == false)
        {
            ucDateSI12.Enabled = false;
            ucDateSI12.Text = "";
        }
    }
    protected void chkTankCleanedS12_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS12.Checked == true)
        {
            ucDateS12.Enabled = true;
            ucDateS12.Text = txtDate.Text;
        }
        if (chkTankCleanedS12.Checked == false)
        {
            ucDateS12.Enabled = false;
            ucDateS12.Text = "";
        }
    }

    protected void chkTankCleanedP13_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP13.Checked == true)
        {
            ucDateP13.Enabled = true;
            ucDateP13.Text = txtDate.Text;
        }
        if (chkTankCleanedP13.Checked == false)
        {
            ucDateP13.Enabled = false;
            ucDateP13.Text = "";
        }
    }
    protected void chkTankCleanedPI13_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI13.Checked == true)
        {
            ucDatePI13.Enabled = true;
            ucDatePI13.Text = txtDate.Text;
        }
        if (chkTankCleanedPI13.Checked == false)
        {
            ucDatePI13.Enabled = false;
            ucDatePI13.Text = "";
        }
    }
    protected void chkTankCleanedC13_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC13.Checked == true)
        {
            ucDateC13.Enabled = true;
            ucDateC13.Text = txtDate.Text;
        }
        if (chkTankCleanedC13.Checked == false)
        {
            ucDateC13.Enabled = false;
            ucDateC13.Text = "";
        }
    }
    protected void chkTankCleanedSI13_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI13.Checked == true)
        {
            ucDateSI13.Enabled = true;
            ucDateSI13.Text = txtDate.Text;
        }
        if (chkTankCleanedSI13.Checked == false)
        {
            ucDateSI13.Enabled = false;
            ucDateSI13.Text = "";
        }
    }
    protected void chkTankCleanedS13_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS13.Checked == true)
        {
            ucDateS13.Enabled = true;
            ucDateS13.Text = txtDate.Text;
        }
        if (chkTankCleanedS13.Checked == false)
        {
            ucDateS13.Enabled = false;
            ucDateS13.Text = "";
        }
    }

    protected void chkTankCleanedP14_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP14.Checked == true)
        {
            ucDateP14.Enabled = true;
            ucDateP14.Text = txtDate.Text;
        }
        if (chkTankCleanedP14.Checked == false)
        {
            ucDateP14.Enabled = false;
            ucDateP14.Text = "";
        }
    }
    protected void chkTankCleanedPI14_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI14.Checked == true)
        {
            ucDatePI14.Enabled = true;
            ucDatePI14.Text = txtDate.Text;
        }
        if (chkTankCleanedPI14.Checked == false)
        {
            ucDatePI14.Enabled = false;
            ucDatePI14.Text = "";
        }
    }
    protected void chkTankCleanedC14_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC14.Checked == true)
        {
            ucDateC14.Enabled = true;
            ucDateC14.Text = txtDate.Text;
        }
        if (chkTankCleanedC14.Checked == false)
        {
            ucDateC14.Enabled = false;
            ucDateC14.Text = "";
        }
    }
    protected void chkTankCleanedSI14_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI14.Checked == true)
        {
            ucDateSI14.Enabled = true;
            ucDateSI14.Text = txtDate.Text;
        }
        if (chkTankCleanedSI14.Checked == false)
        {
            ucDateSI14.Enabled = false;
            ucDateSI14.Text = "";
        }
    }
    protected void chkTankCleanedS14_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS14.Checked == true)
        {
            ucDateS14.Enabled = true;
            ucDateS14.Text = txtDate.Text;
        }
        if (chkTankCleanedS14.Checked == false)
        {
            ucDateS14.Enabled = false;
            ucDateS14.Text = "";
        }
    }

    protected void chkTankCleanedP15_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedP15.Checked == true)
        {
            ucDateP15.Enabled = true;
            ucDateP15.Text = txtDate.Text;
        }
        if (chkTankCleanedP15.Checked == false)
        {
            ucDateP15.Enabled = false;
            ucDateP15.Text = "";
        }
    }
    protected void chkTankCleanedPI15_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedPI15.Checked == true)
        {
            ucDatePI15.Enabled = true;
            ucDatePI15.Text = txtDate.Text;
        }
        if (chkTankCleanedPI15.Checked == false)
        {
            ucDatePI15.Enabled = false;
            ucDatePI15.Text = "";
        }
    }
    protected void chkTankCleanedC15_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedC15.Checked == true)
        {
            ucDateC15.Enabled = true;
            ucDateC15.Text = txtDate.Text;
        }
        if (chkTankCleanedC15.Checked == false)
        {
            ucDateC15.Enabled = false;
            ucDateC15.Text = "";
        }
    }
    protected void chkTankCleanedSI15_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedSI15.Checked == true)
        {
            ucDateSI15.Enabled = true;
            ucDateSI15.Text = txtDate.Text;
        }
        if (chkTankCleanedSI15.Checked == false)
        {
            ucDateSI15.Enabled = false;
            ucDateSI15.Text = "";
        }
    }
    protected void chkTankCleanedS15_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkTankCleanedS15.Checked == true)
        {
            ucDateS15.Enabled = true;
            ucDateS15.Text = txtDate.Text;
        }
        if (chkTankCleanedS15.Checked == false)
        {
            ucDateS15.Enabled = false;
            ucDateS15.Text = "";
        }
    }

    protected void gvDryBulk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDryBulkCargoSummary();
    }

    protected void gvDryBulk_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }

    protected void gvLiquidBulk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLiquidBulkCargoSummary();
    }

    protected void gvMethanol_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMethanolSummary();
    }
}
