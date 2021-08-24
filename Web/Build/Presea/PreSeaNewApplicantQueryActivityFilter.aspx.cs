using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaNewApplicantQueryActivityFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");
        MenuActivityFilterMain.AccessRights = this.ViewState;
        MenuActivityFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("style", "border:0px;background-color:White");
            ddlSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
            cblApplicantStaus.DataSource =PhoenixPreSeaNewApplicantManagement.ListPreSeaApplicantStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , "NAP,INV,REC,NRC,CON,REJ,WTL");            
            cblApplicantStaus.DataBind();
            txtName.Focus();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        string strApplicantStatus = "";

        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("txtName", txtName.Text);
        criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
        criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
        criteria.Add("txtDOBStartDate", txtDOBStartDate.Text);
        criteria.Add("txtDOBEndDate", txtDOBEndDate.Text);
        criteria.Add("lstNationality", lstNationality.SelectedList);
        criteria.Add("ddlSex", ddlSex.SelectedHard);
        criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
        criteria.Add("ddlState", ddlState.SelectedState);
        criteria.Add("ddlCity", ddlCity.SelectedCity);
        criteria.Add("ucBatch", ucBatch.SelectedBatch);
        criteria.Add("ddlQualificaiton", ddlQualificaiton.SelectedQualification);
        criteria.Add("ucPreSeaCourse", ucPreSeaCourse.SelectedCourse);
        criteria.Add("ucExamVenue1", ucExamVenue1.SelectedExamVenue);
        criteria.Add("ucExamVenue2", ucExamVenue2.SelectedExamVenue);

        foreach (ListItem item in cblApplicantStaus.Items)
        {
            if (item.Selected == true)
                strApplicantStatus = strApplicantStatus + ",";
        }
        criteria.Add("strApplicantStatus", strApplicantStatus);
        criteria.Add("txtRecordedByName", txtRecordedByName.Text);
        criteria.Add("rblRecordedBy", rblRecordedBy.SelectedValue);
        criteria.Add("txtRecordedByName", txtRecordedByName.Text);

        Filter.CurrentPreSeaNewApplicantFilterCriteria = criteria;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
    }
    protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        string strApplicantStatus = "";

        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
            criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
            criteria.Add("txtDOBStartDate", txtDOBStartDate.Text);
            criteria.Add("txtDOBEndDate", txtDOBEndDate.Text);
            criteria.Add("lstNationality", lstNationality.SelectedList);
            criteria.Add("ddlSex", ddlSex.SelectedHard);
            criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
            criteria.Add("ddlState", ddlState.SelectedState);
            criteria.Add("ddlCity", ddlCity.SelectedCity);
            criteria.Add("ucBatch", ucBatch.SelectedBatch);
            criteria.Add("ddlQualificaiton", ddlQualificaiton.SelectedQualification);
            criteria.Add("ucPreSeaCourse", ucPreSeaCourse.SelectedCourse);
            criteria.Add("ucExamVenue1", ucExamVenue1.SelectedExamVenue);
            criteria.Add("ucExamVenue2", ucExamVenue2.SelectedExamVenue);

            foreach (ListItem item in cblApplicantStaus.Items)
            {
                if (item.Selected == true)
                {                    
                    strApplicantStatus = strApplicantStatus + item.Value + ",";
                }
            }
            criteria.Add("strApplicantStatus", strApplicantStatus);            
            criteria.Add("rblRecordedBy", rblRecordedBy.SelectedValue);
            criteria.Add("txtRecordedByName", txtRecordedByName.Text);

            Filter.CurrentPreSeaNewApplicantFilterCriteria = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), null);
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));
    }
    protected void rblRecordedBy_IndexChanged(object sender, EventArgs e)
    {
        if (rblRecordedBy.SelectedValue == "0")
        {
            lblName.Visible = true;
            txtRecordedByName.Visible = true;
        }
        if (rblRecordedBy.SelectedValue == "1")
        {
            lblName.Visible = false;
            txtRecordedByName.Text = "";
            txtRecordedByName.Visible = false;
        }
    }
}

