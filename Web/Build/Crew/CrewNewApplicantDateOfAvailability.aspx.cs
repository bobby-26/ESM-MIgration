using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantDateOfAvailability : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    string status = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDO.AccessRights = this.ViewState;
            MenuDO.MenuList = toolbar.Show();

            if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            if (!IsPostBack)
            {
                status = Request.QueryString["status"];
                ViewState["DOAID"] = string.Empty;
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"] != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();
                DAO();
                SetEmployeePrimaryDetails();
                gvDOA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void DAO()
    {
        DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(strEmployeeId));

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
    private void BindData()
    {

        DataSet ds;
        ds = PhoenixCrewDateOfAvailability.CrewDateOfAvailabilityList(General.GetNullableInteger(strEmployeeId));

        gvDOA.DataSource = ds;

        gvDOA.VirtualItemCount = ds.Tables[0].Rows.Count;
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
    private void ResetFields()
    {
        txtDOAGivenDate.Text = string.Empty;
        ddlDOAMethod.SelectedQuick = string.Empty;
        txtDTOfTelConf.Text = string.Empty;
        txtDOA.Text = string.Empty;
        txtStandbyDate.Text = string.Empty;
        chkDOACancellation.Checked = false;
        txtRemarks.Text = string.Empty;
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
                                                                    General.GetNullableInteger(ViewState["DOAID"].ToString()),
                                                                    chkDOACancellation.Checked ? 1 : 0,
                                                                    General.GetNullableDateTime(txtFollowupDate.Text) // FollowUp Date
                                                                    , txtRemarks.Text
                                                                    );
                ViewState["DOAID"] = string.Empty;
                BindData();
                ResetFields();
                DAO();
                gvDOA.Rebind();
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

        if (string.IsNullOrEmpty(txtDOAGivenDate.Text))
            ucError.ErrorMessage = "DOA Given date is required.";

        if (ViewState["STATUS"].ToString().ToUpper() != "IN-ACTIVE")
        {
            if (string.IsNullOrEmpty(txtDOA.Text))
                ucError.ErrorMessage = "DOA is required.";
            else if (DateTime.TryParse(txtDOA.Text, out dt) == false)
                ucError.ErrorMessage = "DOA  is not in correct format";
        }
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                ViewState["STATUS"] = dt.Rows[0]["FLDSTATUS"].ToString();
                if (ViewState["STATUS"].ToString() == "IN-ACTIVE")
                {
                    txtDOA.CssClass = "input";
                }
                if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
                {
                    lblDateofTeleconference.Text = "Last Contact Date";
                    lblFollowUpDate.Text = "Next Contact Date";
                    lblFollowUpDate.Visible = true;
                    txtFollowupDate.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void gvDOA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDOA.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDOA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            {
                GridHeaderItem item = e.Item as GridHeaderItem;

                item["lblDateOfTeleConfHeader"].Text = "Last Contact Date";
            }
        }
    }
}
