using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PlannedMaintenanceVesselSurveyCertificateExtentionUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCertificatesRenewal.AccessRights = this.ViewState;
            MenuCertificatesRenewal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {                
                ViewState["ATTACHMENTTYPE"] = "EXTNCERTIFICATE";
                ViewState["DTKEY"] = string.Empty;
                ViewState["VSLID"] = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                if(!string.IsNullOrEmpty(Request.QueryString["dtkey"]))
                    ViewState["DTKEY"] = Request.QueryString["dtkey"];

                ViewState["VSLID"] = Request.QueryString["vslid"] != null ? Request.QueryString["vslid"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                BindDetails();

                txtExpirydate.Enabled = false;
                txtSurveyDue.Enabled = false;     
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDetails()
    {
        if (!string.IsNullOrEmpty(ViewState["DTKEY"].ToString()) && !string.IsNullOrEmpty(ViewState["VSLID"].ToString()))
        {
            DataTable dt;
            dt = PhoenixPlannedMaintenanceSurveySchedule.CertificateExtensionDetails(int.Parse(ViewState["VSLID"].ToString())
                                                                                    , new Guid(ViewState["DTKEY"].ToString()));
            if(dt.Rows.Count > 0)
            {
                txtExpirydate.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
                txtSurveyDue.Text = dt.Rows[0]["FLDDUEDATE"].ToString();
                txtExtnExpiryDate.Text = dt.Rows[0]["FLDEXTNEXPIRYDATE"].ToString();
                txtExtnSurveyDue.Text = dt.Rows[0]["FLDEXTNNEXTSURVEYDATE"].ToString();
                txtRemarks.Text = dt.Rows[0]["FLDEXTNREMARKS"].ToString();
                if(dt.Rows[0]["FLDATTACHMENTYN"].ToString() == "1")
                {
                    cmdExtnAtt.Visible = true;
                    cmdExtnAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + new Guid(ViewState["DTKEY"].ToString()) + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=EXTNCERTIFICATE&VESSELID=" + ViewState["VSLID"].ToString() + "'); return false;");

                }

            }
        }

    }    


    protected void MenuCertificatesRenewal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(!IsValidDetails(txtExtnExpiryDate.Text,txtExtnSurveyDue.Text,txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!string.IsNullOrEmpty(ViewState["DTKEY"].ToString()) && !string.IsNullOrEmpty(ViewState["VSLID"].ToString()))
                {
                    PhoenixPlannedMaintenanceSurveySchedule.CertificateExtensionUpdate(General.GetNullableDateTime(txtExtnExpiryDate.Text)
                                                                                        , General.GetNullableDateTime(txtExtnSurveyDue.Text)
                                                                                        , txtRemarks.Text
                                                                                        , int.Parse(ViewState["VSLID"].ToString())
                                                                                        , new Guid(ViewState["DTKEY"].ToString()));

                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["DTKEY"].ToString()), PhoenixModule.PLANNEDMAINTENANCE, null, null, string.Empty, ViewState["ATTACHMENTTYPE"].ToString(), int.Parse(ViewState["VSLID"].ToString()));
                }
            }
               
                
                //EditVesselCertificate();
                ucStatus.Text = "Certificate Details Updated Sucessfully";
                ucStatus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDetails(
        string extDue
        , string extSurveyDue
        , string Reason
        )
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (Reason.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";

        if (!DateTime.TryParse(extDue, out resultDate) && !DateTime.TryParse(extSurveyDue, out resultDate))
            ucError.ErrorMessage = "Extn. Expiry Date or Extn. Survey Due is required";

        return (!ucError.IsError);
    }
}
