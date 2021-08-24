using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
public partial class CrewMedicalAdd : PhoenixBasePage
{

    string empid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewMedical.AccessRights = this.ViewState;
        MenuCrewMedical.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            txtPlaceOfIssue.Focus();
            ViewState["CREWFLAGMEDICALID"] = Request.QueryString["CREWFLAGMEDICALID"];

            ViewState["isaddedseafarer"] = "";
            if (Request.QueryString["PORTAL"] != null)
                ViewState["isaddedseafarer"] = 1;


            SetEmployeePrimaryDetails();
            cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(1, null);
            cblMedicalTest.DataBindings.DataTextField = "FLDNAMEOFMEDICAL";
            cblMedicalTest.DataBindings.DataValueField = "FLDDOCUMENTMEDICALID";
            cblMedicalTest.DataBind();
            if (Request.QueryString["CREWFLAGMEDICALID"] != null)
            {
                EditCrewMedical(Convert.ToInt32(Request.QueryString["CREWFLAGMEDICALID"]));
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                lblVerifiedDate.Visible = false;
                ucVerifiedDate.Visible = false;
                lblVerifiedByHeader.Visible = false;
                txtVerifiedBy.Visible = false;
                lblVerificationMethod.Visible = false;
                ucVerificationMethod.Visible = false;
            }
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {
                ViewState["LASTSIGNOFF"] = dt.Rows[0]["FLDLASTSIGNOFFDATE"].ToString();
                ViewState["POOL"] = dt.Rows[0]["FLDPOOL"].ToString();
                ucMedical.Pool = General.GetNullableInteger(dt.Rows[0]["FLDPOOL"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditCrewMedical(int medicalid)
    {
        DataTable dt = PhoenixCrewMedicalDocuments.EditCrewFlagMedical(Convert.ToInt32(empid), medicalid);

        if (dt.Rows.Count > 0)
        {
            ucMedical.SelectedHard = dt.Rows[0]["FLDMEDICALID"].ToString();
            txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOFISSUE"].ToString();
            txtIssueDate.Text = dt.Rows[0]["FLDISSUEDDATE"].ToString();
            txtExpiryDate.Text = dt.Rows[0]["FLDEXPIRYDATE"].ToString();

            ucFlag.SelectedFlag = dt.Rows[0]["FLDFLAGID"].ToString();
            ddlStatus.SelectedHard = dt.Rows[0]["FLDSTATUS"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBY"].ToString();
            ucVerifiedDate.Text = dt.Rows[0]["FLDVERIFIEDDATE"].ToString();
            ucVerificationMethod.SelectedQuick = dt.Rows[0]["FLDVERIFICATIONMETHOD"].ToString();
            txtWeight.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
            if (dt.Rows[0]["FLDMEDICALID"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM"))
            {
                ucFlag.Enabled = true;
                ucFlag.CssClass = "dropdown_mandatory";
            }
            else
            {
                ucFlag.Enabled = false;
                ucFlag.CssClass = "input";
                ucFlag.SelectedFlag = "";
            }
            if (dt.Rows[0]["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            || dt.Rows[0]["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
            {

                txtExpiryDate.CssClass = "input";
                txtRemarks.CssClass = "input_mandatory";
                cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(1, General.GetNullableInteger(ucMedical.SelectedHard));
                cblMedicalTest.DataBindings.DataTextField = "FLDNAMEOFMEDICAL";
                cblMedicalTest.DataBindings.DataValueField = "FLDDOCUMENTMEDICALID";
                cblMedicalTest.DataBind();
            }
            else
            {
                txtExpiryDate.CssClass = "input_mandatory";
                txtRemarks.CssClass = "input";
            }
            string[] strmedical = dt.Rows[0]["FLDUNFITMEDICAL"].ToString().Split(',');
            foreach (string item in strmedical)
            {
                if (item.Trim() != "")
                {
                    for (int i = 0; i < cblMedicalTest.Items.Count; i++)
                    {
                        if (cblMedicalTest.Items[i].Value == item)
                            cblMedicalTest.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strmedical = new StringBuilder();


                foreach (ButtonListItem item in cblMedicalTest.Items)
                {
                    if (item.Selected == true)
                    {
                        strmedical.Append(item.Value.ToString());
                        strmedical.Append(",");
                    }
                }

                if (strmedical.Length > 1)
                {
                    strmedical.Remove(strmedical.Length - 1, 1);
                }
                if (!IsValidMedical(ucMedical.SelectedHard, txtIssueDate.Text, txtExpiryDate.Text, ucFlag.SelectedFlag, txtPlaceOfIssue.Text, ddlStatus.SelectedHard, strmedical.ToString(),txtWeight.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["CREWFLAGMEDICALID"] != null)
                {

                    PhoenixCrewMedicalDocuments.UpdateCrewFlagMedical(
                            Convert.ToInt32(ViewState["CREWFLAGMEDICALID"].ToString())
                          , Convert.ToInt32(empid)
                          , Convert.ToInt32(ucMedical.SelectedHard)
                          , txtPlaceOfIssue.Text
                          , General.GetNullableDateTime(txtIssueDate.Text)
                          , General.GetNullableDateTime(txtExpiryDate.Text)
                          , General.GetNullableInteger(ucFlag.SelectedFlag)
                          , General.GetNullableInteger(ddlStatus.SelectedHard)
                          , General.GetNullableString(txtRemarks.Text)
                          , General.GetNullableString(strmedical.ToString())
                          , General.GetNullableDecimal(txtWeight.Text)
                          , General.GetNullableInteger(General.GetNullableDateTime(ucVerifiedDate.Text) != null ? "1" : "0")
                          , General.GetNullableDateTime(ucVerifiedDate.Text)
                          , General.GetNullableInteger(ucVerificationMethod.SelectedQuick)
                          
                          );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

                }
                else
                {
                    PhoenixCrewMedicalDocuments.InsertCrewFlagMedical(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                      , Convert.ToInt32(empid)
                      , Convert.ToInt32(ucMedical.SelectedHard)
                      , txtPlaceOfIssue.Text
                      , General.GetNullableDateTime(txtIssueDate.Text)
                      , General.GetNullableDateTime(txtExpiryDate.Text)
                      , General.GetNullableInteger(ucFlag.SelectedFlag)
                      , General.GetNullableInteger(ddlStatus.SelectedHard)
                      , General.GetNullableString(txtRemarks.Text)
                      , General.GetNullableString(strmedical.ToString())
                      , General.GetNullableDecimal(txtWeight.Text)
                      , General.GetNullableDateTime(ucVerifiedDate.Text)
                      , General.GetNullableInteger(ucVerificationMethod.SelectedQuick)
                      ,General.GetNullableInteger(ViewState["isaddedseafarer"].ToString())
                      );
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMedical(string medicalid, string dateofissue, string dateofexpiry, string flagid, string placeofissue, string status, string medicaltest , string weight)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");


        if (!Int16.TryParse(medicalid, out resultInt))
            ucError.ErrorMessage = "Medical is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (ViewState["LASTSIGNOFF"].ToString() != string.Empty && (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ViewState["LASTSIGNOFF"].ToString())) < 0))
        {
            ucError.ErrorMessage = "Issue Date should be greater than Last SignOff Date";
        }
        if (string.IsNullOrEmpty(placeofissue))
        {
            ucError.ErrorMessage = "Place of Issue is required";
        }

        if (string.IsNullOrEmpty(weight.Trim()))
        {
            ucError.ErrorMessage = "Weight is required";
        }

        if (status != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            && status != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {
            if (string.IsNullOrEmpty(dateofexpiry))
                ucError.ErrorMessage = "Expiry Date is required.";
            if (dateofissue != null && dateofexpiry != null)
            {
                if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                    if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                        ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
            }
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (medicalid == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (General.GetNullableInteger(flagid) == null)
                    ucError.ErrorMessage = "Flag is required for Flag Medical.";

            if (medicalid != ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (!Int16.TryParse(status, out resultInt))
                    ucError.ErrorMessage = "Status is required";
        }
        if (status == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            || status == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {

            if (txtRemarks.Text == "")
                ucError.ErrorMessage = "Remarks is required.";

            if (medicaltest == "")
                ucError.ErrorMessage = "Select atleast one Medical Test.";
        }

        return (!ucError.IsError);
    }

    protected void DisableFlag(object sender, EventArgs e)
    {
        if (ucMedical.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM"))
        {
            ucFlag.Enabled = true;
            ucFlag.CssClass = "dropdown_mandatory";
        }
        else
        {
            ucFlag.Enabled = false;
            ucFlag.CssClass = "input";
            ucFlag.SelectedFlag = "";
        }
        if (ucMedical.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "P&I")
             || ucMedical.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "UKP"))
        {
            cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(1, General.GetNullableInteger(ucMedical.SelectedHard));
            cblMedicalTest.DataBindings.DataTextField = "FLDNAMEOFMEDICAL";
            cblMedicalTest.DataBindings.DataValueField = "FLDDOCUMENTMEDICALID";
            cblMedicalTest.DataBind();
        }
        else
        {
            cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(1, null);
            cblMedicalTest.DataBindings.DataTextField = "FLDNAMEOFMEDICAL";
            cblMedicalTest.DataBindings.DataValueField = "FLDDOCUMENTMEDICALID";
            cblMedicalTest.DataBind();
        }

    }

    protected void Status_OnTextChangedEvent(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            || ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {

            txtExpiryDate.CssClass = "input";
            txtRemarks.CssClass = "input_mandatory";
            cblMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical(1, General.GetNullableInteger(ucMedical.SelectedHard));
            cblMedicalTest.DataBindings.DataTextField = "FLDNAMEOFMEDICAL";
            cblMedicalTest.DataBindings.DataValueField = "FLDDOCUMENTMEDICALID";
            cblMedicalTest.DataBind();
        }
        else
        {
            txtExpiryDate.CssClass = "input_mandatory";
            txtRemarks.CssClass = "input";
        }
    }
}
