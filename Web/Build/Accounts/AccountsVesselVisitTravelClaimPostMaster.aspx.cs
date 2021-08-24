using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelClaimPostMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimPostMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTravelClaim')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimSearch.aspx'); return true;", "Find", "search.png", "FIND");
            MenuTravelClaim.AccessRights = this.ViewState;
            MenuTravelClaim.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                Session["New"] = "N";

                if (Request.QueryString["VisitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;

                if (Request.QueryString["TravelClaimId"] != "")
                    ViewState["TravelClaimId"] = Request.QueryString["TravelClaimId"];
                else
                    ViewState["TravelClaimId"] = null;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbar.AddButton("Voucher Posting", "POSTING", ToolBarDirection.Right);
            toolbar.AddButton("Travel Claim", "CLAIM", ToolBarDirection.Right);
            MenuTravelClaimMain.AccessRights = this.ViewState;
            MenuTravelClaimMain.MenuList = toolbar.Show();
            MenuTravelClaimMain.SelectedMenuIndex = 2;
           
            gvTravelClaim.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            //   BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelClaimMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("POSTING"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimPosting.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimHistory.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelClaim_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvTravelClaim.Rebind();
    }

    protected void gvTravelClaim_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvTravelClaim_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "ChangePageSize")
            {
                return;
            }
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                ViewState["VisitId"] = ((RadLabel)e.Item.FindControl("lblVisitId")).Text;
                ViewState["TravelClaimId"] = ((RadLabel)e.Item.FindControl("lblTravelClaimId")).Text;
            }
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


    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFORMNUMBER", "FLDEMPLOYEENAME", "FLDDEPARTMENTNAME", "FLDVESSELNAME", "FLDPORTNAME", "FLDPURPOSE", "FLDCLAIMSTATUS" };
            string[] alCaptions = { "Form Number", "Employee Name", "Department", "Vessel Name", "Port", "Purpose", "Claim Status" };

            DataSet ds = new DataSet();

            NameValueCollection nvc = Filter.CurrentTravelClaimPostingFilter;

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPostList("",
                  nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
                 (int)ViewState["PAGENUMBER"],
                  gvTravelClaim.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount,
                  nvc != null ? General.GetNullableString(nvc.Get("txtFormNumber")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null
                  );

            General.SetPrintOptions("gvTravelClaim", "Travel Claim", alCaptions, alColumns, ds);

            gvTravelClaim.DataSource = ds;
            gvTravelClaim.VirtualItemCount = iRowCount;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["VisitId"] == null)
                {
                    ViewState["VisitId"] = dr["FLDVISITID"].ToString();
                    ViewState["TravelClaimId"] = dr["FLDTRAVELCLAIMID"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //   ShowNoRecordsFound(dt, gvTravelClaim);
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNUMBER", "FLDEMPLOYEEID", "FLDEMPLOYEENAME", "FLDFLEETDESCRIPTION", "FLDVESSELNAME", "FLDPORTNAME", "FLDPURPOSE", "FLDCURRENCYCODE", "FLDCLAIMSTATUS" , "FLDCLAIMVOUCHERNO" };
        string[] alCaptions = { "Form Number", "Employee Id", "Employee Name", "Fleet", "Vessel Name", "Port", "Purpose", "Reimbursement Currency", "Claim Status" , "Claim Voucher Number" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        NameValueCollection nvc = Filter.CurrentTravelClaimPostingFilter;

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPostList("",
              nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
              (int)ViewState["PAGENUMBER"],
              gvTravelClaim.PageSize,
              ref iRowCount,
              ref iTotalPageCount,
              nvc != null ? General.GetNullableString(nvc.Get("txtFormNumber")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
              nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelClaim.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Claim</h3></td>");
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


    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblVisitId = ((RadLabel)gvTravelClaim.Items[rowindex].FindControl("lblVisitId"));
            RadLabel lblTravelClaimId = ((RadLabel)gvTravelClaim.Items[rowindex].FindControl("lblTravelClaimId"));

            if (lblVisitId != null)
                ViewState["VisitId"] = lblVisitId.Text;
            if (lblTravelClaimId != null)
                ViewState["TravelClaimId"] = lblTravelClaimId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvTravelClaim.SelectedIndexes.Clear();
        gvTravelClaim.EditIndexes.Clear();
        gvTravelClaim.DataSource = null;
        gvTravelClaim.Rebind();
    }

    protected void gvTravelClaim_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelClaim.CurrentPageIndex + 1;
        BindData();
    }
}

