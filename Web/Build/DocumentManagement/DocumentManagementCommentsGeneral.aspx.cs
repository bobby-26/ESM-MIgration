using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementCommentsGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);    

        if (!IsPostBack)
        {
            if (Request.QueryString["COMMENTSID"] != null && Request.QueryString["COMMENTSID"].ToString() != string.Empty)
                ViewState["COMMENTSID"] = Request.QueryString["COMMENTSID"].ToString();
            else
                ViewState["COMMENTSID"] = null;

            BindData();
        }        
    }

    private void BindData()
    {
        DataSet ds = new DataSet();

        if (ViewState["COMMENTSID"] != null && General.GetNullableGuid(ViewState["COMMENTSID"].ToString()) != null)
        {
            ds = PhoenixDocumentManagementDocument.EditDMSComments(new Guid(ViewState["COMMENTSID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtComment.Value = ds.Tables[0].Rows[0]["FLDCOMMENTS"].ToString();
            }
        }
    }
    
    protected void MenuBugDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
    }    
}
