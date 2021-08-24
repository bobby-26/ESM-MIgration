using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Registers_RegistersTestAnswers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersTestAnswers.aspx?QuestionId="+Request.QueryString["QuestionId"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTestAnswers')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreAnswers.AccessRights = this.ViewState;
            MenuOffshoreAnswers.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Test Questions", "QUESTION",ToolBarDirection.Right);
            MenuTest.AccessRights = this.ViewState;
            MenuTest.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["courseid"] = "";

                if (Request.QueryString["courseid"] != null && Request.QueryString["courseid"].ToString() != "")
                    ViewState["courseid"] = Request.QueryString["courseid"].ToString();

                DataTable dt = PhoenixRegistersTestAnswers.ListTestQuestions(General.GetNullableInteger(string.IsNullOrEmpty(Request.QueryString["QuestionId"]) ? "" : Request.QueryString["QuestionId"]));
                if (dt.Rows.Count > 0)
                {
                    txtQuestion.Text = dt.Rows[0]["FLDQUESTION"].ToString();
                    gvTestAnswers.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                }
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName= ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("QUESTION"))
        {
            Response.Redirect("../Registers/RegistersTestQuestions.aspx?courseid=" + ViewState["courseid"].ToString());
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDQUESTION", "FLDANSWER", "FLDSORTORDER", "FLDCORRECTANSWER" };
        string[] alCaptions = { "Question", "Answer", "Order", "Correct Answer" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersTestAnswers.SearchTestAnswers(General.GetNullableInteger(Request.QueryString["QuestionId"]), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvTestAnswers.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TestAnswers.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Test Answers</h3></td>");
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
        string[] alColumns = {"FLDQUESTION", "FLDANSWER", "FLDSORTORDER", "FLDCORRECTANSWER" };
        string[] alCaptions = {"Question", "Answer", "Order","Correct Answer" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixRegistersTestAnswers.SearchTestAnswers(General.GetNullableInteger(Request.QueryString["QuestionId"]), sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvTestAnswers.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvTestAnswers", "Test Answers", alCaptions, alColumns, ds);

        gvTestAnswers.DataSource = ds;
        gvTestAnswers.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }
    
    protected void gvTestAnswers_ItemDataBound(object sender, GridItemEventArgs e)
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
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }


    protected void gvTestAnswers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(Request.QueryString["QuestionId"],
                    ((RadTextBox)e.Item.FindControl("txtAnswerAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSortAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersTestAnswers.InsertTestAnswer(int.Parse(Request.QueryString["QuestionId"]),
                    ((RadTextBox)e.Item.FindControl("txtAnswerAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSortAdd")).Text),
                    General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkCorrectAnswerYNAdd")).Checked == true ? "1" : "0"));

                BindData();
                gvTestAnswers.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int answerid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDANSWERID"].ToString());
                PhoenixRegistersTestAnswers.DeleteTestAnswer(answerid);
                BindData();
                gvTestAnswers.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(Request.QueryString["QuestionId"],
                    ((RadTextBox)e.Item.FindControl("txtAnswerEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucSortEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                int answerid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDANSWERID"].ToString());
                
                string iscorrect = String.Empty;
                RadCheckBox CorrectYN = ((RadCheckBox)e.Item.FindControl("chkCorrectAnswerYNEdit"));
                if (CorrectYN != null)
                {
                    iscorrect = CorrectYN.Checked == true ? "1" : "0";
                }
                PhoenixRegistersTestAnswers.UpdateTestAnswer(answerid,
                    int.Parse(Request.QueryString["QuestionId"]),
                    ((RadTextBox)e.Item.FindControl("txtAnswerEdit")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucSortEdit")).Text),
                    General.GetNullableInteger(iscorrect));
                
                BindData();
                gvTestAnswers.Rebind();
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    private bool IsValidData(string questionid, string answer, string sort)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(questionid) == null)
            ucError.ErrorMessage = "Question is required.";

        if (General.GetNullableString(answer) == null)
            ucError.ErrorMessage = "Answer is required.";

        if (General.GetNullableInteger(sort) == null)
            ucError.ErrorMessage = "Sort Order is required.";

        return (!ucError.IsError);
    }
    protected void gvTestAnswers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTestAnswers.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
