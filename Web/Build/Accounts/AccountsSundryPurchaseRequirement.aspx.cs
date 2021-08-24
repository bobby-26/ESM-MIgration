using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsSundryPurchaseRequirement : PhoenixBasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddImageLink("javascript:var save=document.getElementById('ifMoreInfo').contentDocument.getElementById('cmdHiddenSubmit1'); if(save!=null) save.click(); return false;" , "New", string.Empty, "NEW"); 
            //toolbarmain.AddImageLink("javascript:var save=document.getElementById('ifMoreInfo').contentDocument.getElementById('cmdHiddenSubmit2'); if(save!=null) save.click(); return false;" , "Save", string.Empty, "SAVE");

            //toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            //toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuStoreItemMain.Title = "Sundry Purchase Requirement";
            MenuStoreItemMain.AccessRights = this.ViewState;
            MenuStoreItemMain.MenuList = toolbarmain.Show();
           
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsSundryPurchaseRequirement.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvBondReq')", "Print Grid", "icon_print.png", "PRINT");            
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
           // MenuBondReq.SetTrigger(pnlComponent);

            if (!IsPostBack)
            {

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["REQID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvBondReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                string REQID = ((RadLabel)e.Item.FindControl("lblReqId")).Text;
                PhoenixAccountsSundryPurchase.ConfirmSundryPurchaseRequirement(new Guid(REQID.ToString()));
                ucstatus.Text = "Requirement Approved Successfully...";
                ucstatus.Visible = true;
                ViewState["REQID"] = null;
                BindData();

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
            string[] alColumns = { "FLDREQUIREMENTNO", "FLDDATE", "FLDDEPARTMENTNAME", "FLDSTOCKTYPE", "FLDVESSELNAME" };
            string[] alCaptions = { "Req.No", "Date", "Department", "Stock Type", "Vessel/Office" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaseRequirement(
                            null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Sundry Purchase Requirement", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREQUIREMENTNO", "FLDDATE", "FLDDEPARTMENTNAME", "FLDSTOCKTYPE", "FLDVESSELNAME" };
            string[] alCaptions = { "Req.No", "Date", "Department", "Stock Type", "Vessel/Office" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaseRequirement(
                            null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvBondReq.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvBondReq", "Sundry Purchase Requirement", alCaptions, alColumns, ds);

            gvBondReq.DataSource = ds;
            gvBondReq.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
               
                if (ViewState["REQID"] == null)
                {
                
                    ViewState["REQID"] = ds.Tables[0].Rows[0]["FLDREQUIREMENTID"].ToString();  
                }
                  if (ViewState["CURRENTTAB"] == null)
                    ViewState["CURRENTTAB"] ="../Accounts/AccountsSundryPurchaseRequirementGeneral.aspx";
               ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?REQID=" + ViewState["REQID"];                             
            }
            else
            {
             
             
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSundryPurchaseRequirementGeneral.aspx";
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBondReq.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvBondReq_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        
        gvBondReq.SelectedIndexes.Add(se.NewSelectedIndex);
        string REQID = ((RadLabel)gvBondReq.Items[se.NewSelectedIndex].FindControl("lblReqId")).Text;
        ViewState["REQID"] = REQID;
        BindData();
    }


    //protected void gvBondReq_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }           
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
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
            ViewState["REQID"] = null;
            BindData();           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvBondReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string REQID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReqId")).Text;
    //        PhoenixAccountsSundryPurchase.ConfirmSundryPurchaseRequirement(new Guid(REQID.ToString()));
    //        ucstatus.Text = "Requirement Approved Successfully...";
    //        ucstatus.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    ViewState["REQID"] = null;
    //    BindData();
    //}
}

