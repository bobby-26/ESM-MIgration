using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Registers_RegistersDebriefingQuestion : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingQuestion.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDebriefingQuestion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingQuestion.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingQuestion.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "RESET");        
        MenuRegistersDebriefingQuestion.AccessRights = this.ViewState;
        MenuRegistersDebriefingQuestion.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
           
            BindChategoryList();
            gvDebriefingQuestion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void RegistersgvDebriefingQuestion_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDebriefingQuestion.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ViewState["PAGENUMBER"] = 1;
            ClearFilter();
            BindData();
            gvDebriefingQuestion.Rebind();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME","FLDQUESTIONNAME","FLDRANK","FLDSORTORDER",
            "FLDREQUIREREMARK","FLDACTIVEYN" };
        string[] alCaptions = { "Category", "Question", "Rank Applicable", "Sort Order", "Require Remark ?", "Active ?" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersDebriefingQuestion.Questionsearch(General.GetNullableInteger(ddlcategory.SelectedValue)
                                                                           , txtQuestionName.Text
                                                                           , General.GetNullableString(ucRank.SelectedRank)
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , gvDebriefingQuestion.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                     );
        General.SetPrintOptions("gvDebriefingQuestion", "De-Briefing Question", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDebriefingQuestion.DataSource = ds;
            gvDebriefingQuestion.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDebriefingQuestion.DataSource = "";
        }
    }
    private void UpdateQuestion(int Questionid, int? categoryid, string questionname, string ranklist, int sortorder, int remark, int active)
    {
        PhoenixRegistersDebriefingQuestion.UpdateQuestion(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Questionid, categoryid, questionname, ranklist, sortorder, remark, active);

    }
    private bool IsValidQuestion(string Categoryid,string QuestionName, string sortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Categoryid) == null)
            ucError.ErrorMessage = "Category is required.";
        
        if (QuestionName.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";
        if (sortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";


        return (!ucError.IsError);
    }
    
    private void DeleteQuestionId(int QuestionId)
    {
        PhoenixRegistersDebriefingQuestion.DeleteQuestion(1, QuestionId);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCATEGORYNAME","FLDQUESTIONNAME","FLDRANK","FLDSORTORDER",
            "FLDREQUIREREMARK","FLDACTIVEYN", };
        string[] alCaptions = { "Category", "Question", "Rank Applicable", "Sort Order", "Require Remark ?", "Active ?" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersDebriefingQuestion.Questionsearch(General.GetNullableInteger(ddlcategory.SelectedValue), txtQuestionName.Text, General.GetNullableString(ucRank.SelectedRank), sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
             gvDebriefingQuestion.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=>De-Briefing Question.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>De-Briefing Question</h3></td>");
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
   
    private void ClearFilter()
    {
        ucRank.SelectedRank = "";
        txtQuestionName.Text = "";
        ddlcategory.SelectedValue = "";
    }

    public void BindChategoryList()
    {
        DataSet ds = PhoenixRegistersDebriefingQuestion.CategoryList();
        ddlcategory.DataSource = ds;
        ddlcategory.DataTextField = "FLDCATEGORYNAME";
        ddlcategory.DataValueField = "FLDCATEGORYID";
        ddlcategory.DataBind();
       // ddlcategory.Items.Insert(0, new ListItem("--Select--", ""));
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvDebriefingQuestion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
                string RnList = "";
                string Ranklist = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RnList += li.Value + ",";
                    }
                }

                if (RnList != "")
                {
                    Ranklist = "," + RnList;
                }
                else
                {
                    ucError.ErrorMessage = "Applicable Rank Is Required";
                }
                if (!IsValidQuestion(General.GetNullableString(ddlcategory.SelectedValue),
                    ((RadTextBox)e.Item.FindControl("txtQuestionNameAdd")).Text.Trim()
                         , (((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text)))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersDebriefingQuestion.InsertQuestion(1
                     , General.GetNullableInteger(ddlcategory.SelectedValue)
                     , ((RadTextBox)e.Item.FindControl("txtQuestionNameAdd")).Text.Trim()
                     , General.GetNullableString(Ranklist)
                     , Int32.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text)
                     , (((RadCheckBox)e.Item.FindControl("chkRequireRemarkrAdd")).Checked==true) ? 1 : 0
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0);
                BindData();
                gvDebriefingQuestion.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                chk.Visible = true;
                string RList = "";
                string RankList = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }
                else
                {
                    ucError.ErrorMessage = "RankList Is Required";
                }
                if (!IsValidQuestion((((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue),
                          ((RadTextBox)e.Item.FindControl("txtQuestionNameEdit")).Text.Trim()
                       , (((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text)
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateQuestion(
                      Int32.Parse(((RadTextBox)e.Item.FindControl("txtQuestionIdEdit")).Text)
                          , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue)
                          , ((RadTextBox)e.Item.FindControl("txtQuestionNameEdit")).Text.Trim()
                          , General.GetNullableString(RankList)
                          , Int32.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text)
                          , (((RadCheckBox)e.Item.FindControl("ChkRequireRemarkEdit")).Checked==true) ? 1 : 0
                          , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true) ? 1 : 0);
                BindData();
                gvDebriefingQuestion.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteQuestionId(Int32.Parse(((RadLabel)e.Item.FindControl("lblQuestionId")).Text));
                BindData();
                gvDebriefingQuestion.Rebind();
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

    protected void gvDebriefingQuestion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebriefingQuestion.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDebriefingQuestion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox ddl1 = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            if (ddl1 != null)
                ddl1.SelectedValue = drv["FLDCATEGORYID"].ToString();

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersRank.ListRank();
                chkUserGroupEdit.DataBindings.DataTextField = "FLDRANKNAME";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDRANKID";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                foreach (ButtonListItem li in chk.Items)
                {
                    string[] slist = drv["FLDRANKAPPLICABLE"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }

            }
            RadLabel lblRankApplicable = (RadLabel)e.Item.FindControl("lblRankApplicable");
            UserControlToolTip ucRankApplicable = (UserControlToolTip)e.Item.FindControl("ucRankApplicable");
            if (ucRankApplicable != null)
            {
                ucRankApplicable.Position = ToolTipPosition.TopCenter;
                ucRankApplicable.TargetControlId = lblRankApplicable.ClientID;
            }
        }

            LinkButton cb1 = (LinkButton)e.Item.FindControl("cmdselect");
            if (cb1 != null)
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblQuestionId");
                if (l
                    != null)
                {
                    cb1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDebriefingQuestionOption.aspx?Questionid=" + l.Text + "');return false;");
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

            RadCheckBoxList chkRankAdd = (RadCheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
            if (chkRankAdd != null)
            {
                chkRankAdd.DataSource = PhoenixRegistersRank.ListRank();
                chkRankAdd.DataBindings.DataTextField = "FLDRANKNAME";
                chkRankAdd.DataBindings.DataValueField = "FLDRANKID";
                chkRankAdd.DataBind();
                chkRankAdd.Enabled = true;
            }
        }
    }

    protected void gvDebriefingQuestion_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
