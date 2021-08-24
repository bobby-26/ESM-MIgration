using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterUserFuntionalRole : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterUserFuntionalRole.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRole')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterUserFuntionalRole.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterUserFuntionalRole.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersRole.AccessRights = this.ViewState;
            MenuRegistersRole.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRole.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRoleCODE", "FLDRoleNAME", "FLDPROCESS" };
        string[] alCaptions = { "Code", "Name", "Process" };
        string sortexpression;
        int? sortdirection = null;



        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersRole.RoleSearch(txtCode.Text, txtName.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Functional_Role.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Functional Role </h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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

    protected void RegistersRole_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtCode.Text = "";
                txtName.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRole.SelectedCellIndexes.Clear();
        gvRole.EditIndexes.Clear();
        gvRole.DataSource = null;
        gvRole.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRole.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDRoleCODE", "FLDRoleNAME", "FLDPROCESS" };
        string[] alCaptions = { "Code", "Name", "Process" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (General.IsHRMSRequiredYN())
        {
            gvRole.ShowFooter = false;
        }

        ds = PhoenixRegistersRole.RoleSearch(txtCode.Text, txtName.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRole.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRole", "Functional Role", alCaptions, alColumns, ds);

        gvRole.DataSource = ds;
        gvRole.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvRole_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRole(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertRole(
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRole(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateRole(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblRoleIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                 );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["RoleId"] = ((RadLabel)e.Item.FindControl("lblRoleId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRole_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton cmdmap = (LinkButton)e.Item.FindControl("cmdProcessMapping");

            if (cmdmap != null)
            {
                cmdmap.Visible = SessionUtil.CanAccess(this.ViewState, cmdmap.CommandName);
                cmdmap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersFunctionalRoleProcessMapping.aspx?roleid=" + drv["FLDROLEID"].ToString() + "');return false;");

            }

            LinkButton cmdDesignationMapping = (LinkButton)e.Item.FindControl("cmdDesignationMapping");

            if (cmdDesignationMapping != null)
            {
                cmdDesignationMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdDesignationMapping.CommandName);
                cmdDesignationMapping.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersFunctionalRoleDesignationMapping.aspx?roleid=" + drv["FLDROLEID"].ToString() + "');return false;");

            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (General.IsHRMSRequiredYN())
            {
                if (sb != null) sb.Visible = false;
                if (cb != null) cb.Visible = false;
                if (cmdDesignationMapping != null) cmdDesignationMapping.Visible = false;
                if (db != null) db.Visible = false;
                if (eb != null) eb.Visible = false;                
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private void InsertRole(string code, string name)
    {
        PhoenixRegistersRole.InsertRole(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             code, name);
    }

    private void UpdateRole(int Roleid, string code, string name)
    {
        PhoenixRegistersRole.UpdateRole(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Roleid, code, name);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRole(string code, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRole;

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteRole(int Rolecode)
    {
        PhoenixRegistersRole.DeleteRole(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Rolecode);
    }
    protected void gvRole_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvRole_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRole.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteRole(Int32.Parse(ViewState["RoleId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}