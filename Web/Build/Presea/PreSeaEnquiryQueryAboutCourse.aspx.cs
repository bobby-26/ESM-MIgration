using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using SouthNests.Phoenix.Inspection;

public partial class PreSeaEnquiryQueryAboutCourse : PhoenixBasePage
{
    protected string Code = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidEntry())
            {
                ucError.Visible = true;
                return;
            }
            SendMail();            
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Enquiry /Query about courses ";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendMail()
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append("Dear Sir/Madam");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("COURSE : " + ucCourse.SelectedName);                
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();        
        sbemailbody.AppendLine();
        sbemailbody.Append(txtQueryRemarks.Text);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thanks and Regards,");
        sbemailbody.AppendLine(txtName.Text);

        PhoenixMail.SendMail("admission.sims@samundra.com"
              , txtEmailId.Text.Trim()
              , ""
              , txtName.Text + " - " + txtEmailId.Text 
              , sbemailbody.ToString(), false
              , System.Net.Mail.MailPriority.Normal
              , ""
              , null);
        ucStatus.Visible = true;
        ucStatus.Text = "Email Sent";
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("../PreSea/PreSeaRegisterdCandidatesLogin.aspx");
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucCourse.SelectedCourse = "";
        txtName.Text = "";
        ucDOB.Text = "";
        txtContact.Text = "";
        txtMobileNo.Text = "";
        txtEmailId.Text = "";
        txtQueryRemarks.Text = "";
    }

    private bool IsValidEntry()
    {
        DateTime outdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCourse.SelectedCourse) == null)
            ucError.ErrorMessage = "Course is required.";

        if (General.GetNullableString(txtName.Text)== null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableDateTime(ucDOB.Text) == null)
            ucError.ErrorMessage = "Date of Birth(DOB) is required.";
        else if(DateTime.TryParse(ucDOB.Text,out outdate) && DateTime.Compare(outdate,DateTime.Today) >= 0)
            ucError.ErrorMessage = "Date of Birth(DOB) should be less than current date";

        if(txtContact.Text.Length == 1)
            ucError.ErrorMessage = "Contact No. is required";        
        else if(!txtContact.IsValidPhoneNumber())
            ucError.ErrorMessage = "Enter area code for phone number";

        if (General.GetNullableString(txtMobileNo.Text) == null)
            ucError.ErrorMessage = "Mobile No. is required";
        else if(!General.IsValidPhoneNumber(txtMobileNo.Text))
            ucError.ErrorMessage = "Please enter valid phoneno";

        if (General.GetNullableString(txtEmailId.Text) == null)
            ucError.ErrorMessage = "Email Address is required";
        else if(!General.IsvalidEmail(txtEmailId.Text))
            ucError.ErrorMessage = "Please enter valid e-mail1";
        

        if (General.GetNullableString(txtQueryRemarks.Text) == null)
            ucError.ErrorMessage = "Questions / Comments is required";        

        return (!ucError.IsError);
    }
}
