using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Collections;
using Telerik.Web.UI;

public partial class DocumentManagementQuestionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDMSQuestions.AccessRights = this.ViewState;
            MenuDMSQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SECTIONID"] = "";
                ViewState["REVISIONID"] = "";
                ViewState["QUESTIONID"] = "";

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

                if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
                    ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();

                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                {
                    ViewState["QUESTIONID"] = Request.QueryString["QUESTIONID"].ToString();

                    cbActive.Enabled = true;
                    BindDMSQuestion();
                }
                else
                {
                    cbActive.Checked = true;
                    cbActive.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDMSQuestion()
    {
        if (ViewState["QUESTIONID"] != null && ViewState["QUESTIONID"].ToString() != string.Empty)
        {
            DataSet ds = new DataSet();
            ds = PhoenixDocumentManagementQuestion.DMSQuestionEdit(new Guid(ViewState["QUESTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtQuestion.Text = dr["FLDDMSQUESTION"].ToString();
                txtSortorder.Text = dr["FLDSORTORDER"].ToString();

                if (dr["FLDACTIVEYN"].ToString() == "1")
                    cbActive.Checked = true;
                else
                    cbActive.Checked = false;

            }
        }
    }

    protected void MenuDMSQuestions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if(CommandName.ToUpper().Equals("SAVE"))
            {
                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                {
                    UpdateDMSQuestion();                    
                }
                else
                {
                    InsertDMSQuestion();                    
                }
               
            }         
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       
    }
    protected void InsertDMSQuestion()
    {
        try
        {
            if (!IsValidDMSQuestion(txtQuestion.Text,txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementQuestion.DMSQuestionInsert(General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                                                                    , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                                                    , General.GetNullableString(txtQuestion.Text)
                                                                    , General.GetNullableInteger(txtSortorder.Text));

            ucStatus.Text = "DMS Question Added Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('QuestionEdit', 'Questions');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void UpdateDMSQuestion()
    {
        try
        {
            if(!IsValidDMSQuestion(txtQuestion.Text,txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }
            int Active = cbActive.Checked == true ? 1 : 0;
            PhoenixDocumentManagementQuestion.DMSQuestionUpdate(General.GetNullableGuid(ViewState["QUESTIONID"].ToString())
                                                                    , General.GetNullableString(txtQuestion.Text)
                                                                    , General.GetNullableInteger(txtSortorder.Text)
                                                                    , Active);

            ucStatus.Text = "DMS Question Updated Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('QuestionEdit', 'Questions');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidDMSQuestion (string Question, string SortOrder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Question.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";
        if (SortOrder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort order is required.";

        return (!ucError.IsError);
    }
}