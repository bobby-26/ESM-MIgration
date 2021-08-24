using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersRegionCountry : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersRegionCountry.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvregioncountry')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuRegionCountry.AccessRights = this.ViewState;
        MenuRegionCountry.MenuList = toolbar.Show();
          
        if (!IsPostBack)
        {
            if (Request.QueryString["regionid"] != null)
                ViewState["region"] = Request.QueryString["regionid"].ToString();
        }
        bindata();
    }
    protected void RegionCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOUNTRYNAME" };
        string[] alCaptions = { "Country" };

        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["region"].ToString()));

        General.ShowExcel("Region", dt, alColumns, alCaptions, null, null);
    }


  
    private void bindata()
    {
        string[] alColumns = { "FLDCOUNTRYNAME" };
        string[] alCaptions = { "Country" };
        
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["region"].ToString()));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvregioncountry", "Region", alCaptions, alColumns, ds);
        gvregioncountry.DataSource = ds;
        
        

    }
 

    private bool IsValidCountry(string country)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(country).HasValue)
            ucError.ErrorMessage = "Country  is required";


        return (!ucError.IsError);

    }


    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();
        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }


    protected void gvregioncountry_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            
            bindata();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvregioncountry_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void gvregioncountry_ItemCommand(object sender, GridCommandEventArgs e)
    {
      
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            string country = ((UserControlCountry)e.Item.FindControl("ddlcountrylist")).SelectedCountry;
            if (!IsValidCountry(country))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersRegion.UpdateRegionCountry(General.GetNullableInteger(country)
                                          , General.GetNullableInteger(ViewState["region"].ToString()));
            bindata();
            gvregioncountry.Rebind();
        }
        if (e.CommandName.ToUpper().Equals("DEL"))
        {
            PhoenixRegistersRegion.DeleteRegionCountry(Int32.Parse(((RadLabel)e.Item.FindControl("lblCountrycode")).Text));
            bindata();
            gvregioncountry.Rebind();
        }
    }
}
