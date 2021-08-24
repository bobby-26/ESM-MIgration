using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Inspection_InspectionShipBoardTaskFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", false);
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

            VesselConfiguration();
            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;
            BindInspection();

            DateTime now = DateTime.Now;
            txtFrom.Text = now.Date.AddMonths(-3).ToShortDateString();
            txtTo.Text = now.Date.AddMonths(+3).ToShortDateString();

            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

            //DateTime now1 = DateTime.Now;
            //txtPreFrom.Text = now.Date.AddMonths(-3).ToShortDateString();
            //txtPreTo.Text = now.Date.AddMonths(+3).ToShortDateString();
            
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                chkOfficeAuditDeficiencies.Enabled = false;
            }
            //BindCategory();
            //BindSubcategory();
            //BindUser();
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

            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucInspectionType", ucInspectionType.SelectedHard);
            criteria.Add("ucInspection", ddlInspection.SelectedValue);
            criteria.Add("chkExcludeVIR", (chkExcludeVIR.Checked == true ? "1" : "0"));
            criteria.Add("ddlSourceType", ddlSourceType.SelectedValue == "0" ? null : ddlSourceType.SelectedValue);
            criteria.Add("txtSourceRefNo", txtSourceRefNo.Text);
            criteria.Add("chkShowRescheduledTasks", (chkShowRescheduledTasks.Checked == true ? "1" : "0"));
            criteria.Add("txtItem", txtItem.Text);
            criteria.Add("ucChapter", ucChapter.SelectedChapter);
            criteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucNonConformanceCategory", ucNonConformanceCategory.SelectedQuick);
            criteria.Add("ucVerficationLevel", ucVerficationLevel.SelectedHard);
            criteria.Add("chkOfficeAuditDeficiencies", (chkOfficeAuditDeficiencies.Checked == true ? "1" : "0"));
            criteria.Add("ucCompany", ucCompany.SelectedCompany);                      
            criteria.Add("chkPendingRescheduleTask", (chkPendingRescheduleTask.Checked == true ? "1" : "0"));                      
            
            Filter.CurrentShipBoardTaskFilter = criteria;

            //NameValueCollection precriteria = new NameValueCollection();

            //precriteria.Clear();

            //precriteria.Add("txtFrom", txtPreFrom.Text);
            //precriteria.Add("txtTo", txtPreTo.Text);
            //precriteria.Add("ddlAcceptance", ddlAcceptance.SelectedValue);
            //precriteria.Add("txtDoneFrom", txtPreDoneDateFrom.Text);
            //precriteria.Add("txtDoneTo", txtPreDoneDateTo.Text);
            //precriteria.Add("ucDepartment", ucDepartment.SelectedDepartment);
            //precriteria.Add("ddlCategory", ddlCategory.SelectedValue);
            //precriteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
            //precriteria.Add("ucVessel", ucVessel.SelectedVessel);
            //precriteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            //precriteria.Add("ucVesselType", ucVesselType.SelectedHard);
            //precriteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            //precriteria.Add("ddlSourceType", ddlPreSourceType.SelectedValue == "0" ? null : ddlPreSourceType.SelectedValue);
            //precriteria.Add("txtSourceRefNo", txtPreSourceRefNo.Text);
            //precriteria.Add("ddlAcceptedBy", ddlAcceptedBy.SelectedValue);

            //Filter.CurrentPreventiveTaskFilter = precriteria;
            //Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
    protected void chkOfficeAuditDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAuditDeficiencies.Checked == true)
            ucCompany.Enabled = true;
        else
            ucCompany.Enabled = false;
    }

    protected void BindInspection()
    {
        //ucInspection.InspectionType = ucInspectionType.SelectedHard;
        //ucInspection.InspectionList = PhoenixInspection.ListInspection(General.GetNullableInteger(ucInspectionType.SelectedHard),
        //    null, null);

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

    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null,
            null,
            General.GetNullableGuid(ddlInspection.SelectedValue));
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

}
