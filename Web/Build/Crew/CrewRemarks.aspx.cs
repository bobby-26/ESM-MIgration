using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["ACCESS"] = "0";
        if (General.GetNullableInteger(Filter.CurrentCrewSelection).HasValue)
            ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value);

        if (ViewState["ACCESS"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();
        }
        if (!IsPostBack)
        {
            ViewState["DOAID"] = string.Empty;

            //ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            DAO();
            SetEmployeePrimaryDetails();
            gvDiscussion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    
        EnableFollowup();     
    }


    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;
        BindDiscussion();
    }

    private void BindDiscussion()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ucUser.SelectedUser != "")
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(
                        GetCurrentEmployeeDTkey()
                        , null
                        , sortexpression
                        , sortdirection
                        , gvDiscussion.CurrentPageIndex + 1
                        , gvDiscussion.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , "4"
                        , General.GetNullableInteger(ucUser.SelectedUser));
        }
        else
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(
                        GetCurrentEmployeeDTkey()
                        , null
                        , sortexpression
                        , sortdirection
                        , gvDiscussion.CurrentPageIndex + 1
                        , gvDiscussion.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , "4"
                        , null);

        }

        gvDiscussion.DataSource = ds.Tables[0];
        gvDiscussion.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvDiscussion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                //ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text, txtDOA.Text, txtFollowupDate.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (chkDisable.Checked != true)
            {
                if (!IsCrewOnboard())
                {
                    PhoenixCrewManagement.CrewFollowUpInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(Filter.CurrentCrewSelection),
                        txtDOA.Text,
                        General.GetNullableInteger(ViewState["DOAID"].ToString()),
                        General.GetNullableDateTime(txtFollowupDate.Text),
                        General.GetNullableInteger(ucSeafarerRequirement.SelectedQuick));
                }
            }
            PhoenixCommonDiscussion.TransTypeDiscussionInsert(GetCurrentEmployeeDTkey()
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), "4");

            PhoenixCommonDiscussion.discussionupdateintoempstatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), General.GetNullableInteger(Filter.CurrentCrewSelection));
         
            chkDisable.Checked = false;
            txtNotesDescription.Text = "";
            AllowNonmandatory(null, null);

            BindDiscussion();
            gvDiscussion.Rebind();

            EnableFollowup();

        }
    }


    protected void ImgSearch_Click(object sender, EventArgs e)
    {
        BindDiscussion();
        gvDiscussion.Rebind();
    }
   
    protected void AllowNonmandatory(object sender, EventArgs e)
    {
        if (chkDisable.Checked == true)
        {
            txtDOA.CssClass = "";
            txtFollowupDate.CssClass = "";
            txtNotesDescription.Focus();
        }
        else
        {
            txtDOA.CssClass = "input_mandatory";
            txtFollowupDate.CssClass = "input_mandatory";
            if (General.GetNullableString(txtDOA.Text) == null)
                txtDOA.FindControl("txtDate").Focus();
            else
                txtFollowupDate.FindControl("txtDate").Focus();
        }
    }

    private bool IsCommentValid(string strComment, string doa, string followupdate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        if (!IsCrewOnboard())
        {
            if (chkDisable.Checked != true)
            {
                if (General.GetNullableDateTime(doa) == null)
                    ucError.ErrorMessage = "Crew Date of Availability is required.";

                if (General.GetNullableDateTime(followupdate) == null)
                    ucError.ErrorMessage = "Crew Follow Up Date is required.";
                if (!string.IsNullOrEmpty(followupdate)
                && DateTime.TryParse(followupdate, out resultdate))
                {
                    if (DateTime.Compare(resultdate, General.GetNullableDateTime(DateTime.Now.ToString("dd/MMM/yyyy")).Value) < 0)
                        ucError.ErrorMessage = "Follow Up Date should be later than current date";                    
                }
                if (!string.IsNullOrEmpty(doa)
                  && DateTime.TryParse(doa, out resultdate))
                {
                    if (DateTime.Compare(resultdate, DateTime.Now) < 0)
                        ucError.ErrorMessage = "Date of Availability should be later than current date";
                }
            }

        }

        return (!ucError.IsError);
    }
    
    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }
    
    private void DAO()
    {
        DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(Filter.CurrentCrewSelection));

        if (dtDAOEdit.Rows.Count > 0)
        {
            ViewState["DOAID"] = dtDAOEdit.Rows[0]["FLDDOAID"].ToString();

            txtDOA.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOA"].ToString());            
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
            }
            DataTable dt1 = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt1.Rows.Count > 0)
            {
                txtMobileNumber1.Text = dt1.Rows[0]["FLDMOBILENUMBER"].ToString();
                txtMobileNumber2.Text = dt1.Rows[0]["FLDMOBILENUMBER2"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsCrewOnboard()
    {
        DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        string crewstatusid = dt.Rows[0]["FLDEMPLOYEESTATUS"].ToString();

        if (crewstatusid == "216" || crewstatusid == "220")
            return true;
        else
            return false;

    }


    private void EnableFollowup()
    {
        if (IsCrewOnboard())
        {
            txtDOA.Enabled = false;
            txtDOA.Text = "";
            txtDOA.CssClass = "readonlytextbox";
            txtFollowupDate.Enabled = false;
            txtFollowupDate.Text = "";
            txtFollowupDate.CssClass = "readonlytextbox";
        }
        else
        {
            txtDOA.Enabled = true;
            txtFollowupDate.Enabled = true;
        }
    }


}
