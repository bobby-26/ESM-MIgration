using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionSealIssuedtoVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealIssuedtoVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "VExcel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "VPRINT");
            MenuSealExport.AccessRights = this.ViewState;
            MenuSealExport.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealIssuedtoVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i> ", "SExcel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealNumber')", "Print Grid", "<i class=\"fas fa-print\"></i>", "SPRINT");
            MenuSealNumberExport.AccessRights = this.ViewState;
            MenuSealNumberExport.MenuList = toolbargrid.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["LOCATIONID"] = Request.QueryString["LOCATIOND"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTSELECTEDVESSELID"] = null;

                ViewState["PAGENUMBERSL"] = 1;

              
                gvVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSealNumber.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();

            //BindDataSL();
            //SetPageNavigatorSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VEXCEL"))
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
            string[] alColumns = { "FLDVESSELNAME", "FLDISSUEDQTY" };
            string[] alCaptions = { "Vessel Name", "Issued" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealIssuedSearch(new Guid(ViewState["LOCATIONID"].ToString())
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Vessel list", ds.Tables[0], alColumns, alCaptions, null, null);
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
            string[] alColumns = { "FLDVESSELNAME", "FLDISSUEDQTY" };
            string[] alCaptions = { "Vessel Name", "Issued" };

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealIssuedSearch(new Guid(ViewState["LOCATIONID"].ToString())
                           , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                           , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvVessel", "Vessel list", alCaptions, alColumns, ds);
            gvVessel.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {



                if (ViewState["CURRENTSELECTEDVESSELID"] == null)
                {
                    ViewState["CURRENTSELECTEDVESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                }
                //SetRowSelection();
            }


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVessel_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvVessel_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            BindPageURL(nCurrentRow);
    //           // SetRowSelection();
    //            BindDataSL();
    //           // SetPageNavigatorSL();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvVessel_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        BindPageURL(nCurrentRow);
        ViewState["PAGENUMBERSL"] = 1;
        BindDataSL();
        //SetPageNavigatorSL();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblVesselId = (RadLabel)gvVessel.Items[rowindex].FindControl("lblVesselId");
            if (lblVesselId != null)
            {
                ViewState["CURRENTSELECTEDVESSELID"] = lblVesselId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvVessel_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
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
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //Seal number grid

    private void BindDataSL()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Seal Number", "Issued Date" };

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealNumberIssuedSearch(new Guid(ViewState["LOCATIONID"].ToString())
                           , General.GetNullableInteger(ViewState["CURRENTSELECTEDVESSELID"].ToString())
                           , gvSealNumber.CurrentPageIndex + 1
                           , gvSealNumber.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSealNumber", "Seal Numbers", alCaptions, alColumns, ds);
            gvSealNumber.DataSource = ds;
            gvSealNumber.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTSL"] = iRowCount;
            ViewState["TOTALPAGECOUNTSL"] = iTotalPageCount;
            // SetPageNavigatorSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuSealNumberExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEXCEL"))
            {
                ShowExcelSL();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelSL()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Seal Number", "Issued Date" };

            if (ViewState["ROWCOUNTSL"] == null || Int32.Parse(ViewState["ROWCOUNTSL"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNTSL"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealInventory.SealNumberIssuedSearch(new Guid(ViewState["LOCATIONID"].ToString())
                           , General.GetNullableInteger(ViewState["CURRENTSELECTEDVESSELID"].ToString())
                           , Convert.ToInt16(ViewState["PAGENUMBERSL"].ToString())
                           , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Seal Numbers", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["SORTEXPRESSIONSL"] = se.SortExpression;

            if (ViewState["SORTDIRECTIONSL"] != null && ViewState["SORTDIRECTIONSL"].ToString() == "0")
                ViewState["SORTDIRECTIONSL"] = 1;
            else
                ViewState["SORTDIRECTIONSL"] = 0;
            BindDataSL();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSIONSL"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONSL"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTIONSL"] == null || ViewState["SORTDIRECTIONSL"].ToString() == "0")
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




    protected void gvVessel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
                // SetRowSelection();
                BindDataSL();
                // SetPageNavigatorSL();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvSealNumber_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataSL();
    }
}
