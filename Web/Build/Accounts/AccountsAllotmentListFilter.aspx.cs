using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Web.Profile;
using Telerik.Web.UI;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class Accounts_AccountsAllotmentListFilter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
          //  btnSubmit.Attributes.Add("style", "border:0px;background-color:White;visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
          
            toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

            ucRequestStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 238, "PDA");

            MenuActivityFilterMain.MenuList = toolbar.Show();
          
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.CurrentAccountsAllotmentRequestFilter;
        ViewState["REQUESTSTATUS"]= PhoenixCommonRegisters.GetHardCode(1, 238, "PDA");

        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        if (nvc != null && !IsPostBack)
        {

            ddlVessel.SelectedVessel = (General.GetNullableInteger(nvc.Get("ddlVessel")) == null) ? "" : nvc.Get("ddlVessel").ToString();
            ddlMonth.SelectedMonth = (General.GetNullableInteger(nvc.Get("ddlMonth")) == null) ? "" : nvc.Get("ddlMonth").ToString();
            ddlyear.SelectedYearinstr = (General.GetNullableInteger(nvc.Get("ddlyear")) == null) ? "" : nvc.Get("ddlyear").ToString();
            ucZone.SelectedZone = (General.GetNullableInteger(nvc.Get("ucZone")) == null) ? "" : nvc.Get("ucZone").ToString();
            ddlCurrencyCode.SelectedCurrency = (General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) == null) ? "" : nvc.Get("ddlCurrencyCode").ToString();
            ucTechFleet.SelectedFleet = (General.GetNullableInteger(nvc.Get("ucTechFleet")) == null) ? "" : nvc.Get("ucTechFleet").ToString();
            ucOwner.SelectedAddress = (General.GetNullableInteger(nvc.Get("ucOwner")) == null) ? "" : nvc.Get("ucOwner").ToString();
            ddlCountry.SelectedCountry = (General.GetNullableInteger(nvc.Get("ddlCountry")) == null) ? "" : nvc.Get("ddlCountry").ToString();
            ucNationality.SelectedNationality = (General.GetNullableInteger(nvc.Get("ucNationality")) == null) ? "" : nvc.Get("ucNationality").ToString();
            ddlAllotmentType.SelectedHard = (General.GetNullableInteger(nvc.Get("ddlAllotmentType")) == null) ? "" : nvc.Get("ddlAllotmentType").ToString();
            ucPool.SelectedPool = (General.GetNullableInteger(nvc.Get("ucPool")) == null) ? "" : nvc.Get("ucPool").ToString();
            ucRequestStatus.SelectedHard = (General.GetNullableInteger(nvc.Get("ucRequestStatus")) == null) ? ViewState["REQUESTSTATUS"].ToString() : nvc.Get("ucRequestStatus").ToString();

         

            if ((nvc.Get("ddlVessel") != "Dummy") && (nvc.Get("ddlVessel") != null))
                ddlVessel.SelectedVessel = nvc.Get("ddlVessel").ToString();
            else
                ddlVessel.SelectedVessel = "";

            if ((nvc.Get("ucZone") != "Dummy") && (nvc.Get("ucZone") != null))
                ucZone.SelectedZone = nvc.Get("ucZone").ToString();
            else
                ucZone.SelectedZone = "";

            if ((nvc.Get("ddlCurrencyCode") != "Dummy") && (nvc.Get("ddlCurrencyCode") != null))
                ddlCurrencyCode.SelectedCurrency = nvc.Get("ddlCurrencyCode").ToString();
            else
                ddlCurrencyCode.SelectedCurrency = "";

            if ((nvc.Get("ucTechFleet") != "Dummy") && (nvc.Get("ucTechFleet") != null))
                ucTechFleet.SelectedFleet = nvc.Get("ucTechFleet").ToString();
            else
                ucTechFleet.SelectedFleet = "";

            if ((nvc.Get("ucOwner") != "Dummy") && (nvc.Get("ucOwner") != null))
                ucOwner.SelectedAddress = nvc.Get("ucOwner").ToString();
            else
                ucOwner.SelectedAddress = "";


            if ((nvc.Get("ddlCountry") != "Dummy") && (nvc.Get("ddlCountry") != null))
                ddlCountry.SelectedCountry = nvc.Get("ddlCountry").ToString();
            else
                ddlCountry.SelectedCountry = "";

            if ((nvc.Get("ucNationality") != "Dummy") && (nvc.Get("ucNationality") != null))
                ucNationality.SelectedNationality = nvc.Get("ucNationality").ToString();
            else
                ucNationality.SelectedNationality = "";

            if ((nvc.Get("ddlAllotmentType") != "Dummy") && (nvc.Get("ddlAllotmentType") != null))
                ddlAllotmentType.SelectedHard = nvc.Get("ddlAllotmentType").ToString();
            else
                ddlAllotmentType.SelectedHard = "";

            if ((nvc.Get("ucRequestStatus") != "Dummy") && (nvc.Get("ucRequestStatus") != null))
                ucRequestStatus.SelectedHard = nvc.Get("ucRequestStatus").ToString();
            else
                ucRequestStatus.SelectedHard = ViewState["REQUESTSTATUS"].ToString();

            if ((nvc.Get("ucPool") != "Dummy") && (nvc.Get("ucPool") != null))
                ucPool.SelectedPool = nvc.Get("ucPool").ToString();
            else
                ucPool.SelectedPool = "";

        }
    }
    protected void BindClear()
    {
        ddlVessel.SelectedVessel = string.Empty;
        ddlMonth.SelectedMonth = string.Empty;
        ddlyear.SelectedYearinstr = string.Empty;
        ucZone.SelectedZone = string.Empty;
        ddlCurrencyCode.SelectedCurrency = string.Empty;
        ucTechFleet.SelectedFleet = string.Empty;
        ucOwner.SelectedAddress = string.Empty;
        ddlCountry.SelectedCountry = string.Empty;
        ucNationality.SelectedNationality = string.Empty;
        ddlAllotmentType.SelectedHard = string.Empty;
        ucPool.SelectedPool = string.Empty;
        ucRequestStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 238, "PDA");

        Filter.CurrentAccountsAllotmentRequestFilter = null;

    }
      protected void AccountsAllotmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ddlVessel", General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue ? ddlVessel.SelectedVessel : string.Empty);
            criteria.Add("ddlMonth", ddlMonth.SelectedMonth);
            criteria.Add("ddlyear", ddlyear.SelectedYearinstr);
            criteria.Add("ucZone", ucZone.SelectedZone);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucOwner", ucOwner.SelectedAddress);
            criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
            criteria.Add("ucNationality", ucNationality.SelectedNationality);
            criteria.Add("ddlAllotmentType", ddlAllotmentType.SelectedHard);
            criteria.Add("ucRequestStatus", ucRequestStatus.SelectedHard);
            criteria.Add("ucPool", ucPool.SelectedPool);
            
          
            Filter.CurrentAccountsAllotmentRequestFilter = criteria;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);




        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {

            BindClear();

        }
     

    }
    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

     
            if (General.GetNullableInteger(ucRequestStatus.SelectedHard) == null)
                ucError.ErrorMessage = "Request Status is required.";
       

        return (!ucError.IsError);
    }
}