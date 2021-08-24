using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PlannedMaintenanceLubOilShoreAnalysis :  PhoenixBasePage

{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

          
            if (!IsPostBack)
            {
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

        DataSet ds = PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    sortexpression, sortdirection,
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

    private void LuboilStoreInsert(string vendor, DateTime? offloadeddate, DateTime? samplereceiveddate,int status,string jobs)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, vendor, offloadeddate , samplereceiveddate, status, jobs);
        ucStatus.Text = "Information Added";
    }

    private void LuboilStoreUpdate(Guid luboilid, DateTime? offloadeddate, DateTime? samplereceiveddate, int status)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, luboilid, offloadeddate, samplereceiveddate, status);
        ucStatus.Text = "Information Added";
    }

    protected void gvLuboillist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ucdate = ((UserControlDate)e.Item.FindControl("ucdateadd")).Text;
                if (!IsValidconfiguration(((UserControlMultiColumnAddress)e.Item.FindControl("ddlmulticolumnaddress")).SelectedValue,
                  ucdate))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                LuboilStoreInsert(
                    (General.GetNullableString(((UserControlMultiColumnAddress)e.Item.FindControl("ddlmulticolumnaddress")).SelectedValue)),
                    (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucdateadd")).Text)),
                    (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucsampledateadd")).Text)),
                      1,null
                   );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string ucdate = ((UserControlDate)e.Item.FindControl("ucdateedit")).Text;
                if (!IsValidconfigurationupdate(ucdate))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                LuboilStoreUpdate(
                   (Guid.Parse(((RadLabel)e.Item.FindControl("lblLuboiledit")).Text)),
                   (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucdateedit")).Text)),
                   (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucsampledateedit")).Text)),
                   (int.Parse(((RadDropDownList)e.Item.FindControl("ddlStatusedit")).SelectedValue))
                 // (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtjobedit")).Text))
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
    private bool IsValidconfiguration(string ddlmulticolumnaddress, string ucdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLuboillist;

        if (ddlmulticolumnaddress.Equals("") || ddlmulticolumnaddress.Equals("Dummy"))
            ucError.ErrorMessage = "Vendor is required.";

        if (string.IsNullOrEmpty(ucdate))
        ucError.ErrorMessage = "Sample offloaded Date is required.";
        return (!ucError.IsError);
    }
    private bool IsValidconfigurationupdate(string ucdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLuboillist;
        if (string.IsNullOrEmpty(ucdate))
          ucError.ErrorMessage = "Sample offloaded Date is required.";
        return (!ucError.IsError);
    }

    protected void gvLuboillist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            
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
            ImageButton cmdComponent = (ImageButton)e.Item.FindControl("cmdComponent");
            if (cmdComponent != null)
            {
                cmdComponent.Visible = SessionUtil.CanAccess(this.ViewState, cmdComponent.CommandName);
                cmdComponent.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceLuboilComponent.aspx?Luboilid=" + drv["FLDLUBOILID"].ToString()  + "','','1000','500'); return false;");
            }
             RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlStatusedit");
            if (ddl != null)
            {
                ddl.SelectedIndex = 0;
                ddl.SelectedValue = drv["FLDSTATUS"].ToString();
            }
            ImageButton cmdJob = (ImageButton)e.Item.FindControl("cmdJob");
            if (cmdJob != null)
            {
                 cmdJob.Visible = SessionUtil.CanAccess(this.ViewState, cmdJob.CommandName);
                 cmdJob.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobsLink.aspx?Luboilid=" + drv["FLDLUBOILID"].ToString() + "','','1000','500'); return false;");

            }
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                                    + PhoenixModule.PLANNEDMAINTENANCE + "');return true;");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
       // BindData();
        gvLuboillist.Rebind();
    }
}