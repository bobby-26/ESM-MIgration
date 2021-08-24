using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class RegistersPICtravelclaim : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
      
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Registers/RegistersPICtravelclaim.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvEmployee')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageButton("../Registers/RegistersPICtravelclaim.aspx", "Find", "search.png", "FIND");
        
        MenuUsageEmployee.AccessRights = this.ViewState;
        MenuUsageEmployee.MenuList = toolbargrid.Show();

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        menuemployee.Title = "Employee";
        menuemployee.AccessRights = this.ViewState;
        menuemployee.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvEmployee.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (Request.QueryString["page"] != null)
            {
                ViewState["PAGENUMBER"] = Request.QueryString["page"].ToString(); 
            }
        }
     //   BindData();
       
    }

    protected void MenuUsageEmployee_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
           
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }   

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENUMBER", "FLDDESCRIPTION","FLDUSERNAME","FLDEMAIL", "FLDACTIVEYN"};
        string[] alCaptions = { "Employee Code", "Employee Name", "User Name","Email ID", "Active" };

        int? iActiveYN = 0;
        if (chkActive.Checked)
            iActiveYN = 1;

        DataSet ds = PhoenixCommonRegisters.EmployeeSubAccountList(int.Parse(ViewState["PAGENUMBER"].ToString()), gvEmployee.PageSize
                        , ref iRowCount 
                        ,ref iTotalPageCount                        
                        ,txtSubAccountCodeSearch.Text 
                        ,txtDescriptionSearch.Text
                        , iActiveYN
        );

        gvEmployee.DataSource = ds;
        gvEmployee.VirtualItemCount = iRowCount;

       
        General.SetPrintOptions("gvEmployee", "Sub Account", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvEmployee_RowEditing(object sender, GridViewEditEventArgs de)
    //{

    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //    SetPageNavigator();

    //}

    //protected void gvEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixCommonRegisters.EmployeeSubAccountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                            , int.Parse(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblOfficeStaffId")).Text.ToString())
    //                            , int.Parse(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActive")).Checked == true ?"1":"0")
    //                            , General.GetNullableGuid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblSubAccountMapId")).Text.ToString())
    //                            );


    //        _gridView.EditIndex = -1;
    //        BindData();
            

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployee.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEmployee_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void gvEmployee_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                PhoenixCommonRegisters.EmployeeSubAccountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                              , int.Parse(((RadLabel)e.Item.FindControl("lblOfficeStaffId")).Text.ToString())
                              , int.Parse(((CheckBox)e.Item.FindControl("chkActive")).Checked == true ? "1" : "0")
                              , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblSubAccountMapId")).Text.ToString())
                              );
                Rebind();
            }
           

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                 GridDataItem item = (GridDataItem)e.Item;
                //  string[] strValues = new string[2];
                //strValues = e.CommandArgument.ToString().Split('^');
                //int nCurrentRow = Int32.Parse(strValues[0]);
                
                string strOfficestaffId = ((RadLabel)e.Item.FindControl("lblOfficeStaffId")).Text;
                Response.Redirect("../Registers/RegistersTravelClaimapprovalPIC.aspx?OFFICESTAFFID=" + strOfficestaffId + "&page=" + ViewState["PAGENUMBER"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("ORDER"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string strOfficestaffId = ((RadLabel)e.Item.FindControl("lblOfficeStaffId")).Text;
                Response.Redirect("../Registers/RegistersTravelClaimapprovalPIC.aspx?OFFICESTAFFID=" + strOfficestaffId + "&page=" + ViewState["PAGENUMBER"].ToString());
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
        gvEmployee.SelectedIndexes.Clear();
        gvEmployee.EditIndexes.Clear();
        gvEmployee.DataSource = null;
        gvEmployee.Rebind();
    }
    //protected void gvEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();

    //}


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENUMBER", "FLDDESCRIPTION", "FLDUSERNAME", "FLDEMAIL", "FLDACTIVEYN", "FLDPIC1", "FLDPIC2", "FLDPIC3", "FLDPIC4" };
        string[] alCaptions = { "Employee Code", "Employee Name", "User Name", "Email ID", "Active", "Level 1", "Level 2","Level 3", "Level 4"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? iActiveYN = 0;
        if (chkActive.Checked)
            iActiveYN = 1;


        DataSet ds = PhoenixRegistersOfficeStaff.EmployeeSubAccountListForTravelclaimExcel(int.Parse(ViewState["PAGENUMBER"].ToString()), -1
                        , ref iRowCount
                        , ref iTotalPageCount
                        , txtSubAccountCodeSearch.Text
                        , txtDescriptionSearch.Text
                        , iActiveYN
        );

        Response.AddHeader("Content-Disposition", "attachment; filename=Employee.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Account</h3></td>");
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

  

   
}
