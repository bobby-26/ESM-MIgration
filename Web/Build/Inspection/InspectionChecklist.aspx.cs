using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionChecklist : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDepartmentType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuRegistersDepartmentType.AccessRights = this.ViewState;
            MenuRegistersDepartmentType.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDepartmentType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDQUESTION" };
        string[] alCaptions = { "Question" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixInspectionChecklist.InspectionRegisterChecklistSearch(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDepartmentType.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CheckList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>P&I CheckList </h3></td>");
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

    protected void RegistersDepartmentType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {              
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvDepartmentType.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

        string[] alColumns = { "FLDQUESTION" };
        string[] alCaptions = { "Question" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionChecklist.InspectionRegisterChecklistSearch(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvDepartmentType.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDepartmentType", "P&I CheckList", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDepartmentType.DataSource = ds;
            gvDepartmentType.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDepartmentType.DataSource = "";
        }
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvDepartmentType.Rebind();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDepartmentType.Rebind();
    }

    private bool IsValidChecklist(string checklist, string departments, string activeyn)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (checklist.Trim().Equals(""))
            ucError.ErrorMessage = "Question is required.";

        if (departments.Trim().Equals(","))
            ucError.ErrorMessage = "Departments Involved is required.";
        if (activeyn.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }

    private void DeleteDepartmentType(int DepartmentTypecode)
    {

    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDepartmentType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDepartmentType.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDepartmentType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
          
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string question = ((RadTextBox)e.Item.FindControl("txtQuestionAdd")).Text;
                string activeyn = ((RadComboBox)e.Item.FindControl("ddlActiveAdd")).SelectedValue;
                RadCheckBoxList cblList = (RadCheckBoxList)e.Item.FindControl("chkDepartmentsAdd");
                string departments = ",";
                foreach (ButtonListItem item in cblList.Items)
                {
                    if (item.Selected) departments = departments + item.Value.ToString() + ",";
                }
                if (!IsValidChecklist(question, departments, activeyn))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionChecklist.InspectionRegisterChecklistInsert(
                                question, departments, int.Parse(activeyn));
                BindData();
                gvDepartmentType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
               
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string question = ((RadTextBox)e.Item.FindControl("txtQuestion")).Text;
                string activeyn = ((RadComboBox)e.Item.FindControl("ddlActiveEdit")).SelectedValue;
                RadCheckBoxList cblList = (RadCheckBoxList)e.Item.FindControl("chkDepartments");
                string departments = ",";
                foreach (ButtonListItem item in cblList.Items)
                {
                    if (item.Selected) departments = departments + item.Value.ToString() + ",";
                }
                if (!IsValidChecklist(question, departments, activeyn))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionChecklist.InspectionRegisterChecklistUpdate(
                                General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblQuestionIDEdit")).Text),
                                question, departments, int.Parse(activeyn));
                BindData();
                gvDepartmentType.Rebind();
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

    protected void gvDepartmentType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
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

            RadCheckBoxList cblList = (RadCheckBoxList)e.Item.FindControl("chkDepartments");
            if (cblList != null)
            {
                foreach (ButtonListItem item in cblList.Items)
                {
                    item.Selected = false;
                    if (!string.IsNullOrEmpty(drv["FLDDEPARTMENTSINVOLVED"].ToString()) && drv["FLDDEPARTMENTSINVOLVED"].ToString().Contains("," + item.Value.ToString() + ","))
                        item.Selected = true;
                }
            }

            RadCheckBoxList cblListItem = (RadCheckBoxList)e.Item.FindControl("chkDepartmentsItem");
            if (cblListItem != null)
            {
                foreach (ButtonListItem item in cblListItem.Items)
                {
                    item.Selected = false;
                    if (!string.IsNullOrEmpty(drv["FLDDEPARTMENTSINVOLVED"].ToString()) && drv["FLDDEPARTMENTSINVOLVED"].ToString().Contains("," + item.Value.ToString() + ","))
                        item.Selected = true;
                }
            }
            RadComboBox ddlActiveEdit = (RadComboBox)e.Item.FindControl("ddlActiveEdit");
            if (ddlActiveEdit != null)
            {
                if (drv["FLDACTIVEYNID"].ToString() == "1")
                    ddlActiveEdit.SelectedValue = "1";
                if (drv["FLDACTIVEYNID"].ToString() == "0")
                    ddlActiveEdit.SelectedValue = "0";
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

    protected void gvDepartmentType_SortCommand(object sender, GridSortCommandEventArgs e)
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
    
}
