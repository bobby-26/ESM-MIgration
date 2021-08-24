using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Registers_RegisterCrewRoleConfigurationUserList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        MenuUser.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["APPROVERROLEID"] = "";

            if (Request.QueryString["ApproverRoleId"] != null && Request.QueryString["ApproverRoleId"].ToString() != "")
                ViewState["APPROVERROLEID"] = Request.QueryString["ApproverRoleId"].ToString();
            BindApproverRole();
            BindDesignation();
        }
    }

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        ds = PhoenixRegisterCrewApprovalConfiguration.CrewApprovalUserSearch(
                       General.GetNullableInteger(ddlDesignation.SelectedValue),
                       General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvUser.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);

        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvUser_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvUser_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvUser_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    }
    protected void BindDesignation()
    {
        ddlDesignation.DataSource = PhoenixRegisterCrewApprovalConfiguration.CrewDesignationRoleMappingList(null,null,General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()));
        ddlDesignation.DataTextField = "FLDDESIGNATIONNAME";
        ddlDesignation.DataValueField = "FLDDESIGNATIONID";
        ddlDesignation.DataBind();
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindApproverRole()
    {
        DataSet ds;
        ds = PhoenixRegisterCrewApprovalConfiguration.CrewRoleConfigurationList(General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtRole.Text = dr["FLDCONFIGURATIONNAME"].ToString();
        }
    }
    protected void gvUser_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUser.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}