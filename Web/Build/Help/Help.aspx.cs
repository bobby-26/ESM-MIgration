using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Help;

public partial class Help : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //SessionUtil.PageAccessRights(this.ViewState);           
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("View", "VIEW");
                toolbar.AddButton("Edit", "EDIT");
                toolbar.AddButton("Draft", "DRAFT");
                MenuHelpMode.AccessRights = this.ViewState;
                MenuHelpMode.MenuList = toolbar.Show();
                MenuHelpMode.SelectedMenuIndex = 0;

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddButton("Save", "SAVE");
                MenuHelp.AccessRights = this.ViewState;
                MenuHelp.MenuList = toolbargrid.Show();

                toolbargrid = new PhoenixToolbar();
                toolbargrid.AddButton("Save", "SAVE");
                toolbargrid.AddButton("Confirm", "CONFIRM");
                MenuDraft.AccessRights = this.ViewState;
                MenuDraft.MenuList = toolbargrid.Show();

                if (Request.QueryString["pid"] != null)
                    ViewState["PAGEID"] = Request.QueryString["pid"];
                else if (Request.QueryString["page"].IndexOf("?") >= 0)
                    ViewState["PAGEID"] = PhoenixHelp.InsertHelp(Request.QueryString["page"].Substring(0, Request.QueryString["page"].IndexOf("?")), null);
                else
                    ViewState["PAGEID"] = PhoenixHelp.InsertHelp(Request.QueryString["page"], null);
                SetPageInformation();
            }
            BindData();
            BindDataLink();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHelpMode_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("VIEW"))
            {
                divView.Visible = true;
                divEdit.Visible = false;
                divDraft.Visible = false;
                SetPageInformation();
            }
            else if (dce.CommandName.ToUpper().Equals("EDIT"))
            {
                divView.Visible = false;
                divEdit.Visible = true;
                divDraft.Visible = false;
                SetPageInformation();
            }
            else if (dce.CommandName.ToUpper().Equals("DRAFT"))
            {
                divView.Visible = false;
                divEdit.Visible = false;
                divDraft.Visible = true;
            }
            BindData();
            BindDataLink();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHelp_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixHelp.InsertHelpSummary(int.Parse(ViewState["PAGEID"].ToString()), txtSummary.Text, txtUsage.Content);
                ucStatus.Text = "Help Saved";
                SetPageInformation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDraft_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixHelp.InsertHelpDraftSummary(int.Parse(ViewState["PAGEID"].ToString()), txtDraftSummary.Text, txtDraftUsage.Content);
                ucStatus.Text = "Help Saved";
                SetPageInformation();
            }
            else if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixHelp.ConfirmHelpDraft(int.Parse(ViewState["PAGEID"].ToString()));
                ucStatus.Text = "Help Draft Confirmed.";
                SetPageInformation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetPageInformation()
    {
        try
        {
            DataTable dt = PhoenixHelp.EditHelpSummary(int.Parse(ViewState["PAGEID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtSummary.Text = dt.Rows[0]["FLDSUMMARY"].ToString();
                txtUsage.Content = dt.Rows[0]["FLDUSAGE"].ToString();
                divSummary.InnerHtml = dt.Rows[0]["FLDSUMMARY"].ToString();
                divUsage.InnerHtml = dt.Rows[0]["FLDUSAGE"].ToString();
                txtDraftSummary.Text = dt.Rows[0]["FLDDRAFTSUMMARY"].ToString();
                txtDraftUsage.Content = dt.Rows[0]["FLDDRAFTUSAGE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {

        try
        {
            DataTable dt = new DataTable();
            if (divView.Visible || divEdit.Visible)
                dt = PhoenixHelp.ListHelpField(int.Parse(ViewState["PAGEID"].ToString()));
            else if (divDraft.Visible)
                dt = PhoenixHelp.ListHelpDraftField(int.Parse(ViewState["PAGEID"].ToString()));
            GridView gv = null;
            if (divView.Visible)
            {
                gv = gvHelpView;
            }
            else if (divEdit.Visible)
            {
                gv = gvHelp;
            }
            else if (divDraft.Visible)
            {
                gv = gvHelpDraft;
            }

            if (dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gv);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDataLink()
    {

        try
        {
            DataTable dt = new DataTable();
            if (divView.Visible || divEdit.Visible)
                dt = PhoenixHelp.ListHelpLink(int.Parse(ViewState["PAGEID"].ToString()));
            else if (divDraft.Visible)
                dt = PhoenixHelp.ListHelpDraftLink(int.Parse(ViewState["PAGEID"].ToString()));
            GridView gv = null;

            if (divView.Visible)
            {
                gv = gvLink;
            }
            else if (divEdit.Visible)
            {
                gv = gvLinkEdit;
            }
            else if (divDraft.Visible)
            {
                gv = gvLinkDraft;
            }

            if (dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gv);
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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHelp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            int id = General.GetNullableInteger(_gridView.DataKeys[nCurrentRow].Value.ToString()).Value;
            if (divEdit.Visible)
                PhoenixHelp.DeleteHelpFieldList(id);
            else if (divDraft.Visible)
                PhoenixHelp.DeleteHelpDraftFieldList(id);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvHelp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
    }
    protected void gvHelp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string name = ((TextBox)_gridView.FooterRow.FindControl("txtFiledNameAdd")).Text;
                string desc = ((TextBox)_gridView.FooterRow.FindControl("txtFieldDescAdd")).Text;
                string valuedesc = ((TextBox)_gridView.FooterRow.FindControl("txtValueDescAdd")).Text;
                if (!IsValidFieldList(name))
                {
                    ucError.Visible = true;
                    return;
                }
                if (divEdit.Visible)
                    PhoenixHelp.InsertHelpFieldList(int.Parse(ViewState["PAGEID"].ToString()), name, desc, valuedesc);
                else if (divDraft.Visible)
                    PhoenixHelp.InsertHelpDraftFieldList(int.Parse(ViewState["PAGEID"].ToString()), name, desc, valuedesc);
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHelp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvHelp_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtFiledNameEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHelp_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            int id = General.GetNullableInteger(_gridView.DataKeys[nCurrentRow].Value.ToString()).Value;
            string name = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFiledNameEdit")).Text;
            string desc = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFieldDescEdit")).Text;
            string valuedesc = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueDescEdit")).Text;
            if (!IsValidFieldList(name))
            {
                ucError.Visible = true;
                return;
            }
            _gridView.EditIndex = -1;
            if (divEdit.Visible)
                PhoenixHelp.UpdateHelpFieldList(id, name, desc, valuedesc);
            else if (divDraft.Visible)
                PhoenixHelp.UpdateHelpDraftFieldList(id, name, desc, valuedesc);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLink_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string id = ((DropDownList)_gridView.FooterRow.FindControl("ddlPageAdd")).SelectedValue;

                if (divEdit.Visible)
                {
                    if (General.GetNullableInteger(id).HasValue)
                        PhoenixHelp.InsertHelpLink(int.Parse(ViewState["PAGEID"].ToString()), int.Parse(id));
                    else
                    {
                        string pageid = PhoenixHelp.InsertHelp(null, "New Help Link");
                        PhoenixHelp.InsertHelpLink(int.Parse(ViewState["PAGEID"].ToString()), int.Parse(pageid));
                    }
                }
                else if (divDraft.Visible)
                {
                    if (General.GetNullableInteger(id).HasValue)
                        PhoenixHelp.InsertHelpDraftLink(int.Parse(ViewState["PAGEID"].ToString()), int.Parse(id));
                    else
                    {
                        string pageid = PhoenixHelp.InsertHelp(null, "New Help Link");
                        PhoenixHelp.InsertHelpDraftLink(int.Parse(ViewState["PAGEID"].ToString()), int.Parse(pageid));
                    }
                }
                BindDataLink();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLink_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlPageEdit");
            if (ddl != null)
            {
                PopulateHelpList(ddl);
                ddl.SelectedValue = drv["FLDPAGEID"].ToString();
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlPageAdd");
            PopulateHelpList(ddl);
        }
    }
    protected void gvLink_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataLink();
    }
    protected void gvLink_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindDataLink();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlPageEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLink_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            int id = General.GetNullableInteger(_gridView.DataKeys[nCurrentRow].Value.ToString()).Value;
            string desc = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescription")).Text;
            string pageid = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlPageEdit")).SelectedValue;
            if (!IsValidHelpLink(desc))
            {
                ucError.Visible = true;
                return;
            }
            _gridView.EditIndex = -1;
            if (divEdit.Visible)
                PhoenixHelp.UpdateHelpLink(id, int.Parse(pageid), desc);
            else if (divDraft.Visible)
                PhoenixHelp.UpdateHelpDraftLink(id, int.Parse(pageid), desc);
            BindDataLink();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLink_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        string id = _gridView.DataKeys[e.NewSelectedIndex].Value.ToString();
        Response.Redirect("Help.aspx?pid=" + id, false);
    }
    private bool IsValidFieldList(string fieldname)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (fieldname.Trim() == string.Empty)
        {
            ucError.ErrorMessage = "Filed Name is mandatory.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidHelpLink(string desc)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (desc.Trim() == string.Empty)
        {
            ucError.ErrorMessage = "Description is mandatory.";
        }
        return (!ucError.IsError);
    }
    private void PopulateHelpList(DropDownList ddlPage)
    {
        ddlPage.DataSource = PhoenixHelp.ListHelp(null, null);
        ddlPage.DataBind();
        ddlPage.Items.Insert(0, new ListItem("--Create New Help Link--", ""));
    }
}
