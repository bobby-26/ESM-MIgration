using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceReturnRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitTravelAdvanceReturnRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelAdvanceReturnSearch.aspx'); return true;", "Find", "search.png", "FIND");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["VisitId"] = null;
                ViewState["TravelAdvanceId"] = null;

                ViewState["PAGENUMBER"] = 1;
                gvTravelAdvance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
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

    protected void gvTravelAdvance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
            if (cmdPost != null) cmdPost.Visible = SessionUtil.CanAccess(this.ViewState, cmdPost.CommandName);

            ImageButton cmdRepost = (ImageButton)e.Item.FindControl("cmdRepost");
            if (cmdRepost != null) cmdRepost.Visible = SessionUtil.CanAccess(this.ViewState, cmdRepost.CommandName);

            RadLabel lblVoucherId = (RadLabel)e.Item.FindControl("lblVoucherId");
            if (lblVoucherId != null && lblVoucherId.Text == "")
            {
                if (cmdEdit != null) cmdEdit.Visible = true;
            }
            else
            {
                if (cmdEdit != null) cmdEdit.Visible = false;
            }

            RadLabel lblAdvanceStatusCode = (RadLabel)e.Item.FindControl("lblAdvanceStatusCode");
            if (lblAdvanceStatusCode != null && lblAdvanceStatusCode.Text == "APP")
            {
                if (cmdPost != null) cmdPost.Visible = true;
                if (cmdRepost != null) cmdRepost.Visible = true;
            }
            else
            {
                if (cmdPost != null) cmdPost.Visible = false;
                if (cmdRepost != null) cmdRepost.Visible = false;
            }
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblBalance = (RadLabel)e.Item.FindControl("lblBalance");
            if (lblBalance != null && lblBalance.Text != "" && Convert.ToDecimal(lblBalance.Text) < 0)
            {
                lblBalance.Text = "(" + (Convert.ToDecimal(lblBalance.Text) * (-1)).ToString() + ")";
                e.Item.Cells[9].ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void gvTravelAdvance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                // BindPageURL(nCurrentRow);
                ViewState["VisitId"] = ((RadLabel)e.Item.FindControl("lblVisitId")).Text;
                ViewState["TravelAdvanceId"] = ((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text;

                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitTravelAdvanceReturn.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString();

                gvTravelAdvance.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
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

            string[] alColumns = { "FLDFORMNUMBER", "FLDTRAVELADVANCENUMBER", "FLDEMPLOYEEID", "FLDEMPLOYEENAME", "FLDCURRENCYCODE", "FLDREQUESTAMOUNT", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDRETURNDATE", "FLDBALANCE", "FLDCLAIMSUBMITTED", "FLDQUICKNAME" };
            string[] alCaptions = { "Visit Form No.", "Travel Advance Number", "Employee Id", "Employee Name", "Currency", "Requested Amount", "Payment Amount", "Return Amount", "Return Date", "Balance", "Claim Submitted", "Advance Status" };

            DataSet ds = new DataSet();
            NameValueCollection nvc = Filter.CurrentTravelAdvance;

            ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnList(
                    nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeName")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtAdvanceNumber")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtVisitNo")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtPurpose")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("chkCurrencyList")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("chkClaimStatus")) : null,
                    gvTravelAdvance.CurrentPageIndex + 1,
                    gvTravelAdvance.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    nvc != null ? General.GetNullableInteger(nvc.Get("ucAdvanceStatus")) : null);

            General.SetPrintOptions("gvTravelAdvance", "Travel Advance Return", alCaptions, alColumns, ds);

            gvTravelAdvance.DataSource = ds;
            gvTravelAdvance.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["TravelAdvanceId"] == null)
                {
                    ViewState["VisitId"] = dr["FLDVISITID"].ToString();
                    ViewState["TravelAdvanceId"] = dr["FLDTRAVELADVANCEID"].ToString();

                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitTravelAdvanceReturn.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitTravelAdvanceReturn.aspx?visitId=&TravelAdvanceId=";
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

    //private void SetRowSelection()
    //{
    //    gvTravelAdvance.SelectedIndex = -1;
    //    for (int i = 0; i < gvTravelAdvance.Rows.Count; i++)
    //    {
    //        if (gvTravelAdvance.DataKeys[i].Value.ToString().Equals(ViewState["TravelAdvanceId"].ToString()))
    //        {
    //            gvTravelAdvance.SelectedIndex = i;
    //        }
    //    }
    //    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitTravelAdvanceReturn.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString();
    //}

    private void BindPageURL(int rowindex)
    {
        try
        {
            if (rowindex != 0)
            {
                ViewState["VisitId"] = ((RadLabel)gvTravelAdvance.Items[rowindex].FindControl("lblVisitId")).Text;
                ViewState["TravelAdvanceId"] = ((RadLabel)gvTravelAdvance.Items[rowindex].FindControl("lblTravelAdvanceId")).Text;

                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitTravelAdvanceReturn.aspx?visitId=" + ViewState["VisitId"].ToString() + "&TravelAdvanceId=" + ViewState["TravelAdvanceId"].ToString();
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

        string[] alColumns = { "FLDFORMNUMBER", "FLDTRAVELADVANCENUMBER", "FLDEMPLOYEEID", "FLDEMPLOYEENAME", "FLDCURRENCYCODE", "FLDREQUESTAMOUNT", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDRETURNDATE", "FLDBALANCE", "FLDCLAIMSUBMITTED", "FLDQUICKNAME" };
        string[] alCaptions = { "Visit Form No.", "Travel Advance Number", "Employee Id", "Employee Name", "Currency", "Requested Amount", "Payment Amount", "Return Amount", "Return Date", "Balance", "Claim Submitted", "Advance Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        NameValueCollection nvc = Filter.CurrentTravelAdvance;

        ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnList(
                nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeName")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtAdvanceNumber")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtVisitNo")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtPurpose")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("chkCurrencyList")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("chkClaimStatus")) : null,
                gvTravelAdvance.CurrentPageIndex + 1,
                gvTravelAdvance.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                nvc != null ? General.GetNullableInteger(nvc.Get("ucAdvanceStatus")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelAdvanceReturn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance Return</h3></td>");
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

    protected void Rebind()
    {
        gvTravelAdvance.SelectedIndexes.Clear();
        gvTravelAdvance.EditIndexes.Clear();
        gvTravelAdvance.DataSource = null;
        gvTravelAdvance.Rebind();
    }

    protected void gvTravelAdvance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelAdvance.CurrentPageIndex + 1;
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvTravelAdvance.Rebind();
    }
}
