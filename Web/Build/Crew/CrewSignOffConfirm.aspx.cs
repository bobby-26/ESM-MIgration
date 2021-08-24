using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewSignOffConfirm : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string strEmployeeId = string.Empty;
            string strSignOnOffId = string.Empty;
            SessionUtil.PageAccessRights(this.ViewState);
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            if (!string.IsNullOrEmpty(Request.QueryString["signonoffid"]))
                strSignOnOffId = Request.QueryString["signonoffid"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuSignOff.AccessRights = this.ViewState;
            MenuSignOff.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                ViewState["empid"] = strEmployeeId;
                ViewState["signonoffid"] = strSignOnOffId;
                SetEmployeePrimaryDetails();

                if (!String.IsNullOrEmpty(strEmployeeId))
                {
                    CrewSignOffEdit(General.GetNullableInteger(ViewState["signonoffid"].ToString()));
                }

            }

            
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void CrewSignOffEdit(int? signonid)
    {
        DataTable dt = PhoenixCrewSignOnOff.CrewVeseelSignOffEdit(General.GetNullableInteger(ViewState["empid"].ToString()), signonid);
        if (dt.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dt.Rows[0]["FLDBTOD"].ToString()))
            {
                txtBtoD.Text = General.GetDateTimeToString(dt.Rows[0]["FLDBTOD"].ToString());
            }
            if (dt.Rows[0]["FLDDOA"].ToString() != "" || dt.Rows[0]["FLDDOA"].ToString() != null)
            {
                txtDOA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDOA"].ToString());
            }
            if (dt.Rows[0]["FLDDOAGIVENDATE"].ToString() != "" || dt.Rows[0]["FLDDOAGIVENDATE"].ToString() != null)
            {
                txtDOAGivenDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDOAGIVENDATE"].ToString());
            }
            if (dt.Rows[0]["FLDSTANDBYDATE"].ToString() != "" || dt.Rows[0]["FLDSTANDBYDATE"].ToString() != null)
            {
                txtStandByDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSTANDBYDATE"].ToString());
            }
            if (dt.Rows[0]["FLDETOD"].ToString() != "" || dt.Rows[0]["FLDETOD"].ToString() != null)
            {
                txtEndToD.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETOD"].ToString());
            }
            if (dt.Rows[0]["FLDLEAVESTARTDATE"].ToString() != "" || dt.Rows[0]["FLDLEAVESTARTDATE"].ToString() != null)
            {
                txtLeaveStartDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLEAVESTARTDATE"].ToString());
            }
            if (dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString() != "" || dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString() != null)
            {
                txtLeaveCompletionDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString());
            }
            
            txtSignOffDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNOFFDATE"].ToString());

            if (dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString() != null)
            {
                ddlPort.SelectedSeaport = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
            }
            if (dt.Rows[0]["FLDSIGNOFFREASONID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFREASONID"].ToString() != null)
            {
                ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();
            }
            if (dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString() != null)
            {
                ddlSignOn.SelectedSignOn = dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString();
            }
            txtSignOffRemarks.Text = dt.Rows[0]["FLDSIGNOFFREMARKS"].ToString();
            if (dt.Rows[0]["FLDISEDITABLE"].ToString() == "0")
            {
                MenuSignOff.Visible = false;
            }
            else
            {
                txtSignedOn.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNONDATE"].ToString());
                MenuSignOff.Visible = true;
            }
        }
    }

    protected void EnableControls()
    {
        txtSignOffDate.ReadOnly = false;
        txtSignOffDate.CssClass = "readonlytextbox";
        MenuSignOff.Visible = true;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.CssClass = "input";
        ddlPort.Enabled = true;
        txtStandByDate.ReadOnly = false;
        txtStandByDate.CssClass = "input";
        txtLeaveStartDate.ReadOnly = false;
        txtLeaveStartDate.CssClass = "readonlytextbox";
        txtLeaveCompletionDate.ReadOnly = false;
        txtLeaveCompletionDate.CssClass = "input";
        txtDOA.ReadOnly = false;
        txtDOA.CssClass = "input";
        txtDOAGivenDate.ReadOnly = false;
        txtDOAGivenDate.CssClass = "input";
        txtEndToD.ReadOnly = false;
        txtEndToD.CssClass = "input_mandatory";
        txtSignOffRemarks.ReadOnly = false;
        txtSignOffRemarks.CssClass = "input";
        chkEndTravel.Enabled = true;
        ddlSignOn.Enabled = true;
        ddlSignOffReason.Enabled = true;
        txtSignOffDate.Text = "";
        txtSignOffDate.CssClass = "input_mandatory";
    }

    protected void DisableControls()
    {
        txtSignOffDate.ReadOnly = true;
        txtSignOffDate.CssClass = "readonlytextbox";
        MenuSignOff.Visible = false;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.CssClass = "readonlytextbox";
        ddlPort.Enabled = false;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.CssClass = "readonlytextbox";
        txtLeaveStartDate.ReadOnly = true;
        txtLeaveStartDate.CssClass = "readonlytextbox";
        txtLeaveCompletionDate.ReadOnly = true;
        txtLeaveCompletionDate.CssClass = "readonlytextbox";
        txtDOA.ReadOnly = true;
        txtDOA.CssClass = "readonlytextbox";
        txtDOAGivenDate.ReadOnly = true;
        txtDOAGivenDate.CssClass = "readonlytextbox";
        txtEndToD.ReadOnly = true;
        txtEndToD.CssClass = "readonlytextbox";
        txtSignOffRemarks.ReadOnly = true;
        txtSignOffRemarks.CssClass = "readonlytextbox";
        chkEndTravel.Enabled = false;
        ddlSignOn.Enabled = false;
        ddlSignOffReason.Enabled = false;
    }

    protected void OnEndTravelClick(object sender, EventArgs e)
    {
        if (chkEndTravel.Checked == true)
        {
            if (txtSignOffDate.Text != null)
            {
                txtEndToD.Text = txtSignOffDate.Text;
                DateTime dt = Convert.ToDateTime(txtEndToD.Text);
                txtLeaveStartDate.Text = dt.AddDays(1).ToString();
                txtEndToD.CssClass = "readonlytextbox";
                txtEndToD.ReadOnly = true;
            }
        }
        else
        {
            txtEndToD.Text = "";
            txtLeaveStartDate.Text = "";
            txtEndToD.CssClass = "input";
            txtEndToD.ReadOnly = false;
        }
    }

    protected void OnEtodClick(object sender, EventArgs e)
    {

        if (txtSignOffDate.Text != null)
        {

            DateTime dt = Convert.ToDateTime(txtEndToD.Text);
            txtLeaveStartDate.Text = dt.AddDays(1).ToString();
            if (chkEndTravel.Checked == true)
            {
                txtEndToD.CssClass = "readonlytextbox";
                txtEndToD.ReadOnly = true;
            }
        }

    }

    protected void CrewSignOff_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCrewSignOff())
                {
                    ucError.Visible = true;
                    return;
                }
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Plese note,the seafarer would be signed off with the details provided.Kindly confirm to proceed";
                RadWindowManager1.RadConfirm("Please note,the seafarer would be signed off with the details provided.Kindly confirm to proceed", "confirm", 320, 150, null, "Confirm");

            }
        }
        catch (Exception ex)
        {
            SetEmployeePrimaryDetails();
            CrewSignOffEdit(null);
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCrewSignOff()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultDate;
        int compareresult;

        if (General.GetNullableInteger(ViewState["empid"].ToString()) == null)
        {
            ucError.ErrorMessage = "Select a Employee from the list";

        }
        if (!DateTime.TryParse(txtSignOffDate.Text, out resultDate))
            ucError.ErrorMessage = "SignOff Date is required";
        else if (DateTime.TryParse(txtSignOffDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-Off Date should be earlier than current date";
        }
        else
        {
            if (DateTime.Compare(DateTime.Parse(txtSignedOn.Text), DateTime.Parse(txtSignOffDate.Text)) > 0)
            {
                ucError.ErrorMessage = "SignOff Date should not be greater than SignOn Date";
            }
        }
        if (!DateTime.TryParse(txtEndToD.Text, out resultDate))
            ucError.ErrorMessage = "End ToD Date is required";
        else if (DateTime.TryParse(txtEndToD.Text, out resultDate))
        {
            compareresult = DateTime.Compare(DateTime.Parse(txtSignOffDate.Text), resultDate);
            if (chkEndTravel.Checked == true)
            {
                if (compareresult > 0)
                    ucError.ErrorMessage = "End ToD Date should be greater or equal than SignOff Date";
            }
            else
            {
                if (compareresult > 0 || compareresult == 0)
                    ucError.ErrorMessage = "End ToD Date should be greater than SignOff Date";
            }

        }

        if (DateTime.TryParse(txtSignOffDate.Text, out resultDate) && DateTime.TryParse(txtLeaveStartDate.Text, out resultDate))
        {
            if (!DateTime.TryParse(txtEndToD.Text, out resultDate))
            {
                compareresult = DateTime.Compare(DateTime.Parse(txtSignOffDate.Text), DateTime.Parse(txtLeaveStartDate.Text));
                if (compareresult > 0 || compareresult == 0)
                {
                    ucError.ErrorMessage = "Leave Start Date should be greater than SignOff Date";
                }
            }
            else
            {
                compareresult = DateTime.Compare(resultDate, DateTime.Parse(txtLeaveStartDate.Text));
                if (compareresult > 0 || compareresult == 0)
                {
                    ucError.ErrorMessage = "Leave Start Date should be greater than End ToD Date";
                }
            }
        }

        if (!DateTime.TryParse(txtLeaveStartDate.Text, out resultDate) && DateTime.TryParse(txtLeaveCompletionDate.Text, out resultDate))
        {
            ucError.ErrorMessage = "Leave Start Date is required";
        }
        else if (DateTime.TryParse(txtLeaveStartDate.Text, out resultDate) && DateTime.TryParse(txtLeaveCompletionDate.Text, out resultDate))
        {
            int compareResult1 = DateTime.Compare(DateTime.Parse(txtLeaveStartDate.Text), DateTime.Parse(txtLeaveCompletionDate.Text));
            if (compareResult1 > 0 || compareResult1 == 0)
            {
                ucError.ErrorMessage = "Leave Completion Date should be greater than Leave Start Date";
            }
        }
        if (ddlPort.SelectedSeaport.Trim().Equals("Dummy") || ddlPort.SelectedSeaport.Trim().Equals(""))
            ucError.ErrorMessage = "Port is required.";
        if (ddlSignOffReason.SelectedSignOffReason.Trim().Equals("Dummy") || ddlSignOffReason.SelectedSignOffReason.Trim().Equals(""))
            ucError.ErrorMessage = "Signoff Reason is required.";
        if (txtDOA.Text != "" || txtDOA.Text != null)
        {
            if (txtDOAGivenDate.Text == "")
                ucError.ErrorMessage = "DOA Given Date is required.";
        }
        if (txtDOAGivenDate.Text != "" || txtDOAGivenDate.Text != null)
        {
            if (txtDOA.Text == "")
                ucError.ErrorMessage = "DOA is required.";
        }
        return (!ucError.IsError);
    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNONDATE"].ToString()));
                txtVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                txtReliefDue.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELEFDUEDATE"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ResetFormControlValues(Control parent)
    {

        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void btnSignOff_Click(object sender, EventArgs e)
    {
        try
        {
           

                PhoenixCrewSignOnOff.CrewSignOffUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        int.Parse(ViewState["signonoffid"].ToString()),
                                                         int.Parse(ViewState["empid"].ToString()), int.Parse(ddlPort.SelectedSeaport),
                                                         DateTime.Parse(txtSignOffDate.Text), General.GetNullableDateTime(txtLeaveStartDate.Text), General.GetNullableDateTime(txtLeaveCompletionDate.Text)
                                                         , int.Parse(ddlSignOffReason.SelectedSignOffReason), txtSignOffRemarks.Text, General.GetNullableInteger(ddlSignOn.SelectedSignOn), General.GetNullableDateTime(txtDOA.Text)
                                                         , General.GetNullableDateTime(txtDOAGivenDate.Text), General.GetNullableDateTime(txtStandByDate.Text), General.GetNullableDateTime(txtEndToD.Text), 0, 1);

                ResetFormControlValues(this);
                SetEmployeePrimaryDetails();
                MenuSignOff.Visible = false;
                if (Request.QueryString["r"] != null)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList(null,'ifMoreInfo',null);", true);

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
