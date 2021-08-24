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
public partial class OptionsDirectIncidentNearmissReporting : PhoenixBasePage
{
    protected string Code = "";
    protected string databasecode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            lblHeader.Visible = false;
            if (Request.QueryString["category"].ToString() != null)
            {
                ViewState["CATEGORY"] = Request.QueryString["category"].ToString();
            }
            
            int installcode = Getinstallcode();
            databasecode = GetDataBaseCode();

            lblManagement.Text = HttpContext.Current.Session["companyname"].ToString();

            if (databasecode.ToUpper().Equals("PHOENIX"))
            {
                txtName.CssClass = "input_mandatory";
                txtRank.CssClass = "input_mandatory";
                lblNote.Text = "Note: All crew members should participate in reporting of near-misses/ unsafe acts-conditions. This reporting should not only be limited to senior officers or only one department.";
            }

            if (installcode != 0)
            {
                ddlVessel.SelectedVessel = installcode.ToString();
                ddlVessel.Enabled = false;
            }
            BindSubCategory();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["CATEGORY"].ToString() == "1")
        {
            MenuInspectionUnSafe.Title = "Details of Unsafe Act / Unsafe Condition";
        }
        else if (ViewState["CATEGORY"].ToString() == "2")
        {
            MenuInspectionUnSafe.Title = "Details of Report";
        }
        MenuInspectionUnSafe.AccessRights = this.ViewState;
        MenuInspectionUnSafe.MenuList = toolbar.Show();
    }
    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    protected int Getinstallcode()
    {
        DataSet ds = PhoenixInspectionIncident.GetInstallCode();
        int installcode = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDINSTALLCODE"]);
        return installcode;
    }

    protected string GetDataBaseCode()
    {
        DataTable ds = PhoenixInspectionIncident.GetDataBaseCode();
        string databasecode = ds.Rows[0]["FLDDATABASECODE"].ToString();
        return databasecode;
    }
    private void BindSubCategory()
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ucCategory.SelectedHard));
        ddlSubcategory.DataSource = dt;
        ddlSubcategory.DataTextField = "FLDIMMEDIATECAUSE";
        ddlSubcategory.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
            
            PhoenixInspectionUnsafeActsConditions.DirectIncidentNearmissInsert(null, txtInvestigationAndEvidence.Text.Trim(), int.Parse(Request.QueryString["category"].ToString()), int.Parse(ddlVessel.SelectedVessel)
                , General.GetNullableString(txtLocation.Text), DateTime.Parse(ucDate.Text + " " + txtTimeOfIncident.SelectedTime), int.Parse(ucCategory.SelectedHard)
                , new Guid(ddlSubcategory.SelectedValue),General.GetNullableString(txtRank.Text), General.GetNullableString(txtName.Text));
            ucStatus.Text = "Information is recorded.";
            txtInvestigationAndEvidence.Text = "";

            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Report Incident/Near Miss";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
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

    private bool IsValidEntry()
    {
        databasecode = GetDataBaseCode();
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (txtLocation.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Location is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Date is required.";

        if (txtTimeOfIncident.SelectedTime == null)
            ucError.ErrorMessage = "Time is required.";
        else
        {
            if (General.GetNullableDateTime(ucDate.Text + " " + txtTimeOfIncident.SelectedTime) == null)
                ucError.ErrorMessage = "Time is not a valid time.";
        }

        if (General.GetNullableDateTime(ucDate.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "Date should not be the future date.";

        if (General.GetNullableInteger(ucCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Category is required.";

        if (ddlSubcategory.SelectedIndex == 0)
            ucError.ErrorMessage = "Sub-category is required.";

        if (txtInvestigationAndEvidence.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Description of Unsafe act/condition is required.";

        if (databasecode.ToUpper().Equals("PHOENIX"))
        {
            if (General.GetNullableString(txtRank.Text.Trim()) == null)
                ucError.ErrorMessage = "Rank is required.";

            if (General.GetNullableString(txtName.Text.Trim()) == null)
                ucError.ErrorMessage = "Name is required.";
        }

            return (!ucError.IsError);
    }    
}
