using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementDocumentSectionContentEditReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Revise", "REVISE", ToolBarDirection.Right);
        MenuOfficeComment.AccessRights = this.ViewState;
        MenuOfficeComment.MenuList = toolbarmain.Show();


        if (!IsPostBack)
        {
         
            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
            else
                ViewState["SECTIONID"] = "";

            if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
                ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();
            else
                ViewState["REVISIONID"] = "";

            //string str = null;
            //str = ViewState["REVISIONID"].ToString();

            BindOfficeComments();
        }
    }

    protected void MenuOfficeComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (ViewState["DOCUMENTID"].ToString() != null && ViewState["DOCUMENTID"].ToString() != "" && ViewState["SECTIONID"].ToString() != null && ViewState["SECTIONID"].ToString() != "")
            {
                if (CommandName.ToUpper().Equals("REVISE"))
                {
                    PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonOfficeRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["DOCUMENTID"].ToString())
                        , new Guid(ViewState["SECTIONID"].ToString())
                        , txtOfficeComments.Text
                        , new Guid(ViewState["REVISIONID"].ToString())
                        );
                    //BindOfficeComments();
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"], false);
            }
            BindOfficeComments();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    private void BindOfficeComments()
    {

        DataTable dt = new DataTable();
        txtOfficeComments.Text = null;
        int rowcount = 0; 

        if (ViewState["SECTIONID"] != null && ViewState["SECTIONID"].ToString() != "")
        {
            dt = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonEditRemarks(new Guid(ViewState["SECTIONID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                dt = RemoveDuplicateRows(dt, "FLDOFFICEREMARKS");
                repDiscussion.DataSource = dt;
                repDiscussion.DataBind();
                dt.DefaultView.ToTable(true, "FLDOFFICEREMARKS");
            }
            else
            {
                ShowNoRecordsFound(dt, repDiscussion);
            }
            for (rowcount = 0; rowcount < dt.Rows.Count; rowcount++)
            {
                txtOfficeComments.Text = txtOfficeComments.Text.Trim() + " " + dt.Rows[rowcount]["FLDOFFICEREMARKS"].ToString();
            }

        }               

    }

    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

}
