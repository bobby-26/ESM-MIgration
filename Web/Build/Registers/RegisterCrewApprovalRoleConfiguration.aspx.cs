using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterCrewApprovalRoleConfiguration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalRoleConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewApproverRole')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalRoleConfiguration.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalRoleConfiguration.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCRoleConfigurationAdd.aspx?" + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuCrewApproverRole.AccessRights = this.ViewState;
            MenuCrewApproverRole.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewApproverRole.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuCrewApproverRole_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                gvCrewApproverRole.CurrentPageIndex = 0;
               
                gvCrewApproverRole.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtRole.Text = "";
                txtShortCode.Text = "";
              
                gvCrewApproverRole.Rebind();
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
        gvCrewApproverRole.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCONFIGURATIONCODE", "FLDCONFIGURATIONNAME" };
        string[] alCaptions = { "Code", "Role" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegisterCrewApprovalConfiguration.CrewApproverRoleSearch(General.GetNullableString(txtShortCode.Text)
                                                                            , General.GetNullableString(txtRole.Text)
                                                                            , sortexpression, sortdirection
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , gvCrewApproverRole.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        General.SetPrintOptions("gvCrewApproverRole", "Crew Approver Role", alCaptions, alColumns, ds);
        gvCrewApproverRole.DataSource = ds;
        gvCrewApproverRole.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrewApproverRole_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewApproverRole.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewApproverRole_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "ADD")
            {
                RadTextBox txtcodeAdd = (RadTextBox)e.Item.FindControl("txtcodeAdd");
                RadTextBox txtRoleAdd = (RadTextBox)e.Item.FindControl("txtRoleAdd");
                if (!IsValidation(txtcodeAdd.Text,
                                    txtRoleAdd.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterCrewApprovalConfiguration.CrewApproverRoleInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , txtRoleAdd.Text,txtcodeAdd.Text);
                gvCrewApproverRole.Rebind();
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
                RadLabel lblRoleId = (RadLabel)e.Item.FindControl("lblRoleId");
                PhoenixRegisterCrewApprovalConfiguration.CrewApprovalRoleDelete(new Guid(lblRoleId.Text));
                gvCrewApproverRole.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if(e.CommandName.ToUpper()=="SAVE")
            {
                RadLabel lblRoleIdEdit = (RadLabel)e.Item.FindControl("lblRoleIdEdit");
                RadTextBox txtcodeEdit = (RadTextBox)e.Item.FindControl("txtcodeEdit");
                RadTextBox txtRoleEdit = (RadTextBox)e.Item.FindControl("txtRoleEdit");
                if (!IsValidation(txtcodeEdit.Text,
                                    txtRoleEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterCrewApprovalConfiguration.CrewApproverRoleUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               ,txtRoleEdit.Text
                                                                               ,txtcodeEdit.Text 
                                                                               ,new Guid(lblRoleIdEdit.Text));
              
                gvCrewApproverRole.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidation(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Role is required.";

        return (!ucError.IsError);
    }
    protected void gvCrewApproverRole_ItemDataBound(object sender, GridItemEventArgs e)
    {
        ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
        if (db != null)
        {
            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCONFIGURATIONCODE", "FLDCONFIGURATIONNAME" };
        string[] alCaptions = { "Code", "Role" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterCrewApprovalConfiguration.CrewApproverRoleSearch(General.GetNullableString(txtShortCode.Text)
                                                                              , General.GetNullableString(txtRole.Text)
                                                                              , sortexpression, sortdirection
                                                                              , (int)ViewState["PAGENUMBER"]
                                                                              , gvCrewApproverRole.PageSize
                                                                              , ref iRowCount
                                                                              , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewApproverRole.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='3' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Crew Approver Role</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='3' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='100%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
}