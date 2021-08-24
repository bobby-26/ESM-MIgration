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


public partial class RegistersBudgetGroup : PhoenixBasePage
{
    ////protected override void Render(HtmlTextWriter writer)
    ////{
    ////    foreach (GridViewRow r in gvBudgetGroup.Rows)
    ////    {
    ////        if (r.RowType == DataControlRowType.DataRow)
    ////        {
    ////            Page.ClientScript.RegisterForEventValidation(gvBudgetGroup.UniqueID, "Edit$" + r.RowIndex.ToString());
    ////        }
    ////    }
    ////    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            
            toolbar.AddFontAwesomeButton("../Registers/RegistersBudgetGroup.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetGroup')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuBudgetGroup.AccessRights = this.ViewState;
            MenuBudgetGroup.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vessel Map", "MAP", ToolBarDirection.Right);
            MenuBudget.AccessRights = this.ViewState;
            MenuBudget.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvBudgetGroup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

         
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

            if (CommandName.ToUpper().Equals("MAP"))
            {
                Response.Redirect("../Registers/RegistersBudgetVesselMap.aspx");
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
        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", "FLDACCOUNTTYPENAME", "FLDTECHGROUPYN" };
        string[] alCaptions = { "Code", "Name", "Account Type ", "Technical Group" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersBudgetGroup.BudgetGroupSearch(
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetGroup.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Group</h3></td>");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", "FLDACCOUNTTYPENAME" };
        string[] alCaptions = { "Code", "Name", "Account Type " };

        DataSet ds = PhoenixRegistersBudgetGroup.BudgetGroupSearch(
                                                sortexpression, sortdirection,
                                                gvBudgetGroup.CurrentPageIndex+1,
                                                gvBudgetGroup.PageSize,
                                                //General.ShowRecords(null),
                                                ref iRowCount,
                                                ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetGroup", "Budget Group", alCaptions, alColumns, ds);

        gvBudgetGroup.DataSource = ds;
        gvBudgetGroup.VirtualItemCount = iRowCount;
               
        ViewState["ROWCOUNT"] = iRowCount;
        
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

            if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblBudgetGroupIdEdit")).Text) == null)
            {
                InsertBudgetGroup(
                                

                                 ((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text,
                    ((UserControlHard)e.Item.FindControl("ucAccountTypeEdit")).SelectedHard,
                 (((RadCheckBox)e.Item.FindControl("chkTechGroup")).Checked.Equals(true)) ? 1 : 0
                );
            }
            else
            {
                UpdateBudgetGroup(
                      ((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text,
                      ((UserControlHard)e.Item.FindControl("ucAccountTypeEdit")).SelectedHard,
                 (((RadCheckBox)e.Item.FindControl("chkTechGroup")).Checked.Equals(true)) ? 1 : 0
                 );
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
        //gvBudgetGroup.SelectedIndex = e.NewSelectedIndex;
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


    protected void gvBudgetGroup_Command(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblBudgetGroupId")).Text) != null)
                {
                    DeleteBudgetGroup(Int32.Parse(((RadLabel)e.Item.FindControl("lblBudgetGroupId")).Text));
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

       
        if (e.Item is GridEditableItem)
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
        }

        UserControlHard ucAccountTypeEdit = (UserControlHard)e.Item.FindControl("ucAccountTypeEdit");
        DataRowView drview = (DataRowView)e.Item.DataItem;
        if (ucAccountTypeEdit != null) ucAccountTypeEdit.SelectedHard = drview["FLDACCOUNTTYPE"].ToString();
    }
    private void InsertBudgetGroup(string budgetgroupid, string accounttype, int techgroupyn)
    {
        if (!IsValidBudgetGroup(accounttype))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersBudgetGroup.InsertBudgetGroup(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , int.Parse(budgetgroupid), int.Parse(accounttype), techgroupyn);
    }

    private void UpdateBudgetGroup(string budgetgroupid, string accounttype, int techgroupyn)
    {
        if (!IsValidBudgetGroup(accounttype))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersBudgetGroup.UpdateBudgetGroup(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , int.Parse(budgetgroupid), int.Parse(accounttype), techgroupyn);

        ucStatus.Text = "Budget Group information updated successfully";
    }

    private bool IsValidBudgetGroup(string accounttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvBudgetGroup;

        if (General.GetNullableInteger(accounttype) == null)
            ucError.ErrorMessage = "Account Type is required";

        return (!ucError.IsError);
    }

    private void DeleteBudgetGroup(int budgetgroupid)
    {
        PhoenixRegistersBudgetGroup.DeleteBudgetGroup(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, budgetgroupid);
    }
  
}
