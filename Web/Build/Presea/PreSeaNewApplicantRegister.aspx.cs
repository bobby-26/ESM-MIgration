using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using System.Web;

public partial class PreSeaNewApplicantRegister : PhoenixBasePage
{
    #region :   Page Events     :

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuPreSeaApplication.MenuList = toolbar.Show();
            MenuPreSeaApplication.SetTrigger(pnlOnlineApplication);
           

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("SSLC / 10th  ", "SSLC");
            toolbarsub.AddButton("Intermediate (10+2/Diploma)", "INTERMEDIATE");
            toolbarsub.AddButton("Graduation ", "GRADUATION");
            if (!IsPostBack)
            {
                BindCourse();
                ViewState["FAMILYID"] = "";             
                ucGender.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                ucNationality.SelectedNationality = "97";
                ddlColourBlindness.SelectedIndex = 2;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :  Method :

    protected void BindCourse()
    {
        DataTable dt = PhoenixPreSeaCourse.EditPreSeaCourse(null);
        ddlCourse.DataSource = dt;
        ddlCourse.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlCourse.Items.Insert(0, li);

        ddlBatch.Items.Insert(0, li);
        ddlBatch.DataBind();
    }

    protected void BindBatch(string course)
    {
        ddlBatch.DataSource = PhoenixPreSeaBatch.ListBatchforPlan(General.GetNullableInteger(course), null, null);
        ddlBatch.DataBind();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlBatch.Items.Insert(0, li);
    }

    public static bool IsValidName(string text)
    {

        string regex = "^[a-zA-Z. ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }

    private bool IsValidPrimaryDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        Int32 resultInt;

        if (int.TryParse(ddlCourse.SelectedValue, out resultInt) == false)
            ucError.ErrorMessage = "Course is required";

        if (int.TryParse(ddlBatch.SelectedValue, out resultInt) == false)
            ucError.ErrorMessage = "Batch is required";

        if (Int32.TryParse(ucHighestQualificaiton.SelectedQualification, out resultInt) == false)
            ucError.ErrorMessage = "Your Highest Qualification is required";

        if (txtFirstname.Text.Trim() == "")
            ucError.ErrorMessage = "First Name is required";
        if (!DateTime.TryParse(txtDateofBirth.Text, out resultDate))
        {
            ucError.ErrorMessage = "Date Of Birth is required";
        }
        else if (DateTime.TryParse(txtDateofBirth.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now.AddYears(-14)) > 0)
        {
            ucError.ErrorMessage = "Must be above a minimum age of 14 years and above.";
        }
        if (Int32.TryParse(ucNationality.SelectedNationality, out resultInt) == false)
            ucError.ErrorMessage = "Nationality is required";

        if (txtEmail.Text.Trim() == "")
            ucError.ErrorMessage = "Email is required";

        if (txtContact.Text.Trim() == "")
            ucError.ErrorMessage = "Contact No is required";

        if(string.IsNullOrEmpty(txtHeight.Text))
            ucError.ErrorMessage = "Height is required";

        if (string.IsNullOrEmpty(txtWeight.Text))
            ucError.ErrorMessage = "Weight is required";

        if (ddlColourBlindness.SelectedIndex <= 0)
            ucError.ErrorMessage = "Colour blindness is required";








        if (!IsValidName(txtFirstname.Text.Trim()) && !String.IsNullOrEmpty(txtFirstname.Text.Trim()))
            ucError.ErrorMessage = "Firstname should contain alphabets only";

        if (!IsValidName(txtMiddlename.Text.Trim()) && !String.IsNullOrEmpty(txtMiddlename.Text.Trim()))
            ucError.ErrorMessage = "Middlename should contain alphabets only";

        if (!IsValidName(txtLastname.Text.Trim()) && !String.IsNullOrEmpty(txtLastname.Text.Trim()))
            ucError.ErrorMessage = "Lastname should contain alphabets only";


        return (!ucError.IsError);
    }

    protected void UpdatePreSeaMainPersonalInformation()
    {
        try
        {
                int iEmployeeId = 0;
                Guid iDtkey = new Guid();
                PhoenixPreSeaNewApplicantPersonal.InsertPreSeaNewApplicantPersonal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , txtFirstname.Text
                                                                            , txtLastname.Text
                                                                            , txtMiddlename.Text
                                                                            , Convert.ToInt32(ucNationality.SelectedNationality)
                                                                            , Convert.ToDateTime(txtDateofBirth.Text)
                                                                            , ""
                                                                            , Convert.ToInt32(ucGender.SelectedHard)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableDecimal(txtHeight.Text)
                                                                            , General.GetNullableDecimal(txtWeight.Text)
                                                                            , null //appno
                                                                            , null //doj                              
                                                                            , Convert.ToInt32(ddlEyeSight.SelectedValue)
                                                                            , Convert.ToInt32(ddlColourBlindness.SelectedValue)
                                                                            , General.GetNullableString("")
                                                                            , 0
                                                                            , General.GetNullableString("")
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableString("")
                                                                            , General.GetNullableInteger(ddlBatch.SelectedValue)
                                                                            , General.GetNullableInteger(ddlCourse.SelectedValue)
                                                                            , General.GetNullableInteger("")
                                                                            , General.GetNullableInteger("")
                                                                            , txtEmail.Text.Trim()
                                                                            , txtContact.Text.Trim()
                                                                            , General.GetNullableInteger(ucHighestQualificaiton.SelectedQualification)
                                                                            , ref iEmployeeId
                                                                            , ref iDtkey);

                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('AddPreseaApplicant', null, null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Tabstrip Events   :

    protected void PreSeaApplication_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPrimaryDetails())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaMainPersonalInformation();
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Filter.CurrentPreSeaNewApplicantSelection = null;
                Response.Redirect(Request.RawUrl);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkSignOut_Click(object sender, EventArgs e)
    {
        Filter.CurrentPreSeaNewApplicantSelection = null;
        Response.Redirect(Request.RawUrl);
    }

    #endregion

    #region :   Other Events :

    protected void Course_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlCourse.SelectedValue).HasValue)
        {
            BindBatch(ddlCourse.SelectedValue);
        }
    }

    #endregion

}

