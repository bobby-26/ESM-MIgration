using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewWorkingGearIndividualRequest : PhoenixBasePage
{
    string empid = string.Empty;
    string vslid = string.Empty;
    string rnkid = string.Empty;
    string crewplanid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            empid = Request.QueryString["empid"];
            vslid = Request.QueryString["vesslid"];
            crewplanid = Request.QueryString["crewplanid"];
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ORDERID"] = null;
                ViewState["ACTIVE"] = null;

                SetEmployeePrimaryDetails();
            }
            if (Request.QueryString["orderid"] != null && !String.IsNullOrEmpty(Request.QueryString["orderid"]))
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            EditWorkGearOrder(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty));
            MainMenu();
            BindData();
            SetPageNavigator();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["ACTIVE"] == null || ViewState["ACTIVE"].ToString() == "1")
        {
            toolbar.AddButton("Save", "SAVE");
            if (ViewState["ORDERID"] != null)
            {
                toolbar.AddButton("Add Items", "ADDITEM");
                toolbar.AddButton("Confirm Request", "CONFIRM");
                toolbar.AddButton("Cancel Request", "REVERSE");
            }
        }
        else if (ViewState["ACTIVE"] != null && ViewState["ACTIVE"].ToString() == "0" && ViewState["ORDERID"] != null)
        {
            toolbar.AddButton("Cancel Request", "REVERSE");
            toolbar.AddButton("View Request", "REPORT");
        }
        CrewWorkGearIndividualRequest.AccessRights = this.ViewState;
        CrewWorkGearIndividualRequest.MenuList = toolbar.Show();
    }

    protected void CrewWorkGearIndividualRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidWorkGearRequest(txtRequestDate.Text, ucMultiAddr.SelectedValue, ddlPayby.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] == null)
                {
                    Guid outorderid = new Guid();
                    PhoenixCrewWorkGearIndividualRequest.InsertIndividualRequest(DateTime.Parse(txtRequestDate.Text)
                                                                                , int.Parse(ucMultiAddr.SelectedValue)
                                                                                , General.GetNullableGuid(crewplanid)
                                                                                , General.GetNullableByte(ddlPayby.SelectedValue)
                                                                                , ref outorderid);

                    if (outorderid != null)
                    {
                        ViewState["ORDERID"] = outorderid.ToString();
                        MainMenu();
                        string itemlist = string.Empty;
                        string qtylist = string.Empty;
                        foreach (GridViewRow r in gvReq.Rows)
                        {
                            Label lblitem = (Label)r.FindControl("lblItemId");
                            Label lblqty = (Label)r.FindControl("lblQuantity");
                            if (lblitem != null)
                                itemlist += lblitem.Text + ",";
                            if (lblqty != null)
                                qtylist += (String.IsNullOrEmpty(lblqty.Text) ? "0" : lblqty.Text) + ",";
                        }
                        if (!String.IsNullOrEmpty(itemlist))
                            PhoenixCrewWorkGearIndividualRequest.InsertIndividualRequestItem(outorderid, itemlist, qtylist);
                    }
                    ucStatus.Text = "Requisition Saved Successfully.";
                }
                else
                {
                    PhoenixCrewWorkGearIndividualRequest.UpdateIndividualRequest( new Guid(ViewState["ORDERID"].ToString())
                                                                                , DateTime.Parse(txtRequestDate.Text)
                                                                                , int.Parse(ucMultiAddr.SelectedValue)
                                                                                , General.GetNullableByte(ddlPayby.SelectedValue));
                    ucStatus.Text = "Requisition Details Updated Successfully.";
                }
                EditWorkGearOrder(General.GetNullableGuid(ViewState["ORDERID"].ToString()));
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixCrewWorkGearIndividualRequest.ConfirmIndividualRequest(new Guid(ViewState["ORDERID"].ToString()));
                ucStatus.Text = "Requisition Details Updated Successfully.";
                Response.Redirect(Request.RawUrl);
            }
            else if (dce.CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKGEARINDIVIDUALREQUEST&orderid=" + ViewState["ORDERID"].ToString() + "&showmenu=0&showword=no&showexcel=no&emailyn=2", false);
            }
            else if (dce.CommandName.ToUpper().Equals("ADDITEM"))
            {
                Response.Redirect("../Crew/CrewWorkingGearOrderFormStoreItemSelection.aspx?back=yes&vesselid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid +"&orderid=" + ViewState["ORDERID"] + "", false);
            }
            else if (dce.CommandName.ToUpper().Equals("REVERSE"))
            {
                if (ViewState["ORDERID"] != null)
                {
                    PhoenixCrewWorkGearIndividualRequest.DeleteIndividualRequest(new Guid(ViewState["ORDERID"].ToString()));
                    ucStatus.Text = "Request entry removed Successfully.";
                }

                Response.Redirect("../Crew/CrewWorkGearIssueGeneral.aspx?vslid=" + Request.QueryString["vesslid"] + "&crewplanid=" + crewplanid + "&empid=" + empid + "", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void WorkGearRequestItems_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {

                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                rnkid = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
            if (!String.IsNullOrEmpty(vslid))
            {
                DataSet ds = PhoenixRegistersVessel.PrintVessel(int.Parse(vslid));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditWorkGearOrder(Guid? gOrderId)
    {
        DataTable dt = PhoenixCrewWorkGearIndividualRequest.EditIndividualRequest(new Guid(crewplanid), gOrderId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            ucMultiAddr.SelectedValue = dr["FLDADDRESSCODE"].ToString();
            ucMultiAddr.Text = dr["FLDNAME"].ToString();
            txtRequestDate.Text = dr["FLDORDERDATE"].ToString();
            ddlPayby.SelectedValue = dr["FLDPAYABLEBY"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();

            ViewState["ORDERID"] = dr["FLDORDERID"].ToString();

            WorkGearItemMenu();
            EnablePage();
        }
    }

    private void EnablePage()
    {
        bool editable = ViewState["ACTIVE"].Equals("0") ? false : true; //Enable or disable all controls

        txtRequestDate.Enabled = editable;
        ucMultiAddr.Enabled = editable;
        ddlPayby.Enabled = editable;
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY" };
        string[] alCaptions = { "Name", "Quantity" };

        string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["MSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewWorkGearIndividualRequest.SearchWorkGearIndividualRequestItemList(int.Parse(empid),General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                                                        , General.GetNullableGuid(crewplanid)
                                                        , General.GetNullableInteger(rnkid)
                                                        , General.GetNullableInteger(vslid)
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

            General.SetPrintOptions("gvReq", "Working Gear Individual Request", alCaptions, alColumns, ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                gvReq.DataSource = dt;
                gvReq.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvReq);
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
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY"};
            string[] alCaptions = { "Name", "Quantity"};
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewWorkGearIndividualRequest.SearchWorkGearIndividualRequestItemList(int.Parse(empid),General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                                                                                , General.GetNullableGuid(crewplanid)
                                                                                , General.GetNullableInteger(rnkid)
                                                                                , General.GetNullableInteger(vslid)
                                                                                , 1
                                                                                , iRowCount
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
            string title = "Working Gear Individual Request";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                if (ViewState["ORDERID"] == null)
                    edit.Visible = false;
            }

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                if (ViewState["ORDERID"] == null)
                    del.Visible = false;
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            UserControls_UserControlWorkingGearSize ucSize = (UserControls_UserControlWorkingGearSize)e.Row.FindControl("ucSizeEdit");
            if (ucSize != null)
                ucSize.SelectedSize = drv["FLDSIZEID"].ToString();
        }
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
    }

    protected void gvReq_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void gvReq_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string orderlineid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderlineidEdit")).Text;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text;
            string size = ((UserControls_UserControlWorkingGearSize)_gridView.Rows[nCurrentRow].FindControl("ucSizeEdit")).SelectedSize;
            if (!IsValidRequestLineItem(quantity,size))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewWorkGearIndividualRequest.UpdateIndividualRequestItem(new Guid(orderlineid),int.Parse(size),decimal.Parse(quantity));

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void gvReq_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixCrewWorkingGearOrderForm.DeleteOrderFormLineItem(id);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    private bool IsValidWorkGearRequest(string date, string supplier, string payby)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(supplier))
            ucError.ErrorMessage = "Supplier is required";
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Request date is required";
        else if (DateTime.Parse(date) > DateTime.Today)
            ucError.ErrorMessage = "Request date should not be greater than today.";
        if (String.IsNullOrEmpty(payby))
            ucError.ErrorMessage = "Payable by is required";

        return (!ucError.IsError);
    }

    private bool IsValidRequestLineItem(string quantity,string size)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }
        if (!General.GetNullableInteger(size).HasValue)
        {
            ucError.ErrorMessage = "Size is required.";
        }
        return (!ucError.IsError);
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
            BindData();
            SetPageNavigator();
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
            gvReq.SelectedIndex = -1;
            gvReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
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

    private void WorkGearItemMenu()
    {
        if (ViewState["ACTIVE"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewWorkingGearIndividualRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkGearItem')", "Print Grid", "icon_print.png", "PRINT");
            
            WorkGearRequestItems.AccessRights = this.ViewState;
            WorkGearRequestItems.MenuList = toolbar.Show();
            WorkGearRequestItems.SetTrigger(pnlWorkGearRequest);
        }
        if (ViewState["ACTIVE"].ToString() == "0")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewWorkingGearIndividualRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkGearItem')", "Print Grid", "icon_print.png", "PRINT");
            WorkGearRequestItems.AccessRights = this.ViewState;
            WorkGearRequestItems.MenuList = toolbar.Show();
        }
    }

}
