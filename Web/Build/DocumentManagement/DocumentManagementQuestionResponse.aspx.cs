using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementQuestionResponse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {                

                ViewState["PAGENUMBER"] = 1;
                ViewState["sectionid"] = "";
                ViewState["revisionid"] = "";
                ViewState["questionid"] = "";

                if (Request.QueryString["sectionid"] != null && Request.QueryString["sectionid"].ToString() != "")
                    ViewState["sectionid"] = Request.QueryString["sectionid"].ToString();
                if (Request.QueryString["revisionid"] != null && Request.QueryString["revisionid"].ToString() != "")
                    ViewState["revisionid"] = Request.QueryString["revisionid"].ToString();

                BindQuestion();
                BindAnswer();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindQuestion()
    {
        DataTable dt = PhoenixDocumentManagementQuestion.UserUnansweredQuestionList(new Guid(ViewState["sectionid"].ToString())
                                                                                    , General.GetNullableGuid(ViewState["revisionid"].ToString()));
       
        if (dt.Rows.Count > 0)
        {
            lblQuestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
            ViewState["questionid"] = dt.Rows[0]["FLDQUESTIONID"].ToString();
            lblsection.Text = dt.Rows[0]["FLDSECTIONNAME"].ToString();
            lblOfficeRemarks.Text = "Details of Change: " + dt.Rows[0]["FLDOFFICEREMARKS"].ToString();

            if (dt.Rows[0]["FLDPROCEEDYN"].ToString().Equals("1"))
            {
                if (dt.Rows.Count.Equals(1))
                    btnNext.Text = "Submit";

                lblNote.Text = dt.Rows[0]["FLDINSTRUCTION"].ToString();
            }
            else
            {
                lblNote.Text = "";
                lblQuestion.Text = "";
                btnNext.Visible = false;
                PhoenixDocumentManagementQuestion.DMSSectionReadInsert(new Guid(ViewState["sectionid"].ToString()), new Guid(ViewState["revisionid"].ToString()));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "fnReloadList('codehelp2', 'ifMoreInfo', '','');", true);
            }
        }
        else
        {
            btnNext.Visible = false;            
            lblNote.Text = "";
            lblQuestion.Text = "";
            ViewState["questionid"] = "";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp1', 'codehelp');", true);
        }
    }

    protected void BindAnswer()
    {
        DataTable dt = PhoenixDocumentManagementQuestion.QuestionOptionList(General.GetNullableGuid(ViewState["questionid"].ToString()));
        rblAnswerList.DataSource = dt;
        rblAnswerList.DataTextField = "FLDDMSOPTION";
        rblAnswerList.DataValueField = "FLDOPTIONID";
        rblAnswerList.DataBind();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            string answerlist = "";

            answerlist = ReadRadioButtonList(rblAnswerList);

            if (General.GetNullableString(answerlist) == null)
            {
                ucError.ErrorMessage = "Answer is required.";
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementQuestion.UserAnsweredQuestion(new Guid(ViewState["questionid"].ToString())
                                                                , new Guid(answerlist.ToString()));

            ucStatus.Text = "Your answer is correct.";

            BindQuestion();
            BindAnswer();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public static string ReadRadioButtonList(RadioButtonList rbl)
    {
        string list = string.Empty;

        foreach (ListItem item in rbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
