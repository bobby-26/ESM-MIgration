using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreInterviewAnswers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreInterviewAnswers.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreAnswers')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreAnswers.AccessRights = this.ViewState;
            MenuOffshoreAnswers.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                BindQuestions(ddlQuestion);
                gvOffshoreAnswers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDQUESTION", "FLDANSWER", "FLDSCORE" };
        string[] alCaptions = { "Question", "Answer", "Score" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreInterviewAnswer.SearchInterviewAnswers(General.GetNullableInteger(ddlQuestion.SelectedValue), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreAnswers.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreInterviewAnswers.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Interview Answers</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOffshoreAnswers_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUESTION", "FLDANSWER", "FLDSCORE" };
        string[] alCaptions = { "Question", "Answer", "Score" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreInterviewAnswer.SearchInterviewAnswers(General.GetNullableInteger(ddlQuestion.SelectedValue), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreAnswers.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreAnswers", "Interview Answers", alCaptions, alColumns, ds);

        gvOffshoreAnswers.DataSource = ds;
        gvOffshoreAnswers.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvOffshoreAnswers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            RadComboBox ddlQuestionEdit = (RadComboBox)e.Item.FindControl("ddlQuestionEdit");
            if (ddlQuestionEdit != null)
            {
                BindQuestions(ddlQuestionEdit);
                ddlQuestionEdit.SelectedValue = dr["FLDQUESTIONID"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            RadComboBox ddlQuestionAdd = (RadComboBox)e.Item.FindControl("ddlQuestionAdd");
            if (ddlQuestionAdd != null)
            {
                BindQuestions(ddlQuestionAdd);
            }
        }
    }

    protected void BindQuestions(RadComboBox ddl)
    {
        ddl.DataSource = PhoenixCrewOffshoreInterviewQuestion.ListInterviewQuestions(null);
        ddl.DataTextField = "FLDQUESTION";
        ddl.DataValueField = "FLDQUESTIONID";
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddl.DataBind();
        
    }

    protected void gvOffshoreAnswers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlQuestionAdd")).SelectedValue,
                    ((RadTextBox)e.Item.FindControl("txtAnswerAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreInterviewAnswer.InsertInterviewAnswer(int.Parse(((RadComboBox)e.Item.FindControl("ddlQuestionAdd")).SelectedValue),
                    ((RadTextBox)e.Item.FindControl("txtAnswerAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text));

                BindData();
                gvOffshoreAnswers.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int answerid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDANSWERID"].ToString());

                PhoenixCrewOffshoreInterviewAnswer.DeleteInterviewAnswer(answerid);
                BindData();
                gvOffshoreAnswers.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlQuestionEdit")).SelectedValue,
                    ((RadTextBox)e.Item.FindControl("txtAnswerEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                int answerid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDANSWERID"].ToString());

                PhoenixCrewOffshoreInterviewAnswer.UpdateInterviewAnswer(answerid,
                                                                        int.Parse(((RadComboBox)e.Item.FindControl("ddlQuestionEdit")).SelectedValue),
                                                                        ((RadTextBox)e.Item.FindControl("txtAnswerEdit")).Text,
                                                                        int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text));
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string questionid, string answer, string score)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(questionid) == null)
            ucError.ErrorMessage = "Question is required.";

        if (General.GetNullableString(answer) == null)
            ucError.ErrorMessage = "Answer is required.";

        if (General.GetNullableInteger(score) == null)
            ucError.ErrorMessage = "Score is required.";

        return (!ucError.IsError);
    }
    protected void gvOffshoreAnswers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreAnswers.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}