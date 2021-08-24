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
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionDrugAndAlcoholTestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
            MenuInspectionScheduleGeneral.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }


                if (Request.QueryString["DrugAlcoholTestId"] != null)
                {
                    ViewState["DrugAlcoholTestId"] = Request.QueryString["DrugAlcoholTestId"].ToString();
                    EditDrugAndAlcoholTest(new Guid(Request.QueryString["DrugAlcoholTestId"].ToString()));
                }


                ViewState["TYPE"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindInternalOrganization();
                BindExternalOrganization();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void BindInternalOrganization()
    {
        ddlOrganization.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")));
        ddlOrganization.DataTextField = "FLDORGANIZATIONNAME";
        ddlOrganization.DataValueField = "FLDORGANIZATIONID";
        ddlOrganization.DataBind();
        ddlOrganization.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindExternalOrganization()
    {
        ddlExternalOrganizationName.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
        ddlExternalOrganizationName.DataTextField = "FLDORGANIZATIONNAME";
        ddlExternalOrganizationName.DataValueField = "FLDORGANIZATIONID";
        ddlExternalOrganizationName.DataBind();
        ddlExternalOrganizationName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    //protected void ucVessel_Changed(object sender, EventArgs e)
    //{
    //    ViewState["VESSELID"] = ddlVessel.SelectedVessel;

    //}


    private void EditDrugAndAlcoholTest(Guid DrugAlcoholTestId)
    {
        try
        {
            DataSet ds = PhoenixInspectionDrugAndAlcoholTest.EditDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode, DrugAlcoholTestId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucport.SelectedValue = dr["FLDPORTID"].ToString();
                ucport.Text = dr["FLDPORTNAME"].ToString();
                ucAuditCategory.SelectedHard = dr["FLDTYPE"].ToString();
                txtDateofTest.Text = dr["FLDALCOHOLTESTDATE"].ToString();
                chkDrugAndAlcoholTestYN.Checked = dr["FLDDRUGALCOHOLTESTYN"].ToString() == "1" ? true : false;

                if (dr["FLDTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {

                    ddlOrganization.SelectedValue = dr["FLDORGANIZATIONID"].ToString();
                    ddlOrganization.Text = dr["FLDCOMPANY"].ToString();
                    EnableDisableExternal(false, "input");

                    ucInspector.SelectedValue = dr["FLDINSPECTERID"].ToString();
                    ucInspector.Text = dr["FLDINSPECTERNAME"].ToString();
                }
                else
                {
                    EnableDisableInternal(false, "input");

                    ddlExternalOrganizationName.SelectedValue = dr["FLDORGANIZATIONID"].ToString();
                    ddlExternalOrganizationName.Text = dr["FLDCOMPANY"].ToString();

                    txtExternalInspectorName.Text = dr["FLDINSPECTERNAME"].ToString();
                    txtExternalOrganisationName.Text = dr["FLDCOMPANY"].ToString();

                }

                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                ViewState["DRUGALCOHOLTESTID"] = dr["FLDDRUGALCOHOLTESTID"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInspectionScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='DrugAndAlcoholTest'>" + "\n";
            scriptClosePopup += "fnReloadList('City');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='DrugAndAlcoholTestNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {


                if (ViewState["DrugAlcoholTestId"] != null)
                {
                    if (!IsValidDrugAndAlcoholTest())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    if (ucAuditCategory.SelectedHard.ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                    {

                        PhoenixInspectionDrugAndAlcoholTest.UpdateDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["DrugAlcoholTestId"].ToString())
                                                    , int.Parse(ddlVessel.SelectedVessel)
                                                    , chkDrugAndAlcoholTestYN.Checked.Equals(true) ? 1 : 0
                                                    , General.GetNullableDateTime(txtDateofTest.Text)
                                                    , General.GetNullableInteger(ucport.SelectedValue)
                                                    , General.GetNullableString(ucport.Text)
                                                    , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                    , General.GetNullableString(ucInspector.Text)
                                                    , General.GetNullableInteger(ucInspector.SelectedValue)
                                                    , General.GetNullableInteger(ddlOrganization.SelectedValue)
                                                    , General.GetNullableString(ddlOrganization.Text)

                                                    );
                    }
                    else
                    {
                        PhoenixInspectionDrugAndAlcoholTest.UpdateDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                  , new Guid(ViewState["DrugAlcoholTestId"].ToString())
                                                  , int.Parse(ddlVessel.SelectedVessel)
                                                  , chkDrugAndAlcoholTestYN.Checked.Equals(true) ? 1 : 0
                                                  , General.GetNullableDateTime(txtDateofTest.Text)
                                                  , General.GetNullableInteger(ucport.SelectedValue)
                                                  , General.GetNullableString(ucport.Text)
                                                  , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                  , General.GetNullableString(txtExternalInspectorName.Text)
                                                  , null
                                                  , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
                                                  , General.GetNullableString(ddlExternalOrganizationName.Text)

                                                  );
                    }

                    ucStatus.Text = "Information Updated";
                }
                else
                {
                    if (!IsValidDrugAndAlcoholTest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (ucAuditCategory.SelectedHard.ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                    {

                        PhoenixInspectionDrugAndAlcoholTest.InsertDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                     , int.Parse(ddlVessel.SelectedVessel)
                                                    , chkDrugAndAlcoholTestYN.Checked.Equals(true) ? 1 : 0
                                                    , General.GetNullableDateTime(txtDateofTest.Text)
                                                    , General.GetNullableInteger(ucport.SelectedValue)
                                                    , General.GetNullableString(ucport.Text)
                                                    , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                    , General.GetNullableString(ucInspector.Text)
                                                    , General.GetNullableInteger(ucInspector.SelectedValue)
                                                    , General.GetNullableInteger(ddlOrganization.SelectedValue)
                                                    , General.GetNullableString(ddlOrganization.Text)

                                                   );
                    }
                    else
                    {
                        PhoenixInspectionDrugAndAlcoholTest.InsertDrugAndAlcoholTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(ddlVessel.SelectedVessel)
                                                   , chkDrugAndAlcoholTestYN.Checked.Equals(true) ? 1 : 0
                                                   , General.GetNullableDateTime(txtDateofTest.Text)
                                                   , General.GetNullableInteger(ucport.SelectedValue)
                                                   , General.GetNullableString(ucport.Text)
                                                   , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                   , General.GetNullableString(txtExternalInspectorName.Text.ToString())
                                                   , null
                                                  , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
                                                  , General.GetNullableString(ddlExternalOrganizationName.Text)

                                                  );
                    }

                    ucStatus.Text = "Information Added";

                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "DrugAndAlcoholTest", scriptClosePopup);

            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["DrugAlcoholTestId"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public bool IsValidDrugAndAlcoholTest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucAuditCategory.SelectedHard.ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
        {

            if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                ucError.ErrorMessage = "Vessel is required";

            if (General.GetNullableDateTime(txtDateofTest.Text) == null)
                ucError.ErrorMessage = "Date of Test is required";

            if (General.GetNullableInteger(ucAuditCategory.SelectedHard) == null)
                ucError.ErrorMessage = "Type is required";

            if (General.GetNullableInteger(ucInspector.SelectedValue) == null)
                ucError.ErrorMessage = "Inspector Name is required";

            if (General.GetNullableInteger(ddlOrganization.SelectedValue) == null)
                ucError.ErrorMessage = "Organization is required";
        }
        else
        {
            if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                ucError.ErrorMessage = "Vessel is required";

            if (General.GetNullableDateTime(txtDateofTest.Text) == null)
                ucError.ErrorMessage = "Date of Test is required";

            if (General.GetNullableInteger(ucAuditCategory.SelectedHard) == null)
                ucError.ErrorMessage = "Type is required";

            if (General.GetNullableString(txtExternalInspectorName.Text) == null)
                ucError.ErrorMessage = "Inspector Name is required";

            if (General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue) == null)
                ucError.ErrorMessage = "Organization is required";

        }

        return (!ucError.IsError);
    }
    protected void Bind_UserControls(object sender, EventArgs e)
    {
        //BindInspection();
        if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            EnableDisableExternal(true, "input_mandatory");
            EnableDisableInternal(false, "input");
        }
        else if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
        {
            EnableDisableExternal(false, "input");
            EnableDisableInternal(true, "input_mandatory");
        }

        ViewState["TYPE"] = ucAuditCategory.SelectedHard;
    }

    protected void EnableDisableExternal(bool value, string cssclass)
    {
        txtExternalInspectorName.Enabled = value;

        txtExternalOrganisationName.Enabled = value;
        ddlExternalOrganizationName.Enabled = value;

        if (value == false)
        {
            txtExternalInspectorName.Text = "";
            ddlExternalOrganizationName.SelectedValue = "";
            ddlExternalOrganizationName.Text = "";
            txtExternalOrganisationName.Text = "";

        }

        ddlExternalOrganizationName.CssClass = cssclass;
        txtExternalInspectorName.CssClass = cssclass;
    }

    protected void EnableDisableInternal(bool value, string cssclass)
    {
        ucInspector.Enabled = value;
        ddlOrganization.Enabled = value;

        if (value == false)
        {
            ucInspector.SelectedValue = "";
            ucInspector.Text = "";
            txtOrganization.Text = "";
            ddlOrganization.SelectedValue = "";
            ddlOrganization.Text = "";
        }

        ucInspector.CssClass = cssclass;
        ddlOrganization.CssClass = cssclass;
    }

    private void Reset()
    {
        ViewState["FLDDRUGALCOHOLTESTID"] = null;

    }

}