using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsFunctionPermission : PhoenixBasePage
{
    public static Boolean f;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Add", "ADDURL", ToolBarDirection.Right);
            MenuSecurityAccessRights.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFunctionPermission.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            Filter.CurrentMenuCodeSelection = Request.QueryString["menucode"].ToString();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFunctionPermission.SelectedIndexes.Clear();
        gvFunctionPermission.EditIndexes.Clear();
        gvFunctionPermission.DataSource = null;
        gvFunctionPermission.Rebind();
    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.Equals("ADDURL"))
            {
                Response.Redirect("OptionsFunctionPermissionPageSelection.aspx?menucode=" + Request.QueryString["menucode"].ToString());
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
            int iRowCount = 0;
            int iTotalPageCount = 0;


            DataSet ds1 = FunctionPermissions.FunctionPermissionsSearch(int.Parse(Filter.CurrentMenuCodeSelection.ToString()),
                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvFunctionPermission.PageSize, ref iRowCount,
                ref iTotalPageCount);

            gvFunctionPermission.DataSource = ds1;
            gvFunctionPermission.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
   
    private bool IsValidFunctionPermission(string functioncode, string command, string url,
        short view, short amend, short add, short delete, short authorize, short special)
    {
        ucError.HeaderMessage = "Please provide the following information";

        if (functioncode.Trim() == "")
            ucError.ErrorMessage = "Function Code is required";

        if (command.Trim() == "")
            ucError.ErrorMessage = "Command is required";

        if (url.Trim() == "")
            ucError.ErrorMessage = "Url is required.";

        if ((view == 0) && (amend == 0) && (add == 0) && (delete == 0) && (authorize == 0) && (special == 0))
            ucError.ErrorMessage = "Select one Permission.";

        return (!ucError.IsError);
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

  
    protected void gvFunctionPermission_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string functioncode = ((RadTextBox)e.Item.FindControl("txtFunctionCodeAdd")).Text;
                string command = ((RadTextBox)e.Item.FindControl("txtCommandAdd")).Text;
                short view = short.Parse(((RadioButton)e.Item.FindControl("radioViewYNAdd")).Checked ? "1" : "0");
                short amend = short.Parse(((RadioButton)e.Item.FindControl("radioAmendYNAdd")).Checked ? "1" : "0");
                short add = short.Parse(((RadioButton)e.Item.FindControl("radioAddYNAdd")).Checked ? "1" : "0");
                short delete = short.Parse(((RadioButton)e.Item.FindControl("radioDeleteYNAdd")).Checked ? "1" : "0");
                short authorize = short.Parse(((RadioButton)e.Item.FindControl("radioAuthorizeYNAdd")).Checked ? "1" : "0");
                short special = short.Parse(((RadioButton)e.Item.FindControl("radioSpecialYNAdd")).Checked ? "1" : "0");
                string url = ((RadTextBox)e.Item.FindControl("txtUrlAdd")).Text;

                if (!IsValidFunctionPermission(functioncode, command, url,
                    view, amend, add, delete, authorize, special))
                {
                    ucError.Visible = true;
                    return;
                }

                FunctionPermissions.FunctionPermissionsSave(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(Request.QueryString["menucode"].ToString()),
                    command,
                    url, view);

                Rebind();
                ((RadTextBox)e.Item.FindControl("txtFunctionCodeAdd")).Focus();
               
                ucStatus.Text = "Information added";

            }
      
           else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                FunctionPermissions.FunctionPermissionsDelete(int.Parse(((RadLabel)e.Item.FindControl("lblFunctionId")).Text));
                ucStatus.Text = "Information deleted";
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

    protected void gvFunctionPermission_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
              
            }
                if (e.Item is GridFooterItem)
                {
                    LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
                    if (cmdAdd != null)
                        cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
                }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFunctionPermission_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFunctionPermission.CurrentPageIndex + 1;
        BindData();
    }
}
