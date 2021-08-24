using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsGroupFunctionPermission : PhoenixBasePage
{
    public static Boolean f;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.Equals("USERGROUP"))
            Response.Redirect("OptionsUserGroups.aspx");

        else if (CommandName.Equals("APPLICATION"))
            Response.Redirect("OptionsApplication.aspx");

        else if (CommandName.Equals("MENU"))
            Response.Redirect("OptionsMenuAccess.aspx");

        else if (CommandName.Equals("FUNCTION"))
            Response.Redirect("OptionsFunctionAccess.aspx");

        else if (CommandName.Equals("PAGE"))
            Response.Redirect("OptionsFunctionPermission.aspx");
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
            DataSet ds1 = FunctionPermissions.GroupFunctionPermissionsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Request.QueryString["menucode"].ToString()), General.GetNullableInteger(Request.QueryString["groupcode"].ToString()));

            gvFunctionPermission.DataSource = ds1;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidFunctionPermission(string functioncode, string url)
    {
        ucError.HeaderMessage = "Please provide the following information";

        int? ug = General.GetNullableInteger(Request.QueryString["groupcode"].ToString());
        if (ug == null)
            ucError.ErrorMessage = "User Group is required";

        if (functioncode.Trim() == "")
            ucError.ErrorMessage = "Function Code is required";

        if (url.Trim() == "")
            ucError.ErrorMessage = "Url is required.";

        return (!ucError.IsError);
    }


    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{

    //}

    protected void gvFunctionPermission_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    private void menuSelection()
    {
        string menucode = Request.QueryString["menucode"].ToString();
        DataSet ds = new DataSet();
        ds = SessionUtil.MenuList(1);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (menucode == ds.Tables[0].Rows[i]["FLDMENUCODE"].ToString())
            {
                ViewState["SELECTEDMENUCODE"] = menucode;
                Filter.CurrentMenuCodeSelection = ds.Tables[0].Rows[i]["FLDMENUVALUE"].ToString();
                Filter.CurrentMenuCodeSelectionUrl = "~/" + ds.Tables[0].Rows[i]["FLDURL"].ToString();
            }
        }
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

    protected void Rebind()
    {
        gvFunctionPermission.SelectedIndexes.Clear();
        gvFunctionPermission.EditIndexes.Clear();
        gvFunctionPermission.DataSource = null;
        gvFunctionPermission.Rebind();
    }

    protected void ucUserGroup_TextChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvFunctionPermission_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string url = ((RadLabel)e.Item.FindControl("lblUrl")).Text;
                string command = ((RadLabel)e.Item.FindControl("lblCommand")).Text;
                FunctionPermissions.GroupFunctionPermissionsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Request.QueryString["groupcode"].ToString()), ((RadLabel)e.Item.FindControl("lblFunctionId")).Text, command, url);
                ucStatus.Text = "Information deleted";

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string functionid = ((RadLabel)e.Item.FindControl("lblFunctionIdEdit")).Text;
                string functioncode = ((RadTextBox)e.Item.FindControl("txtFunctionCodeEdit")).Text;
                short view = short.Parse(((RadCheckBox)e.Item.FindControl("chkViewYNEdit")).Checked == true ? "1" : "0");
                string url = ((RadLabel)e.Item.FindControl("lblUrlEdit")).Text;
                string command = ((RadLabel)e.Item.FindControl("lblCommandEdit")).Text;

                if (!IsValidFunctionPermission(functioncode, url))
                {
                    ucError.Visible = true;
                    return;
                }

                FunctionPermissions.GroupFunctionPermissionsSave(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       int.Parse(Request.QueryString["groupcode"].ToString()),
                       functioncode, command,
                       view,
                       url);

                ucStatus.Text = "Information updated";

                Rebind();
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFunctionPermission_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }

    protected void gvFunctionPermission_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFunctionPermission.CurrentPageIndex + 1;
        BindData();
    }
}
