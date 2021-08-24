using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;

public partial class DocumentManagement_DocumentManagementConfigurationNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["DOCUMENTID"] = "";
                ViewState["COMPANYID"] = "0";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
                {
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                }


                BindGroup();
                BindDesignation();
                Bind();
            }


            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindGroup()
    {
        DataSet ds = PhoenixRegistersGroupRank.ListGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlGroupRank.DataSource = ds;
        ddlGroupRank.DataBindings.DataTextField = "FLDGROUPRANK";
        ddlGroupRank.DataBindings.DataValueField = "FLDGROUPRANKID";
        ddlGroupRank.DataBind();
    }
    protected void BindDesignation()
    {
        DataSet dss = PhoenixDocumentManagementDocument.ListFunctionRole();
        ddlDesignation.DataSource = dss;
        ddlDesignation.DataBindings.DataTextField = "FLDROLENAME";
        ddlDesignation.DataBindings.DataValueField = "FLDFUNCTIONALROLEID";
        ddlDesignation.DataBind();
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != string.Empty)
                {
                    string GroupRankList = General.RadCheckBoxList(ddlGroupRank);
                    string DesignationList = General.RadCheckBoxList(ddlDesignation);

                    PhoenixDocumentManagementQuestion.UserDocumentConfigurationUpdate(new Guid(ViewState["DOCUMENTID"].ToString()), General.GetNullableString(GroupRankList), General.GetNullableString(DesignationList));
                    Bind();
                    ucStatus.Text = "User Configuration Updated.";
                    String script = "javascript:fnReloadList('codehelp1',null,'true');";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Bind()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != string.Empty)
        {
            DataTable dt;
            dt = PhoenixDocumentManagementQuestion.UserDocumentConfigurationList(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                General.RadBindCheckBoxList(ddlGroupRank, dr["FLDREADGROUPRANKIDS"].ToString());
                General.RadBindCheckBoxList(ddlDesignation, dr["FLDREADUSERDESIGNATIONIDS"].ToString());
            }
        }
    }

    protected void chkGrouprankAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkGrouprankAll.Checked == true)
        {
            foreach (ButtonListItem item in ddlGroupRank.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in ddlGroupRank.Items)
            {
                item.Selected = false;
            }
        }
    }

    protected void chkCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in ddlDesignation.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in ddlDesignation.Items)
            {
                item.Selected = false;
            }
        }
    }
}