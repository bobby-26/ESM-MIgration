using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Collections;
using Telerik.Web.UI;

public partial class InspectionQuestionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionQuestions.AccessRights = this.ViewState;
            MenuInspectionQuestions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["INSPECTIONID"] = "";
                ViewState["QUESTIONID"] = "";

                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();

                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                {
                    ViewState["QUESTIONID"] = Request.QueryString["QUESTIONID"].ToString();

                    cbActive.Enabled = true;
                    BindInspectionQuestion();
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
            return;
        }
    }
    private void BindInspectionQuestion()
    {
        if (ViewState["QUESTIONID"] != null && ViewState["QUESTIONID"].ToString() != string.Empty)
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionQuestion.InspectionQuestionEdit(new Guid(ViewState["QUESTIONID"].ToString()));
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

    protected void MenuInspectionQuestions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                {
                    UpdateQuestion();
                }
                else
                {
                    InsertQuestion();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void InsertQuestion()
    {
        try
        {
            if (!IsValidQuestion(txtQuestion.Text, txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionQuestion.InspectionQuestionInsert(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                                    , General.GetNullableString(txtQuestion.Text)
                                                                    , General.GetNullableInteger(txtSortorder.Text));

            ucStatus.Text = "Inspection Question Added Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('QuestionEdit', 'Questions');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void UpdateQuestion()
    {
        try
        {
            if (!IsValidQuestion(txtQuestion.Text, txtSortorder.Text))
            {
                ucError.Visible = true;
                return;
            }
            int Active = cbActive.Checked == true ? 1 : 0;
            PhoenixInspectionQuestion.InspectionQuestionUpdate(General.GetNullableGuid(ViewState["QUESTIONID"].ToString())
                                                                    , General.GetNullableString(txtQuestion.Text)
                                                                    , General.GetNullableInteger(txtSortorder.Text)
                                                                    , Active);

            ucStatus.Text = "Inspection Question Updated Successfully";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('QuestionEdit', 'Questions');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidQuestion(string Question, string SortOrder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Question.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";
        if (SortOrder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort order is required.";

        return (!ucError.IsError);
    }
}