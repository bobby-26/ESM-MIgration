using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Export2XL;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobHistoryTemplateList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvHistoryTemplateList.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvHistoryTemplateList.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobHistoryTemplateList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobHistoryTemplateList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar.Show();
            //cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["JOBID"] = "";
                if (Request.QueryString["JOBID"] != "" && Request.QueryString["JOBID"] != null)
                    ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();
                gvHistoryTemplateList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceJobHistoryTemplateList.JobHistoryTemplateSearch(General.GetNullableGuid(ViewState["JOBID"].ToString()), General.GetNullableString(txtTemplateName.Text)
                                                                                           , sortexpression
                                                                                           , sortdirection
                                                                                           , gvHistoryTemplateList.CurrentPageIndex + 1
                                                                                           , gvHistoryTemplateList.PageSize
                                                                                           , ref iRowCount, ref iTotalPageCount);
        if (dt.Rows.Count > 0)
        {

            gvHistoryTemplateList.DataSource = dt;
            gvHistoryTemplateList.VirtualItemCount = iRowCount;
        }
        else
        {
            gvHistoryTemplateList.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



    }
    protected void MenuHistoryTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvHistoryTemplateList.CurrentPageIndex = 0;
                gvHistoryTemplateList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtTemplateName.Text = "";
                gvHistoryTemplateList.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvHistoryTemplateList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{

        //    RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
        //    RadLabel formid = (RadLabel)e.Item.FindControl("lblFormID");
        //    DataRowView drv = (DataRowView)e.Item.DataItem;
        //    string VerifiedYN = ((RadLabel)e.Item.FindControl("lblVerifiedYN")).Text;
        //    cb.Checked = VerifiedYN.ToString().Equals("0") ? false : true;

        //    string jvscript = "";

        //    if (cb != null) jvscript = "javascript:selectJob('" + ViewState["JOBID"].ToString() + "','" + formid.Text + "',this);";
        //    if (cb != null) cb.Attributes.Add("onclick", jvscript);

        //}
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHistoryTemplateList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvHistoryTemplateList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
            RadLabel formid = (RadLabel)e.Item.FindControl("lblFormID");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            cb.Checked = drv["FLDVERIFIEDYN"].ToString().Equals("0") ? false : true;

            string jvscript = "";

            if (cb != null) jvscript = "javascript:selectJob('" + ViewState["JOBID"].ToString() + "','" + formid.Text + "',this);";
            if (cb != null) cb.Attributes.Add("onclick", jvscript);

        }
    }
    protected void gvHistoryTemplateList_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

    }
    private bool IsValidHistoryTemplate(string Componentjobid)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (Componentjobid == null || Componentjobid == "")
        {
            ucError.ErrorMessage = "Please select a Job";
        }

        return (!ucError.IsError);
    }

}


