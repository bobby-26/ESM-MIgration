using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;


public partial class OwnerBudgetLubOil : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbarmain.AddButton("Technical", "TECHNICAL", ToolBarDirection.Right);
            toolbarmain.AddButton("Luboil", "LUBOIL", ToolBarDirection.Right);
            toolbarmain.AddButton("Crew Expense", "EXPENSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Crew Wages", "CREWWAGE", ToolBarDirection.Right);
            toolbarmain.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
            toolbarmain.AddButton("Revisions", "REVISION", ToolBarDirection.Right);
            toolbarmain.AddButton("Proposals", "PROPOSALS", ToolBarDirection.Right);


            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbarmain.Show();
            MenuMain.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                if (Request.QueryString["proposalid"] != null)
                {
                    ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
                }
                if (Request.QueryString["revisionid"] != null)
                {
                    ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
                }
                ViewState["LUBOILID"] = "";
                LubOilEdit();
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListLubOilAddress.aspx?addresstype=130,131&producttype=47', true); "); 
            }
            BindData();        

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PROPOSALID"] != null && ViewState["PROPOSALID"].ToString() != "")
                {
                    Guid? luboilid = Guid.Empty;
                    luboilid = General.GetNullableGuid(ViewState["LUBOILID"].ToString());

                    if (!IsValidData(txtVendorId.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilInsert(new Guid(ViewState["PROPOSALID"].ToString())
                                                                    , int.Parse(txtVendorId.Text)
                                                                    , ref luboilid);

                    ViewState["LUBOILID"] = General.GetNullableGuid(luboilid.ToString());
                    LubOilEdit();
                    BindData();
                    gvLubOil.Rebind();
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROPOSALS"))
        {
            Response.Redirect("OwnerBudgetProposal.aspx");
        }
        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("CREWWAGE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetProposedCrewWages.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetVesselParticulars.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetTechnicalProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetExpenseReport.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("EXPENSE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetCrewExpense.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
    }
    protected bool IsValidData(string vendorid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(vendorid) == null)
            ucError.ErrorMessage = "Vendor is Required.";
       

        return (!ucError.IsError);
    }
    protected void LubOilEdit()
    {
        if (ViewState["PROPOSALID"] != null && ViewState["PROPOSALID"].ToString() != "")
        {
            DataTable dt = new DataTable();
            dt = PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilEdit(new Guid(ViewState["PROPOSALID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtVendorId.Text = dt.Rows[0]["FLDVENDORID"].ToString();

                txtVenderName.Text = dt.Rows[0]["FLDNAME"].ToString();
                txtVendorCode.Text = dt.Rows[0]["FLDCODE"].ToString();
                txtSailingDays.Text = dt.Rows[0]["FLDSAILINGDAYSPERYEAR"].ToString();
                ViewState["LUBOILID"] = General.GetNullableGuid(dt.Rows[0]["FLDLUBOILID"].ToString());

               
            }
        }
    }
    private void BindData()
    {
        if (ViewState["LUBOILID"] != null && ViewState["LUBOILID"].ToString() != "")
        {
            DataTable dt = PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilLineItemList(new Guid(ViewState["LUBOILID"].ToString()));

           
                gvLubOil.DataSource = dt;
           //     gvLubOil.DataBind();
           
        }
    }
    protected void gvLubOil_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
        }
    }
    protected void gvLubOil_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidLineItem(((UserControlQuick)e.Item.FindControl("ucProductAdd")).SelectedQuick
                                       ,"0.00"
                                       ,"0.00"
                                       ,"0.00"
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilLineItemInsert(new Guid(ViewState["LUBOILID"].ToString())
                                                                        , new Guid(ViewState["PROPOSALID"].ToString())
                                                                        , int.Parse(((UserControlQuick)e.Item.FindControl("ucProductAdd")).SelectedQuick)
                                                                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOilPerDayAdd")).Text)
                                                                        , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlFrequencyAdd")).SelectedValue)
                                                                        );
                BindData();
                gvLubOil.Rebind();
                

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilLineItemDelete(new Guid(((RadLabel)e.Item.FindControl("lblLubOilLineItemId")).Text));
                BindData();
             
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubOil_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if(e.Item is GridHeaderItem)
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
        if(e.Item is GridEditableItem)
        {
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()) == null)
            {
                RadLabel lblProduct = (RadLabel)e.Item.FindControl("lblProduct");
                if (lblProduct != null)
                    lblProduct.Text = "Total Cost";
                
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                    db.Visible = false;
                LinkButton dbEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (dbEdit != null)
                    dbEdit.Visible = false;
                e.Item.Font.Bold = true;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox ddlFrequencyEdit = (RadComboBox)e.Item.FindControl("ddlFrequencyEdit");
            if (ddlFrequencyEdit != null)
                ddlFrequencyEdit.SelectedValue = drv["FLDFREQUENCY"].ToString();

        }
        if(e.Item is GridFooterItem)
        {
            UserControlQuick ucProductAdd = (UserControlQuick)e.Item.FindControl("ucProductAdd");
            if (ucProductAdd != null)
            {
                ucProductAdd.QuickTypeCode = "114";
                ucProductAdd.bind();
            }
        }
    }
  

    protected void gvLubOil_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvLubOil_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

  

    protected void gvLubOil_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (!IsValidLineItem(((RadLabel)e.Item.FindControl("lblVendorProductId")).Text
                                ,"0.00"
                                ,"0.00"
                                ,"0.00"
                                    ))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilLineItemUpdate(new Guid(((RadLabel)e.Item.FindControl("lblLubOilLineItemId")).Text)
                                                                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOilPerDayEdit")).Text)
                                                                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucUnitPriceEdit")).Text)
                                                                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucPricePerYearEdit")).Text)
                                                                    , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlFrequencyEdit")).SelectedValue)
                                                                    );

            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidLineItem(string productid, string oilperday,string unitprice, string totalprice)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(productid) == null)
            ucError.ErrorMessage = "Oil Type is required.";

        if (General.GetNullableDecimal(oilperday) == null)
            ucError.ErrorMessage = "Consumption per Day is required.";

        if (General.GetNullableDecimal(unitprice) == null)
            ucError.ErrorMessage = "Unit Price is required.";

        if (General.GetNullableDecimal(totalprice) == null)
            ucError.ErrorMessage = "Total Cost is required.";

        return (!ucError.IsError);
    }
    
    protected void gvLubOil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLubOil.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
