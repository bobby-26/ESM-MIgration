using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewPersonalDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
              

            if (!IsPostBack)
            {
                 ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                 if (Request.QueryString["employeeid"] != null)
               {

                   ListCrewInformation(Request.QueryString["employeeid"].ToString() );
                   SetEmployeePassportDetails(Request.QueryString["employeeid"].ToString());
                }
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     

    protected void CrewMainPersonal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    public void ListCrewInformation(string empolyeeid)
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(empolyeeid));
            if (dt.Rows.Count > 0)
            {
                txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ucSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
                txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                ucNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
                ucRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();
                txtCDCNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetEmployeePassportDetails(string empolyeeid)
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeePassport(Convert.ToInt32(empolyeeid));

        if (dt.Rows.Count > 0)
        {
            //txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
            ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
            //ucECNR.SelectedHard = dt.Rows[0]["FLDECNRYESNO"].ToString();
            
        }

    }
}

   
