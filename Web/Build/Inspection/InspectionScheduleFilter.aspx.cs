using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionScheduleFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionScheduleMaster.aspx", false);
        }
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["Filter"] = null;

            VesselConfiguration();
            ucVessel.Enabled = true;
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            BindAttendingSupt();
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
            }
            GetInspectionCompanyList();
            Bind_UserControls(sender, new EventArgs());
        }
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
    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();
            string status = "";
            if (General.GetNullableInteger(ucStatus.SelectedHard) == null || ucStatus.SelectedHard.ToString().ToUpper() == "Dummy")
                status = ViewState["Filter"]!=null ? ViewState["Filter"].ToString() : null;
            else
                status = ucStatus.SelectedHard;
            criteria.Add("ucInspectionType",PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,148,"INS"));
            criteria.Add("ucInspectionCategory", ucInspectionCategory.SelectedHard);
            criteria.Add("ucInspection", ddlInspection.SelectedValue);
            criteria.Add("ucPort", ucPort.SelectedValue);
            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ucStatus",status);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ddlCompany", ddlCompany.SelectedValue);
            criteria.Add("txtInspector", txtInspector.Text);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucCharterer", ucCharterer.SelectedAddress);
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("ddlAttendingSupt", ddlAttendingSupt.SelectedValue);
            criteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucChapter", ucChapter.SelectedChapter);
            criteria.Add("Isrejected", chkRejection.Checked  == true ? "1" : "0");

            Filter.CurrentInspectionScheduleFilterCriteria = criteria;

            //Response.Redirect("../Inspection/InspectionScheduleMaster.aspx", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void GetInspectionCompanyList()
    {
        DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindInspection()
    {        
        ucInspection.InspectionType = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ucInspection.InspectionList = PhoenixInspection.ListInspection(
                                       General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS"))
                                       , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                       , null
                                       );
        ucInspection.DataBind();
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ddlInspection_Changed(object sender, EventArgs e)
    {
        BindChapter();
    }

    protected void BindChapter()
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 148, "INS")),
            General.GetNullableInteger(ucInspectionCategory.SelectedHard),
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }

    protected void BindAttendingSupt()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlAttendingSupt.DataSource = ds;
        ddlAttendingSupt.DataTextField = "FLDDESIGNATIONNAME";
        ddlAttendingSupt.DataValueField = "FLDUSERCODE";
        ddlAttendingSupt.DataBind();
        ddlAttendingSupt.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
