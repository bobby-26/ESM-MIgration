using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersOtherCompanyList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOtherCompanyList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersOtherCompanyList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersOtherCompany.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            
            MenuRegistersCompany.AccessRights = this.ViewState;
            MenuRegistersCompany.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                dgCompany.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                chkActive.Checked = true;
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

        string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDEXPIRYDATE" };
        string[] alCaptions = { "Company Name", "Licence No.", "Expiry Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strCompanySearch = (txtSearchCompany.Text == null) ? "" : txtSearchCompany.Text;

        ds = PhoenixRegistersOtherCompany.OtherCompanySearch(
            strCompanySearch,
            chkActive.Checked == true ? 1 : 0,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            dgCompany.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=OtherCompany.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Other Company</h3></td>");
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

    protected void RegistersCompany_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            dgCompany.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        dgCompany.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string strCompanySearch = (txtSearchCompany.Text == null) ? "" : txtSearchCompany.Text;

        DataSet ds = PhoenixRegistersOtherCompany.OtherCompanySearch(
              strCompanySearch
              , chkActive.Checked == true ? 1 : 0
            , sortexpression
            , sortdirection
            , int.Parse(ViewState["PAGENUMBER"].ToString())
            , dgCompany.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            );

        string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDEXPIRYDATE" };
        string[] alCaptions = { "Company Name", "Licence No.", "Expiry Date" };

        General.SetPrintOptions("gvCompany", "Company", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            dgCompany.DataSource = ds;
            dgCompany.VirtualItemCount = iRowCount;
        }
        else
        {
            dgCompany.DataSource = "";
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        dgCompany.Rebind();
    }
    private void DeleteCompany(int CompanyId)
    {
        PhoenixRegistersOtherCompany.DeleteOtherCompany(0, CompanyId);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void dgCompany_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCompany(Int32.Parse(((RadLabel)e.Item.FindControl("lblCompanyId")).Text));
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

    protected void dgCompany_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgCompany.CurrentPageIndex + 1;
        BindData();
    }
    protected void dgCompany_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel l = (RadLabel)e.Item.FindControl("lblCompanyId");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                edit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersOtherCompany.aspx?CompanyId=" + l.Text + "');return false;");
            }
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCompany");
            if (lb != null)
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersOtherCompany.aspx?CompanyId=" + l.Text + "');return false;");
        }
    }

    protected void dgCompany_SortCommand(object sender, GridSortCommandEventArgs e)
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
            
            