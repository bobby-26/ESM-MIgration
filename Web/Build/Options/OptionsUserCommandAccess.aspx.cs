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

public partial class OptionsUserCommandAccess : PhoenixBasePage
{
    public string url;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Menu Permissions", "MENUPERMISSIONS", ToolBarDirection.Right);

        MenuUrlSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ddlFolders.Items.Clear();
            ViewState["path"] = HttpContext.Current.Server.MapPath("~");
            string[] dirs = Directory.GetDirectories(ViewState["path"].ToString());
            ddlFolders.Items.Clear();
            ddlFolders.Items.Add(new DropDownListItem("--Select--", ""));
            foreach (string p in dirs)
            {
                    string dp = p.Substring(ViewState["path"].ToString().Length + 1);
                    ddlFolders.Items.Add(new DropDownListItem(dp, dp));
            }

            ShowFiles(ddlFolders.Items[0].Value);
            ddlFiles.Items.Insert(0, new DropDownListItem("--Select--", ""));

            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            
            }

        }
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
        gvCommand.SelectedIndexes.Clear();
        gvCommand.EditIndexes.Clear();
        gvCommand.DataSource = null;
        gvCommand.Rebind();
    }


    protected void ddlFolders_TextChanged(object sender, EventArgs e)
    {
        ShowFiles(ddlFolders.SelectedValue);
    }

    protected void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        Rebind();
    }

    private void ShowFiles(string selectedvalue)
    {
        string[] files = Directory.GetFiles(ViewState["path"].ToString() + "\\" + selectedvalue, "*.*").Where(file => file.ToLower().EndsWith(".aspx") || file.ToLower().EndsWith(".rpt")).ToArray();

        ddlFiles.Items.Clear();
        ListItem itemSelect = new ListItem();
        itemSelect.Text = "--Select--";
        itemSelect.Value = "Dummy";
        ddlFiles.Items.Add(new DropDownListItem("--Select--", ""));
        foreach (string p in files)
        {
            string f = p.Substring(ViewState["path"].ToString().Length + 1);
            ListItem item = new ListItem();
            item.Text = "~/" + f.Replace('\\', '/');
            item.Value = "~/" + f.Replace('\\', '/');
            ddlFiles.Items.Add(new DropDownListItem("~/" + f.Replace('\\', '/'), "~/" + f.Replace('\\', '/')));
        }
    }

    protected void UrlSelection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.Equals("MENUPERMISSIONS"))
            Response.Redirect("OptionsUserMenuAccess.aspx?usercode=" + Request.QueryString["usercode"].ToString());
        Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindData()
    {

        string usercode = Request.QueryString["usercode"].ToString();


        string urlcode = ddlFiles.SelectedValue;
        DataSet ds = FunctionPermissions.UserPermissionsList(int.Parse(usercode), ddlFolders.SelectedValue,General.GetNullableString(urlcode));
        gvCommand.DataSource = ds;
        gvCommand.VirtualItemCount = ds.Tables[0].Rows.Count;

    }

  
    protected void gvCommand_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {

                string usercode = Request.QueryString["usercode"].ToString();

                RadGrid gv = (RadGrid)e.Item.FindControl("gvUserGroups");
                string urlcode = ((RadLabel)e.Item.FindControl("lblURL")).Text;

                string command = ((RadLabel)e.Item.FindControl("lblCommandName")).Text;

                gv.DataSource = FunctionPermissions.UserCommandPermissionsByUserGroup(int.Parse(usercode), urlcode, command);
                gv.DataBind();
              
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

  
    protected void gvCommand_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCommand.CurrentPageIndex + 1;
        BindData();
    }
}
