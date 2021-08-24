using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using System.IO;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class OptionsFunctionPermissionPageSelection : PhoenixBasePage
{
    public string url;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Permissions", "PAGEACCESS", ToolBarDirection.Right);
            MenuUrlSelection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ddlFolders.Items.Clear();
                ViewState["path"] = HttpContext.Current.Server.MapPath("~");
                string[] dirs = Directory.GetDirectories(ViewState["path"].ToString());
                int cnt = dirs.Length;
                if (cnt == 0)
                {
                    dirs[0] = ViewState["path"].ToString();
                }
                else
                {
                    dirs[cnt - 1] = ViewState["path"].ToString();
                }
                string dp = string.Empty;
                foreach (string p in dirs)
                {
                    if (p != ViewState["path"].ToString())
                    {
                        dp = p.Substring(ViewState["path"].ToString().Length + 1);
                    }
                    else
                    {
                        dp = "Web";
                    }
                   
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = dp;
                    item.Value = dp;
                    ddlFolders.Items.Add(item);
                 
                }
                ShowFiles(ddlFolders.Items[0].Value);
              
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ddlFolders_TextChanged(object sender, EventArgs e)
    {
        ShowFiles(ddlFolders.SelectedValue);
    }

    protected void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    private void ShowFiles(string selectedvalue)
    {
        string[] files = new string[10000];
        if (selectedvalue.ToUpper() != "WEB")
            files = Directory.GetFiles(ViewState["path"].ToString() + "\\" + selectedvalue, "*.*").Where(file => file.ToLower().EndsWith(".aspx") || file.ToLower().EndsWith(".rpt")).ToArray();
        else
            files = Directory.GetFiles(ViewState["path"].ToString(), "*.*").Where(file => file.ToLower().EndsWith(".aspx") || file.ToLower().EndsWith(".rpt")).ToArray();

        ddlFiles.Items.Clear();
        foreach (string p in files)
        {
            string f = p.Substring(ViewState["path"].ToString().Length + 1);
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = "~/" + f.Replace('\\', '/');
            item.Value = "~/" + f.Replace('\\', '/');
             ddlFiles.Items.Add(item);
            //ddlFiles.Items.Add(new DropDownListItem("~/" + f.Replace('\\', '/'), "~/" + f.Replace('\\', '/')));
        }
    }

    protected void UrlSelection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.Equals("PAGEACCESS"))
                Response.Redirect("OptionsFunctionPermission.aspx?menucode=" + Request.QueryString["menucode"].ToString());
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindData()
    {

        string urlcode = ddlFiles.SelectedValue;
        DataSet ds = FunctionPermissions.FunctionPermissionsList(int.Parse(Request.QueryString["menucode"].ToString()), urlcode);


        gvCommand.DataSource = ds;
        gvCommand.VirtualItemCount = ds.Tables[0].Rows.Count;
    }


    protected void Rebind()
    {
        gvCommand.SelectedIndexes.Clear();
        gvCommand.EditIndexes.Clear();
        gvCommand.DataSource = null;
        gvCommand.Rebind();
    }

    protected void gvCommand_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                FunctionPermissions.FunctionPermissionsSave(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     int.Parse(Request.QueryString["menucode"].ToString()),
                     ((RadTextBox)e.Item.FindControl("txtCommandNameAdd")).Text,
                    ddlFiles.SelectedValue, 1
                    );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtCommandNameAdd")).Focus();
            }
          
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                FunctionPermissions.FunctionPermissionsDelete(
                    int.Parse(((RadLabel)e.Item.FindControl("lblFunctionId")).Text));
                Rebind();
            }
         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCommand_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
        if (e.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvCommand_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCommand.CurrentPageIndex + 1;
        BindData();
    }
}
