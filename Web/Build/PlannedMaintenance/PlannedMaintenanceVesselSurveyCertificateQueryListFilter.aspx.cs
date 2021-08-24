using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateQueryListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Go", "SEARCH");
        SurveyCertificateFilter.AccessRights = this.ViewState;
        SurveyCertificateFilter.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            BindSurveyType();
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
    }
    private void BindSurveyType()
    {
        ddlSurveyType.DataSource = PhoenixRegistersVesselSurvey.SurveyTypeList();
        ddlSurveyType.DataValueField = "FLDSURVEYTYPEID";
        ddlSurveyType.DataTextField = "FLDSURVEYTYPENAME";
        ddlSurveyType.DataBind();
        ddlSurveyType.Items.Insert(0, new ListItem("--Select--", string.Empty));
    }
    protected void SurveyCertificateFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "SEARCH")
            {
                if (ucVessel.SelectedVessel.ToUpper().Equals("DUMMY"))
                {
                    ucError.ErrorMessage = "Please Select Vessel";
                    ucError.Visible = true;
                    return;
                }
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("VesselId", ucVessel.SelectedVessel);
                nvc.Add("IssuedFrom", ucFromDate.Text);
                nvc.Add("IssuedTo", ucToDate.Text);
                nvc.Add("SurveyType", ddlSurveyType.SelectedValue);
                nvc.Add("SurveyNumber", txtSurveyNumber.Text);
                nvc.Add("EndorsementsYN", chkEndorse.Checked ? "1" : "0");
                nvc.Add("ShowNotApplicable", chkShowNotApplicable.Checked ? "1" : "0");
                Filter.VesselSurveyCertificateFilter = nvc;

                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateQueryList.aspx",false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
