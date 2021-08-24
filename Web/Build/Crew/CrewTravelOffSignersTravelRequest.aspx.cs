using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewTravelOffSignersTravelRequest : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("GENERATE TRAVEL REQ", "GENERATETRAVELREQUSET",ToolBarDirection.Right);
        MenuCrewTravel.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSEL"] = null;
            ViewState["VESSELID"] = null;          
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["EMPLOYEEID"] = Request.QueryString["empid"].ToString();
            ViewState["VESSEL"] = Request.QueryString["vessel"].ToString();
            ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();
            txtVessel.Text = ViewState["VESSEL"].ToString();
            SetEmployeePrimaryDetails();
            BindVesselAccount();
        }
    }
    protected void MenuCrewTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERATETRAVELREQUSET"))
            {
                if (!IsValidCrewChangeRequest(txtDateOfCrewChange.Text, ucport.SelectedSeaport, ucCrewChangeReason.SelectedReason, 
                    txtOrigin.SelectedValue, txtDestination.SelectedValue, txtDepatureDate.Text, txtArrivalDate.Text, ucPaymentmode.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewTravelRequest.InsertTravelRequestOffsigners
                        (int.Parse(ViewState["EMPLOYEEID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()),DateTime.Parse(txtDateOfCrewChange.Text)
                        ,int.Parse(ucport.SelectedSeaport),int.Parse(ucCrewChangeReason.SelectedReason)
                         ,int.Parse(txtOrigin.SelectedValue),int.Parse(txtDestination.SelectedValue),DateTime.Parse(txtDepatureDate.Text)
                         ,DateTime.Parse(txtArrivalDate.Text),int.Parse(ucPaymentmode.SelectedHard)
                         ,General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                    ucStatus.Text = "Travel Request Generated";
                    Clear();
                    //ApproveCrewTravelReq(null, null);
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Clear()
    {
        txtDateOfCrewChange.Text ="";
        txtOrigin.Text="";
        txtDestination.Text="";
        txtDepatureDate.Text="";
        txtArrivalDate.Text="";      
    }
    protected void ApproveCrewTravelReq(object sender, EventArgs e)
    {
        try
        {
            if ((sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1) || sender == null)
            {
                ucStatus.Text = "Travel Request Generated.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidCrewChangeRequest(string traveldate, string strPortId, string crewchangereason, string origin,
        string destination, string depaturedate, string arrivaldate, string paymentmode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Account is required.";

        if (General.GetNullableDateTime(traveldate) == null)
            ucError.ErrorMessage = "Crew Change is required.";

        if (General.GetNullableInteger(strPortId) == null)
            ucError.ErrorMessage = "Port is required";

        if (General.GetNullableInteger(crewchangereason) == null)
            ucError.ErrorMessage = "Reason is required";

        if (string.IsNullOrEmpty(origin.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(depaturedate) == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "Arrival date is required.";

        if (General.GetNullableInteger(paymentmode) == null)
            ucError.ErrorMessage = "Payment mode is required.";

        if (DateTime.TryParse(depaturedate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date";

        if (DateTime.TryParse(depaturedate, out resultdate) && resultdate < DateTime.Today)
        {
            ucError.ErrorMessage = "Departure date should be later than or equal to current date";
        }
        if (DateTime.TryParse(arrivaldate, out resultdate) && resultdate < DateTime.Today)
        {
            ucError.ErrorMessage = "Arrival date should be later than or equal to current date";
        }
        
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindVesselAccount()
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
               General.GetNullableInteger(ViewState["VESSELID"].ToString()), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

}
