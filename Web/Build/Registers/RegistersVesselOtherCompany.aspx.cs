using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselOtherCompany : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselOtherCompany.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselOtherCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselOtherCompany.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselOtherCompanyList.aspx", "Add Training Schedule", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuRegistersVesselOtherCompany.AccessRights = this.ViewState;
            MenuRegistersVesselOtherCompany.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDDWT","FLDIMONUMBER"};
        string[] alCaptions = { "Company name", "Vessel name", "DWT","IMO Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersVesselOtherCompany.VesselOtherCompanySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            txtVesselName.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVesselOtherCompany.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=VesselOtherCompany.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Other Company</h3></td>");
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

    protected void RegistersVesselOtherCompany_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvVesselOtherCompany.Rebind();
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvVesselOtherCompany.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDDWT", "FLDIMONUMBER" };
        string[] alCaptions = { "Company Name", "Vessel Name", "DWT", "IMO Number" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersVesselOtherCompany.VesselOtherCompanySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , txtVesselName.Text, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvVesselOtherCompany.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvVesselOtherCompany", "Vessel Other Company", alCaptions, alColumns, ds);
       

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselOtherCompany.DataSource = ds;
            gvVesselOtherCompany.VirtualItemCount = iRowCount;
        }
        else
        {
            gvVesselOtherCompany.DataSource = "";
        }
    }
    private void DeleteVesselOtherCompany(int companyid)
    {
        PhoenixRegistersVesselOtherCompany.DeleteVesselOtherCompany(1, companyid);
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvVesselOtherCompany.Rebind();
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvVesselOtherCompany_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselOtherCompany.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvVesselOtherCompany_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;       
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteVesselOtherCompany(Int32.Parse(((RadLabel)e.Item.FindControl("lblCompanyid")).Text));
            BindData();
            gvVesselOtherCompany.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvVesselOtherCompany_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            RadLabel l = (RadLabel)e.Item.FindControl("lblCompanyid");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVesselOtherCompanyName");
            lb.Attributes.Add("onclick", "javascript:openNewWindow('VesselOtherCompany', '', '" + Session["sitepath"] + "/Registers/RegistersVesselOtherCompanyList.aspx?companyid=" + l.Text + "');return false;");
        }
    }

    protected void gvVesselOtherCompany_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        gvVesselOtherCompany.Rebind();
    }
}
