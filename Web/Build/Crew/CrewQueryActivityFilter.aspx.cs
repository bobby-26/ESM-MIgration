using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;

public partial class Crew_CrewQueryActivityFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("style", "border:0px;background-color:White;visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

            MenuActivityFilterMain.MenuList = toolbar.Show();           
            //ddlCountry.SelectedCountry = "97";
            //ddlState.Country = "97";
            //ddlInActive.SelectedValue = "";
            rblInActive_SelectedIndex(null, null);

        }

    }



    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        if (nvc != null && !IsPostBack)
        {
            txtName.Text = (nvc.Get("txtName") == null) ? "" : nvc.Get("txtName").ToString();
            txtFileNumber.Text = (nvc.Get("txtFileNumber") == null) ? "" : nvc.Get("txtFileNumber").ToString();
            //txtRefNumber.Text = (nvc.Get("txtRefNumber") == null) ? "" : nvc.Get("txtRefNumber").ToString();
            txtPassortNo.Text = (nvc.Get("txtPassortNo") == null) ? "" : nvc.Get("txtPassortNo").ToString();
            txtSeamanbookNo.Text = (nvc.Get("txtSeamanbookNo") == null) ? "" : nvc.Get("txtSeamanbookNo").ToString();
            txtNOKName.Text = (nvc.Get("txtNOKName") == null) ? "" : nvc.Get("txtNOKName").ToString();
            txtAppliedStartDate.Text = (nvc.Get("txtAppliedStartDate") == null) ? "" : nvc.Get("txtAppliedStartDate").ToString();
            txtAppliedEndDate.Text = (nvc.Get("txtAppliedEndDate") == null) ? "" : nvc.Get("txtAppliedEndDate").ToString();
            txtDOAStartDate.Text = (nvc.Get("txtDOAStartDate") == null) ? "" : nvc.Get("txtDOAStartDate").ToString();
            txtDOAEndDate.Text = (nvc.Get("txtDOAEndDate") == null) ? "" : nvc.Get("txtDOAEndDate").ToString();
            txtDOBStartDate.Text = (nvc.Get("txtDOBStartDate") == null) ? "" : nvc.Get("txtDOBStartDate").ToString();
            txtDOBEndDate.Text = (nvc.Get("txtDOBEndDate") == null) ? "" : nvc.Get("txtDOBEndDate").ToString();

            ddlSailedRank.SelectedRank = (General.GetNullableInteger(nvc.Get("ddlSailedRank")) == null) ? "" : nvc.Get("ddlSailedRank").ToString();
            ddlVesselType.SelectedVesseltype = (General.GetNullableInteger(nvc.Get("ddlVesselType")) == null) ? "" : nvc.Get("ddlVesselType").ToString();
            ddlCourse.SelectedDocument = (General.GetNullableInteger(nvc.Get("ddlCourse")) == null) ? "" : nvc.Get("ddlCourse").ToString();
            ddlLicences.SelectedDocument = (General.GetNullableInteger(nvc.Get("ddlLicences")) == null) ? "" : nvc.Get("ddlLicences").ToString();
            ddlEngineType.SelectedEngineName = (General.GetNullableInteger(nvc.Get("ddlEngineType")) == null) ? "" : nvc.Get("ddlEngineType").ToString();
            ddlVisa.SelectedCountry = (General.GetNullableInteger(nvc.Get("ddlVisa")) == null) ? "" : nvc.Get("ddlVisa").ToString();
            ddlZone.SelectedZone = (General.GetNullableInteger(nvc.Get("ddlZone")) == null) ? "" : nvc.Get("ddlZone").ToString();
            ddlPrevCompany.SelectedOtherCompany = (General.GetNullableInteger(nvc.Get("ddlPrevCompany")) == null) ? "" : nvc.Get("ddlPrevCompany").ToString();
            ddlCountry.SelectedCountry = (General.GetNullableInteger(nvc.Get("ddlCountry")) == null) ? "" : nvc.Get("ddlCountry").ToString();
            ddlState.SelectedState = (General.GetNullableInteger(nvc.Get("ddlState")) == null) ? "" : nvc.Get("ddlState").ToString();
            ddlCity.SelectedCity = (General.GetNullableInteger(nvc.Get("ddlCity")) == null) ? "" : nvc.Get("ddlCity").ToString();

            if ((nvc.Get("ddlInActive") != "Dummy") && (nvc.Get("ddlInActive") != null))
                ddlInActive.SelectedValue = nvc.Get("ddlInActive").ToString();
            else
                ddlInActive.SelectedValue = "";

            if ((nvc.Get("ddlVessel") != "Dummy") && (nvc.Get("ddlVessel") != null))
                ddlVessel.SelectedVessel = nvc.Get("ddlVessel").ToString();
            else
                ddlVessel.SelectedVessel = "";

            lstNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
            lstNationality.SelectedList = (nvc.Get("lstNationality") == null) ? "" : nvc.Get("lstNationality").ToString();

            ddlVesselType.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
            ddlVesselType.SelectedVesseltype = (nvc.Get("ddlVesselType") == null) ? "" : nvc.Get("ddlVesselType").ToString();

            lstRank.RankList = PhoenixRegistersRank.ListRank();
            lstRank.selectedlist = (nvc.Get("lstRank") == null) ? "" : nvc.Get("lstRank").ToString();

            PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 54, 1, "NWA,OBP,OLP,ONB");
            if (nvc.Get("ddlStatus") != null)
            {
                string strlist = "," + nvc.Get("ddlStatus").ToString() + ",";
                foreach (RadListBoxItem item in lstStatus.Items)
                {

                    item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                    item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
                }
            }
            else
            {
                lstStatus.SelectedValue = null;
            }
            if ((nvc.Get("ddlPool") != "Dummy") && (nvc.Get("ddlPool") != null))
            {
                ddlPool.PoolList = PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster();
                ddlPool.SelectedPool = nvc.Get("ddlPool").ToString();
            }
            else
            {
                ddlPool.SelectedPoolValue = "";
            }

            if ((nvc.Get("lstBatch") != "Dummy") && (nvc.Get("lstBatch") != null))
            {
                lstBatch.BatchList = PhoenixRegistersBatch.ListBatch(General.GetNullableInteger(null), General.GetNullableInteger(""), 1);
                lstBatch.SelectedList = nvc.Get("lstBatch").ToString();
            }
            else
            {
                lstBatch.SelectedList = "";
            }

            if ((nvc.Get("chkIncludepastexp") != "") && (nvc.Get("chkIncludepastexp") != null))
                chkIncludepastexp.Checked = (nvc.Get("chkIncludepastexp").ToString() == "0") ? false : true;
            else
            {
                chkIncludepastexp.Checked = false;
            }

            if ((nvc.Get("ddlFlag") != "Dummy") && (nvc.Get("ddlFlag") != null))
                ddlFlag.SelectedFlag = nvc.Get("ddlFlag").ToString();
            else
                ddlFlag.SelectedFlag = "";

            if ((nvc.Get("ucPrincipal") != "Dummy") && (nvc.Get("ucPrincipal") != null))
                ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal").ToString();
            else
            {
                ucPrincipal.SelectedAddress = "";
            }

            if ((nvc.Get("ucPrincipalntbr") != "Dummy") && (nvc.Get("ucPrincipalntbr") != null))
                ucPrincipalntbr.SelectedAddress = nvc.Get("ucPrincipalntbr").ToString();
            else
            {
                ucPrincipalntbr.SelectedAddress = "";
            }
        }
    }

    protected void BindClear()
    {
        txtName.Text = string.Empty;
        txtFileNumber.Text = string.Empty;
        txtPassortNo.Text = string.Empty;
        txtSeamanbookNo.Text = string.Empty;
        ddlSailedRank.SelectedRank = string.Empty;
        ddlVesselType.SelectedVesseltype = string.Empty;
        txtAppliedStartDate.Text = string.Empty;
        txtAppliedEndDate.Text = string.Empty;
        txtDOAStartDate.Text = string.Empty;
        txtDOAEndDate.Text = string.Empty;
        txtDOBStartDate.Text = string.Empty;
        txtDOBEndDate.Text = string.Empty;
        ddlCourse.SelectedDocument = string.Empty;
        ddlLicences.SelectedDocument = string.Empty;
        ddlEngineType.SelectedEngineName = string.Empty;
        ddlVisa.SelectedCountry = string.Empty;
        ddlZone.SelectedZone = string.Empty;
        lstRank.selectedlist = string.Empty;
        lstNationality.SelectedList = string.Empty;
        ddlPrevCompany.SelectedOtherCompany = string.Empty;
        ddlInActive.SelectedValue = string.Empty;
        ddlCountry.SelectedCountry = string.Empty;
        ddlState.SelectedState = string.Empty;
        ddlCity.SelectedCity = string.Empty; 
        txtNOKName.Text = string.Empty;
        ddlPool.SelectedPool = string.Empty;
        ddlVessel.SelectedVessel = string.Empty;
        lstBatch.SelectedList = string.Empty;
        ddlFlag.SelectedFlag = string.Empty;
        ucPrincipal.SelectedAddress = string.Empty;
        ucPrincipalntbr.SelectedAddress = string.Empty;
        chkIncludepastexp.Checked = false;
        lstStatus.SelectedValue = ""; 
        lstStatus.ClearSelection();
        lstStatus.ClearChecked();

        Filter.CurrentNewApplicantFilterCriteria = null;

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript1'>" ;
        Script += "fnReloadList();";
        Script += "</script>" ;

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("txtName", txtName.Text);
        criteria.Add("txtFileNumber", txtFileNumber.Text);
        criteria.Add("txtRefNumber", string.Empty);
        criteria.Add("txtPassortNo", txtPassortNo.Text);
        criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);
        criteria.Add("ddlSailedRank", ddlSailedRank.SelectedRank);
        criteria.Add("ddlVesselType", ddlVesselType.SelectedVesseltype);
        criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
        criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
        criteria.Add("txtDOAStartDate", txtDOAStartDate.Text);
        criteria.Add("txtDOAEndDate", txtDOAEndDate.Text);
        criteria.Add("txtDOBStartDate", txtDOBStartDate.Text);
        criteria.Add("txtDOBEndDate", txtDOBEndDate.Text);
        criteria.Add("ddlCourse", ddlCourse.SelectedDocument);
        criteria.Add("ddlLicences", ddlLicences.SelectedDocument);
        criteria.Add("ddlEngineType", ddlEngineType.SelectedEngineName);
        criteria.Add("ddlVisa", ddlVisa.SelectedCountry);
        criteria.Add("ddlZone", ddlZone.SelectedZone);
        criteria.Add("lstRank", lstRank.selectedlist);
        criteria.Add("lstNationality", lstNationality.SelectedList);
        criteria.Add("ddlPrevCompany", ddlPrevCompany.SelectedOtherCompany);
        criteria.Add("ddlActiveYN", ddlInActive.SelectedValue);
        criteria.Add("ddlStatus", StatusSelectedList());
        criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
        criteria.Add("ddlState", ddlState.SelectedState);
        criteria.Add("ddlCity", ddlCity.SelectedCity);
        criteria.Add("txtNOKName", txtNOKName.Text);
        criteria.Add("chkIncludepastexp", chkIncludepastexp.Checked == true ? "1" : "0");
        criteria.Add("ddlPool", General.GetNullableString(ddlPool.SelectedPool) != null ? ddlPool.SelectedPool : string.Empty);
        criteria.Add("ddlVessel", General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue ? ddlVessel.SelectedVessel : string.Empty);
        criteria.Add("lstBatch", lstBatch.SelectedList);
        criteria.Add("ddlFlag", ddlFlag.SelectedFlag);
        criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
        criteria.Add("ucPrincipalntbr", ucPrincipalntbr.SelectedAddress);
        Filter.CurrentNewApplicantFilterCriteria = criteria;
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript1", Script, false);
        
    }

    protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>" ;
        Script += "fnReloadList();";
        Script += "</script>" ;

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }

            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNumber", txtFileNumber.Text);
            criteria.Add("txtRefNumber", string.Empty);
            criteria.Add("txtPassortNo", txtPassortNo.Text);
            criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);
            criteria.Add("ddlSailedRank", ddlSailedRank.SelectedRank);
            criteria.Add("ddlVesselType", ddlVesselType.SelectedVesseltype);
            criteria.Add("txtAppliedStartDate", txtAppliedStartDate.Text);
            criteria.Add("txtAppliedEndDate", txtAppliedEndDate.Text);
            criteria.Add("txtDOAStartDate", txtDOAStartDate.Text);
            criteria.Add("txtDOAEndDate", txtDOAEndDate.Text);
            criteria.Add("txtDOBStartDate", txtDOBStartDate.Text);
            criteria.Add("txtDOBEndDate", txtDOBEndDate.Text);
            criteria.Add("ddlCourse", ddlCourse.SelectedDocument);
            criteria.Add("ddlLicences", ddlLicences.SelectedDocument);
            criteria.Add("ddlEngineType", ddlEngineType.SelectedEngineName);
            criteria.Add("ddlVisa", ddlVisa.SelectedCountry);
            criteria.Add("ddlZone", ddlZone.SelectedZone);
            criteria.Add("lstRank", lstRank.selectedlist);
            criteria.Add("lstNationality", lstNationality.SelectedList);
            criteria.Add("ddlPrevCompany", ddlPrevCompany.SelectedOtherCompany);
            criteria.Add("ddlActiveYN", ddlInActive.SelectedValue);
            criteria.Add("ddlStatus", StatusSelectedList());
            criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
            criteria.Add("ddlState", ddlState.SelectedState);
            criteria.Add("ddlCity", ddlCity.SelectedCity);
            criteria.Add("txtNOKName", txtNOKName.Text);
            criteria.Add("chkIncludepastexp", chkIncludepastexp.Checked == true ? "1" : "0");
            criteria.Add("ddlPool", General.GetNullableString(ddlPool.SelectedPool) != null ? ddlPool.SelectedPool : string.Empty);
            criteria.Add("ddlVessel", General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue ? ddlVessel.SelectedVessel : string.Empty);
            criteria.Add("lstBatch", lstBatch.SelectedList);
            criteria.Add("ddlFlag", ddlFlag.SelectedFlag);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ucPrincipalntbr", ucPrincipalntbr.SelectedAddress);
            Filter.CurrentNewApplicantFilterCriteria = criteria;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);




        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {

            BindClear();
            
        }
        if (CommandName.ToUpper().Equals("CANCEL"))
        {

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }

        
    }
    protected void rblInActive_SelectedIndex(object sender, EventArgs e)
    {
        if (ddlInActive.SelectedValue == "1")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, string.Empty);
            lstStatus.DataBind();
        }
        else if (ddlInActive.SelectedValue == "0")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP");
            lstStatus.DataBind();
        }
        else
        {
            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , 54, 1, string.Empty);
            ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP").Tables[0]);

            lstStatus.DataSource = ds;
            lstStatus.DataBind();
        }
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
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Checked == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlFlag.SelectedFlag) != null)
        {
            if (General.GetNullableInteger(ddlLicences.SelectedDocument) == null)
                ucError.ErrorMessage = "Licence is required.";
        }

        return (!ucError.IsError);
    }
    
}
