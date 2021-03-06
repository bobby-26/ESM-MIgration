using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

public partial class VesselAccountsCTMNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsCTMNew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarmain.AddImageLink("javascript:CallPrint('gvCTM')", "Print Grid", "icon_print.png", "PRINT");
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsCTMNew.aspx", "Add New", "add.png", "NEW");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
                string[] alCaptions = { "Date", "Required Port", "ETA", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

                DataSet ds = PhoenixVesselAccountsCTMNew.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                           , null, null
                           , sortexpression, sortdirection
                            , 1
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("CTM Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTMGeneralNew.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "Date", "Required Port", "ETA", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCTMNew.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , null, null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTM.DataSource = ds;
                gvCTM.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTM);
            }

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
    protected void gvCTM_RowDataBound(object sender, GridViewRowEventArgs e)
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
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    ImageButton up = (ImageButton)e.Row.FindControl("cmdUpdate");
        //    if (up != null)
        //    {
        //        up.Visible = SessionUtil.CanAccess(this.ViewState, up.CommandName);
        //        up.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','VesselAccountsCTMGeneralNew.aspx?CTMID=" + drv["FLDCAPTAINCASHID"].ToString() + "&r=1'); return false;");
        //    }
        //    if (drv["FLDRECEIVEDDATE"].ToString() != string.Empty || drv["FLDACTIVEYN"].ToString() == "0") up.Visible = false;
        //}
    }
    protected void gvCTM_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvCTM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                _gridView.SelectedIndex = nCurrentRow;
                string ctmid = _gridView.DataKeys[nCurrentRow].Value.ToString();
                string activey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEditable")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsCTMGeneralNew.aspx?CTMID=" + ctmid + "&a=" + activey, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTM_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            PhoenixVesselAccountsCTMNew.DeleteCaptainCash(id.Value);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
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
        ViewState["CTMID"] = null;
        BindData();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCTM.SelectedIndex = -1;
        gvCTM.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        ViewState["CTMID"] = null;
        BindData();
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + (ViewState["TOTALPAGECOUNT"].ToString() == "0" ? ViewState["TOTALPAGECOUNT"].ToString() : ViewState["PAGENUMBER"].ToString());
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
        {
            return true;
        }

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
    //protected void gvCTM_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    GridView _gridView = sender as GridView;
    //    _gridView.SelectedIndex = se.NewSelectedIndex;
    //    string ctmid = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
    //    string activey = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblEditable")).Text;
    //    ViewState["CTMID"] = ctmid;
    //    ViewState["ACTIVEYN"] = activey;
    //    BindData();
    //}
    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        DataList dl = (DataList)MenuCTMMain.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMGeneralNew.aspx"))
    //            {
    //                MenuCTMMain.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMBOW.aspx"))
    //            {
    //                MenuCTMMain.SelectedMenuIndex = 1;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMCalculationNew.aspx"))
    //            {
    //                MenuCTMMain.SelectedMenuIndex = 2;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("VesselAccountsCTMDenominationNew.aspx"))
    //            {
    //                MenuCTMMain.SelectedMenuIndex = 3;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["CTMID"] = null;
    //        BindData();
    //        for (int i = 0; i < gvCTM.DataKeyNames.Length; i++)
    //        {
    //            if (gvCTM.DataKeyNames[i] == ViewState["CTMID"].ToString())
    //            {
    //                gvCTM.SelectedIndex = i;
    //                break;
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