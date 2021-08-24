using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagenetTimePicker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ucPublishedDateEdit.Text = DateTime.Now.ToString();
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"].ToString() != "")
                ViewState["FORMID"] = Request.QueryString["formid"].ToString();
           
            if (Request.QueryString["formrevisionid"] != null && Request.QueryString["formrevisionid"].ToString() != "")
            {
                ViewState["FORMREVISIONID"] = Request.QueryString["formrevisionid"].ToString();

            }
     
        }
    }
    protected void Onclick_approve(object sender, EventArgs e)
    {
        if (ViewState["FORMREVISIONID"] != null && General.GetNullableGuid(ViewState["FORMREVISIONID"].ToString()) != null)
        {

            string pubdate = ucPublishedDateEdit.Text;
            PhoenixDocumentManagementForm.FormPublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["FORMID"].ToString())
                    ,General.GetNullableDateTime(ucPublishedDateEdit.Text)
                    , new Guid(ViewState["FORMREVISIONID"].ToString()));
                    


            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
           

        }
       
    }
    protected void Onclick_cancel(object sender, EventArgs e)
    {
        Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx");
    }
}