using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersAccountUsageEmployee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersAccountUsageEmployee.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvEmployee')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersAccountUsageEmployee.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        
        MenuUsageEmployee.AccessRights = this.ViewState;
        MenuUsageEmployee.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvEmployee.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuUsageEmployee_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvEmployee.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ACCOUNTS"))
            {
                Response.Redirect("../Registers/RegistersAccountMaster.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
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

        string[] alColumns = { "FLDEMPLOYEENUMBER", "FLDDESCRIPTION","FLDUSERNAME", "FLDACTIVEYN"};
        string[] alCaptions = { "Code", "Description","User name", "Active" };

        int? iActiveYN = 0;
        if (chkActive.Checked.Equals(true))
            iActiveYN = 1;

        DataSet ds = PhoenixCommonRegisters.EmployeeSubAccountList(gvEmployee.CurrentPageIndex + 1, gvEmployee.PageSize
                        ,ref iRowCount 
                        ,ref iTotalPageCount                        
                        ,txtSubAccountCodeSearch.Text 
                        ,txtDescriptionSearch.Text
                        , iActiveYN
        );

        gvEmployee.DataSource = ds;
        gvEmployee.VirtualItemCount = iRowCount;
         
        General.SetPrintOptions("gvEmployee", "Sub Account", alCaptions, alColumns, ds);
    }
    protected void gvEmployee_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                bool? Activeyn = ((RadCheckBox)_gridView.Items[nCurrentRow].FindControl("chkActive")).Checked;

                PhoenixCommonRegisters.EmployeeSubAccountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , int.Parse(((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblOfficeStaffId")).Text.ToString())
                                    , Activeyn.Equals(true) ? 1 : 0
                                    , General.GetNullableGuid(((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblSubAccountMapId")).Text.ToString())
                                    );
                gvEmployee.SelectedIndexes.Clear();
                gvEmployee.EditIndexes.Clear();
                gvEmployee.DataSource = null;
                gvEmployee.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEmployee_ItemCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
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

        string[] alColumns = { "FLDEMPLOYEENUMBER", "FLDDESCRIPTION", "FLDUSERNAME", "FLDACTIVEYN" };
        string[] alCaptions = { "Code", "Description", "User name", "Active" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //int? iActiveYN = 0;
        //if (chkActive.Checked)
        //    iActiveYN = 1;
        

        DataSet ds = PhoenixCommonRegisters.EmployeeSubAccountList(gvEmployee.CurrentPageIndex + 1, gvEmployee.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , txtSubAccountCodeSearch.Text
                        , txtDescriptionSearch.Text
                        , null 
        );

        Response.AddHeader("Content-Disposition", "attachment; filename=SubAccount.xls");
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
