using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionSIPTankNewFuelCofiguration : PhoenixBasePage
{
    string tootip = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        bindToolTip();

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", tootip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddButton("Back", "BACK",ToolBarDirection.Right);
        TabTankInformation.AccessRights = this.ViewState;
        TabTankInformation.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                UcVessel.SelectedVessel = Request.QueryString["VESSELID"].ToString();
            }
            UcVessel.DataBind();
            UcVessel.bind();

        }


        FuelTankSummary(0);
        FuelTankSummary(1);
        FuelTankSummary(2);
    }
    private void bindToolTip()
    {
        try
        {
            DataSet ds = PhoenixRegistersSIPToolTipConfiguration.SIPToolTipConfigList(General.GetNullableInteger(Request.QueryString["SIPCONFIGID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableString(ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString()) != null)
                    tootip = ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void TabTankInformation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselPosition/VesselPositionSIPConfiguration.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //Label TankID = (Label)_gridView.Rows[nCurrentRow].FindControl("lblTankID");
                //PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //Label TankID = (Label)_gridView.Rows[nCurrentRow].FindControl("lblTankID");
                //PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //Label TankID = (Label)_gridView.Rows[nCurrentRow].FindControl("lblTankID");
                //PhoenixVesselPositionSIPTankConfuguration.DelateSIPTankInsert(new Guid(TankID.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSIPTanksConfuguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del!= null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, "--Select--");
                    if (General.GetNullableGuid(drv["FLDNEWFUELTYPE"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDNEWFUELTYPE"].ToString();
                }
                RadRadioButtonList rdl = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");
                if (rdl != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) != null)
                        rdl.SelectedValue = drv["FLDTANKCLEANNOTREQ"].ToString();
                    else
                        rdl.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 1);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, "--Select--");
                    if (General.GetNullableGuid(drv["FLDNEWFUELTYPE"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDNEWFUELTYPE"].ToString();
                }
                RadRadioButtonList rdl = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");
                if (rdl != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) != null)
                        rdl.SelectedValue = drv["FLDTANKCLEANNOTREQ"].ToString();
                    else
                        rdl.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridEditableItem)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null && General.GetNullableGuid(drv["FLDSIPTANKID"].ToString()) == null)
                {
                    del.Visible = false;
                }

                RadComboBox ddlOilType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
                if (ddlOilType != null)
                {
                    DataSet ds = PhoenixRegistersOilType.ListOilType(1, 0);
                    ddlOilType.DataSource = ds;
                    ddlOilType.DataBind();
                    ddlOilType.Items.Insert(0, "--Select--");
                    if (General.GetNullableGuid(drv["FLDNEWFUELTYPE"].ToString()) != null)
                        ddlOilType.SelectedValue = drv["FLDNEWFUELTYPE"].ToString();
                }

                RadRadioButtonList rdl = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");
                if (rdl != null)
                {
                    if (General.GetNullableInteger(drv["FLDTANKCLEANNOTREQ"].ToString()) != null)
                        rdl.SelectedValue = drv["FLDTANKCLEANNOTREQ"].ToString();
                    else
                        rdl.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSIPTanksConfuguration_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
            RadCheckBox chkTankCleanNotReq = (RadCheckBox)e.Item.FindControl("chkTankCleanNotReq");
            RadRadioButtonList rdTankCleanNotReq = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");

            PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankNewFuel(
                        General.GetNullableGuid(lblSIPTankId.Text)
                        , General.GetNullableGuid(FuelType.SelectedValue)
                        , General.GetNullableInteger( rdTankCleanNotReq.SelectedValue.ToString())
                        );

            RebindFuelTank();
            FuelTankSummary(0);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSettlingServeice_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
            RadCheckBox chkTankCleanNotReq = (RadCheckBox)e.Item.FindControl("chkTankCleanNotReq");
            RadRadioButtonList rdTankCleanNotReq = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");

            PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankNewFuel(
                        General.GetNullableGuid(lblSIPTankId.Text)
                        , General.GetNullableGuid(FuelType.SelectedValue)
                        , General.GetNullableInteger(rdTankCleanNotReq.SelectedValue.ToString())
                        );

            RebindServiceandSettlingTank();
            FuelTankSummary(1);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLubeOilTank_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadLabel lblSIPTankId = (RadLabel)e.Item.FindControl("lblispTankIDEdit");
            RadComboBox FuelType = (RadComboBox)e.Item.FindControl("ddlNewFeulType");
            RadCheckBox chkTankCleanNotReq = (RadCheckBox)e.Item.FindControl("chkTankCleanNotReq");
            RadRadioButtonList rdTankCleanNotReq = (RadRadioButtonList)e.Item.FindControl("rdTankCleanNotReq");

            PhoenixVesselPositionSIPTankConfuguration.UpdateSIPTankNewFuel(
                        General.GetNullableGuid(lblSIPTankId.Text)
                        , General.GetNullableGuid(FuelType.SelectedValue)
                        , General.GetNullableInteger(rdTankCleanNotReq.SelectedValue.ToString())
                        );
            RebindLubeOilTank();
            FuelTankSummary(2);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void FuelTankSummary(int tanktype)
    {
        if (tanktype == 0)
            divFuelTankSummary.Controls.Clear();
        if (tanktype == 1)
            divServiceSettlingTankSummary.Controls.Clear();
        if (tanktype == 2)
            divLubeOilTank.Controls.Clear();

        int? count = 0;
      DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPFuelTypeAggregate(
                        General.GetNullableInteger(UcVessel.SelectedVessel)
                        , tanktype
                        ,ref count
                        );

        Table tblFuelTankSummary = new Table();
        tblFuelTankSummary.Style.Add("Width", "98%");

        TableRow trFuelTankSummaryTop = new TableRow();

        TableCell tcFuelTankSummaryTop1 = new TableCell();
        TableCell tcFuelTankSummaryTop2 = new TableCell();
        TableCell tcFuelTankSummaryTop3 = new TableCell();

        tcFuelTankSummaryTop1.Style.Add("Width", "50%");
        tcFuelTankSummaryTop2.Style.Add("Width", "20%");
        tcFuelTankSummaryTop3.Style.Add("Width", "15%");
        tcFuelTankSummaryTop2.Text = "<b>Total No.of Tank : </b>";
        tcFuelTankSummaryTop3.Text = "<b>"+ count.ToString() + "</b>";
        tcFuelTankSummaryTop3.ColumnSpan = 2;

        trFuelTankSummaryTop.Cells.Add(tcFuelTankSummaryTop1);
        trFuelTankSummaryTop.Cells.Add(tcFuelTankSummaryTop2);
        trFuelTankSummaryTop.Cells.Add(tcFuelTankSummaryTop3);

        tblFuelTankSummary.Rows.Add(trFuelTankSummaryTop);

        if (ds.Tables[0].Rows.Count > 0)
        {

            


            TableRow trFuelTankSummary = new TableRow();

            TableCell tcFuelTankSummary1 = new TableCell();
            TableCell tcFuelTankSummary2 = new TableCell();
            TableCell tcFuelTankSummary3 = new TableCell();
            TableCell tcFuelTankSummary4 = new TableCell();

            tcFuelTankSummary1.Style.Add("Width", "50%");
            tcFuelTankSummary2.Style.Add("Width", "20%");
            tcFuelTankSummary3.Style.Add("Width", "15%");
            tcFuelTankSummary4.Style.Add("Width", "15%");

            tcFuelTankSummary1.Text = "<b></b>";
            tcFuelTankSummary2.Text = "<u><b>Fuel Type/Sulphur %</b></u>";
            tcFuelTankSummary3.Text = "<u><b>Capacity</b></u>";
            tcFuelTankSummary4.Text = "<u><b>No.of Tank</b></u>";

            tcFuelTankSummary1.HorizontalAlign = HorizontalAlign.Center;
            tcFuelTankSummary2.HorizontalAlign = HorizontalAlign.Left;
            tcFuelTankSummary3.HorizontalAlign = HorizontalAlign.Center;
            tcFuelTankSummary4.HorizontalAlign = HorizontalAlign.Center;

            trFuelTankSummary.Cells.Add(tcFuelTankSummary1);
            trFuelTankSummary.Cells.Add(tcFuelTankSummary2);
            trFuelTankSummary.Cells.Add(tcFuelTankSummary3);
            trFuelTankSummary.Cells.Add(tcFuelTankSummary4);

            tblFuelTankSummary.Rows.Add(trFuelTankSummary);

            DataTable dt = ds.Tables[0];
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];

                TableRow trFuelTankSummary1 = new TableRow();

                TableCell tcFuelTankSummary5 = new TableCell();
                TableCell tcFuelTankSummary6 = new TableCell();
                TableCell tcFuelTankSummary7 = new TableCell();
                TableCell tcFuelTankSummary8 = new TableCell();

                tcFuelTankSummary5.Style.Add("Width", "50%");
                tcFuelTankSummary6.Style.Add("Width", "20%");
                tcFuelTankSummary7.Style.Add("Width", "15%");
                tcFuelTankSummary8.Style.Add("Width", "15%");

                tcFuelTankSummary5.Text = "<b></b>";
                tcFuelTankSummary6.Text = "<b>"+ dr["FLDSULPHURPERCENT"].ToString() + "</b>";
                tcFuelTankSummary7.Text = "<b>" + dr["FLDFUELCAPACITY"].ToString() + "</b>";
                tcFuelTankSummary8.Text = "<b>" + dr["FLDNOOFTANK"].ToString() + "</b>";

                tcFuelTankSummary5.HorizontalAlign = HorizontalAlign.Center;
                tcFuelTankSummary6.HorizontalAlign = HorizontalAlign.Left;
                tcFuelTankSummary7.HorizontalAlign = HorizontalAlign.Center;
                tcFuelTankSummary8.HorizontalAlign = HorizontalAlign.Center;


                trFuelTankSummary1.Cells.Add(tcFuelTankSummary5);
                trFuelTankSummary1.Cells.Add(tcFuelTankSummary6);
                trFuelTankSummary1.Cells.Add(tcFuelTankSummary7);
                trFuelTankSummary1.Cells.Add(tcFuelTankSummary8);


                tblFuelTankSummary.Rows.Add(trFuelTankSummary1);

            }
        }
        if (tanktype == 0)
            divFuelTankSummary.Controls.Add(tblFuelTankSummary);
        if (tanktype == 1)
            divServiceSettlingTankSummary.Controls.Add(tblFuelTankSummary);
        if (tanktype == 2)
            divLubeOilTank.Controls.Add(tblFuelTankSummary);
    }
    protected void gvSIPTanksConfuguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPNewFuelTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        gvSIPTanksConfuguration.DataSource = ds;

    }
    protected void RebindFuelTank()
    {
        gvSIPTanksConfuguration.SelectedIndexes.Clear();
        gvSIPTanksConfuguration.EditIndexes.Clear();
        gvSIPTanksConfuguration.DataSource = null;
        gvSIPTanksConfuguration.Rebind();
    }
    protected void gvSettlingServeice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPNewFuelSettlingServiceTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        gvSettlingServeice.DataSource = ds;

    }
    protected void RebindServiceandSettlingTank()
    {
        gvSettlingServeice.SelectedIndexes.Clear();
        gvSettlingServeice.EditIndexes.Clear();
        gvSettlingServeice.DataSource = null;
        gvSettlingServeice.Rebind();
    }
    protected void gvLubeOilTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionSIPTankConfuguration.SIPNewLubeOilTankSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        gvLubeOilTank.DataSource = ds;

    }
    protected void RebindLubeOilTank()
    {
        gvLubeOilTank.SelectedIndexes.Clear();
        gvLubeOilTank.EditIndexes.Clear();
        gvLubeOilTank.DataSource = null;
        gvLubeOilTank.Rebind();
    }
}
