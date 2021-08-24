using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Export2XL;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.IO;

public partial class InspectionDeficiencyXLReportRequest : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request XL Report", "GO");
            MenuDefeciencyFilter.AccessRights = this.ViewState;
            MenuDefeciencyFilter.MenuList = toolbar.Show();
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
            ucFromDate.Text = "01/01/2011";
            ucToDate.Text = DateTime.Now.ToShortDateString();
            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucFleet.Enabled = false;
                chkOfficeAuditDeficiencies.Enabled = false;
            }

        }
    }

    protected void MenuDefeciencyFilter_TabStripCommand(object sender, EventArgs e)
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
            string filtercondition = (General.GetNullableInteger(ddlNCType.SelectedValue) + ","
                                                + General.GetNullableInteger(ucVessel.SelectedVessel.Equals("Dummy") ? "null" : ucVessel.SelectedVessel) + ",'"
                                                + General.GetNullableString(txtRefNo.Text) + "','" 
                                                + ucFromDate.Text+ "','" 
                                                + ucToDate.Text + "'," 
                                                + General.GetNullableInteger(ucStatus.SelectedHard.Equals("Dummy") ? "null" : ucStatus.SelectedHard) + ",'" 
                                                + General.GetNullableString(txtSourceRefNo.Text) + "'," 
                                                + General.GetNullableInteger(cblRCA.Items[0].Selected ? "1" : "0") + "," 
                                                + General.GetNullableInteger((cblRCA.Items[1].Selected ? "1" : "0")) + "," 
                                                + (cblRCA.Items[2].Selected ? "1" : "0") + "," 
                                                + (ucAddrOwner.SelectedAddress == "Dummy" ? "null" : ucAddrOwner.SelectedAddress)+ "," 
                                                + (ucVesselType.SelectedHard == "Dummy"?"null":ucVesselType.SelectedHard) + "," 
                                                + (ucNonConformanceCategory.SelectedQuick == "Dummy"?"null":ucNonConformanceCategory.SelectedQuick) + "," 
                                                + (ucFleet.SelectedFleet == "Dummy"?"null":ucFleet.SelectedFleet) + ","
                                                + (ucInspectionType.SelectedHard == "Dummy" ? "null" : ucInspectionType.SelectedHard) + ","
                                                + (ucInspectionCategory.SelectedHard == "Dummy" ? "null" : ucInspectionCategory.SelectedHard) + ",'"
                                                + (ddlInspection.SelectedValue) + "','"
                                                + (ucChapter.SelectedChapter) + "',"
                                                + General.GetNullableInteger(ddlSource.SelectedValue == "Dummy" ? "null" : ddlSource.SelectedValue) + "," 
                                                + General.GetNullableString(txtKey.Text == ""?"null":txtKey.Text) + "," 
                                                + PhoenixSecurityContext.CurrentSecurityContext.CompanyID + "," 
                                                + General.GetNullableInteger(chkOfficeAuditDeficiencies.Checked ? "1" : "0") + "," 
                                                + "null");

            string procedure = "PRINSPECTIONXLDEFICIENCYSEARCH";
            string reportname = "Quality-Deficiency";

            PhoenixXLReportRequest.InsertReportRequest(General.GetNullableString(filtercondition)
                                                        , General.GetNullableString(procedure)
                                                        , General.GetNullableString(reportname));
            Status.Text = "XL Report Request initiated successfully.You will receive a mail in 1hour.";
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
        if (chkOfficeAuditDeficiencies.Checked)
            ucCompany.Enabled = true;
        else
            ucCompany.Enabled = false;
    }

    protected void BindInspection()
    {
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new ListItem("--Select--","Dummy"));
    }

}
