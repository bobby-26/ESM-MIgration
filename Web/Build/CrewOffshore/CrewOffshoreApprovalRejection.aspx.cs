using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreApprovalRejection : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
            strEmployeeId = string.Empty;
        else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            strEmployeeId = Request.QueryString["empid"];
        else if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
            strEmployeeId = Filter.CurrentCrewSelection;
        else if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
            strEmployeeId = Filter.CurrentNewApplicantSelection;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["empid"] = strEmployeeId;
            ViewState["crewplandtkey"] = "";
            ViewState["crewplanid"] = "";
            ViewState["calledfrom"] = "";
        }
            PhoenixToolbar toolbar = new PhoenixToolbar();          

            if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                SetEmployeePrimaryDetails();

            if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                SetNewApplicantPrimaryDetails();

            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
            {
                ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
            }

            if (Request.QueryString["crewplandtkey"] != null && Request.QueryString["crewplandtkey"].ToString() != "")
            {
                ViewState["crewplandtkey"] = Request.QueryString["crewplandtkey"].ToString();
            }

            if (Request.QueryString["calledfrom"] != null && Request.QueryString["calledfrom"].ToString() != "")
            {
                ViewState["calledfrom"] = Request.QueryString["calledfrom"].ToString();
            }

            if (!string.IsNullOrEmpty(ViewState["calledfrom"].ToString()) && ViewState["calledfrom"].ToString().Equals("pendingapproval"))
            {
                ucPDStatus.ShortNameFilter = "APR";
                ucPDStatus.Enabled = false;
                ucPDStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 99, "APR");
                toolbar.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
            }

            if (!string.IsNullOrEmpty(ViewState["calledfrom"].ToString()) && ViewState["calledfrom"].ToString().Equals("rejectedproposal"))
            {
                ucPDStatus.ShortNameFilter = "AWA";
                toolbar.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
            }

            if (!string.IsNullOrEmpty(ViewState["calledfrom"].ToString()) && ViewState["calledfrom"].ToString().Equals("proposalhistory"))
            {
                ucPDStatus.ShortNameFilter = "APR";
                ucPDStatus.Enabled = false;
                ucPDStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 99, "APR");
                toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            }

            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();

            BindData();
        
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
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

        if (ucUser.SelectedUser != "")
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["crewplandtkey"].ToString()), null, sortexpression, sortdirection
              , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, "11", General.GetNullableInteger(ucUser.SelectedUser));
        }
        else
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["crewplandtkey"].ToString()), null, sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, "11", null);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            repDiscussion.DataSource = ds.Tables[0];
            repDiscussion.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], repDiscussion);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("POSTCOMMENT"))
            {
                if (!IsCommentValid(txtNotesDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreApproveProposal.RejectProposal(General.GetNullableGuid(ViewState["crewplanid"].ToString()), General.GetNullableInteger(ucPDStatus.SelectedHard));

                PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(ViewState["crewplandtkey"].ToString())
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , txtNotesDescription.Text.Trim(), "11");

                BindData();
                txtNotesDescription.Text = "";
                ucStatus.Text = "Approval rejection correspondence updated.";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList('codehelprejection', null, true);", true);
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreProposalHistory.aspx?personalmaster=true");
                }
                if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreProposalHistory.aspx?newapplicant=true");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {

        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();

    }

    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["empid"].ToString()));

            tdempno.Visible = false;
            tdempnum.Visible = false;

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ImgSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
