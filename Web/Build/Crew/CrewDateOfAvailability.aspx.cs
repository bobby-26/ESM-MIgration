using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewDateOfAvailability : PhoenixBasePage
{
    string strEmployeeId;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDO.AccessRights = this.ViewState;
            MenuDO.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewDateOfAvailability.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvDOA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            Filter.CurrentCrewSelection = Request.QueryString["empid"];
            if (!IsPostBack)
            {
                ViewState["DOAID"] = string.Empty;
                ViewState["LAUNCHEDFROM"] = string.Empty;
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"] != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();
                SetEmployeePrimaryDetails();
                SetEmployeeContact();
                DAO();
                gvDOA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DisableDoa()
    {
        txtDOAGivenDate.ReadOnly = true; txtDOAGivenDate.Enabled = false;
        txtDOAGivenDate.CssClass = "readonlytextbox";
        ddlDOAMethod.Enabled = false;
        ddlDOAMethod.CssClass = "readonlytextbox";
        txtDTOfTelConf.ReadOnly = true; txtDTOfTelConf.Enabled = false;
        txtDTOfTelConf.CssClass = "readonlytextbox";
        txtDOA.ReadOnly = true; txtDOA.Enabled = false;
        txtDOA.CssClass = "readonlytextbox";
        txtStandbyDate.ReadOnly = true; txtStandbyDate.Enabled = false;
        txtStandbyDate.CssClass = "readonlytextbox";
        txtFollowupDate.ReadOnly = true; txtFollowupDate.Enabled = false;
        txtFollowupDate.CssClass = "readonlytextbox";
        chkDOACancellation.Enabled = false;
        txtRemarks.CssClass = "readonlytextbox"; txtRemarks.Enabled = true;
        txtRemarks.ReadOnly = false;
    }
    private void DAO()
    {
        DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(strEmployeeId));

        if (ViewState["ONBOARD"].ToString() != string.Empty)
        {
            //DisableDoa();
            //MenuDO.Visible = false;
        }
        if (dtDAOEdit.Rows.Count > 0)
        {
            chkDOACancellation.Enabled = true;
            ViewState["DOAID"] = dtDAOEdit.Rows[0]["FLDDOAID"].ToString();
            txtDOAGivenDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOAGIVENDATE"].ToString());

            ddlDOAMethod.SelectedQuick = dtDAOEdit.Rows[0]["FLDDOAMETHOD"].ToString();

            txtDTOfTelConf.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDATEOFTELCONF"].ToString());

            txtDOA.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOA"].ToString());

            txtStandbyDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDSTANDBYDATE"].ToString());
            txtFollowupDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDFOLLOWUPDATE"].ToString());
            txtExpectedsalary.Text = dtDAOEdit.Rows[0]["FLDEXPECTEDSALARY"].ToString();
        }
        else
        {
            if (string.IsNullOrEmpty(ViewState["DOAID"].ToString()))
            {
                chkDOACancellation.Enabled = false;
                chkDOACancellation.Checked = false;
                txtRemarks.ReadOnly = true;
                txtRemarks.CssClass = "readonlytextbox";
            }
            else
            {
                chkDOACancellation.Enabled = true;
            }
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtDOA_TextChanged(object sender, EventArgs e)
    {
        DateTime FollowupDate;
        DateTime DOAdate;
        if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
        {
            DOAdate = DateTime.Parse(txtDOA.Text);
            FollowupDate = DOAdate.AddDays(14);
            txtFollowupDate.Text = FollowupDate.ToString();

        }
    }
    private void BindData()
    {
        string strEmp = Request.QueryString["empid"];
        string[] alColumns = { };
        string[] alCaptions = { };
        alColumns = new string[] { "FLDDOAGIVENDATE", "FLDDOAMETHOD", "FLDDATEOFTELCONF", "FLDSTANDBYDATE", "FLDDOA", "FLDCANCELLATIONDATE", "FLDEXPECTEDSALARY" };

        if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            alCaptions = new string[] { "DOA Given Date", "DOA Method", "Last Contact Date", "Standby Date", "Date of Avail.", "Cancellation Date", "expected salary" };
        else
            alCaptions = new string[] { "DOA Given Date", "DOA Method", "Dt.Of TeleConf.", "Standby Date", "Date of Avail.", "Cancellation Date", "expected salary" };


        DataSet ds = PhoenixCrewDateOfAvailability.CrewDateOfAvailabilityList(General.GetNullableInteger(strEmp));
        General.SetPrintOptions("gvDOA", "Crew Date of Availability", alCaptions, alColumns, ds);
        gvDOA.DataSource = ds;
        gvDOA.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void ShowExcel()
    {
        string strEmp = Request.QueryString["empid"];
        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };
        alColumns = new string[] { "FLDDOAGIVENDATE", "FLDDOAMETHOD", "FLDDATEOFTELCONF", "FLDSTANDBYDATE", "FLDDOA", "FLDCANCELLATIONDATE", "FLDEXPECTEDSALARY" };

        if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            alCaptions = new string[] { "DOA Given Date", "DOA Method", "Last Contact Date", "Standby Date", "Date of Avail.", "Cancellation Date", "expected salary" };
        else
            alCaptions = new string[] { "DOA Given Date", "DOA Method", "Dt.Of TeleConf.", "Standby Date", "Date of Avail.", "Cancellation Date", "expected salary" };

        ds = PhoenixCrewDateOfAvailability.CrewDateOfAvailabilityList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew Date of Availability.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Date of Availability</center></h5></td></tr>");
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
    private void ResetFields()
    {
        txtDOAGivenDate.Text = string.Empty;
        ddlDOAMethod.SelectedQuick = string.Empty;
        txtDTOfTelConf.Text = string.Empty;
        txtDOA.Text = string.Empty;
        txtStandbyDate.Text = string.Empty;
        chkDOACancellation.Checked = false;
        txtFollowupDate.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }
    protected void gvDOA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void DOA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidDOA())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewDateOfAvailability.DateOfAvailabilityInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    General.GetNullableInteger(strEmployeeId),
                                                                    txtDOAGivenDate.Text,
                                                                    General.GetNullableInteger(ddlDOAMethod.SelectedQuick),
                                                                    General.GetNullableDateTime(txtDTOfTelConf.Text),
                                                                    General.GetNullableDateTime(txtStandbyDate.Text),
                                                                    txtDOA.Text,
                                                                    General.GetNullableInteger(ViewState["DOAID"].ToString())
                                                                    , chkDOACancellation.Checked ? 1 : 0
                                                                    , General.GetNullableDateTime(txtFollowupDate.Text)
                                                                    , txtRemarks.Text
                                                                    , General.GetNullableInteger(txtExpectedsalary.Text));
                ViewState["DOAID"] = string.Empty;
                BindData();
                ResetFields();
                DAO();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidDOA()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dt;

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";

        }
        if (txtDOAGivenDate.Text == null)

            ucError.ErrorMessage = "DOA Given date is required.";

        if (ViewState["STATUS"].ToString().ToUpper() != "0")
        {
            if (string.IsNullOrEmpty(txtDOA.Text))
                ucError.ErrorMessage = "DOA is required.";
            else if (DateTime.TryParse(txtDOA.Text, out dt) == false)
                ucError.ErrorMessage = "Date of availability  is not in correct format";
        }
        //if (!string.IsNullOrEmpty(txtDOA.Text)
        //     && DateTime.TryParse(txtDOA.Text, out resultdate))
        //{
        //    if (DateTime.Compare(resultdate, DateTime.Now) <= 0)
        //        ucError.ErrorMessage = "Date of Availability should not be earlier than current date";
        //}
        if (txtDOAGivenDate.Text != null)
        {
            if (DateTime.TryParse(txtDOAGivenDate.Text, out dt) == false)
                ucError.ErrorMessage = "DOA Given date is not in correct format";
        }
        if (txtDTOfTelConf.Text != null)
        {
            if (DateTime.TryParse(txtDTOfTelConf.Text, out dt) == false)
                ucError.ErrorMessage = "Date Of Teleconference  is not in correct format";
        }


        if (txtStandbyDate.Text != null)
        {
            if (DateTime.TryParse(txtStandbyDate.Text, out dt) == false)
                ucError.ErrorMessage = "Stand by date  is not in correct format";
        }
        if (txtFollowupDate.Text != null)
        {
            if (DateTime.TryParse(txtFollowupDate.Text, out dt) == false)
                ucError.ErrorMessage = "Follow up date  is not in correct format";

        }
        if (chkDOACancellation.Checked && txtRemarks.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Reason for Cancellation is required.";
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
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                //txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                ViewState["STATUS"] = dt.Rows[0]["FLDSTATUSTYPE"].ToString();
                ViewState["ONBOARD"] = dt.Rows[0]["FLDPRESENTVESSEL"].ToString();
                if (ViewState["STATUS"].ToString() == "0")
                {
                    txtDOA.CssClass = "input";
                }
                if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
                {
                    lblDateOfTeleconference.Text = "Last Contact Date";
                    lblFollowUpDate.Text = "Next Contact Date";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeeContact()
    {
        try
        {
            DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = dt.Rows[0]["FLDEMPLOYEEADDRESSID"].ToString();
                ucPerRelation.SelectedQuick = dt.Rows[0]["FLDRELATIONNO"].ToString();
                txtlocalPhoneNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtlocalPhoneNumber.Text = dt.Rows[1]["FLDPHONENUMBER"].ToString();
                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();
                txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                txtLocalMobileNumber.Text = dt.Rows[1]["FLDMOBILENUMBER"].ToString();
                txtLocalPhoneNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalPhoneNumber2.Text = dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                txtLocalMobileNumber2.Text = dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
                txtLocalMobileNumber3.Text = dt.Rows[1]["FLDMOBILENUMBER3"].ToString();
                ucLocRelation.SelectedQuick = dt.Rows[1]["FLDRELATIONNO"].ToString();

                DisableContact();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DisableContact()
    {
        txtPhoneNumber.ReadOnly = true;
        txtPhoneNumber.CssClass = "readonlytextbox";
        txtEmail.ReadOnly = true;
        txtEmail.CssClass = "readonlytextbox";
        //txtMobileNumber.ReadOnly = true;
        txtMobileNumber.CssClass = "readonlytextbox";
        txtlocalPhoneNumber.ReadOnly = true;
        txtlocalPhoneNumber.CssClass = "readonlytextbox";
        txtPhoneNumber2.ReadOnly = true;
        txtPhoneNumber2.CssClass = "readonlytextbox";
        //txtMobileNumber2.ReadOnly = true;
        txtMobileNumber2.CssClass = "readonlytextbox";
        //txtMobileNumber3.ReadOnly = true;
        txtMobileNumber3.CssClass = "readonlytextbox";
        //txtLocalMobileNumber.ReadOnly = true;
        txtLocalMobileNumber.CssClass = "readonlytextbox";
        txtLocalPhoneNumber2.ReadOnly = true;
        txtLocalPhoneNumber2.CssClass = "readonlytextbox";
        //txtLocalMobileNumber2.ReadOnly = true;
        txtLocalMobileNumber2.CssClass = "readonlytextbox";
        //txtLocalMobileNumber3.ReadOnly = true;
        txtLocalMobileNumber3.CssClass = "readonlytextbox";
        ucLocRelation.Enabled = false;
        ucPerRelation.Enabled = false;


    }
    protected void RemarksMandatory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cbk = (CheckBox)sender;
            if (cbk.Checked)
            {
                txtRemarks.CssClass = "input_mandatory";
                txtRemarks.ReadOnly = false;
            }
            else
            {
                txtRemarks.CssClass = "readonlytextbox";
                txtRemarks.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
