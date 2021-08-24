using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRoundsJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPONENTJOBID"] = null;
            if (Request.QueryString["COMPONENTJOBID"] != null)
                ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"];
           
            gvRoundsJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponentJob.aspx?mode=multi&componentjobid=" + ViewState["COMPONENTJOBID"].ToString() + "', true);", "Component Jobs", "<i class=\"fas fa-cogs\"></i>", "ADDPART");

        MenuRoundsJob.MenuList = toolbar.Show();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceComponentJob.RoundsJobSearch(General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , gvRoundsJob.CurrentPageIndex + 1
                                                                            , gvRoundsJob.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        if (dt.Rows.Count > 0)
        {
            gvRoundsJob.DataSource = dt;
            gvRoundsJob.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRoundsJob.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRoundsJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    if(e.CommandName.ToUpper().Equals("DELETE"))
        //    {

        //        int iMessageCode = 0;
        //        string iMessageText = "";
        //        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        //        PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(ViewState["COMPONENTJOBID"].ToString())
        //                                                        , null
        //                                                        //, gvRoundsJob.DataKeys[e.RowIndex].Value.ToString()
        //                                                        , e.Item.RowIndex.ToString()
        //                                                        , 1
        //                                                        , ref iMessageCode
        //                                                        , ref iMessageText);
        //    }
        //    BindData();
        //    gvRoundsJob.Rebind();
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void gvRoundsJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvRoundsJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lbJobTitle = (RadLabel)e.Item.FindControl("lblJobTitle");
                RadLabel lbCompName = (RadLabel)e.Item.FindControl("lblCompName");
                UserControlToolTip uctJobTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipJobTitle");
                UserControlToolTip uctCompName = (UserControlToolTip)e.Item.FindControl("ucToolTipCompName");
                uctJobTitle.Position = ToolTipPosition.TopCenter;
                uctCompName.Position = ToolTipPosition.TopCenter;
                uctJobTitle.TargetControlId = lbJobTitle.ClientID;
                uctCompName.TargetControlId = lbCompName.ClientID;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    //if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    //    db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRoundsJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRoundsJob_SortCommand(object source, GridSortCommandEventArgs e)
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


    protected void gvRoundsJob_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";

            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            string compjobid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOMPONENTJOBID"].ToString();

            PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(ViewState["COMPONENTJOBID"].ToString()), null
                , compjobid
                , 1, ref iMessageCode, ref iMessageText);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
