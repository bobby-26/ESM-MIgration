using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersDepartment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDepartment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDepartment.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersDepartment.AccessRights = this.ViewState;
            MenuRegistersDepartment.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDepartment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDDEPARTMENTCODE","FLDDEPARTMENTNAME","FLDDEPARTMENTTYPENAME"  };
        string[] alCaptions = {"Code", "Name","Type " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDepartment.DepartmentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtDepartmentCode.Text,txtSearch.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Department.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Department</h3></td>");
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
   
    protected void RegistersDepartment_TabStripCommand(object sender, EventArgs e)
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
                txtSearch.Text = "";
                txtDepartmentCode.Text = "";
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
        gvDepartment.SelectedIndexes.Clear();
        gvDepartment.EditIndexes.Clear();
        gvDepartment.DataSource = null;
        gvDepartment.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDDEPARTMENTCODE", "FLDDEPARTMENTNAME", "FLDDEPARTMENTTYPENAME" };
        string[] alCaptions = { "Code", "Name", "Type " };

        DataSet ds = PhoenixRegistersDepartment.DepartmentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtDepartmentCode.Text, txtSearch.Text, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDepartment.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDepartment", "Department", alCaptions, alColumns, ds);

        gvDepartment.DataSource = ds;
        gvDepartment.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvDepartment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtDepartmentCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDepartmentNameAdd")).Text
                    , ((UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeAdd")).SelectedDepartmentType))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertDepartment(
                    ((RadTextBox)e.Item.FindControl("txtDepartmentCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDepartmentNameAdd")).Text,
                    ((UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeAdd")).SelectedDepartmentType
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtDepartmentCodeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtDepartmentCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDepartmentNameEdit")).Text
                    , ((UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeEdit")).SelectedDepartmentType))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateDepartment(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblDepartmentCodeEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtDepartmentCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDepartmentNameEdit")).Text,
                     ((UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeEdit")).SelectedDepartmentType
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["DepartmentCode"] = ((RadLabel)e.Item.FindControl("lblDepartmentCode")).Text;
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
   protected void gvDepartment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item.IsInEditMode)
        {
            UserControlDepartmentType ucDeptartmentType = (UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeEdit");
            DataRowView drvDeptartmentType = (DataRowView)e.Item.DataItem;
            if (ucDeptartmentType != null) ucDeptartmentType.SelectedDepartmentType = DataBinder.Eval(e.Item.DataItem, "FLDDEPARTMENTTYPEID").ToString();
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
    private void InsertDepartment(string Departmentcode,string Departmentname,string DepartmentTypename )
    {
        PhoenixRegistersDepartment.InsertDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Departmentcode, Departmentname, DepartmentTypename);
    }

    private void UpdateDepartment(int departmentid,string Departmentcode, string Departmentname, string DepartmentTypename)
    {
        PhoenixRegistersDepartment.UpdateDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, departmentid, Departmentcode, Departmentname, DepartmentTypename);
        ucStatus.Text = "Department information updated successfully";
    }

    private bool IsValidDepartment(string Departmentcode,string Departmentname, string DepartmentType)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        RadGrid _gridView = gvDepartment;

        if (Departmentname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (Departmentcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (DepartmentType == null || !Int16.TryParse(DepartmentType, out result))
            ucError.ErrorMessage = "Type  is required.";

        return (!ucError.IsError);
    }

    private void DeleteDepartment(int Departmentcode)
    {
        PhoenixRegistersDepartment.DeleteDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Departmentcode);
    }

    protected void gvDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDepartment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDepartment_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteDepartment(Int32.Parse(ViewState["DepartmentCode"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
