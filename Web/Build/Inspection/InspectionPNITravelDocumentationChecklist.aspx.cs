using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class InspectionPNITravelDocumentationChecklist : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPNITravelDocumentationChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvQuick.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDSORTORDER", "FLDSORTORDERNAME", "FLDPART", "FLDDEPARTMENTNAME", "FLDNAME", "FLDACTIVE","FLDSHORTCODE" };
        string[] alCaptions = { "Sort Order", "Sort Order Name", "Part", "Department", "Item", "Active", "Short Code" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPNI.PNITravelCheckListList(General.GetNullableInteger(ucDepartment.SelectedDepartment),
                    (int)ViewState["PAGENUMBER"],
                    gvQuick.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"P&IDocumentation.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>P&I Documentation</h3></td>");
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

    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvQuick.Rebind();
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
        string[] alColumns = { "FLDSORTORDER", "FLDSORTORDERNAME", "FLDPART", "FLDDEPARTMENTNAME", "FLDNAME", "FLDACTIVE", "FLDSHORTCODE" };
        string[] alCaptions = { "Sort Order", "Sort Order Name", "Part", "Department", "Item", "Active", "Short Code" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPNI.PNITravelCheckListList(General.GetNullableInteger(ucDepartment.SelectedDepartment),
                    (int)ViewState["PAGENUMBER"],
                    gvQuick.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvQuick", "P&I Documentation", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuick.DataSource = ds;
            gvQuick.VirtualItemCount = iRowCount;
        }
        else
        {
            gvQuick.DataSource = "";
        }
    }
    private bool IsValidQuick(string name, string department, string sortorder, string part, int? active)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (name.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Item is required.";
        }
        if (General.GetNullableInteger(department) == null)
            ucError.ErrorMessage = "Department is required.";

        if (General.GetNullableInteger(sortorder) == null)
            ucError.ErrorMessage = "Valid SortOrder is required.";


        if (part.Contains("--Select--"))
            ucError.ErrorMessage = "Part is required.";

        if (active == null)
            ucError.ErrorMessage = "Active Yes or No required.";


        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtItemNameAdd")).Text
                                    , ((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd")).SelectedDepartment
                                    , ((RadTextBox)e.Item.FindControl("txtSortOrderAdd")).Text
                                    , ((RadDropDownList)e.Item.FindControl("ddlPartAdd")).SelectedItem.ToString()
                                    , ((RadCheckBox)e.Item.FindControl("cbActiveAdd")).Checked == true ? 1 : 0))
                {
                    ucError.Visible = true;
                    return;
                }
                string deptName="";
                DataSet ds = PhoenixRegistersDepartment.EditDepartment(General.GetNullableInteger(((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd")).SelectedDepartment).Value);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    deptName = ds.Tables[0].Rows[0]["FLDDEPARTMENTNAME"].ToString();
                PhoenixInspectionPNI.PNITravelCheckListInsert(int.Parse(((RadTextBox)e.Item.FindControl("txtSortOrderAdd")).Text)
                                                             , ((RadTextBox)e.Item.FindControl("txtSOrtOrderNameAdd")).Text
                                                             , (((RadDropDownList)e.Item.FindControl("ddlPartAdd")).SelectedItem).ToString()
                                                             , int.Parse(((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd")).SelectedDepartment)
                                                             , deptName
                                                              , ((RadTextBox)e.Item.FindControl("txtItemNameAdd")).Text
                                                             , ((RadCheckBox)e.Item.FindControl("cbActiveAdd")).Checked == true ? 1 : 0
                                                             , ((RadTextBox)e.Item.FindControl("txtshortcodenameadd")).Text
                                                             );
                BindData();
                gvQuick.Rebind();
                //((RadTextBox)_gridView.FooterRow.FindControl("txtItemNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionPNI.PNITravelCheckListDelete(new Guid(((RadLabel)e.Item.FindControl("lblId")).Text.ToString()));
                BindData();
                gvQuick.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtItemNameEdit")).Text
                                    , ((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).SelectedDepartment
                                    , ((RadTextBox)e.Item.FindControl("txtSortOrderEdit")).Text
                                    , ((RadLabel)e.Item.FindControl("lblPartIdedit")).Text
                                    , ((RadCheckBox)e.Item.FindControl("cbActiveEdit")).Checked == true ? 1 : 0))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPNI.PNITravelCheckListUpdate(new Guid(((RadLabel)e.Item.FindControl("lblIdEdit")).Text.ToString()),
                                                               int.Parse(((RadTextBox)e.Item.FindControl("txtSortOrderEdit")).Text),
                                                              ((RadTextBox)e.Item.FindControl("txtSortOrderNameEdit")).Text,
                                                              ((RadTextBox)e.Item.FindControl("txtItemNameEdit")).Text,
                                                             ((RadCheckBox)e.Item.FindControl("cbActiveEdit")).Checked == true ? 1 : 0,
                                                              General.GetNullableInteger(((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).SelectedDepartment),
                                                              General.GetNullableString(((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).SelectedDepartmentName)
                                                               , ((RadTextBox)e.Item.FindControl("txtshortcodenameedit")).Text
                                                               , ((RadDropDownList)e.Item.FindControl("ddlPartEdit")).SelectedItem.Text
                                                             );
                BindData();
                gvQuick.Rebind();
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
            e.Canceled = true;
        }
    }

    protected void gvQuick_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuick.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvQuick_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


            LinkButton status = (LinkButton)e.Item.FindControl("cblActiveEdit");
            RadDropDownList ddlPartEdit = (RadDropDownList)e.Item.FindControl("ddlPartEdit");
            if (status != null)
            {
                // status.SelectedValue = ((Label)e.Row.FindControl("lblActiveEdit")).Text;
            }
            if (ddlPartEdit != null)
            {

                ddlPartEdit.SelectedItem.Text = ((RadLabel)e.Item.FindControl("lblPartIdedit")).Text;
            }
            UserControlDepartment ucDepartment = ((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit"));
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDepartment != null) ucDepartment.SelectedDepartment = drv["FLDDEPARTMENTID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlDepartment ucDepartment = ((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd"));
            if (ucDepartment != null)
                ucDepartment.DepartmentList = PhoenixRegistersDepartment.Listdepartment(1, null);
        }
    }
}
   
