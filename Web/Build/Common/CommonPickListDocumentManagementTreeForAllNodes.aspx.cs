using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;

public partial class Common_CommonPickListDocumentManagementTreeForAllNodes : PhoenixBasePage
{
    int num = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Done", "DONE");
        //MenuDocument.AccessRights = this.ViewState;
        //MenuDocument.MenuList = toolbarmain.Show();

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            if (Request.QueryString["companyid"] != null && Request.QueryString["companyid"].ToString() != "")
                ViewState["companyid"] = Request.QueryString["companyid"].ToString();
            else
                ViewState["companyid"] = "";
            //Filter.CurrentDMSDocumentFilter = null;
            ViewState["DOCUMENTID"] = "";
            ViewState["selectednode"] = "";
            ViewState["TYPE"] = "0";

            BindDocumentCategoryTree();
        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                unorderedList = GenerateULForOffice(sb);
            }
            else
            {
                DataSet ds = PhoenixDocumentManagementDocument.DocumentTreeList(companyid);
                DataTable table = ds.Tables[0];
                DataRow[] parentMenus = table.Select("FLDLEVEL=1");
                unorderedList = GenerateUL(parentMenus, table, sb);
            }
            sitemap.InnerHtml = "<li id=\"parentlist\"><a href=\"#\" >HSEQA Document</a>" + unorderedList + "</li>";

            string script = "PrepareTree(); resizeFrame(document.getElementById('divDocumentCategory'));\r\n;";
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    private string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
    {
        sb.AppendLine("<ul id=ul" + num + ">");
        if (menu.Length > 0)
        {
            foreach (DataRow dr in menu)
            {
                string menuText = dr["FLDNAME"].ToString();
                string pid = dr["FLDREFERENCEID"].ToString();
                string title = dr["FLDTYPENAME"].ToString();
                string type = dr["FLDTYPE"].ToString();

                string line = String.Format(@"<li id=""" + pid + @"""><a href=""javascript:void(0);"" onclick=""OnNodeClick(" + type + ",'" + menuText + @"','" + pid + @"');"" title={0}>{1}</a>", title, menuText);
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
        return sb.ToString();
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
}
