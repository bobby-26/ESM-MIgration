using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class VesselAccountPortageBillComponentTrackInsert : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbarmain.Show();
            toolbarmain = new PhoenixToolbar();
            if (!IsPostBack)
            {

                ViewState["EMPID"] = "";
                DataSet ds = PhoenixRegistersContractComponentTracking.CrewComponentTrackList(null);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcomponentgroup.DataSource = ds;
                    ddlcomponentgroup.DataBind();
                }
                ddlcomponentgroup.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlcomponentgroup.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidInsert())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPortageBill.InsertTrackComponentManual(int.Parse(ddlVesselChargeable.SelectedVessel)
                    , int.Parse(ViewState["EMPID"].ToString()), int.Parse(ViewState["RANKID"].ToString())
                    , new Guid(ddlcomponent.SelectedValue), int.Parse(ddlcomponentgroup.SelectedValue)
                    , DateTime.Parse(ucFromDate.Text), DateTime.Parse(ucToDate.Text)
                    , decimal.Parse(txtAmount.Text), txtRemarksEdit.Text.Trim()
                    , txtVoucherNo.Text.Trim()
                    , int.Parse(ddlCurrency.SelectedCurrency)
                 );
                if (Request.QueryString["accessfrom"].ToString() == "1")
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?accessfrom=1");
                else
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?accessfrom=0");
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["accessfrom"].ToString() == "1")
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?accessfrom=1");
                else
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?accessfrom=0");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidInsert()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableInteger(ViewState["EMPID"].ToString()).HasValue)
        {
            ucError.ErrorMessage = "Employee File No. is required.";
        }
        if (!General.GetNullableInteger(ddlVesselChargeable.SelectedVessel).HasValue)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (!General.GetNullableInteger(ddlCurrency.SelectedCurrency).HasValue)
        {
            ucError.ErrorMessage = "Currency is required.";
        }
        if (!General.GetNullableInteger(ddlcomponentgroup.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Component Group is required.";
        }
        if (!General.GetNullableGuid(ddlcomponent.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Component is required.";
        }
        if (!General.GetNullableDecimal(txtAmount.Text).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        if (General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From date is required";
        if (General.GetNullableDateTime(ucToDate.Text) == null)
            ucError.ErrorMessage = "To date is required";
        else if (DateTime.TryParse(ucFromDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ucToDate.Text)) > 0)
            ucError.ErrorMessage = "To date should be later than or equal to From date";
        return (!ucError.IsError);
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeListByFileNo(txtFileNo.Text.Trim());
            if (dt.Rows.Count > 0)
            {

                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                //txtVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                ViewState["EMPID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                lblLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                lblPresentVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                lblNextVessel.Text = dt.Rows[0]["FLDNEXTVESSEL"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";

        return (!ucError.IsError);
    }
    protected void ImgBtnValidFileno_Click(object sender, EventArgs e)
    {
        if (IsValidFileNoCheck())
        {
            SetEmployeePrimaryDetails();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }
    protected void ddlComponentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = PhoenixRegistersContractComponentTracking.ListCrewComponentddlList(General.GetNullableInteger(ddlcomponentgroup.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcomponent.DataSource = ds;
                ddlcomponent.DataBind();
            }
            ddlcomponent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlcomponent.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}