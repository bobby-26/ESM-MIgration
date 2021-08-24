using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using SouthNests.Phoenix.Inspection;
using System.Web;
public partial class OptionsDirectIncidentReporting : PhoenixBasePage
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
            if (Request.QueryString["category"].ToString() == "1")
            {
                MenuSecurityUser.Title = "Details of Incident/Near Miss";
                //lblHeader.Text = "Comprehensive Description of Near Miss";
            }
            else if (Request.QueryString["category"].ToString() == "2")
            {
                MenuSecurityUser.Title = "Details of Report";
                //lblHeader.Text = "Comprehensive Description of Report";
            }
            else if (Request.QueryString["category"].ToString() == "3")
            {
                MenuSecurityUser.Title = "Crew Complaints";
                lblRank.Visible = true;
                lblName.Visible = true;
                txtRank.Visible = true;
                txtName.Visible = true;
                //lblNote.Visible = true;
            }
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
            if (Request.QueryString["category"].ToString() == "3")
            {
                PhoenixInspectionIncident.CrewComplaintsInsert(null, txtInvestigationAndEvidence.Text.Trim(), ddlVessel.SelectedValue,
                    General.GetNullableString(txtRank.Text), General.GetNullableString(txtName.Text));
            }
            else
            {
                PhoenixInspectionIncident.DirectIncidentInsert(null, txtInvestigationAndEvidence.Text.Trim(), int.Parse(Request.QueryString["category"].ToString()), ddlVessel.SelectedValue);
            }
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

        if (txtInvestigationAndEvidence.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Description of Incident is required.";

        return (!ucError.IsError);
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
