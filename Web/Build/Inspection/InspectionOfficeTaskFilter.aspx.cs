using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionOfficeTaskFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            DateTime now = DateTime.Now;
            uccartargetdatefrom.Text = now.Date.AddMonths(-3).ToShortDateString();
            uccartargetdateto.Text = now.Date.AddMonths(+3).ToShortDateString();

            //DateTime now1 = DateTime.Now;
            //txtFrom.Text = now.Date.AddMonths(-3).ToShortDateString();
            //txtTo.Text = now.Date.AddMonths(+3).ToShortDateString();

            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;
            BindInspection();
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                //chkOfficeAuditDeficiencies.Enabled = false;
            }
            //BindCategory();
            //BindSubcategory();
            //BindUser();
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
            //NameValueCollection criteria = new NameValueCollection();

            //criteria.Clear();

            //criteria.Add("txtFrom", txtFrom.Text);
            //criteria.Add("txtTo", txtTo.Text);
            //criteria.Add("ddlStatus", ddlAcceptance.SelectedValue);
            //criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            //criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            //criteria.Add("ucDepartment", ucDepartment.SelectedDepartment);
            //criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            //criteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
            //criteria.Add("ucVessel", ucVessel.SelectedVessel);
            //criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            //criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            //criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            //criteria.Add("ddlSourceType", ddlSourceType.SelectedValue == "0" ? null : ddlSourceType.SelectedValue);
            //criteria.Add("txtSourceRefNo", txtSourceRefNo.Text);
            //criteria.Add("ddlAcceptedBy", ddlAcceptedBy.SelectedValue);
            //criteria.Add("txtWONoFrom", txtWONoFrom.Text);
            //criteria.Add("txtWONoTo", txtWONoTo.Text);
            //criteria.Add("chkOfficeAuditDeficiencies", (chkOfficeAuditDeficiencies.Checked == true ? "1" : "0"));
            //criteria.Add("ucCompany", ucCompany.SelectedCompany);

            //Filter.CurrentOfficeTaskFilter = criteria;

            NameValueCollection carcriteria = new NameValueCollection();

            carcriteria.Clear();

            carcriteria.Add("txtFrom", uccartargetdatefrom.Text);
            carcriteria.Add("txtTo", uccartargetdateto.Text);
            carcriteria.Add("ucStatus", ucStatus.SelectedHard);
            carcriteria.Add("ucVessel", ucVessel.SelectedVessel);
            carcriteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            carcriteria.Add("txtDoneFrom", uccarcompletiondatefrom.Text);
            carcriteria.Add("txtDoneTo", uccarcompletiondateto.Text);
            carcriteria.Add("ucVesselType", ucVesselType.SelectedHard);
            carcriteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            carcriteria.Add("ucInspectionType", ucInspectionType.SelectedHard);
            carcriteria.Add("ucInspection", ddlInspection.SelectedValue);
            carcriteria.Add("chkExcludeVIR", (chkExcludeVIR.Checked == true ? "1" : "0"));
            carcriteria.Add("ddlSourceType", ddlcarsourcetype.SelectedValue == "0" ? null : ddlcarsourcetype.SelectedValue);
            carcriteria.Add("txtSourceRefNo", txtcarsourcerefno.Text);
            carcriteria.Add("chkShowRescheduledTasks", (chkShowRescheduledTasks.Checked == true ? "1" : "0"));
            carcriteria.Add("txtItem", txtItem.Text);
            carcriteria.Add("ucChapter", ucChapter.SelectedChapter);
            carcriteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            carcriteria.Add("ucNonConformanceCategory", ucNonConformanceCategory.SelectedQuick);
            carcriteria.Add("ucVerficationLevel", ucVerficationLevel.SelectedHard);
            carcriteria.Add("chkOfficeAuditDeficiencies", (chkOfficeAudit.Checked == true ? "1" : "0"));
            carcriteria.Add("ucCompany", ddlCompany.SelectedCompany);
            carcriteria.Add("chkPendingRescheduleTask", (chkPendingRescheduleTask.Checked == true ? "1" : "0"));

            Filter.CurrentOfficeCorrectiveTaskFilter = carcriteria;
            //Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    //protected void BindCategory()
    //{
    //    ddlCategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
    //    ddlCategory.DataTextField = "FLDCATEGORYNAME";
    //    ddlCategory.DataValueField = "FLDCATEGORYID";
    //    ddlCategory.DataBind();
    //    ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}

    //protected void BindSubcategory()
    //{
    //    ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
    //    ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
    //    ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
    //    ddlSubcategory.DataBind();
    //    ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}

    //protected void chkOfficeAuditDeficiencies_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkOfficeAuditDeficiencies.Checked == true)
    //        ucCompany.Enabled = true;
    //    else
    //        ucCompany.Enabled = false;
    //}
    protected void chkOfficeAudit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAudit.Checked == true)
            ddlCompany.Enabled = true;
        else
            ddlCompany.Enabled = false;
    }
    //protected void ddlCategory_Changed(object sender, EventArgs e)
    //{
    //    BindSubcategory();
    //}

    //protected void BindUser()
    //{
    //    DataSet ds = PhoenixInspection.UserList(null);
    //    ddlAcceptedBy.DataSource = ds.Tables[0];
    //    ddlAcceptedBy.DataTextField = "FLDDESIGNATIONNAME";
    //    ddlAcceptedBy.DataValueField = "FLDUSERCODE";
    //    ddlAcceptedBy.DataBind();
    //    ddlAcceptedBy.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}
    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null,
            null,
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }
    protected void ucInspectionType_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }
    protected void BindInspection()
    {
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , null
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
