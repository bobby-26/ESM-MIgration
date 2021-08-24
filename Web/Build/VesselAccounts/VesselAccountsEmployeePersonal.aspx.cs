using System;
using System.Data;
using System.Drawing;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeePersonal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            CrewMainPersonal.AccessRights = this.ViewState;
            CrewMainPersonal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Filter.CurrentVesselCrewSelection != null)
                {
                    ListCrewInformation();
                    CalculateBMI(null, null);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void ListCrewInformation()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToInt32(Filter.CurrentVesselCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtGender.Text = dt.Rows[0]["FLDSEXNAME"].ToString();
                txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtPool.Text = dt.Rows[0]["FLDPOOLNAME"].ToString();
                txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                txtPlaceofBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
                txtMaritialStatus.Text = dt.Rows[0]["FLDMARITALSTATUSNAME"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtZone.Text = dt.Rows[0]["FLDZONENAME"].ToString();
                txtHeight.Text = General.GetNullableDecimal(dt.Rows[0]["FLDHEIGHT"].ToString()).HasValue ? Convert.ToInt64(Math.Floor(Convert.ToDouble(dt.Rows[0]["FLDHEIGHT"].ToString()))).ToString() : string.Empty;
                txtWeight.Text = General.GetNullableDecimal(dt.Rows[0]["FLDWEIGHT"].ToString()).HasValue ? Convert.ToInt64(Math.Floor(Convert.ToDouble(dt.Rows[0]["FLDWEIGHT"].ToString()))).ToString() : string.Empty;
                txtShoes.Text = String.Format("{0:#,##,###.00}", dt.Rows[0]["FLDSHOESCMS"].ToString());
                txtRankApplied.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                txtRankPosted.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                txtDateofJoin.Text = dt.Rows[0]["FLDDATEOFJOINING"].ToString();
                txtHairColor.Text = dt.Rows[0]["FLDHAIRCOLOR"].ToString();
                txtEyeColor.Text = dt.Rows[0]["FLDEYECOLOR"].ToString();
                txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();
                txtINDOsNumber.Text = dt.Rows[0]["FLDINDOSNO"].ToString();
                txtBMI.Text = dt.Rows[0]["FLDBMI"].ToString();
                txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();


                //ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                //DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                //ViewState["dtkey"] = string.Empty;
                //if (dta.Rows.Count > 0)
                //{
                //    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                //    imgPhoto.ImageUrl = HttpContext.Current.Session["sitepath"] + "/attachments/" + dta.Rows[0]["FLDFILEPATH"].ToString();
                //    aCrewImg.HRef = "#";
                //    aCrewImg.Attributes["onclick"] = "javascript:parent.Openpopup('codehelp1', '', '" + imgPhoto.ImageUrl + "');";
                //}                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CalculateBMI(object sender, EventArgs e)
    {
        string desc = string.Empty;
        decimal wt, ht, bmi;
        if (decimal.TryParse(txtWeight.Text, out wt) && decimal.TryParse(txtHeight.Text, out ht))
        {
            if (txtWeight.Text != "0.00" && txtHeight.Text != "0.00")
            {
                bmi = Math.Round(wt / ((ht / 100) * (ht / 100)), 0);
                txtBMI.Text = bmi.ToString();
                if (bmi < 20)
                    desc = "Underweight";
                else if (bmi >= 20 && bmi <= 24)
                    desc = "Normal Weight";
                else if (bmi >= 25 && bmi <= 29)
                    desc = "Light to Moderate Overweight";
                else if (bmi >= 30 && bmi <= 39)
                    desc = "Strong Overweight";
                else if (bmi >= 40)
                    desc = "Extreme Overweight";
                if (!string.IsNullOrEmpty(txtDateofBirth.Text))
                {
                    TimeSpan s = DateTime.Now.Subtract(DateTime.Parse(txtDateofBirth.Text));
                    int age = s.Days / 365;
                    if (age >= 19 && age <= 24)
                        desc += ", Desireable BMI : 19 - 24";
                    if (age >= 25 && age <= 34)
                        desc += ", Desireable BMI : 20 - 24";
                    if (age >= 35 && age <= 44)
                        desc += ", Desireable BMI : 22 - 27";
                    if (age >= 45 && age <= 54)
                        desc += ", Desireable BMI : 23 - 28";
                    if (age >= 65)
                        desc += ", Desireable BMI : 24 - 29";
                }
                txtBMI.ToolTip = desc;
                txtBMI.BackColor = BMIColor(bmi);

            }
            else
            {
                txtBMI.BackColor = Color.Empty;
                txtBMI.CssClass = "readonlytextbox";
                txtBMI.Text = "";
            }
        }
    }
    private Color BMIColor(decimal iBMI)
    {
        Color c = new Color();
        if (iBMI >= 20 && iBMI <= 24)
            c = System.Drawing.Color.FromArgb(255, 255, 255);
        else if ((iBMI >= 19 && iBMI < 20) || (iBMI > 24 && iBMI <= 28))
            c = System.Drawing.Color.FromArgb(255, 255, 153);
        else if ((iBMI > 16 && iBMI <= 18) || (iBMI > 28 && iBMI <= 32))
            c = System.Drawing.Color.FromArgb(255, 204, 153);
        else if (iBMI <= 16 || iBMI > 32)
            c = System.Drawing.Color.FromArgb(255, 0, 0);
        return c;
    }
 
}
