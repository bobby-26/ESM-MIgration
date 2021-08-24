using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegistersAppraisalOwnerOccasionMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalOwnerOccasionMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOwnerMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            gvOwnerMappingTab.AccessRights = this.ViewState;
            gvOwnerMappingTab.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["OCCASIONID"] = null;

                if (Request.QueryString["occasionid"] != null)
                {
                    ViewState["OCCASIONID"] = Request.QueryString["occasionid"].ToString();
                }

                gvOwnerMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindOccasion();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindOccasion()
    {
        try
        {
            DataTable dt = PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionEdit(int.Parse(ViewState["OCCASIONID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtOccasion.Text = dt.Rows[0]["FLDOCCASION"].ToString();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvOwnerMappingTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDOWNERNAME", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Owner", "Active" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersAppraisalOwnerOccasion.RegistersAppraisalOwnerOccasionSearch(int.Parse(ViewState["OCCASIONID"].ToString())
                    , null
                    , sortexpression, sortdirection,
                    1,
                    gvOwnerMapping.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Occasion Owner Mapping", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }


    protected void gvOwnerMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnerMapping.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDOWNERNAME", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Owner","Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAppraisalOwnerOccasion.RegistersAppraisalOwnerOccasionSearch(int.Parse(ViewState["OCCASIONID"].ToString()), null
                    , sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvOwnerMapping.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

        General.SetPrintOptions("gvOwnerMapping", "Occasion Owner Mapping", alCaptions, alColumns, ds);

        gvOwnerMapping.DataSource = ds;
        gvOwnerMapping.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvOwnerMapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                PhoenixRegistersAppraisalOwnerOccasion.RegistersAppraisalOwnerOccasionInsert(
                    int.Parse(ViewState["OCCASIONID"].ToString())
                     , General.GetNullableInteger(((UserControlMultiColumnAddress)e.Item.FindControl("ucAddrOwner")).SelectedValue)
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
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

    protected void gvOwnerMapping_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            PhoenixRegistersAppraisalOwnerOccasion.RegistersAppraisalOwnerOccasionUpdate(
                    new Guid(((RadLabel)e.Item.FindControl("lblOwnerMappingid")).Text)
                     //, General.GetNullableInteger(((UserControlMultiColumnAddress)e.Item.FindControl("ucAddrOwner")).SelectedValue)
                     , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblOwnerIdEdit")).Text)
                     , General.GetNullableInteger(ViewState["OCCASIONID"].ToString())
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                     );

            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwnerMapping_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            PhoenixRegistersAppraisalOwnerOccasion.RegistersAppraisalOwnerOccasionDelete(new Guid(((RadLabel)e.Item.FindControl("lblOwnerMappingID")).Text));

            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvOwnerMapping_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

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

    protected void gvOwnerMapping_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace("ASC", "").Replace("DESC", "");
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


    protected void Rebind()
    {
        gvOwnerMapping.SelectedIndexes.Clear();
        gvOwnerMapping.EditIndexes.Clear();
        gvOwnerMapping.DataSource = null;
        gvOwnerMapping.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
