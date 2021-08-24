using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegistersCrewInactiveCategory : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersCrewInactiveCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewInactiveCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCrewInactiveCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");        
        MenuRegistersInactiveCategory.AccessRights = this.ViewState;
        MenuRegistersInactiveCategory.MenuList = toolbar.Show();


        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvCrewInactiveCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINACTIVECATEGORYNAME" ,"FLDACTIVEYN"};
        string[] alCaptions = { "In-Active Category" , "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string InactiveCategorySearch = (txtInactiveCategory.Text == null) ? "" : txtInactiveCategory.Text.Trim();

        ds = PhoenixRegistersCrewInactiveCategory.InActiveCategorySearch(InactiveCategorySearch, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCrewInactiveCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        
        if (ds.Tables.Count > 0)
            General.ShowExcel("In-Active Category", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersInactiveCategory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvCrewInactiveCategory.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            txtInactiveCategory.Text = "";            
            BindData();
            gvCrewInactiveCategory.Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINACTIVECATEGORYNAME", "FLDACTIVEYN" };
        string[] alCaptions = { "In-Active Category", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string txtInactiveCategorySearch = (txtInactiveCategory.Text == null) ? "" : txtInactiveCategory.Text.Trim();

        DataSet ds = PhoenixRegistersCrewInactiveCategory.InActiveCategorySearch(txtInactiveCategorySearch, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCrewInactiveCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCrewInactiveCategory", "In-Active Category", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewInactiveCategory.DataSource = ds;
            gvCrewInactiveCategory.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCrewInactiveCategory.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewInactiveCategory.Rebind();
    }


    private void InsertInactiveCategory(string InactiveCategoryname, int activeyn, string shortname)
    {
        if (!IsValidInactiveCategory(InactiveCategoryname, shortname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCrewInactiveCategory.InsertInactiveCategory(
            InactiveCategoryname
            , activeyn
            ,shortname);
    }

    private void UpdateInactiveCategory(int inactivecategoryid, string InactiveCategoryname, int activeyn, string shortname)
    {
        if (!IsValidInactiveCategory(InactiveCategoryname, shortname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCrewInactiveCategory.UpdateInactiveCategory(
            inactivecategoryid
            , InactiveCategoryname
            , activeyn
            , shortname);
    }

    private bool IsValidInactiveCategory(string InactiveCategoryname, string Shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (InactiveCategoryname.Trim().Equals(""))
            ucError.ErrorMessage = "In-Active Category is required.";

        if (Shortname.Trim().Equals(""))
            ucError.ErrorMessage = "Short Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteInactiveCategory(int inactivecategoryid)
    {
        PhoenixRegistersCrewInactiveCategory.DeleteInactiveCategory(inactivecategoryid);
    }

   

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewInactiveCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;
          
          else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertInactiveCategory(
                    ((RadTextBox)e.Item.FindControl("txtInactiveCategoryAdd")).Text.Trim()
                    , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0
                    , ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text.Trim()
                );

                BindData();
                gvCrewInactiveCategory.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteInactiveCategory(Int32.Parse(((RadLabel)e.Item.FindControl("lblInactiveCategoryId")).Text));

                BindData();
                gvCrewInactiveCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string InactiveCategoryname = ((RadTextBox)e.Item.FindControl("txtInactiveCategoryEdit")).Text.Trim();
                string Shortname = ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text.Trim();
                if (!IsValidInactiveCategory(InactiveCategoryname, Shortname))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateInactiveCategory(
                      Int32.Parse(((RadLabel)e.Item.FindControl("lblInactiveCategoryIdEdit")).Text)
                      , InactiveCategoryname
                      , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true) ? 1 : 0
                      , Shortname
                  );
                ucStatus.Text = "Updated Successfully.";
                BindData();
                gvCrewInactiveCategory.Rebind();
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

    protected void gvCrewInactiveCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewInactiveCategory.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewInactiveCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
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

    protected void gvCrewInactiveCategory_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        gvCrewInactiveCategory.Rebind();
    }
}
