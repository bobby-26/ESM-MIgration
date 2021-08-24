using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersInsuranceType : PhoenixBasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersInsuranceType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInsuranceType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersInsuranceType.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuRegistersInsuranceType.AccessRights = this.ViewState;
            MenuRegistersInsuranceType.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvInsuranceType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        string[] alColumns = { "FLDINSURANCETYPENAME", "FLDPREMIUM", "FLDPRINCIPALSHARE", "FLDCREWSHARE", "FLDPRINCIPALSHARETILLENDOFLEAVE", "FLDLEAVEPERDAY" };
        string[] alCaptions = { "Insurance Type", "Premium", "Principal Share", "Crew Share", "End Of Leave", "Leave Per Day" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersInsuranceType.InsuranceTypeSearch(1, txtSearch.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=InsuanceType.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Insurance Type</h3></td>");
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

    protected void RegistersInsuranceType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvInsuranceType.Rebind();
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

    protected void gvInsuranceType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINSURANCETYPENAME", "FLDPREMIUM", "FLDPRINCIPALSHARE", "FLDCREWSHARE", "FLDPRINCIPALSHARETILLENDOFLEAVE", "FLDLEAVEPERDAY" };
        string[] alCaptions = { "Insurance Type", "Premium", "Principal Share", "Crew Share", "End Of Leave", "Leave Per Day" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersInsuranceType.InsuranceTypeSearch(1, txtSearch.Text, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvInsuranceType", "Insurance Type", alCaptions, alColumns, ds);

        gvInsuranceType.DataSource = ds;
        gvInsuranceType.VirtualItemCount = iRowCount;

    }

    protected void txtPrincipalShareEdit_TextChanged(object sender, EventArgs e)
    {

        try
        {
            RadTextBox thisTextBox = (RadTextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            decimal premmium = decimal.Parse(((RadTextBox)thisGridViewRow.FindControl("txtPremiumEdit")).Text.Trim('_'));
            decimal principalshare = decimal.Parse(((UserControlDecimal)thisGridViewRow.FindControl("txtPrincipalShareEdit")).Text.Trim('_'));
            decimal crewshare = premmium - principalshare;
            ((RadTextBox)thisGridViewRow.FindControl("txtCrewShareEdit")).Text = crewshare.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
    }

    protected void txtPrincipalShareAdd_TextChanged(object sender, EventArgs e)
    {

        decimal premmium = decimal.Parse(((UserControlDecimal)gvInsuranceType.FindControl("txtPremiumEdit")).Text);
        decimal principalshare = decimal.Parse(((UserControlDecimal)gvInsuranceType.FindControl("txtPrincipalShareEdit")).Text);
        decimal crewshare = premmium - principalshare;
        ((RadTextBox)gvInsuranceType.FindControl("txtCrewShareEdit")).Text = crewshare.ToString();
    }

    protected void gvInsuranceType_ItemCommand(object sender, GridCommandEventArgs e) 
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        GridFooterItem item = (GridFooterItem)gvInsuranceType.MasterTableView.GetItems(GridItemType.Footer)[0];
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            string name = ((RadTextBox)item.FindControl("txtInsuranceTypeNameAdd")).Text;
            string premium = ((UserControlDecimal)item.FindControl("txtPremiumAdd")).Text.Trim('_');
            string principal = ((UserControlDecimal)item.FindControl("txtPrincipalShareAdd")).Text.Trim('_');
            string leaveperday = ((UserControlDecimal)item.FindControl("txtLeavePerDayAdd")).Text;
            if (!IsValidInsuranceType(name, premium, principal, leaveperday))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            else
            {

                InsertInsuranceType(
                    name,
                    decimal.Parse(premium),
                    decimal.Parse(principal),
                    decimal.Parse(premium) - decimal.Parse(principal),
                    (((CheckBox)item.FindControl("chkPrincipalShareTillEndOfLeaveAdd")).Checked) ? 1 : 0,
                    Int32.Parse(leaveperday)
                );
                gvInsuranceType.Rebind();

            }

        }
        else
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                string InsurencetypeId = ((RadLabel)e.Item.FindControl("lblInsuranceTypeIdEdit")).Text;

                if (InsurencetypeId == string.Empty || InsurencetypeId == "")
                {
                   
                }
                else
                {
                    string name = ((RadTextBox)eeditedItem.FindControl("txtInsuranceTypeNameEdit")).Text;
                    string premium = ((UserControlDecimal)eeditedItem.FindControl("txtPremiumEdit")).Text.Trim('_');
                    string principal = ((UserControlDecimal)eeditedItem.FindControl("txtPrincipalShareEdit")).Text.Trim('_');
                    string leaveperday = ((UserControlDecimal)eeditedItem.FindControl("txtLeavePerDayEdit")).Text;
                    if (!IsValidInsuranceType(name, premium, principal, leaveperday))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateInsuranceType(
                         Int32.Parse(((RadLabel)eeditedItem.FindControl("lblInsuranceTypeIdEdit")).Text),
                         name,
                         decimal.Parse(premium),
                         decimal.Parse(principal),
                         decimal.Parse(premium) - decimal.Parse(principal),
                         (((CheckBox)eeditedItem.FindControl("chkPrincipalShareTillEndOfLeaveEdit")).Checked) ? 1 : 0,
                         Int32.Parse(leaveperday)
                     );
                    gvInsuranceType.Rebind();
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
       
    }

    protected void gvInsuranceType_DeleteCommand(object sender, GridCommandEventArgs e) 
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        try
        {
            DeleteInsuranceType(Int32.Parse(((RadLabel)eeditedItem.FindControl("lblInsuranceTypeId")).Text));
            gvInsuranceType.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInsuranceType_SortCommand(object sender, GridSortCommandEventArgs e) 
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

    protected void gvInsuranceType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvInsuranceType, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    //protected void gvInsuranceType_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvInsuranceType.SelectedIndex = e.NewSelectedIndex;
    //}

    protected void gvInsuranceType_ItemDataBound(object sender, GridItemEventArgs e) 
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvInsuranceType.Rebind();
    }

    private void InsertInsuranceType(string insurancetypename, decimal premium, decimal principalshare, decimal crewshare, int? principalsharetillendofleave, Int32 leaveperday)
    {

        PhoenixRegistersInsuranceType.InsertInsuranceType(0, insurancetypename, premium, principalshare, crewshare, principalsharetillendofleave, leaveperday);
    }

    private void UpdateInsuranceType(int insurancetypeid, string insurancetypename, decimal premium, decimal principalshare, decimal crewshare, int? principalsharetillendofleave, Int32 leaveperday)
    {
        PhoenixRegistersInsuranceType.UpdateInsuranceType(0, insurancetypeid, insurancetypename, premium, principalshare, crewshare, principalsharetillendofleave, leaveperday);
        ucStatus.Text = "Insurance Type information updated";
    }

    private bool IsValidInsuranceType(string insurancetypename, string premium, string principalshare, string leaveperday)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (insurancetypename.Trim().Equals(""))
            ucError.ErrorMessage = "Insurance name is required.";

        if (premium.Trim().Equals(""))
            ucError.ErrorMessage = "Premium is required.";

        if (principalshare.Trim().Equals(""))
            ucError.ErrorMessage = "Principal Share is required.";

        if (leaveperday.Trim().Equals(""))
            ucError.ErrorMessage = "Leave perday is required.";

        return (!ucError.IsError);
    }

    private void DeleteInsuranceType(int insurancetypeid)
    {
        PhoenixRegistersInsuranceType.DeleteInsuranceType(0, insurancetypeid);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}