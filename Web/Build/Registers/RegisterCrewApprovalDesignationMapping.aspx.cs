using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterCrewApprovalDesignationMapping : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalDesignationMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewDesignation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalDesignationMapping.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalDesignationMapping.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersDesignation.AccessRights = this.ViewState;
            MenuRegistersDesignation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindCrewApprovalRole();
                ddlApproverRole.SelectedIndex = 1;
            }


        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewDesignation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DESELECT"))
            {
                PhoenixRegisterCrewApprovalConfiguration.CrewApproverDesignationMappingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text)
                                                                        , new Guid(ddlApproverRole.SelectedValue));
                BindData();
                BindCrewDesignationMapping();
                gvCrewDesignation.Rebind();
                gvDesignation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
                ((RadTextBox)e.Item.FindControl("txtNameEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewDesignation_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvDesignation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                PhoenixRegisterCrewApprovalConfiguration.CrewApproverDesignationMappingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text)
                                                                        , new Guid(ddlApproverRole.SelectedValue));
                BindData();
                BindCrewDesignationMapping();
                gvDesignation.Rebind();
                gvCrewDesignation.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        BindData();
        BindCrewDesignationMapping();

    }
    protected void gvDesignation_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvDesignation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDesignation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCrewApprovalRole()
    {
        ddlApproverRole.DataSource = PhoenixRegisterCrewApprovalConfiguration.CrewRoleConfigurationList();
        ddlApproverRole.DataTextField = "FLDCONFIGURATIONNAME";
        ddlApproverRole.DataValueField = "FLDROLECONFIGURATIONID";
        ddlApproverRole.DataBind();

    }

    protected void MenuRegistersDesignation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();
                BindCrewDesignationMapping();
                gvCrewDesignation.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtCode.Text = "";
                txtName.Text = "";
                BindData();
                BindCrewDesignationMapping();
                gvDesignation.Rebind();
                gvCrewDesignation.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "DESIGNATION CODE", "DESIGNATIO NNAME" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegisterCrewApprovalConfiguration.CrewDesignationSearch(txtCode.Text, txtName.Text, General.GetNullableGuid(ddlApproverRole.SelectedValue), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDesignation.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDesignation", "Registers", alCaptions, alColumns, ds);
        gvDesignation.DataSource = ds;
        gvDesignation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindCrewDesignationMapping()
    {

        string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "Code", "Name" };

        DataSet ds = new DataSet();

        ds = PhoenixRegisterCrewApprovalConfiguration.CrewDesignationRoleMappingList(txtCode.Text, txtName.Text, General.GetNullableGuid(ddlApproverRole.SelectedValue));

        General.SetPrintOptions("gvCrewDesignation", "Crew Designation Mapping", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewDesignation.DataSource = ds;
        }
    }

    protected void ddlApproverRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        BindCrewDesignationMapping();
    }


    protected void gvCrewDesignation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCrewDesignationMapping();
    }
}