﻿using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;

public partial class PurchaseReportInvNotReceived : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            txtVendorId.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ViewState["SelectedVesselList"] = "";
                BindVesselFleetList();
                BindVesselList();
                BindVesselPurchaserList();
                BindVesselSupdtList();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindVesselSupdtList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVesselSupdt();
        ddlSupdtList.Items.Add("select");
        ddlSupdtList.DataSource = ds;
        ddlSupdtList.DataTextField = "FLDUSERNAME";
        ddlSupdtList.DataValueField = "FLDUSERCODE";
        ddlSupdtList.DataBind();
        ddlSupdtList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindVesselPurchaserList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVesselPurchaser();
        ddlPurchaserList.Items.Add("select");
        ddlPurchaserList.DataSource = ds;
        ddlPurchaserList.DataTextField = "FLDUSERNAME";
        ddlPurchaserList.DataValueField = "FLDUSERCODE";
        ddlPurchaserList.DataBind();
        ddlPurchaserList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
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

        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist.ToString() == "0" ? null : strfleetlist.ToString());

        ViewState["SelectedVesselList"] = "";
        foreach (ListItem item in chkVesselList.Items)
            item.Selected = false;

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

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string prams = "";

                    prams += "&vesselid=" + General.GetNullableString(ViewState["SelectedVesselList"].ToString());
                    prams += "&fromdate=" + General.GetNullableDateTime(ucFromDate.Text);
                    prams += "&todate=" + General.GetNullableDateTime(ucToDate.Text);
                    prams += "&purchaserid=" + General.GetNullableInteger(ddlPurchaserList.SelectedValue);
                    prams += "&supdtid=" + General.GetNullableInteger(ddlSupdtList.SelectedValue);
                    prams += "&vendorid=" + General.GetNullableInteger(txtVendorId.Text);
                    prams += "&ponumber=" + General.GetNullableString(txtPONumber.Text);
                    prams += "&poamount=" + General.GetNullableDecimal(ucPoAmount.Text);
                    prams += "&noofdays=" + General.GetNullableInteger(ddlNoOfDays.SelectedValue);

                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=3&reportcode=POWITHOUTINV" + prams);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";


        //GridView _gridview = gvCrew;

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);

    }
}
