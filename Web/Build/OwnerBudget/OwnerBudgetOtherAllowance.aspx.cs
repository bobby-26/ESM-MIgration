using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class OwnerBudgetOtherAllowance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetOtherAllowance.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvContactType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuInspectionContactType.AccessRights = this.ViewState;
            MenuInspectionContactType.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                ucUnion.DataBind();
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
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDALLOWANCENAME", "FLDAMOUNT","FREQUENCY" };
        string[] alCaptions = { "Allowance", "Amount","Frequency" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOwnerBudgetRegisters.AllowanceSearch(General.GetNullableInteger(ucUnion.SelectedAddress)
                       , null
                       , 1
                       , iRowCount
                       , ref iRowCount
                       , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=OtherAllowances.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allowances</h3></td>");
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

    protected void InspectionContactType_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDALLOWANCENAME", "FLDAMOUNT", "FREQUENCY" };
        string[] alCaptions = { "Allowance", "Amount", "Frequency" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixOwnerBudgetRegisters.AllowanceSearch(General.GetNullableInteger(ucUnion.SelectedAddress)
                      , null
                      , gvContactType.CurrentPageIndex+1
                      , gvContactType.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount);

        General.SetPrintOptions("gvContactType", "Other Allowances", alCaptions, alColumns, ds);
     
        gvContactType.DataSource = ds;
  
        gvContactType.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


    }

    protected void gvContactType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

     

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidContactType(ucUnion.SelectedAddress
                                    , ((UserControlQuick)e.Item.FindControl("ucAllowance")).SelectedQuick
                                    , (((UserControlMaskNumber)e.Item.FindControl("ucAmountAdd")).Text)
                                    , (((RadDropDownList)e.Item.FindControl("ddlFrequencyAdd")).SelectedValue)
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerBudgetRegisters.AllowanceInsert(int.Parse(ucUnion.SelectedAddress)
                                                , int.Parse(((UserControlQuick)e.Item.FindControl("ucAllowance")).SelectedQuick)
                                                , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountAdd")).Text)
                                                , int.Parse(((RadDropDownList)e.Item.FindControl("ddlFrequencyAdd")).SelectedValue)
                                                );
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidContactType(ucUnion.SelectedAddress
                                    , ((RadLabel)e.Item.FindControl("lblAllownace")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAmount")).Text
                                    , ((RadDropDownList)e.Item.FindControl("ddlFrequency")).SelectedValue
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerBudgetRegisters.AllowanceUpdate(new Guid(((RadLabel)e.Item.FindControl("lblAllowanceId")).Text.ToString())
                                                           , int.Parse(((RadLabel)e.Item.FindControl("lblAllownace")).Text)
                                                           , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmount")).Text)
                                                           , int.Parse(((RadDropDownList)e.Item.FindControl("ddlFrequency")).SelectedValue)
                                                            );
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudgetRegisters.AllowanceDelete(new Guid(((RadLabel)e.Item.FindControl("lblAllowanceId")).Text.ToString()));
            }
            BindData();
            gvContactType.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvContactType_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvContactType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }
            RadDropDownList ddlFrequency = (RadDropDownList)e.Item.FindControl("ddlFrequency");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ddlFrequency != null)
            {
                ddlFrequency.SelectedValue = drv["FLDFREQUENCY"].ToString();
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
            UserControlQuick ucAllowance = (UserControlQuick)e.Item.FindControl("ucAllowance");
            if (ucAllowance != null)
            {
                ucAllowance.QuickTypeCode = "111";
                ucAllowance.bind();
            }
        }

        
    }
    protected void gvContactType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        BindData();
    }
    protected void gvContactType_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }
   

    private bool IsValidContactType(string union, string allowance, string amount, string frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(union) == null)
            ucError.ErrorMessage = "Union is required.";

        if (General.GetNullableInteger(allowance) == null)
            ucError.ErrorMessage = "Allowance is required.";

        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount is required.";

        if (General.GetNullableInteger(frequency) == null)
            ucError.ErrorMessage = "Frequency is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
    protected void gvContactType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContactType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }



    protected void ucUnion_TextChangedEvent(object sender, EventArgs e)
    {
        gvContactType.Rebind();
    }
}

