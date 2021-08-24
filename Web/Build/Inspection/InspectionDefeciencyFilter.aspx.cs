using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionDefeciencyFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionDeficiencyList.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuDefeciencyFilter.AccessRights = this.ViewState;
        MenuDefeciencyFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            VesselConfiguration();
            ucVessel.Enabled = true;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }
            BindInspection();
            DateTime now = DateTime.Now;
            ucFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
            ucToDate.Text = DateTime.Now.ToShortDateString();
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucFleet.Enabled = false;
                chkOfficeAuditDeficiencies.Enabled = false;
            }

            ucStatus.ShortNameFilter = "OPN,CLD,CAD,REV,CMP";
            //chkOfficeAuditDeficiencies.Enabled = false;            
        }
    }

    protected void MenuDefeciencyFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();

            criteria.Add("txtRefNo", txtRefNo.Text.Trim());
            criteria.Add("txtFromDate", ucFromDate.Text);
            criteria.Add("txtToDate", ucToDate.Text);
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucDefeciencyType", ddlNCType.SelectedValue);
            criteria.Add("txtSourceRefNo", txtSourceRefNo.Text);
            criteria.Add("chkRCAReqd", ((cblRCA.Items[0].Selected) ? "1" : null));
            criteria.Add("chkRCACompleted", ((cblRCA.Items[1].Selected) ? "1" : null));
            criteria.Add("chkRCAPending", ((cblRCA.Items[2].Selected) ? "1" : null));
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddressType", ucAddrOwner.SelectedAddress);
            criteria.Add("ucCategory", ucNonConformanceCategory.SelectedQuick);
            criteria.Add("ucFleet",ucFleet.SelectedFleet);
            criteria.Add("ucInspectionType", ucInspectionType.SelectedHard);
            criteria.Add("ucInspectionCategory", ucInspectionCategory.SelectedHard);
            criteria.Add("ucInspection", ddlInspection.SelectedValue);
            criteria.Add("ucChapter",ucChapter.SelectedChapter);
            criteria.Add("txtKey", txtKey.Text);
            criteria.Add("ddlSource", ddlSource.SelectedValue == "0" ? null : ddlSource.SelectedValue);
            criteria.Add("chkOfficeAuditDeficiencies",(chkOfficeAuditDeficiencies.Checked.Equals(true)? "1" :"0"));
            criteria.Add("ucCompany", ucCompany.SelectedCompany);

            Filter.CurrentDeficiencyFilter = criteria;
            Filter.CurrentSelectedDeficiency = null;
            //Response.Redirect("../Inspection/InspectionDeficiencyList.aspx", false);
        }
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucAddrOwner_Changed(object sender, EventArgs e)
    {
        if (ViewState["COMPANYID"] != null && ViewState["COMPANYID"].ToString() != "")
        {
            ucVessel.Company = ViewState["COMPANYID"].ToString();
            ucVessel.Owner = General.GetNullableString(ucAddrOwner.SelectedAddress);
            ucVessel.bind();
        }
    }

    protected void ucInspectionType_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }

    protected void ucInspectionCategory_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }

    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(ucInspectionType.SelectedHard), 
            General.GetNullableInteger(ucInspectionCategory.SelectedHard),
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }
    protected void chkOfficeAuditDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAuditDeficiencies.Checked.Equals(true))
            ucCompany.Enabled = true;
        else
            ucCompany.Enabled = false;
    }

    protected void BindInspection()
    {
        //ucInspection.InspectionCategory = ucInspectionCategory.SelectedHard;
        //ucInspection.InspectionType = ucInspectionType.SelectedHard;        
        //ucInspection.InspectionList = PhoenixInspection.ListInspection(General.GetNullableInteger(ucInspectionType.SelectedHard),
        //    General.GetNullableInteger(ucInspectionCategory.SelectedHard), null);

        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
