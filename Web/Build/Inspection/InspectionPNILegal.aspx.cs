using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Registers;

public partial class InspectionPNILegal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Submit to A/C", "SUBMIT");
        toolbarmain.AddButton("Save", "SAVE");
        LegalTabStrip.AccessRights = this.ViewState;
        LegalTabStrip.MenuList = toolbarmain.Show();
       
        if (!IsPostBack)
        {
            ViewState["PNI"] = null;
            ViewState["PNICASEID"] = null;
            ViewState["SUBMIT"] = null;
            if (Request.QueryString["PNIId"] != null)
            {
                ViewState["PNI"] = Request.QueryString["PNIId"].ToString();
                EditLegal();
                EditQuality();
            }
            ucConfirm.Visible = false;
            if (Request.QueryString["PNICASEID"] != null && !string.IsNullOrEmpty(Request.QueryString["PNICASEID"].ToString()))
                ViewState["PNICASEID"] = Request.QueryString["PNICASEID"].ToString();

            lnkPNIChecklist.Visible = false;
            if (ViewState["PNICASEID"] != null && !string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
            {
                lnkPNIChecklist.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionPNIChecklist.aspx?&PNICASEID=" + ViewState["PNICASEID"].ToString() + "&DEPARTMENTID=3');return true;");
                lnkPNIChecklist.Visible = true;
            }
        }      
       
    }
    public void EditLegal()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["PNI"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDLEGALREMARKS"].ToString();
            ucDate.Text = ds.Tables[0].Rows[0]["FLDDEADLINEDATE"].ToString();
            ViewState["SUBMIT"] = ds.Tables[0].Rows[0]["FLDDATESUBMITTEDTOACCT"].ToString();
            if (General.GetNullableDateTime(ViewState["SUBMIT"].ToString()) != null)
            {
                txtRemarks.Enabled = false;
                ucDate.Enabled = false;
            }
        }        
    }
    private void EditQuality()
    {
        if (ViewState["PNI"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["PNI"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtPNIClub.Text = dr["FLDPNICLUBNAME"].ToString();
                txtDeductible.Text = dr["FLDCREWDEDUCTIBLE"].ToString();
                txtQualityincharge.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
                chkPNIReportyn.Checked = dr["FLDREPORTEDTOPAIYN"].ToString() == "1" ? true : false;
                txtReportedDate.Text = dr["FLDREPORTEDDATE"].ToString();
                ucESMOwner.SelectedHard = dr["FLDESMOROWNER"].ToString();
                txtRefNo.Text = dr["FLDPNIREFNO"].ToString();
            }
        }
    }
    public void LegalTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            
            if (dce.CommandName.ToUpper().Equals("SUBMIT"))
            {
                if (!IsValidRemarks(txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableDateTime(ViewState["SUBMIT"].ToString()) != null)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = "It's Already submitted to A/C.";
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.Visible = true;
                ucConfirm.Text = "Are you sure want to submit to A/C";
                return;
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidInspectionPNILegalQuality())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["PNI"] != null)
                {

                    PhoenixInspectionPNI.UpdateLegalDetails(General.GetNullableGuid(ViewState["PNI"].ToString()), txtRemarks.Text, General.GetNullableDateTime(ucDate.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);                               
                
                    //PhoenixInspectionPNI.PNIQualityUpdate(new Guid(ViewState["PNI"].ToString()), chkPNIReportyn.Checked ? 1 : 0,
                    //    General.GetNullableDateTime(txtReportedDate.Text), int.Parse(ucESMOwner.SelectedHard), txtRefNo.Text.Trim());

                    ucStatus.Text = "Legal & Quality details are updated.";
                }

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                EditLegal();
                EditQuality();
           }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidInspectionPNILegalQuality()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtRemarks.Text))
            ucError.ErrorMessage = "Legal Department remarks is required.";

       /* if (General.GetNullableDateTime(txtReportedDate.Text) == null)
            ucError.ErrorMessage = "Reported Date is required.";

        if (string.IsNullOrEmpty(ucESMOwner.SelectedHard) || ucESMOwner.SelectedHard.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Company/Owner is required.";

        if (string.IsNullOrEmpty(txtRefNo.Text.Trim()))
            ucError.ErrorMessage = "P & I Ref.No is Required.";*/

        return (!ucError.IsError);
    }

    public bool IsValidRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please fill the following details";

        if (remarks.Equals("") || string.IsNullOrEmpty(remarks))
        {
            ucError.ErrorMessage = "Remarks is required.";
        }
        return (!ucError.IsError);
    }
    public void SubmitToAcc_Click(object sender,EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixInspectionPNI.UpdateLegalDetails(General.GetNullableGuid(ViewState["PNI"].ToString()), txtRemarks.Text, General.GetNullableDateTime(ucDate.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode, 2);
            }
            EditLegal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
