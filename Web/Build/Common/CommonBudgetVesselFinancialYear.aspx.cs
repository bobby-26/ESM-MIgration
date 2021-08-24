using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonBudgetVesselFinancialYear : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {        
        SessionUtil.PageAccessRights(this.ViewState);
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Common/CommonBudgetVesselFinancialYear.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgFinancialYearSetup')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Common/CommonBudgetVesselFinancialYear.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
       
       
        toolbar.AddButton("Owner Reporting Format", "OWNERREPORT", ToolBarDirection.Right);
        toolbar.AddButton("Financial Year", "FINANCIALYEAR", ToolBarDirection.Right);
        toolbar.AddButton("Vessel List", "VESSELLIST", ToolBarDirection.Right);



        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        MenuBudgetTab.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            txtAccountId.Attributes.Add("style", "visibility:hidden");
            txtVesselId.Attributes.Add("style", "visibility:hidden");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ISFIRSTYEAR"] = "YES";
            dgFinancialYearSetup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

         }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;              
        int? sortdirection = null;
        int? iFinancialYear = null;
        string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR" };
        string[] alCaptions = { "Vessel Name", "Financial Start Year", "Financial End Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        vesselid = txtVesselId.Text.Equals("") ? "1" : txtVesselId.Text;

        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselFinancialYear.VesselFinancialYearSearch(
            General.GetNullableInteger(vesselid)
            , General.GetNullableInteger(txtAccountId.Text)
            , iFinancialYear
            , sortexpression, sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselFinanacialYear.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Financial Year</h3></td>");
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

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VESSELLIST"))
        {
            Response.Redirect("../Common/CommonBudgetVesselList.aspx?", false);
        }
        if (CommandName.ToUpper().Equals("OWNERREPORT"))
        {
            Response.Redirect("../Registers/RegistersOwnerReportingFormat.aspx?", false);
        }
    } 

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
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
            return;
        }
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iFinancialYear = null;
        string vesselid = "";
        string accountid = "";

        string[] alColumns = { "FLDVESSELNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR" };
        string[] alCaptions = { "Vessel Name", "Financial Start Year", "Financial End Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        vesselid = txtVesselId.Text.Equals("") ? "1" : txtVesselId.Text;
        accountid = txtAccountId.Text.Equals("") ? "1" : txtAccountId.Text;
        string acdesc = txtAccountDescription.Text;
        string accode = txtAccountCode.Text;

        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        DataSet ds = PhoenixRegistersVesselFinancialYear.VesselFinancialYearSearch(
            General.GetNullableInteger(vesselid)
            , General.GetNullableInteger(txtAccountId.Text)
            , iFinancialYear
            , sortexpression, sortdirection
            , dgFinancialYearSetup.CurrentPageIndex+1
            , dgFinancialYearSetup.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["ISFIRSTYEAR"] = "NO";
        }
        dgFinancialYearSetup.DataSource = ds;
            dgFinancialYearSetup.VirtualItemCount = iRowCount;
     
         General.SetPrintOptions("dgFinancialYearSetup", "Financial Year", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
     //   SetPageNavigator();
    }

    protected void dgFinancialYearSetup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        dgFinancialYearSetup.Rebind();
        


        if (e.CommandName.ToUpper().Equals("ADD"))
        {

            InsertFinancialYearSetup(
                ((UserControlDate)e.Item.FindControl("txtFinancialStartYearAdd")).Text,
                General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtFinancialYear")).Text)
            );
            Rebind();
       //     dgFinancialYearSetup.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateFinancialYearSetup(((RadTextBox)e.Item.FindControl("txtMapCode")).Text
                , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtFinancialYearEdit")).Text)
                , ((UserControlDate)e.Item.FindControl("txtFinancialStartYearEdit")).Text
                , ((UserControlDate)e.Item.FindControl("txtFinancialEndYearEdit")).Text);

           Rebind();
            //dgFinancialYearSetup.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            Rebind();
         //   dgFinancialYearSetup.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeletedFinancialYearSetup(((RadTextBox)e.Item.FindControl("txtMapCode")).Text);
            BindData();            
        }       
        else
        {
            BindData();
        }
    }
    protected void Rebind()
    {
        dgFinancialYearSetup.SelectedIndexes.Clear();
        dgFinancialYearSetup.EditIndexes.Clear();
        dgFinancialYearSetup.DataSource = null;
        dgFinancialYearSetup.Rebind();
    }
    protected void dgFinancialYearSetup_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void dgFinancialYearSetup_SortCommand(object sender, GridSortCommandEventArgs e)
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



    protected void dgFinancialYearSetup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

  

    protected void dgFinancialYearSetup_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        //if (e.Item is GridDataItem)
        //{
        //    // Get the LinkButton control in the first cell
        //    LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
        //    // Get the javascript which is assigned to this LinkButton
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    // Add this javascript to the onclick Attribute of the row
        //    e.Item.Attributes["ondblclick"] = _jsDouble;
        //}

        if (e.Item is GridDataItem)
        {
            RadLabel lblIsRecentFinancialYear = (RadLabel)e.Item.FindControl("lblIsRecentFinancialYear");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (lblIsRecentFinancialYear.Text == "1")
            {
                if (db != null) db.Visible = true;

                        }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

  
    protected void InsertFinancialYearSetup(string strFinancialStartYear, int? iYear)
    {
        if (!IsValidData(strFinancialStartYear != null ? strFinancialStartYear : string.Empty))
        {
            ucError.Visible = true;
            return;
        }

        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(txtVesselId.Text), 
                General.GetNullableDateTime(strFinancialStartYear), 
                iYear,
                General.GetNullableInteger(txtAccountId.Text));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateFinancialYearSetup(string strMapCode, int? iYear, string strFinancialYearStartDate, string strFinancialYearEndDate)
    {
        if (!IsValidDate(strFinancialYearStartDate, strFinancialYearEndDate))
        {
            ucError.Visible = true;
            return;
        }
        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapUpdate(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(strMapCode), int.Parse(txtVesselId.Text), 
                iYear, DateTime.Parse(strFinancialYearEndDate),
                General.GetNullableInteger(txtAccountId.Text));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeletedFinancialYearSetup(string strMapCode)
    {
        try
        {
            PhoenixRegistersVesselFinancialYear.VesselFinancialYearMapDelete(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(strMapCode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidVessel()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(txtVesselId.Text) == null)
            ucError.ErrorMessage = "Vessel account is required.";
        
        return (!ucError.IsError);
    }

    private bool IsValidData(string strFinancialStartYear)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(txtAccountId.Text) == null)
            ucError.ErrorMessage = "Please select a vessel account";

        if (strFinancialStartYear.Trim().Equals("") && ViewState["ISFIRSTYEAR"].ToString() == "YES")
            ucError.ErrorMessage = "Financial start year is required";

        return (!ucError.IsError);
    }

    private bool IsValidDate(string StartDate, string EndDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtEndDate = null;

        if (EndDate == null)
            ucError.ErrorMessage = "Financial Year End Date is required.";
        else
            dtEndDate = DateTime.Parse(EndDate);

        if (dtEndDate < DateTime.Parse(StartDate))
            ucError.ErrorMessage = "Financial Year End Date should be later than Financial Year Start Date.";

        return (!ucError.IsError);
    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
     //   SetPageNavigator();
     
    }



    protected void dgFinancialYearSetup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgFinancialYearSetup.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
