using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersCompetenceConduct : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in GvcompetenceConduct.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(GvcompetenceConduct.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Registers/Registerscompetenceconduct.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('GvcompetenceConduct')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar1.AddImageButton("../Registers/Registerscompetenceconduct.aspx", "Find", "search.png", "FIND");
            MenucompetenceConduct.AccessRights = this.ViewState;
            MenucompetenceConduct.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            BindData();
            SetPageNavigator();
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

        string[] alColumns = { "FLDRANK", "FLDCOMPETENCEQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Category", "Evaluation Item", "Order Sequence" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAppraisalCompetenceQuestion.AppraisalCompetenceQuestionSearch(
                                                                                          General.GetNullableInteger(ddlcompetenceconduct.SelectedHard)
                                                                                          , sortexpression
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , General.ShowRecords(null)
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );

        General.SetPrintOptions("GvcompetenceConduct", "competence Conduct", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GvcompetenceConduct.DataSource = ds;
            GvcompetenceConduct.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, GvcompetenceConduct);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void MenucompetenceConduct_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                GvcompetenceConduct.EditIndex = -1;
                GvcompetenceConduct.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

        string[] alColumns = { "FLDRANK", "FLDCOMPETENCEQUESTION", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Category", "Evaluation Item", "Order Sequence" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersAppraisalCompetenceQuestion.AppraisalCompetenceQuestionSearch(
                                                                                          General.GetNullableInteger(ddlcompetenceconduct.SelectedHard)
                                                                                         , sortexpression
                                                                                         , sortdirection
                                                                                         , (int)ViewState["PAGENUMBER"]
                                                                                         , General.ShowRecords(null)
                                                                                         , ref iRowCount
                                                                                         , ref iTotalPageCount
                                                                                      );

        Response.AddHeader("Content-Disposition", "attachment; filename=competence.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>competence Conduct </h3></td>");
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        GvcompetenceConduct.EditIndex = -1;
        GvcompetenceConduct.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        GvcompetenceConduct.SelectedIndex = -1;
        GvcompetenceConduct.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    protected void GvcompetenceConduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidcompetenceConduct(((UserControlHard)_gridView.FooterRow.FindControl("ddlcompetenceconductinsert")).SelectedHard.ToUpper(),
                    ((TextBox)_gridView.FooterRow.FindControl("txtevaluationiteminsert")).Text,
                    ((UserControlNumber)_gridView.FooterRow.FindControl("ucordersequenceInsert")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertcompetenceConduct(
                    int.Parse(((UserControlHard)_gridView.FooterRow.FindControl("ddlcompetenceconductinsert")).SelectedHard.ToString()),
                    ((TextBox)_gridView.FooterRow.FindControl("txtevaluationiteminsert")).Text,
                    int.Parse(((UserControlNumber)_gridView.FooterRow.FindControl("ucordersequenceInsert")).Text));


                BindData();
                SetPageNavigator();
                ((UserControlHard)_gridView.FooterRow.FindControl("ddlcompetenceconductinsert")).Focus();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deletecompetenceconduct(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblquestionid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidcompetenceConduct(string category, string evaluationitem, string ordersequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        GridView _gridView = GvcompetenceConduct;

        if (General.GetNullableInteger(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (evaluationitem.Trim().Equals(""))
            ucError.ErrorMessage = "Evaluation Item is required.";

        if (General.GetNullableInteger(ordersequence) == null)
            ucError.ErrorMessage = "Order Sequence is required.";

        return (!ucError.IsError);
    }

    private void InsertcompetenceConduct(int? category, string evaluationitem, int? ordersequence)
    {
        PhoenixRegistersAppraisalCompetenceQuestion.InsertAppraisalCompetenceQuestion(
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , category
                                                                         , evaluationitem
                                                                         , ordersequence
                                                                  );
    }

    private void UpdatecompetenceConduct(int questionid, int? category, string evaluationitem, int? ordersequence)
    {
        PhoenixRegistersAppraisalCompetenceQuestion.UpdateAppraisalCompetenceQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                        , category
                                                                        , evaluationitem
                                                                        , ordersequence
                                                                 );
    }

    private void deletecompetenceconduct(int questionid)
    {
        PhoenixRegistersAppraisalCompetenceQuestion.DeleteAppraisalCompetenceQuestion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , questionid
                                                                 );
    }

    protected void GvcompetenceConduct_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtevaluationitemedit")).Focus();
        SetPageNavigator();
    }

    protected void GvcompetenceConduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void GvcompetenceConduct_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidcompetenceConduct(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlcompetenceconductedit")).SelectedHard.ToUpper(),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtevaluationitemedit")).Text,
                    ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucordersequenceEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdatecompetenceConduct(
                             int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblquestionidedit")).Text)
                             , int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlcompetenceconductedit")).SelectedHard.ToUpper())
                             , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtevaluationitemedit")).Text
                             , int.Parse(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("ucordersequenceEdit")).Text)
                             );
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void GvcompetenceConduct_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void GvcompetenceConduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ddlcompetenceconductedit");
            DataRowView drvHard = (DataRowView)e.Row.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDRANK"].ToString();
        }
    }

    protected void GvcompetenceConduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        GvcompetenceConduct.EditIndex = -1;
        GvcompetenceConduct.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
