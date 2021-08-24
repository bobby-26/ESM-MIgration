using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Registers_RegistersBank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersBank.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBank.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBank.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBank.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            MenuRegistersBank.AccessRights = this.ViewState;
            MenuRegistersBank.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvBank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["PageNo"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PageNo"].ToString());
                }
                else
                {
                    ViewState["PAGENUMBER"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRegistersBank_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../Registers/RegistersBankAdd.aspx?id=" + "&PageNo=" + ViewState["PAGENUMBER"], true);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtname.Text = "";
                txtShortcode.Text = "";
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
        gvBank.SelectedIndexes.Clear();
        gvBank.EditIndexes.Clear();
        gvBank.DataSource = null;
        gvBank.Rebind();
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDSHORTCODE", "FLDACCOUNTNODIGIT", "FLDACCOUNTNOPATTERN", "FLDALPHANUMERICYN", "FLDISACTIVE" };
        string[] alCaptions = { "Name", "Short Name", "A/C No. Digits", "A/C No Format", "Allow character YN", "Active YN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersBank.BankSearch(General.GetNullableString(txtname.Text)
                                                    , General.GetNullableString(txtShortcode.Text)
                                                    , sortexpression
                                                    , sortdirection
                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , gvBank.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount);

        General.SetPrintOptions("gvBank", "Bank", alCaptions, alColumns, ds);

        gvBank.DataSource = ds;
        gvBank.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvBank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string DTKey = ((RadLabel)e.Item.FindControl("lblDTkey")).Text;
                Response.Redirect("../Registers/RegistersBankAdd.aspx?id=" + DTKey.ToString() + "&PageNo=" + ViewState["PAGENUMBER"], true);
            }
            if (e.CommandName == "Page" )
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDSHORTCODE", "FLDACCOUNTNODIGIT", "FLDACCOUNTNOPATTERN", "FLDALPHANUMERICYN", "FLDISACTIVE" };
        string[] alCaptions = { "Name", "Short Name", "A/C No Digits", "A/C No. Format", "Allow character YN", "Active YN" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersBank.BankSearch(General.GetNullableString(txtname.Text)
                                                  , General.GetNullableString(txtShortcode.Text)
                                                  , sortexpression
                                                  , sortdirection
                                                  , 1
                                                  , iRowCount
                                                  , ref iRowCount
                                                  , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Bank.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank </h3></td>");
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
    protected void gvBank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = (ViewState["PAGENUMBER"] != null) ? ViewState["PAGENUMBER"] : (gvBank.CurrentPageIndex + 1);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBank_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            
        }
    }
    protected void gvBank_SortCommand(object source, GridSortCommandEventArgs e)
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