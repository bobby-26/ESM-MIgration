using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentParentSectionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
            MenuLocation.MenuList = toolbarmain.Show();

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
                BindParentDropdown();
                SetParent();
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }

    protected void BindParentDropdown()
    {
        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionListByParent(
                                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , new Guid(ViewState["DOCUMENTID"].ToString())
                                        , null
                                        , null);

        if (ds.Tables.Count > 0)
        {
            ddlParentSection.DataSource = ds.Tables[0];
            ddlParentSection.DataTextField = "FLDSECTIONFULLNAME";
            ddlParentSection.DataValueField = "FLDSECTIONID";
            ddlParentSection.DataBind();

            ddlParentSection.Items.Insert(0, new DropDownListItem("--Select--") );
        }
    }

    protected void SetParent()
    {
        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string strParent = ds.Tables[0].Rows[0]["FLDPARENTSECTIONID"].ToString();            
            ddlParentSection.SelectedValue = strParent;
        }
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) == null)
                {
                    lblMessage.Text = "Document is not selected.";
                    return;
                }
                if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
                {
                    lblMessage.Text = "Section is not selected.";
                    return;
                }

                PhoenixDocumentManagementDocumentSection.DocumentSectionParentUpdate(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["DOCUMENTID"].ToString())
                            , new Guid(ViewState["SECTIONID"].ToString())
                            , General.GetNullableGuid(ddlParentSection.SelectedValue)
                            );

                lblMessage.Text = "Parent Section has been set successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                lblMessage.Visible = true;

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('CI','ifMoreInfo');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }
}
