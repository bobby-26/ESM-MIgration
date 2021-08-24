using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsProjectBillingMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsProjectBillingMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");

            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsProjectBillingItemFilter.aspx')", "Find", "search.png", "FIND");
            //toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../Accounts/AccountsProjectBillingMaster.aspx'); return false;", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            // MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vessel Specific", "VESSELSPECIFIC", ToolBarDirection.Right);
            toolbarmain.AddButton("General", "GENERAL", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Project Billing";
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            // MenuOrderFormMain.SetTrigger(pnlOrderForm);
            if (!IsPostBack)
            {
                Session["New"] = "N";


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["projectbillingid"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["projectbillingid1"] = null;

                if (Request.QueryString["projectbillingid"] != null)
                {
                    ViewState["projectbillingid"] = Request.QueryString["projectbillingid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid"];
                }
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid"];
            }
            if (CommandName.ToUpper().Equals("VESSELSPECIFIC") && ViewState["projectbillingid"] != null && ViewState["projectbillingid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsProjectBillingItemByVessel.aspx?projectbillingid=" + ViewState["projectbillingid"]);
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "Project Billing Group",
                                "Project Billing Name",
                                "Project Billing Description"

                              };

        string[] alColumns = {
                                "FLDQUICKNAME",
                                "FLDPROJECTBILLINGNAME",
                                "FLDBILLINGITEMDESCRIPTION"

                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedProjectBillingItem;
        ds = PhoenixAccountsProjectBilling.ProjectBillingList(
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtProjectBillingName")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucBillingUnit")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucProjectBillingGroups")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrencyCode")) : null
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                   , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProjectBilling.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedProjectBillingItem;
        ds = PhoenixAccountsProjectBilling.ProjectBillingList(
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtProjectBillingName")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucBillingUnit")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucProjectBillingGroups")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrencyCode")) : null
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , gvFormDetails.PageSize
                                                   , ref iRowCount, ref iTotalPageCount);

        string[] alCaptions = {
                                "Project Billing Group",
                                "Project Billing Name",
                                "Project Billing Description"

                              };

        string[] alColumns = {
                                "FLDQUICKNAME",
                                "FLDPROJECTBILLINGNAME",
                                "FLDBILLINGITEMDESCRIPTION"

                             };

        General.SetPrintOptions("gvVoucher", "Project Billing", alCaptions, alColumns, ds);

        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["Projectbillingid"] == null || nvc != null)
            {
                ViewState["projectbillingid1"] = ds.Tables[0].Rows[0]["FLDPROJECTBILLINGID"].ToString();
                // gvFormDetails.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null && ViewState["projectbillingid"] != null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid"].ToString();
            }
            else if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid1"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=";
            }
            DataTable dt = ds.Tables[0];
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        gvFormDetails.SelectedIndexes.Clear();
        if (ViewState["projectbillingid"] != null)
        {
            foreach (GridDataItem item in gvFormDetails.Items)
            {
                if (item.GetDataKeyValue("FLDPROJECTBILLINGID").ToString().Equals(ViewState["projectbillingid"].ToString()))
                {
                    gvFormDetails.SelectedIndexes.Add(item.ItemIndex);

                }
            }
        }
    }

    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }

    }

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        // if (Session["New"].ToString() == "Y")
        // {
        //     gvFormDetails.SelectedIndex = 0;
        //     Session["New"] = "N";
        //     BindPageURL(gvFormDetails.SelectedIndex);
        // }
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["projectbillingid"] = ((RadLabel)gvFormDetails.Items[rowindex].FindControl("lblProjectBillingid")).Text;

            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsProjectBillingItemGeneral.aspx?projectbillingid=" + ViewState["projectbillingid"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvFormDetails.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
}
