using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewAcademic : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //if (!IsPostBack)
        //{
        //    if (Request.QueryString["ACADEMICID"] != null)
        //    {
        //        EditCrewAcademics(Int16.Parse(Request.QueryString["ACADEMICID"].ToString()));
        //    }
        //}
        base.Render(writer);
    }

    string empid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"].ToString();
        if (!IsPostBack)
        {
            ViewState["EMPLOYEEACADEMICID"] = Request.QueryString["ACADEMICID"];
           
            if (Request.QueryString["ACADEMICID"] != null)
            {
                EditCrewAcademics(Int16.Parse(Request.QueryString["ACADEMICID"].ToString()));
            }
            ((RadComboBox)ddlCertificate.FindControl("ddlQualification")).Focus();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewAcademic.AccessRights = this.ViewState;
        MenuCrewAcademic.MenuList = toolbar.Show();
    }

    protected void EditCrewAcademics(int academicid)
    {
        DataTable dt = PhoenixCrewAcademicQualification.EditEmployeeAcademic(Convert.ToInt32(empid), academicid);

        if (dt.Rows.Count > 0)
        {
            txtInstitution.Text = dt.Rows[0]["FLDINSTITUTION"].ToString();

            ddlCertificate.SelectedQualification = dt.Rows[0]["FLDCERTIFICATE"].ToString();
            txtFromDate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
            txtToDate.Text = dt.Rows[0]["FLDTODATE"].ToString();
            txtPassDate.Text = dt.Rows[0]["FLDDATEOFPASS"].ToString();
            txtPercentage.Text = dt.Rows[0]["FLDPERCENTAGE"].ToString();
            txtGrade.Text = dt.Rows[0]["FLDGRADE"].ToString();
            ucCountry.SelectedCountry = dt.Rows[0]["FLDCOUNTRYID"].ToString();
            ucState.Country = dt.Rows[0]["FLDCOUNTRYID"].ToString();
            ucState.SelectedState = dt.Rows[0]["FLDSTATEID"].ToString();

            ddlPlace.SelectedCity = dt.Rows[0]["FLDPLACEOFINSTITUTION"].ToString();
        }
    }

    protected void ResetCrewAcademic()
    {
        txtInstitution.Text = "";
        ddlPlace.SelectedCity = "";
        ddlCertificate.SelectedQualification = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtPassDate.Text = "";
        txtPercentage.Text = "";
        txtGrade.Text = "";
    }

    protected void CrewAcademic_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        //string state = null;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (Request.QueryString["ACADEMICID"] != null)
                {
                    //state = ViewState["STATEID"].ToString();
                    //ddlPlace.SelectedCity = ViewState["PLACEID"].ToString();

                    if (IsValidAcademic(ddlPlace.SelectedCity, txtFromDate.Text, txtToDate.Text, ddlCertificate.SelectedQualification,
                        txtPassDate.Text, txtPercentage.Text, txtGrade.Text))
                    {
                        PhoenixCrewAcademicQualification.UpdateEmployeeAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , Convert.ToInt32(empid)
                                                        , Convert.ToInt32(ViewState["EMPLOYEEACADEMICID"].ToString())
                                                        , General.GetNullableString(txtInstitution.Text)
                                                        , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                        , Convert.ToDateTime(txtFromDate.Text)
                                                        , Convert.ToDateTime(txtToDate.Text)
                                                        , Convert.ToInt32(ddlCertificate.SelectedQualification)
                                                        , Convert.ToDateTime(txtPassDate.Text)
                                                        , Convert.ToDouble(txtPercentage.Text)
                                                        , txtGrade.Text
                                                        , General.GetNullableInteger((ucCountry.SelectedCountry))
                                                        , General.GetNullableInteger((ucState.SelectedState))
                                                        );
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }

                }
                else
                {
                    if (IsValidAcademic(ddlPlace.SelectedCity, txtFromDate.Text, txtToDate.Text, ddlCertificate.SelectedQualification,
                            txtPassDate.Text, txtPercentage.Text, txtGrade.Text))
                    {

                        PhoenixCrewAcademicQualification.InsertEmployeeAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                      , Convert.ToInt32(empid)
                                                      , General.GetNullableString(txtInstitution.Text)
                                                      , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                      , Convert.ToDateTime(txtFromDate.Text)
                                                      , Convert.ToDateTime(txtToDate.Text)
                                                      , Convert.ToInt32(ddlCertificate.SelectedQualification)
                                                      , Convert.ToDateTime(txtPassDate.Text)
                                                      , General.GetNullableDecimal(txtPercentage.Text)
                                                      , txtGrade.Text
                                                      , General.GetNullableInteger((ucCountry.SelectedCountry))
                                                      , General.GetNullableInteger((ucState.SelectedState))
                                                      );

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                        ResetCrewAcademic();
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAcademic(string placeofinstitution, string fromdate, string todate, string certificate, string passingdate, string percentage, string grade)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        Int32 result;

        if (certificate == "Dummy" || certificate.Trim() == "")
        {
            ucError.ErrorMessage = "Qualification is required";
        }
        if (General.GetNullableString(txtInstitution.Text) == null)
        {
            ucError.ErrorMessage = "Institution is required";
        }
        if (General.GetNullableDateTime(fromdate) == null)
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (General.GetNullableDateTime(todate) == null)
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (General.GetNullableDateTime(fromdate) != null
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        if (General.GetNullableDateTime(passingdate) == null)
        {
            ucError.ErrorMessage = "Pass Date is required";
        }
        else if (DateTime.TryParse(passingdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Pass Date should be earlier than current date";
        }
        else if (General.GetNullableDateTime(todate) != null
            && DateTime.TryParse(passingdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "Pass Date should be later than 'To Date'";
        }

        if (string.IsNullOrEmpty(percentage) && string.IsNullOrEmpty(grade))
        {
            ucError.ErrorMessage = "Either Percentage or Grade  is required";
        }

        if (int.TryParse(ddlPlace.SelectedCity, out result) == false)
            ucError.ErrorMessage = "Place of institution is required";

        return (!ucError.IsError);
    }

    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.Country = ucCountry.SelectedCountry;
        ucState.StateList = PhoenixRegistersState.ListState(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(ucCountry.SelectedCountry));
        if (IsPostBack)
            ((RadComboBox)ucCountry.FindControl("ddlCountry")).Focus();
    }

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlPlace.State = ucState.SelectedState;
        ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger(ucCountry.SelectedCountry) == null ? 0 : General.GetNullableInteger(ucCountry.SelectedCountry)
            , General.GetNullableInteger(ucState.SelectedState) == null ? 0 : General.GetNullableInteger(ucState.SelectedState));
        //if (IsPostBack)
        //    ((RadComboBox)ucState.FindControl("ddlState")).Focus();
    }
}

