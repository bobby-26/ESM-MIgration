using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionAdminRiskAssessment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Incident/NearMiss Deletion", "INCDELETION",ToolBarDirection.Right);
        toolbar.AddButton("Corrective Task Deletion", "CARDELETION", ToolBarDirection.Right);
        toolbar.AddButton("RA Date Change", "RADATECHANGE", ToolBarDirection.Right);
        toolbar.AddButton("RA Deletion", "RADELETION", ToolBarDirection.Right);

        MenuRiskAssessmentGeneral.AccessRights = this.ViewState;
        MenuRiskAssessmentGeneral.MenuList = toolbar.Show();
        MenuRiskAssessmentGeneral.SelectedMenuIndex = 2;

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuRiskAssessment.AccessRights = this.ViewState;
        MenuRiskAssessment.MenuList = toolbar.Show();
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        if (!IsPostBack)
        {
            txtRAId.Attributes.Add("style", "display:none");
            txtRaType.Attributes.Add("style", "display:none");
            ucVessel.Enabled = true;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucVessel.bind();
            }

            imgShowRA.Attributes.Add("onclick", "return showPickList('spnRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?vesselid=" + (General.GetNullableInteger(ucVessel.SelectedVessel) == null ? "0" : ucVessel.SelectedVessel) + "', true); ");
        }
    }

    protected void RiskAssessmentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RADELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminRAList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("CARDELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminCARList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("INCDELETION"))
        {
            Response.Redirect("../Inspection/InspectionAdminIncidentList.aspx", true);
        }

    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        imgShowRA.Attributes.Add("onclick", "return showPickList('spnRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?vesselid=" + (General.GetNullableInteger(ucVessel.SelectedVessel) == null ? "0" : ucVessel.SelectedVessel) + "', true); ");
    }

    protected void MenuRiskAssessment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidInput())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionRiskAssessment.UpdateRADate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(txtRAId.Text), int.Parse(txtRaType.Text), General.GetNullableInteger(ucVessel.SelectedVessel),
                DateTime.Parse(ucDate.Text), DateTime.Parse(ucApprovedDate.Text), DateTime.Parse(ucIssuedDate.Text),General.GetNullableDateTime(ucIntendedWorkDate.Text));
            ucStatus.Text = "RA Date updated successfully.";
            BindDate();
        }
    }  

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(txtRAId.Text) == null)
            ucError.ErrorMessage = "Please select the RA.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Prepared Date is required.";

        if (General.GetNullableDateTime(ucDate.Text) != null)
        {
            if (General.GetNullableDateTime(ucDate.Text) > (General.GetNullableDateTime(ucIntendedWorkDate.Text)))
                ucError.ErrorMessage = "Prepared Date should not be greater than intended work date.";

            if (General.GetNullableDateTime(ucDate.Text) > System.DateTime.Now)
                ucError.ErrorMessage = "Prepared Date should not be future date.";
        }

        if (General.GetNullableDateTime(ucApprovedDate.Text) == null)
            ucError.ErrorMessage = "Approved Date is required.";

        if (General.GetNullableDateTime(ucApprovedDate.Text) != null)
        {
            if (General.GetNullableDateTime(ucApprovedDate.Text) > (General.GetNullableDateTime(ucIntendedWorkDate.Text)))
                ucError.ErrorMessage = "Approved Date should not be greater than intended work date.";

            if (General.GetNullableDateTime(ucApprovedDate.Text) > System.DateTime.Now)
                ucError.ErrorMessage = "Approved Date should not be future date.";
        }

        if (General.GetNullableDateTime(ucIssuedDate.Text) == null)
            ucError.ErrorMessage = "Issued Date is required.";

        if (General.GetNullableDateTime(ucIssuedDate.Text) != null)
        {
            if (General.GetNullableDateTime(ucIssuedDate.Text) > (General.GetNullableDateTime(ucIntendedWorkDate.Text)))
                ucError.ErrorMessage = "Issued Date should not be greater than intended work date.";

            if (General.GetNullableDateTime(ucIssuedDate.Text) > System.DateTime.Now)
                ucError.ErrorMessage = "Issued Date should not be future date.";
        }
        if (txtRaType.Text == "2" || txtRaType.Text == "3" || txtRaType.Text == "4")
        {
            if (General.GetNullableDateTime(ucIntendedWorkDate.Text) == null)
                ucError.ErrorMessage = "Intended Work Date required.";
        }

        return (!ucError.IsError);
    }

    protected void BindDate()
    {
        if (txtRAId.Text != "" && General.GetNullableGuid(txtRAId.Text) != null)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (txtRaType.Text == "1")
            {
                ds = PhoenixInspectionRiskAssessmentProcess.EditInspectionRiskAssessmentProcess(General.GetNullableGuid(txtRAId.Text));
                dt = ds.Tables[0];
                ucIntendedWorkDate.Text = "";
            }
            if (txtRaType.Text == "2")
            {
                dt = PhoenixInspectionRiskAssessmentGeneric.EditPhoenixInspectionRiskAssessmentGeneric(General.GetNullableGuid(txtRAId.Text));
                ucIntendedWorkDate.Text = dt.Rows[0]["FLDINTENDEDWORKDATE"].ToString();
            }
            if (txtRaType.Text == "3")
            {
                dt = PhoenixInspectionRiskAssessmentMachinery.EditRiskAssessmentMachinery(General.GetNullableGuid(txtRAId.Text));
                ucIntendedWorkDate.Text = dt.Rows[0]["FLDINTENDEDWORKDATE"].ToString();
            }
            if (txtRaType.Text == "4")
            {
                dt = PhoenixInspectionRiskAssessmentNavigation.EditRiskAssessmentNavigation(General.GetNullableGuid(txtRAId.Text));
                ucIntendedWorkDate.Text = dt.Rows[0]["FLDINTENDEDWORKDATE"].ToString();
            }
            ucDate.Text = dt.Rows[0]["FLDPREPAREDDATE"].ToString();
            ucApprovedDate.Text = dt.Rows[0]["FLDAPPROVEDDATE"].ToString();
            ucIssuedDate.Text = dt.Rows[0]["FLDISSUEDDATE"].ToString();
        }
        else
        {
            ucDate.Text = "";
            ucApprovedDate.Text = "";
            ucIssuedDate.Text = "";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindDate();
    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        BindDate();
    }
}
