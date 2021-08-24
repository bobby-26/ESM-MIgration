using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentDrugAndAlcoholTest : PhoenixBasePage
{
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            VesselConfiguration();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuIncidentDrugTest.AccessRights = this.ViewState;
            MenuIncidentDrugTest.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentDrugAndAlcoholTest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDrugAlcoholTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuDrugAlcoholTest.AccessRights = this.ViewState;
            MenuDrugAlcoholTest.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = null;
                ViewState["INCIDENTVESSELID"] = "";
                ViewState["INCIDENTDATE"] = "";
                BindPrimaryDetails();
                gvDrugAlcoholTest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["carid"] != null)
            {
                ViewState["CARID"] = Request.QueryString["carid"];
            }
            else
            {
                ViewState["CARID"] = null;
            }
            BindVessel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvDrugAlcoholTest.SelectedIndexes.Clear();
        gvDrugAlcoholTest.EditIndexes.Clear();
        gvDrugAlcoholTest.DataSource = null;
        gvDrugAlcoholTest.Rebind();
    }
    protected void BindVessel()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["INCIDENTVESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
        }
    }

    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXTENSIONREASON"))
        {
            Response.Redirect("../Inspection/InspectionIncidentCARExtensionReason.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
        }
        else if (dce.CommandName.ToUpper().Equals("CAR"))
        {
            Response.Redirect("../Inspection/InspectionIncidentCAR.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
        }
    }

    protected void MenuIncidentDrugTest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //int timeofincident = txtTimeOfTest.SelectedTime;

                if (!IsValidTestDate(txtDateOfTest.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionIncident.UpdateDrugTestDate(new Guid(Filter.CurrentIncidentID), DateTime.Parse(txtDateOfTest.Text + " " + txtTimeOfTest.SelectedTime.Value));
                ucStatus.Text = "Drug & Alcohol Test Date updated successfully.";
                BindPrimaryDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidTestDate(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Drug & Alcohol Test Date is required.";
        else
        {
            if (DateTime.Parse(date) > DateTime.Today)
                ucError.ErrorMessage = "'Drug & Alcohol Test Date' should not be the future date.";
        }

        if (txtTimeOfTest.SelectedTime == null)
            ucError.ErrorMessage = "'Drug & Alcohol Test Time' is required.";
        else
        {
            if (General.GetNullableDateTime(date + " " + txtTimeOfTest.SelectedTime) == null)
                ucError.ErrorMessage = "'Drug & Alcohol Test Time' is not a valid time.";
        }

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDRESULTNAME" };
        string[] alCaptions = { "Crew Name", "Rank", "Result" };

        string sortexpression;
        int? sortdirection = null;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncident.IncidentDrugAlcoholTestSearch(General.GetNullableGuid(Filter.CurrentIncidentID),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Drug And Alcohol Test", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void MenuDrugAlcoholTest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

    private void BindPrimaryDetails()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDateOfTest.Text = General.GetDateTimeToString(dr["FLDALCOHOLTESTDATE"].ToString());
            if (dr["FLDALCOHOLTESTDATETIME"].ToString() != "")
            {
                txtTimeOfTest.SelectedDate = Convert.ToDateTime(dr["FLDALCOHOLTESTDATETIME"].ToString());
            }
            if (dr["FLDALCOHOLTESTDATETIME"].ToString() == "")
            {
                txtTimeOfTest.SelectedDate = null;
            }
            ViewState["INCIDENTDATE"] = dr["FLDDATEOFINCIDENT"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDRESULTNAME" };
        string[] alCaptions = { "Crew Name", "Rank", "Result" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncident.IncidentDrugAlcoholTestSearch(General.GetNullableGuid(Filter.CurrentIncidentID),
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDrugAlcoholTest.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDrugAlcoholTest", "Drug And Alcohol Test", alCaptions, alColumns, ds);

        gvDrugAlcoholTest.DataSource = ds;
        gvDrugAlcoholTest.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvDrugAlcoholTest_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu != null && Filter.CurrentSelectedIncidentMenu.ToString() == "ilog")
                    db.Visible = false;
                else
                    db.Visible = true;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlHard ucResult = (UserControlHard)e.Item.FindControl("ucResult");
            if (ucResult != null) ucResult.SelectedHard = drv["FLDRESULT"].ToString();

            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
                if (Filter.CurrentSelectedIncidentMenu != null && Filter.CurrentSelectedIncidentMenu.ToString() == "ilog")
                    eb.Visible = false;
                else
                    eb.Visible = true;
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (Filter.CurrentSelectedIncidentMenu != null && Filter.CurrentSelectedIncidentMenu.ToString() == "ilog")
                    ab.Visible = false;
                else
                    ab.Visible = true;
            }

            RadTextBox txtCrewName = (RadTextBox)e.Item.FindControl("txtCrewName");
            RadTextBox txtCrewRank = (RadTextBox)e.Item.FindControl("txtCrewRank");

            RadTextBox txtCrewId = (RadTextBox)e.Item.FindControl("txtCrewId");
            txtCrewId.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtOfficePersonName = (RadTextBox)e.Item.FindControl("txtOfficePersonName");
            RadTextBox txtOfficePersonDesignation = (RadTextBox)e.Item.FindControl("txtOfficePersonDesignation");

            RadTextBox officestaffid = (RadTextBox)e.Item.FindControl("txtPersonOfficeId");
            officestaffid.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtPersonOfficeEmail = (RadTextBox)e.Item.FindControl("txtPersonOfficeEmail");
            txtPersonOfficeEmail.Attributes.Add("style", "visibility:hidden");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
            else
                ViewState["VESSELID"] = "Dummy";

            BindVessel();

            LinkButton cc = (LinkButton)e.Item.FindControl("imgShowCrewInCharge");
            LinkButton po = (LinkButton)e.Item.FindControl("imgPersonOffice");

            if (cc != null)
                cc.Attributes.Add("onclick",
                    "return showPickList('spnCrewInCharge', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&Date=" + ViewState["INCIDENTDATE"].ToString() + "', true); ");
            if (po != null)
                po.Attributes.Add("onclick",
                    "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
               + PhoenixModule.QUALITY + "', true);");

            if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                ab.Visible = false;

            if (ViewState["INCIDENTVESSELID"].ToString() == "0")
            {
                if (cc != null)
                    cc.Visible = false;
                if (po != null)
                    po.Visible = true;

                if (txtCrewName != null)
                    txtCrewName.Visible = false;
                if (txtCrewRank != null)
                    txtCrewRank.Visible = false;
                if (txtCrewId != null)
                    txtCrewId.Visible = false;

                if (txtOfficePersonName != null)
                    txtOfficePersonName.Visible = true;
                if (txtOfficePersonDesignation != null)
                    txtOfficePersonDesignation.Visible = true;
            }
            else
            {
                if (cc != null)
                    cc.Visible = true;
                if (po != null)
                    po.Visible = false;

                if (txtCrewName != null)
                    txtCrewName.Visible = true;
                if (txtCrewRank != null)
                    txtCrewRank.Visible = true;

                if (txtOfficePersonName != null)
                    txtOfficePersonName.Visible = false;
                if (txtOfficePersonDesignation != null)
                    txtOfficePersonDesignation.Visible = false;

                if (officestaffid != null)
                    officestaffid.Visible = false;
                if (txtPersonOfficeEmail != null)
                    txtPersonOfficeEmail.Visible = false;
            }
        }
    }

    protected void gvDrugAlcoholTest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadTextBox empid;
                if (ViewState["INCIDENTVESSELID"].ToString() == "0")
                    empid = (RadTextBox)e.Item.FindControl("txtPersonOfficeId");
                else
                    empid = (RadTextBox)e.Item.FindControl("txtCrewId");

                if (!IsValidDrugResult(empid.Text, ((UserControlHard)e.Item.FindControl("ucResultAdd")).SelectedHard.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionIncident.InsertIncidentDrugAlcoholTest(new Guid(Filter.CurrentIncidentID),
                    General.GetNullableInteger(empid.Text), null,
                    General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucResultAdd")).SelectedHard.ToString()));

                ucStatus.Text = "'Drug Test' details added successfully.";

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionIncident.DeleteIncidentDrugAlcoholTest(new Guid(((RadLabel)e.Item.FindControl("lblDrugTestid")).Text));
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
    private bool IsValidResult(string result)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(result) || result.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Result is required.";

        return (!ucError.IsError);
    }

    private bool IsValidDrugResult(string empid, string result)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["INCIDENTVESSELID"].ToString() == "0")
        {
            if (string.IsNullOrEmpty(empid))
                ucError.ErrorMessage = "Office Employee Name is required.";
        }
        else
        {
            if (string.IsNullOrEmpty(empid))
                ucError.ErrorMessage = "Crew Name is required.";
        }


        if (string.IsNullOrEmpty(result) || result.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Result is required.";

        return (!ucError.IsError);
    }

    protected void gvDrugAlcoholTest_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblempid");
            RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId");

            if (!IsValidResult(((UserControlHard)e.Item.FindControl("ucResult")).SelectedHard.ToString()))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionIncident.InsertIncidentDrugAlcoholTest(new Guid(Filter.CurrentIncidentID),
                General.GetNullableInteger(empid.Text), General.GetNullableInteger(rankid.Text),
                General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucResult")).SelectedHard.ToString()));

            ucStatus.Text = "'Drug Test' details updated successfully.";

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDrugAlcoholTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDrugAlcoholTest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
