using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLicenceRequestExtraInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);    
        LicenceRequest.AccessRights = this.ViewState;
        LicenceRequest.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
        
            ViewState["REQUESTID"] = null;
            ViewState["DTKEY"] = null;
            ViewState["flag"] = null;
            if (Request.QueryString["dtkey"] != null && Request.QueryString["dtkey"].ToString() != string.Empty)
                ViewState["DTKEY"] = Request.QueryString["dtkey"].ToString();

            if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() != string.Empty)
                ViewState["REQUESTID"] = Request.QueryString["rid"].ToString();

            BindData();
        
            ddlFlagAddress.DataSource = PhoenixRegistersAddress.ListFlagAddress(General.GetNullableInteger(ViewState["flag"].ToString()));
            ddlFlagAddress.DataTextField = "FLDNAME";
            ddlFlagAddress.DataValueField = "FLDADDRESSCODE";
            ddlFlagAddress.DataBind();
            
            if (ViewState["ADDRESSCODE"].ToString() != string.Empty) ddlFlagAddress.SelectedValue = ViewState["ADDRESSCODE"].ToString();

        }  
      
    }
    
    protected void ddlFlagAddress_DataBound(object sender, EventArgs e)
    {
        ddlFlagAddress.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void LicenceRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["REQUESTID"] != null)
                {
                    PhoenixCrewLicenceRequest.UpdateLicenceRequestMoreinfo(new Guid(ViewState["REQUESTID"].ToString()), txtAuthorizedRep.Text.Trim(), txtDesignation.Text.Trim(), General.GetNullableInteger(ddlCompanyNameInReport.SelectedCompany),General.GetNullableInteger(ddlFlagAddress.SelectedValue));
                    
                    ucStatus.Text = "Saved Successfully.";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
                }
            }
            if (CommandName.ToUpper().Equals("POSTCOMMENT"))
            {
                if (!IsCommentValid(txtNotesDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(ViewState["DTKEY"].ToString())
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , txtNotesDescription.Text.Trim(), "4");

                BindDiscussion();
                gvDiscussion.Rebind();

                txtNotesDescription.Text = "";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            if (ViewState["REQUESTID"] != null)
            {
                DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtReqNo.Text = dt.Rows[0]["FLDREQUISITIONNUMBER"].ToString();
                    txtName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                    txtFileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
                    txtFlag.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
                    ViewState["flag"] = dt.Rows[0]["FLDFLAGID"].ToString();
                    txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();                    
                    ddlCompanyNameInReport.SelectedCompany = dt.Rows[0]["FLDCOMPANYADDRINREPORT"].ToString();
                    ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                    txtAuthorizedRep.Text = dt.Rows[0]["FLDAUTHORIZEDREP"].ToString();
                    txtDesignation.Text = dt.Rows[0]["FLDDESIGNATION"].ToString();
                    txtJoinedVessel.Text = dt.Rows[0]["FLDJOINEDVESSEL"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void OnClickJoinedVessel(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewLicenceRequest.UpdateLicenceRequestJoinedVessel(new Guid(ViewState["REQUESTID"].ToString()));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["DTKEY"] == null ? null : ViewState["DTKEY"].ToString())
                                                                   , null, sortexpression, sortdirection
                                                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , gvDiscussion.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount
                                                                   , "4"
                                                                   , General.GetNullableInteger(ucUser.SelectedUser));
        }
        else
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["DTKEY"] == null ? null : ViewState["DTKEY"].ToString())
                                                                 , null
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
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

    protected void ImgBtnSearch_Click(object sender, EventArgs e)
    {
        BindDiscussion();
        gvDiscussion.Rebind();
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is Required";

        return (!ucError.IsError);
    }
    
    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;
        BindDiscussion();
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




}