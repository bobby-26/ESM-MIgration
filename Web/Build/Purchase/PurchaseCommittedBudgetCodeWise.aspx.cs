using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Web.UI;


public partial class PurchaseCommittedBudgetCodeWise : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"];
                }

                ViewState["SelectedFleetList"] = "";
                ViewState["SelectedVesselList"] = "";
                BindVesselFleetList();
                BindVesselList();

                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbargrid.AddImageButton("../Purchase/PurchaseCommittedBudgetCodeWise.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvCommittedCost')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../Purchase/PurchaseCommittedBudgetCodeWise.aspx", "Search", "search.png", "FIND");

                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
                MenuOrderForm.SetTrigger(pnlOrderForm);

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //        if (dce.CommandName.ToUpper().Equals("COMMENTS"))
    //        {
    //            if (ViewState["orderid"].ToString() == "")
    //            {
    //                ucError.ErrorMessage = "Please Select a Requistion";
    //                ucError.Visible=true;
    //            }
    //            else
    //                Response.Redirect("../Purchase/PurchaseOrderFormComments.aspx?frmreport=yes&pageno=0&orderid=" + ViewState["orderid"].ToString());
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDHARDNAME", "FLDREFERENCENUMBER", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVAL", "FLDNAME" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Description", "Budget Group", "PO Number", "Amount (USD)", "Committed Date", "Vendor" };

        ds = PhoenixPurchaseOrderForm.PurchaseCommittedCostReportBudgetCodeWise(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ViewState["SelectedVesselList"].ToString()
            , General.GetNullableDateTime(ucFromDate.Text)
            , General.GetNullableDateTime(ucToDate.Text)
            , General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode)
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=CommittedCost-BudgetCodeWise.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Committed Cost - Budget Code wise" + "</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;

                    BindData();
                }
            }
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

    private void BindData()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDHARDNAME", "FLDREFERENCENUMBER", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVAL", "FLDNAME" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Description", "Budget Group", "PO Number", "Amount (USD)", "Committed Date", "Vendor" };

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseOrderForm.PurchaseCommittedCostReportBudgetCodeWise(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ViewState["SelectedVesselList"].ToString() 
            , General.GetNullableDateTime(ucFromDate.Text) 
            , General.GetNullableDateTime(ucToDate.Text) 
            , General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode) 
            );

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            gvCommittedCost.DataSource = ds;
            gvCommittedCost.DataBind();

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCommittedCost);
        }
        General.SetPrintOptions("gvCommittedCost", "Committed Cost - Budget Code wise", alCaptions, alColumns, ds);
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        chkFleetList.Items.Add("select");
        chkFleetList.DataSource = ds;
        chkFleetList.DataTextField = "FLDFLEETDESCRIPTION";
        chkFleetList.DataValueField = "FLDFLEETID";
        chkFleetList.DataBind();
        chkFleetList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkFleetList_Changed(object sender, EventArgs e)
    {
        StringBuilder strfleetlist = new StringBuilder();

        ViewState["SelectedVesselList"] = "";

        foreach (ListItem item in chkFleetList.Items)
        {
            if (item.Selected == true)
            {
                strfleetlist.Append(item.Value.ToString());
                strfleetlist.Append(",");
            }
        }
        if (strfleetlist.Length > 1)
        {
            strfleetlist.Remove(strfleetlist.Length - 1, 1);
        }
        if (strfleetlist.ToString().Contains("Dummy"))
        {
            strfleetlist = new StringBuilder();
            strfleetlist.Append("0");
        }
        if (strfleetlist.ToString() == null || strfleetlist.ToString() == "")
            strfleetlist.Append("-1");

        ViewState["SelectedFleetList"] = strfleetlist;

        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist.ToString() == "0" ? null : strfleetlist.ToString());

        foreach (ListItem item in chkVesselList.Items)
        {
            item.Selected = false;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            string vesselid = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vesselid = dr["FLDVESSELID"].ToString();
                foreach (ListItem item in chkVesselList.Items)
                {
                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
                        break;
                    }
                }
            }
        }
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        ViewState["SelectedVesselList"] = "";
        foreach (ListItem item in chkVesselList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }

    protected void gvCommittedCost_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    }

    public bool IsValidFilter()
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";


        if (DateTime.TryParse(ucFromDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(ucFromDate.Text) && string.IsNullOrEmpty(ucToDate.Text))
        {
            ucError.ErrorMessage = "To Date is Required.";
        }
        if (!string.IsNullOrEmpty(ucFromDate.Text)
            && DateTime.TryParse(ucToDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucFromDate.Text)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        if (ViewState["SelectedVesselList"].ToString().Equals(""))
            ucError.ErrorMessage = "Select atleast one Vessel.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

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

    protected void gvCommittedCost_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }
}
