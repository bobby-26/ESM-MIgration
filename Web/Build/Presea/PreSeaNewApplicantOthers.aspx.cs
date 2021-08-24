using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaNewApplicantOthers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            toolbarAddress.AddButton("Save", "SAVE");
            PreSeaOthersMain.AccessRights = this.ViewState;
            PreSeaOthersMain.MenuList = toolbarAddress.Show();
            if (!IsPostBack)
            {
                
                ViewState["OTHERSID"] = null;              
                if (Filter.CurrentPreSeaNewApplicantSelection != null)
                {
                    BindCheckBoxValues();
                    SetEmployeePrimaryDetails();
                    ListPreSeaExamVenue();
                    ListPreSeaOthers();
                    ucCountry_TextChanged("97", null);
                    ddlState_TextChanged(null, null);
                    country.Visible = false;
                }
                ucNewspaperMagazine.QuickTypeCode = ((int)(PhoenixQuickTypeCode.NEWSPAPERMAGAZINE)).ToString();
                ucSchoolCollage.QuickTypeCode = ((int)(PhoenixQuickTypeCode.SCHOOLCOLLEGE)).ToString();
                ucDirectContact.QuickTypeCode = ((int)(PhoenixQuickTypeCode.DIRECTCONTACT)).ToString();
            }
            if (!IsPostBack)
                ddlState_TextChanged(null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void ListPreSeaExamVenue()
    {        
        DataSet ds = PhoenixPreSeaExamVenue.SearchBatchExamVenueList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
        if (ds.Tables[0].Rows.Count > 0)
        {
            rblExamVenueFirst.DataSource = ds.Tables[0];
            rblExamVenueFirst.DataBind();
            rblExamVenueSecond.DataSource = ds.Tables[0];
            rblExamVenueSecond.DataBind();
        }
    }
    public void ListPreSeaOthers()
    {
        DataTable dt = PhoenixPreSeaNewApplicantOthers.PreSeaNewApplicantOtherDetailsList(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection));
        if (dt.Rows.Count > 0)
        {
            ucNewspaperMagazine.SelectedQuick = dt.Rows[0]["FLDNEWSPAPERMAGAZINE"].ToString();
            ucSchoolCollage.SelectedQuick = dt.Rows[0]["FLDSCHOOLCOLLEGE"].ToString();

            ucState.SelectedState = dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString();

            ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger("97")
            , General.GetNullableInteger(dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString()) == null ? 0 : General.GetNullableInteger(dt.Rows[0]["FLDEDUCATIONJOBFAIRSTATE"].ToString()));

            ddlPlace.SelectedCity = dt.Rows[0]["FLDEDUCATIONJOBFAIRCITY"].ToString(); 
            txtInternet.Text = dt.Rows[0]["FLDINTERNET"].ToString();
            ucDirectContact.SelectedQuick = dt.Rows[0]["FLDDIRECTCONTACT"].ToString();
            txtOthers.Text = dt.Rows[0]["FLDOTHERS"].ToString();
            ViewState["OTHERSID"] = dt.Rows[0]["FLDOTHERSID"].ToString();

            rblExamVenueFirst.SelectedValue = dt.Rows[0]["FLDEXAMVENUE1"].ToString();
            rblExamVenueSecond.SelectedValue = dt.Rows[0]["FLDEXAMVENUE2"].ToString();
            txtAboutYourselfRemarks.Text = dt.Rows[0]["FLDABOUTYOURSELFREMARKS"].ToString();

            string[] knowntype = dt.Rows[0]["FLDINSTITUTEKNOWNTYPE"].ToString().Split(',');

            foreach (string item in knowntype)
            {
                if (item != "")
                {
                    if (chkNewspaperMagazine.InputAttributes["value"] == item.ToString())
                        chkNewspaperMagazine.Checked = true;
                    if (chkFamilyRelativeFriends.InputAttributes["value"] == item.ToString())
                        chkFamilyRelativeFriends.Checked = true;
                    if (chkSchoolCollege.InputAttributes["value"] == item.ToString())
                        chkSchoolCollege.Checked = true;
                    if (chkEducationJoFfair.InputAttributes["value"] == item.ToString())
                        chkEducationJoFfair.Checked = true;
                    if (chkEmailBySims.InputAttributes["value"] == item.ToString())
                        chkEmailBySims.Checked = true;
                    if (chkShiksha.InputAttributes["value"] == item.ToString())
                        chkShiksha.Checked = true;
                    if (chkInternet.InputAttributes["value"] == item.ToString())
                        chkInternet.Checked = true;
                    if (chkFlyers.InputAttributes["value"] == item.ToString())
                        chkFlyers.Checked = true;
                    if (chkDirectContact.InputAttributes["value"] == item.ToString())
                        chkDirectContact.Checked = true;
                    if (chkOthers.InputAttributes["value"] == item.ToString())
                        chkOthers.Checked = true;
                }
            }
        }

    }
    public void BindCheckBoxValues()
    {
        chkNewspaperMagazine.InputAttributes.Add("value", "1217");
        chkFamilyRelativeFriends.InputAttributes.Add("value", "1218");
        chkSchoolCollege.InputAttributes.Add("value", "1219");
        chkEducationJoFfair.InputAttributes.Add("value", "1220");
        chkEmailBySims.InputAttributes.Add("value", "1221");
        chkShiksha.InputAttributes.Add("value", "1222");
        chkInternet.InputAttributes.Add("value", "1223");
        chkFlyers.InputAttributes.Add("value", "1224");
        chkDirectContact.InputAttributes.Add("value", "1225");
        chkOthers.InputAttributes.Add("value", "1226");
    }
    protected void PreSeaOthersMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                StringBuilder strknowntype = new StringBuilder();

                if (chkNewspaperMagazine.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkNewspaperMagazine.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkFamilyRelativeFriends.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkFamilyRelativeFriends.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkSchoolCollege.Checked == true)
                {

                    strknowntype = strknowntype.Append(chkSchoolCollege.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkEducationJoFfair.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkEducationJoFfair.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkEmailBySims.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkEmailBySims.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkShiksha.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkShiksha.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkInternet.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkInternet.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkFlyers.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkFlyers.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkDirectContact.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkDirectContact.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (chkOthers.Checked == true)
                {
                    strknowntype = strknowntype.Append(chkOthers.InputAttributes["value"]);
                    strknowntype.Append(",");
                }
                if (ViewState["OTHERSID"] != null)
                {
                    PhoenixPreSeaNewApplicantOthers.UpdatePreSeaNewApplicantOtherDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableInteger(ucNewspaperMagazine.SelectedQuick)
                                                             , General.GetNullableInteger(ucSchoolCollage.SelectedQuick)
                                                             , General.GetNullableInteger(ucState.SelectedState)
                                                             , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                             , txtInternet.Text
                                                             , General.GetNullableInteger(ucDirectContact.SelectedQuick)
                                                             , txtOthers.Text
                                                             , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                             , General.GetNullableInteger(ViewState["OTHERSID"].ToString())
                                                             , General.GetNullableInteger(rblExamVenueFirst.SelectedValue.ToString())
                                                             , General.GetNullableInteger(rblExamVenueSecond.SelectedValue.ToString())
                                                             , General.GetNullableString(txtAboutYourselfRemarks.Text)
                                                             , strknowntype.ToString());
                }
                else
                {
                    PhoenixPreSeaNewApplicantOthers.InertPreSeaNewApplicantOtherDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableInteger(ucNewspaperMagazine.SelectedQuick)
                                                             , General.GetNullableInteger(ucSchoolCollage.SelectedQuick)
                                                             , General.GetNullableInteger(ucState.SelectedState)
                                                             , General.GetNullableInteger(ddlPlace.SelectedCity)
                                                             , txtInternet.Text
                                                             , General.GetNullableInteger(ucDirectContact.SelectedQuick)
                                                             , txtOthers.Text
                                                             , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                             , General.GetNullableInteger(rblExamVenueFirst.SelectedValue.ToString())
                                                             , General.GetNullableInteger(rblExamVenueSecond.SelectedValue.ToString())
                                                             , General.GetNullableString(txtAboutYourselfRemarks.Text)
                                                             , strknowntype.ToString());
                }
                ListPreSeaOthers();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void InstituteKnownTypeCheckedAdd(string id)
    {
        StringBuilder str = new StringBuilder();

    }
    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.Country = "97";
        ucState.StateList = PhoenixRegistersState.ListState(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(ucCountry.SelectedCountry));
        if (IsPostBack)
            ((DropDownList)ucCountry.FindControl("ddlCountry")).Focus();
    }

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlPlace.State = ucState.SelectedState;
        ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger("97")
            , General.GetNullableInteger(ucState.SelectedState) == null ? 0 : General.GetNullableInteger(ucState.SelectedState));
        if (IsPostBack)
            ((DropDownList)ucState.FindControl("ddlState")).Focus();
    }  
}
