using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
public partial class PlannedMaintenanceOverDueJobMonthWiseChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        string compname = "";
        string valuelist = "";
        string colorlist = "";

        ViewState["fromdate"] = string.Empty;
        ViewState["todate"] = string.Empty;
        ViewState["vesselid"] = string.Empty;

        if (!IsPostBack)
        {
            ddlVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte("1")
                                                                , General.GetNullableByte("1")
                                                                , General.GetNullableByte("1")
                                                                , General.GetNullableByte("1")
                                                                , PhoenixVesselEntityType.VSL
                                                                , null);

            ddlVessel.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["fromdate"]))
                txtFromDate.Text = Request.QueryString["fromdate"];

            if (!string.IsNullOrEmpty(Request.QueryString["todate"]))
                txtToDate.Text = Request.QueryString["todate"];

            if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
                ddlVessel.SelectedValue = Request.QueryString["vesselid"];

        }

        try
        {
            string vesselId = ddlVessel.SelectedValue == "" ? Request.QueryString["vesselid"] : ddlVessel.SelectedValue;
            if (!string.IsNullOrEmpty(vesselId))
            {
                DataTable dt = PhoenixPlannedMaintenanceWorkOrderReport.OverDueMonthwise(General.GetNullableDateTime(txtFromDate.Text),
                    General.GetNullableDateTime(txtToDate.Text), General.GetNullableInteger(""), General.GetNullableString(vesselId));

                if (dt.Rows.Count > 0)
                {
                    for (int col = 2; col <= dt.Columns.Count - 1; col++)
                    {
                        compname = compname + "\"" + dt.Columns[col].ColumnName + "\"" + ",";
                        if (dt.Rows[0][dt.Columns[col].ColumnName].ToString() == "")
                        {
                            valuelist = valuelist + "0,";
                        }
                        else
                        {
                            valuelist = valuelist + dt.Rows[0][dt.Columns[col].ColumnName].ToString() + ",";
                        }
                        colorlist = colorlist + "'#27727B'" + ",";
                    }

                    //int i = 0;
                    //while (i < dt.Rows.Count)
                    //{
                    //    valuelist = valuelist + dt.Rows[i]["FLDDEFECTCOUNT"].ToString() + ",";

                    //    i++;
                    //}
                    compname = compname.TrimEnd(',');
                    valuelist = valuelist.TrimEnd(',');
                    colorlist = colorlist.TrimEnd(',');
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chart popup", "var dataSName = [" + compname + "]; var dataValues= [" + valuelist + "]; var colourList = [" + colorlist + "]; calllineChart('Overdue Jobs Month wise(%)', '', 'PMS',dataSName,dataValues,colourList,'%','ChartDiv');", true);
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
	}
}