using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshoreReimbursement : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Filter.CurrentMenuCodeSelection == "VAC-PBL-VED")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Cash Advance", "CASHADVANCE");
                toolbar.AddButton("Earnings/Deductions", "EARNINGDEDUCTION");
                toolbar.AddButton("Reimbursements", "REIMBURSEMENTS");
                MenuReimbursementGeneral.AccessRights = this.ViewState;
                MenuReimbursementGeneral.MenuList = toolbar.Show();
                MenuReimbursementGeneral.SelectedMenuIndex = 2;
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReimbursement.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreREimbursementFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReimbursement.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuReimbursement.AccessRights = this.ViewState;
            MenuReimbursement.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                BindYear();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.bind();
                    UcVessel.Enabled = false;
                }

                gvRem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReimbursementGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EARNINGDEDUCTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOffshoreEarningDeduction.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("CASHADVANCE"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOffshoreCashAdvance.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReimbursement_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
             
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewOffshoreReimbursementFilterSelection = null;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                    UcVessel.SelectedVessel = "";
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRem.Rebind();
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

        string[] alColumns = { "FLDREFERENCENO", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDACCOUNTOF", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAMOUNTUSD", "FLDSUBMISSIONDATE", "FLDSUBACCOUNT" };
        string[] alCaptions = { "Ref. No", "File No", "Employee Name", "Rank", "Account of", "Reimbursement/Recovery", "Purpose", "Description", "Currency", "Amount", "Amount (USD)", "Claim Submission date", "Budget Code" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CrewOffshoreReimbursementFilterSelection;

        DataTable dt = PhoenixCrewOffshoreReimbursement.SearchCrewReimbursement(null
                                                                    , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                    , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                    , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                    , General.GetNullableInteger(UcVessel.SelectedVessel)
                                                                    , sortexpression, sortdirection
                                                                    , 1, iRowCount
                                                                    , ref iRowCount, ref iTotalPageCount
                                                                    , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                    , General.GetNullableInteger(ddlYear.SelectedValue));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        if (ds.Tables.Count > 0)
            General.ShowExcel("Reimbursements/Recoveries", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        try
        {
            string[] alColumns = { "FLDREFERENCENO", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDACCOUNTOF", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAMOUNTUSD", "FLDSUBMISSIONDATE", "FLDSUBACCOUNT" };
            string[] alCaptions = { "Ref. No", "File No", "Employee Name", "Rank", "Account of", "Reimbursement/Recovery", "Purpose", "Description", "Currency", "Amount", "Amount (USD)", "Claim Submission date", "Budget Code" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewOffshoreReimbursementFilterSelection;

            DataTable dt = PhoenixCrewOffshoreReimbursement.SearchCrewReimbursement(null
                                                                                , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                                , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                                , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                                , General.GetNullableInteger(UcVessel.SelectedVessel)
                                                                                , sortexpression, sortdirection
                                                                                , (int)ViewState["PAGENUMBER"], gvRem.PageSize
                                                                                , ref iRowCount, ref iTotalPageCount
                                                                                , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                                , General.GetNullableInteger(ddlYear.SelectedValue));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvRem", "Reimbursements/Recoveries", alCaptions, alColumns, ds);
            gvRem.DataSource = dt;
            gvRem.VirtualItemCount = iRowCount;


            if (Filter.CurrentMenuCodeSelection == "VAC-PBL-VED")
            {
                gvRem.MasterTableView.ShowFooter = false;
            }
            else
            {
                gvRem.MasterTableView.ShowFooter = true;
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
  

   

   
  
    protected void gvRem_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            GridView _gridView = (GridView)sender;
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
   

    private bool IsValidateReimbursement(string signonoffid, string hardcode, string currency, string amt, string earorded, string approvedamt, string desc, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";
        if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
            ucError.ErrorMessage = "Month is required.";
        if (General.GetNullableInteger(ddlYear.SelectedValue) == null)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableInteger(signonoffid) == null)
            ucError.ErrorMessage = "Employee is required.";
        if (!General.GetNullableInteger(earorded).HasValue)
            ucError.ErrorMessage = "Reimbursement/Recovery is required";
        if (!General.GetNullableInteger(hardcode).HasValue)
            ucError.ErrorMessage = "Purpose is required";
        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required";
        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Curreny is required";
        if (!General.GetNullableDecimal(amt).HasValue)
            ucError.ErrorMessage = "Amount is required";
        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Claim submission date is required.";

        return (!ucError.IsError);
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlEarDed_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = (RadComboBox)sender;
            
            //GridDataItem row = (GridDataItem)ddl.Parent.Parent;
            UserControlHard status = null;
            if (ddl.ID == "ddlDescAdd")
            {
                GridFooterItem row = (GridFooterItem)ddl.Parent.Parent;
                status = row.FindControl("ddlDescAdd") as UserControlHard;
                if (General.GetNullableInteger(ddl.SelectedValue).HasValue && General.GetNullableInteger(ddl.SelectedValue).Value < 0)
                {
                    status.HardList = PhoenixRegistersHard.ListHard(1, 129, 0, "CRR,ADL,TFR");
                }
                else
                {
                    status.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "TRV,USV,AFR,EBG,CFE,LFE,MEF");
                }
            }
            else if(ddl.ID == "ddlDesc")
            {
                GridDataItem row = (GridDataItem)ddl.Parent.Parent;
                status = row.FindControl("ddlDesc") as UserControlHard;
                if (General.GetNullableInteger(ddl.SelectedValue).HasValue && General.GetNullableInteger(ddl.SelectedValue).Value < 0)
                {
                    status.HardList = PhoenixRegistersHard.ListHard(1, 129, 0, "CRR,ADL,TFR");
                }
                else
                {
                    status.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "TRV,USV,AFR,EBG,CFE,LFE,MEF");
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
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRem.Rebind();
    }
    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindData();
        gvRem.Rebind();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        gvRem.Rebind();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        gvRem.Rebind();
    }

    protected void BindEmployee(RadComboBox ddl)
    {
        try
        {
            ddl.Items.Clear();
            ddl.DataSource = PhoenixCrewOffshoreReimbursement.OffshoreEmployeeList(General.GetNullableInteger(UcVessel.SelectedVessel),
                General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));

            ddl.DataTextField = "FLDNAME";
            ddl.DataValueField = "FLDSIGNONOFFID"; //"FLDEMPLOYEEID";          
            ddl.DataBind();
            ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear()
    {
        try
        {
            for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
            {
                RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
                ddlYear.Items.Add(li);
            }
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRem_ItemCommand(object sender, GridCommandEventArgs e)
    {
     
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
      
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string id = string.Empty;
                id = ((RadComboBox)e.Item.FindControl("ddlEmployeeAdd")).SelectedValue;
                string hardcode = ((UserControlHard)e.Item.FindControl("ddlDescAdd")).SelectedHard;
                string desc = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;
                string currency = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text;
                string appamt = amt;
                string earorded = ((RadComboBox)e.Item.FindControl("ddlEarDedAdd")).SelectedValue;
                string budget = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string date = ((UserControlDate)e.Item.FindControl("ucDateAdd")).Text;

                if (!IsValidateReimbursement(id, hardcode, currency, amt, earorded, appamt, desc, date))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? reimbursementid = null;
                PhoenixCrewOffshoreReimbursement.InsertCrewReimbursement(General.GetNullableInteger(id), int.Parse(hardcode), int.Parse(currency), decimal.Parse(amt)
                    , General.GetNullableDecimal(appamt), General.GetNullableInteger(budget), int.Parse(earorded), desc, General.GetNullableInteger(UcVessel.SelectedVessel)
                    , DateTime.Parse(date), ref reimbursementid, General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
              
                BindData();
                gvRem.Rebind();
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
                
                try
                {
                    Guid id = (Guid)gvRem.MasterTableView.DataKeyValues[e.Item.ItemIndex]["FLDREIMBURSEMENTID"];//.DataKeys[nCurrentRow].Value;
                    string empid = ((RadLabel)e.Item.FindControl("lblEmpIDEdit")).Text;
                    string signonoffid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                    string hardcode = ((UserControlHard)e.Item.FindControl("ddlDesc")).SelectedHard;
                    string desc = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
                    string currency = ((UserControlCurrency)e.Item.FindControl("ddlCurrency")).SelectedCurrency;
                    string amt = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                    string date = ((UserControlDate)e.Item.FindControl("ucDateEdit")).Text;
                    string earorded = ((RadComboBox)e.Item.FindControl("ddlEarDed")).SelectedValue;
                    string budget = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;

                    if (!IsValidateReimbursement(signonoffid, hardcode, currency, amt, earorded, amt, desc, date))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewOffshoreReimbursement.UpdateCrewReimbursement(id, int.Parse(hardcode), int.Parse(currency), decimal.Parse(amt)
                        , General.GetNullableDecimal(amt)
                        , General.GetNullableInteger(budget), int.Parse(earorded), desc, General.GetNullableInteger(UcVessel.SelectedVessel), General.GetNullableDateTime(date)
                        , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(signonoffid));
                   
                    BindData();
                    gvRem.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
               
                try
                {
                    Guid id = (Guid)gvRem.MasterTableView.DataKeyValues[e.Item.ItemIndex]["FLDREIMBURSEMENTID"];
                    PhoenixCrewOffshoreReimbursement.DeleteCrewReimbursement(id);
                    BindData();
                    gvRem.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
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

    protected void gvRem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    if (Filter.CurrentMenuCodeSelection == "VAC-PBL-VED") del.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, del.CommandName)) del.Visible = false;
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
                if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null)
                {
                    if (Filter.CurrentMenuCodeSelection == "VAC-PBL-VED") ed.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
                }

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                           + PhoenixModule.CREW + "'); return false;");
                    HtmlGenericControl html = new HtmlGenericControl();
                    if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    //att.ImageUrl = Session["images"] + "/no-attachment.png";
                }

                UserControlCurrency uc = (UserControlCurrency)e.Item.FindControl("ddlCurrency");
                if (uc != null) uc.SelectedCurrency = drv["FLDCURRENCYID"].ToString();

                RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlEarDed");
                if (ddl != null)
                {
                    if (ddl.Items.FindItemByValue(drv["FLDEARNINGDEDUCTION"].ToString()) != null)
                        ddl.SelectedValue = drv["FLDEARNINGDEDUCTION"].ToString();
                    if (General.GetNullableInteger(drv["FLDEARNINGDEDUCTION"].ToString()).HasValue && General.GetNullableInteger(drv["FLDEARNINGDEDUCTION"].ToString()).Value < 0)
                        ddlEarDed_SelectedIndexChanged(ddl, null);
                }

                UserControlHard hrd = (UserControlHard)e.Item.FindControl("ddlDesc");
                if (hrd != null) hrd.SelectedHard = drv["FLDHARDCODE"].ToString();

                LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
                if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblApprovedAmount");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                    //lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    //lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

                //e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRem, "Select$" + e.Item.RowIndex.ToString(), false);
                LinkButton refno = (LinkButton)e.Item.FindControl("lnkRefNo");
                if (refno != null)
                    refno.Attributes.Add("onclick", "parent.Openpopup('Details', '', '../Crew/CrewReimbursementDetail.aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "&readonly=true');return false;");

            }
           
            if (e.Item is GridFooterItem)
            {
                RadComboBox ve = (RadComboBox)e.Item.FindControl("ddlEmployeeAdd");
                if (ve != null)
                {
                    BindEmployee(ve);
                }

                LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetAdd");
                if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRem_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
