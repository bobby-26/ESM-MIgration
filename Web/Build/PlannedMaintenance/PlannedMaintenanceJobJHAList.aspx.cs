using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobJHAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            if (!IsPostBack)
            {
                confirmDelete.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["JOBID"] = "";
                if (Request.QueryString["JOBID"] != "" && Request.QueryString["JOBID"] != null)
                    ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();
                gvJHA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindCategory();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixPlannedMaintenanceGlobalComponent.ListRiskAssessmentElement(); 
        ddlCategory.DataBind();
    }
    protected void txtJob_TextChanged(object sender, EventArgs e)
    {
        gvJHA.Rebind();
    }
    protected void gvJHA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["JOBID"].ToString()) != null)
        {
            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobJHASearch(General.GetNullableString(txtHNumber.Text), General.GetNullableInteger(ddlCategory.SelectedValue), General.GetNullableString(txtJob.Text)
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                         , gvJHA.CurrentPageIndex + 1
                                                                                         , gvJHA.PageSize
                                                                                         , ref iRowCount, ref iTotalPageCount
                                                                                         , new Guid(ViewState["JOBID"].ToString()));
        }


        gvJHA.DataSource = ds;
        gvJHA.VirtualItemCount = iRowCount;
    }
    protected void gvJHA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton imgShowHazardEdit = (ImageButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                imgShowHazardEdit.Attributes.Add("onclick", "setTimeout(function(){ return showPickList('spnHazardEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHAExtn.aspx?&vesselid=0&status=3', true); },1000)");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "display:none");

        }
    }
    protected void gvJHA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string formid = ((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text;
                string jobid = ViewState["JOBID"].ToString();

                PhoenixPlannedMaintenanceGlobalComponent.GlobalJHAMap(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblID")).Text)
                                                                        , new Guid(formid)
                                                                        , null
                                                                        , ((RadTextBox)e.Item.FindControl("txtHazardEdit")).Text
                                                                        , 1
                                                                        , new Guid(jobid));
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["NROW"] = e.Item.ItemIndex;

                RadWindowManager1.RadConfirm("Are you sure you want to delete this RA.?", "confirmDelete", 320, 150, null, "Confirm");

            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void txtHNumber_TextChanged(object sender, EventArgs e)
    {
        gvJHA.Rebind();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        gvJHA.Rebind();
    }

    protected void confirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int nRow = int.Parse(ViewState["NROW"].ToString());

            RadLabel lblID = (RadLabel)gvJHA.Items[nRow].FindControl("lblID");
            RadLabel lblHazardID = (RadLabel)gvJHA.Items[nRow].FindControl("lblHazardID");
            RadLabel lblJobText = (RadLabel)gvJHA.Items[nRow].FindControl("lblJobText");

          
            PhoenixPlannedMaintenanceGlobalComponent.GlobalJHAMap(General.GetNullableGuid(lblID.Text)
                                                         , new Guid(lblHazardID.Text)
                                                         , null
                                                         , lblJobText.ToString()
                                                         , 0
                                                         , new Guid(ViewState["JOBID"].ToString()));
            gvJHA.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}


