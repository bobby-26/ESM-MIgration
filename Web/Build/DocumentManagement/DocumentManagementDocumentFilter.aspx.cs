using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using System.Data;

public partial class DocumentManagementDocumentFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            MenuIncidentFilter.AccessRights = this.ViewState;
            MenuIncidentFilter.MenuList = toolbar.Show();
            
            GetDocumentList();
            GetDocumentSectionList();
        }
    }

    private void GetDocumentList()
    {
        //DataSet ds = PhoenixDocumentManagementDocument.DocumentList(null);
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlDocument.DataSource = ds;
        //    ddlDocument.DataTextField = "FLDDOCUMENTNAME";
        //    ddlDocument.DataValueField = "FLDDOCUMENTID";

        //    ddlDocument.DataBind();
        //    ddlDocument.Items.Insert(0, new ListItem("--Select--", "Dummy"));            
        //}
    }

    private void GetDocumentSectionList()
    {
        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionList(General.GetNullableGuid(ddlDocument.SelectedValue), null);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSection.DataSource = ds;
            ddlSection.DataTextField = "FLDSECTIONNAME";
            ddlSection.DataValueField = "FLDSECTIONID";

            ddlSection.DataBind();
            ddlSection.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
        else
        {
            ddlSection.DataSource = ds;
            ddlSection.DataTextField = "FLDSECTIONNAME";
            ddlSection.DataValueField = "FLDSECTIONID";

            ddlSection.DataBind();
            ddlSection.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
    }

    protected void ddlDocument_Changed(object sender, EventArgs e)
    {
        GetDocumentSectionList();
    }

    protected void MenuIncidentFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            
            criteria.Add("ddlDocument", General.GetNullableString(ddlDocument.SelectedValue));
            criteria.Add("ddlSection", General.GetNullableString(ddlSection.SelectedValue));
            criteria.Add("ddlRevison", General.GetNullableString(""));
            
            Filter.CurrentDMSDocumentFilter = criteria;
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            //criteria.Clear();
            //Filter.CurrentDrillListFilter = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }   
}
