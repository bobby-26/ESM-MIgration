using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewOffshoreDMRVoyageData : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {

            }

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "VOYAGE");
            toolbarvoyagetap.AddButton("Charter", "VOYAGEDATA");
            if (ViewState["VESSELID"] != null)
            {
                if (int.Parse(ViewState["VESSELID"].ToString()) > 0)
                {
                    toolbarvoyagetap.AddButton("Commenced ROB", "CARGOROB");
                    toolbarvoyagetap.AddButton("Completed ROB", "COMPLETCARGOROB");
                   

                }
            }
           
           
            
           

            MenuVoyageTap.AccessRights = this.ViewState;
            MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
            MenuVoyageTap.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarvoyage = new PhoenixToolbar();
        
            toolbarvoyage.AddButton("Attachment", "ATTACHMENT",ToolBarDirection.Right);
            toolbarvoyage.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarvoyage.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CHARTERID"] = "";

            if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
            {
                Filter.CurrentVPRSVoyageSelection = null;
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "0";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ChangesCommencedDetailsProperties(false);
                    ChangesCompletedDetailsProperties(false);
                    ChangesProposedDetailsProperties(true);
                }
                else
                {
                    ucCharterer.CssClass = "input";
                    ChangesCommencedDetailsProperties(true);
                    ChangesCompletedDetailsProperties(false);
                    ChangesProposedDetailsProperties(false);
                    ucDMRCharter.Enabled = true;
                    ucDMRCharter.CssClass = "input";
                }
            }
            else
            {
                UcVessel.Enabled = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    ChangesCommencedDetailsProperties(false);
                    ChangesCompletedDetailsProperties(false);
                    ChangesProposedDetailsProperties(true);
                }
                else
                {
                    ucCharterer.Enabled = false;
                    ChangesCommencedDetailsProperties(false);
                    ChangesCompletedDetailsProperties(true);
                    ChangesProposedDetailsProperties(false);
                }
            }

            if (ViewState["VESSELID"] != null)
            {
                DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListChartererBasedOnCommencement(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection), 0);
                ucDMRCharter.CharterList = ds;
            }
            BindTrading();
            BindData();
        }        
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;        
    }
    protected void ucCharterer_Changed(object sender, EventArgs e)
    {
        ViewState["CHARTERERID"] = ucCharterer.SelectedAddress;
    } 

    private void BindData()
    {
        if (Filter.CurrentVPRSVoyageSelection != null)
        {
            DataSet ds = PhoenixCrewOffshoreDMRVoyageData.EditVoyageData(General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection));
            DataTable dt = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 0)
            {
                UcVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
                ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();

                ucCommencedDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDCOMMENCEDDATETIME"]);
                ucCompletedDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDCOMPLETEDDATE"]);
                txtChartererID.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
                ucCharterer.SelectedAddress = dt.Rows[0]["FLDCHARTERER"].ToString();

                ucCommensedPort.SelectedValue = dt.Rows[0]["FLDCOMMENCEDPORTID"].ToString();
                ucCommensedPort.Text = dt.Rows[0]["FLDCOMMENCEDPORTNAME"].ToString();
                ucCompletedPort.SelectedValue = dt.Rows[0]["FLDCOMPLETEDPORTID"].ToString();
                ucCompletedPort.Text = dt.Rows[0]["FLDCOMPLETEDPORTNAME"].ToString();

                //txtTimeOfCommenced.DbSelectedDate = String.Format("{0:hh:mm}", dt.Rows[0]["FLDCOMMENCEDDATETIME"]);  //Convert.ToDateTime(dt.Rows[0]["FLDCOMMENCEDDATETIME"]).ToString("hh:mm"); //.ToString();// String.Format("{ 0:HH:mm}", dt.Rows[0]["FLDCOMMENCEDDATETIME"]);
                //txtTimeOfCompleted.DbSelectedDate = String.Format("{0:hh:mm}", dt.Rows[0]["FLDCOMPLETEDDATE"]);  //Convert.ToDateTime(dt.Rows[0]["FLDCOMMENCEDDATETIME"]).ToString("hh:mm");// String.Format("{0:HH:mm}", dt.Rows[0]["FLDCOMPLETEDDATE"]);
                txtChartererInstruction.Text = dt.Rows[0]["FLDCHARTERERVOYAGEINSTRUCTION"].ToString();
                ddlVolume.SelectedValue = dt.Rows[0]["FLDCHARTERERUNIT"].ToString();
                //ucNextCharterer.SelectedAddress = dt.Rows[0]["FLDNEXTCHARTER"].ToString();

                ucEstimatedEndDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDESTIMATEDENDDATE"]);
                txtEstimatedDuration.Text = dt.Rows[0]["FLDESTIMATEDDURATION"].ToString();
                ViewState["CHARTERID"] = dt.Rows[0]["FLDVOYAGEID"].ToString();

                ucEstimatedCommenceDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDESTIMATEDCOMMENCEDATE"]);
                ucPlannedCommensedPort.SelectedValue = dt.Rows[0]["FLDPLANNEDCOMMENCEDPORTID"].ToString();
                ucPlannedCommensedPort.Text = dt.Rows[0]["FLDPLANNEDCOMMENCEDPORT"].ToString();
                txtContractHolder.Text = dt.Rows[0]["FLDHOLDINGCONTRACTOR"].ToString();
                txtWorkScope.Text = dt.Rows[0]["FLDWORKSCOPE"].ToString();
                ddlDPChartererYN.SelectedValue = dt.Rows[0]["FLDDPCHARTERYN"].ToString();
                ViewState["FLDVOYAGEDTKEY"] = dt.Rows[0]["FLDVOYAGEDTKEY"].ToString();

                ucProposedDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDPROPOSEDCOMPLETEDDATE"]);
                ucProposedCompletedPort.SelectedValue = dt.Rows[0]["FLDPROPOSEDCOMPLETEDPORTID"].ToString();
                ucProposedCompletedPort.Text = dt.Rows[0]["FLDPROPOSEDCOMPLETEDPORT"].ToString();
                txtProposedDate.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDPROPOSEDCOMPLETEDDATE"]);

                chkCommencementAtSea.Checked = dt.Rows[0]["FLDCOMMENCEMENTATSEAYN"].ToString().Equals("1") ? true : false;
                //txtCommencedLocation.Text = dt.Rows[0]["FLDCOMMENCEMENTLOCATION"].ToString();
                ucCommencedLat.TextDegree = dt.Rows[0]["FLDCOMMENCEDLAT1"].ToString();
                ucCommencedLong.TextDegree = dt.Rows[0]["FLDCOMMENCEDLONG1"].ToString();
                ucCommencedLat.TextMinute = dt.Rows[0]["FLDCOMMENCEDLAT2"].ToString();
                ucCommencedLong.TextMinute = dt.Rows[0]["FLDCOMMENCEDLONG2"].ToString();
                ucCommencedLat.TextSecond = dt.Rows[0]["FLDCOMMENCEDLAT3"].ToString();
                ucCommencedLong.TextSecond = dt.Rows[0]["FLDCOMMENCEDLONG3"].ToString();
                ucCommencedLat.TextDirection = dt.Rows[0]["FLDCOMMENCEDLATDIR"].ToString();
                ucCommencedLong.TextDirection = dt.Rows[0]["FLDCOMMENCEDLONGDIR"].ToString();
                chkCompletedAtSea.Checked = dt.Rows[0]["FLDCOMPLETIONATSEAYN"].ToString().Equals("1") ? true : false;
                //txtCompletedLocation.Text = dt.Rows[0]["FLDCOMPLETEDLOCATION"].ToString();
                ucCompletedLat.TextDegree = dt.Rows[0]["FLDCOMPLETEDLAT1"].ToString();
                ucCompletedLong.TextDegree = dt.Rows[0]["FLDCOMPLETEDLONG1"].ToString();
                ucCompletedLat.TextMinute = dt.Rows[0]["FLDCOMPLETEDLAT2"].ToString();
                ucCompletedLong.TextMinute = dt.Rows[0]["FLDCOMPLETEDLONG2"].ToString();
                ucCompletedLat.TextSecond = dt.Rows[0]["FLDCOMPLETEDLAT3"].ToString();
                ucCompletedLong.TextSecond = dt.Rows[0]["FLDCOMPLETEDLONG3"].ToString();
                ucCompletedLat.TextDirection = dt.Rows[0]["FLDCOMPLETEDLATDIR"].ToString();
                ucCompletedLong.TextDirection = dt.Rows[0]["FLDCOMPLETEDLONGDIR"].ToString();

                txtOptions.Text = dt.Rows[0]["FLDOPTIONS"].ToString();
                txtkickoff.Text = dt.Rows[0]["FLDKICKOFFMEETINGLOCATION"].ToString();
                ddlpremobinspectionYN.SelectedValue = dt.Rows[0]["FLDPREMOBINSPECTION"].ToString();
                txtpremoblocation.Text = dt.Rows[0]["FLDPREMOBINSPECTIONLOCATION"].ToString();
                txtworkinglocation.Text = dt.Rows[0]["FLDWORKINGLOCATIONANDFIELDNAME"].ToString();
                txtnameofrig.Text = dt.Rows[0]["FLDNAMEOFRIG"].ToString();

                //txtCommencedLocation.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
                ucCommencedLat.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
                ucCommencedLong.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
                ucCommensedPort.Visible = chkCommencementAtSea.Checked.Equals(true) ? false : true;
                //txtCompletedLocation.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
                ucCompletedLat.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
                ucCompletedLong.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
                ucCompletedPort.Visible = chkCompletedAtSea.Checked.Equals(true) ? false : true;

                lblCommencedPort.Visible = chkCommencementAtSea.Checked.Equals(true) ? false : true;
                lblCommencedLat.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
                lblCommencedLong.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
                lblCompletedLat.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
                lblCompletedLong.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
                lblCompletedPort.Visible = chkCompletedAtSea.Checked.Equals(true) ? false : true;
                
                
                if (!string.IsNullOrEmpty(ucCompletedDate.Text))
                {
                    DataSet ds1 = PhoenixCrewOffshoreDMRVoyageData.ListChartererBasedOnCommencement(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection), 1);
                    ucDMRCharter.CharterList = ds1;
                    ucDMRCharter.Enabled = false;
                    ucDMRCharter.CssClass = "readonlytextbox";
                }
                else 
                {
                    DataSet ds2 = PhoenixCrewOffshoreDMRVoyageData.ListChartererBasedOnCommencement(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection), 0);
                    ucDMRCharter.CharterList = ds2;
                }

                if (dt.Rows[0]["FLDOFFHIREYN"].ToString() == "0")
                    ucDMRCharter.SelectedValue = dt.Rows[0]["FLDNEXTVOYAGEID"].ToString();
                else
                    ucDMRCharter.SelectedValue = "Offhire";

                ddlTradingArea.SelectedValue = dt.Rows[0]["FLDTRADINGCODE"].ToString();
                txtEffectiveDate.Text = String.Format("{0:dd/MM/yyyy hh:mm tt}", dt.Rows[0]["FLDEFFECTIVEDATE"]);

                if (dt.Rows[0]["ISPROPOSALCHARTERYN"].ToString() == "0")
                {
                    ucCharterer.Enabled = false;
                }


            }
        }
    }
    private bool IsValidDiscount(string charterer, string vesselid, string tradingarea, string date)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (charterer.Trim().Equals(""))
            ucError.ErrorMessage = "Charterer is required.";

        if (vesselid.Trim().Equals("") || vesselid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        if (tradingarea.Trim().Equals("") || tradingarea.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Charterer trading area is required.";
        if (date == null || date.Trim().Equals("") || date.Trim().Equals("__/__/____"))
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
    }
    //protected void gvTradingArea_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int iCurrentRow = int.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToUpper().Equals("ADD"))
    //    {
    //        if (!IsValidDiscount(ViewState["CHARTERID"].ToString(),
    //                               ViewState["VESSELID"].ToString(),
    //                               ((DropDownList)_gridView.FooterRow.FindControl("ddlTradingArea")).SelectedValue.ToString(),
    //                               ((UserControlDate)_gridView.FooterRow.FindControl("txtEffectiveDate")).Text))
    //        {

    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                 , General.GetNullableGuid(ViewState["CHARTERID"].ToString())
    //                                                                 , int.Parse(ViewState["VESSELID"].ToString())
    //                                                                 , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlTradingArea")).SelectedValue)
    //                                                                 , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("txtEffectiveDate")).Text));
    //        BindDataTradingArea();
    //    }
    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        Guid? id = (Guid)_gridView.DataKeys[iCurrentRow].Value;
    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    //    }
    //    if (e.CommandName.ToUpper().Equals("UPDATE"))
    //    {
    //        if (!IsValidDiscount(ViewState["CHARTERID"].ToString(),
    //                                ViewState["VESSELID"].ToString(),
    //                               ((DropDownList)_gridView.Rows[iCurrentRow].FindControl("ddlTradingAreaEdit")).SelectedValue.ToString(),
    //                               ((UserControlDate)_gridView.Rows[iCurrentRow].FindControl("txtEffectiveDateEdit")).Text))
    //        {

    //            ucError.Visible = true;
    //            return;
    //        }
    //        Guid? id = (Guid)_gridView.DataKeys[iCurrentRow].Value;
    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                 , id
    //                                                                 , General.GetNullableGuid(ViewState["CHARTERID"].ToString())
    //                                                                 , int.Parse(ViewState["VESSELID"].ToString())
    //                                                                 , int.Parse(((DropDownList)_gridView.Rows[iCurrentRow].FindControl("ddlTradingAreaEdit")).SelectedValue)
    //                                                                 , DateTime.Parse(((UserControlDate)_gridView.Rows[iCurrentRow].FindControl("txtEffectiveDateEdit")).Text));
    //    }

    //}

    //protected void gvTradingArea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindDataTradingArea();
    //}
    //protected void gvTradingArea_RowEditing(object sender, GridViewEditEventArgs de)
    //{

    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindDataTradingArea();
    //}
    //protected void gvTradingArea_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DropDownList ddlTradingArea = (DropDownList)e.Row.FindControl("ddlTradingAreaEdit");
    //        if (ddlTradingArea != null)
    //        {
    //            DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListVoyageLoadDetails();
    //            ddlTradingArea.DataSource = ds.Tables[0];
    //            ddlTradingArea.DataTextField = "FLDQUICKNAME";
    //            ddlTradingArea.DataValueField = "FLDQUICKCODE";
    //            ddlTradingArea.DataBind();
    //            ddlTradingArea.Items.Insert(0, new ListItem("--Select--", "Dummy"));

    //            Label lblTradingCode = (Label)e.Row.FindControl("lblTradingCode");
    //            if (lblTradingCode != null)
    //            {
    //                ddlTradingArea.SelectedValue = lblTradingCode.Text;
    //            }
    //        }

    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {

    //        DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListVoyageLoadDetails();
    //        DropDownList ddlTradingArea = (DropDownList)e.Row.FindControl("ddlTradingArea");

    //        ddlTradingArea.DataSource = ds.Tables[0];
    //        ddlTradingArea.DataTextField = "FLDQUICKNAME";
    //        ddlTradingArea.DataValueField = "FLDQUICKCODE";
    //        ddlTradingArea.DataBind();
    //        ddlTradingArea.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //    }
    //}
    //protected void gvTradingArea_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindDataTradingArea();
    //    SetPageNavigator();
    //}
    //protected void gvTradingArea_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        _gridView.EditIndex = -1;
    //        BindDataTradingArea();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvTradingArea.SelectedIndex = -1;
    //    gvTradingArea.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindDataTradingArea();
    //    SetPageNavigator();
    //}
    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvTradingArea.EditIndex = -1;
    //    gvTradingArea.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindDataTradingArea();
    //    SetPageNavigator();
    //}
    //private void BindDataTradingArea()
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    DataSet ds = new DataSet();

    //    ds = PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingAreaSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString())
    //                                                                 , General.GetNullableGuid(ViewState["CHARTERID"].ToString()), (int)ViewState["PAGENUMBER"],
    //                                                      General.ShowRecords(null),
    //                                                      ref iRowCount,
    //                                                      ref iTotalPageCount);


    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        gvTradingArea.DataSource = ds;
    //        gvTradingArea.DataBind();
    //    }
    //    else
    //    {
    //        DataTable dt = ds.Tables[0];
    //        ShowNoRecordsFound(dt, gvTradingArea);
    //    }

    //    ViewState["ROWCOUNT"] = iRowCount;
    //    ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //    SetPageNavigator();
    //}
    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}
    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void VoyageNewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            UcVessel.SelectedVessel = "";
            ViewState["VESSELID"] = "";
            ucCommencedDate.Text = "";
            ucCompletedDate.Text = "";
            ucEstimatedEndDate.Text = "";

        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (Filter.CurrentVPRSVoyageSelection != null)
            {
                UpdateVoyage();
                if (General.GetNullableDateTime(ucCompletedDate.Text) != null)
                {
                    PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingTillDateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(ViewState["CHARTERID"].ToString())
                                                                                    , int.Parse(ViewState["VESSELID"].ToString())
                                                                                    , DateTime.Parse(ucCompletedDate.Text));
                }

                //BindDataTradingArea();
            }
            else
            {
                AddVoyage();
            }
        }

        if (CommandName.ToUpper().Equals("ATTACHMENT") && ViewState["FLDVOYAGEDTKEY"] != null)
        {
            String scriptpopup = String.Format(
               "javascript:parent.Openpopup('att', '', '../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["FLDVOYAGEDTKEY"].ToString() + "&mod=VESSELPOSITION&type=&cmdname=&VESSELID=" + ViewState["VESSELID"].ToString() + "');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

        }

    }

    private void AddVoyage()
    {
        try
        {
            if (!IsValidVoyage(ucCommencedDate.Text, ucCharterer.SelectedAddress, ucCommensedPort.SelectedValue, ucCompletedDate.Text, ucDMRCharter.SelectedValue
                , ucCompletedPort.SelectedValue,ucCommencedLat.Text,ucCommencedLong.Text,ucCompletedLat.Text,ucCompletedLong.Text))
            {
                ucError.Visible = true;
                return;
            }

            Guid? voyageid = null;
            //string timeofcommenced = //txtTimeOfCommenced.SelectedTime.ToString(); //(txtTimeOfCommenced.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfCommenced.Text;
            //string timeofcompleted = //txtTimeOfCompleted.SelectedTime.ToString();//(txtTimeOfCompleted.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfCompleted.Text;
            string timeofproposedcompleted = (txtProposedDate.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtProposedDate.Text;
            Guid? nextvoyageid = null;
            int? offhireflagYN = 0;

            if (ucDMRCharter.SelectedValue == "Offhire")
                offhireflagYN = 1;
            else
                nextvoyageid = General.GetNullableGuid(ucDMRCharter.SelectedValue);


            PhoenixCrewOffshoreDMRVoyageData.InsertVoyageData(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(UcVessel.SelectedVessel.ToString()),
                General.GetNullableInteger(ucCharterer.SelectedAddress.ToString()),
                General.GetNullableDateTime(""),
                General.GetNullableDateTime(ucCommencedDate.Text),
                General.GetNullableInteger(ucCommensedPort.SelectedValue),
                General.GetNullableDateTime(ucCompletedDate.Text),
                General.GetNullableInteger(ucCompletedPort.SelectedValue),
                General.GetNullableDecimal(""),
                General.GetNullableDecimal(""),
                General.GetNullableDecimal(""),
                ref voyageid,
                General.GetNullableString(txtChartererInstruction.Text),
                General.GetNullableGuid(""),
                General.GetNullableGuid(""),
                General.GetNullableInteger(ddlVolume.SelectedValue),
                General.GetNullableDateTime(ucEstimatedEndDate.Text),
                General.GetNullableDateTime(ucEstimatedCommenceDate.Text),
                General.GetNullableInteger(ucPlannedCommensedPort.SelectedValue),
                General.GetNullableString(txtContractHolder.Text),
                General.GetNullableString(txtWorkScope.Text),
                General.GetNullableInteger(ddlDPChartererYN.SelectedValue),
                General.GetNullableInteger(ucNextCharterer.SelectedAddress.ToString()),
                General.GetNullableDateTime(ucProposedDate.Text + " " + timeofproposedcompleted),
                General.GetNullableInteger(ucProposedCompletedPort.SelectedValue),
                nextvoyageid,
                offhireflagYN,
                General.GetNullableInteger(ddlTradingArea.SelectedValue),
                General.GetNullableDateTime(txtEffectiveDate.Text),
                chkCommencementAtSea.Checked.Equals(true) ? 1 : 0,
                "",
                chkCompletedAtSea.Checked.Equals(true) ? 1 : 0,
                "",     
                ucCommencedLat.TextDegree, ucCommencedLong.TextDegree, ucCommencedLat.TextMinute, ucCommencedLong.TextMinute, ucCommencedLat.TextSecond, ucCommencedLong.TextSecond, ucCommencedLat.TextDirection, ucCommencedLong.TextDirection,
                ucCompletedLat.TextDegree, ucCompletedLong.TextDegree, ucCompletedLat.TextMinute, ucCompletedLong.TextMinute, ucCompletedLat.TextSecond, ucCompletedLong.TextSecond, ucCompletedLat.TextDirection, ucCompletedLong.TextDirection,
                General.GetNullableString(txtOptions.Text),
                General.GetNullableString(txtkickoff.Text),
                General.GetNullableInteger(ddlpremobinspectionYN.SelectedValue),
                General.GetNullableString(txtpremoblocation.Text),
                General.GetNullableString(txtworkinglocation.Text),
                General.GetNullableString(txtnameofrig.Text)
              );

            Filter.CurrentVPRSVoyageSelection = voyageid.ToString();
            ucStatus.Text = "Charterer Added";
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateVoyage()
    {
        try
        {
            if (!IsValidVoyage(ucCommencedDate.Text, ucCharterer.SelectedAddress, ucCommensedPort.SelectedValue, ucCompletedDate.Text, ucNextCharterer.SelectedAddress
                , ucCompletedPort.SelectedValue, ucCommencedLat.Text, ucCommencedLong.Text, ucCompletedLat.Text, ucCompletedLong.Text))
            {
                ucError.Visible = true;
                return;
            }

            //string timeofcommenced = txtTimeOfCommenced.SelectedTime.ToString();//(txtTimeOfCommenced.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfCommenced.Text;
            //string timeofcompleted = txtTimeOfCompleted.SelectedTime.ToString(); //(txtTimeOfCompleted.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfCompleted.Text;
            string timeofproposedcompleted = (txtProposedDate.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtProposedDate.Text;

            Guid? nextvoyageid = null;
            int? offhireflagYN = 0;

            if (ucDMRCharter.SelectedValue == "Offhire")
                offhireflagYN = 1;
            else
                nextvoyageid = General.GetNullableGuid(ucDMRCharter.SelectedValue);

            PhoenixCrewOffshoreDMRVoyageData.UpdateVoyageData(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection),
                General.GetNullableInteger(UcVessel.SelectedVessel.ToString()),
                General.GetNullableInteger(ucCharterer.SelectedAddress.ToString()),
                General.GetNullableDateTime(""),
                General.GetNullableDateTime(ucCommencedDate.Text ), General.GetNullableInteger(ucCommensedPort.SelectedValue),
                General.GetNullableDateTime(ucCompletedDate.Text ),
                General.GetNullableInteger(ucCompletedPort.SelectedValue), General.GetNullableDecimal(""),
                General.GetNullableDecimal(""), General.GetNullableDecimal(""), General.GetNullableString(txtChartererInstruction.Text),
                General.GetNullableGuid(""), General.GetNullableGuid(""),
                General.GetNullableInteger(ddlVolume.SelectedValue),
                General.GetNullableString(ucNextCharterer.AddressType.ToString()),
                General.GetNullableInteger(ucNextCharterer.SelectedAddress.ToString()),
                General.GetNullableDateTime(ucEstimatedEndDate.Text),
                General.GetNullableDateTime(ucEstimatedCommenceDate.Text),
                General.GetNullableInteger(ucPlannedCommensedPort.SelectedValue),
                General.GetNullableString(txtContractHolder.Text),
                General.GetNullableString(txtWorkScope.Text),
                General.GetNullableInteger(ddlDPChartererYN.SelectedValue),
                General.GetNullableDateTime(ucProposedDate.Text + " " + timeofproposedcompleted),
                General.GetNullableInteger(ucProposedCompletedPort.SelectedValue),
                nextvoyageid,
                offhireflagYN,
                General.GetNullableInteger(ddlTradingArea.SelectedValue),
                General.GetNullableDateTime(txtEffectiveDate.Text),
                chkCommencementAtSea.Checked.Equals(true) ? 1 : 0,
                "",
                chkCompletedAtSea.Checked.Equals(true) ? 1 : 0,
                "",
                ucCommencedLat.TextDegree, ucCommencedLong.TextDegree, ucCommencedLat.TextMinute, ucCommencedLong.TextMinute, ucCommencedLat.TextSecond, ucCommencedLong.TextSecond, ucCommencedLat.TextDirection, ucCommencedLong.TextDirection,
                ucCompletedLat.TextDegree, ucCompletedLong.TextDegree, ucCompletedLat.TextMinute, ucCompletedLong.TextMinute, ucCompletedLat.TextSecond, ucCompletedLong.TextSecond, ucCompletedLat.TextDirection, ucCompletedLong.TextDirection,
                General.GetNullableString(txtOptions.Text),
                General.GetNullableString(txtkickoff.Text),
                General.GetNullableInteger(ddlpremobinspectionYN.SelectedValue),
                General.GetNullableString(txtpremoblocation.Text),
                General.GetNullableString(txtworkinglocation.Text),
                General.GetNullableString(txtnameofrig.Text)
                );
                
            ucStatus.Text = "Charterer updated";
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOYAGEDATA"))
            {

            }

            if (CommandName.ToUpper().Equals("VOYAGE"))
            {
                Response.Redirect("CrewOffshoreDMRVoyage.aspx", false);
            }

            if (CommandName.ToUpper().Equals("CARGOROB"))
            {
                Response.Redirect("CrewOffshoreDMRVoyageROBDetails.aspx", false);
            }

            if (CommandName.ToUpper().Equals("COMPLETCARGOROB"))
            {
                Response.Redirect("CrewOffshoreDMRVoyageCompetedROB.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidVoyage(string comdate, string charterer, string commensedport, string completeddate, string nextcharterer, string completedport
        , string commencedlat, string commencedlong, string completedlat, string completedlong)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate, resultCompleteDate;
        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null || General.GetNullableInteger(ViewState["VESSELID"].ToString()) == 0)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(charterer) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            ucError.ErrorMessage = "Charterer is required.";

        //if (!string.IsNullOrEmpty(ucEstimatedCommenceDate.Text) && DateTime.TryParse(ucEstimatedCommenceDate.Text, out resultdate))
        //{
        //    if (DateTime.Compare(resultdate, DateTime.Now.Date) < 0)
        //        ucError.ErrorMessage = "Estimated Commenced Date Should not be earlier than current date";
        //}

        if (General.GetNullableDateTime(comdate) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            ucError.ErrorMessage = "Commenced Date is required.";
        if (General.GetNullableInteger(commensedport) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 && chkCommencementAtSea.Checked.Equals(false))
            ucError.ErrorMessage = "Commenced Port is required.";
        if (General.GetNullableString(commencedlat) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 && chkCommencementAtSea.Checked.Equals(true))
            ucError.ErrorMessage = "Commenced Latitude is required.";
        if (General.GetNullableString(commencedlong) == null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 && chkCommencementAtSea.Checked.Equals(true))
            ucError.ErrorMessage = "Commenced Longitude is required.";

        if (!string.IsNullOrEmpty(ucCommencedDate.Text) && DateTime.TryParse(ucCommencedDate.Text, out resultdate))
        {
            if (DateTime.Compare(resultdate, DateTime.Now) > 0)
                ucError.ErrorMessage = "Commenced Date Should not be later than current date";
        }

        if (!string.IsNullOrEmpty(ucCompletedDate.Text) && DateTime.TryParse(ucCompletedDate.Text, out resultCompleteDate))
        {
            if (DateTime.Compare(resultCompleteDate, DateTime.Now) > 0)
                ucError.ErrorMessage = "Commpleted Date Should not be later than current date";
        }

        if (!string.IsNullOrEmpty(ucCommencedDate.Text) && !string.IsNullOrEmpty(ucCompletedDate.Text))
        {
            DateTime CommencedDate = DateTime.Parse(ucCommencedDate.Text);
            DateTime CompletedDate = DateTime.Parse(ucCompletedDate.Text);
            if (DateTime.Compare(CommencedDate, CompletedDate) > 0)
                ucError.ErrorMessage = "Completed Date should be later than Commenced Date";
        }

        DataTable dt = PhoenixCrewOffshoreDMRVoyageData.VoyageCharterStatusFlag(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
        if (dt.Rows.Count > 0)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    if (ucDMRCharter.SelectedCharter == "--Select--")
                    { ucError.ErrorMessage = "Next Charter is manadatory"; }
                }

                if (dt.Rows[0][0].ToString() == "0")
                {
                    if (General.GetNullableDateTime(completeddate) != null)
                    {
                        if (ucDMRCharter.SelectedCharter == "--Select--")
                            ucError.ErrorMessage = "Next Charter is manadatory";

                        if (chkCompletedAtSea.Checked.Equals(true) && General.GetNullableString(completedlat) == null)
                            ucError.ErrorMessage = "Completed Latitude required.";

                        if (chkCompletedAtSea.Checked.Equals(true) && General.GetNullableString(completedlong) == null)
                            ucError.ErrorMessage = "Completed Longitude required.";

                        if (chkCompletedAtSea.Checked.Equals(false) && General.GetNullableString(completedport) == null)
                            ucError.ErrorMessage = "Completed Port is required."; 
                    }
                }

            }
        }

        //if (!string.IsNullOrEmpty(ucCommencedDate.Text) && !string.IsNullOrEmpty(ucProposedDate.Text))
        //{
        //    DateTime CommencedDate = DateTime.Parse(ucCommencedDate.Text);
        //    DateTime ProposedCompletedDate = DateTime.Parse(ucProposedDate.Text);
        //    if (DateTime.Compare(CommencedDate, ProposedCompletedDate) > 0)
        //        ucError.ErrorMessage = "Proposed Completed Date should be later than Commenced Date";
        //}       

        //if (General.GetNullableDateTime(completeddate) != null && (General.GetNullableInteger(completedport) != null) && (General.GetNullableInteger(nextcharterer) == null))
        //{
        //    ucError.ErrorMessage = "Next Charter is required.";
        //}          

        //if (General.GetNullableDateTime(completeddate) != null || (General.GetNullableInteger(nextcharterer) != null) || (General.GetNullableInteger(completedport) != null))
        //{
        //    if (General.GetNullableInteger(nextcharterer) == null)
        //        ucError.ErrorMessage = "Next Charter is required.";
        //    if (General.GetNullableInteger(completedport) == null)
        //        ucError.ErrorMessage = "Completed Port is required.";
        //    if (General.GetNullableDateTime(completeddate) == null)
        //        ucError.ErrorMessage = "Completed Date is required.";
        //}

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindTrading()
    {
        DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListVoyageLoadDetails();
        ddlTradingArea.DataSource = ds.Tables[0];
        ddlTradingArea.DataTextField = "FLDQUICKNAME";
        ddlTradingArea.DataValueField = "FLDQUICKCODE";
        ddlTradingArea.DataBind();
        //ddlTradingArea.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void ChangesCommencedDetailsProperties(bool flag)
    {
        ucCommencedDate.Enabled = flag;
        ucCommensedPort.Enabled = flag;
       // txtTimeOfCommenced.Enabled = flag;
        //txtCommencedLocation.Enabled = flag;
        chkCommencementAtSea.Enabled = flag;

        ucCommencedDate.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCommensedPort.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        //txtTimeOfCommenced.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        //txtCommencedLocation.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCommencedLat.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCommencedLong.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCommencedLat.ReadOnly = flag.Equals(true) ? false : true;
        ucCommencedLong.ReadOnly = flag.Equals(true) ? false : true;
    }

    private void ChangesCompletedDetailsProperties(bool flag)
    {
        ucCompletedDate.Enabled = flag;
        ucCompletedPort.Enabled = flag;
       // txtTimeOfCompleted.Enabled = flag;
        ucDMRCharter.Enabled = flag;
        //txtCompletedLocation.Enabled = flag;
        chkCompletedAtSea.Enabled = flag;

        ucCompletedDate.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCompletedPort.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
       // txtTimeOfCompleted.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        //txtCompletedLocation.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucDMRCharter.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCompletedLat.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCompletedLong.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucCompletedLat.ReadOnly = flag.Equals(true) ? false : true;
        ucCompletedLong.ReadOnly = flag.Equals(true) ? false : true;

    }

    private void ChangesProposedDetailsProperties(bool flag)
    {
        ucEstimatedCommenceDate.Enabled = flag;
        ucEstimatedEndDate.Enabled = flag;
        //txtEstimatedDuration.Enabled = flag;
        txtWorkScope.Enabled = flag;
        ddlDPChartererYN.Enabled = flag;
        txtContractHolder.Enabled = flag;
        ddlTradingArea.Enabled = flag;
        ucPlannedCommensedPort.Enabled = flag;
        txtChartererInstruction.Enabled = flag;

        ucEstimatedCommenceDate.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucEstimatedEndDate.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        //txtEstimatedDuration.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        txtWorkScope.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ddlDPChartererYN.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        txtContractHolder.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ddlTradingArea.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        ucPlannedCommensedPort.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
        txtChartererInstruction.CssClass = flag.Equals(true) ? "input" : "readonlytextbox";
    }


    protected void chkCommencementAtSea_OnCheckedChanged(object sender, EventArgs e)
    {
        lblCommencedPort.Visible = chkCommencementAtSea.Checked.Equals(true) ? false : true;
        lblCommencedLat.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
        lblCommencedLong.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
        ucCommensedPort.Visible = chkCommencementAtSea.Checked.Equals(true) ? false : true;
        ucCommencedLat.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
        ucCommencedLong.Visible = chkCommencementAtSea.Checked.Equals(true) ? true : false;
        if (chkCommencementAtSea.Checked.Equals(true))
            ucCommensedPort.SelectedValue = "";
        else
        {
            ucCommencedLat.Text = "";
            ucCommencedLong.Text = "";
        }
    }

    protected void chkCompletedAtSea_OnCheckedChanged(object sender, EventArgs e)
    {
        lblCompletedPort.Visible = chkCompletedAtSea.Checked.Equals(true) ? false : true;
        lblCompletedLat.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
        lblCompletedLong.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
        ucCompletedPort.Visible = chkCompletedAtSea.Checked.Equals(true) ? false : true;
        ucCompletedLat.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
        ucCompletedLong.Visible = chkCompletedAtSea.Checked.Equals(true) ? true : false;
        if (chkCompletedAtSea.Checked.Equals(true))
            ucCompletedPort.SelectedValue = "";
        else
        {
            ucCompletedLat.Text = "";
            ucCompletedLong.Text = "";
        }
    }

}
