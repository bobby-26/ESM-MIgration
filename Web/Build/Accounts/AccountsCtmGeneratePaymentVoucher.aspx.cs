using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class AccountsCtmGeneratePaymentVoucher : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsCtmGeneratePaymentVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCTM')", "Print Grid", "icon_print.png", "PRINT");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();
            //MenuCTM.SetTrigger(pnlCTM);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate", "GENERATEPAYMENTVOUCHER", ToolBarDirection.Right);
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {


                ViewState["CTMID"] = null;
                ViewState["ACTIVEYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

          
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDVESSELNAME", "FLDETA", "FLDCOMPANYNAME", "FLDARRANGEDVIA", "FLDDELIVEREDBY", "FLDAMOUNTARRANGED", "FLDTOTALCHARGES", "FLDTOTAL" };
                string[] alCaptions = { "Vessel", "ETA", "Company", "Arranged Via", "Delivered By", "Arranged Amount", "Total Charges", "Remittance Amount" };

                DataSet ds = PhoenixAccountsCtm.CtmFinalizeSearch(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , sortexpression, sortdirection
                            , 1
                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("CTM Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
     
        try
        {
            if (CommandName.ToUpper().Equals("GENERATEPAYMENTVOUCHER"))
            {
                try
                {
                    //REMITTANCE
                    DataSet dsSupplier = PhoenixAccountsCtm.CtmGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 3); // 3 SUPPLIER
                    DataTable dtSpplier = dsSupplier.Tables[0];
                    if (dtSpplier.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtSpplier.Rows) // Loop over the rows.
                        {
                            PhoenixAccountsCtm.CtmPaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(row["FLDCOMPANYID"].ToString())
                                                                        , int.Parse(row["FLDADDRESSCODE"].ToString()), General.GetNullableInteger(row["FLDBANKID"].ToString()), 3, "REMITTACE", int.Parse(row["FLDCURRENCYID"].ToString()));
                        }
                    }
                    DataSet dsPortAgent = PhoenixAccountsCtm.CtmGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 4); // 4 PORT AGENT
                    DataTable dtPortAgent = dsPortAgent.Tables[0];
                    if (dtPortAgent.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPortAgent.Rows) // Loop over the rows.
                        {
                            PhoenixAccountsCtm.CtmPaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(row["FLDCOMPANYID"].ToString())
                                                                        , int.Parse(row["FLDADDRESSCODE"].ToString()), General.GetNullableInteger(row["FLDBANKID"].ToString()), 4, "REMITTACE", int.Parse(row["FLDCURRENCYID"].ToString()));
                        }
                    }
                    //HAND CARRY
                    DataSet dsSeafarer = PhoenixAccountsCtm.CtmGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1); // 1 SEAFARER
                    DataTable dtSeafarer = dsSeafarer.Tables[0];
                    if (dtSeafarer.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtSeafarer.Rows) // Loop over the rows.
                        {
                            PhoenixAccountsCtm.CtmPaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(row["FLDCOMPANYID"].ToString())
                                                                        , int.Parse(row["FLDADDRESSCODE"].ToString()), General.GetNullableInteger(""), 1, "CASH", int.Parse(row["FLDCURRENCYID"].ToString()));
                        }
                    }

                    DataSet dsOfficeUser = PhoenixAccountsCtm.CtmGroupSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 2); // 2 OFFICE USER
                    DataTable dtOfficeUser = dsOfficeUser.Tables[0];
                    if (dtOfficeUser.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtOfficeUser.Rows) // Loop over the rows.
                        {
                            PhoenixAccountsCtm.CtmPaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(row["FLDCOMPANYID"].ToString())
                                                                        , int.Parse(row["FLDADDRESSCODE"].ToString()), General.GetNullableInteger(""), 2, "CASH", int.Parse(row["FLDCURRENCYID"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Accounts/AccountsCtmGeneratePaymentVoucher.aspx");
                BindData();
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDVESSELNAME", "FLDETA", "FLDCOMPANYNAME", "FLDARRANGEDVIA", "FLDDELIVEREDBY", "FLDAMOUNTARRANGED", "FLDTOTALCHARGES", "FLDTOTAL" };
            string[] alCaptions = { "Vessel", "ETA", "Company", "Arranged Via", "Delivered By", "Arranged Amount", "Total Charges", "Remittance Amount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsCtm.CtmFinalizeSearch(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvCTM.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);

            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

         


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCTM.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

       }
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string CaptainCashid = (((RadLabel)e.Item.FindControl("lblCaptainCashId")).Text);
                PhoenixAccountsCtm.CtmFinalizeDelete(new Guid(CaptainCashid));
                Rebind();
             
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

        Response.Redirect("../Accounts/AccountsCtmGeneratePaymentVoucher.aspx");
    }

 
  
    //protected void gvCTM_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    GridView _gridView = sender as GridView;
    //    _gridView.SelectedIndex = se.NewSelectedIndex;
    //    string ctmid = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
    //    string activey = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblEditable")).Text;
    //    string peningstatus = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblStatus")).Text;

    //    ViewState["CAPTAINCASHID"] = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblCaptainCashId")).Text;

    //    ViewState["CTMID"] = ctmid;
    //    ViewState["ACTIVEYN"] = activey;

    //    if (peningstatus == "Pending")
    //    {
    //        ViewState["ACTIVEYN"] = "0";
    //    }

    //    BindData();
    //}
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["CTMID"] = null;
    //        BindData();
    //        for (int i = 0; i < gvCTM.DataKeyNames.Length; i++)
    //        {
    //            if (gvCTM.DataKeyNames[i] == ViewState["CTMID"].ToString())
    //            {
    //                gvCTM.SelectedIndex = i;
    //                break;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
   
  

  
    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        DataList dl = (DataList)MenuCTMMain.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            //if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsCtmRequestArrangementGeneral.aspx"))
    //            //{
    //            //    MenuCTMMain.SelectedMenuIndex = 0;
    //            //}
    //            //else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMBOW.aspx"))
    //            //{
    //            //    MenuCTMMain.SelectedMenuIndex = 1;
    //            //}
    //            //else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMCalculation.aspx"))
    //            //{
    //            //    MenuCTMMain.SelectedMenuIndex = 2;
    //            //}
    //            //else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMDenomination.aspx"))
    //            //{
    //            //    MenuCTMMain.SelectedMenuIndex = 3;
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["CTMID"] = null;
    //        BindData();
    //        for (int i = 0; i < gvCTM.DataKeyNames.Length; i++)
    //        {
    //            if (gvCTM.DataKeyNames[i] == ViewState["CTMID"].ToString())
    //            {
    //                gvCTM.SelectedIndex = i;
    //                break;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
}
