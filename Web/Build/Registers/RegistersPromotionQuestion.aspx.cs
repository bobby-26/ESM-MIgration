using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPromotionQuestion : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersPromotionQuestion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('GvPromotionQuestion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuPromotionQuestion.AccessRights = this.ViewState;
            MenuPromotionQuestion.MenuList = toolbar1.Show();

            toolbar1 = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;                
                ViewState["SORTDIRECTION"] = null;
                GvPromotionQuestion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDRANKNAME", "FLDPROMOTIONQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Rank", "Evaluation Item", "Order Sequence" };
       
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());    
   
        DataSet ds = PhoenixRegistersAppraisalPromotionQuestion.AppraisalPromotionQuestionSearch (                                                                                          
                                                                                           General.GetNullableInteger(ucRank.SelectedRank)                                                                                       
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , GvPromotionQuestion.PageSize
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );

        General.SetPrintOptions("GvPromotionQuestion", "Promotion Question", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GvPromotionQuestion.DataSource = ds;
            GvPromotionQuestion.VirtualItemCount = iRowCount;
        }
        else
        {
            GvPromotionQuestion.DataSource = "";
        }
    }

   
    protected void MenuPromotionQuestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                GvPromotionQuestion.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
                GvPromotionQuestion.Rebind();
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

        string[] alColumns = { "FLDRANKNAME", "FLDPROMOTIONQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Rank", "Evaluation Item", "Order Sequence" };
        
        int? sortdirection = null; 
       
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersAppraisalPromotionQuestion.AppraisalPromotionQuestionSearch(                                                                                        
                                                                                         General.GetNullableInteger(ucRank.SelectedRank)                                                                                       
                                                                                         , sortdirection
                                                                                         , (int)ViewState["PAGENUMBER"]
                                                                                         , GvPromotionQuestion.PageSize
                                                                                         , ref iRowCount
                                                                                         , ref iTotalPageCount
                                                                                      );
        Response.AddHeader("Content-Disposition", "attachment; filename=Promotion.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Promotion Question </h3></td>");
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
    private bool IsValidPromotionQuestion(string evaluationitem, string rankid, string ordersequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (evaluationitem.Trim().Equals(""))
            ucError.ErrorMessage = "Evaluation Item is required.";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableInteger(ordersequence) == null)
            ucError.ErrorMessage = "Order Sequence is required.";

        return (!ucError.IsError);
    }

    private void InsertPromotionQuestion(string evaluationitem, int rannid, int? ordersequence)
    {
        PhoenixRegistersAppraisalPromotionQuestion.InsertAppraisalPromotionQuestion (
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode                                                                        
                                                                         , evaluationitem
                                                                         , rannid
                                                                         , ordersequence
                                                                  );
    }

    private void UpdatePromotionQuestion(int questionid, string evaluationitem, int rankid, int? ordersequence)
    {
       PhoenixRegistersAppraisalPromotionQuestion.UpdateAppraisalPromotionQuestion (
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid                                                                      
                                                                        , evaluationitem
                                                                        , rankid
                                                                        , ordersequence
                                                                 );
    }

    private void DeletePromotionQuestion(int questionid)
    {
      PhoenixRegistersAppraisalPromotionQuestion.DeleteAppraisalPromotionQuestion  (
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                 );
    }
    protected void GvPromotionQuestion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPromotionQuestion(
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    ((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank,
                    ((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertPromotionQuestion(
                    ((RadTextBox)e.Item.FindControl("txtevaluationiteminsert")).Text,
                    int.Parse(((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank),
                    int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text));

                BindData();
                GvPromotionQuestion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePromotionQuestion(int.Parse(((RadLabel)e.Item.FindControl("lblquestionid")).Text));
                BindData();
                GvPromotionQuestion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidPromotionQuestion(
                  ((RadTextBox)e.Item.FindControl("txtevaluationitemedit")).Text,
                  ((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank,
                  ((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePromotionQuestion(
                                 int.Parse(((RadLabel)e.Item.FindControl("lblquestionidedit")).Text)
                                 , ((RadTextBox)e.Item.FindControl("txtevaluationitemedit")).Text
                                 , int.Parse(((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank)
                                 , int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text)
                                 );
                BindData();
                GvPromotionQuestion.Rebind();
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

    protected void GvPromotionQuestion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            UserControlRank ucRankEdit = (UserControlRank)e.Item.FindControl("ucRankEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucRankEdit != null) ucRankEdit.SelectedRank = drv["FLDRANKID"].ToString();
        }
    }
      

    protected void GvPromotionQuestion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvPromotionQuestion.CurrentPageIndex + 1;
        BindData();
    }
}
