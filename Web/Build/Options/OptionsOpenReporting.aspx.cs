using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Web;
public partial class OptionsOpenReporting : PhoenixBasePage
{
    protected string Code = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuSecurityUser.AccessRights = this.ViewState;
        MenuSecurityUser.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            ucConfirm.Visible = false;
            lblHeader.Visible = false;

            int installcode = Getinstallcode();

            lblManagement.Text = HttpContext.Current.Session["companyname"].ToString();

            if (installcode != 0)
            {
                ddlVessel.SelectedVessel = installcode.ToString();
                ddlVessel.Enabled = false;
            }
        }
    }

    protected int Getinstallcode()
    {
        DataSet ds = PhoenixInspectionIncident.GetInstallCode();
        int installcode = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDINSTALLCODE"]);
        return installcode;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidEntry())
            {
                ucError.Visible = true;
                return;
            }
            
            PhoenixInspectionIncident.DirectIncidentInsert(null, txtInvestigationAndEvidence.Text.Trim(), 
                int.Parse(Request.QueryString["category"].ToString()), ddlVessel.SelectedValue, General.GetNullableInteger(ucCompany.SelectedCompany));
            
            ucStatus.Text = "Information is recorded.";
            txtInvestigationAndEvidence.Text = "";

            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Open Report";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    private bool IsValidEntry()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";
        else if (General.GetNullableInteger(ddlVessel.SelectedVessel) == 0)
        {
            if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
                ucError.ErrorMessage = "Office is required.";
        }

        if (txtInvestigationAndEvidence.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Description of Incident is required.";

        return (!ucError.IsError);
    }

    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        if (ddlVessel.SelectedVessel == "0")
        {
            ucCompany.Enabled = true;
            ucCompany.SelectedCompany = "";
            ucCompany.CssClass = "input_mandatory";
        }
        else
        {
            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = "";
            ucCompany.CssClass = "input";
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
