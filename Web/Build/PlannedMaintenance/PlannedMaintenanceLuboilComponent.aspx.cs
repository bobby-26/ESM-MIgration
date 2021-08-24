using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;


public partial class PlannedMaintenanceLuboilComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();


            if (!IsPostBack)
            {
                if (Request.QueryString["Luboilid"] != null)
                {
                    ViewState["LUBOILID"] = Request.QueryString["Luboilid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvLuboillist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvLuboillist.SelectedIndexes.Clear();
        gvLuboillist.EditIndexes.Clear();
        gvLuboillist.DataSource = null;
        gvLuboillist.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreanalysisSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Guid.Parse(ViewState["LUBOILID"].ToString()), sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvLuboillist.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        gvLuboillist.DataSource = ds;
        gvLuboillist.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvLuboillist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLuboillist.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void LuboilStoreAnalysisInsert(Guid luboilid, Guid componentid,string sample, DateTime? sampledate, int? rating)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreAnalysisInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, luboilid, componentid, sample, sampledate, rating);
    }
    private void LuboilStoreAnalysisDelete(Guid id)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreAnalysisDelete(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    }
    private void LuboilStoreAnalysisUpdate(Guid id,  string sample, DateTime? sampledate, int? rating)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreAnalysisUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id, sample, sampledate, rating);
    }
    protected void gvLuboillist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ucdate = ((UserControlDate)e.Item.FindControl("ucsampledateadd")).Text;
                if (!IsValidconfiguration(((UserControlMultiColumnComponents)e.Item.FindControl("ucComponentadd")).SelectedValue,
                  ((RadTextBox)e.Item.FindControl("txtsampleadd")).Text, ucdate))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }


                LuboilStoreAnalysisInsert((Guid.Parse(ViewState["LUBOILID"].ToString())),
                    (Guid.Parse(((UserControlMultiColumnComponents)e.Item.FindControl("ucComponentadd")).SelectedValue)),
                    (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtsampleadd")).Text.Trim())),
                    (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucsampledateadd")).Text)),
                    null
                   );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string ucdate = ((UserControlDate)e.Item.FindControl("ucsampledateedit")).Text;
                if (!IsValidconfigurationupdate(
                  ((RadTextBox)e.Item.FindControl("txtsampleedit")).Text, ucdate))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }


                LuboilStoreAnalysisUpdate(
                   (Guid.Parse(((RadLabel)e.Item.FindControl("lblidedit")).Text)),
                   (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtsampleedit")).Text.Trim())),
                   (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucsampledateedit")).Text)),
                    (General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlRatingedit")).SelectedValue))

                  );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblid")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    protected void gvLuboillist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton cmdProperty = (ImageButton)e.Item.FindControl("cmdProperty");
            if (cmdProperty != null)
            {
                cmdProperty.Visible = SessionUtil.CanAccess(this.ViewState, cmdProperty.CommandName);
                cmdProperty.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp2','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancePropertyValueList.aspx?Componentid=" + drv["FLDID"].ToString() + "','','800','400'); return false;");
            }
            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlRatingedit");
            if (ddl != null)
            {
                ddl.SelectedIndex = 0;

                ddl.SelectedValue = drv["FLDRATING"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LuboilStoreAnalysisDelete(Guid.Parse(ViewState["ID"].ToString()));
            Rebind();
            //ucStatus.Text = "Information deleted";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidconfiguration(string ucComponentadd,string txtsampleadd, string ucdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLuboillist;

        if (ucComponentadd.Equals("") || ucComponentadd.Equals("Dummy"))
            ucError.ErrorMessage = "Component is required.";

        if (txtsampleadd.Trim().Equals(""))
            ucError.ErrorMessage = "Sample is required.";

        if (string.IsNullOrEmpty(ucdate))
            ucError.ErrorMessage = "Sampled Date is required.";
        return (!ucError.IsError);
    }
    private bool IsValidconfigurationupdate(string txtsampleedit, string ucdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLuboillist;

        if (txtsampleedit.Trim().Equals(""))
            ucError.ErrorMessage = "Sample is required.";

        if (string.IsNullOrEmpty(ucdate))
            ucError.ErrorMessage = "Sampled Date is required.";
        return (!ucError.IsError);
    }


}