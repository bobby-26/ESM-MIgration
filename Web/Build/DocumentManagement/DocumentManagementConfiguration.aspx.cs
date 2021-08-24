using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;

public partial class DocumentManagementConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["UNIQUEID"] = "";
                ViewState["SECTIONID"] = "";
                ViewState["REVISIONID"] = "";
                ViewState["COMPANYID"] = "0";

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                {
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
                    ViewState["UNIQUEID"] = Request.QueryString["SECTIONID"].ToString();
                }

                if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
                    ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();

                BindGroup();
                BindDesignation();
                Bind();
            }

            //if (ViewState["REVISIONID"].ToString() != string.Empty && ViewState["REVISIONID"] != null)
            //{
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            //}
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
                if (ViewState["SECTIONID"] != null && ViewState["SECTIONID"].ToString() != string.Empty)
                {
                    string GroupRankList = General.RadCheckBoxList(ddlGroupRank);
                    string DesignationList = General.RadCheckBoxList(ddlDesignation);

                    PhoenixDocumentManagementQuestion.UserConfigurationUpdate(new Guid(ViewState["SECTIONID"].ToString()), General.GetNullableGuid(ViewState["REVISIONID"].ToString()), General.GetNullableString(GroupRankList), General.GetNullableString(DesignationList));
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
        if (ViewState["SECTIONID"] != null && ViewState["SECTIONID"].ToString() != string.Empty)
        {
            DataTable dt;
            dt = PhoenixDocumentManagementQuestion.UserConfigurationList(new Guid(ViewState["SECTIONID"].ToString()), General.GetNullableGuid(ViewState["REVISIONID"].ToString()));
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