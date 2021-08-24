using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewPendingApprovalRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (Request.QueryString["access"] == null)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();
            txtNotesDescription.Visible = true;
            lblPostYourCommentsHere.Visible = true;
        }
        else
        {
            txtNotesDescription.Visible = false;
            lblPostYourCommentsHere.Visible = false;
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;            
            
            SetEmployeePrimaryDetails();
            gvDiscussion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }        
    }


    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;
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

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(Request.QueryString["dtkey"].ToString())
                                                                , null, sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvDiscussion.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , "4"
                                                                , null);

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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(Request.QueryString["dtkey"].ToString())
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), "4");
            
            txtNotesDescription.Text = "";
            BindDiscussion();
            gvDiscussion.Rebind();
        }
    }
  
    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";
        return (!ucError.IsError);
    }
    
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Request.QueryString["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
