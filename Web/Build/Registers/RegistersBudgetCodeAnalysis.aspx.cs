using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersBudgetCodeAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
          
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetCodeAnalaysis')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBudgetCodeAnalysis.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBudgetCodeAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuBudgetCodeAnalaysis.AccessRights = this.ViewState;
            MenuBudgetCodeAnalaysis.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvBudgetCodeAnalaysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuBudgetCodeAnalaysis_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlType.SelectedQuick = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvBudgetCodeAnalaysis.Rebind();

            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void BindData(object sender, EventArgs e)
    {
        BindData();
    }
  
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTYPE", "FLDSUBACCOUNT", "FLDNAME", "FLDOWNERBUDGETCODE", "FLDRANK" };
        string[] alCaptions = { "Type", "Budget Code", "Owner Name", "Owner Budget Code", "Criteria" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ddlType.SelectedQuick == PhoenixCommonRegisters.GetQuickCode(157, "CCA"))
        {
            gvBudgetCodeAnalaysis.Columns[3].Visible = false;
            gvBudgetCodeAnalaysis.Columns[4].Visible = true;
        }
        else
        {
            gvBudgetCodeAnalaysis.Columns[3].Visible = true ;
            gvBudgetCodeAnalaysis.Columns[4].Visible = false;
        }

        DataSet ds = PhoenixRegistersBudgetCodeAnalysis.SearchBudgetCodeAnalysis(General.GetNullableInteger(ddlType.SelectedQuick)
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , gvBudgetCodeAnalaysis.CurrentPageIndex+1
                                                                        , gvBudgetCodeAnalaysis.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);
        General.SetPrintOptions("gvBudgetCodeAnalaysis", "Budget Code Analaysis", alCaptions, alColumns, ds);

            gvBudgetCodeAnalaysis.DataSource = ds.Tables[0];
                  gvBudgetCodeAnalaysis.VirtualItemCount = iRowCount;
              ViewState["ROWCOUNT"] = iRowCount;
        //gvBudgetCodeAnalaysis.Rebind();

     
    }

    protected void gvBudgetCodeAnalaysis_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT") || e.CommandName.ToString() == string.Empty)
                return;
         

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadCheckBoxList chka = (RadCheckBoxList)e.Item.FindControl("chkCriteriaAdd");
                if (!IsValidBudgetCodeAnalysis(ddlType.SelectedQuick,
               ((RadTextBox)e.Item.FindControl("txtBudgetId")).Text,
               ((RadTextBox)e.Item.FindControl("txtOwnerId")).Text,
              ((RadTextBox)e.Item.FindControl("txtVariantBudgetId")).Text,
               ((RadDropDownList)e.Item.FindControl("ddlCaptaincashAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersBudgetCodeAnalysis.InsertBudgetCodeAnalysis(General.GetNullableInteger(ddlType.SelectedValue)
                                                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetId")).Text)
                                                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtOwnerId")).Text)
                                                                            , null, General.RadCheckBoxList(chka)
                                                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtVariantBudgetId")).Text)
                                                                            , ((RadDropDownList)e.Item.FindControl("ddlCaptaincashAdd")).SelectedValue);

                BindData();
                gvBudgetCodeAnalaysis.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBudgetCodeAnalaysis_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetCodeAnalaysis.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBudgetCodeAnalaysis_ItemDataBound(Object sender, GridItemEventArgs e)
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

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkCriteriaEdit");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersRank.ListRank();
                chkUserGroupEdit.DataBindings.DataTextField = "FLDRANKNAME";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDRANKID";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk =( (RadCheckBoxList)e.Item.FindControl("chkCriteriaEdit"));
                General.RadBindCheckBoxList(chk, drv["FLDC1"].ToString());
            }
            RadDropDownList ddlCaptainCash = (RadDropDownList)e.Item.FindControl("ddlCaptaincashEdit");
            if (ddlCaptainCash != null)
            {
                ddlCaptainCash.DataSource = PhoenixRegistersPortageBillStandardComponent.ListCaptainCashComponent(null);
                ddlCaptainCash.DataTextField = "FLDCAPTAINCASHTYPE";
                ddlCaptainCash.DataValueField = "FLDDTKEY";
                ddlCaptainCash.DataBind();
                ddlCaptainCash.SelectedValue = drv["FLDC2DESC"].ToString();

            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            RadCheckBoxList chka = (RadCheckBoxList)e.Item.FindControl("chkCriteriaAdd");
            if (chka != null)
            {
                chka.DataSource = PhoenixRegistersRank.ListRank();
                chka.DataBindings.DataTextField = "FLDRANKNAME";
                chka.DataBindings.DataValueField = "FLDRANKID";
                chka.DataBind();
            }
            RadDropDownList ddlCaptainCash = (RadDropDownList)e.Item.FindControl("ddlCaptaincashAdd");
            if (ddlCaptainCash != null)
            {
                ddlCaptainCash.DataSource = PhoenixRegistersPortageBillStandardComponent.ListCaptainCashComponent(null);
                ddlCaptainCash.DataTextField = "FLDCAPTAINCASHTYPE";
                ddlCaptainCash.DataValueField = "FLDDTKEY";
                ddlCaptainCash.DataBind();

            }
        }

    }

  
    protected void gvBudgetCodeAnalaysis_OnSortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void imgShowAccount1_Click(object sender, EventArgs e)
    {
        GridViewRow gv = (GridViewRow)((System.Web.UI.Control)sender).NamingContainer;
        string budgetid = ((RadTextBox)gv.FindControl("txtBudgetId")).Text;
        string ownerid = ((RadTextBox)gv.FindControl("txtOwnerId")).Text;

        String script = String.Format("showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?Budgetid=" + budgetid + "&Ownerid=" + ownerid + "', true); ");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

    }

    protected void imgShowAccount_Click(object sender, EventArgs e)
    {
        GridViewRow gv = (GridViewRow)((System.Web.UI.Control)sender).NamingContainer;
        string budgetid = ((RadTextBox)gv.FindControl("txtBudgetIdEdit")).Text;
        string ownerid = ((RadTextBox)gv.FindControl("txtOwnerIdEdit")).Text;

        String script = String.Format("showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?Budgetid=" + budgetid + "&Ownerid=" + ownerid + "', true); ");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

    }
  
    private bool IsValidBudgetCodeAnalysis(string type, string budgetid, string ownerid, string variantbudgetid, string Captaincash)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvBudgetCodeAnalaysis;

        //if ((type.Trim().Equals("")) || (ddlType.SelectedQuick == PhoenixCommonRegisters.GetQuickCode(157, "PBA")))
        //             ucError.ErrorMessage = "Type is required.";

        if (!General.GetNullableInteger(type).HasValue)
            ucError.ErrorMessage = "Type is required.";
        if (General.GetNullableInteger(type).HasValue && ddlType.SelectedQuick == PhoenixCommonRegisters.GetQuickCode(157, "CCA"))
        {
            if ((General.GetNullableGuid(Captaincash))==null)
                ucError.ErrorMessage = "Criteria is required.";
        }
        if (General.GetNullableInteger(budgetid)==null)
            ucError.ErrorMessage = "Budget Code is required.";
        if (!General.GetNullableInteger(ownerid).HasValue)
            ucError.ErrorMessage = "Owner is required.";

        if (General.GetNullableInteger(variantbudgetid)==null)
            ucError.ErrorMessage = "Variant Budget Code is required.";
        

        return (!ucError.IsError);
    }
 

    protected void gvBudgetCodeAnalaysis_OnDeleteCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        Guid dtkey = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text).Value;
        PhoenixRegistersBudgetCodeAnalysis.DeleteBudgetCodeAnalysis(dtkey);
        BindData();
    }
    protected void gvBudgetCodeAnalaysis_OnUpdateCommand(object sender, GridCommandEventArgs e)
       
    {
        try
        {
           
            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkCriteriaEdit");
            chk.DataBind();
           // if (!IsValidBudgetCodeAnalysis(General.RadCheckBoxList(chk),
           // ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
           //((RadTextBox)e.Item.FindControl("txtOwnerIdEdit")).Text,
           // ((RadTextBox)e.Item.FindControl("txtVariantBudgetIdEdit")).Text,
           // ((RadDropDownList)e.Item.FindControl("ddlCaptaincashEdit")).SelectedValue))

            if (!IsValidBudgetCodeAnalysis(ddlType.SelectedQuick,
           ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
           ((RadTextBox)e.Item.FindControl("txtOwnerIdEdit")).Text,
          ((RadTextBox)e.Item.FindControl("txtVariantBudgetIdEdit")).Text,
           ((RadDropDownList)e.Item.FindControl("ddlCaptaincashEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersBudgetCodeAnalysis.UpdateBudgetCodeAnalysis(int.Parse(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text)
                , int.Parse(((RadTextBox)e.Item.FindControl("txtOwnerIdEdit")).Text)
                , null
                , General.RadCheckBoxList(chk)
                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text).Value
                , int.Parse(((RadTextBox)e.Item.FindControl("txtVariantBudgetIdEdit")).Text)
                , ((RadDropDownList)e.Item.FindControl("ddlCaptaincashEdit")).SelectedValue);


            BindData();
            gvBudgetCodeAnalaysis.Rebind();
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
}