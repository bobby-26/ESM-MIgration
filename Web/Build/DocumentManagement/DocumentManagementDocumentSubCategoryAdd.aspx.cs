using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementDocumentSubCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Add", "ADD");
        MenuLocation.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["PARENTCATEGORYID"] != null && Request.QueryString["PARENTCATEGORYID"].ToString() != string.Empty)
                ViewState["PARENTCATEGORYID"] = Request.QueryString["PARENTCATEGORYID"].ToString();
            else
                ViewState["PARENTCATEGORYID"] = "";         
        }
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                       
            if (General.GetNullableString(txtCategoryNumber.Text) == null)
            {
                lblMessage.Text = "Category number is required.";
                return;
            }
            if (General.GetNullableString(txtCategoryName.Text) == null)
            {
                lblMessage.Text = "Category name is required.";
                return;
            }

            string Script = "";

            if (ViewState["PARENTCATEGORYID"] != null && ViewState["PARENTCATEGORYID"] != null)
            {
                if (dce.CommandName.ToUpper().Equals("ADD"))
                {
                    //PhoenixDocumentManagementCategory.InsertDocumentCategory(
                    //              General.GetNullableGuid(ViewState["PARENTCATEGORYID"].ToString())
                    //            , txtCategoryName.Text
                    //            , chkActiveYN.Checked ? 1 : 0
                    //            , txtCategoryNumber.Text
                    //            , null
                    //            , null
                    //            );

                    Reset();

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('CI','ifMoreInfo',null);";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                    lblMessage.Text = "Sub Category is added.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    lblMessage.Visible = true;                                      
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }

    protected void Reset()
    {
        txtCategoryName.Text = "";
        txtCategoryNumber.Text = "";
        chkActiveYN.Checked = true;
    }
}
