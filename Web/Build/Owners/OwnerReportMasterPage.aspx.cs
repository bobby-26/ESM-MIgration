using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnerReportMasterPage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["DTKEY"] = "";
            ViewState["VESSELID"] = "-1";

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                byte? assignedvessel = 1;
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
                    assignedvessel = 0;

                DataSet vsl = PhoenixRegistersVessel.VesselListCommon(1, 1, null, assignedvessel, PhoenixVesselEntityType.VSL, null);
                
                ddlVessel.DataSource = vsl;
                ddlVessel.DataBind();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            if (Request.QueryString["VESSELID"] != null)
            {
                ddlVessel.ClearSelection();
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();                
                ddlVessel.DataBind();
            }

            ddlVessel.SelectedValue = ViewState["VESSELID"].ToString();
            //ucDate.SelectedDate = DateTime.Now;
            setdate();
            Filter.SelectedOwnersReportVessel = ViewState["VESSELID"].ToString();
            PopulateVesselData();
            BindVesselData();
        }
    }
    private bool IsValidData()
    {
        DataTable dt = PhoenixOwnerReport.OwnersReportRemarksSearch(int.Parse(Filter.SelectedOwnersReportVessel)
               , DateTime.Parse(ucDate.SelectedDate.ToString()));
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
        {
            if (dt.Rows.Count > 0)
            {
                if (General.GetNullableInteger(dt.Rows[0]["FLDFLEETSIGNEDYN"].ToString()) != null && General.GetNullableInteger(dt.Rows[0]["FLDFLEETSIGNEDYN"].ToString()) == 1)
                {
                    ucError.ErrorMessage = "Fleet manager not signed for the selected month.";
                }
            }
            else
            {
                ucError.ErrorMessage = "Fleet manager not signed for the selected month.";
            }
        }
        return (!ucError.IsError);
    }
    private void setdate()
    {
        DataTable dt = PhoenixOwnerReport.LastFleetManagerSign(int.Parse(ViewState["VESSELID"].ToString()));
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
            ucDate.SelectedDate = DateTime.Parse(dt.Rows[0]["FLDLASFLEETSIGN"].ToString());
        else
            ucDate.SelectedDate = DateTime.Now;
    }
    protected void ddlVessel_TextChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            setdate();
            return;
        }
        RadComboBox vessel = (RadComboBox)sender;
        if (e.Value != string.Empty)
        {
            ViewState["VESSELID"] = vessel.SelectedValue.ToString();
            Filter.SelectedOwnersReportVessel = vessel.SelectedValue.ToString();
            PopulateVesselData();
            BindVesselData();
        }
    }

    private void BindVesselData()
    {
        if (General.GetNullableDateTime(ucDate.SelectedDate.ToString()) != null)
        {
            DataSet ds = PhoenixOwnerReport.EditOwnerVesselDetail(int.Parse(Filter.SelectedOwnersReportVessel)
                , General.GetNullableDateTime(ucDate.SelectedDate.ToString()));

            Filter.SelectedOwnersReportDate = ucDate.SelectedDate.ToString();


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtvesselname.Text = dr["FLDVESSELNAME"].ToString();
                txtType.Text = dr["FLDVESSELTYPENAME"].ToString();
                txtFlag.Text = dr["FLDFLAGNAME"].ToString();
                txtOwner.Text = dr["FLDOWNER"].ToString();
                txtMonth.Text = dr["FLDMONTHYEAR"].ToString();
                txtFM.Text = dr["FLDFLEETMANAGER"].ToString();
                txtTechSup.Text = dr["FLDTECHSUPT"].ToString();
                txtMarineSupt.Text = dr["FLDMARINESUPT"].ToString();
                txtMaster.Text = dr["FLDMASTER"].ToString();
                txtCheng.Text = dr["FLDCHEIFENGINEERNAME"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                lblstatusname.Text = dr["FLDSTATUSNAME"].ToString();
                if (dr["FLDISLOCKEDYN"].ToString() != "")
                {
                    Filter.SelectedOwnersReportLockedYN = dr["FLDISLOCKEDYN"].ToString();
                }

                if (dr["FLDDEFALUTSECTIONID"].ToString() != "")
                {
                    Filter.SelectedOwnersReportSection = dr["FLDDEFALUTSECTIONID"].ToString();
                    BindCurrentSection();
                    if (lblcurrenturl.Text != "")
                    {
                        ifMoreInfo.Attributes["src"] = "../" + lblcurrenturl.Text;
                    }
                }

            }
        }
    }

    private void BindCurrentSection()
    {
        DataTable ds = PhoenixOwnerReport.CurrentSectionEdit(General.GetNullableGuid(Filter.SelectedOwnersReportSection)
                                                                , General.GetNullableGuid(ViewState["DTKEY"].ToString()));

        if (ds.Rows.Count > 0)
        {
            DataRow dr = ds.Rows[0];
            lblsectionname.Text = dr["FLDSECTIONNAME"].ToString();
            lblprvsectionid.Text = dr["FLDPREVIOUSSECTIONID"].ToString();
            lblprvurl.Text = dr["FLDPREVIOUSURL"].ToString();
            lblnextsectionid.Text = dr["FLDNEXTSECTIONID"].ToString();
            lblnexturl.Text = dr["FLDNEXTURL"].ToString();
            lblcurrenturl.Text = dr["FLDCURRENTURL"].ToString();
            Filter.SelectedOwnersReportSection = dr["FLDCURRENTSECTIONID"].ToString();
        }
    }

    protected void cmdPrevious_Click(object sender, EventArgs e)
    {
        if (lblprvurl.Text != "")
        {
            Filter.SelectedOwnersReportSection = lblprvsectionid.Text;
            ifMoreInfo.Attributes["src"] = "../" + lblprvurl.Text;
            BindCurrentSection();
        }
    }

    protected void cmddefault_Click(object sender, EventArgs e)
    {
        PhoenixOwnerReport.UserDefaultPageUpdate(General.GetNullableGuid(Filter.SelectedOwnersReportSection));
    }

    protected void cmdNext_Click(object sender, EventArgs e)
    {
        if (lblnexturl.Text != "")
        {
            Filter.SelectedOwnersReportSection = lblnextsectionid.Text;
            ifMoreInfo.Attributes["src"] = "../" + lblnexturl.Text;
            BindCurrentSection();
        }
    }

    private void PopulateVesselData()
    {
        if (General.GetNullableDateTime(ucDate.SelectedDate.ToString()) != null)
        {
            PhoenixOwnerReport.PopulateOwnerVesselDetail(int.Parse(Filter.SelectedOwnersReportVessel)
                , General.GetNullableDateTime(ucDate.SelectedDate.ToString()));
        }
    }

    protected void gvSection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReport.ListSection();
        gvSection.DataSource = dt;
    }

    protected void gvSection_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            string[] myArr = item.DataCell.Text.Split(':');
            item.DataCell.Text = myArr[1].Trim();
        }



        if (e.Item is GridDataItem)
        {

            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");
            LinkButton lnkname = (LinkButton)e.Item.FindControl("lnkname");

            //if ((lnkname != null) && (lnkname.Text != "") && (lnkname.Text != null) && (lblurl.Text != null))
            //{                
            //    ifMoreInfo.Attributes["src"] = "../"+ lblurl.Text;
            //}

            //if ((lnk30Days != null) && (lnk30Days.Text != "") && (lnk30Days.Text != null))
            //{
            //    lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 30 Days','" + lbl30Daysurl.Text + "'); return false;");
            //}

            //if ((lnk60Days != null) && (lnk60Days.Text != "") && (lnk60Days.Text != null))
            //{
            //    lnk60Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60Daysurl.Text + "'); return false;");
            //}

        }
    }

    protected void gvSection_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }

    protected void gvSection_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }

    protected void ucDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            setdate();
            return;
        }
        PopulateVesselData();
        BindVesselData();
        if (General.GetNullableDateTime(ucDate.SelectedDate.ToString()) != null)
        {
            Filter.SelectedOwnersReportDate = ucDate.SelectedDate.ToString();
        }
    }

    protected void gvSection_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("LINK"))
        {
            string lblurl = ((RadLabel)e.Item.FindControl("lblurl")).Text;
            string lblcode = ((RadLabel)e.Item.FindControl("lblcode")).Text;
            string lblSectionId = ((RadLabel)e.Item.FindControl("lblSectionId")).Text;

            if (lblurl != null)
            {
                if (lblcode == "GCN" || lblcode == "UDM" || lblcode == "UPN")
                {
                    ifMoreInfo.Attributes["src"] = "../" + lblurl + "&dtkey=" + ViewState["DTKEY"].ToString() + "&VESSELID=" + Filter.SelectedOwnersReportVessel;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../" + lblurl;
                }

                Filter.SelectedOwnersReportSection = lblSectionId;
                BindCurrentSection();
            }
        }
    }


    protected void lnkPdf_Click(object sender, EventArgs e)
    {
        string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
        string filename = "";
        DataSet ds;
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "9");
        nvc.Add("reportcode", "MONTHLYOWNER");
        nvc.Add("CRITERIA", "");
        Session["PHOENIXREPORTPARAMETERS"] = nvc;

        ds = PhoenixOwnerReportQuality.OwnersPDFReport(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
        filename = "OWNERREPORT_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

        PhoenixSsrsReportsCommon.getVersion();
        PhoenixSsrsReportsCommon.getLogo();
        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
    }

    protected void lnkprevmonth_Click(object sender, EventArgs e)
    {
        if (General.GetNullableDateTime(ucDate.SelectedDate.ToString()) != null)
        {
            ucDate.SelectedDate = DateTime.Parse(ucDate.SelectedDate.ToString()).AddMonths(-1);
            if (!IsValidData())
            {
                ucError.Visible = true;
                setdate();
                return;
            }
            PopulateVesselData();
            BindVesselData();
            Filter.SelectedOwnersReportDate = ucDate.SelectedDate.ToString();
        }
    }

    protected void lnknextmonth_Click(object sender, EventArgs e)
    {
        if (General.GetNullableDateTime(ucDate.SelectedDate.ToString()) != null)
        {
            ucDate.SelectedDate = DateTime.Parse(ucDate.SelectedDate.ToString()).AddMonths(1);
            if (!IsValidData())
            {
                ucError.Visible = true;
                setdate();
                return;
            }
            PopulateVesselData();
            BindVesselData();
            Filter.SelectedOwnersReportDate = ucDate.SelectedDate.ToString();
        }
    }
}