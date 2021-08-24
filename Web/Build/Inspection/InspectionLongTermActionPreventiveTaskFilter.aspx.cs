﻿using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;

public partial class Inspection_InspectionLongTermActionPreventiveTaskFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuScheduleFilter.AccessRights = this.ViewState;
            MenuScheduleFilter.MenuList = toolbar.Show();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            DateTime now = DateTime.Now;
            txtFrom.Text = now.Date.AddMonths(-6).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();

            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;
            //ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                chkOfficeAuditDeficiencies.Enabled = false;
            }

            BindCategory();
            BindSubcategory();
            BindUser();
        }
    }
    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ddlStatus", ddlAcceptance.SelectedValue);
            criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            criteria.Add("ucDepartment", ucDepartment.SelectedDepartment);
            criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            criteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ddlSourceType", ddlSourceType.SelectedValue == "0" ? null : ddlSourceType.SelectedValue);
            criteria.Add("txtSourceRefNo", txtSourceRefNo.Text);
            criteria.Add("ddlAcceptedBy", ddlAcceptedBy.SelectedValue);
            criteria.Add("txtWONoFrom", txtWONoFrom.Text);
            criteria.Add("txtWONoTo", txtWONoTo.Text);
            criteria.Add("chkOfficeAuditDeficiencies", (chkOfficeAuditDeficiencies.Checked ? "1" : "0"));
            criteria.Add("ucCompany", ucCompany.SelectedCompany);

            Filter.CurrentOfficeTaskFilter = criteria;
            //Response.Redirect("../Inspection/InspectionOfficeCorrectiveTasks.aspx", false);

        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindSubcategory()
    {
        ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkOfficeAuditDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAuditDeficiencies.Checked)
            ucCompany.Enabled = true;
        else
            ucCompany.Enabled = false;
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        BindSubcategory();
    }

    protected void BindUser()
    {
        DataSet ds = PhoenixInspection.UserList(null);
        ddlAcceptedBy.DataSource = ds.Tables[0];
        ddlAcceptedBy.DataTextField = "FLDDESIGNATIONNAME";
        ddlAcceptedBy.DataValueField = "FLDUSERCODE";
        ddlAcceptedBy.DataBind();
        ddlAcceptedBy.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    } 

}
