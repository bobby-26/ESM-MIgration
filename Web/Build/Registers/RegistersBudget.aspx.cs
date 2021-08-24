using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBudget : PhoenixBasePage
{
    string straccounttype = "";

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    //    foreach (GridViewRow r in gdBudget.Rows)
    //    //    {
    //    //        if (r.RowType == DataControlRowType.DataRow)
    //    //        {
    //    //            Page.ClientScript.RegisterForEventValidation
    //    //                    (r.UniqueID + "$ctl00");
    //    //            Page.ClientScript.RegisterForEventValidation
    //    //                    (r.UniqueID + "$ctl01");
    //    //        }
    //    //    }
    //    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);
            
            MenuBudget.AccessRights = this.ViewState;
            MenuBudget.MenuList = toolbarmain.Show();
            

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
           toolbargrid.AddFontAwesomeButton("../Registers/RegistersBudget.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBudget')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Filter','Registers/RegistersBudgetFilter.aspx')", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Registers/RegistersBudget.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersBudget.AccessRights = this.ViewState;
            MenuRegistersBudget.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("Style", "Display:None");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["BUDGETID"] = null;
                rblAccountType.Enabled = false;
                BindHard();
              
            }
            //BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gdBudget.Rebind();
    }

    protected void BudgetGroup_Changed(object sender, EventArgs e)
    {
        BindHard();

        if (General.GetNullableInteger(ucHard.SelectedHard) != null)
        {
            DataSet ds = PhoenixRegistersBudgetGroup.EditBudgetGroup(int.Parse(ucHard.SelectedHard));

            if (ds.Tables[0].Rows.Count > 0)
            {
                rblAccountType.SelectedValue = ds.Tables[0].Rows[0]["FLDACCOUNTTYPE"].ToString();
            }
        }
    }

    private void Reset()
    {
        ViewState["BUDGETID"] = null;
        txtSubAccount.Text = "";
        txtDescription.Text = "";
        chkBudgetedExpense.Checked = false;
        ucHard.SelectedHard = "";
        rblAccountType.SelectedIndex = -1;
    }

    protected void RegistersBudgetMenu_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
           // BindData();
           // gdBudget.CurrentPageIndex = 0;
            gdBudget.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentBudgetFilterSelection = null;

            //BindData();
            gdBudget.CurrentPageIndex = 0;
            gdBudget.Rebind();

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCOUNTTYPE", "FLDBUDGETGROUP", "FLDBUDGETEDEXPENSESTATUS", "FLDISACTIVESTATUS", "FLDISRESTRICTTOOPENPROJECTSTATUS" };
        string[] alCaptions = { "Budget Code", "Description", "Account Type", "Budget Group", "Budgeted Expense", "Active(Yes/No)", "Restrict to Open Projects Only" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
            , null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
            , null
            , null
            , sortexpression, sortdirection
           , 1, iRowCount
           , ref iRowCount
           , ref iTotalPageCount
           , nvc != null ? General.GetNullableInteger(nvc.Get("IsActive")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=Budget_Code.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Code</h3></td>");
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

  
    protected void Budget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["BUDGETID"] = null;
                Reset();
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidBudget())
                {
                    ucError.Visible = true;
                    return;
                }

                straccounttype = rblAccountType.SelectedValue;

                if (ViewState["BUDGETID"] == null)
                {

                    PhoenixRegistersBudget.InsertBudget(1, txtSubAccount.Text, txtDescription.Text,
                                                                   Convert.ToInt32(straccounttype.ToString()),
                                                                   Convert.ToInt32(ucHard.SelectedHard),
                                                                   chkBudgetedExpense.Checked==true ? 1 : 0,
                                                                   null,
                                                                   chkactive.Checked==true ? 1 : 0,
                                                                   (bool)chkOpenProjecct.Checked==true ? 1 : 0,
                                                                   (bool)chkIncludefund.Checked==true ? 1 : 0,
                                                                   (bool)chkOffsetting.Checked==true ? 1 : 0);
                    //Reset();
                }
                else
                {
                    PhoenixRegistersBudget.UpdateBudget(1, Convert.ToInt32(ViewState["BUDGETID"]),
                                                                    txtSubAccount.Text,
                                                                    txtDescription.Text,
                                                                    Convert.ToInt32(straccounttype.ToString()),
                                                                    Convert.ToInt32(ucHard.SelectedHard),
                                                                    chkBudgetedExpense.Checked==true ? 1 : 0,
                                                                    chkactive.Checked==true ? 1 : 0,
                                                                    chkOpenProjecct.Checked==true ? 1 : 0,
                                                                    chkIncludefund.Checked==true ? 1 : 0,
                                                                    chkOffsetting.Checked==true ? 1 : 0);
                }
                //BindData();
                gdBudget.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidBudget()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSubAccount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Budget Code is required.";

        if (txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (rblAccountType.SelectedValue == null)
            ucError.ErrorMessage = "Account Type is required";

        if (ucHard.SelectedHard.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Budget Group is required";

        return (!ucError.IsError);
    }
    protected void gdBudget_SortCommand(object sender, GridSortCommandEventArgs e)
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? sortdirection = null;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCOUNTTYPE", "FLDBUDGETGROUP", "FLDBUDGETEDEXPENSESTATUS", "FLDISACTIVESTATUS", "FLDISRESTRICTTOOPENPROJECTSTATUS" };
            string[] alCaptions = { "Budget Code", "Description", "Account Type", "Budget Group", "Budgeted Expense", "Active(Yes/No)", "Restrict to Open Projects Only" };

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

            DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : null
               , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
               , null
               , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
               , null
               , null
               , sortexpression, sortdirection
               , gdBudget.CurrentPageIndex + 1, gdBudget.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , nvc != null ? General.GetNullableInteger(nvc.Get("IsActive")) : null);

            General.SetPrintOptions("gvBudget", "Budget Code", alCaptions, alColumns, ds);


            gdBudget.DataSource = ds;
            gdBudget.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gdBudget_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gdBudget.CurrentPageIndex + 1;
            BindData();
            //int iRowCount = 0;
            //int iTotalPageCount = 0;
            //int? sortdirection = null;

            //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            //string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCOUNTTYPE", "FLDBUDGETGROUP", "FLDBUDGETEDEXPENSESTATUS", "FLDISACTIVESTATUS", "FLDISRESTRICTTOOPENPROJECTSTATUS" };
            //string[] alCaptions = { "Budget Code", "Description", "Account Type", "Budget Group", "Budgeted Expense", "Active(Yes/No)", "Restrict to Open Projects Only" };

            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

            //DataSet ds = PhoenixRegistersBudget.BudgetSearch(
            //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //    , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : null
            //   , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
            //   , null
            //   , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
            //   , null
            //   , null
            //   , sortexpression, sortdirection
            //   , gdBudget.CurrentPageIndex + 1, gdBudget.PageSize
            //   , ref iRowCount
            //   , ref iTotalPageCount
            //   , nvc != null ? General.GetNullableInteger(nvc.Get("IsActive")) : null);

            //General.SetPrintOptions("gvBudget", "Budget Code", alCaptions, alColumns, ds);


            //gdBudget.DataSource = ds;
            //gdBudget.VirtualItemCount = iRowCount;
            //ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BudgetEdit()
    {
        if (ViewState["BUDGETID"] != null)
        {

            DataSet dsBudget = PhoenixRegistersBudget.EditBudget
                (PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ViewState["BUDGETID"].ToString()));

            if (dsBudget.Tables.Count > 0)
            {
                DataRow drBudget = dsBudget.Tables[0].Rows[0];
                txtSubAccount.Text = drBudget["FLDSUBACCOUNT"].ToString();
                txtDescription.Text = drBudget["FLDDESCRIPTION"].ToString();
                ucHard.SelectedHard = drBudget["FLDBUDGETGROUP"].ToString();

                if (drBudget["FLDACCOUNTTYPE"].ToString() != "")
                {
                    rblAccountType.SelectedValue = drBudget["FLDACCOUNTTYPE"].ToString();
                }
                else
                    rblAccountType.SelectedIndex = -1;

                chkBudgetedExpense.Checked = drBudget["FLDBUDGETEDEXPENSE"].ToString() == "1" ? true : false;
                chkactive.Checked = drBudget["FLDISACTIVE"].ToString() == "1" ? true : false;
                chkIncludefund.Checked = drBudget["FLDINCLUDEFUND"].ToString() == "1" ? true : false;
                chkOpenProjecct.Checked = drBudget["FLDISRESTRICTTOOPENPROJECT"].ToString() == "1" ? true : false;
                chkOffsetting.Checked = drBudget["FLDOFFSETTINGYN"].ToString() == "1" ? true : false;
                //BindData();
            }
        }
    }




    protected void gdBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
         
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersBudget.DeleteBudget(1,
                    Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBudgetid")).Text));

            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["BUDGETID"] = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBudgetid")).Text);
                BudgetEdit();
                //BindData();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }




    protected void gdBudget_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        gdBudget.Rebind();
    }

    protected void gdBudget_ItemDataBound(Object sender, GridItemEventArgs e)
    {

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

        }
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
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void BindHard()
    {
        ucHard.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.BUDGETGROUP).ToString();

        rblAccountType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTTYPE));
        rblAccountType.DataTextField = "FLDHARDNAME";
        rblAccountType.DataValueField = "FLDHARDCODE";
        rblAccountType.DataBind();
    }

    public void unCheckRadioButtons()
    {
        foreach (ListItem item in rblAccountType.Items)
        {
            item.Selected = false;
        }
    }

}
