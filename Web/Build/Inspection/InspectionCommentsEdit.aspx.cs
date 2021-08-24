using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Inspection_InspectionCommentsEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {

            if (Request.QueryString["RISKASSESSMENTID"] != null && Request.QueryString["RISKASSESSMENTID"].ToString() != string.Empty)
            {
                ViewState["RISKASSESSMENTID"] = Request.QueryString["RISKASSESSMENTID"].ToString();
            }
            if (Request.QueryString["COMMENTID"] != null && Request.QueryString["COMMENTID"].ToString() != string.Empty)
            {
                ViewState["COMMENTID"] = Request.QueryString["COMMENTID"].ToString();
            }
            BindData();
        }
        
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddImageLink("javascript:openNewWindow('attachment','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
        //        + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT
        //        + "'); return false;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
        //MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();

    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (ViewState["COMMENTID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionRiskAssessmentGenericExtn.RACommentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["RISKASSESSMENTID"].ToString())
                                                                                        , new Guid(ViewState["COMMENTID"].ToString())
                                                                                        , Convert.ToInt32(rblbtn1.SelectedItem.Value)
                                                                                        , General.GetNullableDateTime(ucCommentsDueDate.Text)
                                                                                        , General.GetNullableDateTime(ucCommentsCompletionDate.Text)
                                                                                        , txtOfficeRemarks.Text);
                    txtOfficeRemarks.Text = "";
                    rblbtn1.SelectedItem.Selected = false;
                    ucCommentsDueDate.Text = "";
                    BindData();
                }
            }

            ucStatus.Text = "Saved successfully.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        string strrblbtn1 = "";
        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.RACommentEdit(new Guid(ViewState["COMMENTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
            ucCommentsDueDate.Text = dr["FLDDUEDATE"].ToString();
            ucCommentsCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
            ViewState["ATTACHMENTCODE"] = dr["FLDDTKEY"].ToString();
            
            if (dr["FLDACCEPTEDYN"].ToString() == "1")
            {
                strrblbtn1 = "1";
                rblbtn1.SelectedValue = strrblbtn1;
            }
            else
            {
                strrblbtn1 = "0";
                rblbtn1.SelectedValue = strrblbtn1;
            }
        }
    }


    protected void rblbtn1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblbtn1.SelectedValue == "0")
        {
            ucCommentsDueDate.Enabled = false;
        }
        else
        {
            ucCommentsDueDate.Enabled = true;
        }
    }
}
