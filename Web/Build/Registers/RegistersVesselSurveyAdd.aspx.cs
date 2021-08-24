using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Text;
public partial class Registers_RegistersVesselSurveyAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuRegistersSurveyAdd.AccessRights = this.ViewState;
        MenuRegistersSurveyAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            BindSurveyType();
            BindLastVesselSurveyType();
            BindCategory();
            PopulateDetails(Request.QueryString["type"], Request.QueryString["SurveyId"], Request.QueryString["VesselId"]);
            txtSurvey.Focus();
            ddlSurveyType_SelectedIndexChanged(this, new EventArgs());
        }
    }
    private void BindSurveyType()
    {
        try
        {
            ddlSurveyType.DataSource = PhoenixRegistersVesselSurvey.SurveyTypeList(General.GetNullableInteger("1"));
            ddlSurveyType.DataValueField = "FLDSURVEYTYPEID";
            ddlSurveyType.DataTextField = "FLDSURVEYTYPENAME";
            ddlSurveyType.DataBind();
            ddlSurveyType.Items.Insert(0, new ListItem("--Select--", string.Empty));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindLastVesselSurveyType()
    {
        DataSet ds = PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlLastSurveyType.DataSource = ds;
            ddlLastSurveyType.DataValueField = "FLDSURVEYTYPEID";
            ddlLastSurveyType.DataTextField = "FLDSURVEYTYPENAME";
            ddlLastSurveyType.DataBind();
            ddlLastSurveyType.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }
    private void BindCategory()
    {
        try
        {
            lstCategory.DataSource = PhoenixRegistersVesselSurvey.CertificateCategoryList();
            lstCategory.DataValueField = "FLDCATEGORYID";
            lstCategory.DataTextField = "FLDCATEGORYNAME";
            lstCategory.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PopulateDetails(string type, string SurveyId, string VesselId)
    {
        DataSet ds = PhoenixRegistersVesselSurvey.EditVesselSurvey(General.GetNullableGuid(SurveyId));
        if (type.ToUpper() == "EDIT" && General.GetNullableGuid(SurveyId) != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtSurvey.Text = ds.Tables[0].Rows[0]["FLDSURVEYNAME"].ToString();
                txtFrequency.Text = ds.Tables[0].Rows[0]["FLDFREQUENCY"].ToString();
                ddlSurveyType.SelectedValue = ds.Tables[0].Rows[0]["FLDSURVEYTYPEID"].ToString();
                ucCommenceDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDCOMMENCEMENTDATE"].ToString());
                txtWinPeriod.Text = ds.Tables[0].Rows[0]["FLDWINDOWPERIODBEFORE"].ToString();
                txtPlusMinusPeriod.Text = ds.Tables[0].Rows[0]["FLDWINDOWPERIODAFTER"].ToString();
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                string strlist = "," + ds.Tables[0].Rows[0]["FLDCERTIFICATECATEGORYLIST"].ToString() + ",";
                foreach (ListItem item in lstCategory.Items)
                {
                    if (!item.Value.Equals(""))
                        item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                }
            }
        }
        else
        {
            txtSurvey.Text = string.Empty;
            txtFrequency.Text = string.Empty;
            ddlSurveyType.SelectedValue = string.Empty;
            ucCommenceDate.Text = string.Empty;
            txtWinPeriod.Text = string.Empty;
            txtPlusMinusPeriod.Text = string.Empty;
            txtVessel.Text = ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            ucCommenceDate.Text = (ucCommenceDate.Text == null && ddlSurveyType.SelectedValue == "5") ? ds.Tables[1].Rows[0]["FLDSTARTDATE"].ToString() : ucCommenceDate.Text;
        }
    }

    private void PopulateSurveyConfigDetails()
    {
        DataTable dt = PhoenixRegisterVesselSurveyConfiguration.EditSurveyConfiguration(
           PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dt.Rows.Count > 0)
        {
            ucCommenceDate.Text = dt.Rows[0]["FLDSTARTDATE"].ToString();
            ucLastSurveyDate.Text = dt.Rows[0]["FLDLASTSURVEYDATE"].ToString();
            ddlLastSurveyType.SelectedValue = dt.Rows[0]["FLDSURVEYTYPEID"].ToString();
        }
        else
        {
            ucCommenceDate.Text = "";
            ucLastSurveyDate.Text = "";
            ddlLastSurveyType.SelectedValue = "";
        }
    }
    
    protected void MenuRegistersSurveyAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strlist = new StringBuilder();
                foreach (ListItem item in lstCategory.Items)
                {
                    if (item.Selected == true && !item.Value.Equals(""))
                    {

                        strlist.Append(item.Value.ToString());
                        strlist.Append(",");
                    }

                }
                if (!IsValidSurvey(txtSurvey.Text, ddlSurveyType.SelectedValue, txtFrequency.Text, strlist.ToString()
                    , ucCommenceDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ddlSurveyType.SelectedValue == "5")
                {
                    if (!IsValidCofig(ddlLastSurveyType.SelectedValue, ucLastSurveyDate.Text, ucCommenceDate.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                if (Request.QueryString["type"].ToUpper() == "ADD")
                {
                    if (ddlSurveyType.SelectedValue == "5")
                    {
                        InsertSurveyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableInteger(ddlLastSurveyType.SelectedValue)
                        , General.GetNullableDateTime(ucCommenceDate.Text)
                        , General.GetNullableDateTime(ucLastSurveyDate.Text)
                        );
                    }
                    InsertQuestion(PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtSurvey.Text, ddlSurveyType.SelectedValue
                        , strlist.ToString()
                        , txtFrequency.Text
                        , ucCommenceDate.Text, txtWinPeriod.Text, txtPlusMinusPeriod.Text);
                   
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                }
                else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToUpper() == "EDIT"
                    && Request.QueryString["SurveyId"] != null && Request.QueryString["SurveyId"] != string.Empty)
                {
                    if (ddlSurveyType.SelectedValue == "5")
                    {
                        InsertSurveyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableInteger(ddlLastSurveyType.SelectedValue)
                        , General.GetNullableDateTime(ucCommenceDate.Text)
                        , General.GetNullableDateTime(ucLastSurveyDate.Text));
                    }

                    UpdateQuestion(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Request.QueryString["SurveyId"], txtSurvey.Text, ddlSurveyType.SelectedValue
                        , strlist.ToString()
                        , txtFrequency.Text
                        , ucCommenceDate.Text, txtWinPeriod.Text, txtPlusMinusPeriod.Text);

                    
                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSurvey(string SurveyName, string SurveyType, string Frequency, string CategoryList, string CommenceDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (SurveyType == string.Empty)
            ucError.ErrorMessage = "Survey Type is required.";

        if (SurveyName == string.Empty)
            ucError.ErrorMessage = "Survey is required.";
       
        if (string.IsNullOrEmpty(CommenceDate))
            ucError.ErrorMessage = "Commencement Date is required.";

        else if (General.GetNullableDateTime(CommenceDate) > DateTime.Today)
            ucError.ErrorMessage = "Commencement Date should be Earlier than Today's Date";

        return (!ucError.IsError);
    }

    private bool IsValidCofig(string SurveyType, string LastSurveyDate, string CommenceDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(CommenceDate))
            ucError.ErrorMessage = "Commencement date is required.";

        if ((DateTime.Parse(CommenceDate)).Year!=DateTime.Now.Year)
        {
            if (string.IsNullOrEmpty(LastSurveyDate))
                ucError.ErrorMessage = "Last Survey date is required.";

            else if (General.GetNullableDateTime(LastSurveyDate) > DateTime.Today)
                ucError.ErrorMessage = "Last Survey date should be Earlier than Today's Date";

            if (SurveyType == string.Empty)
                ucError.ErrorMessage = "Last Survey type is required.";
        }


        return (!ucError.IsError);
    }

    private void InsertQuestion(int VesselId, string SurveyName, string SurveyType, string CategoryList, string Frequency, string CommencementDate
        , string WindowPeriod, string PlusOrMInusPeriod)
    {
        PhoenixRegistersVesselSurvey.InsertVesselSurvey(VesselId
            , SurveyName
            , General.GetNullableInteger(SurveyType)
            , CategoryList
            , General.GetNullableInteger(Frequency)
            , General.GetNullableDateTime(CommencementDate)
            , General.GetNullableInteger(WindowPeriod)
            , General.GetNullableInteger(PlusOrMInusPeriod)
            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
             );
    }

    private void InsertSurveyConfiguration(int VesselId, int? SurveyTypeId, DateTime? StartDate, DateTime? LastSurveyDate)
    {

        //PhoenixRegisterVesselSurveyConfiguration.InsertSurveyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //    VesselId, SurveyTypeId, StartDate, LastSurveyDate);
    }

    private void UpdateQuestion(int VesselId, string SurveyId, string SurveyName, string SurveyType, string CategoryList, string Frequency, string CommencementDate
        , string WindowPeriod, string PlusOrMInusPeriod)
    {
        PhoenixRegistersVesselSurvey.UpdateVesselSurvey(
           VesselId
           , General.GetNullableGuid(SurveyId)
            , SurveyName
            , General.GetNullableInteger(SurveyType)
            , CategoryList
            , General.GetNullableInteger(Frequency)
            , General.GetNullableDateTime(CommencementDate)
            , General.GetNullableInteger(WindowPeriod)
            , General.GetNullableInteger(PlusOrMInusPeriod)
            , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

    }

    //protected void btnConfig_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("../Registers/RegistersVesselSurveyConfiguration.aspx?VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
    //}
    protected void ddlSurveyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixRegistersVesselSurvey.GetVesselSurveyFrequency(General.GetNullableInteger(ddlSurveyType.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                txtFrequency.Text = dt.Rows[0]["FLDFREQUENCY"].ToString();
                txtWinPeriod.Text = dt.Rows[0]["FLDWINDOWPERIODBEFORE"].ToString();
                txtPlusMinusPeriod.Text = dt.Rows[0]["FLDWINDOWPERIODAFTER"].ToString();
            }
            else
            {
                txtFrequency.Text = "";
                txtWinPeriod.Text = "";
                txtPlusMinusPeriod.Text = "";
            }
            if (!ddlSurveyType.SelectedValue.Equals("5"))//PERIODIC(ANL,IMS,SPL)
            {
                ucLastSurveyDate.CssClass = ddlLastSurveyType.CssClass = "readonlytextbox";
                ucLastSurveyDate.ReadOnly = true;
                ddlLastSurveyType.Enabled = false;
            }
            else
            {
                ucLastSurveyDate.CssClass = ddlLastSurveyType.CssClass = "input";
                ucLastSurveyDate.ReadOnly = false;
                ddlLastSurveyType.Enabled = true;
                PopulateSurveyConfigDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}
