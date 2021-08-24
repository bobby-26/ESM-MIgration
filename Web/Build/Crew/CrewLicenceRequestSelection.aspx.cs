using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLicenceRequestSelection : PhoenixBasePage
{
    string strEmployeeId = string.Empty, strRankId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Next", "NEXT");
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
                SetEmployeePrimaryDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ucRank.SelectedRank = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("NEXT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Crew/CrewMissingLicence.aspx?empid=" + strEmployeeId + "&rnkid=" + ucRank.SelectedRank + "&vslid=" + ucVessel.SelectedVessel + "&jdate=" + ucDate.Text);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter()
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (ucVessel.SelectedVessel.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Vessel is required.";
        }

        if (string.IsNullOrEmpty(ucDate.Text))
            ucError.ErrorMessage = "Expected Joining Date is required.";
        else if (DateTime.TryParse(ucDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Expected Date of Joining should be later than current date";
        }
        return (!ucError.IsError);
    }

}
