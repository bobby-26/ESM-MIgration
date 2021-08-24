using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersTrainingStaff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersTrainingStaff.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuTrainingStaff.AccessRights = this.ViewState;
            MenuTrainingStaff.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Department", "DEPARTMENT");
            toolbar.AddButton("Staff", "STAFF");
            MenuTraining.AccessRights = this.ViewState;
            MenuTraining.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ddlDepartment.DataSource = PhoenixRegistersTrainingDepartment.ListTrainingDepartment();
                ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
                ddlDepartment.DataValueField = "FLDDEPARTMENTID";

                if (Request.QueryString["departmentid"] != null)
                {
                    ddlDepartment.SelectedValue = Request.QueryString["departmentid"];
                    ddlDepartment.Enabled = false;
                }
            }
            ddlDepartment.DataBind();
            MenuTraining.SelectedMenuIndex = 1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Training_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("DEPARTMENT"))
        {
            Response.Redirect("../Registers/RegistersTrainingDepartment.aspx?departmentid=" + Request.QueryString["departmentid"], false);
        }

    }
    protected void ShowExcel()
    {

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATIONNAME", "FLDFACULTYCODE" };
        string[] alCaptions = { "Name", "Designation", "Code" };

        ds = PhoenixCrewCourseDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=TrainingStaff.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Training Staff</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<b>Department</b> " + ddlDepartment.SelectedItem.Text);
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


    protected void TrainingStaff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
                BindMappingData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindMappingData()
    {

        try
        {
            string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATIONNAME", "FLDFACULTYCODE" };
            string[] alCaptions = { "Name", "Designation", "Code" };
            DataSet ds = PhoenixCrewCourseDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));
            General.SetPrintOptions("gvMapping", "Training Staff", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvMapping.DataSource = ds;
            }
            else
            {
                gvMapping.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMapping(string desingationid, string userid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(desingationid) == null)
            ucError.ErrorMessage = "Designation is required.";

        if (General.GetNullableInteger(userid) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewCourseDesignation.DeleteDesignationMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblMappingIdadd")).Text));
                BindMappingData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadCheckBoxList chkdesignation = ((RadCheckBoxList)e.Item.FindControl("cblDesignationEdit"));
                StringBuilder strdesignationlist = new StringBuilder();
                foreach (ButtonListItem item in chkdesignation.Items)
                {
                    if (item.Selected == true)
                    {
                        strdesignationlist.Append(item.Value.ToString());
                        strdesignationlist.Append(",");
                    }
                }
                if (strdesignationlist.Length > 1)
                {
                    strdesignationlist.Remove(strdesignationlist.Length - 1, 1);
                }

                if (!IsValidMapping(strdesignationlist.ToString(),
                                                    ((RadLabel)e.Item.FindControl("lblUserCodeEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblMappingIdEdit")).Text) == null)
                {
                    PhoenixCrewCourseDesignation.InsertDesignationMapping(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        strdesignationlist.ToString(),
                        Convert.ToInt32(((RadLabel)e.Item.FindControl("lblUserCodeEdit")).Text),
                       ((RadTextBox)e.Item.FindControl("txtFacultyCodeEdit")).Text, null
                );
                }
                else
                {
                    PhoenixCrewCourseDesignation.UpdateDesignationMapping(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(((RadLabel)e.Item.FindControl("lblMappingIdEdit")).Text),
                        strdesignationlist.ToString(),
                        Convert.ToInt32(((RadLabel)e.Item.FindControl("lblUserCodeEdit")).Text),
                        ((RadTextBox)e.Item.FindControl("txtFacultyCodeEdit")).Text,
                        General.GetNullableDecimal(((RadTextBox)e.Item.FindControl("txtFacultyLoadEdit")).Text)
                    );
                }
                BindMappingData();
                gvMapping.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindMappingData();
    }

    protected void gvMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadCheckBoxList cbldesignation = (RadCheckBoxList)e.Item.FindControl("cblDesignationEdit");
            DataRowView drv1 = (DataRowView)e.Item.DataItem;
            if (cbldesignation != null)
            {
                string[] strdesignation = drv1["FLDDESIGNATIONID"].ToString().Split(',');
                foreach (string item in strdesignation)
                {
                    if (item.Trim() != "")
                    {
                        cbldesignation.SelectedValue = item;
                    }
                }
            }
        }
    }
}

                
