using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersFeedbackQuestions : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFeedbackQuestions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");           
            MenuQuestions.AccessRights = this.ViewState;
            MenuQuestions.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuQuestions_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {          
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

        string[] alColumns = { "FLDORDERNO", "FLDQUESTIONNAME" };
        string[] alCaptions = { "Order No","Question"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersFeedbackQuestions.FeedbackQuestionsSearch(
                    sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    gvQuestions.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);
        General.SetPrintOptions("gvQuestions", "Feedback Questions", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuestions.DataSource = ds;
            gvQuestions.VirtualItemCount = iRowCount;
        }
        else
        {
            gvQuestions.DataSource = "";
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDORDERNO", "FLDQUESTIONNAME" };
            string[] alCaptions = { "Order No", "Question" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixRegistersFeedbackQuestions.FeedbackQuestionsSearch(
                   sortexpression, sortdirection,
                   (int)ViewState["PAGENUMBER"],
                   gvQuestions.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=FeedbackQuestions.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Feedback Questions</h3></td>");
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
            foreach (DataRow dr in ds.Tables[0].Rows)
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertQuestion(string questionname, int isobjectivetypeyn)
    {
        PhoenixRegistersFeedbackQuestions.InsertFeedbackQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , questionname
            , isobjectivetypeyn);
    }
    
    private void UpdateQuestion(Guid questionid,int orderno, string questionname, int isobjectivetypeyn)
    {
        PhoenixRegistersFeedbackQuestions.UpdateFeedbackQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , orderno
            , questionid
            , questionname
            , isobjectivetypeyn);
    }

    private void DeleteQuestion(Guid questionid)
    {
        PhoenixRegistersFeedbackQuestions.DeleteFeedbackQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , questionid);
    }

    private bool IsValidQuestion(string question)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (question == null || question.ToString().Trim() == "")
            ucError.ErrorMessage = "Question is required.";

        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvQuestions_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvQuestions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidQuestion(
                     ((RadTextBox)e.Item.FindControl("txtQuestionNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertQuestion(
                    ((RadTextBox)e.Item.FindControl("txtQuestionNameAdd")).Text,
                    int.Parse(((RadCheckBox)e.Item.FindControl("chkObjectiveTypeYNAdd")).Checked == true ? "1" : "0"));
                BindData();
                gvQuestions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string questionid = ((RadLabel)e.Item.FindControl("lblQuestionId")).Text;
                DeleteQuestion(new Guid(questionid));
                BindData();
                gvQuestions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string orderno = ((UserControlMaskNumber)e.Item.FindControl("txtOrderNoEdit")).Text;
                string questionid = ((RadLabel)e.Item.FindControl("lblQuestionIdEdit")).Text;
                RadCheckBox chkObjectiveType = (RadCheckBox)e.Item.FindControl("chkObjectiveTypeEdit");
                string ObjectiveTypeYN = chkObjectiveType.Checked == true ? "1" : "0";

                if (General.GetNullableInteger(orderno) == null)
                {
                    ucError.Visible = true;
                    ucError.ErrorMessage = "Order No. Required";
                    return;
                }

                if (!IsValidQuestion(((RadTextBox)e.Item.FindControl("txtQuestionNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuestion(new Guid(questionid)
                                 , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtOrderNoEdit")).Text)
                                 , ((RadTextBox)e.Item.FindControl("txtQuestionNameEdit")).Text
                                 , int.Parse(ObjectiveTypeYN)
                                  );               
                BindData();
                gvQuestions.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvQuestions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuestions.CurrentPageIndex + 1;
        BindData();
    }
}
