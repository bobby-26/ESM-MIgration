using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Portal;

public partial class OptionsNewApplicant : PhoenixBasePage
{
    protected string Code = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Login", "LOGIN",ToolBarDirection.Right);
        MenuSecurityUser.Title = "Registration";

        MenuSecurityUser.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ddlGender.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;


            cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            cblVesselType.DataTextField = "FLDTYPEDESCRIPTION";
            cblVesselType.DataValueField = "FLDVESSELTYPEID";
            cblVesselType.DataBind();

            //lstVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            //lstVesselType.DataTextField = "FLDVESSELTYPECODE";
            //lstVesselType.DataValueField = "FLDVESSELTYPEID";
            //lstVesselType.DataBind();

            ddlGender.SelectedHard = "157";                 // defaulted to Male
            ddlNationality.SelectedNationality = "97";      // defaulted to Indian

            ucConfirm.Visible = false;

            GenerateAccessCode(null, null);

            lblManagement.Text = HttpContext.Current.Session["companyname"].ToString();
        }
    }
    protected string GetCompanyURL()
    {
        DataTable dt = PhoenixCommonAdministration.GetCompanyURL(2);

        if(dt.Rows.Count > 0)
        {
            string CompanyURL = dt.Rows[0]["FLDURL"].ToString();
            return CompanyURL;
        }
        else
        {
            return null;
        }
    }
    protected void SecurityUser_TabStripCommand(object sender, EventArgs e)
    {
        string password = "";
        string seajobsmailid = "";
        string strEmailTxt = "";

        DataSet ds = PhoenixRegistersHard.EditHardCode(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , 94, "SEA");

        if (ds.Tables[0].Rows.Count > 0)
        {
            seajobsmailid = ds.Tables[0].Rows[0]["FLDHARDNAME"].ToString();
        }

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
              
                if (!IsValidEntry())
                {
                    ucError.Visible = true;
                    GenerateAccessCode(null, null);
                    return;
                }

                StringBuilder strvesseltype = new StringBuilder();

                foreach (RadListBoxItem item in cblVesselType.Items)
                {
                    if (item.Selected == true)
                    {
                        strvesseltype.Append(item.Value.ToString());
                        strvesseltype.Append(",");
                    }
                }

                if (strvesseltype.Length > 1)
                {
                    strvesseltype.Remove(strvesseltype.Length - 1, 1);
                }
                Guid? dtkey = null;
                int employeeid = PhoenixNewApplicantManagement.NewApplicantRegister(
                                    1, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, txtEMail.Text,
                                    Int32.Parse(ddlNationality.SelectedNationality), DateTime.Parse(ucDateofBirth.Text)
                                    , int.Parse(ddlGender.SelectedHard), txtPassport.Text, txtSeamanBookNumber.Text, strvesseltype.ToString(), General.GetNullableInteger(ucRankApplied.SelectedRank)
                                    , ref password, ref dtkey);
                ViewState["dtkey"] = dtkey;
                if (Request.Files["txtFileUpload"].ContentLength > 0)
                {

                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.CREW, null, ".doc,.docx", string.Empty, "CREWRESUME");

                }
               
                //OTP Generation
                DataTable dt = PheonixSeafarerUserLogin.GetUserDetails(txtEMail.Text);
                if (dt.Rows.Count > 0)
                {
                    
                    string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                    string numbers = "1234567890";

                    string characters = numbers;
                    characters += alphabets + small_alphabets + numbers;

                    int length = 4;
                    string otp = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        string character = string.Empty;
                        do
                        {
                            int index = new Random().Next(0, characters.Length);
                            character = characters.ToCharArray()[index].ToString();
                        } while (otp.IndexOf(character) != -1);
                        otp += character;
                       
                    }
                    ViewState["otp"] = "";
                    ViewState["otp"] = otp;
                    PheonixSeafarerUserLogin.SeafarerLoginInsert(General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString()), otp, 1);
                }
                //end OTP generation
                strEmailTxt = @"<pre>Dear <FirstName> <LastName>

Thank you for registering your contact details with us. 

We would request you to take a few minutes to update your profile with the most recent 
information.

Our recruiters across all offices would look forward to get in touch with you.

Your OTP details:
OTP     :<OTP>
Click this link to verify your login details: <otplink>
 
Regards</pre>" + lblManagement.Text +

@"< pre >" + GetCompanyURL() +
 @"</pre>";
               string otplink= Session["sitepath"] + "/" + "Options/OptionsSeafarerCofirmOTP.aspx?email="+txtEMail.Text;

                PhoenixMail.SendMailFromPortal(txtEMail.Text, null, null, "Your application is registered.", strEmailTxt.Replace("<FirstName>", txtFirstName.Text).Replace("<LastName>", txtLastName.Text).Replace("<OTP>",ViewState["otp"].ToString()).Replace("<otplink>", otplink), true, System.Net.Mail.MailPriority.Normal, (new Guid()).ToString(),"1");
                //ucStatus.Text = "Your application is registered. Check your email for login information.";

                DataTable dsEmpNo = PhoenixNewApplicantManagement.NewApplicantList(employeeid);

                string employeenumber = dsEmpNo.Rows[0]["FLDFILENO"].ToString();

                strEmailTxt = @"<pre>The following seafarer has registered into the eLog system.

        Name            :   <FirstName> <LastName>

        Emp. No.        :   <EmpNo>
        
        Contact Details :   Phone  - <Phone>
                            E-Mail - <EMail></pre>";

                PhoenixMail.SendMailFromPortal(seajobsmailid, null, null, "New Applicant Registration Details"
                    , strEmailTxt.Replace("<FirstName>", txtFirstName.Text).Replace("<LastName>", txtLastName.Text).Replace("<EmpNo>", employeenumber).Replace("<Phone>", ucTelephone.Text).Replace("<EMail>", txtEMail.Text)
                    , true, System.Net.Mail.MailPriority.Normal, string.Empty,"1");

                ucConfirm.Visible = true;
                ucConfirm.Text = "Your application is registered. Check your email for login information.";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "New Applicant Registration";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

                strEmailTxt = @"<pre>The following seafarer was unable to register in the eLog system for the below reason.
        
        Error           :   " + ex.Message + @"


        Seafarer  Details
        Name            :   <FirstName> <LastName>

        CDC No.         :   <CDCNO>
        
        Contact Details :   Phone  - <Phone>
                            E-Mail - <EMail></pre>";

                //PhoenixMail.SendMail(seajobsmailid, null, null, "New Applicant Unsuccessful Registration"
                //    , strEmailTxt.Replace("<FirstName>", txtFirstName.Text).Replace("<LastName>", txtLastName.Text).Replace("<CDCNO>", txtSeamanBookNumber.Text).Replace("<Phone>", ucTelephone.Text).Replace("<EMail>", txtEMail.Text)
                //    , true, System.Net.Mail.MailPriority.Normal, string.Empty);

                GenerateAccessCode(null, null);
            }
        }
        if (CommandName.ToUpper().Equals("LOGIN"))
        {
            Response.Redirect("~/Default.aspx");
        }
    }
    protected void DeselectVesseltype(object sender, EventArgs e)
    {
        if (chkSeaFarerExp.Checked == true)
        {

            cblVesselType.SelectedValue = null;
            cblVesselType.Enabled = false;
            dvVesselType.Attributes["class"] = "input";
        }
        else
        {
            dvVesselType.Attributes["class"] = "input_mandatory";
            cblVesselType.Enabled = true;
        }

    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
        try
        {
            if (ucCM.confirmboxvalue == 0)
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        catch (Exception ex)
        {
            ucCM.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidEntry()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtFirstName.Text.Trim().Length == 0)
            ucError.ErrorMessage = "First name is required.";
        if (txtLastName.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Last name is required.";
        if (General.GetNullableInteger(ddlGender.SelectedHard) == null)
            ucError.ErrorMessage = "Gender is required";
        if (General.GetNullableInteger(ddlNationality.SelectedNationality) == null)
            ucError.ErrorMessage = "Nationality is required";
        if (txtPassport.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Passport Number is required.";
        if (txtSeamanBookNumber.Text.Trim().Length == 0)    
            ucError.ErrorMessage = "Seaman Book Number is required.";
        if (txtEMail.Text.Trim().Length == 0)
            ucError.ErrorMessage = "E-Mail is required.";
        if (General.GetNullableDateTime(ucDateofBirth.Text) == null)
            ucError.ErrorMessage = "Date of Birth is required.";
        if (General.GetNullableString(ucTelephone.Text) == null)
            ucError.ErrorMessage = "Mobile Number is required.";

        if (General.GetNullableInteger(ucRankApplied.SelectedRank) == null)
            ucError.ErrorMessage = "Applied Rank is required";

        if (txtAccessCode.Text.ToUpper() != Session["captcha"].ToString().ToUpper())
            ucError.ErrorMessage = "The characters you entered didn't match the word verification.";

        return (!ucError.IsError);
    }

    public void GenerateAccessCode(object sender, EventArgs e)
    {
        Random random = new Random();
        string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder captcha = new StringBuilder();
        for (int i = 0; i < 6; i++)
            captcha.Append(combination[random.Next(combination.Length)]);
        Session["captcha"] = captcha.ToString();
        imgCaptcha.ImageUrl = "../Common/CommonDrawImage.aspx?" + DateTime.Now.Ticks.ToString();
    }
}
