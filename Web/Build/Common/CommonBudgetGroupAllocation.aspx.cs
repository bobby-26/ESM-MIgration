using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonBudgetGroupAllocation : PhoenixBasePage
{

    public decimal accumulatedtotal = 0;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Vessel List", "VESSEL", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELBUDGETID"] = null;
            gvBudgetGroupAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvBudgetPeriodAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvVesselAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["vesselaccountid"] != null)
                ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();
         //   if (Request.QueryString["accountid"] != null)
         //       ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();

    }



        MenuBudgetTab.SelectedMenuIndex = 1;

        toolbar = new PhoenixToolbar();


        toolbar.AddFontAwesomeButton("../Common/CommonBudgetGroupAllocation.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetGroupAllocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("", "Process Charged vouchers", "<i class=\"fas fa-file-alt\"></i>", "PROCESS");
        toolbar.AddFontAwesomeButton("", "Process Committed vouchers", "<i class=\"fas fa-file-alt\"></i>", "COMPROCESS");

        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        BindStartEndDate();

    }

    private void BindStartEndDate()
    {
        string vesselid = "";

        vesselid = Request.QueryString["vesselid"].ToString();

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
        //Rebind();
        //PeriodRebind();
        //VesselRebind();
      //  BindVesselAllocation();
        gvVesselAllocation.Rebind();
       // BindBudgetGroup();
        gvBudgetGroupAllocation.Rebind();
        gvBudgetPeriodAllocation.Rebind();
        BindStartEndDate();
        SetRowSelectionOfVessel();
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
        string[] alColumns = { "FLDBUDGETGROUPNAME", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDACCESSNAME", "FLDAPPORTIONMENTMETHODNAME" };
        string[] alCaptions = { "Budget Group", "Budget Amount", "Allowance", "Access", "Apportionment" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetGroupAllocation.BudgetGroupAllocationSearch(
            General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
             , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
            , sortexpression, sortdirection,
            1,
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("PROCESS"))
        {

            PhoenixCommonBudgetGroupAllocation.BudgetChargedVouchersUpdate(null
           , General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
           , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
           , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : ""));

            BindBudgetPeriod();
            ucSatus.Text = "Charged vouchers are processed";
            ucSatus.Visible = true;
        }
        if (CommandName.ToUpper().Equals("COMPROCESS"))
        {
            PhoenixCommonBudgetGroupAllocation.BudgetCommittedVouchersUpdate(General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
             , General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : ""));

            BindBudgetPeriod();
            ucSatus.Text = "Committed Vouchers are processed";
            ucSatus.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //Rebind();
        //PeriodRebind();
        //VesselRebind();
        //BindStartEndDate();
        gvBudgetGroupAllocation.Rebind();
        gvBudgetPeriodAllocation.Rebind();
        gvVesselAllocation.Rebind();
        BindStartEndDate();
      // SetRowSelectionOfBudgetGroup();

    }

    private void BindVesselAllocation()
    {
        try
        {
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string vesselid = Request.QueryString["vesselid"].ToString();
            string ownerid = "", currentfinyear = "";

            DataSet dsVessel = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));

            if (dsVessel.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVessel.Tables[0].Rows[0];

                ownerid = dr["FLDOWNER"].ToString();
                currentfinyear = dr["FLDCURRENTFINYEARID"].ToString();
                ViewState["currentyear"] = dr["FLDCURRENTFINYEARID"].ToString();
                if (!IsPostBack)
                    ucFinancialYear.SelectedQuick = currentfinyear;
            }
            DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselAllocationSearch(
                  General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(currentfinyear) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
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

        string[] alColumns = { "FLDBUDGETGROUPNAME", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDACCESSNAME", "FLDAPPORTIONMENTMETHODNAME" };
        string[] alCaptions = { "Budget Group", "Budget Amount", "Allowance", "Access", "Apportionment" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetGroupAllocationSearch(
            General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
             , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
            , sortexpression, sortdirection,
            gvBudgetGroupAllocation.CurrentPageIndex + 1,
            gvBudgetGroupAllocation.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetGroupAllocation", "Budget Group Allocation", alCaptions, alColumns, ds);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBudgetGroupAllocation.DataSource = ds;

            if (!IsPostBack)
            {
                ViewState["BudgetGroupId"] = ds.Tables[0].Rows[0]["FLDBUDGETGROUPID"].ToString();
            }
         //  SetRowSelectionOfBudgetGroup();
        }
        else
        {
            gvBudgetGroupAllocation.DataSource = ds;
        }
        gvBudgetGroupAllocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    private void BindBudgetPeriod()
    {


        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetPeriodAllocationSearch(
          Int32.Parse(((ViewState["BudgetGroupId"] == null) || (ViewState["BudgetGroupId"].ToString().Equals(""))) ? "0" : ViewState["BudgetGroupId"].ToString())
           , General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? General.GetNullableInteger(ViewState["currentyear"].ToString()) : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
           , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
           , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
           , General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "")
            );


        gvBudgetPeriodAllocation.DataSource = ds;

        accumulatedtotal = 0;
    }
    protected void gvVesselAllocation_DeleteCommand(object sender, GridCommandEventArgs de)
    {

        try
        {
            RadLabel lblVesselAllocationId = (RadLabel)de.Item.FindControl("lblVesselAllocationId");

            PhoenixCommonBudgetGroupAllocation.BudgetRevisionDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lblVesselAllocationId.Text));

            BindVesselAllocation();
            gvBudgetGroupAllocation.Rebind();
            BindBudgetPeriod();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //StoresubclassDelete(Int32.Parse(ViewState["ID"].ToString()));
            PhoenixCommonBudgetGroupAllocation.BudgetRevisionDelete(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(ViewState["vesselallocationid"].ToString()));

            Rebind();
            PeriodRebind();
            VesselRebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvVesselAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                //  BindVesselAllocation();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //RadLabel lblVesselAllocationId = (RadLabel)e.Item.FindControl("lblVesselAllocationId");

                //PhoenixCommonBudgetGroupAllocation.BudgetRevisionDelete(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //        new Guid(lblVesselAllocationId.Text));

                ViewState["vesselallocationid"] = ((RadLabel)e.Item.FindControl("lblVesselAllocationId")).Text;
                RadWindowManager1.RadConfirm("There are owner budget allocation budget amount in this revision.Deleting this revision will remove owner budget allocation amount as well.Are you sure you want to proceed ? ", "DeleteRecord", 320, 150, null, "Delete");

                return;

                //BindVesselAllocation();
                //BindBudgetGroup();
                //BindBudgetPeriod();

                //Rebind();
                //PeriodRebind();
                //VesselRebind();

                //gvBudgetGroupAllocation.Rebind();
                //gvBudgetPeriodAllocation.Rebind();
                //gvVesselAllocation.Rebind();


            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                // Filter.CurrentBudgetAllocationVesselFilter = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                ViewState["VESSELBUDGETID"] = ((RadLabel)e.Item.FindControl("lblVesselAllocationId")).Text;
                Rebind();
                PeriodRebind();

                // gvBudgetGroupAllocation.Rebind();
                //  gvBudgetPeriodAllocation.Rebind();

                //   BindBudgetGroup();
                //  BindBudgetPeriod();
            }
            else if (e.CommandName.ToUpper().Equals("BUDGETBREAKDOWN"))
            {
                RadLabel lblVesselAllocationId = (RadLabel)e.Item.FindControl("lblVesselAllocationId");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                int lblAccountId = int.Parse(((RadLabel)e.Item.FindControl("lblAccountId")).Text);

                if (lblVesselAllocationId != null)
                    ViewState["VESSELBUDGETID"] = lblVesselAllocationId.Text;
                if (lblVesselId != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                if (ViewState["VESSELBUDGETID"].ToString() != "")
                {
                    Response.Redirect("../Common/CommonBudgetBillingByVessel.aspx?VESSELBUDGETALLOCATIONID=" + ViewState["VESSELBUDGETID"] + "&VESSELID=" + ViewState["VESSELID"] + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"] + "&accountid=" + lblAccountId, false);
                }
                else
                {
                    ucError.ErrorMessage = "Budget Amount is not Define for this Vessel";
                    ucError.Visible = true;
                    return;
                }

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
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


            }
            // else
            //  {
            //      VesselRebind();
            //  }
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



    protected void gvVesselAllocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
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
                //if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

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

    protected void gvVesselAllocation_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVesselAllocation.SelectedIndexes.Add(e.NewSelectedIndex);
        string vesselbudgetid = ((RadLabel)gvVesselAllocation.Items[e.NewSelectedIndex].FindControl("lblVesselAllocationId")).Text;

        ViewState["VESSELBUDGETID"] = vesselbudgetid;
        BindVesselAllocation();
        BindBudgetGroup();
    }
    protected void gvBudgetGroupAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel TI = ((RadLabel)e.Item.FindControl("lblBudgetGroupId"));
                string DD = TI.Text;
                ViewState["BudgetGroupId"] = ((RadLabel)e.Item.FindControl("lblBudgetGroupId")).Text;
                BindBudgetPeriod();
                gvBudgetPeriodAllocation.Rebind();

            }
            else
            {
                Rebind();
                PeriodRebind();
              //  VesselRebind();
     

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetPeriodAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
            }
            else
            {
                gvBudgetGroupAllocation.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetGroupAllocation_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        gvBudgetGroupAllocation.Rebind();
    }

    protected void gvBudgetGroupAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        gvBudgetGroupAllocation.Rebind();
    }

    protected void gvBudgetPeriodAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvBudgetPeriodAllocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCommitmentId = (RadLabel)e.Item.FindControl("lblCommitmentId");
            LinkButton lblCommittedAmount = (LinkButton)e.Item.FindControl("lblCommittedAmount");
            LinkButton lblPaidAmount = (LinkButton)e.Item.FindControl("lblPaidAmount");
            RadLabel lblPeriod = (RadLabel)e.Item.FindControl("lblPeriod");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblYear = (RadLabel)e.Item.FindControl("lblYear");
            RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            RadLabel lblPeriodName = (RadLabel)e.Item.FindControl("lblPeriodName");
            RadLabel lblAllocationId = (RadLabel)e.Item.FindControl("lblAllocationId");

            ImageButton ibBudgetCode = (ImageButton)e.Item.FindControl("cmdBudgetCode");
            if (ibBudgetCode != null)
            {
                ibBudgetCode.Visible = SessionUtil.CanAccess(this.ViewState, ibBudgetCode.CommandName);

                //ibBudgetCode.Attributes.Add("onclick"
                //       , "parent.Openpopup('codehelp1', '', 'CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + drv["FLDACCOUNTID"].ToString()
                //       + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "&finyear=" + drv["FLDFINANCIALYEAR"].ToString() + "&period=" + drv["FLDPERIOD"].ToString() + "&periodallocationid=" + lblAllocationId.Text +
                //        "');return false;");

                ibBudgetCode.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + drv["FLDACCOUNTID"].ToString()
                       + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "&finyear=" + drv["FLDFINANCIALYEAR"].ToString() + "&period=" + drv["FLDPERIOD"].ToString() + "&periodallocationid=" + lblAllocationId.Text +
                                     "');return false;");


            }

            LinkButton lnkPeriodName = (LinkButton)e.Item.FindControl("lnkPeriodName");
            if (lnkPeriodName != null)
            {
                //lnkPeriodName.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + drv["FLDACCOUNTID"].ToString()
                //    + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "&finyear=" + drv["FLDFINANCIALYEAR"].ToString() + "&period=" + drv["FLDPERIOD"].ToString() + "&periodallocationid=" + lblAllocationId.Text +
                //     "');return false;");

                lnkPeriodName.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + drv["FLDACCOUNTID"].ToString()
                    + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "&finyear=" + drv["FLDFINANCIALYEAR"].ToString() + "&period=" + drv["FLDPERIOD"].ToString() + "&periodallocationid=" + lblAllocationId.Text +
                                    "');return false;");

            }


            if (lblCommittedAmount != null)
            {
                //lblCommittedAmount.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetCommittedCostBreakUp.aspx?commitmentid=" + lblCommitmentId.Text
                //    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                //    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                //    + "&budgetgroupid=" + lblBudgetGroupId.Text + "');return false;");

                lblCommittedAmount.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCommittedCostBreakUp.aspx?commitmentid=" + lblCommitmentId.Text
                    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                    + "&budgetgroupid=" + lblBudgetGroupId.Text +
                    "');return false;");

            }

            if (lblPaidAmount != null)
            {
                //lblPaidAmount.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetCommittedCostBreakUpCharged.aspx?commitmentid=" + lblCommitmentId.Text
                //    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                //    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                //    + "&finyear=" + ucFinancialYear.SelectedQuick
                //    + "&budgetgroupid=" + lblBudgetGroupId.Text + "');return false;");

                lblPaidAmount.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetCommittedCostBreakUpCharged.aspx?commitmentid=" + lblCommitmentId.Text
                    + "&vesselid=" + lblVesselId.Text + "&accountid=" + lblAccountId.Text
                    + "&year=" + lblYear.Text + "&month=" + lblPeriod.Text
                    + "&finyear=" + ucFinancialYear.SelectedQuick
                    + "&budgetgroupid=" + lblBudgetGroupId.Text + "');return false;");
            }

            if (General.GetNullableDecimal(drv["FLDTOTALEXPENDITURE"].ToString()) != null)
            {
                RadLabel lblAccumulatedTotalAmount = (RadLabel)e.Item.FindControl("lblAccumulatedTotalAmount");
                accumulatedtotal = accumulatedtotal + decimal.Parse(drv["FLDTOTALEXPENDITURE"].ToString());
                lblAccumulatedTotalAmount.Text = accumulatedtotal.ToString();
            }


            /* Bug Id: 8330 */
            //if (lblTotalAmount != null)
            //{
            //    lblTotalAmount.Attributes.Add("onclick"
            //        , "parent.Openpopup('codehelp1', '', 'CommonBudgetCommittedCostBreakUp.aspx?commitmentid=" + lblCommitmentId.Text + "&commitorpaid=3');return false;");
            //}
        }
    }

    protected void gvBudgetGroupAllocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            RadLabel lblBudgetGroupAllocationID = (RadLabel)e.Item.FindControl("lblBudgetGroupAllocationId");
            RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            RadLabel lblVesselbudgetAllocationId = (RadLabel)e.Item.FindControl("lblVesselbudgetAllocationId");


            if (lblBudgetGroupAllocationID != null)
            {
                //db.Attributes.Add("onclick"
                //    , "parent.Openpopup('codehelp1', '', 'CommonBudgetGroupAllocationList.aspx?BudgetGroupAllocationId="
                //    + lblBudgetGroupAllocationID.Text + "&budgetgroupid=" + lblBudgetGroupId.Text + "&finyear=" 
                //    + ucFinancialYear.SelectedQuick + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"].ToString() + "');return false;");

                db.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonBudgetGroupAllocationList.aspx?BudgetGroupAllocationId="
                    + lblBudgetGroupAllocationID.Text + "&budgetgroupid=" + lblBudgetGroupId.Text + "&finyear="
                    + ucFinancialYear.SelectedQuick + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"].ToString() +  "&vesselbudgetallocationid=" + lblVesselbudgetAllocationId.Text + "');return true;");

            }

            LinkButton ibBudgetCode = (LinkButton)e.Item.FindControl("cmdBudgetCode");
            if (ibBudgetCode != null)
            {
                ibBudgetCode.Visible = SessionUtil.CanAccess(this.ViewState, ibBudgetCode.CommandName);

                //ibBudgetCode.Attributes.Add("onclick"
                //    , "parent.Openpopup('BudgetGroup', '', 'CommonBudgetCodeAllocation.aspx?budgetgroupallocationid="
                //    + drv["FLDBUDGETGROUPALLOCATIONID"].ToString() + "&vesselaccountid=" + drv["FLDACCOUNTID"].ToString() + "&vesselbudgetallocationid=" + General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "") + "');return false;");

                ibBudgetCode.Attributes.Add("onclick", "openNewWindow('BudgetGroup', '', '" + Session["sitepath"] + "/Common/CommonBudgetCodeAllocation.aspx?budgetgroupallocationid="
                    + drv["FLDBUDGETGROUPALLOCATIONID"].ToString() + "&vesselaccountid=" + drv["FLDACCOUNTID"].ToString() + "&vesselbudgetallocationid=" + General.GetNullableGuid(ViewState["VESSELBUDGETID"] != null ? ViewState["VESSELBUDGETID"].ToString() : "") + "');return true;");

                //ibBudgetCode.Attributes.Add("onclick"
                //       , "parent.Openpopup('codehelp1', '', 'CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + drv["FLDACCOUNTID"].ToString()
                //       + "&budgetgroupid=" + drv["FLDBUDGETGROUPID"].ToString() + "&finyear=" + drv["FLDFINANCIALYEAR"].ToString() + "&period=" + drv["FLDPERIOD"].ToString() + "&periodallocationid=" + lblAllocationId.Text +
                //        "');return false;");
            }
        }
    }

    protected void gvBudgetGroupAllocation_SortCommand(object sender, GridSortCommandEventArgs e)
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
     
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindBudgetGroup();
    }

    //private void SetRowSelectionOfVessel()
    //{

    //    for (int i = 0; i < gvVesselAllocation.Items.Count; i++)
    //    {
    //        if (gvVesselAllocation.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["VESSELBUDGETID"].ToString()))
    //        {
    //            gvVesselAllocation.MasterTableView.Items[i].Selected = true;
    //            break;
    //          //  RadLabel lblAccountId = (RadLabel)gvVesselAllocation.Items[item.ItemIndex].FindControl("lblAccountId");

    //          //  if (lblAccountId != null)
    //            //    ViewState["ACCOUNTID"] = lblAccountId.Text;

    //        }
    //    }

    //}

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

    private void SetRowSelectionOfBudgetGroup()
    {
        gvBudgetGroupAllocation.SelectedIndexes.Clear();

        foreach (GridDataItem item in gvBudgetGroupAllocation.Items)
        {
            if (item.GetDataKeyValue("FLDBUDGETGROUPID").ToString().Equals(ViewState["BudgetGroupId"].ToString()))
            {
                gvBudgetGroupAllocation.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }


    protected void gvBudgetGroupAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetGroupAllocation.CurrentPageIndex + 1;
            BindBudgetGroup();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselAllocation.CurrentPageIndex + 1;
            BindVesselAllocation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetPeriodAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetPeriodAllocation.CurrentPageIndex + 1;
            BindBudgetPeriod();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
