using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;

public partial class Registers_RegistersAdminAssetSoftwareList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminAssetSoftwareList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSoftwareMappingAssetList')", "Print Grid", "icon_print.png", "PRINT");
            SoftwareMenuList.AccessRights = this.ViewState;
            SoftwareMenuList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["ASSETTYPEID"] != null)
                {
                    ViewState["ASSETTYPEID"] = Request.QueryString["ASSETTYPEID"].ToString();
                }
                if (Request.QueryString["SOFTWARENAME"] != null)
                {
                    ViewState["SOFTWARENAME"] = Request.QueryString["SOFTWARENAME"].ToString();
                    txtSoftware.Text = ViewState["SOFTWARENAME"].ToString();
                }
                if (Request.QueryString["ZONEID"] != null)
                {
                    ViewState["ZONEID"] = Request.QueryString["ZONEID"];
                }
            }
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SoftwareMenuList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvSoftwareMappingAssetList.EditIndex = -1;
                gvSoftwareMappingAssetList.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNAME"};
            string[] alCaptions = { "Asset"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAdministrationAsset.AdminSoftwareAssetSearch(int.Parse(ViewState["ASSETTYPEID"].ToString()),
                General.GetNullableInteger(ViewState["ZONEID"].ToString()),
                sortdirection, sortexpression,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount
                );

            General.ShowExcel("Installed Software", dt, alColumns, alCaptions, null, string.Empty);
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

            string[] alColumns = { "FLDNAME" };
            string[] alCaptions = { "Asset" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAdministrationAsset.AdminSoftwareAssetSearch(int.Parse(ViewState["ASSETTYPEID"].ToString()),
                General.GetNullableInteger(ViewState["ZONEID"].ToString()),
                sortdirection, sortexpression,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount
                );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSoftwareMappingAssetList.DataSource = dt;
                gvSoftwareMappingAssetList.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvSoftwareMappingAssetList);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvSoftwareMappingAssetList", "Installed Software", alCaptions, alColumns, ds);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSoftwareMappingAssetList_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

   protected void gvSoftwareMappingAssetList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridview = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());

            _gridview.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSoftwareMappingAssetList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
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
               
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
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
        gvSoftwareMappingAssetList.SelectedIndex = -1;
        gvSoftwareMappingAssetList.EditIndex = -1;
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
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSoftwareMappingAssetList.SelectedIndex = -1;
        gvSoftwareMappingAssetList.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
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
}
