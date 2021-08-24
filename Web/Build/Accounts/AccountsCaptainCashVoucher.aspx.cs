using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsCaptainCashVoucher : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbar.AddButton("Log", "LOG", ToolBarDirection.Right);
            toolbar.AddButton("Captain Cash", "CAPTAINCASH", ToolBarDirection.Right);
            toolbar.AddButton("D11", "D11", ToolBarDirection.Right);
            toolbar.AddButton("View Draft", "VIEWDRAFT", ToolBarDirection.Right);
            toolbar.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbar.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            MenuReportsFilter.SelectedMenuIndex = 4;
            if (Request.QueryString["balanceid"] != null && Request.QueryString["balanceid"] != string.Empty)
                ViewState["balanceid"] = Request.QueryString["balanceid"];
            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
                ViewState["vesselid"] = Request.QueryString["vesselid"];
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Accounts/AccountsCaptainCashVoucher.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageLink("javascript:CallPrint('gvCaptainPettyCash')", "Print Grid", "icon_print.png", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddImageButton("../Accounts/AccountsCaptainCashVoucher.aspx", "Export to Excel", "icon_xls.png", "EXCEL1");
            toolbar2.AddImageLink("javascript:CallPrint('gvCaptainCash')", "Print Grid", "icon_print.png", "PRINT1");
            MenuShowExcel1.AccessRights = this.ViewState;
            MenuShowExcel1.MenuList = toolbar2.Show();
            if (!IsPostBack)
            {
                string vesselid, balanceid;

                if (Request.QueryString["vesselid"] != null)
                    vesselid = Request.QueryString["vesselid"].ToString();
                else
                    vesselid = "";

                if (Request.QueryString["balanceid"] != null)
                    balanceid = Request.QueryString["balanceid"].ToString();
                else
                    balanceid = "";
                DataSet ds = PhoenixAccountsCaptainCash.OfficeCaptainCashEdit(int.Parse(vesselid), new Guid(balanceid));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    // ucTitle.Text = dr["FLDVESSELNAME"].ToString() + " - Period Of  " + dr["FLDSDATE"].ToString() + " to " + dr["FLDEDATE"].ToString();

                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PRINCIPAL"] = null;
                ViewState["PAGENUMBER2"] = 1;
                ViewState["SORTEXPRESSION2"] = null;
                ViewState["SORTDIRECTION2"] = null;
                gvCaptainPettyCash.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCaptainCash.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvCaptainPettyCash.SelectedIndexes.Clear();
        gvCaptainPettyCash.EditIndexes.Clear();
        gvCaptainPettyCash.DataSource = null;
        gvCaptainPettyCash.Rebind();
    }

    protected void Rebind2()
    {
        gvCaptainCash.SelectedIndexes.Clear();
        gvCaptainCash.EditIndexes.Clear();
        gvCaptainCash.DataSource = null;
        gvCaptainCash.Rebind();
    }

    protected void ShowExcel_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                ShowExcel1();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCash.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashVoucher.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("VIEWDRAFT"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashDraft.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("D11"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=D11");
            }
            if (CommandName.ToUpper().Equals("CAPTAINCASH"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=CAPTAINCASH");
            }
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=LOG");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDates(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following information";

        if (General.GetNullableDateTime(fromdate) == null)
            ucError.ErrorMessage = "From date is required";
        if (General.GetNullableDateTime(todate) == null)
            ucError.ErrorMessage = "To date is required";
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) > 0)
            ucError.ErrorMessage = "To date should be later than or equal to From date";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
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

        string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDBUDGETCODE", "FLDOWNERBUDGETNAME", "FLDAMOUNT", "FLDPAYMENTRECEIPT" };
        string[] alCaptions = { "Port", "Date", "Purpose", "Budget Code", "Owner Budget Code", "Amount (USD)", "Type" };


        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashSearch(General.GetNullableInteger(ViewState["vesselid"].ToString()), General.GetNullableGuid(ViewState["balanceid"].ToString()), 1
                                                                , sortexpression, sortdirection
                                                                , 1
                                                                , iRowCount, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CaptainPettyCash.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>EXECUTIVE SHIP MANAGEMENT</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void ShowExcel1()
    {
        DataSet ds = new DataSet();
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

        string[] alColumns = { "FLDDATE", "FLDLOGTYPENAME", "FLDWAGENAME", "FLDBUDGETCODE", "FLDOWNERBUDGETNAME", "FLDAMOUNT" };
        string[] alCaptions = { "Date", "Component", "Component Sub-Type", "Budget Code", "Owner Budget Code", "Amount (USD)" };



        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashSearch(General.GetNullableInteger(ViewState["vesselid"].ToString()), General.GetNullableGuid(ViewState["balanceid"].ToString()), 0
                                                                , sortexpression, sortdirection
                                                                , 1
                                                                , iRowCount, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CaptainCash.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>EXECUTIVE SHIP MANAGEMENT</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void budget()
    {
        //btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30', false); ");
        //imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
    }
    private void ShowReport()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 10;
        string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDBUDGETCODE", "FLDOWNERBUDGETNAME", "FLDAMOUNT", "FLDPAYMENTRECEIPT" };
        string[] alCaptions = { "Port", "Date", "Purpose", "Budget Code", "Owner Budget Code", "Amount (USD)", "Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid, balanceid;

        if (Request.QueryString["vesselid"] != null)
            vesselid = Request.QueryString["vesselid"].ToString();
        else
            vesselid = "";

        if (Request.QueryString["balanceid"] != null)
            balanceid = Request.QueryString["balanceid"].ToString();
        else
            balanceid = "";
        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashSearch(General.GetNullableInteger(vesselid), General.GetNullableGuid(balanceid), 1
                                                                , sortexpression, sortdirection
                                                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                , gvCaptainPettyCash.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvCaptainPettyCash", "Captain Cash", alCaptions, alColumns, ds);
        gvCaptainPettyCash.DataSource = ds;
        gvCaptainPettyCash.VirtualItemCount = iRowCount;
        ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["PRINCIPAL"] = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();
        }
        ViewState["ROWSINGRIDVIEW"] = 0;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ShowReport();
        ShowReportFormat2();
        budget();
    }
    protected void gvCaptainPettyCash_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
          
            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lbllineitenid = (RadLabel)e.Item.FindControl("lbllineitenid");
            RadLabel lblAmount = (RadLabel)e.Item.FindControl("lblAmount");

            ImageButton sp = (ImageButton)e.Item.FindControl("cmdSplit");
            if (sp != null) sp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsCaptainCashSplit.aspx?vslid=" + lblVesselId.Text + "&cclid=" + lbllineitenid.Text + "&amt=" + lblAmount.Text + "'); return false;");
            RadLabel lbldtkey = (RadLabel)e.Item.FindControl("lbldtkey");

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lbldtkey.Text + "&mod="
                    + PhoenixModule.ACCOUNTS + "'); return false;");
            }
            RadTextBox txtBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (txtBudgetNameEdit != null) txtBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (txtBudgetIdEdit != null) txtBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (txtBudgetgroupIdEdit != null) txtBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30', false); return false;");
            }

            RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (txtOwnerBudgetNameEdit != null) txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (txtOwnerBudgetIdEdit != null) txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetgroupIdEdit != null) txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            ImageButton ib2 = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            if (ib2 != null)
            {
                ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + drv["FLDPRINCIPAL"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
            }
        }
    }

    private bool IsValidAmount(string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required";

        return (!ucError.IsError);
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReport();

    }
    protected void gvCaptainPettyCash_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
    }

    private void ShowReportFormat2()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDATE", "FLDLOGTYPENAME", "FLDWAGENAME", "FLDBUDGETCODE", "FLDOWNERBUDGETNAME", "FLDAMOUNT" };
        string[] alCaptions = { "Date", "Component", "Component Sub-Type", "Budget Code", "Owner Budget Code", "Amount (USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION2"] == null) ? null : (ViewState["SORTEXPRESSION2"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION2"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION2"].ToString());

        string vesselid, balanceid;

        if (Request.QueryString["vesselid"] != null)
            vesselid = Request.QueryString["vesselid"].ToString();
        else
            vesselid = "";

        if (Request.QueryString["balanceid"] != null)
            balanceid = Request.QueryString["balanceid"].ToString();
        else
            balanceid = "";

        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashSearch(General.GetNullableInteger(vesselid), General.GetNullableGuid(balanceid), 0
                                                                , sortexpression, sortdirection
                                                                , Convert.ToInt16(ViewState["PAGENUMBER2"].ToString())
                                                                //, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvCaptainCash.PageSize
                                                                , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCaptainCash", "Captain Cash", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        gvCaptainCash.DataSource = ds;
        gvCaptainCash.VirtualItemCount = iRowCount;
        ViewState["ROWSINGRIDVIEW2"] = ds.Tables[0].Rows.Count - 1;
        //}
        //else
        //{

        DataTable dt = ds.Tables[0];

        ViewState["ROWSINGRIDVIEW2"] = 0;
        //}

        ViewState["ROWCOUNT2"] = iRowCount;
        ViewState["TOTALPAGECOUNT2"] = iTotalPageCount;
    }

    protected void gvCaptainCash_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

            RadLabel lblPrincipal = (RadLabel)e.Item.FindControl("lblEditPrincipal");

            ImageButton ib3 = (ImageButton)e.Item.FindControl("imgShowAccount1Edit");
            if (ib3 != null && (RadTextBox)e.Item.FindControl("txtBudgetIdEdit") != null)
            {
                RadTextBox budgetid = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                ib3.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + drv["FLDPRINCIPAL"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + budgetid.Text + "', true); ");

            }
            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lbllineitenid = (RadLabel)e.Item.FindControl("lbllineitenid");
            RadLabel lblAmount = (RadLabel)e.Item.FindControl("lblAmount");

            ImageButton sp = (ImageButton)e.Item.FindControl("cmdSplit");
            if (sp != null) sp.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','AccountsCaptainCashSplit.aspx?vslid=" + lblVesselId.Text + "&cclid=" + lbllineitenid.Text + "&amt=" + lblAmount.Text + "'); return false;");


            RadLabel lbldtkey = (RadLabel)e.Item.FindControl("lbldtkey");

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";

                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + lbldtkey.Text + "&mod="
                    + PhoenixModule.ACCOUNTS + "'); return false;");
            }
        }
    }

    private bool IsValidBreakUp(string budgetid, string ownerbudgetid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget code is required.";

        if (!General.GetNullableGuid(ownerbudgetid).HasValue)
            ucError.ErrorMessage = "Owner Budget code is required.";

        return (!ucError.IsError);
    }

    protected void cmdSort2_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReportFormat2();

    }
    protected void gvCaptainCash_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION2"] = se.SortExpression;

        if (ViewState["SORTDIRECTION2"] != null && ViewState["SORTDIRECTION2"].ToString() == "0")
            ViewState["SORTDIRECTION2"] = 1;
        else
            ViewState["SORTDIRECTION2"] = 0;

        ShowReportFormat2();
    }


    protected void gvCaptainPettyCash_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string vessel = ((RadLabel)e.Item.FindControl("lblEditVesselId")).Text;
                string lineitemid = ((RadLabel)e.Item.FindControl("lblEditlineitenid")).Text;
                string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                string strOwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text;

                if (!IsValidBreakUp(budgetid, strOwnerBudgetId))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsCaptainCash.OfficeCaptainCashLineItemUpdate(int.Parse(vessel), new Guid(lineitemid), General.GetNullableInteger(budgetid), General.GetNullableGuid(strOwnerBudgetId));

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
        ShowReport();
    }


    protected void gvCaptainPettyCash_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCaptainPettyCash.CurrentPageIndex + 1;
        ShowReport();
    }

    protected void gvCaptainCash_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string vessel = ((RadLabel)e.Item.FindControl("lblEditVesselId")).Text;
                string lineitemid = ((RadLabel)e.Item.FindControl("lblEditlineitenid")).Text;
                string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                string strOwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtownerbudgetMapidEdit")).Text;

                if (!IsValidBreakUp(budgetid, strOwnerBudgetId))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsCaptainCash.OfficeCaptainCashLineItemUpdate(int.Parse(vessel), new Guid(lineitemid), General.GetNullableInteger(budgetid), General.GetNullableGuid(strOwnerBudgetId));
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
        ShowReportFormat2();
    }


    protected void gvCaptainCash_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCaptainCash.CurrentPageIndex + 1;
        ShowReportFormat2();
    }
}
