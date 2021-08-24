using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Reports;
public partial class CrewContractExtention : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    string rankid = string.Empty;
    string vesselid = string.Empty;
    string planid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
                ViewState["CONTRACTID"] = Request.QueryString["cid"];
            rankid = Request.QueryString["rnkid"];
            vesselid = Request.QueryString["vslid"];

            if (!IsPostBack)
            {
                btnconfirm.Attributes.Add("style", "display:none");
                EditContractDetails();
            }
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidContract())
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to save the record?", "confirm", 320, 150, null, "Confirm");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(strEmployeeId), General.GetNullableGuid(ViewState["CONTRACTID"] == null ? string.Empty : ViewState["CONTRACTID"].ToString()), int.Parse(vesselid));
            if (dt.Rows.Count > 0)
            {
                ViewState["CONTRACTID"] = dt.Rows[0]["FLDCONTRACTID"].ToString();
                ViewState["PAYDATE"] = dt.Rows[0]["FLDPAYDATE"].ToString();
                ViewState["PRANK"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                ViewState["CONTRANK"] = dt.Rows[0]["FLDRANKID"].ToString();
                ViewState["BPPOOL"] = dt.Rows[0]["FLDBPPOOL"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtAddress.Text = dt.Rows[0]["FLDADDRESS"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["SSCALE"] = dt.Rows[0]["FLDSENIORITYSCALE"].ToString();
                string rm = dt.Rows[0]["FLDRANKEXPERIENCE"].ToString();
                rankid = dt.Rows[0]["FLDRANKID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCrewContractSub.AccessRights = this.ViewState;
            MenuCrewContractSub.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            Guid newcontractid = new Guid();
        

            PhoenixCrewContract.InsertCrewContractExtension(new Guid(ViewState["CONTRACTID"].ToString()), DateTime.Parse(txtDate.Text), byte.Parse(txtContractPeriod.Text)
                , decimal.Parse(txtPlusMinusPeriod.Text), ref newcontractid);
            PhoenixCrewContract.InsertCrewContractLock(newcontractid);

            String script = String.Format("javascript:closeTelerikWindow('ext','chml');");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidContract()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        DateTime resultDate;
        decimal decimalresult;

        if (!DateTime.TryParse(txtDate.Text, out resultDate))
            ucError.ErrorMessage = "Pay Commences on is required.";

        if (!int.TryParse(txtContractPeriod.Text, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        if (!decimal.TryParse(txtPlusMinusPeriod.Text, out decimalresult))
            ucError.ErrorMessage = "Contract +/- Period is required.";

        if (General.GetNullableInteger(txtContractPeriod.Text).HasValue
            && General.GetNullableInteger(txtPlusMinusPeriod.Text).HasValue
             && int.Parse(txtPlusMinusPeriod.Text) >= int.Parse(txtContractPeriod.Text))
            ucError.ErrorMessage = "Contract +/- Period cannot exceed the Contract Period.";

        return (!ucError.IsError);
    }

}
