using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;


public partial class DocumentManagement_DocumentManagementDocumentPublishedDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SAVE", "SAVE");
            MenuPublished.AccessRights = this.ViewState;
            MenuPublished.MenuList = toolbarmain.Show();

            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
            {
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            }
            if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
            {
                ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();
            }
            else 
            {
                ViewState["REVISIONID"] = "";
            }
            BindPublishedDate();
        }
    }

    protected void MenuPublished_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (General.GetNullableString(ucPublishedDate.Text) == null)
            {
                lblMessage.Text = "Published Date is required.";
                return;
            }

            string Script = "";

            if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != string.Empty)
            {
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixDocumentManagementDocument.DocumentRevisionApprove(
                                             PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                             , General.GetNullableDateTime(ucPublishedDate.Text)
                                             , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                             );
                    BindPublishedDate();
                    lblMessage.Text = "Published Date is updated.";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('CI','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    private void BindPublishedDate()
    {
        //DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["REFERENCEID"].ToString()));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    txtDirectorComments.Text = dr["FLDDIRECTORCOMMENT"].ToString();
        //    txtDirectorCommentedByName.Text = dr["FLDDIRECTORCOMMENTDBY"].ToString();
        //    ucDirectorCommentDate.Text = dr["FLDDIRECTORCOMMENTDATE"].ToString();
        //}
    }
}
