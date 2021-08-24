using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewCostEvaluationRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCostRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequest.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequest.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
                {
                    if (Filter.CurrentCrewChangePlanFilterSelection != null)
                    {
                        ucVessel.SelectedVessel = Filter.CurrentCrewChangePlanFilterSelection["ucVessel"];
                        ViewState["VESSELID"] = Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString();
                        ucVessel.Enabled = false;
                    }
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;
                ViewState["CITYID"] = null;
                gvCostRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                MenuCrewCostGeneral.AccessRights = this.ViewState;
                toolbar.AddButton("Vessel List", "VESSELLIST");
                MenuCrewCostGeneral.MenuList = toolbar.Show();
            }
            if (Filter.CurrentMenuCodeSelection != "CRW-TRV-CCE-REQ") 
                toolbargrid.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequestGeneral.aspx?vslid=" + ViewState["VESSELID"].ToString(), "Add", "<i class=\"fas fa-plus\"></i>", "ADD");
            MenuCostRequest.AccessRights = this.ViewState;
            MenuCostRequest.MenuList = toolbargrid.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTNO", "FLDVESSELNAME", "FLDNOOFJOINER", "FLDNOOFOFFSIGNER", "FLDREQUESTEDDATE", "FLDVESSELPORTS", "FLDARRIVALDATE", "FLDSTATUSNAME" };
            string[] alCaptions = { "Request No.", "Vessel Name", "Number of OnSigners", "No of OffSigners", "Requested Date", "Port", "Arrival Date", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixCrewCostEvaluationRequest.SearchCrewCostEvaluationRequest(
                    General.GetNullableString(txtRequestNo.Text)
                  , General.GetNullableInteger(General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null ? ucVessel.SelectedVessel : ViewState["VESSELID"].ToString())
                  , General.GetNullableDateTime(txtFromDate.Text)
                  , General.GetNullableDateTime(txtToDate.Text)
                  , General.GetNullableInteger(ucRequestStatus.SelectedHard)
                 , sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvCostRequest.PageSize, ref iRowCount, ref iTotalPageCount);

        
            gvCostRequest.DataSource = ds;

            if (ViewState["REQUESTID"] == null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    if (ViewState["REQUESTID"] != null)
                        ViewState["PAGEURL"] = "../Crew/CrewCostEvaluationGeneral.aspx";
                }
            }

            gvCostRequest.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvCostRequest", "Crew Cost Evaluation Request", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCostRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtRequestNo.Text = "";
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ucRequestStatus.SelectedHard = "";
                gvCostRequest.CurrentPageIndex = 0;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewCostGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VESSELLIST"))
            {
                Response.Redirect("CrewChangePlanFilter.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTNO", "FLDVESSELNAME", "FLDNOOFJOINER", "FLDNOOFOFFSIGNER", "FLDREQUESTEDDATE", "FLDVESSELPORTS", "FLDARRIVALDATE", "FLDSTATUSNAME" };
            string[] alCaptions = { "Request No.", "Vessel Name", "Number of OnSigners", "No of OffSigners", "Requested Date", "Port", "Arrival Date", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixCrewCostEvaluationRequest.SearchCrewCostEvaluationRequest(
                     General.GetNullableString(txtRequestNo.Text)
                   , General.GetNullableInteger(General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null ? ucVessel.SelectedVessel : ViewState["VESSELID"].ToString())
                   , General.GetNullableDateTime(txtFromDate.Text)
                   , General.GetNullableDateTime(txtToDate.Text)
                   , General.GetNullableInteger(ucRequestStatus.SelectedHard)
                  , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Crew Cost Evaluation Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void gvCostRequest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                string RequestId = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string RequestNo = ((RadLabel)e.Item.FindControl("lblRequestNo")).Text;
                ViewState["REQUESTID"] = RequestId;
                PhoenixCrewCostEvaluationRequest.RequestNumber = RequestNo;
                Response.Redirect("CrewCostEvaluationRequestGeneral.aspx?REQUESTID=" + RequestId, false);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string RequestId = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                PhoenixCrewCostEvaluationRequest.CrewChangeCostCancelRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(RequestId));
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
    protected void gvCostRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCostRequest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCostRequest_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to cancel this request'); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

           
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        gvCostRequest.SelectedIndexes.Clear();
        gvCostRequest.EditIndexes.Clear();
        gvCostRequest.DataSource = null;
        gvCostRequest.Rebind();
    }
}
