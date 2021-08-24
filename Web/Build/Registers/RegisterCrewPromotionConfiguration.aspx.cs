using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;

public partial class RegisterCrewPromotionConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Registers/RegisterCrewPromotionConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvPromotion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("../Registers/RegisterCrewPromotionConfiguration.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Registers/RegisterCrewPromotionConfiguration.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("../Registers/RegisterCrewPromotionConfiguration.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            CrewPromotionMenu.AccessRights = this.ViewState;
            CrewPromotionMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPromotion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void CrewPromotionMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegisterCrewPromotionConfigurationEdit.aspx"+ "',false,800,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPromotion.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ucRank.SelectedRank = "";
                BindData();
                gvPromotion.Rebind();
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

        string[] alColumns = { "FLDROWNUMBER", "FLDRANKFROMNAME","FLDRANKTONAME" };
        string[] alCaptions = { "S.No", "Rank From","Rank To" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationSearch(General.GetNullableInteger(ucRank.SelectedRank)
                                                                   ,sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount                                                                 
                                                               );

        General.ShowExcel("Promotion Configuration", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDRANKFROMNAME", "FLDRANKTONAME" };
        string[] alCaptions = { "S.No", "Rank From", "Rank To" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationSearch(General.GetNullableInteger(ucRank.SelectedRank)
                                                                  , sortexpression, sortdirection
                                                                 , (int)ViewState["PAGENUMBER"], gvPromotion.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvPromotion", "Promotion Configuration", alCaptions, alColumns, ds);

            gvPromotion.DataSource = dt;
            gvPromotion.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPromotion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPromotion.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPromotion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            
            else if (e.CommandName.ToUpper().Equals("NAVIGATEEDIT"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblId")).Text;
                
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegisterCrewPromotionConfigurationEdit.aspx?id=" + id + "',false,800,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblId")).Text;
                ViewState["ID"] = id;

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegisterCrewPromotionCopy.aspx?ID=" + ViewState["ID"].ToString() + "',false,380,235);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

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
    
    protected void gvPromotion_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string id = ((RadLabel)e.Item.FindControl("lblId")).Text;
        PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationDelete(new Guid(id));

        BindData();
        gvPromotion.Rebind();
    }

    protected void gvPromotion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName))
                    ed.Visible = false;
            }

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, del.CommandName))
                    del.Visible = false;
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event)");
            }

            LinkButton copy = (LinkButton)e.Item.FindControl("cmdCopy");
            if (copy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, copy.CommandName))
                    copy.Visible = false;
            }


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

    protected void gvPromotion_SortCommand(object sender, GridSortCommandEventArgs e)
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
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

}