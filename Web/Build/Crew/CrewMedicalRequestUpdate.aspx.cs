using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Crew_CrewMedicalRequestUpdate : PhoenixBasePage
{
    string vslid = string.Empty;
    string familyid = string.Empty;
    string newappyn = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);                      
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewMedical.AccessRights = this.ViewState;
            CrewMedical.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["empid"] = Request.QueryString["empid"];
                if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != "Dummy")
                {
                    vslid = Request.QueryString["vslid"];
                }
                familyid = Request.QueryString["familyid"];
                newappyn = Request.QueryString["newappyn"];

                if (newappyn != null)
                {
                    lblRequest.Visible = false;
                    rblRequest.Visible = false;
                }
                ucVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte("1")
                                                                              , General.GetNullableByte("1")
                                                                              , null
                                                                              , General.GetNullableByte("1")
                                                                              , PhoenixVesselEntityType.VSL
                                                                              , null);
                ucVessel.DataTextField = "FLDVESSELNAME";
                ucVessel.DataValueField = "FLDVESSELID";
                ucVessel.DataBind();

                if (vslid != "" && vslid != null)
                {
                    ucVessel.SelectedValue = vslid;

                }
                ViewState["TYPE"] = "";

                SetEmployeePrimaryDetails();

                cblPreMedical.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, 0, "P&I,UKP,PMU");               
                cblPreMedical.DataBind();

                cblFlagMedical.DataSource = PhoenixRegistersFlag.ListFlag(1);                
                cblFlagMedical.DataBind();

                cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(General.GetNullableInteger(ViewState["TYPE"].ToString()), null);                
                cblMedicalTest.DataBind();

                cblVaccination.DataSource = PhoenixRegistersDocumentMedical.ListDocumentVaccination();                
                cblVaccination.DataBind();

                cblDeclaration.DataSource = PhoenixRegistersDocumentMedical.ListDocumentDeclaration();                
                cblDeclaration.DataBind();

                rblFee.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 89, 0, "CMP,EMP");
                //rblFee.DataTextField = "FLDHARDNAME";
                //rblFee.DataValueField = "FLDHARDCODE";
                rblFee.DataBind();

                SetBudgetCodeDetails();          
                rblRequest.SelectedValue = "1";
                BindVesselAccount();
                BindOwnerBudgetCode();
                PopulateEditDetatils();                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void VesselChanged(object sender, EventArgs e)
    {
        if ((General.GetNullableInteger(ucVessel.SelectedValue) != null || vslid != "") && (rblRequest.SelectedValue == "1"))
        {
            string strvesselid = null;
            if (General.GetNullableInteger(ucVessel.SelectedValue) == null)
            {
                strvesselid = vslid;
            }
            else
            {
                strvesselid = ucVessel.SelectedValue;
            }
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strvesselid));
            lblVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            foreach (ButtonListItem li in cblFlagMedical.Items)
            {
                li.Selected = (li.Value == ds.Tables[0].Rows[0]["FLDFLAG"].ToString()) ? true : false;
            }
            foreach (ButtonListItem li in cblPreMedical.Items)
            {
                li.Selected = (li.Value == ds.Tables[0].Rows[0]["FLDMEDICALREQUIRED"].ToString()) ? true : false;
            }
            DataTable mt = PhoenixRegistersDocumentMedical.ListDocumentMedical(1).Tables[0];
            for (int i = 0; i < mt.Rows.Count; i++)
            {
                foreach (ButtonListItem li in cblMedicalTest.Items)
                {
                    if (li.Value == mt.Rows[i]["FLDDOCUMENTMEDICALID"].ToString())
                    {
                        li.Selected = false;
                    }
                }

                if (("," + mt.Rows[i]["FLDMEDICALTYPE"].ToString() + ",").Contains("," + ds.Tables[0].Rows[0]["FLDMEDICALREQUIRED"].ToString() + ","))
                    foreach (ButtonListItem li in cblMedicalTest.Items)
                    {
                        if (li.Value == mt.Rows[i]["FLDDOCUMENTMEDICALID"].ToString())
                        {
                            li.Selected = true;
                        }
                    }
            }

            BindVesselAccount();

            if (ddlBudgetCode.SelectedValue != "")
            {
                ddlOwnerBudgetCode.SelectedValue = "";
                ddlOwnerBudgetCode.Text = "";

                DataSet ds1 = PhoenixCrewMedical.ListOwnerMedicalBudgetCode(General.GetNullableInteger(ucVessel.SelectedValue), Convert.ToInt32(ddlBudgetCode.SelectedValue), General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                ddlOwnerBudgetCode.DataSource = ds1;
                ddlOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETCODE";
                ddlOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETCODEMAPID";
                ddlOwnerBudgetCode.DataBind();
                ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                if (ds1.Tables[0].Rows.Count == 1)
                {
                    ddlOwnerBudgetCode.SelectedValue = ds1.Tables[0].Rows[0]["FLDOWNERBUDGETCODEMAPID"].ToString();
                }
            }

        }
    }
      
    private void SetBudgetCodeDetails()
    {
        DataSet ds = PhoenixRegistersBudget.ListBudget(102);
        ddlBudgetCode.DataSource = ds;
        ddlBudgetCode.DataTextField = "FLDDESCRIPTION";
        ddlBudgetCode.DataValueField = "FLDBUDGETID";
        ddlBudgetCode.DataBind();
        ddlBudgetCode.SelectedValue = "19";
        OnBudgetChange(null, null);
    }


    protected void BindOwnerBudgetCode()
    {
        DataSet ds1 = PhoenixCrewMedical.ListOwnerMedicalBudgetCode(General.GetNullableInteger(ucVessel.SelectedValue), Convert.ToInt32(ddlBudgetCode.SelectedValue), General.GetNullableInteger(ddlAccountDetails.SelectedValue));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ViewState["OWNBUDCOUNT"] = ds1.Tables[0].Rows.Count;
            ddlOwnerBudgetCode.DataSource = ds1;
            ddlOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETCODE";
            ddlOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETCODEMAPID";
            ddlOwnerBudgetCode.DataBind();
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            if (ds1.Tables[0].Rows.Count == 1)
            {
                ddlOwnerBudgetCode.SelectedValue = "";
                ddlOwnerBudgetCode.Text = "";
                ddlOwnerBudgetCode.SelectedValue = ds1.Tables[0].Rows[0]["FLDOWNERBUDGETCODEMAPID"].ToString();
            }
        }
        else
        {
            ViewState["OWNBUDCOUNT"] = null;
            ddlOwnerBudgetCode.Items.Clear();
            ddlOwnerBudgetCode.SelectedValue = "";
            ddlOwnerBudgetCode.Text = "";
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
    }  


    protected void OnBudgetChange(object sender, EventArgs e)
    {
        if (ddlBudgetCode.SelectedValue != "")
        {
            DataSet ds = PhoenixRegistersBudget.EditBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ddlBudgetCode.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBudgetCode.Text = ds.Tables[0].Rows[0]["FLDSUBACCOUNT"].ToString();
            }     
            BindOwnerBudgetCode();
        }
        else
        {
            txtBudgetCode.Text = "";
            ddlOwnerBudgetCode.Items.Clear();
            ddlOwnerBudgetCode.SelectedValue = "";
            ddlOwnerBudgetCode.Text = "";
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
    }

    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMedical())
                {
                    ucError.Visible = true;
                    return;
                }
                SaveMedicalDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveMedicalDetails()
    {
        try
        {
            string strPreMedical = string.Empty, strFlagMedical = string.Empty, strMedicalTest = string.Empty, strVaccanaiton = string.Empty, strDeclaration =string.Empty;
            foreach (ButtonListItem li in cblPreMedical.Items)
            {
                strPreMedical += li.Selected ? li.Value + "," : string.Empty;
            }
            strPreMedical = strPreMedical.TrimEnd(',');

            foreach (ButtonListItem li in cblFlagMedical.Items)
            {
                strFlagMedical += li.Selected ? li.Value + "," : string.Empty;
            }
            strFlagMedical = strFlagMedical.TrimEnd(',');

            foreach (ButtonListItem li in cblMedicalTest.Items)
            {
                strMedicalTest += li.Selected ? li.Value + "," : string.Empty;
            }
            strMedicalTest = strMedicalTest.TrimEnd(',');

            foreach (ButtonListItem li in cblVaccination.Items)
            {
                strVaccanaiton += li.Selected ? li.Value + "," : string.Empty;
            }
            strVaccanaiton = strVaccanaiton.TrimEnd(',');

            foreach (ButtonListItem li in cblDeclaration.Items)
            {
                strDeclaration += li.Selected ? li.Value + "," : string.Empty;
            }
            strDeclaration = strDeclaration.TrimEnd(',');

            PhoenixCrewMedical.UpdateCrewMedicalRequest(General.GetNullableGuid(Request.QueryString["RequestID"])
                , int.Parse(ViewState["empid"].ToString()), int.Parse(ddlClinic.SelectedClinic), General.GetNullableInteger(ucVessel.SelectedValue)
                , strPreMedical, strFlagMedical, strMedicalTest, strVaccanaiton, txtOthers.Text, int.Parse(rblFee.SelectedValue)
                , DateTime.Parse(General.GetNullableDateTime(txtDate.Text).Value.ToShortDateString() + " " + txtTime.TextWithLiterals)
                , General.GetNullableInteger(ddlBudgetCode.SelectedValue), null
                , strDeclaration, General.GetNullableInteger(ddlAccountDetails.SelectedValue),General.GetNullableGuid(ddlOwnerBudgetCode.SelectedValue));

            ucStatus.Text = "Medical Request Details Updated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMedical()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        DateTime resultDate;
        
        if (!int.TryParse(ddlClinic.SelectedClinic, out resultInt))
        {
            ucError.ErrorMessage = "Clinic Address is required.";
        }
        if (!int.TryParse(ucVessel.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (txtDate.Text == null)
        {
            ucError.ErrorMessage = "Appointment Date and Time is required.";
        }
        else if (!DateTime.TryParse(General.GetNullableDateTime(txtDate.Text).Value.ToShortDateString() + " " + txtTime.TextWithLiterals, out resultDate))
        {
            ucError.ErrorMessage = "Appointment Date and Time is not Valid.";
        }
        else if (DateTime.TryParse(txtDate.Text, out resultDate) && resultDate < DateTime.Today)
        {
            ucError.ErrorMessage = "Appointment Date should be later than or equal to current date";
        }
        if (cblMedicalTest.SelectedValue == string.Empty)
        {
            ucError.ErrorMessage = "Please Select atleast One Test.";
        }
        if (!int.TryParse(ddlBudgetCode.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Budget Code is required.";
        }        
        if (!int.TryParse(rblFee.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Payment By is required.";
        }

        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (General.GetNullableInteger(familyid).HasValue)
            {
                dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(ViewState["empid"].ToString()), General.GetNullableInteger(familyid));
            }
            else
            {
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            }
            if (dt.Rows.Count > 0)
            {
                if (General.GetNullableInteger(familyid).HasValue)
                {
                    txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                    txtDOB.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDATEOFBIRTH"].ToString()));
                    ViewState["TYPE"] = dt.Rows[0]["FLDPREGNANCYTESTREQ"].ToString();

                }
                else
                {
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                    txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    txtCDC.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                    txtDOB.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDATEOFBIRTH"].ToString()));
                    txtPlaceofbirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
                    ViewState["TYPE"] = "1";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ClinicAddress(object sender, EventArgs e)
    {
        ddlClinic.SelectedClinic = "";

        ddlClinic.ClinicList = PhoenixRegistersClinic.ListClinic(General.GetNullableInteger(ddlZone.SelectedZone));
    }

    private void PopulateEditDetatils()
    {
        DataTable dt = PhoenixCrewMedical.EditCrewMedicalRequest(General.GetNullableGuid(Request.QueryString["RequestID"])).Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucVessel.SelectedValue = dr["FLDVESSELID"].ToString();            
            ddlClinic.SelectedClinic = dr["FLDADDRESSCODE"].ToString();
            txtDate.Text = dr["FLDAPPOINTMENTDATE"].ToString();
            txtTime.Text = dr["FLDAPPOINTMENTTIME"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETID"].ToString();
            BindVesselAccount();
            if (dr["FLDVESSELACCOUNTID"].ToString() != null) { 
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
            }
            else
            {
                ddlAccountDetails.SelectedValue = "";
            }
            ddlOwnerBudgetCode.SelectedValue = "";
            ddlOwnerBudgetCode.Text = "";

            OnBudgetChange(null, null);
            foreach (RadComboBoxItem item in ddlOwnerBudgetCode.Items)
            {
                if (item.Value == dr["FLDOWNERBUDGETCODE"].ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

            ddlBudgetCode.SelectedValue = dr["FLDBUDGETID"].ToString();            
            rblFee.SelectedValue = dr["FLDPROFESSIONALFEE"].ToString();
            string[] aryMedicalTest = dr["FLDMEDICALTEST"].ToString().Trim().Split(',');
            string[] aryFlagMedical = dr["FLDFLAGMEDICAL"].ToString().Trim().Split(',');
            string[] aryPreMedical = dr["FLDPREMEDICAL"].ToString().Trim().Split(',');
            string[] aryVaccinations = dr["FLDVACCINATION"].ToString().Trim().Split(',');
            string[] aryDeclarations = dr["FLDDECLARATION"].ToString().Trim().Split(',');

            foreach (string item in aryMedicalTest)
            {
                foreach (ButtonListItem li in cblMedicalTest.Items)
                {
                    if (li.Value == item)
                    {
                        li.Selected = true;
                    }
                }                
            }
            foreach (string item in aryFlagMedical)
            {
                foreach (ButtonListItem li in cblFlagMedical.Items)
                {
                    if (li.Value == item)
                    {
                        li.Selected = true;
                    }
                }                
            }
            foreach (string item in aryPreMedical)
            {
                foreach (ButtonListItem li in cblPreMedical.Items)
                {
                    if (li.Value == item)
                    {
                        li.Selected = true;
                    }
                }                
            }
            foreach (string item in aryVaccinations)
            {
                foreach (ButtonListItem li in cblVaccination.Items)
                {
                    if (li.Value == item)
                    {
                        li.Selected = true;
                    }
                }
            }
            foreach (string item in aryDeclarations)
            {
                foreach (ButtonListItem li in cblDeclaration.Items)
                {
                    if (li.Value == item)
                    {
                        li.Selected = true;
                    }
                }
            }
            
            txtOthers.Text = dr["FLDREMARKS"].ToString();

            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(dr["FLDVESSELID"].ToString()));
            lblVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
        }
    }

    protected void SetCourseRequest(ref int? vesselid, int? lftreqyn)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(Int32.Parse(ViewState["empid"].ToString()), lftreqyn);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

        }
    }
    protected void rblRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        int? vesselid = null;
        if (rblRequest.SelectedValue == "2")
        {
            SetCourseRequest(ref vesselid, 1);
            if (vesselid != null)
            {
                ucVessel.Text = "";
                ucVessel.SelectedValue = "";

                ucVessel.SelectedValue = vesselid.ToString();
            }
            foreach (ButtonListItem li in cblMedicalTest.Items)
            {
                if (li.Value == "8")
                    li.Selected = true;
                else
                    li.Selected = false;
            }
            foreach (ButtonListItem li in cblFlagMedical.Items)
            {
                li.Selected = false;
            }
            foreach (ButtonListItem li in cblPreMedical.Items)
            {
                li.Selected = false;
            }
            foreach (ButtonListItem li in cblVaccination.Items)
            {
                li.Selected = false;
            }
            foreach (ButtonListItem li in cblDeclaration.Items)
            {
                li.Selected = false;
            }
            cblMedicalTest.Enabled = false;
            cblFlagMedical.Enabled = false;
            cblPreMedical.Enabled = false;
            cblVaccination.Enabled = false;
            cblDeclaration.Enabled = false;
            txtOthers.ReadOnly = true;
            txtOthers.CssClass = "readonlytextbox";
        }
        else
        {
            SetCourseRequest(ref vesselid, null);
            if (vesselid != null)
            {
                ucVessel.Text = "";
                ucVessel.SelectedValue = "";
                ucVessel.SelectedValue = vesselid.ToString();
            }
            foreach (ButtonListItem li in cblMedicalTest.Items)
            {
                li.Selected = false;
            }
            SetEmployeePrimaryDetails();
            cblMedicalTest.Enabled = true;
            cblFlagMedical.Enabled = true;
            cblPreMedical.Enabled = true;
            cblVaccination.Enabled = true;
            cblDeclaration.Enabled = true;
            txtOthers.ReadOnly = false;
            txtOthers.CssClass = "";

        }
    }
    

    public void BindVesselAccount()
    {
        ddlAccountDetails.SelectedValue = "";
        ddlAccountDetails.Text = "";

        DataSet dsl = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ucVessel.SelectedValue) == 0 ? null : General.GetNullableInteger(ucVessel.SelectedValue), 1);
        ddlAccountDetails.DataSource = dsl;
        ddlAccountDetails.DataBind();

        if (dsl.Tables[0].Rows.Count == 1)
        {
            ddlAccountDetails.SelectedValue = dsl.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
        }
        
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    
    protected void ddlAccountDetails_TextChanged(object sender, EventArgs e)
    {
        BindOwnerBudgetCode();
    }

}
