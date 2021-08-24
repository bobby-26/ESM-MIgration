using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantRemarks : PhoenixBasePage
{
    string strEmployeeId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbarmain.Show();

        if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
            strEmployeeId = string.Empty;
        else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            strEmployeeId = Request.QueryString["empid"];
        else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
            strEmployeeId = Filter.CurrentNewApplicantSelection;

        if (!IsPostBack)
        {
            lblDate.Visible = false;
            txtInActiveDate.Visible = false;

            ViewState["ACTIVEID"] = string.Empty;
            ViewState["DOAID"] = string.Empty;
            ViewState["PLANNED"] = string.Empty;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            CrewInActiveEdit();
            DAO();
            SetEmployeePrimaryDetails();
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
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
                ViewState["PLANNED"] = dt.Rows[0]["FLDPLANNED"].ToString();                
            }
            DataTable dt1 = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(strEmployeeId));
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

    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Rebind();
    }
    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;
        BindData();
    }
    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
      
        if (General.GetNullableInteger(RadMcUserTD.SelectedValue) != null)
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
                        , General.GetNullableInteger(RadMcUserTD.SelectedValue));
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
                ViewState["PAGENUMBER"] = null;
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
        try
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

                PhoenixNewApplicantManagement.NewApplicantFollowUpInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(strEmployeeId),
                    txtDOA.Text,
                    General.GetNullableInteger(ViewState["DOAID"].ToString()),
                    General.GetNullableDateTime(txtFollowupDate.Text),
                    General.GetNullableInteger(ViewState["ACTIVEID"].ToString()),
                    int.Parse(ddlInactiveReason.SelectedHard),
                    byte.Parse(rblInActive.SelectedValue),
                    General.GetNullableDateTime(txtInActiveDate.Text),
                    General.GetNullableInteger(ucSeafarerRequirement.SelectedQuick));

                PhoenixCommonDiscussion.TransTypeDiscussionInsert(GetCurrentEmployeeDTkey()
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , txtNotesDescription.Text.Trim(), "4");

                PhoenixCommonDiscussion.discussionupdateintoempstatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , txtNotesDescription.Text.Trim(), General.GetNullableInteger(strEmployeeId));

                Rebind();
                CrewInActiveEdit();
                DAO();
                txtNotesDescription.Text = "";
                txtInActiveDate.Text = "";
                txtFollowupDate.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsCommentValid(string strComment, string doa, string followupdate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        if (rblInActive.SelectedValue == "1")
        {
            if (General.GetNullableDateTime(doa) == null)
                ucError.ErrorMessage = "Crew Date of Availability is required.";
        }
        if (rblInActive.SelectedValue == "1" && General.GetNullableDateTime(followupdate) == null)
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
            if (DateTime.Compare(resultdate, DateTime.Now) < 0 && rblInActive.SelectedValue != "0")
                ucError.ErrorMessage = "Date of Availability should be later than current date";
        }

        if (string.IsNullOrEmpty(txtInActiveDate.Text) && txtInActiveDate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Relief Due is required.";

        else if (!string.IsNullOrEmpty(txtInActiveDate.Text)
            && DateTime.TryParse(txtInActiveDate.Text, out resultdate))
        {
            if (txtInActiveDate.CssClass == "input_mandatory" && DateTime.Compare(resultdate, DateTime.Now) < 0)
                ucError.ErrorMessage = "Relief Due should be later than current date";
        }

        if (rblInActive.SelectedValue == "")
            ucError.ErrorMessage = "Active/In-Active is required.";

        if (General.GetNullableInteger(ddlInactiveReason.SelectedHard) == null)
            ucError.ErrorMessage = "Reason is required.";

        return (!ucError.IsError);

    }

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

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

    protected void ActiveRemarks(object sender, EventArgs e)
    {
        ddlInactiveReason.SelectedHard = "";

        if (rblInActive.SelectedValue == "1")
        {
            ddlInactiveReason.ShortNameFilter = ViewState["PLANNED"].ToString() == "0" ? "ONB,ONL" : "OLP";
            ddlInactiveReason.HardTypeCode = "54";
            ddlInactiveReason.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, (ViewState["PLANNED"].ToString() == "0" ? "ONB,ONL" : "OLP"));
            txtFollowupDate.Enabled = true;
            txtDOA.Enabled = true;

            if (General.GetNullableString(txtFollowupDate.Text) != null)
                ((RadComboBox)ddlInactiveReason.FindControl("ddlHard")).Focus();
            else
                txtFollowupDate.FindControl("txtDate").Focus();
        }
        else if (rblInActive.SelectedValue == "0")
        {
            ddlInactiveReason.HardTypeCode = "96";
            ddlInactiveReason.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 96, 1, "");

            txtFollowupDate.Text = "";
            txtFollowupDate.Enabled = false;
            txtDOA.Text = "";
            txtDOA.Enabled = false;


        }
        else
        {

            ddlInactiveReason.HardTypeCode = "96";
            ddlInactiveReason.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 96, 1, "");
          
        }
    }

    private void CrewInActiveEdit()
    {
        DataTable dt = PhoenixCrewActive.CrewNewApplicantActiveInactiveList(General.GetNullableInteger(strEmployeeId).Value);

        if (dt.Rows.Count > 0)
        {
            rblInActive.SelectedIndex = -1;         
            ViewState["ACTIVEID"] = dt.Rows[0]["FLDACTIVEID"].ToString();
            ActiveRemarks(null, null);            
            if (((RadComboBox)ddlInactiveReason.FindControl("ddlHard")).ToString().Contains("onboard"))
            {
                lblDate.Visible = true;
                txtInActiveDate.Visible = true;
                lblDate.Text = "Relief Due";
                txtInActiveDate.CssClass = "input_mandatory";
            }
        }
        else
        {
            ActiveRemarks(null, null);
        }
    }

    private void DAO()
    {
        DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(strEmployeeId));

        if (dtDAOEdit.Rows.Count > 0)
        {
            ViewState["DOAID"] = dtDAOEdit.Rows[0]["FLDDOAID"].ToString();

            txtDOA.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOA"].ToString());
            //txtFollowupDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDFOLLOWUPDATE"].ToString()); 
        }
    }

    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ddlReason_TextChanged(object sender, EventArgs e)
    {
        if (rblInActive.SelectedValue == "1" && ((RadComboBox)ddlInactiveReason.FindControl("ddlHard")).SelectedItem.Text.ToLower().Contains("onboard"))
        {
            lblDate.Visible = true;
            txtInActiveDate.Visible = true;
            lblDate.Text = "Relief Due";
            txtInActiveDate.CssClass = "input_mandatory";
        }
        else
        {
            lblDate.Visible = false;
            txtInActiveDate.Visible = false;
            txtInActiveDate.Text = "";
            lblDate.Text = "Date";
            txtInActiveDate.CssClass = "input";
            txtNotesDescription.Focus();
        }
    }
    protected void Rebind()
    {
        gvDiscussion.DataSource = null;
        gvDiscussion.Rebind();
    }
}
