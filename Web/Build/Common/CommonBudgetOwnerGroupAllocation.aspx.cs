using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonBudgetOwnerGroupAllocation : PhoenixBasePage
{
   
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvBudgetGroupAllocation.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {

    //        }
    //    }
    //    if (!IsPostBack)
    //    {
    //        BindVesselAllocation();
    //        BindBudgetGroup();
    //        BindBudgetPeriod();
    //        BindStartEndDate();
    //    }
    //    base.Render(writer);
    //}

    public decimal accumulatedtotal = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Vessel List", "VESSEL", ToolBarDirection.Right);
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELBUDGETID"] = null;
            ViewState["BUDGETGROUPLEVEL"] = 1;
            ViewState["BudgetGroupId"] = "";
            ViewState["VESSELID"] = null;
            ViewState["FINDEMPTYLEVEL"] = 1;

            gvBudgetGroupAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvBudgetPeriodAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvVesselAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            if (Request.QueryString["VESSELBUDGETALLOCATIONID"] != null && Request.QueryString["VESSELBUDGETALLOCATIONID"].ToString() != "")
                ViewState["VESSELBUDGETALLOCATIONID"] = Request.QueryString["VESSELBUDGETALLOCATIONID"].ToString();
            else
                ViewState["VESSELBUDGETALLOCATIONID"] = "";

            if (Request.QueryString["vesselaccountid"] != null)
            {
                ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }

            BindFinancialYearDetails();
            BindStartEndDate();
            BindGroupLevel();
            BindVesselAllocation();
            BindBudgetGroup();
            BindBudgetPeriod();
           
        }

        
        MenuBudgetTab.SelectedMenuIndex = 0;

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetGroupAllocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

       // BindVesselAllocation();
       // BindBudgetGroup();
       // BindBudgetPeriod();
      //  BindStartEndDate();
        BindGroupLevel();
    //    BindFinancialYearDetails();

    }

    private void BindFinancialYearDetails()
    {
        if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        {

            DataSet dsVessel = PhoenixCommonBudgetGroupAllocation.EditVesselBudgetAllocation(General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()));
            if (dsVessel.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVessel.Tables[0].Rows[0];
                ucFinancialYear.SelectedQuick = dr["FLDFINANCIALYEAR"].ToString();
                ViewState["financialyear"] = dr["FLDFINANCIALYEAR"].ToString();
            }

        }
    }
    protected void Rebind()
    {
        gvBudgetGroupAllocation.SelectedIndexes.Clear();
        gvBudgetGroupAllocation.EditIndexes.Clear();
        gvBudgetGroupAllocation.DataSource = null;
        gvBudgetGroupAllocation.Rebind();
    }
    protected void PeriodRebind()
    {
        gvBudgetPeriodAllocation.SelectedIndexes.Clear();
        gvBudgetPeriodAllocation.EditIndexes.Clear();
        gvBudgetPeriodAllocation.DataSource = null;
        gvBudgetPeriodAllocation.Rebind();
    }
    protected void VesselRebind()
    {
        gvVesselAllocation.SelectedIndexes.Clear();
        gvVesselAllocation.EditIndexes.Clear();
        gvVesselAllocation.DataSource = null;
        gvVesselAllocation.Rebind();
    }

    private void BindGroupLevel()
    {
        string vesselid = "";

        vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupLevel(
            int.Parse(vesselid)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["ACCOUNTID"] == null ? "" : ViewState["ACCOUNTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            int count = int.Parse(ds.Tables[0].Rows[0]["MAXLEVEL"].ToString());
            DataTable dt = new DataTable();
            dt.Columns.Add("FLDLEVELID", typeof(string));
            dt.Columns.Add("FLDLEVELNAME", typeof(string));
            if (count == 1)
            {
                dt.Rows.Add("1", "One");
            }
            else if (count == 2)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
            }
            else if (count == 3)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
                dt.Rows.Add("3", "Three");
            }
            else if (count == 4)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
                dt.Rows.Add("3", "Three");
                dt.Rows.Add("4", "Four");
            }
            else if (count == 5)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
                dt.Rows.Add("3", "Three");
                dt.Rows.Add("4", "Four");
                dt.Rows.Add("5", "Five");
            }
            else if (count == 6)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
                dt.Rows.Add("3", "Three");
                dt.Rows.Add("4", "Four");
                dt.Rows.Add("5", "Five");
                dt.Rows.Add("6", "Six");
            }
            else if (count == 7)
            {
                dt.Rows.Add("1", "One");
                dt.Rows.Add("2", "Two");
                dt.Rows.Add("3", "Three");
                dt.Rows.Add("4", "Four");
                dt.Rows.Add("5", "Five");
                dt.Rows.Add("6", "Six");
                dt.Rows.Add("7", "Seven");
            }

            ddlOwnerBudgetGroupLevel.DataSource = dt;
            ddlOwnerBudgetGroupLevel.DataTextField = "FLDLEVELNAME";
            ddlOwnerBudgetGroupLevel.DataValueField = "FLDLEVELID";
            ddlOwnerBudgetGroupLevel.DataBind();
            //ddlOwnerBudgetGroupLevel.Items.Insert(1, new ListItem("1st Level", "1"));
        }
    }


    protected void ddlOwnerBudgetGroupLevel_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["BUDGETGROUPLEVEL"] = int.Parse(ddlOwnerBudgetGroupLevel.SelectedValue.ToString());
        BindBudgetGroup();
        gvBudgetGroupAllocation.Rebind();
    }


    private void BindStartEndDate()
    {
        string vesselid = "";

        vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersVesselFinancialYear.FinancialYearStartEndDates(
            int.Parse(vesselid)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["ACCOUNTID"] == null ? "" : ViewState["ACCOUNTID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtFromDate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
            txtToDate.Text = ds.Tables[0].Rows[0]["FLDENDDATE"].ToString();
        }
        else
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }
    }

    protected void FinancialYear_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELBUDGETID"] = null;
        //BindVesselAllocation();
        //BindBudgetGroup();
        //BindBudgetPeriod();
        gvVesselAllocation.Rebind();
        gvBudgetGroupAllocation.Rebind();
        gvBudgetPeriodAllocation.Rebind();

        BindStartEndDate();
        SetRowSelectionOfVessel();

    }

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Common/CommonBudgetVesselList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("BUDGET"))
        {
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBUDGETGROUPNAME", "FLDBUDGETAMOUNT", "FLDACCESSNAME" };
        string[] alCaptions = { "Budget Group", "Budget Amount", "Access" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationSearch(
           General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
            , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
            , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
            , General.GetNullableInteger(ViewState["OWNERID"] != null ? ViewState["OWNERID"].ToString() : "")
            , (int)ViewState["BUDGETGROUPLEVEL"]
           , sortexpression, sortdirection,
           (int)ViewState["PAGENUMBER"],
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetGroupAllocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Group Allocation</h3></td>");
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

    protected void CommonBudgetGroupAllocation_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
      
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        // gvBudgetGroupAllocation.Rebind();
        // BindBudgetGroup();
        //Rebind();
        gvBudgetGroupAllocation.Rebind();
        gvBudgetPeriodAllocation.Rebind();
        gvVesselAllocation.Rebind();
        BindStartEndDate();
      //  SetRowSelectionOfVessel();
     //   SetRowSelectionOfBudgetGroup();
    }

    private void BindVesselAllocation()
    {
        try
        {
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string vesselid = Request.QueryString["vesselid"] == null ? null : (Request.QueryString["vesselid"]);
            string ownerid = "", currentfinyear = "";

            DataSet dsVessel = PhoenixRegistersVessel.EditVessel(General.GetNullableInteger(vesselid));

            if (dsVessel.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVessel.Tables[0].Rows[0];

                ownerid = dr["FLDOWNER"].ToString();
                ViewState["OWNERID"] = dr["FLDOWNER"].ToString();
                currentfinyear = dr["FLDCURRENTFINYEARID"].ToString();
                ViewState["currentyear"] = dr["FLDCURRENTFINYEARID"].ToString();
                //if (!IsPostBack)
                //    ucFinancialYear.SelectedQuick = currentfinyear;
            }
            DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselAllocationSearch(
                  General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["financialyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
                 , General.GetNullableInteger(vesselid)
                 , General.GetNullableInteger(ViewState["VESSELACCOUNTID"].ToString())
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvVesselAllocation.DataSource = ds;

                if (ViewState["VESSELBUDGETID"] == null || ViewState["VESSELBUDGETID"].ToString() == "")
                {
                    ViewState["VESSELBUDGETID"] = ds.Tables[0].Rows[0]["FLDVESSELBUDGETALLOCATIONID"].ToString();
                    ViewState["ACCOUNTID"] = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
                    //  gvVesselAllocation.SelectedIndex = 0;
                }
              //  SetRowSelectionOfVessel();
            }
            else
            {
                gvVesselAllocation.DataSource = ds;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindBudgetGroup()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLPARENTDOWNERBUDGETGROUP", "FLDOWNERBUDGETGROUP", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDACCESSNAME", "FLDAPPORTIONMENTMETHODNAME" };
        string[] alCaptions = { "Immediate Parent", "Budget Group", "Budget Amount", "Allowance", "Access", "Apportionment" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupAllocationSearch(
            General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["financialyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
             , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
             , General.GetNullableInteger(ViewState["OWNERID"] != null ? ViewState["OWNERID"].ToString() : "")
             //,3794
             , int.Parse(ViewState["BUDGETGROUPLEVEL"].ToString())  //General.GetNullableInteger(ddlOwnerBudgetGroupLevel.SelectedValue) != null ? General.GetNullableInteger(ddlOwnerBudgetGroupLevel.SelectedValue) : 1
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvBudgetGroupAllocation.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetGroupAllocation", "Budget Group Allocation", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!IsPostBack)
            {
                ViewState["BudgetGroupId"] = ds.Tables[0].Rows[0]["FLDOWNERBUDGETGROUPID"].ToString();
            }

           // SetRowSelectionOfBudgetGroup();
            ViewState["FINDEMPTYLEVEL"] = 1;
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }

        gvBudgetGroupAllocation.DataSource = ds;
        gvBudgetGroupAllocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvVesselAllocation_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)    {        gvVesselAllocation.SelectedIndexes.Add(e.NewSelectedIndex);
        string vesselbudgetid = ((RadLabel)gvVesselAllocation.Items[e.NewSelectedIndex].FindControl("lblVesselAllocationId")).Text;
        ViewState["VESSELBUDGETID"] = vesselbudgetid;
        BindVesselAllocation();
        BindBudgetGroup();    }
    private void BindBudgetPeriod()
    {

        if (ViewState["BudgetGroupId"].ToString() != "")
        {
            DataSet ds = PhoenixCommonBudgetGroupAllocation.OwnerBudgetPeriodAllocationSearch(
               General.GetNullableGuid(ViewState["BudgetGroupId"].ToString())
                , General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["financialyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
                , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
                , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
               , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
                );


            gvBudgetPeriodAllocation.DataSource = ds;
        }
        accumulatedtotal = 0;
    }

   
    protected void gvVesselAllocation_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        try
        {
            RadLabel lblVesselAllocationId = (RadLabel)de.Item.FindControl("lblVesselAllocationId");

            PhoenixCommonBudgetGroupAllocation.DeleteBudgetRevision(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lblVesselAllocationId.Text));

            BindVesselAllocation();
            BindBudgetGroup();
            BindBudgetPeriod();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindGroupLevelDEFAULT()
    {
        DataSet ds = PhoenixCommonBudgetGroupAllocation.OwnerBudgetGroupLeveldefault(
          General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
         , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
         , General.GetNullableInteger(ViewState["ACCOUNTID"] == null ? "" : ViewState["ACCOUNTID"].ToString())
         , General.GetNullableGuid(ViewState["VESSELBUDGETID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["setleveldefault"] = ds.Tables[0].Rows[0]["FLDMAXLEVEL"].ToString();
            txtlevelcount.Text = "For this revision amount was given in level" + ' ' + ViewState["setleveldefault"].ToString();
           
        }
        Rebind();
        PeriodRebind();
    }
    protected void gvVesselAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                if (e.CommandName.ToUpper().Equals("EDIT"))
                {
                   // BindVesselAllocation();
                }
                else if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                   // Filter.CurrentBudgetAllocationVesselFilter = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                    ViewState["VESSELBUDGETID"] = ((RadLabel)e.Item.FindControl("lblVesselAllocationId")).Text;
                     BindGroupLevelDEFAULT();
                    Rebind();
                    PeriodRebind();

                }

                //else if (e.CommandName.ToUpper().Equals("BUDGETBREAKDOWN"))
                //{
                //    RadLabel lblVesselAllocationId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselAllocationId");
                //    RadLabel lblVesselId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselId");
                //    if (lblVesselAllocationId != null)
                //        ViewState["VESSELBUDGETID"] = lblVesselAllocationId.Text;
                //    if (lblVesselId != null)
                //        ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                //    Response.Redirect("../Common/CommonBudgetBillingByVessel.aspx?VESSELBUDGETALLOCATIONID=" + ViewState["VESSELBUDGETID"] + "&VESSELID=" + ViewState["VESSELID"] + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"], false);

                //}
                else if (e.CommandName.ToUpper().Equals("SAVE"))
                {
                    if (!IsValidVesselBudget(((UserControlDate)e.Item.FindControl("ucDate")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCommonBudgetGroupAllocation.InsertVesselBudgetAllocation(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int16.Parse(((RadLabel)e.Item.FindControl("lblVesselIdEdit")).Text)
                            , Int16.Parse(ucFinancialYear.SelectedQuick)
                            , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucDate")).Text)
                            , int.Parse(ViewState["VESSELACCOUNTID"].ToString()));

                    Rebind();
                    PeriodRebind();
                    VesselRebind();


                    //BindVesselAllocation();
                    //gvVesselAllocation.Rebind();
                    //BindBudgetGroup();
                    //BindBudgetPeriod();
                }
            }
            else
            {
                BindVesselAllocation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidVesselBudget(string effectivedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        if (General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null)
            ucError.ErrorMessage = "Financial Year is required.";

        return (!ucError.IsError);
    }

    protected void gvVesselAllocation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (ed != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ed.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey="
                   + drv["FLDDTKEY"].ToString() + "&mod=BUDGET&type=BUDGETREVISION'); return false;";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                // db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                // if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                if (drv["FLDLATEST"].ToString() == "0" && SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (drv["FLDLATEST"].ToString() == "0" && SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }



            RadLabel lblOverwritten = (RadLabel)e.Item.FindControl("lblOverwritten");

            if (lblOverwritten != null && lblOverwritten.Text.Trim().ToUpper().Equals("YES"))
                e.Item.ForeColor = System.Drawing.Color.Red;

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucPeriod");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDEFFECTIVEPERIODID"].ToString();

        }
    }
    protected void gvBudgetGroupAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.Item is GridDataItem)
            {
                if (e.CommandName.ToUpper().Equals("EDIT"))
                {
                    ViewState["BudgetGroupId"] = ((RadLabel)e.Item.FindControl("lblBudgetGroupId")).Text;
                    BindBudgetPeriod();
                    gvBudgetPeriodAllocation.Rebind();
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    RadLabel lblBudgetGroupAllocationID = (RadLabel)e.Item.FindControl("lblBudgetGroupAllocationId");
                    RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");

                    PhoenixCommonBudgetGroupAllocation.DeleteOwnerBudgetGroupAllocation(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(lblBudgetGroupAllocationID.Text),
                            General.GetNullableGuid(lblBudgetGroupId.Text));

                    BindVesselAllocation();
                    BindBudgetGroup();
                    BindBudgetPeriod();
                    gvBudgetGroupAllocation.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvBudgetGroupAllocation_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = de.RowIndex;
    //    try
    //    {
    //        RadLabel lblBudgetGroupAllocationID = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblBudgetGroupAllocationId");
    //        RadLabel lblBudgetGroupId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblBudgetGroupId");

    //        PhoenixCommonBudgetGroupAllocation.DeleteOwnerBudgetGroupAllocation(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(lblBudgetGroupAllocationID.Text),
    //                General.GetNullableGuid(lblBudgetGroupId.Text));

    //        BindVesselAllocation();
    //        BindBudgetGroup();
    //        BindBudgetPeriod();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvBudgetPeriodAllocation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCommitmentId = (RadLabel)e.Item.FindControl("lblCommitmentId");
            LinkButton lblCommittedAmount = (LinkButton)e.Item.FindControl("lblCommittedAmount");
            LinkButton lblPaidAmount = (LinkButton)e.Item.FindControl("lblPaidAmount");
            //LinkButton lblTotalAmount = (LinkButton)e.Row.FindControl("lblTotalAmount");
            RadLabel lblPeriod = (RadLabel)e.Item.FindControl("lblPeriod");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblYear = (RadLabel)e.Item.FindControl("lblYear");
            RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            RadLabel lblPeriodName = (RadLabel)e.Item.FindControl("lblPeriodName");
            if (lblCommittedAmount != null)
            {
                //lblCommittedAmount.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetCommittedCostBreakUp.aspx?commitmentid=" + lblCommitmentId.Text
                //    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                //    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                //    + "&budgetgroupid=" + ViewState["BudgetGroupId"] + "');return false;");

                lblCommittedAmount.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCommittedCostBreakUp.aspx?commitmentid=" + lblCommitmentId.Text
                   + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                   + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                   + "&budgetgroupid=" + ViewState["BudgetGroupId"] +
                   "');return false;");

            }

            if (lblPaidAmount != null)
            {
                //lblPaidAmount.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetCommittedCostBreakUpCharged.aspx?commitmentid=" + lblCommitmentId.Text
                //    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                //    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                //    + "&budgetgroupid=" + ViewState["BudgetGroupId"] + "');return false;");

                lblPaidAmount.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCommittedCostBreakUpCharged.aspx?commitmentid=" + lblCommitmentId.Text
    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
    + "&finyear=" + ucFinancialYear.SelectedQuick
    + "&budgetgroupid=" + ViewState["BudgetGroupId"] + "');return false;");
            }

            if (General.GetNullableDecimal(drv["FLDTOTALEXPENDITURE"].ToString()) != null)
            {
                RadLabel lblAccumulatedTotalAmount = (RadLabel)e.Item.FindControl("lblAccumulatedTotalAmount");
                accumulatedtotal = accumulatedtotal + decimal.Parse(drv["FLDTOTALEXPENDITURE"].ToString());
                lblAccumulatedTotalAmount.Text = accumulatedtotal.ToString();
            }
        }
    }

    protected void gvBudgetGroupAllocation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            RadLabel lblBudgetGroupAllocationID = (RadLabel)e.Item.FindControl("lblBudgetGroupAllocationId");
            RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            RadLabel lblBudgetGroup = (RadLabel)e.Item.FindControl("lblBudgetGroup");
            RadLabel lblVesselbudgetAllocationId = (RadLabel)e.Item.FindControl("lblVesselbudgetAllocationId");


            if (lblBudgetGroupAllocationID != null)
            {
                //db.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonOwnerBudgetGroupAllocationList.aspx?BudgetGroupAllocationId="
                //    + lblBudgetGroupAllocationID.Text + "&budgetgroupid=" + lblBudgetGroupId.Text + "&budgetgroupcode=" + lblBudgetGroup.Text + "&finyear="
                //    + ucFinancialYear.SelectedQuick + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"].ToString() + "');return false;");

                db.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonOwnerBudgetGroupAllocationList.aspx?BudgetGroupAllocationId="
                    + lblBudgetGroupAllocationID.Text + "&budgetgroupid=" + lblBudgetGroupId.Text + "&budgetgroupcode=" + lblBudgetGroup.Text + "&finyear="
                    + ucFinancialYear.SelectedQuick + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"].ToString() + "&vesselbudgetallocationid=" + lblVesselbudgetAllocationId.Text +
                         "');return false;");

            }

            LinkButton dbdel = (LinkButton)e.Item.FindControl("cmdDelete");
            if (dbdel != null)
            {
                dbdel.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, dbdel.CommandName)) dbdel.Visible = false;
            }

        }
    }

    protected void gvBudgetGroupAllocation_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindBudgetGroup();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindBudgetGroup();
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void SetRowSelectionOfVessel()
    {
        gvVesselAllocation.SelectedIndexes.Clear();
        if (ViewState["VESSELBUDGETID"] != null && ViewState["VESSELBUDGETID"].ToString() != "")
        {
            foreach (GridDataItem item in gvVesselAllocation.Items)
            {
               if (item.GetDataKeyValue("FLDVESSELBUDGETALLOCATIONID").ToString().Equals(ViewState["VESSELBUDGETID"].ToString()))
                {
                    gvVesselAllocation.SelectedIndexes.Add(item.ItemIndex);
                    RadLabel lblAccountId = (RadLabel)gvVesselAllocation.Items[item.ItemIndex].FindControl("lblAccountId");
                    if (lblAccountId != null)
                        ViewState["ACCOUNTID"] = lblAccountId.Text;
                }
            }
        }
    }

   // private void SetRowSelectionOfVessel()
   // {
        //gvVesselAllocation.SelectedIndex = -1;
        //if (ViewState["VESSELBUDGETID"] != null && ViewState["VESSELBUDGETID"].ToString() != "")
        //{
        //    for (int i = 0; i < gvVesselAllocation.Rows.Count; i++)
        //    {
        //        if (gvVesselAllocation.DataKeys[i].Value.ToString().Equals(ViewState["VESSELBUDGETID"].ToString()))
        //        {
        //            gvVesselAllocation.SelectedIndex = i;
        //            RadLabel lblAccountId = (RadLabel)gvVesselAllocation.Rows[i].FindControl("lblAccountId");

        //            if (lblAccountId != null)
        //                ViewState["ACCOUNTID"] = lblAccountId.Text;
        //        }
        //    }
        //}
  //  }

   
    private void SetRowSelectionOfBudgetGroup()
    {
        gvBudgetGroupAllocation.SelectedIndexes.Clear();

        foreach (GridDataItem item in gvBudgetGroupAllocation.Items)        {
            if (item.GetDataKeyValue("FLDOWNERBUDGETGROUPID").ToString().Equals(ViewState["BudgetGroupId"].ToString()))
            {
                gvBudgetGroupAllocation.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void gvVesselAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindVesselAllocation();
    }

    protected void gvBudgetGroupAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetGroupAllocation.CurrentPageIndex + 1;

        BindBudgetGroup();
    }

    protected void gvBudgetPeriodAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindBudgetPeriod();
    }

   
}
