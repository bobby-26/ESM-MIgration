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
public partial class DocumentPhoenixFeatureList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersCity.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentFeaturelistView.aspx'); return false;", "Filter", "document_view.png", "FIND");
            MenuSecurityAccessRights.MenuList = toolbar.Show();
        }
        BindData();
        
    }
    protected void BindData()
    {
        DataSet ds = SessionUtil.MenuAccessTree(PhoenixSecurityContext .CurrentSecurityContext.UserCode);
        gvMenu.DataSource = ds.Tables[0];
        gvMenu.DataBind();
    }
    protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {

        }
     
    }

    protected void gvMenu_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void SecurityAccessRights_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.Equals("FIND"))
        {

        }
       
    }

    protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label menucode = (Label)e.Row.FindControl("lblMenuCode");
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            
            if (menucode != null )
            {
                if (eb != null) 
                    eb.Attributes.Add("onclick", "javascript:parent.Openpopup('code1','','../Document/DocumentPhoenixFeatureListAdd.aspx?MENUCODE=" +menucode.Text + "'); return true;");
            }
        }
    }
}
