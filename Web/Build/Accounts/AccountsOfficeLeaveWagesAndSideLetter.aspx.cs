using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsOfficeLeaveWagesAndSideLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                //PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Finalized Portage Bill", "PORTAGEBILL");
                //toolbar.AddButton("Voucher", "VOUCHER");
                //MenuPB.AccessRights = this.ViewState;
                //MenuPB.MenuList = toolbar.Show();
                //MenuPB.SelectedMenuIndex = 1;

                ViewState["DATE"] = null;
                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["pno"]) ? 1 : int.Parse(Request.QueryString["pno"]);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            //string date = string.Empty;
            //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            //if (dce.CommandName.ToUpper().Equals("PORTAGEBILL"))
            //{
            //    Response.Redirect("AccountsOfficePortageBill.aspx", true);
            //}
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

            string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDOPENINGBALANCE", "FLDCLOSINGBALANCE" };
            string[] alCaptions = { "From Date", "To Date", "Opening Balance", "Closing Balance" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPB.PageSize, ref iRowCount, ref iTotalPageCount);
             gvPB.DataSource = dt;
            gvPB.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPB.SelectedIndexes.Clear();
        gvPB.EditIndexes.Clear();
        gvPB.DataSource = null;
        gvPB.Rebind();
    }
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            LinkButton Draft = (LinkButton)e.Item.FindControl("cmdDraft");
            if (Draft != null) Draft.Visible = SessionUtil.CanAccess(this.ViewState, Draft.CommandName);

        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
         
            if (e.CommandName.ToUpper().Equals("DRAFT"))
            {
                string pbid = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                Response.Redirect("AccountsOfficeLeaveAndSideLetter.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);
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
    //protected void gvPB_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        //if (e.CommandName.ToUpper().Equals("VIEW"))
    //        //{
    //            //string pbid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPbId")).Text;
    //            //string vslid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
    //            //Response.Redirect("AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);
    //        //}
    //        if (e.CommandName.ToUpper().Equals("DRAFT"))
    //        {
    //            string pbid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPbId")).Text;
    //            string vslid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
    //            Response.Redirect("AccountsOfficeLeaveAndSideLetter.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
  

   
}
