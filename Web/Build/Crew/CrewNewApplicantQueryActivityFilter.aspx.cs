using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewNewApplicantQueryActivityFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);

        MenuActivityFilterMain.AccessRights = this.ViewState;
        MenuActivityFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            btnSubmit.Attributes.Add("style", "display:none;");

            txtName.Focus();
            //if (Request.QueryString["launchedfrom"] == null || Request.QueryString["launchedfrom"].ToString() == "")
            //{
            //    ddlCountry.SelectedCountry = "97";
            //    ddlState.Country = "97";
            //}
            ddlInActive.SelectedValue = "1";
            if (ddlInActive.SelectedValue == "1")
            {
                lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , 54, 1, "ONB,ONL,NWA,TCL");
                lstStatus.DataBind();
            }
            else if (ddlInActive.SelectedValue == "0")
            {
                lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , 96, 1, string.Empty);
                lstStatus.DataBind();
            }


            BindFilterCriteria();
        }


    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }


        txtPassortNo.Text = (nvc.Get("txtPassortNo") == null) ? "" : nvc.Get("txtPassortNo").ToString();
        txtSeamanbookNo.Text = (nvc.Get("txtSeamanbookNo") == null) ? "" : nvc.Get("txtSeamanbookNo").ToString();
        txtFileNumber.Text = (nvc.Get("txtFileNumber") == null) ? "" : nvc.Get("txtFileNumber").ToString();
        txtAppliedStartDate.Text = (nvc.Get("txtAppliedStartDate") == null) ? "" : nvc.Get("txtAppliedStartDate").ToString();
        txtAppliedEndDate.Text = (nvc.Get("txtAppliedEndDate") == null) ? "" : nvc.Get("txtAppliedEndDate").ToString();
        txtDOAStartDate.Text = (nvc.Get("txtDOAStartDate") == null) ? "" : nvc.Get("txtDOAStartDate").ToString();
        txtDOAEndDate.Text = (nvc.Get("txtDOAEndDate") == null) ? "" : nvc.Get("txtDOAEndDate").ToString();
        //txtDOBStartDate.Text = (nvc.Get("txtDOBStartDate") == null) ? "" : nvc.Get("txtDOBStartDate").ToString();
        //txtDOBEndDate.Text = (nvc.Get("txtDOBEndDate") == null) ? "" : nvc.Get("txtDOBEndDate").ToString();
        txtName.Text = (nvc.Get("txtName") == null) ? "" : nvc.Get("txtName").ToString();

        ddlPrevCompany.SelectedOtherCompany = (General.GetNullableInteger(nvc.Get("ddlPrevCompany")) == null) ? "0" : nvc.Get("ddlPrevCompany").ToString();
        ddlInActive.SelectedValue = (General.GetNullableInteger(nvc.Get("ddlInActive")) == null) ? "1" : nvc.Get("ddlInActive").ToString();
        //ddlCountry.SelectedCountry = (General.GetNullableInteger(nvc.Get("ddlCountry")) == null) ? "97" : nvc.Get("ddlCountry").ToString();
        //ddlState.SelectedState = (General.GetNullableInteger(nvc.Get("ddlState")) == null) ? "" : nvc.Get("ddlState").ToString();
        //ddlCity.SelectedCity = (General.GetNullableInteger(nvc.Get("ddlCity")) == null) ? "" : nvc.Get("ddlCity").ToString();
        //ddlCreatedBy.SelectedUser = (General.GetNullableInteger(nvc.Get("ddlCreatedBy")) == null) ? "" : nvc.Get("ddlCreatedBy").ToString();
        ddlZone.SelectedZone = (General.GetNullableInteger(nvc.Get("ddlZone")) == null) ? "" : nvc.Get("ddlZone").ToString();
        ddlSailedRank.SelectedRank = (General.GetNullableInteger(nvc.Get("SelectedRank")) == null) ? "" : nvc.Get("ddlSailedRank").ToString();
        ddlEngineType.SelectedEngineName = (General.GetNullableInteger(nvc.Get("ddlEngineType")) == null) ? "" : nvc.Get("ddlEngineType").ToString();
        ddlCourse.SelectedDocument = (General.GetNullableInteger(nvc.Get("ddlCourse")) == null) ? "" : nvc.Get("ddlCourse").ToString();
        ddlLicences.SelectedDocument = (General.GetNullableInteger(nvc.Get("ddlLicences")) == null) ? "" : nvc.Get("ddlLicences").ToString();
        ddlVisa.SelectedCountry = (General.GetNullableInteger(nvc.Get("ddlVisa")) == null) ? "" : nvc.Get("ddlVisa").ToString();

        lstNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
        lstNationality.SelectedList = (nvc.Get("lstNationality") == null) ? "" : nvc.Get("lstNationality").ToString();

        ddlVesselTypeList.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
        ddlVesselTypeList.SelectedVesseltype = (nvc.Get("ddlVesselType") == null) ? "" : nvc.Get("ddlVesselType").ToString();

        lstRank.RankList = PhoenixRegistersRank.ListRank();
        lstRank.selectedlist = (nvc.Get("lstRank") == null) ? "" : nvc.Get("lstRank").ToString();

        PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 54, 1, "ONB,ONL,NWA");
        if (nvc.Get("ddlStatus") != null)
        {
            string strlist = "," + nvc.Get("ddlStatus").ToString() + ",";
            foreach (RadListBoxItem item in lstStatus.Items)
            {
                item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
        else
        {
            lstStatus.SelectedValue = null;
        }
        if (nvc.Get("ddlInActive") == "2")
        {
            lstStatus.Items.Clear();
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("txtName", txtName.Text);
        criteria.Add("txtFileNumber", txtFileNumber.Text);
        criteria.Add("txtRefNumber", string.Empty);
        criteria.Add("txtPassortNo", txtPassortNo.Text);
        criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);
        criteria.Add("ddlSailedRank", ddlSailedRank.SelectedRank);
        criteria.Add("ddlVesselType", ddlVesselTypeList.SelectedVesseltype);
        criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
        criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
        criteria.Add("txtDOAStartDate", txtDOAStartDate.Text);
        criteria.Add("txtDOAEndDate", txtDOAEndDate.Text);
        criteria.Add("txtDOBStartDate", string.Empty);
        criteria.Add("txtDOBEndDate", string.Empty);
        criteria.Add("ddlCourse", ddlCourse.SelectedDocument);
        criteria.Add("ddlLicences", ddlLicences.SelectedDocument);
        criteria.Add("ddlEngineType", ddlEngineType.SelectedEngineName);
        criteria.Add("ddlVisa", ddlVisa.SelectedCountry);
        criteria.Add("ddlZone", ddlZone.SelectedZone);
        criteria.Add("lstRank", lstRank.selectedlist);
        criteria.Add("lstNationality", lstNationality.SelectedList);
        criteria.Add("ddlPrevCompany", ddlPrevCompany.SelectedOtherCompany);
        criteria.Add("ddlStatus", StatusSelectedList());
        criteria.Add("ddlInActive", ddlInActive.SelectedValue);
        criteria.Add("ddlCountry", string.Empty);
        criteria.Add("ddlState", string.Empty);
        criteria.Add("ddlCity", string.Empty);
        criteria.Add("ddlCreatedBy", string.Empty);

        Filter.CurrentNewApplicantFilterCriteria = criteria;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
    }

    protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNumber", txtFileNumber.Text);
            criteria.Add("txtRefNumber", string.Empty);
            criteria.Add("txtPassortNo", txtPassortNo.Text);
            criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);
            criteria.Add("ddlSailedRank", ddlSailedRank.SelectedRank);
            criteria.Add("ddlVesselType", ddlVesselTypeList.SelectedVesseltype);
            criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
            criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
            criteria.Add("txtDOAStartDate", txtDOAStartDate.Text);
            criteria.Add("txtDOAEndDate", txtDOAEndDate.Text);
            criteria.Add("txtDOBStartDate", string.Empty);
            criteria.Add("txtDOBEndDate", string.Empty);
            criteria.Add("ddlCourse", ddlCourse.SelectedDocument);
            criteria.Add("ddlLicences", ddlLicences.SelectedDocument);
            criteria.Add("ddlEngineType", ddlEngineType.SelectedEngineName);
            criteria.Add("ddlVisa", ddlVisa.SelectedCountry);
            criteria.Add("ddlZone", ddlZone.SelectedZone);
            criteria.Add("lstRank", lstRank.selectedlist);
            criteria.Add("lstNationality", lstNationality.SelectedList);
            criteria.Add("ddlPrevCompany", ddlPrevCompany.SelectedOtherCompany);
            criteria.Add("ddlStatus", StatusSelectedList());
            criteria.Add("ddlInActive", ddlInActive.SelectedValue);
            criteria.Add("ddlCountry", string.Empty);
            criteria.Add("ddlState", string.Empty);
            criteria.Add("ddlCity", string.Empty);
            criteria.Add("ddlCreatedBy", string.Empty);

            Filter.CurrentNewApplicantFilterCriteria = criteria;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentNewApplicantFilterCriteria = null;
            BindFilterCriteria();
        }
        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }


    }
    protected void ddlInActive_SelectedIndex(object sender, EventArgs e)
    {
        if (ddlInActive.SelectedValue == "1")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, "ONB,ONL,NWA,TCL");
            lstStatus.DataBind();
        }
        else if (ddlInActive.SelectedValue == "0")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 96, 1, string.Empty);
            lstStatus.DataBind();
        }
        else if (ddlInActive.SelectedValue == "2")
        {
            lstStatus.Items.Clear();

        }
        else
        {
            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , 54, 1, "ONB,ONL,NWA,TCL");
            ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , 96, 1, string.Empty).Tables[0]);
            lstStatus.DataSource = ds;
            lstStatus.DataBind();
        }
    }
    //protected void ddlCountry_TextChanged(object sender, EventArgs e)
    //{
    //    ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
    //    ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), null);
    //}
    //protected void ddlState_TextChanged(object sender, EventArgs e)
    //{
    //    ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));
    //}
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
}
