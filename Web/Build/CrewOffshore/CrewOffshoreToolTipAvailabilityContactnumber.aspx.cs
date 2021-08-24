using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;

public partial class CrewOffshore_CrewOffshoreToolTipAvailabilityContactnumber : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["employeeid"] != null)
            {
                ViewState["employeeid"] = Request.QueryString["employeeid"];
                BindContactNumber();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private void BindContactNumber()
    {
        if (Request.QueryString["employeeid"] != null && !string.IsNullOrEmpty(Request.QueryString["employeeid"].ToString()))
        {
            DataTable dt = PhoenixNewApplicantManagement.ListEmployeeAddress(int.Parse(ViewState["employeeid"].ToString()));
            
            
                //lblprmISDNumber11.Text = dt.Rows[0]["FLDISDCODE"].ToString();
                lblprmPhoneNumber11.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                //lblprmISDNumber21.Text=dt.Rows[0]["FLDISDCODE"].ToString();
                lblprmPhoneNumber21.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                lblprmMobileNumber11.Text=dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                lblprmMobileNumber21.Text=dt.Rows[0]["FLDMOBILENUMBER2"].ToString();

                //lbllclISDNumber12.Text=dt.Rows[1]["FLDISDCODE"].ToString();
                lbllclPhoneNumber12.Text=dt.Rows[1]["FLDPHONENUMBER"].ToString();
                //lbllclISDNumber22.Text = dt.Rows[1]["FLDISDCODE"].ToString();
                lbllclPhoneNumber22.Text=dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                lbllclMobileNumber12.Text=dt.Rows[1]["FLDMOBILENUMBER"].ToString();
                lbllclMobileNumber22.Text=dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
           
        }
    }

}
