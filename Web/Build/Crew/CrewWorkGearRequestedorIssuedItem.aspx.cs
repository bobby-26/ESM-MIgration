using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewWorkGearRequestedorIssuedItem : PhoenixBasePage
{
    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = string.Empty;
    private string Neededid = string.Empty;
    private string r = string.Empty;
    string rnkid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["crewplanid"] != null)
            ViewState["planid"] = Request.QueryString["crewplanid"];
        if (Request.QueryString["vslid"] != null)
            ViewState["vslid"] = Request.QueryString["vslid"];
        if (Request.QueryString["empid"] != null)
            ViewState["empid"] = Request.QueryString["empid"];
        if (Request.QueryString["Neededid"] != null)
            ViewState["Neededid"] = Request.QueryString["Neededid"];
        if (Request.QueryString["r"] != null)
            ViewState["r"] = Request.QueryString["r"];

        if (ViewState["empid"] != null)
            empid = ViewState["empid"].ToString();
        if (ViewState["vslid"] != null)
            vslid = ViewState["vslid"].ToString();
        if (ViewState["planid"] != null)
            crewplanid = ViewState["planid"].ToString();
        if (ViewState["Neededid"] != null)
            Neededid = ViewState["Neededid"].ToString();
        if (ViewState["r"] != null)
            r = ViewState["r"].ToString();


        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (r == "1")
        {
            toolbar.AddButton("Back", "BACK");

            //toolbar.AddButton("Cancel Request", "REVERSE");
            CrewWorkGearNeededItemRequest.AccessRights = this.ViewState;
            CrewWorkGearNeededItemRequest.MenuList = toolbar.Show();
        }
        SessionUtil.PageAccessRights(this.ViewState);

        //PhoenixToolbar toolbargrid = new PhoenixToolbar();
        //toolbargrid.AddImageButton("../Crew/CrewWorkGearNeededItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
        //toolbargrid.AddImageLink("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "icon_print.png", "PRINT");
        //Menuitems.AccessRights = this.ViewState;
        //Menuitems.MenuList = toolbargrid.Show();

        //PhoenixToolbar toolbargrid1 = new PhoenixToolbar();
        //toolbargrid1.AddImageButton("../Crew/CrewWorkGearNeededItem.aspx", "Export to Excel", "icon_xls.png", "Excel1");
        //toolbargrid1.AddImageLink("javascript:CallPrint('gvRegistersworkinggearitemReq')", "Print Grid", "icon_print.png", "PRINT");
        //Menuitems1.AccessRights = this.ViewState;
        //Menuitems1.MenuList = toolbargrid.Show();


        if (!IsPostBack)
        {

            ViewState["MPAGENUMBER"] = 1;
            ViewState["MSORTEXPRESSION"] = null;
            ViewState["MSORTDIRECTION"] = null;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;


        }
        SetEmployeePrimaryDetails();
        BindData();
        SetPageNavigator();
        BindReq();
        SetPageNavigatorReq();
    }
    protected void Menuitems_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
    protected void Menuitems_TabStripCommand1(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL1"))
            {
                ShowExcelReq();
            }

            BindReq();
            SetPageNavigatorReq();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void CrewWorkGearNeededItemRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Crew/CrewWorkGearNeededItem.aspx?back=yes &vslid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid + "&r=" + r);
        }

    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {

                //txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                //txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                //txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                //rnkid = dt.Rows[0]["FLDRANKPOSTED"].ToString();

                ucTitle.Text = "Working Gear Issued/Requested " + " [" + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString()
                    + " - " + dt.Rows[0]["FLDRANKNAME"].ToString() + "] ";
            }
            else
            {
                ucTitle.Text = "Working Gear Issue/Request";
                ucTitle.ShowMenu = "true";
            }
            if (!String.IsNullOrEmpty(vslid))
            {
                DataSet ds = PhoenixRegistersVessel.PrintVessel(int.Parse(vslid));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Working Gear Item ", "Unit Price", "Order Quantity", "Received Quantity", "Received Date" };

            string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
            int? sortdirection = 1; //default desc order
            if (ViewState["MSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());



            DataSet ds = PhoenixCrewWorkingGearNeededItem.IssuedItemList(General.GetNullableGuid(crewplanid)
                                                                            , General.GetNullableGuid(Neededid)
                                                                            , (int)ViewState["MPAGENUMBER"]
                                                                            , General.ShowRecords(null)
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);


            General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear Issued List", alCaptions, alColumns, ds);

            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    if (ucZone.SelectedZone == "dummy" || ucZone.SelectedZone == "" || ucZone.SelectedZone == null)
            //        ucZone.SelectedZone = ds.Tables[1].Rows[0]["FLDZONEID"].ToString();
            //    if (ddlPayby.SelectedValue == "" || ddlPayby.SelectedValue == null)
            //        ddlPayby.SelectedValue = ds.Tables[1].Rows[0]["FLDPAYABLEBY"].ToString();
            //    if (txtRefNo.Text == null || txtRefNo.Text == "")
            //        txtRefNo.Text = ds.Tables[1].Rows[0]["FLDREFNUMBER"].ToString();
            //    if (txtRequestDate.Text == null || txtRequestDate.Text == "")
            //        txtRequestDate.Text = ds.Tables[1].Rows[0]["FLDREQUESTDATE"].ToString();
            //    if (ucMultiAddr.SelectedValue == "dummy" || ucMultiAddr.SelectedValue == "" || ucMultiAddr.SelectedValue == null)
            //    {
            //        ucMultiAddr.SelectedValue = ds.Tables[1].Rows[0]["FLDSUPPLIERID"].ToString();
            //        ucMultiAddr.Text = ds.Tables[1].Rows[0]["FLDNAME"].ToString();
            //    }
            //    ViewState["NEEDEDID"] = ds.Tables[1].Rows[0]["FLDCREWWGNEEDEDITEMID"].ToString();

            //    PhoenixToolbar toolbar = new PhoenixToolbar();
            //    toolbar.AddButton("Cancel Request", "REVERSE");
            //    CrewWorkGearNeededItemRequest.AccessRights = this.ViewState;
            //    CrewWorkGearNeededItemRequest.MenuList = toolbar.Show();


            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvRegistersworkinggearitem.DataSource = ds;
                gvRegistersworkinggearitem.DataBind();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvRegistersworkinggearitem);
            }
            ViewState["MROWCOUNT"] = iRowCount;
            ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindReq()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Working Gear Item ", "Unit Price", "Order Quantity", "Received Quantity", "Received Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1; //default desc order
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearRequest.RequestedItems(General.GetNullableGuid(crewplanid)
                                                                        , General.GetNullableGuid(Neededid)
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , General.ShowRecords(null)
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);


            General.SetPrintOptions("gvRegistersworkinggearitemReq", "Working Gear Requested List", alCaptions, alColumns, ds);

            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    if (ucZone.SelectedZone == "dummy" || ucZone.SelectedZone == "" || ucZone.SelectedZone == null)
            //        ucZone.SelectedZone = ds.Tables[1].Rows[0]["FLDZONEID"].ToString();
            //    if (ddlPayby.SelectedValue == "" || ddlPayby.SelectedValue == null)
            //        ddlPayby.SelectedValue = ds.Tables[1].Rows[0]["FLDPAYABLEBY"].ToString();
            //    if (txtRefNo.Text == null || txtRefNo.Text == "")
            //        txtRefNo.Text = ds.Tables[1].Rows[0]["FLDREFNUMBER"].ToString();
            //    if (txtRequestDate.Text == null || txtRequestDate.Text == "")
            //        txtRequestDate.Text = ds.Tables[1].Rows[0]["FLDREQUESTDATE"].ToString();
            //    if (ucMultiAddr.SelectedValue == "dummy" || ucMultiAddr.SelectedValue == "" || ucMultiAddr.SelectedValue == null)
            //    {
            //        ucMultiAddr.SelectedValue = ds.Tables[1].Rows[0]["FLDSUPPLIERID"].ToString();
            //        ucMultiAddr.Text = ds.Tables[1].Rows[0]["FLDNAME"].ToString();
            //    }
            //    ViewState["NEEDEDID"] = ds.Tables[1].Rows[0]["FLDCREWWGNEEDEDITEMID"].ToString();

            //    PhoenixToolbar toolbar = new PhoenixToolbar();
            //    toolbar.AddButton("Cancel Request", "REVERSE");
            //    CrewWorkGearNeededItemRequest.AccessRights = this.ViewState;
            //    CrewWorkGearNeededItemRequest.MenuList = toolbar.Show();


            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvRegistersworkinggearitemReq.DataSource = ds;
                gvRegistersworkinggearitemReq.DataBind();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvRegistersworkinggearitemReq);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Working Gear Item ", "Unit Price", "Order Quantity", "Received Quantity", "Received Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
            if (ViewState["MSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewWorkingGearNeededItem.IssuedItemList(General.GetNullableGuid(crewplanid)
                                                                           , General.GetNullableGuid(Neededid)
                                                                           , 1
                                                                           , iRowCount
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);

            string title = "Working Gear Issue/Request.<br/>";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Reference No : " + ds.Tables[1].Rows[0]["FLDREFNUMBER"].ToString() + "<br/> Request date : " + DateTime.Parse(ds.Tables[1].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcelReq()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITPRICE", "FLDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Working Gear Item ", "Unit Price", "Order Quantity", "Received Quantity", "Received Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewWorkingGearRequest.RequestedItems(General.GetNullableGuid(crewplanid)
                                                                           , General.GetNullableGuid(Neededid)
                                                                           , 1
                                                                           , iRowCount
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);



            string title = "Working Gear Issue/Request.<br/>";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Reference No : " + ds.Tables[1].Rows[0]["FLDREFNUMBER"].ToString() + "<br/> Request date : " + DateTime.Parse(ds.Tables[1].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["MPAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["MTOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["MROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPageNavigatorReq()
    {
        try
        {
            cmdPrevious1.Enabled = IsPreviousEnabledReq();
            cmdNext1.Enabled = IsNextEnabledReq();
            lblPagenumber1.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages1.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords1.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

        iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
        iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }
    private Boolean IsPreviousEnabledReq()
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

        iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
        iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private Boolean IsNextEnabledReq()
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


    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["MPAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["MTOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["MPAGENUMBER"] = ViewState["MTOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["MPAGENUMBER"] = 1;

                if ((int)ViewState["MPAGENUMBER"] == 0)
                    ViewState["MPAGENUMBER"] = 1;

                txtnopage.Text = ViewState["MPAGENUMBER"].ToString();
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
    protected void cmdGo_Click1(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage1.Text, out result))
            {
                ViewState["MPAGENUMBER"] = Int32.Parse(txtnopage1.Text);

                if ((int)ViewState["MTOTALPAGECOUNT"] < Int32.Parse(txtnopage1.Text))
                    ViewState["MPAGENUMBER"] = ViewState["MTOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage1.Text))
                    ViewState["MPAGENUMBER"] = 1;

                if ((int)ViewState["MPAGENUMBER"] == 0)
                    ViewState["MPAGENUMBER"] = 1;

                txtnopage1.Text = ViewState["MPAGENUMBER"].ToString();
            }
            BindReq();
            SetPageNavigatorReq();
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
            gvRegistersworkinggearitem.SelectedIndex = -1;
            gvRegistersworkinggearitem.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] - 1;
            else
                ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick1(object sender, CommandEventArgs ce)
    {
        try
        {
            gvRegistersworkinggearitemReq.SelectedIndex = -1;
            gvRegistersworkinggearitemReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindReq();
            SetPageNavigatorReq();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;


        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
}
