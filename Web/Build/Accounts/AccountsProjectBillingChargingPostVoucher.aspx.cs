using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using System.Text;

public partial class AccountsProjectBillingChargingPostVoucher : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCountry.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCountry.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsProjectBillingChargingPostVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCountry')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Accounts/AccountsProjectBillingChargingFilter.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','AccountsProjectBillingChargingFilter.aspx')", "Find", "search.png", "FIND");
            //toolbar.AddImageButton("../Registers/RegistersCountry.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Voucher Posting", "VOUCHERPOSTING");
            MenuProjectBillingMain.AccessRights = this.ViewState;
            MenuProjectBillingMain.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbarmenu = new PhoenixToolbar ();
            toolbarmenu.AddButton ("Post Voucher", "POSTVOUCHER") ;
            MenuProjectBilling.AccessRights = this.ViewState;
            MenuProjectBilling.MenuList =toolbarmenu.Show();   


            MenuProjectBillingMain.SelectedMenuIndex = 1; 
            if (!IsPostBack)
            {
                ViewState["projectbillingissueid"] = null;
                ViewState["company"] = null;
                ViewState["VESSELID"] = null;
                ViewState["companyid"] = null;

                if (Request.QueryString["projectbillingissueid"] != string.Empty)
                {
                    ViewState["projectbillingissueid"] = Request.QueryString["projectbillingissueid"];
                }
                if (Request.QueryString["company"] != string.Empty)
                {
                    ViewState["company"] = Request.QueryString["company"];
                }
                if (Request.QueryString["VESSELID"] != string.Empty)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                }
                if (Request.QueryString["companyid"] != string.Empty)
                {
                    ViewState["companyid"] = Request.QueryString["companyid"];
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["txtBudgetIdEdit"] = null;
                ViewState["txtBudgetgroupIdEdit"] = null;
                ViewState["txtBudgetNameEdit"] = null;
                ViewState["txtBudgetCodeEdit"] = null;
                ViewState["txtOwnerBudgetIdEdit"] = null;
                ViewState["txtOwnerBudgetgroupIdEdit"] = null;
                ViewState["txtOwnerBudgetNameEdit"] = null;
                ViewState["txtOwnerBudgetCodeEdit"] = null;

                txtPostDate.Text = DateTime.UtcNow.ToShortDateString();
                txtBillingCompany.Text = ViewState["company"].ToString(); 
                
            }
            BindData();
            SetPageNavigator();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPROJECTBILLINGNAME", "FLDPROJECTBILLINGGROUP", "FLDVESSELNAME", "FLDISSUEDATE", "FLDISSUEDQTY", "FLDBUDGETCODE", "FLDOWNERBUDGETGROUP", "FLDBILLEDQTY", "FLDSELLINGAMOUNT", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAMOUNTUSD" };
        string[] alCaptions = { "Project Billing Name", "Project Billing Group", "Vessel Issued", "Date Issued", "Issued Quantity","Vessel Budget Code","Owner Budget Code", "Billed Quantity", "Rate", "Currency", "Amount","Amount(USD)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



        ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostByVesselSearch(ViewState["projectbillingissueid"].ToString()
                                    , General.GetNullableDateTime(txtPostDate.Text)
                                    , sortexpression
                                    , sortdirection
                                    , (int)ViewState["PAGENUMBER"]
                                    , General.ShowRecords(null)
                                    , ref iRowCount
                                    , ref iTotalPageCount);
        
        Response.AddHeader("Content-Disposition", "attachment; filename=ProjectBillingVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Project Billing Voucher</h3></td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvCountry.EditIndex = -1;
                gvCountry.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuProjectBillingMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../Accounts/AccountsProjectBillingChargingGeneral.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("VOUCHERPOSTING"))
            {
                StringBuilder strprojectbillingissueid = new StringBuilder();

                GridView gv = (GridView)gvCountry;

                foreach (GridViewRow row in gv.Rows)
                {

                    CheckBox chk1 = (CheckBox)row.FindControl("chckPostVoucher");
                    if (chk1.Checked == true)
                    {

                        strprojectbillingissueid.Append(((Label)row.FindControl("lblVesselId")).Text.ToString());
                        strprojectbillingissueid.Append(",");

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
    protected void MenuProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("POSTVOUCHER"))
            {
                PhoenixAccountsProjectBilling.ProjectBillingVoucherPost(ViewState["projectbillingissueid"].ToString()
                                                                        , int.Parse(ViewState["companyid"].ToString())
                                                                        , General.GetNullableDateTime(txtPostDate.Text));

                ucStatus.Text = "Voucher Posted Successfully.";
                BindData();
                SetPageNavigator();
                            
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
        string[] alColumns = { "FLDPROJECTBILLINGNAME", "FLDPROJECTBILLINGGROUP", "FLDVESSELNAME", "FLDISSUEDATE", "FLDISSUEDQTY", "FLDBUDGETCODE", "FLDOWNERBUDGETGROUP", "FLDBILLEDQTY", "FLDSELLINGAMOUNT", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAMOUNTUSD" };
        string[] alCaptions = { "Project Billing Name", "Project Billing Group", "Vessel Issued", "Date Issued", "Issued Quantity", "Vessel Budget Code", "Owner Budget Code", "Billed Quantity", "Rate", "Currency", "Amount", "Amount(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();
        
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostByVesselSearch(ViewState["projectbillingissueid"].ToString()
                                    , General.GetNullableDateTime(txtPostDate.Text)  
                                    , sortexpression
                                    , sortdirection
                                    , (int)ViewState["PAGENUMBER"]
                                    , General.ShowRecords(null)
                                    , ref iRowCount
                                    , ref iTotalPageCount);

        
        General.SetPrintOptions("gvCountry", "Country", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCountry.DataSource = ds;
            gvCountry.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCountry);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvCountry_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCountry_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCountry, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsProjectBilling.ProjectBillingVoucherPostUpdate(
                                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProjectBillingIssueIdEdit")).Text.ToString())
                                , General .GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text.ToString())
                                , General.GetNullableGuid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetIdEdit")).Text.ToString())
                                , decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtBilledQty")).Text));

                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
            
            }
           
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    
    protected void gvCountry_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
   
    protected void gvCountry_RowEditing(object sender, GridViewEditEventArgs de)
    {

        ViewState["txtBudgetIdEdit"] = null;
        ViewState["txtBudgetgroupIdEdit"] = null;
        ViewState["txtBudgetNameEdit"] = null;
        ViewState["txtBudgetCodeEdit"] = null;
        ViewState["txtOwnerBudgetIdEdit"] = null;
        ViewState["txtOwnerBudgetgroupIdEdit"] = null;
        ViewState["txtOwnerBudgetNameEdit"] = null;
        ViewState["txtOwnerBudgetCodeEdit"] = null;

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCountry_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
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


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");                   
                }                          

            }
            
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtBudgetIdEdit = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            TextBox txtBudgetgroupIdEdit = (TextBox)e.Row.FindControl("txtBudgetgroupIdEdit");
            TextBox txtBudgetNameEdit = (TextBox)e.Row.FindControl("txtBudgetNameEdit");
            TextBox txtBudgetCodeEdit = (TextBox)e.Row.FindControl("txtBudgetCodeEdit");
            TextBox txtOwnerBudgetIdEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetIdEdit");
            TextBox txtOwnerBudgetgroupIdEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetgroupIdEdit");
            TextBox txtOwnerBudgetNameEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetNameEdit");
            TextBox txtOwnerBudgetCodeEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetCodeEdit");

            if (ViewState["txtBudgetIdEdit"] != null && txtBudgetIdEdit != null)
                ((TextBox)e.Row.FindControl("txtBudgetIdEdit")).Text = (ViewState["txtBudgetIdEdit"].ToString());
            if (ViewState["txtBudgetgroupIdEdit"] != null && txtBudgetgroupIdEdit != null)
                ((TextBox)e.Row.FindControl("txtBudgetgroupIdEdit")).Text = (ViewState["txtBudgetgroupIdEdit"].ToString());
            if (ViewState["txtBudgetNameEdit"] != null && txtBudgetNameEdit != null)
                ((TextBox)e.Row.FindControl("txtBudgetNameEdit")).Text = (ViewState["txtBudgetNameEdit"].ToString());
            if (ViewState["txtBudgetCodeEdit"] != null && txtBudgetCodeEdit != null)
                ((TextBox)e.Row.FindControl("txtBudgetCodeEdit")).Text = (ViewState["txtBudgetCodeEdit"].ToString());
            if (ViewState["txtOwnerBudgetIdEdit"] != null && txtOwnerBudgetIdEdit != null)
                ((TextBox)e.Row.FindControl("txtOwnerBudgetIdEdit")).Text = (ViewState["txtOwnerBudgetIdEdit"].ToString());
            if (ViewState["txtOwnerBudgetgroupIdEdit"] != null && txtOwnerBudgetgroupIdEdit != null)
                ((TextBox)e.Row.FindControl("txtOwnerBudgetgroupIdEdit")).Text = (ViewState["txtOwnerBudgetgroupIdEdit"].ToString());
            if (ViewState["txtOwnerBudgetNameEdit"] != null && txtOwnerBudgetNameEdit != null)
                ((TextBox)e.Row.FindControl("txtOwnerBudgetNameEdit")).Text = (ViewState["txtOwnerBudgetNameEdit"].ToString());
            if (ViewState["txtOwnerBudgetCodeEdit"] != null && txtOwnerBudgetCodeEdit != null)
                ((TextBox)e.Row.FindControl("txtOwnerBudgetCodeEdit")).Text = (ViewState["txtOwnerBudgetCodeEdit"].ToString());
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox tb1 = (TextBox)e.Row.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetgroupIdEdit");
            TextBox txtBudgetIdEdit = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            Label lblPOBudgetId = (Label)e.Row.FindControl("lblPOBudgetId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Row.FindControl("btnShowOwnerBudgetEdit");
            TextBox txtOwnerBudgetNameEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetNameEdit");
            TextBox txtOwnerBudgetIdEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetIdEdit");
            TextBox txtOwnerBudgetgroupIdEdit = (TextBox)e.Row.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ibtnShowOwnerBudgetEdit != null && ViewState["VESSELID"] != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + (ViewState["txtBudgetIdEdit"] != null ? ViewState["txtBudgetIdEdit"].ToString() : txtBudgetIdEdit.Text) + "', true); ");         //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            GridView gv = (GridView)gvCountry;
            double totalamount = 0.00;
            double totalamountusd = 0.00;

            foreach (GridViewRow row in gv.Rows)
            {
                Label lbltotalamount = (Label)row.FindControl("lblAmount");
                if (lbltotalamount != null)
                {
                    totalamount = totalamount + Convert.ToDouble(lbltotalamount.Text); 
                }
                Label lbltotalamountusd = (Label)row.FindControl("lblAmountUSD");
                if (lbltotalamountusd != null)
                {
                    totalamountusd = totalamountusd + Convert.ToDouble(lbltotalamountusd.Text);
                }
            }

            Label lbltotalamount1 = (Label)e.Row.FindControl("lblTotalAmount");
            if (lbltotalamount1 != null)
            {
                lbltotalamount1.Text = totalamount.ToString();
                lbltotalamount1.Attributes.Add("style", "visibility:hidden");
            }

            Label lbltotalamountusd1 = (Label)e.Row.FindControl("lblTotalAmountUSD");
            if (lbltotalamountusd1 != null)
                lbltotalamountusd1.Text = totalamountusd.ToString();
        }
        
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCountry.SelectedIndex = -1;
        gvCountry.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //vessel budget edit
        if (Filter.CurrentPickListSelection.Keys[3].ToString().Contains("txtBudgetIdEdit"))
            ViewState["txtBudgetIdEdit"] = Filter.CurrentPickListSelection.Get(3);
        if (Filter.CurrentPickListSelection.Keys[4].ToString().Contains("txtBudgetgroupIdEdit"))
            ViewState["txtBudgetgroupIdEdit"] = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[2].ToString().Contains("txtBudgetNameEdit"))
            ViewState["txtBudgetNameEdit"] = Filter.CurrentPickListSelection.Get(2);
        if (Filter.CurrentPickListSelection.Keys[1].ToString().Contains("txtBudgetCodeEdit"))
            ViewState["txtBudgetCodeEdit"] = Filter.CurrentPickListSelection.Get(1);
        //owner budget edit
        if (Filter.CurrentPickListSelection.Keys[3].ToString().Contains("txtOwnerBudgetIdEdit"))
            ViewState["txtOwnerBudgetIdEdit"] = Filter.CurrentPickListSelection.Get(3);
        if (Filter.CurrentPickListSelection.Keys[4].ToString().Contains("txtOwnerBudgetgroupIdEdit"))
            ViewState["txtOwnerBudgetgroupIdEdit"] = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[2].ToString().Contains("txtOwnerBudgetNameEdit"))
            ViewState["txtOwnerBudgetNameEdit"] = Filter.CurrentPickListSelection.Get(2);
        if (Filter.CurrentPickListSelection.Keys[1].ToString().Contains("txtOwnerBudgetCodeEdit"))
            ViewState["txtOwnerBudgetCodeEdit"] = Filter.CurrentPickListSelection.Get(1);

        
        //ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }
    protected void txtPostDate_TextChangedEvent(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();

    }
    
}
