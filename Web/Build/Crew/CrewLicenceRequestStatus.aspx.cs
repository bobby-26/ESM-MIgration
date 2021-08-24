using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLicenceRequestStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			SessionUtil.PageAccessRights(this.ViewState);
            txtCRAPOId.Attributes.Add("style", "visibility:hidden");
            txtCRAPOEmailHidden.Attributes.Add("style", "visibility:hidden");
            txtFullTermPOEmailHidden.Attributes.Add("style", "visibility:hidden");
            txtFullTermPOId.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            
            if (Request.QueryString["licreqstatusId"] != null)
            {
                ViewState["licreqstatusId"] = Request.QueryString["licreqstatusId"].ToString();

                LicenceRequestStatusEdit(new Guid(Request.QueryString["licreqstatusId"].ToString()));
            }

            LicenceRequestStatus.MenuList = toolbar.Show(); toolbar = new PhoenixToolbar();

            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Status", "STATUS");
			MenuStatus.AccessRights = this.ViewState;
            MenuStatus.MenuList = toolbar.Show();
            MenuStatus.SelectedMenuIndex = 1;

            if (Request.QueryString["pid"] != null)
            {
                DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceProcess(
                    new Guid(Request.QueryString["pid"].ToString()));

                txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
            }
        }
    }

    protected void Status_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("CrewLicenceRequestStatusList.aspx?pid=" + Request.QueryString["pid"].ToString(), true);
        }
    }

    protected void LicenceRequestStatus_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidLicenceRequestStatus(txtDateofFlagStateProcess.Text))
                {
                    if (ViewState["licreqstatusId"] != null)
                    {
                        PhoenixCrewLicenceRequest.UpdateCrewLicenceStatus(
                            new Guid(ViewState["licreqstatusId"].ToString())
                            , General.GetNullableDateTime(txtDateofFlagStateProcess.Text)
                            , General.GetNullableInteger(txtCRAPOId.Text)
                            , General.GetNullableDateTime(txtDateOfHandover.Text)
                            , txtCRANumber.Text
                            , General.GetNullableDateTime(txtExpiryDateofCRA.Text)
                            , txtPaymentDetails.Text 
                            , General.GetNullableDateTime(txtFullTermDocsReceived.Text)
                            , General.GetNullableInteger(txtFullTermPOId.Text)
                            , General.GetNullableDateTime(txtHandoverofFullTerm.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            );
                        
                        ucStatus.Text = "CRA information updated";

                        LicenceRequestStatusEdit(new Guid(ViewState["licreqstatusId"].ToString()));
                    }
                    else
                    {
                        PhoenixCrewLicenceRequest.InsertCrewLicenceStatus(
                            new Guid(Request.QueryString["pid"].ToString())
                            , General.GetNullableDateTime(txtDateofFlagStateProcess.Text)
                            , General.GetNullableInteger(txtCRAPOId.Text)
                            , General.GetNullableDateTime(txtDateOfHandover.Text)
                            , txtCRANumber.Text
                            , General.GetNullableDateTime(txtExpiryDateofCRA.Text)
                            , txtPaymentDetails.Text
                            , General.GetNullableDateTime(txtFullTermDocsReceived.Text)
                            , General.GetNullableInteger(txtFullTermPOId.Text)
                            , General.GetNullableDateTime(txtHandoverofFullTerm.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            );

                        ucStatus.Text = "CRA information Added";
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLicenceRequestStatus(string flagstateprocessdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(flagstateprocessdate) == null)
            ucError.ErrorMessage = "'Date of Flag state process' is required.";

        return (!ucError.IsError);
    }

    private void LicenceRequestStatusEdit(Guid requeststatusid)
    {
        DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequestStatus(requeststatusid);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtDateofFlagStateProcess.Text = dr["FLDFLAGSTATEPROCESSDATE"].ToString();
            txtCRAPOId.Text = dr["FLDCRAHANDEDOVERTO"].ToString();
            txtCRAPOName.Text = dr["FLDCRAHANDEDOVERTONAME"].ToString();
            txtCRAPODesignation.Text = dr["FLDCRAHANDEDOVERTODESIGNATION"].ToString();
            txtDateOfHandover.Text = dr["FLDDATEOFHANDOVER"].ToString();
            txtCRANumber.Text = dr["FLDCRANUMBER"].ToString();
            txtExpiryDateofCRA.Text = dr["FLDDATEOFEXPIRY"].ToString();
            txtPaymentDetails.Text = dr["FLDPAYMENTDETAIL"].ToString();
            txtFullTermDocsReceived.Text = dr["FLDRECDFULLTERMDATE"].ToString();
            txtFullTermPOId.Text = dr["FLDFULLTERMHANDEDOVERTO"].ToString();
            txtFullTermPOName.Text = dr["FLDFULLTERMHANDEDOVERTONAME"].ToString();
            txtFullTermPODesignation.Text = dr["FLDFULLTERMHANDEDOVERTODESIGNATION"].ToString();
            txtHandoverofFullTerm.Text = dr["FLDFULLTERMHANDOVERDATE"].ToString();
            txtRemarks.Text = dr["FLDHANDOVERREMARKS"].ToString();
        }
    }
}
