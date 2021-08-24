using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterCrewConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegisterCrewConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegisterCrewConfiguration.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuRegistersCrew.AccessRights = this.ViewState;
        MenuRegistersCrew.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }


    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDCODE", "FLDDESCRIPTION", "FLDACTIVEYESNO" };
            string[] alCaptions = { "Code", "Description", "Active" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            ds = PhoenixRegisterCrewConfiguration.CrewConfigurationSearch(null, null, sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Crew Configuration", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuRegistersCrew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCrew.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                BindData();
                gvCrew.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Registers/RegisterCrewConfigurationAdd.aspx" + "',false,500,350);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCODE", "FLDDESCRIPTION", "FLDACTIVEYESNO" };
            string[] alCaptions = { "Code", "Description", "Active" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegisterCrewConfiguration.CrewConfigurationSearch(null, null, sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvCrew", "Crew Configuration", alCaptions, alColumns, ds);

            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;


            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegisterCrewConfiguration.DeleteCrewConfig(new Guid(((RadLabel)e.Item.FindControl("lblid")).Text));
                BindData();
                gvCrew.Rebind();
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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
        BindData();

    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblid = (RadLabel)e.Item.FindControl("lblid");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegisterCrewConfigurationAdd.aspx?Id=" + lblid.Text + "',false,500,350); return false;");
            }

        }
    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCrew.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}