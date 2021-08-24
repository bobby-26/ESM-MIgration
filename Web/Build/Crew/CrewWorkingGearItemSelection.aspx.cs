using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class CrewWorkingGearItemSelection : PhoenixBasePage
{
    string empid = string.Empty;
    string vslid = string.Empty;
    string orderid = string.Empty;
    string crewplanid = string.Empty;
    string Neededid = string.Empty;
    string r = string.Empty;
    private string Orderback = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        empid = Request.QueryString["empid"];
        vslid = Request.QueryString["vesslid"];
        crewplanid = Request.QueryString["crewplanid"];
      
        ViewState["r"] = Request.QueryString["r"];
        if (ViewState["r"] != null)
            r = Request.QueryString["r"];
        if (Request.QueryString["Orderback"] != null && Request.QueryString["Orderback"] != "")
            ViewState["Orderback"] = Request.QueryString["Orderback"];
        if (ViewState["Orderback"] != null)
            Orderback = ViewState["Orderback"].ToString();

        if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
            Neededid = Request.QueryString["Neededid"];

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //  if (r == "1")
       if (Orderback != "1")
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);

        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        
        MenuStockItem.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtItemSearch.Text = Request.QueryString["txtname"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvWorkGearItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvWorkGearItem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                if (empid != null)
                {
                    Response.Redirect("CrewWorkGearNeededItem.aspx?empid=" + empid + "&vslid=" + vslid + "&crewplanid=" + crewplanid + "&Neededid=" + Neededid + "&r=" + r, false);
                }
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
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = new DataTable();

            dt = PhoenixCrewWorkingGearNeededItem.SearchWorkGearItemSelection(General.GetNullableGuid(crewplanid),
                                                                                 General.GetNullableGuid(Neededid),
                txtItemSearch.Text, General.GetNullableInteger(ucWorkingGearType.SelectedGearType),
                sortexpression, sortdirection,
                Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
               , gvWorkGearItem.PageSize
               , ref iRowCount
               , ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                Zonename.Text = dt.Rows[0]["FLDZONENAME"].ToString();
            }

            gvWorkGearItem.DataSource = dt;
            gvWorkGearItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvWorkGearItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    

    protected void gvWorkGearItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkGearItem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvWorkGearItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text;
            string lblgearid = ((RadLabel)e.Item.FindControl("lblgearidedit")).Text;

            if (General.GetNullableDecimal(quantity).HasValue)
            {
                PhoenixCrewWorkingGearNeededItem.NeededLineItemsAdd(General.GetNullableGuid(crewplanid), General.GetNullableGuid(Neededid), decimal.Parse(quantity), new Guid(lblgearid));

                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

            }
            BindData();
            gvWorkGearItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkGearItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
        }

    }

    protected void gvWorkGearItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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

    protected void gvWorkGearItem_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


}
