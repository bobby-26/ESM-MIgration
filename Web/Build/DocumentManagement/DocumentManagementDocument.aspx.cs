using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using Telerik.Web.UI;

public partial class DocumentManagementDocument : PhoenixBasePage
{
    int num = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        imgSearch.Visible = SessionUtil.CanAccess(this.ViewState, "KEYWORDSEARCH");
        lblkeyword.Visible = SessionUtil.CanAccess(this.ViewState, "KEYWORDLABEL");
        txtKeyWord.Visible = SessionUtil.CanAccess(this.ViewState, "KEYWORDTEXT");
        if (!IsPostBack)
        {
            ViewState["DOCUMENTID"] = "";
            ViewState["selectednode"] = "";
            ViewState["TYPE"] = "0";
            Session["selectednode"] = "";
            SetMenuVisibility();
            //setMenu();
        }
        BindDocumentCategoryTree();
        setMenu();
    }

    private void setMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["viewonly"] != null)
            toolbar.AddButton("List", "LIST");
        toolbar.AddImageLink("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementComments.aspx?REFERENCEID=" + ViewState["selectednode"].ToString() + "'); return false;", "Comments", "", "COMMENTS", ToolBarDirection.Right);
        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbar.Show();
        MenuDiscussion.SelectedMenuIndex = 1;
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../DocumentManagement/DocumentManagementCommentsList.aspx", false);
        }

    }

    private void BindDocumentCategoryTree()
    {
        try
        {
            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            var sb = new StringBuilder();
            string unorderedList = "";
            sitemap.InnerHtml = "";

            unorderedList = GenerateULForOffice(sb);

            sitemap.InnerHtml = "<li id=\"parentlist\"><a href=\"#\" >HSEQA Document</a>" + unorderedList + "</li>";

            string script = "PrepareTree();resizeFrame(document.getElementById('ifMoreInfo')); resizeFrame(document.getElementById('divDocumentCategory'));\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
            sb = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private string GenerateULForOffice(StringBuilder sb)
    {
        sb.AppendLine("<ul id=ul" + num + ">");
        DataSet ds = PhoenixDocumentManagementDocument.OfficeHSEQATreeEdit(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                );
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            sb.Append(ds.Tables[0].Rows[0]["FLDHTML"]);

        ds = null;
        sb.Append("</ul>");
        num++;
        return sb.ToString();
    }

    private string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
    {
        sb.AppendLine("<ul id=ul" + num + ">");
        if (menu.Length > 0)
        {
            foreach (DataRow dr in menu)
            {
                string handler = dr["FLDURL"].ToString();
                string menuText = dr["FLDNAME"].ToString();
                string pid = dr["FLDREFERENCEID"].ToString();
                string title = dr["FLDTYPENAME"].ToString();
                string type = dr["FLDTYPE"].ToString();

                string line = String.Format(@"<li id=""" + pid + @"""><a href=""javascript:void(0);"" onclick=""SetSourceURL(" + type + ",'" + handler + @"','" + pid + @"');"" title={0}>{1}</a>", title, menuText);
                sb.Append(line);

                string parentId = dr["FLDPARENTREFERENCEID"].ToString();

                DataRow[] subMenu = table.Select(String.Format("FLDPARENTREFERENCEID = {0}", "'" + pid + "'"));
                if (subMenu.Length > 0 && !pid.Equals(parentId))
                {
                    var subMenuBuilder = new StringBuilder();
                    sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                }
                sb.Append("</li>");
            }
        }
        sb.Append("</ul>");
        num++;
        return sb.ToString();
    }        

    

   
    protected void SetMenuVisibility()
    {
        //CheckBox chkShowMenu = (CheckBox)ucTitle.FindControl("chkShowMenu");
        //chkShowMenu.Checked = false;

        //AjaxControlToolkit.ToggleButtonExtender ToggleEx = (AjaxControlToolkit.ToggleButtonExtender)ucTitle.FindControl("ToggleEx");
        //ToggleEx.CheckedImageAlternateText = "Show Menu";
        //ToggleEx.UncheckedImageAlternateText = "Hide Menu";

        //string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        //Script += @"var i = document.getElementById(""ucTitle_chkShowMenu""); javascript:ResizeMenu(i) ;";
        //Script += "</script>" + "\n";
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    //protected void imgSearch_Click(object sender, EventArgs e)
    //{
    //    string searchvalue = "";
    //    searchvalue = txtKeyWord.Text;

    //    ifMoreInfo.Attributes["src"] = "../DocumentManagement/DocumentManagementSearchResults.aspx?keyword=" + searchvalue;

    //}
}
