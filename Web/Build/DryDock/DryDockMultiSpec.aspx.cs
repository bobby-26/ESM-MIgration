using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;

public partial class DryDockMultiSpec : PhoenixBasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                
                BindCatalogList(0);
                ddlCatalogList.SelectedValue = "1";
            }
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCatalogList(int type)
    {
        try
        {
            DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(type);
            ddlCatalogList.DataTextField = "FLDNAME";
            ddlCatalogList.DataValueField = "FLDMULTISPECID";
            ddlCatalogList.DataSource = ds;
            ddlCatalogList.DataBind();
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
            if (ddlCatalogList.SelectedValue.ToString() != string.Empty)
            {
                DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(int.Parse(ddlCatalogList.SelectedValue));
                gvAdditionalSpec.DataSource = ds;
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvAdditionalSpec_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidMultiSpec(((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text,
    //                ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            InsertMultiSpec(
    //                ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text.Trim(),
    //                ((TextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text.Trim(),
    //                int.Parse(ddlCatalogList.SelectedValue),
    //                ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text.Trim()
    //            );
    //            ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Focus();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            if (!IsValidMultiSpec(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            UpdateMultiSpec(

    //                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text.Trim(),
    //                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text.Trim(),
    //                 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text.Trim(),
    //                 General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDtkeyEdit")).Text)
    //             );
    //            _gridView.EditIndex = -1;
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            DeleteMultiSpec(
    //                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDtkey")).Text));
    //        }
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void gvAdditionalSpec_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }

    protected void gvAdditionalSpec_DataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }

    private void InsertMultiSpec(string code, string name, int type, string description)
    {

        PhoenixDryDockMultiSpec.InsertDryDockMultiSpec(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            code, name, type, description);
    }

    private void UpdateMultiSpec(string code, string name, string description, Guid? dtkey)
    {
        PhoenixDryDockMultiSpec.UpdateDryDockMultiSpec(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            code, name, description, dtkey);
    }

    private void DeleteMultiSpec(Guid? dtkey)
    {
        PhoenixDryDockMultiSpec.DeleteDryDockMultiSpec(PhoenixSecurityContext.CurrentSecurityContext.UserCode, dtkey);
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

    protected void ddlCatalogList_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvAdditionalSpec.Rebind();
    }

    private bool IsValidMultiSpec(string Code, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    protected void gvAdditionalSpec_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtShortCodeEdit")).Focus();
    }

    //protected void gvAdditionalSpec_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        if (!IsValidMultiSpec(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text,
    //                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        UpdateMultiSpec(
    //             ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text.Trim(),
    //             ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text.Trim(),
    //             ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text.Trim(),
    //             General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDtkeyEdit")).Text)
    //         );
    //        _gridView.EditIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    BindData();
    //}
    protected void gvAdditionalSpec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvAdditionalSpec_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAdditionalSpec, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvAdditionalSpec_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAdditionalSpec_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvAdditionalSpec_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidMultiSpec(((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertMultiSpec(
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text.Trim(),
                    int.Parse(ddlCatalogList.SelectedValue),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.Trim()
                );
                ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMultiSpec(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateMultiSpec(

                     ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.Trim(),
                     General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text)
                 );

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteMultiSpec(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDtkey")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;


            BindData();
            gvAdditionalSpec.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvAdditionalSpec_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (!IsValidMultiSpec(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateMultiSpec(
                 ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim(),
                 ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text.Trim(),
                 ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.Trim(),
                 General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text)
             );
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        gvAdditionalSpec.Rebind();
    }
}
