using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersOwnerBudgetCodeMap : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    BindData();
    //    BindESMBudgetCodes();
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

     

        toolbar.AddFontAwesomeButton("../Registers/RegistersOwnerBudgetCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOwnerBudgetCodeMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuOwnerBudgetCodeMap.AccessRights = this.ViewState;
        MenuOwnerBudgetCodeMap.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
      
        toolbar.AddFontAwesomeButton("../Registers/RegistersOwnerBudgetCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvESMBudgetCode')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        ESMBudgetCode.AccessRights = this.ViewState;
        ESMBudgetCode.MenuList = toolbar.Show();



        if (!IsPostBack)
        {

            //MenuOwnerBudgetCodeMap.SetTrigger(pnlOwnreBudgetcodeEntry);



            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["PAGENUMBER1"] = 1;
            ViewState["SORTEXPRESSION1"] = null;
            ViewState["SORTDIRECTION1"] = null;
            ViewState["CURRENTINDEX1"] = 1;

            ViewState["BUDGETID"] = "";
            ViewState["BUDGETNAME"] = "";
            gvESMBudgetCode.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        }
        BindESMBudgetCodes();
        BindData();

        //SetPageNavigatorESM();

        //SetPageNavigator();
    }

    private void BindESMBudgetCodes()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDOWNERBUDGETCODEEXISTS" };
        string[] alCaptions = { "ESM Budget Code", "ESM Budget Code Description", "Owner Budget Code Mapped" };

        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        DataSet ds = PhoenixRegistersBudget.ESMOwnerBudgetCodeSearch(
            General.GetNullableInteger(ucOwner.SelectedAddress) == null ? 0 : General.GetNullableInteger(ucOwner.SelectedAddress),
            sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvESMBudgetCode.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        //PhoenixRegistersBudget.BudgetSearch(null, "", "", null, null, null
        //    , sortexpression, sortdirection,
        //    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        //    General.ShowRecords(null),
        //    ref iRowCount,
        //    ref iTotalPageCount);

        General.SetPrintOptions("gvESMBudgetCode", "ESM Budget Codes", alCaptions, alColumns, ds);

            gvESMBudgetCode.DataSource = ds;
        gvESMBudgetCode.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT1"] = iRowCount;
 
    }
    protected void gvESMBudgetCode_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvESMBudgetCode.CurrentPageIndex + 1;
            BindESMBudgetCodes();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvESMBudgetCode_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["BUDGETID"] = ((RadLabel)e.Item.FindControl("lblbudgetid")).Text;
                ViewState["BUDGETNAME"] = ((LinkButton)e.Item.FindControl("lnkBudgetCode")).Text;

                BindData();
                //SetPageNavigator();
                gvOwnerBudgetCodeMap.Rebind();
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

  
    protected void gvESMBudgetCode_OnSortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void ShowExcelESM()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDOWNERBUDGETCODEEXISTS" };
        string[] alCaptions = { "ESM Budget Code", "ESM Budget Code Description", "Owner Budget Code Mapped" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        DataSet ds = PhoenixRegistersBudget.ESMOwnerBudgetCodeSearch(
            General.GetNullableInteger(ucOwner.SelectedAddress),
            sortexpression, sortdirection,
              1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ESMBudgetCode.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ESM Budget Codes </h3></td>");
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

    protected void ESMBudgetCode_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER1"] = 1;
            BindESMBudgetCodes();
            //SetPageNavigatorESM();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelESM();
        }
    }

  

    /// <summary>
    /// ///////// Owner Budget Code Map GridView Begins..
    /// </summary>

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDNOTINCULDEINOWNERREPORTYN" };
        string[] alCaptions = { "Budget Code", "Owner Budget Code", "Not Included in Owners" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixCommonRegisters.OwnerBudgetCodeSearch(
        //    General.GetNullableInteger(ucOwner.SelectedAddress), null,
        //    General.GetNullableInteger(ViewState["BUDGETID"].ToString())
        //    , sortexpression, sortdirection,
        //    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        //    iRowCount,
        //    ref iRowCount,
        //    ref iTotalPageCount);
        ds = PhoenixCommonRegisters.OwnerBudgetCodeMapSearch(
    General.GetNullableInteger(ucOwner.SelectedAddress) == null ? 0 : General.GetNullableInteger(ucOwner.SelectedAddress), null,
    General.GetNullableInteger(ViewState["BUDGETID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["BUDGETID"].ToString())
    , sortexpression, sortdirection,
                1,
               iRowCount,
              ref iRowCount,
              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OwnerBudgetCode.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Owner Budget Code Map</h3></td>");
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

    protected void OwnerBudgetCodeMap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            //SetPageNavigator();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDNOTINCULDEINOWNERREPORTYN" };
        string[] alCaptions = { "Budget Code", "Owner Budget Code", "Not Included in Owners" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonRegisters.OwnerBudgetCodeMapSearch(
            General.GetNullableInteger(ucOwner.SelectedAddress) == null ? 0 : General.GetNullableInteger(ucOwner.SelectedAddress), null,
            General.GetNullableInteger(ViewState["BUDGETID"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["BUDGETID"].ToString())
            , sortexpression, sortdirection,
           gvOwnerBudgetCodeMap.CurrentPageIndex+1,
            gvOwnerBudgetCodeMap.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvOwnerBudgetCodeMap", "Owner Budget Code Map", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvOwnerBudgetCodeMap.DataSource = ds.Tables[0];
        gvOwnerBudgetCodeMap.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        //    gvOwnerBudgetCodeMap.DataBind();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //ShowNoRecordsFound(dt, gvOwnerBudgetCodeMap);
        //}

        //UserControlOwnerBudgetGroup ucOwnerBudgetGroup = (UserControlOwnerBudgetGroup)gvOwnerBudgetCodeMap.FooterRow.FindControl("ucOwnerBudgetGroup");
        //if (ucOwnerBudgetGroup != null)
        //{
        //    ucOwnerBudgetGroup.BudgetCodeList = PhoenixRegistersBudget.ListOwnerBudgetGroup(General.GetNullableInteger(ucOwner.SelectedAddress));
        //}


        //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvOwnerBudgetCodeMap_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnerBudgetCodeMap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   

    protected void gvOwnerBudgetCodeMap_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (ViewState["BUDGETID"] == null)
                {
                    ucError.ErrorMessage = "Please select one 'ESM budget code' to add the Owner budget code.";
                    return;
                }
                if (!IsValidOwnerBudgetCode(((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeId")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertOwnerBudgetCode(
                    ucOwner.SelectedAddress
                    , ViewState["BUDGETID"].ToString()
                    , ((RadTextBox)e.Item.FindControl("txtParentgroupId")).Text
                    , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeId")).Text
                    , (((RadCheckBox)e.Item.FindControl("chkIncludeYNAdd")).Checked==true) ? 0 : 1
                    , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0);
                BindData();
                gvOwnerBudgetCodeMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidOwnerBudgetCode(((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeIdEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOwnerBudgetCode(((RadLabel)e.Item.FindControl("lblOwnerBudgetCodeId")).Text
                                             , ((RadTextBox)e.Item.FindControl("txtParentgroupIdEdit")).Text
                                            , ((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeIdEdit")).Text
                                            , (((RadCheckBox)e.Item.FindControl("chkIncludeYNEdit")).Checked==true) ? 0 : 1
                                            , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true) ? 1 : 0
                                            );
                //_gridView.EditIndex = -1;
                BindData();
                gvESMBudgetCode.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindData();
                gvOwnerBudgetCodeMap.Rebind();
                //_gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOwnerBudgetCode(((RadLabel)e.Item.FindControl("lblOwnerBudgetCodeId")).Text);
                BindData();
                gvESMBudgetCode.Rebind();
            }
            //SetPageNavigator();
            BindESMBudgetCodes();
            BindData();
            //SetPageNavigatorESM();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void gvOwnerBudgetCodeMap_OnSortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvOwnerBudgetCodeMap_OnDeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
        //SetPageNavigator();
    }

  

    protected void gvOwnerBudgetCodeMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        if (e.Item is GridEditableItem)
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
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowOwnerBudgetEdit");

            if ((ib1 != null) && (General.GetNullableInteger(ucOwner.SelectedAddress) != null))
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudgetCodeTree.aspx?iframignore=true&OWNERID=" + ucOwner.SelectedAddress + "', true); ");

        }


        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowOwnerBudgetAdd");

            if ((ib1 != null) && (General.GetNullableInteger(ucOwner.SelectedAddress)!=null))
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudgetCodeTree.aspx?iframignore=true&OWNERID=" + ucOwner.SelectedAddress + "', true); ");

            RadTextBox TB = (RadTextBox)e.Item.FindControl("txtEsmBudgetadd");
            if (TB != null && ViewState["BUDGETNAME"] != null)
                TB.Text = ViewState["BUDGETNAME"].ToString();
        }

    }

  
  

    private void InsertOwnerBudgetCode(string ownerid, string budgetid, string ownerbudgetgroup, string ownerbudgetcode, int Isnotincludedinownerreport)
    {

        PhoenixRegistersBudget.InsertOwnerBudgetCode(
             int.Parse(ownerid)
            , int.Parse(budgetid)
            , General.GetNullableGuid(ownerbudgetgroup)
            , General.GetNullableGuid(ownerbudgetcode)
            , Isnotincludedinownerreport);

    }

    private void InsertOwnerBudgetCode(string ownerid, string budgetid, string ownerbudgetgroup, string ownerbudgetcode, int Isnotincludedinownerreport, int activeyn)
    {

        PhoenixRegistersBudget.InsertOwnerBudgetCode(
             int.Parse(ownerid)
            , int.Parse(budgetid)
            , General.GetNullableGuid(ownerbudgetgroup)
            , General.GetNullableGuid(ownerbudgetcode)
            , Isnotincludedinownerreport
            , activeyn);

    }

    private void UpdateOwnerBudgetCode(string ownerbudgetcodemapid, string ownerbudgetgroup, string ownerbudegtcode, int Isnotincludedinownerreport)
    {
        PhoenixRegistersBudget.UpdateOwnerBudgetCode(
             General.GetNullableGuid(ownerbudgetcodemapid)
             , General.GetNullableGuid(ownerbudgetgroup)
             , General.GetNullableGuid(ownerbudegtcode)
            , Isnotincludedinownerreport);
    }


    private void UpdateOwnerBudgetCode(string ownerbudgetcodemapid, string ownerbudgetgroup, string ownerbudegtcode, int Isnotincludedinownerreport, int activeyn)
    {
        PhoenixRegistersBudget.UpdateOwnerBudgetCode(
             General.GetNullableGuid(ownerbudgetcodemapid)
             , General.GetNullableGuid(ownerbudgetgroup)
             , General.GetNullableGuid(ownerbudegtcode)
            , Isnotincludedinownerreport
            , activeyn
            );
    }

    private bool IsValidOwnerBudgetCode(string ownerbudgetcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucOwner.SelectedAddress) == null)
            ucError.ErrorMessage = "Owner is required.";

        if (ViewState["BUDGETID"] == null || General.GetNullableInteger(ViewState["BUDGETID"].ToString()) == null)
            ucError.ErrorMessage = "Budget Code is required.";

        if (string.IsNullOrEmpty(ownerbudgetcode))
            ucError.ErrorMessage = "Owner Budget Code is required.";

        return (!ucError.IsError);
    }

    private void DeleteOwnerBudgetCode(string ownerbudgetcodeid)
    {
        PhoenixRegistersBudget.DeleteOwnerBudgetCode(new Guid(ownerbudgetcodeid));
    }

   

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
