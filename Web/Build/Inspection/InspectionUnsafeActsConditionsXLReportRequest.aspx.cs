using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class InspectionUnsafeActsConditionsXLReportRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Request XL Report", "GO");
                MenuOpenReportsFilter.AccessRights = this.ViewState;
                MenuOpenReportsFilter.MenuList = toolbar.Show();

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
                BindSubCategory();
                ucIncidentNearMissFromDate.Text = "01/01/2011";
                ucIncidentNearMissToDate.Text = DateTime.Now.ToShortDateString();
                ucStatusofUA.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void MenuOpenReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            NameValueCollection criteria = new NameValueCollection();

            if (dce.CommandName.ToUpper().Equals("GO"))
            { 
                string filtercondition = ("null"+ ","
                                                + "null"+ ","
                                                + General.GetNullableInteger(ucVessel.SelectedVessel.Equals("Dummy")?"null" : ucVessel.SelectedVessel)+","
                                                + General.GetNullableInteger(ucCategory.SelectedHard.Equals("Dummy")?"0" : ucCategory.SelectedHard)+ ",'"
                                                + (ddlSubcategory.SelectedValue) + "',"
                                                + General.GetNullableInteger("0")+",'"
                                                + ucIncidentNearMissFromDate.Text+"','"
                                                + ucIncidentNearMissToDate.Text+"','"
                                                + txtReferenceNumber.Text+"',"
                                                + General.GetNullableInteger(ucStatusofUA.SelectedHard.Equals("Dummy") ? "null" : ucStatusofUA.SelectedHard) + ","
                                                + General.GetNullableInteger(ucTechFleet.SelectedFleet.Equals("Dummy") ? "0" : ucTechFleet.SelectedFleet) + ","
                                                + (ucAddrOwner.SelectedAddress == "Dummy" ? "null" : ucAddrOwner.SelectedAddress) + ","
                                                + (ucVesselType.SelectedHard == "Dummy" ? "null" : ucVesselType.SelectedHard) + ","
                                                + PhoenixSecurityContext.CurrentSecurityContext.CompanyID+","
                                                + General.GetNullableInteger(chkIncidentNearMissRaisedYN.Checked ? "1" : "0"));

                string procedure = "PRINSPECTIONXLUNSAFEACTSCONDITIONSEARCH";
                string reportname = "Quality-Unsafe Acts Conditions";

                PhoenixXLReportRequest.InsertReportRequest(General.GetNullableString(filtercondition)
                                                            , General.GetNullableString(procedure)
                                                            , General.GetNullableString(reportname));

                ucStatus.Text = "XL Report Request initiated successfully.You will receive a mail in 1hour.";
            }

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    private void BindSubCategory()
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ucCategory.SelectedHard));
        ddlSubcategory.DataSource = dt;
        ddlSubcategory.DataTextField = "FLDIMMEDIATECAUSE";
        ddlSubcategory.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
}
