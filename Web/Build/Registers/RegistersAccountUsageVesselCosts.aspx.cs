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

public partial class RegistersAccountUsageVesselCosts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersAccountUsageVesselCosts.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('givVesselCosts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersAccountUsageVesselCosts.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

        MenuRegistersVesselAccount.AccessRights = this.ViewState;
        MenuRegistersVesselAccount.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            givVesselCosts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }    
        EditAccount();
        BindData();
    }
    protected void VesselAccountMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            givVesselCosts.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void givVesselCosts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    private void EditAccount()
    {
        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];

        txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
        txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
        txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
        ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNTCODE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Code", "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixCommonRegisters.SubAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                        , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                        , General.GetNullableInteger(ViewState["USAGE"].ToString())
                        , txtAccountCodeSearch.Text
                        , txtAccountDescSearch.Text
                        , sortexpression
                        , sortdirection
                        , givVesselCosts.CurrentPageIndex + 1
                        , givVesselCosts.PageSize
                        , ref iRowCount, ref iTotalPageCount);

        givVesselCosts.DataSource = ds;
        givVesselCosts.VirtualItemCount = iRowCount;

        General.SetPrintOptions("givVesselCosts", "Sub Account", alCaptions, alColumns, ds);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNTCODE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Code", "Description" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCommonRegisters.SubAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                        , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                        , General.GetNullableInteger(ViewState["USAGE"].ToString())
                        , txtAccountCodeSearch.Text
                        , txtAccountDescSearch.Text
                        , sortexpression
                        , sortdirection
                        , givVesselCosts.CurrentPageIndex + 1
                        , givVesselCosts.PageSize
                        , ref iRowCount, ref iTotalPageCount);       

        Response.AddHeader("Content-Disposition", "attachment; filename=UsageVesselCosts.xls");
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
    protected void givVesselCosts_ItemCommand(object sender, GridCommandEventArgs e)
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
}
