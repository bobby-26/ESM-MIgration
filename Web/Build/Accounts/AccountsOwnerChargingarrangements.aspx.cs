using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsOwnerChargingarrangements : PhoenixBasePage
{
    public int iUserCode;
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    //foreach (GridViewRow r in dgOwnerchargearrangements.Rows)
    //    //{
    //    //    if (r.RowType == DataControlRowType.DataRow)
    //    //    {
    //    //        Page.ClientScript.RegisterForEventValidation(dgOwnerchargearrangements.UniqueID, "Edit$" + r.RowIndex.ToString());
    //    //    }
    //    //}
    //    //base.Render(writer);
    //}
  
    protected void Page_Load(object sender, EventArgs e)
    {
        
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsOwnerChargingarrangements.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgOwnerchargearrangements')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Accounts/AccountsOwnerChargingarrangements.aspx", "Find", "search.png", "FIND");

        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        //MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        toolbarMain.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);
        toolbarMain.AddButton("Invoice", "INVOICE",ToolBarDirection.Right);
      
        MenuOwner.Title = "Owner Charging arrangements";
        MenuOwner.AccessRights = this.ViewState;
        MenuOwner.MenuList = toolbarMain.Show();

        if (!IsPostBack)
        {
           
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            dgOwnerchargearrangements.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PRINCIPALID"] = null;

         
        }

      //  BindData();
      
    }

    protected void MenuOwner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsOwnerChargingarrangements.aspx");
            }
            else if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                if (ViewState["PRINCIPALID"] != null)
                    Response.Redirect("../Accounts/AccountAirfarePrincipalMarkupRegister.aspx?principalId=" + ViewState["PRINCIPALID"].ToString() + "&name=" + ViewState["PRINCIPAL"].ToString());
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
        dgOwnerchargearrangements.SelectedIndexes.Clear();
        dgOwnerchargearrangements.EditIndexes.Clear();
        dgOwnerchargearrangements.DataSource = null;
        dgOwnerchargearrangements.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        int? sortdirection = null;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDMARKUPAIRFARE", "FLDTAXBASISNAME", "FLDISCREDITNOTEDISCOUNTRETURNNAME" };
        string[] alCaptions = { "Code", "Name", " 	Airfare Markup %", "GST Chargeable", "Invoice Credit Note Discount Return" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixRegistersOwnerMapping.Ownermapchargesearch(txtCode.Text.Trim(), txtName.Text.Trim(), sortexpression, sortdirection,
        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
        ref iRowCount,
        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OwnerChargeArrangements.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Owner Charge Arrangements</h3></td>");
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

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDMARKUPAIRFARE", "FLDTAXBASISNAME", "FLDISCREDITNOTEDISCOUNTRETURN" };
        string[] alCaptions = { "Code", "Name", " 	Airfare Markup %", "GST Chargeable", "Invoice Credit Note Discount Return" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersOwnerMapping.Ownermapchargesearch(txtCode.Text.Trim(), txtName.Text.Trim(), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           dgOwnerchargearrangements.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

         dgOwnerchargearrangements.DataSource = ds;
         dgOwnerchargearrangements.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
           
            DataRow dr = ds.Tables[0].Rows[0];

            if (ViewState["PRINCIPALID"] == null)
            {
                ViewState["PRINCIPALID"] = dr["FLDADDRESSCODE"].ToString();
                ViewState["PRINCIPAL"] = dr["FLDNAME"].ToString();
               // dgOwnerchargearrangements.SelectedIndex = 0;
            }
            SetRowSelection();
        }
     
        General.SetPrintOptions("dgOwnerchargearrangements", "Owner Charge Arrangements", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void dgOwnerchargearrangements_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

         //   ViewState["PRINCIPALID"] = ((RadLabel)e.Item.FindControl("lblPrincipalId")).Text;
        //    ViewState["PRINCIPAL"] = ((RadLabel)e.Item.FindControl("lblName")).Text;

          
           if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixRegistersOwnerMapping.Ownerchargingupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , int.Parse(((RadLabel)e.Item.FindControl("lblOwnerId")).Text)
                                                               , (((CheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                                                               , (((CheckBox)e.Item.FindControl("chkCreditnotediscountreturnYNEdit")).Checked) ? 1 : 0
                                                               );
                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void dgOwnerchargearrangements_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //    ViewState["PRINCIPALID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPrincipalId")).Text;
    //    ViewState["PRINCIPAL"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblName")).Text;

    //    if (e.CommandName.ToUpper().Equals("SELECT"))
    //    {
    //        BindPageURL(nCurrentRow);
    //        SetRowSelection();
    //    }

    //    if (e.CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        PhoenixRegistersOwnerMapping.Ownerchargingupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                            , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOwnerId")).Text)
    //                                                            , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
    //                                                            , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCreditnotediscountreturnYNEdit")).Checked) ? 1 : 0
    //                                                            );
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }

    //    else if (e.CommandName.ToUpper().Equals("EDIT"))
    //    {
    //        BindData();
    //        _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
    //    }

    //    else
    //    {
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    SetPageNavigator();
    //}

    //protected void dgOwnerchargearrangements_RowUpdating(object sender, GridViewUpdateEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = de.RowIndex;

    //        PhoenixRegistersOwnerMapping.Ownerchargingupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                            , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOwnerId")).Text)
    //                                                            , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
    //                                                            , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCreditnotediscountreturnYNEdit")).Checked) ? 1 : 0
    //                                                            );
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    protected void dgOwnerchargearrangements_SortCommand(object sender, GridSortCommandEventArgs e)
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

    //protected void dgOwnerchargearrangements_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgOwnerchargearrangements, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    //protected void dgOwnerchargearrangements_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void dgOwnerchargearrangements_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgOwnerchargearrangements.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgOwnerchargearrangements_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
           
            if (e.Item is GridDataItem)
            {
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void dgOwnerchargearrangements_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        // Get the LinkButton control in the first cell
    //        LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
    //        // Get the javascript which is assigned to this LinkButton
    //        string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
    //        // Add this javascript to the onclick Attribute of the row
    //        e.Row.Attributes["ondblclick"] = _jsDouble;
    //    }

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }


    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null)
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null)
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
      
    }

   
    protected void dgOwnerchargearrangements_SelectedIndexChanged(object sender, GridViewSelectEventArgs se)
    {
        dgOwnerchargearrangements.SelectedIndexes.Add(se.NewSelectedIndex);
        string principalId = ((RadLabel)dgOwnerchargearrangements.Items[se.NewSelectedIndex].FindControl("lblPrincipalId")).ToString();
        ViewState["PRINCIPALID"] = principalId;
        BindData();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)dgOwnerchargearrangements.Items[rowindex];
            RadLabel lblName = (RadLabel)dgOwnerchargearrangements.Items[rowindex].FindControl("lblName");
            RadLabel lblPrincipalId = (RadLabel)dgOwnerchargearrangements.Items[rowindex].FindControl("lblPrincipalId");
            if (lblPrincipalId != null)
            {
                ViewState["PRINCIPALID"] = lblPrincipalId.Text;
                ViewState["PRINCIPAL"] = lblName.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 

    private void SetRowSelection()
    {
        dgOwnerchargearrangements.SelectedIndexes.Clear();
        foreach (GridDataItem item in dgOwnerchargearrangements.Items)
        {
            if (item.GetDataKeyValue("FLDADDRESSCODE").ToString().Equals(ViewState["PRINCIPALID"].ToString()))
            {
                dgOwnerchargearrangements.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

   
}
