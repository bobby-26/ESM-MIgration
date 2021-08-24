using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Registers_RegistersRankConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRankConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersRankConfiguration.aspx", "<b>Find</b>", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersRankConfiguration.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRankConfiguration.AccessRights = this.ViewState;
        MenuRankConfiguration.MenuList = toolbar.Show();

        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindFunctionName();
                gvRankConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRankConfiguration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRankConfiguration.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlFunctionName.SelectedValue = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvRankConfiguration.SelectedIndexes.Clear();
        gvRankConfiguration.EditIndexes.Clear();
        gvRankConfiguration.DataSource = null;
        gvRankConfiguration.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDRANKNAME", "FLDRANK"};
        string[] alCaptions = { "Rank", "Rank List"};
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixRegistersRankConfiguration.SearchRankConfiguration(General.GetNullableInteger(ddlFunctionName.SelectedValue)
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvRankConfiguration.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvRankConfiguration", "Rank Configuration", alCaptions, alColumns,ds);

        gvRankConfiguration.DataSource = dt;
        gvRankConfiguration.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvRankConfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadCheckBoxList chk = ((RadCheckBoxList)e.Item.FindControl("chkRanklistAdd"));
                               
                if (!IsValidRankConfiguration(                 
                     (((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank)
                     , General.RadCheckBoxList(chk)
                    , (ddlFunctionName.SelectedValue)
                    ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersRankConfiguration.InsertRankConfiguration(                     
                      int.Parse(((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank)
                     ,General.RadCheckBoxList(chk)
                     ,int.Parse(ddlFunctionName.SelectedValue)
                );
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRankConfiguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
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

            UserControlRank ddl1 = (UserControlRank)e.Item.FindControl("ucRankEdit");
            if (ddl1 != null)
                ddl1.SelectedRank = drv["FLDRANKID"].ToString();

            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkRanklistEdit");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersRank.ListRank();
                chkUserGroupEdit.DataBindings.DataTextField = "FLDRANKNAME";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDRANKID";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk = ((RadCheckBoxList)e.Item.FindControl("chkRanklistEdit"));                
                General.RadBindCheckBoxList(chk, drv["FLDRANKLIST"].ToString());
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            RadCheckBoxList chka = ((RadCheckBoxList)e.Item.FindControl("chkRanklistAdd"));
            if (chka != null)
            {
                chka.DataSource = PhoenixRegistersRank.ListRank();
                chka.DataBindings.DataTextField = "FLDRANKNAME";
                chka.DataBindings.DataValueField = "FLDRANKID";
                chka.DataBind();
            }
        }
    }

    private void BindFunctionName()
    {
        DataTable dt = PhoenixRegistersRankConfiguration.ListRankConfigurationType();
        ddlFunctionName.DataSource = dt;
        ddlFunctionName.DataTextField = "FLDFUNCTIONNAME";
        ddlFunctionName.DataValueField = "FLDFUNCTIONID";
        ddlFunctionName.DataBind();
        ddlFunctionName.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

    private bool IsValidRankConfiguration(string rank, string ranklist,string Functionname)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvRankConfiguration;

        if ((General.GetNullableInteger(Functionname))==null)
            ucError.ErrorMessage = "Function is required.";
        
        if (General.GetNullableInteger(rank)==null)
            ucError.ErrorMessage = "Rank is required.";

        if (ranklist.Trim().Equals(""))
            ucError.ErrorMessage = "Rank List is required.";

        return (!ucError.IsError);
    }

    protected void gvRankConfiguration_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            ViewState["Dtkey"] = ((RadLabel)e.Item.FindControl("lblDtkey")).Text;
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRankConfiguration_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkRanklistEdit");

            if (!IsValidRankConfiguration(
                 ((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank
                 , General.RadCheckBoxList(chk)
                , ddlFunctionName.SelectedValue
                ))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersRankConfiguration.UpdateRankConfiguration(
                  int.Parse(((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank)
                , General.RadCheckBoxList(chk)
                , new Guid(((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text)
            );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRankConfiguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRankConfiguration.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRankConfiguration_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersRankConfiguration.DeleteRankConfiguration(new Guid(ViewState["Dtkey"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}