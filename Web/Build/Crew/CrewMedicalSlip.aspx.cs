using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewMedicalSlip : PhoenixBasePage
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
            toolbar.AddButton("Request Medical", "SAVE", ToolBarDirection.Right);
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

                btnconfirm.Attributes.Add("style", "display:none");

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
                ViewState["MPAGENUMBER"] = 1;
                ViewState["MSORTEXPRESSION"] = null;
                ViewState["MSORTDIRECTION"] = null;
                if (vslid != "" && vslid != null)
                {
                    ucVessel.SelectedValue = vslid;

                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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
                rblFee.DataBind();

                SetBudgetCodeDetails();
                rblRequest.SelectedValue = "1";
                BindVesselAccount();
                EditMedicalSlip();
                BindOwnerBudgetCode();

                gvReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PND"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "PND");
                ViewState["CNC"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "CNC");
                
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
                    if(li.Value == mt.Rows[i]["FLDDOCUMENTMEDICALID"].ToString())
                    {
                        li.Selected = false;
                    }                    
                }                
                if (("," + mt.Rows[i]["FLDMEDICALTYPE"].ToString() + ",").Contains("," + ds.Tables[0].Rows[0]["FLDMEDICALREQUIRED"].ToString() + ","))
                {
                
                    foreach (ButtonListItem li in cblMedicalTest.Items)
                    {
                        if (li.Value == mt.Rows[i]["FLDDOCUMENTMEDICALID"].ToString())
                        {
                            li.Selected = true;
                        }                        
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
        else if ((General.GetNullableInteger(ucVessel.SelectedValue) != null || vslid != "") && (rblRequest.SelectedValue == "2"))
        {
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
                SetBudgetCodeDetails();
            }
       
            foreach (ButtonListItem li in cblFlagMedical.Items)
            {
                li.Selected =  false;
            }
            foreach (ButtonListItem li in cblPreMedical.Items)
            {
                li.Selected =  false;
            }
            foreach (ButtonListItem li in cblVaccination.Items)
            {
                li.Selected = false;
            }
            foreach (ButtonListItem li in cblDeclaration.Items)
            {
                li.Selected = false;
            }
            foreach (ButtonListItem li in cblMedicalTest.Items)
            {
                if (li.Value == "8")
                    li.Selected = true;
                else
                    li.Selected = false;
            }
            
            cblMedicalTest.Enabled = false;
            cblFlagMedical.Enabled = false;
            cblPreMedical.Enabled = false;
            cblVaccination.Enabled = false;
            cblDeclaration.Enabled = false;
            txtOthers.ReadOnly = true;
            txtOthers.CssClass = "readonlytextbox";

            BindOwnerBudgetCode();
            BindVesselAccount();
        }
        else
        {
            SetCourseRequest(ref vesselid, null);
            if (vesselid != null)
            {
                ucVessel.Text = "";
                ucVessel.SelectedValue = "";
                ucVessel.SelectedValue = vesselid.ToString();
                SetBudgetCodeDetails();
            }
            
            foreach (ButtonListItem li in cblMedicalTest.Items)
            {
                   li.Selected = false;
            }

            EditMedicalSlip();
            SetEmployeePrimaryDetails();
            cblMedicalTest.Enabled = true;
            cblFlagMedical.Enabled = true;
            cblPreMedical.Enabled = true;
            cblVaccination.Enabled = true;
            cblDeclaration.Enabled = true;
            txtOthers.ReadOnly = false;
            txtOthers.CssClass = "input";

            BindVesselAccount();
            BindOwnerBudgetCode();

        }
    }
    
    protected void SetCourseRequest(ref int? vesselid, int? lftreqyn)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(General.GetNullableInteger(ViewState["empid"].ToString()), lftreqyn);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

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

    protected void OnBudgetChange(object sender, EventArgs e)
    {
        if (ddlBudgetCode.SelectedValue != "")
        {
            DataSet ds = PhoenixRegistersBudget.EditBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ddlBudgetCode.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBudgetCode.Text = ds.Tables[0].Rows[0]["FLDSUBACCOUNT"].ToString();
            }
            BindVesselAccount();

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

                RadWindowManager1.RadConfirm("Your medical request will be saved when you click on Yes and you will not be able to make any changes. Click on Yes to initiate medical request Or click on No to continue editing?", "btnconfirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {

            string strPreMedical = string.Empty, strFlagMedical = string.Empty, strMedicalTest = string.Empty, strVaccanaiton = string.Empty, strDeclaration = string.Empty;
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

            PhoenixCrewMedical.InsertMedicalRequest(int.Parse(ViewState["empid"].ToString()), int.Parse(ddlClinic.SelectedClinic), General.GetNullableInteger(ucVessel.SelectedValue)
                , strPreMedical, strFlagMedical, strMedicalTest, strVaccanaiton, txtOthers.Text, int.Parse(rblFee.SelectedValue), DateTime.Parse(General.GetNullableDateTime(txtDate.Text).Value.ToShortDateString() + " " + txtTime.TextWithLiterals)
                , General.GetNullableInteger(familyid), General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "PND"))
                , General.GetNullableInteger(ddlBudgetCode.SelectedValue), Convert.ToInt32(rblRequest.SelectedValue), General.GetNullableGuid(Request.QueryString["crewplanid"]), null
                , strDeclaration, General.GetNullableInteger(ddlAccountDetails.SelectedValue), General.GetNullableGuid(ddlOwnerBudgetCode.SelectedValue));

            ucStatus.Text = "Medical Request Initiated";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('chml','ifMoreInfo',null);", true);
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
            ucError.ErrorMessage = "Clinic is required.";
        }
        if (!int.TryParse(ucVessel.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (txtDate.Text == null)
        {
            ucError.ErrorMessage = "Appointment Date and Time is required.";
        }
        else if (!DateTime.TryParse(General.GetNullableDateTime(txtDate.Text).Value.ToShortDateString() +" " + txtTime.TextWithLiterals, out resultDate))
        {
            ucError.ErrorMessage = "Appointment Date and Time is not Valid.";
        }
        else if (DateTime.TryParse(txtDate.Text, out resultDate) && resultDate < DateTime.Today)
        {
            ucError.ErrorMessage = "Appointment Date should be later than or equal to current date";
        }
        if (!int.TryParse(ddlBudgetCode.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Budget Code is required.";
        }
        if (ddlOwnerBudgetCode.SelectedValue == "")
        {
            ucError.ErrorMessage = "Owner Budget Code is mandatory when creating request.";
        }
        if (!int.TryParse(rblFee.SelectedValue, out resultInt))
        {
            ucError.ErrorMessage = "Payment By is required";
        }

        return (!ucError.IsError);
    }
    private void EditMedicalSlip()
    {
        try
        {
            if (!string.IsNullOrEmpty(vslid) && CrewMedical.Visible)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vslid));
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
                    if (("," + mt.Rows[i]["FLDMEDICALTYPE"].ToString() + ",").Contains("," + ds.Tables[0].Rows[0]["FLDMEDICALREQUIRED"].ToString() + ","))
                    {
                        foreach (ButtonListItem li in cblMedicalTest.Items)
                        {
                            if (li.Value == mt.Rows[i]["FLDDOCUMENTMEDICALID"].ToString())
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
            else
            {
                VesselChanged(null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFNUMBER", "FLDVESSELCODE", "FLDJOININGVESSELCODE", "FLDRANKCODE", "FLDNAME", "FLDCLINICADDRESSNAME", "FLDFILENO", "FLDHARDNAME", "FLDSEAFARERNAME", "FLDCREATEDDATE", "FLDCREATEDBY" };
        string[] alCaptions = { "Request Number", "Vessel", "Vessel Joined", "Rank", "Name", "Clinic Name",  "Employee No.", "Status", "Seafarer Name", "Created Date", "Created By" };

        string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
        int? sortdirection = 1; 
        if (ViewState["MSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

        try
        {

            DataTable dt = PhoenixCrewMedical.SearchMedicalRequest(General.GetNullableInteger(ViewState["empid"].ToString()), General.GetNullableInteger(familyid)
                                                        , null
                                                        , null
                                                        , 1
                                                        , null
                                                        , null
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["MPAGENUMBER"]
                                                        , gvReq.PageSize
                                                        , ref iRowCount, ref iTotalPageCount, null);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvReq", "Medical Request", alCaptions, alColumns, ds);

            gvReq.DataSource = dt;
            gvReq.VirtualItemCount = iRowCount;

            ViewState["MROWCOUNT"] = iRowCount;
            ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["MPAGENUMBER"] = ViewState["MPAGENUMBER"] != null ? ViewState["MPAGENUMBER"] : gvReq.CurrentPageIndex + 1;
        BindData();
    }


    protected void gvReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CANCELREQUEST"))
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                PhoenixCrewMedical.CancelMedicalRequest(new Guid(requestid));

                BindData();
                gvReq.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton med = (LinkButton)e.Item.FindControl("cmdMedical");
            if (med != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, med.CommandName)) med.Visible = false;
                med.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&emailyn=0&reportcode=MEDICALSLIP&showword=0&showexcel=0&t=1&reqid=" + drv["FLDREQUESTID"].ToString() + "'); return false;");
            }

            RadLabel lbstatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblRequestId");

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }

            if(lbstatus != null)
            {
                if (lbstatus.Text != ViewState["PND"].ToString())
                    cmdEdit.Visible = false;
                else
                {
                    cmdEdit.Visible = true;
                    cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Crew/CrewMedicalRequestUpdate.aspx?empid=" + Request.QueryString["empid"] + "&vslid=" + Request.QueryString["vslid"] + "&familyid=" + Request.QueryString["familyid"] + "&newappyn=" + Request.QueryString["newappyn"] + "&RequestID=" + lblRequestId.Text.Trim() + "'); return false;");
                }

                if (lbstatus.Text == ViewState["CNC"].ToString())
                {
                    UserControlToolTip uctstatus = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");
                    uctstatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctstatus.ToolTip + "', 'visible');");
                    uctstatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctstatus.ToolTip + "', 'hidden');");
                }
            }
            
            LinkButton cmdCancelRequest = (LinkButton)e.Item.FindControl("cmdCancelRequest");
            if (cmdCancelRequest != null)
            {
                cmdCancelRequest.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to cancel this Request ?'); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancelRequest.CommandName)) cmdCancelRequest.Visible = false;
            }

        }
    }
    
    private bool IsValidMedicalRequest(string invoiceno)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (invoiceno.Trim() == "")
            ucError.ErrorMessage = "Invoice No is required";

        return (!ucError.IsError);
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
            ddlAccountDetails.SelectedValue = "";
            ddlAccountDetails.Text = "";
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
