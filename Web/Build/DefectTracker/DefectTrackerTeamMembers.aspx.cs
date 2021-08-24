using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerTeamMembers : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvTeamMember.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvTeamMember.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DefectTrackerTeamMember_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvTeamMember.EditIndex = -1;
                gvTeamMember.SelectedIndex = -1;
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTeamMember_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((TextBox)_gridView.FooterRow.FindControl("txtTeamMemberNameAdd")).Text),
                     (((TextBox)_gridView.FooterRow.FindControl("txtContactAdd")).Text),
                     (((TextBox)_gridView.FooterRow.FindControl("txtEmailAdd")).Text)
                     ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDefectTracker.InsertTeamMember((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                        (((TextBox)_gridView.FooterRow.FindControl("txtTeamMemberNameAdd")).Text),
                                                        (((TextBox)_gridView.FooterRow.FindControl("txtContactAdd")).Text),
                                                        (((TextBox)_gridView.FooterRow.FindControl("txtEmailAdd")).Text)

                                                      );

                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDefectTracker.DeleteTeamMember((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTeamMemberCode")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool checkvalue(string name, string contact, string emailid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((name == null) || (name == "") || (name == "Dummy"))
            ucError.ErrorMessage = "Name is required.";

        if ((contact == null) || (contact == "") || (contact == "Dummy"))
            ucError.ErrorMessage = "Contact is required.";

        if ((emailid == null) || (emailid == "") || (emailid == "Dummy"))
            ucError.ErrorMessage = "Email is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDefectTracker.TeamMemberList(General.GetNullableGuid(""));

        if (dt.Rows.Count > 0)
        {
            gvTeamMember.DataSource = dt;
            gvTeamMember.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvTeamMember);
        }
     }

    protected void gvTeamMember_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
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
            throw ex;
        }
    }

    protected void gvTeamMember_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvTeamMember_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTeamMember, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void gvTeamMember_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTeamMember_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!checkvalue((((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTeamMemberNameEdit")).Text),
                     (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContactEdit")).Text),
                     (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmailEdit")).Text)
                     ))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDefectTracker.UpdateTeamMember(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTeamMemberIDEdit")).Text),
                                                   General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTeamMemberNameEdit")).Text),
                                                   General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContactEdit")).Text),
                                                   General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmailEdit")).Text)
                                                  );

            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTeamMember_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvTeamMember_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvTeamMember.EditIndex = -1;
        gvTeamMember.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvTeamMember.SelectedIndex = -1;
        gvTeamMember.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

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
