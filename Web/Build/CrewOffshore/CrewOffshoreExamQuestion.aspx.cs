using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshoreExamQuestion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["examrequestid"] = "";
                ViewState["questionid"] = "";
                ViewState["ismultipleoption"] = "";
                ViewState["islastquestion"] = "";
                ViewState["canmovetonextquestion"] = "";
                ViewState["totalquestions"] = "";
                ViewState["correctansweryn"] = "";
                ViewState["score"] = "";
                ViewState["attemptnumber"] = "";
                ViewState["examscore"] = "";
                ViewState["examresult"] = "";
                ViewState["examanswered"] = "";
                ViewState["CourseRequestId"] = "";
                ViewState["ExamId"] = "";

                ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["examrequestid"] != null && Request.QueryString["examrequestid"].ToString() != "")
                    ViewState["examrequestid"] = Request.QueryString["examrequestid"].ToString();
                if (Request.QueryString["courserequestid"] != null && Request.QueryString["courserequestid"].ToString() != "")
                    ViewState["CourseRequestId"] = Request.QueryString["courserequestid"].ToString();
                if (Request.QueryString["examid"] != null && Request.QueryString["examid"].ToString() != "")
                    ViewState["ExamId"] = Request.QueryString["examid"].ToString();
                
                BindExamRequest();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;        
        string testanswer = "";
        btnNext.Visible = true;
        btnSubmit.Visible = false;
        cblAnswerList.Enabled = true;
        rblAnswerList.Enabled = true;
        DataTable dt = PhoenixCrewOffshoreExamRequest.SearchExamQuestions(General.GetNullableGuid(ViewState["examrequestid"].ToString())
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , 1
                                                            , ref iRowCount, ref iTotalPageCount);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (dt.Rows.Count > 0)
        {
            lblQuestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
            ViewState["questionid"] = dt.Rows[0]["FLDQUESTIONID"].ToString();
            ViewState["ismultipleoption"] = dt.Rows[0]["FLDISMULTIPLEANSWER"].ToString();
            ViewState["islastquestion"] = dt.Rows[0]["FLDISLASTQUESTION"].ToString();
            ViewState["canmovetonextquestion"] = dt.Rows[0]["FLDMOVENEXTQUESTIONYN"].ToString(); 
            ViewState["totalquestions"] = dt.Rows[0]["FLDTOTALQUESTIONS"].ToString();
            ViewState["correctansweryn"] = dt.Rows[0]["FLDCORRECTANSWERYN"].ToString();
            ViewState["attemptnumber"] = dt.Rows[0]["FLDATTEMPTNUMBER"].ToString();
            ViewState["score"] = dt.Rows[0]["FLDSCORE"].ToString();
            ViewState["examanswered"] = dt.Rows[0]["FLDEXAMANSWERED"].ToString();
            testanswer = dt.Rows[0]["FLDTESTANSWER"].ToString();
            if (dt.Rows[0]["FLDEXAMSCORE"].ToString() != null && dt.Rows[0]["FLDEXAMSCORE"].ToString() != "")
                ViewState["examscore"] = dt.Rows[0]["FLDEXAMSCORE"].ToString();
            if (dt.Rows[0]["FLDEXAMRESULT"].ToString() != null && dt.Rows[0]["FLDEXAMRESULT"].ToString() != "")
                ViewState["examresult"] = dt.Rows[0]["FLDEXAMRESULT"].ToString();
        }

        if (General.GetNullableInteger(ViewState["islastquestion"].ToString()) == 1)
        {
            btnSubmit.Text = "Submit";            
        }
        if (General.GetNullableInteger(ViewState["islastquestion"].ToString()) == 1 && ViewState["correctansweryn"].ToString().Equals("1"))
        {
            btnSubmit.Visible = true;
        }

        lblNote.Text = "Instructions:";
        lblNote.Text = lblNote.Text + "<br />1) There are totally " + ViewState["totalquestions"].ToString() + " questions need to be answered.";
        lblNote.Text = lblNote.Text + "<br />2) Each correct answer will be given 1 mark if answered correctly in 1st attempt.";
        lblNote.Text = lblNote.Text + "<br />3) Each correct answer will be given 0.5 mark if answered correctly in 2nd attempt.";
        lblNote.Text = lblNote.Text + "<br />4) Maximum 2 attempts are allowed.";        

        if (ViewState["correctansweryn"].ToString().Equals("0") && ViewState["attemptnumber"].ToString()== "1")
        {
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Sorry!! Your answer is wrong. Try Again.";
            
        }
        else if (ViewState["correctansweryn"].ToString().Equals("0") && ViewState["attemptnumber"].ToString() == "2")
        {
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Sorry!! Your answer is wrong. The Correct Answer is option " + testanswer;            
            btnSubmit.Visible = true;
            btnNext.Visible = false;
            cblAnswerList.Enabled = false;
            rblAnswerList.Enabled = false;

        }
        else if (ViewState["correctansweryn"].ToString().Equals("1"))
        {
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Green;
            lblError.Text = "Your answer is correct.";            
            btnSubmit.Visible = true;
            btnNext.Visible = false;
            cblAnswerList.Enabled = false;
            rblAnswerList.Enabled = false;

        }
        else
        {
            lblError.Visible = false;
        }

        
    }

    protected void BindAnswer()
    {
        DataTable dt = PhoenixRegistersTestAnswers.ListTestAnswers(General.GetNullableInteger(ViewState["questionid"].ToString()));
        rblAnswerList.DataSource = dt;
        rblAnswerList.DataTextField = "FLDANSWERTEXT";
        rblAnswerList.DataValueField = "FLDANSWERID";
        rblAnswerList.DataBind();

        cblAnswerList.DataSource = dt;
        cblAnswerList.DataTextField = "FLDANSWERTEXT";
        cblAnswerList.DataValueField = "FLDANSWERID";
        cblAnswerList.DataBind();

        if (ViewState["ismultipleoption"].ToString().Equals("1"))
        {
            cblAnswerList.Visible = true;
            rblAnswerList.Visible = false;
        }
        else
        {
            cblAnswerList.Visible = false;
            rblAnswerList.Visible = true;
        }

        if (General.GetNullableInteger(ViewState["islastquestion"].ToString()) == 1 && General.GetNullableDecimal(ViewState["score"].ToString()) != null)
        {
            rblAnswerList.Visible = false;
            cblAnswerList.Visible = false;
        }
        if (ViewState["correctansweryn"].ToString().Equals("0") && ViewState["attemptnumber"].ToString() == "2")
        {
            cblAnswerList.Enabled = false;
            rblAnswerList.Enabled = false;
        }
        else if (ViewState["correctansweryn"].ToString().Equals("1"))
        {
            cblAnswerList.Enabled = false;
            rblAnswerList.Enabled = false;
        }

        if (ViewState["examanswered"].ToString() != "" && ViewState["examanswered"] != null)
        {
            rblAnswerList.SelectedValue = ViewState["examanswered"].ToString();
            //for (int i= 1; i <= rblAnswerList.Items.Count; i++)
            //    if (rblAnswerList.Items[i].Value == ViewState["examanswered"].ToString())
            //    {
            //        rblAnswerList.SelectedValue = ViewState["examanswered"].ToString();
            //    }

        }

    }

    protected void BindExamRequest()
    {
        Guid? newid = null;
        if (ViewState["CourseRequestId"].ToString() != null && ViewState["CourseRequestId"].ToString() != "")
        {
            PhoenixCrewOffshoreExamRequest.InsertExamRequest(General.GetNullableGuid(ViewState["CourseRequestId"].ToString())
                                                , General.GetNullableGuid(ViewState["ExamId"].ToString())
                                                , General.GetNullableDateTime(DateTime.Now.ToString())
                                                , ref newid
                                                );
            ViewState["examrequestid"] = newid;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindQuestion();
        string answerlist = "";

        if (ViewState["ismultipleoption"].ToString().Equals("1"))
            answerlist = General.ReadCheckBoxList(cblAnswerList);
        else
            answerlist = ReadRadioButtonList(rblAnswerList);

        if (rblAnswerList.Items.Count > 0 && rblAnswerList.Enabled == true)
        {
            if (General.GetNullableString(answerlist) == null)
            {
                ucError.ErrorMessage = "Answer is required.";
                ucError.Visible = true;
                return;
            }
        }

        PhoenixCrewOffshoreExamRequest.UpdateExamAnswer(General.GetNullableGuid(ViewState["examrequestid"].ToString())
                                                            , General.GetNullableInteger(ViewState["questionid"].ToString())
                                                            , General.GetNullableString(answerlist)
                                                            , General.GetNullableInteger(ViewState["islastquestion"].ToString())
                                                            );
        BindQuestion();
        if (ViewState["canmovetonextquestion"].ToString().Equals("1"))
        {
            int pagenumber = (int)ViewState["PAGENUMBER"];
            pagenumber = pagenumber + 1;
            ViewState["PAGENUMBER"] = pagenumber;
            BindQuestion();
            BindAnswer();
        }
        if (General.GetNullableInteger(ViewState["islastquestion"].ToString()) == 1 && General.GetNullableDecimal(ViewState["score"].ToString()) != null)
        {
            lblQuestion.Visible = false;
            rblAnswerList.Visible = false;
            cblAnswerList.Visible = false;
            btnSubmit.Visible = false;
            lblNote.Visible = false;
            lblThanks.Visible = true;
            lblThanks.Text = "Your result is: " + ViewState["examresult"].ToString() + "<br/>Your score is: " + ViewState["examscore"].ToString() +"<br/> Thank You!!!!!";
            lblError.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('chml', null, true);", true);
        } 
        
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //BindQuestion();
        string answerlist = "";

        if (ViewState["ismultipleoption"].ToString().Equals("1"))
            answerlist = General.ReadCheckBoxList(cblAnswerList);
        else
            answerlist = ReadRadioButtonList(rblAnswerList);

        if (General.GetNullableString(answerlist) == null)
        {
            ucError.ErrorMessage = "Answer is required.";
            ucError.Visible = true;
            return;
        }

        PhoenixCrewOffshoreExamRequest.UpdateExamAnswer(General.GetNullableGuid(ViewState["examrequestid"].ToString())
                                                            , General.GetNullableInteger(ViewState["questionid"].ToString())
                                                            , General.GetNullableString(answerlist)
                                                            , General.GetNullableInteger(ViewState["islastquestion"].ToString())
                                                            );
        BindQuestion();       
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
