using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaScoreCardTemplate : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPreSeaScoreCard.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvPreSeaScoreCard.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaScoreCardTemplate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPreSeaScoreCard')", "Print Grid", "icon_print.png", "PRINT");
            MenuPreSeaQuick.AccessRights = this.ViewState;
            MenuPreSeaQuick.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ddlScoreCard.DataSource = PhoenixPreSeaTemplate.ListScoreCardTemplate(null);
                ddlScoreCard.DataBind();
                ddlScoreCard.Items.Insert(0, new ListItem("--Select--", "Dummy"));

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "Section", "Serial No", "Evaluation Item", "Academic Subject", "Active Yes/No" };
        string[] alColumns = { "FLDSECTIONNAME", "FLDFIELDSERIALNO", "FLDFIELDDESCRIPION", "FLDSUBJECTNAME", "FLDACTIVE" };

        ds = PhoenixPreSeaScoreCardTemplate.ListPreSeaScoreCardFields(General.GetNullableInteger(ddlScoreCard.SelectedValue));

        General.ShowExcel("Pre-Sea Scorecard Template", ds.Tables[0], alColumns, alCaptions, null, "");

    }

    protected void PreSeaQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaScoreCard.EditIndex = -1;
                gvPreSeaScoreCard.SelectedIndex = -1;
                BindData();
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

    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();

            string[] alCaptions = { "Section", "Serial No", "Evaluation Item", "Academic Subject", "Active Yes/No" };
            string[] alColumns = { "FLDSECTIONNAME", "FLDFIELDSERIALNO", "FLDFIELDDESCRIPION", "FLDSUBJECTNAME", "FLDACTIVE" };

            ds = PhoenixPreSeaScoreCardTemplate.ListPreSeaScoreCardFields(General.GetNullableInteger(ddlScoreCard.SelectedValue));

            General.SetPrintOptions("gvPreSeaScoreCard", "PreSea", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvPreSeaScoreCard.DataSource = ds;
                gvPreSeaScoreCard.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvPreSeaScoreCard);
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
    }

    protected void gvPreSeaScoreCard_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvPreSeaScoreCard_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvPreSeaScoreCard.EditIndex = -1;
        gvPreSeaScoreCard.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPreSeaScoreCard_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(e.CommandName.Trim()))
            {
                if (e.CommandName.ToUpper().Equals("SORT"))
                    return;
                GridView _gridView = (GridView)sender;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidScoreCardField(((DropDownList)_gridView.FooterRow.FindControl("ddlSectionAdd")).SelectedValue
                                                , ((TextBox)_gridView.FooterRow.FindControl("txtSerialNoAdd")).Text
                                                , ((TextBox)_gridView.FooterRow.FindControl("txtFieldDescAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixPreSeaScoreCardTemplate.InsertPreSeaScoreCardField(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , int.Parse(ddlScoreCard.SelectedValue)
                                                                            , General.GetNullableByte(((DropDownList)_gridView.FooterRow.FindControl("ddlSectionAdd")).SelectedValue)
                                                                            , ((TextBox)_gridView.FooterRow.FindControl("txtSerialNoAdd")).Text
                                                                            , ((TextBox)_gridView.FooterRow.FindControl("txtFieldDescAdd")).Text
                                                                            , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtsubjectIdAdd")).Text)
                                                                            , General.GetNullableByte((((CheckBox)_gridView.FooterRow.FindControl("chkActiveAdd")).Checked ? "1" : "0")));



                    BindData();
                    ((DropDownList)_gridView.FooterRow.FindControl("ddlSectionAdd")).Focus();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaScoreCard_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPreSeaScoreCard, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvPreSeaScoreCard_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtSerialNoEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaScoreCard_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidScoreCardField(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSectionIdEdit")).Text
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSerialNoEdit")).Text
                            , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFieldDescEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPreSeaScoreCardTemplate.UpdatePreSeaScoreCardField(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFieldIdEdit")).Text)
                                                                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSerialNoEdit")).Text
                                                                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFieldDescEdit")).Text
                                                                    , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtsubjectIdEdit")).Text)
                                                                    , General.GetNullableByte((((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveEdit")).Checked ? "1" : "0")));


            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvPreSeaScoreCard_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


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
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                }

                CheckBox chkEdit = (CheckBox)e.Row.FindControl("chkActiveEdit");
                if (chkEdit != null)
                {
                    chkEdit.Checked = drv["FLDACTIVEYN"].ToString().Equals("1");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private bool IsValidScoreCardField(string section, string sNO, string evaluationItem)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(ddlScoreCard.SelectedValue).HasValue)
            ucError.ErrorMessage = "Score Card Template is required.";
        if (!General.GetNullableInteger(section).HasValue)
            ucError.ErrorMessage = "Section for the evaluation criteria is required.";
        if (sNO.Trim() == "")
            ucError.ErrorMessage = "Serial number is required.";
        if (evaluationItem.Trim() == "")
            ucError.ErrorMessage = "Evaluation Criteria is required.";

        return (!ucError.IsError);
    }

    protected void ddlScoreCard_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
