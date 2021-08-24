using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersContractingCompany : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersContractingCompany.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvContractingCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersContractingCompany.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");        
        MenuRegistersContractParty.AccessRights = this.ViewState;
        MenuRegistersContractParty.MenuList = toolbar.Show();        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvContractingCompany.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPANYNAME" , "FLDDESCRIPTION1" };
        string[] alCaptions = { "Code", "Contracting Party", "Report Header" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersContractCompany.ContractingCompanySearch("", "", sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvContractingCompany.PageSize, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Contracting Party", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void MenuRegistersContractParty_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvContractingCompany.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();           
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Registers/RegistersContractingCompanyAdd.aspx", false);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPANYNAME", "FLDDESCRIPTION1" };
        string[] alCaptions = { "Code", "Contracting Party", "Report Header" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        DataSet ds = PhoenixRegistersContractCompany.ContractingCompanySearch("", "", sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvContractingCompany.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvContractingCompany", "Contracting Party", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvContractingCompany.DataSource = ds;
            gvContractingCompany.VirtualItemCount = iRowCount;
        }
        else
        {
            gvContractingCompany.DataSource = "";
        }
    }
    private void DeleteContractParty(int companyid)
    {
        PhoenixRegistersContractCompany.DeleteContractCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode, companyid);
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvContractingCompany_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel companyid = (RadLabel)e.Item.FindControl("lblCompanyID");
                Response.Redirect("../Registers/RegistersContractingCompanyAdd.aspx?companyid=" + companyid.Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteContractParty(Int32.Parse(((RadLabel)e.Item.FindControl("lblCompanyID")).Text));
                BindData();
                gvContractingCompany.Rebind();
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

    protected void gvContractingCompany_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
    }

    protected void gvContractingCompany_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContractingCompany.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvContractingCompany_SortCommand(object sender, GridSortCommandEventArgs e)
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
