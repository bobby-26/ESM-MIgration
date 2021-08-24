using System;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionOperationalHazardPickList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOperationalHazardPickList.aspx?", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOperationalHazardPickList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOperationalHazardPickList.aspx", "Bulk Upload", "<i class=\"fa-upload\"></i>", "BULKSELECT");        
        MenuRA.AccessRights = this.ViewState;
        MenuRA.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["JHAID"] = Request.QueryString["JhaId"].ToString();
            BindProcess();
        }

    }
    private void BindProcess()
    {
        if ((ViewState["JHAID"] != null) && (ViewState["JHAID"].ToString() != ""))
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRiskAssessmentJobHazard(new Guid(ViewState["JHAID"].ToString()));

            ucElement.SelectedCategory = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
        }
    }

    protected void MenuRA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvOperationalHazard.Rebind();
            }

            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtOperationalHazard.Text = "";
                gvOperationalHazard.Rebind();
            }
            else if (CommandName.ToUpper().Equals("BULKSELECT"))
            {
                if (InspectionFilter.FilterCurrentAspects != null)
                {
                    ArrayList selectedHazards = new ArrayList();

                    selectedHazards = (ArrayList)InspectionFilter.FilterCurrentAspects;
                    if (selectedHazards != null && selectedHazards.Count > 0)
                    {
                        foreach (Guid Operationalid in selectedHazards)
                        {
                            string Script = "";
                            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                            Script += "fnReloadList('codehelp1','ifMoreInfo');";
                            Script += "</script>" + "\n";

                            PhoenixInspectionRiskAssessmentJobHazardExtn.JHAOperationalHazardMapping(General.GetNullableGuid(ViewState["JHAID"].ToString())
                                                              , General.GetNullableGuid(Operationalid.ToString()));

                            gvOperationalHazard.Rebind();

                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                        }
                        InspectionFilter.FilterCurrentAspects = null;
                    }
                }
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["JHAID"] != null) && (ViewState["JHAID"].ToString() != ""))
        {
            BindGrid();
        }
    }

    protected void gvOperationalHazard_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("ASPECT"))
        {
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            PhoenixInspectionRiskAssessmentJobHazardExtn.JHAOperationalHazardMapping(General.GetNullableGuid(ViewState["JHAID"].ToString())
                                              , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOperationalid")).Text));

            gvOperationalHazard.Rebind();

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }

        if (e.CommandName.ToUpper().Equals("TEDIT"))
        {
            RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
            Response.Redirect("../Inspection/InspectionOperationalRiskControlMapping.aspx?callfrom=jha&raid=" + ViewState["JHAID"].ToString() + "&Operationalhazardid=" + lblOperationalId.Text, false);
        }
    }

    private void BindGrid()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAOperationalHazardPendingList(General.GetNullableInteger(ucElement.SelectedCategory), General.GetNullableString(txtOperationalHazard.Text.Trim()), General.GetNullableGuid(ViewState["JHAID"].ToString()));
        gvOperationalHazard.DataSource = ds;

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            gvOperationalHazard.Columns[4].Visible = false;
        }
    }

    protected void ucElement_TextChanged(object sender, EventArgs e)
    {
        gvOperationalHazard.Rebind();
    }

    protected void gvOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedChks = new ArrayList();
            Guid index = new Guid();
            foreach (GridDataItem gvrow in gvOperationalHazard.Items)
            {
                bool result = false;
                index = new Guid(gvrow.GetDataKeyValue("FLDRISKASSESSEMENTOPERATIONALHAZARDID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;
                }

                if (InspectionFilter.FilterCurrentAspects != null)
                {
                    SelectedChks = (ArrayList)InspectionFilter.FilterCurrentAspects;
                }
                if (result)
                {
                    if (!SelectedChks.Contains(index))
                        SelectedChks.Add(index);
                }
                else
                    SelectedChks.Remove(index);
            }
            if (SelectedChks != null && SelectedChks.Count > 0)
                InspectionFilter.FilterCurrentAspects = SelectedChks;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetSelectedChks()
    {
        try
        {
            if (Filter.CurrentSelectedDeficiency != null)
            {
                ArrayList SelectedChks = (ArrayList)Filter.CurrentSelectedBulkDeficiencies;
                Guid index = new Guid();
                if (SelectedChks != null && SelectedChks.Count > 0)
                {
                    foreach (GridViewRow row in gvOperationalHazard.Items)
                    {
                        RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                        index = new Guid(gvOperationalHazard.MasterTableView.Items[0].GetDataKeyValue("FLDRISKASSESSEMENTOPERATIONALHAZARDID").ToString());
                        if (SelectedChks.Contains(index))
                        {
                            RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
