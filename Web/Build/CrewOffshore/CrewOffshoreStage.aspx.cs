using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreStage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreStage.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreStage')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreStage.AccessRights = this.ViewState;
            MenuOffshoreStage.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDSTAGE", "FLDORDER", "FLDSTATUSNAME" };
        string[] alCaptions = { "Short Code", "Stage", "Order","Plan Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreStage.SearchOffshoreStage(null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Stage.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Stage</h3></td>");
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

    protected void MenuOffshoreStage_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDSTAGE", "FLDORDER", "FLDSTATUSNAME" };
        string[] alCaptions = { "Short Code", "Stage", "Order", "Plan Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreStage.SearchOffshoreStage(null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreStage", "Stage", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreStage.DataSource = ds;
            gvOffshoreStage.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreStage);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvOffshoreStage_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtShortCodeEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreStage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvOffshoreStage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

            UserControlHard ucStatusEdit = (UserControlHard)e.Row.FindControl("ucStatusEdit");
            if (ucStatusEdit != null)
            {
                ucStatusEdit.HardTypeCode = "99";
                ucStatusEdit.bind();
            }
            Label lblstatus = (Label)e.Row.FindControl("lblStatusID");
            if (lblstatus != null && lblstatus.Text != "")
            {
                ucStatusEdit.SelectedHard = lblstatus.Text;
            }
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            UserControlHard ucStatusAdd = (UserControlHard)e.Row.FindControl("ucStatusAdd");
            if (ucStatusAdd != null)
            {
                ucStatusAdd.HardTypeCode = "99";
                ucStatusAdd.bind();
            }
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreStage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtStageAdd")).Text,
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucOrderAdd")).Text,
                    ((UserControlHard)_gridView.FooterRow.FindControl("ucStatusAdd")).SelectedHard)
                )
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreStage.InsertOffshoreStage(General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtStageAdd")).Text),
                    int.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucOrderAdd")).Text),
                    General.GetNullableInteger(((UserControlHard)_gridView.FooterRow.FindControl("ucStatusAdd")).SelectedHard));
                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int stageid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreStage.DeleteOffshoreStage(stageid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStageEdit")).Text,
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucOrderEdit")).Text,
                    ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                int stageid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreStage.UpdateOffshoreStage(stageid
                    ,General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text)
                    ,General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStageEdit")).Text)
                    ,int.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucOrderEdit")).Text)
                    , General.GetNullableInteger(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard));

                _gridView.EditIndex = -1;
                BindData();

            }
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreStage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreStage_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    private bool IsValidData(string shortcode, string stage, string order,string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(shortcode) == null)
            ucError.ErrorMessage = "Short Code is required.";

        if (General.GetNullableString(stage) == null)
            ucError.ErrorMessage = "Stage is required.";

        if (General.GetNullableInteger(order) == null)
            ucError.ErrorMessage = "Order is required.";

        if (General.GetNullableInteger(status) == null)
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOffshoreStage.SelectedIndex = -1;
        gvOffshoreStage.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreStage.SelectedIndex = -1;
        gvOffshoreStage.EditIndex = -1;
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
        gvOffshoreStage.SelectedIndex = -1;
        gvOffshoreStage.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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
        {
            return true;
        }

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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
