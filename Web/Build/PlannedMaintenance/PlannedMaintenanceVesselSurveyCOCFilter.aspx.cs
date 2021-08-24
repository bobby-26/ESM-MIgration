using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCOCFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Go", "SEARCH",ToolBarDirection.Right);
        SurveyCOCFilter.AccessRights = this.ViewState;
        SurveyCOCFilter.MenuList = toolbarmain.Show();
        
        if(!IsPostBack)
        {
            ucCertificate.DataSource = PhoenixRegistersCertificates.ListCertificates(1, 1);
            ucCertificate.DataTextField = "FLDCERTIFICATENAME";
            ucCertificate.DataValueField = "FLDCERTIFICATEID";
            ucCertificate.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ucCertificate.DataBind();

            BindSurveyType();
        }
        
    }
    private void BindSurveyType()
    {
        ddlSurveyType.DataSource = PhoenixRegistersVesselSurvey.SurveyTypeList();
        ddlSurveyType.DataValueField = "FLDSURVEYTYPEID";
        ddlSurveyType.DataTextField = "FLDSURVEYTYPENAME";
        ddlSurveyType.DataBind();
        ddlSurveyType.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));
    }
    protected void SurveyCOCFilter_TabStripCommand(object sender,EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "SEARCH")
            {
                
                NameValueCollection nvc = Filter.VesselSurveyCOCFilter;
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    
                    nvc.Add("CertificateId", ucCertificate.SelectedValue );
                    nvc.Add("DueFrom", ucFromDate.Text);
                    nvc.Add("DueTo", ucToDate.Text);
                    nvc.Add("SurveyType", ddlSurveyType.SelectedValue);
                    nvc.Add("Status", ddlStatus.SelectedStatus);  
                    Filter.VesselSurveyCOCFilter = nvc;
                }
                else
                {                    
                    nvc["CertificateId"] = ucCertificate.SelectedValue;
                    nvc["DueFrom"] = ucFromDate.Text;
                    nvc["DueTo"] = ucToDate.Text;
                    nvc["SurveyType"] = ddlSurveyType.SelectedValue;
                    nvc["Status"] = ddlStatus.SelectedStatus;
                }
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCOCList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
 
    }
}
