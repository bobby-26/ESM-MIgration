using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class PreSeaNewApplicantRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVEID"] = string.Empty;
            ViewState["ACCESS"] = "1";
            NewApplicantInActiveEdit();
            SetNewApplicantPrimaryDetails();
        }
        if (ViewState["ACCESS"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT");
            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();
        }
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

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(GetCurrentEmployeeDTkey(), null, sortexpression, sortdirection
          , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, "4");

        Title1.Text = "General Remarks";


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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixPreSeaNewApplicantPersonal.NewApplicantFollowUpInsert( PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection)
                                                                         , General.GetNullableDateTime(txtFollowupDate.Text)
                                                                         , General.GetNullableInteger(ViewState["ACTIVEID"].ToString())
                                                                         , int.Parse(ddlInactiveReason.SelectedQuick)
                                                                         , byte.Parse(rblInActive.SelectedValue)
                                                                         , General.GetNullableDateTime(txtFollowupDate.Text));

            PhoenixCommonDiscussion.TransTypeDiscussionInsert(GetCurrentEmployeeDTkey()
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), "4");
            NewApplicantInActiveEdit();
            BindData();
            txtNotesDescription.Text = "";
        }
    }

    
    private bool IsCommentValid(string strComment)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        if (rblInActive.SelectedValue == "1" && General.GetNullableDateTime(txtFollowupDate.Text) == null)
            ucError.ErrorMessage = "Follow Up Date is required.";

        if (!string.IsNullOrEmpty(txtFollowupDate.Text)
            && DateTime.TryParse(txtFollowupDate.Text, out resultdate))
        {
            if (DateTime.Compare(resultdate, General.GetNullableDateTime(DateTime.Now.ToString("dd/MMM/yyyy")).Value) <= 0)
                ucError.ErrorMessage = "Follow Up Date should be later than current date";          
        }

        if (rblInActive.SelectedValue == "")
            ucError.ErrorMessage = "Active/In-Active is required.";

        if (General.GetNullableInteger(ddlInactiveReason.SelectedQuick) == null)
            ucError.ErrorMessage = "Reason is required.";

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

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));
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

    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }

    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlReason_TextChanged(object sender, EventArgs e)
    {
      
    }
    protected void ActiveRemarks(object sender, EventArgs e)
    {
        if (rblInActive.SelectedValue == "1")
        {            
            ddlInactiveReason.QuickTypeCode = "124";
            ddlInactiveReason.QuickList = PhoenixPreSeaQuick.ListQuick(1, 124);
            txtFollowupDate.Visible = true;
            lblFollowupDate.Visible = true;
            if (General.GetNullableString(txtFollowupDate.Text) != null)
                ((RadComboBox)ddlInactiveReason.FindControl("ddlQucik")).Focus();
            else
                txtFollowupDate.FindControl("txtDate").Focus();
        }
        else if (rblInActive.SelectedValue == "0")
        {
            
            ddlInactiveReason.QuickTypeCode = "125";
            ddlInactiveReason.QuickList = PhoenixPreSeaQuick.ListQuick(1, 125);
            txtFollowupDate.Text = "";
            txtFollowupDate.Visible = false;
            lblFollowupDate.Visible = false;
            //((DropDownList)ddlInactiveReason.FindControl("ddlQucik")).Focus();
        }
        else
        {
            //ddlInactiveReason.ShortNameFilter = "DTH,MDL,NTB";
            ddlInactiveReason.QuickTypeCode = "125";
            ddlInactiveReason.QuickList = PhoenixPreSeaQuick.ListQuick(1, 125);
            txtFollowupDate.Visible = true;
            lblFollowupDate.Visible = true;          
        }
    }
    private void NewApplicantInActiveEdit()
    {
        DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantActiveInactiveList( PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));

        if (dt.Rows.Count > 0)
        {
            rblInActive.SelectedIndex = -1;            
            ViewState["ACTIVEID"] = dt.Rows[0]["FLDACTIVEID"].ToString();
            ActiveRemarks(null, null);

        }
        else
        {
            ActiveRemarks(null, null);
        }
    }



}
