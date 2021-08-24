using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonPhoenixAuditChanges : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
            string[] auditparameters = Request.QueryString.AllKeys;
            foreach (string s in auditparameters)
                nvc.Add(s, Request.QueryString[s]);
            nvc.Add("changes", "1");
            string dtkey = Request.QueryString["dtkey"].ToString();
            string shortcode = Request.QueryString["shortcode"].ToString();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarAudit = new PhoenixToolbar();
            toolbarAudit.AddButton("Audit Changes", "AUDITCHANGE", ToolBarDirection.Left);
            toolbarAudit.AddButton("Audit Trail", "AUDITTRAIL",ToolBarDirection.Left);
                MenuPhoenixAudit.MenuList = toolbarAudit.Show();
            MenuPhoenixAudit.SelectedMenuIndex = 0;


            }

            ExecuteAndDisplay(nvc, General.GetNullableGuid(dtkey), shortcode);
    }

    protected void MenuPhoenixAudit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string _querystring = "";
        foreach (string s in Request.QueryString.AllKeys)
        {
            if (_querystring.Equals(""))
                _querystring = _querystring + s + "=" + Request.QueryString[s];
            else
                _querystring = _querystring + "&" + s + "=" + Request.QueryString[s];
        }

        if (CommandName.ToUpper().Equals("AUDITTRAIL"))
        {
            Response.Redirect("../Common/CommonPhoenixAuditTrail.aspx?" + _querystring, false);
        }
    }

    private void ExecuteAndDisplay(NameValueCollection nvc, Guid? dtkey, string shortcode)
    {
        DataTable dt = PhoenixCommonAuditTrail.Execute(nvc, dtkey, shortcode);
        ShowQuery(shortcode, dt);
    }

    private void ShowQuery(string shortcode, DataTable dt)
    {
        gvAuditTrail.DataSource = dt;
    }

    protected void gvAuditTrail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
     
    }
}
