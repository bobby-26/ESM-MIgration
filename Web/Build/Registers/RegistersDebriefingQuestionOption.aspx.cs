using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class Registers_RegistersDebriefingQuestionOption : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingQuestionOption.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDebriefingOption')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuRegistersQuestionOptions.AccessRights = this.ViewState;
        MenuRegistersQuestionOptions.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            if (Request.QueryString["Questionid"] != null)
            {

                ViewState["Questionid"] = Request.QueryString["Questionid"].ToString();
                CompanyEdit(Int32.Parse(Request.QueryString["Questionid"].ToString()));
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvDebriefingOption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME","FLDQUESTIONNAME","FLDOPTIONNAME","FLDSORTORDER","FLDCONCERNREMARK","FLDACTIVEYN" };
        string[] alCaptions = { "Category", "Question", "Option", "Sort Order", "Concern Remark ?", "Active ?" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersDebriefingQuestionOption.QuestionOptionsearch(General.GetNullableInteger(Questionid.Text),
                                                                         General.GetNullableInteger(Categoryid.Text),
                                                                            sortexpression
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , gvDebriefingOption.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                     );
        General.SetPrintOptions("gvDebriefingOption", "De-Briefing Question Option", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDebriefingOption.DataSource = ds;
            gvDebriefingOption.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDebriefingOption.DataSource = "";
        }
    }
    protected void MenuRegistersQuestionOptions_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCATEGORYNAME", "FLDQUESTIONNAME", "FLDOPTIONNAME", "FLDSORTORDER", "FLDCONCERNREMARK", "FLDACTIVEYN" };
        string[] alCaptions = { "Category", "Question", "Option", "Sort Order", "Concern Remark ?", "Active ?" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersDebriefingQuestionOption.QuestionOptionsearch(General.GetNullableInteger(Questionid.Text),
                                                                         General.GetNullableInteger(Categoryid.Text), sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
             gvDebriefingOption.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=>De-Briefing Question Option.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>De-Briefing Question Option</h3></td>");
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
    private void CompanyEdit(int companyid)
    {
        DataSet ds = PhoenixRegistersDebriefingQuestion.QuestionEdit(companyid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblQuestionName.Text = dr["FLDQUESTIONNAME"].ToString();
            Questionid.Text = dr["FLDQUESTIONID"].ToString();
            Categoryid.Text = dr["FLDCATEGORYID"].ToString();
            txtcategory.Text= dr["FLDCATEGORYNAME"].ToString();
            txtcategory.ToolTip= dr["FLDCATEGORYNAME"].ToString();
        }
    }
    
    
    public void DeleteQuestionOption(int optionid)
    {
        PhoenixRegistersDebriefingQuestionOption.DeleteQuestionOption(1, optionid);
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    
    private void UpdateCategory(int Optionid,int? Questionid, string OptionName, int sortorder, int remark, int active)
    {
        PhoenixRegistersDebriefingQuestionOption.UpdateQuestionOption(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Questionid, Optionid, OptionName, sortorder, remark, active);

    }

    private bool IsValidQuestion(string option, string sortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (option.Trim().Equals(""))
            ucError.ErrorMessage = "Option is required.";

        if (sortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";
      
        return (!ucError.IsError);
    }

    
    protected void gvDebriefingOption_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidQuestion(
                    ((RadTextBox)e.Item.FindControl("txtOptionionNameAdd")).Text.Trim(),
                 (((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text
                )
                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersDebriefingQuestionOption.InsertQuestionOption(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , General.GetNullableInteger(Questionid.Text)
                      , General.GetNullableInteger(Categoryid.Text)
                     , ((RadTextBox)e.Item.FindControl("txtOptionionNameAdd")).Text.Trim()
                     , Int32.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text)
                     , (((RadCheckBox)e.Item.FindControl("chkRequireRemarkrAdd")).Checked==true) ? 1 : 0
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0);
                BindData();
                gvDebriefingOption.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidQuestion(
                 ((RadTextBox)e.Item.FindControl("txtOptionionNameEdit")).Text.Trim(),
                (((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text)
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateCategory(
                    Int32.Parse(((RadTextBox)e.Item.FindControl("txtOptionEdit")).Text)
                    , General.GetNullableInteger(Questionid.Text)
                    , ((RadTextBox)e.Item.FindControl("txtOptionionNameEdit")).Text.Trim()
                      , Int16.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text)
                        , (((RadCheckBox)e.Item.FindControl("ChkRequireRemarkEdit")).Checked==true) ? 1 : 0
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0);
                BindData();
                gvDebriefingOption.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteQuestionOption(Int32.Parse(((RadLabel)e.Item.FindControl("lblOptionId")).Text));
                BindData();
                gvDebriefingOption.Rebind();
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

    protected void gvDebriefingOption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebriefingOption.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDebriefingOption_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

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
        

    protected void gvDebriefingOption_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //ViewState["SORTEXPRESSION"] = e.SortExpression;

        //if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
        //    ViewState["SORTDIRECTION"] = 1;
        //else
        //    ViewState["SORTDIRECTION"] = 0;

        //BindData();
    }
}
