using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewSignOff : PhoenixBasePage
{
    int? fldSignOnId;
    string strEmployeeId = string.Empty;
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

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOff.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewSignOff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSignOff.AccessRights = this.ViewState;
            MenuSignOff.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                SetEmployeePrimaryDetails();

                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                CrewSignOffEdit(null);
                ViewState["SIGNOFF"] = 0;
                gvCrewSignOff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNOFFRANK", "FLDSIGNOFFREASON", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Vessel", "Rank", "Reason", "Sign Off Date" };
        DataSet ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), null);
        General.SetPrintOptions("gvCrewSignOff", "Crew Sign Off", alCaptions, alColumns, ds);
        gvCrewSignOff.DataSource = ds;
        gvCrewSignOff.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void gvCrewSignOff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSignOff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

                string signoffid = ((RadLabel)e.Item.FindControl("lblSignOffId")).Text;
                CrewSignOffEdit(General.GetNullableInteger(signoffid));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewSignOff.SelectedIndexes.Clear();
        gvCrewSignOff.EditIndexes.Clear();
        gvCrewSignOff.DataSource = null;
        gvCrewSignOff.Rebind();
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNOFFRANK", "FLDSIGNOFFREASON", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Vessel", "Rank", "Reason", "Sign Off Date" };

        DataSet ds;

        ds = PhoenixCrewSignOnOff.CrewSignOnList(General.GetNullableInteger(strEmployeeId), null);


        Response.AddHeader("Content-Disposition", "attachment; filename=Crew Sign Off.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Sign Off</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void CrewSignOffEdit(int? signonid)
    {
        DataTable dt = PhoenixCrewSignOnOff.CrewSignOffEdit(General.GetNullableInteger(strEmployeeId), signonid);
        if (dt.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dt.Rows[0]["FLDBTOD"].ToString()))
                txtBtoD.Text = General.GetDateTimeToString(dt.Rows[0]["FLDBTOD"].ToString());
            if (dt.Rows[0]["FLDSIGNONOFFID"].ToString() != "" || dt.Rows[0]["FLDSIGNONOFFID"].ToString() != null)
                txtSignOnOffId.Text = dt.Rows[0]["FLDSIGNONOFFID"].ToString();
            if (dt.Rows[0]["FLDDOAGIVENDATE"].ToString() != "" || dt.Rows[0]["FLDDOAGIVENDATE"].ToString() != null)
                txtDOAGivenDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDOAGIVENDATE"].ToString());
            if (dt.Rows[0]["FLDSTANDBYDATE"].ToString() != "" || dt.Rows[0]["FLDSTANDBYDATE"].ToString() != null)
                txtStandByDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSTANDBYDATE"].ToString());
            if (dt.Rows[0]["FLDETOD"].ToString() != "" || dt.Rows[0]["FLDETOD"].ToString() != null)
                txtEndToD.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETOD"].ToString());
            if (dt.Rows[0]["FLDLEAVESTARTDATE"].ToString() != "" || dt.Rows[0]["FLDLEAVESTARTDATE"].ToString() != null)
                txtLeaveStartDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLEAVESTARTDATE"].ToString());
            if (dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString() != "" || dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString() != null)
                txtLeaveCompletionDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLEAVECOMPLETIONDATE"].ToString());
            txtSignOffDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNOFFDATE"].ToString());
            if (dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString() != null)
                ddlPort.SelectedSeaport = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
            if (dt.Rows[0]["FLDSIGNOFFREASONID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFREASONID"].ToString() != null)
                ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();
            if (dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString() != "" || dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString() != null)
                ddlSignOn.SelectedSignOn = dt.Rows[0]["FLDSIGNOFFRELIEVERID"].ToString();
            txtSignOffRemarks.Text = dt.Rows[0]["FLDSIGNOFFREMARKS"].ToString();
            if (dt.Rows[0]["FLDISEDITABLE"].ToString() == "0")
                MenuSignOff.Visible = false;
            else
                txtSignedOn.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNONDATE"].ToString());
            MenuSignOff.Visible = true;
        }
    }
    protected void EnableControls()
    {
        txtSignOffDate.ReadOnly = false;
        txtSignOffDate.Enabled = true;
        txtSignOffDate.CssClass = "readonlytextbox";
        MenuSignOff.Visible = true;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.Enabled = false;
        txtStandByDate.CssClass = "input";
        ddlPort.Enabled = true;
        txtStandByDate.ReadOnly = false;
        txtStandByDate.Enabled = true;
        txtStandByDate.CssClass = "input";
        txtLeaveStartDate.ReadOnly = false;
        txtLeaveStartDate.Enabled = true;
        txtLeaveStartDate.CssClass = "readonlytextbox";
        txtLeaveCompletionDate.ReadOnly = false;
        txtLeaveCompletionDate.Enabled = true;
        txtLeaveCompletionDate.CssClass = "input";
        txtDOA.ReadOnly = false;
        txtDOA.Enabled = true;
        txtDOA.CssClass = "input";
        txtDOAGivenDate.ReadOnly = false;
        txtDOAGivenDate.Enabled = true;
        txtDOAGivenDate.CssClass = "input";
        txtEndToD.ReadOnly = false;
        txtEndToD.Enabled = true;
        txtEndToD.CssClass = "input_mandatory";
        txtSignOffRemarks.ReadOnly = false;
        txtSignOffRemarks.Enabled = true;
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
        txtSignOffDate.Enabled = false;
        txtSignOffDate.CssClass = "readonlytextbox";
        MenuSignOff.Visible = false;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.Enabled = false;
        txtStandByDate.CssClass = "readonlytextbox";
        ddlPort.Enabled = false;
        txtStandByDate.ReadOnly = true;
        txtStandByDate.Enabled = false;
        txtStandByDate.CssClass = "readonlytextbox";
        txtLeaveStartDate.ReadOnly = true;
        txtLeaveStartDate.Enabled = false;
        txtLeaveStartDate.CssClass = "readonlytextbox";
        txtLeaveCompletionDate.ReadOnly = true;
        txtLeaveCompletionDate.Enabled = false;
        txtLeaveCompletionDate.CssClass = "readonlytextbox";
        txtDOA.ReadOnly = true;
        txtDOA.Enabled = false;
        txtDOA.CssClass = "readonlytextbox";
        txtDOAGivenDate.ReadOnly = true;
        txtDOAGivenDate.Enabled = false;
        txtDOAGivenDate.CssClass = "readonlytextbox";
        txtEndToD.ReadOnly = true;
        txtEndToD.Enabled = false;
        txtEndToD.CssClass = "readonlytextbox";
        txtSignOffRemarks.ReadOnly = true;
        txtSignOffRemarks.Enabled = false;
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
                txtEndToD.Enabled = false;
            }
        }
        else
        {
            txtEndToD.Text = "";
            txtLeaveStartDate.Text = "";
            txtEndToD.CssClass = "input";
            txtEndToD.ReadOnly = false;
            txtEndToD.Enabled = true;
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
                txtEndToD.Enabled = false;
            }
        }

    }
    protected void CrewSignOff_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (txtSignOnOffId.Text != "")
        {
            fldSignOnId = int.Parse(txtSignOnOffId.Text);
        }
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCrewSignOff())
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Please note,the seafarer would be signed off with the details provided. Kindly confirm to proceed", "confirm", 320, 150, null, "Confirm");

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

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";
        }
        if (ddlDispensationapplied.SelectedValue == "0")
        {
            if (string.IsNullOrEmpty(txtSignOffRemarks.Text.Trim()))
                ucError.ErrorMessage = "Remarks required";
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
                ucError.ErrorMessage = "SignOff Date should not be greater than SignOn Date";
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
        //else if (DateTime.TryParse(txtLeaveStartDate.Text, out resultDate) && !DateTime.TryParse(txtLeaveCompletionDate.Text, out resultDate))
        //{
        //    ucError.ErrorMessage = "Leave Completion Date is required";
        //}
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

        if (!string.IsNullOrEmpty(txtEndToD.Text) && DateTime.TryParse(txtDOA.Text, out resultDate)
            && DateTime.Compare(resultDate, DateTime.Parse(txtEndToD.Text)) <= 0)
        {
            ucError.ErrorMessage = "Date of Availabiltiy should be later than End ToD Date";
        }
        //if (!string.IsNullOrEmpty(txtEndToD.Text) && DateTime.TryParse(txtDOAGivenDate.Text, out resultDate)
        //    && DateTime.Compare(resultDate, DateTime.Parse(txtEndToD.Text)) <= 0)
        //{
        //    ucError.ErrorMessage = "DOA Given Date Date should be later than End ToD Date";
        //}
        if (!string.IsNullOrEmpty(txtDOA.Text)
             && DateTime.TryParse(txtDOA.Text, out resultDate))
        {
            if (DateTime.Compare(resultDate, DateTime.Now) < 0)
                ucError.ErrorMessage = "Date of Availability should be later than current date";
        }

        if (!string.IsNullOrEmpty(txtDOAGivenDate.Text))
        {
            if (string.IsNullOrEmpty(txtDOA.Text))
            {
                ucError.ErrorMessage = "Date of Availabiltiy is required";
                ucError.Visible = true;

            }
        }


        DataSet ds = PhoenixRegistersreasonssignoff.Listreasonssignoffbyreasonid(General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason));

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDABBREVIATION"].ToString() == "MEDICA")
            {
                if (General.GetNullableString(txtSignOffRemarks.Text) == null)
                    ucError.ErrorMessage = "Signoff Remarks is required.";
            }
            else if (General.GetNullableDateTime(txtSignOffDate.Text) != null && General.GetNullableDateTime(txtReliefDue.Text) != null)
            {
                DateTime reliefduedate = DateTime.Parse(txtReliefDue.Text);
                DateTime signoffdate = DateTime.Parse(txtSignOffDate.Text);
                TimeSpan t = reliefduedate - signoffdate;
                double noofdays = t.TotalDays;
                if (noofdays < 0)
                    noofdays = -(noofdays);

                if (noofdays >= 15)
                {
                    if (General.GetNullableString(txtSignOffRemarks.Text) == null)
                        ucError.ErrorMessage = "Signoff Remarks is required.";
                }

            }
        }


        return (!ucError.IsError);
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));
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
                            ((RadTextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((RadCheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadRadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((RadDropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((RadListBox)c).SelectedIndex = 0;
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
                                                    int.Parse(txtSignOnOffId.Text),
                                                     int.Parse(strEmployeeId), int.Parse(ddlPort.SelectedSeaport),
                                                     DateTime.Parse(txtSignOffDate.Text), General.GetNullableDateTime(txtLeaveStartDate.Text), General.GetNullableDateTime(txtLeaveCompletionDate.Text)
                                                     , int.Parse(ddlSignOffReason.SelectedSignOffReason), txtSignOffRemarks.Text, General.GetNullableInteger(ddlSignOn.SelectedSignOn), General.GetNullableDateTime(txtDOA.Text)
                                                     , General.GetNullableDateTime(txtDOAGivenDate.Text), General.GetNullableDateTime(txtStandByDate.Text), General.GetNullableDateTime(txtEndToD.Text), 0, 1
                                                     , General.GetNullableInteger(ucDelayEarlyReason.SelectedQuick)
                                                     , General.GetNullableInteger(ddlDispensationapplied.SelectedItem.Text == "--select--" ? null : ddlDispensationapplied.SelectedValue));


            ResetFormControlValues(this);
            SetEmployeePrimaryDetails();
            CrewSignOffEdit(null);
            Rebind();
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
    protected void ddlSignOffReason_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersreasonssignoff.Listreasonssignoffbyreasonid(General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason));
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDABBREVIATION"].ToString() == "MEDICA")
                txtSignOffRemarks.CssClass = "input_mandatory";
            else
                txtSignOffRemarks.CssClass = "input";
        }

    }
}
