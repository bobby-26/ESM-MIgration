using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

public partial class Registers_RegistersSignoffFeedBackQuestionOptions : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSignoffFeedBackQuestionOptions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFBQOptions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuRegistersFBQOptions.AccessRights = this.ViewState;
            MenuRegistersFBQOptions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["QuestionId"] = Request.QueryString["QuestionId"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFBQOptions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        gvFBQOptions.Rebind();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDQUESTIONNAME", "FLDOPTIONNAME" };
        string[] alCaptions = { "Question", "Option" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersSignoffFeedBackQuestionOptions.FeedBackQuestionOptionsSearch(
            General.GetNullableInteger(ViewState["QuestionId"].ToString())
            ,sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvFBQOptions.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=SignOffFeedBackQuestionOptions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sign Off FeedBack Question Options</h3></td>");
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
                Response.Write(Regex.Replace(dr[alColumns[i]].ToString(), @"[^\u0000-\u007F]", string.Empty));
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuRegistersFBQOptions_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDQUESTIONNAME", "FLDOPTIONNAME" };
        string[] alCaptions = { "Question", "Option" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersSignoffFeedBackQuestionOptions.FeedBackQuestionOptionsSearch(
            General.GetNullableInteger(ViewState["QuestionId"].ToString())
            ,sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvFBQOptions.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvFBQOptions", "Sign Off FeedBack Question Options", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtQuestion.Text = ds.Tables[0].Rows[0]["FLDQUESTIONNAME"].ToString();
            gvFBQOptions.DataSource = ds;
            gvFBQOptions.VirtualItemCount = iRowCount;
        }
        else
        {
            gvFBQOptions.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {        
        BindData();
        gvFBQOptions.Rebind();
    }
    

    private void InsertOption(int Questionid, string OptionName,string OrderNo)
    {
        PhoenixRegistersSignoffFeedBackQuestionOptions.InsertFeedBackQuestionOptions(
            Questionid
            , OptionName
            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
            ,General.GetNullableInteger(OrderNo)
            );
    }

    private void UpdateOption(int Questionid, int OptionId, string OptionName, string OrderNo)
    {
        PhoenixRegistersSignoffFeedBackQuestionOptions.UpdateFeedBackQuestionOptions(
           Questionid
           , OptionId
           , OptionName
           , PhoenixSecurityContext.CurrentSecurityContext.UserCode
           ,General.GetNullableInteger(OrderNo));

        //ucStatus.Text = "Flag information updated";        
    }

    private bool IsValidOption(string OptionName,string OrderNo)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (OptionName == string.Empty)
            ucError.ErrorMessage = "Option Name is required.";

        if (OrderNo == string.Empty)
            ucError.ErrorMessage = "Order No is required.";
        return (!ucError.IsError);
    }

    private void DeleteOption(int Questionid, int OptionId)
    {
        PhoenixRegistersSignoffFeedBackQuestionOptions.DeleteFeedBackQuestionOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Questionid, OptionId);
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

    protected void gvFBQOptions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOption(((RadTextBox)e.Item.FindControl("txtOptionAdd")).Text, ((UserControlMaskNumber)e.Item.FindControl("txtOrderNoAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }               
                InsertOption(Int32.Parse(ViewState["QuestionId"].ToString())
                                            , ((RadTextBox)e.Item.FindControl("txtOptionAdd")).Text
                                            , ((UserControlMaskNumber)e.Item.FindControl("txtOrderNoAdd")).Text
                   );
                BindData();
                gvFBQOptions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOption(Int32.Parse(Request.QueryString["QuestionId"])
                    , Int32.Parse(((RadLabel)e.Item.FindControl("lblOptionId")).Text));
                BindData();
                gvFBQOptions.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Session["FLAGID"] = ((RadLabel)e.Item.FindControl("lblOptionId")).Text;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string sOrderNo = ((UserControlMaskNumber)e.Item.FindControl("txtOrderNoEdit")).Text;
                if (!IsValidOption(((RadTextBox)e.Item.FindControl("txtOptionEdit")).Text, sOrderNo))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateOption(
                         Int32.Parse(ViewState["QuestionId"].ToString())
                         , Int32.Parse(((RadLabel)e.Item.FindControl("lblOptionIdEdit")).Text)
                         , ((RadTextBox)e.Item.FindControl("txtOptionEdit")).Text
                         , ((UserControlMaskNumber)e.Item.FindControl("txtOrderNoEdit")).Text
                         );
                BindData();
                gvFBQOptions.Rebind();
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

    protected void gvFBQOptions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFBQOptions.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvFBQOptions_ItemDataBound(object sender, GridItemEventArgs e)
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
}
