using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersTravelReasonOwnerBudgetCodeMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersTravelReasonOwnerBudgetCodeMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTravelReason')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuTravelReason.AccessRights = this.ViewState;
        MenuTravelReason.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
          

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            string temp = ucPrincipal.SelectedAddress.ToString();
        }
        BindData();
    }

    protected void MenuTravelReason_TabStripCommand(object sender, EventArgs e)
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
    protected void gvTravelReason_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvTravelReason_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelReason.CurrentPageIndex + 1;
            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravelReason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (General.GetNullableInteger(ucPrincipal.SelectedAddress) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the principal.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersBudget.TravelReasonOwnerBudgetCodeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((Literal)e.Item.FindControl("lblTravelReasonOBCId")).Text)
                    , int.Parse(ucPrincipal.SelectedAddress)
                    , int.Parse(((Literal)e.Item.FindControl("lblTravelReasonId")).Text)
                    , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOnOwnerBudgetIdEdit")).Text)
                    , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOffOwnerBudgetIdEdit")).Text));

                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                if (General.GetNullableInteger(ucPrincipal.SelectedAddress) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the principal.";
                    ucError.Visible = true;
                    return ;
                }
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravelReason_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tb1 = new TableCell();
            TableCell tb2 = new TableCell();
            TableCell tb3 = new TableCell();
            TableCell tb4 = new TableCell();

            tb1.ColumnSpan = 1;
            tb2.ColumnSpan = 2;
            tb3.ColumnSpan = 2;
            tb4.ColumnSpan = 1;

            tb1.Text = "";
            tb2.Text = "On - Signers";
            tb3.Text = "Off - Signers";
            tb4.Text = "";

            tb1.Attributes.Add("style", "text-align:center");
            tb2.Attributes.Add("style", "text-align:center");
            tb3.Attributes.Add("style", "text-align:center");
            tb4.Attributes.Add("style", "text-align:center");

            gv.Cells.Add(tb1);
            gv.Cells.Add(tb2);
            gv.Cells.Add(tb3);
            gv.Cells.Add(tb4);
            gvTravelReason.Controls[0].Controls.AddAt(0, gv);
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton btnShowOnOwnerBudgetEdit = (LinkButton)e.Item.FindControl("btnShowOnOwnerBudgetEdit");
            RadTextBox txtOnOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOnOwnerBudgetNameEdit");
            RadTextBox txtOnOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOnOwnerBudgetIdEdit");
            RadTextBox txtOnOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOnOwnerBudgetgroupIdEdit");
            Literal lblOnBudgetCodeId = (Literal)e.Item.FindControl("lblOnBudgetCodeId");

            if (txtOnOwnerBudgetNameEdit != null)
                txtOnOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOnOwnerBudgetIdEdit != null)
                txtOnOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOnOwnerBudgetgroupIdEdit != null)
                txtOnOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");

            if (btnShowOnOwnerBudgetEdit != null && lblOnBudgetCodeId != null)
            {
                btnShowOnOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOnOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=&Ownerid=" + ucPrincipal.SelectedAddress.ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + lblOnBudgetCodeId.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                if (!SessionUtil.CanAccess(this.ViewState, btnShowOnOwnerBudgetEdit.CommandName)) btnShowOnOwnerBudgetEdit.Visible = false;
            }

            LinkButton btnShowOffOwnerBudgetEdit = (LinkButton)e.Item.FindControl("btnShowOffOwnerBudgetEdit");
            RadTextBox txtOffOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOffOwnerBudgetNameEdit");
            RadTextBox txtOffOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOffOwnerBudgetIdEdit");
            RadTextBox txtOffOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOffOwnerBudgetgroupIdEdit");
            Literal lblOffBudgetCodeId = (Literal)e.Item.FindControl("lblOffBudgetCodeId");

            if (txtOffOwnerBudgetNameEdit != null)
                txtOffOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOffOwnerBudgetIdEdit != null)
                txtOffOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOffOwnerBudgetgroupIdEdit != null)
                txtOffOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");

            if (btnShowOffOwnerBudgetEdit != null && lblOffBudgetCodeId != null)
            {
                btnShowOffOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOffOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=&Ownerid=" + ucPrincipal.SelectedAddress.ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + lblOffBudgetCodeId.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                if (!SessionUtil.CanAccess(this.ViewState, btnShowOffOwnerBudgetEdit.CommandName)) btnShowOffOwnerBudgetEdit.Visible = false;
            }

        }
    }

 

    protected void gvTravelReason_OnDeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREASON", "FLDONSIGNERSUBACCOUNT", "FLDONSIGNEROWNERBUDGETCODE", "FLDOFFSIGNERSUBACCOUNT", "FLDOFFSIGNEROWNERBUDGETCODE" };
        string[] alCaptions = { "Travel Reason", "On - Signers Budget Code", "On - Signers Owner Budget Code", "Off - Signers Budget Code", "Off - Signers Owner Budget Code" };

        DataSet ds = PhoenixRegistersBudget.TravelReasonOwnerBudgetCodeSearch(General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? 0 : General.GetNullableInteger(ucPrincipal.SelectedAddress)
            ,gvTravelReason.CurrentPageIndex+1
            ,gvTravelReason.PageSize
            ,ref iRowCount
            ,ref iTotalPageCount);

        General.SetPrintOptions("gvTravelReason", "Travel Reason Owner Budget Code Map", alCaptions, alColumns, ds);

     
            gvTravelReason.DataSource = ds;
        gvTravelReason.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    
    }
       
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREASON", "FLDONSIGNERSUBACCOUNT", "FLDONSIGNEROWNERBUDGETCODE", "FLDOFFSIGNERSUBACCOUNT", "FLDOFFSIGNEROWNERBUDGETCODE" };
        string[] alCaptions = { "Travel Reason", "On - Signers Budget Code", "On - Signers Owner Budget Code", "Off - Signers Budget Code", "Off - Signers Owner Budget Code" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersBudget.TravelReasonOwnerBudgetCodeSearch(General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? 0 : General.GetNullableInteger(ucPrincipal.SelectedAddress)
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=TravelReason.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Reason Owner Budget Code Map</h3></td>");
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
}
