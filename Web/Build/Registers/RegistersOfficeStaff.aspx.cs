using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOfficeStaff : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOfficeStaff.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOfficeStaff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersOfficeStaff.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersOfficeStaff.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegistersOfficeStaff.aspx", "Add", " <i class=\"fa fa-plus-circle\"></i>", "ADDOFFICESTAFF");            
            MenuRegistersOfficeStaff.AccessRights = this.ViewState;
            MenuRegistersOfficeStaff.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ddlactive.SelectedValue = "1";
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["page"] != null && Request.QueryString["page"].ToString() != "" ? Request.QueryString["page"].ToString() : "1");
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvOfficeStaff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
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
        string[] alColumns = { "FLDSALUTATION", "FLDOFFICEFIRSTNAME", "FLDOFFICESURNAME", "FLDEMPLOYEENUMBER", "FLDDESIGNATIONNAME", "FLDDATEOFBIRTH" };
        string[] alCaptions = { "Salutation", "First Name", "Last Name", "Employee/File Number", "Designation", "D.O.B" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersOfficeStaff.OfficeStaffSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
          General.GetNullableString(txtEmployeeNumber.Text),
          txtFirstName.Text,
          sortexpression, sortdirection,
          (int)ViewState["PAGENUMBER"],
          gvOfficeStaff.PageSize,
          ref iRowCount,
          ref iTotalPageCount
          , int.Parse(ddlactive.SelectedValue)
          , General.GetNullableInteger(ucZone.SelectedZone));

        Response.AddHeader("Content-Disposition", "attachment; filename=Office_Staff.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Office Staff</h3></td>");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void RegistersOfficeStaff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvOfficeStaff.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADDOFFICESTAFF"))
            {
                Response.Redirect("../Registers/RegistersOfficeStaffList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtEmployeeNumber.Text = "";
                txtFirstName.Text = "";
                ucZone.SelectedZone = "";
                ddlactive.SelectedValue = "";
                BindData();
                gvOfficeStaff.Rebind();
                ddlactive.SelectedValue = "1";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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
        string[] alColumns = { "FLDSALUTATION", "FLDOFFICEFIRSTNAME", "FLDOFFICESURNAME", "FLDEMPLOYEENUMBER", "FLDDESIGNATIONNAME", "FLDDATEOFBIRTH" };
        string[] alCaptions = { "Salutation", "First Name", "Last Name", "Employee/File Number", "Designation", "D.O.B" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersOfficeStaff.OfficeStaffSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableString(txtEmployeeNumber.Text),
            txtFirstName.Text,
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvOfficeStaff.PageSize,
            ref iRowCount,
            ref iTotalPageCount
           , int.Parse(ddlactive.SelectedValue)
          , General.GetNullableInteger(ucZone.SelectedZone));
        General.SetPrintOptions("gvOfficeStaff", "Office Staff", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOfficeStaff.DataSource = ds;
            gvOfficeStaff.VirtualItemCount = iRowCount;
        }
        else
        {
            gvOfficeStaff.DataSource = "";
        }
    }    
    private void DeleteOfficeStaff(int companyid)
    {
        PhoenixRegistersOfficeStaff.DeleteOfficeStaff(PhoenixSecurityContext.CurrentSecurityContext.UserCode, companyid);
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOfficeStaff.Rebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    
    protected void gvOfficeStaff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;       
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteOfficeStaff(Int32.Parse(((RadLabel)e.Item.FindControl("lblOfficeStaffid")).Text));
        }
        else if (e.CommandName.ToUpper().Equals("MAP"))
        {
            string officeid = ((RadLabel)e.Item.FindControl("lblOfficeStaffid")).Text;
            Response.Redirect("../Registers/RegistersOfficestaffapproval.aspx?OFFICESTAFFID=" + officeid + "&page=" + ViewState["PAGENUMBER"].ToString(), false);
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string officeid = ((RadLabel)e.Item.FindControl("lblOfficeStaffid")).Text;
            Response.Redirect("../Registers/RegistersOfficeStaffList.aspx?OFFICESTAFFID=" + officeid + "&page=" + ViewState["PAGENUMBER"].ToString(), false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvOfficeStaff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOfficeStaff.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOfficeStaff_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel l = (RadLabel)e.Item.FindControl("lblOfficeStaffid");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                //db.Attributes.Add("onclick", "return fnConfirmDelete()");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton eb1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb1 != null)            
                eb1.Visible = SessionUtil.CanAccess(this.ViewState, eb1.CommandName);
        }
    }
}
