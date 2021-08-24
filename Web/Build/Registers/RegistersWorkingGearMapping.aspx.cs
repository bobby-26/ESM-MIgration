using System;
using System.Data;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearMapping : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Copy", "COPY", ToolBarDirection.Right);
        MenuCopy.AccessRights = this.ViewState;
        MenuCopy.MenuList = toolbar.Show();
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RANKID"] = null;

                cblRanklst.DataSource = PhoenixRegistersRank.ListRank();
                cblRanklst.DataBind();
                cblVessel.DataSource = PhoenixRegistersVessel.ListVessel();
                cblVessel.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Copy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            //int CurrentVessel = Convert.ToInt32(Filter.CurrentVesselMasterFilter);


            if (CommandName.ToUpper().Equals("COPY"))
            {
                StringBuilder strVessel = new StringBuilder();
                StringBuilder strRank = new StringBuilder();

                foreach (ButtonListItem item in cblVessel.Items)
                {
                    if (item.Selected == true)
                    {
                        strVessel.Append(item.Value.ToString());
                        strVessel.Append(",");
                    }
                }
                if (strVessel.Length > 1)
                {
                    strVessel.Remove(strVessel.Length - 1, 1);
                }

                foreach (ButtonListItem item in cblRanklst.Items)
                {
                    if (item.Selected == true)
                    {

                        strRank.Append(item.Value.ToString());
                        strRank.Append(",");

                    }
                }
                if (strRank.Length > 1)
                {
                    strRank.Remove(strRank.Length - 1, 1);
                }

                if (IsValidCopy(strVessel.ToString(), strRank.ToString()))
                {

                    PhoenixRegistersWorkingGearMapping.WorkingGearItemMappingcopy(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                    , int.Parse(ucRank.SelectedRank)
                                                                                    , strVessel.ToString()
                                                                                    , strRank.ToString());

                    ucStatus.Text = "Working Gear successfully copied";
                    ucStatus.Visible = true;
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvRegistersworkinggearitemType_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    try
    //    {

    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {

    //            InsertWorkingGearItemTypeMapping(General.GetNullableInteger(ucRank.SelectedRank)
    //                                                         ,General.GetNullableGuid(((UserControlWorkingGearItemType)_gridView.FooterRow.FindControl("ucWorkingGearItemTypesAdd")).SelectedGearType)
    //                                                         ,General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtQuantity")).Text)
    //                                                         );


    //            BindData();
    //            SetPageNavigator();
    //        }
    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            DeleteWorkingGearItem(((Label)_gridView.Rows[nCurrentRow].FindControl("lblGearmappingid")).Text);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    private void InsertWorkingGearItemTypeMapping(int? rankid, Guid? itemid, int? qunatity)
    {
        if (!IsValidWorkingGearItemInsert(rankid, itemid))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearMapping.WorkingGearMappingInsert(General.GetNullableInteger(ddlVessel.SelectedVessel), rankid, itemid, qunatity);

    }
    private void DeleteWorkingGearItem(string WorkingGearItemMappingId)
    {
        PhoenixRegistersWorkingGearMapping.DeleteWorkingGearItemMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(WorkingGearItemMappingId));
    }
    private bool IsValidWorkingGearItemInsert(int? rankid, Guid? itemid)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (ddlVessel.SelectedVessel == null || ddlVessel.SelectedVessel == "Dummy" || ddlVessel.SelectedVessel == "")
            ucError.ErrorMessage = "Vessel is required.";
        if (rankid == null)
            ucError.ErrorMessage = "Rank is required.";

        if (itemid == null)
            ucError.ErrorMessage = "Working Gear Item Type is required.";
        return (!ucError.IsError);
    }
    private bool IsValidCopy(string strVessel, string strRank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlVessel.SelectedVessel == null || ddlVessel.SelectedVessel == "Dummy" || ddlVessel.SelectedVessel == "")
            ucError.ErrorMessage = "Select the present Vessel.";
        if (ucRank.SelectedRank == null || ucRank.SelectedRank == "Dummy" || ucRank.SelectedRank == "")
            ucError.ErrorMessage = "Select the present Rank.";

        if (strVessel.Trim().Equals(""))
            ucError.ErrorMessage = "Select atleast one Vessel to copy.";
        if (strRank.Trim().Equals(""))
            ucError.ErrorMessage = "Select atleast one Rank to copy.";

        return (!ucError.IsError);
    }
    protected void SelectAllVessel(object sender, EventArgs e)
    {
        if (chkChkAllVessel.Checked == true)
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = false;
            }
        }
    }
  

    protected void SelectAllRank(object sender, EventArgs e)
    {
        if (chkallrank.Checked == true)
        {
            foreach (ButtonListItem item in cblRanklst.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblRanklst.Items)
            {
                item.Selected = false;
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKINGGEARITEMTYPENAME" };
        string[] alCaptions = { "Working Gear Item Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearMapping.WorkingGearItemMappingSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                           , General.GetNullableInteger(ucRank.SelectedRank)
                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvRegistersworkinggearitemType.PageSize, ref iRowCount, ref iTotalPageCount);


        General.ShowExcel("Working Gear Mapping", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {


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

        string[] alColumns = { "FLDWORKINGGEARITEMTYPENAME" };
        string[] alCaptions = { "Working Gear Item Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersWorkingGearMapping.WorkingGearItemMappingSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                             , General.GetNullableInteger(ucRank.SelectedRank)
                                                                             , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                             , gvRegistersworkinggearitemType.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitemType", "Working Gear Mapping", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRegistersworkinggearitemType.DataSource = ds;
            gvRegistersworkinggearitemType.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRegistersworkinggearitemType.DataSource = "";
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvRegistersworkinggearitemType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsInteger(string strVal)
    {
        try
        {
            int intout = Int32.Parse(strVal);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void gvRegistersworkinggearitemType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                InsertWorkingGearItemTypeMapping(General.GetNullableInteger(ucRank.SelectedRank)
                                                             , General.GetNullableGuid(((UserControlWorkingGearItemType)e.Item.FindControl("ucWorkingGearItemTypesAdd")).SelectedGearType)
                                                             , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text)
                                                             );


                BindData();
                gvRegistersworkinggearitemType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteWorkingGearItem(((RadLabel)e.Item.FindControl("lblGearmappingid")).Text);
                BindData();
                gvRegistersworkinggearitemType.Rebind();
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

    protected void gvRegistersworkinggearitemType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersworkinggearitemType.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegistersworkinggearitemType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
    }
}

