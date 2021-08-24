using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMRDeckCargoItem : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRDeckCargoItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDeckCargoItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRDeckCargoItem.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersDeckCargoItem.AccessRights = this.ViewState;
            MenuRegistersDeckCargoItem.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvDeckCargoItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDDECKCARGOITEMCODE", "FLDDECKCARGOITEMNAME", "FLDDECKCARGOITEMUNITNAME" };
        string[] alCaptions = { "Short Code", "Description","Unit" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersDMRDeckCorgoItem.MDRDeckCargoItemSearch(txtDeckCargoItem.Text,
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvDeckCargoItem.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DMRDeckCargoItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Deck Cargo</h3></td>");
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

    protected void RegistersDeckCargoItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvDeckCargoItem.Rebind();
                
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDECKCARGOITEMCODE", "FLDDECKCARGOITEMNAME", "FLDDECKCARGOITEMUNITNAME" };
        string[] alCaptions = { "Short Code", "Description","Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMRDeckCorgoItem.MDRDeckCargoItemSearch(txtDeckCargoItem.Text, 
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvDeckCargoItem.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

        General.SetPrintOptions("gvDeckCargoItem", "Deck Cargo", alCaptions, alColumns, ds);

        gvDeckCargoItem.DataSource = ds;
        gvDeckCargoItem.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvDeckCargoItem_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvDeckCargoItem.SelectedIndex = -1;
    //    gvDeckCargoItem.EditIndex = -1;

    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}
    protected void gvDeckCargoItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDeckCargoItem(((RadTextBox)e.Item.FindControl("txtDeckCargoItemNameAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtDeckCargoItemCodeAdd")).Text,
                 ((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersDMRDeckCorgoItem.InserDeckCargoItemType(
                    ((RadTextBox)e.Item.FindControl("txtDeckCargoItemNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDeckCargoItemCodeAdd")).Text,
                    General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit));
                BindData();
                gvDeckCargoItem.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRDeckCorgoItem.DeleteDMRDeckCargoItemType(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDeckCargoItemID")).Text));
                BindData();
                gvDeckCargoItem.Rebind();
            }
           
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDeckCargoItem(((RadTextBox)e.Item.FindControl("txtDeckCargoItemNameEdit")).Text,
                             ((RadTextBox)e.Item.FindControl("txtDeckCargoItemIDEdit")).Text,
                             ((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersDMRDeckCorgoItem.UpdateDeckCargoItemType(
                         General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDeckCargoItemIDEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtDeckCargoItemIDEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtDeckCargoItemNameEdit")).Text,
                         General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit));
                         ucStatus.Text = "Deck Cargo Item information updated";
             
                BindData();
                gvDeckCargoItem.Rebind();
            }
                
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ((RadLabel)e.Item.FindControl("lbDeckCargoItem")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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
    protected void gvDeckCargoItem_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

   protected void gvDeckCargoItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            if (ucUnit != null)
                ucUnit.SelectedUnit = drv["FLDUNITID"].ToString();
        }
    }
   
    private bool IsValidDeckCargoItem(string DeckCargoItemName, string DeckCargoItemcode,string unit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (DeckCargoItemcode.Trim().Equals(""))
            ucError.ErrorMessage = "Deck Cargo Item Code is required.";

        if (DeckCargoItemName.Trim().Equals(""))
            ucError.ErrorMessage = "Deck Cargo Item Name is required.";

        if (General.GetNullableInteger(unit) == null)
            ucError.ErrorMessage = "Unit is required.";
        return (!ucError.IsError);
    }
    
        protected void gvDeckCargoItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {   
               try
               {
                   ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeckCargoItem.CurrentPageIndex + 1;
                   BindData();
               }
               catch (Exception ex)
               {
                   ucError.ErrorMessage = ex.Message;
                   ucError.Visible = true;
               }
        }   

}
