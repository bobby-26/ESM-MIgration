using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;


public partial class OwnerBudgetCrewExpenseItemsList : PhoenixBasePage
{
	

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetCrewExpenseItemsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucExpense.ExpenseType = null;
                ucExpense.bind();    
            }
             
            
            BindData();
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
        DataSet ds = new DataSet();
        string[] alCaptions = { "Expense", "Officer Type","Amount" };
        string[] alColumns = { "FLDCATEGORYNAME", "FLDOFFICERTYPE", "FLDAMOUNT" };
		
        string sortexpression;
        int? sortdirection = null;
      
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOwnerBudget.CrewExpenseItemsList(General.GetNullableGuid(ucExpense.SelectedExpenseType));

            Response.AddHeader("Content-Disposition", "attachment; filename=\"CrewExpense.xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Crew Expense</h3></td>");
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

    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
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

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        string[] alCaptions = { "Expense", "Officer Type", "Amount" };
        string[] alColumns = { "FLDCATEGORYNAME", "FLDOFFICERTYPE", "FLDAMOUNT" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixOwnerBudget.CrewExpenseItemsList(General.GetNullableGuid(ucExpense.SelectedExpenseType));

            General.SetPrintOptions("gvQuick", "Crew Expenses", alCaptions, alColumns, ds);

     

            gvQuick.DataSource = ds;
            gvQuick.DataBind();
     

    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        
    }



    protected void gvQuick_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuick_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
        
    }
    protected void gvQuick_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvQuick, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
    }
 
    protected void gvQuick_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (!IsValidQuick(((UserControlMaskNumber)e.Item.FindControl("txtContractPeriod")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("txtManRequired")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixOwnerBudget.CrewExpenseItemsUpdate(General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtContractPeriod")).Text),
                                                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtManRequired")).Text),
                                                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text),
                                                new Guid(((RadLabel)e.Item.FindControl("lblExpenseId")).Text.ToString()));
            BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvQuick_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


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

        }

        if (e.Item is GridEditableItem)

        {
			{
				ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			
			}
        }
        if (e.Item is GridFooterItem)

        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       
		ViewState["PAGENUMBER"] = 1;
		BindData();
		
    }

    
    private bool IsValidQuick(string contractperiod,string manrequired,string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvQuick;

		if (General.GetNullableInteger(contractperiod) == null)
		{
            ucError.ErrorMessage = "Contract Period is required.";
		}
        if (General.GetNullableInteger(manrequired) == null)
        {
            ucError.ErrorMessage = "Man Required is required.";
        }
        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount is required.";


        return (!ucError.IsError);
    }

   

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvQuick_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuick.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
