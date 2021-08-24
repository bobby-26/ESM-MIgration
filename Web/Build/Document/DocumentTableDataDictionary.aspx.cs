using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Document;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DocumentTableDataDictionary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                ViewState["PAGENUMBER"] = 1;
                gvTableSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        ds = SouthNests.Phoenix.Document.PhoenixDataDictionary.TableDescriptionSearch((int)ViewState["PAGENUMBER"],
           gvTableSearch.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvTableSearch.DataSource = ds;
        gvTableSearch.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
   
    private bool IsValidData(string ColumnDescription)
    {
        if (ColumnDescription.ToString().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);

    }

    protected void Rebind()
    {
        gvTableSearch.SelectedIndexes.Clear();
        gvTableSearch.EditIndexes.Clear();
        gvTableSearch.DataSource = null;
        gvTableSearch.Rebind();
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

    protected void gvTableSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidData(((RadTextBox)e.Item.FindControl("txtTableDescriptionEdit")).Text))
                {
                    SouthNests.Phoenix.Document.PhoenixDataDictionary.TableDescriptionUpdate(
                                    ((LinkButton)e.Item.FindControl("lnkTableName")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtTableDescriptionEdit")).Text
                                     );
                    /*SouthNests.Phoenix.Document.PhoenixDataDictionary.ColumnDescriptionUserNameUpdate(
                               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                               ((Label)_gridView.Rows[nCurrentRow].FindControl("FlblTableName")).Text,
                               ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkColumnName")).Text,
                                0);*/
                }

                Rebind();

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

    protected void gvTableSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null)
            //{
            //    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            //}

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            }
        }
    }

    protected void gvTableSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTableSearch.CurrentPageIndex + 1;
        BindData();
    }
}
