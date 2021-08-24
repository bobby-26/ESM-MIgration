using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegistersSignoffFeedBackQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSignoffFeedBackQuestions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFeedbackQst')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersSignoffFeedBackQuestionsAdd.aspx?type=ADD&QuestionId=')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuRegistersFeedbackQst.AccessRights = this.ViewState;
            MenuRegistersFeedbackQst.MenuList = toolbar.Show();         
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFeedbackQst.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDORDERNO", "FLDQUESTIONNAME", "FLDRANKAPPLICABLECODE", "FLDISCOMMENTSYNSTATUS", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Order", "Question", "Rank Applicable", "Comments Y/N", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersSignoffFeedBackQuestions.FeedBackQuestionsSearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvFeedbackQst.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=SignOffFeedBackQuestions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sign Off FeedBack Questions</h3></td>");
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

    protected void MenuRegistersFeedbackQst_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDORDERNO", "FLDQUESTIONNAME", "FLDRANKAPPLICABLECODE", "FLDISCOMMENTSYNSTATUS", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Order","Question", "Rank Applicable","Comments Y/N","Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersSignoffFeedBackQuestions.FeedBackQuestionsSearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
             gvFeedbackQst.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvFeedbackQst", "Sign Off FeedBack Questions", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFeedbackQst.DataSource = ds;
            gvFeedbackQst.VirtualItemCount = iRowCount;
        }
        else
        {
            gvFeedbackQst.DataSource = "";
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvFeedbackQst.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {       
        BindData();
        gvFeedbackQst.Rebind();
    }
    

    private void InsertCategory(string OrderNo, string QuestionName,string RankApplicable, int Commentsyn, int Activeyn)
    {
        PhoenixRegistersSignoffFeedBackQuestions.InsertFeedBackQuestions(General.GetNullableInteger(OrderNo)
            ,QuestionName
            ,RankApplicable
            ,Commentsyn
            ,Activeyn
            ,PhoenixSecurityContext.CurrentSecurityContext.UserCode
             );
    }

    private void UpdateCategory(int Questionid, string OrderNo, string QuestionName, string RankApplicable, int Commentsyn, int Activeyn)
    {
        PhoenixRegistersSignoffFeedBackQuestions.UpdateFeedBackQuestions(
            Questionid
            ,General.GetNullableInteger(OrderNo)
            ,QuestionName
            ,RankApplicable
            ,Commentsyn
            ,Activeyn
            ,PhoenixSecurityContext.CurrentSecurityContext.UserCode);  
    }

    private bool IsValidCategory(string CategoryName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (CategoryName == string.Empty)
            ucError.ErrorMessage = "Category Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteCategory(int Questionid)
    {
        PhoenixRegistersSignoffFeedBackQuestions.DeleteFeedBackQuestions(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Questionid);
    }

  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    protected void gvFeedbackQst_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCategory(Int32.Parse(((RadLabel)e.Item.FindControl("lblQuestionId")).Text));
                BindData();
                gvFeedbackQst.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Session["FLAGID"] = ((RadLabel)e.Item.FindControl("lblQuestionId")).Text;
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

    protected void gvFeedbackQst_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFeedbackQst.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvFeedbackQst_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string sQuestionId = ((RadLabel)e.Item.FindControl("lblQuestionId")).Text;
            string sQuestion = ((LinkButton)e.Item.FindControl("lnkQuestionName")).Text;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersSignoffFeedBackQuestionsAdd.aspx?type=EDIT&QuestionId=" + sQuestionId + "');");
            }
            LinkButton cmdOption = (LinkButton)e.Item.FindControl("cmdOption");
            if (cmdOption != null)
            {
                cmdOption.Visible = SessionUtil.CanAccess(this.ViewState, cmdOption.CommandName);
                cmdOption.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersSignoffFeedBackQuestionOptions.aspx?QuestionId=" + sQuestionId + "');");
            }          
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

    protected void gvFeedbackQst_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //ViewState["SORTEXPRESSION"] = e.SortExpression;
        //switch (e.NewSortOrder)
        //{
        //    case GridSortOrder.Ascending:
        //        ViewState["SORTDIRECTION"] = "0";
        //        break;
        //    case GridSortOrder.Descending:
        //        ViewState["SORTDIRECTION"] = "1";
        //        break;
        //}
    }
}
