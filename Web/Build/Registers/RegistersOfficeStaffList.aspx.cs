using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class OfficeStaffList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "BACK");
        toolbarmain.AddButton("Office Staff", "OFFICESTAFF");
        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["OFFICESTAFFID"] != null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            ViewState["PAGENO"] = Request.QueryString["page"].ToString();
            ViewState["OFFICESTAFFID"] = Request.QueryString["OFFICESTAFFID"].ToString();
            toolbarmain.AddButton("Family", "FAMILY");
            toolbarmain.AddButton("Approver", "APPROVERLIST");           
        }
        else
        {
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        MenuSecurityOfficeStaffList.AccessRights = this.ViewState;
        MenuSecurityOfficeStaffList.MenuList = toolbar.Show();
        MenuOfficestaffMain.AccessRights = this.ViewState;
        MenuOfficestaffMain.MenuList = toolbarmain.Show();
        MenuOfficestaffMain.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            if (Request.QueryString["OFFICESTAFFID"] != null)
            {
                OfficeStaffEdit(Int32.Parse(Request.QueryString["OFFICESTAFFID"].ToString()));
            }
        }
    }
    protected void MenuOfficestaffMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["OFFICESTAFFID"] != null)
                    Response.Redirect("../Registers/RegistersOfficeStaff.aspx?page=" + ViewState["PAGENO"].ToString(), false);
                else
                    Response.Redirect("../Registers/RegistersOfficeStaff.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("OFFICESTAFF"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaffList.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("APPROVERLIST"))
            {
                Response.Redirect("../Registers/RegistersOfficestaffapproval.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("FAMILY"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaffFamily.aspx?OFFICESTAFFID=" + ViewState["OFFICESTAFFID"].ToString() + "&page=" + ViewState["PAGENO"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SecurityOfficeStaffList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='OfficeStaffEdit'>" + "\n";
            scriptClosePopup += "fnReloadList('OfficeStaff');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='OfficeStaffAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (ViewState["OFFICESTAFFID"] != null)
                {
                    if (!IsValidOfficeStaff())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersOfficeStaff.UpdateOfficeStaff(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Int32.Parse(ViewState["OFFICESTAFFID"].ToString()),
                        txtEmployeeNumber.Text.Trim(),
                        txtLastname.Text.Trim(),
                        General.GetNullableString(txtFirstName.Text),
                        General.GetNullableString(txtMiddleName.Text),
                        Convert.ToInt16(ucDesignation.SelectedDesignation),
                        General.GetNullableString(txtPassportNo.Text),
                        General.GetNullableDateTime(txtDateOfBirth.Text),
                        null,
                        General.GetNullableInteger(ucNationality.SelectedNationality),
                        General.GetNullableString(txtEmail.Text),
                        General.GetNullableDateTime(txtDateOfIssue.Text),
                        General.GetNullableString(txtPlaceOfIssue.Text),
                        General.GetNullableDateTime(txtExpiryDate.Text),
                        null,
                        null,
                        General.GetNullableInteger(ucDepartment.SelectedDepartment),
                        txtUsername.Text.Trim()
                        , ddlsalutation.SelectedText
                        , General.GetNullableInteger(ucZone.SelectedZone)
                        , int.Parse(chkisactive.Checked == true ? "1" : "0"));
                    ucStatus.Text = "Information Updated";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "OfficeStaffEdit", scriptClosePopup);
                }
                else
                {
                    if (!IsValidOfficeStaff())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixRegistersOfficeStaff.InsertOfficeStaff(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        txtEmployeeNumber.Text.Trim(),
                        txtLastname.Text.Trim(),
                        General.GetNullableString(txtFirstName.Text),
                        General.GetNullableString(txtMiddleName.Text),
                        Convert.ToInt16(ucDesignation.SelectedDesignation),
                        General.GetNullableString(txtPassportNo.Text),
                        General.GetNullableDateTime(txtDateOfBirth.Text),
                        null,
                        General.GetNullableInteger(ucNationality.SelectedNationality),
                        General.GetNullableString(txtEmail.Text),
                        General.GetNullableDateTime(txtDateOfIssue.Text),
                        General.GetNullableString(txtPlaceOfIssue.Text),
                        General.GetNullableDateTime(txtExpiryDate.Text),
                        null,
                        null,
                        General.GetNullableInteger(ucDepartment.SelectedDepartment),
                        txtUsername.Text.Trim()
                        , ddlsalutation.SelectedText
                        , General.GetNullableInteger(ucZone.SelectedZone)
                        , int.Parse(chkisactive.Checked == true ? "1" : "0"));
                    ucStatus.Text = "Information Added";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "OfficeStaffAddNew", scriptClosePopup);
                    Response.Redirect("../Registers/RegistersOfficeStaff.aspx", false);
                }
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

    public bool IsValidOfficeStaff()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        if (ddlsalutation.SelectedValue.Equals("Dummy"))
            ucError.ErrorMessage = "Salutation is required.";
        if (txtFirstName.Text.Equals(""))
            ucError.ErrorMessage = "First Name is required.";

        if (txtEmployeeNumber.Text.Equals(""))
            ucError.ErrorMessage = "Employee Number is required.";

        if (ucNationality.SelectedNationality == "" || !Int16.TryParse(ucNationality.SelectedNationality, out result))
            ucError.ErrorMessage = "Nationality is required.";

        if (ucDesignation.SelectedDesignation == "" || !Int16.TryParse(ucDesignation.SelectedDesignation, out result))
            ucError.ErrorMessage = "Designation is required.";

        if (((txtPassportNo.Text == "") || (General.GetNullableString(txtPassportNo.Text) == null)) && ((General.GetNullableDateTime(txtDateOfIssue.Text) != null) || (General.GetNullableDateTime(txtExpiryDate.Text) != null)))
            ucError.ErrorMessage = "Passport number is required.";

        if ((General.GetNullableDateTime(txtDateOfIssue.Text) != null) && (General.GetNullableDateTime(txtDateOfIssue.Text) >= DateTime.Now))
            ucError.ErrorMessage = "Date of issue should be less than or equal to current date.";

        if ((txtPassportNo.Text != "") || (General.GetNullableString(txtPassportNo.Text) != null))
        {
            if (General.GetNullableDateTime(txtExpiryDate.Text) == null)
                ucError.ErrorMessage = "Expiry date is required.";

            if (General.GetNullableDateTime(txtDateOfIssue.Text) == null)
                ucError.ErrorMessage = "Date of issue is required.";

            if (General.GetNullableDateTime(txtDateOfIssue.Text) >= General.GetNullableDateTime(txtExpiryDate.Text))
                ucError.ErrorMessage = "Date of issue should be earlier than expiry date.";
        }
        if (General.GetNullableDateTime(txtDateOfBirth.Text) == null)
            ucError.ErrorMessage = "Date of Birth is Required.";

        if (General.GetNullableString(txtUsername.Text) == null)
            ucError.ErrorMessage = "Username is Required.";


        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["OFFICESTAFFID"] = null;
        txtLastname.Text = "";
        txtFirstName.Text = "";
        txtMiddleName.Text = "";
        ucDesignation.SelectedDesignation = "";
        txtPassportNo.Text = "";
        txtDateOfBirth.Text = "";
        ucNationality.SelectedNationality = "";
        txtEmail.Text = "";
        txtDateOfIssue.Text = "";
        txtExpiryDate.Text = "";
        txtPlaceOfIssue.Text = "";
        txtEmployeeNumber.Text = "";
        ucDepartment.SelectedDepartment = "";
        txtUsername.Text = "";
        txtSalutation.Text = "";
        ucZone.SelectedZone = "";
    }

    private void OfficeStaffEdit(int companyid)
    {
        try
        {
            DataSet ds = PhoenixRegistersOfficeStaff.EditOfficeStaff(companyid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmployeeNumber.Text = dr["FLDEMPLOYEENUMBER"].ToString();
                txtLastname.Text = dr["FLDOFFICESURNAME"].ToString();
                txtFirstName.Text = dr["FLDOFFICEFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                if (dr["FLDDESIGNATION"].ToString() != "0")
                {
                    ucDesignation.SelectedDesignation = dr["FLDDESIGNATION"].ToString();
                }
                txtPassportNo.Text = dr["FLDPASSPORTNO"].ToString();
                txtDateOfBirth.Text = dr["FLDDATEOFBIRTH"].ToString();

                ucNationality.SelectedNationality = dr["FLDNATIONALITY"].ToString();
                txtEmail.Text = dr["FLDEMAIL"].ToString();
                txtDateOfIssue.Text = dr["FLDDATEOFISSUE"].ToString();
                txtPlaceOfIssue.Text = dr["FLDPLACEOFISSUE"].ToString();
                txtUsername.Text = dr["FLDUSERNAME"].ToString();
                txtExpiryDate.Text = dr["FLDEXPIRYDATE"].ToString();
                ucDepartment.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
                txtSalutation.Text = dr["FLDSALUTATION"].ToString();
                chkisactive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;
                ddlsalutation.SelectedText = dr["FLDSALUTATION"].ToString();
                ucZone.SelectedZone = dr["FLDLOCATION"].ToString();
                txthrmsdesignation.Text = dr["FLDHRMSDESIGNATION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
