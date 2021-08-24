using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;
public partial class VesselAccountPortageBillComponentTrack : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvportagebilltrack')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?" + Request.QueryString.ToString(), "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if (Request.QueryString["accessfrom"].ToString() == "1")
                toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountPortageBillComponentTrack.aspx?", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            Menuportagebilltrack.AccessRights = this.ViewState;
            Menuportagebilltrack.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                DataSet ds = PhoenixRegistersContractComponentTracking.CrewComponentTrackList(null);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcomponentgroup.DataSource = ds;
                    ddlcomponentgroup.DataBind();
                }

                ddlcomponentgroup.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlcomponentgroup.SelectedIndex = 0;
                if (Request.QueryString["accessfrom"].ToString() == "1")
                {
                    ViewState["accessfrom"] = "1";
                    lblEmpName.Visible = true;
                    txtName.Visible = true;
                    lblFrom.Visible = true;
                    txtFrom.Visible = true;
                    txtTo.Visible = true;
                    txtEmpNo.CssClass = "input";

                }
                else
                {
                    ViewState["accessfrom"] = "0";
                    lblEmpName.Visible = false;
                    txtName.Visible = false;
                    lblFrom.Visible = false;
                    txtFrom.Visible = false;
                    txtTo.Visible = false;
                }
                gvportagebilltrack.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal RetainedTotal = 0;
    decimal ReFundTotal = 0;
    string EMPID = string.Empty;

    protected void Menuportagebilltrack_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDCOMPONENTNAME", "FLDFROMDATE", "FLDTODATE", "FLDCLOSINGDATE", "FLDRETAINEDAMOUNT", "FLDREFUNDAMOUNT", "FLDREMARKS" };
                string[] alCaptions = { "File No.", "Name", "Sign on Rank", "Vessel", "Component", "From", "To", "Closing On", "Retained  Amount (USD)", "Refund Amount (USD)", "Remarks" };

                DataSet ds = PhoenixVesselAccountsPortageBill.PortageBillTrackSearch(General.GetNullableString(txtEmpNo.Text.Trim())
                                                                                    , General.GetNullableString(txtName.Text.Trim())
                                                                                    , General.GetNullableDateTime(txtFrom.Text)
                                                                                    , General.GetNullableDateTime(txtTo.Text)
                                                                                    , int.Parse(Request.QueryString["accessfrom"].ToString())
                                                                                    , General.GetNullableInteger(ddlcomponentgroup.SelectedValue)
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , (int)ViewState["PAGENUMBER"]
                                                                                    , iRowCount
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Portage bill component tracking", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvportagebilltrack.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (ViewState["accessfrom"].ToString() == "1")
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrackInsert.aspx?accessfrom=1");
                else
                    Response.Redirect("../VesselAccounts/VesselAccountPortageBillComponentTrackInsert.aspx?accessfrom=0");
            }
            else if (CommandName == "Page")
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

    protected void gvportagebilltrack_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
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
    protected void Rebind()
    {
        gvportagebilltrack.SelectedIndexes.Clear();
        gvportagebilltrack.EditIndexes.Clear();
        gvportagebilltrack.DataSource = null;
        gvportagebilltrack.Rebind();
    }
    private void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDCOMPONENTNAME", "FLDFROMDATE", "FLDTODATE", "FLDCLOSINGDATE", "FLDRETAINEDAMOUNT", "FLDREFUNDAMOUNT", "FLDREMARKS" };
            string[] alCaptions = { "File No.", "Name", "sign on Rank", "Vessel", "Component", "From", "To", "Closing On", "Retained  Amount (USD)", "Refund Amount (USD)", "Remarks" };

            DataSet ds = PhoenixVesselAccountsPortageBill.PortageBillTrackSearch(General.GetNullableString(txtEmpNo.Text.Trim())
                                                                                 , General.GetNullableString(txtName.Text.Trim())
                                                                                 , General.GetNullableDateTime(txtFrom.Text)
                                                                                 , General.GetNullableDateTime(txtTo.Text)
                                                                                 , int.Parse(Request.QueryString["accessfrom"].ToString())
                                                                                 , General.GetNullableInteger(ddlcomponentgroup.SelectedValue)
                                                                                 , sortexpression
                                                                                 , sortdirection, (int)ViewState["PAGENUMBER"]
                                                                                 , gvportagebilltrack.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount);

            General.SetPrintOptions("gvportagebilltrack", "Protagebill tracked component", alCaptions, alColumns, ds);
            string strPreviousRowID;
            if (ds.Tables[0].Rows.Count > 0)
            {
                strPreviousRowID = string.Empty;
                DataColumn GroupBy = new DataColumn();
                GroupBy.ColumnName = "FLDGROUPBY";
                ds.Tables[0].Columns.Add(GroupBy);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["FLDEMPLOYEECODE"].ToString() != "")
                        ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDEMPLOYEECODE"].ToString() + "/" + ds.Tables[0].Rows[i]["FLDEMPNAME"].ToString();

                }
                gvportagebilltrack.DataSource = ds;

            }
            else
            {
                gvportagebilltrack.DataSource = ds;
            }


            gvportagebilltrack.DataSource = ds;
            gvportagebilltrack.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvportagebilltrack_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvportagebilltrack.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvportagebilltrack_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupFooterItem)
        {
            GridGroupFooterItem groupFooter = (GridGroupFooterItem)e.Item;
            groupFooter.Style.Add("font-weight", "bold");
        }

        if (e.Item is GridEditableItem)
        {


            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                if (ViewState["accessfrom"].ToString() == "1" && drv["FLDTYPE"].ToString() == "3")
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                else
                    del.Visible = false;
            }


            EMPID = DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString();
            if (EMPID != string.Empty)
            {
                EMPID = DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString();
                decimal tmpRetained = decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDRETAINEDAMOUNT").ToString());
                decimal tmpReFund = decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDREFUNDAMOUNT").ToString());
                RetainedTotal += tmpRetained;
                ReFundTotal += tmpReFund;

            }
            RadLabel lbts = (RadLabel)e.Item.FindControl("lblcomponentname");
            UserControlToolTip ucts = (UserControlToolTip)e.Item.FindControl("ucToolTipComponent");
            ucts.Position = ToolTipPosition.TopCenter;
            ucts.TargetControlId = lbts.ClientID;
        }
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem GroupHeader = (GridGroupHeaderItem)e.Item;
            GroupHeader.DataCell.Text = GroupHeader.DataCell.Text.Substring(5);
        }
    }
    protected void gvportagebilltrack_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvportagebilltrack_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            string lblid = ((RadLabel)e.Item.FindControl("lblid")).Text;
            PhoenixVesselAccountsPortageBill.DeletePortageBillComponentrack(General.GetNullableGuid(lblid));
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();

    }

    protected void gvportagebilltrack_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            PhoenixVesselAccountsPortageBill.UpdatePortagebillTrackComponent(new Guid(((RadLabel)e.Item.FindControl("lblPortagetrackId")).Text),
              ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text);

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvportagebilltrack_OnCustomAggregate(object sender, GridCustomAggregateEventArgs e)
    {
        string colName = e.Column.UniqueName;

        if (e.Item is GridGroupFooterItem)
        {
            GridGroupHeaderItem correspondingHeader = (e.Item as GridGroupFooterItem).GroupHeaderItem;

            if (colName == "CURRENCY")
            {
                decimal counter = 0;
                GridItem[] groupChildItems = correspondingHeader.GetChildItems();
                for (int i = 0; i < groupChildItems.Length; i++)
                {
                    decimal retained = (decimal)DataBinder.Eval((groupChildItems[i] as GridDataItem).DataItem, "FLDRETAINEDAMOUNT");
                    decimal refund = (decimal)DataBinder.Eval((groupChildItems[i] as GridDataItem).DataItem, "FLDREFUNDAMOUNT");

                    counter += retained - refund;
                }
                e.Result = counter;
            }

            if (colName == "CLOSING")
            {
                e.Result = "Total :";
            }
        }
    }
}