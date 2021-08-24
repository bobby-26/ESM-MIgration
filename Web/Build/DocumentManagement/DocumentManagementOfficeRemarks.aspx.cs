using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementOfficeRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SAVE", "SAVE");
            MenuOfficeRemarks.AccessRights = this.ViewState;
            MenuOfficeRemarks.MenuList = toolbarmain.Show();

            if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
            {
                ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();
            }
            if (Request.QueryString["COMMENTID"] != null && Request.QueryString["COMMENTID"].ToString() != string.Empty)
            {
                ViewState["COMMENTID"] = Request.QueryString["COMMENTID"].ToString();
            }
            if (Request.QueryString["VIEW"].ToString() == "remarks")
            {
                BindOfficeRemarks();
            }
            else
            {
                BindOfficeReviewRemarks();
            }
        }
    }

    protected void MenuOfficeRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (General.GetNullableString(txtOfficeRemarks.Text) == null)
            {
                lblMessage.Text = "Office Remarks is required.";
                return;
            }


            if (ViewState["REFERENCEID"] != null && ViewState["REFERENCEID"] != null && Request.QueryString["VIEW"].ToString() == "remarks")
            {
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixDocumentManagementDocument.DMSCommentOfficeRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["REFERENCEID"].ToString())
                                                                                        , new Guid(ViewState["COMMENTID"].ToString())
                                                                                        , txtOfficeRemarks.Text);
                    txtOfficeRemarks.Text = "";
                    BindOfficeRemarks();
                    lblMessage.Text = ""; 
                }
            }
            else
            {
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixDocumentManagementDocument.DMSCommentReviewUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["COMMENTID"].ToString())
                                                                                        , Convert.ToInt32(rblbtn1.SelectedItem.Value)
                                                                                        , General.GetNullableDateTime(ucCommentsDueDate.Text)
                                                                                        , txtOfficeRemarks.Text);
                    txtOfficeRemarks.Text = "";
                    BindOfficeReviewRemarks();
                    lblMessage.Text = ""; 
                }            
            
            }
            ucStatus.Text = "Office remarks updated.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    private void BindOfficeRemarks()
    {
        DataSet ds = PhoenixDocumentManagementDocument.EditDMSComments(new Guid(ViewState["COMMENTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            repDiscussion.DataSource = ds;
            repDiscussion.DataBind();
            //txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
        }
    }

    private void BindOfficeReviewRemarks()
    {
        DataTable dt = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonRemarks(new Guid(ViewState["COMMENTID"].ToString()));

        if (dt.Rows.Count > 0 && (dt.Rows[0]["FLDCOMMENTS"].ToString() != "" || dt.Rows[0]["FLDCOMMENTS"].ToString() != null))
        {
            //txtOfficeRemarks.Text = dt.Rows[0]["FLDOFFICEREMARKS"].ToString();
            repDiscussion.DataSource = dt;
            repDiscussion.DataBind();

        }
        else
        {
            ShowNoRecordsFound(dt, repDiscussion);
        }
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

}

