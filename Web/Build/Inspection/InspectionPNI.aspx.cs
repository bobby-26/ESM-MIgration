using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;

public partial class InspectionPNI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Inspection/InspectionPNI.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('GVPNI')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:Openpopup('Filter','','InspectionMedicalCaseFilter.aspx?'); return false;", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Inspection/InspectionPNI.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageButton("../Inspection/InspectionPNIOperation.aspx", "Add", "add.png", "ADD");
            MenuPNI.AccessRights = this.ViewState;
            MenuPNI.MenuList = toolbargrid.Show();
            MenuPNI.SetTrigger(pnlPNI);

            if (!IsPostBack)
            {
                VesselConfiguration();

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("List", "LIST");
                toolbarmain.AddButton("Medical Case", "MEDICALCASE");                
                MenuPNImain.AccessRights = this.ViewState;
                MenuPNImain.MenuList = toolbarmain.Show();
                MenuPNImain.SetTrigger(pnlPNI);

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["INSPECTIONPNIID"] = null;
                ViewState["PNICASEID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                MenuPNImain.SelectedMenuIndex = 0; 
            }
            BindData();
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
            int? vesselid = -1;

            string[] alColumns = { "FLDCASENUMBER", "FLDCREWNAME", "FLDILLNESSINJURYDATE", "FLDVESSELCODE" };
            string[] alCaptions = { "Case No", "Crew Name", "Illness Date", "Vessel Code" };
            
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentMedicalCaseFilterCriteria;

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (nvc != null)
                vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

            DataTable dt = PhoenixInspectionPNI.InspectionPNISearch(
                           nvc != null ? General.GetNullableString(nvc.Get("txtCaseNo")) : null
                           , null
                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucTypeOfCase")) : null
                           , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                           , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                           , vesselid
                           , sortexpression, sortdirection
                           , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                           , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


            General.ShowExcel("Inspection PNI",dt , alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void MenuPNI_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
           
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentMedicalCaseFilterCriteria = null;
                BindData();
                SetPageNavigator();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPNImain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (ViewState["INSPECTIONPNIID"] != null && General.GetNullableGuid(ViewState["INSPECTIONPNIID"].ToString()) != null)
            {
                if (dce.CommandName.ToUpper().Equals("LIST"))
                {
                    
                }
                else if (dce.CommandName.ToUpper().Equals("MEDICALCASE"))
                {
                    Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIId=" + ViewState["INSPECTIONPNIID"] + "&PNICASEID=" + ViewState["PNICASEID"], false);
                }
                
            }
            else
            {
                ucError.ErrorMessage = "Please Select the Case No to Proceed Further.";
                ucError.Visible = true;                
            }
            //ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?PNIId=" + ViewState["INSPECTIONPNIID"] + "&PNICASEID=" + ViewState["PNICASEID"];
            
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
            int? vesselid = null;

            string[] alColumns = { "FLDCASENUMBER", "FLDCREWNAME", "FLDILLNESSINJURYDATE", "FLDVESSELCODE"};
            string[] alCaptions = { "Case No", "Crew Name", "Illness Date", "Vessel Code" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentMedicalCaseFilterCriteria;

            //if (nvc != null)
            //    vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (nvc != null)
                vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

            DataTable dt = PhoenixInspectionPNI.InspectionPNISearch(
                            nvc != null ? General.GetNullableString(nvc.Get("txtCaseNo")) : null
                            , null
                            , nvc != null ? General.GetNullableInteger(nvc.Get("ucTypeOfCase")) : null
                            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null                            
                            , vesselid
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("GVPNI", "Inspection PNI", alCaptions, alColumns, ds);

            if (ViewState["CURRENTTAB"] == null)
                ViewState["CURRENTTAB"] = "../Inspection/InspectionPNIOperation.aspx";
            if (ds.Tables[0].Rows.Count > 0)
            {
                GVPNI.DataSource = dt ;
                GVPNI.DataBind();
                if (ViewState["INSPECTIONPNIID"] == null)
                {
                    ViewState["INSPECTIONPNIID"] = dt.Rows[0]["FLDINSPECTIONPNIID"].ToString();
                    ViewState["PNICASEID"] = dt.Rows[0]["FLDPNICASEID"].ToString();
                    GVPNI.SelectedIndex = 0;
                }
//                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?PNIId=" + ViewState["INSPECTIONPNIID"] + "&PNICASEID=" + ViewState["PNICASEID"];
            }
            else
            {
                ViewState["INSPECTIONPNIID"] = null;
                ViewState["PNICASEID"] = null;
                ShowNoRecordsFound(dt, GVPNI);
                //ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
            }
            //SetTabHighlight();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void GVPNI_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void GVPNI_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lblInspectionPNIId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblInspectionPNIId")).Text;
                PhoenixInspectionPNI.InspectionPNIDelete(new Guid(lblInspectionPNIId));
                BindData();
            }
            else
            {
                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GVPNI_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        GVPNI.SelectedIndex = se.NewSelectedIndex;
        string PNIId = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblInspectionPNIId")).Text;
        string PNICaseId = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblPNICaseid")).Text;
        ViewState["INSPECTIONPNIID"] = PNIId;
        ViewState["PNICASEID"] = PNICaseId;
        BindData();
    }

    protected void GVPNI_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void GVPNI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
                ImageButton ckl = (ImageButton)e.Row.FindControl("cmdChkList");
                if (ckl != null) ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
                Label lblPNIid = (Label)e.Row.FindControl("lblInspectionPNIId");
                if (lblPNIid != null && ckl != null && !string.IsNullOrEmpty(lblPNIid.Text))
                {
                    ckl.Visible = true;
                    ckl.Attributes.Add("onclick", "parent.Openpopup('CrewPage', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&showexcel=no&showword=no&reportcode=PNICHECKLIST&pnicaseid=" + lblPNIid.Text + "');return false;");
                }
                else
                    ckl.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            ViewState["ORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            GVPNI.SelectedIndex = -1;
            GVPNI.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["ORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private Boolean IsPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;


    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["INSPECTIONPNIID"] = null;
            //ViewState["PNICASEID"] = null;
            //BindData();
            //SetRowSelection();
            //if (ViewState["INSPECTIONPNIID"] != null)
            //{
            //    for (int i = 0; i < GVPNI.DataKeyNames.Length; i++)
            //    {
            //        if (GVPNI.DataKeyNames[i] == ViewState["INSPECTIONPNIID"].ToString())
            //        {
            //            GVPNI.SelectedIndex = i;
            //            break;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        GVPNI.SelectedIndex = -1;
        if (GVPNI.DataKeys[0] != null)
        {
            for (int i = 0; i < GVPNI.Rows.Count; i++)
            {
                if (GVPNI.DataKeys[i].Value.ToString().Equals(ViewState["INSPECTIONPNIID"].ToString()))
                {
                    GVPNI.SelectedIndex = i;
                    Label lblDtkey = ((Label)GVPNI.Rows[GVPNI.SelectedIndex].FindControl("lblDtkey"));
                    if (lblDtkey != null)
                        ViewState["DTKEY"] = lblDtkey.Text;

                }
            }
        }
    }

    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        DataList dl = (DataList)MenuPNImain.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["CURRENTTAB"].ToString().Trim().Contains("InspectionPNIOperation.aspx"))
    //            {
    //                MenuPNImain.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("InspectionPNILegal.aspx"))
    //            {
    //                MenuPNImain.SelectedMenuIndex = 1;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("InspectionPNIAccounts.aspx"))
    //            {
    //                MenuPNImain.SelectedMenuIndex = 3;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("InspectionPNIWelfare.aspx"))
    //            {
    //                MenuPNImain.SelectedMenuIndex = 2;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


}
