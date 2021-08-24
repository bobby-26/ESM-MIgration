using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionAuditScheduleGeneral : PhoenixBasePage
{
    public int? defaultauditytpe = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
                MenuInspectionScheduleGeneral.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Audit / Inspection List", "LIST");
                toolbar.AddButton("Schedule", "SCHEDULE");
                toolbar.AddButton("Plan", "PLAN");
                toolbar.AddButton("Audit / Inspection Plan", "AUDITPLAN");
                MenuScheduleGeneral.AccessRights = this.ViewState;
                MenuScheduleGeneral.MenuList = toolbar.Show();
                MenuScheduleGeneral.SelectedMenuIndex = 1;

                if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                else
                    Reset();

                ViewState["INTERNALREVIEWSCHEDULEID"] = null;
                ViewState["EXTERNALREVIEWSCHEDULEID"] = null;
                ViewState["FLDINTERIMAUDITID"] = null;
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                if (defaultauditytpe != null)
                    ucAudit.InspectionType = defaultauditytpe.ToString();
                ucInternalAudit.InspectionType = defaultauditytpe.ToString();
                ucInternalAudit.InspectionCategory = PhoenixCommonRegisters.GetHardCode(1, 144, "INT");
                ucExternalAudit.InspectionType = defaultauditytpe.ToString();
                ucExternalAudit.InspectionCategory = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");
                BindShortCodeList();
                BindAuditSchedule();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void BindShortCodeList()
    {
        int? defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
        DataSet ds = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
            General.GetNullableInteger(ucAuditCategory.SelectedHard), 
            General.GetNullableInteger(ucExternalAuditType.SelectedHard),
            General.GetNullableInteger(ucVessel.SelectedVessel));
        ddlAuditShortCodeList.DataSource = ds;
        ddlAuditShortCodeList.DataTextField = "FLDSHORTCODE";
        ddlAuditShortCodeList.DataValueField = "FLDINSPECTIONID";
        ddlAuditShortCodeList.DataBind();
        ddlAuditShortCodeList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ds = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")), null,
            General.GetNullableInteger(ucVessel.SelectedVessel));
        ddlInternalAudit.DataSource = ds;
        ddlInternalAudit.DataTextField = "FLDSHORTCODE";
        ddlInternalAudit.DataValueField = "FLDINSPECTIONID";
        ddlInternalAudit.DataBind();
        ddlInternalAudit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ds = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")), 
            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 190, "INI")),
            General.GetNullableInteger(ucVessel.SelectedVessel));
        ddlExternalAudit.DataSource = ds;
        ddlExternalAudit.DataTextField = "FLDSHORTCODE";
        ddlExternalAudit.DataValueField = "FLDINSPECTIONID";
        ddlExternalAudit.DataBind();
        ddlExternalAudit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindAuditSchedule()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                ucAuditType.SelectedHard = dr["FLDREVIEWTYPEID"].ToString();
                ucAuditCategory.SelectedHard = dr["FLDREVIEWCATEGORYID"].ToString();
                ucExternalAuditType.SelectedHard = dr["FLDEXTERNALAUDITTYPE"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucLastPort.SelectedSeaport = dr["FLDLASTPORTOFAUDITINSPECTION"].ToString();

                int? defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
                DataSet ds1 = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
                    General.GetNullableInteger(dr["FLDREVIEWCATEGORYID"].ToString()), 
                    General.GetNullableInteger(dr["FLDEXTERNALAUDITTYPE"].ToString()),
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                ddlAuditShortCodeList.DataSource = ds1;
                ddlAuditShortCodeList.DataTextField = "FLDSHORTCODE";
                ddlAuditShortCodeList.DataValueField = "FLDINSPECTIONID";
                ddlAuditShortCodeList.DataBind();
                ddlAuditShortCodeList.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                ds1 = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
                    General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")), null,
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                ddlInternalAudit.DataSource = ds1;
                ddlInternalAudit.DataTextField = "FLDSHORTCODE";
                ddlInternalAudit.DataValueField = "FLDINSPECTIONID";
                ddlInternalAudit.DataBind();
                ddlInternalAudit.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                ds1 = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
                    General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")), 
                    General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 190, "INI")),
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                ddlExternalAudit.DataSource = ds1;
                ddlExternalAudit.DataTextField = "FLDSHORTCODE";
                ddlExternalAudit.DataValueField = "FLDINSPECTIONID";
                ddlExternalAudit.DataBind();
                ddlExternalAudit.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    txtWindowperiod.CssClass = "input";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = false;
                    chkInterimAudit.Enabled = false;
                }
                else
                {
                    txtWindowperiod.CssClass = "input_mandatory";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = true;
                    chkInterimAudit.Enabled = true;
                }

                if (dr["FLDEXTERNALAUDITTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 190, "INT"))
                    chkInterimAudit.Enabled = true;
                else
                    chkInterimAudit.Enabled = false;

                ucAudit.SelectedValue = dr["FLDREVIEWID"].ToString();
                ddlAuditShortCodeList.SelectedValue = dr["FLDREVIEWID"].ToString();

                if (dr["FLDISAUTOMATIC"] != null && dr["FLDISAUTOMATIC"].ToString() == "1")
                {
                    rdoAutomatic.Checked = true;
                    ucFrequency.Enabled = true;
                    ucFrequencyValue.Enabled = true;
                    txtWindowperiod.Enabled = true;
                    txtDueDate.Enabled = true;
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                }
                else
                {
                    rdoMannual.Checked = true;
                    ucFrequency.Enabled = false;
                    ucFrequencyValue.Enabled = false;
                    txtWindowperiod.Enabled = false;
                    txtDueDate.Enabled = true;
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
                }
                txtLastDoneDate.Text = General.GetDateTimeToString(dr["FLDLASTDONEDATE"].ToString());
                ucFrequencyValue.Text = dr["FLDFREQUENCY"].ToString();
                ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
                ucDoneType.SelectedHard = dr["FLDISFIXED"].ToString();
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null && rdoAutomatic.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
                ucwindowperiodtype.SelectedHard = dr["FLDWINDOWPERIODTYPE"].ToString();
                txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();

                txtRefNo.Enabled = false;
                ucAuditType.Enabled = ucDoneType.Enabled = ucAudit.Enabled = false;
                ViewState["FLDINTERIMAUDITID"] = dr["FLDINTERIMAUDITID"].ToString();
                if (dr["FLDINTERIMAUDITYN"] != null && dr["FLDINTERIMAUDITYN"].ToString() == "1")
                {
                    chkInterimAudit.Checked = true;
                    ViewState["INTERIMAUDITID"] = dr["FLDREVIEWSCHEDULEID"].ToString();
                    BindInterimAudit();
                    pnlInternalAudit.Enabled = true;
                    pnlExternalAudit.Enabled = true;
                }
                else
                {
                    ViewState["INTERIMAUDITID"] = null;
                    chkInterimAudit.Checked = false;
                    pnlInternalAudit.Enabled = false;
                    pnlExternalAudit.Enabled = false;
                }

                chkISPSAudit.Checked = false;
                chkISPSAudit.Enabled = false;
            }
        }
    }

    private void BindInterimAudit()
    {
        if (ViewState["INTERIMAUDITID"] != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixInspectionAuditSchedule.EditInterimAuditSchedule(new Guid(ViewState["INTERIMAUDITID"].ToString()),
                int.Parse(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")));
            if (dt.Rows.Count > 0)
            {
                txtInternalAuditRefNo.Text = dt.Rows[0]["FLDREFERENCENUMBER"].ToString();
                ucInternalAudit.SelectedInspection = dt.Rows[0]["FLDINSPECTIONID"].ToString();
                txtInternalAuditDueDate.Text = dt.Rows[0]["FLDREVIEWDATE"].ToString();
                ddlInternalAudit.SelectedValue = dt.Rows[0]["FLDINSPECTIONID"].ToString();
                ViewState["INTERNALREVIEWSCHEDULEID"] = dt.Rows[0]["FLDREVIEWSCHEDULEID"].ToString();
            }
            dt = PhoenixInspectionAuditSchedule.EditInterimAuditSchedule(new Guid(ViewState["INTERIMAUDITID"].ToString()),
                int.Parse(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
            if (dt.Rows.Count > 0)
            {
                txtExternalAuditRefNo.Text = dt.Rows[0]["FLDREFERENCENUMBER"].ToString();
                ucExternalFrequencyValue.Text = dt.Rows[0]["FLDFREQUENCY"].ToString();
                ucExternalFrequency.SelectedHard = dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();
                ucExternalAudit.SelectedInspection = dt.Rows[0]["FLDINSPECTIONID"].ToString();
                ucExternalDoneType.SelectedHard = dt.Rows[0]["FLDISFIXED"].ToString();
                txtExternalAuditDueDate.Text = dt.Rows[0]["FLDREVIEWDATE"].ToString();
                txtExternalWindowperiod.Text = dt.Rows[0]["FLDWINDOWPERIOD"].ToString();
                ucExternalwindowperiodtype.SelectedHard = dt.Rows[0]["FLDWINDOWPERIODTYPE"].ToString();
                ddlExternalAudit.SelectedValue = dt.Rows[0]["FLDINSPECTIONID"].ToString();
                ViewState["EXTERNALREVIEWSCHEDULEID"] = dt.Rows[0]["FLDREVIEWSCHEDULEID"].ToString();
            }
        }
    }

    protected void MenuScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PLAN"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordGeneral.aspx?scheduleid=" + ViewState["AUDITSCHEDULEID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("AUDITPLAN"))
            {
                Response.Redirect("../Inspection/InspectionAuditReviewProgram.aspx?scheduleid=" + ViewState["AUDITSCHEDULEID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordList.aspx?callfrom=record");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuInspectionScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string list = ddlAuditShortCodeList.SelectedItem.ToString();
                DateTime? duedate = null;
                DateTime? lastdonedate = General.GetNullableDateTime(txtLastDoneDate.Text);
                double frequency;
                DataTable dt = new DataTable();

                if (IsValidAuditSchedule())
                {
                    if (rdoAutomatic.Checked)
                    {
                        frequency = double.Parse(ucFrequencyValue.Text);

                        if (ucFrequency.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                        {
                            duedate = General.GetNullableDateTime(txtLastDoneDate.Text) != null ? DateTime.Parse(txtLastDoneDate.Text).AddDays(frequency) : General.GetNullableDateTime(txtDueDate.Text);
                        }
                        if (ucFrequency.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 7, "MON"))
                        {
                            duedate = General.GetNullableDateTime(txtLastDoneDate.Text) != null ? DateTime.Parse(txtLastDoneDate.Text).AddMonths(int.Parse(frequency.ToString())) : General.GetNullableDateTime(txtDueDate.Text);
                        }
                        if (ucFrequency.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 7, "WEE"))
                        {
                            int dayss = Convert.ToInt32(7 * int.Parse(ucFrequencyValue.Text));
                            duedate = General.GetNullableDateTime(txtLastDoneDate.Text) != null ? DateTime.Parse(txtLastDoneDate.Text).AddDays(dayss) : General.GetNullableDateTime(txtDueDate.Text);
                        }
                    }
                    else
                        duedate = DateTime.Parse(txtDueDate.Text);
                    DateTime d = DateTime.Parse(duedate.ToString());

                    if (chkInterimAudit.Checked)
                    {
                        int months = 3;
                        txtInternalAuditDueDate.Text = String.Format("{0:dd/MMM/yyyy}", d.AddMonths(months));

                        months = 6;
                        txtExternalAuditDueDate.Text = String.Format("{0:dd/MMM/yyyy}", d.AddMonths(months));
                    }
                    string insid = "";
                    if (ddlAuditShortCodeList.SelectedValue == "IMS + ISPS")
                    {
                        dt = PhoenixInspectionAuditSchedule.GetInspectionID(
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"))
                                , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                , null
                                , "IMS"
                                , d
                                , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                , General.GetNullableInteger(ucVessel.SelectedVessel));
                    }
                    else
                    {
                        dt = PhoenixInspectionAuditSchedule.GetInspectionID(
                                General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"))
                                , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                , null
                                , ddlAuditShortCodeList.SelectedValue
                                , d
                                , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                , General.GetNullableInteger(ucVessel.SelectedVessel));
                    }
                    insid = dt.Rows[0]["FLDINSPECTIONID"].ToString();

                    DateTime? auditduedate;
                    if ((rdoAutomatic.Checked == true && General.GetNullableDateTime(txtLastDoneDate.Text) == null) || rdoAutomatic.Checked == false)
                        auditduedate = DateTime.Parse(txtDueDate.Text);
                    else
                        auditduedate = null;

                    PhoenixInspectionAuditSchedule.UpdateAuditScheduleDetails(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
                        , new Guid(insid)
                        , Int16.Parse(ucVessel.SelectedVessel)
                        , rdoAutomatic.Checked == true ? 1 : 0
                        , General.GetNullableDateTime(txtLastDoneDate.Text)
                        , General.GetNullableInteger(ucFrequencyValue.Text)
                        , General.GetNullableInteger(ucFrequency.SelectedHard)
                        , General.GetNullableInteger(ucDoneType.SelectedHard)
                        , auditduedate
                        , General.GetNullableInteger(txtWindowperiod.Text)
                        , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                        , General.GetNullableInteger(chkInterimAudit.Checked ? "1" : "0")
                        , General.GetNullableGuid(ViewState["FLDINTERIMAUDITID"].ToString())
                        , General.GetNullableString(txtRemarks.Text)
                        , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                        );


                    if (chkInterimAudit.Checked)
                    {
                        //Schedule an Internal audit along with interim audit
                        //Guid? newinsertedid2 = null;
                        dt = PhoenixInspectionAuditSchedule.GetInspectionID(
                            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"))
                            , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                            , null
                            , ddlInternalAudit.SelectedValue
                            , d
                            , null
                            , General.GetNullableInteger(ucVessel.SelectedVessel));
                        insid = dt.Rows[0]["FLDINSPECTIONID"].ToString();

                        if (ViewState["INTERNALREVIEWSCHEDULEID"] == null) //insert
                        {
                            //PhoenixInspectionAuditSchedule.InsertAuditSchedule(
                            //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            //    , new Guid(insid)
                            //    , Int16.Parse(ucVessel.SelectedVessel)
                            //    , 0 //manual
                            //    , null
                            //    , null
                            //    , null
                            //    , null
                            //    , 0
                            //    , null
                            //    , null
                            //    , General.GetNullableDateTime(txtInternalAuditDueDate.Text)
                            //    , 0 //window period is not required for manual audit
                            //    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                            //    , 0
                            //    , General.GetNullableGuid(ViewState["AUDITSCHEDULEID"].ToString())
                            //    , ref newinsertedid2
                            //    , General.GetNullableString(txtRemarks.Text)
                            //    , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                            //    );
                        }
                        else //update
                        {
                            PhoenixInspectionAuditSchedule.UpdateAuditScheduleDetails(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(ViewState["INTERNALREVIEWSCHEDULEID"].ToString())
                                , new Guid(insid)
                                , Int16.Parse(ucVessel.SelectedVessel)
                                , 0 //manual
                                , null
                                , null
                                , null
                                , null
                                , General.GetNullableDateTime(txtInternalAuditDueDate.Text)
                                , 0 //window period is not required for manual audit
                                , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                                , 0
                                , General.GetNullableGuid(ViewState["INTERIMAUDITID"].ToString())
                                , General.GetNullableString(txtRemarks.Text)
                                , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                                );
                        }

                        //Guid? newinsertedid3 = null;
                        dt = PhoenixInspectionAuditSchedule.GetInspectionID(
                            General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"))
                            , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
                            , null
                            , ddlExternalAudit.SelectedValue
                            , d
                            , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 190, "INI"))
                            , General.GetNullableInteger(ucVessel.SelectedVessel));
                        insid = dt.Rows[0]["FLDINSPECTIONID"].ToString();
                        if (ViewState["EXTERNALREVIEWSCHEDULEID"] == null) //insert
                        {
                            //PhoenixInspectionAuditSchedule.InsertAuditSchedule(
                            //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            //    , new Guid(insid)
                            //    , Int16.Parse(ucVessel.SelectedVessel)
                            //    , 1 //automatic
                            //    , null
                            //    , General.GetNullableInteger(ucExternalFrequencyValue.Text)
                            //    , General.GetNullableInteger(ucExternalFrequency.SelectedHard)
                            //    , General.GetNullableInteger(ucExternalDoneType.SelectedHard)
                            //    , 0
                            //    , null
                            //    , null
                            //    , General.GetNullableDateTime(txtExternalAuditDueDate.Text)
                            //    , General.GetNullableInteger(txtExternalWindowperiod.Text)
                            //    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                            //    , 0
                            //    , General.GetNullableGuid(ViewState["AUDITSCHEDULEID"].ToString())
                            //    , ref newinsertedid3
                            //    , General.GetNullableString(txtRemarks.Text)
                            //    , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                            //    );
                        }
                        else //update
                        {
                            PhoenixInspectionAuditSchedule.UpdateAuditScheduleDetails(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(ViewState["EXTERNALREVIEWSCHEDULEID"].ToString())
                                , new Guid(insid)
                                , Int16.Parse(ucVessel.SelectedVessel)
                                , 1 // automatic
                                , null
                                , General.GetNullableInteger(ucExternalFrequencyValue.Text)
                                , General.GetNullableInteger(ucExternalFrequency.SelectedHard)
                                , General.GetNullableInteger(ucExternalDoneType.SelectedHard)
                                , General.GetNullableDateTime(txtExternalAuditDueDate.Text)
                                , General.GetNullableInteger(txtExternalWindowperiod.Text)
                                , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                                , 0
                                , General.GetNullableGuid(ViewState["INTERIMAUDITID"].ToString())
                                , General.GetNullableString(txtRemarks.Text)
                                , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                                );
                        }
                    }
                    BindAuditSchedule();
                    ucStatus.Text = "Schedule details updated successfully";
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }

            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidAuditSchedule()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        //string timeofincident = txtTimeOfIncident.Text.Trim() == "__:__" ? string.Empty : txtTimeOfIncident.Text;       

        if (ucAuditType.SelectedHard.Equals("") || ucAuditType.SelectedHard.Equals("Dummy"))
            ucError.ErrorMessage = "Type is required.";

        if (ucAuditCategory.SelectedHard.Equals("") || ucAuditCategory.SelectedHard.Equals("Dummy"))
            ucError.ErrorMessage = "Category is required.";

        //if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        //{
        //    if (ucExternalAuditType.SelectedHard.Equals("") || ucExternalAuditType.SelectedHard.Equals("Dummy"))
        //        ucError.ErrorMessage = "External Audit Type is required.";
        //}

        //if (ucAudit.SelectedValue.Equals("") || ucAudit.SelectedValue.Equals("Dummy"))
        //    ucError.ErrorMessage = "Audit name is required.";

        if (ddlAuditShortCodeList.SelectedIndex == 0)
            ucError.ErrorMessage = "Audit / Inspection is required.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "'Vessel' is required.";

        if (rdoAutomatic.Checked == true)
        {
            if ((General.GetNullableDateTime(txtLastDoneDate.Text).Equals("") || General.GetNullableDateTime(txtLastDoneDate.Text).Equals(null)) &&
                (General.GetNullableDateTime(txtDueDate.Text).Equals("") || General.GetNullableDateTime(txtDueDate.Text).Equals(null)))
                ucError.ErrorMessage = "Either 'Last done date' or 'Due date' is required.";

            if (General.GetNullableDateTime(txtLastDoneDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Last done date should not be the future date.";

            if (ucFrequencyValue.Text.Equals(""))
                ucError.ErrorMessage = "Frequency value is required.";

            if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
            {
                if (txtWindowperiod.Text.Equals(""))
                    ucError.ErrorMessage = "Window Period value is required.";
            }
        }
        else if (rdoMannual.Checked == true)
        {
            if (General.GetNullableDateTime(txtDueDate.Text).Equals("") || General.GetNullableDateTime(txtDueDate.Text).Equals(null))
                ucError.ErrorMessage = "Due date is required.";
        }
        if (chkInterimAudit.Checked)
        {
            if (ddlInternalAudit.SelectedIndex == 0)
                ucError.ErrorMessage = "Internal Audit name is required.";
            if (ddlExternalAudit.SelectedIndex == 0)
                ucError.ErrorMessage = "Initial Audit name is required.";
            if (ucExternalFrequencyValue.Text.Equals(""))
                ucError.ErrorMessage = "Frequency value is required for Initial Audit.";
            if (txtExternalWindowperiod.Text.Equals(""))
                ucError.ErrorMessage = "Window Period value is required for Initial Audit.";
            if (ucExternalwindowperiodtype.SelectedHard.Equals("") || ucExternalwindowperiodtype.SelectedHard.Equals("Dummy"))
                ucError.ErrorMessage = "Window Period Type is required for Initial Audit.";
        }

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["AUDITSCHEDULEID"] = null;
        txtDueDate.Text = txtRefNo.Text = txtLastDoneDate.Text = ucFrequencyValue.Text = "";
        ucAuditCategory.SelectedHard = ucAudit.SelectedValue = "";
        rdoMannual.Checked = false;
        rdoAutomatic.Checked = true;
        txtLastDoneDate.Enabled = ucFrequencyValue.Enabled = txtDueDate.Enabled = true;
        ucAuditType.Enabled = ucAuditCategory.Enabled = ucFrequency.Enabled = ucAudit.Enabled = true;
        ucExternalAuditType.Enabled = true;
        ucExternalAuditType.SelectedHard = "";
        //ucAudit.CssClass = "dropdown_mandatory";
        Panel1.Enabled = true;
        txtWindowperiod.Text = "";
        txtWindowperiod.Enabled = true;
        ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "FIX");
        ucFrequency.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 7, "DAI");
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedVessel = "";
            ucVessel.Enabled = true;
        }
        chkInterimAudit.Checked = false;
        chkInterimAudit.Enabled = true;
        chkISPSAudit.Checked = false;
        chkISPSAudit.Enabled = false;
        ddlAuditShortCodeList.SelectedIndex = -1;
        ddlAuditShortCodeList.Enabled = true;
        ddlInternalAudit.SelectedIndex = -1;
        ddlExternalAudit.SelectedIndex = -1;
        pnlInternalAudit.Enabled = false;
        pnlExternalAudit.Enabled = false;
        txtLastDoneDate.Enabled = true;
        txtRemarks.Text = "";
        ucLastPort.SelectedSeaport = "";
        txtSerialNumber.Text = "";
    }

    protected void InspectionType_Changed(object sender, EventArgs e)
    {
        if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "INT")))
        {
            ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "VAR");
            ucDoneType.Enabled = false;
            txtWindowperiod.CssClass = "input";

            ucExternalAuditType.Enabled = false;
            ucExternalAuditType.SelectedHard = "";
            //ucExternalAuditType.CssClass = "input";
            chkInterimAudit.Enabled = false;
        }
        else if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "EXT")))
        {
            ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "FIX");
            ucDoneType.Enabled = false;
            txtWindowperiod.CssClass = "input_mandatory";

            ucExternalAuditType.Enabled = true;
            //ucExternalAuditType.CssClass = "input";
            chkInterimAudit.Enabled = true;
        }

        ucAudit.ExternalAuditType = ucExternalAuditType.SelectedHard;
        ucAudit.InspectionList = PhoenixInspection.ListInspection(
            General.GetNullableInteger(ucAuditType.SelectedHard) == null ? defaultauditytpe : General.GetNullableInteger(ucAuditType.SelectedHard)
            , General.GetNullableInteger(ucAuditCategory.SelectedHard) == null ? 0 : General.GetNullableInteger(ucAuditCategory.SelectedHard)
            , General.GetNullableInteger(ucExternalAuditType.SelectedHard));
        BindShortCodeList();
    }

    protected void ExternalAuditType_Changed(object sender, EventArgs e)
    {
        ucAudit.ExternalAuditType = ucExternalAuditType.SelectedHard;
        ucAudit.InspectionList = PhoenixInspection.ListInspection(
            General.GetNullableInteger(ucAuditType.SelectedHard) == null ? defaultauditytpe : General.GetNullableInteger(ucAuditType.SelectedHard)
            , General.GetNullableInteger(ucAuditCategory.SelectedHard) == null ? 0 : General.GetNullableInteger(ucAuditCategory.SelectedHard)
            , General.GetNullableInteger(ucExternalAuditType.SelectedHard));
        BindShortCodeList();

        if (ucExternalAuditType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 190, "INT"))
        {
            chkInterimAudit.Enabled = true;
            chkInterimAudit.Checked = true;
            pnlInternalAudit.Enabled = true;
            pnlExternalAudit.Enabled = true;
        }
        else
        {
            chkInterimAudit.Enabled = false;
            chkInterimAudit.Checked = false;
            pnlInternalAudit.Enabled = false;
            pnlExternalAudit.Enabled = false;
        }
    }

    protected void rdoAutomatic_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoMannual.Checked == true)
        {
            txtDueDate.CssClass = "input_mandatory";
            txtDueDate.Enabled = true;
            //txtDueDate.ReadOnly = false;
            txtDueDate.DatePicker = true;
            txtLastDoneDate.Text = "";
            txtLastDoneDate.DatePicker = false;
            ucFrequencyValue.Text = "";
            txtLastDoneDate.CssClass = "readonlytextbox";
            ucFrequency.Enabled = false;
            ucFrequency.CssClass = "readonlytextbox";
            ucFrequencyValue.Enabled = false;
            ucFrequencyValue.CssClass = "readonlytextbox";
            ucDoneType.Enabled = false;
            ucDoneType.CssClass = "readonlytextbox";
            txtWindowperiod.Text = "";
            txtWindowperiod.Enabled = false;
            txtWindowperiod.CssClass = "readonlytextbox";
            BindManualDetails();
        }
        else if (rdoAutomatic.Checked == true)
        {
            txtDueDate.Text = "";
            txtDueDate.CssClass = "input";
            //txtDueDate.Enabled = false;
            //txtDueDate.ReadOnly = true;
            //txtDueDate.DatePicker = false;
            txtLastDoneDate.CssClass = "input";
            txtLastDoneDate.DatePicker = true;
            txtLastDoneDate.Enabled = true;
            ucFrequency.Enabled = true;
            ucFrequency.CssClass = "input_mandatory";
            ucFrequencyValue.Enabled = true;
            ucFrequencyValue.CssClass = "input_mandatory";
            //ucDoneType.Enabled = true;
            ucDoneType.CssClass = "input_mandatory";
            txtWindowperiod.Text = "";
            txtWindowperiod.Enabled = true;
            txtWindowperiod.CssClass = "input_mandatory";
            BindAutomaticDetails();
        }
    }
    protected void chkInterimAudit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInterimAudit.Checked)
        {
            pnlInternalAudit.Enabled = true;
            pnlExternalAudit.Enabled = true;
        }
        else
        {
            pnlInternalAudit.Enabled = false;
            pnlExternalAudit.Enabled = false;
        }
        ucInternalAudit.SelectedInspection = "";
        ucExternalAudit.SelectedInspection = "";
        ucExternalFrequencyValue.Text = "";
        txtExternalWindowperiod.Text = "";
        ddlInternalAudit.SelectedIndex = 0;
        ddlExternalAudit.SelectedIndex = 0;
    }
    protected void ddlAuditShortCodeList_TextChanged(object sender, EventArgs e)
    {
        if (ddlAuditShortCodeList.SelectedValue == "IMS + ISPS")
        {
            chkISPSAudit.Enabled = false;
            chkISPSAudit.Checked = true;
        }
        else
        {
            chkISPSAudit.Enabled = false;
            chkISPSAudit.Checked = false;
        }
    }
    protected void ucVessel_changed(object sender, EventArgs e)
    {
        BindShortCodeList();
    }

    protected void BindAutomaticDetails()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtLastDoneDate.Text = General.GetDateTimeToString(dr["FLDLASTDONEDATE"].ToString());
                ucFrequencyValue.Text = dr["FLDFREQUENCY"].ToString();
                ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
                ucDoneType.SelectedHard = dr["FLDISFIXED"].ToString();
                if (rdoAutomatic.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                if (rdoMannual.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null && rdoAutomatic.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
            }
        }
    }
    protected void BindManualDetails()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (rdoAutomatic.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                if (rdoMannual.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null && rdoAutomatic.Checked)
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
            }
        }
    }
}
