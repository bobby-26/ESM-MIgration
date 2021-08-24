using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionSealInventory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealInventory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealInventory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
      
            MenuSeal.AccessRights = this.ViewState;
            MenuSeal.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["PAGENUMBER"] = 1;
              
                gvSealInventory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSeal_TabStripCommand(object sender, EventArgs e)
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALTYPENAME", "FLDRECEIVED", "FLDISSUED", "FLDINUSE", "FLDSCRAPPED", "FLDROB" };
            string[] alCaptions = { "Seal Type", "Received", "Issued", "In Use", "Disposed", "ROB" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionSealInventory.SealROBSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableInteger(ucSealType.SelectedQuick), null,
                                Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                                iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Stock Check of seals", ds.Tables[0], alColumns, alCaptions, null, "");
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
            string[] alColumns = { "FLDSEALTYPENAME", "FLDRECEIVED", "FLDISSUED", "FLDINUSE", "FLDSCRAPPED", "FLDROB" };
            string[] alCaptions = { "Seal Type", "Received", "Issued", "In Use", "Disposed", "ROB" };

            DataSet ds = PhoenixInspectionSealInventory.SealROBSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableInteger(ucSealType.SelectedQuick), null,
                                int.Parse(ViewState["PAGENUMBER"].ToString()),
                                gvSealInventory.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSealInventory", "Stock Check of seals", alCaptions, alColumns, ds);
            gvSealInventory.DataSource = ds;
            gvSealInventory.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealInventory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
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
        try
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvSealInventory.Rebind();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealInventory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealInventory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucSealType_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvSealInventory.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealInventory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
