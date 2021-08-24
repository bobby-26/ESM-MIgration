using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegistersBudgetVesselMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersBudgetVesselMap.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetGroup')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuBudgetGroup.AccessRights = this.ViewState;
            MenuBudgetGroup.MenuList = toolbar.Show();
            //MenuBudgetGroup.SetTrigger(pnlBudgetGroup);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Budget Group", "GROUP",ToolBarDirection.Right);
            MenuBudget.AccessRights = this.ViewState;
            MenuBudget.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvBudgetGroup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Budget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GROUP"))
            {
                Response.Redirect("../Registers/RegistersBudgetGroup.aspx");
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", "FLDBUDGETEDYESNO" };
        string[] alCaptions = { "Code", "Name", "Budgeted Expense" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersBudgetGroupVesselMap.BudgetGroupVessselMapSearch(
            General.GetNullableInteger(ucVessel.SelectedVessel),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetGroupVesselMap.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Group Vessel Map</h3></td>");
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

    protected void BudgetGroup_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", "FLDBUDGETEDYESNO" };
        string[] alCaptions = { "Code","Name","Budgeted Expense" };

        DataSet ds = PhoenixRegistersBudgetGroupVesselMap.BudgetGroupVessselMapSearch(
            General.GetNullableInteger(ucVessel.SelectedVessel),
            sortexpression, sortdirection,
             gvBudgetGroup.CurrentPageIndex + 1,
            gvBudgetGroup.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetGroup", "Budget Group Vessel Map", alCaptions, alColumns, ds);

        gvBudgetGroup.DataSource = ds;
        gvBudgetGroup.VirtualItemCount = iRowCount;
      
            gvBudgetGroup.DataSource = ds;
          

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
       
    protected void gvBudgetGroup_SortCommand(object sender, GridSortCommandEventArgs e)
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


    protected void gvBudgetGroup_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            int nCurrentRow = e.Item.RowIndex;

            if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            {
                ucError.ErrorMessage = "Please Select a Vessel";
                ucError.Visible = true;
                return;
            }
            else
            {
                if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblBudgetGroupMapIdEdit")).Text) == null)
                {
                    string lblBudgetCode = ((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text;

                    RadCheckBox chkIsBudgeted = (RadCheckBox)e.Item.FindControl("ckbBudgeted");

                    PhoenixRegistersBudgetGroupVesselMap.BudgetGroupVesselMapInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        int.Parse(ucVessel.SelectedVessel),
                                                        int.Parse(lblBudgetCode), chkIsBudgeted.Checked.Equals(true) ? 1 : 0);
                    ucStatus.Text = "Information Updated";
                }
                else
                {
                    string lblBudgetGroupMapId = ((RadLabel)e.Item.FindControl("lblBudgetGroupMapIdEdit")).Text;
                   

                    RadCheckBox chkIsBudgeted = (RadCheckBox)e.Item.FindControl("ckbBudgeted");
              

                    PhoenixRegistersBudgetGroupVesselMap.BudgetGroupVesselMapUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableGuid(lblBudgetGroupMapId),
                                                        (chkIsBudgeted.Checked == true) ? 1 : 0);
                    ucStatus.Text = "Information Updated";
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetGroup_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }

  
    protected void gvBudgetGroup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Please Select a Vessel";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblBudgetGroupMapId")).Text) != null)
                    {
                        string lblBudgetGroupMapId = ((RadLabel)e.Item.FindControl("lblBudgetGroupMapId")).Text;

                        PhoenixRegistersBudgetGroupVesselMap.BudgetGroupVesselMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(lblBudgetGroupMapId));
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

    protected void gvBudgetGroup_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void gvBudgetGroup_ItemDataBound(Object sender, GridItemEventArgs e)
    {
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

        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

               }
    }

   
    protected void gvBudgetGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetGroup.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidBudgetGroup(string accounttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvBudgetGroup;

        if (General.GetNullableInteger(accounttype) == null)
            ucError.ErrorMessage = "Account Type is required";

        return (!ucError.IsError);
    }

  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
