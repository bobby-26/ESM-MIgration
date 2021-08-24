using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogDeckBook : PhoenixBasePage
{
    int vesselId = 0;
    int usercode = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        SessionUtil.PageAccessRights(this.ViewState);
        ShowToolBar();
        //if (IsPostBack == false)
        //{
        //    GetVesselDetails();
        //    //lblCompanyName.Text = HttpContext.Current.Session["companyname"].ToString();
        //    //lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
        //    //lblPortName.Text = "Dummy Text";
        //    //lblMasterName.Text = "Dummy Text";
        //    //lblWorkingLanguage.Text = "English";
        //    //lblIMO.Text = "Dummy Text";
        //}
    }

    private void ShowToolBar()
    {
        //PhoenixToolbar toolBar = new PhoenixToolbar();
        //toolBar.AddFontAwesomeButton("", "Find", "<span class=\"fas fa-search\" ", "FIND");
        //gvTabStrip.MenuList = toolBar.Show();
    }

    //private void GetVesselDetails()
    //{
    //    DataSet ds =  PhoenixDeckLog.VesselDetails(vesselId, null, null);
    //    lblCompanyName.Text = HttpContext.Current.Session["companyname"].ToString();
    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //         DataRow row= ds.Tables[0].Rows[0];
    //        lblVesselName.Text = row["FLDVESSELNAME"].ToString();
    //        lblPortName.Text = row["FLDPORTREGISTERNAME"].ToString();
    //        //lblMasterName.Text = row[""];
    //        lblWorkingLanguage.Text = "English";
    //        lblIMO.Text = row["FLDIMONUMBER"].ToString();
    //    }
    //}


    //protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    //        if (CommandName.ToUpper().Equals("FIND"))
    //        {
    //            //Response.Redirect("../Log/ElectricLogDeckLogConfiguration.aspx");
    //            Response.Redirect("../Log/ElectricLogDeckBook.aspx");
    //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
    //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', null);", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private string[] DummyData()
    {
        return new string[] { };
    }

    protected void gvShipTime_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvShipTime.DataSource = DummyData();
    }

    protected void gvLookOut_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvLookOut.DataSource = DummyData();
    }

    protected void gvFireSafety_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvFireSafety.DataSource = DummyData();
    }

    protected void gvNoonPos_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvNoonPos.DataSource = DummyData();
    }

    protected void gvFuelROB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvFuelROB.DataSource = DummyData();
    }

    protected void gvRadarLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvRadarLog.DataSource = DummyData();
    }
}